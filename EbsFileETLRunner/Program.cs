using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using EbsDomainObjects;
using FileImporter;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

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
            var config = new LoggingConfiguration();
            config.AddLogSource("EbsFileETLRunner", SourceLevels.All, true)
                  .AddTraceListener(new FlatFileTraceListener("Errors.log"));
            var logWriter = new LogWriter(config);

            try
            {
                if (args.Length < 5)
                {
                    Console.WriteLine(
                        @"Parameters input format 
                            e.g: EbsFileETLRunner.exe myfile.txt ""Data Source=MSSQL1;Initial Catalog=AdventureWorks;"" ""dbo.resultsTable"" ""CurrencyPair,Date"" ""Amount"" ""Type=D""");
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
                var filter = filterBy.Length == 2 ? new KeyValuePair<string, string>(filterBy[0], filterBy[1]) : new KeyValuePair<string, string>("","");

                if (!File.Exists(filePath))
                {
                    logWriter.Write(string.Format("File {0} Does Not Exist", filePath), "Error");
                    return;
                }

                int numberOfRowsLoaded;
                using (var connection = new SqlConnection(connectionString))
                {
                    // ETL Modules
                    var importer = new FlatFileImporter();
                    var aggregator = new FileAggregator.FileAggregator();
                    var uploader = new FileUploader.FileDbUploader();

                    // Extract
                    Tuple<FlatFileHeader, IEnumerable<FlatFileDataRow>> importedData = importer.Import(filePath,
                                                                                                       filter.Key,
                                                                                                       filter.Value);

                    FlatFileHeader header = importedData.Item1;
                    IEnumerable<FlatFileDataRow> dataRows = importedData.Item2;

                    // Transform
                    IEnumerable<EbsAggregatedRow<decimal>> aggregatedData = aggregator.AggregateRows<decimal>(header,
                                                                                                              dataRows,
                                                                                                              aggregationKeys,
                                                                                                              aggregationValueField);

                    // load
                    numberOfRowsLoaded = uploader.Upload(connection, dataTable, aggregatedData);
                }
                logWriter.Write(string.Format("{0} Record Loaded into {1}", numberOfRowsLoaded, dataTable));
            }
            catch (Exception ex)
            {
                logWriter.Write(ex.Message);
                Console.WriteLine(ex.Message);
            }
        }
    }
}
