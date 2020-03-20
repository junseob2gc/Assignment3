using Assignment3.Models;
using Assignment3.Models.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Assignment3
{
    class Program
    {
        // Get all directories on the FTPApp for our class.
        static void GetFtpDirectories()
        {
            List<string> directories = FTPApp.GetDirectory(Constants.FTP.BaseUrl);
            int loc = 0;
            foreach (var directory in directories)
            {
                if (loc == 0)
                {
                    loc++;
                }
                else
                {
                    Console.WriteLine("\n" + directory);
                    List<string> files = FTPApp.GetDirectory(Constants.FTP.BaseUrl + "/" + directory);
                    foreach (var file in files)
                    {
                        Console.WriteLine("\t" + file);
                    }
                }
            }
        }

        // Extract data from the directory
        static void ExtractImageData()
        {
            Student student = new Student() { AbsoluteUrl = Constants.FTP.BaseUrl };
            string directory = "200423859 Junseob Noh";
            // To store StudentId, FirstName, and LastName
            student.FromDirectory(directory);

            string infoFilePath = student.FullPathUrl + "/" + Constants.Locations.InforFile;
            bool fileExists = FTPApp.FileExists(infoFilePath);
            if (fileExists == true)
            {
                byte[] bytes = FTPApp.DownloadFileBytes(infoFilePath);
                string csvData = Encoding.Default.GetString(bytes);
                string[] csvlines = csvData.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
                if (csvlines.Length != 2)
                {
                    Console.WriteLine("Error in CSV format");
                }
                else
                {
                    student.FromCSV(csvlines[1]);
                    Console.WriteLine(">>> myimage.jpg file as Base64:");
                    Console.WriteLine(student.ImageData);
                }
            }
        }

        static List<Student> CollectStudentInfor()
        {
            List<Student> students = new List<Student>();

            // fetch the diretoreis info files and Images, and then display the console them.
            List<string> directories = FTPApp.GetDirectory(Constants.FTP.BaseUrl);

            foreach (var directory in directories)
            {
                Student student = new Student() { AbsoluteUrl = Constants.FTP.BaseUrl };
                // To store StudentId, FirstName, and LastName
                student.FromDirectory(directory);

                string infoFilePath = student.FullPathUrl + "/" + Constants.Locations.InforFile;
                bool fileExists = FTPApp.FileExists(infoFilePath);
                if (fileExists == true)
                {
                    byte[] bytes = FTPApp.DownloadFileBytes(infoFilePath);
                    string csvData = Encoding.Default.GetString(bytes);
                    string[] csvlines = csvData.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
                    if (csvlines.Length != 2)
                    {
                        Console.WriteLine($"{student.FirstName} {student.LastName} has Error in CSV format");
                    }
                    else
                    {
                        student.FromCSV(csvlines[1]);
                    }
                    if (student.StudentId == "200423859")
                    {
                        student.MyRecord = true;
                    }
                    else
                    {
                        student.MyRecord = false;
                    }
                }
                else
                {
                    Console.WriteLine($"{student.FirstName} {student.LastName} Could not find info file:");
                }
                students.Add(student);
            }

            return students;
        }

        static void SaveFiles(List<Student> students, string fileName)
        {
            // 1. Save CSV file
            string studentCSVPath = $"{Constants.Locations.DataFolder}\\{fileName}.csv";

            //Establish a file stream to collect data from the response. this point is local disk.
            using (StreamWriter fs = new StreamWriter(studentCSVPath))
            {
                int headerNum = 0;
                foreach (var student in students)
                {
                    if (headerNum == 0)
                    {
                        fs.Write(student.HeaderRow + "\r\n");
                        headerNum++;
                    }
                    else
                    {
                        fs.Write(student.ToCSV() + "\r\n");
                    }

                }
            }

            // 2. Save JSON file, which is converted from CSV file
            string studentJSONPath = $"{Constants.Locations.DataFolder}\\{fileName}.json";
            string analyticsData = CsvToJson.ReadFile(studentCSVPath);
            using (StreamWriter fs = new StreamWriter(studentJSONPath))
            {
                fs.Write(analyticsData);
            }

            // 3. Save XML file, which is converted from CSV file
            string studentXMLPath = $"{Constants.Locations.DataFolder}\\{fileName}.xml";
            string csv = File.ReadAllText(studentCSVPath);
            XDocument doc = CsvToXml.ConvertCsvToXML(csv, new[] { "," });
            doc.Save(studentXMLPath);

            // 4. Upload the files to My FTP
            foreach (var student in students)
            {
                if (student.MyRecord)
                {
                    FTPApp.UploadFile(studentCSVPath, student.FullPathUrl + "/students.csv");
                    FTPApp.UploadFile(studentJSONPath, student.FullPathUrl + "/students.json");
                    FTPApp.UploadFile(studentXMLPath, student.FullPathUrl + "/students.xml");
                }

            }
        }

        static void Main(string[] args)
        {
            // 1. Retrieve a list of directories from the FTP
            Console.WriteLine(">>> Retrieve a list of directories");
            GetFtpDirectories();
            Console.WriteLine("====End of 1==================================");

            // 2. Extract data from the image
            Console.WriteLine(">>> Output the image file as Base64");
            ExtractImageData();
            Console.WriteLine("====End of 2==================================");

            // Gathering students information
            List<Student> students = CollectStudentInfor();

            // 3. Output data from ToString() and ToCSV().
            // For my record
            Student myRecord = new Student();
            foreach (var student in students)
            {
                Console.WriteLine("=============================================");
                Console.WriteLine("Output data from ToString()");
                Console.WriteLine(student.ToString());
                Console.WriteLine("---------------------------------------------");
                Console.WriteLine("Output data from ToCSV()");
                Console.WriteLine(student.ToCSV());
                if (student.MyRecord)
                {
                    myRecord = student;
                }
            }

            Console.WriteLine("=============================================");
            Console.WriteLine("Output My Record");
            Console.WriteLine(myRecord.ToString());
            Console.WriteLine("=============================================");

            Console.WriteLine($"The number of students is {students.Count}.");
            int countOfCorrectAges = 0;
            int sumOfAges = 0;

            int countOfWrongAge = 0;
            List<int> ages = new List<int>();
            int highestAge = 0;
            int lowestAge = 100;

            foreach (var student in students)
            {
                ages.Add(student.Age);
                if (student.Age > 0 && student.Age < 100)
                {
                    countOfCorrectAges++;
                    sumOfAges += student.Age;
                    if (student.Age > highestAge)
                    {
                        highestAge = student.Age;
                    }
                    if (student.Age < lowestAge)
                    {
                        lowestAge = student.Age;
                    }
                }
                else
                {
                    countOfWrongAge++;
                }
            }

            // These results are before modified values.
            Console.WriteLine("===============================================");
            Console.WriteLine("These results are before modified values.");
            Console.WriteLine($"Average of ages of students : {ages.Average()}");
            Console.WriteLine($"Highest age : {ages.Max()}");
            Console.WriteLine($"Lowest age : {ages.Min()}");

            Console.WriteLine("-----------------------------------------------");

            // These results are valid values of the ages of students.
            double avgOfAges = sumOfAges / countOfCorrectAges;
            Console.WriteLine("These results are valid values of the ages of students.");
            Console.WriteLine($"Average of ages of {countOfCorrectAges} students : {avgOfAges}");
            Console.WriteLine($"Highest age : {highestAge}");
            Console.WriteLine($"Lowest age : {lowestAge}");
            Console.WriteLine("-----------------------------------------------");


            //Save to CSV, JSON and XML files, and then uploading the files to My FTP
            SaveFiles(students, "students");

        }
    }
}
