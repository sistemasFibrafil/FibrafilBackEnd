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
    public class OrdenFabricacionSapRepository : RepositoryBase<OrdenFabricacionSapEntity>, IOrdenFabricacionSapRepository
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
        const string SP_GET_LIST_GENERAL_BY_SEDE = DB_ESQUEMA + "FIB_WEB_SP_PROD_GetListOrdenFabricacionGeneralBySede";


        public OrdenFabricacionSapRepository(IConnectionSql context, IConfiguration configuration)
            : base(context)
        {
            _cnxSap = Utilidades.GetExtraerCadenaConexion(configuration, "ParametersConectionSap");
            _configuration = configuration;
            _aplicacionName = GetType().Name;
        }

        public async Task<ResultadoTransaccion<OrdenFabricacionGeneralSapBySedeEntity>> GetListOrdenFabricacionGeneralBySede(DateTime fecInicial, DateTime fecFinal, string location)
        {
            var response = new List<OrdenFabricacionGeneralSapBySedeEntity>();
            var resultadoTransaccion = new ResultadoTransaccion<OrdenFabricacionGeneralSapBySedeEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_GENERAL_BY_SEDE, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@FecInicial", fecInicial));
                        cmd.Parameters.Add(new SqlParameter("@FecFinal", fecFinal));
                        cmd.Parameters.Add(new SqlParameter("@Location", location));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<OrdenFabricacionGeneralSapBySedeEntity>)context.ConvertTo<OrdenFabricacionGeneralSapBySedeEntity>(reader);
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

        public async Task<ResultadoTransaccion<MemoryStream>> GetOrdenFabricacionGeneralExcelBySede(DateTime fecInicial, DateTime fecFinal, string location)
        {
            var ms = new MemoryStream();
            var response = new List<OrdenFabricacionGeneralSapBySedeEntity>();
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
                    ExportToExcel.ConstructCell("DocEntry", CellValues.String),
                    ExportToExcel.ConstructCell("Número SAP", CellValues.String),
                    ExportToExcel.ConstructCell("Fecha Órden Fabricación", CellValues.String),
                    ExportToExcel.ConstructCell("Fecha Fin", CellValues.String),
                    ExportToExcel.ConstructCell("Fecha Sistema", CellValues.String),
                    ExportToExcel.ConstructCell("Tipo", CellValues.String),
                    ExportToExcel.ConstructCell("Estado", CellValues.String),
                    ExportToExcel.ConstructCell("Norma Reparto", CellValues.String),
                    ExportToExcel.ConstructCell("Item Prod", CellValues.String),
                    ExportToExcel.ConstructCell("Dsc Item Prod", CellValues.String),
                    ExportToExcel.ConstructCell("Almacén", CellValues.String),
                    ExportToExcel.ConstructCell("Unidad Medida", CellValues.String),
                    ExportToExcel.ConstructCell("QProd", CellValues.String),
                    ExportToExcel.ConstructCell("Peso Prod", CellValues.String),
                    ExportToExcel.ConstructCell("Grupo", CellValues.String),
                    ExportToExcel.ConstructCell("SubGrupo", CellValues.String),
                    ExportToExcel.ConstructCell("SubGrupo2", CellValues.String),
                    ExportToExcel.ConstructCell("Item Base", CellValues.String),
                    ExportToExcel.ConstructCell("Dsc Item Base", CellValues.String),
                    ExportToExcel.ConstructCell("UM Compra", CellValues.String),
                    ExportToExcel.ConstructCell("QBase", CellValues.String),
                    ExportToExcel.ConstructCell("Planificado", CellValues.String),
                    ExportToExcel.ConstructCell("IssuedQty", CellValues.String),
                    ExportToExcel.ConstructCell("UM Inventario", CellValues.String),
                    ExportToExcel.ConstructCell("Precio", CellValues.String),
                    ExportToExcel.ConstructCell("WareHouse", CellValues.String),
                    ExportToExcel.ConstructCell("CantMillar", CellValues.String),
                    ExportToExcel.ConstructCell("Situacion", CellValues.String),
                    ExportToExcel.ConstructCell("Destino Prod", CellValues.String),
                    ExportToExcel.ConstructCell("Máquina", CellValues.String),
                    ExportToExcel.ConstructCell("Cento Costo", CellValues.String),
                    ExportToExcel.ConstructCell("Comentarios", CellValues.String),
                    ExportToExcel.ConstructCell("Sede", CellValues.String));
                    sheetData.AppendChild(row);

                    using (SqlConnection conn = new SqlConnection(_cnxSap))
                    {
                        conn.Open();

                        using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_GENERAL_BY_SEDE, conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 0;
                            cmd.Parameters.Add(new SqlParameter("@FecInicial", fecInicial));
                            cmd.Parameters.Add(new SqlParameter("@FecFinal", fecFinal));
                            cmd.Parameters.Add(new SqlParameter("@Location", location));

                            using (var reader = await cmd.ExecuteReaderAsync())
                            {
                                response = (List<OrdenFabricacionGeneralSapBySedeEntity>)context.ConvertTo<OrdenFabricacionGeneralSapBySedeEntity>(reader);
                            }
                        }
                    }

                    //Contenido
                    foreach (var item in response)
                    {
                        row = new Row();
                        row.Append(
                        ExportToExcel.ConstructCell(item.DocEntry.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.DocNum.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.FechaOrdenFabricacion.ToString("dd/MM/yyyy"), CellValues.String),
                        ExportToExcel.ConstructCell(item.FechaFin.ToString("dd/MM/yyyy"), CellValues.String),
                        ExportToExcel.ConstructCell(item.FechaSistema.ToString("dd/MM/yyyy"), CellValues.String),
                        ExportToExcel.ConstructCell(item.Tipo, CellValues.String),
                        ExportToExcel.ConstructCell(item.Estado, CellValues.String),
                        ExportToExcel.ConstructCell(item.NormaReparto, CellValues.String),
                        ExportToExcel.ConstructCell(item.ItemProd, CellValues.String),
                        ExportToExcel.ConstructCell(item.DscItemProd, CellValues.String),
                        ExportToExcel.ConstructCell(item.Almacen, CellValues.String),
                        ExportToExcel.ConstructCell(item.UnidadMedida, CellValues.String),
                        ExportToExcel.ConstructCell(item.QProd.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.PesoProd.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.Grupo, CellValues.String),
                        ExportToExcel.ConstructCell(item.SubGrupo, CellValues.String),
                        ExportToExcel.ConstructCell(item.SubGrupo2, CellValues.String),
                        ExportToExcel.ConstructCell(item.ItemBase, CellValues.String),
                        ExportToExcel.ConstructCell(item.DscItemBase, CellValues.String),
                        ExportToExcel.ConstructCell(item.QBase.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.Planificado.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.IssuedQty.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.UnidadMedidaInventario, CellValues.String),
                        ExportToExcel.ConstructCell(item.Precio.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.WareHouse, CellValues.String),
                        ExportToExcel.ConstructCell(item.CantMillar.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.Situacion, CellValues.String),
                        ExportToExcel.ConstructCell(item.DestinoProd, CellValues.String),
                        ExportToExcel.ConstructCell(item.Maquina, CellValues.String),
                        ExportToExcel.ConstructCell(item.Usuario, CellValues.String),
                        ExportToExcel.ConstructCell(item.CentoCosto, CellValues.String),
                        ExportToExcel.ConstructCell(item.Comentarios, CellValues.String),
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
    }
}
