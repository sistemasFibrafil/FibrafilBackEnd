using System;
using System.Collections.Generic;
namespace Net.Business.Entities.Web
{
    public class SerieEntity
    {
        public SerieEntity()
        {
            Item = new List<SerieItemEntity>();
        }

        public int IdSerie { get; set; }
        public int Linea { get; set; }
        public int IdSede { get; set; }
        public string ObjectCode { get; set; }
        public string Documento { get; set; }
        public Int16 Series { get; set; }
        public string SeriesName { get; set; }
        public int NextNumber { get; set; }
        public string SerieSunat { get; set; }
        public string NumeroSunat { get; set; }
        public List<SerieItemEntity> Item { get; set; }
    }

    public class SerieItemEntity
    {
        public int IdSerie { get; set; }
        public int IdSede { get; set; }
        public string ObjectCode { get; set; }
        public string Documento { get; set; }
        public Int16 Series { get; set; }
        public string SeriesName { get; set; }
        public int NextNumber { get; set; }
        public string SerieSunat { get; set; }
        public string NumeroSunat { get; set; }
    }
}
