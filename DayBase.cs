using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace AdventOfCode2020
{
    public abstract class DayBase<T>
    {
        private const string Session = "";
        protected T Input { get; set; }
        protected abstract T ProcessInput(List<string> rawInput);
        public abstract int Part1();
        public abstract int Part2();

        protected List<string> ReadInput(int day)
        {
            var fileLines = File.ReadLines("input.txt").ToList();
            if (fileLines.Any())
                return fileLines.ToList();

            var webLines = GetInput($"https://adventofcode.com/2020/day/{day}/input");
            File.WriteAllLines("input.txt", webLines);

            return webLines;
        }

        private List<string> GetInput(string uri)
        {
            var request = (HttpWebRequest) WebRequest.Create(uri);
            var cookieContainer = new CookieContainer();
            var cookie = new Cookie("session", Session)
            {
                Domain = "adventofcode.com"
            };
            cookieContainer.Add(cookie);
            request.CookieContainer = cookieContainer;

            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.Brotli;

            using var response = request.GetResponse();
            using var stream = response.GetResponseStream();
            using var reader = new StreamReader(stream);

            string line;
            var lines = new List<string>();
            while ((line = reader.ReadLine()) != null)
                lines.Add(line);

            return lines;
        }
    }
}
