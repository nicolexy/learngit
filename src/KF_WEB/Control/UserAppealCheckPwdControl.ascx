<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserAppealCheckPwdControl.ascx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.Control.UserAppealCheckPwdControl" %>
<script src="../SCRIPTS/Local.js"></script>
<table border="0" cellSpacing="0" cellPadding="0" width="100%">
	<tr>		
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
						align="center">申诉原因</td>
                </tr>
                  <tr>
                    <td colSpan="4"><asp:textbox id="tbReason" Width="100%" Height="60" runat="server" BorderStyle="Groove" TextMode="MultiLine"></asp:textbox></td>
                </tr>
				<tr>
					<td bgColor="#eeeeee" height="20" background="IMAGES/Page/bk_white.gif" colSpan="4"
						align="center">拒绝原因[其他原因]</td>
				</tr>	
				<tr bgColor="#ffffff">
					<td colSpan="4"><asp:textbox id="tbFCheckInfo" Width="100%" Height="60" runat="server" TextMode="MultiLine"></asp:textbox></td>
				</tr>               
                <tr>
                    <td bgColor="#eeeeee" height="20" background="IMAGES/Page/bk_white.gif" colSpan="4"
						align="center">备注</td>
                </tr>
               <tr>
                   <td colSpan="4"><asp:textbox id="tbComment" Width="100%" Height="120" runat="server" BorderStyle="Groove" TextMode="MultiLine"></asp:textbox></td>
               </tr>              
                <tr>
                    <td bgColor="#ffffff" align="center" colSpan="4"><br>						
						<asp:RadioButton GroupName="rbtnSubState" Text="通过" Runat="server" ID="rbtnOK"></asp:RadioButton>
						<asp:RadioButton GroupName="rbtnSubState" Text="拒绝" Runat="server" ID="rbtnReject"></asp:RadioButton>
						<asp:RadioButton GroupName="rbtnSubState" Text="删除" Runat="server" ID="rbtnDelete"></asp:RadioButton>
						<asp:RadioButton GroupName="rbtnSubState" Text="挂起" Runat="server" ID="rbtnSub"></asp:RadioButton>
						</td>
                </tr>
			</table>
		</td>
	</tr>
</table>
