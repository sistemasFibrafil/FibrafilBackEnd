using System;
using System.Text;
using System.Collections.Generic;

namespace Net.Business.Entities.Web
{
    public class OrdenMantenimientoEntity
    {
        public int IdOrdenMantenimiento { get; set; }
    }
    public class OrdenMantenimientoByFechaAndIdEstadoAndNumeroEntity
    {
        public int IdOrdenMantenimiento { get; set; }
        public DateTime FecInicio { get; set; }
        public DateTime FecFin { get; set; }
        public string HoraInicio { get; set; }
        public string HoraFin { get; set; }
        public string NomTipoServicio { get; set; }
        public string NomArea { get; set; }
        public string NomMaquina { get; set; }
        public string NomParte { get; set; }
        public string NomSubParte { get; set; }
        public string NomTecnico { get; set; }
        public string Descripcion { get; set; }
        public string ActividadRealizada { get; set; }
        public string OtrosDestalles { get; set; }
        public string NomSede { get; set; }
        public string NomSolicitante { get; set; }
        public string PuestoSolicitante { get; set; }
        public DateTime FechaEmision { get; set; }
        public string codEstado { get; set; }
        public string NomEstado { get; set; }
    }
}
