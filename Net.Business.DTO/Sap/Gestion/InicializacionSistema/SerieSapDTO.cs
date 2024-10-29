using System;
namespace Net.Business.DTO.Sap
{
    public class SerieSapDTO
    {
        public Int16 Series { get; set; }
        public string ObjectCode { get; set; }
        public string Documento { get; set; }
        public string SeriesName { get; set; }
        public int NextNumber { get; set; }
    }
}
