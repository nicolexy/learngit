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
			<FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT>
			<br>
			<TABLE id="Table4" style="Z-INDEX: 101; LEFT: 16px; WIDTH: 1040px" cellSpacing="1" cellPadding="1"
				width="1040" align="center" border="1">
				<TR bgColor="#eeeeee">
					<TD colSpan="3"><IMG height="16" src="../IMAGES/Page/post.gif" width="15">&nbsp;<asp:label id="lbTitle" runat="server" ForeColor="Red">系统公告管理</asp:label>
						&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
						</FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" Width="73px" ForeColor="Red"></asp:label></SPAN>
					</TD>
				</TR>
				<TR>
					<TD><asp:dropdownlist id="ddlSysList" runat="server" AutoPostBack="True">
							<asp:ListItem Value="1" Selected="True">商户系统公告列表</asp:ListItem>
							<asp:ListItem Value="2">财付通系统首页公告</asp:ListItem>
							<asp:ListItem Value="6">企业收易付公告</asp:ListItem>
                            <asp:ListItem Value="8">财付通银行维护公告</asp:ListItem>
                            <asp:ListItem Value="7">生活缴费维护公告</asp:ListItem>
                            <asp:ListItem Value="11">提现维护公告</asp:ListItem>
                            <asp:ListItem Value="12">向银行卡付款维护公告</asp:ListItem>
                            <asp:ListItem Value="13">还房贷维护公告</asp:ListItem>
                            <asp:ListItem Value="14">信用卡还款维护公告</asp:ListItem>
                            <asp:ListItem Value="21">提现银行接口</asp:ListItem>
                            <asp:ListItem Value="22">向银行卡付款银行接口</asp:ListItem>
                            <asp:ListItem Value="23">还房贷银行接口</asp:ListItem>
                            <asp:ListItem Value="24">信用卡还款银行接口</asp:ListItem>
						</asp:dropdownlist></TD>
                        	<TD style="WIDTH: 290px"><asp:label id="labQueryName" runat="server">银行类型编码</asp:label><asp:textbox id="txtBankType" runat="server" Width="130px"></asp:textbox>
						<asp:DropDownList id="ddl_uctype" runat="server" Visible="False" Enabled="False">
							<asp:ListItem Value="1">每日</asp:ListItem>
							<asp:ListItem Value="2">每周</asp:ListItem>
							<asp:ListItem Value="4">每月</asp:ListItem>
							<asp:ListItem Value="8" Selected="True">维护</asp:ListItem>
						</asp:DropDownList></TD>
                        <TD><asp:button id="btnQuery" runat="server" Text="查询记录" onclick="btnQuery_Click"></asp:button>&nbsp;&nbsp;&nbsp;
                        <asp:button id="btadd" runat="server" Width="80px" Text="新增" onclick="btadd_Click"></asp:button>
                         <asp:button id="btQueryContacts" runat="server" Width="100px" Text="收件人名单管理"></asp:button>
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
								<asp:TemplateColumn HeaderText="序号" Visible="False">
									<ItemTemplate>
										<asp:Label id="labOrder" runat="server">1</asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="FissueTime" HeaderText="时间" DataFormatString="{0:D}"></asp:BoundColumn>
								<asp:BoundColumn DataField="FTitle" HeaderText="公告标题"></asp:BoundColumn>
								<asp:BoundColumn DataField="FIsNewName" HeaderText="是否最新"></asp:BoundColumn>
								<asp:TemplateColumn HeaderText="查看">
									<ItemTemplate>
										<asp:HyperLink id=hlURL runat="server" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.FUrl").ToString().Trim() %>' Target="_blank">查看</asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="FLastTime" HeaderText="到期时间"></asp:BoundColumn>
								<asp:BoundColumn DataField="FuserId" HeaderText="发布人"></asp:BoundColumn>
								<asp:TemplateColumn HeaderText="改变顺序">
									<ItemTemplate>
										<asp:LinkButton id="lbPrior" runat="server" CommandName="PRIOR">上移</asp:LinkButton>&nbsp;
										<asp:LinkButton id="lbNext" runat="server" CommandName="NEXT">下移</asp:LinkButton>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="修改">
									<ItemTemplate>
										<asp:LinkButton id="lbChange" runat="server" CommandName="CHANGE">修改</asp:LinkButton>&nbsp;
										<asp:LinkButton id="lbGoHistory" runat="server" CommandName="GOHISTORY" Visible="False">移到历史</asp:LinkButton>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="删除">
									<ItemTemplate>
										<asp:LinkButton id="lbDel" runat="server" CommandName="DEL">删除</asp:LinkButton>
									</ItemTemplate>
								</asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="操作">
									<ItemTemplate>
										 <asp:LinkButton id="lbNotify" runat="server" Visible="false" Text="通知" href='<%# DataBinder.Eval(Container, "DataItem.UrlNotify").ToString().Trim() %>'  target=_blank></asp:LinkButton>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
							<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD align="center"><asp:button id="btnNew" runat="server" Text="新增" Width="80px" onclick="btnNew_Click"></asp:button>&nbsp;
						<asp:button id="btnIssue" runat="server" Text="发布" Width="80px" onclick="btnIssue_Click"></asp:button></TD>
				</TR>
				<TR>
					<TD><PRE>	
<span style="FONT-SIZE: 9pt; FONT-FAMILY: 宋体; mso-ascii-font-family: 'Times New Roman'; mso-hansi-font-family: 'Times New Roman'">温馨提示：</span><span lang=EN-US style="FONT-SIZE: 9pt"><o:p></o:p></span>
<span lang=EN-US style="FONT-SIZE: 9pt; COLOR: red">1</span><span style="FONT-SIZE: 9pt; COLOR: red; FONT-FAMILY: 宋体; mso-ascii-font-family: 'Times New Roman'; mso-hansi-font-family: 'Times New Roman'">、点击发布后，公告立即更新上生产环境，请慎重使用</span><span lang=EN-US style="FONT-SIZE: 9pt; COLOR: red"><o:p></o:p></span>
<span lang=EN-US style="FONT-SIZE: 9pt">2</span><span style="FONT-SIZE: 9pt; FONT-FAMILY: 宋体; mso-ascii-font-family: 'Times New Roman'; mso-hansi-font-family: 'Times New Roman'">、您可以点击新增、修改、删除、上移、下移来编辑此公告列表，这几个操作不会立即生效，只有当点击发布按钮后，才正式发布到生产环境。</span><span lang=EN-US style="FONT-SIZE: 9pt"><o:p></o:p></span>
					</PRE>
					</TD>
				</TR>
			</TABLE>
            <TABLE id="Table3" style="Z-INDEX: 103; LEFT: 16px; WIDTH: 1005px" cellSpacing="1" cellPadding="1"
				width="1005" align="center" border="1" runat="server">
				<TR>
					<TD><FONT face="宋体">
							<asp:datagrid id="Datagrid2" runat="server" Width="1032px" EnableViewState="False" PageSize="200"
								AutoGenerateColumns="False" CellPadding="3" BackColor="White" BorderWidth="1px" BorderStyle="None"
								BorderColor="#E7E7FF" GridLines="Horizontal">
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<Columns>
									<asp:BoundColumn DataField="Fid" HeaderText="序号" Visible="false"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="Fbanktype" HeaderText="银行编码"></asp:BoundColumn>
									<asp:BoundColumn DataField="FBank_TypeName" HeaderText="银行类型"></asp:BoundColumn>
									<asp:BoundColumn DataField="Ftitle" HeaderText="公告标题"></asp:BoundColumn>
									<asp:BoundColumn DataField="Fstartime" HeaderText="开始时间"></asp:BoundColumn>
									<asp:BoundColumn DataField="Fendtime" HeaderText="结束时间"></asp:BoundColumn>
									<asp:BoundColumn DataField="Fcreateuser" HeaderText="创建人"></asp:BoundColumn>
									<asp:BoundColumn DataField="Fcreatetime" HeaderText="创建时间"></asp:BoundColumn>
									<asp:BoundColumn DataField="Fupdateuser" HeaderText="修改人"></asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="Fupdatetime" HeaderText="修改时间"></asp:BoundColumn>
									<asp:TemplateColumn HeaderText="操作">
										<ItemTemplate>
											<A href='<%# String.Format("SysBulletinManage_Detail.aspx?sysid={0}&fid={1}&opertype=2",DataBinder.Eval(Container, "DataItem.Fsysid").ToString(), DataBinder.Eval(Container, "DataItem.Fid").ToString()) %>'>
												编辑/查看 </A>
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
					<TD><FONT face="宋体">
							<asp:datagrid id="Datagrid3" runat="server" Width="1032px" EnableViewState="False" PageSize="200"
								AutoGenerateColumns="False" CellPadding="3" BackColor="White" BorderWidth="1px" BorderStyle="None"
								BorderColor="#E7E7FF" GridLines="Horizontal">
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<Columns>
									<asp:BoundColumn DataField="FResourcesId" HeaderText="业务编号"></asp:BoundColumn>
									<asp:BoundColumn DataField="FserviceName" HeaderText="业务名称"></asp:BoundColumn>
									<asp:BoundColumn DataField="FAgentName" HeaderText="供应商名"></asp:BoundColumn>
									<asp:BoundColumn DataField="FProvince_name" HeaderText="省名"></asp:BoundColumn>
									<asp:BoundColumn DataField="FCity_name" HeaderText="城市名"></asp:BoundColumn>
									<asp:BoundColumn DataField="FArea_name" HeaderText="地区名"></asp:BoundColumn>
									<asp:BoundColumn DataField="Fcreatetime" HeaderText="创建时间"></asp:BoundColumn>
									<asp:TemplateColumn>
										<ItemTemplate>
											<A href='<%# String.Format("SysBulletinManage_Detail.aspx?sysid=7&fid={0}&opertype=2&uctype=8", DataBinder.Eval(Container, "DataItem.FResourcesId").ToString()) %>'>
												编辑/查看 </A>
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
