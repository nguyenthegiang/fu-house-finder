using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ImageOfHouseDAO
    {
        public static void CreateImageOfHouse(int HosueId, string ImageLink, string uid)
        {
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    ImagesOfHouse img = new ImagesOfHouse();
                    img.HouseId = HosueId;
                    img.ImageLink = ImageLink;
                    img.CreatedBy = uid;
                    img.CreatedDate = DateTime.UtcNow;
                    img.Deleted = false;
                    img.LastModifiedBy = uid;
                    img.LastModifiedDate = DateTime.UtcNow;
                    context.ImagesOfHouses.Add(img);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static void UpdateImageOfHouse(int imageId, string ImageLink, string uid)
        {
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    ImagesOfHouse img = context.ImagesOfHouses.Where(image => image.ImageId == imageId).FirstOrDefault();
                    if (img!= null)
                    {
                        img.ImageLink = ImageLink;
                        img.Deleted = false;
                        img.LastModifiedBy = uid;
                        img.LastModifiedDate = DateTime.Now;
                    }
                    context.ImagesOfHouses.Update(img);
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
