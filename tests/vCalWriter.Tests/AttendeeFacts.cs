using FluentAssertions;

namespace vCalWriter.Tests
{
    public class AttendeeFacts
    {
        private static string ToString(Attendee item)
        {
            var writer = new StringWriter();

            item.Write(writer);
            return writer.ToString();
        }

        [Fact]
        public void BasicTest()
        {
            var item = new Attendee
            {
                Email = "name@doma.in",
            };

            var value = ToString(item);

            value.Should()
                .Be("ATTENDEE:name@doma.in\r\n");
        }

        public static IEnumerable<object[]> AttendeeData
        {
            get
            {
                foreach (var item in Enum.GetValues<AttendeeRole>())
                {
                    yield return new object[] { item };
                }
            }
        }

        [Theory]
        [MemberData(nameof(AttendeeData))]
        public void RoleTest(AttendeeRole role)
        {
            var item = new Attendee
            {
                Role = role,
                Email = "name@domain.int",
            };

            var value = ToString(item);

            value.Should()
                .Be($"ATTENDEE;ROLE={role.ToVCalString()}:{item.Email}\r\n");
        }

        public static IEnumerable<object[]> TypeData
        {
            get
            {
                foreach (var item in Enum.GetValues<AttendeeType>())
                {
                    yield return new object[] { item };
                }
            }
        }

        [Theory]
        [MemberData(nameof(TypeData))]
        public void TypeTest(AttendeeType type)
        {
            var item = new Attendee
            {
                Type = type,
                Email = "name@domain.int",
            };

            var value = ToString(item);

            value.Should()
                .Be($"ATTENDEE;CUTYPE={type.ToVCalString()}:{item.Email}\r\n");
        }

        public static IEnumerable<object[]> StatusData
        {
            get
            {
                foreach (var item in Enum.GetValues<AttendeeStatus>())
                {
                    yield return new object[] { item };
                }
            }
        }

        [Theory]
        [MemberData(nameof(StatusData))]
        public void StatusTest(AttendeeStatus status)
        {
            var item = new Attendee
            {
                Status = status,
                Email = "name@domain.int",
            };

            var value = ToString(item);

            value.Should()
                .Be($"ATTENDEE;PARTSTAT={status.ToVCalString()}:{item.Email}\r\n");
        }

        [Fact]
        public void MembersTest()
        {
            var item = new Attendee
            {
                Members = new[]
                {
                    new Uri("mailto:group@domain.int"),
                },
                Email = "name@domain.int"
            };

            var value = ToString(item);

            value.Should()
                .Be("ATTENDEE;MEMBER=\"mailto:group@domain.int\":name@domain.int\r\n");
        }

        [Fact]
        public void RsvpTest()
        {
            var item = new Attendee
            {
                RsvpExpected = true,
                Email = "name@domain.int",
            };

            var value = ToString(item);

            value.Should()
                .Be("ATTENDEE;RSVP=TRUE:name@domain.int\r\n");
        }

        [Fact]
        public void DelegatedToTest()
        {
            var item = new Attendee
            {
                DelegatedTo = new Uri("mailto:del@domain.int"),
                Email = "name@domain.int",
            };

            var value = ToString(item);

            value.Should()
                .Be("ATTENDEE;DELEGATED-TO=\"mailto:del@domain.int\":name@domain.int\r\n");
        }

        [Fact]
        public void DelegatedFromTest()
        {
            var item = new Attendee
            {
                DelegatedFrom = new Uri("mailto:del@domain.int"),
                Email = "name@domain.int",
            };

            var value = ToString(item);

            value.Should()
                .Be("ATTENDEE;DELEGATED-FROM=\"mailto:del@domain.int\":name@domain.int\r\n");
        }

        [Fact]
        public void SentByTest()
        {
            var item = new Attendee
            {
                SentBy = new Uri("mailto:del@domain.int"),
                Email = "name@domain.int",
            };

            var value = ToString(item);

            value.Should()
                .Be("ATTENDEE;SENT-BY=\"mailto:del@domain.int\":name@domain.int\r\n");
        }

        [Fact]
        public void NameTest()
        {
            var item = new Attendee
            {
                Name = "full name",
                Email = "name@domain.int",
            };

            var value = ToString(item);

            value.Should()
                .Be("ATTENDEE;CN=full name:name@domain.int\r\n");
        }
    }
}