using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace XsltSample.Models
{
    [XmlRoot("Report")]
    public class ReportEnc
    {
        public string Title { get; set; }
        public DateTime CreationDate { get; set; }
        public List<Enc> Encs { get; set; }
    }
}
