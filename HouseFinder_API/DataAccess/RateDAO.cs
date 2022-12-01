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

        /**
         * [House Detail] Guest view Rate & Comment
         */
        public static List<RateDTO> GetListRatesByHouseId(int HouseId)
        {
            List<RateDTO> rateDTOs;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //include address
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    //Get by LandlordId
                    rateDTOs = context.Rates.Where(r => r.Deleted == false).ProjectTo<RateDTO>(config).Where(r => r.HouseId == HouseId).ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return rateDTOs;
        }

        /**
         * Get rate detail
         */
        public static RateDTO GetRateById(int rateId)
        {
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    RateDTO rate = context.Rates.Where(r => r.Deleted == false)
                        .Where(r => r.RateId == rateId && r.Deleted == false)
                        .ProjectTo<RateDTO>(config).FirstOrDefault();
                    if (rate == null)
                    {
                        throw new Exception();
                    }
                    return rate;
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /**
         * [Rate House] Landlord replies the comment
         */
        public static void ReplyComment(int rateId, string reply)
        {
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //Find rooms of this house
                    Rate replyRate = context.Rates.FirstOrDefault(r => r.RateId == rateId);
                    if (replyRate == null)
                    {
                        throw new Exception();
                    }

                    //Update
                    replyRate.LandlordReply = reply;
                    context.Rates.Update(replyRate);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
