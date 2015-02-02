<%@ Page language="c#" Codebehind="UserBankAccountQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.UserBankAccountQuery" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>UserBankAccountQuery</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">
@import url( ../STYLES/ossstyle.css ); .style2 { FONT-WEIGHT: bold; COLOR: #ff0000 }
		</style>
		<script language="javascript" src="../js/common.js"></script>
		<script language="JavaScript">
		
		//---------------- ���������б��� ���
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
	
	//document.getElementById("Hcity").innerText = document.Form1.city.value;
	//document.getElementById("Harea").innerText = document.Form1.area.value;
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
		
		document.Form1.area.selectedIndex = prov;  //ʡ��
        select();
        setCityById(city);  //����
	}
}
--> 
		</script>
	</HEAD>
	<body onload="init('<%=iprov%>','<%=icity%>')"MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE height="116" cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
				<TR>
					<TD bgColor="#666666" style="HEIGHT: 136px"><TABLE height="148" cellSpacing="1" cellPadding="0" width="100%" align="center" border="0">
							<TR>
								<TD bgColor="#eeeeee" height="20" style="HEIGHT: 20px">&nbsp;�˻�״̬:
								</TD>
								<TD width="19%" height="20" bgColor="#ffffff" style="HEIGHT: 20px">&nbsp;
									<asp:Label id="Label1_State" runat="server">����</asp:Label></TD>
								<TD width="7%" bgColor="#eeeeee" style="HEIGHT: 20px">&nbsp;
									<asp:LinkButton id="lkb_acc" runat="server" Visible="False" onclick="lkb_acc_Click"></asp:LinkButton></TD>
								<TD bgColor="#eeeeee" height="20" style="HEIGHT: 20px"><P>&nbsp;�����ʻ�����</P>
								</TD>
								<TD bgColor="#ffffff" height="20" style="HEIGHT: 20px">&nbsp;<FONT face="����"> </FONT>
									<asp:Label id="Label2_BankID" runat="server">4203038256578965</asp:Label></TD>
							</TR>
							<TR>
								<TD width="23%" height="20" bgColor="#eeeeee">&nbsp;QQ �˺�:</TD>
								<TD height="10" colspan="2" bgColor="#ffffff">&nbsp;
									<asp:Label id="Label3_QQID" runat="server">42030</asp:Label></TD>
								<TD width="26%" height="10" bgColor="#eeeeee">&nbsp;��������:</TD>
								<TD width="25%" height="10" bgColor="#ffffff">&nbsp;<FONT face="����">&nbsp;
										<asp:Label id="Label13_BankType" runat="server">��������</asp:Label></FONT></TD>
							</TR>
							<TR>
								<TD bgColor="#eeeeee" height="22" style="HEIGHT: 22px">&nbsp;��������:</TD>
								<TD height="22" colspan="2" bgColor="#ffffff" style="HEIGHT: 22px">&nbsp;
									<asp:Label id="Label4_TrueName" runat="server">Ray</asp:Label></TD>
								<TD bgColor="#eeeeee" height="22" style="HEIGHT: 22px">&nbsp;��������/����������:</TD>
								<TD bgColor="#ffffff" height="22" style="HEIGHT: 22px">&nbsp;<FONT face="����"> </FONT>
									<asp:Label id="Label8_BankName" runat="server">�����й���������ɽ֧��</asp:Label></TD>
							</TR>
							<TR>
								<TD style="HEIGHT: 22px" bgColor="#eeeeee" height="22"><FONT face="����">&nbsp;��˾����:</FONT></TD>
								<TD style="HEIGHT: 22px" bgColor="#ffffff" colSpan="2" height="22"><FONT face="����">&nbsp;
										<asp:Label id="lbCompay" runat="server">Label</asp:Label></FONT></TD>
								<TD style="HEIGHT: 22px" bgColor="#eeeeee" height="22"><FONT face="����">&nbsp;����ʱ��:</FONT></TD>
								<TD style="HEIGHT: 22px" bgColor="#ffffff" height="22"><FONT face="����">&nbsp;
										<asp:Label id="lbAccCreate" runat="server">Label</asp:Label></FONT></TD>
							</TR>
							<TR>
								<TD bgColor="#eeeeee" height="19" style="HEIGHT: 19px">&nbsp;��������/ʡ��</TD>
								<TD height="19" colspan="2" bgColor="#ffffff" style="HEIGHT: 19px">&nbsp; <FONT face="����">
										<select id="area" style="WIDTH: 100px" onchange="select()" name="area" runat="server">
										</select></FONT></TD>
								<TD bgColor="#eeeeee" height="19" style="HEIGHT: 19px">&nbsp;��������:</TD>
								<TD bgColor="#ffffff" height="19" style="HEIGHT: 19px">&nbsp;<FONT face="����"> </FONT>
									<FONT face="����">
										<select id="city" style="WIDTH: 100px" onchange="select()" name="city" runat="server">
										</select></FONT></TD>
							</TR>
							<TR>
								<TD height="18" bgColor="#eeeeee" style="HEIGHT: 18px">&nbsp;����½IP��ַ:</TD>
								<TD height="18" colspan="2" bgColor="#ffffff" style="HEIGHT: 18px">&nbsp;
									<asp:Label id="Label6_LastIP" runat="server">202.103.24.68</asp:Label></TD>
								<TD bgColor="#eeeeee" height="18" style="HEIGHT: 18px"><FONT face="����">&nbsp;������ʱ��:</FONT></TD>
								<TD bgColor="#ffffff" height="18" style="HEIGHT: 18px">&nbsp;<FONT face="����">
										<asp:Label id="Label11_Modify_Time" runat="server">2005-05-02 16:40 </asp:Label>
									</FONT>
								</TD>
							</TR>
							<TR>
								<TD height="20" bgColor="#eeeeee">&nbsp;��ע:</TD>
								<TD height="-1" colspan="4" bgColor="#ffffff">&nbsp;
									<asp:Label id="Label12_Memo" runat="server">�û���2005��3��1���ڹ㶫ʡ�����й���������ɽ֧�п�����Ԥ���5000Ԫ��</asp:Label></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
