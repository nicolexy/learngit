<%@ Register TagPrefix="uc1" TagName="UserAppealCheckControl" Src="../Control/UserAppealCheckControl.ascx" %>
<%@ Page language="c#" Codebehind="TestPage.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages.TestPage" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>TestPage</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<script type="text/javascript">
			var tip = 1;
			function showTips()
			{
				//alert("fired");

				
				if(tip)
				{
					document.getElementById("div_1").style.display = "";
					document.getElementById("cb_2").checked = true;
					tip = 0;
				}
				else
				{
					document.getElementById("div_1").style.display = "none";
					document.getElementById("cb_2").checked = false;
					tip = 1;
				}
			}
			
			function Init()
			{
				document.getElementById("rbtn_pass").checked=true;
			}
			
			function enterPress(e)
					{
						if(window.event) // IE
						{
							if(e.keyCode == 13)
								alert("enter press");
						}
					}
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout" onkeydown="enterPress(event)">
		<form id="Form1" method="post" runat="server">
			<table>
				<tr>
					<td>
						<input type="radio" name="rbtn" value="ͨ��" id="rbtn_pass">ͨ�� <input type="radio" name="rbtn" id="rbtn_reject">�ܾ�
						<asp:CheckBoxList Runat="server" id="CheckBoxList1">
							<asp:ListItem Value="1"></asp:ListItem>
							<asp:ListItem Value="2"></asp:ListItem>
							<asp:ListItem Value="3"></asp:ListItem>
							<asp:ListItem Value="4"></asp:ListItem>
							<asp:ListItem Value="5"></asp:ListItem>
						</asp:CheckBoxList>
					</td>
					<td><input runat="server" type="checkbox" id="cb_2" value="cb2" onclick="showTips()"><asp:Label Runat="server" Text="value" id="Label1"></asp:Label></td>
					<td><asp:CheckBox runat="server" ID="cb_1" Text="cb1" /></td>
					<td>
						<div id="div_1" style="DISPLAY:none">
							<asp:CheckBoxList Runat="server" ID="cbxl_1">
								<asp:ListItem Value="1_1"></asp:ListItem>
								<asp:ListItem Value="1_2"></asp:ListItem>
								<asp:ListItem Value="1_3"></asp:ListItem>
								<asp:ListItem Value="1_4"></asp:ListItem>
							</asp:CheckBoxList>
						</div>
					</td>
				</tr>
			</table>
			<table>
				<tr>
					<td><uc1:UserAppealCheckControl runat="server" ID="Userappealcheckcontrol1" NAME="Userappealcheckcontrol2"></uc1:UserAppealCheckControl></td>
				</tr>
				<tr>
					<td><uc1:UserAppealCheckControl runat="server" ID="Userappealcheckcontrol2" NAME="Userappealcheckcontrol2"></uc1:UserAppealCheckControl></td>
				</tr>
				<tr>
					<td><uc1:UserAppealCheckControl runat="server" ID="Userappealcheckcontrol3" NAME="Userappealcheckcontrol2"></uc1:UserAppealCheckControl></td>
				</tr>
				<tr>
					<td><asp:Button Runat="server" Text="�ύ" id="btn_submit" onclick="btn_submit_Click" /></td>
				</tr>
				<tr>
					<td><asp:Label runat="Server" ID="lb_text1"></asp:Label></td>
				</tr>
				<tr>
					<td><a href='https://www.tenpay.com/certificates/tenpay_tel31.shtml?listid=4385931'> test</a></td>
				</tr>
				<tr>
					<td>
						<p style="PADDING-BOTTOM:10px !important; MARGIN:0px; PADDING-LEFT:0px !important; PADDING-RIGHT:0px !important; PADDING-TOP:10px !important">����2012��05��04���ύ�ĸ��������ֻ������߱���ͨ����ˣ���<A href="https://www.tenpay.com/certificates/tenpay_tel31.shtml?listid=4385931 rel="
								target="_blank">��˼����������ֻ�</A>
							<BR>
							��ܰ��ʾ: �����ӵ�ַ��Ч��Ϊ72Сʱ����������Ч����������ֻ��ʺŵİ󶨣�������޷����������ӵ�ַ������ʹ��IE�������¼<A href="www.tenpay.com">�Ƹ�ͨ��վ</A>�������µ���ʼ����ӵ�ַ��лл��"<a rel="nofollow" target="_blank" style="DISPLAY:none">��˼����������ֻ�
							</a>
							<br>
							https://www.tenpay.com/certificates/tenpay_tel31.shtml?listid=4385931<br>
							��ܰ��ʾ�������ӵ�ַ��Ч��Ϊ72Сʱ����������Ч����������ֻ��ʺŵİ󶨣�������޷����������ӵ�ַ������ʹ��IE�������¼<a href='www.tenpay.com'>�Ƹ�ͨ��վ</a>�������µ���ʼ����ӵ�ַ��лл��
						</p>
					</td>
				</tr>
			</table>
			<p style="PADDING-BOTTOM:10px !important; MARGIN:0px; PADDING-LEFT:0px !important; PADDING-RIGHT:0px !important; PADDING-TOP:10px !important"><br>
				https://www.tenpay.com/certificates/tenpay_tel31.shtml?listid=4385931<br>
				��ܰ��ʾ�������ӵ�ַ��Ч��Ϊ72Сʱ<br>
				������޷����������ӵ�ַ</p>
			<p><asp:TextBox Runat="server" ID="tbx_1" TextMode="MultiLine"></asp:TextBox></p>
			<p><asp:DropDownList Runat="server" ID="ddl_1" AutoPostBack="True"></asp:DropDownList>
			<asp:Button Runat="server" ID="btn_addFastReply" Text="��ӿ�ݻظ�" onclick="btn_addFastReply_Click"></asp:Button>
			<asp:Button Runat="server" ID="btn_updateConfigFile" Text="�޸������ļ�" onclick="btn_updateConfigFile_Click"></asp:Button>
			</p>
		</form>
	</body>
</HTML>
