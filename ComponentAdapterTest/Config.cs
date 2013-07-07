using System.Xml;
using System.IO;
using System.Text;
using System;
using System.IO.Ports;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using VarDictionaryClasses;
using ControlDescriptorClass;

namespace Config
{
    public class ConfigReader
    {
        //variables list
        public VarDictionary varDict {get; set;}
        public List<Mapping> mappingList { get; set;}
        public List<ControlDescriptor> controlDescriptorList {get; set;}
        public string defaultNameSpace { get; set; }
        //methods

        public ConfigReader()
        {
            varDict = new VarDictionary();
            mappingList = new List<Mapping>();
            controlDescriptorList = new List<ControlDescriptor>();
        }

        public void loadFromFile(string fileName)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(fileName);
                parseXmlDoc(doc);
            }
            catch
            {
                throw new FileLoadException();
            }
        }

        public void loadXml(string xmlString)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                parseXmlDoc(doc);
            }
            catch
            {
                throw new XmlException();
            }
        }

        private void parseXmlDoc(XmlDocument doc)
        {
            try
            {
                XmlNodeList varNodeList = doc.SelectNodes("config/varlist/var");
                parseVarList(varNodeList);
                XmlNodeList mappingNodeList = doc.SelectNodes("config/mappinglist/mapping");
                parseMappingList(mappingNodeList);

                XmlNode defaultNameSpaceNode = doc.SelectSingleNode("config/defaultnamespace");
                defaultNameSpace = defaultNameSpaceNode.Attributes["name"].Value;
                XmlNodeList controlNodeList = doc.SelectNodes("config/controllist/control");
                parseXmlDocControlList(controlNodeList);
            }
            catch { }
        }

        private void parseVarList(XmlNodeList varNodeList)
        {
            //----------------------------------
            // var list
            //----------------------------------
            foreach (XmlNode n in varNodeList)
            {
                Var v = new Var();
                try
                {
                    v.name = n.Attributes["name"].Value;
                }
                catch { }

                try
                {
                    v.varType = n.Attributes["type"].Value;
                }
                catch { }

                try
                {
                    v.value = n.Attributes["value"].Value;
                }
                catch { }

                if (varDict != null)
                {
                    varDict.addVar(v);
                }
            }
        }

        private void parseMappingList(XmlNodeList mappingNodeList)
        {
            foreach (XmlNode n in mappingNodeList)
            {
                Mapping m = new Mapping();
                try
                {
                    m.varName = n.Attributes["varname"].Value;
                }
                catch { }

                try
                {
                    switch (n.Attributes["access"].Value)
                    {
                        case "read": m.accessType = AccessType.Read;
                            break;
                        case "write": m.accessType = AccessType.Write;
                            break;
                        case "readwrite": m.accessType = AccessType.ReadWrite;
                            break;
                        default: m.accessType = AccessType.Read;
                            break;
                    }
                }
                catch { }

                try
                {
                    m.componentName = n.Attributes["component"].Value;
                }
                catch { }

                try
                {
                    m.propertyName = n.Attributes["property"].Value;
                }
                catch { }

                if (mappingList != null)
                {
                    mappingList.Add(m);
                }
            }
        }

        private void parseXmlDocControlList(XmlNodeList controlNodeList) 
        {
            //----------------------------------
            // control list
            //----------------------------------
            foreach (XmlNode controlNode in controlNodeList)
            {
                ControlDescriptor cd = readControlNode(controlNode);
                controlDescriptorList.Add(cd);
            }
        }

        ControlDescriptor readControlNode(XmlNode controlNode)
        {
            ControlDescriptor cd = new ControlDescriptor();
            List< PropDescriptor > propertyList = new List<PropDescriptor>();
            cd.propertyList = propertyList;
            List< ControlDescriptor > childControlList = new List<ControlDescriptor>();
            cd.childControlList = childControlList;
            List< ClassPropertyDescriptor > classPropertyList = new List<ClassPropertyDescriptor>();
            cd.classPropertyList = classPropertyList;
            try
            {
                cd.typeName = controlNode.Attributes["class"].Value;
                XmlNodeList propertyNodeList = controlNode.SelectNodes("property");
                foreach(XmlNode propertyNode in propertyNodeList)
                {
                    PropDescriptor pd = readPropertyNode(propertyNode);
                    propertyList.Add(pd);
                }
                XmlNodeList classPropertyNodeList = controlNode.SelectNodes("classproperty");
                foreach(XmlNode classPropertyNode in classPropertyNodeList)
                {
                    ClassPropertyDescriptor cpd = readClassPropertyNode(classPropertyNode);
                    classPropertyList.Add(cpd);
                }
                XmlNodeList childControlNodeList = controlNode.SelectNodes("control");
                foreach(XmlNode childControlNode in childControlNodeList)
                {
                    ControlDescriptor childControlDescriptor = readControlNode(childControlNode);
                    cd.childControlList.Add(childControlDescriptor);
                }
            }
            catch { }
            return cd;
        }

        PropDescriptor readPropertyNode(XmlNode propertyNode)
        {
            PropDescriptor pd = new PropDescriptor();
            pd.name = propertyNode.Attributes["name"].Value;
            pd.value = propertyNode.Attributes["value"].Value;
            return pd;
        }

        ClassPropertyDescriptor readClassPropertyNode(XmlNode propertyNode)
        {
            ClassPropertyDescriptor cpd = new ClassPropertyDescriptor();
            List<ParameterDescriptor> parameterList = new List<ParameterDescriptor>();
            cpd.parameterList = parameterList;
            List<PropDescriptor> propertyList = new List<PropDescriptor>();
            cpd.propertyList = propertyList;
            List< ClassPropertyDescriptor > classPropertyList = new List<ClassPropertyDescriptor>();
            cpd.classPropertyList = classPropertyList;

            cpd.name = propertyNode.Attributes["name"].Value;
            cpd.className = propertyNode.Attributes["class"].Value;
            XmlNodeList parameterNodeList = propertyNode.SelectNodes("parameter");
            foreach(XmlNode parameterNode in parameterNodeList)
            {
                ParameterDescriptor pd = new ParameterDescriptor();
                pd.name = parameterNode.Attributes["name"].Value;
                pd.value = parameterNode.Attributes["value"].Value;
                pd.typeName = parameterNode.Attributes["typename"].Value;
                parameterList.Add(pd);
            }
            XmlNodeList propertyNodeList = propertyNode.SelectNodes("parameter");
            foreach(XmlNode pn in propertyNodeList)
            {
                PropDescriptor property = readPropertyNode(pn);
                propertyList.Add(property);
            }
            XmlNodeList classPropertyNodeList = propertyNode.SelectNodes("class");
            foreach(XmlNode cpn in classPropertyNodeList)
            {
                ClassPropertyDescriptor classProperty = readClassPropertyNode(cpn);
                classPropertyList.Add(classProperty);
            }
            return cpd;
        }
    }
}
