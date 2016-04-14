using CFT.CSOMS.BLL.CFTAccountModule;
using SunLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TENCENT.OSS.CFT.KF.KF_Web.BaseAccount
{
    public partial class InfoCenterDelRecord : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string qqid = Request.QueryString["qqid"].ToString();
            try
            {
                //增加删除认证的操作日志
                DataSet dsdgList = new AuthenInfoService().GetUserClassDeleteList(qqid);
                if (dsdgList != null && dsdgList.Tables.Count > 0 && dsdgList.Tables[0].Rows.Count > 0)
                {
                    this.dgList.Visible = true;
                    this.dgList.DataSource = dsdgList.Tables[0].DefaultView;
                    this.DataBind();
                }
                else
                    this.dgList.Visible = false;
            }
            catch (Exception ex)
            {
                LogHelper.LogError("增加删除认证的操作日志失败：qqid=" + qqid + "异常信息：" + ex.ToString());
            }
        }
    }
}