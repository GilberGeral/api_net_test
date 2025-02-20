using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coink.Models{
  [Table("usuarios")]
  public class Usuario {
    [Key]
    [Column("id_user")]
    public int IdUsuario { get; set; }

    [Column("id_mask")]
    public string IdMask { get; set; } = string.Empty;

    [Column("nombre")]
    public string Nombre { get; set; } = string.Empty;
    
    [Column("telefono")]
    public string Telefono { get; set; } = string.Empty;
    
    [Column("direccion")]
    public string Direccion { get; set; } = string.Empty;
    
    [Column("municipio")]
    public int Municipio { get; set; }

  }//fin de la clase

}//find el namespace