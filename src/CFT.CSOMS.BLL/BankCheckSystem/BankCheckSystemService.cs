using CFT.CSOMS.DAL.BankCheckSystem;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CFT.CSOMS.BLL.BankCheckSystem
{
    public class DescriptionAttribute : Attribute
    {
        public DescriptionAttribute(string desc)
        {
            _Desc = desc;
        }
        private string _Desc;
        public string Desc { get { return _Desc; } }
    }

    public enum OperationType
    {
        [Description("创建账号")]
        CreateUser = 20001,

        [Description("重置密码")]
        ResetPWD = 20003,

        [Description("修改用户基本信息")]
        ModifyUserInfo = 20005,

        [Description("冻结")]
        Freeze = 20007,

        [Description("作废")]
        ToInvalid = 20009,

        [Description("解冻")]
        UnFreeze = 20011
    }
    public enum UserStatus
    {
        [Description("创建帐号未激活")]
        IsInitial = 0,

        [Description("正常")]
        IsNormal = 1,

        [Description("冻结")]
        IsFreeze = 2,

        [Description("作废")]
        IsInvalid = 3,
    }

    public class BankCheckSystemService
    {
        public Dictionary<string, string> GetRights()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("1", "登录");
            dic.Add("2", "修改密码");
            dic.Add("3", "查询权限");
            dic.Add("4", "批量查询");
            return dic;
        }
        /// <summary>
        /// 获取枚举的特性
        /// </summary>
        /// <param name="myEnum"></param>
        /// <returns></returns>
        public string GetEnumDescription(Enum myEnum)
        {
            Type type = myEnum.GetType();
            FieldInfo fd = type.GetField(myEnum.ToString());
            if (fd == null)
                return string.Empty;
            object[] attrs = fd.GetCustomAttributes(typeof(DescriptionAttribute), false);
            string name = string.Empty;
            foreach (DescriptionAttribute attr in attrs)
            {
                name = attr.Desc;
            }
            return name;
        }


        public bool UsersAdd(out string pwd, string Fuser_login_account, string Fbank_id, string Fuser_bind_email, string Fuser_name, string Fuser_id_no,
              string Fcontact_address, string Fcontact_name, string Fcontact_tel, string Fcontact_mobile, string Fuser_id_no_url,
              string Fcontact_qq, string Fremark, List<string> Fauth_levels)
        {
            if (string.IsNullOrEmpty(Fuser_bind_email))
            {
                throw new Exception("绑定邮箱不能为空！");
            }

            return new BankCheckSystemData().UsersAdd(out pwd, Fuser_login_account, Fbank_id, Fuser_bind_email, Fuser_name, Fuser_id_no,
             Fcontact_address, Fcontact_name, Fcontact_tel, Fcontact_mobile, Fuser_id_no_url,
             Fcontact_qq, Fremark, Fauth_levels);
        }

        public bool checkUser(string Fuser_login_account, string Fuser_id_no)
        {
            return new BankCheckSystemData().CheckUser(Fuser_login_account, Fuser_id_no);
        }

        public DataTable getUserinfo(string Fuser_login_account)
        {
            return new BankCheckSystemData().GetUserinfo(Fuser_login_account);
        }
        public DataTable GetUserinfo2(string userBindEmail, string bankId, string userName, string userIdNo, int offset, int limit)
        {
            DataTable dt = new BankCheckSystemData().GetUserinfo2(userBindEmail, bankId, userName, userIdNo, offset, limit);
            dt.Columns.Add("Fuser_status_str");
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    //dr["Fuser_status_str"] = System.Enum.Parse(typeof(UserStatus), dr["Fuser_status"].ToString().Trim()).ToString();
                    dr["Fuser_status_str"] = GetEnumDescription((Enum)System.Enum.Parse(typeof(UserStatus), dr["Fuser_status"].ToString().Trim()));
                }
            }
            return dt;
        }
        public bool ResetPwd(string Fuser_login_account, out string newpassword)
        {
            return new BankCheckSystemData().ResetPwd(Fuser_login_account, out newpassword);
        }

        public DataTable GetUserAuthRelation(string userId)
        {
            return new BankCheckSystemData().GetUserAuthRelation(userId);
        }

        public bool EditAuthRelation(string userId, List<string> rights)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception("userId为空");
            }
            return new BankCheckSystemData().EditAuthRelation(userId, rights);
        }

        public bool EditUsersInfo(string Fuser_bind_email, string Fcontact_name, string Fcontact_mobile, string Fcontact_qq, string Fcontact_email, string Fcontact_manager, string Fremark)
        {
            return new BankCheckSystemData().EditUsersInfo(Fuser_bind_email, Fcontact_name, Fcontact_mobile, Fcontact_qq, Fcontact_email, Fcontact_manager, Fremark);
        }
        public DataTable getUserRecords(string Fuser_bind_email, int offset, int limit)
        {
            DataTable dt = new BankCheckSystemData().getUserRecords(Fuser_bind_email, offset, limit);
            dt.Columns.Add("Fuser_bind_email");
            dt.Columns.Add("Ftype_id_str");

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    dr["Fuser_bind_email"] = Fuser_bind_email;
                    //dr["Ftype_id_str"] = System.Enum.Parse(typeof(OperationType), dr["Ftype_id"].ToString().Trim().ToString()).ToString();
                    dr["Ftype_id_str"] = GetEnumDescription((Enum)System.Enum.Parse(typeof(OperationType), dr["Ftype_id"].ToString().Trim()));
                }
            }
            return dt;
        }
        public bool InsertRecords(string Fuser_bind_email, string TypeId, string Reason, string adminName)
        {
            return new BankCheckSystemData().InsertRecords(Fuser_bind_email, TypeId, Reason, adminName);
        }
        public bool EditUserStatus(string Fuser_bind_email, string Status)
        {
            if (string.IsNullOrEmpty(Fuser_bind_email) || string.IsNullOrEmpty(Status))
            {
                return false;
            }
            return new BankCheckSystemData().EditUserStatus(Fuser_bind_email, Status);
        }
    }
}
