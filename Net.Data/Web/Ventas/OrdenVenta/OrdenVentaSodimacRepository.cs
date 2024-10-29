﻿using System;
using System.IO;
using System.Data;
using System.Linq;
using Net.Connection;
using Net.CrossCotting;
using System.Transactions;
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
    public class OrdenVentaSodimacRepository : RepositoryBase<OrdenVentaSodimacEntity>, IOrdenVentaSodimacRepository
    {
        private string _metodoName;
        private string _aplicacionName;
        private readonly Regex regex = new Regex(@"<(\w+)>.*");

        // PARAMETROS DE COXIÓN
        private readonly string _cnxDos;
        private readonly IConfiguration _configuration;

        // STORED PROCEDURE
        const string DB_ESQUEMA = "";
        const string SP_SET_CREATE = DB_ESQUEMA + "VEN_SP_SetOrdenVentaSodimacCreate";
        const string SP_SET_DETALLE_CREATE = DB_ESQUEMA + "VEN_SP_SetOrdenVentaSodimacDetalleCreate";
        const string SP_GET_BY_ID = DB_ESQUEMA + "VEN_SP_GetListOrdenVentaSodimacById";
        const string SP_GET_DETALLE_BY_ID = DB_ESQUEMA + "VEN_SP_GetListOrdenVentaSodimacDetalleById";

        const string SP_GET_LIST_PENDIENTE_LPN_BY_FILTRO = DB_ESQUEMA + "VEN_SP_GetListOrdenVentaSodimacPendienteLpnByFiltro";
        const string SP_GET_LIST_DETALLE_PENDIENTE_LPN_BY_ID_AND_FILTRO = DB_ESQUEMA + "VEN_SP_GetListOrdenVentaSodimacDetallePendienteLpnByIdAndFiltro";
        const string SP_GET_LIST_LPN_BY_FILTRO = DB_ESQUEMA + "VEN_SP_GetListOrdenVentaSodimacLpnByFiltro";
        const string SP_GET_LST_DETALLE_BY_ID = DB_ESQUEMA + "VEN_SP_GetOrdenVentaSodimacDetalleById";
        const string SP_GET_LIST_BY_FILTRO = DB_ESQUEMA + "VEN_SP_GetListOrdenVentaSodimacByFiltro";

        const string SP_GET_BARCODE_LPN_BY_ID = DB_ESQUEMA + "VEN_SP_GetOrdenVentaSodimacBarcodeLpnById";
        const string SP_GET_LIST_BARCODE_EAN_BY_EAN = DB_ESQUEMA + "VEN_SP_GetListOrdenVentaSodimacBarcodeEanByEan";
        
        const string SP_GET_LPN_NUEVO = DB_ESQUEMA + "VEN_SP_GetOrdenVentaSodimacLpnNuevo";
        const string SP_SET_LPN_UPDATE = DB_ESQUEMA + "VEN_SP_SetOrdenVentaSodimacDetalleLpnUpdate";

        const string SP_GET_LIST_BY_FECHA_NUMERO = DB_ESQUEMA + "VEN_SP_GetListOrdenVentaSodimacByFechaNumero";
        const string SP_GET_LIST_SELVA_BY_FECHA_NUMERO = DB_ESQUEMA + "VEN_SP_GetListOrdenVentaSodimacSelvaByFechaNumero";


        public OrdenVentaSodimacRepository(IConnectionSql context, IConfiguration configuration)
            : base(context)
        {
            _configuration = configuration;
            _aplicacionName = GetType().Name;
            _cnxDos = Utilidades.GetExtraerCadenaConexion(configuration, "ParametersConectionDos");
        }


        public async Task<ResultadoTransaccion<OrdenVentaSodimacEntity>> SetCreate(OrdenVentaSodimacEntity value)
        {
            var resultadoTransaccion = new ResultadoTransaccion<OrdenVentaSodimacEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxDos))
                {
                    using (CommittableTransaction transaction = new CommittableTransaction())
                    {
                        await conn.OpenAsync();
                        conn.EnlistTransaction(transaction);

                        try
                        {
                            using (SqlCommand cmd = new SqlCommand(SP_SET_CREATE, conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandTimeout = 0;
                                cmd.Parameters.Add(new SqlParameter("@IdOrdenVentaSodimac", SqlDbType.Int)).Direction = ParameterDirection.Output;
                                cmd.Parameters.Add(new SqlParameter("@DocEntry", value.DocEntry));
                                cmd.Parameters.Add(new SqlParameter("@DocNum", value.DocNum));
                                cmd.Parameters.Add(new SqlParameter("@NumAtCard", value.NumAtCard));
                                cmd.Parameters.Add(new SqlParameter("@CodEstado", value.CodEstado));
                                cmd.Parameters.Add(new SqlParameter("@DocDate", value.DocDate));
                                cmd.Parameters.Add(new SqlParameter("@DocDueDate", value.DocDueDate));
                                cmd.Parameters.Add(new SqlParameter("@TaxDate", value.TaxDate));
                                cmd.Parameters.Add(new SqlParameter("@CardCode", value.CardCode));
                                cmd.Parameters.Add(new SqlParameter("@CardName", value.CardName));
                                cmd.Parameters.Add(new SqlParameter("@CntctCode", value.CntctCode));
                                cmd.Parameters.Add(new SqlParameter("@CntctName", value.CntctName));
                                cmd.Parameters.Add(new SqlParameter("@Address", value.Address));
                                cmd.Parameters.Add(new SqlParameter("@IdUsuarioCreate", value.IdUsuarioCreate));

                                await cmd.ExecuteNonQueryAsync();

                                value.IdOrdenVentaSodimac = (int)cmd.Parameters["@IdOrdenVentaSodimac"].Value;

                                foreach (var item in value.Item)
                                {
                                    item.IdOrdenVentaSodimac = value.IdOrdenVentaSodimac;
                                }
                            }

                            using (SqlCommand cmd = new SqlCommand(SP_SET_DETALLE_CREATE, conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandTimeout = 0;

                                foreach (var item in value.Item)
                                {
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.Add(new SqlParameter("@IdOrdenVentaSodimac", item.IdOrdenVentaSodimac));
                                    cmd.Parameters.Add(new SqlParameter("@Line", item.Line));
                                    cmd.Parameters.Add(new SqlParameter("@NumLocal", item.NumLocal));
                                    cmd.Parameters.Add(new SqlParameter("@CodEstado", item.CodEstado));
                                    cmd.Parameters.Add(new SqlParameter("@ItemCode", item.ItemCode));
                                    cmd.Parameters.Add(new SqlParameter("@Sku", item.Sku));
                                    cmd.Parameters.Add(new SqlParameter("@Dscription", item.Dscription));
                                    cmd.Parameters.Add(new SqlParameter("@DscriptionLarga", item.DscriptionLarga));
                                    cmd.Parameters.Add(new SqlParameter("@Ean", item.Ean));
                                    cmd.Parameters.Add(new SqlParameter("@Quantity", item.Quantity));

                                    await cmd.ExecuteNonQueryAsync();
                                }
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
        public async Task<ResultadoTransaccion<OrdenVentaSodimacByFiltroEntity>> GetListOrdenVentaSodimacByFiltro(FiltroRequestEntity value)
        {
            var response = new List<OrdenVentaSodimacByFiltroEntity>();
            var resultadoTransaccion = new ResultadoTransaccion<OrdenVentaSodimacByFiltroEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxDos))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_BY_FILTRO, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@FecInicial", value.Fecha1));
                        cmd.Parameters.Add(new SqlParameter("@FecFinal", value.Fecha2));
                        cmd.Parameters.Add(new SqlParameter("@Estado", value.Code1));
                        cmd.Parameters.Add(new SqlParameter("@Filtro", value.TextFiltro1));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<OrdenVentaSodimacByFiltroEntity>)context.ConvertTo<OrdenVentaSodimacByFiltroEntity>(reader);
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
        public async Task<ResultadoTransaccion<OrdenVentaSodimacEntity>> GetOrdenVentaSodimacById(int id)
        {
            var response = new OrdenVentaSodimacEntity();
            var resultadoTransaccion = new ResultadoTransaccion<OrdenVentaSodimacEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxDos))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_BY_ID, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@Id", id));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = context.Convert<OrdenVentaSodimacEntity>(reader);
                        }
                    }

                    using (SqlCommand cmd = new SqlCommand(SP_GET_DETALLE_BY_ID, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@Id", id));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response.Item = (List<OrdenVentaDetalleSodimacEntity>)context.ConvertTo<OrdenVentaDetalleSodimacEntity>(reader);
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
        public async Task<ResultadoTransaccion<OrdenVentaSodimaConsultaFiltroEntity>> GetListOrdenVentaSodimacPendienteLpnByFiltro(FiltroRequestEntity value)
        {
            var response = new List<OrdenVentaSodimaConsultaFiltroEntity>();
            var resultadoTransaccion = new ResultadoTransaccion<OrdenVentaSodimaConsultaFiltroEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxDos))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_PENDIENTE_LPN_BY_FILTRO, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@Filtro", value.TextFiltro1));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<OrdenVentaSodimaConsultaFiltroEntity>)context.ConvertTo<OrdenVentaSodimaConsultaFiltroEntity>(reader);
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
        public async Task<ResultadoTransaccion<OrdenVentaSodimaConsultaFiltroEntity>> GetListOrdenVentaSodimacDetallePendienteLpnByIdAndFiltro(FiltroRequestEntity value)
        {
            var response = new List<OrdenVentaSodimaConsultaFiltroEntity>();
            var resultadoTransaccion = new ResultadoTransaccion<OrdenVentaSodimaConsultaFiltroEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxDos))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_DETALLE_PENDIENTE_LPN_BY_ID_AND_FILTRO, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@IdOrdenVentaSodimac", value.Id1));
                        cmd.Parameters.Add(new SqlParameter("@Filtro", value.TextFiltro1));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<OrdenVentaSodimaConsultaFiltroEntity>)context.ConvertTo<OrdenVentaSodimaConsultaFiltroEntity>(reader);
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
        public async Task<ResultadoTransaccion<OrdenVentaSodimaConsultaFiltroEntity>> GetListOrdenVentaSodimacLpnByFiltro(FiltroRequestEntity value)
        {
            var response = new List<OrdenVentaSodimaConsultaFiltroEntity>();
            var resultadoTransaccion = new ResultadoTransaccion<OrdenVentaSodimaConsultaFiltroEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxDos))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_LPN_BY_FILTRO, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@FecInicial", value.Fecha1));
                        cmd.Parameters.Add(new SqlParameter("@FecFinal", value.Fecha2));
                        cmd.Parameters.Add(new SqlParameter("@Filtro", value.TextFiltro1));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<OrdenVentaSodimaConsultaFiltroEntity>)context.ConvertTo<OrdenVentaSodimaConsultaFiltroEntity>(reader);
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
        public async Task<ResultadoTransaccion<OrdenVentaSodimacEntity>> SetLpnUpdate(OrdenVentaSodimacEntity value)
        {
            var response = new OrdenVentaSodimacEntity();
            var responseItem = new OrdenVentaDetalleSodimacEntity();
            var resultadoTransaccion = new ResultadoTransaccion<OrdenVentaSodimacEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxDos))
                {
                    using (CommittableTransaction transaction = new CommittableTransaction())
                    {
                        await conn.OpenAsync();
                        conn.EnlistTransaction(transaction);

                        try
                        {
                            using (SqlCommand cmd = new SqlCommand(SP_GET_LPN_NUEVO, conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandTimeout = 0;

                                using (var reader = await cmd.ExecuteReaderAsync())
                                {
                                    responseItem = context.Convert<OrdenVentaDetalleSodimacEntity>(reader);
                                }
                            }

                            using (SqlCommand cmdItem = new SqlCommand(SP_SET_LPN_UPDATE, conn))
                            {
                                cmdItem.CommandType = CommandType.StoredProcedure;
                                cmdItem.CommandTimeout = 0;

                                foreach (var item in value.Item)
                                {
                                    cmdItem.Parameters.Clear();
                                    cmdItem.Parameters.Add(new SqlParameter("@IdOrdenVentaSodimac", item.IdOrdenVentaSodimac));
                                    cmdItem.Parameters.Add(new SqlParameter("@Line", item.Line));
                                    cmdItem.Parameters.Add(new SqlParameter("@NumLocal", item.NumLocal));
                                    cmdItem.Parameters.Add(new SqlParameter("@Lpn", responseItem.Lpn));

                                    await cmdItem.ExecuteNonQueryAsync();
                                }
                            }

                            resultadoTransaccion.IdRegistro = 0;
                            resultadoTransaccion.ResultadoCodigo = 0;
                            resultadoTransaccion.ResultadoDescripcion = "Registro actualizado con éxito ..!";
                            resultadoTransaccion.data = response;

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
        public async Task<ResultadoTransaccion<OrdenVentaSodimaConsultaFiltroEntity>> GetListOrdenVentaSodimacDetalleById(FiltroRequestEntity value)
        {
            var response = new List<OrdenVentaSodimaConsultaFiltroEntity>();
            var resultadoTransaccion = new ResultadoTransaccion<OrdenVentaSodimaConsultaFiltroEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxDos))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LST_DETALLE_BY_ID, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@Id", value.Id1));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<OrdenVentaSodimaConsultaFiltroEntity>)context.ConvertTo<OrdenVentaSodimaConsultaFiltroEntity>(reader);
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
        public async Task<ResultadoTransaccion<MemoryStream>> GetBarcodeLpnPdfById(int id)
        {
            var response = new List<OrdenVentaSodimaConsultaFiltroEntity>();
            var resultadoTransaccion = new ResultadoTransaccion<MemoryStream>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxDos))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_BARCODE_LPN_BY_ID, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@Id", id));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<OrdenVentaSodimaConsultaFiltroEntity>)context.ConvertTo<OrdenVentaSodimaConsultaFiltroEntity>(reader);
                        }
                    }
                }

                var pgSize = new iTextSharp.text.Rectangle(9.0f, 6.5f);
                iTextSharp.text.Document doc = new iTextSharp.text.Document(pgSize);
                doc.SetMargins(0f, 0f, 0f, 0f);
                MemoryStream ms = new MemoryStream();
                iTextSharp.text.pdf.BaseFont helvetica = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA, iTextSharp.text.pdf.BaseFont.CP1250, true);
                iTextSharp.text.pdf.PdfWriter write = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, ms);
                doc.Open();

                foreach (var item in response)
                {
                    iTextSharp.text.pdf.PdfContentByte cb1 = write.DirectContent;
                    cb1.BeginText();
                    cb1.SetFontAndSize(helvetica, 0.40f);
                    cb1.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_LEFT, "O/C: " + item.NumAtCard, 0.70f, 5.5f, 0);
                    cb1.EndText();

                    iTextSharp.text.pdf.PdfContentByte cb2 = write.DirectContent;
                    cb2.BeginText();
                    cb2.SetFontAndSize(helvetica, 0.40f);
                    cb2.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_LEFT, "TIENDA: " + item.NumLocal, 0.70f, 5.0f, 0);
                    cb2.EndText();

                    iTextSharp.text.pdf.PdfContentByte cb3 = write.DirectContent;
                    cb3.BeginText();
                    cb3.SetFontAndSize(helvetica, 0.40f);
                    cb3.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_LEFT, item.NomLocal, 0.70f, 4.5f, 0);
                    cb3.EndText();


                    // ============================================================
                    // ===================== INI: BARCODE =========================
                    // ============================================================
                    iTextSharp.text.pdf.PdfContentByte cb = write.DirectContent;
                    iTextSharp.text.pdf.Barcode128 bc = new iTextSharp.text.pdf.Barcode128();
                    bc.TextAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                    bc.Code = item.Lpn;
                    bc.StartStopText = false;
                    bc.CodeType = iTextSharp.text.pdf.Barcode128.UPCE;
                    bc.Extended = true;
                    bc.BarHeight = 35;

                    iTextSharp.text.Image img = bc.CreateImageWithBarcode(cb, iTextSharp.text.BaseColor.Black, iTextSharp.text.BaseColor.Black);
                    cb.SetFontAndSize(helvetica, 12.0f);
                    // Define el tamaño de BARCODE
                    img.ScaleToFit(7.7f, 4.0f);
                    // Define la ubicación de BARCODE
                    img.SetAbsolutePosition(0.4f, 1.0f);
                    cb.AddImage(img);
                    // ============================================================
                    // ===================== FIN: BARCODE =========================
                    // ============================================================

                    doc.NewPage();
                }

                write.Close();
                doc.Close();
                ms.Seek(0, SeekOrigin.Begin);
                var file = ms;

                resultadoTransaccion.IdRegistro = 0;
                resultadoTransaccion.ResultadoCodigo = 0;
                resultadoTransaccion.ResultadoDescripcion = "Se generó correctamente el archivo.s";
                resultadoTransaccion.data = file;

            }
            catch (Exception ex)
            {
                resultadoTransaccion.IdRegistro = -1;
                resultadoTransaccion.ResultadoCodigo = -1;
                resultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
            }

            return resultadoTransaccion;
        }
        public async Task<ResultadoTransaccion<MemoryStream>> GetListBarcodeEanPdfByEan(string ean)
        {
            var response = new List<OrdenVentaSodimaConsultaFiltroEntity>();
            var resultadoTransaccion = new ResultadoTransaccion<MemoryStream>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxDos))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_BARCODE_EAN_BY_EAN, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@Ean", ean));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<OrdenVentaSodimaConsultaFiltroEntity>)context.ConvertTo<OrdenVentaSodimaConsultaFiltroEntity>(reader);
                        }
                    }
                }


                var pgSize = new iTextSharp.text.Rectangle(9.0f, 6.5f);
                iTextSharp.text.Document doc = new iTextSharp.text.Document(pgSize);
                doc.SetMargins(0f, 0f, 0f, 0f);
                MemoryStream ms = new MemoryStream();
                iTextSharp.text.pdf.BaseFont helvetica = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA, iTextSharp.text.pdf.BaseFont.CP1250, true);
                iTextSharp.text.pdf.PdfWriter write = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, ms);
                doc.Open();

                foreach (var item in response)
                {
                    iTextSharp.text.pdf.PdfContentByte cb1 = write.DirectContent;
                    cb1.BeginText();
                    cb1.SetFontAndSize(helvetica, 0.50f);
                    cb1.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_CENTER, item.Sku, 1.5f, 2.5f, 270);
                    cb1.EndText();


                    // ============================================================
                    // ===================== INI: BARCODE =========================
                    // ============================================================
                    iTextSharp.text.pdf.PdfContentByte cb = write.DirectContent;
                    iTextSharp.text.pdf.BarcodeEan bc = new iTextSharp.text.pdf.BarcodeEan();
                    bc.TextAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                    bc.Code = item.Ean;
                    bc.StartStopText = false;
                    bc.CodeType = iTextSharp.text.pdf.Barcode128.EAN13;
                    bc.Extended = true;
                    bc.BarHeight = 30;

                    iTextSharp.text.Image img = bc.CreateImageWithBarcode(cb, iTextSharp.text.BaseColor.Black, iTextSharp.text.BaseColor.Black);
                    cb.SetFontAndSize(helvetica, 12.0f);
                    // Define el tamaño de BARCODE
                    img.ScaleToFit(4.8f, 3.0f);
                    //img.RotationDegrees = 270;
                    // Define la ubicación de BARCODE
                    img.SetAbsolutePosition(2.0f, 2.0f);
                    cb.AddImage(img);
                    // ============================================================
                    // ===================== FIN: BARCODE =========================
                    // ============================================================


                    iTextSharp.text.pdf.PdfContentByte cb3 = write.DirectContent;
                    cb3.BeginText();
                    cb3.SetFontAndSize(helvetica, 0.30f);
                    cb3.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_CENTER, item.DscriptionLarga, 4.5f, 1.7f, 0);
                    cb3.EndText();

                    doc.NewPage();
                }
                

                write.Close();
                doc.Close();
                ms.Seek(0, SeekOrigin.Begin);
                var file = ms;

                resultadoTransaccion.IdRegistro = 0;
                resultadoTransaccion.ResultadoCodigo = 0;
                resultadoTransaccion.ResultadoDescripcion = "Se generó correctamente el archivo.s";
                resultadoTransaccion.data = file;

            }
            catch (Exception ex)
            {
                resultadoTransaccion.IdRegistro = -1;
                resultadoTransaccion.ResultadoCodigo = -1;
                resultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
            }

            return resultadoTransaccion;
        }
        public async Task<ResultadoTransaccion<OrdenVentaSodimaConsultaFiltroEntity>> GetListOrdenVentaSodimacByFechaNumero(FiltroRequestEntity value)
        {
            var response = new List<OrdenVentaSodimaConsultaFiltroEntity>();
            var resultadoTransaccion = new ResultadoTransaccion<OrdenVentaSodimaConsultaFiltroEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxDos))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_BY_FECHA_NUMERO, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@FecInicial", value.Fecha1));
                        cmd.Parameters.Add(new SqlParameter("@FecFinal", value.Fecha2));
                        cmd.Parameters.Add(new SqlParameter("@Tipo", value.Code1));
                        cmd.Parameters.Add(new SqlParameter("@Filtro", value.TextFiltro1));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<OrdenVentaSodimaConsultaFiltroEntity>)context.ConvertTo<OrdenVentaSodimaConsultaFiltroEntity>(reader);
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
        public async Task<ResultadoTransaccion<MemoryStream>> GetListOrdenVentaSodimacExcelByFechaNumero(FiltroRequestEntity value)
        {
            var ms = new MemoryStream();
            var response = new List<OrdenVentaSodimaConsultaFiltroEntity>();
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
                    Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Despacho - Sodimac" };
                    sheets.Append(sheet);

                    workbookPart.Workbook.Save();

                    SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

                    //Cabecera
                    Row row = new Row();
                    row.Append(
                    ExportToExcel.ConstructCell("NUMERO OC", CellValues.String),
                    ExportToExcel.ConstructCell("FECHA ESPERADA", CellValues.String),
                    ExportToExcel.ConstructCell("FECHA VENCIMIENTO", CellValues.String),
                    ExportToExcel.ConstructCell("EAN/UPC", CellValues.String),
                    ExportToExcel.ConstructCell("SKU", CellValues.String),
                    ExportToExcel.ConstructCell("DESCRIPCIÓN", CellValues.String),
                    ExportToExcel.ConstructCell("DESCRIPCIÓN LARGA", CellValues.String),
                    ExportToExcel.ConstructCell("NÚMERO ÍTEM", CellValues.String),
                    ExportToExcel.ConstructCell("NÚMERO LOCAL", CellValues.String),
                    ExportToExcel.ConstructCell("LOCAL", CellValues.String),
                    ExportToExcel.ConstructCell("UNIDADES", CellValues.String),
                    ExportToExcel.ConstructCell("CB", CellValues.String));
                    sheetData.AppendChild(row);

                    using (SqlConnection conn = new SqlConnection(_cnxDos))
                    {
                        conn.Open();

                        using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_BY_FECHA_NUMERO, conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 0;
                            cmd.Parameters.Add(new SqlParameter("@FecInicial", value.Fecha1));
                            cmd.Parameters.Add(new SqlParameter("@FecFinal", value.Fecha2));
                            cmd.Parameters.Add(new SqlParameter("@Tipo", value.Code1));
                            cmd.Parameters.Add(new SqlParameter("@Filtro", value.TextFiltro1));

                            using (var reader = await cmd.ExecuteReaderAsync())
                            {
                                response = (List<OrdenVentaSodimaConsultaFiltroEntity>)context.ConvertTo<OrdenVentaSodimaConsultaFiltroEntity>(reader);
                            }
                        }
                    }

                    //Contenido
                    foreach (var item in response)
                    {
                        row = new Row();
                        row.Append(
                        ExportToExcel.ConstructCell(item.NumAtCard, CellValues.String),
                        ExportToExcel.ConstructCell(item.TaxDate.ToString("dd/MM/yyyy"), CellValues.String),
                        ExportToExcel.ConstructCell(item.DocDueDate.ToString("dd/MM/yyyy"), CellValues.String),
                        ExportToExcel.ConstructCell(item.Ean, CellValues.String),
                        ExportToExcel.ConstructCell(item.Sku, CellValues.String),
                        ExportToExcel.ConstructCell(item.Dscription, CellValues.String),
                        ExportToExcel.ConstructCell(item.DscriptionLarga, CellValues.String),
                        ExportToExcel.ConstructCell(item.Line.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.NumLocal.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.NomLocal, CellValues.String),
                        ExportToExcel.ConstructCell(item.Quantity.ToString(), CellValues.Number),
                        ExportToExcel.ConstructCell(item.Lpn, CellValues.String));
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
        public async Task<ResultadoTransaccion<OrdenVentaSodimacSelvaByFechaNumeroEntity>> GetListOrdenVentaSodimacSelvaFechaNumero(FiltroRequestEntity value)
        {
            var response = new List<OrdenVentaSodimacSelvaByFechaNumeroEntity>();
            var resultadoTransaccion = new ResultadoTransaccion<OrdenVentaSodimacSelvaByFechaNumeroEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxDos))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_SELVA_BY_FECHA_NUMERO, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@FecInicial", value.Fecha1));
                        cmd.Parameters.Add(new SqlParameter("@FecFinal", value.Fecha2));
                        cmd.Parameters.Add(new SqlParameter("@Filtro", value.TextFiltro1));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<OrdenVentaSodimacSelvaByFechaNumeroEntity>)context.ConvertTo<OrdenVentaSodimacSelvaByFechaNumeroEntity>(reader);
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
        public async Task<ResultadoTransaccion<MemoryStream>> GetListOrdenVentaSodimacSelvaPdfByFechaNumero(FiltroRequestEntity value)
        {
            var response = new List<OrdenVentaSodimacSelvaByFechaNumeroEntity>();
            var resultadoTransaccion = new ResultadoTransaccion<MemoryStream>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxDos))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_SELVA_BY_FECHA_NUMERO, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@FecInicial", value.Fecha1));
                        cmd.Parameters.Add(new SqlParameter("@FecFinal", value.Fecha2));
                        cmd.Parameters.Add(new SqlParameter("@Filtro", value.TextFiltro1));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<OrdenVentaSodimacSelvaByFechaNumeroEntity>)context.ConvertTo<OrdenVentaSodimacSelvaByFechaNumeroEntity>(reader);
                        }
                    }

                    iTextSharp.text.Document doc = new iTextSharp.text.Document();
                    doc.SetPageSize(iTextSharp.text.PageSize.A4);
                    doc.SetMargins(10f, 10f, 10f, 10f);
                    MemoryStream ms = new MemoryStream();
                    iTextSharp.text.pdf.PdfWriter write = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, ms);
                    write.ViewerPreferences = iTextSharp.text.pdf.PdfWriter.PageModeUseOutlines;
                    // Our custom Header and Footer is done using Event Handler
                    PageEventHelper pageEventHelper = new PageEventHelper();
                    write.PageEvent = pageEventHelper;

                    // Colocamos la fuente que deseamos que tenga el documento
                    iTextSharp.text.pdf.BaseFont helvetica = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA, iTextSharp.text.pdf.BaseFont.CP1250, true);
                    // Titulo
                    iTextSharp.text.Font parrafoNegro = new iTextSharp.text.Font(helvetica, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.Black);
                    iTextSharp.text.Font parrafoNormal = new iTextSharp.text.Font(helvetica, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.Black);
                    

                    doc.Open();

                    var contador = 0;
                    var listCabecera = response.Select(m=> new {m.Proveedor, m.NumAtCard, m.NumPallet, m.NumLocal, m.NomLocal}).Distinct().OrderBy(m=> m.NumAtCard).ThenBy(m=>m.NomLocal).ToList();
                    var cantidadFila = listCabecera.Count();

                    foreach (var cabecera in listCabecera)
                    {
                        var listDetalle = response.Select(m => new { m.NumAtCard, m.NumLocal, m.Ean, m.Sku, m.DscriptionLarga, m.Quantity }).Where(m => m.NumAtCard == cabecera.NumAtCard & m.NumLocal == cabecera.NumLocal).ToList();
                        // ==================================================================
                        // ======================== INI: CABECERA ===========================
                        // ==================================================================
                        // Se declara la variable tbl dentro del FOR para que salte de página e inicialice en cada FOR 
                        var tbl = new iTextSharp.text.pdf.PdfPTable(new float[] { 20f, 15f, 50f, 15f }) { WidthPercentage = 100 };

                        var c1 = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("PROVEEDOR", parrafoNegro)) { BorderWidth = 1, PaddingBottom = 10, PaddingTop = 10, HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT };
                        tbl.AddCell(c1);
                        c1 = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(cabecera.Proveedor, parrafoNegro)) { BorderWidth = 1, PaddingBottom = 10, PaddingTop = 10, HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER };
                        c1.Colspan = 3;
                        tbl.AddCell(c1);

                        c1 = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("ÓRDEN DE COMPRA", parrafoNegro)) { BorderWidth = 1, PaddingBottom = 10, PaddingTop = 10, HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT };
                        tbl.AddCell(c1);
                        c1 = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(cabecera.NumAtCard, parrafoNegro)) { BorderWidth = 1, PaddingBottom = 10, PaddingTop = 10, HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER };
                        c1.Colspan = 3;
                        tbl.AddCell(c1);

                        c1 = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("BULTO/PALLET", parrafoNegro)) { BorderWidth = 1, PaddingBottom = 10, PaddingTop = 10, HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT };
                        tbl.AddCell(c1);
                        c1 = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("1" + "  DE  " + cabecera.NumPallet.ToString(), parrafoNegro)) { BorderWidth = 1, PaddingBottom = 10, PaddingTop = 10, HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER };
                        c1.Colspan = 3;
                        tbl.AddCell(c1);

                        c1 = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("TIENDA", parrafoNegro)) { BorderWidth = 1, PaddingBottom = 10, PaddingTop = 10, HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT };
                        tbl.AddCell(c1);
                        c1 = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(cabecera.NumLocal + " - " + cabecera.NomLocal , parrafoNegro)) { BorderWidth = 1, PaddingBottom = 10, PaddingTop = 10, HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER };
                        c1.Colspan = 3;
                        tbl.AddCell(c1);
                        // ==================================================================
                        // ======================== FIN: CABECERA ===========================
                        // ==================================================================


                        // ==================================================================
                        // ======================== INI: DETALLE ============================
                        // ==================================================================

                        // ======================== INI: TITULO DEL DETALLE ===============
                        c1 = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("EAN 13", parrafoNegro)) { BorderWidth = 1, PaddingBottom = 10, PaddingTop = 10, HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER };
                        tbl.AddCell(c1);
                        c1 = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("SKU", parrafoNegro)) { BorderWidth = 1, PaddingBottom = 10, PaddingTop = 10, HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER };
                        tbl.AddCell(c1);
                        c1 = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("DESCRIPCIÓN", parrafoNegro)) { BorderWidth = 1, PaddingBottom = 10, PaddingTop = 10, HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER };
                        tbl.AddCell(c1);
                        c1 = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("CANTIDAD", parrafoNegro)) { BorderWidth = 1, PaddingBottom = 10, PaddingTop = 10, HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER };
                        tbl.AddCell(c1);
                        // ======================== FIN: TITULO DEL DETALLE ===============

                        foreach (var detalle in listDetalle)
                        {
                            // ============================================================
                            // ===================== INI: BARCODE =========================
                            // ============================================================
                            iTextSharp.text.pdf.PdfContentByte cb = write.DirectContent;
                            iTextSharp.text.pdf.BarcodeEan bc = new iTextSharp.text.pdf.BarcodeEan();
                            bc.TextAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                            bc.Code = detalle.Ean;
                            bc.StartStopText = false;
                            bc.CodeType = iTextSharp.text.pdf.Barcode128.EAN13;
                            bc.Extended = true;
                            bc.BarHeight = 30;

                            iTextSharp.text.Image img = bc.CreateImageWithBarcode(cb, iTextSharp.text.BaseColor.Black, iTextSharp.text.BaseColor.Black);
                            cb.SetFontAndSize(helvetica, 12.0f);
                            // Define el tamaño de BARCODE
                            img.ScaleToFit(100.0f, 40.0f);
                            // Define la ubicación de BARCODE
                            img.SetAbsolutePosition(0f,0f);
                            // ============================================================
                            // ===================== FIN: BARCODE =========================
                            // ============================================================

                            c1 = new iTextSharp.text.pdf.PdfPCell(img) { BorderWidth = 1, PaddingBottom = 5, PaddingTop = 5, HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER, VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE };
                            tbl.AddCell(c1);
                            c1 = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(detalle.Sku, parrafoNormal)) { BorderWidth = 1, PaddingBottom = 5, PaddingTop = 5, HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER, VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE };
                            tbl.AddCell(c1);
                            c1 = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(detalle.DscriptionLarga, parrafoNormal)) { BorderWidth = 1, PaddingBottom = 5, PaddingTop = 5, HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT, VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE };
                            tbl.AddCell(c1);
                            c1 = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(detalle.Quantity.ToString("N3"), parrafoNormal)) { BorderWidth = 1, PaddingBottom = 5, PaddingTop = 5, HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT, VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE };
                            tbl.AddCell(c1);
                        }
                        doc.Add(tbl);
                        // ==================================================================
                        // ======================== FIN: DETALLE ============================
                        // ==================================================================
                        contador++;
                        if(cantidadFila != contador)
                        {
                            doc.NewPage();
                        }
                    }

                    write.Close();
                    doc.Close();
                    ms.Seek(0, SeekOrigin.Begin);
                    var file = ms;

                    resultadoTransaccion.IdRegistro = 0;
                    resultadoTransaccion.ResultadoCodigo = 0;
                    resultadoTransaccion.ResultadoDescripcion = "Se generó correctamente el archivos.";
                    resultadoTransaccion.data = file;
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
