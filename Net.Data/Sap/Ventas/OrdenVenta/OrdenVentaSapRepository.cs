using System;
using System.IO;
using System.Data;
using Net.Connection;
using Net.CrossCotting;
using Net.Business.Entities;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using Microsoft.Data.SqlClient;
using Net.Business.Entities.Sap;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Packaging;
using Microsoft.Extensions.Configuration;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Office2013.PowerPoint.Roaming;
namespace Net.Data.Sap
{
    public class OrdenVentaSapRepository : RepositoryBase<OrdenVentaSapEntity>, IOrdenVentaSapRepository
    {
        private string _metodoName;
        private string _aplicacionName;
        private readonly Regex regex = new Regex(@"<(\w+)>.*");

        // PARAMETROS DE COXIÓN
        //private readonly string _cnx;
        private readonly string _cnxSap;
        private readonly IConfiguration _configuration;

        // STORED PROCEDURE
        const string DB_ESQUEMA = "";
        const string SP_GET_LIST_SEGUIMIENTO_BY_FECHA = DB_ESQUEMA + "FIB_WEB_VEN_SP_GetListOrdenVentaSeguimientoByFecha";
        const string SP_GET_LIST_SEGUIMIENTO_DETALLADO_BY_FECHA = DB_ESQUEMA + "FIB_WEB_VEN_SP_GetListOrdenVentaSeguimientoDetalladoByFecha";
        const string SP_GET_LIST_PROGRAMACION_BY_FECHA = DB_ESQUEMA + "FIB_WEB_VEN_SP_GetListOrdenVentaProgramacionByFecha";
        const string SP_GET_LIST_PENDIENTE_STOCK_ALMACEN_PRODUCCION_BY_FECHA = DB_ESQUEMA + "FIB_WEB_VEN_SP_GetListOrdenVentaPendienteStockAlmacenProduccionByFecha";

        const string SP_GET_LIST_SODIMAC_BY_FILTRO = DB_ESQUEMA + "VEN_SP_GetListOrdenVentaSodimacPendienteByFiltro";
        const string SP_GET_LIST_SODIMAC_BY_DOCENTRY = DB_ESQUEMA + "VEN_SP_GetOrdenVentaSodimacPendienteByDocEntry";
        

        public OrdenVentaSapRepository(IConnectionSql context, IConfiguration configuration)
            : base(context)
        {
            _cnxSap = Utilidades.GetExtraerCadenaConexion(configuration, "ParametersConectionSap");
            _configuration = configuration;
            _aplicacionName = GetType().Name;
        }

        public async Task<ResultadoTransaccion<OrdenVentaSapByFechaEntity>> GetListOrdenVentaSeguimientoByFecha(FiltroRequestEntity value)
        {
            var response = new List<OrdenVentaSapByFechaEntity>();
            var resultadoTransaccion = new ResultadoTransaccion<OrdenVentaSapByFechaEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_SEGUIMIENTO_BY_FECHA, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@FecInicial", value.Fecha1));
                        cmd.Parameters.Add(new SqlParameter("@FecFinal", value.Fecha1));
                        cmd.Parameters.Add(new SqlParameter("@GrupoCliente", value.Code1));
                        cmd.Parameters.Add(new SqlParameter("@TipDocumento", value.Code2));
                        cmd.Parameters.Add(new SqlParameter("@Status", value.Code3));
                        cmd.Parameters.Add(new SqlParameter("@Filtro", value.TextFiltro1));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<OrdenVentaSapByFechaEntity>)context.ConvertTo<OrdenVentaSapByFechaEntity>(reader);
                        }
                    }

                    resultadoTransaccion.IdRegistro = 0;
                    resultadoTransaccion.ResultadoCodigo = 0;
                    resultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", response.Count);
                    resultadoTransaccion.dataList = response;
                }
            }
            catch (Exception ex)
            {
                resultadoTransaccion.IdRegistro = -1;
                resultadoTransaccion.ResultadoCodigo = -1;
                resultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
            }

            return resultadoTransaccion;
        }

        public async Task<ResultadoTransaccion<MemoryStream>> GetOrdenVentaSeguimientoExcelByFecha(FiltroRequestEntity value)
        {
            var ms = new MemoryStream();
            var response = new List<OrdenVentaSapByFechaEntity>();
            ResultadoTransaccion<MemoryStream> resultadoTransaccion = new ResultadoTransaccion<MemoryStream>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SpreadsheetDocument document = SpreadsheetDocument.Create(ms, SpreadsheetDocumentType.Workbook))
                {
                    WorkbookPart workbookPart = document.AddWorkbookPart();
                    workbookPart.Workbook = new Workbook();

                    WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                    worksheetPart.Worksheet = new Worksheet();

                    Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
                    Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Ventas" };
                    sheets.Append(sheet);

                    workbookPart.Workbook.Save();

                    SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

                    //Cabecera
                    Row row = new Row();
                    row.Append(
                    ExportToExcel.ConstructCell("Código Cliente", CellValues.String),
                    ExportToExcel.ConstructCell("Nombre de Cliente", CellValues.String),
                    ExportToExcel.ConstructCell("País", CellValues.String),
                    ExportToExcel.ConstructCell("Departamento", CellValues.String),
                    ExportToExcel.ConstructCell("Provincia", CellValues.String),
                    ExportToExcel.ConstructCell("Ciudad", CellValues.String),
                    ExportToExcel.ConstructCell("Tipo Documento", CellValues.String),
                    ExportToExcel.ConstructCell("Número Documento", CellValues.String),
                    ExportToExcel.ConstructCell("Fecha Contabilización", CellValues.String),
                    ExportToExcel.ConstructCell("Fecha Emisión", CellValues.String),
                    ExportToExcel.ConstructCell("Fecha Entrega", CellValues.String),
                    ExportToExcel.ConstructCell("Fecha Creación", CellValues.String),
                    ExportToExcel.ConstructCell("Estado", CellValues.String),
                    ExportToExcel.ConstructCell("Moneda", CellValues.String),
                    ExportToExcel.ConstructCell("TC", CellValues.String),
                    ExportToExcel.ConstructCell("Total Documento SOL", CellValues.String),
                    ExportToExcel.ConstructCell("Total Documento USD", CellValues.String),
                    ExportToExcel.ConstructCell("Total Documento SYS", CellValues.String),
                    ExportToExcel.ConstructCell("Código Vendedor", CellValues.String),
                    ExportToExcel.ConstructCell("Vendedor", CellValues.String),
                    ExportToExcel.ConstructCell("Condición de Pago", CellValues.String),
                    ExportToExcel.ConstructCell("Código División", CellValues.String),
                    ExportToExcel.ConstructCell("División", CellValues.String),
                    ExportToExcel.ConstructCell("Código Sector", CellValues.String),
                    ExportToExcel.ConstructCell("Sector", CellValues.String),
                    ExportToExcel.ConstructCell("Días Atraso", CellValues.String),
                    ExportToExcel.ConstructCell("Días Vencimiento", CellValues.String),
                    ExportToExcel.ConstructCell("Origen Cliente", CellValues.String));
                    sheetData.AppendChild(row);

                    using (SqlConnection conn = new SqlConnection(_cnxSap))
                    {
                        conn.Open();

                        using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_SEGUIMIENTO_BY_FECHA, conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 0;
                            cmd.Parameters.Add(new SqlParameter("@FecInicial", value.Fecha1));
                            cmd.Parameters.Add(new SqlParameter("@FecFinal", value.Fecha2));
                            cmd.Parameters.Add(new SqlParameter("@GrupoCliente", value.Code1));
                            cmd.Parameters.Add(new SqlParameter("@TipDocumento", value.Code2));
                            cmd.Parameters.Add(new SqlParameter("@Status", value.Code3));

                            using (var reader = await cmd.ExecuteReaderAsync())
                            {
                                response = (List<OrdenVentaSapByFechaEntity>)context.ConvertTo<OrdenVentaSapByFechaEntity>(reader);
                            }
                        }
                    }

                    //Contenido
                    foreach (var item in response)
                    {
                        row = new Row();
                        row.Append(
                        ExportToExcel.ConstructCell(item.CardCode, CellValues.String),
                        ExportToExcel.ConstructCell(item.CardName, CellValues.String),
                        ExportToExcel.ConstructCell(item.Pais, CellValues.String),
                        ExportToExcel.ConstructCell(item.Departamento, CellValues.String),
                        ExportToExcel.ConstructCell(item.Provincia, CellValues.String),
                        ExportToExcel.ConstructCell(item.Ciudad, CellValues.String),
                        ExportToExcel.ConstructCell(item.NomTipDocumento.ToString(), CellValues.String),
                        ExportToExcel.ConstructCell(item.NumeroDocumento.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.DocDate.ToString("dd/MM/yyyy"), CellValues.String),
                        ExportToExcel.ConstructCell(item.TaxDate.ToString("dd/MM/yyyy"), CellValues.String),
                        ExportToExcel.ConstructCell(item.DocDueDate.ToString("dd/MM/yyyy"), CellValues.String),
                        ExportToExcel.ConstructCell(item.CreateDate.ToString("dd/MM/yyyy"), CellValues.String),
                        ExportToExcel.ConstructCell(item.NomStatus, CellValues.String),
                        ExportToExcel.ConstructCell(item.DocCur, CellValues.String),
                        ExportToExcel.ConstructCell(item.Rate.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.DocTotal.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.DocTotalFC.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.DocTotalSy.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.SlpCode.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.SlpName, CellValues.String),
                        ExportToExcel.ConstructCell(item.PymntGroup, CellValues.String),
                        ExportToExcel.ConstructCell(item.IdDivision, CellValues.String),
                        ExportToExcel.ConstructCell(item.Division, CellValues.String),
                        ExportToExcel.ConstructCell(item.IdSector, CellValues.String),
                        ExportToExcel.ConstructCell(item.Sector, CellValues.String),
                        ExportToExcel.ConstructCell(item.DiasAtraso.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.DiasVenc, CellValues.String),
                        ExportToExcel.ConstructCell(item.OrigenCliente, CellValues.String));
                        sheetData.Append(row);
                    }

                    worksheetPart.Worksheet.Save();
                    document.Close();
                }

                resultadoTransaccion.IdRegistro = 0;
                resultadoTransaccion.ResultadoCodigo = 0;
                resultadoTransaccion.ResultadoDescripcion = "Archivo generado con éxito.";
                resultadoTransaccion.data = ms;
            }
            catch (Exception ex)
            {
                resultadoTransaccion.IdRegistro = -1;
                resultadoTransaccion.ResultadoCodigo = -1;
                resultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
            }

            return resultadoTransaccion;
        }

        public async Task<ResultadoTransaccion<OrdenVentaSapByFechaEntity>> GetListOrdenVentaSeguimientoDetalladoByFecha(FiltroRequestEntity value)
        {
            var response = new List<OrdenVentaSapByFechaEntity>();
            var resultadoTransaccion = new ResultadoTransaccion<OrdenVentaSapByFechaEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_SEGUIMIENTO_DETALLADO_BY_FECHA, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@FecInicial", value.Fecha1));
                        cmd.Parameters.Add(new SqlParameter("@FecFinal", value.Fecha2));
                        cmd.Parameters.Add(new SqlParameter("@GrupoCliente", value.Code1));
                        cmd.Parameters.Add(new SqlParameter("@TipDocumento", value.Code2));
                        cmd.Parameters.Add(new SqlParameter("@Status", value.Code3));
                        cmd.Parameters.Add(new SqlParameter("@Filtro", value.TextFiltro1));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<OrdenVentaSapByFechaEntity>)context.ConvertTo<OrdenVentaSapByFechaEntity>(reader);
                        }
                    }

                    resultadoTransaccion.IdRegistro = 0;
                    resultadoTransaccion.ResultadoCodigo = 0;
                    resultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", response.Count);
                    resultadoTransaccion.dataList = response;
                }
            }
            catch (Exception ex)
            {
                resultadoTransaccion.IdRegistro = -1;
                resultadoTransaccion.ResultadoCodigo = -1;
                resultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
            }

            return resultadoTransaccion;
        }

        public async Task<ResultadoTransaccion<MemoryStream>> GetOrdenVentaSeguimientoDetalladoExcelByFecha(FiltroRequestEntity value)
        {
            var ms = new MemoryStream();
            var response = new List<OrdenVentaSapByFechaEntity>();
            ResultadoTransaccion<MemoryStream> resultadoTransaccion = new ResultadoTransaccion<MemoryStream>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SpreadsheetDocument document = SpreadsheetDocument.Create(ms, SpreadsheetDocumentType.Workbook))
                {
                    WorkbookPart workbookPart = document.AddWorkbookPart();
                    workbookPart.Workbook = new Workbook();

                    WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                    worksheetPart.Worksheet = new Worksheet();

                    Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
                    Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Ventas" };
                    sheets.Append(sheet);

                    workbookPart.Workbook.Save();

                    SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

                    //Cabecera
                    Row row = new Row();
                    row.Append(
                    ExportToExcel.ConstructCell("Código Cliente", CellValues.String),
                    ExportToExcel.ConstructCell("Cliente", CellValues.String),
                    ExportToExcel.ConstructCell("País", CellValues.String),
                    ExportToExcel.ConstructCell("Departamento", CellValues.String),
                    ExportToExcel.ConstructCell("Provincia", CellValues.String),
                    ExportToExcel.ConstructCell("Ciudad", CellValues.String),
                    ExportToExcel.ConstructCell("Tipo Documento", CellValues.String),
                    ExportToExcel.ConstructCell("Número Documento", CellValues.String),
                    ExportToExcel.ConstructCell("Número Pedido", CellValues.String),
                    ExportToExcel.ConstructCell("Número Órden Venta", CellValues.String),
                    ExportToExcel.ConstructCell("Número Factura", CellValues.String),
                    ExportToExcel.ConstructCell("Número Línea", CellValues.String),
                    ExportToExcel.ConstructCell("Fecha Contabilización", CellValues.String),
                    ExportToExcel.ConstructCell("Fecha Emisión", CellValues.String),
                    ExportToExcel.ConstructCell("Fecha Entrega", CellValues.String),
                    ExportToExcel.ConstructCell("Fecha Creación", CellValues.String),
                    ExportToExcel.ConstructCell("Estado", CellValues.String),
                    ExportToExcel.ConstructCell("Código Grupo Artículo", CellValues.String),
                    ExportToExcel.ConstructCell("Nombre de Artículo", CellValues.String),
                    ExportToExcel.ConstructCell("Código de Grupo de Artículo", CellValues.String),
                    ExportToExcel.ConstructCell("Nombre de Grupo de Artículo", CellValues.String),
                    ExportToExcel.ConstructCell("Nombre de SubGrupo de Artículo", CellValues.String),
                    ExportToExcel.ConstructCell("Nombre de SubGrupo de Artículo 2", CellValues.String),
                    ExportToExcel.ConstructCell("Medida", CellValues.String),
                    ExportToExcel.ConstructCell("Color", CellValues.String),
                    ExportToExcel.ConstructCell("Código de Alamcén", CellValues.String),
                    ExportToExcel.ConstructCell("Nombre de Alamcén", CellValues.String),
                    ExportToExcel.ConstructCell("UM Compra", CellValues.String),
                    ExportToExcel.ConstructCell("UM Venta", CellValues.String),
                    ExportToExcel.ConstructCell("UM Inventario", CellValues.String),
                    ExportToExcel.ConstructCell("Stock", CellValues.String),
                    ExportToExcel.ConstructCell("Pendiente", CellValues.String),
                    ExportToExcel.ConstructCell("Solicitado", CellValues.String),
                    ExportToExcel.ConstructCell("Disponible", CellValues.String),
                    ExportToExcel.ConstructCell("Cantidad", CellValues.String),
                    ExportToExcel.ConstructCell("Rollo Pedido", CellValues.String),
                    ExportToExcel.ConstructCell("Kg Pedido", CellValues.String),
                    ExportToExcel.ConstructCell("Tonelada Pedida", CellValues.String),
                    ExportToExcel.ConstructCell("Cantidad Pendiente por Despachar", CellValues.String),
                    ExportToExcel.ConstructCell("Rollo Pendiente", CellValues.String),
                    ExportToExcel.ConstructCell("Kg Pendiente", CellValues.String),
                    ExportToExcel.ConstructCell("Tonelada Pendiente", CellValues.String),
                    ExportToExcel.ConstructCell("Cantidad Despachada", CellValues.String),
                    ExportToExcel.ConstructCell("Moneda", CellValues.String),
                    ExportToExcel.ConstructCell("TC", CellValues.String),
                    ExportToExcel.ConstructCell("Precio", CellValues.String),
                    ExportToExcel.ConstructCell("Total Línea SOL", CellValues.String),
                    ExportToExcel.ConstructCell("Total Línea USD", CellValues.String),
                    ExportToExcel.ConstructCell("Total Línea SYS", CellValues.String),
                    ExportToExcel.ConstructCell("Total Documento USD", CellValues.String),
                    ExportToExcel.ConstructCell("Código Vendedor", CellValues.String),
                    ExportToExcel.ConstructCell("Vendedor", CellValues.String),
                    ExportToExcel.ConstructCell("Condición de Pago", CellValues.String),
                    ExportToExcel.ConstructCell("Código División", CellValues.String),
                    ExportToExcel.ConstructCell("División", CellValues.String),
                    ExportToExcel.ConstructCell("Código Sector", CellValues.String),
                    ExportToExcel.ConstructCell("Sector", CellValues.String),
                    ExportToExcel.ConstructCell("Peso Promedio Kg", CellValues.String),
                    ExportToExcel.ConstructCell("Días Atraso", CellValues.String),
                    ExportToExcel.ConstructCell("Días Vencimiento", CellValues.String),
                    ExportToExcel.ConstructCell("Origen Cliente", CellValues.String),
                    ExportToExcel.ConstructCell("Sede", CellValues.String));
                    sheetData.AppendChild(row);

                    using (SqlConnection conn = new SqlConnection(_cnxSap))
                    {
                        conn.Open();

                        using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_SEGUIMIENTO_DETALLADO_BY_FECHA, conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 0;
                            cmd.Parameters.Add(new SqlParameter("@FecInicial", value.Fecha1));
                            cmd.Parameters.Add(new SqlParameter("@FecFinal", value.Fecha2));
                            cmd.Parameters.Add(new SqlParameter("@GrupoCliente", value.Code1));
                            cmd.Parameters.Add(new SqlParameter("@TipDocumento", value.Code2));
                            cmd.Parameters.Add(new SqlParameter("@Status", value.Code3));
                            cmd.Parameters.Add(new SqlParameter("@Filtro", value.TextFiltro1));

                            using (var reader = await cmd.ExecuteReaderAsync())
                            {
                                response = (List<OrdenVentaSapByFechaEntity>)context.ConvertTo<OrdenVentaSapByFechaEntity>(reader);
                            }
                        }
                    }

                    //Contenido
                    foreach (var item in response)
                    {
                        row = new Row();
                        row.Append(
                        ExportToExcel.ConstructCell(item.CardCode, CellValues.String),
                        ExportToExcel.ConstructCell(item.CardName, CellValues.String),
                        ExportToExcel.ConstructCell(item.Pais, CellValues.String),
                        ExportToExcel.ConstructCell(item.Departamento, CellValues.String),
                        ExportToExcel.ConstructCell(item.Provincia, CellValues.String),
                        ExportToExcel.ConstructCell(item.Ciudad, CellValues.String),
                        ExportToExcel.ConstructCell(item.NomTipDocumento.ToString(), CellValues.String),
                        ExportToExcel.ConstructCell(item.NumeroDocumento.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.NumeroPedido.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.NumeroOrdenVenta, CellValues.String),
                        ExportToExcel.ConstructCell(item.NumeroFactura == null ? null : item.NumeroFactura.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.LineNum.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.DocDate.ToString("dd/MM/yyyy"), CellValues.String),
                        ExportToExcel.ConstructCell(item.TaxDate.ToString("dd/MM/yyyy"), CellValues.String),
                        ExportToExcel.ConstructCell(item.DocDueDate.ToString("dd/MM/yyyy"), CellValues.String),
                        ExportToExcel.ConstructCell(item.CreateDate.ToString("dd/MM/yyyy"), CellValues.String),
                        ExportToExcel.ConstructCell(item.NomStatus, CellValues.String),
                        ExportToExcel.ConstructCell(item.ItemCode, CellValues.String),
                        ExportToExcel.ConstructCell(item.ItemName, CellValues.String),
                        ExportToExcel.ConstructCell(item.CodGrupoArticulo.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.NomGrupoArticulo, CellValues.String),
                        ExportToExcel.ConstructCell(item.NomSubGrupoArticulo, CellValues.String),
                        ExportToExcel.ConstructCell(item.NomSubGrupoArticulo2, CellValues.String),
                        ExportToExcel.ConstructCell(item.Medida, CellValues.String),
                        ExportToExcel.ConstructCell(item.Color, CellValues.String),
                        ExportToExcel.ConstructCell(item.WhsCode, CellValues.String),
                        ExportToExcel.ConstructCell(item.WhsName, CellValues.String),
                        ExportToExcel.ConstructCell(item.BuyUnitMsr, CellValues.String),
                        ExportToExcel.ConstructCell(item.SalUnitMsr, CellValues.String),
                        ExportToExcel.ConstructCell(item.InvntryUom, CellValues.String),
                        ExportToExcel.ConstructCell(item.StockProduccion.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.PendienteProduccion.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.SolicitadoProduccion.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.DisponibleProduccion.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.Quantity.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.RolloPedido.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.KgPedido.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.ToneladaPedida.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.OpenQty.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.RolloPendiente.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.KgPendiente.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.ToneladaPendiente.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.DelivrdQty.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.Currency, CellValues.String),
                        ExportToExcel.ConstructCell(item.Rate.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.Price.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.LineTotal.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.TotalFrgn.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.TotalSumSy.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.DocTotalSy.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.SlpCode.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.SlpName, CellValues.String),
                        ExportToExcel.ConstructCell(item.PymntGroup, CellValues.String),
                        ExportToExcel.ConstructCell(item.IdDivision, CellValues.String),
                        ExportToExcel.ConstructCell(item.Division, CellValues.String),
                        ExportToExcel.ConstructCell(item.IdSector, CellValues.String),
                        ExportToExcel.ConstructCell(item.Sector, CellValues.String),
                        ExportToExcel.ConstructCell(item.PesoPromedioKg.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.DiasAtraso.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.DiasVenc, CellValues.String),
                        ExportToExcel.ConstructCell(item.OrigenCliente, CellValues.String),
                        ExportToExcel.ConstructCell(item.Sede, CellValues.String));
                        sheetData.Append(row);
                    }

                    worksheetPart.Worksheet.Save();
                    document.Close();
                }

                resultadoTransaccion.IdRegistro = 0;
                resultadoTransaccion.ResultadoCodigo = 0;
                resultadoTransaccion.ResultadoDescripcion = "Archivo generado con éxito.";
                resultadoTransaccion.data = ms;
            }
            catch (Exception ex)
            {
                resultadoTransaccion.IdRegistro = -1;
                resultadoTransaccion.ResultadoCodigo = -1;
                resultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
            }

            return resultadoTransaccion;
        }

        public async Task<ResultadoTransaccion<OrdenVentaSapByFechaEntity>> GetListOrdenVentaPendienteStockAlmacenProduccionByFecha(FiltroRequestEntity value)
        {
            var response = new List<OrdenVentaSapByFechaEntity>();
            var resultadoTransaccion = new ResultadoTransaccion<OrdenVentaSapByFechaEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_PENDIENTE_STOCK_ALMACEN_PRODUCCION_BY_FECHA, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@FecInicial", value.Fecha1));
                        cmd.Parameters.Add(new SqlParameter("@FecFinal", value.Fecha2));
                        cmd.Parameters.Add(new SqlParameter("@Filtro", value.TextFiltro1));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<OrdenVentaSapByFechaEntity>)context.ConvertTo<OrdenVentaSapByFechaEntity>(reader);
                        }
                    }

                    resultadoTransaccion.IdRegistro = 0;
                    resultadoTransaccion.ResultadoCodigo = 0;
                    resultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", response.Count);
                    resultadoTransaccion.dataList = response;
                }
            }
            catch (Exception ex)
            {
                resultadoTransaccion.IdRegistro = -1;
                resultadoTransaccion.ResultadoCodigo = -1;
                resultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
            }

            return resultadoTransaccion;
        }

        public async Task<ResultadoTransaccion<MemoryStream>> GetOrdenVentaPendienteStockAlmacenProduccionExcelByFecha(FiltroRequestEntity value)
        {
            var ms = new MemoryStream();
            var response = new List<OrdenVentaSapByFechaEntity>();
            ResultadoTransaccion<MemoryStream> resultadoTransaccion = new ResultadoTransaccion<MemoryStream>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SpreadsheetDocument document = SpreadsheetDocument.Create(ms, SpreadsheetDocumentType.Workbook))
                {
                    WorkbookPart workbookPart = document.AddWorkbookPart();
                    workbookPart.Workbook = new Workbook();

                    WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                    worksheetPart.Worksheet = new Worksheet();

                    Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
                    Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Ventas" };
                    sheets.Append(sheet);

                    workbookPart.Workbook.Save();

                    SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

                    //Cabecera
                    Row row = new Row();
                    row.Append(
                    ExportToExcel.ConstructCell("Código Cliente", CellValues.String),
                    ExportToExcel.ConstructCell("Cliente", CellValues.String),
                    ExportToExcel.ConstructCell("País", CellValues.String),
                    ExportToExcel.ConstructCell("Departamento", CellValues.String),
                    ExportToExcel.ConstructCell("Provincia", CellValues.String),
                    ExportToExcel.ConstructCell("Ciudad", CellValues.String),
                    ExportToExcel.ConstructCell("Tipo de Documento", CellValues.String),
                    ExportToExcel.ConstructCell("Número Pedido", CellValues.String),
                    ExportToExcel.ConstructCell("Número Factura", CellValues.String),
                    ExportToExcel.ConstructCell("Número Línea", CellValues.String),
                    ExportToExcel.ConstructCell("Fecha Contabilización", CellValues.String),
                    ExportToExcel.ConstructCell("Fecha Emisión", CellValues.String),
                    ExportToExcel.ConstructCell("Fecha Entrega", CellValues.String),
                    ExportToExcel.ConstructCell("Código Artículo", CellValues.String),
                    ExportToExcel.ConstructCell("Nombre de Artículo", CellValues.String),
                    ExportToExcel.ConstructCell("Código de Grupo de Artículo", CellValues.String),
                    ExportToExcel.ConstructCell("Nombre de Grupo de Artículo", CellValues.String),
                    ExportToExcel.ConstructCell("Nombre de SubGrupo de Artículo", CellValues.String),
                    ExportToExcel.ConstructCell("Nombre de SubGrupo de Artículo 2", CellValues.String),
                    ExportToExcel.ConstructCell("Medida", CellValues.String),
                    ExportToExcel.ConstructCell("Color", CellValues.String),
                    ExportToExcel.ConstructCell("UM Compra", CellValues.String),
                    ExportToExcel.ConstructCell("UM Venta", CellValues.String),
                    ExportToExcel.ConstructCell("UM Inventario", CellValues.String),
                    ExportToExcel.ConstructCell("Stock", CellValues.String),
                    ExportToExcel.ConstructCell("Pendiente", CellValues.String),
                    ExportToExcel.ConstructCell("Solicitado", CellValues.String),
                    ExportToExcel.ConstructCell("Disponible", CellValues.String),
                    ExportToExcel.ConstructCell("Cantidad", CellValues.String),
                    ExportToExcel.ConstructCell("Rollo Pedido", CellValues.String),
                    ExportToExcel.ConstructCell("Kg Pedido", CellValues.String),
                    ExportToExcel.ConstructCell("Tonelada Pedida", CellValues.String),
                    ExportToExcel.ConstructCell("Cantidad Pendiente por Despachar", CellValues.String),
                    ExportToExcel.ConstructCell("Cantidad Despachada", CellValues.String),
                    ExportToExcel.ConstructCell("Moneda", CellValues.String),
                    ExportToExcel.ConstructCell("TC", CellValues.String),
                    ExportToExcel.ConstructCell("Precio", CellValues.String),
                    ExportToExcel.ConstructCell("Total Línea SOL", CellValues.String),
                    ExportToExcel.ConstructCell("Total Línea USD", CellValues.String),
                    ExportToExcel.ConstructCell("Total Línea SYS", CellValues.String),
                    ExportToExcel.ConstructCell("Total Documento USD", CellValues.String),
                    ExportToExcel.ConstructCell("Código Vendedor", CellValues.String),
                    ExportToExcel.ConstructCell("Vendedor", CellValues.String),
                    ExportToExcel.ConstructCell("Condición de Pago", CellValues.String),
                    ExportToExcel.ConstructCell("Código División", CellValues.String),
                    ExportToExcel.ConstructCell("División", CellValues.String),
                    ExportToExcel.ConstructCell("Código Sector", CellValues.String),
                    ExportToExcel.ConstructCell("Sector", CellValues.String),
                    ExportToExcel.ConstructCell("Peso Promedio Kg", CellValues.String),
                    ExportToExcel.ConstructCell("Días Atraso", CellValues.String),
                    ExportToExcel.ConstructCell("Días Vencimiento", CellValues.String),
                    ExportToExcel.ConstructCell("Origen Cliente", CellValues.String),
                    ExportToExcel.ConstructCell("Sede", CellValues.String));
                    sheetData.AppendChild(row);

                    using (SqlConnection conn = new SqlConnection(_cnxSap))
                    {
                        conn.Open();

                        using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_PENDIENTE_STOCK_ALMACEN_PRODUCCION_BY_FECHA, conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 0;
                            cmd.Parameters.Add(new SqlParameter("@FecInicial", value.Fecha1));
                            cmd.Parameters.Add(new SqlParameter("@FecFinal", value.Fecha2));
                            cmd.Parameters.Add(new SqlParameter("@Filtro", value.TextFiltro1));

                            using (var reader = await cmd.ExecuteReaderAsync())
                            {
                                response = (List<OrdenVentaSapByFechaEntity>)context.ConvertTo<OrdenVentaSapByFechaEntity>(reader);
                            }
                        }
                    }

                    //Contenido
                    foreach (var item in response)
                    {
                        row = new Row();
                        row.Append(
                        ExportToExcel.ConstructCell(item.CardCode, CellValues.String),
                        ExportToExcel.ConstructCell(item.CardName, CellValues.String),
                        ExportToExcel.ConstructCell(item.Pais, CellValues.String),
                        ExportToExcel.ConstructCell(item.Departamento, CellValues.String),
                        ExportToExcel.ConstructCell(item.Provincia, CellValues.String),
                        ExportToExcel.ConstructCell(item.Ciudad, CellValues.String),
                        ExportToExcel.ConstructCell(item.NomTipDocumento.ToString(), CellValues.String),
                        ExportToExcel.ConstructCell(item.NumeroPedido.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.NumeroFactura == null? null: item.NumeroFactura.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.LineNum.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.DocDate.ToString("dd/MM/yyyy"), CellValues.String),
                        ExportToExcel.ConstructCell(item.TaxDate.ToString("dd/MM/yyyy"), CellValues.String),
                        ExportToExcel.ConstructCell(item.DocDueDate.ToString("dd/MM/yyyy"), CellValues.String),
                        ExportToExcel.ConstructCell(item.ItemCode, CellValues.String),
                        ExportToExcel.ConstructCell(item.ItemName, CellValues.String),
                        ExportToExcel.ConstructCell(item.CodGrupoArticulo.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.NomGrupoArticulo, CellValues.String),
                        ExportToExcel.ConstructCell(item.NomSubGrupoArticulo, CellValues.String),
                        ExportToExcel.ConstructCell(item.NomSubGrupoArticulo2, CellValues.String),
                        ExportToExcel.ConstructCell(item.Medida, CellValues.String),
                        ExportToExcel.ConstructCell(item.Color, CellValues.String),
                        ExportToExcel.ConstructCell(item.BuyUnitMsr, CellValues.String),
                        ExportToExcel.ConstructCell(item.SalUnitMsr, CellValues.String),
                        ExportToExcel.ConstructCell(item.InvntryUom, CellValues.String),
                        ExportToExcel.ConstructCell(item.StockProduccion.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.PendienteProduccion.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.SolicitadoProduccion.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.DisponibleProduccion.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.Quantity.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.RolloPedido.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.KgPedido.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.ToneladaPedida.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.OpenQty.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.DelivrdQty.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.Currency, CellValues.String),
                        ExportToExcel.ConstructCell(item.Rate.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.Price.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.LineTotal.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.TotalFrgn.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.TotalSumSy.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.DocTotalSy.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.SlpCode.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.SlpName, CellValues.String),
                        ExportToExcel.ConstructCell(item.PymntGroup, CellValues.String),
                        ExportToExcel.ConstructCell(item.IdDivision, CellValues.String),
                        ExportToExcel.ConstructCell(item.Division, CellValues.String),
                        ExportToExcel.ConstructCell(item.IdSector, CellValues.String),
                        ExportToExcel.ConstructCell(item.Sector, CellValues.String),
                        ExportToExcel.ConstructCell(item.PesoPromedioKg.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.DiasAtraso.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.DiasVenc, CellValues.String),
                        ExportToExcel.ConstructCell(item.OrigenCliente, CellValues.String),
                        ExportToExcel.ConstructCell(item.Sede, CellValues.String));
                        sheetData.Append(row);
                    }

                    worksheetPart.Worksheet.Save();
                    document.Close();
                }

                resultadoTransaccion.IdRegistro = 0;
                resultadoTransaccion.ResultadoCodigo = 0;
                resultadoTransaccion.ResultadoDescripcion = "Archivo generado con éxito.";
                resultadoTransaccion.data = ms;
            }
            catch (Exception ex)
            {
                resultadoTransaccion.IdRegistro = -1;
                resultadoTransaccion.ResultadoCodigo = -1;
                resultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
            }

            return resultadoTransaccion;
        }

        public async Task<ResultadoTransaccion<OrdenVentaSapByFechaEntity>> GetListOrdenVentaProgramacionByFecha(FiltroRequestEntity value)
        {
            var response = new List<OrdenVentaSapByFechaEntity>();
            var resultadoTransaccion = new ResultadoTransaccion<OrdenVentaSapByFechaEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_PROGRAMACION_BY_FECHA, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@FecInicial", value.Fecha1));
                        cmd.Parameters.Add(new SqlParameter("@FecFinal", value.Fecha2));
                        cmd.Parameters.Add(new SqlParameter("@Filtro", value.TextFiltro1));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<OrdenVentaSapByFechaEntity>)context.ConvertTo<OrdenVentaSapByFechaEntity>(reader);
                        }
                    }

                    resultadoTransaccion.IdRegistro = 0;
                    resultadoTransaccion.ResultadoCodigo = 0;
                    resultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", response.Count);
                    resultadoTransaccion.dataList = response;
                }
            }
            catch (Exception ex)
            {
                resultadoTransaccion.IdRegistro = -1;
                resultadoTransaccion.ResultadoCodigo = -1;
                resultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
            }

            return resultadoTransaccion;
        }

        public async Task<ResultadoTransaccion<MemoryStream>> GetOrdenVentaProgramacionExcelByFecha(FiltroRequestEntity value)
        {
            var ms = new MemoryStream();
            var response = new List<OrdenVentaSapByFechaEntity>();
            ResultadoTransaccion<MemoryStream> resultadoTransaccion = new ResultadoTransaccion<MemoryStream>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SpreadsheetDocument document = SpreadsheetDocument.Create(ms, SpreadsheetDocumentType.Workbook))
                {
                    WorkbookPart workbookPart = document.AddWorkbookPart();
                    workbookPart.Workbook = new Workbook();

                    WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                    worksheetPart.Worksheet = new Worksheet();

                    Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
                    Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Ventas" };
                    sheets.Append(sheet);

                    workbookPart.Workbook.Save();

                    SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

                    //Cabecera
                    Row row = new Row();
                    row.Append(
                    ExportToExcel.ConstructCell("Código Cliente", CellValues.String),
                    ExportToExcel.ConstructCell("Nombre de Cliente", CellValues.String),
                    ExportToExcel.ConstructCell("Tipo de Documento", CellValues.String),
                    ExportToExcel.ConstructCell("Número Pedido", CellValues.String),
                    ExportToExcel.ConstructCell("Número Factura", CellValues.String),
                    ExportToExcel.ConstructCell("Fecha Contabilización", CellValues.String),
                    ExportToExcel.ConstructCell("Código Artículo", CellValues.String),
                    ExportToExcel.ConstructCell("Nombre de Artículo", CellValues.String),
                    ExportToExcel.ConstructCell("Grupo de Artículo", CellValues.String),
                    ExportToExcel.ConstructCell("UM", CellValues.String),
                    ExportToExcel.ConstructCell("Stock", CellValues.String),
                    ExportToExcel.ConstructCell("Cantidad", CellValues.String),
                    ExportToExcel.ConstructCell("Cantidad Pendiente por Despachar", CellValues.String),
                    ExportToExcel.ConstructCell("Días de Antiguedad", CellValues.String),
                    ExportToExcel.ConstructCell("Sede", CellValues.String));
                    sheetData.AppendChild(row);

                    using (SqlConnection conn = new SqlConnection(_cnxSap))
                    {
                        conn.Open();

                        using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_PROGRAMACION_BY_FECHA, conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 0;
                            cmd.Parameters.Add(new SqlParameter("@FecInicial", value.Fecha1));
                            cmd.Parameters.Add(new SqlParameter("@FecFinal", value.Fecha2));
                            cmd.Parameters.Add(new SqlParameter("@Filtro", value.TextFiltro1));

                            using (var reader = await cmd.ExecuteReaderAsync())
                            {
                                response = (List<OrdenVentaSapByFechaEntity>)context.ConvertTo<OrdenVentaSapByFechaEntity>(reader);
                            }
                        }
                    }

                    //Contenido
                    foreach (var item in response)
                    {
                        row = new Row();
                        row.Append(
                        ExportToExcel.ConstructCell(item.CardCode, CellValues.String),
                        ExportToExcel.ConstructCell(item.CardName, CellValues.String),
                        ExportToExcel.ConstructCell(item.NomTipDocumento.ToString(), CellValues.String),
                        ExportToExcel.ConstructCell(item.NumeroPedido.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.NumeroFactura == null ? null : item.NumeroFactura.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.DocDate.ToString("dd/MM/yyyy"), CellValues.String),
                        ExportToExcel.ConstructCell(item.ItemCode, CellValues.String),
                        ExportToExcel.ConstructCell(item.ItemName, CellValues.String),
                        ExportToExcel.ConstructCell(item.NomGrupoArticulo, CellValues.String),
                        ExportToExcel.ConstructCell(item.SalUnitMsr, CellValues.String),
                        ExportToExcel.ConstructCell(item.StockProduccion.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.Quantity.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.OpenQty.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.RolloPendiente.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.DiasAntiguedad.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.Sede, CellValues.String));
                        sheetData.Append(row);
                    }

                    worksheetPart.Worksheet.Save();
                    document.Close();
                }

                resultadoTransaccion.IdRegistro = 0;
                resultadoTransaccion.ResultadoCodigo = 0;
                resultadoTransaccion.ResultadoDescripcion = "Archivo generado con éxito.";
                resultadoTransaccion.data = ms;
            }
            catch (Exception ex)
            {
                resultadoTransaccion.IdRegistro = -1;
                resultadoTransaccion.ResultadoCodigo = -1;
                resultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
            }

            return resultadoTransaccion;
        }


        public async Task<ResultadoTransaccion<OrdenVentaSodimacSapEntity>> GetListOrdenVentaSodimacPendienteByFiltro(FiltroRequestEntity value)
        {
            var response = new List<OrdenVentaSodimacSapEntity>();
            var resultadoTransaccion = new ResultadoTransaccion<OrdenVentaSodimacSapEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_SODIMAC_BY_FILTRO, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@Filtro", value.TextFiltro1));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<OrdenVentaSodimacSapEntity>)context.ConvertTo<OrdenVentaSodimacSapEntity>(reader);
                        }
                    }

                    resultadoTransaccion.IdRegistro = 0;
                    resultadoTransaccion.ResultadoCodigo = 0;
                    resultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", response.Count);
                    resultadoTransaccion.dataList = response;
                }
            }
            catch (Exception ex)
            {
                resultadoTransaccion.IdRegistro = -1;
                resultadoTransaccion.ResultadoCodigo = -1;
                resultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
            }

            return resultadoTransaccion;
        }

        public async Task<ResultadoTransaccion<OrdenVentaSodimacSapEntity>> GetOrdenVentaSodimacPendienteByDocEntry(int docEntry)
        {
            var response = new OrdenVentaSodimacSapEntity();
            var resultadoTransaccion = new ResultadoTransaccion<OrdenVentaSodimacSapEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_SODIMAC_BY_DOCENTRY, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@DocEntry", docEntry));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = context.Convert<OrdenVentaSodimacSapEntity>(reader);
                        }
                    }

                    resultadoTransaccion.IdRegistro = 0;
                    resultadoTransaccion.ResultadoCodigo = 0;
                    resultadoTransaccion.ResultadoDescripcion = "Datos obtenidos con éxito ..!";
                    resultadoTransaccion.data = response;
                }
            }
            catch (Exception ex)
            {
                resultadoTransaccion.IdRegistro = -1;
                resultadoTransaccion.ResultadoCodigo = -1;
                resultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
            }

            return resultadoTransaccion;
        }
    }
}
