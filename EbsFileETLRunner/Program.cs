using System;
using System.Collections.Generic;
using EbsDomainObjects;
using FileDbUploader.Tests;
using FileImporter;
using System.IO;

namespace EbsFileETLRunner
{
    /// <summary>
    /// Console Application to run the ETL batch
    /// Parameters input format 
    /// e.g: EbsFileETLRunner.exe myfile.txt "Data Source=MSSQL1;Initial Catalog=AdventureWorks;" "dbo.resultsTable" "CurrencyPair,Date" "Amount" "Type=D");             
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length < 5)
                {
                    Console.WriteLine(
                        @"Parameters input format 
                            e.g: EbsFileETLRunner.exe C:\myfile.txt ""Data Source=MSSQL1;Initial Catalog=AdventureWorks;"" ""dbo.resultsTable"" ""CurrencyPair,Date"" ""Amount"" ""Type=D""");
                    return;
                }

                var filePath = args[0];
                var connectionString = args[1];
                var dataTable = args[2];
                
                // Aggregation Criteria
                var aggregationKeys = args[3].Split(',');
                var aggregationValueField = args[4];

                var filterBy = args.Length > 5 ? 
                    (args[5].Contains("=") ? args[5].Split('=')  : new string[0]) 
                    : new string[0];

                // Filter Criteria
                var filter = filterBy.Length == 2 ? new KeyValuePair<string, string>(filterBy[0], filterBy[1]) : new KeyValuePair<string, string>();

                if (!File.Exists(filePath))
                {
                    Console.WriteLine("File {0} Does Not Exist", filePath);
                    return;
                }

                var connection = new MockDbConnection(connectionString);

                // ETL Modules
                var importer = new FlatFileImporter();
                var aggregator = new FileAggregator.FileAggregator();
                var uploader = new FileDbUploader.FileDbUploader();

                // Extract
                Tuple<FlatFileHeader, IEnumerable<FlatFileDataRow>> importedData = filterBy.Length == 2 ? importer.Import(filePath, filter.Key,
                    filter.Value) : importer.Import(filePath);

                FlatFileHeader header = importedData.Item1;
                IEnumerable<FlatFileDataRow> dataRows = importedData.Item2;

                // Transform
                IEnumerable<EbsAggregatedRow<decimal>> aggregatedData = aggregator.AggregateRows<decimal>(header,
                                                                                                          dataRows,
                                                                                                          aggregationKeys,
                                                                                                          aggregationValueField);

                // load
                int rowsImported = uploader.Upload(connection, dataTable, aggregatedData);

                Console.WriteLine("{0} Record Loaded into {1}", rowsImported, dataTable);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
