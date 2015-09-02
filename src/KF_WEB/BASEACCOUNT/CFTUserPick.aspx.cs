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
    /// CFTUserPick ��ժҪ˵����
    /// </summary>
    public partial class CFTUserPick : System.Web.UI.Page
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

                //if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"CFTUserPick")) Response.Redirect("../login.aspx?wh=1");
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
                    //��ȡ�µ���
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

        #region Web ������������ɵĴ���

        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: �õ����� ASP.NET Web ���������������ġ�
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
        /// �˷��������ݡ�
        /// </summary>
        private void InitializeComponent()
        {

        }

        #endregion

        protected void btnPick_Click(object sender, System.EventArgs e)
        {
            //�쵥,��ȡ���ſ��õ���.
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

        private void WriteImgFile(string imagestr, string filename)
        {

            byte[] imagebyte = Convert.FromBase64String(imagestr);
            string filepath = Server.MapPath(Request.ApplicationPath) + "\\BaseAccount\\" + filename;
            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }

            FileStream fs = new FileStream(filepath, FileMode.Create);

            try
            {
                fs.Write(imagebyte, 0, imagebyte.Length);
            }

            finally
            {
                fs.Close();
            }
        }

        private string GetCreType(string creid)
        {
            if (creid == null || creid.Trim() == "")
                return "δָ������";

            int icreid = 0;

            try
            {
                icreid = Int32.Parse(creid);
            }
            catch
            {
                return "����ȷ����" + creid;
            }

            if (icreid >= 1 && icreid <= 11)
            {
                if (icreid == 1)
                {
                    return "���֤";
                }
                else if (icreid == 2)
                {
                    return "����";
                }
                else if (icreid == 3)
                {
                    return "����֤";
                }
                else if (icreid == 4)
                {
                    return "ʿ��֤";
                }
                else if (icreid == 5)
                {
                    return "����֤";
                }
                else if (icreid == 6)
                {
                    return "��ʱ���֤";
                }
                else if (icreid == 7)
                {
                    return "���ڲ�";
                }
                else if (icreid == 8)
                {
                    return "����֤";
                }
                else if (icreid == 9)
                {
                    return "̨��֤";
                }
                else if (icreid == 10)
                {
                    return "Ӫҵִ��";
                }
                else if (icreid == 11)
                {
                    return "����֤��";
                }
                else
                {
                    return "����ȷ����" + creid;
                }
            }
            else
            {
                return "����ȷ����" + creid;
            }
        }

        protected void btnCommit_Click(object sender, System.EventArgs e)
        {

        }

        protected void btnSum_Click(object sender, System.EventArgs e)
        {

        }

    }

}
