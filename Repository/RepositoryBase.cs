using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BahiKitaab.Repository
{
    public abstract class RepositoryBase
    {
        private readonly string _connectionString;
        public RepositoryBase()
        {
#if DEBUG

            _connectionString = "Server=localhost;Uid=root;Pwd='';database=bahikitab";
#endif
#if RELEASE

            _connectionString = "Server=localhost;Uid=root;Pwd='';database=bahikitab";
#endif
        }

        protected MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}
