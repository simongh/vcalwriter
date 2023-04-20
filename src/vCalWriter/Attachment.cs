using System.Net.Mime;

namespace vCalWriter
{
    public class Attachment
    {
        public Uri? Url { get; set; }

        public ContentType? ContentType { get; set; }

        public byte[]? Data { get; set; }

        public void Write(StringWriter writer)
        {
            var builder = new Builders.PropertyBuilder
            {
                Name = Builders.PropertyNames.Attachment,
            };

            if (Url != null)
            {
                builder.Value.Add(Url);
            }
            else if (Data != null)
            {
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

            builder.Write(writer);
        }
    }
}