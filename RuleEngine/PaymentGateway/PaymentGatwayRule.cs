
using BusinessDomain.PaymentGateway;
using BusinessModel.Common;
using BusinessModel.PaymentGateway;
using BusinessModel.Payout;
using Newtonsoft.Json;
using Services_API.Banking;
using Services_API.PaymentGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RuleEngine.PaymentGateway
{
    public class PaymentGatwayRule : IPaymentGatwayRule
    {

        private readonly IPaymentGatewayService _gatewayservice;
        private readonly IPaymentGatewayBussiness _paymentgatewaybusiness;
        public PaymentGatwayRule(IPaymentGatewayService gatewayconfiguration, IPaymentGatewayBussiness paymentgateconfiuration) => (_gatewayservice, _paymentgatewaybusiness) = (gatewayconfiguration, paymentgateconfiuration);

        public StatusResponse InitiatePayment(PrimaryCheckModel primaryCheck, InitiateRequest initiateRequest)
        {

            if (primaryCheck.statuscode == ResponseCode.Success)
            {
                if (primaryCheck.PreferenceAction == CommonConstants.Five)
                {
                    LinkResponse GatewayResponse = _gatewayservice.EasebuzzPaymentInitiate(initiateRequest);

                    if (GatewayResponse != null)
                    {

                        EaseBuzzCodeResponse objRes = JsonConvert.DeserializeObject<EaseBuzzCodeResponse>((string)GatewayResponse.data);

                        if (objRes.status == "1")
                        {
                            string resultLink = EaseBuzz.BaseURL + "/pay/" + objRes.data;

                            LinkResponseFinal linkResponseFinal = new LinkResponseFinal();
                            linkResponseFinal.Link = resultLink;
                            linkResponseFinal.TxnID = GatewayResponse.TxnID;
                            return new StatusResponse { StatusCode = ResponseCode.Success, StatusDesc = ResponseMessage.Success, Data = linkResponseFinal };
                        }
                        else
                            return new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = ResponseMessage.Failed, Data = GatewayResponse };
                    }

                }

                if (primaryCheck.PreferenceAction == CommonConstants.Six) // BANK 2
                {
                    //Write Action
                    return new StatusResponse { StatusCode = "", StatusDesc = "" };
                }
            }
            else
            {
                return new StatusResponse { StatusCode = primaryCheck.statuscode, StatusDesc = primaryCheck.statusdesc };
            }

            return new StatusResponse();
        }

        public StatusResponse InitiatePaymentwol(PrimaryCheckModel primaryCheck, InitiateRequest initiateRequest)
        {

            if (primaryCheck.statuscode == ResponseCode.Success)
            {
                if (primaryCheck.PreferenceAction == CommonConstants.Five)
                {
                    LinkResponse GatewayResponse = _gatewayservice.EasebuzzPaymentInitiate(initiateRequest);

                    if (GatewayResponse != null)
                    {

                        EaseBuzzCodeResponse objRes = JsonConvert.DeserializeObject<EaseBuzzCodeResponse>((string)GatewayResponse.data);

                        if (objRes.status == "1")
                        {
                            string resultLink = EaseBuzz.BaseURL + "/pay/" + objRes.data;

                            return new StatusResponse { StatusCode = ResponseCode.Success, StatusDesc = ResponseMessage.Success, Data = resultLink };
                        }
                        else
                            return new StatusResponse { StatusCode = ResponseCode.Failed, StatusDesc = ResponseMessage.Failed, Data = GatewayResponse };
                    }

                }

                if (primaryCheck.PreferenceAction == CommonConstants.Six) // BANK 2
                {
                    //Write Action
                    return new StatusResponse { StatusCode = "", StatusDesc = "" };
                }
            }
            else
            {
                return new StatusResponse { StatusCode = primaryCheck.statuscode, StatusDesc = primaryCheck.statusdesc };
            }

            return new StatusResponse();
        }

        public StatusResponse Checkstatus(PrimaryCheckModel primaryCheck, GetStatusRequest getStatusRequest)
        {
            ConverteStatusResponse converteStatusResponse = new ConverteStatusResponse();

            if (primaryCheck.statuscode == ResponseCode.Success)
            {
                if (primaryCheck.PreferenceAction == CommonConstants.Six)
                {
                    string payoutGatewayResponse = _gatewayservice.EasebuzzCheckststatus(getStatusRequest);

                    if (payoutGatewayResponse != null)
                    {

                        EasebuzzStatusResponse root = JsonConvert.DeserializeObject<EasebuzzStatusResponse>(payoutGatewayResponse);

                        if(root != null)
                        {
                            if (root.status=true)
                            {
                                UpdateStatusResponse values=new UpdateStatusResponse();
                                values.txnID = getStatusRequest.txnID;
                                values.CustomerId = getStatusRequest.CustomerId; 
                                values.Status = root.msg.status;
                                values.Response =JsonConvert.SerializeObject(root.msg);
                                values.Request = "";

                                _paymentgatewaybusiness.TxnUpdate(values);

                                converteStatusResponse.txnid = root.msg.txnid;
                                converteStatusResponse.firstname = root.msg.firstname;
                                converteStatusResponse.email = root.msg.email;
                                converteStatusResponse.phone = root.msg.phone;
                                converteStatusResponse.mode = root.msg.mode;
                                converteStatusResponse.unmappedstatus = root.msg.unmappedstatus;
                                converteStatusResponse.cardCategory = root.msg.cardCategory;
                                converteStatusResponse.addedon = root.msg.addedon;
                                converteStatusResponse.payment_source = root.msg.payment_source;
                                converteStatusResponse.PG_TYPE = root.msg.PG_TYPE;
                                converteStatusResponse.bank_ref_num = root.msg.bank_ref_num;
                                converteStatusResponse.bankcode = root.msg.bankcode;
                                converteStatusResponse.name_on_card = root.msg.name_on_card;
                                converteStatusResponse.upi_va = root.msg.upi_va;
                                converteStatusResponse.cardnum = root.msg.cardnum;
                                converteStatusResponse.issuing_bank = root.msg.issuing_bank;
                                converteStatusResponse.easepayid = root.msg.easepayid;
                                converteStatusResponse.amount = root.msg.amount;
                                converteStatusResponse.productinfo = root.msg.productinfo;
                                converteStatusResponse.card_type = root.msg.card_type;
                                converteStatusResponse.status = root.msg.status;
                                converteStatusResponse.bank_name = root.msg.bank_name;

                            }

                        }
                        return new StatusResponse { StatusCode = ResponseCode.Success, StatusDesc = ResponseMessage.Success, Data = converteStatusResponse };


                    }

                    if (primaryCheck.PreferenceAction == CommonConstants.Seven) // BANK 2
                    {
                        //Write Action
                        return new StatusResponse { StatusCode = "", StatusDesc = "" };
                    }
                }
                else
                {
                    return new StatusResponse { StatusCode = primaryCheck.statuscode, StatusDesc = primaryCheck.statusdesc };
                }

                return new StatusResponse();
            }

            return new StatusResponse { StatusCode = "", StatusDesc = "" };
        }


    }
}
