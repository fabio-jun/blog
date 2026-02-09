using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Data.Configurations;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        //Table name
        builder.ToTable("Posts");

        //PK
        builder.HasKey(p => p.Id);

        //Properties
        builder.Property(p => p.Title)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(p => p.Content)
            .IsRequired()
            .HasColumnType("TEXT");
        
        builder.Property(p => p.Slug)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(p => p.IsPublished)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(p => p.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

         builder.Property(p => p.UpdatedAt)
              .IsRequired(false);  // Nullable

          builder.HasIndex(p => p.Slug)
              .IsUnique()
              .HasDatabaseName("IX_Posts_Slug");

          //Post->User (Many-to-One)
          builder.HasOne(p => p.Author) //One post has one author
              .WithMany(u => u.Posts)   //One user has many posts
              .HasForeignKey(p => p.AuthorId)     
              .OnDelete(DeleteBehavior.Restrict);


        
    }
}

