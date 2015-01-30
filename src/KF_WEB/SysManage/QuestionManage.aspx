<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<%@ Page language="c#" Codebehind="QuestionManage.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.SysManage.QuestionManage" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>QuestionManage</title>
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
					<TD><IMG height="16" src="../IMAGES/Page/post.gif" width="15">&nbsp;<asp:label id="lbTitle" runat="server" ForeColor="Red">常见问题维护</asp:label>
						&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
						</FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" Width="73px" ForeColor="Red"></asp:label></SPAN>
					</TD>
				</TR>
				<TR>
					<TD><asp:dropdownlist id="ddlSysList" runat="server" AutoPostBack="True">
							<asp:ListItem Value="51" Selected="True">信用卡还款页面</asp:ListItem>
							<asp:ListItem Value="52">向银行卡付款页面</asp:ListItem>
							<asp:ListItem Value="53">提现页面</asp:ListItem>
                            <asp:ListItem Value="54">快捷支付页面</asp:ListItem>
                            <asp:ListItem Value="55">财付通帐户管理</asp:ListItem>
                            <asp:ListItem Value="56">购买公司业务</asp:ListItem>
						</asp:dropdownlist></TD>
				</TR>
                </TABLE>
			<TABLE id="Table1" style="Z-INDEX: 102; LEFT: 16px; WIDTH: 1040px" cellSpacing="1" cellPadding="1"
				width="1040" align="center" border="1" runat="server">
				<TR>
					<TD><asp:datagrid id="DataGrid1" runat="server" Width="100%" EnableViewState="False" PageSize="200"
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
                                <asp:BoundColumn DataField="FTitle" HeaderText="主题"></asp:BoundColumn>
								<asp:BoundColumn DataField="FissueTime" HeaderText="开始时间" DataFormatString="{0:D}"></asp:BoundColumn>
								<asp:BoundColumn DataField="FLastTime" HeaderText="结束时间"></asp:BoundColumn>
								<asp:TemplateColumn HeaderText="查看">
									<ItemTemplate>
										<asp:HyperLink id=hlURL runat="server" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.FUrl").ToString().Trim() %>' Target="_blank">查看</asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
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
		</form>
	</body>
</HTML>

