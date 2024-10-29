using System;
using System.IO;
using System.Net;
using System.Data;
using System.Text;
using Net.Connection;
using Newtonsoft.Json;
using System.Net.Http;
using Net.CrossCotting;
using Newtonsoft.Json.Linq;
using Net.Business.Entities;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Net.Business.Entities.Sap;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;

namespace Net.Data.Sap
{
    public class FacturacionElectronicaSapRepository : RepositoryBase<FacturacionElectronicaSapEntity>, IFacturacionElectronicaSapRepository
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

        const string SP_GET_LIST_FACTURA_ELECTRONICA = DB_ESQUEMA + "FIB_WEB_SP_VEND_Get_List_FacturaElectronicaAll";
        const string SP_FACTURA_ELECTRONICA_UPDATE = DB_ESQUEMA + "FIB_SP_WEB_SetFacturacionElectronicaUpdate";
        const string SP_FACTURA_ELECTRONICA_ERROR_UPDATE = DB_ESQUEMA + "FIB_SP_WEB_SetFacturacionElectronicaErrorUpdate";

        const string SP_GET_LIST_GUIA_ELECTRONICA_BY_FECHA_NUMERO = DB_ESQUEMA + "FIB_WEB_SP_VEND_GetListGuiaElectronicaByFechaAndNumero";
        const string SP_GET_GUIA_ELECTRONICA_BY_DOCENTRY = DB_ESQUEMA + "FIB_WEB_SP_VEND_GetGuiaElectronicaByDocEntry";
        const string SP_GET_LIST_GUIA_DETALLE_ELECTRONICA_BY_DOCENTRY = DB_ESQUEMA + "FIB_WEB_SP_VEND_GetListGuiaDetalleElectronicaByDocEntry";

        const string SP_GET_LIST_GUIA_INTERNA_ELECTRONICA_BY_FECHA_NUMERO = DB_ESQUEMA + "FIB_WEB_SP_INVE_GuiaInternaElectronicaByFechaAndNumero";
        const string SP_GET_GUIA_INTERNA_ELECTRONICA_BY_DOCENTRY = DB_ESQUEMA + "FIB_WEB_SP_INVE_GetGuiaInternaElectronicaByDocEntry";
        const string SP_GET_LIST_GUIA_DETALLE_INTERNA_ELECTRONICA_BY_DOCENTRY = DB_ESQUEMA + "FIB_WEB_SP_INVE_GetListGuiaDetalleInternaElectronicaByDocEntry";

        public FacturacionElectronicaSapRepository(IConnectionSql context, IConfiguration configuration)
            : base(context)
        {
            _cnxSap = Utilidades.GetExtraerCadenaConexion(configuration, "ParametersConectionSap");
            _configuration = configuration;
            _aplicacionName = GetType().Name;
        }

        public async Task<ResultadoTransaccion<Facturas>> GetListFacturaElectronica(string docNum)
        {
            var response = new List<Facturas>();
            var resultadoTransaccion = new ResultadoTransaccion<Facturas>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_FACTURA_ELECTRONICA, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<Facturas>)context.ConvertTo<Facturas>(reader);
                        }
                    }

                    foreach (Facturas factura in response)
                    {
                        var responseSend = FacturacionElectronica.GetRespuesta(factura.Tsq);

                        if (responseSend.errors == null)
                        {
                            resultadoTransaccion = await UpdateFacturaEnvio(factura, responseSend, conn);
                        }
                        else if (responseSend.errors.ToUpper().Contains("EXISTE"))
                        {
                            var responseQuery = FacturacionElectronica.GetRespuesta(factura.Qry);

                            resultadoTransaccion = await UpdateFacturaConsulta(factura, responseQuery, conn);
                        }
                        else
                        {
                            resultadoTransaccion = await UpdateFacturaError(factura, responseSend, conn);
                        }
                    }

                    conn.Close();
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

        private async Task<ResultadoTransaccion<Facturas>> UpdateFacturaEnvio(Facturas factura, Respuesta respuesta, SqlConnection conn)
        {
            var resultadoTransaccion = new ResultadoTransaccion<Facturas>();

            using (SqlCommand cmd = new SqlCommand(SP_FACTURA_ELECTRONICA_UPDATE, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter("@DocEntry", factura.DocEntry));
                cmd.Parameters.Add(new SqlParameter("@ObjType", factura.ObjType));
                cmd.Parameters.Add(new SqlParameter("@AceptadaPorSunat", respuesta.aceptada_por_sunat));
                cmd.Parameters.Add(new SqlParameter("@SunatDescription", respuesta.sunat_description));
                cmd.Parameters.Add(new SqlParameter("@SunatNote", respuesta.sunat_note));
                cmd.Parameters.Add(new SqlParameter("@SunatResponsecode", respuesta.sunat_responsecode));
                cmd.Parameters.Add(new SqlParameter("@SunatSoapError", respuesta.sunat_soap_error));
                cmd.Parameters.Add(new SqlParameter("@CadenaParaCodigoQr", respuesta.cadena_para_codigo_qr));
                cmd.Parameters.Add(new SqlParameter("@CodigoHash", respuesta.codigo_hash));

                await cmd.ExecuteNonQueryAsync();
            }

            resultadoTransaccion.IdRegistro = 0;
            resultadoTransaccion.ResultadoCodigo = 0;
            resultadoTransaccion.ResultadoDescripcion = "El documento se envío con éxito...!!!";

            return resultadoTransaccion;
        }

        private async Task<ResultadoTransaccion<Facturas>> UpdateFacturaConsulta(Facturas factura, Respuesta respuesta, SqlConnection conn)
        {
            var resultadoTransaccion = new ResultadoTransaccion<Facturas>();

            using (SqlCommand cmd = new SqlCommand(SP_FACTURA_ELECTRONICA_UPDATE, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter("@DocEntry", factura.DocEntry));
                cmd.Parameters.Add(new SqlParameter("@ObjType", factura.ObjType));
                cmd.Parameters.Add(new SqlParameter("@AceptadaPorSunat", respuesta.aceptada_por_sunat));
                cmd.Parameters.Add(new SqlParameter("@SunatDescription", respuesta.sunat_description));
                cmd.Parameters.Add(new SqlParameter("@SunatNote", respuesta.sunat_note));
                cmd.Parameters.Add(new SqlParameter("@SunatResponsecode", respuesta.sunat_responsecode));
                cmd.Parameters.Add(new SqlParameter("@SunatSoapError", respuesta.sunat_soap_error));
                cmd.Parameters.Add(new SqlParameter("@CadenaParaCodigoQr", respuesta.cadena_para_codigo_qr));
                cmd.Parameters.Add(new SqlParameter("@CodigoHash", respuesta.codigo_hash));

                await cmd.ExecuteNonQueryAsync();
            }

            resultadoTransaccion.IdRegistro = 0;
            resultadoTransaccion.ResultadoCodigo = 0;
            resultadoTransaccion.ResultadoDescripcion = "El documento actualizada con éxito...!!!";

            return resultadoTransaccion;
        }

        private async Task<ResultadoTransaccion<Facturas>> UpdateFacturaError(Facturas factura, Respuesta respuesta, SqlConnection conn)
        {
            var resultadoTransaccion = new ResultadoTransaccion<Facturas>();

            using (SqlCommand cmd = new SqlCommand(SP_FACTURA_ELECTRONICA_ERROR_UPDATE, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter("@DocEntry", factura.DocEntry));
                cmd.Parameters.Add(new SqlParameter("@ObjType", factura.ObjType));
                cmd.Parameters.Add(new SqlParameter("@Error", respuesta.errors));

                await cmd.ExecuteNonQueryAsync();
            }

            resultadoTransaccion.IdRegistro = -1;
            resultadoTransaccion.ResultadoCodigo = -1;
            resultadoTransaccion.ResultadoDescripcion = respuesta.errors;

            return resultadoTransaccion;
        }

        public async Task<ResultadoTransaccion<FacturacionElectronicaSapEntity>> GetListGuiaElectronicaByFechaAndNumero(FiltroRequestEntity value)
        {
            var response = new List<FacturacionElectronicaSapEntity>();
            var resultadoTransaccion = new ResultadoTransaccion<FacturacionElectronicaSapEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_GUIA_ELECTRONICA_BY_FECHA_NUMERO, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@FecInicial", value.Fecha1));
                        cmd.Parameters.Add(new SqlParameter("@FecFinal", value.Fecha2));
                        cmd.Parameters.Add(new SqlParameter("@Filtro1", value.TextFiltro1));
                        cmd.Parameters.Add(new SqlParameter("@Filtro2", value.TextFiltro2));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<FacturacionElectronicaSapEntity>)context.ConvertTo<FacturacionElectronicaSapEntity>(reader);
                        }

                        resultadoTransaccion.IdRegistro = 0;
                        resultadoTransaccion.ResultadoCodigo = 0;
                        resultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", response.Count);
                        resultadoTransaccion.dataList = response;
                    }

                    conn.Close();
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

        public async Task<ResultadoTransaccion<FacturacionElectronicaSapEntity>> EnviarGuiaElectronica(FacturacionElectronicaSapEntity value)
        {
            var guia = new Invoice();
            var resultadoTransaccion = new ResultadoTransaccion<FacturacionElectronicaSapEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_GUIA_ELECTRONICA_BY_DOCENTRY, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@DocEntry", value.DocEntry));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            guia = ((List<Invoice>)context.ConvertTo<Invoice>(reader))[0];
                        }
                    }

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_GUIA_DETALLE_ELECTRONICA_BY_DOCENTRY, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@DocEntry", value.DocEntry));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            guia.items = (List<Items>)context.ConvertTo<Items>(reader);
                        }
                    }

                    string tsq = JsonConvert.SerializeObject(guia, Formatting.Indented);
                    var responseSend = FacturacionElectronica.GetRespuesta(tsq);

                    if (responseSend.errors == null)
                    {
                        //using (SqlCommand cmd = new SqlCommand(SP_FACTURA_ELECTRONICA_UPDATE, conn))
                        //{
                        //    cmd.CommandType = CommandType.StoredProcedure;
                        //    cmd.Parameters.Clear();
                        //    cmd.Parameters.Add(new SqlParameter("@DocEntry", docEntry));
                        //    cmd.Parameters.Add(new SqlParameter("@Sunat_Description", responseSend.sunat_description));
                        //    cmd.Parameters.Add(new SqlParameter("@Sunat_Note", responseSend.sunat_note));
                        //    cmd.Parameters.Add(new SqlParameter("@Sunat_Responsecode", responseSend.sunat_responsecode));
                        //    cmd.Parameters.Add(new SqlParameter("@Sunat_Soap_Error", responseSend.sunat_soap_error));
                        //    cmd.Parameters.Add(new SqlParameter("@Cadena_Para_Codigo_Qr", responseSend.cadena_para_codigo_qr));
                        //    cmd.Parameters.Add(new SqlParameter("@Codigo_Hash", responseSend.codigo_hash));

                        //    await cmd.ExecuteNonQueryAsync();
                        //}

                        resultadoTransaccion.IdRegistro = 0;
                        resultadoTransaccion.ResultadoCodigo = 0;
                        resultadoTransaccion.ResultadoDescripcion = "El comprobante se envío con éxito...!!!";
                    }
                    else
                    {
                        //using (SqlCommand cmd = new SqlCommand(SP_FACTURA_ELECTRONICA_ERROR_UPDATE, conn))
                        //{
                        //    cmd.CommandType = CommandType.StoredProcedure;
                        //    cmd.Parameters.Clear();
                        //    cmd.Parameters.Add(new SqlParameter("@DocEntry", docEntry));
                        //    cmd.Parameters.Add(new SqlParameter("@Error", leer_respuesta.errors));

                        //    await cmd.ExecuteNonQueryAsync();
                        //}

                        resultadoTransaccion.IdRegistro = -1;
                        resultadoTransaccion.ResultadoCodigo = -1;
                        resultadoTransaccion.ResultadoDescripcion = responseSend.errors;
                    }

                    conn.Close();
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

        public async Task<ResultadoTransaccion<FacturacionElectronicaSapEntity>> GetListGuiaInternaElectronicaByFechaAndNumero(FiltroRequestEntity value)
        {
            var response = new List<FacturacionElectronicaSapEntity>();
            var resultadoTransaccion = new ResultadoTransaccion<FacturacionElectronicaSapEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_GUIA_INTERNA_ELECTRONICA_BY_FECHA_NUMERO, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@FecInicial", value.Fecha1));
                        cmd.Parameters.Add(new SqlParameter("@FecFinal", value.Fecha2));
                        cmd.Parameters.Add(new SqlParameter("@Filtro1", value.TextFiltro1));
                        cmd.Parameters.Add(new SqlParameter("@Filtro2", value.TextFiltro2));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<FacturacionElectronicaSapEntity>)context.ConvertTo<FacturacionElectronicaSapEntity>(reader);
                        }

                        resultadoTransaccion.IdRegistro = 0;
                        resultadoTransaccion.ResultadoCodigo = 0;
                        resultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", response.Count);
                        resultadoTransaccion.dataList = response;
                    }

                    conn.Close();
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

        public async Task<ResultadoTransaccion<FacturacionElectronicaSapEntity>> EnviarGuiaInternaElectronica(FacturacionElectronicaSapEntity value)
        {
            var guia = new Invoice();
            var resultadoTransaccion = new ResultadoTransaccion<FacturacionElectronicaSapEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxSap))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_GUIA_INTERNA_ELECTRONICA_BY_DOCENTRY, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@DocEntry", value.DocEntry));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            guia = ((List<Invoice>)context.ConvertTo<Invoice>(reader))[0];
                        }
                    }

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_GUIA_DETALLE_INTERNA_ELECTRONICA_BY_DOCENTRY, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@DocEntry", value.DocEntry));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            guia.items = (List<Items>)context.ConvertTo<Items>(reader);
                        }
                    }

                    string tsq = JsonConvert.SerializeObject(guia, Formatting.Indented);
                    var responseSend = FacturacionElectronica.GetRespuesta(tsq);

                    if (responseSend.errors == null)
                    {
                        //using (SqlCommand cmd = new SqlCommand(SP_FACTURA_ELECTRONICA_UPDATE, conn))
                        //{
                        //    cmd.CommandType = CommandType.StoredProcedure;
                        //    cmd.Parameters.Clear();
                        //    cmd.Parameters.Add(new SqlParameter("@DocEntry", docEntry));
                        //    cmd.Parameters.Add(new SqlParameter("@Sunat_Description", responseSend.sunat_description));
                        //    cmd.Parameters.Add(new SqlParameter("@Sunat_Note", responseSend.sunat_note));
                        //    cmd.Parameters.Add(new SqlParameter("@Sunat_Responsecode", responseSend.sunat_responsecode));
                        //    cmd.Parameters.Add(new SqlParameter("@Sunat_Soap_Error", responseSend.sunat_soap_error));
                        //    cmd.Parameters.Add(new SqlParameter("@Cadena_Para_Codigo_Qr", responseSend.cadena_para_codigo_qr));
                        //    cmd.Parameters.Add(new SqlParameter("@Codigo_Hash", responseSend.codigo_hash));

                        //    await cmd.ExecuteNonQueryAsync();
                        //}

                        resultadoTransaccion.IdRegistro = 0;
                        resultadoTransaccion.ResultadoCodigo = 0;
                        resultadoTransaccion.ResultadoDescripcion = "El comprobante se envío con éxito...!!!";
                    }
                    else
                    {
                        //using (SqlCommand cmd = new SqlCommand(SP_FACTURA_ELECTRONICA_ERROR_UPDATE, conn))
                        //{
                        //    cmd.CommandType = CommandType.StoredProcedure;
                        //    cmd.Parameters.Clear();
                        //    cmd.Parameters.Add(new SqlParameter("@DocEntry", docEntry));
                        //    cmd.Parameters.Add(new SqlParameter("@Error", leer_respuesta.errors));

                        //    await cmd.ExecuteNonQueryAsync();
                        //}

                        resultadoTransaccion.IdRegistro = -1;
                        resultadoTransaccion.ResultadoCodigo = -1;
                        resultadoTransaccion.ResultadoDescripcion = responseSend.errors;
                    }

                    conn.Close();
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
