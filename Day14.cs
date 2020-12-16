using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    public sealed class Day14 : DayBase<Dictionary<string, List<(int, int)>>>
    {
        protected override int Day { get; } = 14;

        public Day14() => Input = ProcessInput(ReadInput());

        protected override Dictionary<string, List<(int, int)>> ProcessInput(List<string> rawInput)
        {
            var instructions = new Dictionary<string, List<(int, int)>>();
            string currentMask = "";
            foreach (var line in rawInput)
            {
                if (line.StartsWith("mask = "))
                {
                    currentMask = line.Split("mask = ")[1];
                    instructions[currentMask] = new List<(int, int)>();
                }
                else
                {
                    var memory = line.Replace("mem[", "").Split("] = ");
                    instructions[currentMask].Add((int.Parse(memory[0]), int.Parse(memory[1])));
                }
            }

            return instructions;
        }

        public override long Part1()
        {
            var memoryAddressValues = new long[Input.Values.SelectMany(x => x).Select(x => x.Item1).Max() + 1];

            foreach ((string mask, List<(int, int)> memories) in Input)
            {
                foreach ((int memoryAddress, int memoryValue) in memories)
                {
                    var newMemoryValueBits = Convert.ToString(memoryValue, 2).PadLeft(mask.Length, '0').ToArray();
                    for (var i = 0; i < mask.Length; i++)
                    {
                        if (mask[i] == '0' || mask[i] == '1')
                            newMemoryValueBits[i] = mask[i];
                    }

                    memoryAddressValues[memoryAddress] = Convert.ToInt64(string.Join("", newMemoryValueBits), 2);
                }
            }

            return memoryAddressValues.Sum();
        }

        public override long Part2()
        {
            var memoryAddressValues = new Dictionary<long, long>();
            foreach ((string mask, List<(int, int)> memories) in Input)
            {
                foreach ((int memoryAddress, int memoryValue) in memories)
                {
                    var memoryAddressBits = Convert.ToString(memoryAddress, 2).PadLeft(mask.Length, '0').ToArray();
                    var xCount = 0;
                    for (var i = 0; i < mask.Length; i++)
                    {
                        if (mask[i] == '0')
                            continue;

                        if (mask[i] == '1')
                            memoryAddressBits[i] = mask[i];
                        else if (mask[i] == 'X')
                        {
                            memoryAddressBits[i] = mask[i];
                            xCount++;
                        }
                    }

                    var permutations = Utils.GeneratePermutations(new[] {'0', '1'}, xCount);
                    foreach (var permutation in permutations)
                    {
                        var xIndex = 0;
                        var memoryAddressBitsCopy = new string(memoryAddressBits).ToArray();
                        for (var i = 0; i < mask.Length; i++)
                        {
                            if (memoryAddressBitsCopy[i] == 'X')
                                memoryAddressBitsCopy[i] = permutation[xIndex++];
                        }

                        var newAddress = Convert.ToInt64(string.Join("", memoryAddressBitsCopy), 2);
                        memoryAddressValues[newAddress] = memoryValue;
                    }
                }
            }

            return memoryAddressValues.Values.Sum();
        }
    }
}