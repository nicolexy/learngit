using CFT.Apollo.Logging;
using CFT.CSOMS.BLL.BankCheckSystem;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TENCENT.OSS.CFT.KF.DataAccess;

namespace TENCENT.OSS.CFT.KF.KF_Web.BankCheckSystem
{
    public partial class AutoNoticeAndFreeze : System.Web.UI.Page
    {
        int EffectiveDays = 90;//账户有效天数
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                AotuNotice(7);
                AotuNotice(3);
                AotuNotice(1);
                AutoFreeze();
                Response.Write(1);
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
                LogHelper.LogError("自动通知解冻失败；message=" + ex);
            }
        }

        /// <summary>
        /// 通知商户
        /// </summary>
        /// <param name="AdvanceDay">提前天数</param>
        public void AotuNotice(int AdvanceDay)
        {
            int xx = EffectiveDays - AdvanceDay;

            using (MySqlAccess da = new MySqlAccess(CommLib.DbConnectionString.Instance.GetConnectionString("ZW")))
            {
                //邮件参数
                string Params = "username={0}&day={1}";
                //查询需要通知的用户
                string sql = "select Fuser_bind_email ,Fuser_name from  c2c_zwdb.t_bank_userinfos where Fuser_bind_email in (SELECT Fuser_login_account FROM c2c_zwdb.t_bank_userinfo_login where datediff(now(),Flast_update_date)=?)";

                DataTable dt = da.GetTable_Parameters(sql, new List<string>() { xx.ToString() });
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        try
                        {
                            string Email = dr["Fuser_bind_email"].ToString();
                            string username = dr["Fuser_name"].ToString();
                            TENCENT.OSS.C2C.Finance.Common.CommLib.CommMailSend.SendMsg(Email, "2590", string.Format(Params, username, AdvanceDay));
                            LogHelper.LogInfo("银行查单系统自动通知成功;登录名=" + Email + ";提前天数=" + AdvanceDay);
                        }
                        catch (Exception ex)
                        {
                            LogHelper.LogError("银行查单系统自动通知失败；message=" + ex);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 密码使用满90天，冻结用户
        /// </summary>
        public void AutoFreeze()
        {
            using (MySqlAccess da = new MySqlAccess(CommLib.DbConnectionString.Instance.GetConnectionString("ZW")))
            {
                string sql1 = "SELECT Fuser_login_account FROM c2c_zwdb.t_bank_userinfo_login where datediff(now(),Flast_update_date)>=?";
                DataTable dt = da.GetTable_Parameters(sql1, new List<string>() { EffectiveDays.ToString() });
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        try
                        {
                            string userbindemail = dr["Fuser_login_account"].ToString();
                            if ((new BankCheckSystemService().EditUserStatus(userbindemail, ((int)UserStatus.冻结).ToString())))
                            {
                                LogHelper.LogInfo("银行查单系统自动冻结账号成功。登录名=" + userbindemail);

                            }
                            else
                            {
                                LogHelper.LogError("银行查单系统自动冻结账号失败。登录名=" + userbindemail);
                            }
                        }
                        catch (Exception ex)
                        {
                            LogHelper.LogError("银行查单系统自动冻结账号失败。登录名=" + ";message=" + ex.ToString());
                        }
                    }
                }
            }
        }
    }
}