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
using TENCENT.OSS.CFT.KF.DataAccess;
using System.Web.Services.Protocols;
using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.CFT.KF.KF_Web.classLibrary;
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;
using System.IO;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
    /// <summary>
    /// UserClass 的摘要说明。
    /// </summary>
    public partial class UserClass : TENCENT.OSS.CFT.KF.KF_Web.PageBase
    {
        private string type
        {
            get
            {
                if (ViewState["type"] != null)
                    return ViewState["type"].ToString();
                else
                    return "old";
            }
            set
            {
                ViewState["type"] = value.Trim();
            }
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                Label1.Text = Session["uid"].ToString();
                string szkey = Session["SzKey"].ToString();
                //int operid = Int32.Parse(Session["OperID"].ToString());
                // if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"CFTUserPick")) Response.Redirect("../login.aspx?wh=1");
                if (!classLibrary.ClassLib.ValidateRight("CFTUserPick", this)) Response.Redirect("../login.aspx?wh=1");
            }
            catch
            {
                Response.Redirect("../login.aspx?wh=1");
            }

            if (!IsPostBack)
            {
                tbSumDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                Image1.Attributes.Add("onclick", "ShowImg(test,imgid,this,movediv)");
                Image1.Attributes.Add("onmouseover", "ShowMoveImg(movediv,moveimgid,this,test)");
                Image1.Attributes.Add("onmouseout", "HiddenImg(movediv)");
                Image2.Attributes.Add("onclick", "ShowImg(test,imgid,this,movediv)");
                Image2.Attributes.Add("onmouseover", "ShowMoveImg(movediv,moveimgid,this,test)");
                Image2.Attributes.Add("onmouseout", "HiddenImg(movediv)");

                Image3.Attributes.Add("onclick", "ShowImg(test,imgid,this,movediv)");
                Image3.Attributes.Add("onmouseover", "ShowMoveImg(movediv,moveimgid,this,test)");
                Image3.Attributes.Add("onmouseout", "HiddenImg(movediv)");
                Image4.Attributes.Add("onclick", "ShowImg(test,imgid,this,movediv)");
                Image4.Attributes.Add("onmouseover", "ShowMoveImg(movediv,moveimgid,this,test)");
                Image4.Attributes.Add("onmouseout", "HiddenImg(movediv)");

                if (Request.QueryString["type"] != null && Request.QueryString["type"].Trim() == "new")
                {
                    //邻取新单。
                    type = "new";
                    PickData(1);
                }

                if (type == "new")
                {
                    btnPick.Visible = false;
                    hlNewORder.Target = "_self";
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

        }

        #endregion

        protected void btnPick_Click(object sender, System.EventArgs e)
        {
            //领单,读取四张可用单子.
            PickData(0);
        }

        private void InitPage()
        {
            labcardtype4.Text = "";
            labid4.Text = "";
            Image4.ImageUrl = "#";
            tbidcard4.Text = "";
            rblist4.SelectedIndex = 0;
            labcardtype3.Text = "";
            labid3.Text = "";
            Image3.ImageUrl = "#";
            tbidcard3.Text = "";
            rblist3.SelectedIndex = 0;

            labcardtype2.Text = "";
            labid2.Text = "";
            Image2.ImageUrl = "#";
            tbidcard2.Text = "";
            rblist2.SelectedIndex = 0;
            labcardtype1.Text = "";
            labid1.Text = "";
            Image1.ImageUrl = "#";
            tbidcard1.Text = "";
            rblist1.SelectedIndex = 0;
        }

        private void PickData(int flag)
        {

        }

        protected void btnCommit_Click(object sender, System.EventArgs e)
        {

        }

        protected void btnSum_Click(object sender, System.EventArgs e)
        {

        }
    }

}

