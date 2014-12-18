
var blUsePasswdCol = 0	// blUsePasswdCol 全局变量标示是否使用控件, 0 -- 使用，非0 -- 不使用
var PasswdColVersion = "1,0,0,6"	// 控件的版本号
var PasswdColClassId = "clsid:E787FD25-8D7C-4693-AE67-9406BC6E22DF";          // 控件的classid
var minSaveAmount = 0.01;     // 最小的充值金额
var maxSaveAmount = 50000;   // 最大的充值金额

/*
// 强制使用控件
//判断浏览器类型，非IE则不启动控件
var ver = navigator.appVersion;
if (ver.indexOf("MSIE") != -1)
{
	blUsePasswdCol = 0;
}else{
	blUsePasswdCol = 1;
}
*/

// 获取通用密码(rsap)
function getStrongPasswd(ctrl, seed)
{
	ctrl.SetSalt(seed);
	shap = ctrl.GetSha1Value();
	rsap = ctrl.GetRsaPassword();
	return (seed + shap + rsap);
}

// 获取字符串的字节长度
function strlen(str)
{
	var len;
	var i;
	len = 0;

	for (i=0;i<str.length;i++)
	{
		if (str.charCodeAt(i)>255) len+=2; else len++;
	}
	
	return len;
}
 

// 检查是否为数字
function checkIsInteger(str)
{
    if (str == "")
        return false;
    if (str.search(/^[0-9]+$/) < 0)
        return false;
    else
        return true;
}

// 检查是否为有效的密码，密码只允许由ascii组成，此函数只在修改或注册密码时使用
function checkValidPasswd(str)
{
	var reg = /^[\x00-\x7f]+$/;
	if (! reg.test(str))
	{
		return false;
	}
	
	if (str.length < 6 || str.length > 16)
	{
		return false;	
	}
	
	return true;
}

// 检查是否为有效的suggest
function checkValidSuggest(str)
{
	if (str.length > 256)
	{
		return false;	
	}
	
	return true;
}

// 检查是否为有效的uin
function checkUin(str)
{
    if (!checkIsInteger(str) || str.length > 15)
    {
        return false;
    }

	return true;
}

// 检查是否为中文
function isChn(str)
{
   	var reg = /^[\u4E00-\u9FA5]+$/;
   	if(!reg.test(str))
   	{
		return false;
   	}
   	
   	return true;
}

// 检查是否为有效的真实姓名，只能含有中文或大写的英文字母
function isValidTrueName(strName)
{
	var str = Trim(strName);
	  	
   	//判断是否为全英文大写或全中文，可以包含空格
   	var reg = /^[A-Z \u4E00-\u9FA5]+$/;
   	if(reg.test(str))
   	{
		return true;
   	}
   	
   	return false;
}

// 检查是否为有效的日期(如 2005-06-01)
function isDate(Date)
{
	var datetime = Date;
	var year,month,day;

    if(Date.search(/^[0-9]{4}-[0-9]{2}-[0-9]{2}$/) < 0)
	{
		return false;
	}

    year = datetime.substring(0,4);
	if (parseInt(year, 10) < 1000) 
	{
		return false;	
	}

    month = datetime.substring(5,7);
	if (parseInt(month, 10) < 1 || parseInt(month, 10) > 12) 
	{
		return false;	
	}
	
	day = datetime.substring(8, 10);
	if (parseInt(day, 10) < 1 || parseInt(day, 10) > 31)
	{
		return false;	
	}
	
	return true;
}

// 检查是否为有效的email
function checkMail(str)
{
    if(str.search(/^[^@]+@([^@]+\.)+[^@]+$/) >= 0)
        return true;
    else
        return false;
}

// 检查是否为有效的固定电话号码
function checkIsTelePhone(str)
{
    if(str.search(/^[-0-9]+$/) >= 0)
        return true;
    else
        return false;
}

// 检查是否为有效的银行类型
function checkBankType(str)
{
	if (str < 1001 || str > 1002)
	{
		return false;	
	}
	return true;
}

// 检查是否为有效的开户地区
function checkBankArea(str)
{
	if (str < 1 || str > 31)
	{
		return false;
	}
	
	return true;
}

// 检查是否为有效的开户城市
function checkBankCity(str)
{
	// 目前只检查是否为整数
	if (!checkIsInteger(str))
	{
		return false;
	}
	
	return true;
}

// 检查是否为有效的银行账号
function checkBankId(str)
{
    if(str.search(/^[\0-9]+$/g) >= 0)
        return true;
    else
        return false;
}

function checkBankIdWithType(banktype, bankid)
{
    if(bankid.search(/^[\0-9]+$/g) < 0)
    {
    	window.alert("请输入一个有效的银行卡号，只能包含数字，注意最长不能超过32个字");
        return false;
	}
	
    switch(banktype)
    {
    	case "1001":	// 招行, 12位、16位
			if (bankid.length != 12 && bankid.length!= 16)
			{
				window.alert("请输入一个有效的银行卡号，中国招商银行的卡号只能为12位或16位");
				return false;
			}
    		break;
    	case "1002":	// 工行, 19位
			if (bankid.length != 19)
			{
				window.alert("请输入一个有效的银行卡号，中国工商银行的卡号只能为19位");
				return false;
			}
    		break;
    	default: 
    		window.alert("对不起，该银行类型不支持");
    		return false;
    		break;
    }
    
    return true;
}


// 检查是否为有效的身份证
function checkIdCard(str)
{
    if(str.search(/^[0-9]+[Xx]*$/) >= 0)
        return true;
    else
        return false;
}

// 检查是否为有效的金额(包括小数点后二位)，以元为单位
// 返回值：
// 		true : 正确
//      false: 错误
function checkValidAmount(num)
{
	var len = num.length;
	
	// "." 不能出现在第一个字符及最后一个字符
	if (num.charAt(0) == '.' || num.charAt(len - 1) == '.')
		return false;
	// 小数点后最多只能包含两个有效数字(如果“.”号存在，而且它的存在位置在到数第2个以内则认为错误)
	var idx = 0;
	if ((idx = num.indexOf('.')) >= 0 && idx < len - 1 - 2)
		return false;
	// 数字开头，可以包含小数点
    if(num.search(/^[0-9]+[.]?[0-9]*$/) >= 0)
        return true;
    else
        return false;
        
    return true;
}

// 检查是否为超过充值范围
// 返回值：
//      "true" : 正确
//      其它   : 错误
function checkValidSaveAmount(num)
{
	var retVal = "true";
	if (num < minSaveAmount)
	{
		retVal = "对不起，单次充值的金额最少为" + minSaveAmount + "元";
	}
    else if (num >= maxSaveAmount)
    {
    	retVal = "对不起，单次充值的金额最大为" + maxSaveAmount + "元";
    }

	return retVal;
}

// 根据银行编号获取银行名称
// 输入：
//     bankId : 银行编号
// 输出：
//     正确时返回银行名, 错误时返回 "未定义"
function getBankNameById(bankId)
{
	var bankName = "未定义";

    switch (bankId)
    {
        case 1001:        // 招行
            bankName = "招商银行";
        	break;
        case 1002:        // 工行
        	bankName = "工商银行";
        	break;
        case 1003:        // 建行
        	bankName = "建设银行";
        	break;
        default:
            bankName = "未定义";
        	break;	
    }
    return bankName;
}

// 根据支付类型编号获取支付类型名称
// 输入：
//     typeId : 类型编号
// 输出：
//     正确时返回编号名, 错误时返回"未定义"
function getPayTypeNameByPayTypeId(typeId)
{
	var typeName = "未定义";
    switch (typeId)
    {
    	case 1:        // c2c
    		typeName = "C2C付款";
    		break;
    	case 2:        // b2c
    		typeName = "B2C付款";
    		break;
    	case 3:        // 充值
    		typeName = "充值";
    		break;
       	case 4:        // 快速交易
    		typeName = "快速交易";
    		break;
        case 5:        // 收款/付款
    		typeName = "收款/付款";
    		break;
        case 6:        // 收款/付款
    		typeName = "收款/付款";
    		break;
    	default:
    		typeName = "未定义";
    		break;	
    }
    return typeName;
}

// 根据支付类型编号获取支付类型名称
// 输入：
//     typeId : 类型编号
// 输出：
//     正确时返回编号名, 错误时返回"未定义"
function getPayTypeNameByPayTypeId2(typeId)
{
	var typeName = "未定义";
    switch (typeId)
    {
    	case 1:        // c2c
    		typeName = "C2C付款";
    		break;
    	case 2:        // b2c
    		typeName = "B2C付款";
    		break;
    	case 3:        // 快速交易
    		typeName = "快速交易";
    		break;
      case 4:        // 收款/付款
    		typeName = "收款/付款";
    		break;
      case 5:        // 收款/付款
    		typeName = "收款/付款";
    		break;
    	default:
    		typeName = "未定义";
    		break;	
    }
    return typeName;
}


// 根据支付类型编号获取交易状态名称
// 输入：
//     typeId : 类型编号
// 输出：
//     正确时返回编号名, 错误时返回"未定义"
function getPayStateNameByPayTypeId(typeid)
{
		var typeName = "未定义";
    switch (typeid)
    {
    	case 1:        // c2c
    		typeName = "等待支付";
    		break;
    	case 2:        // b2c
    		typeName = "买方支付成功";
    		break;
    	case 3:        // b2c
    		typeName = "已收到货";
    		break;
    	case 4:        // b2c
    		typeName = "交易结束";
    		break;
    	case 5:        // b2c
    		typeName = "支付失败";
    		break;
    	case 6:        // b2c
    		typeName = "给卖家打款失败";
    		break;
    	case 7:        // b2c
    		typeName = "转入退款";
    		break;
    	case 8:        
    		typeName = "等待收款方确认";
    		break;
    	case 9:        
    		typeName = "已转帐";
    		break;
    	case 10:       
    		typeName = "拒绝转帐";
    		break;
    	default:
    		typeName = "未定义";
    		break;	
    }
    return typeName;
}

// 提现状态
// 根据状态编号获取对应的名称
// 输入：
//     typeid : 状态编号
// 输出：
//     正确时返回编号名, 错误时返回 "未定义"
function getDrawingStatusNameById(typeid)
{
	var typeName = "未定义";

    // 1：成功 2：失败 3：等待付款(未导出) 4：付款中 
    switch (typeid)
    {
        case 1:       
            typeName = "提现成功";
        	break;
        case 2:       
        	typeName = "提现失败";
        	break;
        case 3:
        	typeName = "已申请";
        	break;
        case 4:
        	typeName = "已提交银行";
        	break;
        default:
            typeName = "未定义";
        	break;	
    }
    return typeName;
}

// 密码复杂度检查
// 返回值
//  "true" 	检查通过
//	其它	错误信息
function checkPasswd(passwd)
{
	// 检查passwd的长度是否大于8
	var len = passwd.length;
	if (len < 8 || len > 16) {
        return "密码长度不能小于8位，不能大于16位";
	}

	// 检查密码是否为纯数字
	if(passwd.search(/^[0-9]*$/) >= 0)
		return "密码不能为纯数字，推荐字母(区分大小写)和数字等相结合的方式";

	// 检查密码是否为纯小写字母
	if(passwd.search(/^[a-z]*$/) >= 0)
		return "密码不能为纯字母，推荐字母(区分大小写)和数字等相结合的方式";

	// 检查密码是吉伯为纯大写字母
	if(passwd.search(/^[A-Z]*$/) >= 0)
		return "密码不能为纯字母，推荐字母(区分大小写)和数字等相结合的方式";

	return "true";
}


// 跳转
function jump(surl)
{
	//window.alert("跳转链接:" + surl);
    surl = surl.replace(/=&/g, '%3d&');         // 对于含=的值，需要转换成%3d
    surl = surl.replace(/==&/g, '%3d&');        // 对于含=的值，需要转换成%3d
	// 如果没有指定跳转链接，则后退
	if (surl == "back")
	{
		history.back(-1);
		return;
	} 
	else if (surl == "close")
    {
		self.close();
		return;
    }
	
	// 跳转到指定链接
	//window.location.href= surl;
    location.replace(surl)

	return;
}

/*
==================================================================
LTrim(string):去除左边的空格
==================================================================
*/
function LTrim(str)
{
    var whitespace = new String(" \t\n\r");
    var s = new String(str);

    if (whitespace.indexOf(s.charAt(0)) != -1)
    {
        var j=0, i = s.length;
        while (j < i && whitespace.indexOf(s.charAt(j)) != -1)
        {
            j++;
        }
        s = s.substring(j, i);
    }
    return s;
}

/*
==================================================================
RTrim(string):去除右边的空格
==================================================================
*/
function RTrim(str)
{
    var whitespace = new String(" \t\n\r");
    var s = new String(str);
    
    if (whitespace.indexOf(s.charAt(s.length-1)) != -1)
    {
        var i = s.length - 1;
        while (i >= 0 && whitespace.indexOf(s.charAt(i)) != -1)
        {
            i--;
        }
        s = s.substring(0, i+1);
    }
    return s;
}

/*
==================================================================
Trim(string):去除前后空格
==================================================================
*/
function Trim(str)
{
    return RTrim(LTrim(str));
}



/*
==================================================================
省份编码及城市编码处理
==================================================================
*/
var where = new Array(31); 
function comefrom(loca, locacity, locaid, locacityids) 
{ 
	this.loca = loca; 
	this.locacity = locacity;
	this.locaid = locaid;
	this.locacityids = locacityids;
}

where[0] = new comefrom("请选择省份名","请选择城市名", "", "");
where[1] = new comefrom("北京", "北京", "1", "10"); 
where[2] = new comefrom("上海", "上海", "2", "21"); 
where[3] = new comefrom("天津", "天津", "3", "22");
where[4] = new comefrom("重庆", "重庆", "4", "23"); 
where[5] = new comefrom("河北", "石家庄|张家口|承德|秦皇岛|唐山|廊坊|保定|沧州|衡水|邢台|邯郸", "5", "311|313|314|335|315|316|312|317|318|319|310"); 
where[6] = new comefrom("山西", "太原|大同|朔州|阳泉|长治|晋城|忻州|离石|榆次|临汾|运城", "6", "351|352|349|353|355|356|350|358|354|357|359"); 
where[7] = new comefrom("内蒙古", "呼和浩特(*)|包头|乌海|赤峰|海拉尔|乌兰浩特|通辽|锡林浩特|集宁|东胜|临河|阿拉善左旗", "7", "471|472|473|476|470|482|475|479|474|477|478|483"); 
where[8] = new comefrom("辽宁", "沈阳(*)|朝阳|阜新|铁岭|抚顺|本溪|辽阳|鞍山|丹东|大连|营口|盘锦|锦州|葫芦岛", "8", "24|421|418|410|413|414|419|412|415|411|417|427|416|429"); 
where[9] = new comefrom("吉林", "长春(*)|白城|松原|吉林|四平|辽源|通化|白山|延吉", "9", "431|436|438|432|434|437|435|439|433"); 
where[10] = new comefrom("黑龙江", "哈尔滨(*)|齐齐哈尔|黑河|大庆|伊春|鹤岗|佳木斯|双鸭山|七台河|鸡西|牡丹江|绥化|加格达奇", "10", "451|452|456|459|458|468|454|469|464|467|453|455|457"); 
where[11] = new comefrom("江苏", "南京(*)|苏州|徐州|连云港|宿迁|淮安|盐城|扬州|泰州|南通|镇江|常州|无锡", "11", "25|512|516|518|527|517|515|514|523|513|511|519|510"); 
where[12] = new comefrom("浙江", "杭州(*)|湖州|嘉兴|舟山|宁波|绍兴|金华|台州|温州|丽水|衢州", "12", "571|572|573|580|574|575|579|576|577|578|570"); 
where[13] = new comefrom("安徽", "合肥(*)|宿州|淮北|阜阳|蚌埠|淮南|滁州|马鞍山|芜湖|铜陵|安庆|黄山|六安|巢湖|贵池|宣城", "13", "551|557|561|558|552|554|550|555|553|562|556|559|564|565|566|563"); 
where[14] = new comefrom("福建", "福州(*)|南平|三明|莆田|泉州|厦门|漳州|龙岩|宁德|福安|邵武|石狮|永安|武夷山", "14", "591|599|598|594|595|592|596|597|593|5930|5990|5950|5980|5991"); 
where[15] = new comefrom("江西", "南昌(*)|九江|景德镇|鹰潭|新余|萍乡|赣州|上饶|临川|宜春|吉安|抚州", "15", "791|792|798|701|790|799|797|793|794|795|796|7940"); 
where[16] = new comefrom("山东", "济南(*)|聊城|德州|东营|淄博|潍坊|烟台|威海|青岛|日照|临沂|枣庄|济宁|泰安|莱芜|滨州|菏泽", "16", "531|635|534|546|533|536|535|631|532|633|539|632|537|538|634|543|530"); 
where[17] = new comefrom("河南", "郑州(*)|三门峡|洛阳|焦作|新乡|鹤壁|安阳|濮阳|开封|商丘|许昌|漯河|平顶山|南阳|信阳|济源|周口|驻马店", "17", "371|398|379|391|373|392|372|393|378|370|374|395|375|377|376|3910|394|396"); 
where[18] = new comefrom("湖北", "武汉(*)|十堰|襄樊|荆门|孝感|黄冈|鄂州|黄石|咸宁|荆州|宜昌|恩施|仙桃|潜江|天门", "18", "27|719|710|724|712|713|711|714|715|716|717|718|728|7281|7282");
where[19] = new comefrom("湖南", "长沙(*)|张家界|常德|益阳|岳阳|株洲|湘潭|衡阳|郴州|永州|邵阳|怀化|娄底|吉首", "19", "731|744|736|737|730|733|732|734|735|746|739|745|738|743"); 
where[20] = new comefrom("广东", "广州(*)|深圳|清远|韶关|河源|梅州|潮州|汕头|揭阳|汕尾|惠州|东莞|珠海|中山|江门|佛山|茂名|湛江|阳江|云浮|肇庆", "20", "20|755|763|751|762|753|768|754|663|660|752|769|756|760|750|757|668|759|662|766|758"); 
where[21] = new comefrom("广西", "南宁(*)|桂林|柳州|贺州|玉林|钦州|北海|防城港|百色|河池|贵港|梧州", "21", "771|773|772|774|775|777|779|770|776|778|7750|7740"); 
where[22] = new comefrom("海南", "海口(*)|三亚|儋州", "22", "898|899|890"); 
where[23] = new comefrom("四川", "成都(*)|广元|绵阳|德阳|南充|广安|遂宁|内江|乐山|自贡|泸州|宜宾|攀枝花|巴中|达州|资阳|雅安|西昌", "23", "28|839|816|838|817|826|825|832|833|813|830|831|812|827|818|8320|835|834"); 
where[24] = new comefrom("贵州", "贵阳(*)|六盘水|遵义|毕节|铜仁|安顺|凯里|都匀|兴义", "24", "851|858|852|857|856|853|855|854|859"); 
where[25] = new comefrom("云南", "昆明(*)|曲靖|玉溪|丽江|昭通|思茅|临沧|保山|潞西|泸水|中甸|大理|楚雄|个旧|文山|景洪|红河", "25", "871|874|877|888|870|879|883|875|692|886|887|872|878|873|876|691|8730");
where[26] = new comefrom("西藏", "拉萨(*)|那曲|昌都|林芝|乃东|日喀则|噶尔", "26", "891|896|895|894|893|892|897"); 
where[27] = new comefrom("陕西", "西安(*)|延安|铜川|渭南|咸阳|宝鸡|汉中|榆林|商洛|安康", "27", "29|911|919|913|910|917|916|912|914|915"); 
where[28] = new comefrom("甘肃", "兰州(*)|嘉峪关|白银|天水|酒泉|张掖|金昌|西峰|平凉|定西|陇南|临夏|武威", "28", "931|937|943|938|9370|936|935|934|933|932|939|930|9350"); 
where[29] = new comefrom("宁夏", "银川(*)|石嘴山|吴忠|固原", "29", "951|952|953|954"); 
where[30] = new comefrom("青海", "西宁(*)|平安|海晏|共和|同仁|玛沁|玉树|德令哈", "30", "971|972|970|974|973|975|976|977");
where[31] = new comefrom("新疆", "乌鲁木齐(*)|克拉玛依|石河子|喀什|阿克苏|和田|吐鲁番|哈密|阿图什|博乐|昌吉|库尔勒|伊犁|奎屯|塔城|阿勒泰|五家渠", "31", "991|990|993|998|997|903|995|902|908|909|994|996|999|992|901|906|940"); 
