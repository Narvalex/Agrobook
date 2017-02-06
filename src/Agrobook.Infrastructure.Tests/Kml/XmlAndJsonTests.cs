using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Xml;

namespace Agrobook.Kml.Tests
{
    [TestClass]
    public class XmlAndJsonTests : GivenAKmlStringInMemory
    {
        [TestMethod]
        public void CanConvertKmlFromXmlFormatToJsonFormatBackAndForth()
        {
            var originalDoc = new XmlDocument();
            originalDoc.LoadXml(base.kmlSample);

            // Converting from Xml to Json
            var json = JsonConvert.SerializeXmlNode(originalDoc);

            Assert.IsFalse(string.IsNullOrWhiteSpace(json));

            // Converting from Json to Xml
            var convertedDoc = JsonConvert.DeserializeXmlNode(json);
            var convertedJson = JsonConvert.SerializeXmlNode(convertedDoc);

            Assert.AreEqual(json, convertedJson);
        }
    }
}
