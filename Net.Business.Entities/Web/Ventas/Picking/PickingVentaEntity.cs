using System;
using System.Collections.Generic;

namespace Net.Business.Entities.Web
{
    public class PickingVentaEntity
    {
        public int IdPicking { get; set; }
        public string NumPicking { get; set; }
        public DateTime FecPicking { get; set; }
        public string CodTipoPicking { get; set; }
        public string CodEstado { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string LicTradNum { get; set; }
        public string Comentarios { get; set; }
        public int IdUsuario { get; set; }
        public List<PickingVentaItemByIdPickingEntity> Item { get; set; }
    }

    public class PickingVentaByFiltroEntity
    {
        public int IdPicking { get; set; }
        public string NumPicking { get; set; }
        public DateTime FecPicking { get; set; }
        public string NomTipoPicking { get; set; }
        public string CodEstado { get; set; }
        public string NomEstado { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
    }

    public class PickingVentaByIdPicking
    {
        public int IdPicking { get; set; }
        public string NumPicking { get; set; }
        public DateTime FecPicking { get; set; }
        public string CodTipoPicking { get; set; }
        public string CodEstado { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string LicTradNum { get; set; }
        public string Comentarios { get; set; }
        public List<PickingVentaItemByIdPickingEntity> Item { get; set; }
    }

    public class PickingVentaForEntregaByIdPicking
    {
        public int IdPicking { get; set; }
        public string CardCode { get; set; }
        public string CurrCode { get; set; }
        public string FullCurrName { get; set; }
        public decimal Rate { get; set; }
        public decimal DocRate { get; set; }
        public int SlpCode { get; set; }
        public string SlpName { get; set; }
        public decimal Peso { get; set; }
        public string ShipToCode { get; set; }
        public string Address2 { get; set; }
        public string PayToCode { get; set; }
        public string Address { get; set; }
        public string CodMotTraslado { get; set; }
        public string NomMotTraslado { get; set; }
        public int GroupNum { get; set; }
        public string PymntGroup { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TotalImpuesto { get; set; }
        public decimal Total { get; set; }
        public List<PickingVentaItemForEntregaByIdPickingEntity> Item { get; set; }
    }
}
