using BusinessObjects;
using DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IRateRepository
    {
        void CreateRate(Rate rate);
        public List<RateDTO> GetListRatesByHouseId(int HouseId);
        public RateDTO GetRateById(int rateId);
        public void ReplyComment(int rateId, string reply);
    }
}
