namespace Compiler.Cache
{
    public class Line
    {
        public Line(int number, string content)
        {
            Number = number;
            Content = content ?? string.Empty;
        }

        public int Number { get; set; }

        public string Content { get; set; }
    }
}
