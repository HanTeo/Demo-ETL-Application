using NUnit.Framework;
using FileImporter;
using System.Collections.Generic;

namespace FileAggregator.Tests
{
    [TestFixture]
    public class Test
    {
        private FlatFileHeader header;
        private ICollection<FlatFileDataRow> dataRows;
        private string[] expectedAggregated;

        [SetUp]
        public void Setup()
        {
            header = new FlatFileHeader("CurrencyPair,Date,Type,Amount");
            dataRows = new List<FlatFileDataRow>
                {
                    new FlatFileDataRow("GBP/USD,2010-02-01,D,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-01,D,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-01,D,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-01,D,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-01,D,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-01,D,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-01,D,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-01,D,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-01,D,100000"),
                    new FlatFileDataRow("EUR/USD,2010-02-01,D,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-01,D,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-01,D,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-01,D,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-01,D,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-01,D,100000"),
                    new FlatFileDataRow("EUR/USD,2010-02-01,D,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-01,D,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-01,D,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-01,D,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-01,D,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-01,D,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-01,D,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-01,D,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-01,D,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-01,D,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-01,D,100000"),
                    new FlatFileDataRow("EUR/USD,2010-02-01,D,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-01,D,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-01,D,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-02,D,100000"),
                    new FlatFileDataRow("EUR/USD,2010-02-02,D,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-02,D,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-02,D,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-02,D,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-02,D,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-02,D,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-02,D,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-02,D,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-02,D,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-02,D,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-02,D,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-02,D,100000"),
                    new FlatFileDataRow("EUR/USD,2010-02-02,D,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-02,D,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-02,D,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-02,D,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-02,D,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-02,D,100000"),
                    new FlatFileDataRow("EUR/USD,2010-02-02,D,100000"),
                    new FlatFileDataRow("EUR/USD,2010-02-02,D,100000"),
                    new FlatFileDataRow("EUR/USD,2010-02-02,D,100000"),
                    new FlatFileDataRow("EUR/USD,2010-02-02,D,100000"),
                    new FlatFileDataRow("EUR/USD,2010-02-02,D,100000"),
                    new FlatFileDataRow("EUR/USD,2010-02-02,D,100000"),
                    new FlatFileDataRow("EUR/USD,2010-02-02,D,100000"),
                    new FlatFileDataRow("EUR/USD,2010-02-02,D,100000"),
                    new FlatFileDataRow("EUR/USD,2010-02-02,D,100000"),
                    new FlatFileDataRow("EUR/USD,2010-02-02,D,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-02,D,100000"),
                    new FlatFileDataRow("EUR/USD,2010-02-02,D,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-02,D,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-02,D,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-02,D,100000"),
                    new FlatFileDataRow("EUR/USD,2010-02-02,D,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-02,Q,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-02,Q,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-02,Q,100000"),
                    new FlatFileDataRow("EUR/USD,2010-02-02,Q,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-03,Q,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-03,Q,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-03,Q,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-03,Q,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-03,Q,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-03,Q,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-03,Q,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-03,Q,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-03,Q,100000"),
                    new FlatFileDataRow("GBP/USD,2010-02-03,Q,100000"),
                    new FlatFileDataRow("SGD/USD,2010-02-03,Q,100000")
                };

            expectedAggregated = new[]
                {
                    "[CurrencyPair=GBP/USD, Date=2010-02-01, Amount=2600000]",
                    "[CurrencyPair=EUR/USD, Date=2010-02-01, Amount=300000]",
                    "[CurrencyPair=GBP/USD, Date=2010-02-02, Amount=2400000]",
                    "[CurrencyPair=EUR/USD, Date=2010-02-02, Amount=1500000]",
                    "[CurrencyPair=GBP/USD, Date=2010-02-03, Amount=1000000]",
                    "[CurrencyPair=SGD/USD, Date=2010-02-03, Amount=100000]"
                };
        }

        [Test]
        public void TestThatAggregatorWorks()
        {
            var keyFields = new[] {"CurrencyPair", "Date"};
            var valueField = "Amount";
            var aggregator = new FileAggregator();

            var results = aggregator.AggregateRows<long>(header, dataRows, keyFields, valueField);

            var output = new List<string>();
            foreach (var res in results)
            {
                output.Add(res.ToString());
            }

            Assert.AreEqual(expectedAggregated, output.ToArray());
        }
    }
}