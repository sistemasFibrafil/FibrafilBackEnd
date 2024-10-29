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
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
namespace Net.Data.Web
{
    public class OrdenVentaRepository : RepositoryBase<OrdenVentaEntity>, IOrdenVentaRepository
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
        const string SP_GET_NUMERO = "VEN_SP_GetOrdenVentaNumero";
        const string SP_SET_CREATE = DB_ESQUEMA + "VEN_SP_SetOrdenVentaCreate";
        const string SP_SET_DETALLE_CREATE = DB_ESQUEMA + "VEN_SP_SetOrdenVentaDetalleCreate";
        const string SP_SET_DATOS_SAP_UPDATE = DB_ESQUEMA + "VEN_SP_SetOrdenVentaDatosSapUpdate";
        const string SP_SET__DETALLE_DATOS_SAP_UPDATE = DB_ESQUEMA + "VEN_SP_SetOrdenVentaDetalleDatosSapUpdate";


        public OrdenVentaRepository(IConnectionSql context, IConfiguration configuration)
            : base(context)
        {
            _configuration = configuration;
            _aplicacionName = GetType().Name;
            _connectionSap = new ConnectionSap();
            _cnxDos = Utilidades.GetExtraerCadenaConexion(configuration, "ParametersConectionDos");
            _cnxSap = Utilidades.GetExtraerCadenaConexion(configuration, "ParametersConectionSap");
            _cnxDiApiSap = Utilidades.GetExtraerCadenaConexionDiApiSap(configuration, "ParametersConectionDiApiSap");
        }


        public async Task<ResultadoTransaccion<OrdenVentaEntity>> GetNumero()
        {
            var response = new OrdenVentaEntity();
            var resultadoTransaccion = new ResultadoTransaccion<OrdenVentaEntity>();

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
                            response = context.Convert<OrdenVentaEntity>(reader);
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

        public async Task<ResultadoTransaccion<OrdenVentaEntity>> SetCreate(OrdenVentaEntity value)
        {
            var resultadoTransaccion = new ResultadoTransaccion<OrdenVentaEntity>();

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

                        Documents documentIns = RepositoryBaseSap.oCompany.GetBusinessObject(BoObjectTypes.oOrders);
                        Documents documentQry = RepositoryBaseSap.oCompany.GetBusinessObject(BoObjectTypes.oOrders);

                        try
                        {
                            #region <<< CREACIÓN DE ORDEN DE VENTA >>>
                            // Creación de la OV en la base de datos LOCAL
                            using (SqlCommand cmd = new SqlCommand(SP_SET_CREATE, conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandTimeout = 0;
                                // CABECERA
                                cmd.Parameters.Add(new SqlParameter("@IdOrdenVenta", SqlDbType.Int)).Direction = ParameterDirection.Output;
                                cmd.Parameters.Add(new SqlParameter("@DocDate", value.DocDate));
                                cmd.Parameters.Add(new SqlParameter("@DocDueDate", value.DocDueDate));
                                cmd.Parameters.Add(new SqlParameter("@TaxDate", value.TaxDate));
                                // CLIENTE
                                cmd.Parameters.Add(new SqlParameter("@CardCode", value.CardCode));
                                cmd.Parameters.Add(new SqlParameter("@LicTradNum", value.LicTradNum));
                                cmd.Parameters.Add(new SqlParameter("@CardName", value.CardName));
                                cmd.Parameters.Add(new SqlParameter("@CntctCode", value.CntctCode));
                                cmd.Parameters.Add(new SqlParameter("@PayToCode", value.PayToCode));
                                cmd.Parameters.Add(new SqlParameter("@Address", value.Address));
                                cmd.Parameters.Add(new SqlParameter("@ShipToCode", value.ShipToCode));
                                cmd.Parameters.Add(new SqlParameter("@Address2", value.Address2));
                                cmd.Parameters.Add(new SqlParameter("@NumOrdCom", value.NumOrdCom));
                                cmd.Parameters.Add(new SqlParameter("@DocCur", value.DocCur));
                                cmd.Parameters.Add(new SqlParameter("@DocRate", value.DocRate));
                                cmd.Parameters.Add(new SqlParameter("@GroupNum", value.GroupNum));
                                // AGENCIA
                                cmd.Parameters.Add(new SqlParameter("@CodAgencia", value.CodAgencia));
                                cmd.Parameters.Add(new SqlParameter("@RucAgencia", value.RucAgencia));
                                cmd.Parameters.Add(new SqlParameter("@NomAgencia", value.NomAgencia));
                                cmd.Parameters.Add(new SqlParameter("@CodDirAgencia", value.CodDirAgencia));
                                cmd.Parameters.Add(new SqlParameter("@DirAgencia", value.DirAgencia));
                                // EXPORTACION
                                cmd.Parameters.Add(new SqlParameter("@CodTipFlete", value.CodTipFlete));
                                cmd.Parameters.Add(new SqlParameter("@ValorFlete", value.ValorFlete));
                                cmd.Parameters.Add(new SqlParameter("@TotalFlete", value.TotalFlete));
                                cmd.Parameters.Add(new SqlParameter("@ImporteSeguro", value.ImporteSeguro));
                                cmd.Parameters.Add(new SqlParameter("@Puerto", value.Puerto));
                                // OTROS
                                cmd.Parameters.Add(new SqlParameter("@CodTipVenta", value.CodTipVenta));
                                // PIE
                                cmd.Parameters.Add(new SqlParameter("@SlpCode", value.SlpCode));
                                cmd.Parameters.Add(new SqlParameter("@Comments", value.Comments));
                                // TOTALES
                                cmd.Parameters.Add(new SqlParameter("@DiscPrcnt", value.DiscPrcnt));
                                cmd.Parameters.Add(new SqlParameter("@DiscSum", value.DiscSum));
                                cmd.Parameters.Add(new SqlParameter("@VatSum", value.VatSum));
                                cmd.Parameters.Add(new SqlParameter("@DocTotal", value.DocTotal));
                                // USUARIO
                                cmd.Parameters.Add(new SqlParameter("@IdUsuarioCreate", value.IdUsuarioCreate));

                                await cmd.ExecuteNonQueryAsync();

                                value.IdOrdenVenta = (int)cmd.Parameters["@IdOrdenVenta"].Value;
                            }

                            using (SqlCommand cmd = new SqlCommand(SP_SET_DETALLE_CREATE, conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandTimeout = 0;

                                foreach (var item in value.Linea)
                                {
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.Add(new SqlParameter("@IdOrdenVenta", value.IdOrdenVenta));
                                    cmd.Parameters.Add(new SqlParameter("@Line", item.Line));
                                    cmd.Parameters.Add(new SqlParameter("@ItemCode", item.ItemCode));
                                    cmd.Parameters.Add(new SqlParameter("@Dscription", item.Dscription));
                                    cmd.Parameters.Add(new SqlParameter("@WhsCode", item.WhsCode));
                                    cmd.Parameters.Add(new SqlParameter("@UnitMsr", item.UnitMsr));
                                    cmd.Parameters.Add(new SqlParameter("@Quantity", item.Quantity));
                                    cmd.Parameters.Add(new SqlParameter("@OpenQty", item.OpenQty));
                                    cmd.Parameters.Add(new SqlParameter("@OpenQtyRd", item.OpenQtyRd));
                                    cmd.Parameters.Add(new SqlParameter("@Currency", item.Currency));
                                    cmd.Parameters.Add(new SqlParameter("@PriceBefDi", item.PriceBefDi));
                                    cmd.Parameters.Add(new SqlParameter("@DiscPrcnt", item.DiscPrcnt));
                                    cmd.Parameters.Add(new SqlParameter("@Price", item.Price));
                                    cmd.Parameters.Add(new SqlParameter("@LineTotal", item.LineTotal));
                                    cmd.Parameters.Add(new SqlParameter("@TaxCode", item.TaxCode));
                                    cmd.Parameters.Add(new SqlParameter("@VatPrcnt", item.VatPrcnt));
                                    cmd.Parameters.Add(new SqlParameter("@VatSum", item.VatSum));

                                    await cmd.ExecuteNonQueryAsync();
                                }
                            }
                            #endregion


                            #region <<< SAP >>>
                            try
                            {
                                #region <<< CREACIÓN DE ORDEN DE VENTA EN SAP >>>
                                // Creacion de la solicitud de traslado en la base de datos SAP B1
                                // ===========================================================================================
                                // CABECERA
                                // ===========================================================================================
                                documentIns.DocDate = value.DocDate;
                                documentIns.DocDueDate = value.DocDueDate;
                                documentIns.TaxDate = value.TaxDate;
                                documentIns.DocType = BoDocumentTypes.dDocument_Items;
                                // ===========================================================================================
                                // SOCIO DE NEGOCIO
                                // ===========================================================================================
                                documentIns.CardCode = value.CardCode;
                                documentIns.CardName = value.CardName;
                                documentIns.ContactPersonCode = value.CntctCode;
                                documentIns.PayToCode = value.PayToCode;
                                documentIns.Address = value.Address;
                                documentIns.ShipToCode = value.ShipToCode;
                                documentIns.Address2 = value.Address2;
                                documentIns.UserFields.Fields.Item("U_NroOrden").Value = value.NumOrdCom;
                                documentIns.DocCurrency = value.DocCur;
                                documentIns.DocRate = value.DocRate;
                                documentIns.GroupNumber = value.GroupNum;
                                // ===========================================================================================
                                // AGENCIA
                                // ===========================================================================================
                                documentIns.UserFields.Fields.Item("U_BPP_MDCT").Value = value.CodAgencia;
                                documentIns.UserFields.Fields.Item("U_BPP_MDRT").Value = value.RucAgencia;
                                documentIns.UserFields.Fields.Item("U_BPP_MDNT").Value = value.NomAgencia;
                                documentIns.UserFields.Fields.Item("U_BPP_MDDT").Value = value.DirAgencia;
                                // ===========================================================================================
                                // EXPORTACION
                                // ===========================================================================================
                                documentIns.UserFields.Fields.Item("U_TipoFlete").Value = value.CodTipFlete;
                                documentIns.UserFields.Fields.Item("U_ValorFlete").Value = value.ValorFlete.ToString();
                                documentIns.UserFields.Fields.Item("U_FIB_TFLETE").Value = value.TotalFlete;
                                documentIns.UserFields.Fields.Item("U_FIB_IMPSEG").Value = value.ImporteSeguro;
                                documentIns.UserFields.Fields.Item("U_FIB_PUERTO").Value = value.Puerto;
                                // ===========================================================================================
                                // OTROS
                                // ===========================================================================================
                                documentIns.UserFields.Fields.Item("U_STR_TVENTA").Value = value.CodTipVenta;
                                // ===========================================================================================
                                // PIE
                                // ===========================================================================================
                                documentIns.SalesPersonCode = value.SlpCode;
                                documentIns.Comments = value.Comments;
                                // ===========================================================================================
                                // TOTALES
                                // ===========================================================================================
                                documentIns.DiscountPercent = value.DiscPrcnt;
                                documentIns.DocTotal = value.DocTotal;
                                // ===========================================================================================
                                // DETALLE
                                // ===========================================================================================
                                foreach (var linea in value.Linea)
                                {
                                    documentIns.Lines.ItemCode = linea.ItemCode;
                                    documentIns.Lines.ItemDescription = linea.Dscription;
                                    documentIns.Lines.WarehouseCode = linea.WhsCode;
                                    documentIns.Lines.WarehouseCode = linea.WhsCode;
                                    documentIns.Lines.Quantity = linea.Quantity;
                                    documentIns.Lines.Currency = linea.Currency;
                                    documentIns.Lines.UnitPrice = linea.Price;
                                    documentIns.Lines.DiscountPercent = linea.DiscPrcnt;
                                    documentIns.Lines.LineTotal = linea.LineTotal;
                                    documentIns.Lines.TaxCode = linea.TaxCode;
                                    // ===========================================================================================
                                    // Relación entre la OV de la base local y SAP
                                    // ===========================================================================================
                                    documentIns.Lines.UserFields.Fields.Item("U_FIB_BASETYPE").Value = -1;
                                    documentIns.Lines.UserFields.Fields.Item("U_FIB_BASEENTRY").Value = value.IdOrdenVenta;
                                    documentIns.Lines.UserFields.Fields.Item("U_FIB_BASELINENUM").Value = linea.Line;
                                    // ===========================================================================================
                                    // Relación entre la OV de la base local y SAP
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


                            #region <<< ACTUALIZAR LA OV CON DATOS OBTENIDOS DE SAP >>>
                            using (SqlCommand cmd = new SqlCommand(SP_SET_DATOS_SAP_UPDATE, conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandTimeout = 0;
                                cmd.Parameters.Add(new SqlParameter("@IdOrdenVenta", value.IdOrdenVenta));
                                cmd.Parameters.Add(new SqlParameter("@DocEntry", documentQry.DocEntry));
                                cmd.Parameters.Add(new SqlParameter("@DocNum", documentQry.DocNum));

                                await cmd.ExecuteNonQueryAsync();
                            }

                            using (SqlCommand cmd = new SqlCommand(SP_SET__DETALLE_DATOS_SAP_UPDATE, conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandTimeout = 0;

                                for (int i = 0; i < documentQry.Lines.Count; i++)
                                {
                                    documentQry.Lines.SetCurrentLine(i);

                                    cmd.Parameters.Clear();
                                    cmd.Parameters.Add(new SqlParameter("@IdOrdenVenta", documentQry.Lines.UserFields.Fields.Item("U_FIB_BASEENTRY").Value));
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

            }
            return resultadoTransaccion;
        }

        public async Task<ResultadoTransaccion<OrdenVentaEntity>> SetUpdate(OrdenVentaEntity value)
        {
            var resultadoTransaccion = new ResultadoTransaccion<OrdenVentaEntity>();

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

                        Documents documentUpd = RepositoryBaseSap.oCompany.GetBusinessObject(BoObjectTypes.oOrders);

                        try
                        {
                            #region <<< CREACIÓN DE ORDEN DE VENTA >>>
                            // Creación de la OV en la base de datos LOCAL
                            using (SqlCommand cmd = new SqlCommand(SP_SET_CREATE, conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandTimeout = 0;
                                // CABECERA
                                cmd.Parameters.Add(new SqlParameter("@IdOrdenVenta", SqlDbType.Int)).Direction = ParameterDirection.Output;
                                cmd.Parameters.Add(new SqlParameter("@DocDate", value.DocDate));
                                cmd.Parameters.Add(new SqlParameter("@DocDueDate", value.DocDueDate));
                                cmd.Parameters.Add(new SqlParameter("@TaxDate", value.TaxDate));
                                // CLIENTE
                                cmd.Parameters.Add(new SqlParameter("@CardCode", value.CardCode));
                                cmd.Parameters.Add(new SqlParameter("@LicTradNum", value.LicTradNum));
                                cmd.Parameters.Add(new SqlParameter("@CardName", value.CardName));
                                cmd.Parameters.Add(new SqlParameter("@CntctCode", value.CntctCode));
                                cmd.Parameters.Add(new SqlParameter("@PayToCode", value.PayToCode));
                                cmd.Parameters.Add(new SqlParameter("@Address", value.Address));
                                cmd.Parameters.Add(new SqlParameter("@ShipToCode", value.ShipToCode));
                                cmd.Parameters.Add(new SqlParameter("@Address2", value.Address2));
                                cmd.Parameters.Add(new SqlParameter("@NumOrdCom", value.NumOrdCom));
                                cmd.Parameters.Add(new SqlParameter("@DocCur", value.DocCur));
                                cmd.Parameters.Add(new SqlParameter("@DocRate", value.DocRate));
                                cmd.Parameters.Add(new SqlParameter("@GroupNum", value.GroupNum));
                                // AGENCIA
                                cmd.Parameters.Add(new SqlParameter("@CodAgencia", value.CodAgencia));
                                cmd.Parameters.Add(new SqlParameter("@RucAgencia", value.RucAgencia));
                                cmd.Parameters.Add(new SqlParameter("@NomAgencia", value.NomAgencia));
                                cmd.Parameters.Add(new SqlParameter("@CodDirAgencia", value.CodDirAgencia));
                                cmd.Parameters.Add(new SqlParameter("@DirAgencia", value.DirAgencia));
                                // EXPORTACION
                                cmd.Parameters.Add(new SqlParameter("@CodTipFlete", value.CodTipFlete));
                                cmd.Parameters.Add(new SqlParameter("@ValorFlete", value.ValorFlete));
                                cmd.Parameters.Add(new SqlParameter("@TotalFlete", value.TotalFlete));
                                cmd.Parameters.Add(new SqlParameter("@ImporteSeguro", value.ImporteSeguro));
                                cmd.Parameters.Add(new SqlParameter("@Puerto", value.Puerto));
                                // OTROS
                                cmd.Parameters.Add(new SqlParameter("@CodTipVenta", value.CodTipVenta));
                                // PIE
                                cmd.Parameters.Add(new SqlParameter("@SlpCode", value.SlpCode));
                                cmd.Parameters.Add(new SqlParameter("@Comments", value.Comments));
                                // TOTALES
                                cmd.Parameters.Add(new SqlParameter("@DiscPrcnt", value.DiscPrcnt));
                                cmd.Parameters.Add(new SqlParameter("@DiscSum", value.DiscSum));
                                cmd.Parameters.Add(new SqlParameter("@VatSum", value.VatSum));
                                cmd.Parameters.Add(new SqlParameter("@DocTotal", value.DocTotal));
                                // USUARIO
                                cmd.Parameters.Add(new SqlParameter("@IdUsuarioCreate", value.IdUsuarioCreate));

                                await cmd.ExecuteNonQueryAsync();

                                value.IdOrdenVenta = (int)cmd.Parameters["@IdOrdenVenta"].Value;
                            }

                            using (SqlCommand cmd = new SqlCommand(SP_SET_DETALLE_CREATE, conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandTimeout = 0;

                                foreach (var item in value.Linea)
                                {
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.Add(new SqlParameter("@IdOrdenVenta", value.IdOrdenVenta));
                                    cmd.Parameters.Add(new SqlParameter("@Line", item.Line));
                                    cmd.Parameters.Add(new SqlParameter("@ItemCode", item.ItemCode));
                                    cmd.Parameters.Add(new SqlParameter("@Dscription", item.Dscription));
                                    cmd.Parameters.Add(new SqlParameter("@WhsCode", item.WhsCode));
                                    cmd.Parameters.Add(new SqlParameter("@UnitMsr", item.UnitMsr));
                                    cmd.Parameters.Add(new SqlParameter("@Quantity", item.Quantity));
                                    cmd.Parameters.Add(new SqlParameter("@OpenQty", item.OpenQty));
                                    cmd.Parameters.Add(new SqlParameter("@OpenQtyRd", item.OpenQtyRd));
                                    cmd.Parameters.Add(new SqlParameter("@Currency", item.Currency));
                                    cmd.Parameters.Add(new SqlParameter("@PriceBefDi", item.PriceBefDi));
                                    cmd.Parameters.Add(new SqlParameter("@DiscPrcnt", item.DiscPrcnt));
                                    cmd.Parameters.Add(new SqlParameter("@Price", item.Price));
                                    cmd.Parameters.Add(new SqlParameter("@LineTotal", item.LineTotal));
                                    cmd.Parameters.Add(new SqlParameter("@TaxCode", item.TaxCode));
                                    cmd.Parameters.Add(new SqlParameter("@VatPrcnt", item.VatPrcnt));
                                    cmd.Parameters.Add(new SqlParameter("@VatSum", item.VatSum));

                                    await cmd.ExecuteNonQueryAsync();
                                }
                            }
                            #endregion


                            #region <<< SAP >>>
                            try
                            {
                                #region <<< CREACIÓN DE ORDEN DE VENTA EN SAP >>>
                                // Creacion de la solicitud de traslado en la base de datos SAP B1
                                // ===========================================================================================
                                // CABECERA
                                // ===========================================================================================
                                documentUpd.DocDate = value.DocDate;
                                documentUpd.DocDueDate = value.DocDueDate;
                                documentUpd.TaxDate = value.TaxDate;
                                documentUpd.DocType = BoDocumentTypes.dDocument_Items;
                                // ===========================================================================================
                                // SOCIO DE NEGOCIO
                                // ===========================================================================================
                                documentUpd.CardCode = value.CardCode;
                                documentUpd.CardName = value.CardName;
                                documentUpd.ContactPersonCode = value.CntctCode;
                                documentUpd.PayToCode = value.PayToCode;
                                documentUpd.Address = value.Address;
                                documentUpd.ShipToCode = value.ShipToCode;
                                documentUpd.Address2 = value.Address2;
                                documentUpd.UserFields.Fields.Item("U_NroOrden").Value = value.NumOrdCom;
                                documentUpd.DocCurrency = value.DocCur;
                                documentUpd.DocRate = value.DocRate;
                                documentUpd.GroupNumber = value.GroupNum;
                                // ===========================================================================================
                                // AGENCIA
                                // ===========================================================================================
                                documentUpd.UserFields.Fields.Item("U_BPP_MDCT").Value = value.CodAgencia;
                                documentUpd.UserFields.Fields.Item("U_BPP_MDRT").Value = value.RucAgencia;
                                documentUpd.UserFields.Fields.Item("U_BPP_MDNT").Value = value.NomAgencia;
                                documentUpd.UserFields.Fields.Item("U_BPP_MDDT").Value = value.DirAgencia;
                                // ===========================================================================================
                                // EXPORTACION
                                // ===========================================================================================
                                documentUpd.UserFields.Fields.Item("U_TipoFlete").Value = value.CodTipFlete;
                                documentUpd.UserFields.Fields.Item("U_ValorFlete").Value = value.ValorFlete.ToString();
                                documentUpd.UserFields.Fields.Item("U_FIB_TFLETE").Value = value.TotalFlete;
                                documentUpd.UserFields.Fields.Item("U_FIB_IMPSEG").Value = value.ImporteSeguro;
                                documentUpd.UserFields.Fields.Item("U_FIB_PUERTO").Value = value.Puerto;
                                // ===========================================================================================
                                // OTROS
                                // ===========================================================================================
                                documentUpd.UserFields.Fields.Item("U_STR_TVENTA").Value = value.CodTipVenta;
                                // ===========================================================================================
                                // PIE
                                // ===========================================================================================
                                documentUpd.SalesPersonCode = value.SlpCode;
                                documentUpd.Comments = value.Comments;
                                // ===========================================================================================
                                // TOTALES
                                // ===========================================================================================
                                documentUpd.DiscountPercent = value.DiscPrcnt;
                                documentUpd.DocTotal = value.DocTotal;
                                // ===========================================================================================
                                // DETALLE
                                // ===========================================================================================
                                foreach (var linea in value.Linea)
                                {
                                    documentUpd.Lines.SetCurrentLine(linea.LineNum);
                                    documentUpd.Lines.ItemCode = linea.ItemCode;
                                    documentUpd.Lines.ItemDescription = linea.Dscription;
                                    documentUpd.Lines.WarehouseCode = linea.WhsCode;
                                    documentUpd.Lines.WarehouseCode = linea.WhsCode;
                                    documentUpd.Lines.Quantity = linea.Quantity;
                                    documentUpd.Lines.Currency = linea.Currency;
                                    documentUpd.Lines.UnitPrice = linea.Price;
                                    documentUpd.Lines.DiscountPercent = linea.DiscPrcnt;
                                    documentUpd.Lines.LineTotal = linea.LineTotal;
                                    documentUpd.Lines.TaxCode = linea.TaxCode;
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

            }
            return resultadoTransaccion;
        }
    }
}
