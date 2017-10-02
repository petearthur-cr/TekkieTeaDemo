using System;
using Cloudreach.Connect.Api;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Amazon;
using Amazon.Runtime;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using System.Linq;

namespace TekkieTeaDemo
{

    using static PluginConfiguration;

    public class EventProcessor : IEventProcessor
    {
        public string Queue => "TekkieTea";

        public IResult Implement(ISubscriberContext context)
        {
            try
            {
                context.LogService.Info("Started EventProcessor...");
                var blobUri = context.GetMessage<BlobUri>();
                context.LogService.Info("Blob URI = " + blobUri.Uri);

                var blob = new CloudBlockBlob(new Uri(blobUri.Uri), new StorageCredentials(AccountName, AccountKey));
                string blobData = blob.DownloadText();

                context.LogService.Info("Got blob data");

                AmazonDynamoDBClient client = new AmazonDynamoDBClient(
                    new BasicAWSCredentials(AWSAccessKey, AWSSecretKey), 
                    RegionEndpoint.GetBySystemName(AWSRegion));
                
                Table table = Table.LoadTable(client, TableName);

                var insertedRecords = blobData
                    .Split('\n')
                    .Where(s => s.IndexOf(',') > -1)
                    .Select(line => new Document
                        {
                            ["id"] = line.Substring(0, line.IndexOf(',')), 
                            ["data"] = line.Substring(line.IndexOf(',') + 1)
                        })
                    .Select(document => table.PutItem(document))
                    .Count();

                context.LogService.Info($"Inserted {insertedRecords} records");

                blob.Delete();
                context.LogService.Info("deleted blob.");

                return context.CreateSuccessResult();
            }
            catch (Exception e)
            {
                return context.CreateErrorResult(e.Message);
            }
        }
    }
}
