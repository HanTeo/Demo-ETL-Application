using System;
using System.Linq;
using FileImporter;
using System.Collections.Generic;
using EbsDomainObjects;

namespace FileAggregator
{
    /// <summary>
    /// File aggregator takes the output from FileImporter and aggregates the value column by the fields specified
    /// </summary>
    public class FileAggregator
    {
        /// <summary>
        /// Aggregates the rows.
        /// </summary>
        /// <returns>Sequence of EbsAggregateRow (POCO) that can be consumed by the FileDbUploader</returns>
        /// <param name="header">Header.</param>
        /// <param name="dataRows">Data rows.</param>
        /// <param name="columnsToAggregateBy">Keys which to aggregate by</param>
        /// <param name="columnToAggregate">Field on which to perform the aggregation</param>
        /// <typeparam name="T">DataType for the value field. Supports int, long, float, double, decimal</typeparam>
        public IEnumerable<EbsAggregatedRow<T>> AggregateRows<T>
            (
            FlatFileHeader header,
            IEnumerable<FlatFileDataRow> dataRows,
            string[] columnsToAggregateBy,
            string columnToAggregate
            )
        {
            var aggregate = new Dictionary<AggregateKey, AggregateValue<T>>();

            // Aggregate the value field by the specified keys
            foreach (var dataRow in dataRows)
            {
                var key = ExtractAggregateKeyFromDataRow(columnsToAggregateBy, header, dataRow);
                var val = ExtractAggregateValueFromDataRow<T>(columnToAggregate, header, dataRow);

                AggregateValue<T> existingAggregateValue;
                if (aggregate.TryGetValue(key, out existingAggregateValue))
                {
                    existingAggregateValue.Add(val);
                }
                else
                {
                    aggregate[key] = new AggregateValue<T>(val);
                }
            }

            // Generate the POCO sequence
            return aggregate.Select(agg => new EbsAggregatedRow<T>
                {
                    CurrencyPair = agg.Key.FieldNames[0],
                    Date = agg.Key.FieldNames[1],
                    Amount = agg.Value.CurrentValue
                });
        }


        // Builds the aggregate key from data row.
        private static AggregateKey ExtractAggregateKeyFromDataRow(string[] keyFieldNames, FlatFileHeader header,
                                                            FlatFileDataRow dataRow)
        {
            var fieldValues = new string[keyFieldNames.Length];
            var i = 0;
            foreach (var keyField in keyFieldNames)
            {
                fieldValues[i] = dataRow.Columns[header.HeaderColumnIndex[keyField]];
                i++;
            }

            return new AggregateKey(fieldValues);
        }

        // Extracts the aggregate value from data row. Generically typed so that aggregate value can be specified
        private static T ExtractAggregateValueFromDataRow<T>(string valueFieldName, FlatFileHeader header,
                                                      FlatFileDataRow dataRow)
        {
            var str = dataRow.Columns[header.HeaderColumnIndex[valueFieldName]];

            switch (typeof (T).ToString())
            {
                case "System.Int32":
                    return (T) Convert.ChangeType(int.Parse(str), typeof (T));
                case "System.Int64":
                    return (T) Convert.ChangeType(long.Parse(str), typeof (T));
                case "System.Single":
                    return (T) Convert.ChangeType(float.Parse(str), typeof (T));
                case "System.Double":
                    return (T) Convert.ChangeType(double.Parse(str), typeof (T));
                case "System.Decimal":
                    return (T) Convert.ChangeType(decimal.Parse(str), typeof (T));
                default:
                    throw new ArgumentException(string.Format("Type Not Supported {0}", typeof (T)));
            }
        }
    }
}