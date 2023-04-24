namespace vCalWriter.Builders
{
    public class PropertyBuilder
    {
        public string Name { get; set; }

        public ParameterBuilder Value { get; set; } = new();

        public ParameterCollection Parameters { get; set; } = new();

        internal void Write(string name, TextWriter writer)
        {
            Name = name;
            Write(writer);
        }

        public void Write(TextWriter writer)
        {
            if (string.IsNullOrEmpty(Name))
            {
                return;
            }

            writer.Write(Name);
            WriteValue(writer);
        }

        private void WriteValue(TextWriter writer)
        {
            if (Parameters != null)
            {
                foreach (var param in Parameters)
                {
                    writer.Write(";");
                    param.Write(writer);
                }
            }

            writer.Write(':');
            if (Value != null)
            {
                Value.Write(writer);
            }
            writer.WriteLine();
        }

        public override string ToString()
        {
            using var sw = new StringWriter();
            Write(sw);

            return sw.ToString();
        }
    }
}