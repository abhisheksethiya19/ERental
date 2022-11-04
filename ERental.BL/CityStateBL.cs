using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERental.DAL;
using ERental.Entities;

namespace ERental.BL
{
    public class CityStateBL
    {
        CityStateDAL objCityStateDAL = new CityStateDAL();
        public IEnumerable<City> GetCity()
        {
            return objCityStateDAL.GetCity();
        }

        public IEnumerable<State> GetState()
        {
            return objCityStateDAL.GetState();
        }
    }
}
