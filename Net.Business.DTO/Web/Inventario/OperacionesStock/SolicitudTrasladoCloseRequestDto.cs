using Net.Business.Entities.Web;
namespace Net.Business.DTO.Web
{
    public class SolicitudTrasladoCloseRequestDto
    {
        public int IdSolicitudTraslado { get; set; }
        public int? IdUsuarioClose { get; set; } = null;

        public SolicitudTrasladoEntity ReturnValue()
        {
            return new SolicitudTrasladoEntity()
            {
                IdSolicitudTraslado = IdSolicitudTraslado,
                IdUsuarioClose = IdUsuarioClose,
            };
        }
    }
}
