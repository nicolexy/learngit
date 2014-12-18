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
using TENCENT.OSS.CFT.KF.KF_Web.Query_Service;
using TENCENT.OSS.CFT.KF.Common;
using TENCENT.OSS.CFT.KF.KF_Web;
using System.IO;
using System.Configuration;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
    /// <summary>
    /// batPay 的摘要说明。
    /// </summary>
    public partial class AImageShow : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
             string img = Request.Params["img"].ToString();
            //  FileStream files = new FileStream("D:/visualStudio/Projects/kf_branch20130502/KF_WEB/IMAGES/Page/full.png", FileMode.Open);
            //    byte[] imgByte = new byte[files.Length];
            //    files.Read(imgByte, 0, imgByte.Length);
            //    files.Close();

            //    Response.BinaryWrite(imgByte);

            System.Text.Encoding encoding = System.Text.Encoding.GetEncoding("GB2312");

            // EF6BF9DD6CBB46E6B0524487B1EE5DBA可测试img
            string cgi = ConfigurationManager.AppSettings["GetAppealImageCgi"].ToString(); 
            cgi += img;
            System.Net.HttpWebRequest webrequest = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(cgi);
            System.Net.HttpWebResponse webresponse = (System.Net.HttpWebResponse)webrequest.GetResponse();
            byte[] b = null;
            using (Stream stream = webresponse.GetResponseStream())
            using (MemoryStream ms = new MemoryStream())
            {
                int count = 0;
                do
                {
                    byte[] buf = new byte[1024];
                    count = stream.Read(buf, 0, 1024);
                    ms.Write(buf, 0, count);
                } while (stream.CanRead && count > 0);
                b = ms.ToArray();
            }

            webresponse.Close();

            Response.BinaryWrite(b);
        }
    }
}
