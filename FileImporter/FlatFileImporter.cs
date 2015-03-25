using System;
using System.IO;
using System.Collections.Generic;

namespace FileImporter
{
    public class FlatFileImporter
    {
        /// <summary>
        /// Import the file from specified path
        /// Optionally filters/skips rows if criteria specified
        /// </summary>
        /// <param name="path">file path</param>
        /// <param name="columnToFilter">Column to filter criteria</param>
        /// <param name="filterValue">Filter value criteria</param>
        public Tuple<FlatFileHeader, IEnumerable<FlatFileDataRow>> Import(string path, string columnToFilter = "",
                                                                          string filterValue = "")
        {
            Tuple<FlatFileHeader, IEnumerable<FlatFileDataRow>> data = null;

            try
            {
                if (!File.Exists(path))
                {
                    Console.WriteLine("File Not Found : {0}", path);
                    return null;
                }

                using (var reader = new StreamReader(path))
                {
                    var header = new FlatFileHeader(reader.ReadLine());
                    var dataRows = new List<FlatFileDataRow>();

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var row = new FlatFileDataRow(line);

                        if (PassesFilterCriteria(header, row, columnToFilter, filterValue))
                        {
                            dataRows.Add(row);
                        }
                    }

                    data = Tuple.Create<FlatFileHeader, IEnumerable<FlatFileDataRow>>(header, dataRows);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }

        private static bool PassesFilterCriteria(FlatFileHeader header, FlatFileDataRow row, string columnName,
                                                 string columnValue)
        {
            if (header.HeaderColumnIndex.Count != row.Columns.Length)
            {
                return false;
            }

            if (columnName == string.Empty || columnValue == string.Empty)
            {
                return true;
            }

            return row.Columns[header.HeaderColumnIndex[columnName]] == columnValue;
        }
    }
}