<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="GetUserFundAccountInfoPage.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages.GetUserFundAccountInfoPage" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>GetUserFundAccountInfoPage</title>
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
			<TABLE border="1" cellSpacing="1" cellPadding="1" width="1100">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="5"><FONT face="����"><FONT color="red"><IMG src="../IMAGES/Page/post.gif" width="20" height="16">&nbsp;&nbsp;��ѯͶ���˻������ʺ���Ϣ</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>����Ա����: </FONT><SPAN class="style3"><asp:label id="lb_operatorID" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></TD>
				</TR>
				<tr>
					<td><FONT face="����">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</FONT><span>��ѯ���ͣ�</span>
						<asp:dropdownlist id="dd_queryType" AutoPostBack="True" Runat="server">
							<asp:ListItem Value="1" Selected="True">�������˻���ӦID</asp:ListItem>
							<asp:ListItem Value="2" Selected="False">֤������</asp:ListItem>
							<asp:ListItem Value="3" Selected="False">�Ƹ�ͨ�ʺ�</asp:ListItem>
							<asp:ListItem Value="4" Selected="False">�Ƹ�ͨID</asp:ListItem>
                            <asp:ListItem Value="5" Selected="False">���ͨ�˺�</asp:ListItem>
						</asp:dropdownlist></td>
				</tr>
				<tr>
					<td><FONT face="����">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</FONT>
						<asp:label id="lb_param1" Runat="server">�������˻�ID��</asp:label><asp:textbox id="tbx_param1" Runat="server"></asp:textbox><asp:dropdownlist id="dd_creType" Runat="server" Visible="False">
							<asp:ListItem Value="���֤" Selected="True" />
							<asp:ListItem Value="����" Selected="false" />
							<asp:ListItem Value="��ʱ���֤" Selected="false" />
							<asp:ListItem Value="���ڲ�" Selected="false" />
						</asp:dropdownlist></td>
					<td><FONT face="����">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</FONT><asp:label id="lb_param2" Runat="server" Visible="False">����2��</asp:label><asp:textbox id="tbx_param2" Runat="server" Visible="False"></asp:textbox></td>
				</tr>
				<tr>
					<td colSpan="5" align="center"><asp:button id="btn_serach" Width="80px" Runat="server" Text="��ѯ"></asp:button></td>
				</tr>
			</TABLE>
            <table border="0" cellSpacing="0" cellPadding="0" width="1100">
				<TR>
					<TD vAlign="top"><asp:datagrid id="DataGrid1" runat="server" Width="1100px" BorderColor="#E7E7FF" BorderStyle="None"
							BorderWidth="1px" BackColor="White" CellPadding="1" GridLines="Horizontal" AutoGenerateColumns="False"
							PageSize="5" HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="Ftrade_id" HeaderText="�������˻���ӦID">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fuid" HeaderText="�û���CFT�ڲ�ID">
									<HeaderStyle Width="200px" HorizontalAlign="Center"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Ftrue_name" HeaderText="Ͷ������ʵ����">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="FstateName" HeaderText="������Ч��־">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fcre_type_name" HeaderText="֤������">
									<HeaderStyle Width="80px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fcre_id_hide" HeaderText="֤������">
									<HeaderStyle Width="50px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="FlstateName" HeaderText="����״̬">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fcreate_time" HeaderText="����ʱ��">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fmodify_time" HeaderText="����޸�ʱ��">
									<HeaderStyle Width="50px"></HeaderStyle>
								</asp:BoundColumn>
                                <asp:BoundColumn DataField="Fmobile" HeaderText="���ֻ�">
									<HeaderStyle Width="50px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:ButtonColumn ButtonType="LinkButton" HeaderText="��ϸ��Ϣ" Text="��ϸ" CommandName="detail">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:ButtonColumn>
							</Columns>
							<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid><webdiyer:aspnetpager id="pager" runat="server" HorizontalAlign="right" AlwaysShow="True" NumericButtonTextFormatString="[{0}]"
							SubmitButtonText="ת��" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0" NumericButtonCount="5"></webdiyer:aspnetpager></TD>
				</TR>
			</table>
			<table border="0" cellSpacing="1" cellPadding="0" width="1100" bgColor="black">
				<tr>
					<td bgColor="#eeeeee" height="18" colSpan="4"><span>&nbsp;��ϸ��Ϣ�б�</span></td>
				</tr>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 19px" bgColor="#eeeeee" height="19" width="229"><font face="����">&nbsp;<FONT style="BACKGROUND-COLOR: #eeeeee" face="����">�������˻���ӦID��</FONT></font></TD>
					<TD style="HEIGHT: 19px" bgColor="#ffffff" height="19" width="225"><font face="����">&nbsp;<asp:label id="lb_c1" runat="server"></asp:label></font></TD>
					<TD style="WIDTH: 225px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><font face="����">&nbsp;�û���CFT�ڲ�ID:</font></TD>
					<TD style="WIDTH: 136px; HEIGHT: 20px" bgColor="#ffffff" height="20"><font face="����">&nbsp;<asp:label id="lb_c2" runat="server"></asp:label></font></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 20px" bgColor="#eeeeee" height="20"><font face="����">&nbsp;Ͷ������ʵ����:</font></TD>
					<TD style="HEIGHT: 20px" bgColor="#ffffff" height="20"><font face="����">&nbsp;<asp:label id="lb_c3" runat="server"></asp:label></font></TD>
					<TD style="WIDTH: 225px; HEIGHT: 16px" bgColor="#eeeeee" height="16">&nbsp;&nbsp;������Ч��־:</TD>
					<TD style="WIDTH: 136px; HEIGHT: 16px" bgColor="#ffffff" height="16"><FONT face="����">&nbsp;<asp:label id="lb_c4" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 16px" bgColor="#eeeeee" height="16"><FONT face="����">&nbsp;֤������:</FONT></TD>
					<TD style="HEIGHT: 16px" bgColor="#ffffff" height="16"><font face="����">&nbsp;<asp:label id="lb_c5" runat="server"></asp:label></font></TD>
					<TD style="WIDTH: 225px; HEIGHT: 19px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;֤������:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;<asp:label id="lb_c6" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 19px" bgColor="#eeeeee" height="19"><FONT face="����">&nbsp;����״̬:</FONT></TD>
					<TD style="HEIGHT: 19px" bgColor="#ffffff" height="19"><FONT face="����">&nbsp;<asp:label id="lb_c7" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 225px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;����ʱ��:</FONT></TD>
					<TD style="WIDTH: 136px; HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;<asp:label id="lb_c8" runat="server"></asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;����޸�ʱ��:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;<asp:label id="lb_c9" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;���ֻ���</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;<asp:label id="lb_c10" runat="server"></asp:label></FONT></TD>
				</TR>
                <TR>
                    <TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�˻����:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18" colspan="3"><FONT face="����">&nbsp;<asp:label id="lb_c15" runat="server"></asp:label></FONT></TD>
				</TR>
				<%--<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�����п�:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;<asp:label id="lb_c11" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�˻��ʽ�</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;<asp:label id="lb_c12" runat="server">&nbsp;</asp:label></FONT></TD>
				</TR>
				<TR>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;�����п�״̬:</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;<asp:label id="lb_c13" runat="server"></asp:label></FONT></TD>
					<TD style="WIDTH: 229px; HEIGHT: 18px" bgColor="#eeeeee" height="18"><FONT face="����">&nbsp;��ʱ�䣺</FONT></TD>
					<TD style="HEIGHT: 18px" bgColor="#ffffff" height="18"><FONT face="����">&nbsp;<asp:label id="lb_c14" runat="server">&nbsp;</asp:label></FONT></TD>
				</TR>--%>
			</table>
			<br>
			<p>�û������п��б�</p>
			<table border="0" cellSpacing="0" cellPadding="0" width="1100">
				<TR>
					<TD vAlign="top"><asp:datagrid id="DataGrid_QueryResult" runat="server" Width="1100px" BorderColor="#E7E7FF" BorderStyle="None"
							BorderWidth="1px" BackColor="White" CellPadding="1" GridLines="Horizontal" AutoGenerateColumns="False"
							PageSize="5" HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="Fbank_name" HeaderText="������������">
									<HeaderStyle Width="120px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fcard_tail" HeaderText="���п�β��">
									<HeaderStyle Width="200px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fbank_typeName" HeaderText="������������">
									<HeaderStyle Width="130px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fbind_stateName" HeaderText="���п���״̬">
									<HeaderStyle Width="100px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="Fcreate_time" HeaderText="��ʱ��">
									<HeaderStyle Width="200px" HorizontalAlign="Center"></HeaderStyle>
								</asp:BoundColumn>
							</Columns>
							<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>
