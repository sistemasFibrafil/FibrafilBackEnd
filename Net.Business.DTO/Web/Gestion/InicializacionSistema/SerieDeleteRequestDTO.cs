using Net.Business.Entities.Web;

namespace Net.Business.DTO.Web
{
    public class SerieDeleteRequestDTO
    {
        public int IdSerie { get; set; }

        public SerieEntity ReturnValue()
        {
            return new SerieEntity
            {
                IdSerie = this.IdSerie
            };
        }
    }
}
