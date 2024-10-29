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
namespace Net.Data.Sap
{
    public class FacturaVentaSapRepository : RepositoryBase<FacturaVentaEntity>, IFacturaVentaSapRepository
    {
        private string _metodoName;
        private string _aplicacionName;
        private readonly Regex regex = new Regex(@"<(\w+)>.*");

        // PARAMETROS DE COXIÓN
        private readonly string _cnxSap;
        private readonly IConfiguration _configuration;

        // STORED PROCEDURE
        const string DB_ESQUEMA = "";
        const string SP_GET_LIST_VENTA_BY_FECHA_AND_SLPCODE = DB_ESQUEMA + "WEB_VEN_SP_GetLitVentaByFechaAndSlpCode";
        const string SP_GET_LIST_VENTA_RESUMEN_BY_FECHA_GRUPO1 = DB_ESQUEMA + "WEB_VE_SP_GetLitVentaResumenByFechaGrupo1";
        const string SP_GET_LIST_VENTA_RESUMEN_BY_FECHA_GRUPO2 = DB_ESQUEMA + "WEB_VE_SP_GetLitVentaResumenByFechaGrupo2";
        const string SP_GET_LIST_VENTA_RESUMEN_BY_FECHA_GRUPO3 = DB_ESQUEMA + "WEB_VE_SP_GetLitVentaResumenByFechaGrupo3";

        const string SP_GET_LIST_FACTURA_VENTA_BY_FECHA = DB_ESQUEMA + "WEB_VEN_SP_GetLitFacturaVentaByFecha";
        const string SP_GET_LIST_VENTA_PROYECCION_BY_FECHA = DB_ESQUEMA + "WEB_VEN_SP_GetListVentaProyeccionByFecha";

        public FacturaVentaSapRepository(IConnectionSql context, IConfiguration configuration)
            : base(context)
        {
            _cnxSap = Utilidades.GetExtraerCadenaConexion(configuration, "ParametersConectionSap");
            _configuration = configuration;
            _aplicacionName = GetType().Name;
        }

        public async Task<ResultadoTransaccion<VentaProyeccionByFechaEntity>> GetListVentaProyeccionByFecha(DateTime fecInicial, DateTime fecFinal)
        {
            var response = new List<VentaProyeccionByFechaEntity>();
            var resultadoTransaccion = new ResultadoTransaccion<VentaProyeccionByFechaEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_VENTA_PROYECCION_BY_FECHA, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@FecIni", fecInicial));
                        cmd.Parameters.Add(new SqlParameter("@FecFin", fecFinal));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<VentaProyeccionByFechaEntity>)context.ConvertTo<VentaProyeccionByFechaEntity>(reader);
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

        public async Task<ResultadoTransaccion<VentaResumenByFechaGrupoEntity>> GetListVentaResumenByFechaGrupo1(DateTime fecInicial, DateTime fecFinal, string grupo)
        {
            var response = new List<VentaResumenByFechaGrupoEntity>();
            var resultadoTransaccion = new ResultadoTransaccion<VentaResumenByFechaGrupoEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_VENTA_RESUMEN_BY_FECHA_GRUPO1, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@FecIni", fecInicial));
                        cmd.Parameters.Add(new SqlParameter("@FecFin", fecFinal));
                        cmd.Parameters.Add(new SqlParameter("@Grupo", grupo));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<VentaResumenByFechaGrupoEntity>)context.ConvertTo<VentaResumenByFechaGrupoEntity>(reader);
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

        public async Task<ResultadoTransaccion<VentaResumenByFechaGrupoEntity>> GetListVentaResumenByFechaGrupo2(DateTime fecInicial, DateTime fecFinal, string grupo)
        {
            var response = new List<VentaResumenByFechaGrupoEntity>();
            var resultadoTransaccion = new ResultadoTransaccion<VentaResumenByFechaGrupoEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_VENTA_RESUMEN_BY_FECHA_GRUPO2, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@FecIni", fecInicial));
                        cmd.Parameters.Add(new SqlParameter("@FecFin", fecFinal));
                        cmd.Parameters.Add(new SqlParameter("@Grupo", grupo));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<VentaResumenByFechaGrupoEntity>)context.ConvertTo<VentaResumenByFechaGrupoEntity>(reader);
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

        public async Task<ResultadoTransaccion<VentaResumenByFechaGrupoEntity>> GetListVentaResumenByFechaGrupo3(DateTime fecInicial, DateTime fecFinal, string grupo)
        {
            var response = new List<VentaResumenByFechaGrupoEntity>();
            var resultadoTransaccion = new ResultadoTransaccion<VentaResumenByFechaGrupoEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_VENTA_RESUMEN_BY_FECHA_GRUPO3, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@FecIni", fecInicial));
                        cmd.Parameters.Add(new SqlParameter("@FecFin", fecFinal));
                        cmd.Parameters.Add(new SqlParameter("@Grupo", grupo));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<VentaResumenByFechaGrupoEntity>)context.ConvertTo<VentaResumenByFechaGrupoEntity>(reader);
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

        public async Task<ResultadoTransaccion<MemoryStream>> GetVentaResumenExcelByFechaGrupo(DateTime fecInicial, DateTime fecFinal, string grupo)
        {
            var ms = new MemoryStream();
            var lista1 = new List<VentaResumenByFechaGrupoEntity>();
            var lista2 = new List<VentaResumenByFechaGrupoEntity>();
            var lista3 = new List<VentaResumenByFechaGrupoEntity>();
            var resultadoTransaccion = new ResultadoTransaccion<MemoryStream>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_VENTA_RESUMEN_BY_FECHA_GRUPO1, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@FecIni", fecInicial));
                        cmd.Parameters.Add(new SqlParameter("@FecFin", fecFinal));
                        cmd.Parameters.Add(new SqlParameter("@Grupo", grupo));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            lista1 = (List<VentaResumenByFechaGrupoEntity>)context.ConvertTo<VentaResumenByFechaGrupoEntity>(reader);
                        }
                    }

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_VENTA_RESUMEN_BY_FECHA_GRUPO2, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@FecIni", fecInicial));
                        cmd.Parameters.Add(new SqlParameter("@FecFin", fecFinal));
                        cmd.Parameters.Add(new SqlParameter("@Grupo", grupo));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            lista2 = (List<VentaResumenByFechaGrupoEntity>)context.ConvertTo<VentaResumenByFechaGrupoEntity>(reader);
                        }
                    }

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_VENTA_RESUMEN_BY_FECHA_GRUPO3, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@FecIni", fecInicial));
                        cmd.Parameters.Add(new SqlParameter("@FecFin", fecFinal));
                        cmd.Parameters.Add(new SqlParameter("@Grupo", grupo));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            lista3 = (List<VentaResumenByFechaGrupoEntity>)context.ConvertTo<VentaResumenByFechaGrupoEntity>(reader);
                        }
                    }
                }

                ms = GetArchivoVentaResumenExcelByFechaGrupo(lista1, lista2, lista3);

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

        private MemoryStream GetArchivoVentaResumenExcelByFechaGrupo(List<VentaResumenByFechaGrupoEntity> lista1, List<VentaResumenByFechaGrupoEntity> lista2, List<VentaResumenByFechaGrupoEntity> lista3)
        {
            var ms = new MemoryStream();

            using (SpreadsheetDocument document = SpreadsheetDocument.Create(ms, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                Sheets sheets = document.WorkbookPart.Workbook.AppendChild(new Sheets());

                SetArchivoVentaResumenExcelByFechaGrupo1(document, workbookPart, sheets, lista1);
                SetArchivoVentaResumenExcelByFechaGrupo2(document, workbookPart, sheets, lista2);
                SetArchivoVentaResumenExcelByFechaGrupo3(document, workbookPart, sheets, lista3);

                workbookPart.Workbook.Save();
                document.Close();
            }

            return ms;
        }

        private void SetArchivoVentaResumenExcelByFechaGrupo1(SpreadsheetDocument document, WorkbookPart workbookPart, Sheets sheets, List<VentaResumenByFechaGrupoEntity> lista)
        {
            WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet();

            Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Vendedor-Grupo" };
            sheets.Append(sheet);

            SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

            //Cabecera
            Row row = new Row();
            row.Append(
            ExportToExcel.ConstructCell("Vendedor", CellValues.String),
            ExportToExcel.ConstructCell("Grupo", CellValues.String),
            ExportToExcel.ConstructCell("Cantidad", CellValues.String),
            ExportToExcel.ConstructCell("Total USD", CellValues.String));
            sheetData.AppendChild(row);

            //Contenido
            foreach (var item in lista)
            {
                row = new Row();
                row.Append(
                ExportToExcel.ConstructCell(item.NomVendedor, CellValues.String),
                ExportToExcel.ConstructCell(item.NomGrupo, CellValues.String),
                ExportToExcel.ConstructCell(item.Cantidad.ToString(), CellValues.Number),
                ExportToExcel.ConstructCell(item.TotalItemUSD.ToString(), CellValues.Number));
                sheetData.Append(row);
            }
        }
        
        private void SetArchivoVentaResumenExcelByFechaGrupo2(SpreadsheetDocument document, WorkbookPart workbookPart, Sheets sheets, List<VentaResumenByFechaGrupoEntity> lista)
        {
            WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet();

            Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 2, Name = "Vendedor-Grupo-UM" };
            sheets.Append(sheet);

            SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

            //Cabecera
            Row row = new Row();
            row.Append(
            ExportToExcel.ConstructCell("Vendedor", CellValues.String),
            ExportToExcel.ConstructCell("Grupo", CellValues.String),
            ExportToExcel.ConstructCell("Unidad de Medida", CellValues.String),
            ExportToExcel.ConstructCell("Cantidad", CellValues.String),
            ExportToExcel.ConstructCell("Total USD", CellValues.String));
            sheetData.AppendChild(row);

            //Contenido
            foreach (var item in lista)
            {
                row = new Row();
                row.Append(
                ExportToExcel.ConstructCell(item.NomVendedor, CellValues.String),
                ExportToExcel.ConstructCell(item.NomGrupo, CellValues.String),
                ExportToExcel.ConstructCell(item.UnidadMedida, CellValues.String),
                ExportToExcel.ConstructCell(item.Cantidad.ToString(), CellValues.Number),
                ExportToExcel.ConstructCell(item.TotalItemUSD.ToString(), CellValues.Number));
                sheetData.Append(row);
            }
        }
        
        private void SetArchivoVentaResumenExcelByFechaGrupo3(SpreadsheetDocument document, WorkbookPart workbookPart, Sheets sheets, List<VentaResumenByFechaGrupoEntity> lista)
        {
            WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet();

            Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 3, Name = "Vendedor-Grupo-Artículo-UM" };
            sheets.Append(sheet);

            SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

            //Cabecera
            Row row = new Row();
            row.Append(
            ExportToExcel.ConstructCell("Vendedor", CellValues.String),
            ExportToExcel.ConstructCell("Grupo", CellValues.String),
            ExportToExcel.ConstructCell("Artículo", CellValues.String),
            ExportToExcel.ConstructCell("Unidad de Medida", CellValues.String),
            ExportToExcel.ConstructCell("Cantidad", CellValues.String),
            ExportToExcel.ConstructCell("Total USD", CellValues.String));
            sheetData.AppendChild(row);

            //Contenido
            foreach (var item in lista)
            {
                row = new Row();
                row.Append(
                ExportToExcel.ConstructCell(item.NomVendedor, CellValues.String),
                ExportToExcel.ConstructCell(item.NomGrupo, CellValues.String),
                ExportToExcel.ConstructCell(item.ItemName, CellValues.String),
                ExportToExcel.ConstructCell(item.UnidadMedida, CellValues.String),
                ExportToExcel.ConstructCell(item.Cantidad.ToString(), CellValues.Number),
                ExportToExcel.ConstructCell(item.TotalItemUSD.ToString(), CellValues.Number));
                sheetData.Append(row);
            }
        }

        public async Task<ResultadoTransaccion<VentaByFechaSlpCodeEntity>> GetListVentaByFechaAndSlpCode(FiltroRequestEntity value)
        {
            var response = new List<VentaByFechaSlpCodeEntity>();
            var resultadoTransaccion = new ResultadoTransaccion<VentaByFechaSlpCodeEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_VENTA_BY_FECHA_AND_SLPCODE, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@FecInicial", value.Fecha1));
                        cmd.Parameters.Add(new SqlParameter("@FecFinal", value.Fecha2));
                        cmd.Parameters.Add(new SqlParameter("@SlpCode", value.Code1));
                        cmd.Parameters.Add(new SqlParameter("@CardName", value.TextFiltro1));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<VentaByFechaSlpCodeEntity>)context.ConvertTo<VentaByFechaSlpCodeEntity>(reader);
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

        public async Task<ResultadoTransaccion<MemoryStream>> GetListVentaExcelByFechaAndSlpCode(FiltroRequestEntity value)
        {
            var ms = new MemoryStream();
            var response = new List<VentaByFechaSlpCodeEntity>();
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
                    ExportToExcel.ConstructCell("Ubidad de Negocio", CellValues.String),
                    ExportToExcel.ConstructCell("Empresa Asociada", CellValues.String),
                    ExportToExcel.ConstructCell("Código de Cliente", CellValues.String),
                    ExportToExcel.ConstructCell("Nombre de Cliente", CellValues.String),
                    ExportToExcel.ConstructCell("División", CellValues.String),
                    ExportToExcel.ConstructCell("Sector", CellValues.String),
                    ExportToExcel.ConstructCell("Pais", CellValues.String),
                    ExportToExcel.ConstructCell("Departamento", CellValues.String),
                    ExportToExcel.ConstructCell("Provincia", CellValues.String),
                    ExportToExcel.ConstructCell("Cuidad", CellValues.String),

                    ExportToExcel.ConstructCell("Tipo de Documento", CellValues.String),
                    ExportToExcel.ConstructCell("Fecha de Contabilización", CellValues.String),
                    ExportToExcel.ConstructCell("Número de Documento", CellValues.String),
                    ExportToExcel.ConstructCell("Número de Guía", CellValues.String),
                    ExportToExcel.ConstructCell("Número de Pedido", CellValues.String),
                    ExportToExcel.ConstructCell("Fecha de Pedido", CellValues.String),
                    ExportToExcel.ConstructCell("Vendedor", CellValues.String),
                    ExportToExcel.ConstructCell("Codicion de Pago", CellValues.String),

                    ExportToExcel.ConstructCell("Código de Artículo", CellValues.String),
                    ExportToExcel.ConstructCell("Nombre de Artículo", CellValues.String),
                    ExportToExcel.ConstructCell("Grupo", CellValues.String),
                    ExportToExcel.ConstructCell("Forcast", CellValues.String),
                    ExportToExcel.ConstructCell("SubGrupo", CellValues.String),
                    ExportToExcel.ConstructCell("SubGrupo 2", CellValues.String),
                    ExportToExcel.ConstructCell("Medida", CellValues.String),
                    ExportToExcel.ConstructCell("Color", CellValues.String),
                    ExportToExcel.ConstructCell("Porcentaje", CellValues.String),

                    ExportToExcel.ConstructCell("UM", CellValues.String),
                    ExportToExcel.ConstructCell("Cantidad", CellValues.String),
                    ExportToExcel.ConstructCell("PesoItem", CellValues.String),
                    ExportToExcel.ConstructCell("PesoP romedio Kg", CellValues.String),
                    ExportToExcel.ConstructCell("Peso VentaKg", CellValues.String),
                    ExportToExcel.ConstructCell("Peso", CellValues.String),

                    ExportToExcel.ConstructCell("Rollo Vendido", CellValues.String),
                    ExportToExcel.ConstructCell("Kg Vendido", CellValues.String),
                    ExportToExcel.ConstructCell("Tonelada Vendida", CellValues.String),

                    ExportToExcel.ConstructCell("Moneda", CellValues.String),
                    ExportToExcel.ConstructCell("TC", CellValues.String),
                    ExportToExcel.ConstructCell("Precio", CellValues.String),
                    ExportToExcel.ConstructCell("Precio Kg", CellValues.String),
                    ExportToExcel.ConstructCell("Costo SOL", CellValues.String),
                    ExportToExcel.ConstructCell("Costo USD", CellValues.String),

                    ExportToExcel.ConstructCell("Total Costo Item SOL", CellValues.String),
                    ExportToExcel.ConstructCell("Total Costo Item USD", CellValues.String),

                    ExportToExcel.ConstructCell("Total Item SOL", CellValues.String),
                    ExportToExcel.ConstructCell("Total Item USD", CellValues.String),
                    ExportToExcel.ConstructCell("Sede", CellValues.String),
                    ExportToExcel.ConstructCell("Ubigeo", CellValues.String));
                    sheetData.AppendChild(row);

                    using (SqlConnection conn = new SqlConnection(_cnxSap))
                    {
                        conn.Open();

                        using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_VENTA_BY_FECHA_AND_SLPCODE, conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 0;
                            cmd.Parameters.Add(new SqlParameter("@FecInicial", value.Fecha1));
                            cmd.Parameters.Add(new SqlParameter("@FecFinal", value.Fecha2));
                            cmd.Parameters.Add(new SqlParameter("@SlpCode", value.Code1));
                            cmd.Parameters.Add(new SqlParameter("@CardName", value.TextFiltro1));

                            using (var reader = await cmd.ExecuteReaderAsync())
                            {
                                response = (List<VentaByFechaSlpCodeEntity>)context.ConvertTo<VentaByFechaSlpCodeEntity>(reader);
                            }
                        }
                    }

                    //Contenido
                    foreach (var item in response)
                    {
                        row = new Row();
                        row.Append(
                        ExportToExcel.ConstructCell(item.UnidadNegocio, CellValues.String),
                        ExportToExcel.ConstructCell(item.EmpresaAsociada, CellValues.String),
                        ExportToExcel.ConstructCell(item.CardCode, CellValues.String),
                        ExportToExcel.ConstructCell(item.CardName, CellValues.String),
                        ExportToExcel.ConstructCell(item.Division, CellValues.String),
                        ExportToExcel.ConstructCell(item.Sector, CellValues.String),
                        ExportToExcel.ConstructCell(item.Pais, CellValues.String),
                        ExportToExcel.ConstructCell(item.Departamento, CellValues.String),
                        ExportToExcel.ConstructCell(item.Provincia, CellValues.String),
                        ExportToExcel.ConstructCell(item.Cuidad, CellValues.String),

                        ExportToExcel.ConstructCell(item.TipoDocumento, CellValues.String),
                        ExportToExcel.ConstructCell(item.FecContabilizacion.ToString("dd/MM/yyyy"), CellValues.String),
                        ExportToExcel.ConstructCell(item.NumeroDocumento, CellValues.String),
                        ExportToExcel.ConstructCell(item.NumeroGuia, CellValues.String),
                        ExportToExcel.ConstructCell(item.NumeroPedido.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.FechaPedido == null? "" : Convert.ToDateTime(item.FechaPedido).ToString("dd/MM/yyyy"), CellValues.String),
                        ExportToExcel.ConstructCell(item.NomVendedor, CellValues.String),
                        ExportToExcel.ConstructCell(item.NomCondicionPago, CellValues.String),

                        ExportToExcel.ConstructCell(item.ItemCode, CellValues.String),
                        ExportToExcel.ConstructCell(item.ItemName, CellValues.String),
                        ExportToExcel.ConstructCell(item.NomGrupo, CellValues.String),
                        ExportToExcel.ConstructCell(item.Forcast.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.NomSubGrupo, CellValues.String),
                        ExportToExcel.ConstructCell(item.NomSubGrupo2, CellValues.String),
                        ExportToExcel.ConstructCell(item.Medida, CellValues.String),
                        ExportToExcel.ConstructCell(item.Color, CellValues.String),
                        ExportToExcel.ConstructCell(item.Porcentaje, CellValues.String),

                        ExportToExcel.ConstructCell(item.UnidadMedida, CellValues.String),
                        ExportToExcel.ConstructCell(item.Cantidad.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.PesoItem.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.PesoPromedioKg.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.PesoVentaKg.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.Peso.ToString(), CellValues.Number),

                        ExportToExcel.ConstructCell(item.RolloVendido.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.KgVendido.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.ToneladaVendida.ToString(), CellValues.Number),

                        ExportToExcel.ConstructCell(item.CodMoneda, CellValues.String),
                        ExportToExcel.ConstructCell(item.TipoCambio.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.Precio.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.PrecioKg.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.CostoSOL.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.CostoUSD.ToString(), CellValues.Number),

                        ExportToExcel.ConstructCell(item.TotalCostoItemSOL.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.TotalCostoItemUSD.ToString(), CellValues.Number),

                        ExportToExcel.ConstructCell(item.TotalItemSOL.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.TotalItemUSD.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.Sede, CellValues.String),
                        ExportToExcel.ConstructCell(item.Ubigeo, CellValues.String));
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

        public async Task<ResultadoTransaccion<FacturaVentaByFechaEntity>> GetListFacturaVentaByFecha(FiltroRequestEntity value)
        {
            var response = new List<FacturaVentaByFechaEntity>();
            var resultadoTransaccion = new ResultadoTransaccion<FacturaVentaByFechaEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_FACTURA_VENTA_BY_FECHA, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@FecInicial", value.Fecha1));
                        cmd.Parameters.Add(new SqlParameter("@FecFinal", value.Fecha2));
                        cmd.Parameters.Add(new SqlParameter("@Filtro", value.TextFiltro1));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<FacturaVentaByFechaEntity>)context.ConvertTo<FacturaVentaByFechaEntity>(reader);
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

        public async Task<ResultadoTransaccion<MemoryStream>> GetListFacturaVentaExcelByFecha(FiltroRequestEntity value)
        {
            var ms = new MemoryStream();
            var response = new List<FacturaVentaByFechaEntity>();
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
                    ExportToExcel.ConstructCell("Nombre de Cliente", CellValues.String),
                    ExportToExcel.ConstructCell("Fecha de Contabilización", CellValues.String),
                    ExportToExcel.ConstructCell("Fecha de Vencimiento", CellValues.String),
                    ExportToExcel.ConstructCell("Días Vencidas", CellValues.String),
                    ExportToExcel.ConstructCell("Número de Documento", CellValues.String),
                    ExportToExcel.ConstructCell("Vendedor", CellValues.String),
                    ExportToExcel.ConstructCell("Moneda", CellValues.String),
                    ExportToExcel.ConstructCell("Total", CellValues.String),
                    ExportToExcel.ConstructCell("Cobrado", CellValues.String),
                    ExportToExcel.ConstructCell("Saldo", CellValues.String));
                    sheetData.AppendChild(row);

                    using (SqlConnection conn = new SqlConnection(_cnxSap))
                    {
                        conn.Open();

                        using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_FACTURA_VENTA_BY_FECHA, conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 0;
                            cmd.Parameters.Add(new SqlParameter("@FecInicial", value.Fecha1));
                            cmd.Parameters.Add(new SqlParameter("@FecFinal", value.Fecha2));
                            cmd.Parameters.Add(new SqlParameter("@Filtro", value.TextFiltro1));

                            using (var reader = await cmd.ExecuteReaderAsync())
                            {
                                response = (List<FacturaVentaByFechaEntity>)context.ConvertTo<FacturaVentaByFechaEntity>(reader);
                            }
                        }
                    }

                    //Contenido
                    foreach (var item in response)
                    {
                        row = new Row();
                        row.Append(
                        ExportToExcel.ConstructCell(item.CardName, CellValues.String),
                        ExportToExcel.ConstructCell(item.FecContabilizacion.ToString("dd/MM/yyyy"), CellValues.String),
                        ExportToExcel.ConstructCell(item.FecContabilizacion.ToString("dd/MM/yyyy"), CellValues.String),
                        ExportToExcel.ConstructCell(item.DiaVencido.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.NumeroDocumento, CellValues.String),
                        ExportToExcel.ConstructCell(item.NomVendedor, CellValues.String),
                        ExportToExcel.ConstructCell(item.CodMoneda, CellValues.String),
                        ExportToExcel.ConstructCell(item.Total.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.Cobrado.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.Saldo.ToString(), CellValues.Number));
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
