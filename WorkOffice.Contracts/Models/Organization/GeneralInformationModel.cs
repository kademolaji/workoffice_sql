using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace WorkOffice.Contracts.Models
{
   public class GeneralInformationModel : BaseModel
    {
        public long GeneralInformationId { get; set; }
        public string Organisationname { get; set; }
        public string Taxid { get; set; }
        public string Regno { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Note { get; set; }
        public string Zipcode { get; set; }
        public string Currency { get; set; }
        [JsonIgnore]
        public byte[] ImgLogo { get; set; }
        [JsonIgnore]
        public string Imgtype { get; set; }
        public bool Ismulticompany { get; set; }
        public int Subsidiary_level { get; set; }
    }
}
