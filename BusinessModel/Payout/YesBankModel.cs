using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Payout
{ 
    public class YBPTRequest
    {
        public YBPTRequest()
        {
            Data = new Data();
            Risk = new Risk();
        }
        public Data Data { get; set; }
        public Risk Risk { get; set; }

    }
    public class Risk
    {
        public Risk()
        {
            DeliveryAddress = new DeliveryAddress();
        }
        public DeliveryAddress DeliveryAddress { get; set; }
    }
    public class DeliveryAddress
    {

        public DeliveryAddress()
        {
            AddressLine = new List<string>();
            CountySubDivision = new List<string>();
        }

        public List<string> AddressLine { get; set; }
        public string StreetName { get; set; }
        public string BuildingNumber { get; set; }
        public string PostCode { get; set; }
        public string TownName { get; set; }
        public List<string> CountySubDivision { get; set; }
        public string Country { get; set; }
    }
    public class Data
    {
        public string ConsentId { get; set; }
        public Data()
        {
            Initiation = new Initiation();
        }
        public Initiation Initiation { get; set; }

    }
    public class Initiation
    {
        public string InstructionIdentification { get; set; }
        public string EndToEndIdentification { get; set; }
        public Initiation()
        {
            InstructedAmount = new InsAmt();
            DebtorAccount = new DebAcc();
            CreditorAccount = new CreAcc();
            RemittanceInformation = new RemInfo();

        }
        public InsAmt InstructedAmount { get; set; }
        public DebAcc DebtorAccount { get; set; }
        public CreAcc CreditorAccount { get; set; }
        public RemInfo RemittanceInformation { get; set; }

        public string ClearingSystemIdentification { get; set; }
    }
    public class InsAmt
    {
        public string Amount { get; set; }
        public string Currency { get; set; }
    }
    public class DebAcc
    {
        public string Identification { get; set; }
        public string SecondaryIdentification { get; set; }

    }
    public class CreAcc
    {
        public string SchemeName { get; set; }
        public string Identification { get; set; }
        public string Name { get; set; }
        public CreAcc()
        {
            Unstructured = new unstruct();
        }
        public unstruct Unstructured { get; set; }
    }
    public class unstruct
    {
        public unstruct()
        {
            ContactInformation = new ContactInformation();
        }
        public ContactInformation ContactInformation { get; set; }
    }
    public class ContactInformation
    {
        public string EmailAddress { get; set; }
        public string MobileNumber { get; set; }
    }
    public class RemInfo
    {
        public string Reference { get; set; }
        public RemInfo()
        {
            Unstructured = new Unstructured();
        }
        public Unstructured Unstructured { get; set; }

    }
    public class Unstructured
    {
        public string CreditorReferenceInformation { get; set; }
    }
    public class YesBankPayRequest
    {
        [Required]
        public String TransactionID { get; set; }
        [Required]
        public string TxnAmount { get; set; }
        [Required]
        public string BeneIfscCode { get; set; }
        [Required]
        public string BeneAccNum { get; set; }
        [Required]
        public string TxnPaymode { get; set; }
        [Required]
        public string BeneBankName { get; set; }
        [Required]
        public string BeneName { get; set; }
        [Required]
        public string EmailAddress { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Address1 { get; set; }
        [Required]
        public string Address2 { get; set; }
        [Required]
        public string Pincode { get; set; }
        [Required]
        public string State { get; set; }


    }
    public class CheckStatus
    {
        public datas Data { get; set; }

    }
    public class datas
    {
        public string InstrId { get; set; }
        public string ConsentId { get; set; }
        public string SecondaryIdentification { get; set; }
    }
    public class CheckStatusRequest
    {
        public string referenceID { get; set; }


    }

    public class CreditorAccount
    {
        public string SchemeName { get; set; }
        public string Identification { get; set; }
        public string Name { get; set; }
        public object BeneficiaryCode { get; set; }
        public Unstructured Unstructured { get; set; }
        public RemittanceInformation RemittanceInformation { get; set; }
        public string ClearingSystemIdentification { get; set; }
    }

    public class DataDUP
    {
        public string ConsentId { get; set; }
        public string TransactionIdentification { get; set; }
        public string Status { get; set; }
        public DateTime CreationDateTime { get; set; }
        public DateTime StatusUpdateDateTime { get; set; }
        public InitiationDUP Initiation { get; set; }
    }

    public class DebtorAccount
    {
        public string Identification { get; set; }
        public string SecondaryIdentification { get; set; }
    }

    public class DeliveryAddressDUP
    {
        public List<string> AddressLine { get; set; }
        public string StreetName { get; set; }
        public string BuildingNumber { get; set; }
        public string PostCode { get; set; }
        public string TownName { get; set; }
        public List<string> CountySubDivision { get; set; }
        public string Country { get; set; }
    }

    public class InitiationDUP
    {
        public string InstructionIdentification { get; set; }
        public string EndToEndIdentification { get; set; }
        public InstructedAmount InstructedAmount { get; set; }
        public DebtorAccount DebtorAccount { get; set; }
        public CreditorAccount CreditorAccount { get; set; }
        public RemittanceInformation RemittanceInformation { get; set; }
        public string ClearingSystemIdentification { get; set; }
    }

    public class InstructedAmount
    {
        public string Amount { get; set; }
        public string Currency { get; set; }
    }

    public class Links
    {
        public string Self { get; set; }
    }

    public class RemittanceInformation
    {
        public string Reference { get; set; }
        public UnstructuredDUP Unstructured { get; set; }
    }

    public class RiskDUP
    {
        public object PaymentContextCode { get; set; }
        public DeliveryAddressDUP DeliveryAddress { get; set; }
    }

    public class YesBankResponse
    {
        public DataDUP Data { get; set; }
        public RiskDUP Risk { get; set; }
        public Links Links { get; set; }
    }

    public class UnstructuredDUP
    {
        public ContactInformation ContactInformation { get; set; }
        public string CreditorReferenceInformation { get; set; }
    }
    public class CreditorAccountDUP
    {
        public string SchemeName { get; set; }
        public string Identification { get; set; }
        public string Name { get; set; }
        public object BeneficiaryCode { get; set; }
        public Unstructured Unstructured { get; set; }
        public RemittanceInformation RemittanceInformation { get; set; }
        public string ClearingSystemIdentification { get; set; }
    }
    public class DeliveryAddressDup
    {
        public string AddressLine { get; set; }
        public string StreetName { get; set; }
        public string BuildingNumber { get; set; }
        public string PostCode { get; set; }
        public string TownName { get; set; }
        public string CountySubDivision { get; set; }
        public string Country { get; set; }
    }

    public class InitiationDup
    {
        public string InstructionIdentification { get; set; }
        public string EndToEndIdentification { get; set; }
        public InstructedAmount InstructedAmount { get; set; }
        public DebtorAccount DebtorAccount { get; set; }
        public CreditorAccountDUP CreditorAccount { get; set; }
    }


    
   

}
