namespace AoC2023.Day20;
public abstract class Module
{
    public required string Name { get; init; }
    public required string[] Destinations { get; init; }
    public required Action<Pulse> SendPulse { get; init; }

    public abstract void ProcessPulse(Pulse pulse);

    protected void Send(bool pulse)
    {
        foreach (var destination in Destinations)
            SendPulse(new(Name, destination, pulse));
    }
}

public class Conjunction : Module
{
    public required Dictionary<string, bool> InputStates { get; init; }

    public override void ProcessPulse(Pulse pulse)
    {
        InputStates[pulse.From] = pulse.Value;

        var pulseToSend = !InputStates.Values.All(v => v);
        Send(pulseToSend);
    }
}

public class FlipFlop : Module
{
    public bool State { get; private set; }

    public override void ProcessPulse(Pulse pulse)
    {
        if (!pulse.Value)
        {
            State = !State;
            Send(State);
        }
    }
}

public class BroadCaster : Module
{
    public override void ProcessPulse(Pulse pulse)
    {
        Send(pulse.Value);
    }
}
