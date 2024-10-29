using System;
using Net.Business.Entities.Web;
namespace Net.Business.DTO.Web
{
    public class LocalUpdateDto
    {
        public int NumLocal { get; set; }
        public string NomLocal { get; set; }
        public Boolean EsOriente { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public int? IdUsuarioUpdate { get; set; } = null;

        public LocalEntity ReturnValue()
        {
            return new LocalEntity
            {
                NumLocal = this.NumLocal,
                NomLocal = this.NomLocal,
                EsOriente = this.EsOriente,
                CardCode = this.CardCode,
                CardName = this.CardName,
                IdUsuarioUpdate = this.IdUsuarioUpdate,
            };
        }
    }
}
