using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Net.Business.Entities.Sap
{
    [Table("OCRG")]
    public class GrupoSocioNegocioSapEntity
    {
        [Key]
        public Int16 GroupCode { get; set; }
        public string GroupName { get; set; }
        public string GroupType { get; set; }
    }
}
