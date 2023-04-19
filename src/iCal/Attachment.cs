using System;
using System.Net.Mime;

namespace vCal
{
    public class Attachment
    {
        public Uri Url { get; set; }

        public ContentType ContentType { get; set; }
    }
}