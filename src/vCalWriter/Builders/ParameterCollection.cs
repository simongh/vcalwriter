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
    }
}