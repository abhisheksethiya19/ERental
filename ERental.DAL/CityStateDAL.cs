using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERental.Entities;
using ERental.EFCore;

namespace ERental.DAL
{
    public class CityStateDAL
    {
        ERentalContext _context = new ERentalContext();
        public IEnumerable<City> GetCity()
        {
            return _context.Cities;
        }

        public IEnumerable<State> GetState()
        {
            return _context.States;
        }
    }
}
