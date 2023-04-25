namespace vCalWriter
{
    public class Organiser
    {
        public Builders.ParameterCollection Parameters { get; set; } = new();

        /// <summary>
        /// Sets the organisers email. Required
        /// </summary>
        public string? Email { get; set; }

        public string? Name { get; set; }

        public Uri? SentBy { get; set; }

        public void Write(TextWriter writer)
        {
            var builder = new Builders.PropertyBuilder();

            if (string.IsNullOrEmpty(Email))
                return;

            if (SentBy != null)
            {
                builder.Parameters.Add(new Builders.ParameterBuilder
                {
                    Name = Builders.ParameterNames.SentBy,
                }.Add(SentBy));
            }

            if (!string.IsNullOrEmpty(Name))
            {
                builder.Parameters.Add(new Builders.ParameterBuilder
                {
                    Name = Builders.ParameterNames.CommonName,
                }.Add(Name));
            }

            if (Parameters != null)
            {
                builder.Parameters.Merge(Parameters);
            }

            builder.Value.Add(Email);
            builder.Write(Builders.PropertyNames.Organizer, writer);
        }
    }
}