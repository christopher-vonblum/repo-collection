using System;

namespace CoreUi.Attributes
{
    public class DescriptionAttribute : CoreUiAttribute
    {
        public string Description { get; }

        public DescriptionAttribute(string description)
        {
            Description = description;
        }
    }
}