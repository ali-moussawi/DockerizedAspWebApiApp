using Polly;
using System.Data;
using System.Data.Common;

namespace Subscriptionapi.Models
{
    public class CustomDb : DbConnection
    {
        private readonly DbConnection _innerConnection;
        private readonly Policy _retryPolicy;

        public CustomDb(DbConnection innerConnection)
        {
            _innerConnection = innerConnection;

        
            _retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

 

        public override string ConnectionString
        {
            get => _innerConnection.ConnectionString;
            set => _innerConnection.ConnectionString = value;
        }

        public override string Database => throw new NotImplementedException();

        public override string DataSource => throw new NotImplementedException();

        public override string ServerVersion => throw new NotImplementedException();

        public override ConnectionState State => throw new NotImplementedException();

        public override void ChangeDatabase(string databaseName)
        {
            throw new NotImplementedException();
        }

        public override void Close()
        {
            throw new NotImplementedException();
        }

        public override void Open()
        {
            throw new NotImplementedException();
        }

        public override Task OpenAsync(CancellationToken cancellationToken)
        {
            return _retryPolicy.ExecuteAsync(() => _innerConnection.OpenAsync(cancellationToken));
        }




        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            throw new NotImplementedException();
        }




        protected override DbCommand CreateDbCommand()
        {
            throw new NotImplementedException();
        }

     
    }

}