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
	/// changeUserName_2 ��ժҪ˵����
	/// </summary>
	public partial class changeUserName_2 : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
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



		protected void Button_Update_Click(object sender, System.EventArgs e)
        {
            string url = "";
            try
            {
                //У���û���QQ�ź������Ƿ����
                string qqid = this.TX_QQID.Text.Trim();
                string name = this.txtOldName.Text.Trim();
                string newName = this.txbNewName.Text.Trim();
                string mail = this.txMail.Text.Trim();

                bool exeSign = CheckOldName(qqid, name);
                if (exeSign == false)
                {
                    WebUtils.ShowMessage(this.Page, "�Բ��� QQ�����ԭ���������� �����ύ���룡");
                    return;
                }

                //�ϴ���Ҫ��ͼƬ�������ض�Ӧ�������ϵĵ�ַ
                //����ļ�
                string alPath;
                HtmlInputFile inputFile = this.File1;

                alPath = PublicRes.upImage(inputFile, "Account");

                //string mail  = this.txMail.Text.Trim();
                string reason = this.txReason.Text.Trim();

                reason = classLibrary.setConfig.replaceSqlStr(reason);

                string fetchNo = "101" + DateTime.Now.ToString("yyyyMMddHHmmssff").ToString();  //101�޸�����
                string commTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").ToString();

                url = "fetchName.aspx?QQID=" + qqid + "&mail=" + mail + "&reason=" + TENCENT.OSS.C2C.Finance.Common.CommLib.CommQuery.URLDecode(reason)
                    + "&oldName=" + TENCENT.OSS.C2C.Finance.Common.CommLib.CommQuery.URLEncode(name) + "&newName="
                    + TENCENT.OSS.C2C.Finance.Common.CommLib.CommQuery.URLEncode(newName)
                    + "&fetchNo=" + fetchNo + "&commTime=" + commTime + "&accPath=" + alPath + "&infoPath=" + alPath;
              
            }
            catch (Exception eSys)
            {
                WebUtils.ShowMessage(this.Page, "�޸������쳣��" + PublicRes.GetErrorMsg(eSys.Message.ToString()));
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
        //            throw new Exception("�ϴ����ļ�����ȷ������Ϊjpg,gif,bmp");
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
        //        string errMsg = "�ϴ��ļ�ʧ�ܣ�" + PublicRes.GetErrorMsg(eStr.Message.ToString());
        //        throw new Exception(errMsg);
        //    }
        //    return alPath;
        //}


		/*   ���������ϵͳ ��ԭ�汾
		private void Button_Update_Click(object sender, System.EventArgs e)
		{
			//У���û���QQ�ź������Ƿ����
			string qqid = this.TX_QQID.Text.Trim();
			string name = this.txtOldName.Text.Trim();
			string newName = this.txbNewName.Text.Trim();
			
			bool exeSign = CheckOldName(qqid,name);
			if (exeSign == false)
			{
				//WebUtils.ShowMessage(this.Page,"�Բ��� QQ�����ԭ���������� �����ύ���룡");
				return;
			}

			//�ϴ���Ҫ��ͼƬ�������ض�Ӧ�������ϵĵ�ַ
			//����ļ�
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

			//�ϴ��ļ�,�ϴ�4��ͼƬ
			try
			{
				for(int i=1;i<=4;i++)
				{
					//�����ϴ������п�Ϊ��
					if(i!=4 || (i==4 && inputFile[4].Value != ""))
					{
						if (al[i].ToString().ToLower() != ".jpg" && al[i].ToString().ToLower() != ".gif" && al[i].ToString().ToLower() != ".bmp")
						{
							WebUtils.ShowMessage(this.Page,"�ϴ��ĵ�"+ i +"���ļ�����ȷ������Ϊjpg,gif,bmp");	
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
				string errMsg = "�ϴ��ļ�ʧ�ܣ�" + eStr.Message.ToString().Replace("'","��");
				WebUtils.ShowMessage(this.Page,""+ errMsg);	
				return;
			}

			//string oldName,newName,QQID,mail,reason,accPath,infoPath,oldIdCardPath,newIdCardPath;
			//������ת��ַ
			string mail  = this.txMail.Text.Trim();
			string reason= this.txReason.Text.Trim();

			reason = classLibrary.setConfig.replaceSqlStr(reason);

			string fetchNo = "101" + DateTime.Now.ToString("yyyyMMddHHmmssff").ToString();  //101�޸�����
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

			//����service
			try
			{
				Finance_ManageService.Finance_Manage fs = new TENCENT.OSS.CFT.KF.KF_Web.Finance_ManageService.Finance_Manage();
				exeSign = fs.CheckOldName(qqid,oldName,out Msg);
				//�ж�ִ�н��
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
				WebUtils.ShowMessage(this.Page,e.Message.ToString().Replace("'","��"));
				return false;
			}
			
			/*
			//����service
			try
			{
				Finance_ManageService.Finance_Manage  fm = new Finance_Web.Finance_ManageService.Finance_Manage();
				exeSign = fm.CheckOldName(qqid,oldName,out Msg);
				
				//�ж�ִ�н��
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
				WebUtils.ShowMessage(this.Page,e.Message.ToString().Replace("'","��"));
				return false;
			}
			*/
		}	
	}
	
	
}
