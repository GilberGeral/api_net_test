using Microsoft.EntityFrameworkCore;
using Coink.Models;

namespace CoinkApi.Data{
  
  public class AppDbContext : DbContext    {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Departamento> Departamentos { get; set; }
    public DbSet<Municipio> Municipios { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
  }//fin de la clase

}//fin del namespace