using System;

namespace Net.Business.Entities.Sap
{
    public class OrdenVentaSapEntity
    {
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
    }
    
    public class OrdenVentaSapByFechaEntity
    {
        public string CodTipDocumento { get; set; }
        public string NomTipDocumento { get; set; }
        public int NumeroDocumento { get; set; }
        public int NumeroPedido { get; set; }
        public string NumeroOrdenVenta { get; set; }
        public int? NumeroFactura { get; set; } = null;
        public int LineNum { get; set; }
        public DateTime DocDate { get; set; }
        public DateTime DocDueDate { get; set; }
        public DateTime TaxDate { get; set; }
        public DateTime CreateDate { get; set; }
        public string LineStatus { get; set; }
        public string CodStatus { get; set; }
        public string NomStatus { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string Pais { get; set; }
        public string Departamento { get; set; }
        public string Provincia { get; set; }
        public string Ciudad { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int CodGrupoArticulo { get; set; }
        public string NomGrupoArticulo { get; set; }
        public string NomSubGrupoArticulo { get; set; }
        public string NomSubGrupoArticulo2 { get; set; }
        public string Medida { get; set; }
        public string Color { get; set; }
        public string WhsCode { get; set; }
        public string WhsName { get; set; }
        public string BuyUnitMsr { get; set; }
        public string SalUnitMsr { get; set; }
        public string InvntryUom { get; set; }
        public decimal Stock { get; set; }
        public decimal Pendiente { get; set; }
        public decimal Solicitado { get; set; }
        public decimal Disponible { get; set; }
        public decimal StockProduccion { get; set; }
        public decimal PendienteProduccion { get; set; }
        public decimal SolicitadoProduccion { get; set; }
        public decimal DisponibleProduccion { get; set; }
        public decimal Quantity { get; set; }
        public decimal RolloPedido { get; set; }
        public decimal KgPedido { get; set; }
        public decimal ToneladaPedida { get; set; }
        public decimal OpenQty { get; set; }
        public decimal RolloPendiente { get; set; }
        public decimal KgPendiente { get; set; }
        public decimal ToneladaPendiente { get; set; }
        public decimal DelivrdQty { get; set; }
        public string Currency { get; set; }
        public string DocCur { get; set; }
        public Decimal Rate { get; set; }
        public Decimal Price { get; set; }
        public Decimal LineTotal { get; set; }
        public Decimal TotalFrgn { get; set; }
        public Decimal TotalSumSy { get; set; }
        public decimal DocTotal { get; set; }
        public decimal DocTotalFC { get; set; }
        public decimal DocTotalSy { get; set; }
        public int SlpCode { get; set; }
        public string SlpName { get; set; }
        public string PymntGroup { get; set; }
        public string IdDivision { get; set; }
        public string Division { get; set; }
        public string IdSector { get; set; }
        public string Sector { get; set; }
        public decimal PesoPromedioKg { get; set; }
        public int DiasAntiguedad { get; set; }
        public int DiasAtraso { get; set; }
        public string DiasVenc { get; set; }
        public string OrigenCliente { get; set; }
        public string Sede { get; set; }


        /// <summary>
        /// Filtro
        /// </summary>
        public DateTime FecInicial { get; set; }
        public DateTime FecFinal { get; set; }
        public string GrupoCliente { get; set; }
        public string TipDocumento { get; set; }
        public string Status { get; set; }
    }

    public class OrdenVentaCsigSapEntity
    {
        public int DocNum { get; set; }
        public DateTime? ShipDate { get; set; }
        public string ItemName { get; set; }
        public string CardName { get; set; }
        public string NomTrasportista { get; set; }
        public string DirTransportista { get; set; }
        public decimal Cantidad { get; set; }
        public DateTime? FechaDespacho { get; set; }
        public int Status { get; set; }
        public int Cumplimiento { get; set; }
        public decimal PesoApox { get; set; }
    }

    public class OrdenVentaSodimacSapEntity
    {
        public int DocEntry { get; set; }
        public string NumAtCard { get; set; }
        public int DocNum { get; set; }
        public DateTime DocDate { get; set; }
        public DateTime DocDueDate { get; set; }
        public DateTime TaxDate { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public int CntctCode { get; set; }
        public string CntctName { get; set; }
        public string Address2 { get; set; }
    }
}
