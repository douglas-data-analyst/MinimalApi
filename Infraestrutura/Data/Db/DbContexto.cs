namespace Infraestrutura.Data.Db;

    public class DbContexto
    {
        public string ConnectionString { get; set; }

        public DbContexto(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }
   
