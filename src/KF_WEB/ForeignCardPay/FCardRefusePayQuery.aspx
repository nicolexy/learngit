<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="FCardRefusePayQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.ForeignCardPay.FCardRefusePayQuery" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>FCardRefusePayQuery</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		<script language="javascript">
					function openModeBegin()
					{
						var returnValue=window.showModalDialog("../Control/CalendarForm2.aspx",Form1.TextBoxBeginDate.value,'dialogWidth:375px;DialogHeight=260px;status:no');
						if(returnValue != null) Form1.TextBoxBeginDate.value=returnValue;
		            }
		            function openModeEnd() {
		                var returnValue = window.showModalDialog("../Control/CalendarForm2.aspx", Form1.TextBoxEndDate.value, 'dialogWidth:375px;DialogHeight=260px;status:no');
		                if (returnValue != null) Form1.TextBoxEndDate.value = returnValue;
		            }
		</script>
        <script>
            function CheckEmail() {
                var txtEmail = document.getElementById("txtEmail");

                if (txtEmail.value.replace(/^\s*/, "").replace(/\s*$/, "").length == 0) {
                    txtEmail.focus();
                    txtEmail.select();
                    alert("���䲻����Ϊ��!");
                    return false;
                }
            }
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE style="LEFT: 5%; POSITION: absolute; TOP: 5%" cellSpacing="1" cellPadding="1" width="820"
				border="1">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colspan="4"><FONT face="����"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;�⿨�ܸ���ѯ</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>����Ա����: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></TD>
				</TR>
				<TR>
                    <TD align="right"><asp:label id="Label2" runat="server">�̻���ţ�</asp:label></TD>
                    <TD><asp:textbox id="txtspid" style="WIDTH: 180px;" runat="server"></asp:textbox></TD>
					<TD align="right"><asp:label id="Label4" runat="server">�̼Ҷ����ţ�</asp:label></TD>
                    <TD><asp:textbox id="txtCoding" style="WIDTH: 180px;" runat="server"></asp:textbox></TD>
				</TR>
                <TR>
                    <TD align="right"><asp:label id="Label3" runat="server">�Ƹ�ͨ�����ţ�</asp:label></TD>
                    <TD><asp:textbox id="txtspListID" style="WIDTH: 180px;" runat="server"></asp:textbox></TD>
                     <TD align="right"><asp:label id="Label6" runat="server">�ܸ�״̬��</asp:label></TD>
                    <TD><asp:dropdownlist id="check_state" runat="server" Width="152px">
							<asp:ListItem Value="" Selected="True" >����״̬</asp:ListItem>
							<asp:ListItem Value="1">�鵥</asp:ListItem>
							<asp:ListItem Value="2">�ܸ�</asp:ListItem>
                            <asp:ListItem Value="3">Ԥ�ٲ�</asp:ListItem>
                            <asp:ListItem Value="4">�ٲ�</asp:ListItem>
                            <asp:ListItem Value="5">�ܸ��˿�</asp:ListItem>
                            <asp:ListItem Value="6">���ڽⶳ</asp:ListItem>
						</asp:dropdownlist></TD>
				</TR>
                <TR>
                <TD align="right"><asp:label id="Label7" runat="server">�̻�����״̬��</asp:label></TD>
                    <TD><asp:dropdownlist id="sp_process_state" runat="server" Width="152px">
							<asp:ListItem Value="" Selected="True" >����״̬</asp:ListItem>
							<asp:ListItem Value="1">δ����</asp:ListItem>
							<asp:ListItem Value="2">������</asp:ListItem>
                            <asp:ListItem Value="3">ͬ��ܸ�</asp:ListItem>
                            <asp:ListItem Value="4">���账��</asp:ListItem>
                            <asp:ListItem Value="5">�̻�����ת�˿�</asp:ListItem>
						</asp:dropdownlist></TD>
                    <TD align="right"><asp:label id="Label5" runat="server">�鵥���ڣ�</asp:label></TD>
                    <TD><asp:textbox id="TextBoxBeginDate" runat="server"></asp:textbox><asp:imagebutton id="ButtonBeginDate" runat="server" ImageUrl="../Images/Public/edit.gif" CausesValidation="False"></asp:imagebutton>
                    ��<asp:textbox id="TextBoxEndDate" runat="server"></asp:textbox><asp:imagebutton id="ButtonEndDate" runat="server" ImageUrl="../Images/Public/edit.gif" CausesValidation="False"></asp:imagebutton>
                    <Font color="red">*</Font></TD>
				</TR>
				<TR>
                    <TD align="center" colspan="4"><Font color="red">*����</Font>&nbsp;&nbsp;&nbsp;&nbsp;<asp:button id="btnQuery" runat="server" Width="80px" Text="�� ѯ" onclick="btnQuery_Click"></asp:button>
				</TR>
			</TABLE>
			<TABLE id="Table2" style="Z-INDEX: 102; LEFT: 5.02%; WIDTH: 85%; POSITION: absolute; TOP: 184px; HEIGHT: 35%"
				cellSpacing="1" cellPadding="1" width="808" border="1" runat="server">
				<TR>
					<TD vAlign="top"><asp:datagrid id="DataGrid1" runat="server" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px"
							BackColor="White" CellPadding="3" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="Fspid" HeaderText="�̻����"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fcoding" HeaderText="�̼Ҷ�����"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Flistid" HeaderText="�Ƹ�ͨ������"></asp:BoundColumn>
                                <asp:BoundColumn DataField="FModify_time" HeaderText="�鵥ʱ��"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fbank_req_refund_fee_str" HeaderText="�鵥���"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fbank_currency_freeze_fee_str" HeaderText="������"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fbank_currency_refund_fee_str" HeaderText="�ܸ��˿���"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fcheck_state_str" HeaderText="�ܸ�״̬"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fsp_process_state_str" HeaderText="�̻�����״̬"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Frisk_process_state_str" HeaderText="��ش���״̬"></asp:BoundColumn>
							</Columns>
                            <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
                <TR height="25">
					<TD><webdiyer:aspnetpager id="pager" runat="server" AlwaysShow="True" NumericButtonCount="5" ShowCustomInfoSection="left"
							PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager" HorizontalAlign="right" 
							SubmitButtonText="ת��" NumericButtonTextFormatString="[{0}]"></webdiyer:aspnetpager></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
