using System.IO;

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

        private static void Fold(StringWriter writer, string value)
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
    }
}