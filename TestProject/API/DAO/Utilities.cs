using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.API.DAO {
    public class Utilities {

        public int GetMaxDBStubs() {

            int maxStubs = 10; //what we assume is default if connection fails

            using (IDbConnection con = DBConnection.Instance.GetOpenConnection()) {
                string script = "Select count(*) from Stub";

                maxStubs = con.Query<int>(script).FirstOrDefault();
            } 
                
            return maxStubs; 
        }
    }
}
