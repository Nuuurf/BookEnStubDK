using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestfulApi.DAL;
using TestProject.API.DAO;

namespace TestProject.API {
    public class ConnectionTest {

        [Test]
        public void CanConnectionBeEtablished_ShouldBeOpen() {
            Assert.That(DBConnection.Instance.TryConnection()[0], Is.EqualTo(ConnectionState.Open));
        }

    }
}
