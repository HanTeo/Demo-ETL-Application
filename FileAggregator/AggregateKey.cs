using System;

namespace FileAggregator
{
    /// <summary>
    /// Composite Key for aggregation implements strongly typed Equals Method
    /// </summary>
    public class AggregateKey : IEquatable<AggregateKey>
    {
        public string CompositeKey { get; private set; }
        public string[] FieldNames { get; private set; }

        public AggregateKey(string[] fieldNames)
        {
            CompositeKey = string.Join("#", fieldNames);
            FieldNames = fieldNames;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                throw new ArgumentException("obj");

            return Equals(obj as AggregateKey);
        }

        public override int GetHashCode()
        {
            return CompositeKey.GetHashCode();
        }

        #region IEquatable implementation

        bool IEquatable<AggregateKey>.Equals(AggregateKey other)
        {
            if (other == null)
                throw new ArgumentException("other");

            return CompositeKey == other.CompositeKey;
        }

        #endregion
    }
}