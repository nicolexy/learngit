<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="historyModify.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.historyModify" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>historyModify</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
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
		</script>
		<script language="javascript">
					function openModeEnd()
					{
					var returnValue=window.showModalDialog("../Control/CalendarForm2.aspx",Form1.TextBoxEndDate.value,'dialogWidth:375px;DialogHeight=260px;status:no');
					if(returnValue != null) Form1.TextBoxEndDate.value=returnValue;
					}
		</script>
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<br>
			<table border="0" cellSpacing="1" cellPadding="0" width="95%" bgColor="#666666" align="center">
				<tr bgColor="#e4e5f7" background="../IMAGES/Page/bg_bl.gif">
					<td style="HEIGHT: 16px" height="16" vAlign="middle" colSpan="2">
						<table border="0" cellSpacing="0" cellPadding="1" width="100%" background="../IMAGES/Page/bg_bl.gif"
							height="8">
							<tr background="../IMAGES/Page/bg_bl.gif">
								<td style="HEIGHT: 24px" height="24" width="80%"><font color="#ff0000"><STRONG><FONT color="#ff0000">&nbsp;</FONT></STRONG><IMG src="../IMAGES/Page/post.gif" width="20" height="16">
										�û��޸���Ϣ </font><span class="style6"><FONT color="red">��ѯ</FONT></span>
								</td>
								<td style="HEIGHT: 24px" width="20%"></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr bgColor="#ffffff">
					<td><FONT face="����">
							<TABLE id="Table1" border="0" cellSpacing="0" cellPadding="1" width="100%" align="center"
								height="45">
								<TBODY>
									<TR bgColor="lightgrey">
										<TD style="HEIGHT: 24px"><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT></TD>
										<TD style="HEIGHT: 24px" colSpan="2"><FONT face="����">&nbsp; ��ʼ����
												<asp:textbox id="TextBoxBeginDate" runat="server" Width="122px">2005-05-01</asp:textbox><asp:imagebutton id="ButtonBeginDate" runat="server" CausesValidation="False" ImageUrl="../Images/Public/edit.gif"></asp:imagebutton>&nbsp;&nbsp;&nbsp;&nbsp; 
												��������
												<asp:textbox id="TextBoxEndDate" runat="server" Width="122px"></asp:textbox><asp:imagebutton id="ButtonEndDate" runat="server" CausesValidation="False" ImageUrl="../Images/Public/edit.gif"></asp:imagebutton></FONT></TD>
										<TD style="HEIGHT: 24px"><FONT face="����">&nbsp;&nbsp; </FONT>
										</TD>
									</TR>
									<TR bgColor="#eeeeee">
										<TD><FONT face="����"></FONT></TD>
										<TD colSpan="2"><FONT face="����">&nbsp; ��ȷ��ѯ
												<asp:textbox id="txbCustom" runat="server" Width="122px"></asp:textbox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
												��&nbsp; ����
												<asp:dropdownlist id="ddlCondition" runat="server" Width="131px" AutoPostBack="True" onselectedindexchanged="ddlCondition_SelectedIndexChanged">
													<asp:ListItem Value="fqqid" Selected="True">���û��ʺ�</asp:ListItem>
													<asp:ListItem Value="FbankNO">�����п�����</asp:ListItem>
													<asp:ListItem Value="FName">���޸�ǰ����</asp:ListItem>
													<asp:ListItem Value="FNameNew">���޸ĺ�����</asp:ListItem>
													<asp:ListItem Value="FfetchMail">��ȡ��������</asp:ListItem>
												</asp:dropdownlist></FONT></TD>
										<TD><FONT face="����"></FONT></TD>
									</TR>
								</TBODY>
							</TABLE>
						</FONT>
					</td>
					<td width="15%">
						<DIV align="center">&nbsp;
							<asp:button id="btQuery" runat="server" Width="108px" BorderStyle="Groove" Text="�� ѯ" Height="27px" onclick="btQuery_Click"></asp:button></DIV>
					</td>
				</tr>
			</table>
			<br>
			<TABLE id="Table1" border="0" cellSpacing="0" cellPadding="0" width="95%" align="center"
				height="0">
				<TR>
					<TD bgColor="#666666">
						<TABLE id="Table2" border="0" cellSpacing="1" cellPadding="0" width="100%" align="center">
							<TR bgColor="#e4e5f7">
								<TD style="HEIGHT: 21px" bgColor="#e4e5f7" height="21" background="../IMAGES/Page/bg_bl.gif"
									colSpan="2">
									<table border="0" cellSpacing="0" cellPadding="0" width="100%">
										<tr>
											<td style="WIDTH: 754px" width="754"><FONT color="#ff0000"><IMG src="../IMAGES/Page/post.gif" width="20" height="16">&nbsp; 
													��ʷ��Ϣ�޸���ϸ</FONT>
											</td>
											<td style="WIDTH: 125px" width="125">
												<div align="center">�Զ����ҳ��С��</div>
											</td>
											<td width="15%">
												<P align="left">&nbsp;
													<asp:dropdownlist id="ddlPageSize" runat="server" Width="88px">
														<asp:ListItem Value="20">ÿҳ20��</asp:ListItem>
														<asp:ListItem Value="30">ÿҳ30��</asp:ListItem>
														<asp:ListItem Value="50">ÿҳ50��</asp:ListItem>
													</asp:dropdownlist></P>
											</td>
										</tr>
									</table>
								</TD>
							</TR>
							<TR>
								<TD bgColor="#ffffff" height="12"><FONT face="����">
										<DIV align="left"><asp:datagrid id="dgInfo" runat="server" Width="100%" AutoGenerateColumns="False">
												<AlternatingItemStyle BackColor="#EEEEEE"></AlternatingItemStyle>
												<HeaderStyle Font-Bold="True" Wrap="False" BackColor="ActiveBorder"></HeaderStyle>
												<Columns>
													<asp:BoundColumn DataField="Fqqid" HeaderText="�û��ʺ�">
														<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="FName" HeaderText="�޸�ǰ����">
														<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="FNameNew" HeaderText="�޸ĺ�����">
														<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="FcleanMibao" HeaderText="���ܱ�">
														<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="FfetchMail" HeaderText="ȡ���ʼ���ַ">
														<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="Freason" HeaderText="�޸�ԭ��">
														<HeaderStyle Wrap="False"></HeaderStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="FcommitTime" HeaderText="�ύʱ��">
														<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
													</asp:BoundColumn>
												</Columns>
											</asp:datagrid></DIV>
									</FONT>
									<webdiyer:aspnetpager id="AspNetPager1" runat="server" AlwaysShow="True" ShowCustomInfoSection="left"
										NumericButtonTextFormatString="[{0}]" SubmitButtonText="ת��" HorizontalAlign="right" CssClass="mypager"
										ShowInputBox="always" PagingButtonSpacing="0" NumericButtonCount="10"></webdiyer:aspnetpager></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
			<FONT face="����"></FONT>
		</form>
	</body>
</HTML>
