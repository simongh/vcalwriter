using System.Net.Mime;

namespace vCalWriter
{
    public class Attachment
    {
        public Builders.ParameterCollection Parameters { get; set; } = new();

        /// <summary>
        /// Sets the url of the attachment. Will be used instead of <see cref="Data"/> if set
        /// </summary>
        public Uri? Url { get; set; }

        public ContentType? ContentType { get; set; }

        /// <summary>
        /// Sets the data to be used. If a <see cref="Url"/> is set, this wll be ignored
        /// </summary>
        public byte[]? Data { get; set; }

        /// <summary>
        /// Sets other data to be used. Will be ignored if <see cref="Url"/> or <see cref="Data"/> is set
        /// </summary>
        public string? Value { get; set; }

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
            else if (!string.IsNullOrEmpty(Value))
            {
                builder.Value.Add(Value);
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