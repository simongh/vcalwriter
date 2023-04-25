namespace vCalWriter
{
    public class AudioAlarm : Alarm
    {
        public Attachment? Attachment { get; set; }

        public AudioAlarm()
        {
            AlarmType = vCalWriter.AlarmType.Audio;
        }

        protected override void InnerWrite(TextWriter writer)
        {
            if (Attachment != null)
            {
                Attachment.Write(writer);
            }
        }
    }
}