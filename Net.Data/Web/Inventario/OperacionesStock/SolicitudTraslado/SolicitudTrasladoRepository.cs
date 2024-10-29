using System;
using SAPbobsCOM;
using System.Data;
using Net.Connection;
using Net.CrossCotting;
using System.Transactions;
using Net.Business.Entities;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Net.Business.Entities.Web;
using Net.Business.Entities.Sap;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
namespace Net.Data.Web
{
    public class SolicitudTrasladoRepository : RepositoryBase<SolicitudTrasladoEntity>, ISolicitudTrasladoRepository
    {
        private string _metodoName;
        private string _aplicacionName;
        private readonly Regex regex = new Regex(@"<(\w+)>.*");

        // PARAMETROS DE COXIÓN
        private readonly string _cnxDos;
        private readonly string _cnxSap;
        private readonly IConfiguration _configuration;
        private readonly IConnectionSap _connectionSap;
        private readonly ConnectionSapEntity _cnxDiApiSap;

        // STORED PROCEDURE
        const string DB_ESQUEMA = "";
        const string SP_GET_NUMERO = DB_ESQUEMA + "INV_SP_GetSolicitudTrasladoNumero";
        const string SP_GET_BY_ID = DB_ESQUEMA + "INV_SP_GetSolicitudTrasladoById";
        const string SP_GET_LIST_DETALLE_BY_ID = DB_ESQUEMA + "INV_SP_GetSolicitudTrasladoDetalleById";
        const string SP_GET_LIST_FILTRO = DB_ESQUEMA + "INV_SP_GetListSolicitudTrasladoByFiltro";

        const string SP_SET_CREATE = DB_ESQUEMA + "INV_SP_SetSolicitudTrasladoCreate";
        const string SP_SET_DETALLE_CREATE = DB_ESQUEMA + "INV_SP_SetSolicitudTrasladoDetalleCreate";
        const string SP_SET_DATOS_SAP_UPDATE = DB_ESQUEMA + "INV_SP_SetSolicitudTrasladoDatosSapUpdate";
        const string SP_SET_DETALLE_DATOS_SAP_UPDATE = DB_ESQUEMA + "INV_SP_SetSolicitudTrasladoDetalleDatosSapUpdate";

        const string SP_SET_UPDATE = DB_ESQUEMA + "INV_SP_SetSolicitudTrasladoCreate";

        // SAP
        const string SP_GET_DOCNUM_BY_DOCENTRY = DB_ESQUEMA + "INV_SP_GetSolicitudTrasladoDocNumByEntry";



        public SolicitudTrasladoRepository(IConnectionSql context, IConfiguration configuration)
            : base(context)
        {
            _configuration = configuration;
            _aplicacionName = GetType().Name;
            _connectionSap = new ConnectionSap();
            _cnxDos = Utilidades.GetExtraerCadenaConexion(configuration, "ParametersConectionDos");
            _cnxSap = Utilidades.GetExtraerCadenaConexion(configuration, "ParametersConectionSap");
            _cnxDiApiSap = Utilidades.GetExtraerCadenaConexionDiApiSap(configuration, "ParametersConectionDiApiSap");
        }


        public async Task<ResultadoTransaccion<SolicitudTrasladoEntity>> GetNumero()
        {
            var response = new SolicitudTrasladoEntity();
            var resultadoTransaccion = new ResultadoTransaccion<SolicitudTrasladoEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxDos))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_NUMERO, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = context.Convert<SolicitudTrasladoEntity>(reader);
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
        public async Task<ResultadoTransaccion<SolicitudTrasladoEntity>> GetListByFiltro(FiltroRequestEntity value)
        {
            var response = new List<SolicitudTrasladoEntity>();
            var resultadoTransaccion = new ResultadoTransaccion<SolicitudTrasladoEntity>();

            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            resultadoTransaccion.NombreMetodo = _metodoName;
            resultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxDos))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_FILTRO, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@FecInicial", value.Fecha1));
                        cmd.Parameters.Add(new SqlParameter("@FecFinal", value.Fecha2));
                        cmd.Parameters.Add(new SqlParameter("@Estado", value.TextFiltro1));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<SolicitudTrasladoEntity>)context.ConvertTo<SolicitudTrasladoEntity>(reader);
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
        public async Task<ResultadoTransaccion<SolicitudTrasladoEntity>> GetById(int id)
        {
            var response = new SolicitudTrasladoEntity();
            var resultadoTransaccion = new ResultadoTransaccion<SolicitudTrasladoEntity>();

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
                            response = context.Convert<SolicitudTrasladoEntity>(reader);
                        }
                    }

                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_DETALLE_BY_ID, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@Id", id));

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response.Item = (List<SolicitudTrasladoDetalleEntity>)context.ConvertTo<SolicitudTrasladoDetalleEntity>(reader);
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
        public async Task<ResultadoTransaccion<SolicitudTrasladoEntity>> SetCreate(SolicitudTrasladoEntity value)
        {
            var responde = new SolicitudTrasladoSapEntity();
            var resultadoTransaccion = new ResultadoTransaccion<SolicitudTrasladoEntity>();

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

                        _connectionSap.ConnectToCompany(_cnxDiApiSap);
                        RepositoryBaseSap.oCompany.StartTransaction();

                        StockTransfer documentIns = RepositoryBaseSap.oCompany.GetBusinessObject(BoObjectTypes.oInventoryTransferRequest);
                        StockTransfer documentQry = RepositoryBaseSap.oCompany.GetBusinessObject(BoObjectTypes.oInventoryTransferRequest);

                        try
                        {
                            #region <<< CREACIÓN DE SOLICITUD DE TRASLADO >>>
                            // Creación de la solicitud de traslado en la base de datos LOCAL
                            using (SqlCommand cmd = new SqlCommand(SP_SET_CREATE, conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandTimeout = 0;
                                // CABECERA
                                cmd.Parameters.Add(new SqlParameter("@IdSolicitudTraslado", SqlDbType.Int)).Direction = ParameterDirection.Output;
                                cmd.Parameters.Add(new SqlParameter("@Numero", value.Numero));
                                cmd.Parameters.Add(new SqlParameter("@DocDate", value.DocDate));
                                cmd.Parameters.Add(new SqlParameter("@DocDueDate", value.DocDueDate));
                                cmd.Parameters.Add(new SqlParameter("@TaxDate", value.TaxDate));
                                // CLIENTE
                                cmd.Parameters.Add(new SqlParameter("@CardCode", value.CardCode));
                                cmd.Parameters.Add(new SqlParameter("@CardName", value.CardName));
                                cmd.Parameters.Add(new SqlParameter("@CntctCode", value.CntctCode));
                                cmd.Parameters.Add(new SqlParameter("@Address", value.Address));
                                // ALMACÉN
                                cmd.Parameters.Add(new SqlParameter("@FromWhsCode", value.FromWhsCode));
                                cmd.Parameters.Add(new SqlParameter("@ToWhsCode", value.ToWhsCode));
                                // Otros
                                cmd.Parameters.Add(new SqlParameter("@CodTipTraslado", value.CodTipTraslado));
                                cmd.Parameters.Add(new SqlParameter("@CodMotTraslado", value.CodMotTraslado));
                                cmd.Parameters.Add(new SqlParameter("@CodTipSalida", value.CodTipSalida));
                                //PIE
                                cmd.Parameters.Add(new SqlParameter("@SlpCode", value.SlpCode));
                                cmd.Parameters.Add(new SqlParameter("@JrnlMemo", value.JrnlMemo));
                                cmd.Parameters.Add(new SqlParameter("@Comments", value.Comments));
                                //USUARIO
                                cmd.Parameters.Add(new SqlParameter("@IdUsuarioCreate", value.IdUsuarioCreate));

                                await cmd.ExecuteNonQueryAsync();

                                value.IdSolicitudTraslado = (int)cmd.Parameters["@IdSolicitudTraslado"].Value;
                            }

                            using (SqlCommand cmd = new SqlCommand(SP_SET_DETALLE_CREATE, conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandTimeout = 0;

                                foreach (var item in value.Item)
                                {
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.Add(new SqlParameter("@IdSolicitudTraslado", value.IdSolicitudTraslado));
                                    cmd.Parameters.Add(new SqlParameter("@Line", item.Line));
                                    cmd.Parameters.Add(new SqlParameter("@ItemCode", item.ItemCode));
                                    cmd.Parameters.Add(new SqlParameter("@Dscription", item.Dscription));
                                    cmd.Parameters.Add(new SqlParameter("@FromWhsCode", item.FromWhsCode));
                                    cmd.Parameters.Add(new SqlParameter("@ToWhsCode", item.ToWhsCode));
                                    cmd.Parameters.Add(new SqlParameter("@UnitMsr", item.UnitMsr));
                                    cmd.Parameters.Add(new SqlParameter("@Quantity", item.Quantity));
                                    cmd.Parameters.Add(new SqlParameter("@OpenQty", item.OpenQty));
                                    cmd.Parameters.Add(new SqlParameter("@OpenQtyRd", item.OpenQtyRd));
                                    cmd.Parameters.Add(new SqlParameter("@IdUsuarioCreate", item.IdUsuarioCreate));

                                    await cmd.ExecuteNonQueryAsync();
                                }
                            }
                            #endregion


                            #region <<< SAP >>>
                            try
                            {
                                #region <<< CREACIÓN DE SOLICITUD DE TRASLADO EN SAP >>>
                                // Creacion de la solicitud de traslado en la base de datos SAP B1
                                // ===========================================================================================
                                // CABECERA
                                // ===========================================================================================
                                documentIns.DocDate = value.DocDate;
                                documentIns.DueDate = value.DocDueDate;
                                documentIns.TaxDate = value.TaxDate;
                                documentIns.DocObjectCode = BoObjectTypes.oInventoryTransferRequest;
                                // ===========================================================================================
                                // SOCIO DE NEGOCIO
                                // ===========================================================================================
                                documentIns.CardCode = value.CardCode;
                                documentIns.CardName = value.CardName;
                                documentIns.ContactPerson = value.CntctCode;
                                documentIns.Address = value.Address;
                                // ===========================================================================================
                                // OTROS
                                // ===========================================================================================
                                documentIns.FromWarehouse = value.FromWhsCode;
                                documentIns.ToWarehouse = value.ToWhsCode;
                                if (value.CodTipTraslado != "") documentIns.UserFields.Fields.Item("U_FIB_TIP_TRAS").Value = value.CodTipTraslado;
                                if (value.CodMotTraslado != "") documentIns.UserFields.Fields.Item("U_BPP_MDMT").Value = value.CodMotTraslado;
                                if (value.CodTipSalida   != "") documentIns.UserFields.Fields.Item("U_BPP_MDTS").Value = value.CodTipSalida;

                                // ===========================================================================================
                                // PIE
                                // ===========================================================================================
                                documentIns.SalesPersonCode = value.SlpCode;
                                documentIns.JournalMemo = value.JrnlMemo;
                                documentIns.Comments = value.Comments;

                                // ===========================================================================================
                                // DETALLE
                                // ===========================================================================================
                                foreach (var item in value.Item)
                                {
                                    documentIns.Lines.ItemCode = item.ItemCode;
                                    documentIns.Lines.ItemDescription = item.Dscription;
                                    documentIns.Lines.FromWarehouseCode = item.FromWhsCode;
                                    documentIns.Lines.WarehouseCode = item.ToWhsCode;
                                    documentIns.Lines.Quantity = (double)item.Quantity;
                                    // ===========================================================================================
                                    // Relación entre la solicitud de tralado de la base local y SAP
                                    // ===========================================================================================
                                    documentIns.Lines.UserFields.Fields.Item("U_FIB_BASETYPE").Value = -1;
                                    documentIns.Lines.UserFields.Fields.Item("U_FIB_BASEENTRY").Value = value.IdSolicitudTraslado;
                                    documentIns.Lines.UserFields.Fields.Item("U_FIB_BASELINENUM").Value = item.Line;
                                    // ===========================================================================================
                                    // Relación entre la solicitud de tralado de la base local y SAP
                                    // ===========================================================================================
                                    documentIns.Lines.Add();
                                }

                                var reg = documentIns.Add();

                                if (reg != 0)
                                {
                                    transaction.Rollback();
                                    if (RepositoryBaseSap.oCompany is not null)
                                    {
                                        if (RepositoryBaseSap.oCompany.InTransaction)
                                        {
                                            RepositoryBaseSap.oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                                        }
                                    }
                                    if (RepositoryBaseSap.oCompany is not null)
                                    {
                                        if (RepositoryBaseSap.oCompany.Connected)
                                        {
                                            _connectionSap.DisConnectToCompany();
                                        }
                                    }
                                    resultadoTransaccion.IdRegistro = -1;
                                    resultadoTransaccion.ResultadoCodigo = -1;
                                    resultadoTransaccion.ResultadoDescripcion = RepositoryBaseSap.oCompany.GetLastErrorDescription();
                                    return resultadoTransaccion;
                                }
                                else
                                {
                                    var docEntryTmp = RepositoryBaseSap.oCompany.GetNewObjectKey();
                                    value.DocEntry = docEntryTmp == null ? 0 : int.Parse(docEntryTmp);
                                    documentQry.GetByKey(value.DocEntry);
                                }
                                #endregion
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                if (RepositoryBaseSap.oCompany is not null)
                                {
                                    if (RepositoryBaseSap.oCompany.InTransaction)
                                    {
                                        RepositoryBaseSap.oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                                    }
                                }
                                if (RepositoryBaseSap.oCompany is not null)
                                {
                                    if (RepositoryBaseSap.oCompany.Connected)
                                    {
                                        _connectionSap.DisConnectToCompany();
                                    }
                                }
                                resultadoTransaccion.IdRegistro = -1;
                                resultadoTransaccion.ResultadoCodigo = -1;
                                resultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
                                return resultadoTransaccion;
                            }
                            #endregion


                            #region <<< ACTUALIZAR LA SOLICITUD DE TRASLADO CON DATOS OBTENIDOS DE SAP >>>
                            using (SqlCommand cmd = new SqlCommand(SP_SET_DATOS_SAP_UPDATE, conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandTimeout = 0;
                                cmd.Parameters.Add(new SqlParameter("@IdSolicitudTraslado", value.IdSolicitudTraslado));
                                cmd.Parameters.Add(new SqlParameter("@DocEntry", documentQry.DocEntry));
                                cmd.Parameters.Add(new SqlParameter("@DocNum", documentQry.DocNum));

                                await cmd.ExecuteNonQueryAsync();
                            }

                            using (SqlCommand cmd = new SqlCommand(SP_SET_DETALLE_DATOS_SAP_UPDATE, conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandTimeout = 0;

                                for (int i = 0; i < documentQry.Lines.Count; i++)
                                {
                                    documentQry.Lines.SetCurrentLine(i);

                                    cmd.Parameters.Clear();
                                    cmd.Parameters.Add(new SqlParameter("@IdSolicitudTraslado", documentQry.Lines.UserFields.Fields.Item("U_FIB_BASEENTRY").Value));
                                    cmd.Parameters.Add(new SqlParameter("@Line", documentQry.Lines.UserFields.Fields.Item("U_FIB_BASELINENUM").Value));
                                    cmd.Parameters.Add(new SqlParameter("@DocEntry", documentQry.Lines.DocEntry));
                                    cmd.Parameters.Add(new SqlParameter("@LineNum", documentQry.Lines.LineNum));
                                    await cmd.ExecuteNonQueryAsync();
                                }
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            if (RepositoryBaseSap.oCompany is not null)
                            {
                                if (RepositoryBaseSap.oCompany.InTransaction)
                                {
                                    RepositoryBaseSap.oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                                }
                            }
                            if (RepositoryBaseSap.oCompany is not null)
                            {
                                if (RepositoryBaseSap.oCompany.Connected)
                                {
                                    _connectionSap.DisConnectToCompany();
                                }
                            }
                            resultadoTransaccion.IdRegistro = -1;
                            resultadoTransaccion.ResultadoCodigo = -1;
                            resultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
                            return resultadoTransaccion;
                        }


                        transaction.Commit();
                        if (RepositoryBaseSap.oCompany is not null)
                        {
                            if (RepositoryBaseSap.oCompany.InTransaction)
                            {
                                RepositoryBaseSap.oCompany.EndTransaction(BoWfTransOpt.wf_Commit);
                            }
                        }
                        if (RepositoryBaseSap.oCompany is not null)
                        {
                            if (RepositoryBaseSap.oCompany.Connected)
                            {
                                _connectionSap.DisConnectToCompany();
                            }
                        }
                        resultadoTransaccion.IdRegistro = 0;
                        resultadoTransaccion.ResultadoCodigo = 0;
                        resultadoTransaccion.ResultadoDescripcion = "Registro procesado con éxito ..!";
                        return resultadoTransaccion;
                    }
                }
            }
            catch (Exception ex)
            {
                resultadoTransaccion.IdRegistro = -1;
                resultadoTransaccion.ResultadoCodigo = -1;
                resultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
                return resultadoTransaccion;
            }
        }
        public async Task<ResultadoTransaccion<SolicitudTrasladoEntity>> SetUpdate(SolicitudTrasladoEntity value)
        {
            var responde = new SolicitudTrasladoSapEntity();
            var resultadoTransaccion = new ResultadoTransaccion<SolicitudTrasladoEntity>();

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

                        _connectionSap.ConnectToCompany(_cnxDiApiSap);
                        RepositoryBaseSap.oCompany.StartTransaction();

                        StockTransfer documentUpd = RepositoryBaseSap.oCompany.GetBusinessObject(BoObjectTypes.oInventoryTransferRequest);

                        try
                        {
                            #region <<< CREACIÓN DE SOLICITUD DE TRASLADO >>>
                            // Creación de la solicitud de traslado en la base de datos LOCAL
                            using (SqlCommand cmd = new SqlCommand(SP_SET_CREATE, conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandTimeout = 0;
                                // CABECERA
                                cmd.Parameters.Add(new SqlParameter("@IdSolicitudTraslado", SqlDbType.Int)).Direction = ParameterDirection.Output;
                                cmd.Parameters.Add(new SqlParameter("@Numero", value.Numero));
                                cmd.Parameters.Add(new SqlParameter("@DocDate", value.DocDate));
                                cmd.Parameters.Add(new SqlParameter("@DocDueDate", value.DocDueDate));
                                cmd.Parameters.Add(new SqlParameter("@TaxDate", value.TaxDate));
                                // CLIENTE
                                cmd.Parameters.Add(new SqlParameter("@CardCode", value.CardCode));
                                cmd.Parameters.Add(new SqlParameter("@CardName", value.CardName));
                                cmd.Parameters.Add(new SqlParameter("@CntctCode", value.CntctCode));
                                cmd.Parameters.Add(new SqlParameter("@Address", value.Address));
                                // ALMACÉN
                                cmd.Parameters.Add(new SqlParameter("@FromWhsCode", value.FromWhsCode));
                                cmd.Parameters.Add(new SqlParameter("@ToWhsCode", value.ToWhsCode));
                                // Otros
                                cmd.Parameters.Add(new SqlParameter("@CodTipTraslado", value.CodTipTraslado));
                                cmd.Parameters.Add(new SqlParameter("@CodMotTraslado", value.CodMotTraslado));
                                cmd.Parameters.Add(new SqlParameter("@CodTipSalida", value.CodTipSalida));
                                //PIE
                                cmd.Parameters.Add(new SqlParameter("@SlpCode", value.SlpCode));
                                cmd.Parameters.Add(new SqlParameter("@JrnlMemo", value.JrnlMemo));
                                cmd.Parameters.Add(new SqlParameter("@Comments", value.Comments));
                                //USUARIO
                                cmd.Parameters.Add(new SqlParameter("@IdUsuarioUpdate", value.IdUsuarioUpdate));

                                await cmd.ExecuteNonQueryAsync();

                                value.IdSolicitudTraslado = (int)cmd.Parameters["@IdSolicitudTraslado"].Value;
                            }

                            using (SqlCommand cmd = new SqlCommand(SP_SET_DETALLE_CREATE, conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandTimeout = 0;

                                foreach (var item in value.Item)
                                {
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.Add(new SqlParameter("@IdSolicitudTraslado", value.IdSolicitudTraslado));
                                    cmd.Parameters.Add(new SqlParameter("@Line", item.Line));
                                    cmd.Parameters.Add(new SqlParameter("@ItemCode", item.ItemCode));
                                    cmd.Parameters.Add(new SqlParameter("@Dscription", item.Dscription));
                                    cmd.Parameters.Add(new SqlParameter("@FromWhsCode", item.FromWhsCode));
                                    cmd.Parameters.Add(new SqlParameter("@ToWhsCode", item.ToWhsCode));
                                    cmd.Parameters.Add(new SqlParameter("@UnitMsr", item.UnitMsr));
                                    cmd.Parameters.Add(new SqlParameter("@Quantity", item.Quantity));
                                    cmd.Parameters.Add(new SqlParameter("@OpenQty", item.OpenQty));
                                    cmd.Parameters.Add(new SqlParameter("@OpenQtyRd", item.OpenQtyRd));
                                    cmd.Parameters.Add(new SqlParameter("@IdUsuarioUpdate", item.IdUsuarioUpdate));

                                    await cmd.ExecuteNonQueryAsync();
                                }
                            }
                            #endregion


                            #region <<< SAP >>>
                            try
                            {
                                #region <<< CREACIÓN DE SOLICITUD DE TRASLADO EN SAP >>>
                                // Creacion de la solicitud de traslado en la base de datos SAP B1
                                // ===========================================================================================
                                // CABECERA
                                // ===========================================================================================
                                documentUpd.DocDate = value.DocDate;
                                documentUpd.DueDate = value.DocDueDate;
                                documentUpd.TaxDate = value.TaxDate;
                                documentUpd.DocObjectCode = BoObjectTypes.oInventoryTransferRequest;
                                // ===========================================================================================
                                // SOCIO DE NEGOCIO
                                // ===========================================================================================
                                documentUpd.CardCode = value.CardCode;
                                documentUpd.CardName = value.CardName;
                                documentUpd.ContactPerson = value.CntctCode;
                                documentUpd.Address = value.Address;
                                // ===========================================================================================
                                // OTROS
                                // ===========================================================================================
                                documentUpd.FromWarehouse = value.FromWhsCode;
                                documentUpd.ToWarehouse = value.ToWhsCode;
                                if (value.CodTipTraslado != "") documentUpd.UserFields.Fields.Item("U_FIB_TIP_TRAS").Value = value.CodTipTraslado;
                                if (value.CodMotTraslado != "") documentUpd.UserFields.Fields.Item("U_BPP_MDMT").Value = value.CodMotTraslado;
                                if (value.CodTipSalida != "") documentUpd.UserFields.Fields.Item("U_BPP_MDTS").Value = value.CodTipSalida;

                                // ===========================================================================================
                                // PIE
                                // ===========================================================================================
                                documentUpd.SalesPersonCode = value.SlpCode;
                                documentUpd.JournalMemo = value.JrnlMemo;
                                documentUpd.Comments = value.Comments;

                                // ===========================================================================================
                                // DETALLE
                                // ===========================================================================================
                                foreach (var item in value.Item)
                                {
                                    documentUpd.Lines.SetCurrentLine(item.LineNum);
                                    documentUpd.Lines.ItemCode = item.ItemCode;
                                    documentUpd.Lines.ItemDescription = item.Dscription;
                                    documentUpd.Lines.FromWarehouseCode = item.FromWhsCode;
                                    documentUpd.Lines.WarehouseCode = item.ToWhsCode;
                                    documentUpd.Lines.Quantity = item.Quantity;
                                    documentUpd.Lines.Add();
                                }

                                if(documentUpd.GetByKey(value.DocEntry))
                                {
                                    var reg = documentUpd.Update();

                                    if (reg != 0)
                                    {
                                        transaction.Rollback();
                                        if (RepositoryBaseSap.oCompany is not null)
                                        {
                                            if (RepositoryBaseSap.oCompany.InTransaction)
                                            {
                                                RepositoryBaseSap.oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                                            }
                                        }
                                        if (RepositoryBaseSap.oCompany is not null)
                                        {
                                            if (RepositoryBaseSap.oCompany.Connected)
                                            {
                                                _connectionSap.DisConnectToCompany();
                                            }
                                        }
                                        resultadoTransaccion.IdRegistro = -1;
                                        resultadoTransaccion.ResultadoCodigo = -1;
                                        resultadoTransaccion.ResultadoDescripcion = RepositoryBaseSap.oCompany.GetLastErrorDescription();
                                        return resultadoTransaccion;
                                    }
                                }
                                #endregion
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                if (RepositoryBaseSap.oCompany is not null)
                                {
                                    if (RepositoryBaseSap.oCompany.InTransaction)
                                    {
                                        RepositoryBaseSap.oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                                    }
                                }
                                if (RepositoryBaseSap.oCompany is not null)
                                {
                                    if (RepositoryBaseSap.oCompany.Connected)
                                    {
                                        _connectionSap.DisConnectToCompany();
                                    }
                                }
                                resultadoTransaccion.IdRegistro = -1;
                                resultadoTransaccion.ResultadoCodigo = -1;
                                resultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
                                return resultadoTransaccion;
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            if (RepositoryBaseSap.oCompany is not null)
                            {
                                if (RepositoryBaseSap.oCompany.InTransaction)
                                {
                                    RepositoryBaseSap.oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                                }
                            }
                            if (RepositoryBaseSap.oCompany is not null)
                            {
                                if (RepositoryBaseSap.oCompany.Connected)
                                {
                                    _connectionSap.DisConnectToCompany();
                                }
                            }
                            resultadoTransaccion.IdRegistro = -1;
                            resultadoTransaccion.ResultadoCodigo = -1;
                            resultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
                            return resultadoTransaccion;
                        }


                        transaction.Commit();
                        if (RepositoryBaseSap.oCompany is not null)
                        {
                            if (RepositoryBaseSap.oCompany.InTransaction)
                            {
                                RepositoryBaseSap.oCompany.EndTransaction(BoWfTransOpt.wf_Commit);
                            }
                        }
                        if (RepositoryBaseSap.oCompany is not null)
                        {
                            if (RepositoryBaseSap.oCompany.Connected)
                            {
                                _connectionSap.DisConnectToCompany();
                            }
                        }
                        resultadoTransaccion.IdRegistro = 0;
                        resultadoTransaccion.ResultadoCodigo = 0;
                        resultadoTransaccion.ResultadoDescripcion = "Registro procesado con éxito ..!";
                        return resultadoTransaccion;
                    }
                }
            }
            catch (Exception ex)
            {
                resultadoTransaccion.IdRegistro = -1;
                resultadoTransaccion.ResultadoCodigo = -1;
                resultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
                return resultadoTransaccion;
            }
        }
        public async Task<ResultadoTransaccion<SolicitudTrasladoEntity>> SetClose(SolicitudTrasladoEntity value)
        {
            var responde = new SolicitudTrasladoSapEntity();
            var resultadoTransaccion = new ResultadoTransaccion<SolicitudTrasladoEntity>();

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

                        _connectionSap.ConnectToCompany(_cnxDiApiSap);
                        RepositoryBaseSap.oCompany.StartTransaction();

                        StockTransfer documentQry = RepositoryBaseSap.oCompany.GetBusinessObject(BoObjectTypes.oInventoryTransferRequest);

                        try
                        {
                            #region <<< CREACIÓN DE SOLICITUD DE TRASLADO >>>
                            // Creación de la solicitud de traslado en la base de datos LOCAL
                            using (SqlCommand cmd = new SqlCommand(SP_SET_CREATE, conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandTimeout = 0;
                                // CABECERA
                                cmd.Parameters.Add(new SqlParameter("@IdSolicitudTraslado", value.IdSolicitudTraslado));
                                cmd.Parameters.Add(new SqlParameter("@IdUsuarioClose", value.IdUsuarioClose));

                                await cmd.ExecuteNonQueryAsync();
                            }
                            #endregion


                            #region <<< SAP >>>
                            try
                            {
                                #region <<< CREACIÓN DE SOLICITUD DE TRASLADO EN SAP >>>
                                // Creacion de la solicitud de traslado en la base de datos SAP B1
                                // ===========================================================================================
                                // CABECERA
                                // ===========================================================================================
                                documentQry.GetByKey(value.DocEntry);

                                var reg = documentQry.Close();

                                if (reg != 0)
                                {
                                    transaction.Rollback();
                                    if (RepositoryBaseSap.oCompany is not null)
                                    {
                                        if (RepositoryBaseSap.oCompany.InTransaction)
                                        {
                                            RepositoryBaseSap.oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                                        }
                                    }
                                    if (RepositoryBaseSap.oCompany is not null)
                                    {
                                        if (RepositoryBaseSap.oCompany.Connected)
                                        {
                                            _connectionSap.DisConnectToCompany();
                                        }
                                    }
                                    resultadoTransaccion.IdRegistro = -1;
                                    resultadoTransaccion.ResultadoCodigo = -1;
                                    resultadoTransaccion.ResultadoDescripcion = RepositoryBaseSap.oCompany.GetLastErrorDescription();
                                    return resultadoTransaccion;
                                }
                                #endregion
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                if (RepositoryBaseSap.oCompany is not null)
                                {
                                    if (RepositoryBaseSap.oCompany.InTransaction)
                                    {
                                        RepositoryBaseSap.oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                                    }
                                }
                                if (RepositoryBaseSap.oCompany is not null)
                                {
                                    if (RepositoryBaseSap.oCompany.Connected)
                                    {
                                        _connectionSap.DisConnectToCompany();
                                    }
                                }
                                resultadoTransaccion.IdRegistro = -1;
                                resultadoTransaccion.ResultadoCodigo = -1;
                                resultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
                                return resultadoTransaccion;
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            if (RepositoryBaseSap.oCompany is not null)
                            {
                                if (RepositoryBaseSap.oCompany.InTransaction)
                                {
                                    RepositoryBaseSap.oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                                }
                            }
                            if (RepositoryBaseSap.oCompany is not null)
                            {
                                if (RepositoryBaseSap.oCompany.Connected)
                                {
                                    _connectionSap.DisConnectToCompany();
                                }
                            }
                            resultadoTransaccion.IdRegistro = -1;
                            resultadoTransaccion.ResultadoCodigo = -1;
                            resultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
                            return resultadoTransaccion;
                        }


                        transaction.Commit();
                        if (RepositoryBaseSap.oCompany is not null)
                        {
                            if (RepositoryBaseSap.oCompany.InTransaction)
                            {
                                RepositoryBaseSap.oCompany.EndTransaction(BoWfTransOpt.wf_Commit);
                            }
                        }
                        if (RepositoryBaseSap.oCompany is not null)
                        {
                            if (RepositoryBaseSap.oCompany.Connected)
                            {
                                _connectionSap.DisConnectToCompany();
                            }
                        }
                        resultadoTransaccion.IdRegistro = 0;
                        resultadoTransaccion.ResultadoCodigo = 0;
                        resultadoTransaccion.ResultadoDescripcion = "Registro procesado con éxito ..!";
                        return resultadoTransaccion;
                    }
                }
            }
            catch (Exception ex)
            {
                resultadoTransaccion.IdRegistro = -1;
                resultadoTransaccion.ResultadoCodigo = -1;
                resultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
                return resultadoTransaccion;
            }
        }
    }
}
