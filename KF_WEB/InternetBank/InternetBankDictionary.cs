using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TENCENT.OSS.CFT.KF.KF_Web.InternetBank
{
    public static class InternetBankDictionary
    {
        public static Dictionary<string, string> TradeState
        {
            get
            {
                if (_tradeState == null)
                {
                    _tradeState = new Dictionary<string, string>();
                    _tradeState.Add("0", "初始");
                    _tradeState.Add("1", "取扣款URL成功");
                    _tradeState.Add("2", "取扣款URL失败");
                    _tradeState.Add("3", "扣款成功");
                    _tradeState.Add("4", "扣款失败");
                    _tradeState.Add("5", "发货成功");
                    _tradeState.Add("6", "发货失败");
                }
                return _tradeState;
            }
        }

        static Dictionary<string, string> _tradeState = null;

        public static Dictionary<string, string> PayChannel
        {
            get
            {
                if (_payChannel == null)
                {
                    _payChannel = new Dictionary<string, string>();
                    _payChannel.Add("4002", "财付通直通车");
                    _payChannel.Add("3003", "农行电话银行直充");
                    _payChannel.Add("100", "财付通余额");
                    _payChannel.Add("4001", "网汇通(财付通)");
                    _payChannel.Add("1028", "广银联(财付通)");
                    _payChannel.Add("1006", "中国民生银行");
                    _payChannel.Add("1005", "农行(财付通网关)");
                    _payChannel.Add("1004", "上海浦发银行");
                    _payChannel.Add("1009", "兴业银行");
                    _payChannel.Add("1008", "深圳发展银行");
                    _payChannel.Add("13", "拍拍充值");
                    _payChannel.Add("1003", "建行((财付通网关)");
                    _payChannel.Add("1002", "工行(财付通网关)");
                    _payChannel.Add("1001", "招行(财付通网关)");
                    _payChannel.Add("10001", "Q币联盟(财付通网关)");
                    _payChannel.Add("WY51", "网银五一赠送");
                    _payChannel.Add("8", "农行国际信用卡");
                    _payChannel.Add("5", "农业银行");
                    _payChannel.Add("4", "银联");
                    _payChannel.Add("3002", "建行直充电话银行");
                    _payChannel.Add("3", "招商银行");
                    _payChannel.Add("2002", "工行绑定");
                    _payChannel.Add("2001", "招行绑定");
                    _payChannel.Add("20", "招行B2C电话银行");
                    _payChannel.Add("2", "建设银行");
                    _payChannel.Add("19", "工行B2C电话银行");
                    _payChannel.Add("17", "香港渣打银行");
                    _payChannel.Add("16", "广银联");
                    _payChannel.Add("15", "招行银行卡绑定");
                    _payChannel.Add("1", "工商银行");
                    _payChannel.Add("3004", "广工行电话银行直充");
                    _payChannel.Add("5005", "农行电子账单");
                    _payChannel.Add("1020", "交通银行");
                    _payChannel.Add("9999", "财付通余额");
                    _payChannel.Add("8002", "财付通网关");
                    _payChannel.Add("TENPAY", "财付通实收(虚拟)");
                    _payChannel.Add("18", "希之光网吧平台");
                    _payChannel.Add("6", "IPAY");
                    _payChannel.Add("1036", "工行国际信用卡");
                    _payChannel.Add("5001", "财付一点通");
                    _payChannel.Add("2842", "兴业银行");
                    _payChannel.Add("33", "财付通直通车(33)");
                    _payChannel.Add("8004", "一点通");
                    _payChannel.Add("1052", "中国银行");
                    _payChannel.Add("8008", "财付直通车（子账户）");
                    _payChannel.Add("35", "境外卡支付");
                    _payChannel.Add("8006", "QQ返利积分子账户");
                    _payChannel.Add("8007", "财付通游戏子账户");
                    _payChannel.Add("TENPAY_TM", "QQ特卖场(虚拟)");
                    _payChannel.Add("1038", "新版招行(财付通网关)");
                    _payChannel.Add("2003", "建行一点通");
                    _payChannel.Add("34", "财付通充值卡");
                    _payChannel.Add("11", "兴业银行QQ秀");
                    _payChannel.Add("TENPAY_REFUND", "财付通退款(虚拟)");
                    _payChannel.Add("TENPAY_MATCH_REWARD", "财付通符合返利策略(虚拟)");
                    _payChannel.Add("1060", "工行商城(财付通网关)");
                    _payChannel.Add("TENPAY_GW", "财付通网关(虚拟)");
                    _payChannel.Add("TENPAY_WB", "希之光网吧平台(虚拟)");
                    _payChannel.Add("TENPAY_TM2", "QQ特卖场(虚拟2)");
                    _payChannel.Add("8009", "财付通充值卡(子帐户)");
                    _payChannel.Add("8101", "财付通余额(财付通小钱包)");
                    _payChannel.Add("8102", "银行卡(财付通小钱包)");
                    _payChannel.Add("8104", "一点通(财付通小钱包)");
                    _payChannel.Add("HDC", "财付通个帐补充值（虚拟）");
                    _payChannel.Add("TENPAY_HANDLE", "财付通网关手工提供（虚拟）");
                    _payChannel.Add("BANK_3", "招商银行2(虚拟)");
                    _payChannel.Add("BANK_1", "工商银行2(虚拟)");
                    _payChannel.Add("7001", "游戏子账户(7001)");
                    _payChannel.Add("2011", "财付通余额(pay.qq.com)");
                    _payChannel.Add("2012", "财付通网关(pay.qq.com)");
                    _payChannel.Add("2013", "一点通(pay.qq.com)");
                    _payChannel.Add("36", "手机财付通");
                    _payChannel.Add("37", "拍拍子渠道");
                    _payChannel.Add("38", "无线移动电商");
                    _payChannel.Add("8010", "财付通直通车V3");
                    _payChannel.Add("39", "手机支付(pay.qq.com)");
                    _payChannel.Add("MPAY_TENPAY", "MPAY财付通支付(虚拟)");
                    _payChannel.Add("8116", "财付通光大储值卡");
                    _payChannel.Add("40", "手机腾讯网wap支付");
                    _payChannel.Add("7002", "彩贝积分子账户(7002)");
                    _payChannel.Add("41", "无线电商营销活动");
                    _payChannel.Add("8132", "刷卡支付");
                    _payChannel.Add("7003", "微支付（手机）");
                    _payChannel.Add("2021", "财付通彩贝");
                    _payChannel.Add("8114", "快捷支付");
                    _payChannel.Add("3100", "互娱营销活动（余额）");
                    _payChannel.Add("3101", "互娱营销活动（网关）");
                    _payChannel.Add("2022", "财付通彩贝（小钱包）");
                    _payChannel.Add("42", "财付通会员工行信用卡积分");
                    _payChannel.Add("43", "财付通会员中信信用卡积分");
                }
                return _payChannel;
            }
        }

        static Dictionary<string, string> _payChannel = null;

        public static Dictionary<string, string> ServiceCode
        {
            get
            {
                if (_serviceCode == null)
                {
                    _serviceCode = new Dictionary<string, string>();
                    _serviceCode.Add("LTMCLUB", "QQ会员");
                    _serviceCode.Add("-XXQSHOW", "QQ秀");
                    _serviceCode.Add("-QQS2", "QQ秀");
                    _serviceCode.Add("XXLOVE", "QQ交友");
                    _serviceCode.Add("QQACCT_SAVE", "Q币充值");
                    _serviceCode.Add("QTD512M", "网络硬盘");
                    _serviceCode.Add("XXZXYY", "绿钻贵族");
                    _serviceCode.Add("XXQQF", "QQ秀红钻");
                    _serviceCode.Add("XXQGAME", "QQ游戏蓝钻");
                    _serviceCode.Add("XXJZGW", "黄钻贵族");
                    _serviceCode.Add("XXQQT", "QQ堂紫钻");
                    _serviceCode.Add("-QQPOINT", "Q点");
                    _serviceCode.Add("-BANKQB", "虚拟QQ卡");
                    _serviceCode.Add("-BANKLH", "6位靓号");
                    _serviceCode.Add("QQR2BY", "QQ音速");
                    _serviceCode.Add("PETVIP", "QQ宠物粉钻");
                    _serviceCode.Add("-QQSSZ", "QQ闪装");
                    _serviceCode.Add("-SGROUP", "高级群");
                    _serviceCode.Add("XXAQXZ", "爱情小镇VIP");
                    _serviceCode.Add("QQPETTC", "宠物套餐");
                    _serviceCode.Add("-HXFR", "QQ华夏");
                    _serviceCode.Add("-MMJOL", "魔界OL");
                    _serviceCode.Add("-MXYD2", "侠义道2");
                    _serviceCode.Add("-BANKTY", "财付通支付Q币体验");
                    _serviceCode.Add("-MMOYU", "魔域");
                    _serviceCode.Add("-QQLIVEJC", "QQLive英超");
                    _serviceCode.Add("-GRSPACE", "QQ空间物品");
                    _serviceCode.Add("-XXQQCW", "宠物礼包");
                    _serviceCode.Add("-QQSGPAY", "QQ三国点");
                    _serviceCode.Add("-PETLB", "猪猪超市礼包");
                    _serviceCode.Add("-CFDQ", "穿越火线");
                    _serviceCode.Add("-DNFDQ", "DNF点券");
                    _serviceCode.Add("HLVIP", "豆友俱乐部");
                    _serviceCode.Add("DNFHZ", "DNF黑钻");
                    _serviceCode.Add("QQFCZZ", "QQ飞车紫钻");
                    _serviceCode.Add("YKYXCQ", "QQ音信贵族");
                    _serviceCode.Add("CFCLUB", "CF会员");
                    _serviceCode.Add("MAGPT", "读书VIP");
                    _serviceCode.Add("XXYYHZ", "黄绿贵族");
                    _serviceCode.Add("-QQBOOKAT", "VIP图书单购");
                    _serviceCode.Add("BANK", "网银大包月");
                    _serviceCode.Add("-XXXY", "寻仙仙玉");
                    _serviceCode.Add("-SLYX", "丝路英雄");
                    _serviceCode.Add("-LTGJQ", "新高级群按条");
                    _serviceCode.Add("-LHSC", "靓号商城");
                    _serviceCode.Add("-XXYXLB", "欢乐大礼包");
                    _serviceCode.Add("-BANKTY1", "网银体验购买");
                    _serviceCode.Add("QQKDC", "QQ飞车点券");
                    _serviceCode.Add("-XXYYSC", "QQ空间背景音乐");
                    _serviceCode.Add("XXVIP", "寻仙VIP");
                    _serviceCode.Add("QQXWZZ", "QQ炫舞紫钻");
                    _serviceCode.Add("-ZYXX", "中国高校就业信息");
                    _serviceCode.Add("-QCWQB", "QQ秀抢车位");
                    _serviceCode.Add("QQBOOKBY", "图书VIP系统包月");
                    _serviceCode.Add("-XWDQ", "QQ炫舞点券");
                    _serviceCode.Add("-YXDWEB", "英雄岛WEB支付");
                    _serviceCode.Add("-FFOWY", "自由幻想彩玉");
                    _serviceCode.Add("BOOKCLUB", "读书VIP-QQ会员合作");
                    _serviceCode.Add("-SPXT", "2010腾讯游戏嘉年华线上售票系统");
                    _serviceCode.Add("-FHZG", "烽火战国");
                    _serviceCode.Add("QQYS", "QQ医生");
                    _serviceCode.Add("-AVAD", "ava点");
                    _serviceCode.Add("-GFYX", "功夫英雄");
                    _serviceCode.Add("-KLBB", "快乐宝贝");
                    _serviceCode.Add("-MTDL", "摩天大楼");
                    _serviceCode.Add("DMLQ", "大明龙权");
                    _serviceCode.Add("-QQNCPP", "QQ农场道具");
                    _serviceCode.Add("-QQHXSJ", "QQ幻想世界金子");
                    _serviceCode.Add("-QQGJD", "QQ电脑管家");
                    _serviceCode.Add("-QXZB", "七雄争霸");
                    _serviceCode.Add("XXSQQM", "超级QQ");
                    _serviceCode.Add("-HSPPDJ", "幻世&拍拍道具商城");
                    _serviceCode.Add("-TMXY", "QQ西游");
                    _serviceCode.Add("-QMMT", "签名美图");
                    _serviceCode.Add("AVAVIP", "AVA精英");
                    _serviceCode.Add("-FCPPQG", "拍拍网虚拟道具特卖商品");
                    _serviceCode.Add("LKWG", "洛克王国VIP");
                    _serviceCode.Add("-QQXXYB", "QQ仙侠传点券");
                    _serviceCode.Add("-XQWEB", "超级QQ按条产品");
                    _serviceCode.Add("-LOLDQ", "LOL点券");
                    _serviceCode.Add("-MHDL", "魔幻大陆元宝");
                    _serviceCode.Add("-TXSP", "腾讯视频单片付费");
                    _serviceCode.Add("-XJHX", "江湖笑金币");
                    _serviceCode.Add("CFTVIP", "财付通SVIP");
                    _serviceCode.Add("XYVIP", "QQ西游VIP");
                    _serviceCode.Add("-QQJX", "QQ九仙");
                    _serviceCode.Add("-CJQAT", "会员活动支付");
                    _serviceCode.Add("NBAVIP", "NBA会员");
                    _serviceCode.Add("-CHHJ", "楚河汉界");
                    _serviceCode.Add("-JJDQ", "九界点券");
                    _serviceCode.Add("XXQFHH", "红钻豪华版");
                    _serviceCode.Add("-QQFCW", "QQ飞车web道具商城");
                    _serviceCode.Add("-LOLSC", "LOLWEB商城");
                    _serviceCode.Add("-TTGM", "道聚城-tiantang道具商城");
                    _serviceCode.Add("-QCSC", "道聚城-fight道具商城");
                    _serviceCode.Add("-QQNBB", "道聚城-QQ宝贝道具商城");
                    _serviceCode.Add("-MHDLWB", "道聚城-mo道具商城");
                    _serviceCode.Add("-HXSJWB", "道聚城-幻想世界商城购物");
                    _serviceCode.Add("-YCDM", "原创动漫点券");
                    _serviceCode.Add("-QQLH", "QQ靓号");
                    _serviceCode.Add("XXZXGP", "蓝钻豪华版");
                    _serviceCode.Add("QQPACKET", "QQ钻皇");
                    _serviceCode.Add("%MZYFF", "魔钻贵族");
                    _serviceCode.Add("DSYQBY", "读书VIP-原创");
                    _serviceCode.Add("XSBY", "读书VIP-图书");
                    _serviceCode.Add("-CFTHYJF", "银行信用卡积分兑换");
                    _serviceCode.Add("TXSP", "好莱坞会员");
                }
                return _serviceCode;
            }           
        }

        static Dictionary<string, string> _serviceCode = null;

        public static Dictionary<string, string> BillState
        {
            get 
            {
                if (_billState == null)
                {
                    _billState = new Dictionary<string, string>();
                    _billState.Add("0", "未对帐");
                    _billState.Add("1", "成功交易");
                    _billState.Add("2", "待补送");
                    _billState.Add("3", "对帐补送成功");
                    _billState.Add("4", "对帐补送失败");
                    _billState.Add("5", "对帐不符(见错误码)");
                    _billState.Add("6", "补送失败后补送Q币");
                    _billState.Add("7", "手工补送成功");
                    _billState.Add("8", "手工补送失败");
                    _billState.Add("9", "客服已处理");
                    _billState.Add("10", "已退费");
                }
                return _billState;
            }
        }

        static Dictionary<string, string> _billState = null;

        public static int GetMemberDiscount(int level)
        {
            if (level < 5)
            {
                return 200;
            }
            else
            {
                return 500;
            }
        }

        public static Dictionary<string, string> AtuoRenewState
        {
            get
            {
                if (_atuoRenewState == null)
                {
                    _atuoRenewState = new Dictionary<string, string>();
                    _atuoRenewState.Add("TXSP", "好莱坞影院");
                    _atuoRenewState.Add("XXQQF", "红钻贵族");
                    _atuoRenewState.Add("WDSSX", "红钻贵族");
                    _atuoRenewState.Add("XXJZGW", "黄钻贵族");
                    _atuoRenewState.Add("LTWXCLUB", "黄钻贵族");
                    _atuoRenewState.Add("QZYXC", "黄钻贵族");
                    _atuoRenewState.Add("QWXXJZGW", "黄钻贵族");
                    _atuoRenewState.Add("XXQGAME", "蓝钻贵族");
                    _atuoRenewState.Add("YXXYQH", "蓝钻贵族");

                    _atuoRenewState.Add("XXGAMEYF", "蓝钻贵族");
                    _atuoRenewState.Add("XXZXYY", "绿钻贵族");
                    _atuoRenewState.Add("XXYYSJ", "绿钻贵族");
                    _atuoRenewState.Add("PETVIP", "粉钻贵族");
                    _atuoRenewState.Add("DNFHZ", "DNF黑钻");
                    _atuoRenewState.Add("QQR2BY", "音速紫钻");
                    _atuoRenewState.Add("QQFCZZ", "飞车紫钻");
                    _atuoRenewState.Add("XXQQT", "QQ堂紫钻");
                    _atuoRenewState.Add("LTMCLUB", "QQ会员");

                    _atuoRenewState.Add("SHSQ", "QQ会员");
                    _atuoRenewState.Add("WDYLC", "QQ会员");
                    _atuoRenewState.Add("CFCLUB", "CF会员");
                    _atuoRenewState.Add("XXLOVE", "QQ交友");
                    _atuoRenewState.Add("QQYXGZ", "音信贵族");
                    _atuoRenewState.Add("DDCLUB", "QQ会员");
                    _atuoRenewState.Add("SJCLUB", "QQ会员");
                    _atuoRenewState.Add("XXSQQ", "超级QQ");
                    _atuoRenewState.Add("XXSQQM", "超级QQ");
                    _atuoRenewState.Add("XXSQQY", "超级QQ");

                    _atuoRenewState.Add("LKWG", "洛克王国VIP");
                    _atuoRenewState.Add("AVAVIP", "AVA精英");
                    _atuoRenewState.Add("XYVIP", "QQ西游VIP");
                    _atuoRenewState.Add("XXVIP", "寻仙VIP");
                    _atuoRenewState.Add("QQXWZZ", "炫舞紫钻");
                    _atuoRenewState.Add("XXJZGHH", "豪华黄钻贵族");
                }
                return _atuoRenewState;
            }
        }

        static Dictionary<string, string> _atuoRenewState = null;

        public static Dictionary<string, string> CurTypeAI
        {
            get
            {
                if (_cur_typeAI == null)
                {
                    _cur_typeAI = new Dictionary<string, string>();
                    _cur_typeAI.Add("156", "人民币");
                    _cur_typeAI.Add("840", "美元");
                    _cur_typeAI.Add("344", "港币");
                    _cur_typeAI.Add("392", "日元");
                    _cur_typeAI.Add("826", "英镑");
                    _cur_typeAI.Add("036", "澳元");
                    _cur_typeAI.Add("554", "新西");
                    _cur_typeAI.Add("124", "加元");
                    _cur_typeAI.Add("978", "欧元");
                    _cur_typeAI.Add("756", "瑞士");
                    _cur_typeAI.Add("208", "丹朗");
                    _cur_typeAI.Add("578", "挪威克朗");
                    _cur_typeAI.Add("752", "瑞典");
                    _cur_typeAI.Add("702", "坡币");
                    _cur_typeAI.Add("764", "泰铢");
                }
                return _cur_typeAI;
            }
        }

        static Dictionary<string, string> _cur_typeAI = null;

        public static Dictionary<string, string> CurTypeAIMark
        {
            get
            {
                if (_cur_typeAIMark == null)
                {
                    _cur_typeAIMark = new Dictionary<string, string>();
                    _cur_typeAIMark.Add("156", "RMB");
                    _cur_typeAIMark.Add("840", "USD");
                    _cur_typeAIMark.Add("344", "HKD");
                    _cur_typeAIMark.Add("392", "JPY");
                    _cur_typeAIMark.Add("826", "GBP");
                    _cur_typeAIMark.Add("036", "AUD");
                    _cur_typeAIMark.Add("554", "NZD");
                    _cur_typeAIMark.Add("124", "CAD");
                    _cur_typeAIMark.Add("978", "EURO");
                    _cur_typeAIMark.Add("756", "SEK");
                    _cur_typeAIMark.Add("208", "DKK");
                    _cur_typeAIMark.Add("578", "NOK");
                    _cur_typeAIMark.Add("752", "RKS");
                    _cur_typeAIMark.Add("702", "SGD");
                    _cur_typeAIMark.Add("764", "THB");
                }
                return _cur_typeAIMark;
            }
        }

        static Dictionary<string, string> _cur_typeAIMark = null;

        public static Dictionary<string, string> CurTypeAIComment
        {
            get
            {
                if (_cur_typeAIMarkComment == null)
                {
                    _cur_typeAIMarkComment = new Dictionary<string, string>();
                    _cur_typeAIMarkComment.Add("CNY", "人民币");
                    _cur_typeAIMarkComment.Add("RMB", "人民币");
                    _cur_typeAIMarkComment.Add("USD", "美元");
                    _cur_typeAIMarkComment.Add("JPY", "日元");
                    _cur_typeAIMarkComment.Add("HKD", "港币");
                    _cur_typeAIMarkComment.Add("TWD", "台币");
                    _cur_typeAIMarkComment.Add("GBP", "英镑");
                    _cur_typeAIMarkComment.Add("AUD", "澳元");
                    _cur_typeAIMarkComment.Add("EUR", "欧元");
                    _cur_typeAIMarkComment.Add("SGD", "新加坡元");
                    _cur_typeAIMarkComment.Add("CAD", "加元");
                    _cur_typeAIMarkComment.Add("NZD", "纽元");
                    _cur_typeAIMarkComment.Add("SEK", "瑞典克朗");
                    _cur_typeAIMarkComment.Add("NOK", "挪威克朗");
                    _cur_typeAIMarkComment.Add("KRW", "韩元");
                }
                return _cur_typeAIMarkComment;
            }
        }

        static Dictionary<string, string> _cur_typeAIMarkComment = null;
    }
}