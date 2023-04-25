using FluentAssertions;

namespace vCalWriter.Tests
{
    public class RecurrenceRuleFacts
    {
        private static string ToString(RecurrenceRule item)
        {
            var writer = new StringWriter();

            item.Write(writer);
            return writer.ToString();
        }

        public static IEnumerable<object[]> FrequencyData
            => Enum
                .GetValues<RecurrenceFrequency>()
                .Select(v => new object[] { v });

        [Theory]
        [MemberData(nameof(FrequencyData))]
        public void FrequencyTest(RecurrenceFrequency recurrence)
        {
            var rule = new RecurrenceRule
            {
                Frequency = recurrence,
            };

            var value = ToString(rule);

            value.Should()
                .Be($"RRULE:FREQ={recurrence.ToVCalString()}\r\n");
        }

        [Fact]
        public void CountTest()
        {
            var rule = new RecurrenceRule
            {
                Frequency = RecurrenceFrequency.Hours,
                Count = 5,
            };

            var value = ToString(rule);

            value.Should()
                .Be("RRULE:FREQ=HOURLY;COUNT=5\r\n");
        }

        [Fact]
        public void UntilTest()
        {
            var when = DateTimeOffset.UtcNow;

            var rule = new RecurrenceRule
            {
                Frequency = RecurrenceFrequency.Hours,
                Until = when,
            };

            var value = ToString(rule);

            value.Should()
                .Be($"RRULE:FREQ=HOURLY;UNTIL={when:yyyyMMddTHHmmss}\r\n");
        }

        [Fact]
        public void IntervalTest()
        {
            var rule = new RecurrenceRule
            {
                Frequency = RecurrenceFrequency.Hours,
                Interval = 5,
            };

            var value = ToString(rule);

            value.Should()
                .Be($"RRULE:FREQ=HOURLY;INTERVAL=5\r\n");
        }

        public static IEnumerable<object[]> ByData
        {
            get
            {
                yield return new object[] { "BYSECOND" };
                yield return new object[] { "BYMINUTE" };
                yield return new object[] { "BYHOUR" };
                yield return new object[] { "BYMONTH" };
                yield return new object[] { "BYMONTHDAY" };
                yield return new object[] { "BYYEARDAY" };
                yield return new object[] { "BYWEEKNO" };
            }
        }

        [Theory]
        [MemberData(nameof(ByData))]
        public void ByTest(string byPart)
        {
            var rule = new RecurrenceRule
            {
                Frequency = RecurrenceFrequency.Hours,
            };

            var parts = new[] { 5, 10 };

            switch (byPart)
            {
                case "BYSECOND":
                    rule.BySeconds = parts;
                    break;

                case "BYMINUTE":
                    rule.ByMinutes = parts;
                    break;

                case "BYHOUR":
                    rule.ByHours = parts;
                    break;

                case "BYMONTH":
                    rule.ByMonth = parts;
                    break;

                case "BYMONTHDAY":
                    rule.ByMonthDay = parts;
                    break;

                case "BYYEARDAY":
                    rule.ByYearDay = parts;
                    break;

                case "BYWEEKNO":
                    rule.ByWeek = parts;
                    break;
            }

            var value = ToString(rule);

            value.Should()
                .Be($"RRULE:FREQ=HOURLY;{byPart}=5,10\r\n");
        }

        public static IEnumerable<object[]> DayData
            => Enum
            .GetValues<DayOfWeek>()
            .Select(v => new object[] { v });

        [Theory]
        [MemberData(nameof(DayData))]
        public void ByDayTest(DayOfWeek day)
        {
            var rule = new RecurrenceRule
            {
                Frequency = RecurrenceFrequency.Hours,
                ByWeekDay = new[] { (day, default(int?)), (day, null) },
            };

            var value = ToString(rule);

            var dd = day.ToString().Substring(0, 2).ToUpper();

            value.Should()
                .Be($"RRULE:FREQ=HOURLY;BYDAY={dd},{dd}\r\n");
        }

        [Theory]
        [MemberData(nameof(DayData))]
        public void StartOfWeekTest(DayOfWeek day)
        {
            var rule = new RecurrenceRule
            {
                Frequency = RecurrenceFrequency.Hours,
                StartOfWeek = day,
            };

            var value = ToString(rule);

            value.Should()
                .Be($"RRULE:FREQ=HOURLY;WKST={day.ToString().Substring(0, 2).ToUpper()}\r\n");
        }
    }
}