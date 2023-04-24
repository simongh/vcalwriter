namespace vCalWriter.Builders
{
    public static class ContentFolder
    {
        public static string Fold(string value)
        {
            using var reader = new StringReader(value);
            using var writer = new StringWriter();

            string line;
            while (null != (line = reader.ReadLine()))
            {
                Fold(writer, line);
            }

            return writer.ToString();
        }

        private static void Fold(TextWriter writer, string value)
        {
            if (value.Length < 74)
            {
                writer.Write(value);
                return;
            }

            writer.WriteLine(value.Substring(0, 74));
            writer.Write(' ');
            Fold(value.Substring(74));
        }

        public static void Fold(Stream data, Stream output)
        {
            using var reader = new StreamReader(data);
            using var writer = new StreamWriter(output);

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                Fold(writer, line);
            }
        }
    }
}