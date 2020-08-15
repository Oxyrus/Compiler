using System;

namespace Compiler
{
    public class Output
    {
        public Output(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            SplitLines(value);
            FormatValue();
        }

        public string[] Value { get; private set; }

        public string FormattedValue { get; private set; }

        private void SplitLines(string value)
        {
            Value = value.Split("\r\n", StringSplitOptions.None);
        }

        private void FormatValue()
        {
            for (int i = 0; i < Value.Length; i++)
            {
                var line = (i + 1).ToString() + " " + Value[i] + "\r\n";

                FormattedValue += line;
            }
        }
    }
}
