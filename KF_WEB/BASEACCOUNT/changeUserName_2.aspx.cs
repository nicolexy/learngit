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

using Tencent.DotNet.Common.UI;
using Tencent.DotNet.OSS.Web.UI;
using TENCENT.OSS.C2C.Finance.Common;
using TENCENT.OSS.CFT.KF.Common;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
	/// <summary>
	/// changeUserName_2 的摘要说明。
	/// </summary>
	public partial class changeUserName_2 : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
			/*
			if (Session["uid"] == null)
			{
				Response.Redirect("../login.aspx?wh=1");
			}
			*/
			
			string szkey = Session["SzKey"].ToString();
			//int operid = Int32.Parse(Session["OperID"].ToString());
			
			//if (!AllUserRight.ValidRight(szkey,operid,PublicRes.GROUPID,"InfoCenter")) Response.Redirect("../login.aspx?wh=1");

			if(!classLibrary.ClassLib.ValidateRight("InfoCenter",this)) Response.Redirect("../login.aspx?wh=1");

			this.Label1.Text = Session["OperID"].ToString();
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



		protected void Button_Update_Click(object sender, System.EventArgs e)
        {
            string url = "";
            try
            {
                //校验用户的QQ号和密码是否相符
                string qqid = this.TX_QQID.Text.Trim();
                string name = this.txtOldName.Text.Trim();
                string newName = this.txbNewName.Text.Trim();
                string mail = this.txMail.Text.Trim();

                bool exeSign = CheckOldName(qqid, name);
                if (exeSign == false)
                {
                    WebUtils.ShowMessage(this.Page, "对不起！ QQ号码和原姓名不符！ 不能提交申请！");
                    return;
                }

                //上传需要的图片，并返回对应服务器上的地址
                //存放文件
                string alPath;
                HtmlInputFile inputFile = this.File1;

                alPath = PublicRes.upImage(inputFile, "Account");

                //string mail  = this.txMail.Text.Trim();
                string reason = this.txReason.Text.Trim();

                reason = classLibrary.setConfig.replaceSqlStr(reason);

                string fetchNo = "101" + DateTime.Now.ToString("yyyyMMddHHmmssff").ToString();  //101修改姓名
                string commTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").ToString();

                url = "fetchName.aspx?QQID=" + qqid + "&mail=" + mail + "&reason=" + TENCENT.OSS.C2C.Finance.Common.CommLib.CommQuery.URLDecode(reason)
                    + "&oldName=" + TENCENT.OSS.C2C.Finance.Common.CommLib.CommQuery.URLEncode(name) + "&newName="
                    + TENCENT.OSS.C2C.Finance.Common.CommLib.CommQuery.URLEncode(newName)
                    + "&fetchNo=" + fetchNo + "&commTime=" + commTime + "&accPath=" + alPath + "&infoPath=" + alPath;
              
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "修改姓名异常！" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
                return;
            }

            Response.Redirect(url);
		}

        //public static string UploadImage(HtmlInputFile File)
        //{
        //    string s1 = File.Value;
        //    string szTypeName = s1.Substring(s1.Length - 4, 4);
        //    string upStr = null;
        //    string alPath;
        //    try
        //    {
        //        if (szTypeName.ToLower() != ".jpg" && szTypeName.ToLower() != ".gif" && szTypeName.ToLower() != ".bmp")
        //        {
        //            throw new Exception("上传的文件不正确，必须为jpg,gif,bmp");
        //        }

        //        if (File.Value != "")
        //        {
        //            string fileName = "s1" + DateTime.Now.ToString("yyyyMMddHHmmss") + szTypeName; //
        //            //System.Configuration.ConfigurationManager.AppSettings["uploadPath"].ToString();

        //            string src = System.Configuration.ConfigurationManager.AppSettings["KFWebSrc"].ToString();


        //            upStr = "uploadfile\\" + System.DateTime.Now.ToString("yyyyMMdd") + "\\CSOMS\\Account";


        //            string targetPath = src + upStr;

        //            SunLibrary.LoggerFactory.Get("kf_web changeUserName_2:").Info(" targetPath= " + targetPath);

        //            PublicRes.CreateDirectory(targetPath);

        //            SunLibrary.LoggerFactory.Get("kf_web changeUserName_2:").Info("createdirectory success");

        //            string path = targetPath + "\\" + fileName;
        //            File.PostedFile.SaveAs(path);

        //            SunLibrary.LoggerFactory.Get("kf_web changeUserName_2:").Info("upFile success");

        //            //alPath.Add(upStr+ "/" +fileName);	
        //            alPath = upStr.Replace("\\", "/") + "/" + fileName;
        //        }
        //        else
        //        {
        //            //alPath.Add(upStr+ "/blankBankCard.gif");
        //            alPath = upStr + "/blankBankCard.gif";
        //        }
        //    }
        //    catch (Exception eStr)
        //    {
        //        string errMsg = "上传文件失败！" + PublicRes.GetErrorMsg(eStr.Message.ToString());
        //        throw new Exception(errMsg);
        //    }
        //    return alPath;
        //}


		/*   这个是账务系统 的原版本
		private void Button_Update_Click(object sender, System.EventArgs e)
		{
			//校验用户的QQ号和密码是否相符
			string qqid = this.TX_QQID.Text.Trim();
			string name = this.txtOldName.Text.Trim();
			string newName = this.txbNewName.Text.Trim();
			
			bool exeSign = CheckOldName(qqid,name);
			if (exeSign == false)
			{
				//WebUtils.ShowMessage(this.Page,"对不起！ QQ号码和原姓名不符！ 不能提交申请！");
				return;
			}

			//上传需要的图片，并返回对应服务器上的地址
			//存放文件
			string s1 = File1.Value;
			string s2 = File2.Value;
			string s3 = File3.Value;
			string s4 = File4.Value;

			int i1 = s1.Length;
			int i2 = s2.Length;
			int i3 = s3.Length;
			int i4 = s4.Length;

			ArrayList al = new ArrayList();
			al.Add(s1.Substring(i1-4,4));
			al.Add(s1.Substring(i1-4,4));
			al.Add(s2.Substring(i2-4,4));
			al.Add(s3.Substring(i3-4,4));

			if (i4 > 4)
			{
				al.Add(s4.Substring(i4-4,4));	
			}
			else
			{
				al.Add(null);
			}
			

			HtmlInputFile [] inputFile = new HtmlInputFile[5]; 
			inputFile[1] = File1;
			inputFile[2] = File2;
			inputFile[3] = File3;
			inputFile[4] = File4;

			ArrayList alPath = new ArrayList();

			string upStr = null;

			//上传文件,上传4个图片
			try
			{
				for(int i=1;i<=4;i++)
				{
					//允许上传的银行卡为空
					if(i!=4 || (i==4 && inputFile[4].Value != ""))
					{
						if (al[i].ToString().ToLower() != ".jpg" && al[i].ToString().ToLower() != ".gif" && al[i].ToString().ToLower() != ".bmp")
						{
							WebUtils.ShowMessage(this.Page,"上传的第"+ i +"个文件不正确，必须为jpg,gif,bmp");	
							return;
						}
					}

					if (inputFile[i].Value != "")
					{
						string fileName = "s" + i + DateTime.Now.ToString("yyyyMMddHHmmss") + al[i]; //
						upStr = "uploadfile";//System.Configuration.ConfigurationManager.AppSettings["uploadPath"].ToString();
						string path = Server.MapPath(Request.ApplicationPath) + "\\" + upStr + "\\" + fileName;
						inputFile[i].PostedFile.SaveAs(path);

						alPath.Add(upStr+ "/" +fileName);	
					}
					else
					{
						alPath.Add(upStr+ "/blankBankCard.gif");
					}				
				}	
			}
			catch(Exception eStr)
			{
				string errMsg = "上传文件失败！" + eStr.Message.ToString().Replace("'","’");
				WebUtils.ShowMessage(this.Page,""+ errMsg);	
				return;
			}

			//string oldName,newName,QQID,mail,reason,accPath,infoPath,oldIdCardPath,newIdCardPath;
			//生成跳转地址
			string mail  = this.txMail.Text.Trim();
			string reason= this.txReason.Text.Trim();

			reason = classLibrary.setConfig.replaceSqlStr(reason);

			string fetchNo = "101" + DateTime.Now.ToString("yyyyMMddHHmmssff").ToString();  //101修改姓名
			string commTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").ToString();

			string url = "fetchName.aspx?QQID=" + qqid + "&mail=" + mail + "&reason=" + Common.CommLib.CommQuery.URLEncode(reason) + "&oldName=" + Common.CommLib.CommQuery.URLEncode(name) + "&newName=" + Common.CommLib.CommQuery.URLEncode(newName) 
				+ "&fetchNo=" + fetchNo + "&commTime=" + commTime + "&accPath=" + alPath[0].ToString() + "&infoPath=" + alPath[1].ToString()
				+ "&newIdCardPath=" + alPath[2].ToString() + "&oldIdCardPath=" + alPath[3].ToString();
			Response.Redirect(url);
		}
		*/

		private bool CheckOldName(string qqid,string oldName)
		{
			string Msg = null;
			bool exeSign;

			//调用service
			try
			{
				Finance_ManageService.Finance_Manage fs = new TENCENT.OSS.CFT.KF.KF_Web.Finance_ManageService.Finance_Manage();
				exeSign = fs.CheckOldName(qqid,oldName,out Msg);
				//判断执行结果
				if (exeSign == false)
				{
					WebUtils.ShowMessage(this.Page,Msg);
					return false;
				}
				else
				{
					return true;
				}	
			}
			catch(Exception e)
			{
				WebUtils.ShowMessage(this.Page,e.Message.ToString().Replace("'","’"));
				return false;
			}
			
			/*
			//调用service
			try
			{
				Finance_ManageService.Finance_Manage  fm = new Finance_Web.Finance_ManageService.Finance_Manage();
				exeSign = fm.CheckOldName(qqid,oldName,out Msg);
				
				//判断执行结果
				if (exeSign == false)
				{
					WebUtils.ShowMessage(this.Page,Msg);
					return false;
				}
				else
				{
					return true;
				}	
			}
			catch(Exception e)
			{
				WebUtils.ShowMessage(this.Page,e.Message.ToString().Replace("'","’"));
				return false;
			}
			*/
		}	
	}
	
	
}
