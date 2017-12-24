using System;

namespace XsltSample.Models
{
    public class Enc
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime PublicationDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int Price { get; set; }
    }
}