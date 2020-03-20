using System;
using System.Xml.Linq;

namespace Assignment3.Models.Utilities
{
    class CsvToXml
    {
        /// <summary>
        /// Conversion the input file from csv format to XML
        /// Conversion Method
        /// </summary>
        /// <param name="csvString" > 
        /// cvs string to converted
        /// </param>
        /// <param name="separatorField">
        /// separator used by the csv file
        /// </param>
        /// <return>
        /// XDocument with the created XML
        /// </return>
        public static XDocument ConvertCsvToXML(string csvString, string[] separatorField)

        {
            //split the rows
            var sep = new[] { "\r\n" };
            string[] rows = csvString.Split(sep, StringSplitOptions.RemoveEmptyEntries);
            //Create the declaration
            var xsurvey = new XDocument(
                new XDeclaration("1.0", "UTF-8", "yes"));
            var xroot = new XElement("root"); //Create the root
            for (int i = 0; i < rows.Length; i++)
            {
                //Create each row
                if (i > 0)
                {
                    xroot.Add(rowCreator(rows[i], rows[0], separatorField));
                }
            }
            xsurvey.Add(xroot);
            return xsurvey;
        }

        /// <summary>
        /// Private. Take a csv line and convert in a row - var node
        /// with the fields values as attributes. 
        /// <param name=""row"" />csv row to process</param />
        /// <param name=""firstRow"" />First row with the fields names</param />
        /// <param name=""separatorField"" />separator string use in the csv fields</param />
        /// </summary></returns />
        private static XElement rowCreator(string row,
                       string firstRow, string[] separatorField)
        {

            string[] temp = row.Split(separatorField, StringSplitOptions.None);
            string[] names = firstRow.Split(separatorField, StringSplitOptions.None);
            var xrow = new XElement("row");
            for (int i = 0; i < temp.Length; i++)
            {
                //Create the element var and Attributes with the field name and value
                var xvar = new XElement("var",
                                        new XAttribute("name", names[i]),
                                        new XAttribute("value", temp[i]));
                xrow.Add(xvar);
            }
            return xrow;
        }
    }
}
