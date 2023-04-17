using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;

namespace iCal
{
    public class Calendar
    {
        public string ProductId { get; set; } = "Simon//iCal";

        public StringCollection Parameters { get; set; } = new();

        public ICollection<CalendarEntry> Entries { get; set; } = new List<CalendarEntry>();

        public void Write(StringWriter writer)
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

            writer.Write("END:VCALENDAR");
        }
    }
}