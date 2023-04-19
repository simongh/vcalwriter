using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace vCal
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
        private int? _priority;

        public Builders.PropertyCollection Properties { get; set; } = new();

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

        public int? Priority
        {
            get => _priority;
            set
            {
                if (value.HasValue && (value < 0 || value > 9))
                    throw new ArgumentOutOfRangeException(nameof(value), "Priority must be greater than 0 and less than 9");
                _priority = value;
            }
        }

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
            builder.Write(Builders.PropertyNames.DateTimeStamp, writer);

            builder.Value.Add(Id);
            builder.Write(Builders.PropertyNames.UniqueIdentifier, writer);

            builder.Value.Add(StartsAt, datePart: AllDay ? Builders.DatePart.DateTime : Builders.DatePart.DateTime);
            builder.Write(Builders.PropertyNames.DateTimeStart, writer);

            if (Classification.HasValue)
            {
                builder.Value.Add(Classification.Value.ToVCalString());
                builder.Write(Builders.PropertyNames.Classification, writer);
            }

            if (Created.HasValue)
            {
                builder.Value.Add(Created.Value);
                builder.Write(Builders.PropertyNames.DateTimeCreated, writer);
            }
            if (!string.IsNullOrEmpty(Description))
            {
                builder.Value.Add(Description!);
                builder.Write(Builders.PropertyNames.Description, writer);
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
                }.Write(Builders.PropertyNames.GeographicPosition, writer);
            }

            if (LastModified.HasValue)
            {
                builder.Value.Add(LastModified.Value);
                builder.Write(Builders.PropertyNames.LastModified, writer);
            }

            if (!string.IsNullOrEmpty(Location))
            {
                builder.Value.Add(Location);
                builder.Write(Builders.PropertyNames.Location, writer);
            }

            if (Organiser != null)
            {
                Organiser.Write(writer);
            }

            if (Priority.HasValue)
            {
                builder.Value.Add(Priority.Value);
                builder.Write(Builders.PropertyNames.Priority, writer);
            }

            if (Sequence.HasValue)
            {
                builder.Value.Add(Sequence.Value);
                builder.Write(Builders.PropertyNames.SequenceNumber, writer);
            }

            if (Status.HasValue)
            {
                builder.Value.Add(Status.Value.ToVCalString());
                builder.Write(Builders.PropertyNames.Status, writer);
            }

            if (!string.IsNullOrEmpty(Summary))
            {
                builder.Value.Add(Summary);
                builder.Write(Builders.PropertyNames.Summary, writer);
            }

            if (Transparency.HasValue)
            {
                builder.Value.Add(Transparency.Value ? "TRANSPARENT" : "OPAQUE");
                builder.Write(Builders.PropertyNames.TimeTransparency, writer);
            }

            if (Url != null)
            {
                builder.Value.Add(Url);
                builder.Write(Builders.PropertyNames.Url, writer);
            }

            //recurid

            if (Repeat != null)
            {
                Repeat.Write(writer);
            }

            if (EndsAt.HasValue)
            {
                builder.Value.Add(EndsAt.Value, datePart: AllDay ? Builders.DatePart.DateTime : Builders.DatePart.DateTime);
                builder.Write(Builders.PropertyNames.DateTimeEnd, writer);
            }
            else if (Duration.HasValue)
            {
                builder.Value.Add(Duration.Value);
                builder.Write(Builders.PropertyNames.Duration, writer);
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
                builder.Write(Builders.PropertyNames.Categories, writer);
            }

            if (!string.IsNullOrEmpty(Contact))
            {
                builder.Value.Add(Contact);
                builder.Write(Builders.PropertyNames.Contact, writer);
            }

            if (Exclusions?.Any() == true)
            {
                builder.Value.Add(Exclusions.Select(v => v.ToVCalString()));
                builder.Write(Builders.PropertyNames.ExceptionDateTimes, writer);
            }
            //rstatus

            if (!string.IsNullOrEmpty(RelatedTo))
            {
                builder.Value.Add(RelatedTo);
                builder.Write(Builders.PropertyNames.RelatedTo, writer);
            }

            if (Resources?.Any() == true)
            {
                builder.Value.Add(Resources);
                builder.Write(Builders.PropertyNames.Resources, writer);
            }

            if (Repeats?.Any() == true)
            {
                builder.Value.Add(Repeats.Select(r => r.ToVCalString()));
                builder.Write(Builders.PropertyNames.RecurrenceDateTimes, writer);
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