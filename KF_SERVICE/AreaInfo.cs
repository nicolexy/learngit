using System;
using System.Configuration;
using TENCENT.OSS.CFT.KF.DataAccess;
using TENCENT.OSS.CFT.KF.Common;
using System.Data;
using System.Collections;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
//using System.Web.UI.HtmlControls;

namespace TENCENT.OSS.CFT.KF.KF_Service
{
	/// <summary>
	/// 地区处理类。
	/// </summary>
	public class AreaInfo
	{
		private static comefrom[] where = new comefrom[32];

		public static void GetAllArea(DropDownList ddlarea)
		{
			ddlarea.Items.Clear();
			foreach(comefrom cf in where)
			{
				ddlarea.Items.Add(new ListItem(cf.longloca,cf.locaid));
			}
		}

		public static void GetAllCity(long areaid, DropDownList ddlcity)
		{
			ddlcity.Items.Clear();

			if(areaid <0 || areaid > 31)
				return;

			string tmp = where[areaid].locacityids;
			string[] tmps = tmp.Split('|');

			string tmp2 = where[areaid].locacity;
			string[] tmps2 = tmp2.Split('|');
			
			for(int i = 0; i < tmps.Length; i++)
			{
				ddlcity.Items.Add(new ListItem(tmps2[i],tmps[i]));
			}
			
		}

		/// <summary>
		/// 获取省的简称名字.
		/// </summary>
		/// <param name="code">省的ID</param>
		/// <returns>省的简称</returns>
		public static string GetAreaName(long code)
		{
			return where[code].loca;
		}

		/// <summary>
		/// 获取省的长名字
		/// </summary>
		/// <param name="code">省的ID</param>
		/// <returns>省的长名</returns>
		public static string GetAreaName_Long(long code)
		{
			try
			{
				return where[code].longloca;
			}
			catch
			{
				return "未知" + code;
			}
		}

		/// <summary>
		/// 获取地区的简称
		/// </summary>
		/// <param name="code">省ID</param>
		/// <param name="citycode">地区ID</param>
		/// <returns>地区简称</returns>
		public static string GetCityName(long code, string citycode)
		{
			if(code == 0 || citycode == null || citycode.Trim() == "")
			{
				return "";
			}

			string tmp = where[code].locacityids;
			string[] tmps = tmp.Split('|');

			string tmp2 = where[code].locacity;
			string[] tmps2 = tmp2.Split('|');
			
			for(int i = 0; i < tmps.Length; i++)
			{
				if(tmps[i].Trim() == citycode.Trim())
					return tmps2[i];
			}

			return "";
		}

		/// <summary>
		/// 获取地区的长名
		/// </summary>
		/// <param name="code">省ID</param>
		/// <param name="citycode">地区ID</param>
		/// <returns>地区长名</returns>
		public static string GetCityName_Long(long code,string citycode)
		{
			try
			{
				if(code == 0 || citycode == null || citycode.Trim() == "" || citycode.Trim() == "--")
				{
					return "";
				}

				string tmp = where[code].locacityids;
				string[] tmps = tmp.Split('|');

				string tmp2 = where[code].longlocacity;
				string[] tmps2 = tmp2.Split('|');
			
				for(int i = 0; i < tmps.Length; i++)
				{
					if(tmps[i].Trim() == citycode.Trim())
						return tmps2[i];
				}

				return "";
			}
			catch
			{
				return "未知" + code + "!" + citycode;
			}
		}


		/// <summary>
		/// 获取省简称+地区简称
		/// </summary>
		/// <param name="code">省ID</param>
		/// <param name="citycode">地区ID</param>
		/// <returns>省简称+地区简称</returns>
		public static string GetAllName(long code, string citycode)
		{
			if(code == 0 )
			{
				return "";
			}

			return GetAreaName_Long(code) + GetCityName_Long(code,citycode);
		}

		static AreaInfo()
		{
			where[0] = new comefrom("请选择省份名","","请选择城市名","", "", "");
			where[1] = new comefrom("北京","北京市", "北京","北京市", "1", "10"); 
			where[2] = new comefrom("上海","上海市", "上海", "上海市","2", "21"); 
			where[3] = new comefrom("天津","天津市", "天津","天津市", "3", "22");
			where[4] = new comefrom("重庆","重庆市", "重庆","重庆市", "4", "23"); 
			where[5] = new comefrom("河北", "河北省","石家庄|张家口|承德|秦皇岛|唐山|廊坊|保定|沧州|衡水|邢台|邯郸", 
				"石家庄市|张家口市|承德市|秦皇岛市|唐山市|廊坊市|保定市|沧州市|衡水市|邢台市|邯郸市",
				"5", "311|313|314|335|315|316|312|317|318|319|310"); 
			where[6] = new comefrom("山西","山西省", "太原|大同|朔州|阳泉|长治|晋城|忻州|离石|榆次|临汾|运城", 
				"太原市|大同市|朔州市|阳泉市|长治市|晋城市|忻州市|离石市|榆次市|临汾市|运城市",
				"6", "351|352|349|353|355|356|350|358|354|357|359"); 

			where[7] = new comefrom("内蒙古","内蒙古自治区" ,"呼和浩特|包头|乌海|赤峰|海拉尔|乌兰浩特|通辽|锡林浩特|集宁|东胜|临河|阿拉善左旗", 
				"呼和浩特市|包头市|乌海市|赤峰市|海拉尔市|乌兰浩特市|通辽市|锡林浩特市|集宁市|东胜市|临河市|阿拉善左旗",
				"7", "471|472|473|476|470|482|475|479|474|477|478|483"); 

			where[8] = new comefrom("辽宁","辽宁省", "沈阳|朝阳|阜新|铁岭|抚顺|本溪|辽阳|鞍山|丹东|大连|营口|盘锦|锦州|葫芦岛", 
				"沈阳市|朝阳市|阜新市|铁岭市|抚顺市|本溪市|辽阳市|鞍山市|丹东市|大连市|营口市|盘锦市|锦州市|葫芦岛市", 
				"8", "24|421|418|410|413|414|419|412|415|411|417|427|416|429"); 

			where[9] = new comefrom("吉林","吉林省", "长春|白城|松原|吉林|四平|辽源|通化|白山|延吉", 
				"长春市|白城市|松原市|吉林市|四平市|辽源市|通化市|白山市|延吉市",
				"9", "431|436|438|432|434|437|435|439|433"); 

			where[10] = new comefrom("黑龙江","黑龙江省", "哈尔滨|齐齐哈尔|黑河|大庆|伊春|鹤岗|佳木斯|双鸭山|七台河|鸡西|牡丹江|绥化|加格达奇", 
				"哈尔滨市|齐齐哈尔市|黑河市|大庆市|伊春市|鹤岗市|佳木斯市|双鸭山市|七台河市|鸡西市|牡丹江市|绥化市|加格达奇市",
				"10", "451|452|456|459|458|468|454|469|464|467|453|455|457"); 

			where[11] = new comefrom("江苏","江苏省", "南京|苏州|徐州|连云港|宿迁|淮安|盐城|扬州|泰州|南通|镇江|常州|无锡", 
				"南京市|苏州市|徐州市|连云港市|宿迁市|淮安市|盐城市|扬州市|泰州市|南通市|镇江市|常州市|无锡市",
				"11", "25|512|516|518|527|517|515|514|523|513|511|519|510"); 

			where[12] = new comefrom("浙江","浙江省", "杭州|湖州|嘉兴|舟山|宁波|绍兴|金华|台州|温州|丽水|衢州",
				"杭州市|湖州市|嘉兴市|舟山市|宁波市|绍兴市|金华市|台州市|温州市|丽水市|衢州市",
				"12", "571|572|573|580|574|575|579|576|577|578|570"); 

			where[13] = new comefrom("安徽","安徽省", "合肥|宿州|淮北|阜阳|蚌埠|淮南|滁州|马鞍山|芜湖|铜陵|安庆|黄山|六安|巢湖|贵池|宣城",
				"合肥市|宿州市|淮北市|阜阳市|蚌埠市|淮南市|滁州市|马鞍山市|芜湖市|铜陵市|安庆市|黄山市|六安市|巢湖市|贵池市|宣城市",
				"13", "551|557|561|558|552|554|550|555|553|562|556|559|564|565|566|563"); 

			where[14] = new comefrom("福建","福建省", "福州|南平|三明|莆田|泉州|厦门|漳州|龙岩|宁德|福安|邵武|石狮|永安|武夷山|福清", 
				"福州市|南平市|三明市|莆田市|泉州市|厦门市|漳州市|龙岩市|宁德市|福安市|邵武市|石狮市|永安市|武夷山市|福清市", 
				"14", "591|599|598|594|595|592|596|597|593|5930|5990|5950|5980|5991|5995"); 

			where[15] = new comefrom("江西","江西省", "南昌|九江|景德镇|鹰潭|新余|萍乡|赣州|上饶|临川|宜春|吉安|抚州", 
				"南昌市|九江市|景德镇市|鹰潭市|新余市|萍乡市|赣州市|上饶市|临川市|宜春市|吉安市|抚州市",
				"15", "791|792|798|701|790|799|797|793|794|795|796|7940"); 

			where[16] = new comefrom("山东","山东省", "济南|聊城|德州|东营|淄博|潍坊|烟台|威海|青岛|日照|临沂|枣庄|济宁|泰安|莱芜|滨州|菏泽", 
				"济南市|聊城市|德州市|东营市|淄博市|潍坊市|烟台市|威海市|青岛市|日照市|临沂市|枣庄市|济宁市|泰安市|莱芜市|滨州市|菏泽市",
				"16", "531|635|534|546|533|536|535|631|532|633|539|632|537|538|634|543|530"); 

			where[17] = new comefrom("河南","河南省", "郑州|三门峡|洛阳|焦作|新乡|鹤壁|安阳|濮阳|开封|商丘|许昌|漯河|平顶山|南阳|信阳|济源|周口|驻马店", "郑州市|三门峡市|洛阳市|焦作市|新乡市|鹤壁市|安阳市|濮阳市|开封市|商丘市|许昌市|漯河市|平顶山市|南阳市|信阳市|济源市|周口市|驻马店市", 
				"17", "371|398|379|391|373|392|372|393|378|370|374|395|375|377|376|3910|394|396"); 

			where[18] = new comefrom("湖北","湖北省", "武汉|十堰|襄樊|荆门|孝感|黄冈|鄂州|黄石|咸宁|荆州|宜昌|恩施|仙桃|潜江|天门", 
				"武汉市|十堰市|襄樊市|荆门市|孝感市|黄冈市|鄂州市|黄石市|咸宁市|荆州市|宜昌市|恩施市|仙桃市|潜江市|天门市",
				"18", "27|719|710|724|712|713|711|714|715|716|717|718|728|7281|7282");

			where[19] = new comefrom("湖南","湖南省", "长沙|张家界|常德|益阳|岳阳|株洲|湘潭|衡阳|郴州|永州|邵阳|怀化|娄底|吉首", 
				"长沙市|张家界市|常德市|益阳市|岳阳市|株洲市|湘潭市|衡阳市|郴州市|永州市|邵阳市|怀化市|娄底市|吉首市", 
				"19", "731|744|736|737|730|733|732|734|735|746|739|745|738|743"); 

			where[20] = new comefrom("广东","广东省" ,"广州|深圳|清远|韶关|河源|梅州|潮州|汕头|揭阳|汕尾|惠州|东莞|珠海|中山|江门|佛山|茂名|湛江|阳江|云浮|肇庆", "广州市|深圳市|清远市|韶关市|河源市|梅州市|潮州市|汕头市|揭阳市|汕尾市|惠州市|东莞市|珠海市|中山市|江门市|佛山市|茂名市|湛江市|阳江市|云浮市|肇庆市", 
				"20", "20|755|763|751|762|753|768|754|663|660|752|769|756|760|750|757|668|759|662|766|758"); 

			where[21] = new comefrom("广西","广西自治区", "南宁|桂林|柳州|贺州|玉林|钦州|北海|防城港|百色|河池|贵港|梧州", 
				"南宁市|桂林市|柳州市|贺州市|玉林市|钦州市|北海市|防城港市|百色市|河池市|贵港市|梧州市", 
				"21", "771|773|772|774|775|777|779|770|776|778|7750|7740"); 

			where[22] = new comefrom("海南","海南省", "海口|三亚|儋州","海口市|三亚市|儋州市", "22", "898|899|890"); 

			where[23] = new comefrom("四川","四川省", "成都|广元|绵阳|德阳|南充|广安|遂宁|内江|乐山|自贡|泸州|宜宾|攀枝花|巴中|达州|资阳|雅安|西昌", 
				"成都市|广元市|绵阳市|德阳市|南充市|广安市|遂宁市|内江市|乐山市|自贡市|泸州市|宜宾市|攀枝花市|巴中市|达州市|资阳市|雅安市|西昌市",
				"23", "28|839|816|838|817|826|825|832|833|813|830|831|812|827|818|8320|835|834"); 

			where[24] = new comefrom("贵州","贵州省", "贵阳|六盘水|遵义|毕节|铜仁|安顺|凯里|都匀|兴义", 
				"贵阳市|六盘水市|遵义市|毕节市|铜仁市|安顺市|凯里市|都匀市|兴义市", 
				"24", "851|858|852|857|856|853|855|854|859"); 

			where[25] = new comefrom("云南","云南省", "昆明|曲靖|玉溪|丽江|昭通|思茅|临沧|保山|潞西|泸水|中甸|大理|楚雄|个旧|文山|景洪|红河", 
				"昆明市|曲靖市|玉溪市|丽江市|昭通市|思茅市|临沧市|保山市|潞西市|泸水县|中甸县|大理市|楚雄市|个旧市|文山州|景洪市|红河州",
				"25", "871|874|877|888|870|879|883|875|692|886|887|872|878|873|876|691|8730");

			where[26] = new comefrom("西藏","西藏自治区", "拉萨|那曲|昌都|林芝|乃东|日喀则|噶尔", 
				"拉萨市|那曲地区|昌都地区|林芝地区|乃东县|日喀则市|噶尔县",
				"26", "891|896|895|894|893|892|897"); 

			where[27] = new comefrom("陕西","陕西省", "西安|延安|铜川|渭南|咸阳|宝鸡|汉中|榆林|商洛|安康", 
				"西安市|延安市|铜川市|渭南市|咸阳市|宝鸡市|汉中市|榆林市|商洛市|安康市",
				"27", "29|911|919|913|910|917|916|912|914|915"); 

			where[28] = new comefrom("甘肃","甘肃省", "兰州|嘉峪关|白银|天水|酒泉|张掖|金昌|西峰|平凉|定西|陇南|临夏", 
				"兰州市|嘉峪关市|白银市|天水市|酒泉市|张掖市|金昌市|西峰市|平凉市|定西市|陇南市|临夏市", 
				"28", "931|937|943|938|9370|936|935|934|933|932|939|930"); 

			//where[29] = new comefrom("宁夏","宁夏自治区","银川市|石嘴山市|吴忠市|固原市","银川|石嘴山|吴忠|固原", "29", "951|952|953|954"); 
			where[29] = new comefrom("宁夏","宁夏自治区","银川|石嘴山|吴忠|固原","银川市|石嘴山市|吴忠市|固原市", "29", "951|952|953|954"); 

			where[30] = new comefrom("青海","青海省", "西宁|平安|海晏|共和|同仁|玛沁|玉树|德令哈", 
				"西宁市|平安县|海晏县|共和县|同仁县|玛沁县|玉树州|德令哈市", 
				"30", "971|972|970|974|973|975|976|977");

            where[31] = new comefrom("新疆", "新疆自治区", "乌鲁木齐|克拉玛依|石河子|喀什|阿克苏|和田|吐鲁番|哈密|阿图什|博乐|昌吉|库尔勒|伊犁|奎屯|塔城|阿勒泰|五家渠", "乌鲁木齐市|克拉玛依市|石河子市|喀什市|阿克苏市|和田地区|吐鲁番地区|哈密市|阿图什市|博乐市|昌吉州|库尔勒市|伊犁州|奎屯市|塔城地区|阿勒泰地区|五家渠市", 
				"31", "991|990|993|998|997|903|995|902|908|909|994|996|999|992|901|906|940"); 

		}
	}

	/// <summary>
	/// 一个省的所有信息.
	/// </summary>
	internal class comefrom
	{
		//loca, locacity, locaid, locacityids

		/// <summary>
		/// 省的简称
		/// </summary>
		public string loca = "";

		/// <summary>
		/// 省的长名字
		/// </summary>
		public string longloca = "";
		
		/// <summary>
		/// 市县的简称串
		/// </summary>
		public string locacity = "";

		/// <summary>
		/// 市县的长名串.
		/// </summary>
		public string longlocacity = "";

		/// <summary>
		/// 省ID
		/// </summary>
		public string locaid = "";

		/// <summary>
		/// 市ID串
		/// </summary>
		public string locacityids = "";

		/// <summary>
		/// 解析JS中的地址串
		/// </summary>
		/// <param name="aloca">省简称</param>
		/// <param name="alongloca">省的全名</param>
		/// <param name="alocacity">城市简称</param>
		/// <param name="alonglocacity">城市全名</param>
		/// <param name="alocaid">省代码</param>
		/// <param name="alocacityids">城市代码</param>
		public comefrom(string aloca, string alongloca, string alocacity, string alonglocacity, string alocaid, string alocacityids)
		{
			loca = aloca;
			longloca = alongloca;

			locacity = alocacity;
			longlocacity = alonglocacity;

			locaid = alocaid;
			locacityids = alocacityids;
		}
	}
}
