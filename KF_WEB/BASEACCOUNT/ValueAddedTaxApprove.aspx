<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="ValueAddedTaxApprove.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.ValueAddedTaxApprove" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>ValueAddedTaxApprove</title>
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
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="4"><FONT face="����"><FONT color="red"><IMG src="../IMAGES/Page/post.gif" width="20" height="16">&nbsp;&nbsp;Ӫ�����̻����</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>����Ա����: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" Width="73px" ForeColor="Red"></asp:label></SPAN></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 96px; HEIGHT: 27px" align="right">�̻��ţ�</TD>
					<TD style="WIDTH: 180px; HEIGHT: 27px" align="left"><asp:textbox id="txtSpid" runat="server"></asp:textbox></TD>
					<TD style="WIDTH: 100px; HEIGHT: 27px" align="right">���״̬��</TD>
					<TD style="WIDTH: 203px; HEIGHT: 27px"><asp:dropdownlist id="ddlFlag" Width="200px" Runat="server">
							<asp:ListItem Value="1">����������Ȩ����ϴ�</asp:ListItem>
							<asp:ListItem Value="2">�������������</asp:ListItem>
							<asp:ListItem Value="3">ȫ���޸������</asp:ListItem>
							<asp:ListItem Value="4">���ͨ��</asp:ListItem>
							<asp:ListItem Value="5">��˲�ͨ��</asp:ListItem>
							<asp:ListItem Value="6">�ռ�����Ϣ�޸���</asp:ListItem>
							<asp:ListItem Value="7">(�û������޸ĳɹ�)�ռ�����Ϣ�޸ĳɹ�</asp:ListItem>
							<asp:ListItem Value="8">�ռ�����Ϣ�޸�ʧ��</asp:ListItem>
							<asp:ListItem Value="9">���̻��ύȫ���޸�����</asp:ListItem>
							<asp:ListItem Value="10">ȫ���޸���Ȩ����ϴ�</asp:ListItem>
							<asp:ListItem Value="2|3" Selected="True">�����״̬</asp:ListItem>
							<asp:ListItem Value="">ȫ��</asp:ListItem>
						</asp:dropdownlist></TD>
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
									<asp:BoundColumn DataField="ApplyTypeStr" HeaderText="��������">
										<HeaderStyle Width="150px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="ApplyTime" HeaderText="����ʱ��">
										<HeaderStyle Width="150px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FlagStr" HeaderText="״̬">
										<HeaderStyle Width="300px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:TemplateColumn>
										<ItemStyle Width="30px"></ItemStyle>
										<ItemTemplate>
											<asp:HyperLink id="hylDetail" runat="server" Text='�鿴' NavigateUrl='<%#"ValueAddedTaxDetail.aspx?TaskID=" +DataBinder.Eval(Container.DataItem, "TaskId")%>'>
											</asp:HyperLink>
										</ItemTemplate>
									</asp:TemplateColumn>
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
			</div>
		</form>
	</body>
</HTML>
