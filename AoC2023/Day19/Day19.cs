using System.Linq.Expressions;

namespace AoC2023.Day19;

public class Day19 : IMDay
{
    private static Dictionary<string, Func<Part, bool>> _functions = [];
    public string FilePath { private get; init; } = "Day19\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var (instructions, parts) = await GetInstructionsAndParts();
        _functions = instructions
            .Select(i => (i.Name, Function: ParseFunction(i)))
            .ToDictionary(kvp => kvp.Name, kvp => kvp.Function);

        return parts
            .Where(p => _functions["in"](p))
            .Select(p => p.X + p.M + p.A + p.S)
            .Sum()
            .ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var (instructions, _) = await GetInstructionsAndParts();
        AoCRange defaultRange = new(1, 4000);
        Ranges start = new(defaultRange, defaultRange, defaultRange, defaultRange);

        var graphedInstructions = instructions
            .Select(ParseGraphedInstruction)
            .ToDictionary(i => i.Name, i => i);
        graphedInstructions.Add("A", new("A", []));
        graphedInstructions.Add("R", new("R", []));

        return GetPossibilities(graphedInstructions["in"], graphedInstructions, start).ToString();
    }

    private static long GetPossibilities(GraphedInstruction instruction, Dictionary<string, GraphedInstruction> allInstructions, Ranges currentRange)
    {
        Console.WriteLine($"{instruction.Name}: {currentRange}");

        if (!currentRange.IsValid)
            return 0;

        if (instruction.Name == "R")
            return 0;

        if (instruction.Name == "A")
            return currentRange.X.Length * currentRange.X.Length * currentRange.A.Length * currentRange.S.Length;

        var total = 0L;
        foreach (var child in instruction.Children)
        {
            var other = currentRange;
            if (child.Condition is not null)
            {
                Console.WriteLine($"[{instruction.Name}]  {child}");
                var (left, right) = child.Condition.Value.GetNewRanges(currentRange);
                currentRange = child.Condition.Value.Check == '<' ? right : left;
                other = child.Condition.Value.Check == '<' ? left : right;

                Console.WriteLine($"[{instruction.Name}]  To function: {other}");
                Console.WriteLine($"[{instruction.Name}]  Remaining: {currentRange}");
            }
            Console.WriteLine($"[{instruction.Name}]  [{child.Name}]: {other}");
            total += GetPossibilities(allInstructions[child.Name], allInstructions, other);
        }

        return total;
    }

    private static Func<Part, bool> ParseFunction(Instruction instruction)
    {
        var parameter = Expression.Parameter(typeof(Part), "p");
        var returnVariable = Expression.Variable(typeof(bool), "accept");
        var returnLabel = Expression.Label();
        var callFunction = typeof(Day19).GetMethod(nameof(CallFunction), BindingFlags.Static | BindingFlags.NonPublic);

        var expressions = instruction.Instructions.Select(step => (Expression)(step switch
            {
                "A" => Expression.Assign(returnVariable, Expression.Constant(true)),
                "R" => Expression.Assign(returnVariable, Expression.Constant(false)),
                _ when step.Contains(':') => ParseStep(step, parameter, returnVariable, returnLabel, callFunction!),
                _ => Expression.Assign(returnVariable, Expression.Call(callFunction!, Expression.Constant(step), parameter))
            }))
            .ToList();

        expressions.Add(Expression.Label(returnLabel));
        expressions.Add(returnVariable);

        var block = Expression.Block(typeof(bool), new[] { returnVariable }, expressions);

        return Expression.Lambda<Func<Part, bool>>(block, parameter).Compile();
    }

    private static ConditionalExpression ParseStep(string step, ParameterExpression parameter, ParameterExpression variable, LabelTarget returnLabel, MethodInfo callFunction)
    {
        var (condition, ret) = step.Split(':');
        BinaryExpression testExp;

        if (condition!.Contains('>'))
        {
            var (p, v) = condition.Split('>');
            testExp = Expression.GreaterThan(Expression.Property(parameter, p!.ToUpper()), Expression.Constant(v!.ParseToInt()));
        }
        else
        {
            var (p, v) = condition.Split('<');
            testExp = Expression.LessThan(Expression.Property(parameter, p!.ToUpper()), Expression.Constant(v!.ParseToInt()));
        }

        Expression assign = ret is not "A" and not "R"
            ? Expression.Call(callFunction!, Expression.Constant(ret), parameter)
            : Expression.Constant(ret == "A");

        return Expression.IfThen(testExp, Expression.Block(Expression.Assign(variable, assign), Expression.Return(returnLabel)));
    }

    private static bool CallFunction(string name, Part part) =>
        _functions[name](part);

    private static GraphedInstruction ParseGraphedInstruction(Instruction instruction)
    {
        List<ConditionalInstruction> conditionalInstructions = [];

        foreach (var inner in instruction.Instructions)
        {
            if (inner.Contains(':'))
            {
                var (condition, childName) = inner.Split(':');
                conditionalInstructions.Add(new(childName!, ParseCondition(condition!)));
            }
            else
                conditionalInstructions.Add(new(inner, null));
        }

        return new(instruction.Name, conditionalInstructions.ToArray());
    }

    private static Condition ParseCondition(string condition)
    {
        return new((char)(condition![0] - 32), condition[1], condition[2..].ParseToInt());
    }

    private async Task<(Instruction[], Part[])> GetInstructionsAndParts()
    {
        var (instructions, parts) = await GetInput();
        return (instructions!.Select(ParseInstruction).ToArray(), parts!.Select(ParsePart).ToArray());
    }

    private static Instruction ParseInstruction(string line)
    {
        var (name, rest) = line.Split('{');
        var instructions = rest![..^1].Split(',');
        return new(name!, instructions);
    }

    private static Part ParsePart(string line)
    {
        var (x, m, a , s) = line[1..^1].Split(',');
        return new(x![2..].ParseToInt(), m![2..].ParseToInt(), a![2..].ParseToInt(), s![2..].ParseToInt());
    }

    private async Task<string[][]> GetInput() =>
        await FileParser.ReadBlocksAsStringArray(FilePath);

    private record struct Instruction(string Name, string[] Instructions);
    private record struct Part(int X, int M, int A, int S);

    private record struct Condition(char Property, char Check, long Value)
    {
        public readonly (Ranges trueRange, Ranges falseRange) GetNewRanges(Ranges current) =>
            Property switch
            {
                'X' when Check == '<' => current.SplitOnX(Value, false),
                'X' when Check == '>' => current.SplitOnX(Value, true),
                'M' when Check == '<' => current.SplitOnM(Value, false),
                'M' when Check == '>' => current.SplitOnM(Value, true),
                'A' when Check == '<' => current.SplitOnA(Value, false),
                'A' when Check == '>' => current.SplitOnA(Value, true),
                'S' when Check == '<' => current.SplitOnS(Value, false),
                'S' when Check == '>' => current.SplitOnS(Value, true),
                _ => throw new NotSupportedException("Strange...")
            };

        public readonly override string ToString() =>
            $"{Property} {Check} {Value}";
    };

    private record struct ConditionalInstruction(string Name, Condition? Condition)
    {
        public readonly override string ToString()
        {
            if (Condition is not null)
                return $"{Name}: {Condition}";

            return Name;
        }
    }

    private record GraphedInstruction(string Name, ConditionalInstruction[] Children);
    private record struct Ranges(AoCRange X, AoCRange M, AoCRange A, AoCRange S)
    {
        public readonly bool IsValid =>
            X.Length > 0 && M.Length > 0 && A.Length > 0 && S.Length > 0;

        public readonly (Ranges left, Ranges right) SplitOnX(long value, bool includeValueLeft = true) =>
            (new Ranges(AoCRange.New(X.Start, value - (includeValueLeft ? 0 : 1)), M, A, S),
             new Ranges(AoCRange.New(value + (includeValueLeft ? 1 : 0), X.End), M, A, S));

        public readonly (Ranges left, Ranges right) SplitOnM(long value, bool includeValueLeft = true) =>
            (new Ranges(X, AoCRange.New(M.Start, value - (includeValueLeft ? 0 : 1)), A, S),
             new Ranges(X, AoCRange.New(value + (includeValueLeft ? 1 : 0), M.End), A, S));

        public readonly (Ranges left, Ranges right) SplitOnA(long value, bool includeValueLeft = true) =>
            (new Ranges(X, M, AoCRange.New(A.Start, value - (includeValueLeft ? 0 : 1)), S),
             new Ranges(X, M, AoCRange.New(value + (includeValueLeft ? 1 : 0), A.End), S));

        public readonly (Ranges left, Ranges right) SplitOnS(long value, bool includeValueLeft = true) =>
            (new Ranges(X, M, A, AoCRange.New(S.Start, value - (includeValueLeft ? 0 : 1))),
             new Ranges(X, M, A, AoCRange.New(value + (includeValueLeft ? 1 : 0), S.End)));

        public readonly override string ToString()
        {
            var invalid = !IsValid ? " INVALID" : string.Empty;
            return $"X: {X}, M: {M}, A: {A}, S: {S}{invalid}";
        }
    };
}
