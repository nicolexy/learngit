<%@ Control Language="c#" AutoEventWireup="True" Codebehind="UserClassCheckControl.ascx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.Control.UserClassCheckControl" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script src="../SCRIPTS/Local.js"></script>
<table cellSpacing="0" cellPadding="0" width="100%" border="0">
	<tr>
		<td width="30%" valign="top">
			<table cellSpacing="1" cellPadding="0" width="100%" border="1">
				<tr bgColor="#ffffff">
					<td align="center" width="100%" background="IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="20">���֤ɨ���</td>
				</tr>
				
				<tr bgColor="#ffffff">
					<td align="center" height="20" style="PADDING-BOTTOM: 3px; PADDING-LEFT: 3px; PADDING-RIGHT: 3px; PADDING-TOP: 3px">
                    <asp:image id="Image1" runat="server" Height="255px" Width="330px"></asp:image><asp:image id="Image2" runat="server" Height="255px" Width="330px"></asp:image>
                    </td>
				</tr>
				<tr>
					<td bgColor="#ffffff" align="center"><button onmousedown="TurnBig(<%=imgOther.ClientID%>);" >�Ŵ�</button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						<button onmousedown="TurnShort(<%=imgOther.ClientID%>);" >��С</button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						<button onmousedown="TurnBack(<%=imgOther.ClientID%>);" >��ԭ</button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						<button onmousedown="RotaLeft(<%=imgOther.ClientID%>);" >����ת</button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						<button onmousedown="RotaRight(<%=imgOther.ClientID%>);" >����ת</button>
					</td>
				</tr>
				<tr>
					<td bgColor="#ffffff" align="center">
						<br>
						<asp:RadioButtonList id="rbtnAppeal" Runat="server" RepeatDirection="Horizontal">
							<asp:ListItem Value="1">ͨ��</asp:ListItem>
							<asp:ListItem Value="2">�ܾ�</asp:ListItem>
							<asp:ListItem Value="3">����</asp:ListItem>
						</asp:RadioButtonList>
					</td>
				</tr>
			</table>
		</td>
		<td width="70%" valign="top">
			<table cellSpacing="1" cellPadding="0" width="100%" border="1">
				<tr bgColor="#ffffff">
					<td align="center" background="IMAGES/Page/bk_white.gif" bgColor="#eeeeee" colSpan="2"
						height="20" valign="top">��̨����(
						<asp:label id="lblfid" runat="server" ForeColor="Red"></asp:label>)</td>
				</tr>
				<tr bgColor="#ffffff">
					<td width="25%" bgColor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;�û��ʺ�</td>
					<td width="75%">&nbsp;
						<asp:label id="labFQQid" runat="server" ForeColor="#8080FF"></asp:label></td>
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;֤������</td>
					<td>&nbsp;
						<asp:label id="labFcreid" runat="server" ForeColor="#8080FF"></asp:label></td>
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;֤������</td>
					<td>&nbsp;
						<asp:label id="labFcre_type" runat="server" ForeColor="#8080FF"></asp:label></td>
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;����</td>
					<td>&nbsp;
						<asp:label id="labFtruename" runat="server" ForeColor="#8080FF"></asp:label></td>
				</tr>
				<tr>
					<td align="center" background="IMAGES/Page/bk_white.gif" bgColor="#eeeeee" colSpan="4"
						height="20">�ܾ�ԭ��</td>
				</tr>
				<tr>
					<td colspan="2" bgColor="#ffffff">
						<asp:checkboxlist id="RejectReason" Runat="server" RepeatLayout="Flow">
							<asp:ListItem Value="1">δ�ṩ���֤ɨ���</asp:ListItem>
							<asp:ListItem Value="2">�ϴ���ɨ���������������������Ч</asp:ListItem>
							<asp:ListItem Value="3">֤��������ԭע��֤�����벻��</asp:ListItem>
							<asp:ListItem Value="4">�ϴ���ɨ�����Ƹ�ͨע�����ϲ���</asp:ListItem>
							<asp:ListItem Value="5">��֧�ֽ������޸�Ϊ�س�</asp:ListItem>
							<asp:ListItem Value="0">����ԭ��</asp:ListItem>
						</asp:checkboxlist><asp:textbox id="tbFCheckInfo" runat="server" Width="100%" TextMode="MultiLine"></asp:textbox>
					</td>
				</tr>
				<tr>
					<td align="center" background="IMAGES/Page/bk_white.gif" bgColor="#eeeeee" colSpan="2"
						height="20">��ע</td>
				</tr>
				<tr bgColor="#ffffff">
					<td colSpan="2"><asp:textbox id="tbComment" runat="server" Width="100%" TextMode="MultiLine" BorderStyle="Groove"
							Height="60px"></asp:textbox></td>
				</tr>
			</table>
		</td>
	</tr>
</table>
<div id="Divimg">
	<asp:Image id="imgOther" runat="server" Height="250px" Width="300px" style="DISPLAY:none"></asp:Image>
</div>
<script> 
function TurnBig(p)
 {
	p.style.display='block';
	if(parseInt(p.style.width) < 1000)
	{
		p.style.width = parseInt(p.style.width) + 60  + 'px';
		p.style.height = parseInt(p.style.height) + 50  + 'px';
	}
 }
 function TurnShort(p)
 {
	p.style.display='block';
	if(parseInt(p.style.width) > 300)
	{
		p.style.width = parseInt(p.style.width) - 60  + 'px';
		p.style.height = parseInt(p.style.height) - 50  + 'px';
	}
 }
 function TurnBack(p)
 {
	p.style.display='none';
 }
</script>
