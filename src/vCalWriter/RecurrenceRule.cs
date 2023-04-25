namespace vCalWriter
{
    public enum RecurrenceFrequency
    {
        Seconds,
        Minutes,
        Hours,
        Days,
        Weeks,
        Monthly,
        Years,
    }

    /// <summary>
    /// Define a repeat pattern
    /// </summary>
    public class RecurrenceRule
    {
        /// <summary>
        /// Add a addititional parameters to the property
        /// </summary>
        public Builders.ParameterCollection Parameters { get; set; } = new();

        /// <summary>
        /// Sets the frequency of the repeat. Required
        /// </summary>
        public RecurrenceFrequency Frequency { get; set; }

        /// <summary>
        /// Sets the end of the repeat. If <see cref="Count"/> is set, that will be used in preference
        /// </summary>
        public DateTimeOffset? Until { get; set; }

        /// <summary>
        /// Sets the timezone format to use for the repeat end
        /// </summary>
        public DateTimeKind TimeKind { get; set; } = DateTimeKind.Local;

        /// <summary>
        /// Sets the date format for the repeat end
        /// </summary>
        public Builders.DatePart DatePart { get; set; } = Builders.DatePart.DateTime;

        /// <summary>
        /// Sets the number of repeats. This will be used in preference to <see cref="Until"/> if set
        /// </summary>
        public int? Count { get; set; }

        /// <summary>
        /// Sets the gap between repeats. Should be a positive value
        /// </summary>
        public int? Interval { get; set; }

        /// <summary>
        /// Sets the seconds to repeat on. Valid values are 0-60
        /// </summary>
        public ICollection<int>? BySeconds { get; set; }

        /// <summary>
        /// Ste the minutes to repeat on. Valid values are 0-59
        /// </summary>
        public ICollection<int>? ByMinutes { get; set; }

        /// <summary>
        /// Sets the hours to repeat on. Valid values are 0-23
        /// </summary>
        public ICollection<int>? ByHours { get; set; }

        /// <summary>
        /// Sets the month to repeat on. Valid values are 1-12
        /// </summary>
        public ICollection<int>? ByMonth { get; set; }

        /// <summary>
        /// Sets the days of the week to repeat on. Should only be set when the <see cref="Frequency"/> is Monthly or Yearly. An optional offet can also be set and should be 1-4 and can be negative
        /// </summary>
        public ICollection<(DayOfWeek Day, int? Offset)>? ByWeekDay { get; set; }

        /// <summary>
        /// Sets the month day to repeat on. Valid values are 1-31 and can be negative
        /// </summary>
        public ICollection<int>? ByMonthDay { get; set; }

        /// <summary>
        /// Sets the day in the year to repeat on. Valid values are 1-366 and can be negative
        /// </summary>
        public ICollection<int>? ByYearDay { get; set; }

        /// <summary>
        /// Sets the weeks to repeat on. Valid values are 1-53 and can be negative
        /// </summary>
        public ICollection<int>? ByWeek { get; set; }

        /// <summary>
        /// Sets the start of the working week and defaults to Monday
        /// </summary>
        public DayOfWeek? StartOfWeek { get; set; }

        public void Write(TextWriter writer)
        {
            var parts = new Builders.ParameterCollection
            {
                new Builders.ParameterBuilder
                {
                    Name = "FREQ",
                }.Add(Frequency.ToVCalString())
            };

            if (Count.HasValue)
            {
                parts.Add(new Builders.ParameterBuilder
                {
                    Name = "COUNT",
                }.Add(Count.Value));
            }
            else if (Until.HasValue)
            {
                parts.Add(new Builders.ParameterBuilder
                {
                    Name = "UNTIL",
                }.Add(Until.Value, DatePart, TimeKind));
            }

            if (Interval.HasValue)
            {
                parts.Add(new Builders.ParameterBuilder
                {
                    Name = "INTERVAL",
                }.Add(Interval.Value));
            }

            FromList(parts, "BYSECOND", BySeconds);
            FromList(parts, "BYMINUTE", ByMinutes);
            FromList(parts, "BYHOUR", ByHours);
            FromList(parts, "BYMONTH", ByMonth);
            FromList(parts, "BYMONTHDAY", ByMonthDay);
            FromList(parts, "BYYEARDAY", ByYearDay);
            FromList(parts, "BYWEEKNO", ByWeek);

            if (ByWeekDay?.Any() == true)
            {
                parts.Add(new Builders.ParameterBuilder
                {
                    Name = "BYDAY"
                }.Add(ByWeekDay.Select(v => $"{(v.Offset.HasValue ? v.Offset : "")}{ToDayString(v.Day)}")));
            }

            if (StartOfWeek.HasValue)
            {
                parts.Add(new Builders.ParameterBuilder
                {
                    Name = "WKST"
                }.Add(ToDayString(StartOfWeek.Value)));
            }

            var builder = new Builders.PropertyBuilder
            {
                Name = Builders.PropertyNames.RecurrenceRule,
            };

            if (Parameters != null)
            {
                builder.Parameters.Merge(Parameters);
            }

            builder.Value.Add(parts);
            builder.Write(writer);
        }

        private void FromList(Builders.ParameterCollection builder, string field, IEnumerable<int>? values)
        {
            if (values?.Any() != true)
            {
                return;
            }

            builder.Add(new Builders.ParameterBuilder
            {
                Name = field
            }.Add(values));
        }

        private string ToDayString(DayOfWeek dayOfWeek)
        {
            return dayOfWeek switch
            {
                DayOfWeek.Sunday => "SU",
                DayOfWeek.Monday => "MO",
                DayOfWeek.Tuesday => "TU",
                DayOfWeek.Wednesday => "WE",
                DayOfWeek.Thursday => "TH",
                DayOfWeek.Friday => "FR",
                DayOfWeek.Saturday => "SA",
                _ => throw new ApplicationException("Unexpected day of the week"),
            };
        }
    }
}