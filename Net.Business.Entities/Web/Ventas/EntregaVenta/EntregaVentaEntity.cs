using System;
using System.Collections.Generic;
namespace Net.Business.Entities.Web
{
    public class EntregaVentaEntity
    {
        // CABECERA
        public int IdEntregaVenta { get; set; }
        public int DocEntry { get; set; }
        public string ObjType { get; set; }
        public int Series { get; set; }
        public int DocNum { get; set; }
        public string DocStatus { get; set; }
        public string TipDocSunat { get; set; }
        public string SerSunat { get; set; }
        public string NumSunat { get; set; }
        public DateTime DocDate { get; set; }
        public DateTime DocDueDate { get; set; }
        public DateTime TaxDate { get; set; }
        // CLIENTE
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string LicTradNum { get; set; }
        public string DocCur { get; set; }
        public double DocRate { get; set; }
        // LOGISTICA
        public string ShipToCode { get; set; }
        public string Address2 { get; set; }
        public string PayToCode { get; set; }
        public string Address { get; set; }
        public string CodMotTraslado { get; set; }
        public string OtrMotTraslado { get; set; }
        // FINANZAS
        public int CodCondicionPago { get; set; }
        // EXPORTACIÓN
        public string RucDesInternacional { get; set; }
        public string DesGuiaInternacional { get; set; }
        public string DirDesInternacional { get; set; }
        public string NumContenedor { get; set; }
        public string NumPrecinto1 { get; set; }
        public string NumPrecinto2 { get; set; }
        public string NumPrecinto3 { get; set; }
        public string NumPrecinto4 { get; set; }
        // TRANSPORTISTA
        public string CodTipTransporte { get; set; }
        // Transportista 1
        public bool ManTransportista1 { get; set; }
        public string CodTransportista1 { get; set; }
        public string CodTipDocIdeTransportista1 { get; set; }
        public string RucTransportista1 { get; set; }
        public string NomTransportista1 { get; set; }
        public string NumPlaca1 { get; set; }
        // Conductor
        public string CodTipDocIdeConductor1 { get; set; }
        public string NumDocIdeConductor1 { get; set; }
        public string DenConductor1 { get; set; }
        public string NomConductor1 { get; set; }
        public string ApeConductor1 { get; set; }
        public string LicConductor1 { get; set; }
        // Transportista 2
        public bool ManTransportista2 { get; set; }
        public string CodTransportista2 { get; set; }
        public string RucTransportista2 { get; set; }
        public string NomTransportista2 { get; set; }
        public string DirTransportista2 { get; set; }
        // PIE
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
        // USUARIO
        public int IdUsuario { get; set; }

        public List<EntregaVentaItemEntity> Item { get; set; } = new List<EntregaVentaItemEntity>();
    }
}
