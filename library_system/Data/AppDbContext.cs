using System;
using Microsoft.EntityFrameworkCore;
using library_system.Models;

public class AppDbContext : DbContext
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<BookAuthor> BookAuthor { get; set; }
    public DbSet<Loan> Loans { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost;Database=library_systemDB;Integrated Security=False;User Id=sa;Password=Abcd1234!;TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>()     // M:M relation with books-authors using a bridge table BookAuthor
            .HasMany(b => b.Authors)    // (each has a 1:M relation to BookAuthor).
            .WithMany(a => a.Books)
            .UsingEntity<BookAuthor>();

        modelBuilder.Entity<Book>()
            .HasMany(b => b.Loans)
            .WithOne(l => l.Book);
        
        modelBuilder.Entity<Loan>()
            .Property(l => l.status)
            .HasConversion<string>();
    }
}