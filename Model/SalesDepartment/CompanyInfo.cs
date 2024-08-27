using Goodjob.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.SalesDepartment
{/// <summary>
 /// This model represents the properties and methods of a Mem_Info.
 /// </summary>
    public class CompanyInfo
    {
        #region Member Variables
        private int memID;
        private string memName;
        private string licenceNumber;
        private short calling;
        private short properity;
        private DateTime foundDate;
        private short registerFund;
        private short employeeNumber;
        private string companyIntroduction;
        private string contactPerson;
        private string contactDepartment;
        private string contacttelZ;
        private string contactTel;
        private string contacttelE;
        private string oldContactTel;
        private bool telShowFlag;
        private string contactfaxZ;
        private string contactFax;
        private string contactfaxE;
        private bool faxShowFlag;
        private string email;
        private bool emailShowFlag;
        private int addressP;
        private int addressC;
        private string address;
        private string zipCode;
        private string homePage;
        private short mailCode;
        private string logoFileName;
        private DateTime logoUpdatedate;
        private short logoShowFlag;
        private bool iscommend;
        private bool hasPage;
        private DateTime registerDate;
        private DateTime updateDate;



        #endregion

        #region Constructors
        public CompanyInfo() { }

        public CompanyInfo(int memID, string memName, string licenceNumber, short calling, short properity, DateTime foundDate, short registerFund, short employeeNumber, string companyIntroduction, string contactPerson, string contactDepartment, string contacttel_Z, string contactTel, string contacttel_E, string oldContactTel, bool telShowFlag, string contactfaxZ, string contactFax, string contactfaxE, bool faxShowFlag, string email, bool emailShowFlag, int address_P, int address_C, string address, string zipCode, string homePage, short mailCode, string logoFileName, DateTime logoUpdatedate, short logoShowFlag, bool iscommend, bool hasPage, DateTime registerDate, DateTime updateDate, string qq, bool qqFlag, string phone, bool phoneFlag)
        {
            this.memID = memID;
            this.memName = memName;
            this.licenceNumber = licenceNumber;
            this.calling = calling;
            this.properity = properity;
            this.foundDate = foundDate;
            this.registerFund = registerFund;
            this.employeeNumber = employeeNumber;
            this.companyIntroduction = companyIntroduction;
            this.contactPerson = contactPerson;
            this.contactDepartment = contactDepartment;
            this.contacttelZ = contacttel_Z;
            this.contactTel = contactTel;
            this.contacttelE = contacttelE;
            this.oldContactTel = oldContactTel;
            this.telShowFlag = telShowFlag;
            this.contactfaxZ = contactfaxZ;
            this.contactFax = contactFax;
            this.contactfaxE = contactfaxE;
            this.faxShowFlag = faxShowFlag;
            this.email = email;
            this.emailShowFlag = emailShowFlag;
            this.addressP = address_P;
            this.addressC = address_C;
            this.address = address;
            this.zipCode = zipCode;
            this.homePage = homePage;
            this.mailCode = mailCode;
            this.logoFileName = logoFileName;
            this.logoUpdatedate = logoUpdatedate;
            this.logoShowFlag = logoShowFlag;
            this.iscommend = iscommend;
            this.hasPage = hasPage;
            this.registerDate = registerDate;
            this.updateDate = updateDate;
            this.QQ = qq;
            this.QQFlag = qqFlag;
            this.Phone = phone;
            this.PhoneFlag = phoneFlag;
        }


        public CompanyInfo(int memID, string memName, string licenceNumber, short calling, short properity, DateTime foundDate, short registerFund, short employeeNumber, string companyIntroduction, string contactPerson, string contactDepartment, string contacttel_Z, string contactTel, string contacttel_E, string oldContactTel, bool telShowFlag, string contactfaxZ, string contactFax, string contactfaxE, bool faxShowFlag, string email, bool emailShowFlag, int address_P, int address_C, string address, string zipCode, string homePage, short mailCode, string logoFileName, DateTime logoUpdatedate, short logoShowFlag, bool iscommend, bool hasPage, DateTime registerDate, DateTime updateDate, string qq, bool qqFlag, string phone, bool phoneFlag, int address_T)
        {
            this.memID = memID;
            this.memName = memName;
            this.licenceNumber = licenceNumber;
            this.calling = calling;
            this.properity = properity;
            this.foundDate = foundDate;
            this.registerFund = registerFund;
            this.employeeNumber = employeeNumber;
            this.companyIntroduction = companyIntroduction;
            this.contactPerson = contactPerson;
            this.contactDepartment = contactDepartment;
            this.contacttelZ = contacttel_Z;
            this.contactTel = contactTel;
            this.contacttelE = contacttel_E;
            this.oldContactTel = oldContactTel;
            this.telShowFlag = telShowFlag;
            this.contactfaxZ = contactfaxZ;
            this.contactFax = contactFax;
            this.contactfaxE = contactfaxE;
            this.faxShowFlag = faxShowFlag;
            this.email = email;
            this.emailShowFlag = emailShowFlag;
            this.addressP = address_P;
            this.addressC = address_C;
            this.address = address;
            this.zipCode = zipCode;
            this.homePage = homePage;
            this.mailCode = mailCode;
            this.logoFileName = logoFileName;
            this.logoUpdatedate = logoUpdatedate;
            this.logoShowFlag = logoShowFlag;
            this.iscommend = iscommend;
            this.hasPage = hasPage;
            this.registerDate = registerDate;
            this.updateDate = updateDate;
            this.QQ = qq;
            this.QQFlag = qqFlag;
            this.Phone = phone;
            this.PhoneFlag = phoneFlag;
            this.AddressT = address_T;
        }
        #endregion

        #region Public Properties

        public int MemID
        {
            get { return memID; }
            set { memID = value; }
        }

        public string MemName
        {
            get { return memName; }
            set { memName = value; }
        }

        public string LicenceNumber
        {
            get { return licenceNumber; }
            set { licenceNumber = value; }
        }

        public short Calling
        {
            get { return calling; }
            set { calling = value; }
        }

        public string CallingName
        {
            get
            {
                if (calling < 57)
                {
                    return NameProvider.GetIndustryName(calling);
                }
                else
                {
                    return "";
                }
            }
        }

        //public short Properity
        //{
        //    get { return properity; }
        //    set { properity = value; }
        //}

        public string PropertyName
        {
            get { return NameProvider.GetPropertyName(properity); }
        }

        public DateTime FoundDate
        {
            get { return foundDate; }
            set { foundDate = value; }
        }

        //public short RegisterFund
        //{
        //    get { return registerFund; }
        //    set { registerFund = value; }
        //}

        public string RegisterFundName
        {
            get { return NameProvider.GetRegisterFundName(registerFund); }
        }

        public short EmployeeNumber
        {
            get { return employeeNumber; }
            set { employeeNumber = value; }
        }

        public string EmployeeNumberName
        {
            get { return NameProvider.GetEmployeeSizeName(employeeNumber); }
        }

        public string CompanyIntroduction
        {
            get { return companyIntroduction; }
            set { companyIntroduction = value; }
        }

        public string ContactPerson
        {
            get { return contactPerson; }
            set { contactPerson = value; }
        }

        public string ContactDepartment
        {
            get { return contactDepartment; }
            set { contactDepartment = value; }
        }

        public string ContacttelZ
        {
            get { return contacttelZ; }
            set { contacttelZ = value; }
        }

        public string ContactTel
        {
            get { return contactTel; }
            set { contactTel = value; }
        }

        public string ContacttelE
        {
            get { return contacttelE; }
            set { contacttelE = value; }
        }

        public string OldContactTel
        {
            get { return oldContactTel; }
            set { oldContactTel = value; }
        }

        //public bool TelShowFlag
        //{
        //    get { return telShowFlag; }
        //    set { telShowFlag = value; }
        //}

        public string ContactfaxZ
        {
            get { return contactfaxZ; }
            set { contactfaxZ = value; }
        }

        public string ContactFax
        {
            get { return contactFax; }
            set { contactFax = value; }
        }

        public string ContactfaxE
        {
            get { return contactfaxE; }
            set { contactfaxE = value; }
        }

        //public bool FaxShowFlag
        //{
        //    get { return faxShowFlag; }
        //    set { faxShowFlag = value; }
        //}

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        //public bool EmailShowFlag
        //{
        //    get { return emailShowFlag; }
        //    set { emailShowFlag = value; }
        //}

        public int AddressP
        {
            get { return addressP; }
            set { addressP = value; }
        }

        public int AddressC
        {
            get { return addressC; }
            set { addressC = value; }
        }

        public int AddressT { get; set; }


        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        public string AddressName
        {
            get { return NameProvider.GetProvinceName(addressP) + " " + NameProvider.GetCityName(addressC); }
        }

        public string ZipCode
        {
            get { return zipCode; }
            set { zipCode = value; }
        }

        public string HomePage
        {
            get { return homePage; }
            set { homePage = value; }
        }

        //public short MailCode
        //{
        //    get { return mailCode; }
        //    set { mailCode = value; }
        //}

        public string MailCodeName
        {
            get { return NameOtherProvider.GetReceiveMailCodeName(mailCode); }
        }

        //public string LogoFileName
        //{
        //    get { return logoFileName; }
        //    set { logoFileName = value; }
        //}

        //public DateTime LogoUpdatedate
        //{
        //    get { return logoUpdatedate; }
        //    set { logoUpdatedate = value; }
        //}

        //public short LogoShowFlag
        //{
        //    get { return logoShowFlag; }
        //    set { logoShowFlag = value; }
        //}

        public bool Iscommend
        {
            get { return iscommend; }
            set { iscommend = value; }
        }

        public bool HasPage
        {
            get { return hasPage; }
            set { hasPage = value; }
        }

        public string RegisterDate
        {
            get { return registerDate.ToString("g"); }
        }

        public string UpdateDate
        {
            get { return updateDate.ToString("g"); }
        }

        //mingxing add
        //zhouzhou amend
        public string ContactTelStr
        {
            get { return (string.IsNullOrEmpty(contacttelZ) ? string.Empty : contacttelZ + "-") + contactTel + (string.IsNullOrEmpty(contacttelE) ? string.Empty : "-" + contacttelE) + (telShowFlag ? "(公开)" : "(不公开)"); }
        }

        public string ContactFaxStr
        {
            get { return (string.IsNullOrEmpty(contactfaxZ) ? string.Empty : contactfaxZ + "-") + contactFax + (string.IsNullOrEmpty(contactfaxE) ? string.Empty : "-" + contactfaxE) + (faxShowFlag ? "(公开)" : "(不公开)"); }
        }

        public string ContactEmailStr
        {
            get { return email + (emailShowFlag ? "(公开)" : "(不公开)"); }
        }

        public string CompanyLogo
        {
            get { return (logoShowFlag == 0) ? "没有" : logoFileName + (logoShowFlag == 1 ? "（公开）" : "（不公开）"); }
        }


        public string QQ { set; get; }
        public bool QQFlag { set; get; }
        public string Phone { set; get; }
        public bool PhoneFlag { set; get; }


        #endregion
    }
}
