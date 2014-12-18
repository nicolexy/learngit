using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using CFT.CSOMS.DAL.CFTAccount;
using CFT.CSOMS.DAL.Infrastructure;
using System.Web.Script.Serialization;

namespace CFT.CSOMS.DAL.SPOA
{
    /// <summary>
    /// 商户相关查询类
    /// </summary>
    public class MerchantHandle
    {
        //合同查询接口返回结果json解析
        public static DataSet JsonTurnDSForContactQuery(string json, out string errMsg)
        {
            DataSet dsresult = null;
            errMsg = "";
            dsresult = new DataSet();
            DataTable dt = new DataTable();

            if (json == null || json == "")
            {
                errMsg = "解析json失败,返回结果有误" + json;
                return null;
            }
            try
            {
                JavaScriptSerializer js = new JavaScriptSerializer();   //实例化一个能够序列化数据的类
                Contact list = js.Deserialize<Contact>(json);    //将json数据转化为对象类型并赋值给list
                string result = list.Result;
                if (result == "OK")
                {
                    try
                    {
                        string dataCount = list.DataCount;
                        if (int.Parse(dataCount) > 0)
                        {
                            List<Data> data = list.data;
                            dt = CommQuery.ToDataTableTow(data);
                            dsresult.Tables.Add(dt);
                        }
                        else
                            dsresult = null;
                    }
                    catch
                    {
                        dsresult = null;
                    }
                }
                return dsresult;
            }
            catch
            {
                errMsg = "解析json失败,返回结果有误:" + json;
                return null;
            }

        }

        #region 合同查询接口返回结果json之构造对象
        public struct Contact
        {
            public string Result { get; set; }
            public List<Data> data;
            public string Msg { get; set; }
            public string DataCount { get; set; }
        }
        public struct Data
        {
            public string ORGID { get; set; }
            public string ORGFULLNAME { get; set; }
            public string VENDORID { get; set; }
            public string VENDORNAME { get; set; }
            public string CUSTOMERID { get; set; }
            public string CUSTOMERNAME { get; set; }
            public string CONTRACTID { get; set; }
            public string DEPTID { get; set; }
            public string DEPTNAME { get; set; }
            public string CATEGORYID { get; set; }
            public string CATEGORYNAME { get; set; }
            public string FULLPATHCATEGORYNAME { get; set; }
            public string ISIMPORTANT { get; set; }
            public string ISPURCHASE { get; set; }
            public string CONTRACTNO { get; set; }
            public string ISWARRANT { get; set; }
            public string ISAREA { get; set; }
            public string AREAID { get; set; }
            public string AREANAME { get; set; }
            public string STAFFID { get; set; }
            public string STAFFNAME { get; set; }
            public string WRITERID { get; set; }
            public string WRITERNAME { get; set; }
            public string ISDATEPROMISE { get; set; }
            public string ISSTANDARD { get; set; }
            public string STARTDATE { get; set; }
            public string ENDDATE { get; set; }
            public string CONTENT { get; set; }
            public string BALANCETERM { get; set; }
            public string SETTLEMODENAME { get; set; }
            public string SETTLEMODEID { get; set; }
            public string SETTLEMODERATIO { get; set; }
            public string TOTALPRICE { get; set; }
            public string CURRENCYNAME { get; set; }
            public string CURRENCYCODE { get; set; }
            public string CURRENCYRATIO { get; set; }
            public string TOTALAMOUNT { get; set; }
            public string STATE { get; set; }
            public string FOLDERNO { get; set; }
            public string ARCHIVEDAY { get; set; }
            public string CREATEDTIME { get; set; }
            public string ISSTRUCTURE { get; set; }
            public string BUID { get; set; }
            public string BUNAME { get; set; }
            public string ISCLOSEDPAYMENT { get; set; }

        };
        #endregion
    }

    #region
    public class ContractObject
    {
        public class BaseRequest
        {
            public string AppId { get; set; }
            public string BatchId { get; set; }

            public string Request;

        }

        public class RequestClass
        {
            public string StartRow { get; set; }
            public string RowCount { get; set; }
            public string OrderBy { get; set; }
            public string SortOption { get; set; }
            public ConditionClass condition;
        }

        public class ConditionClass
        {
            public string ContractNo { get; set; }
            public string VendorName { get; set; }
            public string CustomerName { get; set; }
            public string StartCreatedTime { get; set; }
            public string EndCreatedTime { get; set; }
            public string StartArchiveDay { get; set; }
            public string EndArchiveDay { get; set; }
            public string StartBeginDate { get; set; }
            public string EndBeginDate { get; set; }
            public string StartEndDate { get; set; }
            public string EndEndDate { get; set; }
            public string DeptID { get; set; }
        }



     
    }
     #endregion
}
