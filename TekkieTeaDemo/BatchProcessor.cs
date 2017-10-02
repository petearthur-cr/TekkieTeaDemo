using System;
using System.Linq;
using Cloudreach.Connect.Api;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;

namespace TekkieTeaDemo
{
    using static PluginConfiguration;

    public class BatchProcessor : IBatchProcessor
    {
        public int Frequency => 1;
        public UnitOfTime UnitOfTime => UnitOfTime.MINUTES;

        public IResult Implement(IPluginContext context)
        {
            try
            {
                var queue = context.GetQueue("TekkieTea");

                context.LogService.Info("Started looking for blobs to transfer...");

                var account = new CloudStorageAccount(new StorageCredentials(AccountName, AccountKey), true);

                account
                    .CreateCloudBlobClient()
                    .GetContainerReference(BlobContainer)
                    .ListBlobs(null, true)
                    .Select(blob =>
                            queue.Publish(new BlobUri { Uri = blob.Uri.ToString() }))
                    .Count();

                context.LogService.Info("finished scanning blobs.");

                return null;
            }
            catch (Exception e)
            {
                context.LogService.Info("We came up with an error");
                context.LogService.Error(e.Message);
                return context.CreateErrorResult(-1, e.Message);
            }
        }
    }
}
