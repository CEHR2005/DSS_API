using DSS_API.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace TodoApi.Models;

public class TodoContext : DbContext
{
    public TodoContext(DbContextOptions<TodoContext> options)
    : base(options)
    {
    }

    public DbSet<TodoItem> TodoItems { get; set; } = null!;
    public DbSet<User> User { get; set; } = null!;

    public DbSet<Article> Article { get; set; } = null!;

    public DbSet<Comment> Comment { get; set; } = null!;



}