using System;
using System.Data;

namespace FileDbUploader.Tests
{
    public class MockDbDataParameter : IDbDataParameter
    {
        private string parameterName;
        private DbType dbType;
        private ParameterDirection direction;
        private object parameterValue;
        private int parameterSize;

        #region IDbDataParameter implementation

        byte IDbDataParameter.Precision
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        byte IDbDataParameter.Scale
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        int IDbDataParameter.Size
        {
            get { return parameterSize; }
            set { parameterSize = value; }
        }

        #endregion

        #region IDataParameter implementation

        DbType IDataParameter.DbType
        {
            get { return dbType; }
            set { dbType = value; }
        }

        ParameterDirection IDataParameter.Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        bool IDataParameter.IsNullable
        {
            get { throw new NotImplementedException(); }
        }

        string IDataParameter.ParameterName
        {
            get { return parameterName; }
            set { parameterName = value; }
        }

        string IDataParameter.SourceColumn
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        DataRowVersion IDataParameter.SourceVersion
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        object IDataParameter.Value
        {
            get { return parameterValue; }
            set { parameterValue = value; }
        }

        #endregion
    }
}