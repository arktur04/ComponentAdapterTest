using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDescriptorClass
{
    public class PropDescriptor
    {
        public string name { set; get; }
        public string value { set; get; }
    }

    public class ParameterDescriptor
    {
        public string name { set; get; }
        public string typeName { set; get; }
        public string value { set; get; }
    }

    public class ClassPropertyDescriptor
    {
        public string name { set; get; }
        public string className { set; get; }
        // ctor parameter list
        public List< ParameterDescriptor > parameterList { set; get; } //parameter name <-> parameter value pair
        //properties of an object
        public List< PropDescriptor > propertyList { set; get; } //property name <-> property value pair
        //class properties of an object
        public List< ClassPropertyDescriptor > classPropertyList { set; get; } //property name <-> property value pair
    }
    
    public class ControlDescriptor
    {
        public string typeName { set; get; }
        public List< PropDescriptor > propertyList {set; get;} //property name <-> property value pair
        public List<ClassPropertyDescriptor> classPropertyList { set; get; } //property name <-> property value pair
        public List< ControlDescriptor > childControlList { set; get; }
    }
}
