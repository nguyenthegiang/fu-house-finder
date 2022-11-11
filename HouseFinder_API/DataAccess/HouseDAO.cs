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
        public static HouseDTO CreateHouse(string houseName, string information, int addressId, int villageId, string landlordId, int campusId,
            decimal powerPrice, decimal waterPrice, bool fingerprintLock, bool camera, bool parking)
        {
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    House house = new House();
                    house.HouseName = houseName;
                    house.Information = information;
                    house.AddressId = addressId;
                    house.CampusId = campusId;
                    house.VillageId = villageId;
                    house.LandlordId = landlordId;
                    house.PowerPrice = powerPrice;
                    house.WaterPrice = waterPrice;
                    house.FingerprintLock = fingerprintLock;
                    house.Camera = camera;
                    house.Parking = parking;
                    house.View = 0;
                    house.Deleted = false;
                    house.CreatedDate = DateTime.UtcNow;
                    house.LastModifiedDate = DateTime.UtcNow;
                    house.CreatedBy = landlordId;
                    house.LastModifiedBy = landlordId;
                    context.Houses.Add(house);
                    context.SaveChanges();
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    var mapper = config.CreateMapper();
                    return mapper.Map<HouseDTO>(house);
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
                    //Find rooms of this house
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
                    foreach(Room o in rooms)
                    {
                        o.Deleted = true;
                        context.Rooms.Update(o);
                    }
                    List<Rate> rates = context.Rates.Where(r => r.HouseId == houseId).ToList();
                    foreach(Rate r in rates)
                    {
                        r.Deleted = true;
                        context.Rates.Update(r);
                    }
                    List<ImagesOfHouse> imagesOfHouses = context.ImagesOfHouses.Where(i => i.HouseId == houseId).ToList();
                    foreach(ImagesOfHouse i in imagesOfHouses)
                    {
                        i.Deleted = true;
                        context.ImagesOfHouses.Update(i);
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
        ////(Unused) Get list of houses, with Address & Images
        //public static List<HouseDTO> GetAllHouses()
        //{
        //    List<HouseDTO> houseDTOs;
        //    try
        //    {
        //        using (var context = new FUHouseFinderContext())
        //        {
        //            //include address, images
        //            MapperConfiguration config;
        //            config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
        //            houseDTOs = context.Houses
        //                //unnecessary includes
        //                //.Include(house => house.Address)
        //                //.Include(house => house.ImagesOfHouses)
        //                .ProjectTo<HouseDTO>(config).ToList();

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

        /*[Home Page] Get list of available houses, with Address, Images & Rooms
            Availabe house: house with at least 1 available room */
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
                    } else
                    {
                        //special case: no rate => averageRate = 0
                        houseDTO.AverageRate = 0;
                    }
                    
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            //Remove unnecessary data to make API Response Body lighter
            houseDTOs.ForEach(delegate(AvailableHouseDTO houseDTO) 
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

        //[House Detail] Get House Detail information
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

                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return houseDTO;
        }

        //Get list of houses by landlordId, with Address
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
                    List<RoomDTO>  rooms = context.Rooms
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

        //[Staff - Dashboard] Count total houses
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

        //[Staff - Dashboard] [Home Page] Count available houses
        public static int CountAvailableHouse()
        {
            int availableHouse;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //Count available houses: houses having at least 1 room available
                    availableHouse = context.Rooms
                        //not considering deleted rooms
                        .Where(room => room.Deleted == false)
                        //not considering deleted houses
                        .Where(room => room.House.Deleted == false)
                        .Where(room => room.Status.StatusName.Equals("Available"))
                        .GroupBy(room => room.HouseId)
                        .Count();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return availableHouse;
        }

        //[Staff/list-report] Get list of report house
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

    }
}
