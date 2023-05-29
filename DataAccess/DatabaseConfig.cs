
namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    public class DatabaseConfig : IDatabaseConfig
    {
        public DatabaseConfig(string connValue)
        {
            this.ConnectionString = connValue;
        }

        public string ConnectionString { get; }
    }
}
