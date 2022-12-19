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
    public class HouseDAO
    {
        /**
         * [Staff - List Houses]
         * Get all undeleted houses
         */
        public static List<HouseDTO> GetAllHouses()
        {
            List<HouseDTO> houseDTOs;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //include address
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    //Get by LandlordId
                    houseDTOs = context.Houses.Where(h => h.Deleted == false).ProjectTo<HouseDTO>(config).ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return houseDTOs;
        }

        /**
         Add a new House into the Database
         */
        public static HouseDTO CreateHouse(House house)
        {
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    context.Houses.Add(house);
                    context.SaveChanges();
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    var mapper = config.CreateMapper();

                    return context.Houses.Where(h => h.HouseId == house.HouseId).ProjectTo<HouseDTO>(config).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void UpdateHouseByHouseId(House house)
        {
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //Find house
                    House updatedHouse = context.Houses.FirstOrDefault(h => h.HouseId == house.HouseId);
                    if (updatedHouse == null)
                    {
                        throw new Exception();
                    }

                    //Update
                    context.Entry<House>(updatedHouse).State = EntityState.Detached;
                    context.Houses.Update(house);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void DeleteHouseByHouseId(int houseId)
        {
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    House updatedHouse = context.Houses.FirstOrDefault(h => h.HouseId == houseId);
                    if (updatedHouse == null)
                    {
                        throw new Exception();
                    }

                    //Delete by changing Status to Disabled
                    updatedHouse.Deleted = true;
                    List<Room> rooms = context.Rooms.Where(r => r.HouseId == houseId).ToList();
                    foreach (Room o in rooms)
                    {
                        o.Deleted = true;
                        context.Rooms.Update(o);
                    }
                    List<Rate> rates = context.Rates.Where(r => r.HouseId == houseId).ToList();
                    foreach (Rate r in rates)
                    {
                        r.Deleted = true;
                        context.Rates.Update(r);
                    }
                    List<ImagesOfHouse> imagesOfHouses = context.ImagesOfHouses.Where(i => i.HouseId == houseId).ToList();
                    foreach (ImagesOfHouse i in imagesOfHouses)
                    {
                        i.Deleted = true;
                        context.ImagesOfHouses.Update(i);
                    }
                    List<Report> reports = context.Reports.Where(r => r.HouseId == houseId).ToList();
                    foreach(Report r in reports)
                    {
                        r.Deleted = true;
                        context.Reports.Update(r);
                    }
                    context.Entry<House>(updatedHouse).State = EntityState.Detached;
                    context.Houses.Update(updatedHouse);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /**
         * [Home Page] Get list of available houses, with Address, Images & Rooms
            Availabe house: house with at least 1 available room;

            Return list of all available Houses in the system (Houses that have available Rooms) 
            with additional information to support filtering
        */
        public static List<AvailableHouseDTO> GetAvailableHouses()
        {
            List<AvailableHouseDTO> houseDTOs;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //include address, images
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    houseDTOs = context.Houses
                        //not selecting deleted house
                        .Where(house => house.Deleted == false)
                        //not getting houses of Inactive Landlord
                        .Where(house => house.Landlord.Status.StatusName.Equals("Active"))
                        //include this for finding DistrictId
                        .Include(house => house.Village.Commune)
                        .ProjectTo<AvailableHouseDTO>(config).ToList();

                    //find lowest room price & highest room price
                    for (int i = 0; i < houseDTOs.Count; i++)
                    {
                        houseDTOs[i] = RoomDAO.GetRoomPriceByOfHouse(houseDTOs[i]);
                    }
                }

                /*for each house: check to see if it has available room
                if not, remove from list*/
                List<AvailableHouseDTO> availableHouses = new List<AvailableHouseDTO>();
                foreach (AvailableHouseDTO houseDTO in houseDTOs)
                {
                    if (RoomDAO.CountAvailableRoomByHouseId(houseDTO.HouseId) > 0)
                    {
                        availableHouses.Add(houseDTO);
                    }
                }
                houseDTOs = availableHouses;

                //Add data for DTO
                foreach (AvailableHouseDTO houseDTO in houseDTOs)
                {
                    //(RoomTypeIds)
                    /*Get list (as a string) of ID of RoomTypes of all Rooms of each House
                    -> For Filtering by RoomType*/
                    houseDTO.RoomTypeIds = "";
                    foreach (RoomDTO roomDTO in houseDTO.Rooms)
                    {
                        if (!houseDTO.RoomTypeIds.Contains(roomDTO.RoomTypeId.ToString()))
                        {
                            houseDTO.RoomTypeIds += roomDTO.RoomTypeId.ToString();
                        }
                    }

                    //(Commune, District)
                    /*Get CommuneId & DistrictId of the Village of this House
                    -> For Filtering by Region*/
                    houseDTO.CommuneId = houseDTO.Village.CommuneId;
                    houseDTO.DistrictId = houseDTO.Village.Commune.DistrictId;

                    //(RoomUtility)
                    /*Get Utilities of that at least 1 Room of this House has
                     -> For Filtering by RoomUtility*/
                    foreach (RoomDTO roomDTO in houseDTO.Rooms)
                    {
                        if (roomDTO.Fridge) houseDTO.Fridge = true;
                        if (roomDTO.Kitchen) houseDTO.Kitchen = true;
                        if (roomDTO.WashingMachine) houseDTO.WashingMachine = true;
                        if (roomDTO.Desk) houseDTO.Desk = true;
                        if (roomDTO.NoLiveWithHost) houseDTO.NoLiveWithHost = true;
                        if (roomDTO.Bed) houseDTO.Bed = true;
                        if (roomDTO.ClosedToilet) houseDTO.ClosedToilet = true;
                    }

                    //(Rate)
                    /*Calculate Average Rate of this house based on List Rate of it
                     -> For Filtering by Rate*/
                    float sumRate = 0;
                    if (houseDTO.Rates.Count != 0)
                    {
                        foreach (RateDTO rate in houseDTO.Rates)
                        {
                            sumRate += (float)rate.Star;
                        }
                        houseDTO.AverageRate = sumRate / houseDTO.Rates.Count;
                    }
                    else
                    {
                        //special case: no rate => averageRate = 0
                        houseDTO.AverageRate = 0;
                    }

                    //(Statistics Information)
                    houseDTO.TotallyAvailableRoomCount = RoomDAO.CountTotallyAvailableRoomByHouseId(houseDTO.HouseId);
                    houseDTO.PartiallyAvailableRoomCount = RoomDAO.CountPartiallyAvailableRoomByHouseId(houseDTO.HouseId);
                    houseDTO.AvailableCapacityCount = (int)RoomDAO.CountAvailableCapacityByHouseId(houseDTO.HouseId);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            //Remove unnecessary data to make API Response Body lighter
            houseDTOs.ForEach(delegate (AvailableHouseDTO houseDTO)
            {
                houseDTO.Rooms = null;
                houseDTO.Village = null;
                houseDTO.Rates = null;
            });

            return houseDTOs;
        }

        ////(Unused) Search house by name, with Address
        //public static List<HouseDTO> GetHouseByName(string houseName)
        //{
        //    List<HouseDTO> houseDTOs;
        //    try
        //    {
        //        using (var context = new FUHouseFinderContext())
        //        {
        //            //include Address into Houses
        //            MapperConfiguration config;
        //            config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
        //            houseDTOs = context.Houses.Include(h => h.Address).ProjectTo<HouseDTO>(config).Where(p => p.HouseName.Contains(houseName)).ToList();

        //            //find lowest room price & highest room price
        //            for (int i = 0; i < houseDTOs.Count; i++)
        //            {
        //                houseDTOs[i] = RoomDAO.GetRoomPriceById(houseDTOs[i]);
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception(e.Message);
        //    }
        //    return houseDTOs;
        //}

        /** [House Detail] Get House Detail information;
            Find detail information of a House by its Id;
            Also find its CommuneId & DistrictId
         */
        public static HouseDTO GetHouseById(int houseId)
        {
            HouseDTO houseDTO;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //include Address into Houses
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    //Get by ID
                    houseDTO = context.Houses.Where(h => h.Deleted == false).Include(h => h.Address).ProjectTo<HouseDTO>(config)
                        .Where(p => p.HouseId == houseId).FirstOrDefault();
                    houseDTO.ImagesOfHouses = context.ImagesOfHouses
                        .Where(img => img.ImageId == houseDTO.HouseId && img.Deleted == false)
                        .OrderBy(img => img.ImageId).ProjectTo<ImagesOfHouseDTO>(config).ToList();

                    //(Commune, District)
                    /*Get CommuneId & DistrictId of the Village of this House*/
                    houseDTO.CommuneId = VillageDAO.GetVillageById((int)houseDTO.VillageId).CommuneId;
                    houseDTO.DistrictId = CommuneDAO.GetCommuneById((int)houseDTO.CommuneId).DistrictId;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return houseDTO;
        }

        /**
         * [House Detail] 
         * Increase 'view' of this House by 1 when user click House Detail
         */
        public static void IncreaseView(int HouseId)
        {
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //Find house
                    House house = context.Houses.FirstOrDefault(h => h.HouseId == HouseId);
                    if (house == null)
                    {
                        throw new Exception();
                    }

                    //Increase view
                    house.View++;

                    //Update to DB
                    context.Entry<House>(house).State = EntityState.Detached;
                    context.Houses.Update(house);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /**        
         * Get list of houses by landlordId, with Address;
           Get list of Houses of 1 Landlord for him to manage
         */
        public static List<HouseDTO> GetListHousesByLandlordId(string LandlordId)
        {
            List<HouseDTO> houseDTOs;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //include address
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    //Get by LandlordId
                    houseDTOs = context.Houses.Where(h => h.Deleted == false).Include(h => h.Address).ProjectTo<HouseDTO>(config).Where(h => h.LandlordId.Equals(LandlordId)).ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return houseDTOs;
        }

        //[Landlord - List Rooms] Get total amount of money of rooms that has not been rented (of 1 House)
        public static decimal? GetMoneyForNotRentedRooms(int HouseId)
        {
            decimal? totalMoney = 0;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //Get rooms by HouseID, include Images
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    List<RoomDTO> rooms = context.Rooms
                        .Where(r => r.Deleted == false)
                        .Where(r => r.HouseId == HouseId)
                        .Where(r => r.Status.StatusName.Equals("Available") || r.Status.StatusName.Equals("Disabled"))
                        .ProjectTo<RoomDTO>(config).ToList();

                    //Count total money
                    foreach (RoomDTO r in rooms)
                    {
                        totalMoney += r.PricePerMonth;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return totalMoney;
        }

        /**
         *  [Staff - Dashboard] Count total houses;
            Count total number of Houses in the system
         */
        public static int CountTotalHouse()
        {
            int totalHouse;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //Count total houses
                    totalHouse = context.Houses.Where(h => h.Deleted == false).Count();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return totalHouse;
        }

        /**
         *  [Staff - Dashboard] [Home Page] Count available houses
            Count number of available Houses in the system
         */
        public static int CountAvailableHouse()
        {
            return GetAvailableHouses().Count();
        }

        //[Staff/list-report] Get list of reported houses
        public static List<ReportHouseDTO> GetListReportHouse()
        {
            List<ReportHouseDTO> houses;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    //Get by LandlordId
                    houses = context.Houses.Where(h => h.Deleted == false).ProjectTo<ReportHouseDTO>(config).ToList();

                    foreach (ReportHouseDTO house in houses)
                    {
                        house.NumberOfReport = ReportDAO.CountTotalReportByHouseId(house.HouseId);
                        house.ListReports = ReportDAO.GetReportByHouseId(house.HouseId);
                    }
                    houses.RemoveAll(house => house.NumberOfReport == 0);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return houses;
        }
        
        //[Staff/list-report] Count total of reported houses
        public static int CountTotalReportedHouse()
        {
            return GetListReportHouse().Count();
        }
    }
}
