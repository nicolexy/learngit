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
    }
}
