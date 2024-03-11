using ItaliaTreni.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ItaliaTreni.Infrastructure.EntityTypeBuilders;

public sealed class FileDataEntityTypeBuilder : IEntityTypeConfiguration<FileData>
{
    public void Configure(EntityTypeBuilder<FileData> builder)
    {
        builder.ToTable("FileDatas");

        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.HasKey(x => x.Id);

        builder.Property(x => x.MM)
            .IsRequired();

        builder.Property(x => x.P1)
            .IsRequired();

        builder.Property(x => x.P2)
            .IsRequired();

        builder.Property(x => x.P3)
            .IsRequired();

        builder.Property(x => x.P4)
            .IsRequired();
    }
}
