﻿using AutoMapper;
using BusinessObjects;
using DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class AddressDAO
    {
        public static AddressDTO CreateAddress(string _address, string googleAddress)
        {
            try
            {
                using(var context = new FUHouseFinderContext())
                {
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    var mapper = config.CreateMapper();
                    Address address = new Address();
                    address.Addresses = _address;
                    address.GoogleMapLocation = googleAddress;
                    address.CreatedDate = DateTime.UtcNow;
                    address.LastModifiedDate = DateTime.UtcNow;
                    context.Addresses.Add(address);
                    context.SaveChanges();
                    return mapper.Map<AddressDTO>(address);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void UpdateAddress(Address address)
        {
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    context.Addresses.Update(address);
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
