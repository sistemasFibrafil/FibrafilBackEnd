using System;
using System.IO;
using System.Text;
using System.Data;
using Net.Data.Web;
using Net.Connection;
using Net.CrossCotting;
using Net.Business.Entities;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using Microsoft.Data.SqlClient;
using Net.Business.Entities.Web;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Packaging;
using Microsoft.Extensions.Configuration;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Net.Data.Web
{
    public class OrdenMantenimientoSapRepository : RepositoryBase<OrdenMantenimientoEntity>, IOrdenMantenimientoWebRepository
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
        const string SP_GET_LIST_BY_FECHA_IDESTADO_NUMERO = DB_ESQUEMA + "FIB_WEB_SP_PROD_GetListOrdenMatenimientoByFechaAndIdEstadoAndNumero";


        public OrdenMantenimientoSapRepository(IConnectionSql context, IConfiguration configuration)
            : base(context)
        {
            _cnxSap = Utilidades.GetExtraerCadenaConexion(configuration, "ParametersConectionSap");
            _configuration = configuration;
            _aplicacionName = GetType().Name;
        }

        public async Task<ResultadoTransaccion<OrdenMantenimientoByFechaAndIdEstadoAndNumeroEntity>> GetListOrdenMatenimientoByFechaAndIdEstadoAndNumero(DateTime? fecInicial, DateTime? fecFinal, string idEstado, string numero)
        {
            var response = new List<OrdenMantenimientoByFechaAndIdEstadoAndNumeroEntity>();
            var resultadoTransaccion = new ResultadoTransaccion<OrdenMantenimientoByFechaAndIdEstadoAndNumeroEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_BY_FECHA_IDESTADO_NUMERO, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@FecInicial", fecInicial));
                        cmd.Parameters.Add(new SqlParameter("@FecFinal", fecFinal));
                        cmd.Parameters.Add(new SqlParameter("@IdEstado", idEstado));
                        cmd.Parameters.Add(new SqlParameter("@Numero", numero));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<OrdenMantenimientoByFechaAndIdEstadoAndNumeroEntity>)context.ConvertTo<OrdenMantenimientoByFechaAndIdEstadoAndNumeroEntity>(reader);
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

        public async Task<ResultadoTransaccion<MemoryStream>> GetOrdenMatenimientoExcelByFechaAndIdEstadoAndNumero(DateTime? fecInicial, DateTime? fecFinal, string idEstado, string numero)
        {
            var ms = new MemoryStream();
            var response = new List<OrdenMantenimientoByFechaAndIdEstadoAndNumeroEntity>();
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
                    Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Órden de Mantenimiento" };
                    sheets.Append(sheet);

                    workbookPart.Workbook.Save();

                    SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

                    //Cabecera
                    Row row = new Row();
                    row.Append(
                    ExportToExcel.ConstructCell("Numero Órden Mantenimiento", CellValues.String),
                    ExportToExcel.ConstructCell("Fecha Inicio", CellValues.String),
                    ExportToExcel.ConstructCell("Fecha Fin", CellValues.String),
                    ExportToExcel.ConstructCell("Hora Inicio", CellValues.String),
                    ExportToExcel.ConstructCell("Hora Fin", CellValues.String),
                    ExportToExcel.ConstructCell("NomTipoServicio", CellValues.String),
                    ExportToExcel.ConstructCell("NomArea", CellValues.String),
                    ExportToExcel.ConstructCell("NomMaquina", CellValues.String),
                    ExportToExcel.ConstructCell("NomParte", CellValues.String),
                    ExportToExcel.ConstructCell("NomSubParte", CellValues.String),
                    ExportToExcel.ConstructCell("NomTecnico", CellValues.String),
                    ExportToExcel.ConstructCell("Descripcion", CellValues.String),
                    ExportToExcel.ConstructCell("ActividadRealizada", CellValues.String),
                    ExportToExcel.ConstructCell("OtrosDestalles", CellValues.String),
                    ExportToExcel.ConstructCell("NomSede", CellValues.String),
                    ExportToExcel.ConstructCell("NomSolicitante", CellValues.String),
                    ExportToExcel.ConstructCell("PuestoSolicitante", CellValues.String),
                    ExportToExcel.ConstructCell("FechaEmision", CellValues.String),
                    ExportToExcel.ConstructCell("NomEstado", CellValues.String));
                    sheetData.AppendChild(row);

                    using (SqlConnection conn = new SqlConnection(_cnxSap))
                    {
                        conn.Open();

                        using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_BY_FECHA_IDESTADO_NUMERO, conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 0;
                            cmd.Parameters.Add(new SqlParameter("@FecInicial", fecInicial));
                            cmd.Parameters.Add(new SqlParameter("@FecFinal", fecFinal));
                            cmd.Parameters.Add(new SqlParameter("@IdEstado", idEstado));
                            cmd.Parameters.Add(new SqlParameter("@Numero", numero));

                            using (var reader = await cmd.ExecuteReaderAsync())
                            {
                                response = (List<OrdenMantenimientoByFechaAndIdEstadoAndNumeroEntity>)context.ConvertTo<OrdenMantenimientoByFechaAndIdEstadoAndNumeroEntity>(reader);
                            }
                        }
                    }

                    //Contenido
                    foreach (var item in response)
                    {
                        row = new Row();
                        row.Append(
                        ExportToExcel.ConstructCell(item.IdOrdenMantenimiento.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.FecInicio.ToString("dd/MM/yyyy"), CellValues.String),
                        ExportToExcel.ConstructCell(item.FecFin.ToString("dd/MM/yyyy"), CellValues.String),
                        ExportToExcel.ConstructCell(item.HoraInicio, CellValues.String),
                        ExportToExcel.ConstructCell(item.HoraFin, CellValues.String),
                        ExportToExcel.ConstructCell(item.NomTipoServicio, CellValues.String),
                        ExportToExcel.ConstructCell(item.NomArea, CellValues.String),
                        ExportToExcel.ConstructCell(item.NomMaquina, CellValues.String),
                        ExportToExcel.ConstructCell(item.NomParte, CellValues.String),
                        ExportToExcel.ConstructCell(item.NomSubParte, CellValues.String),
                        ExportToExcel.ConstructCell(item.NomTecnico, CellValues.String),
                        ExportToExcel.ConstructCell(item.Descripcion, CellValues.String),
                        ExportToExcel.ConstructCell(item.ActividadRealizada, CellValues.String),
                        ExportToExcel.ConstructCell(item.OtrosDestalles, CellValues.String),
                        ExportToExcel.ConstructCell(item.NomSede, CellValues.String),
                        ExportToExcel.ConstructCell(item.NomSolicitante, CellValues.String),
                        ExportToExcel.ConstructCell(item.PuestoSolicitante, CellValues.String),
                        ExportToExcel.ConstructCell(item.FechaEmision.ToString("dd/MM/yyyy"), CellValues.String),
                        ExportToExcel.ConstructCell(item.NomEstado, CellValues.String));
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
