<%@ Control Language="c#" AutoEventWireup="True" Codebehind="UserClassCheckControl.ascx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.Control.UserClassCheckControl" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script src="../SCRIPTS/Local.js"></script>
<table cellSpacing="0" cellPadding="0" width="100%" border="0">
	<tr>
		<td width="30%" valign="top">
			<table cellSpacing="1" cellPadding="0" width="100%" border="1">
				<tr bgColor="#ffffff">
					<td align="center" width="100%" background="IMAGES/Page/bk_white.gif" bgColor="#eeeeee"
						height="20">身份证扫描件</td>
				</tr>
				
				<tr bgColor="#ffffff">
					<td align="center" height="20" style="PADDING-BOTTOM: 3px; PADDING-LEFT: 3px; PADDING-RIGHT: 3px; PADDING-TOP: 3px">
                    <asp:image id="Image1" runat="server" Height="255px" Width="330px"></asp:image><asp:image id="Image2" runat="server" Height="255px" Width="330px"></asp:image>
                    </td>
				</tr>
				<tr>
					<td bgColor="#ffffff" align="center"><button onmousedown="TurnBig(<%=imgOther.ClientID%>);" >放大</button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						<button onmousedown="TurnShort(<%=imgOther.ClientID%>);" >缩小</button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						<button onmousedown="TurnBack(<%=imgOther.ClientID%>);" >还原</button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						<button onmousedown="RotaLeft(<%=imgOther.ClientID%>);" >左旋转</button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						<button onmousedown="RotaRight(<%=imgOther.ClientID%>);" >右旋转</button>
					</td>
				</tr>
				<tr>
					<td bgColor="#ffffff" align="center">
						<br>
						<asp:RadioButtonList id="rbtnAppeal" Runat="server" RepeatDirection="Horizontal">
							<asp:ListItem Value="1">通过</asp:ListItem>
							<asp:ListItem Value="2">拒绝</asp:ListItem>
							<asp:ListItem Value="3">挂起</asp:ListItem>
						</asp:RadioButtonList>
					</td>
				</tr>
			</table>
		</td>
		<td width="70%" valign="top">
			<table cellSpacing="1" cellPadding="0" width="100%" border="1">
				<tr bgColor="#ffffff">
					<td align="center" background="IMAGES/Page/bk_white.gif" bgColor="#eeeeee" colSpan="2"
						height="20" valign="top">后台资料(
						<asp:label id="lblfid" runat="server" ForeColor="Red"></asp:label>)</td>
				</tr>
				<tr bgColor="#ffffff">
					<td width="25%" bgColor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;用户帐号</td>
					<td width="75%">&nbsp;
						<asp:label id="labFQQid" runat="server" ForeColor="#8080FF"></asp:label></td>
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;证件号码</td>
					<td>&nbsp;
						<asp:label id="labFcreid" runat="server" ForeColor="#8080FF"></asp:label></td>
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;证件类型</td>
					<td>&nbsp;
						<asp:label id="labFcre_type" runat="server" ForeColor="#8080FF"></asp:label></td>
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;名称</td>
					<td>&nbsp;
						<asp:label id="labFtruename" runat="server" ForeColor="#8080FF"></asp:label></td>
				</tr>
				<tr>
					<td align="center" background="IMAGES/Page/bk_white.gif" bgColor="#eeeeee" colSpan="4"
						height="20">拒绝原因</td>
				</tr>
				<tr>
					<td colspan="2" bgColor="#ffffff">
						<asp:checkboxlist id="RejectReason" Runat="server" RepeatLayout="Flow">
							<asp:ListItem Value="1">未提供身份证扫描件</asp:ListItem>
							<asp:ListItem Value="2">上传的扫描件不够完整、清晰、有效</asp:ListItem>
							<asp:ListItem Value="3">证件号码与原注册证件号码不符</asp:ListItem>
							<asp:ListItem Value="4">上传的扫描件与财付通注册资料不符</asp:ListItem>
							<asp:ListItem Value="5">不支持将姓名修改为呢称</asp:ListItem>
							<asp:ListItem Value="0">其它原因</asp:ListItem>
						</asp:checkboxlist><asp:textbox id="tbFCheckInfo" runat="server" Width="100%" TextMode="MultiLine"></asp:textbox>
					</td>
				</tr>
				<tr>
					<td align="center" background="IMAGES/Page/bk_white.gif" bgColor="#eeeeee" colSpan="2"
						height="20">备注</td>
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
