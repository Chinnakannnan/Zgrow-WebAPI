
namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    public interface IDatabaseConfig
    {
        string ConnectionString { get; }
    }
}
