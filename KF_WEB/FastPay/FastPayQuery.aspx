<%@ Page language="c#" Codebehind="FastPayQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.FastPay.FastPayQuery" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>FastPayQuery</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		<script src="../SCRIPTS/Local.js">
			function openModeBegin()
			{
				var returnValue=window.showModalDialog("../Control/CalendarForm2.aspx",Form1.tbx_beginDate.value,'dialogWidth:375px;DialogHeight=260px;status:no');
				if(returnValue != null) Form1.tbx_beginDate.value=returnValue;
			}
			
			function openModeEnd()
			{
				var returnValue=window.showModalDialog("../Control/CalendarForm2.aspx",Form1.tbx_endDate.value,'dialogWidth:375px;DialogHeight=260px;status:no');
				if(returnValue != null) Form1.tbx_endDate.value=returnValue;
			}
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" style="POSITION: absolute; TOP: 5%; LEFT: 5%" cellSpacing="1" cellPadding="1"
				width="800" border="1">
				<TR>
					<TD style="WIDTH: 800px" bgColor="#e4e5f7" colSpan="4"><FONT face="����"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;һ��ͨҵ��</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>����Ա����: </FONT><asp:label id="Label1" runat="server" Width="73px" ForeColor="Red"></asp:label></TD>
				</TR>
				<asp:panel id="PanelList" runat="server">
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label2" runat="server">�Ƹ�ͨ�˺�</asp:label></TD>
						<TD width="250">
							<asp:textbox id="txtQQ" runat="server"></asp:textbox></TD>
						<TD width="150" align="right">
							<asp:label id="Label3" runat="server">�ڲ�ID</asp:label></TD>
						<TD width="250">
							<asp:textbox id="tbx_uid" runat="server" Width="200"></asp:textbox></TD>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label17" runat="server">��������</asp:label></TD>
						<TD width="250">
							<asp:DropDownList id="ddl_BankType" runat="server">
								<asp:ListItem Value="" Selected="True">ȫ��</asp:ListItem>
								<asp:ListItem Value="2001">����һ��ͨ</asp:ListItem>
								<asp:ListItem Value="2002">����һ��ͨ</asp:ListItem>
								<asp:ListItem Value="3001">��ҵ���ÿ�</asp:ListItem>
								<asp:ListItem Value="3002">�������ÿ�</asp:ListItem>
							</asp:DropDownList></TD>
						<TD width="150" align="right">
							<asp:label id="Label18" runat="server">���п���</asp:label></TD>
						<TD width="250">
							<asp:textbox id="tbx_bankID" runat="server" Width="200"></asp:textbox></TD>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label19" runat="server">֤������</asp:label></TD>
						<TD width="250">
							<asp:DropDownList id="ddl_creType" runat="server">
								<asp:ListItem Value="" Selected="True">ȫ��</asp:ListItem>
								<asp:ListItem Value="1">���֤</asp:ListItem>
								<asp:ListItem Value="2">����</asp:ListItem>
								<asp:ListItem Value="3">����֤</asp:ListItem>
							</asp:DropDownList></TD>
						<TD width="150" align="right">
							<asp:label id="Label20" runat="server">֤����</asp:label></TD>
						<TD width="250">
							<asp:textbox id="tbx_creID" runat="server" Width="200"></asp:textbox></TD>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label21" runat="server">Э���</asp:label></TD>
						<TD width="250">
							<asp:textbox id="tbx_serNum" runat="server"></asp:textbox></TD>
						<TD width="150" align="right">
							<asp:label id="Label22" runat="server">�ֻ�����</asp:label></TD>
						<TD width="250">
							<asp:textbox id="tbx_phone" runat="server" Width="200"></asp:textbox></TD>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label23" runat="server">��ʼ����</asp:label></TD>
						<TD width="250">
							<asp:textbox id="tbx_beginDate" runat="server"></asp:textbox>
							<asp:imagebutton id="ButtonBeginDate" runat="server" CausesValidation="False" ImageUrl="../Images/Public/edit.gif"></asp:imagebutton></TD>
						<TD width="150" align="right">
							<asp:label id="Label24" runat="server">��������</asp:label></TD>
						<TD width="250">
							<asp:textbox id="tbx_endDate" runat="server"></asp:textbox>
							<asp:imagebutton id="ButtonEndDate" runat="server" CausesValidation="False" ImageUrl="../Images/Public/edit.gif"></asp:imagebutton></TD>
					</TR>
					<TR>
						<TD colSpan="4" align="center"><FONT face="����">
								<asp:button id="btnSearch" runat="server" Width="80px" Text="�� ѯ"></asp:button></FONT></TD>
					</TR>
					<TR>
						<TD colSpan="4" align="center">
							<asp:datagrid id="dgList" runat="server" Width="100%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
								HorizontalAlign="Center" AutoGenerateColumns="False" GridLines="Horizontal" CellPadding="1"
								BackColor="White" BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF">
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="Findex" Visible="False"></asp:BoundColumn>
									<asp:BoundColumn DataField="Fbind_serialno" HeaderText="�����к�">
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fuin" HeaderText="�Ƹ�ͨ�˺�">
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fbank_typeStr" HeaderText="��������">
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fprotocol_no" HeaderText="Э����">
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fbank_statusStr" HeaderText="���а�״̬">
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fcard_tail" HeaderText="���п�����λ">
										<HeaderStyle Wrap="False"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Ftruename" HeaderText="���п��˻���">
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
									</asp:BoundColumn>
									<asp:TemplateColumn HeaderText="��ϸ">
										<ItemTemplate>
											<a href='./BankCardUnbind.aspx?type=edit&Findex=<%# DataBinder.Eval(Container, "DataItem.Findex")%>&Fuid=<%# DataBinder.Eval(Container, "DataItem.Fuid")%>'>
												��ϸ</a>
										</ItemTemplate>
									</asp:TemplateColumn>
								</Columns>
							</asp:datagrid>
							<WEBDIYER:ASPNETPAGER id="pager" runat="server" HorizontalAlign="right" NumericButtonCount="10" PagingButtonSpacing="0"
								ShowInputBox="always" CssClass="mypager" SubmitButtonText="ת��" NumericButtonTextFormatString="[{0}]"
								ShowCustomInfoSection="left" AlwaysShow="True"></WEBDIYER:ASPNETPAGER></WEBDIYER:ASPNETPAGER></TD>
					</TR>
				</asp:panel>
				<asp:panel id="PanelMod" runat="server" HorizontalAlign="Center" Visible="False">
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label6" runat="server">�Ƹ�ͨ�˺�</asp:label></TD>
						<TD width="250">
							<asp:label id="lblFuin" runat="server"></asp:label></TD>
						<TD width="150" align="right">
							<asp:label id="Label8" runat="server">��������</asp:label></TD>
						<TD width="250">
							<asp:label id="lblFbank_type" runat="server"></asp:label></TD>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label4" runat="server">�����к�</asp:label></TD>
						<TD width="250">
							<asp:label id="lblFbind_serialno" runat="server"></asp:label></TD>
						<TD width="150" align="right">
							<asp:label id="Label5" runat="server">Э����</asp:label></TD>
						<TD width="250">
							<asp:label id="lblFprotocol_no" runat="server"></asp:label></TD>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label7" runat="server">���а�״̬</asp:label></TD>
						<TD width="250">
							<asp:label id="lblFbank_status" runat="server"></asp:label></TD>
						<TD width="150" align="right">
							<asp:label id="Label10" runat="server">���п�����λ</asp:label></TD>
						<TD width="250">
							<asp:label id="lblFcard_tail" runat="server"></asp:label></TD>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label9" runat="server">���п��˻���</asp:label></TD>
						<TD width="250">
							<asp:label id="lblFtruename" runat="server"></asp:label></TD>
						<TD width="150" align="right">
							<asp:label id="Label12" runat="server">��������</asp:label></TD>
						<TD width="250">
							<asp:label id="lblFbind_type" runat="server"></asp:label></TD>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label14" runat="server">��Ч��ʶ</asp:label></TD>
						<TD width="250">
							<asp:label id="lblFbind_flag" runat="server"></asp:label></TD>
						<TD width="150" align="right">
							<asp:label id="Label15" runat="server">���������п���</asp:label></TD>
						<TD width="250">
							<asp:label id="lblFbank_id" runat="server"></asp:label></TD>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label11" runat="server">����״̬</asp:label></TD>
						<TD width="250">
							<asp:label id="lblFbind_status" runat="server"></asp:label></TD>
						<TD>
							<asp:Label id="lblFindex" Visible="False" Runat="server"></asp:Label></TD>
						<TD>
							<asp:Label id="lblFuid" Visible="False" Runat="server"></asp:Label></TD>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label25" runat="server">֤������</asp:label></TD>
						<TD width="250">
							<asp:label id="lblcreType" runat="server"></asp:label></TD>
						<TD width="150" align="right">
							<asp:label id="Label16" runat="server">֤������</asp:label></TD>
						<TD width="250">
							<asp:label id="lblCreID" runat="server"></asp:label></TD>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label26" runat="server">�ֻ���</asp:label></TD>
						<TD width="250">
							<asp:label id="lblPhone" runat="server"></asp:label></TD>
						<TD width="150" align="right">
							<asp:label id="Label28" runat="server">�ڲ�ID</asp:label></TD>
						<TD width="250">
							<asp:label id="lblUid" runat="server"></asp:label></TD>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label13" runat="server">��ע</asp:label></TD>
						<TD colSpan="3">
							<asp:TextBox id="txtFmemo" Width="100%" Runat="server" TextMode="MultiLine"></asp:TextBox></TD>
					</TR>
					<TR>
						<TD colSpan="4" align="center">
							<asp:Button id="btnUnbind" Text="�����" Runat="server" Enabled="False"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
							<asp:Button id="btnCancel" Text=" ȡ  �� " Runat="server"></asp:Button></TD>
					</TR>
				</asp:panel>
			</TABLE>
		</form>
	</body>
</HTML>
