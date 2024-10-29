using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Net.Business.Entities.Sap
{
    [Table("OLCT")]
    public class SedeSapEntity
    {
        [Key]
        public int Code { get; set; }
        public string Location { get; set; }
    }
}
