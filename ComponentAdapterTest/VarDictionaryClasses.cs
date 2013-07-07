using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VarDictionaryClasses
{
    //м.б. не нужно
//    public enum VarType { Float, UInt16, UInt32, Int16, Int32 }

    public class Var
    {
        public Var(){}

        public Var(string name, string varType, string value)
        {
            this.name = name;
            this.varType = varType;
            this.value = value;
        }

        public string name { get; set; }
        public string varType { get; set; }
        public string value { get; set; }
    }

    // класс VarDictionary предназначен для того, чтобы хранить переменные и находить их по имени
    public class VarDictionary: Dictionary<string, Var>
    {
        public Var addVar(string name, string varType, string value)
        {
            Var var = new Var(name, varType, value);
            Add(name, var);
            return var;
        }

        public Var addVar(Var var)
        {
            if (var != null)
            {
                Add(var.name, var);
            }
            return var;
        }
    }

    public enum AccessType {Read, Write, ReadWrite};

    // класс Mapping задает связь между именем переменной и парой (имя компонента, имя свойства)
    public class Mapping
    {
        public string varName { get; set; }
        public AccessType accessType { get; set; }
        public string componentName { get; set; }
        public string propertyName { get; set; }
    }

    public class Mapper
    {
        public List< Mapping > mappingList{get; set;}
        public VarDictionary varDict { get; set; }
        public List<Control> controlList { get; set; }

        public Control getControl(string name)
        {
            if(controlList != null)
                foreach(Control c in controlList)
                {
                    if(c.Name == name)
                    {
                        return c;
                    }
                }
            return null;
        }

        public void applyMapping()
        {
            if ((mappingList != null) && (varDict != null))
            {
                foreach (Mapping m in mappingList)
                {
                    Control c = getControl(m.componentName);
                    var prop = c.GetType().GetProperty(m.propertyName);
                    Var v = varDict[m.varName];
                    if (m.accessType == AccessType.Read || m.accessType == AccessType.ReadWrite)
                    {
                        if (!c.InvokeRequired)
                        {
                            prop.SetValue(c, v.value, null);
                        }
                        else
                        {
                            c.Invoke(new Action<object, object, object[]>((obj, value, index) => prop.SetValue(obj, value, index)), c, v.value, null);
                        }
                    }
                }
            }
        }
    }
}
