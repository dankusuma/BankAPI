using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Bank.Core.Entity
{
    public class Upload
    {
        public string photoName { get; set; }
        public string videoName { get; set; }

        string folderLocation = @"D:\";

        IFormFile uploadedPhoto { get; set; }
        IFormFile uploadedVideo { get; set; }

        public string stringPhoto { get; set; }
        public string stringVideo { get; set; }

        public string status { get; set; }
        public void convertToString()
        {
            using (MemoryStream ms = new MemoryStream())
            {
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
            uploadedPhoto = new FormFile(msPhoto, 0, photoBytes.Length, photoName, photoName);


            byte[] videoBytes = Convert.FromBase64String(stringVideo);
            MemoryStream msVideo = new MemoryStream(videoBytes);
            uploadedVideo = new FormFile(msVideo, 0, videoBytes.Length, videoName, videoName);


        }

        public void doUpload(string maxPictureSize, string maxVideoSize)
        {
            int maxPictureSizenumber = Convert.ToInt32(maxPictureSize);
            int maxVideoSizenumber = Convert.ToInt32(maxVideoSize);
            try
            {
                string statusFoto = doUpload(uploadedPhoto, new[] { ".jpg", ".png", ".bmp" }, photoName, maxPictureSizenumber);
                string statusVideo = doUpload(uploadedVideo, new[] { ".mp4", ".avi", ".mpg" }, videoName, maxVideoSizenumber);
                if (statusFoto != "Success!")
                {
                    status = statusFoto;
                    return;
                }
                else if (statusVideo != "Success!")
                {
                    status = statusVideo;
                    return;
                }
                else
                {
                    status = "Success!";
                }
            }
            catch (Exception e)
            {
                status = e.Message;
            }
        }
        public string doUpload(IFormFile file, string[] listFormat, string namaFile, int size)
        {
            if (file == null || file.Length == 0)
            {
                return "Foto kosong!";
            }
            else if (!contains(namaFile, listFormat))
            {
                return "Invalid Format for : " + namaFile;
            }
            else if (file.Length > size)
            {
                return "File Kebesaran";
            }

            else
            {
                string fileName = file.FileName;
                string targetFileName = folderLocation + namaFile;
                FileStream stream = new FileStream(targetFileName, FileMode.Create);
                file.CopyTo(stream);
                stream.Close();
                return "Success!";
            }
        }

        public bool contains(string a, string[] b)
        {
            for (int i = 0; i < b.Length; i++)
            {
                if (a.Contains(b[i]))
                {
                    return true;
                }
            }
            return false;
        }

    }

}

