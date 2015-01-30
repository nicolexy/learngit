<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<%@ Page language="c#" Codebehind="OverseasPayQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.OverseasPayQuery" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>OverseasPayQuery</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); .style2 { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		<script language="javascript">

					function openModeBegin()

					{

						var returnValue=window.showModalDialog("../Control/CalendarForm2.aspx",Form1.TextBoxBeginDate.value,'dialogWidth:375px;DialogHeight=260px;status:no');

						if(returnValue != null) Form1.TextBoxBeginDate.value=returnValue;

					}

					function openModeEnd()

					{

					var returnValue=window.showModalDialog("../Control/CalendarForm2.aspx",Form1.TextBoxEndDate.value,'dialogWidth:375px;DialogHeight=260px;status:no');

					if(returnValue != null) Form1.TextBoxEndDate.value=returnValue;

					}

		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="900" border="1">
				<TR>
					<TD bgColor="#e4e5f7" colSpan="3"><FONT face="����" color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">
							&nbsp;&nbsp;&nbsp;����֧��������ѯ</FONT> </FONT></TD>
					<TD align="right" bgColor="#e4e5f7"><FONT face="����">����Ա����: <SPAN class="style3">
								<asp:label id="Label1" runat="server" Width="73px"></asp:label></SPAN></FONT></TD>
				</TR>
				<TR>
					<TD align="right"><asp:radiobutton id="rbtKeyQuery" GroupName="GroupQuery" Runat="server" Text=""></asp:radiobutton></TD>
					<TD colSpan="3"><asp:label id="Label5" runat="server">֧�����׵���</asp:label><asp:textbox id="txtInvoiceID" runat="server" Width="400px"></asp:textbox></TD>
				</TR>
				<TR>
					<TD align="right" rowSpan="2"><asp:radiobutton id="rbtOtherQuery" GroupName="GroupQuery" Runat="server" Text=""></asp:radiobutton></TD>
					<TD><asp:label id="Label3" runat="server">��ʼ����</asp:label><asp:textbox id="TextBoxBeginDate" runat="server"></asp:textbox><asp:imagebutton id="ButtonBeginDate" runat="server" CausesValidation="False" ImageUrl="../Images/Public/edit.gif"></asp:imagebutton></TD>
					<TD><asp:label id="Label4" runat="server">��������</asp:label><asp:textbox id="TextBoxEndDate" runat="server"></asp:textbox><asp:imagebutton id="ButtonEndDate" runat="server" CausesValidation="False" ImageUrl="../Images/Public/edit.gif"></asp:imagebutton>&nbsp;&nbsp;&nbsp;ע:��������²�ѯ</TD>
				</TR>
				<TR>
					<td colSpan="4"><asp:label id="Label2" runat="server">������</asp:label><asp:textbox id="txtOrder_no" runat="server"></asp:textbox><asp:label id="Label6" runat="server">���ز����Ľ��׺�</asp:label><asp:textbox id="txtTransactionid" runat="server"></asp:textbox><asp:label id="Label7" runat="server">�ջ�QQ��</asp:label><asp:textbox id="txtShipcontent" runat="server"></asp:textbox><asp:label id="Label29" runat="server">֧����ʽ</asp:label><asp:dropdownlist id="ddlpaychannel" Runat="server">
							<asp:ListItem Value="1">paypal</asp:ListItem>
						</asp:dropdownlist></td>
				</TR>
				<tr>
					<TD align="center" colSpan="4"><asp:button id="btnQuery" Width="80px" Runat="server" Text="��ѯ" onclick="btnQuery_Click"></asp:button></TD>
				</tr>
			</TABLE>
			<TABLE id="Table2" cellSpacing="1" cellPadding="1" width="900" border="1" runat="server">
				<TR>
					<TD vAlign="top"><asp:datagrid id="DataGrid1" runat="server" Width="100%" AutoGenerateColumns="False" GridLines="Horizontal"
							CellPadding="3" BackColor="White" BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF" PageSize="5">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="invoice_id" HeaderText="֧�����׵���"></asp:BoundColumn>
								<asp:BoundColumn DataField="order_date" HeaderText="��������"></asp:BoundColumn>
								<asp:BoundColumn DataField="order_title" HeaderText="ҵ������"></asp:BoundColumn>
								<asp:BoundColumn DataField="Shipcontent" HeaderText="�ջ�QQ��"></asp:BoundColumn>
								<asp:BoundColumn DataField="amount" HeaderText="���"></asp:BoundColumn>
								<asp:BoundColumn DataField="ip_region" HeaderText="�����˹���"></asp:BoundColumn>
								<asp:ButtonColumn Text="��ϸ" CommandName="Select"></asp:ButtonColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<tr>
					<td><webdiyer:aspnetpager id="pager" runat="server" AlwaysShow="True" ShowCustomInfoSection="left" NumericButtonTextFormatString="[{0}]"
							SubmitButtonText="ת��" HorizontalAlign="right" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0"
							NumericButtonCount="5" OnPageChanged="ChangePage"></webdiyer:aspnetpager></td>
				</tr>
			</TABLE>
			<asp:panel id="PanelDetail" Runat="server" Visible="False">
				<TABLE cellSpacing="1" cellPadding="1" width="900" border="1" runat="server">
					<TR>
						<TD align="right">
							<asp:label id="Label8" runat="server">֧�����׵���</asp:label></TD>
						<TD colSpan="3">
							<asp:label id="lblinvoice_id" runat="server"></asp:label></TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:label id="Label14" runat="server">������</asp:label></TD>
						<TD>
							<asp:label id="lblorder_no" runat="server"></asp:label></TD>
						<TD align="right">
							<asp:label id="Label9" runat="server">��������</asp:label></TD>
						<TD>
							<asp:label id="lblorder_date" runat="server"></asp:label></TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:label id="Label10" runat="server">������</asp:label></TD>
						<TD>
							<asp:label id="lblchnid" runat="server"></asp:label></TD>
						<TD align="right">
							<asp:label id="Label12" runat="server">��������</asp:label></TD>
						<TD>
							<asp:label id="lblchtype" runat="server"></asp:label></TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:label id="Label11" runat="server">���</asp:label></TD>
						<TD>
							<asp:label id="lblamount" runat="server"></asp:label></TD>
						<TD align="right">
							<asp:label id="Label15" runat="server">����</asp:label></TD>
						<TD>
							<asp:label id="lblcurrency" runat="server"></asp:label></TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:label id="Label13" runat="server">�ջ���QQ��</asp:label></TD>
						<TD>
							<asp:label id="lblshipcontent" runat="server"></asp:label></TD>
						<TD align="right">
							<asp:label id="Label16" runat="server">ʵ��֧�����</asp:label></TD>
						<TD>
							<asp:label id="lblsettle_amount" runat="server"></asp:label></TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:label id="Label31" runat="server">ʵ��֧������</asp:label></TD>
						<TD>
							<asp:label id="lblsettle_currency" runat="server"></asp:label></TD>
						<TD align="right">
							<asp:label id="Label33" runat="server">����״̬</asp:label></TD>
						<TD>
							<asp:label id="lblpayment_status" runat="server"></asp:label></TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:label id="Label18" runat="server">����������</asp:label></TD>
						<TD>
							<asp:label id="lblpayer_email" runat="server"></asp:label></TD>
						<TD align="right">
							<asp:label id="Label34" runat="server">�����˹���</asp:label></TD>
						<TD>
							<asp:label id="lblip_region" runat="server"></asp:label></TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:label id="Label17" runat="server">������״̬</asp:label></TD>
						<TD>
							<asp:label id="lblpayer_status" runat="server"></asp:label></TD>
						<TD align="right">
							<asp:label id="Label20" runat="server">�����˹��Ҵ���</asp:label></TD>
						<TD>
							<asp:label id="lblcountrycode" runat="server"></asp:label></TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:label id="Label19" runat="server">���ز����Ľ��׺�</asp:label></TD>
						<TD>
							<asp:label id="lbltransactionid" runat="server"></asp:label></TD>
						<TD align="right">
							<asp:label id="Label22" runat="server">��������</asp:label></TD>
						<TD>
							<asp:label id="lbltransactiontype" runat="server"></asp:label></TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:label id="Label21" runat="server">��������</asp:label></TD>
						<TD>
							<asp:label id="lblpaymenttype" runat="server"></asp:label></TD>
						<TD align="right">
							<asp:label id="Label24" runat="server">������</asp:label></TD>
						<TD>
							<asp:label id="lblfee_amount" runat="server"></asp:label></TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:label id="Label23" runat="server">˰��</asp:label></TD>
						<TD>
							<asp:label id="lbltax_amount" runat="server"></asp:label></TD>
						<TD align="right">
							<asp:label id="Label26" runat="server">����</asp:label></TD>
						<TD>
							<asp:label id="lblexchange_rate" runat="server"></asp:label></TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:label id="Label25" runat="server">������id</asp:label></TD>
						<TD>
							<asp:label id="lblpayerid" runat="server"></asp:label></TD>
						<TD align="right">
							<asp:label id="Label28" runat="server">֧����ʽ</asp:label></TD>
						<TD>
							<asp:label id="lblpaychannel" runat="server"></asp:label></TD>
					</TR>
					<TR>
						<TD align="right">
							<asp:label id="Label27" runat="server">�������ʱ��</asp:label></TD>
						<TD>
							<asp:label id="lblcomplete_time" runat="server"></asp:label></TD>
						<TD align="right">
							<asp:label id="Label30" runat="server">֧��ʱ��</asp:label></TD>
						<TD>
							<asp:label id="lblpayment_date" runat="server"></asp:label></TD>
					</TR>
				</TABLE>
			</asp:panel></form>
	</body>
</HTML>
