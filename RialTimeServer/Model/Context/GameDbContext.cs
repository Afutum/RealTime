﻿using Microsoft.EntityFrameworkCore;
using RialTimeServer.Model.Entity;

namespace RialTimeServer.Model.Context
{
    public class GameDbContext:DbContext
    {
        public DbSet<User> Users { get; set; }

        readonly string connectionString = "server=localhost;database=realtime_game;user=jobi;password=jobi;";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0)));
        }
    }
}
