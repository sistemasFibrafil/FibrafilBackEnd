using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Net.Business.Entities.Sap
{
    [Table("@FIB_SECTOR")]
    public class SectorSocioNegocioSapEntity
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
