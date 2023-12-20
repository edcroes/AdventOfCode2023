namespace AoC2023.Day20;

public class Day20 : IMDay
{
    public string FilePath { private get; init; } = "Day20\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        Queue<Pulse> bus = [];
        var modules = await GetModules(bus);

        var low = 0L;
        var high = 0L;

        for (var i = 0; i < 1000; i++)
        {
            PressButtonAndWaitTillPulsesEnd(bus, modules, pulse =>
            {
                if (pulse.Value)
                    high++;
                else
                    low++;
            });
        }

        return (low * high).ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        Queue<Pulse> bus = [];
        var modules = await GetModules(bus);

        if (!modules.Values.Any(m => m.Destinations.Contains("rx")))
            return "NO TESTDATA";

        var moduleToRx = modules.Keys.Single(k => modules[k].Destinations.Contains("rx"));
        var modulesTo = modules.Keys
            .Where(k => modules[k].Destinations.Contains(moduleToRx))
            .ToDictionary(m => m, k => 0L);

        var buttonPress = 0L;
        while (modulesTo.Values.Any(v => v == 0))
        {
            buttonPress++;
            PressButtonAndWaitTillPulsesEnd(bus, modules, pulse =>
            {
                if (pulse.Value && pulse.To == moduleToRx && modulesTo.TryGetValue(pulse.From, out long value) && value == 0)
                    modulesTo[pulse.From] = buttonPress;
            });
        }

        return modulesTo.Values.Aggregate(AoCMath.LeastCommonMultiple).ToString();
    }

    private static void PressButtonAndWaitTillPulsesEnd(Queue<Pulse> bus, Dictionary<string, Module> modules, Action<Pulse> doSomethingWithPulse)
    {
        bus.Enqueue(new("button", "broadcaster", false));
        while (bus.Count > 0)
        {
            var pulse = bus.Dequeue();

            doSomethingWithPulse(pulse);

            if (modules.TryGetValue(pulse.To, out var module))
                module.ProcessPulse(pulse);
        }
    }

    private async Task<Dictionary<string, Module>> GetModules(Queue<Pulse> bus)
    {
        var input = await GetInput();
        return input
            .Select(l => ParseModule(l, input, bus))
            .ToDictionary(m => m.Name, m => m);
    }

    private static Module ParseModule((string module, string[] destinations) module, (string, string[])[] others, Queue<Pulse> bus) =>
        module.module[0] switch
        {
            '%' => new FlipFlop() { Name = module.module[1..], Destinations = module.destinations, SendPulse = bus.Enqueue },
            '&' => ParseConjunctionModule(module.module[1..], module.destinations, others, bus),
            _ => new BroadCaster() { Name = module.module, Destinations = module.destinations, SendPulse = bus.Enqueue }
        };

    private static Conjunction ParseConjunctionModule(string name, string[] destinations, (string m, string[] d)[] others, Queue<Pulse> bus)
    {
        var states = others
            .Where(o => o.d.Contains(name))
            .Select(o => o.m[1..])
            .ToDictionary(m => m, v => false);

        return new() { Name = name, Destinations = destinations, InputStates = states, SendPulse = bus.Enqueue };
    }

    private async Task<(string module, string[] destinations)[]> GetInput() =>
        (await FileParser.ReadLinesAsStringArray(FilePath, "->"))
            .Select(l => (l[0], l[1].Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)))
            .ToArray();
}
