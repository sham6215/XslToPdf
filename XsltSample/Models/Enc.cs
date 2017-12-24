using System;
using System.Xml.Serialization;

namespace XsltSample.Models
{
    public class Enc
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [XmlIgnore]
        public DateTime PublicationDate { get; set; }
        [XmlElement("PublicationDate")]
        public string PublicationDateReport
        {
            get
            {
                return PublicationDate.ToString("yyyy-MM-dd");
            }
            set
            {
                PublicationDate = DateTime.Parse(value);
            }
        }
        [XmlIgnore]
        public DateTime ExpiryDate { get; set; }
        [XmlElement("ExpiryDate")]
        public string ExpiryDateReport
        {
            get
            {
                return ExpiryDate.ToString("yyyy-MM-dd");
            }
            set
            {
                ExpiryDate = DateTime.Parse(value);
            }
        }
        
        public int Price { get; set; }
    }
}