using System;
using System.Collections.Generic;
using System.IO;

namespace vCal.Builders
{
    public enum DatePart
    {
        DateTime,
        DateOnly,
        TimeOnly,
    }

    public class ParameterBuilder
    {
        private string? _value;

        public string? Name { get; set; }

        public ParameterBuilder Add(byte[] data)
        {
            _value = Convert.ToBase64String(data);

            return this;
        }

        public ParameterBuilder Add(bool data)
        {
            _value = data.ToString().ToUpper();

            return this;
        }

        public ParameterBuilder Add(Uri data)
        {
            _value = $"\"{data}\"";

            return this;
        }

        private string FormatHelper(DatePart part)
        {
            var format = "";
            if (part != DatePart.TimeOnly)
                format = "yyyyMMdd";

            if (part == DatePart.DateTime)
                format += "T";

            if (part != DatePart.DateOnly)
                format += "HHmmss";

            return format;
        }

        public ParameterBuilder Add(DateTime data, DatePart datePart = DatePart.DateTime, DateTimeKind timeKind = DateTimeKind.Local)
        {
            var format = FormatHelper(datePart);
            _value = data.ToString(format);

            if (timeKind == DateTimeKind.Utc)
                _value += "Z";

            return this;
        }

        public ParameterBuilder Add(DateTimeOffset data, DatePart datePart = DatePart.DateTime, DateTimeKind timeKind = DateTimeKind.Local)
        {
            var format = FormatHelper(datePart);
            _value = data.ToString(format);

            if (timeKind == DateTimeKind.Utc)
                _value += "Z";

            return this;
        }

        public ParameterBuilder Add(TimeSpan data)
        {
            _value = data.ToDuration();

            return this;
        }

        public ParameterBuilder Add<T>(T data) where T : struct
        {
            _value = data.ToString();

            return this;
        }

        public ParameterBuilder Add(string? value)
        {
            _value = value;

            return this;
        }

        public ParameterBuilder Add<T>(IEnumerable<T> values, string format = "{0}")
        {
            using var writer = new StringWriter();

            var first = true;
            foreach (var value in values)
            {
                if (!first)
                    writer.Write(',');

                writer.Write(format, value);
                first = false;
            }

            _value = writer.ToString();

            return this;
        }

        public void Clear()
        {
            _value = null;
        }

        public void Write(StringWriter writer)
        {
            if (!string.IsNullOrEmpty(Name))
            {
                writer.Write(Name);
                writer.Write('=');
            }

            if (_value == null)
                return;

            writer.Write(_value);
        }

        public override string ToString()
        {
            using var writer = new StringWriter();
            Write(writer);
            return writer.ToString();
        }
    }
}