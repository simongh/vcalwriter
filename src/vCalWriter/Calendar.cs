using System.Collections.Specialized;

namespace vCalWriter
{
    public class Calendar
    {
        public string ProductId { get; set; } = "Simon//SimplevCal";

        public StringCollection Parameters { get; set; } = new();

        public ICollection<CalendarEntry> Entries { get; set; } = new List<CalendarEntry>();

        public ICollection<Alarm> Alarms { get; set; } = new List<Alarm>();

        public void Write(TextWriter writer)
        {
            writer.WriteLine("BEGIN:VCALENDAR");
            writer.WriteLine("PRODID:");
            writer.WriteLine(ProductId);
            writer.WriteLine("VERSION:2.0");

            if (Parameters != null)
            {
                foreach (var item in Parameters)
                {
                    writer.WriteLine(item);
                }
            }

            if (Entries != null)
            {
                foreach (var item in Entries)
                {
                    item.Write(writer);
                }
            }

            if (Alarms != null)
            {
                foreach (var item in Alarms)
                {
                    item.Write(writer);
                }
            }

            writer.Write("END:VCALENDAR");
        }
    }
}