using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace TestProject.API.Utilities
{
    public class PredefinedMocks
    {
        public static (Mock<IDbConnection> DbConnection, Mock<IDbTransaction> DbTransaction) ConnectionMocks()
        {
            var mockDbConnection = new Mock<IDbConnection>();
            var mockDbTransaction = new Mock<IDbTransaction>();

            mockDbConnection.Setup(conn => conn.BeginTransaction(It.IsAny<IsolationLevel>()))
                .Returns(mockDbTransaction.Object);
            mockDbTransaction.Setup(trans => trans.Commit());
            mockDbTransaction.Setup(trans => trans.Rollback());

            return (mockDbConnection, mockDbTransaction);
        }

    }
}
