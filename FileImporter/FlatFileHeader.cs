using System.Collections.Generic;

namespace FileImporter
{
    public class FlatFileHeader
    {
        public Dictionary<string, int> HeaderColumnIndex { get; private set; }

        public FlatFileHeader(string rawHeaderData)
        {
            var headerArray = rawHeaderData.Split(new[] {','});
            HeaderColumnIndex = new Dictionary<string, int>();

            var index = 0;
            foreach (var name in headerArray)
            {
                HeaderColumnIndex[name] = index;
                index++;
            }
        }
    }
}