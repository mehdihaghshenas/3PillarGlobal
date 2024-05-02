using Azure.Core;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using System.Linq;
using System.Net;
using Microsoft.Extensions.Options;
using Azure.Storage.Blobs.Models;
using Azure.Storage;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebApplication_API.Services
{
    public class BlobService : IBlobService
    {
        private IOptionsMonitor<BlobConfigs> _blobConfigs;

        public BlobService(IOptionsMonitor<BlobConfigs> blobConfigs)
        {
            _blobConfigs = blobConfigs;
        }
        public async Task UploadAttachment(int invoiceId, IFormFile[] files, bool isSecure)
        {
            CloudStorageAccount storageAccount;

            Dictionary<string, object> dict = new Dictionary<string, object>();
            if (CloudStorageAccount.TryParse(_blobConfigs.CurrentValue.ConnctionString, out storageAccount))
            {
                try
                {
                    // Create the CloudBlobClient that represents the 
                    // Blob storage endpoint for the storage account.
                    CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();
                    // Create a container called 'quickstartblobs' and 
                    // append a GUID value to it to make the name unique.
                    CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(isSecure ? _blobConfigs.CurrentValue.SecureContainer : _blobConfigs.CurrentValue.PublicContainer);
                    bool isCreated = await cloudBlobContainer.CreateIfNotExistsAsync();
                    if (isCreated)
                    {
                        // Set the permissions so the blobs are public.
                        BlobContainerPermissions permissions = new BlobContainerPermissions
                        {
                            PublicAccess = isSecure ? BlobContainerPublicAccessType.Off : BlobContainerPublicAccessType.Blob,
                        };
                        await cloudBlobContainer.SetPermissionsAsync(permissions);
                    }
                    var client = new BlobServiceClient(_blobConfigs.CurrentValue.ConnctionString)
                    .GetBlobContainerClient(_blobConfigs.CurrentValue.SecureContainer);
                    List<Task> list = new List<Task>();
                    foreach (var file in files)
                    {
                        list.Add( UploadWithTransferOptionsAsync(client, file));
                    }
                    await Task.WhenAll(list.ToArray()); 
                }
                catch
                {
                    throw;
                }
            }
            else
            {
                throw new BadHttpRequestException("Azure Connection Error");
            }
        }
        public string GenerateBlobToken(string url)
        {
            var client = new BlobServiceClient(_blobConfigs.CurrentValue.ConnctionString)
                .GetBlobContainerClient(_blobConfigs.CurrentValue.SecureContainer)
                .GetBlobClient(url);
            var uri = GetServiceSasUriForContainer(client, null);
            return uri.Query[1..].ToString();
        }
        private static Uri GetServiceSasUriForContainer(BlobClient containerClient,
                                         string? storedPolicyName = null, int hourToExpire = 24)
        {
            // Check whether this BlobContainerClient object has been authorized with Shared Key.
            if (containerClient.CanGenerateSasUri)
            {
                // Create a SAS token that's valid for one hour.
                BlobSasBuilder sasBuilder = new BlobSasBuilder()
                {
                    BlobContainerName = containerClient.Name,
                    Resource = "b"
                };

                if (storedPolicyName == null)
                {
                    sasBuilder.ExpiresOn = DateTimeOffset.UtcNow.AddHours(hourToExpire);
                    sasBuilder.SetPermissions(BlobContainerSasPermissions.Read);
                }
                else
                {
                    sasBuilder.Identifier = storedPolicyName;
                }

                Uri sasUri = containerClient.GenerateSasUri(sasBuilder);
                return sasUri;
            }
            else
            {
                throw new Exception("Can not generate sas, please contact to devops");
            }
        }
        public static async Task UploadWithTransferOptionsAsync(
            BlobContainerClient containerClient,
            IFormFile file)
        {
            BlobClient blobClient = containerClient.GetBlobClient(file.FileName);

            var transferOptions = new StorageTransferOptions
            {
                // Set the maximum number of parallel transfer workers
                MaximumConcurrency = 2,

                // Set the initial transfer length to 8 MiB
                InitialTransferSize = 8 * 1024 * 1024,

                // Set the maximum length of a transfer to 4 MiB
                MaximumTransferSize = 4 * 1024 * 1024
            };

            var uploadOptions = new BlobUploadOptions()
            {
                TransferOptions = transferOptions
            };

            var fileStream = file.OpenReadStream();
            await blobClient.UploadAsync(fileStream, uploadOptions);
            fileStream.Close();
        }
    }
}