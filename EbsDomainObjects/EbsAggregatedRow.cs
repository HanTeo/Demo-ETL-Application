namespace EbsDomainObjects
{
    /// <summary>
    /// Domain specific Data Transfer Object that is consumed by ORM layer
    /// </summary>
    public struct EbsAggregatedRow<T>
    {
        public string CurrencyPair { get; set; }
        public string Date { get; set; }
        public T Amount { get; set; }

        public override string ToString()
        {
            return string.Format("[CurrencyPair={0}, Date={1}, Amount={2}]", CurrencyPair, Date, Amount);
        }
    }
}