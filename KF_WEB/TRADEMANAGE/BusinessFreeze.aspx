<%@ Page language="c#" Codebehind="BusinessFreeze.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.BusinessFreeze" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>BusinessFreeze</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		 p.font1 {
                font-weight:bold;
                font-size:14px;
                color:red;
		    }
    
    </style>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE style="POSITION: absolute; TOP: 5%; LEFT: 5%" cellSpacing="1" cellPadding="1" width="820"
				border="1">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colspan="4"><FONT face="宋体"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;商户冻结</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></TD>
				</TR>
				<TR>
					<td align="right" width="25%"><asp:label id="Label5" runat="server">商户号</asp:label></td>
					<TD width="25%"><asp:textbox id="txtFspid" runat="server"></asp:textbox></TD>
					<TD width="45%"><asp:CheckBox ID="cbxFreeze" Runat="server" Text="暂停结算"></asp:CheckBox>
						&nbsp;<asp:CheckBox ID="cbxPay" Runat="server" Text="关闭支付"></asp:CheckBox>
                        &nbsp;<asp:CheckBox ID="cbxAccLose" Runat="server" Text="账号挂失"></asp:CheckBox>
                        &nbsp;<asp:CheckBox ID="cbxCloseAgent" Runat="server" Text="关闭中介"></asp:CheckBox>
                        </TD>
					<TD align="center" width="30%"><asp:Button ID="btnFreeze" Runat="server" Text="提 交" onclick="btnFreeze_Click"></asp:Button>&nbsp;&nbsp;<asp:Button ID="btnQuery" Runat="server" Text="查 询" onclick="btnQuery_Click"></asp:Button></TD>
				</TR>
				<tr>
					<td align="right" width="25%"><asp:label id="Label2" runat="server">原因</asp:label></td>
					<td colspan="3"><asp:TextBox ID="txtReason" Runat="server" TextMode="MultiLine" Width="440px"></asp:TextBox>
					</td>
				</tr>
			</TABLE>
              <div id="RemaindDiv" runat="server" visible="true" style="LEFT: 5.02%; WIDTH: 85%; POSITION: absolute; TOP: 25%;">
                <p class="font1">提醒：<br />请先在“结算规则查询”中核实是否是T+0，若是T+0，必须在“结算规则查询”中先关闭T+0，且次日再操作“暂停结算”。非T+0商户可直接操作“暂停结算”。
                </p>
            </div>
            <TABLE id="Table2" visible="false" style="Z-INDEX: 102; LEFT: 5.02%; WIDTH: 85%; POSITION: absolute; TOP: 25%;"
				cellSpacing="1" cellPadding="1" width="808" border="1" runat="server">
				<TR>
					<TD colspan="2" vAlign="top"><asp:datagrid id="DataGrid1" runat="server" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px"
							BackColor="White" CellPadding="3" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
                                <asp:BoundColumn DataField="companyname" HeaderText="商户名称"></asp:BoundColumn>
								<asp:BoundColumn DataField="applyuser" HeaderText="申请人"></asp:BoundColumn>
                                <asp:BoundColumn DataField="checkuser" HeaderText="审核人"></asp:BoundColumn>
                                <asp:BoundColumn DataField="amendtype_str" HeaderText="类型"></asp:BoundColumn>
                                <asp:BoundColumn DataField="applytime" HeaderText="申请时间"></asp:BoundColumn>
                                <asp:BoundColumn DataField="checktime" HeaderText="审核时间"></asp:BoundColumn>
                                <asp:BoundColumn DataField="amendstate_str" HeaderText="当前状态"></asp:BoundColumn>
                                <asp:BoundColumn DataField="applyresult" HeaderText="原因" Visible="false"></asp:BoundColumn>
								<asp:ButtonColumn Text="详情" HeaderText="详情" CommandName="detail"></asp:ButtonColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
                <tr id="reasonTR">
                  <td width="25%" align="right">申请原因：</td>
                  <td><asp:TextBox ID="txtApplyResult" Runat="server" TextMode="MultiLine" Width="440px"></asp:TextBox></td>
                </tr>
			</TABLE>
		</form>
	</body>
</HTML>
