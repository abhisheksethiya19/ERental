using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ERental.Entities
{
    public partial class Vendor
    {
        public Vendor()
        {
            Products = new HashSet<Product>();
        }

        public int VendorId { get; set; }
        [DisplayName("User Name")]
        [Required]
        public string? UserName { get; set; }
        [DisplayName("First Name")]
        [Required]

        public string? FirstName { get; set; }
        [DisplayName("Last Name")]
        [Required]
        public string? LastName { get; set; }
        [DisplayName("Address")]
        [Required]

        public string? Address { get; set; }
        [DisplayName("State")]
        [Required]
        public int? StateId { get; set; }
        [DisplayName("City")]
        [Required]
        public int? CityId { get; set; }
        [DisplayName("Pin Code")]
        [Required]
        public int? PinCode { get; set; }
        [DisplayName("Phone Number")]
        [Required]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone Number should be 10 numbers")]
        public string? PhoneNo { get; set; }
        [DisplayName("Email id")]
        [Required]
        [EmailAddress]
        public string? EmailId { get; set; }

        public virtual City? City { get; set; }
        public virtual State? State { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
