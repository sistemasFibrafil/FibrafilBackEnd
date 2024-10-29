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
using Azure;
using System.Collections.Generic;
namespace Net.Data.Web
{
    public class EntregaVentaRepository : RepositoryBase<EntregaVentaEntity>, IEntregaVentaRepository
    {
        private string _metodoName;
        private string _aplicacionName;
        private readonly Regex regex = new Regex(@"<(\w+)>.*");

        // PARAMETROS DE COXIÓN
        private readonly string _cnxSap;
        private readonly IConfiguration _configuration;
        private readonly IConnectionSap _connectionSap;
        private readonly ConnectionSapEntity _cnxDiApiSap;


        // STORED PROCEDURE
        const string DB_ESQUEMA = "";
        const string SP_SET_ENTREGA_CREATE = DB_ESQUEMA + "FIB_WEB_VE_SP_SetEntregaVentaCreate";
        const string SP_SET_ENTREGA_VAL_SAPL_UPDATE = DB_ESQUEMA + "FIB_WEB_VE_SP_SetEntregaVentaValSapUpdate";
        const string SP_SET_ENTREGA_ITEM_CREATE = DB_ESQUEMA + "FIB_WEB_VE_SP_SetEntregaVentaItemCreate";
        const string SP_SET_ENTREGA_ITEM_VAL_SAP_UPDATE = DB_ESQUEMA + "FIB_WEB_VE_SP_SetEntregaVentaItemValSapUpdate";

        const string SP_SET_PICKING_ITEM_ESTADO_UPDATE = DB_ESQUEMA + "FIB_WEB_VE_SP_SetPickingVentaItemEstadoUpdate";

        const string SP_SET_SERIE_NUMERO_SUNAT_UPDATE = DB_ESQUEMA + "FIB_WEB_GE_SP_SetSerieNumeroSunatUpdate";



        public EntregaVentaRepository(IConnectionSql context, IConfiguration configuration)
            : base(context)
        {
            _configuration = configuration;
            _aplicacionName = GetType().Name;
            _connectionSap = new ConnectionSap();
            _cnxSap = Utilidades.GetExtraerCadenaConexion(configuration, "ParametersConectionSap");
            _cnxDiApiSap = Utilidades.GetExtraerCadenaConexionDiApiSap(configuration, "ParametersConectionDiApiSap");
        }


        public async Task<ResultadoTransaccion<EntregaVentaEntity>> SetCreate(EntregaVentaEntity value)
        {
            var entregaVenta = new EntregaVentaEntity();
            var resultadoTransaccion = new ResultadoTransaccion<EntregaVentaEntity>();

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
                            #region <<< CREACIÓN DE GUÍA >>>
                            // Creación de guía en la base de datos LOCAL

                            using (SqlCommand cmd = new SqlCommand(SP_SET_ENTREGA_CREATE, conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandTimeout = 0;
                                // CABECERA
                                cmd.Parameters.Add(new SqlParameter("@IdEntregaVenta", SqlDbType.Int)).Direction = ParameterDirection.Output;
                                cmd.Parameters.Add(new SqlParameter("@ObjType", value.ObjType));
                                cmd.Parameters.Add(new SqlParameter("@Series", value.Series));
                                cmd.Parameters.Add(new SqlParameter("@TipDocSunat", value.TipDocSunat));
                                cmd.Parameters.Add(new SqlParameter("@SerSunat", value.SerSunat));
                                cmd.Parameters.Add(new SqlParameter("@NumSunat", value.NumSunat));
                                cmd.Parameters.Add(new SqlParameter("@DocStatus", value.DocStatus));
                                cmd.Parameters.Add(new SqlParameter("@DocDate", value.DocDate));
                                cmd.Parameters.Add(new SqlParameter("@DocDueDate", value.DocDueDate));
                                // CLIENTE
                                cmd.Parameters.Add(new SqlParameter("@CardCode", value.CardCode));
                                cmd.Parameters.Add(new SqlParameter("@CardName", value.CardName));
                                cmd.Parameters.Add(new SqlParameter("@LicTradNum", value.LicTradNum));
                                cmd.Parameters.Add(new SqlParameter("@DocCur", value.DocCur));
                                cmd.Parameters.Add(new SqlParameter("@DocRate", value.DocRate));
                                // LOGÍSTICA
                                cmd.Parameters.Add(new SqlParameter("@ShipToCode", value.ShipToCode));
                                cmd.Parameters.Add(new SqlParameter("@Address2", value.Address2));
                                cmd.Parameters.Add(new SqlParameter("@PayToCode", value.PayToCode));
                                cmd.Parameters.Add(new SqlParameter("@Address", value.Address));
                                // Motivo de traslado
                                cmd.Parameters.Add(new SqlParameter("@CodMotTraslado", value.CodMotTraslado));
                                cmd.Parameters.Add(new SqlParameter("@OtrMotTraslado", value.OtrMotTraslado));
                                // FINANZAS
                                cmd.Parameters.Add(new SqlParameter("@CodCondicionPago", value.CodCondicionPago));
                                // EXPORTACIÓN
                                cmd.Parameters.Add(new SqlParameter("@RucDesInternacional", value.RucDesInternacional));
                                cmd.Parameters.Add(new SqlParameter("@DesGuiaInternacional", value.DesGuiaInternacional));
                                cmd.Parameters.Add(new SqlParameter("@DirDesInternacional", value.DirDesInternacional));
                                cmd.Parameters.Add(new SqlParameter("@NumContenedor", value.NumContenedor));
                                cmd.Parameters.Add(new SqlParameter("@NumPrecinto1", value.NumPrecinto1));
                                cmd.Parameters.Add(new SqlParameter("@NumPrecinto2", value.NumPrecinto2));
                                cmd.Parameters.Add(new SqlParameter("@NumPrecinto3", value.NumPrecinto3));
                                cmd.Parameters.Add(new SqlParameter("@NumPrecinto4", value.NumPrecinto4));
                                // TRANSPORTISTA
                                cmd.Parameters.Add(new SqlParameter("@CodTipTransporte", value.CodTipTransporte));
                                // Transporte 1
                                cmd.Parameters.Add(new SqlParameter("@ManTransportista1", value.ManTransportista1));
                                cmd.Parameters.Add(new SqlParameter("@CodTransportista1", value.CodTransportista1));
                                cmd.Parameters.Add(new SqlParameter("@CodTipDocIdeTransportista1", value.CodTipDocIdeTransportista1));
                                cmd.Parameters.Add(new SqlParameter("@RucTransportista1", value.RucTransportista1));
                                cmd.Parameters.Add(new SqlParameter("@NomTransportista1", value.NomTransportista1));
                                cmd.Parameters.Add(new SqlParameter("@NumPlaca1", value.NumPlaca1));
                                // Conductor
                                cmd.Parameters.Add(new SqlParameter("@CodTipDocIdeConductor1", value.CodTipDocIdeConductor1));
                                cmd.Parameters.Add(new SqlParameter("@NumDocIdeConductor1", value.NumDocIdeConductor1));
                                cmd.Parameters.Add(new SqlParameter("@DenConductor1", value.DenConductor1));
                                cmd.Parameters.Add(new SqlParameter("@NomConductor1", value.NomConductor1));
                                cmd.Parameters.Add(new SqlParameter("@ApeConductor1", value.ApeConductor1));
                                cmd.Parameters.Add(new SqlParameter("@LicConductor1", value.LicConductor1));
                                // Transporte 2
                                cmd.Parameters.Add(new SqlParameter("@ManTransportista2", value.ManTransportista2));
                                cmd.Parameters.Add(new SqlParameter("@CodTransportista2", value.CodTransportista2));
                                cmd.Parameters.Add(new SqlParameter("@RucTransportista2", value.RucTransportista2));
                                cmd.Parameters.Add(new SqlParameter("@NomTransportista2", value.NomTransportista2));
                                cmd.Parameters.Add(new SqlParameter("@DirTransportista2", value.DirTransportista2));
                                //PIE
                                cmd.Parameters.Add(new SqlParameter("@SlpCode", value.SlpCode));
                                cmd.Parameters.Add(new SqlParameter("@TotalBulto", value.TotalBulto));
                                cmd.Parameters.Add(new SqlParameter("@TotalKilo", value.TotalKilo));
                                cmd.Parameters.Add(new SqlParameter("@Comments", value.Comments));
                                cmd.Parameters.Add(new SqlParameter("@VatSum", value.VatSum));
                                cmd.Parameters.Add(new SqlParameter("@VatSumFC", value.VatSumFC));
                                cmd.Parameters.Add(new SqlParameter("@VatSumy", value.VatSumy));
                                cmd.Parameters.Add(new SqlParameter("@DocTotal", value.DocTotal));
                                cmd.Parameters.Add(new SqlParameter("@DocTotalFC", value.DocTotalFC));
                                cmd.Parameters.Add(new SqlParameter("@DocTotalSy", value.DocTotalSy));
                                //USUARIO
                                cmd.Parameters.Add(new SqlParameter("@IdUsuarioCreate", value.IdUsuario));

                                await cmd.ExecuteNonQueryAsync();

                                value.IdEntregaVenta = (int)cmd.Parameters["@IdEntregaVenta"].Value;
                            }

                            using (SqlCommand cmd = new SqlCommand(SP_SET_ENTREGA_ITEM_CREATE, conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandTimeout = 0;

                                foreach (var item in value.Item)
                                {
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.Add(new SqlParameter("@IdEntregaVenta", value.IdEntregaVenta));
                                    cmd.Parameters.Add(new SqlParameter("@LineNum", item.LineNum));
                                    cmd.Parameters.Add(new SqlParameter("@ObjType", item.ObjType));
                                    cmd.Parameters.Add(new SqlParameter("@IdBase", item.IdBase));
                                    cmd.Parameters.Add(new SqlParameter("@LineBase", item.LineBase));
                                    cmd.Parameters.Add(new SqlParameter("@BaseType", item.BaseType));
                                    cmd.Parameters.Add(new SqlParameter("@BaseEntry", item.BaseEntry));
                                    cmd.Parameters.Add(new SqlParameter("@BaseLine", item.BaseLine));
                                    cmd.Parameters.Add(new SqlParameter("@ItemCode", item.ItemCode));
                                    cmd.Parameters.Add(new SqlParameter("@Dscription", item.Dscription));
                                    cmd.Parameters.Add(new SqlParameter("@WhsCode", item.WhsCode));
                                    cmd.Parameters.Add(new SqlParameter("@UnitMsr", item.UnitMsr));
                                    cmd.Parameters.Add(new SqlParameter("@Quantity", item.Quantity));
                                    cmd.Parameters.Add(new SqlParameter("@Peso", item.Peso));
                                    cmd.Parameters.Add(new SqlParameter("@TaxCode", item.TaxCode));
                                    cmd.Parameters.Add(new SqlParameter("@AcctCode", item.AcctCode));
                                    cmd.Parameters.Add(new SqlParameter("@Currency", item.Currency));
                                    cmd.Parameters.Add(new SqlParameter("@PriceDefDi", item.PriceDefDi));
                                    cmd.Parameters.Add(new SqlParameter("@DiscPrcnt", item.DiscPrcnt));
                                    cmd.Parameters.Add(new SqlParameter("@Price", item.Price));
                                    cmd.Parameters.Add(new SqlParameter("@LineTotal", item.LineTotal));
                                    cmd.Parameters.Add(new SqlParameter("@TotalFrgn", item.TotalFrgn));
                                    cmd.Parameters.Add(new SqlParameter("@TotalSumSy", item.TotalSumSy));

                                    await cmd.ExecuteNonQueryAsync();
                                }
                            }

                            #endregion


                            #region <<< SE OBTIENE LA GUÍA >>>
                            // Después de la creacíon en la base LOCAL se obtiene para realizar la migración

                            using (SqlCommand cmd = new SqlCommand("", conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandTimeout = 0;
                                cmd.Parameters.Add(new SqlParameter("@IdEntregaVenta", value.IdEntregaVenta));

                                using (var reader = await cmd.ExecuteReaderAsync())
                                {
                                    entregaVenta = context.Convert<EntregaVentaEntity>(reader);
                                }
                            }

                            using (SqlCommand cmd = new SqlCommand("", conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandTimeout = 0;
                                cmd.Parameters.Add(new SqlParameter("@IdEntregaVenta", value.IdEntregaVenta));

                                using (var reader = await cmd.ExecuteReaderAsync())
                                {
                                    entregaVenta.Item = (List<EntregaVentaItemEntity>)context.ConvertTo<EntregaVentaItemEntity>(reader);
                                }
                            }
                            #endregion


                            #region <<< ACTUALIZACIÓN DE NUMERO SUNAT >>>

                            using (SqlCommand cmd = new SqlCommand(SP_SET_SERIE_NUMERO_SUNAT_UPDATE, conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandTimeout = 0;

                                cmd.Parameters.Add(new SqlParameter("@Series", entregaVenta.Series));
                                cmd.Parameters.Add(new SqlParameter("@SerieSunat", entregaVenta.SerSunat));
                                cmd.Parameters.Add(new SqlParameter("@NumeroSunat", entregaVenta.NumSunat));

                                await cmd.ExecuteNonQueryAsync();
                            }

                            #endregion


                            #region <<< ACTUALIZACIÓN EL ESTADO DE PICKING >>>
                            // Actualización del estado de la cabecera y detalle, después de la creación de guía

                            using (SqlCommand cmd = new SqlCommand(SP_SET_PICKING_ITEM_ESTADO_UPDATE, conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandTimeout = 0;

                                foreach (var item in entregaVenta.Item)
                                {
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.Add(new SqlParameter("@IdPicking", item.IdBase));
                                    cmd.Parameters.Add(new SqlParameter("@LineNum", item.LineBase));

                                    await cmd.ExecuteNonQueryAsync();
                                }
                            }

                            #endregion


                            #region <<< MIGRACIÓN DE GUÍA A SAP >>>
                            // Migración de la guía, después de crear en la base de datos LOCAL

                            _connectionSap.ConnectToCompany(_cnxDiApiSap);

                            Documents document = RepositoryBaseSap.oCompany.GetBusinessObject(BoObjectTypes.oDeliveryNotes);

                            // ===========================================================================================
                            // CABECERA
                            // ===========================================================================================
                            document.Series = entregaVenta.Series;
                            document.UserFields.Fields.Item("U_BPP_MDTD").Value = entregaVenta.TipDocSunat;
                            document.UserFields.Fields.Item("U_BPP_MDSD").Value = entregaVenta.SerSunat;
                            document.UserFields.Fields.Item("U_BPP_MDCD").Value = entregaVenta.NumSunat;
                            document.DocDate = entregaVenta.DocDate;
                            document.DocDueDate = entregaVenta.DocDueDate;
                            document.TaxDate = entregaVenta.TaxDate;
                            document.DocType = BoDocumentTypes.dDocument_Items;
                            document.DocObjectCode = BoObjectTypes.oDeliveryNotes;
                            // ===========================================================================================
                            // CLIENTE
                            // ===========================================================================================
                            document.CardCode = entregaVenta.CardCode;
                            document.CardName = entregaVenta.CardName;
                            document.DocCurrency = entregaVenta.DocCur;
                            document.DocRate = entregaVenta.DocRate;
                            // ===========================================================================================
                            // LOGISTICA
                            // ===========================================================================================
                            document.ShipToCode = entregaVenta.ShipToCode;
                            document.Address2 = entregaVenta.Address2;
                            document.PayToCode = entregaVenta.PayToCode;
                            document.Address = entregaVenta.Address;
                            document.UserFields.Fields.Item("U_BPP_MDMT").Value = entregaVenta.CodMotTraslado;
                            document.UserFields.Fields.Item("U_BPP_MDOM").Value = entregaVenta.OtrMotTraslado;

                            // ===========================================================================================
                            // FINANZAS
                            // ===========================================================================================
                            document.GroupNumber = entregaVenta.CodCondicionPago;

                            // ===========================================================================================
                            // EXPORTACIÓN
                            // ===========================================================================================
                            document.UserFields.Fields.Item("U_RUCDestInter").Value = entregaVenta.RucDesInternacional;
                            document.UserFields.Fields.Item("U_DestGuiaInter").Value = entregaVenta.DesGuiaInternacional;
                            document.UserFields.Fields.Item("U_DireccDestInter").Value = entregaVenta.DirDesInternacional;
                            document.UserFields.Fields.Item("U_STR_NCONTENEDOR").Value = entregaVenta.NumContenedor;
                            document.UserFields.Fields.Item("U_STR_NPRESCINTO").Value = entregaVenta.NumPrecinto1;
                            document.UserFields.Fields.Item("U_FIB_NPRESCINTO2").Value = entregaVenta.NumPrecinto2;
                            document.UserFields.Fields.Item("U_FIB_NPRESCINTO3").Value = entregaVenta.NumPrecinto3;
                            document.UserFields.Fields.Item("U_FIB_NPRESCINTO4").Value = entregaVenta.NumPrecinto4;

                            // ===========================================================================================
                            // TRANSPORTISTA
                            // ===========================================================================================
                            document.UserFields.Fields.Item("U_FIB_TIP_TRANS").Value = entregaVenta.CodTipTransporte;
                            // Transportista 1
                            document.UserFields.Fields.Item("U_FIB_COD_TRA").Value = entregaVenta.CodTransportista1;
                            document.UserFields.Fields.Item("U_FIB_TIPDOC_TRA").Value = entregaVenta.CodTipDocIdeTransportista1;
                            document.UserFields.Fields.Item("U_FIB_RUC_TRANS2").Value = entregaVenta.RucTransportista1;
                            document.UserFields.Fields.Item("U_FIB_TRANS2").Value = entregaVenta.NomTransportista1;
                            document.UserFields.Fields.Item("U_BPP_MDVC").Value = entregaVenta.NumPlaca1;
                            document.UserFields.Fields.Item("U_FIB_TIPDOC_COND").Value = entregaVenta.CodTipDocIdeConductor1;
                            document.UserFields.Fields.Item("U_FIB_NUMDOC_COD").Value = entregaVenta.NumDocIdeConductor1;
                            document.UserFields.Fields.Item("U_BPP_MDFN").Value = entregaVenta.DenConductor1;
                            document.UserFields.Fields.Item("U_FIB_NOM_COND").Value = entregaVenta.NomConductor1;
                            document.UserFields.Fields.Item("U_FIB_APE_COND").Value = entregaVenta.ApeConductor1;
                            document.UserFields.Fields.Item("U_BPP_MDFC").Value = entregaVenta.LicConductor1;
                            // Transportista 2
                            document.UserFields.Fields.Item("U_BPP_MDCT").Value = entregaVenta.CodTransportista2;
                            document.UserFields.Fields.Item("U_BPP_MDRT").Value = entregaVenta.RucTransportista2;
                            document.UserFields.Fields.Item("U_BPP_MDNT").Value = entregaVenta.NomTransportista2;
                            document.UserFields.Fields.Item("U_BPP_MDDT").Value = entregaVenta.DirTransportista2;

                            // ===========================================================================================
                            // PIE
                            // ===========================================================================================
                            document.SalesPersonCode = entregaVenta.SlpCode;
                            document.UserFields.Fields.Item("U_FIB_NBULTOS").Value = entregaVenta.TotalBulto;
                            document.UserFields.Fields.Item("U_FIB_KG").Value = entregaVenta.TotalKilo;
                            document.Comments = entregaVenta.Comments;

                            // ===========================================================================================
                            // DETALLE
                            // ===========================================================================================
                            foreach (var item in entregaVenta.Item)
                            {
                                document.Lines.BaseType = item.BaseType;
                                document.Lines.BaseEntry = item.BaseEntry;
                                document.Lines.BaseLine = item.BaseLine;
                                document.Lines.ItemCode = item.ItemCode;
                                document.Lines.WarehouseCode = item.WhsCode;
                                document.Lines.Quantity = item.Quantity;
                                document.Lines.TaxCode = item.TaxCode;
                                document.Lines.AccountCode = item.AcctCode;
                                document.Lines.Currency = item.Currency;
                                document.Lines.DiscountPercent = item.DiscPrcnt;
                                document.Lines.Price = item.Price;
                                // ===========================================================================================
                                // Relación entre la guía de la base local y SAP
                                // ===========================================================================================
                                document.Lines.UserFields.Fields.Item("U_FIB_BASETYPE").Value = -1;
                                document.Lines.UserFields.Fields.Item("U_FIB_BASEENTRY").Value = item.IdEntregaVenta;
                                document.Lines.UserFields.Fields.Item("U_FIB_BASELINENUM").Value = item.LineNum;
                                // ===========================================================================================
                                // Relación entre la guía de la base local y SAP
                                // ===========================================================================================
                                document.Lines.Add();
                            }

                            var docEntry = document.Add();

                            if (docEntry != 0)
                            {
                                transaction.Rollback();
                                _connectionSap.DisConnectToCompany();
                                resultadoTransaccion.IdRegistro = -1;
                                resultadoTransaccion.ResultadoCodigo = -1;
                                resultadoTransaccion.ResultadoDescripcion = RepositoryBaseSap.oCompany.GetLastErrorDescription();
                                return resultadoTransaccion;
                            }

                            #endregion


                            #region <<< SE OBTIENE LA GUÍA DE SAP >>>
                            // Después de la creacíon en SAP se obtiene actualizar datos en la base LOCAL
                            #endregion


                            #region <<< ACTUALIZACIÓN DE GUÍA >>>
                            // Actualización de la cabecera y detalle, después de la migración a SAP

                            using (SqlCommand cmd = new SqlCommand(SP_SET_ENTREGA_VAL_SAPL_UPDATE, conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;

                                //cmd.Parameters.Add(new SqlParameter("@IdPicking", value.IdPicking));

                                await cmd.ExecuteNonQueryAsync();
                            }

                            using (SqlCommand cmd = new SqlCommand(SP_SET_ENTREGA_ITEM_VAL_SAP_UPDATE, conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;

                                //cmd.Parameters.Add(new SqlParameter("@IdPicking", value.IdPicking));

                                await cmd.ExecuteNonQueryAsync();
                            }
                            #endregion


                            resultadoTransaccion.IdRegistro = 0;
                            resultadoTransaccion.ResultadoCodigo = 0;
                            resultadoTransaccion.ResultadoDescripcion = "Registro procesado con éxito ..!";

                            transaction.Commit();
                            _connectionSap.DisConnectToCompany();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            _connectionSap.DisConnectToCompany();
                            resultadoTransaccion.IdRegistro = -1;
                            resultadoTransaccion.ResultadoCodigo = -1;
                            resultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _connectionSap.DisConnectToCompany();
                resultadoTransaccion.IdRegistro = -1;
                resultadoTransaccion.ResultadoCodigo = -1;
                resultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
            }

            return resultadoTransaccion;
        }
    }
}
