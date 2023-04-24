using vCalWriter.Builders;

namespace vCalWriter
{
    public enum AttendeeRole
    {
        Chair,
        Required,
        Optional,
        None,
    }

    public enum AttendeeStatus
    {
        NeedsAction,
        Accepted,
        Declined,
        Tentative,
        Delegated,
        Completed,
        InProcess,
    }

    public enum AttendeeType
    {
        Individual,
        Group,
        Resource,
        Room,
        Unknown
    }

    public class Attendee
    {
        public AttendeeType? Type { get; set; }

        public ICollection<Uri> Members { get; set; } = new List<Uri>();
        public AttendeeRole? Role { get; set; }
        public AttendeeStatus? Status { get; set; }

        public bool RsvpExpected { get; set; }
        public Uri? DelegatedTo { get; set; }
        public Uri? DelegatedFrom { get; set; }

        public Uri? SentBy { get; set; }

        public string? Name { get; set; }
        public string? Email { get; set; }

        public ParameterCollection Parameters { get; set; } = new();

        public void Write(TextWriter writer)
        {
            var builder = new Builders.PropertyBuilder();

            if (Type.HasValue)
            {
                builder.Parameters.Add(new Builders.ParameterBuilder
                {
                    Name = Builders.ParameterNames.CalendarUserType
                }.Add(Type.Value.ToVCalString()));
            }

            if (Members?.Any() == true)
            {
                builder.Parameters.Add(new Builders.ParameterBuilder
                {
                    Name = Builders.ParameterNames.Member,
                }.Add(Members, "\"{0}\""));
            }

            if (Role.HasValue)
            {
                builder.Parameters.Add(new Builders.ParameterBuilder
                {
                    Name = Builders.ParameterNames.Role,
                }.Add(Role.Value.ToVCalString()));
            }

            if (Status.HasValue)
            {
                builder.Parameters.Add(new Builders.ParameterBuilder
                {
                    Name = Builders.ParameterNames.ParticipationStatus,
                }.Add(Status.Value.ToVCalString()));
            }

            if (RsvpExpected)
            {
                builder.Parameters.Add(new Builders.ParameterBuilder
                {
                    Name = Builders.ParameterNames.RsvpExpectation,
                }.Add(true));
            }

            if (DelegatedTo != null)
            {
                builder.Parameters.Add(new Builders.ParameterBuilder
                {
                    Name = Builders.ParameterNames.DelegatedTo,
                }.Add(DelegatedTo));
            }

            if (DelegatedFrom != null)
            {
                builder.Parameters.Add(new Builders.ParameterBuilder
                {
                    Name = Builders.ParameterNames.DelegatedFrom,
                }.Add(DelegatedFrom));
            }

            if (SentBy != null)
            {
                builder.Parameters.Add(new Builders.ParameterBuilder
                {
                    Name = Builders.ParameterNames.SentBy,
                }.Add(SentBy));
            }

            if (!string.IsNullOrEmpty(Name))
            {
                builder.Parameters.Add(new Builders.ParameterBuilder
                {
                    Name = Builders.ParameterNames.CommonName,
                }.Add(Name));
            }

            if (Parameters != null)
            {
                builder.Parameters.Merge(Parameters);
            }

            builder.Value.Add(Email);
            builder.Write(Builders.PropertyNames.Attendee, writer);
        }
    }
}