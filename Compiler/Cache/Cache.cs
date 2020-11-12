using System.Collections.Generic;

namespace Compiler.Cache
{
    public class Cache
    {
        private static List<Line> Lines { get; } = new List<Line>();

        public static void Clear() => Lines.Clear();

        public static void Populate(string content)
        {
            var lineNumber = Lines.Count;

            Lines.Add(new Line(lineNumber, content));
        }

        public static Line GetLine(int lineNumber)
        {
            if (LineExists(lineNumber))
            {
                return Lines[lineNumber - 1];
            }
            return new Line(Lines.Count + 1, "@EOF@");
        }

        private static bool LineExists(int lineNumber) => lineNumber <= Lines.Count;
    }
}
