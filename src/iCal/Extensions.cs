using System;

namespace iCal
{
    internal static class Extensions
    {
        public static string ToVCalString(this DateTimeOffset value)
            => value.ToUniversalTime().ToString("yyyyMMddTHHmmssZ");

        public static string ToVCalString(this EventStatus value)
            => value.ToString().ToUpper();

        public static string ToVCalString(this Classification value)
            => value.ToString().ToUpper();

        public static string ToVCalString(this AttendeeRole value)
            => value switch
            {
                AttendeeRole.Required => "REQ-PARTICIPANT",
                AttendeeRole.Optional => "OPT-PARTICIPANT",
                AttendeeRole.Chair => "CHAIR",
                AttendeeRole.None => "NON-PARTICIPANT",
                _ => throw new ApplicationException("Unexpected role"),
            };

        public static string ToVCalString(this AttendeeStatus value)
            => value switch
            {
                AttendeeStatus.NeedsAction => "NEEDS-ACTION",
                AttendeeStatus.InProcess => "IN-PROCESS",
                _ => value.ToString().ToUpper(),
            };

        public static string ToVCalString(this AttendeeType value)
            => value.ToString().ToUpper();

        public static string ToDuration(this TimeSpan value)
        {
            var result = "";

            if (value.TotalMilliseconds < 0)
                result += "-";

            if (value.Days != 0)
                result += value.Days.ToString("dD");

            return result + value.ToString("hhHmmMssS");
        }
    }
}