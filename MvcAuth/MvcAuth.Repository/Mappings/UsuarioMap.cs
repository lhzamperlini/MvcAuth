using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MvcAuth.Domain.Models;

namespace MvcAuth.Repository.Mappings;
public class UsuarioMap : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {

        builder.ToTable(nameof(Usuario));

    }
}
