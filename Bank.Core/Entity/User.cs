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

        public void doUpload()
        {
            try
            {
                status = doUpload(uploadedPhoto, new[] { ".jpg", ".png", ".bmp" }, photoName);
                status = doUpload(uploadedVideo, new[] { ".mp4", ".avi", ".mpg" }, videoName);
            }
            catch (Exception e)
            {
                status = e.Message;
            }
        }
        public string doUpload(IFormFile a, string[] b, string c)
        {
            if (a == null || a.Length == 0)
            {
                return "Foto kosong!";
            }
            else if (!contains(c, b))
            {
                return "Invalid Format for : " + photoName;
            }
            else if (a.Length > 10000000)
            {
                return "File Kebesaran";
            }

            else
            {
                string fileName = a.FileName;
                string targetFileName = folderLocation + c;
                FileStream stream = new FileStream(targetFileName, FileMode.Create);
                a.CopyTo(stream);
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

