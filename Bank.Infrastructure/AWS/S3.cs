using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Infrastructure.AWS
{
    public class S3
    {
        private readonly IConfiguration _configuration;
        public S3(IConfiguration configuration, IAmazonS3 s3Client)
        {
            _configuration = configuration;
            bucketName =  _configuration["S3BucketName"];
            client = s3Client;
        }
        public string bucketName { get; set; }
        public IAmazonS3 client { get; set; }


        public async Task<Stream> ReadObjectDataAsync(string keyName)
        {
            // snippet-start:[S3.dotnetv3.GetObjectExample]

            try
            {
                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = keyName,
                };

                using (var getObjectResponse = await client.GetObjectAsync(request))
                {
                    using (var responseStream = getObjectResponse.ResponseStream)
                    {
                        var stream = new MemoryStream();
                        await responseStream.CopyToAsync(stream);
                        stream.Position = 0;
                        return stream;
                    }
                }
            }
            catch (AmazonS3Exception e)
            {
                throw new Exception("Read object operation failed.", e);
            }
        }

        public async Task<string> UploadObjectFromContentAsync(
           IFormFile content)
        {
            string id = Guid.NewGuid().ToString();
            using (var newMemoryStream = new MemoryStream())
            {
                content.CopyTo(newMemoryStream);

                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = newMemoryStream,
                    Key = id,
                    BucketName = bucketName,
                };

                var fileTransferUtility = new TransferUtility(client);
                await fileTransferUtility.UploadAsync(uploadRequest);
                return id;
            }
        }


    }
}
