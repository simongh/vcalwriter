namespace vCalWriter
{
    public enum AlarmType
    {
        Audio,
        Display,
        Email,
    }

    public class Alarm
    {
        public Builders.PropertyCollection Properties { get; set; } = new();

        public Attachment? AudioAttachment { get; set; }

        public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();

        public string? Description { get; set; }

        public string? Summary { get; set; }

        public DateTimeOffset Trigger { get; set; }

        public AlarmType AlarmType { get; set; }

        public TimeSpan? Duration { get; set; }

        public int? Repeat { get; set; }

        public ICollection<Attendee> Attendees { get; set; } = new List<Attendee>();

        public void Write(StringWriter writer)
        {
            writer.WriteLine("BEGIN:VALARM");

            var builder = new Builders.PropertyBuilder();

            builder.Value.Add(AlarmType.ToVCalString());
            builder.Write(Builders.PropertyNames.Action, writer);

            builder.Value.Add(Trigger);
            builder.Write(Builders.PropertyNames.Trigger, writer);

            if (AlarmType == AlarmType.Display || AlarmType == AlarmType.Email)
            {
                builder.Value.Add(Description);
                builder.Write(Builders.PropertyNames.Description, writer);
            }

            if (AlarmType == AlarmType.Email)
            {
                if (Attendees != null)
                {
                    foreach (var item in Attendees)
                    {
                        item.Write(writer);
                    }
                }

                builder.Value.Add(Summary);
                builder.Write(Builders.PropertyNames.Summary, writer);

                if (Attachments != null)
                {
                    foreach (var item in Attachments)
                    {
                        item.Write(writer);
                    }
                }
            }

            if (Duration.HasValue)
            {
                builder.Value.Add(Duration.Value);
                builder.Write(Builders.PropertyNames.Duration, writer);
            }

            if (Repeat.HasValue)
            {
                builder.Value.Add(Repeat.Value);
                builder.Write(Builders.PropertyNames.Repeat, writer);
            }

            if (AlarmType == AlarmType.Audio && AudioAttachment != null)
            {
                AudioAttachment.Write(writer);
            }

            if (Properties != null)
            {
                foreach (var property in Properties)
                {
                    property.Write(writer);
                }
            }

            writer.WriteLine("END:VALARM");
        }
    }
}