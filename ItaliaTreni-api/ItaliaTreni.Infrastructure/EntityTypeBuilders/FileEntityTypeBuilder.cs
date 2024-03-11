using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItaliaTreni.Infrastructure.EntityTypeBuilders;

public sealed class FileEntityTypeBuilder : IEntityTypeConfiguration<ItaliaTreni.Domain.Model.File>
{
    public void Configure(EntityTypeBuilder<ItaliaTreni.Domain.Model.File> builder)
    {
        builder.ToTable("Files");

        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.InsertDate)
            .IsRequired();

        builder.HasMany(x => x.FileDatas)
            .WithOne();
    }
}
