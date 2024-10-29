using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Net.Business.Entities;
using Net.Business.Entities.Sap;
using Net.Business.Entities.Web;
using Net.Connection;
using Net.CrossCotting;
using Net.Data.Sap;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using DocumentFormat.OpenXml.Office.CustomUI;
using Azure;

namespace Net.Data.Web
{
    public class ForcastventaRepository : RepositoryBase<ForcastVentaEntity>, IForcastVentaRepository
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

        const string SP_GET_LIST_SCOC = DB_ESQUEMA + "FIB_WEB_VEN_SP_GetListForcastVentaConSinOc";
        const string SP_GET_LIST_NEGOCIO = DB_ESQUEMA + "FIB_WEB_VEN_SP_GetListForcastVentaNegocio";
        const string SP_GET_LIST_GRUPO = DB_ESQUEMA + "FIB_WEB_VEN_SP_GetListGrupoArticulo";
        const string SP_GET_LIST_ESTADO = DB_ESQUEMA + "FIB_WEB_VEN_SP_GetListForcastVentaEstado";
        const string SP_GET_LIST_BY_ID= DB_ESQUEMA + "FIB_WEB_SP_VEN_GetListForcastVentaById";
        const string SP_GET_LIST_BY_FECHA = DB_ESQUEMA + "FIB_WEB_SP_VEN_GetListForcastVentaByFecha";
        const string SP_SET_CREATE = DB_ESQUEMA + "FIB_WEB_VEN_SP_SetForcastVentaCreate";
        const string SP_SET_UPDATE = DB_ESQUEMA + "FIB_WEB_VEN_SP_SetForcastVentaUpdate";
        const string SP_SET_DELETE = DB_ESQUEMA + "FIB_WEB_VEN_SP_SetForcastVentaDelete";

        public ForcastventaRepository(IConnectionSql context, IConfiguration configuration)
           : base(context)
        {
            _cnxSap = Utilidades.GetExtraerCadenaConexion(configuration, "ParametersConectionSap");
            _configuration = configuration;
            _aplicacionName = GetType().Name;
        }

        public async Task<ResultadoTransaccion<MemoryStream>> GetForcastVentaPlantillaExcel()
        {
            var ms = new MemoryStream();
            var lista1 = new List<ForcastVentaConSinOcEntity>();
            var lista2 = new List<ForcastVentaNegocioEntity>();
            var lista3 = new List<GrupoArticuloSapEntity>();
            var lista4 = new List<ForcastVentaEstadoEntity>();
            ResultadoTransaccion<MemoryStream> resultadoTransaccion = new ResultadoTransaccion<MemoryStream>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_SCOC, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            lista1 = (List<ForcastVentaConSinOcEntity>)context.ConvertTo<ForcastVentaConSinOcEntity>(reader);
                        }
                    }

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_NEGOCIO, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            lista2 = (List<ForcastVentaNegocioEntity>)context.ConvertTo<ForcastVentaNegocioEntity>(reader);
                        }
                    }

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_GRUPO, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            lista3 = (List<GrupoArticuloSapEntity>)context.ConvertTo<GrupoArticuloSapEntity>(reader);
                        }
                    }

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_ESTADO, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            lista4 = (List<ForcastVentaEstadoEntity>)context.ConvertTo<ForcastVentaEstadoEntity>(reader);
                        }
                    }

                    ms = GetForcastPlantillaExcel(lista1, lista2, lista3, lista4);
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

        public async Task<ResultadoTransaccion<ForcastVentaEntity>> SetImport(ForcastVentaEntity value)
        {
            var resultadoTransaccion = new ResultadoTransaccion<ForcastVentaEntity>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    using (CommittableTransaction transaction = new CommittableTransaction())
                    {
                        await conn.OpenAsync();
                        conn.EnlistTransaction(transaction);

                        try
                        {
                            using (SqlCommand cmdItem = new SqlCommand(SP_SET_CREATE, conn))
                            {
                                cmdItem.CommandType = CommandType.StoredProcedure;
                                cmdItem.CommandTimeout = 0;

                                foreach (var item in value.Item)
                                {
                                    cmdItem.Parameters.Clear();
                                    cmdItem.Parameters.Add(new SqlParameter("@IdConSinOc", item.IdConSinOc));
                                    cmdItem.Parameters.Add(new SqlParameter("@IdNegocio", item.IdNegocio));
                                    cmdItem.Parameters.Add(new SqlParameter("@ItmsGrpCod", item.ItmsGrpCod));
                                    cmdItem.Parameters.Add(new SqlParameter("@ItemCode", item.ItemCode));
                                    cmdItem.Parameters.Add(new SqlParameter("@DocNum", item.DocNum));
                                    cmdItem.Parameters.Add(new SqlParameter("@CardCode", item.CardCode));
                                    cmdItem.Parameters.Add(new SqlParameter("@FecRegistro", item.FecRegistro));
                                    cmdItem.Parameters.Add(new SqlParameter("@UnidadMedida", item.UnidadMedida));
                                    cmdItem.Parameters.Add(new SqlParameter("@Cantidad", item.Cantidad));
                                    cmdItem.Parameters.Add(new SqlParameter("@Kg", item.Kg));
                                    cmdItem.Parameters.Add(new SqlParameter("@Precio", item.Precio));
                                    cmdItem.Parameters.Add(new SqlParameter("@CodEstado", item.CodEstado));
                                    cmdItem.Parameters.Add(new SqlParameter("@IdUsuarioCreate", value.IdUsuario));

                                    await cmdItem.ExecuteNonQueryAsync();
                                }
                            }

                            resultadoTransaccion.IdRegistro = 0;
                            resultadoTransaccion.ResultadoCodigo = 0;
                            resultadoTransaccion.ResultadoDescripcion = "Registro creado con éxito ..!";

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            resultadoTransaccion.IdRegistro = -1;
                            resultadoTransaccion.ResultadoCodigo = -1;
                            resultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
                        }
                    }
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

        public async Task<ResultadoTransaccion<ForcastVentaEntity>> SetCreate(ForcastVentaEntity value)
        {
            var resultadoTransaccion = new ResultadoTransaccion<ForcastVentaEntity>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    using (CommittableTransaction transaction = new CommittableTransaction())
                    {
                        await conn.OpenAsync();
                        conn.EnlistTransaction(transaction);

                        try
                        {
                            using (SqlCommand cmdItem = new SqlCommand(SP_SET_CREATE, conn))
                            {
                                cmdItem.CommandType = CommandType.StoredProcedure;
                                cmdItem.CommandTimeout = 0;

                                cmdItem.Parameters.Add(new SqlParameter("@IdConSinOc", value.IdConSinOc));
                                cmdItem.Parameters.Add(new SqlParameter("@IdNegocio", value.IdNegocio));
                                cmdItem.Parameters.Add(new SqlParameter("@ItmsGrpCod", value.ItmsGrpCod));
                                cmdItem.Parameters.Add(new SqlParameter("@ItemCode", value.ItemCode));
                                cmdItem.Parameters.Add(new SqlParameter("@DocNum", value.DocNum));
                                cmdItem.Parameters.Add(new SqlParameter("@CardCode", value.CardCode));
                                cmdItem.Parameters.Add(new SqlParameter("@FecRegistro", value.FecRegistro));
                                cmdItem.Parameters.Add(new SqlParameter("@UnidadMedida", value.UnidadMedida));
                                cmdItem.Parameters.Add(new SqlParameter("@Cantidad", value.Cantidad));
                                cmdItem.Parameters.Add(new SqlParameter("@Kg", value.Kg));
                                cmdItem.Parameters.Add(new SqlParameter("@Precio", value.Precio));
                                cmdItem.Parameters.Add(new SqlParameter("@CodEstado", value.CodEstado));
                                cmdItem.Parameters.Add(new SqlParameter("@IdUsuarioCreate", value.IdUsuario));

                                await cmdItem.ExecuteNonQueryAsync();
                            }

                            resultadoTransaccion.IdRegistro = 0;
                            resultadoTransaccion.ResultadoCodigo = 0;
                            resultadoTransaccion.ResultadoDescripcion = "Registro creado con éxito ..!";

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            resultadoTransaccion.IdRegistro = -1;
                            resultadoTransaccion.ResultadoCodigo = -1;
                            resultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
                        }
                    }
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

        public async Task<ResultadoTransaccion<ForcastVentaEntity>> SetUpdate(ForcastVentaEntity value)
        {
            var resultadoTransaccion = new ResultadoTransaccion<ForcastVentaEntity>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    using (CommittableTransaction transaction = new CommittableTransaction())
                    {
                        await conn.OpenAsync();
                        conn.EnlistTransaction(transaction);

                        try
                        {
                            using (SqlCommand cmdItem = new SqlCommand(SP_SET_UPDATE, conn))
                            {
                                cmdItem.CommandType = CommandType.StoredProcedure;
                                cmdItem.CommandTimeout = 0;

                                cmdItem.Parameters.Add(new SqlParameter("@IdForcastVenta", value.IdForcastVenta));
                                cmdItem.Parameters.Add(new SqlParameter("@IdConSinOc", value.IdConSinOc));
                                cmdItem.Parameters.Add(new SqlParameter("@IdNegocio", value.IdNegocio));
                                cmdItem.Parameters.Add(new SqlParameter("@ItmsGrpCod", value.ItmsGrpCod));
                                cmdItem.Parameters.Add(new SqlParameter("@ItemCode", value.ItemCode));
                                cmdItem.Parameters.Add(new SqlParameter("@DocNum", value.DocNum));
                                cmdItem.Parameters.Add(new SqlParameter("@CardCode", value.CardCode));
                                cmdItem.Parameters.Add(new SqlParameter("@FecRegistro", value.FecRegistro));
                                cmdItem.Parameters.Add(new SqlParameter("@UnidadMedida", value.UnidadMedida));
                                cmdItem.Parameters.Add(new SqlParameter("@Cantidad", value.Cantidad));
                                cmdItem.Parameters.Add(new SqlParameter("@Kg", value.Kg));
                                cmdItem.Parameters.Add(new SqlParameter("@Precio", value.Precio));
                                cmdItem.Parameters.Add(new SqlParameter("@CodEstado", value.CodEstado));
                                cmdItem.Parameters.Add(new SqlParameter("@IdUsuarioUpdate", value.IdUsuario));

                                await cmdItem.ExecuteNonQueryAsync();
                            }

                            resultadoTransaccion.IdRegistro = 0;
                            resultadoTransaccion.ResultadoCodigo = 0;
                            resultadoTransaccion.ResultadoDescripcion = "Registro actualizado con éxito ..!";

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            resultadoTransaccion.IdRegistro = -1;
                            resultadoTransaccion.ResultadoCodigo = -1;
                            resultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
                        }
                    }
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

        public async Task<ResultadoTransaccion<ForcastVentaEntity>> SetDelete(int idForcastVenta)
        {
            var resultadoTransaccion = new ResultadoTransaccion<ForcastVentaEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    using (CommittableTransaction transaction = new CommittableTransaction())
                    {
                        await conn.OpenAsync();
                        conn.EnlistTransaction(transaction);

                        try
                        {
                            using (SqlCommand cmd = new SqlCommand(SP_SET_DELETE, conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandTimeout = 0;
                                cmd.Parameters.Add(new SqlParameter("@IdForcastVenta", idForcastVenta));

                                await cmd.ExecuteNonQueryAsync();
                            }

                            resultadoTransaccion.IdRegistro = 0;
                            resultadoTransaccion.ResultadoCodigo = 0;
                            resultadoTransaccion.ResultadoDescripcion = "Registro eliminado con éxito ..!";

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            resultadoTransaccion.IdRegistro = -1;
                            resultadoTransaccion.ResultadoCodigo = -1;
                            resultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
                        }
                    }
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

        public async Task<ResultadoTransaccion<ForcastVentaEntity>> GetById(int idForcastVenta)
        {
            var response = new ForcastVentaEntity();
            var resultadoTransaccion = new ResultadoTransaccion<ForcastVentaEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_BY_ID, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@IdForcastVenta", idForcastVenta));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = context.Convert<ForcastVentaEntity>(reader);
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

        public async Task<ResultadoTransaccion<ForcastVentaByFechaEntity>> GetListForcastVentaByFecha(DateTime fecInicial, DateTime fecFinal)
        {
            var response = new List<ForcastVentaByFechaEntity>();
            var resultadoTransaccion = new ResultadoTransaccion<ForcastVentaByFechaEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_BY_FECHA, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@FecInicial", fecInicial));
                        cmd.Parameters.Add(new SqlParameter("@FecFinal", fecFinal));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<ForcastVentaByFechaEntity>)context.ConvertTo<ForcastVentaByFechaEntity>(reader);
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



        /// GENERACIÓN DE ARCHIVO EXCEL
        private MemoryStream GetForcastPlantillaExcel(List<ForcastVentaConSinOcEntity> lista1, List<ForcastVentaNegocioEntity> lista2, List<GrupoArticuloSapEntity> lista3, List<ForcastVentaEstadoEntity> lista4)
        {
            MemoryStream ms = new MemoryStream();

            using (SpreadsheetDocument document = SpreadsheetDocument.Create(ms, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorkbookStylesPart wbsp = workbookPart.AddNewPart<WorkbookStylesPart>();
                wbsp.Stylesheet = ExportToExcel.GenerateStyleSheet();
                wbsp.Stylesheet.Save();

                Sheets sheets = document.WorkbookPart.Workbook.AppendChild(new Sheets());

                GetForcastExcel(document, workbookPart, sheets);
                GetConSinOcExcel(document, workbookPart, sheets, lista1);
                GetNegocioExcel(document, workbookPart, sheets, lista2);
                GetGrupoArticuloExcel(document, workbookPart, sheets, lista3);
                GetEstadoExcel(document, workbookPart, sheets, lista4);

                workbookPart.Workbook.Save();
                document.Close();
            }

            return ms;
        }

        private void GetForcastExcel(SpreadsheetDocument document, WorkbookPart workbookPart, Sheets sheets)
        {
            // Agregar la primera hoja de cálculo: "Forcast de venta"
            WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());

            Sheet sheet = new Sheet() { Id = document.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Forcast de venta" };
            sheets.Append(sheet);

            // Agregar datos a la primera hoja de cálculo
            SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

            //Cabecera
            Row row = new Row();
            row.Append(
            ExportToExcel.ConstructCell("IdConSinOc", CellValues.String),
            ExportToExcel.ConstructCell("IdNegocio", CellValues.String),
            ExportToExcel.ConstructCell("ItmsGrpCod", CellValues.String),
            ExportToExcel.ConstructCell("ItemCode", CellValues.String),
            ExportToExcel.ConstructCell("DocNum", CellValues.String),
            ExportToExcel.ConstructCell("CardCode", CellValues.String),
            ExportToExcel.ConstructCell("FecRegistro", CellValues.String),
            ExportToExcel.ConstructCell("UnidadMedida", CellValues.String),
            ExportToExcel.ConstructCell("Cantidad", CellValues.String),
            ExportToExcel.ConstructCell("Kg", CellValues.String),
            ExportToExcel.ConstructCell("Precio", CellValues.String),
            ExportToExcel.ConstructCell("CodEstado", CellValues.String));
            sheetData.AppendChild(row);

            var response = new List<ForcastVentaEntity>() { new ForcastVentaEntity { IdConSinOc = 1, IdNegocio = 1, ItmsGrpCod = 100, ItemCode = "PTCORAM3H5", DocNum = 1000018699, CardCode = "C10416126670", FecRegistro = DateTime.Now, UnidadMedida = "KG", Cantidad = 120, Kg = 120, Precio = 50, CodEstado = "01" } };

            //Contenido
            foreach (var item in response)
            {
                row = new Row();
                row.Append(
                ExportToExcel.ConstructCell(item.IdConSinOc.ToString(), CellValues.Number),
                ExportToExcel.ConstructCell(item.IdNegocio.ToString(), CellValues.Number),
                ExportToExcel.ConstructCell(item.ItmsGrpCod.ToString(), CellValues.Number),
                ExportToExcel.ConstructCell(item.ItemCode, CellValues.String),
                ExportToExcel.ConstructCell(item.DocNum.ToString(), CellValues.Number),
                ExportToExcel.ConstructCell(item.CardCode, CellValues.String),
                ExportToExcel.ConstructCell(item.FecRegistro.ToString("dd/MM/yyyy"), CellValues.String),
                ExportToExcel.ConstructCell(item.UnidadMedida, CellValues.String),
                ExportToExcel.ConstructCell(item.Cantidad.ToString(), CellValues.Number),
                ExportToExcel.ConstructCell(item.Kg.ToString(), CellValues.Number),
                ExportToExcel.ConstructCell(item.Precio.ToString(), CellValues.Number),
                ExportToExcel.ConstructCell(item.CodEstado, CellValues.String));
                sheetData.Append(row);
            }
        }

        private void GetConSinOcExcel(SpreadsheetDocument document, WorkbookPart workbookPart, Sheets sheets, List<ForcastVentaConSinOcEntity> lista)
        {
            // Agregar la primera hoja de cálculo: "CS OC"
            WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());

            Sheet sheet = new Sheet() { Id = document.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = 2, Name = "CS OC" };
            sheets.Append(sheet);

            // Agregar datos a la primera hoja de cálculo
            SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

            //Cabecera
            Row row = new Row();
            row.Append(
            ExportToExcel.ConstructCell("Código", CellValues.String),
            ExportToExcel.ConstructCell("Nombre", CellValues.String));
            sheetData.AppendChild(row);

            //Contenido
            foreach (var item in lista)
            {
                row = new Row();
                row.Append(
                ExportToExcel.ConstructCell(item.IdConSinOc.ToString(), CellValues.Number),
                ExportToExcel.ConstructCell(item.NomConSinOc, CellValues.String));
                sheetData.Append(row);
            }
        }

        private void GetNegocioExcel(SpreadsheetDocument document, WorkbookPart workbookPart, Sheets sheets, List<ForcastVentaNegocioEntity> lista)
        {
            // Agregar la primera hoja de cálculo: "Negocio"
            WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());

            Sheet sheet = new Sheet() { Id = document.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = 3, Name = "Negocio" };
            sheets.Append(sheet);

            // Agregar datos a la primera hoja de cálculo
            SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

            //Cabecera
            Row row = new Row();
            row.Append(
            ExportToExcel.ConstructCell("Código", CellValues.String),
            ExportToExcel.ConstructCell("Nombre", CellValues.String));
            sheetData.AppendChild(row);

            //Contenido
            foreach (var item in lista)
            {
                row = new Row();
                row.Append(
                ExportToExcel.ConstructCell(item.IdNegocio.ToString(), CellValues.Number),
                ExportToExcel.ConstructCell(item.NomNegocio, CellValues.String));
                sheetData.Append(row);
            }
        }

        private void GetGrupoArticuloExcel(SpreadsheetDocument document, WorkbookPart workbookPart, Sheets sheets, List<GrupoArticuloSapEntity> lista)
        {
            // Agregar la primera hoja de cálculo: "Grupo"
            WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());

            Sheet sheet = new Sheet() { Id = document.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = 4, Name = "Grupo" };
            sheets.Append(sheet);

            // Agregar datos a la primera hoja de cálculo
            SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

            //Cabecera
            Row row = new Row();
            row.Append(
            ExportToExcel.ConstructCell("Código", CellValues.String),
            ExportToExcel.ConstructCell("Nombre", CellValues.String));
            sheetData.AppendChild(row);

            //Contenido
            foreach (var item in lista)
            {
                row = new Row();
                row.Append(
                ExportToExcel.ConstructCell(item.ItmsGrpCod.ToString(), CellValues.Number),
                ExportToExcel.ConstructCell(item.ItmsGrpNam, CellValues.String));
                sheetData.Append(row);
            }
        }

        private void GetEstadoExcel(SpreadsheetDocument document, WorkbookPart workbookPart, Sheets sheets, List<ForcastVentaEstadoEntity> lista)
        {
            // Agregar la primera hoja de cálculo: "Estado"
            WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());

            Sheet sheet = new Sheet() { Id = document.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = 5, Name = "Estado" };
            sheets.Append(sheet);

            // Agregar datos a la primera hoja de cálculo
            SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

            //Cabecera
            Row row = new Row();
            row.Append(
            ExportToExcel.ConstructCell("Código", CellValues.String),
            ExportToExcel.ConstructCell("Nombre", CellValues.String));
            sheetData.AppendChild(row);

            //Contenido
            foreach (var item in lista)
            {
                row = new Row();
                row.Append(
                ExportToExcel.ConstructCell(item.CodEstado, CellValues.String),
                ExportToExcel.ConstructCell(item.NomEstado, CellValues.String));
                sheetData.Append(row);
            }
        }
    }
}
