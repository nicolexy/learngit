<%@ Page language="c#" Codebehind="MerchantInfoModifyQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.MerchantInfoModifyQuery" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>MerchantInfoModifyQuery</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		<script src="../SCRIPTS/Local.js"></script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" style="LEFT: 5%; POSITION: absolute; TOP: 5%" cellSpacing="1" cellPadding="1"
				width="850" border="1">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="6"><FONT face="����"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;�̻���Ϣ�޸Ĳ�ѯ</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>����Ա����: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" Width="73px" ForeColor="Red"></asp:label></SPAN></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 100px; HEIGHT: 27px" align="right">�̻��ţ�</TD>
					<TD style="WIDTH: 163px; HEIGHT: 27px" align="left">
						<asp:TextBox id="txtspid" runat="server"></asp:TextBox></TD>
					<TD style="WIDTH: 96px; HEIGHT: 27px" align="right">C�˺ţ�</TD>
					<TD style="WIDTH: 180px; HEIGHT: 27px" align="left">
						<asp:TextBox id="txtAccountC" runat="server"></asp:TextBox></TD>
				</TR>
                <TR>
					<TD style="WIDTH: 100px; HEIGHT: 27px" align="right">�޸����ͣ�</TD>
					<TD style="WIDTH: 163px; HEIGHT: 27px">
						<asp:DropDownList id="ddlModType" runat="server" Width="150px" AutoPostBack="True">
						</asp:DropDownList></TD>
                    <TD align="center" colspan="2"><asp:Button id="btnSearch" runat="server" Text="�� ѯ" onclick="btnSearch_Click"></asp:Button></TD>
				</TR>
			</TABLE>
			<div style="LEFT: 5%; OVERFLOW: auto; WIDTH:90%; POSITION: absolute; TOP: 170px; HEIGHT: 800px">
				<table cellSpacing="0" cellPadding="0" border="0" Width=100%>
					<TR>
						<TD vAlign="top" align="center"><asp:datagrid id="dgList" runat="server" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
								HorizontalAlign="Center" AutoGenerateColumns="False" GridLines="Horizontal" CellPadding="1" BackColor="White"
								BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF" AllowPaging="True" Width="100%" PageSize="10">
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="TaskId" HeaderText="���">
										<HeaderStyle Width="70px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="SPID" HeaderText="Spid">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="CompanyName" HeaderText="�̻�����">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="ApplyUser" HeaderText="������">
										<HeaderStyle Width="80px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="ApplyTime" HeaderText="����ʱ��">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="CheckUser" HeaderText="�����">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="CheckTime" HeaderText="���ʱ��">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="AmendStateName" HeaderText="��ǰ״̬">
										<HeaderStyle Width="80px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="ApplyTypeName" HeaderText="�޸�����">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
								</Columns>
								<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
							</asp:datagrid></TD>
					</TR>
				</table>
			</div>
		</form>
	</body>
</HTML>
