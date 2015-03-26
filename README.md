# flat-file-etl-application
ETL Application for loading flat file into Database with On The Fly Aggregation

[EbsDomainObject] is the domain specific dll that contains all the domain objects

[EbsFileETLRunner] is a console application that allows a file to be Extracted, Transformed and Loaded into a database

[Components of the ETL]
FileImporter - Extracts the contents of the files into rows that can be aggregated. Optional Filters Apply
FileAggregator - Aggregates the data row according to supplied Keys and Value columns
FileUploader - Upserts the Aggregated Data into SQL Database. It will also aggregate the value if an existing record that matches the criteria is found.

[Unit Tests]
Usage patterns and unit tests can be found for the above modules
- FileImporters.Tests
- FileAggregator.Tests
- FileDbUploader.Tests

