using System;
using System.Data;
using System.Text;

namespace FileDbUploader.Tests
{
    public class MockDbCommand : IDbCommand
    {
        private string commandText;
        private CommandType commandType;
        private IDataParameterCollection parameters = new MockDataParameterCollection();
        public StringBuilder ExecutedCommands = new StringBuilder();

        #region IDbCommand implementation

        void IDbCommand.Cancel()
        {
            throw new NotImplementedException();
        }

        IDbDataParameter IDbCommand.CreateParameter()
        {
            return new MockDbDataParameter();
        }

        int IDbCommand.ExecuteNonQuery()
        {
            var processedCommand = commandText;
            foreach (IDbDataParameter parameter in parameters)
            {
                if (parameter.DbType != DbType.String)
                {
                    processedCommand = processedCommand.Replace("@" + parameter.ParameterName,
                                                                parameter.Value.ToString());
                }
                else
                {
                    processedCommand = processedCommand.Replace("@" + parameter.ParameterName,
                                                                string.Format("'{0}'", parameter.Value));
                }
            }
            ExecutedCommands.AppendLine(processedCommand);
            return 1;
        }

        IDataReader IDbCommand.ExecuteReader()
        {
            throw new NotImplementedException();
        }

        IDataReader IDbCommand.ExecuteReader(CommandBehavior behavior)
        {
            throw new NotImplementedException();
        }

        object IDbCommand.ExecuteScalar()
        {
            throw new NotImplementedException();
        }

        void IDbCommand.Prepare()
        {
            throw new NotImplementedException();
        }

        string IDbCommand.CommandText
        {
            get { return commandText; }
            set { commandText = value; }
        }

        int IDbCommand.CommandTimeout
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        CommandType IDbCommand.CommandType
        {
            get { return commandType; }
            set { commandType = value; }
        }

        IDbConnection IDbCommand.Connection
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        IDataParameterCollection IDbCommand.Parameters
        {
            get { return parameters; }
        }

        IDbTransaction IDbCommand.Transaction
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        UpdateRowSource IDbCommand.UpdatedRowSource
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        #endregion

        #region IDisposable implementation

        void IDisposable.Dispose()
        {
        }

        #endregion
    }
}