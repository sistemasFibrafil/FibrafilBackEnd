using System;
namespace Net.Business.Entities.Web
{
    public class LocalEntity
    {
        public int NumLocal { get; set; }
        public string NomLocal { get; set; }
        public Boolean EsOriente { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public int? IdUsuarioCreate { get; set; } = null;
        public int? IdUsuarioUpdate { get; set; } = null;
    }
}
