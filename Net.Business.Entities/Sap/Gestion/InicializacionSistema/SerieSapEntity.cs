using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Net.Business.Entities.Sap
{
    [Table("NNM1")]
    public class SerieSapEntity
    {
        [Key]
        public Int16 Series { get; set; }
        public string ObjectCode { get; set; }
        public string Documento { get; set; }
        public string SeriesName { get; set; }
        public int NextNumber { get; set; }
    }
}
