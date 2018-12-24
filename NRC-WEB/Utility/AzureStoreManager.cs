using log4net;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using NRC_WEB.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ZXing;

namespace NRC_WEB.Utility
{
    public class AzureStoreManager
    {
        private CloudBlobClient _client;
        ILog logger = LogManager.GetLogger("RollingFile");
        Uri _baseUri;
        string storageAccountName;
        string storageAccountKey;
        StorageCredentials credentials;
        public AzureStoreManager()
        {
            try
            {
                var baseUriString = ConfigurationManager.AppSettings["BlobBaseUri"];
                _baseUri = new Uri(baseUriString);
                storageAccountName = ConfigurationManager.AppSettings["CloudStorageAccountName"];
                storageAccountKey = ConfigurationManager.AppSettings["CloudStorageAccessKey"];
                StorageCredentials credentials = new StorageCredentials(storageAccountName, storageAccountKey);
                _client = new CloudBlobClient(_baseUri, credentials);
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("Exception occurred: {0}", ex);
                throw;
            }
        }

        public void SaveBarCodeToCloud(Ticket ticket)
        {
            try
            {
                string blobContainerName = ConfigurationManager.AppSettings["BlobContainerName"];
                string barcodeExtension = ConfigurationManager.AppSettings["BarcodeExtension"];
                string newFileName = ticket.TicketNumber + barcodeExtension;          
                var blobContainer = _client.GetContainerReference(blobContainerName);
                var blob = blobContainer.GetBlockBlobReference(newFileName);
                ticket.BarcodeUri = string.Empty;

                MemoryStream barcodeStream = new MemoryStream();

                // instantiate a writer object
                var barcodeWriter = new BarcodeWriter();

                // set the barcode format
                barcodeWriter.Format = BarcodeFormat.AZTEC;
             
                // write text and generate a 2-D barcode as a bitmap
                Bitmap image = barcodeWriter.Write(ticket.TicketNumber);
                image.Save(barcodeStream, ImageFormat.Bmp);
                barcodeStream.Position = 0;
                //Upload barcode image from stream
                blob.UploadFromStreamAsync(barcodeStream); 
                Uri blobUri = GetCloudUri(blobContainer, blob);
                ticket.BarcodeUri =  blobUri.AbsoluteUri;
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("Exception occurred: {0}", ex);
                throw;
            }

        }

        public static void TestBarcode()
        {
            // instantiate a writer object
            var barcodeWriter = new BarcodeWriter();

            // set the barcode format
            barcodeWriter.Format = BarcodeFormat.AZTEC;
            var newguid = Guid.NewGuid().ToString();
            var newfilename = newguid + ".bmp";
            // write text and generate a 2-D barcode as a bitmap
            Bitmap image = barcodeWriter.Write(newguid);

            //.Save(@"C:\Users\jeremy\Desktop\generated.bmp");
            image.Save(newfilename);
        }

        public string SaveImageFileToCloud(string base64string)
        {
            try
            {
                var imageUri = string.Empty;
                if (!string.IsNullOrEmpty(base64string))
                {
                    string blobContainerName = ConfigurationManager.AppSettings["BlobContainerName"];
                    var blobContainer = _client.GetContainerReference(blobContainerName);
                    string newFileName = Guid.NewGuid() + ".png";
                    var blob = blobContainer.GetBlockBlobReference(newFileName);
                    //await blob.UploadFromStreamAsync(httpPostedFile.InputStream);
                    var imageByteArray = Convert.FromBase64String(base64string);
                    var imageStream = new MemoryStream(imageByteArray);
                    blob.UploadFromStreamAsync(imageStream);
                    Uri blobUri = GetCloudUri(blobContainer, blob);
                    imageUri = blobUri.AbsoluteUri;
                }

                return imageUri;
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("Exception occurred: {0}", ex);
                throw;
            }

        }

        public Uri GetCloudUri(CloudBlobContainer container, CloudBlockBlob blob)
        {
            //This block uses permissions and a signature. Meant for private blobs
            //SharedAccessBlobPolicy policy = new SharedAccessBlobPolicy();
            //policy.Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.List;
            //policy.SharedAccessExpiryTime = DateTime.Now.AddYears(1);
            //policy.SharedAccessStartTime = DateTime.Now.AddMinutes(-20);
            //string signature = blob.GetSharedAccessSignature(policy);
            //var uri = new Uri(_baseUri, $"/{container.Name}/{blob.Name}{signature}");


            var uri = new Uri(_baseUri, $"/{container.Name}/{blob.Name}");
            return uri;
        }

    }

}