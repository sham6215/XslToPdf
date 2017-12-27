using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Xsl;

namespace XsltSample.Services
{
    public class XmlToHtmlService
    {
        public XmlDocument Document { get; private set; }
        public string XslSchemaPath { get; private set; }
        /// <summary>
        /// Returns byte[] representation of UTF8 encoded XML
        /// </summary>
        private byte[] XmlData
        {
            get
            {
                using (MemoryStream ms = new MemoryStream())
                using (StreamWriter sr = new StreamWriter(ms, Encoding.UTF8))
                {
                    Document.Save(ms);
                    return ms.ToArray();
                }
            }
        }

        /// <summary>
        /// Converts XML data with XSL schema to an HTML
        /// </summary>
        /// <param name="document">XML Document with data</param>
        /// <param name="schema">XSL convertion schema</param>
        public XmlToHtmlService(XmlDocument document, string schemaPath)
        {
            Document = document;
            XslSchemaPath = schemaPath;
        }

        /// <summary>
        /// Returns byte[] representation of UTF8 encoded HTML
        /// </summary>
        /// <returns></returns>
        public byte[] GetHtml()
        {
            byte[] result = null;
            XslCompiledTransform transform = new XslCompiledTransform();
            transform.Load(XslSchemaPath);

            XmlWriterSettings settings = new XmlWriterSettings()
            {
                ConformanceLevel = ConformanceLevel.Fragment,
                Encoding = Encoding.UTF8
            };
            
            using (MemoryStream msr = new MemoryStream(XmlData))
            using (MemoryStream msw = new MemoryStream())
            using (XmlReader xr = XmlReader.Create(msr))
            using (XmlWriter xw = XmlWriter.Create(msw, settings))
            {
                transform.Transform(xr, xw);
                result = msw.ToArray();
            }

            return result;
        }
    }
}
