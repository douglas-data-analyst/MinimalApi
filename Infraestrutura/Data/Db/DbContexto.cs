using Microsoft.EntityFrameworkCore;

namespace Infraestrutura.Data.Db;

    public class DbContexto : DbContext
    {
        public string ConnectionString { get; set; }

        public DbContexto(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }
   
