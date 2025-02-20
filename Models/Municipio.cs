using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coink.Models{
  [Table("municipios")]
  public class Municipio {
    [Key]
    [Column("id_municipio")]
    public int IdMunicipio { get; set; }

    [Column("id_departamento")]
    public int IdDepartamento { get; set; }

    [Column("nombre")]
    public string Nombre { get; set; } = string.Empty;
  }//fin de la clase

}//find el namespace