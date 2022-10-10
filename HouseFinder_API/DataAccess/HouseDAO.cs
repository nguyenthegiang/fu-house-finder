using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class HouseDAO
    {
        public static List<House> GetAllHouses()
        {
            var listHouses = new List<House>();
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    listHouses = context.Houses.ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return listHouses;
        }
    }
}
