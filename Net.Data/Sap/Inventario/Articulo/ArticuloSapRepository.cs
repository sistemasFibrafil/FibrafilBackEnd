using System;
using System.IO;
using System.Linq;
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
    public class ArticuloSapRepository : RepositoryBase<ArticuloSapEntity>, IArticuloSapRepository
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
        const string SP_GET_ARTICULO_BY_CODE = DB_ESQUEMA + "WEB_INV_SP_GetArticuloByCode";
        const string SP_GET_LIST_ARTICULO_BY_FILTRO = DB_ESQUEMA + "WEB_INV_SP_GetListArticuloByFiltro";
        const string SP_GET_LIST_STOCK_GENERAL_BY_ALMACEN = DB_ESQUEMA + "WEB_INV_SP_GetListStockGeneralByAlmacen";
        const string SP_GET_LIST_STOCK_GENERAL_DETALLADO_ALMACEN_BY_ALMACEN = DB_ESQUEMA + "WEB_INV_SP_GetListStockGeneralDetalladoAlmacenByAlmacen";
        const string SP_GET_LIST_ARTICULO_VENTA_STOCK_BY_GRUPO_SUBGRUPO = DB_ESQUEMA + "FIB_WEB_INV_SP_GetListArticuloVentaStockByGrupoSubGrupo";
        const string SP_GET_LIST_ARTICULO_VENTA_GRUPO_SUBGRUPO_ESTADO = DB_ESQUEMA + "FIB_WEB_INV_SP_GetListArticuloVentaByGrupoSubGrupoEstado";
        const string SP_GET_LIST_MOVIMIENTO_STOCK_BY_FECHA_SEDE = DB_ESQUEMA + "FIB_WEB_INV_SP_GetListMovimientoStockByFechaSede";
        const string SP_GET_VENTA_BY_CODE = DB_ESQUEMA + "WEB_INV_SP_GetArticuloVentaByCode";

        const string SP_GET_FOR_ORDEN_VENTA_SODIMAC_SKU = DB_ESQUEMA + "VEN_SP_GetArticuloForOrdenVentaSodimacBySku";


        public ArticuloSapRepository(IConnectionSql context, IConfiguration configuration)
            : base(context)
        {
            _cnxSap = Utilidades.GetExtraerCadenaConexion(configuration, "ParametersConectionSap");
            _configuration = configuration;
            _aplicacionName = GetType().Name;
        }

        public async Task<ResultadoTransaccion<ArticuloSapEntity>> GetListByFiltro(FiltroRequestEntity value)
        {
            var response = new List<ArticuloSapEntity>();
            var resultadoTransaccion = new ResultadoTransaccion<ArticuloSapEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_ARTICULO_BY_FILTRO, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@ArtInv", value.Code1));
                        cmd.Parameters.Add(new SqlParameter("@ArtVen", value.Code2));
                        cmd.Parameters.Add(new SqlParameter("@ArtCom", value.Code3));
                        cmd.Parameters.Add(new SqlParameter("@Filtro", value.TextFiltro1));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<ArticuloSapEntity>)context.ConvertTo<ArticuloSapEntity>(reader);
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

        public async Task<ResultadoTransaccion<ArticuloSapEntity>> GetByCode(string itemCode)
        {
            var response = new ArticuloSapEntity();
            var resultadoTransaccion = new ResultadoTransaccion<ArticuloSapEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_ARTICULO_BY_CODE, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@ItemCode", itemCode));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = context.Convert<ArticuloSapEntity>(reader);
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

        public async Task<ResultadoTransaccion<MovimientoStockSapByFechaSedeEntity>> GetListMovimientoStockByFechaSede(FiltroRequestEntity value)
        {
            var response = new List<MovimientoStockSapByFechaSedeEntity>();
            var resultadoTransaccion = new ResultadoTransaccion<MovimientoStockSapByFechaSedeEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_MOVIMIENTO_STOCK_BY_FECHA_SEDE, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@FecIni", value.Fecha1));
                        cmd.Parameters.Add(new SqlParameter("@FecFin", value.Fecha2));
                        cmd.Parameters.Add(new SqlParameter("@Location", value.Code1));
                        cmd.Parameters.Add(new SqlParameter("@TipMovimiento", value.Code2));
                        cmd.Parameters.Add(new SqlParameter("@Filtro", value.TextFiltro1));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<MovimientoStockSapByFechaSedeEntity>)context.ConvertTo<MovimientoStockSapByFechaSedeEntity>(reader);
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

        public async Task<ResultadoTransaccion<MemoryStream>> GetListMovimientoStockExcelByFechaSede(FiltroRequestEntity value)
        {
            var ms = new MemoryStream();
            var response = new List<MovimientoStockSapByFechaSedeEntity>();
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
                    Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Movimiento de Stock" };
                    sheets.Append(sheet);

                    workbookPart.Workbook.Save();

                    SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

                    //Cabecera
                    Row row = new Row();
                    row.Append(
                    ExportToExcel.ConstructCell("Tipo de Movimiento", CellValues.String),
                    ExportToExcel.ConstructCell("Número de Guía SAP", CellValues.String),
                    ExportToExcel.ConstructCell("Número de Guía SUNAT", CellValues.String),
                    ExportToExcel.ConstructCell("Fecha de Guía", CellValues.String),
                    ExportToExcel.ConstructCell("Código de Cliente", CellValues.String),
                    ExportToExcel.ConstructCell("Nombre de Cliente", CellValues.String),
                    ExportToExcel.ConstructCell("Usuario", CellValues.String),
                    ExportToExcel.ConstructCell("Código de Artículo", CellValues.String),
                    ExportToExcel.ConstructCell("Nombre de Artículo", CellValues.String),
                    ExportToExcel.ConstructCell("Sede", CellValues.String),
                    ExportToExcel.ConstructCell("Centro de Costo", CellValues.String),
                    ExportToExcel.ConstructCell("Almacén de Origen", CellValues.String),
                    ExportToExcel.ConstructCell("Almacén de Destino", CellValues.String),
                    ExportToExcel.ConstructCell("Bultos", CellValues.String),
                    ExportToExcel.ConstructCell("Total Kg", CellValues.String),
                    ExportToExcel.ConstructCell("UM", CellValues.String),
                    ExportToExcel.ConstructCell("Cantidad", CellValues.String),
                    ExportToExcel.ConstructCell("Fecha de Pedido", CellValues.String),
                    ExportToExcel.ConstructCell("Número de Pedido", CellValues.String),
                    ExportToExcel.ConstructCell("Número de Fctura SAP", CellValues.String),
                    ExportToExcel.ConstructCell("Número de Fctura SUNAT", CellValues.String),
                    ExportToExcel.ConstructCell("Nombre de Transportista", CellValues.String),
                    ExportToExcel.ConstructCell("RUC de Transportista", CellValues.String),
                    ExportToExcel.ConstructCell("Placa de Transportista", CellValues.String),
                    ExportToExcel.ConstructCell("Nombre de Conductor", CellValues.String),
                    ExportToExcel.ConstructCell("Lincencia de Conductor", CellValues.String));
                    sheetData.AppendChild(row);

                    using (SqlConnection conn = new SqlConnection(_cnxSap))
                    {
                        conn.Open();

                        using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_MOVIMIENTO_STOCK_BY_FECHA_SEDE, conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 0;
                            cmd.Parameters.Add(new SqlParameter("@FecIni", value.Fecha1));
                            cmd.Parameters.Add(new SqlParameter("@FecFin", value.Fecha2));
                            cmd.Parameters.Add(new SqlParameter("@Location", value.Code1));
                            cmd.Parameters.Add(new SqlParameter("@TipMovimiento", value.Code2));
                            cmd.Parameters.Add(new SqlParameter("@Filtro", value.TextFiltro1));

                            using (var reader = await cmd.ExecuteReaderAsync())
                            {
                                response = (List<MovimientoStockSapByFechaSedeEntity>)context.ConvertTo<MovimientoStockSapByFechaSedeEntity>(reader);
                            }
                        }
                    }

                    //Contenido
                    foreach (var item in response)
                    {
                        row = new Row();
                        row.Append(
                        ExportToExcel.ConstructCell(item.NomTipoMovimiento, CellValues.String),
                        ExportToExcel.ConstructCell(item.NumeroGuiaSAP.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.NumeroGuiaSUNAT, CellValues.String),
                        ExportToExcel.ConstructCell(item.DocDate.ToString("dd/MM/yyyy"), CellValues.String),
                        ExportToExcel.ConstructCell(item.CardCode, CellValues.String),
                        ExportToExcel.ConstructCell(item.CardName, CellValues.String),
                        ExportToExcel.ConstructCell(item.Usuario, CellValues.String),
                        ExportToExcel.ConstructCell(item.ItemCode, CellValues.String),
                        ExportToExcel.ConstructCell(item.ItemName, CellValues.String),
                        ExportToExcel.ConstructCell(item.Sede, CellValues.String),
                        ExportToExcel.ConstructCell(item.CentroCosto, CellValues.String),
                        ExportToExcel.ConstructCell(item.AlmacenOrigen, CellValues.String),
                        ExportToExcel.ConstructCell(item.AlmacenDestino, CellValues.String),
                        ExportToExcel.ConstructCell(item.Bulto.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.TotalKg.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.UnidadMedida, CellValues.String),
                        ExportToExcel.ConstructCell(item.Quantity.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.NumeroPedido == null ? null : item.NumeroPedido.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.FechaPedido == null ? null : Convert.ToDateTime(item.FechaPedido).ToString("dd/MM/yyyy"), CellValues.String),
                        ExportToExcel.ConstructCell(item.NumeroFcturaSAP == null ? null : item.NumeroFcturaSAP.ToString(), CellValues.Number),
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

        public async Task<ResultadoTransaccion<ArticuloSapEntity>> GetListStockGeneralByAlmacen(FiltroRequestEntity value)
        {
            var response = new List<ArticuloSapEntity>();
            var resultadoTransaccion = new ResultadoTransaccion<ArticuloSapEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_STOCK_GENERAL_BY_ALMACEN, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@ExcluirInactivo", value.Val1));
                        cmd.Parameters.Add(new SqlParameter("@ExcluirSinStock", value.Val2));
                        cmd.Parameters.Add(new SqlParameter("@Almacen", value.Code1));
                        cmd.Parameters.Add(new SqlParameter("@Filtro", value.TextFiltro1));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<ArticuloSapEntity>)context.ConvertTo<ArticuloSapEntity>(reader);
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

        public async Task<ResultadoTransaccion<MemoryStream>> GetListStockGeneralByAlmacenExcel(FiltroRequestEntity value)
        {
            var ms = new MemoryStream();
            var response = new List<ArticuloSapEntity>();
            var resultadoTransaccion = new ResultadoTransaccion<MemoryStream>();
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
                    Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Stock General" };
                    sheets.Append(sheet);

                    workbookPart.Workbook.Save();

                    SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

                    //Cabecera
                    Row row = new Row();
                    row.Append
                    (
                        ExportToExcel.ConstructCell("Código de Artículo", CellValues.String),
                        ExportToExcel.ConstructCell("Nombre de Artículo", CellValues.String),
                        ExportToExcel.ConstructCell("UM", CellValues.String),
                        ExportToExcel.ConstructCell("Stock", CellValues.String),
                        ExportToExcel.ConstructCell("Comprometido", CellValues.String),
                        ExportToExcel.ConstructCell("Solicitado", CellValues.String),
                        ExportToExcel.ConstructCell("Disponible", CellValues.String),
                        ExportToExcel.ConstructCell("Peso Promedio Kg", CellValues.String),
                        ExportToExcel.ConstructCell("Peso Kg", CellValues.String),
                        ExportToExcel.ConstructCell("Fecha de Producción", CellValues.String)
                    );
                    sheetData.AppendChild(row);

                    using (SqlConnection conn = new SqlConnection(_cnxSap))
                    {
                        conn.Open();

                        using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_STOCK_GENERAL_BY_ALMACEN, conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 0;
                            cmd.Parameters.Add(new SqlParameter("@ExcluirInactivo", value.Val1));
                            cmd.Parameters.Add(new SqlParameter("@ExcluirSinStock", value.Val2));
                            cmd.Parameters.Add(new SqlParameter("@Almacen", value.Code1));
                            cmd.Parameters.Add(new SqlParameter("@Filtro", value.TextFiltro1));

                            using (var reader = await cmd.ExecuteReaderAsync())
                            {
                                response = (List<ArticuloSapEntity>)context.ConvertTo<ArticuloSapEntity>(reader);
                            }
                        }
                    }

                    //Contenido
                    foreach (var item in response)
                    {
                        row = new Row();
                        row.Append
                        (
                            ExportToExcel.ConstructCell(item.ItemCode, CellValues.String),
                            ExportToExcel.ConstructCell(item.ItemName, CellValues.String),
                            ExportToExcel.ConstructCell(item.InvntryUom, CellValues.String),
                            ExportToExcel.ConstructCell(item.OnHand.ToString(), CellValues.Number),
                            ExportToExcel.ConstructCell(item.IsCommited.ToString(), CellValues.Number),
                            ExportToExcel.ConstructCell(item.OnOrder.ToString(), CellValues.Number),
                            ExportToExcel.ConstructCell(item.Available.ToString(), CellValues.Number),
                            ExportToExcel.ConstructCell(item.PesoPromedioKg.ToString(), CellValues.Number),
                            ExportToExcel.ConstructCell(item.PesoKg.ToString(), CellValues.Number),
                            ExportToExcel.ConstructCell(item.FecProduccion == null ? "" : Convert.ToDateTime(item.FecProduccion).ToString("dd/MM/yyyy"), CellValues.String)
                        );
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

        public async Task<ResultadoTransaccion<ArticuloSapEntity>> GetListStockGeneralDetalladoAlmacenByAlmacen(FiltroRequestEntity value)
        {
            var response = new List<ArticuloSapEntity>();
            var resultadoTransaccion = new ResultadoTransaccion<ArticuloSapEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_STOCK_GENERAL_DETALLADO_ALMACEN_BY_ALMACEN, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@ExcluirInactivo", value.Val1));
                        cmd.Parameters.Add(new SqlParameter("@ExcluirSinStock", value.Val2));
                        cmd.Parameters.Add(new SqlParameter("@Almacen", value.Code1));
                        cmd.Parameters.Add(new SqlParameter("@Filtro", value.TextFiltro1));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<ArticuloSapEntity>)context.ConvertTo<ArticuloSapEntity>(reader);
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

        public async Task<ResultadoTransaccion<MemoryStream>> GetListStockGeneralDetalladoAlmacenByAlmacenExcel(FiltroRequestEntity value)
        {
            var ms = new MemoryStream();
            var response = new List<ArticuloSapEntity>();
            var resultadoTransaccion = new ResultadoTransaccion<MemoryStream>();
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
                    Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Stock General Detallado" };
                    sheets.Append(sheet);

                    workbookPart.Workbook.Save();

                    SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

                    //Cabecera
                    Row row = new Row();
                    row.Append
                    (
                        ExportToExcel.ConstructCell("Código de Artículo", CellValues.String),
                        ExportToExcel.ConstructCell("Nombre de Artículo", CellValues.String),
                        ExportToExcel.ConstructCell("Código de Almacén", CellValues.String),
                        ExportToExcel.ConstructCell("Nombre de Almacén", CellValues.String),
                        ExportToExcel.ConstructCell("UM", CellValues.String),
                        ExportToExcel.ConstructCell("Stock", CellValues.String),
                        ExportToExcel.ConstructCell("Comprometido", CellValues.String),
                        ExportToExcel.ConstructCell("Solicitado", CellValues.String),
                        ExportToExcel.ConstructCell("Disponible", CellValues.String),
                        ExportToExcel.ConstructCell("Peso Promedio Kg", CellValues.String),
                        ExportToExcel.ConstructCell("Peso Kg", CellValues.String),
                        ExportToExcel.ConstructCell("Fecha de Producción", CellValues.String)
                    );
                    sheetData.AppendChild(row);

                    using (SqlConnection conn = new SqlConnection(_cnxSap))
                    {
                        conn.Open();

                        using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_STOCK_GENERAL_DETALLADO_ALMACEN_BY_ALMACEN, conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 0;
                            cmd.Parameters.Add(new SqlParameter("@ExcluirInactivo", value.Val1));
                            cmd.Parameters.Add(new SqlParameter("@ExcluirSinStock", value.Val2));
                            cmd.Parameters.Add(new SqlParameter("@Almacen", value.Code1));
                            cmd.Parameters.Add(new SqlParameter("@Filtro", value.TextFiltro1));

                            using (var reader = await cmd.ExecuteReaderAsync())
                            {
                                response = (List<ArticuloSapEntity>)context.ConvertTo<ArticuloSapEntity>(reader);
                            }
                        }
                    }

                    //Contenido
                    foreach (var item in response)
                    {
                        row = new Row();
                        row.Append
                        (
                            ExportToExcel.ConstructCell(item.ItemCode, CellValues.String),
                            ExportToExcel.ConstructCell(item.ItemName, CellValues.String),
                            ExportToExcel.ConstructCell(item.WhsCode, CellValues.String),
                            ExportToExcel.ConstructCell(item.WhsName, CellValues.String),
                            ExportToExcel.ConstructCell(item.InvntryUom, CellValues.String),
                            ExportToExcel.ConstructCell(item.OnOrder.ToString(), CellValues.Number),
                            ExportToExcel.ConstructCell(item.IsCommited.ToString(), CellValues.Number),
                            ExportToExcel.ConstructCell(item.OnOrder.ToString(), CellValues.Number),
                            ExportToExcel.ConstructCell(item.Available.ToString(), CellValues.Number),
                            ExportToExcel.ConstructCell(item.PesoPromedioKg.ToString(), CellValues.Number),
                            ExportToExcel.ConstructCell(item.PesoKg.ToString(), CellValues.Number),
                            ExportToExcel.ConstructCell(item.FecProduccion == null ? "" : Convert.ToDateTime(item.FecProduccion).ToString("dd/MM/yyyy"), CellValues.String)
                        );
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

        public async Task<ResultadoTransaccion<ArticuloVentaStockByGrupoSubGrupo>> GetListArticuloVentaStockByGrupoSubGrupo(FiltroRequestEntity value)
        {
            var response = new List<ArticuloVentaStockByGrupoSubGrupo>();
            var resultadoTransaccion = new ResultadoTransaccion<ArticuloVentaStockByGrupoSubGrupo>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_ARTICULO_VENTA_STOCK_BY_GRUPO_SUBGRUPO, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@Grupo", value.Code1));
                        cmd.Parameters.Add(new SqlParameter("@SubGrupo", value.Code2));
                        cmd.Parameters.Add(new SqlParameter("@SubGrupo2", value.Code3));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<ArticuloVentaStockByGrupoSubGrupo>)context.ConvertTo<ArticuloVentaStockByGrupoSubGrupo>(reader);
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

        public async Task<ResultadoTransaccion<MemoryStream>> GetListArticuloVentaStockExcelByGrupoSubGrupo(FiltroRequestEntity value)
        {
            var ms = new MemoryStream();
            var response = new List<ArticuloVentaStockByGrupoSubGrupo>();
            var resultadoTransaccion = new ResultadoTransaccion<MemoryStream>();
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
                    Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Artículos de venta - Stock" };
                    sheets.Append(sheet);

                    workbookPart.Workbook.Save();

                    SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

                    //Cabecera
                    Row row = new Row();
                    row.Append(
                    ExportToExcel.ConstructCell("Código de Artículo", CellValues.String),
                    ExportToExcel.ConstructCell("Nombre de Artículo", CellValues.String),
                    ExportToExcel.ConstructCell("Grupo", CellValues.String),
                    ExportToExcel.ConstructCell("SubGrupo", CellValues.String),
                    ExportToExcel.ConstructCell("NomGrupo 2", CellValues.String),
                    ExportToExcel.ConstructCell("UM", CellValues.String),
                    ExportToExcel.ConstructCell("Stock", CellValues.String),
                    ExportToExcel.ConstructCell("Comprometido", CellValues.String),
                    ExportToExcel.ConstructCell("Solicitado", CellValues.String),
                    ExportToExcel.ConstructCell("Disponible", CellValues.String),
                    ExportToExcel.ConstructCell("Peso Promedio Kg", CellValues.String));
                    sheetData.AppendChild(row);

                    using (SqlConnection conn = new SqlConnection(_cnxSap))
                    {
                        conn.Open();

                        using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_ARTICULO_VENTA_STOCK_BY_GRUPO_SUBGRUPO, conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 0;
                            cmd.Parameters.Add(new SqlParameter("@Grupo", value.Code1));
                            cmd.Parameters.Add(new SqlParameter("@SubGrupo", value.Code2));
                            cmd.Parameters.Add(new SqlParameter("@SubGrupo2", value.Code3));

                            using (var reader = await cmd.ExecuteReaderAsync())
                            {
                                response = (List<ArticuloVentaStockByGrupoSubGrupo>)context.ConvertTo<ArticuloVentaStockByGrupoSubGrupo>(reader);
                            }
                        }
                    }

                    //Contenido
                    foreach (var item in response)
                    {
                        row = new Row();
                        row.Append(
                        ExportToExcel.ConstructCell(item.ItemCode, CellValues.String),
                        ExportToExcel.ConstructCell(item.ItemName, CellValues.String),
                        ExportToExcel.ConstructCell(item.NomGrupo, CellValues.String),
                        ExportToExcel.ConstructCell(item.NomSubGrupo, CellValues.String),
                        ExportToExcel.ConstructCell(item.NomSubGrupo2, CellValues.String),
                        ExportToExcel.ConstructCell(item.UnidadVenta, CellValues.String),
                        ExportToExcel.ConstructCell(item.Stock.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.Comprometido.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.Solicitado.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.Disponible.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.PesoPromedioKg.ToString(), CellValues.Number));
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

        public async Task<ResultadoTransaccion<ArticuloVentaByGrupoSubGrupoEstado>> GetListArticuloVentaByGrupoSubGrupoEstado(FiltroRequestEntity value)
        {
            var response = new List<ArticuloVentaByGrupoSubGrupoEstado>();
            var resultadoTransaccion = new ResultadoTransaccion<ArticuloVentaByGrupoSubGrupoEstado>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_ARTICULO_VENTA_GRUPO_SUBGRUPO_ESTADO, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@Grupo", value.Code1));
                        cmd.Parameters.Add(new SqlParameter("@SubGrupo", value.Code2));
                        cmd.Parameters.Add(new SqlParameter("@SubGrupo2", value.Code3));
                        cmd.Parameters.Add(new SqlParameter("@Estado", value.Code4));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<ArticuloVentaByGrupoSubGrupoEstado>)context.ConvertTo<ArticuloVentaByGrupoSubGrupoEstado>(reader);
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

        public async Task<ResultadoTransaccion<MemoryStream>> GetListArticuloVentaExcelByGrupoSubGrupoEstado(FiltroRequestEntity value)
        {
            var ms = new MemoryStream();
            var response = new List<ArticuloVentaByGrupoSubGrupoEstado>();
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
                    Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Stock de Articulo de Venta" };
                    sheets.Append(sheet);

                    workbookPart.Workbook.Save();

                    SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

                    //Cabecera
                    Row row = new Row();
                    row.Append(
                    ExportToExcel.ConstructCell("Código de Artículo", CellValues.String),
                    ExportToExcel.ConstructCell("Nombre de Artículo", CellValues.String),
                    ExportToExcel.ConstructCell("Grupo", CellValues.String),
                    ExportToExcel.ConstructCell("SubGrupo", CellValues.String),
                    ExportToExcel.ConstructCell("SubGrupo 2", CellValues.String),
                    ExportToExcel.ConstructCell("Estado", CellValues.String),
                    ExportToExcel.ConstructCell("UM", CellValues.String),
                    ExportToExcel.ConstructCell("Peso Item", CellValues.String),
                    ExportToExcel.ConstructCell("Peso Promedio Kg", CellValues.String));
                    sheetData.AppendChild(row);

                    using (SqlConnection conn = new SqlConnection(_cnxSap))
                    {
                        conn.Open();

                        using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_ARTICULO_VENTA_GRUPO_SUBGRUPO_ESTADO, conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 0;
                            cmd.Parameters.Add(new SqlParameter("@Grupo", value.Code1));
                            cmd.Parameters.Add(new SqlParameter("@SubGrupo", value.Code2));
                            cmd.Parameters.Add(new SqlParameter("@SubGrupo2", value.Code3));
                            cmd.Parameters.Add(new SqlParameter("@Estado", value.Code4));

                            using (var reader = await cmd.ExecuteReaderAsync())
                            {
                                response = (List<ArticuloVentaByGrupoSubGrupoEstado>)context.ConvertTo<ArticuloVentaByGrupoSubGrupoEstado>(reader);
                            }
                        }
                    }

                    //Contenido
                    foreach (var item in response)
                    {
                        row = new Row();
                        row.Append(
                        ExportToExcel.ConstructCell(item.ItemCode, CellValues.String),
                        ExportToExcel.ConstructCell(item.ItemName, CellValues.String),
                        ExportToExcel.ConstructCell(item.NomGrupo, CellValues.String),
                        ExportToExcel.ConstructCell(item.NomSubGrupo, CellValues.String),
                        ExportToExcel.ConstructCell(item.NomSubGrupo2, CellValues.String),
                        ExportToExcel.ConstructCell(item.NomEstado, CellValues.String),
                        ExportToExcel.ConstructCell(item.UnidadVenta, CellValues.String),
                        ExportToExcel.ConstructCell(item.PesoItem.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.PesoPromedioKg.ToString(), CellValues.Number));
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

        public async Task<ResultadoTransaccion<ArticuloSapForSodimacBySkuItemEntity>> GetArticuloForOrdenVentaSodimacBySku(ArticuloSapForSodimacBySkuEntity value)
        {
            var articulo = new ArticuloSapEntity();
            var response = new List<ArticuloSapForSodimacBySkuItemEntity>();
            var resultadoTransaccion = new ResultadoTransaccion<ArticuloSapForSodimacBySkuItemEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    conn.Open();

                    var listSku = value.Item.Select(x => x.Sku).Distinct().ToList();

                    foreach (var sku in listSku)
                    {
                        using (SqlCommand cmd = new SqlCommand(SP_GET_FOR_ORDEN_VENTA_SODIMAC_SKU, conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 0;
                            cmd.Parameters.Add(new SqlParameter("@Sku", sku));

                            using (var reader = await cmd.ExecuteReaderAsync())
                            {
                                articulo = context.Convert<ArticuloSapEntity>(reader);
                            }
                        }

                        if(articulo == null)
                        {
                            resultadoTransaccion.IdRegistro = -1;
                            resultadoTransaccion.ResultadoCodigo = -1;
                            resultadoTransaccion.ResultadoDescripcion = "El SKU " + sku + " no existe en SAP Business One.";
                            return resultadoTransaccion;
                        }

                        foreach (var item in value.Item)
                        {
                            if(item.Sku == sku)
                            {
                                item.ItemCode = articulo.ItemCode;
                                item.Dscription = articulo.ItemName;
                            }
                        }
                    }

                    //var lista = value.Item.OrderBy(x => x.Line).ToList();

                    resultadoTransaccion.IdRegistro = 0;
                    resultadoTransaccion.ResultadoCodigo = 0;
                    resultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", value.Item.Count);
                    resultadoTransaccion.dataList = value.Item.OrderBy(x => x.Line).ToList();
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

        public async Task<ResultadoTransaccion<ArticuloDocumentoSapEntity>> GetArticuloVentaByCode(FiltroRequestEntity value)
        {
            var response = new ArticuloDocumentoSapEntity();
            var resultadoTransaccion = new ResultadoTransaccion<ArticuloDocumentoSapEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_VENTA_BY_CODE, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@CardCode", value.Code1));
                        cmd.Parameters.Add(new SqlParameter("@Currency", value.Code2));
                        cmd.Parameters.Add(new SqlParameter("@ItemCode", value.Code3));
                        cmd.Parameters.Add(new SqlParameter("@SlpCode", value.Id1));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = context.Convert<ArticuloDocumentoSapEntity>(reader);
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
    }
}
