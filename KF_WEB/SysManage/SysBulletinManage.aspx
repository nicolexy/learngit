<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<%@ Page language="c#" Codebehind="SysBulletinManage.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.SysManage.SysBulletinManage" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>SysBulletinManage</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		<script src="../SCRIPTS/Local.js"></script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����">
			</FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����">
			</FONT><FONT face="����"></FONT><FONT face="����"></FONT><FONT face="����"></FONT>
			<br>
			<TABLE id="Table4" style="Z-INDEX: 101; LEFT: 16px; WIDTH: 1040px" cellSpacing="1" cellPadding="1"
				width="1040" align="center" border="1">
				<TR bgColor="#eeeeee">
					<TD colSpan="3"><IMG height="16" src="../IMAGES/Page/post.gif" width="15">&nbsp;<asp:label id="lbTitle" runat="server" ForeColor="Red">ϵͳ�������</asp:label>
						&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
						</FONT>����Ա����: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" Width="73px" ForeColor="Red"></asp:label></SPAN>
					</TD>
				</TR>
				<TR>
					<TD><asp:dropdownlist id="ddlSysList" runat="server" AutoPostBack="True">
							<asp:ListItem Value="1" Selected="True">�̻�ϵͳ�����б�</asp:ListItem>
							<asp:ListItem Value="2">�Ƹ�ͨϵͳ��ҳ����</asp:ListItem>
							<asp:ListItem Value="6">��ҵ���׸�����</asp:ListItem>
                            <asp:ListItem Value="8">�Ƹ�ͨ����ά������</asp:ListItem>
                            <asp:ListItem Value="7">����ɷ�ά������</asp:ListItem>
                            <asp:ListItem Value="11">����ά������</asp:ListItem>
                            <asp:ListItem Value="12">�����п�����ά������</asp:ListItem>
                            <asp:ListItem Value="13">������ά������</asp:ListItem>
                            <asp:ListItem Value="14">���ÿ�����ά������</asp:ListItem>
                            <asp:ListItem Value="21">�������нӿ�</asp:ListItem>
                            <asp:ListItem Value="22">�����п��������нӿ�</asp:ListItem>
                            <asp:ListItem Value="23">���������нӿ�</asp:ListItem>
                            <asp:ListItem Value="24">���ÿ��������нӿ�</asp:ListItem>
						</asp:dropdownlist></TD>
                        	<TD style="WIDTH: 290px"><asp:label id="labQueryName" runat="server">�������ͱ���</asp:label><asp:textbox id="txtBankType" runat="server" Width="130px"></asp:textbox>
						<asp:DropDownList id="ddl_uctype" runat="server" Visible="False" Enabled="False">
							<asp:ListItem Value="1">ÿ��</asp:ListItem>
							<asp:ListItem Value="2">ÿ��</asp:ListItem>
							<asp:ListItem Value="4">ÿ��</asp:ListItem>
							<asp:ListItem Value="8" Selected="True">ά��</asp:ListItem>
						</asp:DropDownList></TD>
                        <TD><asp:button id="btnQuery" runat="server" Text="��ѯ��¼" onclick="btnQuery_Click"></asp:button>&nbsp;&nbsp;&nbsp;
                        <asp:button id="btadd" runat="server" Width="80px" Text="����" onclick="btadd_Click"></asp:button>
                         <asp:button id="btQueryContacts" runat="server" Width="100px" Text="�ռ�����������"></asp:button>
                        </TD>
				</TR>
                </TABLE>
			<TABLE id="Table1" style="Z-INDEX: 102; LEFT: 16px; WIDTH: 1040px" cellSpacing="1" cellPadding="1"
				width="1040" align="center" border="1" runat="server">
				<TR>
					<TD><asp:datagrid id="DataGrid1" runat="server" Width="100%" EnableViewState="False" PageSize="200"  OnItemDataBound="DGData_ItemDataBound"
							AutoGenerateColumns="False" CellPadding="3" BackColor="White" BorderWidth="1px" BorderStyle="None"
							BorderColor="#E7E7FF" GridLines="Horizontal">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
								<asp:BoundColumn Visible="False" DataField="FID" HeaderText="FID"></asp:BoundColumn>
								<asp:TemplateColumn HeaderText="���" Visible="False">
									<ItemTemplate>
										<asp:Label id="labOrder" runat="server">1</asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="FissueTime" HeaderText="ʱ��" DataFormatString="{0:D}"></asp:BoundColumn>
								<asp:BoundColumn DataField="FTitle" HeaderText="�������"></asp:BoundColumn>
								<asp:BoundColumn DataField="FIsNewName" HeaderText="�Ƿ�����"></asp:BoundColumn>
								<asp:TemplateColumn HeaderText="�鿴">
									<ItemTemplate>
										<asp:HyperLink id=hlURL runat="server" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.FUrl").ToString().Trim() %>' Target="_blank">�鿴</asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="FLastTime" HeaderText="����ʱ��"></asp:BoundColumn>
								<asp:BoundColumn DataField="FuserId" HeaderText="������"></asp:BoundColumn>
								<asp:TemplateColumn HeaderText="�ı�˳��">
									<ItemTemplate>
										<asp:LinkButton id="lbPrior" runat="server" CommandName="PRIOR">����</asp:LinkButton>&nbsp;
										<asp:LinkButton id="lbNext" runat="server" CommandName="NEXT">����</asp:LinkButton>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="�޸�">
									<ItemTemplate>
										<asp:LinkButton id="lbChange" runat="server" CommandName="CHANGE">�޸�</asp:LinkButton>&nbsp;
										<asp:LinkButton id="lbGoHistory" runat="server" CommandName="GOHISTORY" Visible="False">�Ƶ���ʷ</asp:LinkButton>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="ɾ��">
									<ItemTemplate>
										<asp:LinkButton id="lbDel" runat="server" CommandName="DEL">ɾ��</asp:LinkButton>
									</ItemTemplate>
								</asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="����">
									<ItemTemplate>
										 <asp:LinkButton id="lbNotify" runat="server" Visible="false" Text="֪ͨ" href='<%# DataBinder.Eval(Container, "DataItem.UrlNotify").ToString().Trim() %>'  target=_blank></asp:LinkButton>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
							<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD align="center"><asp:button id="btnNew" runat="server" Text="����" Width="80px" onclick="btnNew_Click"></asp:button>&nbsp;
						<asp:button id="btnIssue" runat="server" Text="����" Width="80px" onclick="btnIssue_Click"></asp:button></TD>
				</TR>
				<TR>
					<TD><PRE>	
<span style="FONT-SIZE: 9pt; FONT-FAMILY: ����; mso-ascii-font-family: 'Times New Roman'; mso-hansi-font-family: 'Times New Roman'">��ܰ��ʾ��</span><span lang=EN-US style="FONT-SIZE: 9pt"><o:p></o:p></span>
<span lang=EN-US style="FONT-SIZE: 9pt; COLOR: red">1</span><span style="FONT-SIZE: 9pt; COLOR: red; FONT-FAMILY: ����; mso-ascii-font-family: 'Times New Roman'; mso-hansi-font-family: 'Times New Roman'">����������󣬹�����������������������������ʹ��</span><span lang=EN-US style="FONT-SIZE: 9pt; COLOR: red"><o:p></o:p></span>
<span lang=EN-US style="FONT-SIZE: 9pt">2</span><span style="FONT-SIZE: 9pt; FONT-FAMILY: ����; mso-ascii-font-family: 'Times New Roman'; mso-hansi-font-family: 'Times New Roman'">�������Ե���������޸ġ�ɾ�������ơ��������༭�˹����б��⼸����������������Ч��ֻ�е����������ť�󣬲���ʽ����������������</span><span lang=EN-US style="FONT-SIZE: 9pt"><o:p></o:p></span>
					</PRE>
					</TD>
				</TR>
			</TABLE>
            <TABLE id="Table3" style="Z-INDEX: 103; LEFT: 16px; WIDTH: 1005px" cellSpacing="1" cellPadding="1"
				width="1005" align="center" border="1" runat="server">
				<TR>
					<TD><FONT face="����">
							<asp:datagrid id="Datagrid2" runat="server" Width="1032px" EnableViewState="False" PageSize="200"
								AutoGenerateColumns="False" CellPadding="3" BackColor="White" BorderWidth="1px" BorderStyle="None"
								BorderColor="#E7E7FF" GridLines="Horizontal">
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<Columns>
									<asp:BoundColumn DataField="Fid" HeaderText="���" Visible="false"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="Fbanktype" HeaderText="���б���"></asp:BoundColumn>
									<asp:BoundColumn DataField="FBank_TypeName" HeaderText="��������"></asp:BoundColumn>
									<asp:BoundColumn DataField="Ftitle" HeaderText="�������"></asp:BoundColumn>
									<asp:BoundColumn DataField="Fstartime" HeaderText="��ʼʱ��"></asp:BoundColumn>
									<asp:BoundColumn DataField="Fendtime" HeaderText="����ʱ��"></asp:BoundColumn>
									<asp:BoundColumn DataField="Fcreateuser" HeaderText="������"></asp:BoundColumn>
									<asp:BoundColumn DataField="Fcreatetime" HeaderText="����ʱ��"></asp:BoundColumn>
									<asp:BoundColumn DataField="Fupdateuser" HeaderText="�޸���"></asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="Fupdatetime" HeaderText="�޸�ʱ��"></asp:BoundColumn>
									<asp:TemplateColumn HeaderText="����">
										<ItemTemplate>
											<A href='<%# String.Format("SysBulletinManage_Detail.aspx?sysid={0}&fid={1}&opertype=2",DataBinder.Eval(Container, "DataItem.Fsysid").ToString(), DataBinder.Eval(Container, "DataItem.Fid").ToString()) %>'>
												�༭/�鿴 </A>
										</ItemTemplate>
									</asp:TemplateColumn>
								</Columns>
								<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
							</asp:datagrid></FONT></TD>
				</TR>
			</TABLE>
			<TABLE id="Table2" cellSpacing="1" cellPadding="1" width="1005" border="1" runat="server"
				align="center" style="Z-INDEX: 103; LEFT: 16px; WIDTH: 1005px">
				<TR>
					<TD><FONT face="����">
							<asp:datagrid id="Datagrid3" runat="server" Width="1032px" EnableViewState="False" PageSize="200"
								AutoGenerateColumns="False" CellPadding="3" BackColor="White" BorderWidth="1px" BorderStyle="None"
								BorderColor="#E7E7FF" GridLines="Horizontal">
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<Columns>
									<asp:BoundColumn DataField="FResourcesId" HeaderText="ҵ����"></asp:BoundColumn>
									<asp:BoundColumn DataField="FserviceName" HeaderText="ҵ������"></asp:BoundColumn>
									<asp:BoundColumn DataField="FAgentName" HeaderText="��Ӧ����"></asp:BoundColumn>
									<asp:BoundColumn DataField="FProvince_name" HeaderText="ʡ��"></asp:BoundColumn>
									<asp:BoundColumn DataField="FCity_name" HeaderText="������"></asp:BoundColumn>
									<asp:BoundColumn DataField="FArea_name" HeaderText="������"></asp:BoundColumn>
									<asp:BoundColumn DataField="Fcreatetime" HeaderText="����ʱ��"></asp:BoundColumn>
									<asp:TemplateColumn>
										<ItemTemplate>
											<A href='<%# String.Format("SysBulletinManage_Detail.aspx?sysid=7&fid={0}&opertype=2&uctype=8", DataBinder.Eval(Container, "DataItem.FResourcesId").ToString()) %>'>
												�༭/�鿴 </A>
										</ItemTemplate>
									</asp:TemplateColumn>
								</Columns>
								<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
							</asp:datagrid></FONT></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
