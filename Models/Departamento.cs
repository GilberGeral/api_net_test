using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coink.Models{
  [Table("departamentos")]
  public class Departamento {
    [Key]
    [Column("id_departamento")]
    public int Id { get; set; }

    [Column("id_pais")]
    public int IdPais { get; set; }

    [Column("nombre")]
    public string Nombre { get; set; } = string.Empty;
  }//fin de la clase

}//find el namespace
