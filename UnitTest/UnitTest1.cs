using System;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Config;
using ControlDescriptorClass;
using System.Collections.Generic;

namespace UnitTest
{
    //------------------------------------------------
    [TestClass]
    public class ConfigTest
    {

        /*
        [TestMethod]
        public void GetPropertySubstrTest()
        {
            Assert.AreEqual("name0", ConfigReader.getPropertySubstr("name0")[0]);
            Assert.AreEqual("name0", ConfigReader.getPropertySubstr("name0.name1.name2.name3")[0]);
            Assert.AreEqual("name1", ConfigReader.getPropertySubstr("name0.name1.name2.name3")[1]);
            Assert.AreEqual("name2", ConfigReader.getPropertySubstr("name0.name1.name2.name3")[2]);
            Assert.AreEqual("name3", ConfigReader.getPropertySubstr("name0.name1.name2.name3")[3]);
            Assert.AreEqual(4, ConfigReader.getPropertySubstr("name0.name1.name2.name3").Count);
        }
         * */
string xmlTest1 =
"<?xml version=\"1.0\"?>\r\n" +
"<config>\r\n" +
"  <varlist>\r\n" +
"    <var name=\"FloatVar1\" type=\"float\" value=\"100.0\"/>\r\n" +
"    <var name=\"FloatVar2\" type=\"int32\" value=\"1002\"/>\r\n" +
"  </varlist>\r\n" +

"  <mappinglist>\r\n" +
"	  <mapping varname=\"FloatVar2\" access=\"read\" component=\"label1\" property=\"Text\"/>\r\n" +
"	  <mapping varname=\"FloatVar2\" access=\"readwrite\" component=\"button1\" property=\"Text\"/>\r\n" +
"  </mappinglist>\r\n" +

"  <controllist>\r\n" +
"    <control class=\"Label\">\r\n" +
"      <property name=\"Name\" value=\"label2\"/>\r\n" +
"      <property name=\"Text\" value=\"ololo1\"/>\r\n" +
"      <classproperty name=\"Location\" class=\"Point\">\r\n" +
"        <parameter name=\"X\" value=\"100\"/>\r\n" +
"        <parameter name=\"Y\" value=\"200\"/>\r\n" +
"      </classproperty>\r\n" +
"      <!-- здесь могут находиться вложенные контролы, образующие иерархию-->\r\n" +
"      <!--\r\n" +
"        <control class=\"Label\">\r\n" +
"          <property name=\"Name\" value=\"label2\"/>\r\n" +
"          <property name=\"Text\" value=\"ololo1\"/>\r\n" +
"          <property name=\"Location\" class=\"Point\">\r\n" +
"            <parameter name=\"X\" value=\"100\"/>\r\n" +
"            <parameter name=\"Y\" value=\"200\"/>\r\n" +
"          </property>\r\n" +
"        </control>\r\n" +
"       -->\r\n" +
"    </control>\r\n" +
"    <control class=\"System.Windows.Forms.Label\">\r\n" +
"      <property name=\"Name\" value=\"label3\"/>\r\n" +
"      <property name=\"Text\" value=\"ololo2\"/>\r\n" +
"      <classproperty name=\"Location\" class=\"Point\">\r\n" +
"        <parameter name=\"X\" value=\"100\"/>\r\n" +
"        <parameter name=\"Y\" value=\"250\"/>\r\n" +
"      </classproperty>\r\n" +
"      <!-- здесь могут находиться вложенные контролы, оSбразующие иерархию-->\r\n" +
"    </control>\r\n" +
"  </controllist>\r\n" +
"</config>";

string xmlTest2 =   // control list only
"<?xml version=\"1.0\"?>\r\n" +
"<config>\r\n" +
"  <controllist>\r\n" +
"    <control class=\"Label\">\r\n" +
"      <property name=\"Name\" value=\"label1\"/>\r\n" +
"      <property name=\"Text\" value=\"text1\"/>\r\n" +
"      <classproperty name=\"Location\" class=\"Point\">\r\n" +
"        <parameter name=\"X\" value=\"100\"/>\r\n" +
"        <parameter name=\"Y\" value=\"200\"/>\r\n" +
"      </classproperty>\r\n" +
"    </control>\r\n" +
"    <control class=\"System.Windows.Forms.Label\">\r\n" +
"      <property name=\"Name\" value=\"label3\"/>\r\n" +
"      <property name=\"Text\" value=\"ololo2\"/>\r\n" +
"      <classproperty name=\"Location\" class=\"Point\">\r\n" +
"        <parameter name=\"X\" value=\"100\"/>\r\n" +
"        <parameter name=\"Y\" value=\"250\"/>\r\n" +
"      </classproperty>\r\n" +
"      <!-- здесь могут находиться вложенные контролы, оSбразующие иерархию-->\r\n" +
"    </control>\r\n" +
"  </controllist>\r\n" +
"</config>";

        [TestMethod]
        public void parseXmlDocTest()
        {
            ConfigReader configReader = new ConfigReader();

            configReader.loadXml(xmlTest2);
            List<ControlDescriptor> cdList = configReader.controlDescriptorList;

            Assert.IsTrue(cdList.Count == 2);
            Assert.IsTrue(cdList[0].typeName == "Label");
            Assert.IsTrue(cdList[0].propertyList.Count == 2);
            Assert.IsTrue(cdList[0].propertyList[0].name == "Name");
            Assert.IsTrue(cdList[0].propertyList[0].value == "label1");
            Assert.IsTrue(cdList[0].propertyList[1].name == "Text");
            Assert.IsTrue(cdList[0].propertyList[1].value == "text1");
            Assert.IsTrue(cdList[0].classPropertyList.Count == 1);
            Assert.IsTrue(cdList[0].classPropertyList[0].name == "Location");
            Assert.IsTrue(cdList[0].classPropertyList[0].className == "Point");
            Assert.IsTrue(cdList[0].classPropertyList[0].parameterList.Count == 2);
            Assert.IsTrue(cdList[0].classPropertyList[0].parameterList[0].name == "X");
            Assert.IsTrue(cdList[0].classPropertyList[0].parameterList[0].typeName == "int");
            Assert.IsTrue(cdList[0].classPropertyList[0].parameterList[0].value == "100");
            Assert.IsTrue(cdList[0].classPropertyList[0].parameterList[1].name == "Y");
            Assert.IsTrue(cdList[0].classPropertyList[0].parameterList[1].typeName == "int");
            Assert.IsTrue(cdList[0].classPropertyList[0].parameterList[1].value == "200");
        }
    }
}
