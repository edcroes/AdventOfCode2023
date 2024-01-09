using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using AoC.Common.Reflection;

namespace AoC.Common.Collections;

/// <summary>
/// Currently the PriorityQueue does not have a Remove method. It is implemented already in main but not yet available atm.
/// To work around this, use Reflection to access private stuff, but do it in a way that is fast enough.
/// 
/// https://github.com/dotnet/runtime/blob/main/src/libraries/System.Collections/src/System/Collections/Generic/PriorityQueue.cs
/// </summary>
[DebuggerDisplay("Count = {Count}")]
public class PriorityQueueWithRemove<TElement, TPriority> : PriorityQueue<TElement, TPriority>
{
    public PriorityQueueWithRemove() : base() { }

    public PriorityQueueWithRemove(int initialCapacity) : base(initialCapacity) { }

    public PriorityQueueWithRemove(IComparer<TPriority>? comparer) : base(comparer) { }

    public PriorityQueueWithRemove(int initialCapacity, IComparer<TPriority>? comparer) : base(initialCapacity, comparer) { }

    public PriorityQueueWithRemove(IEnumerable<(TElement Element, TPriority Priority)> items) : base(items) { }

    public PriorityQueueWithRemove(IEnumerable<(TElement Element, TPriority Priority)> items, IComparer<TPriority>? comparer) : base(items, comparer) { }

    private static readonly RefGetter<PriorityQueue<TElement, TPriority>, int> _refGetSize =
        PrivateFieldAccess.CreateRefGetter<PriorityQueue<TElement, TPriority>, int>("_size");
    private ref int Size => ref _refGetSize(this);

    private static readonly RefGetter<PriorityQueue<TElement, TPriority>, int> _refGetVersion =
        PrivateFieldAccess.CreateRefGetter<PriorityQueue<TElement, TPriority>, int>("_version");
    private ref int Version => ref _refGetVersion(this);

    private static readonly RefGetter<PriorityQueue<TElement, TPriority>, (TElement Element, TPriority Priority)[]> _refGetNodes =
        PrivateFieldAccess.CreateRefGetter<PriorityQueue<TElement, TPriority>, (TElement Element, TPriority Priority)[]>("_nodes");
    private ref (TElement Element, TPriority Priority)[] Nodes => ref _refGetNodes(this);

    private static readonly MethodInfo _moveDownDefaultComparerInfo = typeof(PriorityQueue<TElement, TPriority>).GetMethod("MoveDownDefaultComparer", BindingFlags.NonPublic | BindingFlags.Instance)!;
    private Action<PriorityQueue<TElement, TPriority>, (TElement, TPriority), int>? _moveDownDefaultComparer =
        (Action<PriorityQueue<TElement, TPriority>, (TElement, TPriority), int>)Delegate.CreateDelegate(typeof(Action<PriorityQueue<TElement, TPriority>, (TElement, TPriority), int>), _moveDownDefaultComparerInfo, true)!;

    private static readonly MethodInfo _moveDownCustomComparerInfo = typeof(PriorityQueue<TElement, TPriority>).GetMethod("MoveDownCustomComparer", BindingFlags.NonPublic | BindingFlags.Instance)!;
    private Action<PriorityQueue<TElement, TPriority>, (TElement, TPriority), int>? _moveDownCustomComparer =
        (Action<PriorityQueue<TElement, TPriority>, (TElement, TPriority), int>) Delegate.CreateDelegate(typeof(Action<PriorityQueue<TElement, TPriority>, (TElement, TPriority), int>), _moveDownCustomComparerInfo, true)!;

    /// <summary>
    /// This remove method is borrowed from
    /// https://github.com/dotnet/runtime/blob/main/src/libraries/System.Collections/src/System/Collections/Generic/PriorityQueue.cs
    /// 
    /// Remove is already implemented but not yet available. Until then we use it with dirty reflection.
    /// </summary>
    public bool Remove(TElement element, [MaybeNullWhen(false)] out TElement removedElement, [MaybeNullWhen(false)] out TPriority priority, IEqualityComparer<TElement>? equalityComparer = null)
    {
        int index = FindIndex(element, equalityComparer);
        if (index < 0)
        {
            removedElement = default;
            priority = default;
            return false;
        }

        (TElement Element, TPriority Priority)[] nodes = Nodes;
        (removedElement, priority) = nodes[index];
        int newSize = --Size;
        
        if (index < newSize)
        {
            // We're removing an element from the middle of the heap.
            // Pop the last element in the collection and sift downward from the removed index.
            (TElement Element, TPriority Priority) lastNode = nodes[newSize];

            if (Comparer == Comparer<TPriority>.Default)
            {
                _moveDownDefaultComparer!(this, lastNode, index);
            }
            else
            {
                _moveDownCustomComparer!(this, lastNode, index);
            }
        }

        nodes[newSize] = default;
        Version++;

        return true;
    }

    private int FindIndex(TElement element, IEqualityComparer<TElement>? equalityComparer)
    {
        equalityComparer ??= EqualityComparer<TElement>.Default;
        ReadOnlySpan<(TElement Element, TPriority Priority)> nodes = Nodes.AsSpan(0, Size);

        // Currently the JIT doesn't optimize direct EqualityComparer<T>.Default.Equals
        // calls for reference types, so we want to cache the comparer instance instead.
        // TODO https://github.com/dotnet/runtime/issues/10050: Update if this changes in the future.
        if (typeof(TElement).IsValueType && equalityComparer == EqualityComparer<TElement>.Default)
        {
            for (int i = 0; i < nodes.Length; i++)
            {
                if (EqualityComparer<TElement>.Default.Equals(element, nodes[i].Element))
                {
                    return i;
                }
            }
        }
        else
        {
            for (int i = 0; i < nodes.Length; i++)
            {
                if (equalityComparer.Equals(element, nodes[i].Element))
                {
                    return i;
                }
            }
        }

        return -1;
    }
}
