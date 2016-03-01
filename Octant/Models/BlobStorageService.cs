using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Octant.Models
{
    public static class BlobStorageService
    {
        public static CloudBlobClient GetCloudBlobClient()
        {
            var storageAccount = CloudStorageAccount.Parse(
                    ConfigurationManager.AppSettings["StorageConnectionString"]
                );
            return storageAccount.CreateCloudBlobClient();
        }

        public static CloudBlobContainer GetCloudBlobContainer()
        {
            var storageAccount = CloudStorageAccount.Parse(
                    ConfigurationManager.AppSettings["StorageConnectionString"]
                );

            var client = storageAccount.CreateCloudBlobClient();
            var container = client.GetContainerReference("images");

            if (container.CreateIfNotExists())
            {
                container.SetPermissions(
                   new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob }
                );
            }

            return container;
        }
    }
}
