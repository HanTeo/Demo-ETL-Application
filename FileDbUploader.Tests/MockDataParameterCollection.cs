using System;
using System.Data;
using System.Collections.Generic;

namespace FileDbUploader.Tests
{
    public class MockDataParameterCollection : IDataParameterCollection
    {
        private List<object> parameters = new List<object>();

        #region IDataParameterCollection implementation

        void IDataParameterCollection.RemoveAt(string parameterName)
        {
            throw new NotImplementedException();
        }

        int IDataParameterCollection.IndexOf(string parameterName)
        {
            throw new NotImplementedException();
        }

        bool IDataParameterCollection.Contains(string parameterName)
        {
            throw new NotImplementedException();
        }

        object IDataParameterCollection.this[string index]
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        #endregion

        #region IList implementation

        int System.Collections.IList.Add(object value)
        {
            parameters.Add(value);
            return 1;
        }

        void System.Collections.IList.Clear()
        {
            parameters.Clear();
        }

        bool System.Collections.IList.Contains(object value)
        {
            throw new NotImplementedException();
        }

        int System.Collections.IList.IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        void System.Collections.IList.Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        void System.Collections.IList.Remove(object value)
        {
            throw new NotImplementedException();
        }

        void System.Collections.IList.RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        bool System.Collections.IList.IsFixedSize
        {
            get { throw new NotImplementedException(); }
        }

        bool System.Collections.IList.IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        object System.Collections.IList.this[int index]
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        #endregion

        #region ICollection implementation

        void System.Collections.ICollection.CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        int System.Collections.ICollection.Count
        {
            get { throw new NotImplementedException(); }
        }

        bool System.Collections.ICollection.IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        object System.Collections.ICollection.SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IEnumerable implementation

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return parameters.GetEnumerator();
        }

        #endregion
    }
}