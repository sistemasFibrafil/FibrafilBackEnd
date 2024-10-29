using System;
using System.Data;
using Net.Connection;
using Net.CrossCotting;
using Net.Business.Entities;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Net.Business.Entities.Sap;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml.Office2013.PowerPoint.Roaming;

namespace Net.Data.Sap
{
    public class PagoRecibidoSapRepository : RepositoryBase<CobranzaCarteraVencidaByFechaCorteSapEntity>, IPagoRecibidoSapRepository
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
        const string SP_GET_LIST_COBRANZA_CARTERA_VENCIDA_BY_FECHA_CORTE = DB_ESQUEMA + "FIB_WEB_GEBA_SP_GetListCobranzaCarteraVencidaByFechaCorte";


        public PagoRecibidoSapRepository(IConnectionSql context, IConfiguration configuration)
            : base(context)
        {
            _cnxSap = Utilidades.GetExtraerCadenaConexion(configuration, "ParametersConectionSap");
            _configuration = configuration;
            _aplicacionName = GetType().Name;
        }

        public async Task<ResultadoTransaccion<CobranzaCarteraVencidaByFechaCorteSapEntity>> GetListCobranzaCarteraVencidaByFechaCorte(FiltroRequestEntity value)
        {
            var response = new List<CobranzaCarteraVencidaByFechaCorteSapEntity>();
            var resultadoTransaccion = new ResultadoTransaccion<CobranzaCarteraVencidaByFechaCorteSapEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_COBRANZA_CARTERA_VENCIDA_BY_FECHA_CORTE, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@FecCorte", value.Fecha1));
                        cmd.Parameters.Add(new SqlParameter("@GroupCode", value.Code1));
                        cmd.Parameters.Add(new SqlParameter("@Filtro", value.TextFiltro1));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<CobranzaCarteraVencidaByFechaCorteSapEntity>)context.ConvertTo<CobranzaCarteraVencidaByFechaCorteSapEntity>(reader);
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

        public async Task<ResultadoTransaccion<MemoryStream>> GetListCobranzaCarteraVencidaExcelByFechaCorte(FiltroRequestEntity value)
        {
            var ms = new MemoryStream();
            var response = new List<CobranzaCarteraVencidaByFechaCorteSapEntity>();
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
                    Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Cobranza de Cartera Venciadas" };
                    sheets.Append(sheet);

                    workbookPart.Workbook.Save();

                    SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

                    //Cabecera
                    Row row = new Row();
                    row.Append(
                    ExportToExcel.ConstructCell("Código de Cliente", CellValues.String),
                    ExportToExcel.ConstructCell("Nombre de Cliente", CellValues.String),
                    ExportToExcel.ConstructCell("Grupo de Cliente", CellValues.String),
                    ExportToExcel.ConstructCell("Línea de Crédito", CellValues.String),
                    ExportToExcel.ConstructCell("Vendedor", CellValues.String),
                    ExportToExcel.ConstructCell("Número de Asiento", CellValues.String),
                    ExportToExcel.ConstructCell("Número de SAP", CellValues.String),
                    ExportToExcel.ConstructCell("Tipo de Documento", CellValues.String),
                    ExportToExcel.ConstructCell("Número de Documento", CellValues.String),
                    ExportToExcel.ConstructCell("Fecha de Contablización", CellValues.String),
                    ExportToExcel.ConstructCell("Fecha de Emisón", CellValues.String),
                    ExportToExcel.ConstructCell("Fecha de Vencimiento", CellValues.String),
                    ExportToExcel.ConstructCell("Comentarios", CellValues.String),
                    ExportToExcel.ConstructCell("Segmento 0", CellValues.String),
                    ExportToExcel.ConstructCell("Condicion de Pago", CellValues.String),
                    ExportToExcel.ConstructCell("Moneda", CellValues.String),
                    ExportToExcel.ConstructCell("Saldo SOL", CellValues.String),
                    ExportToExcel.ConstructCell("Saldo USD", CellValues.String),
                    ExportToExcel.ConstructCell("Saldo SYS", CellValues.String),
                    ExportToExcel.ConstructCell("0-15 días", CellValues.String),
                    ExportToExcel.ConstructCell("16-30 días", CellValues.String),
                    ExportToExcel.ConstructCell("31-60 días", CellValues.String),
                    ExportToExcel.ConstructCell("+60 días", CellValues.String));
                    sheetData.AppendChild(row);

                    using (SqlConnection conn = new SqlConnection(_cnxSap))
                    {
                        conn.Open();

                        using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_COBRANZA_CARTERA_VENCIDA_BY_FECHA_CORTE, conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 0;
                            cmd.Parameters.Add(new SqlParameter("@FecCorte", value.Fecha1));
                            cmd.Parameters.Add(new SqlParameter("@GroupCode", value.Code1));
                            cmd.Parameters.Add(new SqlParameter("@Filtro", value.TextFiltro1));

                            using (var reader = await cmd.ExecuteReaderAsync())
                            {
                                response = (List<CobranzaCarteraVencidaByFechaCorteSapEntity>)context.ConvertTo<CobranzaCarteraVencidaByFechaCorteSapEntity>(reader);
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
                        ExportToExcel.ConstructCell(item.GroupName, CellValues.String),
                        ExportToExcel.ConstructCell(item.CreditLine.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.SlpName, CellValues.String),
                        ExportToExcel.ConstructCell(item.NumeroAsiento.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.NumeroSAP.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.TipoDocumento, CellValues.String),
                        ExportToExcel.ConstructCell(item.NumeroDocumento, CellValues.String),
                        ExportToExcel.ConstructCell(item.DocDate.ToString("dd/MM/yyyy"), CellValues.String),
                        ExportToExcel.ConstructCell(item.TaxDate.ToString("dd/MM/yyyy"), CellValues.String),
                        ExportToExcel.ConstructCell(item.DueDate.ToString("dd/MM/yyyy"), CellValues.String),
                        ExportToExcel.ConstructCell(item.Comments, CellValues.String),
                        ExportToExcel.ConstructCell(item.Segment_0, CellValues.Number),
                        ExportToExcel.ConstructCell(item.CondicionPago, CellValues.String),
                        ExportToExcel.ConstructCell(item.Moneda, CellValues.String),
                        ExportToExcel.ConstructCell(item.SaldoSOL.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.SaldoUSD.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.SaldoSYS.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.De_0_15_Dias.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.De_16_30_Dias.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.De_31_60_Dias.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.Mas_60_Dias.ToString(), CellValues.Number));
                        sheetData.Append(row);
                    }

                    var saldoSOL = response.Sum(x=>x.SaldoSOL);
                    var saldoUSD = response.Sum(x=>x.SaldoUSD);
                    var saldoSYS = response.Sum(x=>x.SaldoSYS);
                    var de_0_15_Dias = response.Sum(x=>x.De_0_15_Dias);
                    var de_16_30_Dias = response.Sum(x=>x.De_16_30_Dias);
                    var de_31_60_Dias = response.Sum(x=>x.De_31_60_Dias);
                    var mas_60_Dias = response.Sum(x=>x.Mas_60_Dias);

                    //Pie
                    row = new Row();
                    row.Append(
                    ExportToExcel.ConstructCell("Total", CellValues.String),
                    ExportToExcel.ConstructCell("", CellValues.String),
                    ExportToExcel.ConstructCell("", CellValues.String),
                    ExportToExcel.ConstructCell("", CellValues.String),
                    ExportToExcel.ConstructCell("", CellValues.String),
                    ExportToExcel.ConstructCell("", CellValues.String),
                    ExportToExcel.ConstructCell("", CellValues.String),
                    ExportToExcel.ConstructCell("", CellValues.String),
                    ExportToExcel.ConstructCell("", CellValues.String),
                    ExportToExcel.ConstructCell("", CellValues.String),
                    ExportToExcel.ConstructCell("", CellValues.String),
                    ExportToExcel.ConstructCell("", CellValues.String),
                    ExportToExcel.ConstructCell("", CellValues.String),
                    ExportToExcel.ConstructCell("", CellValues.String),
                    ExportToExcel.ConstructCell("", CellValues.String),
                    ExportToExcel.ConstructCell("", CellValues.String),
                    ExportToExcel.ConstructCell(saldoSOL.ToString(), CellValues.Number),
                    ExportToExcel.ConstructCell(saldoUSD.ToString(), CellValues.Number),
                    ExportToExcel.ConstructCell(saldoSYS.ToString(), CellValues.Number),
                    ExportToExcel.ConstructCell(de_0_15_Dias.ToString(), CellValues.Number),
                    ExportToExcel.ConstructCell(de_16_30_Dias.ToString(), CellValues.Number),
                    ExportToExcel.ConstructCell(de_31_60_Dias.ToString(), CellValues.Number),
                    ExportToExcel.ConstructCell(mas_60_Dias.ToString(), CellValues.Number));
                    sheetData.AppendChild(row);

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
