using System;
using System.Collections.Generic;

namespace ERental.Entities
{
    public partial class City
    {
        public City()
        {
            Customers = new HashSet<Customer>();
            Vendors = new HashSet<Vendor>();
        }

        public int CityId { get; set; }
        public string? CityName { get; set; }
        public int? StateId { get; set; }

        public virtual State? State { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<Vendor> Vendors { get; set; }
    }
}
