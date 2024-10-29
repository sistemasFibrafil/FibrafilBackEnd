using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
namespace Net.Business.Entities.Web
{
    public class ForcastVentaEntity
    {
        public ForcastVentaEntity()
        {
            Item = new List<ForcastVentaImportEntity>();
        }

        public int IdForcastVenta { get; set; }
        public int IdConSinOc { get; set; }
        public string NomConSinOc { get; set; }
        public int IdNegocio { get; set; }
        public string NomNegocio { get; set; }
        public int ItmsGrpCod { get; set; }
        public string ItmsGrpNam { get; set; }
        public string ItemCode { get; set; }
        public int DocNum { get; set; }
        public string CardCode { get; set; }
        public DateTime FecRegistro { get; set; }
        public string UnidadMedida { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Kg { get; set; }
        public decimal Precio { get; set; }
        public string CodEstado { get; set; }
        public string NomEstado { get; set; }
        public int IdUsuario { get; set; }

        public IFormFile Archivo { get; set; }

        public List<ForcastVentaImportEntity> Item { get; set; }
    }

    public class ForcastVentaImportEntity
    {
        public int IdForcastVenta { get; set; }
        public int IdConSinOc { get; set; }
        public int IdNegocio { get; set; }
        public int ItmsGrpCod { get; set; }
        public string ItemCode { get; set; }
        public int DocNum { get; set; }
        public string CardCode { get; set; }
        public DateTime FecRegistro { get; set; }
        public string UnidadMedida { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Kg { get; set; }
        public decimal Precio { get; set; }
        public string CodEstado { get; set; }
    }

    public class ForcastVentaByFechaEntity
    {
        public int IdForcastVenta { get; set; }
        public string NomConSinOc { get; set; }
        public string NomNegocio { get; set; }
        public string ItmsGrpNam { get; set; }
        public string ItemName { get; set; }
        public int DocNum { get; set; }
        public string CardName { get; set; }
        public DateTime FecRegistro { get; set; }
        public string UnidadMedida { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Kg { get; set; }
        public decimal Precio { get; set; }
        public decimal Total { get; set; }
        public string NomEstado { get; set; }
    }
}
