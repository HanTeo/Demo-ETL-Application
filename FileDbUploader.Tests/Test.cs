using NUnit.Framework;
using System;
using FileImporter;
using System.Collections.Generic;
using EbsDomainObjects;

namespace FileDbUploader.Tests
{
    [TestFixture]
    public class Test
    {
        private string expectedSql; 

        [SetUp]
        public void SetUp()
        {
            expectedSql = @"MERGE 
				   MyTable AS target
				USING 
				   (select 'GBP/USD' as CurrencyPair, '2010-02-01' as Date, 2600000 as Amount) AS source
				ON 
				   target.CurrencyPair = source.CurrencyPair 
				AND
				   target.Date = source.Date 
				   
				WHEN MATCHED THEN 
				   UPDATE SET target.Amount = target.Amount + source.Amount
				WHEN NOT MATCHED THEN 
				   INSERT (CurrencyPair, Date, Amount) VALUES ('GBP/USD', '2010-02-01', 2600000)
				; 
MERGE 
				   MyTable AS target
				USING 
				   (select 'EUR/USD' as CurrencyPair, '2010-02-01' as Date, 300000 as Amount) AS source
				ON 
				   target.CurrencyPair = source.CurrencyPair 
				AND
				   target.Date = source.Date 
				   
				WHEN MATCHED THEN 
				   UPDATE SET target.Amount = target.Amount + source.Amount
				WHEN NOT MATCHED THEN 
				   INSERT (CurrencyPair, Date, Amount) VALUES ('EUR/USD', '2010-02-01', 300000)
				; 
MERGE 
				   MyTable AS target
				USING 
				   (select 'GBP/USD' as CurrencyPair, '2010-02-02' as Date, 2100000 as Amount) AS source
				ON 
				   target.CurrencyPair = source.CurrencyPair 
				AND
				   target.Date = source.Date 
				   
				WHEN MATCHED THEN 
				   UPDATE SET target.Amount = target.Amount + source.Amount
				WHEN NOT MATCHED THEN 
				   INSERT (CurrencyPair, Date, Amount) VALUES ('GBP/USD', '2010-02-02', 2100000)
				; 
MERGE 
				   MyTable AS target
				USING 
				   (select 'EUR/USD' as CurrencyPair, '2010-02-02' as Date, 1400000 as Amount) AS source
				ON 
				   target.CurrencyPair = source.CurrencyPair 
				AND
				   target.Date = source.Date 
				   
				WHEN MATCHED THEN 
				   UPDATE SET target.Amount = target.Amount + source.Amount
				WHEN NOT MATCHED THEN 
				   INSERT (CurrencyPair, Date, Amount) VALUES ('EUR/USD', '2010-02-02', 1400000)
				; 
MERGE 
				   MyTable AS target
				USING 
				   (select 'SGD/USD' as CurrencyPair, '2010-02-03' as Date, 1400000 as Amount) AS source
				ON 
				   target.CurrencyPair = source.CurrencyPair 
				AND
				   target.Date = source.Date 
				   
				WHEN MATCHED THEN 
				   UPDATE SET target.Amount = target.Amount + source.Amount
				WHEN NOT MATCHED THEN 
				   INSERT (CurrencyPair, Date, Amount) VALUES ('SGD/USD', '2010-02-03', 1400000)
				; 
MERGE 
				   MyTable AS target
				USING 
				   (select 'EUR/USD' as CurrencyPair, '2010-02-03' as Date, 200000 as Amount) AS source
				ON 
				   target.CurrencyPair = source.CurrencyPair 
				AND
				   target.Date = source.Date 
				   
				WHEN MATCHED THEN 
				   UPDATE SET target.Amount = target.Amount + source.Amount
				WHEN NOT MATCHED THEN 
				   INSERT (CurrencyPair, Date, Amount) VALUES ('EUR/USD', '2010-02-03', 200000)
				; 
MERGE 
				   MyTable AS target
				USING 
				   (select 'GBP/USD' as CurrencyPair, '2010-02-03' as Date, 500000 as Amount) AS source
				ON 
				   target.CurrencyPair = source.CurrencyPair 
				AND
				   target.Date = source.Date 
				   
				WHEN MATCHED THEN 
				   UPDATE SET target.Amount = target.Amount + source.Amount
				WHEN NOT MATCHED THEN 
				   INSERT (CurrencyPair, Date, Amount) VALUES ('GBP/USD', '2010-02-03', 500000)
				; " + Environment.NewLine;
        }

        /// <summary>
        /// Test simulates the full ETL cycle with a Mocked DB
        /// Extract - Extracts the data from file
        /// Transform - Transform the source data by aggregating it according to the aggregation criteria
        /// Load - Upload the transformed to the database
        /// </summary>
        [Test]
        public void FullEndToEndTest()
        {
            var filePath = string.Format("{0}/{1}", Environment.CurrentDirectory, "TestFlatFile");
            var connectionString = "fakeConnectionString";
            var connection = new MockDbConnection(connectionString);

            // ETL Modules
            var importer = new FlatFileImporter();
            var aggregator = new FileAggregator.FileAggregator();
            var uploader = new global::FileUploader.FileDbUploader();

            // Aggregation and Filter Criteria
            var aggregationKeys = new[] {"CurrencyPair", "Date"};
            var aggregationValueField = "Amount";
            var filter = new KeyValuePair<string, string>("Type", "D");

            // Extract
            Tuple<FlatFileHeader, IEnumerable<FlatFileDataRow>> importedData = importer.Import(filePath, filter.Key,
                                                                                               filter.Value);
            FlatFileHeader header = importedData.Item1;
            IEnumerable<FlatFileDataRow> dataRows = importedData.Item2;

            // Transform
            IEnumerable<EbsAggregatedRow<decimal>> aggregatedData = aggregator.AggregateRows<decimal>(header, dataRows,
                                                                                                      aggregationKeys,
                                                                                                      aggregationValueField);

            // load
            int rowsImported = uploader.Upload(connection, "MyTable", aggregatedData);

            var executedSql = connection.Command.ExecutedCommands.ToString();

            Assert.AreEqual(expectedSql, executedSql);
        }
    }
}