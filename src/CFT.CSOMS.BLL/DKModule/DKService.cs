using CFT.CSOMS.DAL.DKModule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CFT.CSOMS.BLL.DKModule
{
   public  class DKService
    {
        public DataTable GetDKBankList()
        {
            DataTable dt=new DKData().GetDKBankList();
            DataTable dtnew = new DataTable();
            dtnew.Columns.Add("Fbank_name", typeof(string));
            dtnew.Columns.Add("Fbank_sname",typeof(string));
            dtnew.Columns.Add("Fbank_type", typeof(string));

            foreach (DataRow dr in dt.Rows) 
            {
                if (dr["pay_bank_type"].ToString().Trim() != "") 
                {
                    DataRow drnew = dtnew.NewRow();
                    drnew["Fbank_sname"] = dr["bank_sname"].ToString().Trim();
                    drnew["Fbank_type"] = dr["pay_bank_type"].ToString().Trim();
                    dtnew.Rows.Add(drnew);
                }
            }
            return dtnew;
        }
        public DataTable GetDKLimit_List(string bank_sname, string bankaccno, string servicecode, int limStart, int limMax)
        {
            return new DKData().GetDKLimit_List(bank_sname, bankaccno, servicecode, limStart, limMax);
        }
    }
}
