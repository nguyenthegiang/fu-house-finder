using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessObjects;
using DataAccess.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class RateDAO
    {
        /**
         * [House Detail] Guest create new Rate & Comment
         */
        public static void CreateRate(Rate rate)
        {
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    context.Rates.Add(rate);
                    context.SaveChanges();
                }
            } catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
