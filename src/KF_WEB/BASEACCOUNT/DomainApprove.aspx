<%@ Page language="c#" Codebehind="DomainApprove.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.DomainApprove" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>DomainApprove</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		<script src="../SCRIPTS/Local.js"></script>
        <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE cellSpacing="1" cellPadding="1" width="850" border="1">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="6"><FONT face="����"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;�̻��޸��������</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>����Ա����: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" Width="73px" ForeColor="Red"></asp:label></SPAN></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 96px; HEIGHT: 27px" align="right">�̻��ţ�</TD>
					<TD style="WIDTH: 180px; HEIGHT: 27px" align="left"><asp:textbox id="txtSpid" runat="server"></asp:textbox></TD>
					<TD style="WIDTH: 100px; HEIGHT: 27px" align="right">�̻����ƣ�</TD>
					<TD style="WIDTH: 163px; HEIGHT: 27px" align="left"><asp:textbox id="txtCompanyName" runat="server"></asp:textbox></TD>
					<TD style="WIDTH: 163px; HEIGHT: 27px" align="left">�������ͣ�</TD>
					<TD style="WIDTH: 163px; HEIGHT: 27px" align="left"><asp:dropdownlist id="ddlSubmitType" runat="server">
							<asp:ListItem Value="1">����</asp:ListItem>
							<asp:ListItem Value="2">����</asp:ListItem>
							<asp:ListItem Value="3">�̻�����</asp:ListItem>
						</asp:dropdownlist></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 96px; HEIGHT: 25px" align="right">�ύʱ���</TD>
					<TD style="WIDTH: 180px; HEIGHT: 25px">
                        <input type="text" runat="server" id="TextBoxBeginDate" onclick="WdatePicker()" />
                        <img onclick="TextBoxBeginDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="ѡ������" />
					</TD>
					<TD style="WIDTH: 96px; HEIGHT: 25px" align="right">��</TD>
					<TD style="WIDTH: 180px; HEIGHT: 25px">
                        <input type="text" runat="server" id="TextBoxEndDate" onclick="WdatePicker()" />
                        <img onclick="TextBoxEndDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="ѡ������" />
					</TD>
					<TD style="WIDTH: 100px; HEIGHT: 27px" align="right">���״̬��</TD>
					<TD style="WIDTH: 163px; HEIGHT: 27px"><asp:dropdownlist id="ddlState" runat="server">
							<asp:ListItem Value="">ȫ��</asp:ListItem>
							<asp:ListItem Value="-3" Selected="True">�ȴ��ͷ����</asp:ListItem>
							<asp:ListItem Value="0">�ȴ��������</asp:ListItem>
							<asp:ListItem Value="3">���ͨ��</asp:ListItem>
							<asp:ListItem Value="4">��˲�ͨ��</asp:ListItem>
						</asp:dropdownlist></TD>
				</TR>
				<TR>
					<TD align="center" colSpan="6"><asp:button id="btnSearch" runat="server" Text="�� ѯ" onclick="btnSearch_Click"></asp:button></TD>
				</TR>
			</TABLE>
			<table cellSpacing="0" cellPadding="0" border="0">
				<TR>
					<TD vAlign="top" align="center"><asp:datagrid id="dgList" runat="server" Width="850px" BorderColor="#E7E7FF" BorderStyle="None"
							BorderWidth="1px" BackColor="White" CellPadding="1" GridLines="Horizontal" AutoGenerateColumns="False" HorizontalAlign="Center"
							HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<Columns>
								<asp:BoundColumn Visible="False" DataField="Taskid" HeaderText="���"></asp:BoundColumn>
								<asp:BoundColumn DataField="Spid" HeaderText="�̻���">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="CompanyName"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="AmendType"></asp:BoundColumn>
								<asp:BoundColumn DataField="AmendTypeStr" HeaderText="��������">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="Type"></asp:BoundColumn>
								<asp:BoundColumn DataField="TypeStr" Visible="False"></asp:BoundColumn>
								<asp:BoundColumn DataField="WWWAdress" Visible="False"></asp:BoundColumn>
								<asp:BoundColumn DataField="OldDomain" Visible="False"></asp:BoundColumn>
								<asp:BoundColumn DataField="NewDomain" Visible="False"></asp:BoundColumn>
								<asp:BoundColumn DataField="ContactEmail" Visible="False"></asp:BoundColumn>
								<asp:BoundColumn DataField="OldEmail" Visible="False"></asp:BoundColumn>
								<asp:BoundColumn DataField="NewEmail" Visible="False"></asp:BoundColumn>
								<asp:BoundColumn DataField="OldCompanyname" Visible="False"></asp:BoundColumn>
								<asp:BoundColumn DataField="NewCompanyname" Visible="False"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="AmendState"></asp:BoundColumn>
								<asp:BoundColumn DataField="AmendStateStr" HeaderText="���״̬">
									<HeaderStyle Width="80px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="ApplyTime" HeaderText="�ύʱ��">
									<HeaderStyle Width="120px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="CheckUser"></asp:BoundColumn>
								<asp:BoundColumn DataField="CheckTime" HeaderText="����ʱ��">
									<HeaderStyle Width="120px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="Memo"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="Reason"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="IDImage"></asp:BoundColumn>
                                <asp:BoundColumn Visible="False" DataField="ElseImage"></asp:BoundColumn>
								<asp:ButtonColumn Text="�鿴" CommandName="Select"></asp:ButtonColumn>
							</Columns>
							<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
				<TR height="25">
					<TD><webdiyer:aspnetpager id="pager" runat="server" HorizontalAlign="right" PageSize="10" NumericButtonTextFormatString="[{0}]"
							SubmitButtonText="ת��" OnPageChanged="ChangePage" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0"
							ShowCustomInfoSection="left" NumericButtonCount="5" AlwaysShow="True" CustomInfoTextAlign="Center"></webdiyer:aspnetpager></TD>
				</TR>
			</table>
			<asp:Panel ID="PanelDetail" Runat="server">
				<TABLE border="1" cellSpacing="1" cellPadding="1" width="850">
					<TR>
						<TD style="WIDTH: 100px; HEIGHT: 27px" align="right">�̻��ţ�</TD>
						<TD style="WIDTH: 163px; HEIGHT: 27px" align="left">
							<asp:label id="lblSpid" runat="server"></asp:label></TD>
						<TD style="WIDTH: 100px; HEIGHT: 27px" align="right">�̻����ƣ�</TD>
						<TD style="WIDTH: 163px; HEIGHT: 27px" align="left">
							<asp:label id="lblCompanyName" runat="server"></asp:label></TD>
						<TD style="WIDTH: 100px; HEIGHT: 27px" align="right">�������ͣ�</TD>
						<TD style="WIDTH: 163px; HEIGHT: 27px" align="left">
							<asp:label id="lblAmendTypeStr" runat="server"></asp:label></TD>
					</TR>
					<TR>
						<TD style="WIDTH: 100px; HEIGHT: 27px" align="right">���״̬��</TD>
						<TD style="WIDTH: 163px; HEIGHT: 27px">
							<asp:label id="lblState" runat="server"></asp:label></TD>
					</TR>
					<asp:Panel id="DomainPanel" Runat="server">
						<TR>
							<TD style="WIDTH: 100px; HEIGHT: 27px" align="right">������</TD>
							<TD colSpan="5">
								<asp:label id="lblWWWAdress" runat="server"></asp:label></TD>
						</TR>
						<TR>
							<TD style="WIDTH: 100px; HEIGHT: 27px" align="right">������ͣ�</TD>
							<TD style="WIDTH: 163px; HEIGHT: 27px">
								<asp:label id="lblType" runat="server"></asp:label></TD>
						</TR>
						<TR>
							<TD style="WIDTH: 100px; HEIGHT: 27px" align="right">ԭ������</TD>
							<TD style="WIDTH: 163px; HEIGHT: 27px">
								<asp:label id="lblOldDomain" runat="server"></asp:label></TD>
							<TD style="WIDTH: 100px; HEIGHT: 27px" align="right">��������</TD>
							<TD style="WIDTH: 163px; HEIGHT: 27px">
								<asp:label id="lblNewDomain" runat="server"></asp:label></TD>
						</TR>
					</asp:Panel>
					<asp:Panel id="EmailPanel" Runat="server">
						<TR>
							<TD style="WIDTH: 100px; HEIGHT: 27px" align="right">���䣺</TD>
							<TD colSpan="5">
								<asp:label id="lblEmail" runat="server"></asp:label></TD>
						</TR>
						<TR>
							<TD style="WIDTH: 100px; HEIGHT: 27px" align="right">ԭ���䣺</TD>
							<TD style="WIDTH: 163px; HEIGHT: 27px">
								<asp:label id="lblOldEmail" runat="server"></asp:label></TD>
							<TD style="WIDTH: 100px; HEIGHT: 27px" align="right">�����䣺</TD>
							<TD style="WIDTH: 163px; HEIGHT: 27px">
								<asp:label id="lblNewEmail" runat="server"></asp:label></TD>
						</TR>
						<TR>
							<TD style="WIDTH: 100px; HEIGHT: 27px" align="right">ͼƬ��</TD>
							<TD colSpan="3" align="center">
								<asp:image id="Image1" runat="server" Width="400px" Height="300px"></asp:image>
                                <asp:image id="Image2" runat="server" Width="400px" Height="300px"></asp:image>
                            </TD>
						</TR>
					</asp:Panel>
					<asp:Panel id="CompanyNamePanel" Runat="server">
						<TR>
							<TD style="WIDTH: 100px; HEIGHT: 27px" align="right">ԭ�̻����ƣ�</TD>
							<TD style="WIDTH: 163px; HEIGHT: 27px">
								<asp:label id="lblOldCompanyName" runat="server"></asp:label></TD>
							<TD style="WIDTH: 100px; HEIGHT: 27px" align="right">���̻����ƣ�</TD>
							<TD style="WIDTH: 163px; HEIGHT: 27px">
								<asp:label id="lblNewCompanyName" runat="server"></asp:label></TD>
						</TR>
					</asp:Panel>
					<TR>
						<TD style="WIDTH: 100px; HEIGHT: 27px" align="right">��ע��</TD>
						<TD colSpan="5">
							<asp:label id="lblMemo" runat="server"></asp:label></TD>
					</TR>
					<TR>
						<TD style="WIDTH: 100px; HEIGHT: 27px" align="right">�ܾ�ԭ��</TD>
						<TD colSpan="5">
							<asp:textbox id="txtReason" runat="server" Width="100%"></asp:textbox></TD>
					</TR>
					<TR>
						<TD style="WIDTH: 100px; HEIGHT: 27px" align="right">�ύʱ�䣺</TD>
						<TD style="WIDTH: 163px; HEIGHT: 27px">
							<asp:label id="lblApplyTime" runat="server"></asp:label></TD>
						<TD style="WIDTH: 100px; HEIGHT: 27px" align="right">�����ˣ�</TD>
						<TD style="WIDTH: 163px; HEIGHT: 27px">
							<asp:label id="lblCheckUser" runat="server"></asp:label></TD>
						<TD style="WIDTH: 100px; HEIGHT: 27px" align="right">����ʱ�䣺</TD>
						<TD style="WIDTH: 163px; HEIGHT: 27px">
							<asp:label id="lblCheckTime" runat="server"></asp:label></TD>
					</TR>
					<TR>
						<TD>
							<asp:Label id="lblID" Runat="server" Visible="False"></asp:Label></TD>
					</TR>
					<TR>
						<TD colSpan="6" align="center">
							<asp:button id="btnApprove" Text="���ͨ��" Runat="server" onclick="btnApprove_Click"></asp:button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
							<asp:button id="btnCancel" Text="��˲�ͨ��" Runat="server" onclick="btnCancel_Click"></asp:button></TD>
					</TR>
				</TABLE>
			</asp:Panel>
		</form>
	</body>
</HTML>
