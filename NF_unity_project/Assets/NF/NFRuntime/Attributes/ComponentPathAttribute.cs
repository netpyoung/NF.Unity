using System;

namespace NFRuntime.Attributes
{
    public class ComponentPathAttribute : Attribute
    {
        public readonly string Path;

        public ComponentPathAttribute(string path)
        {
            this.Path = path;
        }
    }
}
