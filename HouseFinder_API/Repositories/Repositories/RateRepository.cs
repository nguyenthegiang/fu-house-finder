using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using DataAccess;
using DataAccess.DTO;

namespace Repositories
{
    public class RateRepository : IRateRepository
    {
        public void CreateRate(Rate rate) => RateDAO.CreateRate(rate);
        public List<RateDTO> GetListRatesByHouseId(int HouseId) => RateDAO.GetListRatesByHouseId(HouseId);
        public RateDTO GetRateById(int rateId) => RateDAO.GetRateById(rateId);
        public void ReplyComment(int rateId, string reply) => RateDAO.ReplyComment(rateId, reply);
    }
}
