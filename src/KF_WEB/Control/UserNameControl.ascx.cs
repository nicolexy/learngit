using CFT.CSOMS.BLL.CFTAccountModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TENCENT.OSS.CFT.KF.KF_Web.Control
{
    public partial class UserControl : System.Web.UI.UserControl
    {
        public string QQID
        {
            get
            {
                return GetQQID();
            }
        }
        public string Fuid
        {
            get
            {
                return GetFuid();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        private string GetQQID()
        {
            string inputQQ = this.TextBox1_InputQQ.Text.Trim();
            if (inputQQ == "") 
            {
                throw new Exception("请输入账号!");
            }
            string queryType = GetUserType();
            return AccountService.GetQQID(queryType, inputQQ);
        }
        private string GetFuid()
        {
            string inputQQ = this.TextBox1_InputQQ.Text.Trim();
            if (inputQQ == "")
            {
                throw new Exception("请输入账号!");
            }
            string queryType = GetUserType();
            if (queryType == "WeChatUid")
            {
                return inputQQ;
            }
            string QQID = AccountService.GetQQID(queryType, inputQQ);
            return new AccountService().QQ2Uid(QQID);
        }

        private string GetUserType()
        {
            string usertype = "";
            foreach (System.Web.UI.Control c in this.Controls)
            {
                if (c is RadioButton)
                {
                    RadioButton rdb = c as RadioButton;
                    usertype = rdb.ID;
                    break;
                }
            }
            return usertype;
        }
    }
}