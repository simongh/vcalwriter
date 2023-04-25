namespace vCalWriter.Builders
{
    public class ParameterCollection : System.Collections.ObjectModel.Collection<ParameterBuilder>
    {
        public void Merge(ParameterCollection other)
        {
            foreach (var item in other)
            {
                Add(item);
            }
        }

        public void Write(TextWriter writer)
        {
            var isFirst = true;
            foreach (var param in this)
            {
                if (!isFirst)
                    writer.Write(";");
                isFirst = false;

                param.Write(writer);
            }
        }
    }
}