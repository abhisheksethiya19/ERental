using System;
using System.Collections.Generic;

namespace ERental.Entities
{
    public partial class State
    {
        public State()
        {
            Cities = new HashSet<City>();
            Customers = new HashSet<Customer>();
            Vendors = new HashSet<Vendor>();
        }

        public int StateId { get; set; }
        public string? StateName { get; set; }

        public virtual ICollection<City> Cities { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<Vendor> Vendors { get; set; }
    }
}
