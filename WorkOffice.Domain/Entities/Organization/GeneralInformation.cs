using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WorkOffice.Domain.Entities
{
    public class GeneralInformation : Entity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid GeneralInformationId { get; set; }
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
        public byte[] ImgLogo { get; set; }
        public string Imgtype { get; set; }
        public bool Ismulticompany { get; set; }
        public int Subsidiary_level { get; set; }
    }
}
