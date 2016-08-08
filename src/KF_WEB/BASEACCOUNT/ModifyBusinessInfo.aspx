<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<%@ Page language="c#" Codebehind="ModifyBusinessInfo.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.ModifyBusinessInfo" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>ModifyBusinessInfo</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		<script src="../SCRIPTS/Local.js"></script>
		<script language="javascript">
		    function ShowBankCard() {
		        var txtBankCard = document.getElementById('txtBankCard');
		        var btnBankCardCheck = document.getElementById('btnBankCardCheck');
		        txtBankCard.disabled = false;
		        btnBankCardCheck.disabled = false;
		    }

		    function HideBankCard() {
		        var txtBankCard = document.getElementById('txtBankCard');
		        var btnBankCardCheck = document.getElementById('btnBankCardCheck');
		        txtBankCard.disabled = true;
		        btnBankCardCheck.disabled = true;
		    }
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE style="margin-left:5%;margin-top:20px" cellSpacing="1" cellPadding="1" width="900"
				border="1">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="5"><FONT face="����"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;�̻������޸�</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>����Ա����: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" Width="73px" ForeColor="Red"></asp:label></SPAN></TD>
				</TR>
				<TR>
					<td align="right"><asp:label id="Label5" runat="server">�̻����</asp:label></td>
					<td><asp:radiobutton id="rbtBusiness" onclick="ShowBankCard();" GroupName="rbtBusinessType" Text="ֱ���̻�"
							Runat="server"></asp:radiobutton><asp:radiobutton id="rbtAgency" onclick="HideBankCard();" GroupName="rbtBusinessType" Text="�н��̻�"
							Runat="server"></asp:radiobutton></td>
					<TD align="right"><asp:label id="Label2" runat="server">�̻���</asp:label></TD>
					<TD><asp:textbox id="txtFspid" runat="server"></asp:textbox></TD>
				</TR>
				<TR>
					<TD align="right"><asp:label id="Label4" runat="server">���кź���λ</asp:label></TD>
					<TD><asp:textbox id="txtBankCard" runat="server"></asp:textbox><asp:button id="btnBankCardCheck" runat="server" Width="80px" Text="�� ��" onclick="btnBankCardCheck_Click"></asp:button></TD>
                    <TD align="center" colSpan="2"><FONT face="����"><asp:button id="btnSearch" runat="server" Width="80px" Text="�� ѯ" onclick="btnSearch_Click"></asp:button></FONT></TD>
				</TR>
				
			</TABLE>
			<TABLE id="Table1" style="margin-left:5%;margin-top:20px" cellSpacing="1" cellPadding="1"
				width="900px" border="1" runat="server">
				<TR>
					<TD align="right" width="80"><asp:label id="Label12" runat="server">��������</asp:label></TD>
					<TD width="370px"><asp:textbox id="txtSendEmail" runat="server"></asp:textbox></TD>
					<TD width="80px" align="right" height="30">&nbsp;</TD>
					<TD width="370px">&nbsp;</TD>
				</TR>
                <TR>
					<TD align="right"><asp:label id="Label14" runat="server">�̻���Կ</asp:label></TD>
					<TD><asp:label id="lblMerKey" runat="server" Visible=False></asp:label>&nbsp;&nbsp;&nbsp;&nbsp;����̻�������Կ���޷��������ã�����
                        <asp:linkbutton id="btnSendEmail" runat="server" Font-Bold="True" CausesValidation="False" onclick="btnSendEmail_Click">�ط���Կ</asp:linkbutton></TD>
					<TD align="right"><asp:label id="Label10" runat="server">��¼����</asp:label></TD>
					<TD><FONT face="����">����̻����ǹ���Ա���룬�޷���½��ҵ�棬����</FONT>
					<asp:linkbutton id="Linkbutton2" runat="server" Font-Bold="True" CausesValidation="False" onclick="Linkbutton2_Click">��������</asp:linkbutton>

					</TD>

				</TR>
				<TR>
					<TD align="right"><asp:label id="Label19" runat="server">֪ͨ�ʼ�</asp:label></TD>
					<TD colspan="3">����̻�û���յ�֪ͨ�ʼ����׵��
                        <asp:linkbutton id="btnSendEmailAgain" runat="server" Font-Bold="True" CausesValidation="False" onclick="btnSendEmailAgain_Click">�ط�֪ͨ�ʼ�</asp:linkbutton>
					</TD>
                   <%-- <TD align="right"><asp:label id="Label18" runat="server">�̻�֤��</asp:label></TD>
                    <TD>����̻�û���յ�֤�飬����
                        <asp:linkbutton id="btnSendCertificateAgain" runat="server" Font-Bold="True" CausesValidation="False" onclick="btnSendCertificateAgain_Click">�ط�֤��</asp:linkbutton>
                    </TD>--%>
				</TR>
				<TR>
					<TD align="right"><asp:label id="Label6" runat="server">��վ����</asp:label></TD>
					<TD colspan="3"><asp:label id="lblFspName" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD align="right"><asp:label id="Label9" runat="server">����վ����</asp:label></TD>
					<TD colspan="3"><asp:textbox id="txtFspName" runat="server" Width="100%"></asp:textbox></TD>
				</TR> 
                <TR>
                    <TD align="right"><asp:label id="Label7" runat="server">��ַ</asp:label></TD>
					<TD colspan="3"><asp:label id="lblAddress" runat="server"></asp:label></TD>
                </TR>
               
                <TR>
				   <TD align="right"><asp:label id="Label11" runat="server">����ַ</asp:label></TD>
				   <TD colspan="3"><asp:textbox id="txtAddress" runat="server" Width="100%"></asp:textbox></TD>
                </TR>
                <TR>
                    <TD align="right"><asp:label id="Label8" runat="server">������</asp:label></TD>
                    <TD><asp:label id="lblEmail" runat="server"></asp:label></TD>
                    <TD align="right"><asp:label id="Label3" runat="server">��ϵ���ֻ�</asp:label></TD>
					<TD><asp:label id="lblContactMobile" runat="server"></asp:label></TD>
                </TR>
                 <TR>
                    <TD align="right"><asp:label id="Label13" runat="server">�°�����</asp:label></TD>
                    <TD><asp:textbox id="txtEmail" runat="server" Width="300px"></asp:textbox></TD>
					<TD align="right"><asp:label id="Label15" runat="server">����ϵ���ֻ�</asp:label></TD>
					<TD><asp:textbox id="txtContactMobile" runat="server"  Width="300px"></asp:textbox></TD>
                </TR>
                <TR>
                     <TD align="right"><FONT face="����">�ϴ���������ɨ���</FONT></TD>
                    <TD>
                        <INPUT id="FileEmail" style="WIDTH:300px; HEIGHT: 21px" type="file" size="21" name="FileEmail"
							runat="server"><SPAN class="style5"><Font color="red">*</Font></SPAN>
                    </TD>
                    <TD align="right"><asp:label id="Label16" runat="server">�ϴ��ֻ������ɨ���</asp:label></TD>
                    <TD> <INPUT id="fileMobileChange" style=" HEIGHT: 21px; width:300px" type="file" size="21" name="fileMobileChange"
							runat="server"><SPAN class="style5"><Font color="red">*</Font></TD>
                </TR>
                  <TR>
                     <TD align="right" ><FONT face="����">�����˺��ϴ����֤ɨ���</FONT></TD>
                    <TD>
                        <INPUT id="FileCredit" style="WIDTH: 300px; HEIGHT: 21px" type="file" size="21" name="FileCredit"
							runat="server"><SPAN class="style5"><Font color="red">*</Font></SPAN>
                    </TD>
                    <TD align="right"><asp:label id="Label17" runat="server">�����ϴ����֤ɨ���</asp:label></TD>
                   <TD><INPUT id="fileMobileCredit" style="HEIGHT: 21px; width:300px" type="file" size="21" name="fileMobileCredit"
							runat="server"><SPAN class="style5"><Font color="red">*</Font></TD>
                </TR>
                 <TR>
                    <TD align="right"><FONT face="����">�޸�ԭ��</FONT></TD>
					<TD colspan="3"><asp:textbox id="tbReasonText" runat="server" Width="488px" Height="120px" TextMode="MultiLine"></asp:textbox>
					</TD>
				</TR>
                <TR>
					<td colspan="4" align="center"> 
                        <asp:button id="btnSave" runat="server" Width="60px" Text="�ύ�޸�" onclick="btnSave_Click"></asp:button>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:button id="btnHisSearch" runat="server" Width="60px" Text="�鿴��ʷ" onclick="btnHisSearch_Click"></asp:button>
                    </td>
				</TR>
			</TABLE>
				<table style="margin-left:5%;margin-top:20px" cellSpacing="0" cellPadding="0" width="900px" border="0">
					<tr>
						<TD vAlign="top" >
                            <div style="width:900px;overflow-x: auto">
                            <asp:datagrid id="dgList" runat="server" Width="1150px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
								HorizontalAlign="Center" PageSize="5" AutoGenerateColumns="False" GridLines="Horizontal" CellPadding="1" BackColor="White"
								BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF" AllowPaging="True">
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="oldcompanyname" HeaderText="���̻���">
										<HeaderStyle Width="120px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="oldwwwaddress" HeaderText="����ַ">
										<HeaderStyle Width="120px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="oldemail" HeaderText="������">
										<HeaderStyle Width="150px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="newcompanyname" HeaderText="���̻���">
										<HeaderStyle Width="120px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="newwwwaddress" HeaderText="����ַ">
										<HeaderStyle Width="120px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="newemail" HeaderText="������">
										<HeaderStyle Width="150px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="DictName" HeaderText="״̬">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="ApplyUser" HeaderText="�ύ��">
										<HeaderStyle Width="100px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="ApplyTime" HeaderText="�ύʱ��">
										<HeaderStyle Width="150px"></HeaderStyle>
									</asp:BoundColumn>
								</Columns>
								<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
							</asp:datagrid>
                            </div>
                       </TD>
					</tr>
                    <tr>
						<TD vAlign="top" align="left">
                            <asp:datagrid id="dgContactMobileHistory" runat="server" Width="900px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
								HorizontalAlign="Center" PageSize="5" AutoGenerateColumns="False" GridLines="Horizontal" CellPadding="1" BackColor="White"
								BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF" AllowPaging="True">
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="oldContactMobile" HeaderText="���ֻ���">
										<HeaderStyle></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="NewContactMobile" HeaderText="���ֻ���">
										<HeaderStyle></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="AmendStateStr" HeaderText="״̬">
										<HeaderStyle></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="ApplyUser" HeaderText="�ύ��">
										<HeaderStyle></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="ApplyTime" HeaderText="�ύʱ��">
										<HeaderStyle></HeaderStyle>
									</asp:BoundColumn>
								</Columns>
								<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
							</asp:datagrid></TD>
					</tr>
				</table>
		</form>
	</body>
</HTML>
