using DSS_API.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace TodoApi.Models;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options)
    : base(options)
    {
    }

    public DbSet<User> User { get; set; } = null!;

    public DbSet<Article> Article { get; set; } = null!;

    public DbSet<Comment> Comment { get; set; } = null!;



}