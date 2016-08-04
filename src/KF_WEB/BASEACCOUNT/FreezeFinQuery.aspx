<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="FreezeFinQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.FreezeFinQuery" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>�����ʽ��¼��ѯ</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
    <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE border="1" cellSpacing="1" cellPadding="1" width="1100">
				<TR>
					<TD style="WIDTH: 443px; HEIGHT: 20px" bgColor="#e4e5f7" colSpan="2"><FONT color="red" face="����"><IMG src="../IMAGES/Page/post.gif" width="20" height="16"><asp:label id="lb_pageTitle" Runat="server">�����ʽ��¼��ѯ</asp:label></FONT></TD>
					<td style="HEIGHT: 20px"></FONT>&nbsp;&nbsp;&nbsp;&nbsp;����Ա����: </FONT><SPAN class="style3"><asp:label id="lb_operatorID" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></td>
				</TR>
				<tr>
					<TD rowSpan="3" align="right"><asp:label id="Label1" Runat="server">��ѯ������</asp:label></TD>
					<TD style="WIDTH: 317px"><FONT face="����">&nbsp;&nbsp;&nbsp; </FONT>
						<asp:label id="Label3" runat="server">&nbsp;&nbsp;��ʼ���ڣ�&nbsp;&nbsp;</asp:label>
                        <input type="text" runat="server" id="tbx_beginDate" onclick="WdatePicker()" />
                        <img onclick="tbx_beginDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="ѡ������" />
					</TD>
					<td><asp:label id="Label2" runat="server">&nbsp;&nbsp;&nbsp;&nbsp;�������ڣ�&nbsp;&nbsp;&nbsp;&nbsp;</asp:label>
                        <input type="text" runat="server" id="tbx_endDate" onclick="WdatePicker()" />
                        <img onclick="tbx_endDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="ѡ������" />
					</td>
				</tr>
				<TR>
					<td style="WIDTH: 317px"><asp:label id="lb_listID" runat="server">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;�Ƹ�ͨ�ʺţ�</asp:label><asp:textbox id="tbx_payAccount" Runat="server" Width="150px"></asp:textbox></td>
					<td><asp:label id="Label4" Runat="server">&nbsp;&nbsp;&nbsp;&nbsp;�����Ԫ)</asp:label><asp:textbox id="tbx_fin" Runat="server" Width="150px"></asp:textbox></td>
				</TR>
				<tr>
					<TD align="right"><asp:button id="btnQuery" runat="server" Width="80" Text="�� ѯ" onclick="btnQuery_Click"></asp:button></TD>
				</tr>
			</TABLE>
			<table border="0" cellSpacing="0" cellPadding="0" width="1100">
				<TR>
					<TD vAlign="top"><asp:datagrid id="DataGrid_QueryResult" runat="server" Width="1100px" BorderColor="#E7E7FF" BorderStyle="None"
							BorderWidth="1px" BackColor="White" CellPadding="1" GridLines="Horizontal" AutoGenerateColumns="False"
							PageSize="5" HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
							Font-Size="13px">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="Fqqid" HeaderText="�Ƹ�ͨ�ʺ�">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="strFreason" HeaderText="����ԭ��">
									<HeaderStyle Width="150px"></HeaderStyle>
								</asp:BoundColumn>
                                <asp:BoundColumn DataField="strType" HeaderText="״̬">
									<HeaderStyle Width="110px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="strSignName" HeaderText="����״̬">
									<HeaderStyle Width="110px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fconnum" HeaderText="�����Ԫ��">
									<HeaderStyle Width="120px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fmodify_time" HeaderText="ʱ��">
									<HeaderStyle Width="160px" HorizontalAlign="Center"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Flistid" HeaderText="������/���׵���">
									<HeaderStyle Width="150px" HorizontalAlign="Center"></HeaderStyle>
								</asp:BoundColumn>
								<asp:ButtonColumn HeaderText="��ϸ����" ButtonType="LinkButton" Text="��ϸ����">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:ButtonColumn>
							</Columns>
							<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid><webdiyer:aspnetpager id="pager" runat="server" HorizontalAlign="right" AlwaysShow="True" NumericButtonTextFormatString="[{0}]"
							SubmitButtonText="ת��" CssClass="mypager" ShowInputBox="always" PageSize="10" PagingButtonSpacing="0" NumericButtonCount="5"></webdiyer:aspnetpager></TD>
				</TR>
			</table>
			<table id="tb_detail" border="0" cellSpacing="1" cellPadding="0" width="1100" bgColor="black"
				runat="server">
				<tr>
					<td bgColor="#eeeeee" height="18" colSpan="4"><span>&nbsp;��ϸ��Ϣ�б�</span></td>
				</tr>
				<TR>
					<TD style="WIDTH: 225px" bgColor="#eeeeee" height="18" width="195"><font face="����">&nbsp;&nbsp;���ᵥ��:</font></TD>
					<TD colspan="3" bgColor="#ffffff" height="19" width="236"><font face="����">&nbsp;<asp:label id="lb_c1" runat="server"></asp:label></font></TD>
					<%--<TD style="WIDTH: 229px; HEIGHT: 19px" bgColor="#eeeeee" height="19" width="229"><FONT style="BACKGROUND-COLOR: #eeeeee" face="����">&nbsp;&nbsp;��ˮ��:</FONT></TD>
					<TD style="HEIGHT: 19px" bgColor="#ffffff" height="19" width="225"><font face="����">&nbsp;<asp:label id="lb_c2" runat="server"></asp:label></font></TD>--%>
				</TR>
				<tr>
					<TD style="WIDTH: 225px" bgColor="#eeeeee" height="18" width="195"><font face="����">&nbsp;&nbsp;�û��ʺ�:</font></TD>
					<TD bgColor="#ffffff" height="19" width="236"><font face="����">&nbsp;<asp:label id="lb_c19" runat="server"></asp:label></font></TD>
					<TD style="WIDTH: 225px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><font face="����">&nbsp;&nbsp;�û�����:</font></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><font face="����">&nbsp;<asp:label id="lb_c3" runat="server"></asp:label></font></TD>
				</tr>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><font face="����">&nbsp;&nbsp;����ԭ��:</font></TD>
					<TD style="HEIGHT: 20px" bgColor="#ffffff" height="20"><font face="����">&nbsp;<asp:label id="lb_c4" runat="server"></asp:label></font></TD>
					<TD style="WIDTH: 225px; HEIGHT: 16px" bgColor="#eeeeee" height="16">&nbsp;&nbsp;&nbsp;���׽��(Ԫ):</TD>
					<TD style="WIDTH: 136px; HEIGHT: 16px" bgColor="#ffffff" height="16"><FONT face="����">&nbsp;<asp:label id="lb_c5" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 16px" bgColor="#eeeeee" height="16"><FONT face="����">&nbsp;&nbsp;������(Ԫ):</FONT></TD>
					<TD style="HEIGHT: 16px" bgColor="#ffffff" height="16"><font face="����">&nbsp;<asp:label id="lb_c6" runat="server"></asp:label></font></TD>
					<TD style="WIDTH: 225px; HEIGHT: 19px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;&nbsp;�ʻ����(Ԫ):</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;<asp:label id="lb_c7" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 19px" bgColor="#eeeeee" height="19"><FONT face="����">&nbsp;&nbsp;�ʻ��������(Ԫ):</FONT></TD>
					<TD style="HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;<asp:label id="lb_c8" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;&nbsp;����:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;<asp:label id="lb_c9" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;&nbsp;�û����е�����:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;<asp:label id="lb_c10" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 19px" bgColor="#eeeeee" height="18"><FONT face="����">����޸�ʱ�䣺</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;<asp:label id="lb_c11" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 19px" bgColor="#eeeeee" height="19"><FONT face="����">&nbsp;&nbsp;��ע:</FONT></TD>
					<TD style="HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;<FONT face="����"><asp:label id="lb_c12" runat="server"></asp:label></FONT></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 19px" bgColor="#eeeeee" height="18"><FONT face="����">���׵��ţ�</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;<asp:label id="lb_c13" runat="server"></asp:label></FONT></TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>
