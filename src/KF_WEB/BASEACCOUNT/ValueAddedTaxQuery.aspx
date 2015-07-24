<%@ Page language="c#" Codebehind="ValueAddedTaxQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.ValueAddedTaxQuery" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>ValueAddedTaxQuery</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE style="POSITION: absolute; TOP: 5%; LEFT: 5%" id="Table1" border="1" cellSpacing="1"
				cellPadding="1" width="850">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="4"><FONT face="����"><FONT color="red"><IMG src="../IMAGES/Page/post.gif" width="20" height="16">&nbsp;&nbsp;�̻�Ӫ������ѯ</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>����Ա����: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" Width="73px" ForeColor="Red"></asp:label></SPAN></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 96px; HEIGHT: 27px" align="right">�̻��ţ�</TD>
					<TD style="WIDTH: 180px; HEIGHT: 27px" align="left"><asp:textbox id="txtSpid" runat="server"></asp:textbox></TD>
					<TD style="WIDTH: 100px; HEIGHT: 27px" align="right">�̻����ƣ�</TD>
					<TD style="WIDTH: 203px; HEIGHT: 27px"><asp:textbox id="txtCompanyName" runat="server"></asp:textbox></TD>
				</TR>
				<TR>
					<TD colSpan="4" align="center"><asp:button id="btnSearch" Width="80px" Runat="server" Text="��  ѯ" onclick="btnSearch_Click"></asp:button></TD>
				</TR>
			</TABLE>
			<div style="POSITION: absolute; WIDTH: 850px; HEIGHT: 800px; OVERFLOW: auto; TOP: 140px; LEFT: 5%">
				<table border="0" cellSpacing="0" cellPadding="0">
					<TR>
						<TD vAlign="top" align="center"><asp:datagrid id="dgList" runat="server" Width="850px" BorderColor="#E7E7FF" BorderStyle="None"
								BorderWidth="1px" BackColor="White" CellPadding="1" GridLines="Horizontal" AutoGenerateColumns="False" HorizontalAlign="Center"
								HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="SPID" HeaderText="�̻���">
										<HeaderStyle Width="150px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="CompanyName" HeaderText="�̻�����">
										<HeaderStyle Width="200px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="TaxInvoiceFlagStr" HeaderText="״̬">
										<HeaderStyle Width="300px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:ButtonColumn Text="�鿴" CommandName="Select"></asp:ButtonColumn>
								</Columns>
								<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
							</asp:datagrid></TD>
					</TR>
					<TR height="25">
						<TD><webdiyer:aspnetpager id="pager" runat="server" HorizontalAlign="right" PageSize="10" NumericButtonTextFormatString="[{0}]"
								SubmitButtonText="ת��" OnPageChanged="ChangePage" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0"
								ShowCustomInfoSection="left" NumericButtonCount="10" AlwaysShow="True" CustomInfoTextAlign="Center"></webdiyer:aspnetpager></TD>
					</TR>
				</table>
				<asp:Panel ID="PanelDetail" Runat="server" Visible=False>
					<TABLE border="1" cellSpacing="0" cellPadding="0" width="850" bordercolor="black" style="border-collapse:collapse;">
						<TR>
							<TD style="WIDTH: 100px; HEIGHT: 27px" align="right">�̻��ţ�</TD>
							<TD style="WIDTH: 163px; HEIGHT: 27px" align="left">
								<asp:label id="lblSpid" runat="server"></asp:label></TD>
							<TD style="WIDTH: 100px; HEIGHT: 27px" align="right">�̻����ƣ�</TD>
							<TD style="WIDTH: 163px; HEIGHT: 27px" align="left">
								<asp:label id="lblCompanyName" runat="server"></asp:label></TD>
							<TD style="WIDTH: 100px; HEIGHT: 27px" align="right">״̬��</TD>
							<TD style="WIDTH: 163px; HEIGHT: 27px" align="left">
								<asp:label id="lblTaxInvoiceFlag" runat="server"></asp:label></TD>
						</TR>
						<TR>
							<TD style="WIDTH: 100px; HEIGHT: 27px" align="right">��˰�����ͣ�</TD>
							<TD style="WIDTH: 163px; HEIGHT: 27px">
								<asp:label id="lblTaxerType" runat="server"></asp:label></TD>
							<TD style="WIDTH: 100px; HEIGHT: 27px" align="right">��Ʊ���ͣ�</TD>
							<TD style="WIDTH: 163px; HEIGHT: 27px" align="left">
								<asp:label id="lblTaxInvoiceType" runat="server"></asp:label></TD>
							<TD style="WIDTH: 130px; HEIGHT: 27px" align="right">��˾���ƣ���Ʊ̧ͷ����</TD>
							<TD style="WIDTH: 163px; HEIGHT: 27px" align="left">
								<asp:label id="lblTaxerCompanyName" runat="server"></asp:label></TD>
						</TR>
						<TR>
							<TD style="WIDTH: 100px; HEIGHT: 27px" align="right">��˰��ʶ��ţ�</TD>
							<TD style="WIDTH: 163px; HEIGHT: 27px">
								<asp:label id="lblTaxerID" runat="server"></asp:label></TD>
							<TD style="WIDTH: 110px; HEIGHT: 27px" align="right">��˾�����������У�</TD>
							<TD style="WIDTH: 163px; HEIGHT: 27px" align="left">
								<asp:label id="lblTaxerBasebankName" runat="server"></asp:label></TD>
							<TD style="WIDTH: 120px; HEIGHT: 27px" align="right">��˾�����������˺ţ�</TD>
							<TD style="WIDTH: 163px; HEIGHT: 27px" align="left">
								<asp:label id="lblTaxerBaseBankAcct" runat="server"></asp:label></TD>
						</TR>
                        <TR>
                        <TD style="WIDTH: 100px; HEIGHT: 27px" align="right">��˾�绰��</TD>
							<TD style="WIDTH: 163px; HEIGHT: 27px" align="left">
								<asp:label id="lblTaxerCompanyPhone" runat="server"></asp:label></TD>
							<TD style="WIDTH: 100px; HEIGHT: 27px" align="right">��˾��ַ��</TD>
							<TD style="WIDTH: 163px; HEIGHT: 27px" colspan="3">
								<asp:label id="lblTaxerCompanyAddress" runat="server"></asp:label></TD>
						</TR>
						<TR>
							<TD style="WIDTH: 100px; HEIGHT: 27px" align="right">�ռ���������</TD>
							<TD style="WIDTH: 163px; HEIGHT: 27px">
								<asp:label id="lblTaxerReceiverName" runat="server"></asp:label></TD>
							<TD style="WIDTH: 100px; HEIGHT: 27px" align="right">�ռ��˵�ַ��</TD>
							<TD style="WIDTH: 163px; HEIGHT: 27px" align="left">
								<asp:label id="lblTaxerReceiverAddr" runat="server"></asp:label></TD>
							<TD style="WIDTH: 100px; HEIGHT: 27px" align="right">�ʱࣺ</TD>
							<TD style="WIDTH: 163px; HEIGHT: 27px" align="left">
								<asp:label id="lblTaxerReceiverPostalCode" runat="server"></asp:label></TD>
						</TR>
						<TR>
							<TD style="WIDTH: 100px; HEIGHT: 27px" align="right">�̻����ͣ�</TD>
							<TD style="WIDTH: 163px; HEIGHT: 27px">
								<asp:label id="lblTaxerUserType" runat="server"></asp:label></TD>
							<TD style="WIDTH: 100px; HEIGHT: 27px" align="right">��ϵ�˵绰��</TD>
							<TD style="WIDTH: 163px; HEIGHT: 27px" align="left">
								<asp:label id="lblTaxerReceiverPhone" runat="server"></asp:label></TD>
							<TD style="WIDTH: 100px; HEIGHT: 27px" align="right"></TD>
							<TD style="WIDTH: 163px; HEIGHT: 27px" align="left"></TD>
						</TR>
						<TR>
							<TD style="WIDTH: 100px; HEIGHT: 27px" align="right">��˱�ע��</TD>
							<TD colSpan="5">
								<asp:TextBox id="txtTaxInvoiceMemo" Width="600px" Runat="server" ReadOnly="True" TextMode="MultiLine"></asp:TextBox></TD>
						</TR>
						<TR>
							<TD colSpan="3" align="center">
								<asp:Button id="btnAllModify" Text="����ȫ����Ϣ�޸�" Runat="server" Visible="False" onclick="btnAllModify_Click"></asp:Button></TD>
							<TD colSpan="3" align="center">
								<asp:Button id="btnLittleModify" Text="�����ռ�����Ϣ�޸�" Runat="server" Visible="False" onclick="btnLittleModify_Click"></asp:Button></TD>
						</TR>
					</TABLE>
				</asp:Panel>
			</div>
		</form>
	</body>
</HTML>
