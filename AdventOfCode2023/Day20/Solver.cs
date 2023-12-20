namespace AdventOfCode2023.Day20
{
    using AdventOfCode2023.Utils;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Solver : ISolver
    {
        public string Part1(string input)
        {
            var lines = input.AsList();
            Dictionary<string, Module> modules = [];
            foreach (var line in lines)
            {
                var elems = line.Split(" -> ");
                List<string> outputs = [];
                foreach (var outMod in elems[1].Split(", "))
                        outputs.Add(outMod);

                var name = elems[0];
                var type = name[0];
                if (type != 'b')
                    name = name[1..];

                modules[name] = new(name, type, outputs);
            }

            foreach (var kvp in modules)
            {
                foreach (var output in kvp.Value.Outputs)
                {
                    if (modules.TryGetValue(output, out var module))
                        if (module.Type == '&')
                            modules[output].AddInput(kvp.Key);
                }
            }

            Console.WriteLine(ModulesToUML(modules));

            var pulses = CountPulses(modules, 1000);

            return pulses.ToString();
        }

        public string Part2(string input)
        {
            var lines = input.AsList();
            Dictionary<string, Module> modules = [];
            foreach (var line in lines)
            {
                var elems = line.Split(" -> ");
                List<string> outputs = [];
                foreach (var outMod in elems[1].Split(", "))
                    outputs.Add(outMod);

                var name = elems[0];
                var type = name[0];
                if (type != 'b')
                    name = name[1..];

                modules[name] = new(name, type, outputs);
            }

            foreach (var kvp in modules)
            {
                foreach (var output in kvp.Value.Outputs)
                {
                    if (modules.TryGetValue(output, out var module))
                        if (module.Type == '&')
                            modules[output].AddInput(kvp.Key);
                }
            }

            var buttonPresses = CountButtons(modules);

            return buttonPresses.ToString();
        }

        private class Module(string name, char type, List<string> outputs)
        {
            public string Name = name;
            public char Type = type;
            public List<string> Outputs = outputs;
            private Dictionary<string, bool> _inputs = [];
            private bool _state = false;

            public Queue<(bool level, string source, string destination)> HandlePulse(bool level, string source)
            {
                Queue<(bool level, string source, string destination)> pulseQueue = [];

                var newPulseLevel = false;
                switch(Type)
                {
                    case 'b': newPulseLevel = level; break;
                    case '%':
                        if (level)
                            return [];

                        _state = !_state;
                        newPulseLevel = _state;
                        break;

                    case '&':
                        _inputs[source] = level;
                        newPulseLevel = !_inputs.All(kvp => kvp.Value);
                        break;
                }

                foreach (string dest in Outputs)
                {
                    pulseQueue.Enqueue((newPulseLevel, Name, dest));
                }

                return pulseQueue;
            }

            public void AddInput(string source)
            {
                _inputs[source] = false;
            }

            public string ToString(bool withLabel = true)
            {
                var stateStr = "?";
                switch(Type)
                {
                    case '%': stateStr = _state ? "hi" : "lo"; break;
                    case '&':
                        var stateVals = _inputs.Select(kvp => kvp.Value ? "hi" : "lo");
                        stateStr = string.Join(',', stateVals);
                        break;

                }

                var label = withLabel ? $"{Type}{Name}:" : "";
                return $"{label}{stateStr}";
            }
        }

        private long CountPulses(Dictionary<string, Module> modules, int buttonPresses)
        {
            var lowCount = 0L;
            var highCount = 0L;

            //Console.WriteLine("                                    " + string.Join(' ', modules.Values.Select(mbox => mbox.Name.PadRight(2)).Where(n => n != "broadcaster").ToList()));

            while (buttonPresses > 0)
            {
                buttonPresses--;

                Queue<(bool level, string source, string destination)> pulseQueue = [];
                pulseQueue.Enqueue(new(false, "button", "broadcaster"));
                while (pulseQueue.Count > 0)
                {
                    var (level, source, destination) = pulseQueue.Dequeue();
                    if (level)
                        highCount++;
                    else
                        lowCount++;

                    if (modules.TryGetValue(destination, out var module))
                    {
                        var newQueue = module.HandlePulse(level, source);
                        while (newQueue.Count > 0)
                        {
                            var pulse = newQueue.Dequeue();
                            pulseQueue.Enqueue(pulse);
                        }
                    }

                    //Console.Write(PulseToString(level, source, destination, 11, 11) + " | ");
                    //Console.Write(StateToString(1000 - buttonPresses, modules, true) + " | ");
                    //Console.WriteLine($"lowCount={lowCount}, highCount={highCount}");
                }
            }

            return lowCount * highCount;
        }

        private List<(string, bool)> FindContributors(Dictionary<string, Module> modules, string target, bool triggerLevel)
        {
            List<(string, bool)> contributors = [];

            foreach (var parent in modules.Where(m => m.Value.Outputs.Contains(target)))
            {
                // if any of the module's parents are FlipFlops, then none of this module's parents should be viewed as contributors
                if (parent.Value.Type != '&')
                    return [];

                var parents = FindContributors(modules, parent.Value.Name, !triggerLevel);
                if (parents.Count != 0)
                    contributors.AddRange(parents);
                else
                    contributors.Add((parent.Value.Name, triggerLevel));
            }

            return contributors;
        }

        private long CountButtons(Dictionary<string, Module> modules)
        {
            var buttonPresses = 0L;
            Dictionary<(string, bool), long> rxSources = [];

            var contributors = FindContributors(modules, "rx", false);
            foreach (var v in contributors)
            {
                rxSources[v] = 0;
            }

            //Console.WriteLine("      " + string.Join(' ', modules.Values.Select(mbox => mbox.Name.PadRight(2)).Where(n => n != "broadcaster").ToList()));
            //Console.WriteLine(StateToString(buttonPresses, modules, false));

            while (true)
            {
                buttonPresses++;

                Queue<(bool level, string source, string destination)> pulseQueue = [];
                pulseQueue.Enqueue(new(false, "", "broadcaster"));
                while (pulseQueue.Count > 0)
                {
                    var (level, source, destination) = pulseQueue.Dequeue();

                    if (rxSources.TryGetValue((source, level), out long value) && value == 0)
                        rxSources[(source, level)] = buttonPresses;

                    if (modules.TryGetValue(destination, out var module))
                    {
                        var newQueue = module.HandlePulse(level, source);
                        while (newQueue.Count > 0)
                        {
                            var pulse = newQueue.Dequeue();
                            pulseQueue.Enqueue(pulse);
                        }
                    }
                }

                //Console.WriteLine(StateToString(buttonPresses, modules.Where(kvp => rxSources.ContainsKey(kvp.Value.Name) || kvp.Value.Name == modules.First(m => m.Value.Outputs.Contains("rx")).Value.Name).ToDictionary()));

                //Console.WriteLine(StateToString(buttonPresses, modules, false));

                if (rxSources.Values.All(x => x != 0))
                    break;
            }

            return Utils.Lcm<long>(rxSources.Values.Select(v => v).ToArray());
        }

        private string ModulesToString(List<Module> modules, bool withLabels = true)
        {
            var sb = new StringBuilder();
            foreach(var module in modules)
            {
                sb.Append(module.ToString(withLabels) + " ");
            }
            return sb.ToString().TrimEnd();
        }

        private string ModulesToUML(Dictionary<string, Module> modules)
        {
            var sb = new StringBuilder();
            sb.AppendLine("@startuml");
            sb.AppendLine("skinparam activity {");
            sb.AppendLine("    BackgroundColor<<FlipFlop>> LightBlue");
            sb.AppendLine("    BackgroundColor<<Conjugate>> Salmon");
            sb.AppendLine("}");
            sb.AppendLine("(*) --> \"broadcaster\"");
            foreach (var kvp in modules)
            {
                foreach(var dest in kvp.Value.Outputs)
                {
                    var color = modules.TryGetValue(dest, out var m) ? m.Type switch
                    {
                        '%' => " <<FlipFlop>>",
                        '&' => " <<Conjugate>>",
                        _ => "",
                    }:"";
                    sb.AppendLine($"\"{kvp.Value.Name}\" --> \"{dest}\"{color}");
                }
            }
            sb.AppendLine("@enduml");
            return sb.ToString();
        }

        private string StateToString(long counter, Dictionary<string, Module> modules, bool withLabels = true)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{counter.ToString().PadLeft(4)}: ");
            sb.Append(ModulesToString(modules.Values.Where(m => m.Name != "broadcaster").ToList(), withLabels));

            return sb.ToString();
        }

        private string PulseToString(bool level, string source, string destination, int leftPad, int rightPad)
        {
            StringBuilder sb = new StringBuilder();
            var pulseStr = level ? "hi" : "lo";
            sb.Append($"{source.PadLeft(leftPad)}-{pulseStr}->{destination.PadRight(rightPad)}");
            return sb.ToString();
        }
    }
}
