using ADC.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADC.Persistence.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{

    public DbSet<UserEntity> Users { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        // Relaciones de las tablas.....


        base.OnConfiguring(optionsBuilder);
    }


}