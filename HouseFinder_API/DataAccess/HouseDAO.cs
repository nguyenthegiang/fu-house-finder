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
                    houseDTO = context.Houses.Include(h => h.Address).ProjectTo<HouseDTO>(config)
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
                    houseDTOs = context.Houses.Include(h => h.Address).ProjectTo<HouseDTO>(config).Where(h => h.LandlordId.Equals(LandlordId)).ToList();
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
                    totalHouse = context.Houses.Count();
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

    }
}
