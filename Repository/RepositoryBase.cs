using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmetify.Repository
{
    public abstract class RepositoryBase
    {
        private readonly string _connectionString;
        public RepositoryBase()
        {
#if DEBUG

            //_connectionString = "DataSource=bahikitab-aws.c3s6wewcwox1.us-east-1.rds.amazonaws.com;Port=3306;Uid=admin;Pwd=Il6oOvguA2SB5IEQxWCJ;database=bahikitab";
            _connectionString = "Server=localhost;Uid=root;Pwd='';database=cosmetify";
#endif
#if RELEASE

            _connectionString = "Server=192.168.1.90;Uid=cosdb;Pwd=Cosmetify@123;database=cosmetify";
#endif
#if TESTING

            //_connectionString = "DataSource=bahikitab-aws.c3s6wewcwox1.us-east-1.rds.amazonaws.com;Port=3306;Uid=admin;Pwd=Il6oOvguA2SB5IEQxWCJ;database=bahikitab";
            _connectionString = "Server=localhost;Uid=root;Pwd='';database=cosmetify";
#endif
        }

        protected MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}
