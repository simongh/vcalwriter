using System;
using System.Net.Mime;

namespace iCal
{
    public class Attachment
    {
        public Uri Url { get; set; }

        public ContentType ContentType { get; set; }
    }
}