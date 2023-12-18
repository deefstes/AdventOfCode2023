using AdventOfCode2023.Utils.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    public class Experiment
    {
        public object Run()
        {
            List<Program> programs = [];
            List<int> speeds = [5, 10, 15];
            var totalTime = 5.0;
            var desiredDistance = 0.63;
            var margin = 0.01;

            foreach (int s0 in speeds)
                foreach (int s1 in speeds)
                    foreach (int s2 in speeds)
                        foreach (int s3 in speeds)
                            foreach (int s4 in speeds)
                                foreach (int s5 in speeds)
                                    foreach (int s6 in speeds)
                                        foreach (int s7 in speeds)
                                            foreach (int s8 in speeds)
                                                foreach (int s9 in speeds)
                                                {
                                                    var slotSpeeds = new int[10];
                                                    slotSpeeds[0] = s0;
                                                    slotSpeeds[1] = s1;
                                                    slotSpeeds[2] = s2;
                                                    slotSpeeds[3] = s3;
                                                    slotSpeeds[4] = s4;
                                                    slotSpeeds[5] = s5;
                                                    slotSpeeds[6] = s6;
                                                    slotSpeeds[7] = s7;
                                                    slotSpeeds[8] = s8;
                                                    slotSpeeds[9] = s9;

                                                    var program = new Program();
                                                    program.AddSlot(TrainingSlot.NewBySpeedDuration(s0, totalTime / 10.0));
                                                    program.AddSlot(TrainingSlot.NewBySpeedDuration(s1, totalTime / 10.0));
                                                    program.AddSlot(TrainingSlot.NewBySpeedDuration(s2, totalTime / 10.0));
                                                    program.AddSlot(TrainingSlot.NewBySpeedDuration(s3, totalTime / 10.0));
                                                    program.AddSlot(TrainingSlot.NewBySpeedDuration(s4, totalTime / 10.0));
                                                    program.AddSlot(TrainingSlot.NewBySpeedDuration(s5, totalTime / 10.0));
                                                    program.AddSlot(TrainingSlot.NewBySpeedDuration(s6, totalTime / 10.0));
                                                    program.AddSlot(TrainingSlot.NewBySpeedDuration(s7, totalTime / 10.0));
                                                    program.AddSlot(TrainingSlot.NewBySpeedDuration(s8, totalTime / 10.0));
                                                    program.AddSlot(TrainingSlot.NewBySpeedDuration(s9, totalTime / 10.0));

                                                    var ratio = program.TotalDistance / desiredDistance;
                                                    if (ratio > 1-margin &&
                                                        ratio < 1+margin &&
                                                        program.slots.Where(s => s.Speed == 10).Count() == 3)

                                                        programs.Add(program);
                                                }

            return programs;
        }

        private class Program
        {
            public readonly List<TrainingSlot> slots = [];
            public double TotalDistance => slots.Sum(s => s.Distance);
            public double TotalTime => slots.Sum(s => s.Duration);

            public void AddSlot(TrainingSlot slot)
            {
                slots.Add(slot);
            }
        }

        private class TrainingSlot
        {
            public double Duration;
            public double Speed;
            public double Distance;

            public static TrainingSlot NewBySpeedDuration(double speed, double duration)
            {
                return new TrainingSlot
                {
                    Duration = duration,
                    Speed = speed,
                    Distance = speed * duration / 60
                };
            }

            public static TrainingSlot NewBySpeedDistance(double speed, double distance)
            {
                return new TrainingSlot
                {
                    Duration = distance / speed * 60,
                    Speed = speed,
                    Distance = distance
                };
            }
        }
    }
}
