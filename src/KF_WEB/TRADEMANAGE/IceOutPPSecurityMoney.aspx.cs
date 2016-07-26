using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using CFT.CSOMS.BLL.TradeModule;

namespace TENCENT.OSS.CFT.KF.KF_Web.TradeManage
{
    public partial class IceOutPPSecurityMoney : TENCENT.OSS.CFT.KF.KF_Web.PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Label_uid.Text = Session["uid"].ToString();
            if (!IsPostBack)
            {
                if (Request.Params["action"] != null && Request.Params["action"].ToString() != "")
                {
                    string action = Request.Params["action"].ToString();
                    switch (action)
                    {
                        case "IceOut":
                            {
                                string uin = Request.Params["uin"];
                                string transactionId = Request.Params["transaction_id"];
                                Response.ContentType = "application/json";
                                string ret = IceOutPPSecurityMoneyByUin(uin, transactionId);
                                Response.Write(ret);
                                Response.End();
                            }
                            break;
                    }
                }
            }
        }

        private string IceOutPPSecurityMoneyByUin(string uin,string transactionId)
        {
            StringBuilder sb = new StringBuilder();
            string msg = "";
            bool result = new TradeService().IsIceOutPPSecurtyMoney(uin, transactionId, out msg);
            sb.Append("{\"ret\":\"" + (result ? "解冻成功" : "解冻失败:【"+msg+"】") + "\"}");
            return sb.ToString();
        }

    }
}