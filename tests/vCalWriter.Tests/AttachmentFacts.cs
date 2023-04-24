using FluentAssertions;

namespace vCalWriter.Tests
{
    public class AttachmentFacts
    {
        private static string ToString(Attachment item)
        {
            var writer = new StringWriter();

            item.Write(writer);
            return writer.ToString();
        }

        [Fact]
        public void UrlTest()
        {
            var item = new Attachment()
            {
                Url = new("http://some.where"),
            };

            var value = ToString(item);

            value.Should()
                .Be($"ATTACH:{item.Url}\r\n");
        }

        [Fact]
        public void BinaryTest()
        {
            var item = new Attachment()
            {
                Data = new byte[] { 0x0, 0x0 },
            };

            var value = ToString(item);

            value.Should()
               .Be("ATTACH;ENCODING=BASE64;VALUE=BINARY:AAA=\r\n");
        }

        [Fact]
        public void BinaryWithContentTest()
        {
            var item = new Attachment()
            {
                Data = new byte[] { 0x0, 0x0 },
                ContentType = new("text/html")
            };

            var value = ToString(item);

            value.Should()
               .Be("ATTACH;ENCODING=BASE64;VALUE=BINARY;FMTTYPE=text/html:AAA=\r\n");
        }
    }
}