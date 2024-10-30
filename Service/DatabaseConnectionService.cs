using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module05Exercise01.Service
{
    public class DatabaseConnectionService
    {
        private readonly string _connectiongString;

        public DatabaseConnectionService()
        {
            _connectiongString = "Server=localhost; Database=CompanyDB; User ID=testuser; Password=testuser";
        }

        public string GetConnectionString()
        {
            return _connectiongString;
        }
    }
}
