using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace commLib.Entity.HKWallet
{
    [Serializable]
    public class HKWalletRealNameCheckModel
    {
        [XmlElement("approval_id")]
        public string approval_id { get; set; }

        [XmlElement("uin")]
        public string uin { get; set; }

        [XmlElement("uid")]
        public string uid { get; set; }

        [XmlElement("create_time")]
        public string create_time { get; set; }

        [XmlElement("modify_time")]
        public string modify_time { get; set; }

        [XmlElement("operate_time")]
        public string operate_time { get; set; }

        [XmlElement("Operator")]
        public string Operator { get; set; }

        [XmlElement("operate_ip")]
        public string operate_ip { get; set; }

        [XmlElement("state")]
        public string state { get; set; }
        [XmlElement("first_name")]
        public string first_name { get; set; }

        [XmlElement("last_name")]
        public string last_name { get; set; }

        [XmlElement("birthday")]
        public string birthday { get; set; }

        [XmlElement("cre_type")]
        public string cre_type { get; set; }

        [XmlElement("country")]
        public string country { get; set; }

        [XmlElement("cre_id")]
        public string cre_id { get; set; }

        [XmlElement("photo_path1")]
        public string photo_path1 { get; set; }

        [XmlElement("photo_path2")]
        public string photo_path2 { get; set; }

        [XmlElement("photo_path3")]
        public string photo_path3 { get; set; }

        [XmlElement("memo")]
        public string memo { get; set; }

        [XmlElement("type")]
        public string type { get; set; }

        [XmlElement("gender")]
        public string gender { get; set; }

        [XmlElement("Operator_str")]
        public string Operator_str
        {
            get
            {
                return System.Web.HttpUtility.UrlDecode(Operator);
            }
            set { }
        }
        [XmlElement("state_str")]
        public string state_str
        {
            get
            {
                return state == "1" ? "待审核" :
                        state == "2" ? "审核通过" :
                        state == "3" ? "客服审核不通过" :
                         state == "4" ? "机器审核不通过" :
                        "未知：" + state;
            }
            set { }
        }
        [XmlElement("name")]
        public string name
        {
            get
            {
                return first_name + last_name;
            }
            set { }
        }
        [XmlElement("cre_type_str")]
        public string cre_type_str
        {
            get
            {
                return cre_type == "1" ? "中国身份证" :
                                    cre_type == "2" ? "护照" :
                                    cre_type == "3" ? "军官证" :
                                    cre_type == "4" ? "士兵证" :
                                    cre_type == "5" ? "回乡证" :
                                    cre_type == "6" ? "中国临时身份证" :
                                    cre_type == "7" ? "户口簿" :
                                    cre_type == "8" ? "警官证" :
                                    cre_type == "9" ? "台胞证" :
                                    cre_type == "10" ? "营业执照" :
                                    cre_type == "11" ? "其它证件" :
                                    cre_type == "21" ? "香港永久身份证" :
                        "未知：" + cre_type;
            }
            set { }
        }
        [XmlElement("type_str")]
        public string type_str
        {
            get
            {
                return type == "1" ? "客服验证" :
                        type == "2" ? "机器验证" :
                        type == "3" ? "银行验证" :
                        "未知：" + type;
            }
            set { }
        }
        [XmlElement("memo_str")]
        public string memo_str
        {
            get
            {
                return memo == "01" ? "登记姓名和证件照片中内容不符" :
                        memo == "02" ? "登记出生日期和证件照片中内容不符" :
                        memo == "03" ? "登记证件类型和证件照片中内容不符" :
                        memo == "04" ? "登记证件号码和证件照片中内容不符" :
                        memo == "05" ? "证件照片模糊不清" :
                        "";
            }
            set { }
        }


    }
}
