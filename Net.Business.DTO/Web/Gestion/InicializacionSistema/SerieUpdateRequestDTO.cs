using Net.Business.Entities.Web;
using System;

namespace Net.Business.DTO.Web
{
    public class SerieUpdateRequestDTO
    {
        public int IdSerie { get; set; }
        public string SerieSunat { get; set; }
        public string NumeroSunat { get; set; }

        public SerieEntity ReturnValue()
        {
            return new SerieEntity
            {
                IdSerie = this.IdSerie,
                SerieSunat = this.SerieSunat,
                NumeroSunat = this.NumeroSunat,
            };
        }
    }
}
