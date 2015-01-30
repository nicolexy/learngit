using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using CFT.CSOMS.DAL.RefundModule;

namespace CFT.CSOMS.BLL.RefundModule
{
    
    public class BankEmailService
    {
        //查询公告联系人所有分组
        public DataSet QueryAllContactsGroup(int limit, int offset)
        {
            return new BankEmailData().QueryAllContactsGroup(limit, offset);
        }

        //添加公告联系人分组
        public void AddContactsGroup(string groupName, string createuser)
        {
            new BankEmailData().AddContactsGroup(groupName, createuser);
        }

        //删除公告联系人分组
        public void DelContactsGroup(string id, string updateuser)
        {
            new BankEmailData().DelContactsGroup(id, updateuser);
        }

        //查询某组公告联系人
        public DataSet QueryOneGroupContacts(string groupId, int limit, int offset)
        {
            return new BankEmailData().QueryOneGroupContacts(groupId, limit, offset);
        }

        //添加公告联系人
        public void AddContacts(string groupId, string name, string email, string createuser)
        {
            new BankEmailData().AddContacts(groupId, name, email, createuser);
        }

        //删除公告联系人
        public void DelContacts(string id, string updateuser)
        {
            new BankEmailData().DelContacts(id, updateuser);
        }
    }
}
