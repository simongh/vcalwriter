namespace vCalWriter
{
    public class DisplayAlarm : Alarm
    {
        public string? Description { get; set; }

        public DisplayAlarm()
        {
            AlarmType = AlarmType.Display;
        }

        protected override void InnerWrite(TextWriter writer)
        {
            var builder = new Builders.PropertyBuilder();

            builder.Value.Add(Description);
            builder.Write(Builders.PropertyNames.Description, writer);
        }
    }
}