using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;
using TENCENT.OSS.CFT.KF.DataAccess;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.TokenCoin;
using SunLibrary;

namespace TENCENT.OSS.CFT.KF.KF_Web.TokenCoin
{
	/// <summary>
    /// GwqQuery 的摘要说明。
	/// </summary>
    public partial class GwqShow : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
            try
            {
                this.labUid.Text = Session["uid"].ToString();
                if (!TENCENT.OSS.CFT.KF.KF_Web.classLibrary.ClassLib.ValidateRight("InfoCenter", this)) Response.Redirect("../login.aspx?wh=1");
            }
            catch  //如果没有登陆或者没有权限就跳出
            {
                Response.Redirect("../login.aspx?wh=1");
            }

            if (!Page.IsPostBack)
            {
                string id = Request.QueryString["id"];
                string user = Request.QueryString["user"];
                if (id != null && id != "" && user != null && user != "")
                {
                    Query_Service.Query_Service qs = new Query_Service.Query_Service();
                    user = qs.Uid2QQ(user); //要把id转化为最新的QQID进行查询

                    if (user == null || user.Trim() == "")
                        WebUtils.ShowMessage(this.Page, "UID转换成帐号时失败，请检查是否存在此uid.");

                    TextBoxId.Text = id;
                    TextBoxUserId.Text = user;
                    Button1_Click(null, null);
                }
            }
		}

		#region Web 窗体设计器生成的代码
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
            this.Button1.Click += new System.EventHandler(this.Button1_Click);
            this.DataGrid1.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.DataGrid1_ItemDataBound);
		}
		#endregion

        public void ChangePage(object src, Wuqi.Webdiyer.PageChangedEventArgs e)
        {
            
        }

        private void BindInfo()
        {
            if (Session["uid"] == null)
            {
                Response.Redirect("../login.aspx?wh=1"); //重新登陆
            }

            Query_Service.Query_Service myService = new TENCENT.OSS.CFT.KF.KF_Web.Query_Service.Query_Service();
            myService.Finance_HeaderValue = classLibrary.setConfig.setFH(this);

            string id = this.TextBoxId.Text.Trim();
            string user = this.TextBoxUserId.Text.Trim();

            T_GWQ pub = myService.GetGwqInfo(user, id);
            if (pub == null)
            {
                throw new LogicException("没有找到记录，请确认你的输入！");
            }

            TextBoxTdeId.Text = pub.ftde_id;
            TextBoxSonId.Text = pub.fson_id;
            TextBoxAttName.Text = pub.fatt_name;
            TextBoxMerId.Text = pub.fmer_id;
            TextBoxPubId.Text = pub.fpub_id;
            TextBoxPubUId.Text = pub.fpub_uid;
            TextBoxPubName.Text = pub.fpub_name;
            TextBoxPubIp.Text = pub.fpub_ip;
            TextBoxType.Text = Sun.EnumGetName(typeof(CoinType), pub.ftype);
            TextBoxPubType.Text = Sun.EnumGetName(typeof(PubType), pub.fpub_type);

            TextBoxPubTime.Text = pub.fpub_time;
            TextBoxPubUser.Text = pub.fpub_user;
            TextBoxFee.Text = Define.FeeFormat(pub.ftype, pub.ffee, true);
            TextBoxFactFee.Text = Sun.MoneyFormat(pub.ffact_fee);
            TextBoxUsePro.Text = pub.fuse_pro;
            TextBoxMinFee.Text = Sun.MoneyFormat(pub.fmin_fee);
            TextBoxMaxNum.Text = pub.fmax_num;
            TextBoxDonateType.Text = Sun.EnumGetName(typeof(DonateType), pub.fdonate_type);
            TextBoxSTime.Text = pub.fstime;
            TextBoxETime.Text = pub.fetime;

            TextBoxUserUid.Text = pub.fuser_uid;
            TextBoxUseTime.Text = pub.fuse_time;
            TextBoxDonateId.Text = pub.fdonate_id;
            TextBoxDonateUid.Text = pub.fdonate_uid;
            TextBoxDonateNum.Text = pub.fdonate_num;
            TextBoxDonateTime.Text = pub.fdonate_time;
            TextBoxState.Text = Sun.EnumGetName(typeof(GwqState), pub.fstate);
            TextBoxModifyTime.Text = pub.fmodify_time;

            TextBoxUserIp.Text = pub.fuser_ip;
            TextBoxSpid.Text = pub.fspid;
            TextBoxListid.Text = pub.flistid;
            TextBoxUseListid.Text = pub.fuse_listid;
            TextBoxListState.Text = Sun.EnumGetName(typeof(ListState), pub.flist_state);
            TextBoxAdjustFlag.Text = Sun.EnumGetName(typeof(GwqAdjust), pub.fadjust_flag);

            TextBoxAcUin.Text = pub.fac_uin;
            TextBoxAcUid.Text = pub.fac_uid;
            TextBoxAcNum.Text = pub.fac_num;
            TextBoxAcFlag.Text = Sun.EnumGetName(typeof(AcFlag), pub.fac_flag);
            TextBoxAcSTime.Text = pub.fac_stime;
            TextBoxAcETime.Text = pub.fac_etime;

            TextBoxUrl.Text = pub.furl;
            TextBoxMemo.Text = pub.fmemo;

            DataSet ds = myService.GetGwqRoll(TextBoxUserId.Text.Trim(), pub.fticket_id);
            DataGrid1.DataSource = ds.Tables[0];
            DataGrid1.DataBind();
            ds.Dispose();
        }

        private void Button1_Click(object sender, System.EventArgs e)
        {
            //if (!base.CheckString(TextBoxUserId, "使用者帐号")) return;
            //if (!base.CheckString(TextBoxId, "财付券ID号")) return;

            try
            {
                string t_id = this.TextBoxId.Text.Trim();
                string user_id = this.TextBoxUserId.Text.Trim();
                if (string.IsNullOrEmpty(t_id) && string.IsNullOrEmpty(user_id)) 
                {
                    WebUtils.ShowMessage(this.Page, "请输入财付通账号或财付券ID！");
                    return;
                }

                //LeaveModifyState();
                BindInfo();
            }
            catch (SoapException eSoap)
            {
                //setInfoNull(); //如果失败，清空数据
                string str = PublicRes.GetErrorMsg(eSoap.Message.ToString());
                WebUtils.ShowMessage(this.Page, str);
            }
            catch (Exception emsg)
            {
                WebUtils.ShowMessage(this.Page, emsg.Message);
            }
        }
        private void DataGrid1_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemIndex >= 0)
            {
                e.Item.Cells[3].Text = Sun.EnumGetName(typeof(RollRequestType), e.Item.Cells[3].Text);
                e.Item.Cells[5].Text = Sun.MoneyFormat(e.Item.Cells[5].Text);
                e.Item.Cells[6].Text = Sun.EnumGetName(typeof(RollListState), e.Item.Cells[6].Text);
                e.Item.Cells[7].Text = Sun.EnumGetName(typeof(GwqState), e.Item.Cells[7].Text);
                e.Item.Cells[8].Text = Sun.EnumGetName(typeof(GwqState), e.Item.Cells[8].Text);
            }
        }
	}
}
