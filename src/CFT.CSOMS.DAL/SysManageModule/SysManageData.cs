using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TENCENT.OSS.C2C.Finance.DataAccess;
using TENCENT.OSS.C2C.Finance.Common.CommLib;
using CFT.CSOMS.DAL.Infrastructure;
using CFT.CSOMS.DAL.CFTAccount;

namespace CFT.CSOMS.DAL.SysManageModule
{
    public class SysManageData
    {
        //查询公告联系人所有分组
        public DataSet QueryAllContactsGroup(int limit, int offset)
        {
            try
            {
                using (var da = MySQLAccessFactory.GetMySQLAccess("BulletinContacts"))
                {
                    da.OpenConn();
                    string Sql = string.Format(" select Fid, FgroupName from c2c_db_inc.t_bankbulletin_contacts_groups where Fstate=0 limit {0},{1}", limit, offset);
                    DataSet ds = da.dsGetTotalData(Sql);

                    return ds;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("查询公告联系人所有分组出错："+ex.Message);
            }
        }

        //添加公告联系人分组
        public void AddContactsGroup(string groupName, string createuser)
        {
            try
            {
                using (var da = MySQLAccessFactory.GetMySQLAccess("BulletinContacts"))
                {
                    da.OpenConn();//Fstate 0 有效 1 删除
                    string Sql = string.Format("insert into c2c_db_inc.t_bankbulletin_contacts_groups (FgroupName,Fcreateuser,Fcreatetime,Fstate)values('{0}','{1}','{2}',0)", groupName,createuser, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") );
                    if (!da.ExecSql(Sql))
                    {
                        throw new Exception("添加出错");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("添加公告联系人分组出错：" + ex.Message);
            }
        }

        //删除公告联系人分组
        public void DelContactsGroup(string id, string updateuser)
        {
            try
            {
                using (var da = MySQLAccessFactory.GetMySQLAccess("BulletinContacts"))
                {
                    da.OpenConn();//Fstate 0 有效 1 删除
                    string Sql = string.Format("update c2c_db_inc.t_bankbulletin_contacts_groups set Fstate=1,Fupdateuser='{0}',Fupdatetime='{1}' where Fid='{2}'", updateuser, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), id);
                    if (!da.ExecSql(Sql))
                    {
                        throw new Exception("删除出错");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("删除公告联系人分组出错：" + ex.Message);
            }
        }


        //查询某组公告联系人
        public DataSet QueryOneGroupContacts(string groupId, int limit, int offset)
        {
            try
            {
                using (var da = MySQLAccessFactory.GetMySQLAccess("BulletinContacts"))
                {
                    da.OpenConn();
                    string Sql = string.Format(" select Fid, Fname,Femail from c2c_db_inc.t_bankbulletin_contacts where Fstate=0 and FgroupId='{0}' limit {1},{2}", groupId,limit, offset);
                    DataSet ds = da.dsGetTotalData(Sql);

                    return ds;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("查询公告联系人出错：" + ex.Message);
            }
        }

        //添加公告联系人
        public void AddContacts(string groupId, string name, string email, string createuser)
        {
            try
            {
                using (var da = MySQLAccessFactory.GetMySQLAccess("BulletinContacts"))
                {
                    da.OpenConn();//Fstate 0 有效 1 删除
                    string Sql = string.Format("insert into c2c_db_inc.t_bankbulletin_contacts (FgroupId,Fname,Femail,Fcreateuser,Fcreatetime,Fstate)values('{0}','{1}','{2}','{3}','{4}',0)", groupId, name, email, createuser, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    if (!da.ExecSql(Sql))
                    {
                        throw new Exception("添加出错");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("添加公告联系人出错：" + ex.Message);
            }
        }

        //删除公告联系人
        public void DelContacts(string id,string updateuser)
        {
            try
            {
                using (var da = MySQLAccessFactory.GetMySQLAccess("BulletinContacts"))
                {
                    da.OpenConn();//Fstate 0 有效 1 删除
                    string Sql = string.Format("update c2c_db_inc.t_bankbulletin_contacts set Fstate=1,Fupdateuser='{0}',Fupdatetime='{1}' where Fid='{2}'", updateuser, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), id);
                    if (!da.ExecSql(Sql))
                    {
                        throw new Exception("删除出错");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("删除公告联系人出错：" + ex.Message);
            }
        }

        /// <summary>
        /// 银行公告接口查询
        /// </summary>
        /// <param name="businesstype"></param>
        /// <param name="op_support_flag"></param>
        /// <param name="banktype"></param>
        /// <param name="bulletin_id"></param>
        /// <param name="bull_state"></param>
        /// <param name="bull_type"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="current_datetime"></param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="total_num"></param>
        /// <returns></returns>
        public DataSet QueryBankBulletin(string businesstype, int op_support_flag, int banktype, string bulletin_id,
            string bull_state, string bull_type, string starttime, string endtime, string current_datetime, int limit, int offset, out int total_num)
        {
            string req = "";
            req = "businesstype=" + businesstype;
            if (op_support_flag != 0)
                req += "&op_support_flag=" + op_support_flag;
            if (banktype != 0)
                req += "&banktype=" + banktype;
            if (bulletin_id != "")
                req += "&bulletin_id=" + bulletin_id;
            if (bull_state != "")
                req += "&bull_state=" + bull_state;
            if (bull_type != "")
                req += "&bull_type=" + bull_type;
            if (starttime != "")
                req += "&starttime=" + starttime;
            if (endtime != "")
                req += "&endtime=" + endtime;
            if (current_datetime != "")
                req += "&current_datetime=" + current_datetime;
            req += "&limit=" + limit + "&channeltype=1&offset=" + offset;
            string request_type = "2559";
            string msgno = System.DateTime.Now.ToString("yyyyMMddHHmmss") + request_type + PublicRes.NewStaticNoManage();
            req += "&MSG_NO=" + msgno;

            string ip = CFT.Apollo.Common.Configuration.AppSettings.Get<string>("Relay_IP", "172.27.31.177");
            int port = CFT.Apollo.Common.Configuration.AppSettings.Get<int>("Relay_PORT", 22000);
            string Msg = "";
            total_num = 0;
            
            string answer = RelayAccessFactory.RelayInvoke(req, request_type, false, false, ip, port);
            DataSet ds = null;
            if (answer == "")
            {
                return null;
            }

            
            //解析
            ds = CommQuery.PaseRelayXml(answer, out Msg, out total_num);
            if (Msg != "")
            {
                total_num = 0;
                throw new Exception("请求串：" + req + " " + Msg);
            }

            return ds;
        }

        /// <summary>
        /// 银行公告接口新增，为了测试查询接口写的代码，单元测试中可批量新增公告
        /// </summary>
        /// <param name="businesstype"></param>
        /// <param name="op_support_flag"></param>
        /// <param name="banktype"></param>
        /// <param name="bulletin_id"></param>
        /// <param name="closetype"></param>
        /// <param name="title"></param>
        /// <param name="maintext"></param>
        /// <param name="popuptext"></param>
        /// <param name="createuser"></param>
        /// <param name="updateuser"></param>
        /// <param name="op_flag"></param>
        /// <param name="bull_type"></param>
        /// <param name="startime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        public bool UpdateBankBulletin(string businesstype, int op_support_flag, int banktype, string bulletin_id,
            string closetype, string title, string maintext, string popuptext, string createuser, string updateuser,
            string op_flag, string bull_type, string startime, string endtime)
        {
            string request = "bulletin_id=" + bulletin_id + "&";
            request += "banktype=" + banktype + "&";
            request += "businesstype=" + businesstype + "&";
            request += "op_support_flag=" + op_support_flag + "&";
            request += "closetype=" + closetype + "&";
            request += "title=" + title + "&";
            request += "maintext=" + maintext + "&";
            request += "popuptext=" + popuptext + "&";
            request += "startime=" + startime + "&";
            request += "endtime=" + endtime + "&";
            request += "createuser=" + createuser + "&";
            request += "updateuser=" + updateuser + "&";
            if (op_flag != "")
                request += "op_flag=" + op_flag + "&";
            if (bull_type != "")
                request += "bull_type=" + bull_type;
            string request_type = "8481";
            string msgno = System.DateTime.Now.ToString("yyyyMMddHHmmss") + request_type + PublicRes.NewStaticNoManage();
            request += "MSG_NO=" + msgno;

            string ip = CFT.Apollo.Common.Configuration.AppSettings.Get<string>("Relay_IP", "172.27.31.177");
            int port = CFT.Apollo.Common.Configuration.AppSettings.Get<int>("Relay_PORT", 22000);
            string answer = RelayAccessFactory.RelayInvoke(request, request_type, false, false, ip, port);
            if (answer.IndexOf("result=0") > -1)
                return true;
            else
                throw new Exception("更新公告失败");
        }
    
    }
}
