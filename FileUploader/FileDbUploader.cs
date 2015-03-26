using System.Data;
using System.Collections.Generic;
using Dapper;
using EbsDomainObjects;

namespace FileUploader
{
    /// <summary>
    /// Uploads aggregated data rows into DB
    /// Assumes: 3 columns CurrencyPair, Date, Amount in Target Table
    /// </summary>
    public class FileDbUploader
    {
        public string UpsertCommand =
            @"MERGE 
				   {0} AS target
				USING 
				   (select @CurrencyPair as CurrencyPair, @Date as Date, @Amount as Amount) AS source
				ON 
				   target.CurrencyPair = source.CurrencyPair 
				AND
				   target.Date = source.Date 
				   
				WHEN MATCHED THEN 
				   UPDATE SET target.Amount = target.Amount + source.Amount
				WHEN NOT MATCHED THEN 
				   INSERT (CurrencyPair, Date, Amount) VALUES (@CurrencyPair, @Date, @Amount)
				; ";

        /// <summary>
        /// Upload the specified Aggregated DataRows - EbsAggregateRow to a specified DataTable in DB
        /// </summary>
        /// <param name="connection">IDbConnection</param>
        /// <param name="dataTableName">Data table name.</param>
        /// <param name="dataRows">Data rows</param>
        public int Upload(IDbConnection connection, string dataTableName,
                          IEnumerable<EbsAggregatedRow<decimal>> dataRows)
        {
            connection.Open();
            var sql = string.Format(UpsertCommand, dataTableName);
            return connection.Execute(sql, dataRows);
        }
    }
}