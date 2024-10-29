using Net.Business.Entities.Web;
using System;
namespace Net.Business.DTO.Web
{
    public class LocalCreateDto
    {
        public int NumLocal { get; set; }
        public string NomLocal { get; set; }
        public Boolean EsOriente { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public int? IdUsuarioCreate { get; set; } = null;

        public LocalEntity ReturnValue()
        {
            return new LocalEntity
            {
                NumLocal = this.NumLocal,
                NomLocal = this.NomLocal,
                EsOriente = this.EsOriente,
                CardCode = this.CardCode,
                CardName = this.CardName,
                IdUsuarioCreate = this.IdUsuarioCreate,
            };
        }
    }
}
