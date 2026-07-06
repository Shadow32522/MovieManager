using Microsoft.EntityFrameworkCore;
using MovieManager.DAL.Entities;

namespace MovieManager.DAL.Data
{
    public class MovieDbContext : DbContext
    {
        public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<MovieActor> MovieActors { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MovieActor>()
                .HasKey(ma => new { ma.MovieId, ma.ActorId });

            modelBuilder.Entity<MovieActor>()
                .HasOne(ma => ma.Movie)
                .WithMany(m => m.MovieActors)
                .HasForeignKey(ma => ma.MovieId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<MovieActor>()
                .HasOne(ma => ma.Actor)
                .WithMany(a => a.MovieActors)
                .HasForeignKey(ma => ma.ActorId)
                .OnDelete(DeleteBehavior.Cascade);

            
            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasOne(r => r.Movie)
                    .WithMany(m => m.Reviews)
                    .HasForeignKey(r => r.MovieId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.Property(r => r.ReviewerName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(r => r.Comment)
                    .HasMaxLength(1000);

                entity.ToTable("Reviews", t => t.HasCheckConstraint("CK_Review_Score", "[Score] >= 1 AND [Score] <= 10"));
            });

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.Property(m => m.Title)
                    .IsRequired()
                    .HasMaxLength(200);
                
                entity.HasOne<Genre>()
                    .WithMany()
                    .HasForeignKey("GenreId")
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<Director>()
                    .WithMany()
                    .HasForeignKey("DirectorId")
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.Property(g => g.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasIndex(g => g.Name)
                    .IsUnique();
            });

            modelBuilder.Entity<Director>(entity =>
            {
                entity.Property(d => d.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(d => d.LastName)
                    .IsRequired()
                    .HasMaxLength(100);
            });
            
            modelBuilder.Entity<Actor>(entity =>
            {
                entity.Property(a => a.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(a => a.LastName)
                    .IsRequired()
                    .HasMaxLength(100);
            });
        }
    }
}