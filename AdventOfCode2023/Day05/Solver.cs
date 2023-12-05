using AdventOfCode2023.Utils;

namespace AdventOfCode2023.Day05
{
    public class Solver : ISolver
    {
        public string Part1(string input)
        {
            HashSet<long> seeds = [];
            Dictionary<string, List<Mapping>> mappings = [];

            string currentMapping = "";
            foreach (string line in input.AsList())
            {
                if (line.StartsWith("seeds:"))
                {
                    seeds = line.Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(s => long.Parse(s)).ToHashSet();
                    continue;
                }

                if (string.IsNullOrEmpty(line))
                {
                    currentMapping = "";
                    continue;
                }

                if (line.EndsWith("map:"))
                {
                    currentMapping = line.Split(' ')[0];
                    mappings[currentMapping] = [];
                    continue;
                }

                var numbers = line.Split(' ').Select(s => long.Parse(s)).ToArray();
                var dstRangeStart = numbers[0];
                var srcRangeStart = numbers[1];
                var rangeLen = numbers[2];

                mappings[currentMapping].Add(new Mapping(srcRangeStart, dstRangeStart, rangeLen));
            }

            Dictionary<long, long> seedToLocationMapping = [];
            foreach (long seed in seeds)
            {
                seedToLocationMapping[seed] = Lookup(mappings["humidity-to-location"],
                    Lookup(mappings["temperature-to-humidity"],
                    Lookup(mappings["light-to-temperature"],
                    Lookup(mappings["water-to-light"],
                    Lookup(mappings["fertilizer-to-water"],
                    Lookup(mappings["soil-to-fertilizer"],
                    Lookup(mappings["seed-to-soil"], seed)))))));
            }

            return seedToLocationMapping.Values.Min().ToString();
        }

        public string Part2(string input)
        {
            var lines = input.AsList();

            List<(long, long)> seeds = [];
            var seedsData = lines[0]
                .Split()
                .Where(x => x != string.Empty && x != "seeds:")
                .Select(long.Parse)
                .ToList();

            for (int i = 0; i < seedsData.Count; i += 2)
            {
                seeds.Add((seedsData[i], seedsData[i + 1]));
            }

            List<(long, long)> seedsChanged = [];
            List<(long, long)> seedsEscrow = [];

            foreach (var line in lines.Skip(1))
            {
                seedsEscrow = [];
                if (line == "" || line.Contains("map"))
                {
                    foreach (var seedRange in seedsChanged)
                        seeds.Add((seedRange.Item1, seedRange.Item2));
                    seedsChanged = [];
                    continue;
                }

                var elements = line
                    .Split()
                    .Where(x => x != string.Empty)
                    .Select(long.Parse)
                    .ToList();

                foreach (var seedRange in seeds)
                {
                    var mappingStart = elements[1];
                    var mappingEnd = elements[1] + elements[2] - 1;
                    var mappingOffset = elements[0] - elements[1];

                    var seedStart = seedRange.Item1;
                    var seedEnd = seedRange.Item1 + seedRange.Item2 - 1;

                    long overlapStart = 0;
                    long overlapStop = 0;

                    if (mappingStart < seedStart
                        && mappingEnd >= seedStart
                        && mappingEnd <= seedEnd)
                    {
                        overlapStart = seedStart;
                        overlapStop = mappingEnd;
                        seedsChanged.Add((overlapStart + mappingOffset, overlapStop - overlapStart + 1));
                        if (overlapStop + 1 <= seedEnd)
                            seedsEscrow.Add((overlapStop + 1, seedEnd - (overlapStop + 1) + 1));
                    }
                    else if (
                        mappingStart >= seedStart
                        && mappingEnd <= seedEnd)
                    {
                        overlapStart = mappingStart;
                        overlapStop = mappingEnd;
                        seedsChanged.Add((overlapStart + mappingOffset, overlapStop - overlapStart + 1));
                        if (overlapStop + 1 <= seedEnd)
                            seedsEscrow.Add((overlapStop + 1, seedEnd - (overlapStop + 1) + 1));
                        if (overlapStart - 1 >= seedStart)
                            seedsEscrow.Add((seedStart, overlapStart - 1 - (seedStart) + 1));
                    }
                    else if (mappingStart >= seedStart
                        && mappingStart <= seedEnd
                        && mappingEnd > seedEnd)
                    {
                        overlapStart = mappingStart;
                        overlapStop = seedEnd;
                        seedsChanged.Add((overlapStart + mappingOffset, overlapStop - overlapStart + 1));
                        if (overlapStart - 1 >= seedStart)
                            seedsEscrow.Add((seedStart, overlapStart - 1 - (seedStart) + 1));
                    }
                    else if (mappingStart < seedStart
                        && mappingEnd > seedEnd)
                    {
                        overlapStart = seedStart;
                        overlapStop = seedEnd;
                        seedsChanged.Add((overlapStart + mappingOffset, overlapStop - overlapStart + 1));
                    }
                    else
                    {
                        seedsEscrow.Add((seedStart, seedEnd - seedStart + 1));
                    }
                }

                seeds = seedsEscrow;
            }

            foreach (var seedRange in seedsChanged)
                seeds.Add((seedRange.Item1, seedRange.Item2));

            return seeds.Min(x => x.Item1).ToString();
        }

        public string Part2_Failure(string input)
        {
            List<long> seedsStart = [];
            List<long> seedsCount = [];
            Dictionary<string, List<Mapping>> mappings = [];

            string currentMapping = "";
            foreach (string line in input.AsList())
            {
                if (line.StartsWith("seeds:"))
                {
                    var seedsLine = line.Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(s => long.Parse(s)).ToList();
                    seedsStart = seedsLine.Where((c, i) => i % 2 == 0).ToList();
                    seedsCount = seedsLine.Where((c, i) => i % 2 != 0).ToList();
                    continue;
                }

                if (string.IsNullOrEmpty(line))
                {
                    currentMapping = "";
                    continue;
                }

                if (line.EndsWith("map:"))
                {
                    currentMapping = line.Split(' ')[0];
                    mappings[currentMapping] = [];
                    continue;
                }

                var numbers = line.Split(' ').Select(s => long.Parse(s)).ToArray();
                var dstRangeStart = numbers[0];
                var srcRangeStart = numbers[1];
                var rangeLen = numbers[2];

                mappings[currentMapping].Add(new Mapping(srcRangeStart, dstRangeStart, rangeLen));
            }

            var t2h = CombineMappings(
                mappings["temperature-to-humidity"],
                mappings["humidity-to-location"]);

            var l2t = CombineMappings(
                mappings["light-to-temperature"],
                t2h);

            var w2l = CombineMappings(
                mappings["water-to-light"],
                l2t);

            var f2w = CombineMappings(
                mappings["fertilizer-to-water"],
                w2l);

            var s2f = CombineMappings(
                mappings["soil-to-fertilizer"],
                f2w);

            var seedToLocationMapping = CombineMappings(
                mappings["seed-to-soil"],
                s2f);

            List<Mapping> seeds = [];
            for (int i = 0; i < seedsStart.Count; i++)
            {
                seeds.Add(new Mapping(
                    srcRangeStart: seedsStart[i],
                    dstRangeStart: seedsStart[i],
                    rangeLen: seedsCount[i]));
            }

            var finalLookup = CombineMappings(seeds, seedToLocationMapping, false);

            return finalLookup.Min(m => m.dstRangeStart).ToString();
        }

        public class Mapping(long srcRangeStart, long dstRangeStart, long rangeLen)
        {
            public readonly long srcRangeStart = srcRangeStart;
            public readonly long dstRangeStart = dstRangeStart;
            public readonly long rangeLen = rangeLen;
            public readonly long srcRangeEnd = srcRangeStart + rangeLen - 1;
            public readonly long dstRangeEnd = dstRangeStart + rangeLen - 1;
        }

        private static long Lookup(List<Mapping> mapping, long input)
        {
            var range = mapping.Where(m => m.srcRangeStart <= input).FirstOrDefault(m => m.srcRangeStart + m.rangeLen > input);
            if (range == null)
                return input;

            var offset = input - range.srcRangeStart;
            return range.dstRangeStart + offset;
        }

        private static long ReverseLookup(List<Mapping> mapping, long input)
        {
            var range = mapping.Where(m => m.dstRangeStart <= input).FirstOrDefault(m => m.dstRangeStart + m.rangeLen > input);
            if (range == null)
                return input;

            var offset = input - range.dstRangeStart;
            return range.srcRangeStart + offset;
        }

        private List<Mapping> CombineMappings(List<Mapping> lowerOrder, List<Mapping> higherOrder, bool expand = true)
        {
            var combined = new List<Mapping>();

            if (expand)
            {
                // Add two mappings at the botom and top end of lowerOrder to cover the range available from higherOrder that are not covered by lowerOrder
                var minHigherOrderStart = higherOrder.Min(m => m.srcRangeStart);
                var maxHigherOrderEnd = higherOrder.Max(m => m.srcRangeEnd);
                var minLowerOrderStart = lowerOrder.Min(m => m.srcRangeStart);
                var maxLowerOrderEnd = lowerOrder.Max(m => m.srcRangeEnd);

                if (minHigherOrderStart < minLowerOrderStart)
                {
                    lowerOrder.Add(new Mapping(
                        srcRangeStart: minHigherOrderStart,
                        dstRangeStart: minHigherOrderStart,
                        rangeLen: minLowerOrderStart - minHigherOrderStart));
                }
                if (maxHigherOrderEnd > maxLowerOrderEnd)
                {
                    lowerOrder.Add(new Mapping(
                        srcRangeStart: maxLowerOrderEnd + 1,
                        dstRangeStart: maxLowerOrderEnd + 1,
                        rangeLen: maxHigherOrderEnd - maxLowerOrderEnd));
                }
            }

            lowerOrder.Sort((p, q) => p.srcRangeStart.CompareTo(q.srcRangeStart));

            // Iterate over all the mappings in lowerOrder and combine them with higherOrder
            foreach (var mapping in lowerOrder)
            {
                long partitionStart = mapping.srcRangeStart;
                long partitionEnd = mapping.srcRangeStart + mapping.rangeLen - 1;

                while (partitionStart <= partitionEnd)
                {
                    Mapping? dstPartitionStart = null;
                    Mapping? dstPartitionEnd = null;
                    dstPartitionStart = higherOrder.Where(m => m.srcRangeStart <= Lookup(lowerOrder, partitionStart)).FirstOrDefault(m => m.srcRangeStart + m.rangeLen > Lookup(lowerOrder, partitionStart));
                    dstPartitionEnd = higherOrder.Where(m => m.srcRangeStart <= Lookup(lowerOrder, partitionEnd)).FirstOrDefault(m => m.srcRangeStart + m.rangeLen > Lookup(lowerOrder, partitionEnd));

                    if (dstPartitionStart == null && dstPartitionEnd == null)
                    {
                        combined.Add(new Mapping(
                            srcRangeStart: partitionStart,
                            dstRangeStart: Lookup(higherOrder, Lookup(lowerOrder, partitionStart)),
                            rangeLen: partitionEnd - partitionStart + 1));

                        partitionStart = partitionEnd + 1;
                    }
                    else if (dstPartitionStart == null)
                    {
                        var minRangeStart = higherOrder.Min(m => m.srcRangeStart);
                        combined.Add(new Mapping(
                            srcRangeStart: partitionStart,
                            dstRangeStart: Lookup(higherOrder, Lookup(lowerOrder, partitionStart)),
                            rangeLen: ReverseLookup(lowerOrder, minRangeStart - 1) - partitionStart + 1));

                        partitionStart = ReverseLookup(lowerOrder, minRangeStart);
                        partitionEnd = mapping.srcRangeStart + mapping.rangeLen - 1;
                    }
                    else if (dstPartitionEnd == null)
                    {
                        combined.Add(new Mapping(
                            srcRangeStart: partitionStart,
                            dstRangeStart: Lookup(higherOrder, Lookup(lowerOrder, partitionStart)),
                            rangeLen: dstPartitionStart.srcRangeEnd - Lookup(lowerOrder, partitionStart)+1));

                        partitionStart += dstPartitionStart.srcRangeEnd - Lookup(lowerOrder, partitionStart) + 1;
                    }
                    else if (dstPartitionEnd.Equals(dstPartitionStart))
                    {
                        combined.Add(new Mapping(
                            srcRangeStart: partitionStart,
                            dstRangeStart: Lookup(higherOrder, Lookup(lowerOrder, partitionStart)),
                            rangeLen: partitionEnd - partitionStart + 1));

                        partitionStart = partitionEnd + 1;
                        partitionEnd = mapping.srcRangeStart + mapping.rangeLen - 1;
                    }
                    else
                    {
                        partitionEnd = ReverseLookup(lowerOrder, dstPartitionStart.srcRangeStart + dstPartitionStart.rangeLen - 1);

                        combined.Add(new Mapping(
                            srcRangeStart: partitionStart,
                            dstRangeStart: Lookup(higherOrder, Lookup(lowerOrder, partitionStart)),
                            rangeLen: partitionEnd - partitionStart + 1));

                        partitionStart = partitionEnd + 1;
                        partitionEnd = mapping.srcRangeStart + mapping.rangeLen - 1;

                    }
                }
            }

            combined.Sort((p, q) => p.srcRangeStart.CompareTo(q.srcRangeStart));

            return combined;
        }
    }
}
