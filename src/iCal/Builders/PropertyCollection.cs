﻿using iCal.Builders;

namespace vCal.Builders
{
    public class PropertyCollection : System.Collections.ObjectModel.Collection<PropertyBuilder>
    {
        public void Merge(PropertyCollection other)
        {
            foreach (var item in other)
            {
                Add(item);
            }
        }
    }
}