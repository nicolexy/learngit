<%@ Page language="c#" Codebehind="ChangeUserInfo_old.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.ChangeUserName" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>UserAccountQuery</title>
		<meta http-equiv="Content-Type" content="text/html; charset=gb2312">
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); .style4 { COLOR: #ff0000 }
	.style5 { COLOR: #ff0000; FONT-WEIGHT: bold }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		<SCRIPT language="javascript">
		function IsNumber(string,sign) 
		{ 
			var number; 
			if (string==null) 
				return false; 
 
			number = new Number(string); 
			if (isNaN(number)) 
			{ 
				return false; 
			} 
			else
			{ 
				return true; 
			} 
		}
			function checkvlid()
			{
				with(Form1)
				{
					if(TX_QQID.value=="")
					{
						alert("用户帐户不能为空!!");
						TX_QQID.focus();
						return false;
					}	
				}
				return true;
			}
		</SCRIPT>
		<script language="javascript" src="../js/common.js"></script>
		<script language="JavaScript">
		
		<!--
//----------------Form提交检查
function submitForm()
{
	with(document.Form1)
	{
		// 银行类型检查
	    if (bank_type.value.length < 1 || !checkBankType(bank_type.value))
	    {
	        window.alert("请选择开户银行名称");
	        return false;
	    }

		// 开户地区检查
	    if (area.value.length < 1 || !checkBankArea(area.value))
	    {
	        window.alert("请选择开户银行所在省份");
	        return false;
	    }
		
		// 开户城市检查
	    if (city.value.length < 1 || !checkBankCity(city.value))
	    {
	        window.alert("请选择开户银行所在城市");
	        return false;
	    }
	    
	    // 支行名称 bank_name
	    if (bank_name.value.length < 1)
		{
	        window.alert("请输入开户行支行名称");
	        return false;		
		}
	    if (bank_name.value.length > 64)
	    {
	        window.alert("请输入有效的银行名称，注意长度不能超过64个字");
	        return false;
	    }

	    // 银行账号 bank_id
	    if (bank_id.value.length < 1)
	    {
	        window.alert("请输入银行账号");
	        return false;
	    }
	    if (!checkBankId(bank_id.value))
	    {
	        window.alert("请输入一个有效的银行账号，只能包含数字，且长度不能超过32位");
	        return false;
	    }

		// 支付密码
		if (document.passwd.value.length < 1)
		{
	        window.alert("请输入支付密码");
	        return false;
		}
	    p.value = document.passwd.value;
	    	    
	}
	
    // 提交form表单
    document.Form1.submit();
}

// 清空输入框的信息
function clearForm()
{
	with (document.Form1)
	{
		bank_name.value = "";
		bank_id.value = "";
	}
}

//----------------输入框中回车键处理函数
function dealKeyPress()
{
    if(event.keyCode == 13)
    {
    submitForm();
    }
}

//---------------- 地区及城市编码 相关
function select() 
{
	with(document.Form1.area) 
	{ 
		var loca2 = options[selectedIndex].value; 
	}
	
	for(i = 0;i < where.length; i++) 
	{
		if (where[i].locaid == loca2) 
		{
			loca3 = (where[i].locacity).split("|");
			loca4 = (where[i].locacityids).split("|");
			for(j = 0; j < loca3.length; j++) 
			{
				with(document.Form1.city) 
				{ 
					length = loca3.length; 
					options[j].text = loca3[j]; 
					options[j].value = loca4[j];
					var loca5=options[selectedIndex].value;
				}
			}
			break;
		}	
	}
	
	document.getElementById("Hcity").innerText = document.Form1.city.value;
	document.getElementById("Harea").innerText = document.Form1.area.value;
}

function setCityById(CityId)
{
	with(document.Form1.area) 
	{ 
		var areaId = options[selectedIndex].value; 
	}
	
	for(i = 0;i < where.length; i++) 
	{
		if (where[i].locaid == areaId) 
		{
			cityArray = (where[i].locacityids).split("|");
			for(j = 0; j < cityArray.length; j++) 
			{
				if (cityArray[j] == CityId)
				{
					document.Form1.city.selectedIndex = j;
					break;
				}	
			}
			break;
		}	
	}
}

function init(prov,city) 
{
	with(document.Form1.area)
	{
		length = where.length;
    	for(k = 0; k < where.length; k++) 
    	{ 
       		options[k].text = where[k].loca; 
       		options[k].value = where[k].locaid; 
    	}

       	options[selectedIndex].text = where[0].loca; 
       	options[selectedIndex].value = where[0].locaid;
       	
    }
	
	with(document.Form1.city) 
	{
		loca3 = (where[0].locacity).split("|");
		loca4 = (where[0].locacityids).split("|");
		length = loca3.length;
		
		for(l=0; l<length; l++) 
		{ 
			options[l].text = loca3[l]; 
			options[l].value = loca4[l]; 
		}
		
		options[selectedIndex].text = loca3[0]; 
		options[selectedIndex].value = loca4[0];
		
		//prov = 20;
		//city = 753;
		
		document.Form1.area.selectedIndex = prov;  //省份
        select();
        setCityById(city);  //城市
	}
}
--> 
		</script>
	</HEAD>
	<body onload="init('<%=iprov%>','<%=icity%>')"MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT>
			<br>
			<table cellSpacing="1" cellPadding="0" width="90%" align="center" bgColor="#666666" border="0">
				<tr bgColor="#e4e5f7">
					<td vAlign="middle" colSpan="2" height="20">
						<table height="90%" cellSpacing="0" cellPadding="1" width="97%" border="0">
							<tr>
								<td width="80%" height="18"><font color="#ff0000"><STRONG><FONT color="#ff0000">&nbsp;</FONT></STRONG><IMG height="16" src="../IMAGES/Page/post.gif" width="20">
										用户查询</font>
									<div align="right"></div>
								</td>
								<td width="20%">操作员代码: <span class="style4">
										<asp:label id="Label_uid" runat="server">Label</asp:label></span></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr bgColor="#ffffff">
					<td>
						<div align="center"></div>
						<div align="left">
							<table height="100%" cellSpacing="0" cellPadding="1" width="100%" border="0">
								<tr>
									<td width="19%">&nbsp;</td>
									<td width="78%">输入：&nbsp;&nbsp;
										<asp:textbox id="TX_QQID" runat="server" BorderWidth="1px"></asp:textbox>
										&nbsp;&nbsp;
										<asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" ControlToValidate="TX_QQID" ErrorMessage="RequiredFieldValidator">请输入帐户号码</asp:requiredfieldvalidator></td>
									<TD width="3%">&nbsp;</TD>
								</tr>
							</table>
						</div>
						<div align="left"></div>
						<div align="center"></div>
					</td>
					<td width="25%">
						<div align="center">&nbsp;
							<asp:button id="Button1" runat="server" Text="查 询" onclick="Button1_Click"></asp:button></div>
					</td>
				</tr>
			</table>
			<br>
			<TABLE height="77" cellSpacing="0" cellPadding="0" width="90%" align="center" border="0">
				<TR>
					<TD bgColor="#666666">
						<TABLE height="163" cellSpacing="1" cellPadding="0" width="100%" align="center" border="0">
							<TR bgColor="#e4e5f7">
								<TD style="WIDTH: 699px" width="699" height="30"><FONT color="#ff0000">&nbsp;<IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;用户资料</FONT>
									<DIV align="center"><FONT face="宋体"></FONT></DIV>
								</TD>
								<TD width="173">
									<div align="center">&nbsp;</div>
								</TD>
								<TD align="right" width="52"><asp:imagebutton id="ImageButton3" runat="server" ImageUrl="../Images/Page/down.gif"></asp:imagebutton></TD>
							</TR>
							<TR>
								<TD bgColor="#ffffff" colSpan="3" height="12">
									<table cellSpacing="1" cellPadding="0" width="100%" border="0">
										<tr>
											<td bgColor="#333333">
												<TABLE id="Table1" height="100%" cellSpacing="1" cellPadding="0" width="100%" align="center"
													border="0">
													<TR>
														<TD style="WIDTH: 225px" width="225" bgColor="#eeeeee" height="18"><font face="宋体"> &nbsp;QQ号:</font></TD>
														<TD width="236" bgColor="#ffffff" height="19"><font face="宋体">&nbsp;<asp:label id="Label1_Fqqid" runat="server">
																88254028</asp:label></font></TD>
														<TD style="WIDTH: 229px; HEIGHT: 19px" width="229" bgColor="#eeeeee" height="19"><font face="宋体">&nbsp;<FONT style="BACKGROUND-COLOR: #eeeeee" face="宋体">最后修改时间:</FONT></font></TD>
														<TD style="HEIGHT: 19px" width="195" bgColor="#ffffff" height="19"><font face="宋体">&nbsp;
																<asp:textbox id="TX_Fmodify_time" runat="server" BorderWidth="0px" Width="170px" BorderColor="Gray"
																	ReadOnly="True" BorderStyle="Dashed">2005-05-02 16:40</asp:textbox></font></TD>
													</TR>
													<TR>
														<TD style="WIDTH: 225px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><font face="宋体">&nbsp;真实姓名:</font></TD>
														<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><font face="宋体"><asp:textbox id="TextBox2_Ftruename" runat="server" BorderWidth="0px" Width="170px" BorderColor="Gray"
																	ReadOnly="True" BorderStyle="Dashed">张爱国</asp:textbox></font></TD>
														<TD style="WIDTH: 229px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><font face="宋体">&nbsp;公司名称:
															</font>
														</TD>
														<TD style="HEIGHT: 20px" bgColor="#ffffff" height="20"><font face="宋体">&nbsp;<FONT face="宋体">
																	<asp:textbox id="Textbox4_Company" runat="server" BorderWidth="0px" Width="170px" BorderColor="Gray"
																		ReadOnly="True" BorderStyle="Dashed">公司</asp:textbox></FONT></font></TD>
													</TR>
													<TR>
														<TD style="WIDTH: 225px; HEIGHT: 16px" bgColor="#eeeeee" height="16">&nbsp;用户性别:</TD>
														<TD style="WIDTH: 136px; HEIGHT: 16px" bgColor="#ffffff" height="16"><FONT face="宋体"><asp:dropdownlist id="DropDownList1_Sex" runat="server" Width="139px">
																	<asp:ListItem Value="0" Selected="True">未指定</asp:ListItem>
																	<asp:ListItem Value="1">男</asp:ListItem>
																	<asp:ListItem Value="2">女</asp:ListItem>
																</asp:dropdownlist></FONT></TD>
														<TD style="WIDTH: 229px; HEIGHT: 16px" bgColor="#eeeeee" height="16"><FONT face="宋体">&nbsp;年龄:
																<asp:regularexpressionvalidator id="RegularExpressionValidator5" runat="server" ControlToValidate="Textbox5_Fage"
																	ErrorMessage="RegularExpressionValidator" ValidationExpression="^[0-9]{2}">请输入正确年龄</asp:regularexpressionvalidator></FONT></TD>
														<TD style="HEIGHT: 16px" bgColor="#ffffff" height="16"><font face="宋体">&nbsp;
																<asp:textbox id="Textbox5_Fage" runat="server" BorderWidth="0px" Width="170px" BorderColor="Gray"
																	ReadOnly="True" BorderStyle="Dashed">22</asp:textbox></font></TD>
													</TR>
													<TR>
														<TD style="WIDTH: 225px; HEIGHT: 19px" bgColor="#eeeeee" height="18"><FONT face="宋体">&nbsp;固定电话:
																<asp:regularexpressionvalidator id="RegularExpressionValidator6" runat="server" ControlToValidate="Textbox6_Fphone"
																	ErrorMessage="RegularExpressionValidator" ValidationExpression="^[0-9/-]+">请输入数字和“-”</asp:regularexpressionvalidator></FONT></TD>
														<TD style="WIDTH: 136px; HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="宋体"><asp:textbox id="Textbox6_Fphone" runat="server" BorderWidth="0px" Width="170px" BorderColor="Gray"
																	ReadOnly="True" BorderStyle="Dashed">0755-55789460</asp:textbox></FONT></TD>
														<TD style="WIDTH: 229px; HEIGHT: 19px" bgColor="#eeeeee" height="19"><FONT face="宋体">&nbsp;手机号码:</FONT></TD>
														<TD style="HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;
																<asp:textbox id="Textbox7_Fmobile" runat="server" BorderWidth="0px" Width="170px" BorderColor="Gray"
																	ReadOnly="True" BorderStyle="Dashed">19971213148</asp:textbox></FONT></TD>
													</TR>
													<TR>
														<TD style="WIDTH: 225px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="宋体">&nbsp;证件类型:</FONT></TD>
														<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="宋体"><asp:dropdownlist id="DropDownList2_certify" runat="server" Width="139px"></asp:dropdownlist></FONT></TD>
														<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="宋体">&nbsp;证件号码:</FONT></TD>
														<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="宋体">&nbsp;
																<asp:textbox id="Textbox13_Fcreid" runat="server" BorderWidth="0px" Width="170px" BorderColor="Gray"
																	ReadOnly="True" BorderStyle="Dashed">19971213148</asp:textbox></FONT></TD>
													</TR>
													<TR>
														<TD style="WIDTH: 225px; HEIGHT: 19px" bgColor="#eeeeee" height="18"><FONT face="宋体">&nbsp;邮政编码:
																<asp:regularexpressionvalidator id="RegularExpressionValidator4" runat="server" ControlToValidate="Textbox11_Fpcode"
																	ErrorMessage="RegularExpressionValidator" ValidationExpression="^[0-9]+">邮政编码格式不正确</asp:regularexpressionvalidator></FONT></TD>
														<TD style="WIDTH: 136px; HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="宋体"><asp:textbox id="Textbox11_Fpcode" runat="server" BorderWidth="0px" Width="170px" BorderColor="Gray"
																	ReadOnly="True" BorderStyle="Dashed">434322400</asp:textbox></FONT></TD>
														<TD style="WIDTH: 229px; HEIGHT: 19px" bgColor="#eeeeee" height="19"><FONT face="宋体">&nbsp;用户Email:
																<asp:regularexpressionvalidator id="RegularExpressionValidator1" runat="server" ControlToValidate="Textbox7_Femail"
																	ErrorMessage="RegularExpressionValidator" Width="88px" ValidationExpression="^[\w-]+@[\w]+\.(com|net|org|edu|cn|mil)">邮件格式不正确</asp:regularexpressionvalidator></FONT></TD>
														<TD style="HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="宋体">&nbsp;<FONT face="宋体">
																	<asp:textbox id="Textbox7_Femail" runat="server" BorderWidth="0px" Width="170px" BorderColor="Gray"
																		ReadOnly="True" BorderStyle="Dashed">163@163.com</asp:textbox></FONT></FONT></TD>
													</TR>
													<TR>
														<TD style="WIDTH: 225px; HEIGHT: 19px" bgColor="#eeeeee" height="18">&nbsp;城市:</TD>
														<TD style="WIDTH: 136px; HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="宋体"><select id="area" style="WIDTH: 100px" onchange="select()" name="area" runat="server"></select></FONT></TD>
														<TD style="WIDTH: 229px; HEIGHT: 19px" bgColor="#eeeeee" height="19"><FONT style="BACKGROUND-COLOR: #eeeeee" face="宋体">&nbsp;地区/省市:</FONT></TD>
														<TD style="HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="宋体"><select id="city" style="WIDTH: 100px" onchange="select()" name="city" runat="server"></select></FONT></TD>
													</TR>
													<TR>
														<TD style="WIDTH: 225px" bgColor="#eeeeee" height="18"><FONT face="宋体">&nbsp;联系地址:</FONT></TD>
														<TD bgColor="#ffffff" colSpan="3" height="20"><FONT face="宋体"><asp:textbox id="Textbox10_Faddress" runat="server" BorderWidth="0px" Width="170px" BorderColor="Gray"
																	ReadOnly="True" BorderStyle="Dashed">深圳高新科技园</asp:textbox></FONT></TD>
													</TR>
													<TR>
														<TD style="WIDTH: 225px" bgColor="#eeeeee" height="18"><font face="宋体">&nbsp;备注:</font></TD>
														<TD bgColor="#ffffff" colSpan="3" height="20"><font face="宋体"><asp:textbox id="TX_Memo" runat="server" BorderWidth="0px" Width="621px" BorderColor="Gray" ReadOnly="True"
																	BorderStyle="Dashed">备注信息</asp:textbox></font></TD>
													</TR>
													<TR>
														<TD style="WIDTH: 225px" bgColor="#eeeeee" height="18"><font face="宋体">&nbsp;证书信息:</font></TD>
														<TD bgColor="#ffffff" colSpan="3" height="20"><font face="宋体"><asp:textbox id="tbCrt" runat="server" BorderWidth="0px" Width="621px" BorderColor="Gray" ReadOnly="True"
																	BorderStyle="Dashed">证书信息</asp:textbox></font></TD>
													</TR>
												</TABLE>
											</td>
										</tr>
									</table>
								</TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
			<P align="center"><INPUT id="Hcity" style="Z-INDEX: 103; POSITION: absolute; TOP: 432px; LEFT: 42px" type="hidden"
					name="Hcity" runat="server" onserverchange="Hcity_ServerChange"><INPUT id="Harea" style="Z-INDEX: 105; POSITION: absolute; TOP: 432px; LEFT: 172px" type="hidden"
					name="Harea" runat="server"></FONT></P>
		</form>
	</body>
</HTML>
