using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VarDictionaryClasses;
using Config;
using TestClasses;
using ControlDescriptorClass;
using System.Reflection;

namespace ComponentAdapterTest
{
    public partial class Form1 : Form
    {
        public VarDictionary varDict = new VarDictionary();
        public List<Mapping> mappingList = new List<Mapping>();
        public List<Control> controlList = new List<Control>();
        public Mapper mapper = new Mapper();
        public DbMock dbMock = new DbMock();
        public List<ControlDescriptor> controlDescriptorList = new List<ControlDescriptor>();
        public string defaultNameSpace;

        public Form1()
        {
            InitializeComponent();
        }

        private void initDictionary()
        {
            ConfigReader config = new ConfigReader();
            config.varDict = varDict;
            config.mappingList = mappingList;
            config.controlDescriptorList = controlDescriptorList;
            config.loadFromFile("C:/Users/V/Desktop/ComponentAdapterTest/ComponentAdapterTest/config.xml");
            defaultNameSpace = config.defaultNameSpace;
            dbMock.dict = varDict;
            dbMock.mapper = mapper;
        }

        public Type getAssemblyNameContainingType(string typeName)
        {
            foreach (Assembly currentAssembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type t = currentAssembly.GetType(typeName, false, true);

                if (t != null)
                    return t;
            }
            return null;
        }

        private void buildControls()
        {
            foreach (ControlDescriptor cd in controlDescriptorList)
            {
                //create a control 

                string nameSpace = "";
                if(!cd.typeName.Contains('.'))
                    nameSpace = defaultNameSpace + ".";
                Type t = getAssemblyNameContainingType(nameSpace + cd.typeName);

                Control control = (Control)Activator.CreateInstance(t);

                Object obj = control;
                foreach (PropDescriptor property in cd.propertyList)
                { 
                    string[] propertyName = property.name.Split('.');

                    PropertyInfo prop;
                    for (int i = 0; i < propertyName.Length - 1; i++)
                    {
                        prop = obj.GetType().GetProperty(propertyName[i]);
                        obj = prop.GetValue(obj, null);
                    }
                    prop = obj.GetType().GetProperty(propertyName.Last());
                    prop.SetValue(obj, Convert.ChangeType(property.value, prop.PropertyType), null);
                }

                foreach (ClassPropertyDescriptor cpd in cd.classPropertyList)
                {
                    Type objType = getAssemblyNameContainingType(cpd.className);

                    List<Object> args = new List<Object>();
                    foreach (ParameterDescriptor pd in cpd.parameterList)
                    {
                       // pd.name
                        Object value;
                        switch (pd.typeName)
                        {
                            case "int": value = Convert.ToInt32(pd.value);
                                break;
                            case "string": value = pd.value;
                                break;
                            default: value = Convert.ToInt32(pd.value);  //int type by default
                                break;
                        }
                        args.Add(value); 
                    }

                    Object propObj = Activator.CreateInstance(objType, args.ToArray());

                    PropertyInfo prop;
                    prop = obj.GetType().GetProperty(cpd.name);
                    prop.SetValue(obj, Convert.ChangeType(propObj, prop.PropertyType), null);
                }
                
                Controls.Add(control);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            initDictionary();

            buildControls();

            foreach (KeyValuePair<string, Var> psv in varDict)
            {
                textBox1.AppendText("Key: " + psv.Key + " , Value: name = " + psv.Value.name + " type = " + psv.Value.varType + " value = " + psv.Value.value + "\r\n");
            }

            mapper.varDict = varDict;
            mapper.mappingList = mappingList;
            mapper.controlList = controlList;

            mapper.controlList.Add(label1);
            mapper.controlList.Add(button1);

            mapper.applyMapping();
        }
    }
}
