using System;
using System.Collections.Specialized;

namespace iCal
{
    public class Alarm
    {
        public StringDictionary Parameters { get; set; } = new();

        public Attachment? Attachment { get; set; }

        public string? Description { get; set; }

        public Repeat? Repeat { get; set; }

        public DateTimeOffset Trigger { get; set; }

        public object AlarmType { get; set; }
    }
}