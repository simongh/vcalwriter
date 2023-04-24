using System.Net.Mime;

namespace vCalWriter
{
    public class Attachment
    {
        public Builders.ParameterCollection Parameters { get; set; } = new();

        public Uri? Url { get; set; }

        public ContentType? ContentType { get; set; }

        public byte[]? Data { get; set; }

        public void Write(TextWriter writer)
        {
            var builder = new Builders.PropertyBuilder
            {
                Name = Builders.PropertyNames.Attachment,
            };

            if (Url != null)
            {
                builder.Value.Add(Url.ToString());
            }
            else if (Data != null)
            {
                builder.Parameters.Add(new Builders.ParameterBuilder()
                {
                    Name = Builders.ParameterNames.Encoding
                }.Add("BASE64"));
                builder.Parameters.Add(new Builders.ParameterBuilder
                {
                    Name = Builders.ParameterNames.DataType,
                }.Add("BINARY"));
                builder.Value.Add(Data);
            }
            else
                return;

            if (ContentType != null)
            {
                builder.Parameters.Add(new Builders.ParameterBuilder
                {
                    Name = Builders.ParameterNames.FormatType
                }.Add(ContentType.ToString()));
            }

            if (Parameters != null)
            {
                builder.Parameters.Merge(Parameters);
            }

            builder.Write(writer);
        }
    }
}