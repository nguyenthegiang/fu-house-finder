using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class StorageRepository : IStorageRepository
    {
        private readonly IAmazonS3 Client;
        private readonly string Bucket;
        public StorageRepository(string accessKey, string secretKey, string bucket)
        {
            Client = new AmazonS3Client(accessKey, secretKey, RegionEndpoint.APSoutheast1);
            Bucket = bucket;

        }

        public bool DeleteFile(string fileName)
        {
            throw new NotImplementedException();
        }

        public string RetrieveFile(string fileName)
        {
            string url = "";
            try
            {
                GetPreSignedUrlRequest request = new GetPreSignedUrlRequest
                {
                    BucketName = Bucket,
                    Key = fileName,
                    Expires = DateTime.UtcNow.AddMinutes(30),
                    Verb = HttpVerb.GET
                };
                url = Client.GetPreSignedURL(request);
            }
            catch (Exception)
            {
            }
            return url;
        }

        /**
         * Upload file to Server Amazon S3
         */
        public async Task<bool> UploadFileAsync(string fileName, Stream file)
        {
            try
            {
                var fileTransferUtility = new TransferUtility(Client);
                string keyName = fileName;
                await fileTransferUtility.UploadAsync(file, Bucket, keyName);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}
