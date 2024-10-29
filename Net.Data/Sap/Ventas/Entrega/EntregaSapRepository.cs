using System;
using System.IO;
using System.Data;
using Net.Connection;
using Net.CrossCotting;
using System.Transactions;
using Net.Business.Entities;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using Microsoft.Data.SqlClient;
using Net.Business.Entities.Sap;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using SAPbobsCOM;

namespace Net.Data.Sap
{
    public class EntregaSapRepository : RepositoryBase<EntregaVentaSapEntity>, IEntregaSapRepository
    {
        private string _metodoName;
        private string _aplicacionName;
        private readonly Regex regex = new Regex(@"<(\w+)>.*");

        // PARAMETROS DE COXIÓN
        private readonly string _cnxSap;
        private readonly ConnectionSapEntity _cnxDiApiSap;
        private readonly IConfiguration _configuration;


        // STORED PROCEDURE
        const string DB_ESQUEMA = "";
                
        const string SP_GET_LIST_DESPACHO_MERCADERIA_BY_FECHA_SEDE = DB_ESQUEMA + "FIB_WEB_SP_VEND_GetListGuiaDespachoMercaderiaByFechaSede";

        public EntregaSapRepository(IConnectionSql context, IConfiguration configuration)
            : base(context)
        {
            _cnxSap = Utilidades.GetExtraerCadenaConexion(configuration, "ParametersConectionSap");
            _cnxDiApiSap = Utilidades.GetExtraerCadenaConexionDiApiSap(configuration, "ParametersConectionDiApiSap");
            _configuration = configuration;
            _aplicacionName = GetType().Name;
        }

        public async Task<ResultadoTransaccion<GuiaDespachoMercaderiaSapByFechaSedeEntity>> GetListGuiaDespachoMercaderiaByFechaSede(DateTime fecIni, DateTime fecFin, string location)
        {
            var response = new List<GuiaDespachoMercaderiaSapByFechaSedeEntity>();
            var resultadoTransaccion = new ResultadoTransaccion<GuiaDespachoMercaderiaSapByFechaSedeEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_DESPACHO_MERCADERIA_BY_FECHA_SEDE, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@FecIni", fecIni));
                        cmd.Parameters.Add(new SqlParameter("@FecFin", fecFin));
                        cmd.Parameters.Add(new SqlParameter("@Location", location));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<GuiaDespachoMercaderiaSapByFechaSedeEntity>)context.ConvertTo<GuiaDespachoMercaderiaSapByFechaSedeEntity>(reader);
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

        public async Task<ResultadoTransaccion<MemoryStream>> GetGuiaDespachoMercaderiaExcelByFechaSede(DateTime fecIni, DateTime fecFin, string location)
        {
            var ms = new MemoryStream();
            var response = new List<GuiaDespachoMercaderiaSapByFechaSedeEntity>();
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
                    Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Órden de Fabricación" };
                    sheets.Append(sheet);

                    workbookPart.Workbook.Save();

                    SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

                    //Cabecera
                    Row row = new Row();
                    row.Append(
                    ExportToExcel.ConstructCell("Tipo", CellValues.String),
                    ExportToExcel.ConstructCell("Número Pedido", CellValues.String),
                    ExportToExcel.ConstructCell("Fecha Contabilización Guía", CellValues.String),
                    ExportToExcel.ConstructCell("Número Guía", CellValues.String),
                    ExportToExcel.ConstructCell("Código Cliente", CellValues.String),
                    ExportToExcel.ConstructCell("Nombre Cliente", CellValues.String),
                    ExportToExcel.ConstructCell("Código Artículo", CellValues.String),
                    ExportToExcel.ConstructCell("Nombre Artículo", CellValues.String),
                    ExportToExcel.ConstructCell("Almacén Origen", CellValues.String),
                    ExportToExcel.ConstructCell("Almacén Destino", CellValues.String),
                    ExportToExcel.ConstructCell("Bultos", CellValues.String),
                    ExportToExcel.ConstructCell("Total Kg", CellValues.String),
                    ExportToExcel.ConstructCell("Cantidad", CellValues.String),
                    ExportToExcel.ConstructCell("Número Fctura", CellValues.String),
                    ExportToExcel.ConstructCell("Nombre Transportista", CellValues.String),
                    ExportToExcel.ConstructCell("RUC Transportista", CellValues.String),
                    ExportToExcel.ConstructCell("Placa Transportista", CellValues.String),
                    ExportToExcel.ConstructCell("Nombre Conductor", CellValues.String),
                    ExportToExcel.ConstructCell("Lincencia Conductor", CellValues.String));
                    sheetData.AppendChild(row);

                    using (SqlConnection conn = new SqlConnection(_cnxSap))
                    {
                        conn.Open();

                        using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_DESPACHO_MERCADERIA_BY_FECHA_SEDE, conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 0;
                            cmd.Parameters.Add(new SqlParameter("@FecIni", fecIni));
                            cmd.Parameters.Add(new SqlParameter("@FecFin", fecFin));
                            cmd.Parameters.Add(new SqlParameter("@Location", location));

                            using (var reader = await cmd.ExecuteReaderAsync())
                            {
                                response = (List<GuiaDespachoMercaderiaSapByFechaSedeEntity>)context.ConvertTo<GuiaDespachoMercaderiaSapByFechaSedeEntity>(reader);
                            }
                        }
                    }

                    //Contenido
                    foreach (var item in response)
                    {
                        row = new Row();
                        row.Append(
                        ExportToExcel.ConstructCell(item.Tipo, CellValues.String),
                        ExportToExcel.ConstructCell(item.NumeroPedido.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.DocDate.ToString("dd/MM/yyyy"), CellValues.String),
                        ExportToExcel.ConstructCell(item.NumeroGuiaSUNAT, CellValues.String),
                        ExportToExcel.ConstructCell(item.CardCode, CellValues.String),
                        ExportToExcel.ConstructCell(item.CardName, CellValues.String),
                        ExportToExcel.ConstructCell(item.ItemCode, CellValues.String),
                        ExportToExcel.ConstructCell(item.ItemName, CellValues.String),
                        ExportToExcel.ConstructCell(item.AlmacenOrigen, CellValues.String),
                        ExportToExcel.ConstructCell(item.AlmacenDestino, CellValues.String),
                        ExportToExcel.ConstructCell(item.Bulto.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.TotalKg.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.Quantity.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.NumeroFcturaSUNAT, CellValues.String),
                        ExportToExcel.ConstructCell(item.NomTransportista, CellValues.String),
                        ExportToExcel.ConstructCell(item.RucTransportista, CellValues.String),
                        ExportToExcel.ConstructCell(item.PlacaTransportista, CellValues.String),
                        ExportToExcel.ConstructCell(item.NomConductor, CellValues.String),
                        ExportToExcel.ConstructCell(item.LincenciaConductor, CellValues.String));
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
    }
}
