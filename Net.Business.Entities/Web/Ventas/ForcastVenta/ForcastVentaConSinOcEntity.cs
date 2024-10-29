using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Net.Business.Entities.Web
{
    [Table("ForcastVentaConSinOc")]
    public class ForcastVentaConSinOcEntity
    {
        [Key]
        public int IdConSinOc { get; set; }
        public string NomConSinOc { get; set; }
    }
}
