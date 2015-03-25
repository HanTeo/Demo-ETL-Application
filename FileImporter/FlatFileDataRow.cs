namespace FileImporter
{
    public class FlatFileDataRow
    {
        public FlatFileDataRow(string rawRowData)
        {
            Columns = rawRowData.Split(new[] {','});
        }

        public string[] Columns { get; private set; }
    }
}