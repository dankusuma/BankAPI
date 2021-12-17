using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Bank.Core.Entity
{
    public class Upload
    {
        string photoName { get; set; }
        string videoName { get; set; }

        string folderLocation = @"D:\Quiz\.Net Bootcamp\Self Exercise\TestingUpload\TestingUpload\upload\photo";

        public IFormFile uploadedPhoto { get; set; }
        public IFormFile uploadedVideo { get; set; }

        public string stringPhoto { get; set; }
        public string stringVideo { get; set; }

        public Upload(string photoName, string videoName) {
            this.photoName = photoName;
            this.videoName = videoName;
        }

        public void convertToString() {
            using (MemoryStream ms = new MemoryStream()) {
                uploadedPhoto.CopyTo(ms);
                byte[] fileBytes = ms.ToArray();
                stringPhoto = Convert.ToBase64String(fileBytes);
            }

            using (MemoryStream ms = new MemoryStream())
            {
                uploadedVideo.CopyTo(ms);
                byte[] fileBytes = ms.ToArray();
                stringVideo = Convert.ToBase64String(fileBytes);
            }
        }

        public void convertToFile()
        {
            byte[] photoBytes = Convert.FromBase64String(stringPhoto);
            MemoryStream msPhoto = new MemoryStream(photoBytes);
            uploadedPhoto = new FormFile(msPhoto, 0, photoBytes.Length,photoName,photoName);
            msPhoto.Close();

            byte[] videoBytes = Convert.FromBase64String(stringVideo);
            MemoryStream msVideo = new MemoryStream(videoBytes);
            uploadedVideo = new FormFile(msVideo, 0, videoBytes.Length, videoName, videoName);
            msVideo.Close();
            
        }

        public void doUpload() {
            doUploadFoto();
            doUploadVideo();
        }
        public void doUploadFoto() {
            if (uploadedPhoto == null || uploadedPhoto.Length == 0)
            {
                return;
            }
            
            string fileName = uploadedPhoto.FileName;
            string targetFileName = folderLocation + photoName;
            using (var stream = new FileStream(targetFileName, FileMode.Create))
            {
                uploadedPhoto.CopyTo(stream);
            }
        }

        public void doUploadVideo()
        {
            if (uploadedVideo == null || uploadedVideo.Length == 0)
            {
                return;
            }

            string fileName = uploadedVideo.FileName;
            string targetFileName = folderLocation + videoName;
            using (var stream = new FileStream(targetFileName, FileMode.Create))
            {
                uploadedVideo.CopyTo(stream);
            }
        }

    }
}
