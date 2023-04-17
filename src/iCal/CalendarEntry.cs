using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using vCal.Builders;

namespace iCal
{
    public enum Classification
    {
        Public,
        Private,
        Confidential,
    }

    public enum EventStatus
    {
        Tentative,
        Confirmed,
        Cancelled,
    }

    public class CalendarEntry
    {
        public PropertyCollection Properties { get; set; } = new();

        public string Id { get; set; } = Guid.NewGuid().ToString();

        public DateTimeOffset TimeStamp { get; set; } = DateTimeOffset.UtcNow;

        public DateTimeOffset StartsAt { get; set; }

        public bool AllDay { get; set; }

        public Classification? Classification { get; set; }
        public DateTimeOffset? Created { get; set; }

        public string? Description { get; set; }

        public GeoLocation? GeoLocation { get; set; }

        public DateTimeOffset? LastModified { get; set; }
        public string? Location { get; set; }
        public Organiser? Organiser { get; set; }
        public int? Priority { get; set; }
        public int? Sequence { get; set; }
        public EventStatus? Status { get; set; }
        public string? Summary { get; set; }
        public bool? Transparency { get; set; }
        public Uri? Url { get; set; }

        public RecurrenceRule? Repeat { get; set; }

        public DateTimeOffset? EndsAt { get; set; }

        public TimeSpan? Duration { get; set; }

        public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
        public ICollection<Attendee> Attendees { get; set; } = new List<Attendee>();
        public ICollection<string> Categories { get; set; } = new List<string>();
        public ICollection<string> Comments { get; set; } = new List<string>();

        public string? Contact { get; set; }

        public string? RelatedTo { get; set; }
        public ICollection<string> Resources { get; set; } = new List<string>();

        public ICollection<DateTimeOffset> Exclusions { get; set; } = new List<DateTimeOffset>();

        public ICollection<DateTimeOffset> Repeats { get; set; } = new List<DateTimeOffset>();

        public void Write(StringWriter writer)
        {
            writer.WriteLine("BEGIN:VEVENT");

            var builder = new Builders.PropertyBuilder();

            builder.Value.Add(TimeStamp);
            builder.Write("DTSTAMP", writer);

            builder.Value.Add(Id);
            builder.Write("UID", writer);

            builder.Value.Add(StartsAt, datePart: AllDay ? Builders.DatePart.DateTime : Builders.DatePart.DateTime);
            builder.Write("DTSTART", writer);

            if (Classification.HasValue)
            {
                builder.Value.Add(Classification.Value.ToVCalString());
                builder.Write("CLASS", writer);
            }

            if (Created.HasValue)
            {
                builder.Value.Add(Created.Value);
                builder.Write("CREATED", writer);
            }
            if (!string.IsNullOrEmpty(Description))
            {
                builder.Value.Add(Description!);
                builder.Write("DESCRIPTION", writer);
            }

            if (GeoLocation.HasValue)
            {
                new Builders.PropertyBuilder
                {
                    Parameters = new()
                    {
                        new Builders.ParameterBuilder().Add(GeoLocation.Value.Latitude),
                        new Builders.ParameterBuilder().Add(GeoLocation.Value.Longitude)
                    }
                }.Write("GEO", writer);
            }

            if (LastModified.HasValue)
            {
                builder.Value.Add(LastModified.Value);
                builder.Write("LAST-MODIFIED", writer);
            }

            if (!string.IsNullOrEmpty(Location))
            {
                builder.Value.Add(Location);
                builder.Write("LOCATION", writer);
            }

            if (Organiser != null)
            {
                Organiser.Write(writer);
            }

            if (Priority.HasValue)
            {
                builder.Value.Add(Priority.Value);
                builder.Write("PRIORITY", writer);
            }

            if (Sequence.HasValue)
            {
                builder.Value.Add(Sequence.Value);
                builder.Write("SEQUENCE", writer);
            }

            if (Status.HasValue)
            {
                builder.Value.Add(Status.Value.ToVCalString());
                builder.Write("STATUS", writer);
            }

            if (!string.IsNullOrEmpty(Summary))
            {
                builder.Value.Add(Summary);
                builder.Write("SUMMARY", writer);
            }

            if (Transparency.HasValue)
            {
                builder.Value.Add(Transparency.Value ? "TRANSPARENT" : "OPAQUE");
                builder.Write("TRANSP", writer);
            }

            if (Url != null)
            {
                builder.Value.Add(Url);
                builder.Write("URL", writer);
            }

            //recurid

            if (Repeat != null)
            {
                Repeat.Write(writer);
            }

            if (EndsAt.HasValue)
            {
                builder.Value.Add(EndsAt.Value, datePart: AllDay ? Builders.DatePart.DateTime : Builders.DatePart.DateTime);
                builder.Write("DTEND", writer);
            }
            else if (Duration.HasValue)
            {
                builder.Value.Add(Duration.Value);
                builder.Write("DURATION", writer);
            }

            //attach

            if (Attendees != null)
            {
                foreach (var item in Attendees)
                {
                    item.Write(writer);
                }
            }

            if (Categories?.Any() == true)
            {
                builder.Value.Add(Categories);
                builder.Write("CATEGORIES", writer);
            }

            if (!string.IsNullOrEmpty(Contact))
            {
                builder.Value.Add(Contact);
                builder.Write("CONTACT", writer);
            }

            if (Exclusions?.Any() == true)
            {
                builder.Value.Add(Exclusions.Select(v => v.ToVCalString()));
                builder.Write("EXDATE", writer);
            }
            //rstatus

            if (!string.IsNullOrEmpty(RelatedTo))
            {
                builder.Value.Add(RelatedTo);
                builder.Write("RELATED-TO", writer);
            }

            if (Resources?.Any() == true)
            {
                builder.Value.Add(Resources);
                builder.Write("RESOURCES", writer);
            }

            if (Repeats?.Any() == true)
            {
                builder.Value.Add(Repeats.Select(r => r.ToVCalString()));
                builder.Write("RDATE", writer);
            }

            if (Properties?.Any() == true)
            {
                foreach (var parameter in Properties)
                {
                    parameter.Write(writer);
                }
            }

            writer.WriteLine("END:VEVENT");
        }
    }
}