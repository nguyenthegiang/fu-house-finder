﻿using AutoMapper;
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
        //Get list of houses, with Address & Images
        public static List<HouseDTO> GetAllHouses()
        {
            List<HouseDTO> houseDTOs;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //include address, images
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    houseDTOs = context.Houses
                        //unnecessary includes
                        //.Include(house => house.Address)
                        //.Include(house => house.ImagesOfHouses)
                        .ProjectTo<HouseDTO>(config).ToList();

                    //find lowest room price & highest room price
                    for (int i = 0; i < houseDTOs.Count; i++)
                    {
                        houseDTOs[i] = RoomDAO.GetRoomPriceById(houseDTOs[i]);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return houseDTOs;
        }

        /*[Home Page] Get list of available houses, with Address, Images & Rooms
            Availabe house: house with at least 1 available room */
        public static List<HouseDTO> GetAvailableHouses()
        {
            List<HouseDTO> houseDTOs;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //include address, images
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    houseDTOs = context.Houses
                        //unnecessary includes
                        //.Include(house => house.Address)
                        //.Include(house => house.ImagesOfHouses)
                        .ProjectTo<HouseDTO>(config).ToList();

                    //find lowest room price & highest room price
                    for (int i = 0; i < houseDTOs.Count; i++)
                    {
                        houseDTOs[i] = RoomDAO.GetRoomPriceById(houseDTOs[i]);
                    }
                }

                /*for each house: check to see if it has available room
                if not, remove from list*/
                List<HouseDTO> availableHouses = new List<HouseDTO>();
                foreach (HouseDTO houseDTO in houseDTOs)
                {
                    if (RoomDAO.CountAvailableRoomByHouseId(houseDTO.HouseId) > 0)
                    {
                        availableHouses.Add(houseDTO);
                    }
                }
                houseDTOs = availableHouses;

                //Get list (as a string) of ID of RoomTypes of all Rooms of each House -> For Filtering by RoomType
                foreach (HouseDTO houseDTO in houseDTOs)
                {
                    houseDTO.RoomTypeIds = "";
                    foreach (RoomDTO roomDTO in houseDTO.Rooms)
                    {
                        if (!houseDTO.RoomTypeIds.Contains(roomDTO.RoomTypeId.ToString()))
                        {
                            houseDTO.RoomTypeIds += roomDTO.RoomTypeId.ToString();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return houseDTOs;
        }

        //Search house by name, with Address
        public static List<HouseDTO> GetHouseByName(string houseName)
        {
            List<HouseDTO> houseDTOs;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //include Address into Houses
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    houseDTOs = context.Houses.Include(h => h.Address).ProjectTo<HouseDTO>(config).Where(p => p.HouseName.Contains(houseName)).ToList();

                    //find lowest room price & highest room price
                    for (int i = 0; i < houseDTOs.Count; i++)
                    {
                        houseDTOs[i] = RoomDAO.GetRoomPriceById(houseDTOs[i]);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return houseDTOs;
        }

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

        //[Landlord - List Rooms] Get total amount of money of rooms that has not been rented
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
                    availableHouse = context.Rooms.Where(r => r.Status.StatusName.Equals("Available")).GroupBy(r => r.HouseId).Count();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return availableHouse;
        }

        //[Homepage] Count totally available room by house id
        public static int CountTotallyAvailableRoomByHouseId(int houseId)
        {
            int total;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //Get list totally available room by house id
                    List<RoomDTO> availableRooms = RoomDAO.GetAvailableRoomsByHouseId(houseId).Where(r => r.CurrentAmountOfPeople == 0).ToList();
                    //Calculate the number of totally available rooms
                    total = availableRooms.Count();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return total;
        }

    }
}
