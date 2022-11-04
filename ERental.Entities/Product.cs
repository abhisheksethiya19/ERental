using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ERental.Entities
{
    public partial class Product
    {
        public int ProductId { get; set; }
        [DisplayName("Product Name")]
        [Required]
        public string? ProductName { get; set; }
        [DisplayName("Product Type")]
        [Required]
        public string? ProductType { get; set; }
        [DisplayName("Description")]
        [Required]
        public string? Description { get; set; }
        [DisplayName("Price/Month")]
        [Required]
        public decimal? PricePerMonth { get; set; }
        [DisplayName("Initial Deposit")]
        [Required]
        public decimal? InitialDeposit { get; set; }
        [DisplayName("Free Delivery")]
        [Required]
        public bool FreeDelivery { get; set; }
        [DisplayName("Is Available?")]
        [Required]
        public bool IsAvailable { get; set; }
        [DisplayName("Vendor")]
        [Required]
        public int? VendorId { get; set; }
        [DisplayName("Delivery By")]
        [Required]
        public string? DeliveryBy { get; set; }

        public virtual Vendor? Vendor { get; set; }
    }
}
