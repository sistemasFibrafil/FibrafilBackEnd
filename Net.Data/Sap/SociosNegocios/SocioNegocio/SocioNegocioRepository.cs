using System;
using System.IO;
using System.Data;
using Net.Connection;
using Net.CrossCotting;
using Net.Business.Entities;
using DocumentFormat.OpenXml;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Net.Business.Entities.Sap;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Packaging;
using Microsoft.Extensions.Configuration;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Net.Data.Sap
{
    public class SocioNegocioRepository : RepositoryBase<SocioNegocioSapEntity>, ISocioNegocioRepository
    {
        private string _metodoName;
        private string _aplicacionName;
        private readonly Regex regex = new Regex(@"<(\w+)>.*");

        // PARAMETROS DE COXIÓN
        private readonly string _cnxSap;
        private readonly IConfiguration _configuration;

        // STORED PROCEDURE
        const string DB_ESQUEMA = "";
        const string SP_GET_LIST_BY_FILTRO = DB_ESQUEMA + "WEB_NEG_SP_GetListSocioNegocioByFiltro";
        const string SP_GET_BY_CARDCODE = DB_ESQUEMA + "WEB_NEG_SP_GetSocioNegocioByCode";
        const string SP_GET_LIST_BY_SECTOR_ESTADO = DB_ESQUEMA + "WEB_NEG_SP_GetLitClienteBySectorEstado";

        public SocioNegocioRepository(IConnectionSql context, IConfiguration configuration)
            : base(context)
        {
            _cnxSap = Utilidades.GetExtraerCadenaConexion(configuration, "ParametersConectionSap");
            _configuration = configuration;
            _aplicacionName = GetType().Name;
        }

        public async Task<ResultadoTransaccion<SocioNegocioSapEntity>> GetListByFiltro(FiltroRequestEntity value)
        {
            var response = new List<SocioNegocioSapEntity>();
            var resultadoTransaccion = new ResultadoTransaccion<SocioNegocioSapEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_BY_FILTRO, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@CardType", value.Code1));
                        cmd.Parameters.Add(new SqlParameter("@TransType", value.Code2));
                        cmd.Parameters.Add(new SqlParameter("@Filtro", value.TextFiltro1));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<SocioNegocioSapEntity>)context.ConvertTo<SocioNegocioSapEntity>(reader);
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

        public async Task<ResultadoTransaccion<SocioNegocioSapEntity>> GetByCardCode(string cardCode)
        {
            var response = new SocioNegocioSapEntity();
            var resultadoTransaccion = new ResultadoTransaccion<SocioNegocioSapEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_BY_CARDCODE, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@CardCode", cardCode));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = context.Convert<SocioNegocioSapEntity>(reader);
                        }
                    }

                    resultadoTransaccion.IdRegistro = 0;
                    resultadoTransaccion.ResultadoCodigo = 0;
                    resultadoTransaccion.ResultadoDescripcion = "Dato obtenido con éxito.";
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

        public async Task<ResultadoTransaccion<SocioNegocioSapEntity>> GetLitClienteBySectorEstado(string sector, string estado, string filtro)
        {
            var response = new List<SocioNegocioSapEntity>();
            var resultadoTransaccion = new ResultadoTransaccion<SocioNegocioSapEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_BY_SECTOR_ESTADO, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@Sector", sector));
                        cmd.Parameters.Add(new SqlParameter("@Estado", estado));
                        cmd.Parameters.Add(new SqlParameter("@Filtro", filtro));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<SocioNegocioSapEntity>)context.ConvertTo<SocioNegocioSapEntity>(reader);
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

        public async Task<ResultadoTransaccion<MemoryStream>> GetLitClienteExcelBySectorEstado(string sector, string estado, string filtro)
        {
            var ms = new MemoryStream();
            var response = new List<SocioNegocioSapEntity>();
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
                    Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Facturas de ventas" };
                    sheets.Append(sheet);

                    workbookPart.Workbook.Save();

                    SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

                    //Cabecera
                    Row row = new Row();
                    row.Append(
                    ExportToExcel.ConstructCell("Código de Cliente", CellValues.String),
                    ExportToExcel.ConstructCell("RUC", CellValues.String),
                    ExportToExcel.ConstructCell("Nombre de Cliente", CellValues.String),
                    ExportToExcel.ConstructCell("Vendedor", CellValues.String),
                    ExportToExcel.ConstructCell("Direccion", CellValues.String),
                    ExportToExcel.ConstructCell("Sector", CellValues.String),
                    ExportToExcel.ConstructCell("División", CellValues.String),
                    ExportToExcel.ConstructCell("País", CellValues.String),
                    ExportToExcel.ConstructCell("Departamento", CellValues.String),
                    ExportToExcel.ConstructCell("Provincia", CellValues.String),
                    ExportToExcel.ConstructCell("Distrito", CellValues.String),
                    ExportToExcel.ConstructCell("Ubigeo", CellValues.String),
                    ExportToExcel.ConstructCell("Contacto", CellValues.String),
                    ExportToExcel.ConstructCell("Teléfono de Contacto", CellValues.String),
                    ExportToExcel.ConstructCell("Fecha de Alta", CellValues.String),
                    ExportToExcel.ConstructCell("Fecha de Baja", CellValues.String),
                    ExportToExcel.ConstructCell("Fecha de Última Venta", CellValues.String),
                    ExportToExcel.ConstructCell("Estado", CellValues.String));
                    sheetData.AppendChild(row);

                    using (SqlConnection conn = new SqlConnection(_cnxSap))
                    {
                        conn.Open();

                        using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_BY_SECTOR_ESTADO, conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 0;
                            cmd.Parameters.Add(new SqlParameter("@Sector", sector));
                            cmd.Parameters.Add(new SqlParameter("@Estado", estado));
                            cmd.Parameters.Add(new SqlParameter("@Filtro", filtro));

                            using (var reader = await cmd.ExecuteReaderAsync())
                            {
                                response = (List<SocioNegocioSapEntity>)context.ConvertTo<SocioNegocioSapEntity>(reader);
                            }
                        }
                    }

                    //Contenido
                    foreach (var item in response)
                    {
                        row = new Row();
                        row.Append(
                        ExportToExcel.ConstructCell(item.CardCode, CellValues.String),
                        ExportToExcel.ConstructCell(item.LicTradNum, CellValues.String),
                        ExportToExcel.ConstructCell(item.CardName, CellValues.String),
                        ExportToExcel.ConstructCell(item.SlpName, CellValues.String),
                        ExportToExcel.ConstructCell(item.Address, CellValues.String),
                        ExportToExcel.ConstructCell(item.NomSector, CellValues.String),
                        ExportToExcel.ConstructCell(item.NomDivision, CellValues.String),
                        ExportToExcel.ConstructCell(item.Pais, CellValues.String),
                        ExportToExcel.ConstructCell(item.NomDepartamento, CellValues.String),
                        ExportToExcel.ConstructCell(item.NomProvincia, CellValues.String),
                        ExportToExcel.ConstructCell(item.NomDistrito, CellValues.String),
                        ExportToExcel.ConstructCell(item.Ubigeo, CellValues.String),
                        ExportToExcel.ConstructCell(item.NomContacto, CellValues.String),
                        ExportToExcel.ConstructCell(item.TelContacto, CellValues.String),
                        ExportToExcel.ConstructCell(item.CreateDate.ToString("dd/MM/yyyy"), CellValues.String),
                        ExportToExcel.ConstructCell(item.LowDate == null ? null : Convert.ToDateTime(item.LowDate).ToString("dd/MM/yyyy"), CellValues.String),
                        ExportToExcel.ConstructCell(item.FechaUltimaVenta == null ? null : Convert.ToDateTime(item.FechaUltimaVenta).ToString("dd/MM/yyyy"), CellValues.String),
                        ExportToExcel.ConstructCell(item.NomStatus, CellValues.String));
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
