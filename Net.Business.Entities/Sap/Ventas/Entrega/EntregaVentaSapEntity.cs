using System;
using System.Collections.Generic;

namespace Net.Business.Entities.Sap
{
    public class EntregaVentaSapEntity
    {

        public int IdEntregaVenta { get; set; }
        public int DocEntry { get; set; }
        public int Series { get; set; }
        public int DocNum { get; set; }
        public string DocStatus { get; set; }
        public string SerieSunat { get; set; }
        public string NumeroSunat { get; set; }
        public DateTime DocDate { get; set; }
        public DateTime DocDueDate { get; set; }
        public DateTime TaxDate { get; set; }

        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string LicTradNum { get; set; }
        public string DocCur { get; set; }
        public double DocRate { get; set; }

        public string ShipToCode { get; set; }
        public string Address2 { get; set; }
        public string PayToCode { get; set; }
        public string Address { get; set; }
        public string CodMotTraslado { get; set; }
        public string OtrMotTraslado { get; set; }

        public int CodCondicionPago { get; set; }

        public string RucDesInternacional { get; set; }
        public string DesGuiaInternacional { get; set; }
        public string DirDesInternacional { get; set; }
        public string NumContenedor { get; set; }
        public string NumPrecinto01 { get; set; }
        public string NumPrecinto02 { get; set; }
        public string NumPrecinto03 { get; set; }
        public string NumPrecinto04 { get; set; }

        //Transportista
        public string CodTipTransporte { get; set; }
        //Transportista 1
        public bool ManTransportista1 { get; set; }
        public string CodTransportista1 { get; set; }
        public string CodTipDocIdeTransportista1 { get; set; }
        public string RucTransportista1 { get; set; }
        public string NomTransportista1 { get; set; }
        public string NumPlaca1 { get; set; }

        public string CodTipDocIdeConductor1 { get; set; }
        public string NumDocIdeConductor1 { get; set; }
        public string DenConductor1 { get; set; }
        public string NomConductor1 { get; set; }
        public string ApeConductor1 { get; set; }
        public string LicConductor1 { get; set; }
        //Transportista 2
        public bool ManTransportista2 { get; set; }
        public string CodTransportista2 { get; set; }
        public string RucTransportista2 { get; set; }
        public string NomTransportista2 { get; set; }
        public string DirTransportista2 { get; set; }

        public int SlpCode { get; set; }
        public double TotalBulto { get; set; }
        public double TotalKilo { get; set; }
        public string Comments { get; set; }

        public double VatSum { get; set; }
        public double VatSumFC { get; set; }
        public double VatSumy { get; set; }

        public double DocTotal { get; set; }
        public double DocTotalFC { get; set; }
        public double DocTotalSy { get; set; }
        public int IdUsuario { get; set; }

        public List<EntregaVentaItemSapEntity> Item { get; set; } = new List<EntregaVentaItemSapEntity>();
    }

    public class GuiaDespachoMercaderiaSapByFechaSedeEntity
    {
        public string Tipo { get; set; }
        public int? NumeroPedido { get; set; }
        public string NumeroGuiaSUNAT { get; set; }
        public DateTime DocDate { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string AlmacenOrigen { get; set; }
        public string AlmacenDestino { get; set; }
        public decimal Bulto { get; set; }
        public decimal TotalKg { get; set; }
        public decimal Quantity { get; set; }
        public string NumeroFcturaSUNAT { get; set; }
        public string NomTransportista { get; set; }
        public string RucTransportista { get; set; }
        public string PlacaTransportista { get; set; }
        public string NomConductor { get; set; }
        public string LincenciaConductor { get; set; }
    }
}
