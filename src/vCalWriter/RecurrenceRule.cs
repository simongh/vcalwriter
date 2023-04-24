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

    public class RecurrenceRule
    {
        public RecurrenceFrequency Frequency { get; set; }

        public DateTimeOffset? Until { get; set; }

        public int? Count { get; set; }

        public int? Interval { get; set; }

        public ICollection<int>? BySeconds { get; set; }
        public ICollection<int>? ByMinutes { get; set; }
        public ICollection<int>? ByHours { get; set; }

        public ICollection<int>? ByMonth { get; set; }
        public ICollection<DayOfWeek>? ByWeekDay { get; set; }
        public ICollection<int>? ByMonthDay { get; set; }
        public ICollection<int>? ByYearDay { get; set; }
        public ICollection<int>? ByWeek { get; set; }

        public DayOfWeek? StartOfWeek { get; set; }

        public void Write(TextWriter writer)
        {
            var builder = new Builders.PropertyBuilder
            {
                Name = Builders.PropertyNames.RecurrenceRule,
            };

            builder.Parameters.Add(new Builders.ParameterBuilder
            {
                Name = "FREQ",
            }.Add(Frequency));

            if (Count.HasValue)
            {
                builder.Parameters.Add(new Builders.ParameterBuilder
                {
                    Name = "COUNT",
                }.Add(Count.Value));
            }
            else if (Until.HasValue)
            {
                builder.Parameters.Add(new Builders.ParameterBuilder
                {
                    Name = "UNTIL",
                }.Add(Until.Value));
            }

            if (Interval.HasValue)
            {
                builder.Parameters.Add(new Builders.ParameterBuilder
                {
                    Name = "INTERVAL",
                }.Add(Interval.Value));
            }

            FromList(builder, "BYSECOND", BySeconds);
            FromList(builder, "BYMINUTE", ByMinutes);
            FromList(builder, "BYHOUR", ByHours);
            FromList(builder, "BYMONTH", ByMonth);
            FromList(builder, "BYMONTHDAY", ByMonthDay);
            FromList(builder, "BYYEARDAY", ByYearDay);
            FromList(builder, "BYWEEKNO", ByWeek);

            if (ByWeekDay?.Any() == true)
            {
                builder.Parameters.Add(new Builders.ParameterBuilder
                {
                    Name = "BYDAY"
                }.Add(ByWeekDay.Select(ToDayString)));
            }

            if (StartOfWeek.HasValue)
            {
                builder.Parameters.Add(new Builders.ParameterBuilder
                {
                    Name = "WKST"
                }.Add(ToDayString(StartOfWeek.Value)));
            }

            builder.Write(writer);
        }

        private void FromList(Builders.PropertyBuilder builder, string field, IEnumerable<int>? values)
        {
            if (values?.Any() != true)
            {
                return;
            }

            builder.Parameters.Add(new Builders.ParameterBuilder
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