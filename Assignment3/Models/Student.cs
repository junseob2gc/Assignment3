using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment3.Models
{
    public class Student
    {
        public string HeaderRow = $"{nameof(Student.StudentId)},{nameof(Student.FirstName)},{nameof(Student.LastName)},{nameof(Student.DateOfBirth)},{nameof(Student.Age)},{nameof(Student.ImageData)},{nameof(Student.MyRecord)}";
        public string StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        private string _DateOfBirth;
        public string DateOfBirth
        {
            get { return _DateOfBirth; }
            set
            {
                _DateOfBirth = value;
                // Convert DateOfBirth to DataTime. _DateOfBirth will be string, so it needs to convert to date type.
                DateTime dtOut;
                DateTime.TryParse(_DateOfBirth, out dtOut);
                DateOfBirthDT = dtOut;

                _Age = DateTime.Now.Year - DateOfBirthDT.Year;
                if (DateTime.Now.DayOfYear < DateOfBirthDT.DayOfYear)
                    _Age = _Age - 1;
            }
        }

        private int _Age;
        // read-only Age
        public int Age { get { return _Age; } }


        public DateTime DateOfBirthDT { get; internal set; }

        public string ImageData { get; set; }

        public bool MyRecord { get; set; }

        public string AbsoluteUrl { get; set; }
        public string FullPathUrl
        {
            get
            {
                return AbsoluteUrl + "/" + Directory;
            }
        }
        public string Directory { get; set; }
        public List<string> Exceptions { get; set; } = new List<string>();

        public void FromDirectory(string directory)
        {
            Directory = directory;  // Is this line to set value for Directory?

            if (String.IsNullOrEmpty(directory.Trim()))
            {
                return;
            }

            string[] data = directory.Trim().Split(" ", StringSplitOptions.None);

            StudentId = data[0];
            FirstName = data[1];
            LastName = data[2];
        }

        public override string ToString()
        {
            string result = $"{StudentId} {FirstName} {LastName} {DateOfBirth} {Age} {ImageData} {MyRecord}";
            return result;
        }

        public void FromCSV(string csvdata)
        {
            string[] data = csvdata.Split(",", StringSplitOptions.None);

            try
            {
                StudentId = data[0];
                FirstName = data[1];
                LastName = data[2];
                DateOfBirth = data[3];
                ImageData = data[4];
            }
            catch (Exception e)
            {
                Exceptions.Add(e.Message);
            }

        }

        public string ToCSV()
        {
            string result = $"{StudentId},{FirstName},{LastName},{DateOfBirthDT.ToShortDateString()},{Age},{ImageData},{MyRecord}";
            return result;
        }

    }
}
