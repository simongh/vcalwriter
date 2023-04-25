namespace vCalWriter
{
    public enum AlarmType
    {
        Audio,
        Display,
        Email,
    }

    /// <summary>
    /// Basic alarm containg required properties
    /// </summary>
    public class Alarm
    {
        public Builders.PropertyCollection Properties { get; set; } = new();

        public DateTimeOffset Trigger { get; set; }

        /// <summary>
        /// Sets the type of alarm. Leave null to use a define a custom alarm
        /// </summary>
        public AlarmType? AlarmType { get; set; }

        public TimeSpan? Duration { get; set; }

        public int? Repeat { get; set; }

        protected virtual void InnerWrite(TextWriter writer)
        { }

        public void Write(TextWriter writer)
        {
            writer.WriteLine("BEGIN:VALARM");

            var builder = new Builders.PropertyBuilder();

            if (AlarmType.HasValue)
            {
                builder.Value.Add(AlarmType.Value.ToVCalString());
                builder.Write(Builders.PropertyNames.Action, writer);
            }

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