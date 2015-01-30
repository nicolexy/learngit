using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using CFT.CSOMS.DAL.WechatPay;

namespace CFT.CSOMS.BLL.WechatPay
{
    public class WechatMerchantService
    {
        public string WechatMchIdToSpId(string mchId) 
        {
            try
            {
                return new WechatMerchantData().WechatMchIdToSpId(mchId);
            }
            catch 
            {
                return "";
            }
        }

        public string WechatSpIdToMchId(string spId) 
        {
            string ret = "";
            try
            {
                DataTable dt = new WechatMerchantData().WechatSpIdToMchId(spId);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        ret += dr["mchid"].ToString();
                        ret += ",";
                    }
                    ret = ret.Substring(0, ret.Length - 1);
                }
            }
            catch 
            {
                return "";
            }
            return ret;
        }

        public DataTable QueryWechatSpList() 
        {
            return new WechatMerchantData().QueryWechatSpList();
        }

        public DataTable QueryWechatSpDetail() 
        {
            return new WechatMerchantData().QueryWechatSpDetail();
        }

        public DataTable QueryWechatSpCheck()
        {
            return new WechatMerchantData().QueryWechatSpCheck();
        }

    }
}
