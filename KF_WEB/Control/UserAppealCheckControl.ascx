<%@ Control Language="c#" AutoEventWireup="True" Codebehind="UserAppealCheckControl.ascx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.Control.UserAppealCheckControl" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script src="../SCRIPTS/Local.js"></script>
<script type="text/javascript">
			function st2(e,t,x)
			{
				if(e.checked)
				{
					t.style.display = 'block';
					x.checked = true;
				}
				else
				{
					t.style.display = 'none';
				}
			}
			
			function st3(e,x)
			{
				if(e.checked)
				{
					x.checked = true;
				}
			}
			
			function st4(e,x)
			{
				e.style.display = 'block';
				x.style.display = 'none';
			}
</script>
<table border="0" cellSpacing="0" cellPadding="0" width="100%">
	<tr>
		<td vAlign="top" width="30%">
			<table border="1" cellSpacing="1" cellPadding="0" width="100%">
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee" height="20" background="IMAGES/Page/bk_white.gif" width="100%"
						align="center">���֤ɨ���</td>
				</tr>
				<tr id="tr_picSelect" runat="server" >
					<td align="center"><span><a style="COLOR: blue; CURSOR: hand; TEXT-DECORATION: underline" runat="server" id="hbtn_fPic">��һ��ͼƬ</a>&nbsp;&nbsp;<a style="COLOR: blue; CURSOR: hand; TEXT-DECORATION: underline" name="top" runat="server" id="hbtn_sPic">�ڶ���ͼƬ</a></span></td>
				</tr>
				<tr bgColor="#ffffff">
					<td style="PADDING-BOTTOM: 3px; PADDING-LEFT: 3px; PADDING-RIGHT: 3px; PADDING-TOP: 3px"
						height="20" align="center"><asp:image id="Image1" Width="370px" Height="300px" runat="server" ></asp:image><asp:image id="Image2" runat="server" Width="370px" Height="300px"></asp:image></td>
				</tr>
				<tr>
					<td bgColor="#ffffff" align="center"><button 
            onmousedown="TurnBig(<%=imgOther.ClientID%>);" 
            >�Ŵ�</button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<button 
            onmousedown="TurnShort(<%=imgOther.ClientID%>);" 
            >��С</button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<button 
            onmousedown="TurnBack(<%=imgOther.ClientID%>);" 
            >��ԭ</button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<button 
            onmousedown="RotaLeft(<%=imgOther.ClientID%>);" 
            >����ת</button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<button 
            onmousedown="RotaRight(<%=imgOther.ClientID%>);" 
            >����ת</button>
					</td>
				</tr>
				<tr>
					<td bgColor="#ffffff" align="center"><br>
						<asp:RadioButton GroupName="rbtnSubState" Text="ͨ��" Runat="server" ID="rbtnOK"></asp:RadioButton>
						<asp:RadioButton GroupName="rbtnSubState" Text="�ܾ�" Runat="server" ID="rbtnReject"></asp:RadioButton>
						<asp:RadioButton GroupName="rbtnSubState" Text="ɾ��" Runat="server" ID="rbtnDelete"></asp:RadioButton>
						<asp:RadioButton GroupName="rbtnSubState" Text="����" Runat="server" ID="rbtnSub"></asp:RadioButton>
						<!--
						<asp:radiobuttonlist id="rbtnAppeal" RepeatDirection="Horizontal" Runat="server">
							<asp:ListItem Value="1">ͨ��</asp:ListItem>
							<asp:ListItem Value="2">�ܾ�</asp:ListItem>
							<asp:ListItem Value="3">ɾ��</asp:ListItem>
							<asp:ListItem Value="4">����</asp:ListItem>
						</asp:radiobuttonlist></td>
						--></td>
				<tr>
					<td bgColor="#eeeeee" height="20" background="IMAGES/Page/bk_white.gif" align="center">����ԭ��</td>
				</tr>
				<tr bgColor="#ffffff">
					<td><asp:textbox id="tbReason" Width="100%" Height="120px" runat="server" BorderStyle="Groove" TextMode="MultiLine"></asp:textbox></td>
				</tr>
				<tr>
					<td>
						<p></p>
						<p></p>
					</td>
				</tr>
				<tr>
					<td bgColor="#eeeeee" height="20" background="IMAGES/Page/bk_white.gif" colSpan="4"
						align="center">��ע</td>
				</tr>
				<tr bgColor="#ffffff">
					<td colSpan="4"><asp:textbox id="tbComment" Width="100%" Height="120" runat="server" BorderStyle="Groove" TextMode="MultiLine"></asp:textbox></td>
				</tr>
			</table>
		</td>
		<td vAlign="top" width="70%">
			<table border="1" cellSpacing="1" cellPadding="0" width="100%">
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee" height="20" vAlign="top" background="IMAGES/Page/bk_white.gif"
						width="50%" colSpan="2" align="center">��̨����(
                        <asp:label id="lbldb" runat="server" ForeColor="Red"></asp:label>
                        <asp:label id="lbltb" runat="server" ForeColor="Red"></asp:label>
                        <asp:label id="lblftype" runat="server" ForeColor="Red"></asp:label>
						<asp:label id="lblfid" runat="server" ForeColor="Red"></asp:label>)</td>
					<td bgColor="#eeeeee" background="IMAGES/Page/bk_white.gif" width="50%" colSpan="2"
						align="center">�û��ύ����(
						<asp:label id="labFTypeName" runat="server" ForeColor="Red"></asp:label>)</td>
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee" height="20" width="25%">&nbsp;&nbsp;&nbsp;&nbsp;�û��ʺ�</td>
					<td width="25%">&nbsp;
						<asp:label id="labFQQid" runat="server" ForeColor="#8080FF"></asp:label></td>
					<td width="25%">&nbsp;
						<asp:label id="labIsAnswer" runat="server" ForeColor="#00C000"></asp:label></td>
					<td bgColor="#eeeeee" width="25%">&nbsp;&nbsp;&nbsp;&nbsp;ԭ�ܱ���</td>
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;֤������</td>
					<td>&nbsp;
						<asp:label id="labFcreid" runat="server" ForeColor="#8080FF"></asp:label></td>
					<td>&nbsp;
						<asp:label id="cre_id" runat="server" ForeColor="#00C000"></asp:label></td>
					<td bgColor="#eeeeee">&nbsp;&nbsp;&nbsp;&nbsp;֤������</td>
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;֤������</td>
					<td>&nbsp;
						<asp:label id="labFcre_type" runat="server" ForeColor="#8080FF"></asp:label></td>
					<td>&nbsp;
						<asp:label id="cre_type" runat="server" ForeColor="#00C000"></asp:label></td>
					<td bgColor="#eeeeee">&nbsp;&nbsp;&nbsp;&nbsp;֤������</td>
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;�����ַ</td>
					<td>&nbsp;
						<asp:label id="labFEmail" runat="server" ForeColor="#8080FF"></asp:label></td>
					<td>&nbsp;
						<asp:label id="email" runat="server" ForeColor="#00C000"></asp:label></td>
					<td bgColor="#eeeeee">&nbsp;&nbsp;&nbsp;&nbsp;�����ַ</td>
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;����</td>
					<td>&nbsp;
						<asp:label id="labFtruename" runat="server" ForeColor="#8080FF"></asp:label></td>
					<td>&nbsp;
						<asp:label id="old_name" runat="server" ForeColor="#00C000"></asp:label></td>
					<td bgColor="#eeeeee">&nbsp;&nbsp;&nbsp;&nbsp;����</td>
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;�ܽ��</td>
					<td>&nbsp;
						<asp:label id="labFbalance" runat="server" ForeColor="#8080FF"></asp:label></td>
					<td>&nbsp;
						<asp:label id="new_name" runat="server" ForeColor="#00C000"></asp:label></td>
					<td bgColor="#eeeeee">&nbsp;&nbsp;&nbsp;&nbsp;������</td>
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;������</td>
					<td>&nbsp;
						<asp:label id="labFCon" runat="server" ForeColor="#8080FF"></asp:label></td>
					<td>&nbsp;
						<asp:label id="clear_pps" runat="server" ForeColor="#00C000"></asp:label></td>
					<td bgColor="#eeeeee">&nbsp;&nbsp;&nbsp;&nbsp;�Ƿ�����ܱ�����</td>
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;�����ʺ�</td>
					<td>&nbsp;
						<asp:label id="labFBankAcc" runat="server" ForeColor="#8080FF"></asp:label></td>
					<td>&nbsp;
						<asp:label id="lblBindMobileUser" runat="server" ForeColor="#00C000"></asp:label></td>
					<td bgColor="#eeeeee">&nbsp;&nbsp;&nbsp;&nbsp;���ֻ�</td>
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;����˱�׼��</td>
					<td>&nbsp;
						<asp:label id="lblstandard_score" runat="server" ForeColor="#8080FF"></asp:label></td>
					<td>&nbsp;
						<asp:label id="lblBindMailUser" runat="server" ForeColor="#00C000"></asp:label></td>
					<td bgColor="#eeeeee">&nbsp;&nbsp;&nbsp;&nbsp;������</td>
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;ʵ�ʵ÷�</td>
					<td>&nbsp;
						<asp:label id="lblscore" runat="server" ForeColor="#8080FF"></asp:label></td>
					<td>&nbsp;
						<asp:label id="labFstatename" runat="server" ForeColor="#00C000"></asp:label></td>
					<td bgColor="#eeeeee">&nbsp;&nbsp;&nbsp;&nbsp;������ǰ״̬</td>
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;�÷���ϸ</td>
					<td>&nbsp;
						<asp:label id="lbldetail_score" runat="server" ForeColor="#8080FF"></asp:label></td>
					<td>&nbsp;
						<asp:label id="new_cre_id" runat="server" ForeColor="#00C000"></asp:label></td>
					<td bgColor="#eeeeee">&nbsp;&nbsp;&nbsp;&nbsp;��֤������</td>
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;��ر��</td>
					<td >&nbsp;
						<asp:label id="lblrisk_result" runat="server" ForeColor="#8080FF"></asp:label></td>
                    <td>&nbsp;
						<asp:label id="lbauthenState" runat="server" ForeColor="#00C000"></asp:label></td>
					<td bgColor="#eeeeee">&nbsp;&nbsp;&nbsp;&nbsp;ʵ����֤</td>
				</tr>
				<tr>
					<td bgColor="#eeeeee" height="20" background="IMAGES/Page/bk_white.gif" colSpan="4"
						align="center">�ܾ�ԭ��</td>
				</tr>
				<tr>
					<td colSpan="4">
						<table>
							<tr>
								<td style="WIDTH: 320px" width="320">
									<div style="MARGIN: 0px 0px 30px; FONT-SIZE: 15px"><asp:checkbox id="cb_1" runat="server" Text="δ��ʾͼƬ" onfocus="Init()"></asp:checkbox></div>
									<div style="MARGIN: 0px 0px 30px; FONT-SIZE: 15px"><asp:checkbox id="cb_5" runat="server" Text="δ�ṩ���֤ɨ���"></asp:checkbox></div>
									<div style="MARGIN: 0px 0px 30px; FONT-SIZE: 15px"><asp:checkbox id="cb_2" runat="server" Text="�ϴ���ɨ���������������������Ч"></asp:checkbox></div>
									<div style="MARGIN: 0px 0px 30px; FONT-SIZE: 15px"><asp:checkbox id="cb_3" runat="server" Text="֤��������ԭע��֤�����벻��"></asp:checkbox></div>
									<div style="MARGIN: 0px 0px 30px; FONT-SIZE: 15px"><asp:checkbox id="cb_4" runat="server" Text="ʵ����֤��Ч����Ϊ16��80����"></asp:checkbox></div>
									<div style="MARGIN: 0px 0px 30px; FONT-SIZE: 15px"><asp:checkbox id="cb_0" runat="server" Text="����ԭ��"></asp:checkbox></div>
								</td>
								<td width="350">
									<div style="DISPLAY: none" id="div_cbxl2" runat="server"><asp:checkboxlist id="cbxl_2" Runat="server" CellSpacing="0" CellPadding="0" CssClass="MARGIN: 0px"
											Font-Size="10">
											<asp:ListItem Value="ɨ������岻����"></asp:ListItem>
											<asp:ListItem Value="ɨ���������������"></asp:ListItem>
											<asp:ListItem Value="ɨ�����֤�����벻����"></asp:ListItem>
											<asp:ListItem Value="ɨ����ϵ�ַģ��"></asp:ListItem>
											<asp:ListItem Value="ɨ�����ͷ����Ƭ������"></asp:ListItem>
											<asp:ListItem Value="ɨ���������"></asp:ListItem>
											<asp:ListItem Value="ɨ����ǲ�ɫ"></asp:ListItem>
											<asp:ListItem Value="ɨ�����Ϊ������Ƭ"></asp:ListItem>
											<asp:ListItem Value="ɨ�������Ϳ����Ч"></asp:ListItem>
											<asp:ListItem Value="ɨ���������������޸Ĺ�"></asp:ListItem>
											<asp:ListItem Value="ɨ���������Ч���ڣ��ѹ��ڣ�"></asp:ListItem>
											<asp:ListItem Value="ɨ�����ɫ�нϴ�ɫ��"></asp:ListItem>
											<asp:ListItem Value="ɨ����Ƿ���"></asp:ListItem>
										</asp:checkboxlist></div>
									<div style="DISPLAY: none" id="div_cbx_3" runat="server"><asp:checkbox id="cbx_detail_cbx3" Font-Size="10" Runat="server" Text="���������ύ������ʱ���ϴ��˻��ʽ���Դ��ͼ����ο������������һء�ָ����&#13;&#10;&#9;&#9;&#9;&#9;&#9;&#9;&#9;&#9;&#9;http://help.tenpay.com/cgi-bin/helpcenter/help_center.cgi?id=2232&amp;type=0"></asp:checkbox></div>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td bgColor="#eeeeee" height="20" background="IMAGES/Page/bk_white.gif" colSpan="4"
						align="center">����ԭ��</td>
				</tr>
				<tr bgColor="#ffffff">
					<td colSpan="4"><asp:textbox id="tbFCheckInfo" Width="100%" Height="60" runat="server" TextMode="MultiLine"></asp:textbox></td>
				</tr>
			</table>
		</td>
	</tr>
</table>
<div id="Divimg"><asp:image style="DISPLAY: none" id="imgOther" Width="300px" Height="250px" runat="server"></asp:image></div>
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
</TR></TABLE>
