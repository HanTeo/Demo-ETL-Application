using System;
using System.Data;

namespace FileDbUploader.Tests
{
    public class MockDbConnection : IDbConnection
    {
        private ConnectionState state;
        public MockDbCommand Command;

        public MockDbConnection(string connectionString)
        {
        }

        #region IDbConnection implementation

        IDbTransaction IDbConnection.BeginTransaction()
        {
            throw new NotImplementedException();
        }

        IDbTransaction IDbConnection.BeginTransaction(IsolationLevel il)
        {
            throw new NotImplementedException();
        }

        void IDbConnection.ChangeDatabase(string databaseName)
        {
            throw new NotImplementedException();
        }

        void IDbConnection.Close()
        {
            state = ConnectionState.Closed;
        }

        IDbCommand IDbConnection.CreateCommand()
        {
            Command = new MockDbCommand();

            return Command;
        }

        void IDbConnection.Open()
        {
            state = ConnectionState.Open;
        }

        string IDbConnection.ConnectionString { get; set; }

        int IDbConnection.ConnectionTimeout
        {
            get { throw new NotImplementedException(); }
        }

        string IDbConnection.Database
        {
            get { throw new NotImplementedException(); }
        }

        ConnectionState IDbConnection.State
        {
            get { return state; }
        }

        #endregion

        #region IDisposable implementation

        void IDisposable.Dispose()
        {   
        }

        #endregion
    }
}