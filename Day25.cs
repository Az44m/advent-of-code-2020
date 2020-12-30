using System.Collections.Generic;

namespace AdventOfCode2020
{
    public sealed class Day25 : DayBase<(int, int)>
    {
        protected override int Day { get; } = 25;

        public Day25() => Input = ProcessInput(ReadInput());

        protected override (int, int) ProcessInput(List<string> rawInput) => (int.Parse(rawInput[0]), int.Parse(rawInput[1]));

        public override long Part1()
        {
            (int publicKeyCard, int publicKeyDoor) = Input;

            long transform = 1;
            long encryptionKey = 1;
            var mod = 20201227;

            while (transform != publicKeyCard)
            {
                transform = transform * 7 % mod;
                encryptionKey = encryptionKey * publicKeyDoor % mod;
            }

            return encryptionKey;
        }

        public override long Part2() => -1;
    }
}