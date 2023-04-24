namespace vCalWriter
{
    public class EmailAlarm : Alarm
    {
        public ICollection<Attendee> Attendees { get; set; } = new List<Attendee>();

        public string? Description { get; set; }

        public string? Summary { get; set; }

        public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();

        public EmailAlarm()
        {
            AlarmType = AlarmType.Email;
        }

        protected override void InnerWrite(TextWriter writer)
        {
            if (Attendees != null)
            {
                foreach (var item in Attendees)
                {
                    item.Write(writer);
                }
            }

            var builder = new Builders.PropertyBuilder();

            builder.Value.Add(Description);
            builder.Write(Builders.PropertyNames.Description, writer);

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
    }
}