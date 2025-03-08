using Minio;
using Minio.DataModel.Args;
using PatientManagerClassLibrary.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientManagerClassLibrary.Services
{
    public class MinioService : IMinioService
    {
        private readonly IMinioClient _minioClient;
        private const string BUCKET_NAME = "checkup-images";

        public MinioService()
        {
            _minioClient = new MinioClient()
                .WithCredentials("yourMinioAccessKey", "yourMinioPassword")
                .WithEndpoint("localhost:9000")
                .Build();
        }

        public async Task DeleteObject(string objectId)
        {
            await _minioClient.RemoveObjectAsync(
                new RemoveObjectArgs()
                    .WithBucket(BUCKET_NAME)
                    .WithObject(objectId)
            );
        }

        public async Task<MinioObjectResponse> GetObject(string objectId)
        {
            MemoryStream memoryStream = new();

            var objectResponse = await _minioClient.GetObjectAsync(
                new GetObjectArgs()
                    .WithBucket(BUCKET_NAME)
                    .WithObject(objectId)
                    .WithCallbackStream(stream =>
                    {
                        stream.CopyTo(memoryStream);
                    })
                );

            memoryStream.Position = 0;

            return new()
            {
                Data = memoryStream,
                ContentType = objectResponse.ContentType
            };
        }

        public async Task<string> PutObject(Stream data, string fileName, string contentType, long size)
        {
            string objectId = $"{Guid.NewGuid().ToString()}_{fileName}";

            await _minioClient.PutObjectAsync(
                new PutObjectArgs()
                    .WithBucket(BUCKET_NAME)
                    .WithObjectSize(size)
                    .WithContentType(contentType)
                    .WithObject(objectId)
                    .WithStreamData(data)
                );

            return objectId;
        }
    }
}
