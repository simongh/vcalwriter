using FluentAssertions;

namespace vCalWriter.Tests
{
    public class OrganiserFacts
    {
        private static string ToString(Organiser item)
        {
            var writer = new StringWriter();

            item.Write(writer);
            return writer.ToString();
        }

        [Fact]
        public void EmailTest()
        {
            var item = new Organiser
            {
                Email = "name@domain.int"
            };

            var value = ToString(item);

            value.Should()
                .Be("ORGANIZER:name@domain.int\r\n");
        }

        [Fact]
        public void NameTest()
        {
            var item = new Organiser
            {
                Email = "name@domain.int",
                Name = "full name",
            };

            var value = ToString(item);

            value.Should()
                .Be("ORGANIZER;CN=full name:name@domain.int\r\n");
        }

        [Fact]
        public void SentByTest()
        {
            var item = new Organiser
            {
                Email = "name@domain.int",
                SentBy = new Uri("mailto:sender@domain.int"),
            };

            var value = ToString(item);

            value.Should()
                .Be("ORGANIZER;SENT-BY=\"mailto:sender@domain.int\":name@domain.int\r\n");
        }
    }
}