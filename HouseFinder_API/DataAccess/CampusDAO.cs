using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CampusDAO
    {
        public static List<Campus> GetAllCampuses()
        {
            var listCampus = new List<Campus>();
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    listCampus = context.Campuses.ToList();
                }
            } catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return listCampus;
        }
    }
}
