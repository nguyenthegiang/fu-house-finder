using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface IStorageRepository
    {
        public Task<bool> UploadFileAsync(string fileName, Stream file);
        public string RetrieveFile(string fileName);
        public Boolean DeleteFile(string fileName);
    }
}
