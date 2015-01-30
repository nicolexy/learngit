
var blUsePasswdCol = 0	// blUsePasswdCol ȫ�ֱ�����ʾ�Ƿ�ʹ�ÿؼ�, 0 -- ʹ�ã���0 -- ��ʹ��
var PasswdColVersion = "1,0,0,6"	// �ؼ��İ汾��
var PasswdColClassId = "clsid:E787FD25-8D7C-4693-AE67-9406BC6E22DF";          // �ؼ���classid
var minSaveAmount = 0.01;     // ��С�ĳ�ֵ���
var maxSaveAmount = 50000;   // ���ĳ�ֵ���

/*
// ǿ��ʹ�ÿؼ�
//�ж���������ͣ���IE�������ؼ�
var ver = navigator.appVersion;
if (ver.indexOf("MSIE") != -1)
{
	blUsePasswdCol = 0;
}else{
	blUsePasswdCol = 1;
}
*/

// ��ȡͨ������(rsap)
function getStrongPasswd(ctrl, seed)
{
	ctrl.SetSalt(seed);
	shap = ctrl.GetSha1Value();
	rsap = ctrl.GetRsaPassword();
	return (seed + shap + rsap);
}

// ��ȡ�ַ������ֽڳ���
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
 

// ����Ƿ�Ϊ����
function checkIsInteger(str)
{
    if (str == "")
        return false;
    if (str.search(/^[0-9]+$/) < 0)
        return false;
    else
        return true;
}

// ����Ƿ�Ϊ��Ч�����룬����ֻ������ascii��ɣ��˺���ֻ���޸Ļ�ע������ʱʹ��
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

// ����Ƿ�Ϊ��Ч��suggest
function checkValidSuggest(str)
{
	if (str.length > 256)
	{
		return false;	
	}
	
	return true;
}

// ����Ƿ�Ϊ��Ч��uin
function checkUin(str)
{
    if (!checkIsInteger(str) || str.length > 15)
    {
        return false;
    }

	return true;
}

// ����Ƿ�Ϊ����
function isChn(str)
{
   	var reg = /^[\u4E00-\u9FA5]+$/;
   	if(!reg.test(str))
   	{
		return false;
   	}
   	
   	return true;
}

// ����Ƿ�Ϊ��Ч����ʵ������ֻ�ܺ������Ļ��д��Ӣ����ĸ
function isValidTrueName(strName)
{
	var str = Trim(strName);
	  	
   	//�ж��Ƿ�ΪȫӢ�Ĵ�д��ȫ���ģ����԰����ո�
   	var reg = /^[A-Z \u4E00-\u9FA5]+$/;
   	if(reg.test(str))
   	{
		return true;
   	}
   	
   	return false;
}

// ����Ƿ�Ϊ��Ч������(�� 2005-06-01)
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

// ����Ƿ�Ϊ��Ч��email
function checkMail(str)
{
    if(str.search(/^[^@]+@([^@]+\.)+[^@]+$/) >= 0)
        return true;
    else
        return false;
}

// ����Ƿ�Ϊ��Ч�Ĺ̶��绰����
function checkIsTelePhone(str)
{
    if(str.search(/^[-0-9]+$/) >= 0)
        return true;
    else
        return false;
}

// ����Ƿ�Ϊ��Ч����������
function checkBankType(str)
{
	if (str < 1001 || str > 1002)
	{
		return false;	
	}
	return true;
}

// ����Ƿ�Ϊ��Ч�Ŀ�������
function checkBankArea(str)
{
	if (str < 1 || str > 31)
	{
		return false;
	}
	
	return true;
}

// ����Ƿ�Ϊ��Ч�Ŀ�������
function checkBankCity(str)
{
	// Ŀǰֻ����Ƿ�Ϊ����
	if (!checkIsInteger(str))
	{
		return false;
	}
	
	return true;
}

// ����Ƿ�Ϊ��Ч�������˺�
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
    	window.alert("������һ����Ч�����п��ţ�ֻ�ܰ������֣�ע������ܳ���32����");
        return false;
	}
	
    switch(banktype)
    {
    	case "1001":	// ����, 12λ��16λ
			if (bankid.length != 12 && bankid.length!= 16)
			{
				window.alert("������һ����Ч�����п��ţ��й��������еĿ���ֻ��Ϊ12λ��16λ");
				return false;
			}
    		break;
    	case "1002":	// ����, 19λ
			if (bankid.length != 19)
			{
				window.alert("������һ����Ч�����п��ţ��й��������еĿ���ֻ��Ϊ19λ");
				return false;
			}
    		break;
    	default: 
    		window.alert("�Բ��𣬸��������Ͳ�֧��");
    		return false;
    		break;
    }
    
    return true;
}


// ����Ƿ�Ϊ��Ч�����֤
function checkIdCard(str)
{
    if(str.search(/^[0-9]+[Xx]*$/) >= 0)
        return true;
    else
        return false;
}

// ����Ƿ�Ϊ��Ч�Ľ��(����С������λ)����ԪΪ��λ
// ����ֵ��
// 		true : ��ȷ
//      false: ����
function checkValidAmount(num)
{
	var len = num.length;
	
	// "." ���ܳ����ڵ�һ���ַ������һ���ַ�
	if (num.charAt(0) == '.' || num.charAt(len - 1) == '.')
		return false;
	// С��������ֻ�ܰ���������Ч����(�����.���Ŵ��ڣ��������Ĵ���λ���ڵ�����2����������Ϊ����)
	var idx = 0;
	if ((idx = num.indexOf('.')) >= 0 && idx < len - 1 - 2)
		return false;
	// ���ֿ�ͷ�����԰���С����
    if(num.search(/^[0-9]+[.]?[0-9]*$/) >= 0)
        return true;
    else
        return false;
        
    return true;
}

// ����Ƿ�Ϊ������ֵ��Χ
// ����ֵ��
//      "true" : ��ȷ
//      ����   : ����
function checkValidSaveAmount(num)
{
	var retVal = "true";
	if (num < minSaveAmount)
	{
		retVal = "�Բ��𣬵��γ�ֵ�Ľ������Ϊ" + minSaveAmount + "Ԫ";
	}
    else if (num >= maxSaveAmount)
    {
    	retVal = "�Բ��𣬵��γ�ֵ�Ľ�����Ϊ" + maxSaveAmount + "Ԫ";
    }

	return retVal;
}

// �������б�Ż�ȡ��������
// ���룺
//     bankId : ���б��
// �����
//     ��ȷʱ����������, ����ʱ���� "δ����"
function getBankNameById(bankId)
{
	var bankName = "δ����";

    switch (bankId)
    {
        case 1001:        // ����
            bankName = "��������";
        	break;
        case 1002:        // ����
        	bankName = "��������";
        	break;
        case 1003:        // ����
        	bankName = "��������";
        	break;
        default:
            bankName = "δ����";
        	break;	
    }
    return bankName;
}

// ����֧�����ͱ�Ż�ȡ֧����������
// ���룺
//     typeId : ���ͱ��
// �����
//     ��ȷʱ���ر����, ����ʱ����"δ����"
function getPayTypeNameByPayTypeId(typeId)
{
	var typeName = "δ����";
    switch (typeId)
    {
    	case 1:        // c2c
    		typeName = "C2C����";
    		break;
    	case 2:        // b2c
    		typeName = "B2C����";
    		break;
    	case 3:        // ��ֵ
    		typeName = "��ֵ";
    		break;
       	case 4:        // ���ٽ���
    		typeName = "���ٽ���";
    		break;
        case 5:        // �տ�/����
    		typeName = "�տ�/����";
    		break;
        case 6:        // �տ�/����
    		typeName = "�տ�/����";
    		break;
    	default:
    		typeName = "δ����";
    		break;	
    }
    return typeName;
}

// ����֧�����ͱ�Ż�ȡ֧����������
// ���룺
//     typeId : ���ͱ��
// �����
//     ��ȷʱ���ر����, ����ʱ����"δ����"
function getPayTypeNameByPayTypeId2(typeId)
{
	var typeName = "δ����";
    switch (typeId)
    {
    	case 1:        // c2c
    		typeName = "C2C����";
    		break;
    	case 2:        // b2c
    		typeName = "B2C����";
    		break;
    	case 3:        // ���ٽ���
    		typeName = "���ٽ���";
    		break;
      case 4:        // �տ�/����
    		typeName = "�տ�/����";
    		break;
      case 5:        // �տ�/����
    		typeName = "�տ�/����";
    		break;
    	default:
    		typeName = "δ����";
    		break;	
    }
    return typeName;
}


// ����֧�����ͱ�Ż�ȡ����״̬����
// ���룺
//     typeId : ���ͱ��
// �����
//     ��ȷʱ���ر����, ����ʱ����"δ����"
function getPayStateNameByPayTypeId(typeid)
{
		var typeName = "δ����";
    switch (typeid)
    {
    	case 1:        // c2c
    		typeName = "�ȴ�֧��";
    		break;
    	case 2:        // b2c
    		typeName = "��֧���ɹ�";
    		break;
    	case 3:        // b2c
    		typeName = "���յ���";
    		break;
    	case 4:        // b2c
    		typeName = "���׽���";
    		break;
    	case 5:        // b2c
    		typeName = "֧��ʧ��";
    		break;
    	case 6:        // b2c
    		typeName = "�����Ҵ��ʧ��";
    		break;
    	case 7:        // b2c
    		typeName = "ת���˿�";
    		break;
    	case 8:        
    		typeName = "�ȴ��տȷ��";
    		break;
    	case 9:        
    		typeName = "��ת��";
    		break;
    	case 10:       
    		typeName = "�ܾ�ת��";
    		break;
    	default:
    		typeName = "δ����";
    		break;	
    }
    return typeName;
}

// ����״̬
// ����״̬��Ż�ȡ��Ӧ������
// ���룺
//     typeid : ״̬���
// �����
//     ��ȷʱ���ر����, ����ʱ���� "δ����"
function getDrawingStatusNameById(typeid)
{
	var typeName = "δ����";

    // 1���ɹ� 2��ʧ�� 3���ȴ�����(δ����) 4�������� 
    switch (typeid)
    {
        case 1:       
            typeName = "���ֳɹ�";
        	break;
        case 2:       
        	typeName = "����ʧ��";
        	break;
        case 3:
        	typeName = "������";
        	break;
        case 4:
        	typeName = "���ύ����";
        	break;
        default:
            typeName = "δ����";
        	break;	
    }
    return typeName;
}

// ���븴�Ӷȼ��
// ����ֵ
//  "true" 	���ͨ��
//	����	������Ϣ
function checkPasswd(passwd)
{
	// ���passwd�ĳ����Ƿ����8
	var len = passwd.length;
	if (len < 8 || len > 16) {
        return "���볤�Ȳ���С��8λ�����ܴ���16λ";
	}

	// ��������Ƿ�Ϊ������
	if(passwd.search(/^[0-9]*$/) >= 0)
		return "���벻��Ϊ�����֣��Ƽ���ĸ(���ִ�Сд)�����ֵ����ϵķ�ʽ";

	// ��������Ƿ�Ϊ��Сд��ĸ
	if(passwd.search(/^[a-z]*$/) >= 0)
		return "���벻��Ϊ����ĸ���Ƽ���ĸ(���ִ�Сд)�����ֵ����ϵķ�ʽ";

	// ��������Ǽ���Ϊ����д��ĸ
	if(passwd.search(/^[A-Z]*$/) >= 0)
		return "���벻��Ϊ����ĸ���Ƽ���ĸ(���ִ�Сд)�����ֵ����ϵķ�ʽ";

	return "true";
}


// ��ת
function jump(surl)
{
	//window.alert("��ת����:" + surl);
    surl = surl.replace(/=&/g, '%3d&');         // ���ں�=��ֵ����Ҫת����%3d
    surl = surl.replace(/==&/g, '%3d&');        // ���ں�=��ֵ����Ҫת����%3d
	// ���û��ָ����ת���ӣ������
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
	
	// ��ת��ָ������
	//window.location.href= surl;
    location.replace(surl)

	return;
}

/*
==================================================================
LTrim(string):ȥ����ߵĿո�
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
RTrim(string):ȥ���ұߵĿո�
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
Trim(string):ȥ��ǰ��ո�
==================================================================
*/
function Trim(str)
{
    return RTrim(LTrim(str));
}



/*
==================================================================
ʡ�ݱ��뼰���б��봦��
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

where[0] = new comefrom("��ѡ��ʡ����","��ѡ�������", "", "");
where[1] = new comefrom("����", "����", "1", "10"); 
where[2] = new comefrom("�Ϻ�", "�Ϻ�", "2", "21"); 
where[3] = new comefrom("���", "���", "3", "22");
where[4] = new comefrom("����", "����", "4", "23"); 
where[5] = new comefrom("�ӱ�", "ʯ��ׯ|�żҿ�|�е�|�ػʵ�|��ɽ|�ȷ�|����|����|��ˮ|��̨|����", "5", "311|313|314|335|315|316|312|317|318|319|310"); 
where[6] = new comefrom("ɽ��", "̫ԭ|��ͬ|˷��|��Ȫ|����|����|����|��ʯ|�ܴ�|�ٷ�|�˳�", "6", "351|352|349|353|355|356|350|358|354|357|359"); 
where[7] = new comefrom("���ɹ�", "���ͺ���(*)|��ͷ|�ں�|���|������|��������|ͨ��|���ֺ���|����|��ʤ|�ٺ�|����������", "7", "471|472|473|476|470|482|475|479|474|477|478|483"); 
where[8] = new comefrom("����", "����(*)|����|����|����|��˳|��Ϫ|����|��ɽ|����|����|Ӫ��|�̽�|����|��«��", "8", "24|421|418|410|413|414|419|412|415|411|417|427|416|429"); 
where[9] = new comefrom("����", "����(*)|�׳�|��ԭ|����|��ƽ|��Դ|ͨ��|��ɽ|�Ӽ�", "9", "431|436|438|432|434|437|435|439|433"); 
where[10] = new comefrom("������", "������(*)|�������|�ں�|����|����|�׸�|��ľ˹|˫Ѽɽ|��̨��|����|ĵ����|�绯|�Ӹ����", "10", "451|452|456|459|458|468|454|469|464|467|453|455|457"); 
where[11] = new comefrom("����", "�Ͼ�(*)|����|����|���Ƹ�|��Ǩ|����|�γ�|����|̩��|��ͨ|��|����|����", "11", "25|512|516|518|527|517|515|514|523|513|511|519|510"); 
where[12] = new comefrom("�㽭", "����(*)|����|����|��ɽ|����|����|��|̨��|����|��ˮ|����", "12", "571|572|573|580|574|575|579|576|577|578|570"); 
where[13] = new comefrom("����", "�Ϸ�(*)|����|����|����|����|����|����|��ɽ|�ߺ�|ͭ��|����|��ɽ|����|����|���|����", "13", "551|557|561|558|552|554|550|555|553|562|556|559|564|565|566|563"); 
where[14] = new comefrom("����", "����(*)|��ƽ|����|����|Ȫ��|����|����|����|����|����|����|ʯʨ|����|����ɽ", "14", "591|599|598|594|595|592|596|597|593|5930|5990|5950|5980|5991"); 
where[15] = new comefrom("����", "�ϲ�(*)|�Ž�|������|ӥ̶|����|Ƽ��|����|����|�ٴ�|�˴�|����|����", "15", "791|792|798|701|790|799|797|793|794|795|796|7940"); 
where[16] = new comefrom("ɽ��", "����(*)|�ĳ�|����|��Ӫ|�Ͳ�|Ϋ��|��̨|����|�ൺ|����|����|��ׯ|����|̩��|����|����|����", "16", "531|635|534|546|533|536|535|631|532|633|539|632|537|538|634|543|530"); 
where[17] = new comefrom("����", "֣��(*)|����Ͽ|����|����|����|�ױ�|����|���|����|����|���|���|ƽ��ɽ|����|����|��Դ|�ܿ�|פ���", "17", "371|398|379|391|373|392|372|393|378|370|374|395|375|377|376|3910|394|396"); 
where[18] = new comefrom("����", "�人(*)|ʮ��|�差|����|Т��|�Ƹ�|����|��ʯ|����|����|�˲�|��ʩ|����|Ǳ��|����", "18", "27|719|710|724|712|713|711|714|715|716|717|718|728|7281|7282");
where[19] = new comefrom("����", "��ɳ(*)|�żҽ�|����|����|����|����|��̶|����|����|����|����|����|¦��|����", "19", "731|744|736|737|730|733|732|734|735|746|739|745|738|743"); 
where[20] = new comefrom("�㶫", "����(*)|����|��Զ|�ع�|��Դ|÷��|����|��ͷ|����|��β|����|��ݸ|�麣|��ɽ|����|��ɽ|ï��|տ��|����|�Ƹ�|����", "20", "20|755|763|751|762|753|768|754|663|660|752|769|756|760|750|757|668|759|662|766|758"); 
where[21] = new comefrom("����", "����(*)|����|����|����|����|����|����|���Ǹ�|��ɫ|�ӳ�|���|����", "21", "771|773|772|774|775|777|779|770|776|778|7750|7740"); 
where[22] = new comefrom("����", "����(*)|����|����", "22", "898|899|890"); 
where[23] = new comefrom("�Ĵ�", "�ɶ�(*)|��Ԫ|����|����|�ϳ�|�㰲|����|�ڽ�|��ɽ|�Թ�|����|�˱�|��֦��|����|����|����|�Ű�|����", "23", "28|839|816|838|817|826|825|832|833|813|830|831|812|827|818|8320|835|834"); 
where[24] = new comefrom("����", "����(*)|����ˮ|����|�Ͻ�|ͭ��|��˳|����|����|����", "24", "851|858|852|857|856|853|855|854|859"); 
where[25] = new comefrom("����", "����(*)|����|��Ϫ|����|��ͨ|˼é|�ٲ�|��ɽ|º��|��ˮ|�е�|����|����|����|��ɽ|����|���", "25", "871|874|877|888|870|879|883|875|692|886|887|872|878|873|876|691|8730");
where[26] = new comefrom("����", "����(*)|����|����|��֥|�˶�|�տ���|����", "26", "891|896|895|894|893|892|897"); 
where[27] = new comefrom("����", "����(*)|�Ӱ�|ͭ��|μ��|����|����|����|����|����|����", "27", "29|911|919|913|910|917|916|912|914|915"); 
where[28] = new comefrom("����", "����(*)|������|����|��ˮ|��Ȫ|��Ҵ|���|����|ƽ��|����|¤��|����|����", "28", "931|937|943|938|9370|936|935|934|933|932|939|930|9350"); 
where[29] = new comefrom("����", "����(*)|ʯ��ɽ|����|��ԭ", "29", "951|952|953|954"); 
where[30] = new comefrom("�ຣ", "����(*)|ƽ��|����|����|ͬ��|����|����|�����", "30", "971|972|970|974|973|975|976|977");
where[31] = new comefrom("�½�", "��³ľ��(*)|��������|ʯ����|��ʲ|������|����|��³��|����|��ͼʲ|����|����|�����|����|����|����|����̩|�����", "31", "991|990|993|998|997|903|995|902|908|909|994|996|999|992|901|906|940"); 
