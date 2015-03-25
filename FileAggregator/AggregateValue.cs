namespace FileAggregator
{
    /// <summary>
    /// Generic Adder Class that is used by File Aggregator to perform aggregation
    /// </summary>
    public class AggregateValue<T>
    {
        public T CurrentValue { get; private set; }

        public AggregateValue()
        {
        }

        public AggregateValue(T value) : this()
        {
            CurrentValue = value;
        }

        public void Add(T value)
        {
            dynamic a = CurrentValue;
            dynamic b = value;

            CurrentValue = a + b;
        }
    }
}