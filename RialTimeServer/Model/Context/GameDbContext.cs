using Microsoft.EntityFrameworkCore;
using RialTimeServer.Model.Entity;

namespace RialTimeServer.Model.Context
{
    public class GameDbContext:DbContext
    {
        public DbSet<User> Users { get; set; }

#if DEBUG
        //ローカルPCで実行時の接続先
        readonly string connectionString = "server=localhost;database=realtime_game;user=jobi;password=jobi;";

#else
       //AzureVMで実行時の接続先
       readonly string connectionString = "server=soccergame.japaneast.cloudapp.azure.com;database=realtime_game;user=student;password=Yoshidajobi2023;";

#endif
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0)));
        }
    }
}
