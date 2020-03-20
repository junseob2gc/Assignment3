using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment3.Models
{
    public class Constants
    {
        public readonly Student Student = new Student { StudentId = "200423859", FirstName = "Junseob", LastName = "Noh" };
        public class Locations
        {
            public readonly static string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            public readonly static string ExePath = Environment.CurrentDirectory;

            public readonly static string ContentFolder = $"{ExePath}\\..\\..\\..\\Content";
            public readonly static string DataFolder = $"{ExePath}\\..\\..\\..\\Content\\Data";
            public readonly static string ImageFolder = $"{ExePath}\\..\\..\\..\\Content\\Images";

            public const string InforFile = "info.csv";
            public const string ImageFile = "myimage.jpg";
            public const string ImageFile3 = "myimage3.jpg";
        }
        public class FTP
        {
            public const string Username = @"bdat100119f\bdat1001";
            public const string Password = "bdat1001";

            public const string BaseUrl = "ftp://waws-prod-dm1-127.ftp.azurewebsites.windows.net/bdat1001-20914";

            public const int OperationPauseTime = 1000;
        }
    }
}
