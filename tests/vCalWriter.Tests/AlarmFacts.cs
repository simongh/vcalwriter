using FluentAssertions;

namespace vCalWriter.Tests
{
    public class AlarmFacts
    {
        private static string ToString(Alarm alarm)
        {
            var writer = new StringWriter();

            alarm.Write(writer);
            return writer.ToString();
        }

        [Fact]
        public void AudioAttachmentTest()
        {
            var item = new Alarm
            {
                AudioAttachment = new()
                {
                    Url = new Uri("http://value.int"),
                },
            };

            var value = ToString(item);

            value.Should().Contain("ATTACHMENT:\"http://value.int/\"\r\n");
        }

        [Fact]
        public void DescriptionTest()
        {
            var item = new Alarm
            {
                Description = "text",
                AlarmType = AlarmType.Email,
            };

            var value = ToString(item);

            value.Should().Contain("DESCRIPTION:text\r\n");
        }

        [Fact]
        public void SummaryTest()
        {
            var item = new Alarm
            {
                AlarmType = AlarmType.Email,
                Summary = "text",
            };

            var value = ToString(item);

            value.Should().Contain("SUMMARY:text\r\n");
        }

        [Fact]
        public void TriggerTest()
        {
            var item = new Alarm
            {
                Trigger = DateTimeOffset.Now.Date,
            };

            var value = ToString(item);

            value.Should().Contain("TRIGGER:");
        }

        [Fact]
        public void DurationTest()
        {
            var item = new Alarm
            {
                Duration = TimeSpan.FromMinutes(10),
            };

            var value = ToString(item);

            value.Should().Contain("DURATION:PT0H10M0S\r\n");
        }

        [Fact]
        public void RepeatTest()
        {
            var item = new Alarm
            {
                Repeat = 3,
            };

            var value = ToString(item);

            value.Should().Contain("REPEAT:3\r\n");
        }

        [Fact]
        public void SingleAttendeeTest()
        {
            var item = new Alarm
            {
                AlarmType = AlarmType.Email,
                Attendees = new[]
                {
                    new Attendee
                    {
                        Email = "name@domain.int",
                    },
                },
            };

            var value = ToString(item);

            value.Should().Contain("ATTENDEE:name@domain.int\r\n");
        }

        [Fact]
        public void MultipleAttendeeTest()
        {
            var item = new Alarm
            {
                AlarmType = AlarmType.Email,
                Attendees = new[]
                {
                    new Attendee
                    {
                        Email = "name@domain.int",
                    },
                    new Attendee
                    {
                        Email = "two@domain.int",
                    },
                },
            };

            var value = ToString(item);

            value.Should().Contain("ATTENDEE:name@domain.int\r\n");
            value.Should().Contain("ATTENDEE:two@domain.int\r\n");
        }

        private Alarm TestAlarm => new Alarm
        {
            AlarmType = AlarmType.Audio,
            Attendees = new[]
                {
                    new Attendee
                    {
                        Email = "email",
                    },
                },
            Attachments = new[]
                {
                    new Attachment
                    {
                        Data = new byte[] {0x0},
                    },
                },
            AudioAttachment = new()
            {
                Data = new byte[] { 0x0 },
            },
            Description = "text",
            Summary = "text",
            Duration = TimeSpan.FromMinutes(10),
            Repeat = 3,
            Trigger = DateTimeOffset.Now,
        };

        [Fact]
        public void AudioAlarmTest()
        {
            var item = TestAlarm;
            item.AlarmType = AlarmType.Audio;

            var value = ToString(item);

            value.Should()
                .NotContain("DESCRIPTION")
                .And.NotContain("SUMMARY")
                .And.NotContain("ATTENDEE");

            value.Should()
                .Contain("ATTACHMENT")
                .And.Contain("TRIGGER")
                .And.Contain("ACTION")
                .And.Contain("DURATION")
                .And.Contain("REPEAT");
        }

        [Fact]
        public void DisplayAlarmTest()
        {
            var item = TestAlarm;
            item.AlarmType = AlarmType.Display;

            var value = ToString(item);

            value.Should()
                .NotContain("SUMMARY")
                .And.NotContain("ATTACHMENT")
                .And.NotContain("ATTENDEE");

            value.Should()
                .Contain("DESCRIPTION")
                .And.Contain("TRIGGER")
                .And.Contain("ACTION")
                .And.Contain("DURATION")
                .And.Contain("REPEAT");
        }

        [Fact]
        public void EmailAlarmTest()
        {
            var item = TestAlarm;
            item.AlarmType = AlarmType.Email;

            var value = ToString(item);

            value.Should()
                .Contain("SUMMARY")
                .And.Contain("ATTACHMENT")
                .And.Contain("ATTENDEE")
                .And.Contain("DESCRIPTION")
                .And.Contain("TRIGGER")
                .And.Contain("ACTION")
                .And.Contain("DURATION")
                .And.Contain("REPEAT");
        }
    }
}