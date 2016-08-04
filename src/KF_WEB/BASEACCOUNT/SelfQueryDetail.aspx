<%@ Page language="c#" Codebehind="SelfQueryDetail.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.SelfQueryDetail" %>
<%@ Register TagPrefix="uc1" TagName="FunctionControl" Src="../Control/FunctionControl.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>SelfQueryDetail</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		<script src="../SCRIPTS/Local.js"></script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table cellSpacing="0" cellPadding="0" width="820" border="0" align="center">
				<tr>
					<td colSpan="4">基本信息
					</td>
				</tr>
				<tr>
					<td colSpan="4">
						<hr align="left" width="100%" color="#cccccc" SIZE="1">
					</td>
				</tr>
				<tr>
					<td width="20%">商户名称<font color="red" size="1">*</font>:</td>
					<td width="30%"><asp:label id="CompanyName_value" runat="server"></asp:label></td>
					<td width="20%">商户地址<font color="red" size="1">*</font>:</td>
					<td width="30%"><asp:label id="CompanyAddress_value" runat="server"></asp:label></td>
				</tr>
				<tr>
					<td>网站域名<font color="red" size="1">*</font>:
					</td>
					<td><asp:label id="WWWAdress_value" runat="server"></asp:label></td>
					<td>邮政编码<font color="red" size="1">*</font>:</td>
					<td><asp:label id="Postalcode_value" runat="server"></asp:label></td>
				</tr>
				<tr>
					<td>行业类别:</td>
					<td><asp:label id="TradeType_value" runat="server"></asp:label></td>
					<td>推荐人:</td>
					<td><asp:label id="SuggestUser_value" runat="server"></asp:label></td>
				</tr>
				<tr>
					<td>所属区域<font color="red" size="1">*</font>:</td>
					<TD><asp:label id="AreaID_value" runat="server"></asp:label></TD>
					<td>联系人<font color="red" size="1">*</font>:</td>
					<td><asp:label id="ContactUser_value" runat="server"></asp:label></td>
				</tr>
				<tr>
					<td>所属BD<font color="red" size="1">*</font>:</td>
					<TD><asp:label id="BDID_value" runat="server"></asp:label></TD>
					<td>联系电话<font color="red" size="1">*</font>:</td>
					<td><asp:label id="ContactPhone_value" runat="server"></asp:label></td>
				</tr>
				<tr>
					<td>Email<font color="red" size="1">*</font>:</td>
					<TD><asp:label id="ContactEmail_value" runat="server"></asp:label></TD>
					<td>联系手机</SPAN>:</td>
					<td><asp:label id="ContactMobile_value" runat="server"></asp:label></td>
				</tr>
				<tr>
					<td>QQ:</td>
					<TD ><asp:label id="ContactQQ_value" runat="server"></asp:label></TD>
                    <td>合作形式:</td>
					<TD ><asp:label id="txtBDSpidType" runat="server"></asp:label></TD>
				</tr>
                <tr>
					<td>接入形式:</td>
					<TD ><asp:label id="txtInType" runat="server"></asp:label></TD>
                    <td>微信appid:</td>
					<TD ><asp:label id="txtAPPID" runat="server"></asp:label></TD>
				</tr>
                <tr>
					<td>是否需缴保证金:</td>
					<TD ><asp:label id="txtIsWXBeil" runat="server"></asp:label></TD>
                    <td>保证金额度:</td>
					<TD ><asp:label id="txtWXBeil" runat="server"></asp:label></TD>
				</tr>
                <tr>
					<td>固定保证金:</td>
					<TD ><asp:label id="txtChangelessBeil" runat="server"></asp:label></TD>
                    <td>固保缴纳方式:</td>
					<TD ><asp:label id="txtChangelessBeilType" runat="server"></asp:label></TD>
				</tr>
                <tr>
					<td>商户现金账号:</td>
					<TD ><asp:label id="txtQQID" runat="server"></asp:label></TD>
                    <td>商户号:</td>
					<TD ><asp:label id="txtSPID" runat="server"></asp:label></TD>
				</tr>
                 <tr>
					<td>上级商户:</td>
					<TD ><asp:label id="txtSuperiorSpid" runat="server"></asp:label></TD>
                    <td></td>
					<TD ><asp:label id="Label2" runat="server"></asp:label></TD>
				</tr>

				<tr>
					<td colSpan="4">
						<hr align="left" width="100%" color="#cccccc" SIZE="1">
					</td>
				</tr>
				<tr>
					<td colSpan="4">结算基本信息
					</td>
				</tr>
				<tr>
					<td colSpan="4">
						<hr align="left" width="100%" color="#cccccc" SIZE="1">
					</td>
				</tr>
				<tr>
					<td>开户类型<font color="red" size="1">*</font>:</td>
					<TD><asp:label id="UserType_value" runat="server"></asp:label></TD>
					<td>开户人身份证<font color="red" size="1">*</font>:</td>
					<TD><asp:label id="IdentityCardNum_value" runat="server"></asp:label></TD>
				</tr>
				<TR>
					<td>开户名称<font color="red" size="1">*</font>:</td>
					<td><asp:label id="BankUserName_value" runat="server"></asp:label></td>
					<td>开户省份<font color="red" size="1">*</font>:</td>
					<TD><asp:label id="AreaCode_value" runat="server"></asp:label></TD>
				</TR>
				<tr>
					<td>开户银行<font color="red" size="1">*</font>:</td>
					<td><asp:label id="BankType_value" runat="server"></asp:label></td>
					<td>开户城市<font color="red" size="1">*</font>:</td>
					<TD><asp:label id="CityCode_value" runat="server"></asp:label></TD>
				</tr>
				<tr>
					<td>开户行<font color="red" size="1">*</font>:</td>
					<td><asp:label id="BankName_value" runat="server"></asp:label></td>
					<td>银行帐号<font color="red" size="1">*</font>:</td>
					<TD><asp:label id="BankAccounts_value" runat="server"></asp:label></TD>
				</tr>
				<TR>
					<td rowSpan="2">结算费率:</td>
					<TD colSpan="3" rowSpan="2"><asp:textbox id="JSMemo_value" runat="server" TextMode="MultiLine" Width="500px" Enabled="False"></asp:textbox></TD>
				</TR>
				<tr>
					<td colSpan="4"></td>
				</tr>
				<TR>
					<TD>开通功能<font color="red" size="1">*</font>:
					</TD>
					<TD colSpan="3"><uc1:functioncontrol id="NewFunction" runat="server"></uc1:functioncontrol></TD>
				</TR>
				<TR>
					<TD>其它补充信息:
					</TD>
					<TD colSpan="3"><asp:label id="OtherMemo_value" runat="server"></asp:label></TD>
				</TR>
				<tr>
					<td colSpan="4">
						<hr align="left" width="100%" color="#cccccc" SIZE="1">
					</td>
				</tr>
				<tr>
					<td colSpan="4">接入信息
					</td>
				</tr>
                  <tr>
					<td colSpan="4"><font color="red">审批信息</font>
					</td>
				</tr>
                <tr>
                <td colspan="4">
                    <table border="1" cellpadding="1" cellspacing="1" width="100%">
                        <tr>
                        <td width="100%"><asp:datagrid id="dgList" runat="server" BorderColor="#E7E7FF" border="1"
								BorderWidth="1px"  CellPadding="1" GridLines="Horizontal" AutoGenerateColumns="False" HorizontalAlign="Center"
								HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                 AllowPaging="True" Width="100%" PageSize="10">
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle HorizontalAlign="Center" ></ItemStyle>
								<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="CheckUser" HeaderText="处理人信息">
									</asp:BoundColumn>
									<asp:BoundColumn DataField="CheckTime" HeaderText="操作时间">
									</asp:BoundColumn>
									<asp:BoundColumn DataField="CheckMemo" HeaderText="操作备注">
									</asp:BoundColumn>
								</Columns>
								<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
							</asp:datagrid>
                        </td>
                        </tr>
                    </table>
                </td>
                </tr>
				<tr>
					<td colSpan="4">
						<hr align="left" width="100%" color="#cccccc" SIZE="1">
					</td>
				</tr>
				<tr>
					<td rowSpan="2">作废备注:</td>
					<TD colSpan="3" rowSpan="2"><asp:textbox id="ErrorMemo_value" runat="server" TextMode="MultiLine" Width="500px"></asp:textbox></TD>
				</tr>
				<tr>
				</tr>
				<tr>
					<td colspan="2"></td>
					<td><asp:Button ID="btnApprove" Runat="server" Text="审核同意" onclick="btnApprove_Click"></asp:Button></td>
					<td><asp:Button ID="btnReject" Runat="server" Text="审核拒绝" onclick="btnReject_Click"></asp:Button></td>
				</tr>
               
			</table>
		</form>
	</body>
</HTML>
