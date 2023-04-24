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

        public DateTimeOffset Trigger { get; set; }

        public AlarmType AlarmType { get; set; }

        public TimeSpan? Duration { get; set; }

        public int? Repeat { get; set; }

        protected virtual void InnerWrite(TextWriter writer)
        { }

        public void Write(TextWriter writer)
        {
            writer.WriteLine("BEGIN:VALARM");

            var builder = new Builders.PropertyBuilder();

            builder.Value.Add(AlarmType.ToVCalString());
            builder.Write(Builders.PropertyNames.Action, writer);

            builder.Value.Add(Trigger);
            builder.Write(Builders.PropertyNames.Trigger, writer);

            InnerWrite(writer);

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