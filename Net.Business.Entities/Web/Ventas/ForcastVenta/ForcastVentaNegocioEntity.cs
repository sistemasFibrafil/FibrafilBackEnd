using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Net.Business.Entities.Web
{
    [Table("ForcastVentaNegocio")]
    public class ForcastVentaNegocioEntity
    {
        [Key]
        public int IdNegocio { get; set; }
        public string NomNegocio { get; set; }
    }
}
