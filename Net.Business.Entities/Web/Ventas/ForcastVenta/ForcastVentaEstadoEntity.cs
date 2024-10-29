using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Net.Business.Entities.Web
{
    [Table("ForcastVentaEstado")]
    public class ForcastVentaEstadoEntity
    {
        [Key]
        public string CodEstado { get; set; }
        public string NomEstado { get; set; }
    }
}
