using System;

namespace Contract
{
    [System.AttributeUsage(System.AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class MVCFeatureDescriptionAttribute : Attribute
    {
        string description;
        public string Description
        {
            get { return this.description; }
            set { this.description = value; }
        }
    }
}
