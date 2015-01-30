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
						align="center">身份证扫描件</td>
				</tr>
				<tr id="tr_picSelect" runat="server" >
					<td align="center"><span><a style="COLOR: blue; CURSOR: hand; TEXT-DECORATION: underline" runat="server" id="hbtn_fPic">第一张图片</a>&nbsp;&nbsp;<a style="COLOR: blue; CURSOR: hand; TEXT-DECORATION: underline" name="top" runat="server" id="hbtn_sPic">第二张图片</a></span></td>
				</tr>
				<tr bgColor="#ffffff">
					<td style="PADDING-BOTTOM: 3px; PADDING-LEFT: 3px; PADDING-RIGHT: 3px; PADDING-TOP: 3px"
						height="20" align="center"><asp:image id="Image1" Width="370px" Height="300px" runat="server" ></asp:image><asp:image id="Image2" runat="server" Width="370px" Height="300px"></asp:image></td>
				</tr>
				<tr>
					<td bgColor="#ffffff" align="center"><button 
            onmousedown="TurnBig(<%=imgOther.ClientID%>);" 
            >放大</button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<button 
            onmousedown="TurnShort(<%=imgOther.ClientID%>);" 
            >缩小</button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<button 
            onmousedown="TurnBack(<%=imgOther.ClientID%>);" 
            >还原</button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<button 
            onmousedown="RotaLeft(<%=imgOther.ClientID%>);" 
            >左旋转</button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<button 
            onmousedown="RotaRight(<%=imgOther.ClientID%>);" 
            >右旋转</button>
					</td>
				</tr>
				<tr>
					<td bgColor="#ffffff" align="center"><br>
						<asp:RadioButton GroupName="rbtnSubState" Text="通过" Runat="server" ID="rbtnOK"></asp:RadioButton>
						<asp:RadioButton GroupName="rbtnSubState" Text="拒绝" Runat="server" ID="rbtnReject"></asp:RadioButton>
						<asp:RadioButton GroupName="rbtnSubState" Text="删除" Runat="server" ID="rbtnDelete"></asp:RadioButton>
						<asp:RadioButton GroupName="rbtnSubState" Text="挂起" Runat="server" ID="rbtnSub"></asp:RadioButton>
						<!--
						<asp:radiobuttonlist id="rbtnAppeal" RepeatDirection="Horizontal" Runat="server">
							<asp:ListItem Value="1">通过</asp:ListItem>
							<asp:ListItem Value="2">拒绝</asp:ListItem>
							<asp:ListItem Value="3">删除</asp:ListItem>
							<asp:ListItem Value="4">挂起</asp:ListItem>
						</asp:radiobuttonlist></td>
						--></td>
				<tr>
					<td bgColor="#eeeeee" height="20" background="IMAGES/Page/bk_white.gif" align="center">申诉原因</td>
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
						align="center">备注</td>
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
						width="50%" colSpan="2" align="center">后台资料(
                        <asp:label id="lbldb" runat="server" ForeColor="Red"></asp:label>
                        <asp:label id="lbltb" runat="server" ForeColor="Red"></asp:label>
                        <asp:label id="lblftype" runat="server" ForeColor="Red"></asp:label>
						<asp:label id="lblfid" runat="server" ForeColor="Red"></asp:label>)</td>
					<td bgColor="#eeeeee" background="IMAGES/Page/bk_white.gif" width="50%" colSpan="2"
						align="center">用户提交资料(
						<asp:label id="labFTypeName" runat="server" ForeColor="Red"></asp:label>)</td>
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee" height="20" width="25%">&nbsp;&nbsp;&nbsp;&nbsp;用户帐号</td>
					<td width="25%">&nbsp;
						<asp:label id="labFQQid" runat="server" ForeColor="#8080FF"></asp:label></td>
					<td width="25%">&nbsp;
						<asp:label id="labIsAnswer" runat="server" ForeColor="#00C000"></asp:label></td>
					<td bgColor="#eeeeee" width="25%">&nbsp;&nbsp;&nbsp;&nbsp;原密保答案</td>
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;证件号码</td>
					<td>&nbsp;
						<asp:label id="labFcreid" runat="server" ForeColor="#8080FF"></asp:label></td>
					<td>&nbsp;
						<asp:label id="cre_id" runat="server" ForeColor="#00C000"></asp:label></td>
					<td bgColor="#eeeeee">&nbsp;&nbsp;&nbsp;&nbsp;证件号码</td>
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;证件类型</td>
					<td>&nbsp;
						<asp:label id="labFcre_type" runat="server" ForeColor="#8080FF"></asp:label></td>
					<td>&nbsp;
						<asp:label id="cre_type" runat="server" ForeColor="#00C000"></asp:label></td>
					<td bgColor="#eeeeee">&nbsp;&nbsp;&nbsp;&nbsp;证件类型</td>
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;邮箱地址</td>
					<td>&nbsp;
						<asp:label id="labFEmail" runat="server" ForeColor="#8080FF"></asp:label></td>
					<td>&nbsp;
						<asp:label id="email" runat="server" ForeColor="#00C000"></asp:label></td>
					<td bgColor="#eeeeee">&nbsp;&nbsp;&nbsp;&nbsp;邮箱地址</td>
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;名称</td>
					<td>&nbsp;
						<asp:label id="labFtruename" runat="server" ForeColor="#8080FF"></asp:label></td>
					<td>&nbsp;
						<asp:label id="old_name" runat="server" ForeColor="#00C000"></asp:label></td>
					<td bgColor="#eeeeee">&nbsp;&nbsp;&nbsp;&nbsp;名称</td>
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;总金额</td>
					<td>&nbsp;
						<asp:label id="labFbalance" runat="server" ForeColor="#8080FF"></asp:label></td>
					<td>&nbsp;
						<asp:label id="new_name" runat="server" ForeColor="#00C000"></asp:label></td>
					<td bgColor="#eeeeee">&nbsp;&nbsp;&nbsp;&nbsp;新名称</td>
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;冻结金额</td>
					<td>&nbsp;
						<asp:label id="labFCon" runat="server" ForeColor="#8080FF"></asp:label></td>
					<td>&nbsp;
						<asp:label id="clear_pps" runat="server" ForeColor="#00C000"></asp:label></td>
					<td bgColor="#eeeeee">&nbsp;&nbsp;&nbsp;&nbsp;是否清空密保资料</td>
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;银行帐号</td>
					<td>&nbsp;
						<asp:label id="labFBankAcc" runat="server" ForeColor="#8080FF"></asp:label></td>
					<td>&nbsp;
						<asp:label id="lblBindMobileUser" runat="server" ForeColor="#00C000"></asp:label></td>
					<td bgColor="#eeeeee">&nbsp;&nbsp;&nbsp;&nbsp;绑定手机</td>
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;免审核标准分</td>
					<td>&nbsp;
						<asp:label id="lblstandard_score" runat="server" ForeColor="#8080FF"></asp:label></td>
					<td>&nbsp;
						<asp:label id="lblBindMailUser" runat="server" ForeColor="#00C000"></asp:label></td>
					<td bgColor="#eeeeee">&nbsp;&nbsp;&nbsp;&nbsp;绑定邮箱</td>
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;实际得分</td>
					<td>&nbsp;
						<asp:label id="lblscore" runat="server" ForeColor="#8080FF"></asp:label></td>
					<td>&nbsp;
						<asp:label id="labFstatename" runat="server" ForeColor="#00C000"></asp:label></td>
					<td bgColor="#eeeeee">&nbsp;&nbsp;&nbsp;&nbsp;审批当前状态</td>
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;得分明细</td>
					<td>&nbsp;
						<asp:label id="lbldetail_score" runat="server" ForeColor="#8080FF"></asp:label></td>
					<td>&nbsp;
						<asp:label id="new_cre_id" runat="server" ForeColor="#00C000"></asp:label></td>
					<td bgColor="#eeeeee">&nbsp;&nbsp;&nbsp;&nbsp;新证件号码</td>
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;风控标记</td>
					<td >&nbsp;
						<asp:label id="lblrisk_result" runat="server" ForeColor="#8080FF"></asp:label></td>
                    <td>&nbsp;
						<asp:label id="lbauthenState" runat="server" ForeColor="#00C000"></asp:label></td>
					<td bgColor="#eeeeee">&nbsp;&nbsp;&nbsp;&nbsp;实名认证</td>
				</tr>
				<tr>
					<td bgColor="#eeeeee" height="20" background="IMAGES/Page/bk_white.gif" colSpan="4"
						align="center">拒绝原因</td>
				</tr>
				<tr>
					<td colSpan="4">
						<table>
							<tr>
								<td style="WIDTH: 320px" width="320">
									<div style="MARGIN: 0px 0px 30px; FONT-SIZE: 15px"><asp:checkbox id="cb_1" runat="server" Text="未显示图片" onfocus="Init()"></asp:checkbox></div>
									<div style="MARGIN: 0px 0px 30px; FONT-SIZE: 15px"><asp:checkbox id="cb_5" runat="server" Text="未提供身份证扫描件"></asp:checkbox></div>
									<div style="MARGIN: 0px 0px 30px; FONT-SIZE: 15px"><asp:checkbox id="cb_2" runat="server" Text="上传的扫描件不够完整、清晰、有效"></asp:checkbox></div>
									<div style="MARGIN: 0px 0px 30px; FONT-SIZE: 15px"><asp:checkbox id="cb_3" runat="server" Text="证件号码与原注册证件号码不符"></asp:checkbox></div>
									<div style="MARGIN: 0px 0px 30px; FONT-SIZE: 15px"><asp:checkbox id="cb_4" runat="server" Text="实名认证有效年限为16至80周岁"></asp:checkbox></div>
									<div style="MARGIN: 0px 0px 30px; FONT-SIZE: 15px"><asp:checkbox id="cb_0" runat="server" Text="其它原因"></asp:checkbox></div>
								</td>
								<td width="350">
									<div style="DISPLAY: none" id="div_cbxl2" runat="server"><asp:checkboxlist id="cbxl_2" Runat="server" CellSpacing="0" CellPadding="0" CssClass="MARGIN: 0px"
											Font-Size="10">
											<asp:ListItem Value="扫描件整体不清晰"></asp:ListItem>
											<asp:ListItem Value="扫描件上姓名不清晰"></asp:ListItem>
											<asp:ListItem Value="扫描件上证件号码不清晰"></asp:ListItem>
											<asp:ListItem Value="扫描件上地址模糊"></asp:ListItem>
											<asp:ListItem Value="扫描件上头像照片不清晰"></asp:ListItem>
											<asp:ListItem Value="扫描件不完整"></asp:ListItem>
											<asp:ListItem Value="扫描件非彩色"></asp:ListItem>
											<asp:ListItem Value="扫描件疑为网络照片"></asp:ListItem>
											<asp:ListItem Value="扫描件经过涂改无效"></asp:ListItem>
											<asp:ListItem Value="扫描件经过其他软件修改过"></asp:ListItem>
											<asp:ListItem Value="扫描件不在有效期内（已过期）"></asp:ListItem>
											<asp:ListItem Value="扫描件颜色有较大色差"></asp:ListItem>
											<asp:ListItem Value="扫描件是反面"></asp:ListItem>
										</asp:checkboxlist></div>
									<div style="DISPLAY: none" id="div_cbx_3" runat="server"><asp:checkbox id="cbx_detail_cbx3" Font-Size="10" Runat="server" Text="请您重新提交申述表时，上传账户资金来源截图。请参考“特殊申诉找回”指引：&#13;&#10;&#9;&#9;&#9;&#9;&#9;&#9;&#9;&#9;&#9;http://help.tenpay.com/cgi-bin/helpcenter/help_center.cgi?id=2232&amp;type=0"></asp:checkbox></div>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td bgColor="#eeeeee" height="20" background="IMAGES/Page/bk_white.gif" colSpan="4"
						align="center">其他原因</td>
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
