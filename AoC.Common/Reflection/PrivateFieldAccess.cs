using System.Reflection.Emit;
using System.Reflection;

namespace AoC.Common.Reflection;

// Thanks to Glenn Slayden @ https://stackoverflow.com/questions/16073091/is-there-a-way-to-create-a-delegate-to-get-and-set-values-for-a-fieldinfo

public static class PrivateFieldAccess
{
    public static RefGetter<TClass, TField> CreateRefGetter<TClass, TField>(string fieldName)
    {
        const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;

        var field = typeof(TClass).GetField(fieldName, flags) ?? throw new MissingFieldException(typeof(TClass).Name, fieldName);
        var methodName = $"__refGet{typeof(TClass).Name}Field{fieldName}";

        DynamicMethod method = new(
            methodName,
            typeof(TField).MakeByRefType(),
            [typeof(TClass)],
            typeof(TClass),
            true
        );

        var il = method.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldflda, field);
        il.Emit(OpCodes.Ret);

        return (RefGetter<TClass, TField>)method.CreateDelegate(typeof(RefGetter<TClass, TField>));
    }
}

public delegate ref TField RefGetter<TClass, TField>(TClass obj);
