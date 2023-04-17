using System;
using System.IO;

namespace iCal
{
    public class Organiser
    {
        public string? Email { get; set; }

        public string? Name { get; set; }

        public Uri? SentBy { get; set; }

        public void Write(StringWriter writer)
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

            builder.Value.Add(Email);
            builder.Write("ORGANIZER", writer);
        }
    }
}