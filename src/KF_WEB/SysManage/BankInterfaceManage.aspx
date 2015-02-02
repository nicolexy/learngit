<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<%@ Page language="c#" Codebehind="BankInterfaceManage.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.SysManage.BankInterfaceManage" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>BankInterfaceManage</title>
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
					<TD colSpan="4"><IMG height="16" src="../IMAGES/Page/post.gif" width="15">&nbsp;<asp:label id="lbTitle" runat="server" ForeColor="Red">银行接口维护管理</asp:label>
						&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
						</FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" Width="73px" ForeColor="Red"></asp:label></SPAN>
					</TD>
				</TR>
				<TR>
					<TD><asp:dropdownlist id="ddlSysList" runat="server" AutoPostBack="True">
                            <asp:ListItem Value="1" Selected="True">提现银行接口</asp:ListItem>
                            <asp:ListItem Value="2">向银行卡付款接口</asp:ListItem>
                            <asp:ListItem Value="3">还房贷银行接口</asp:ListItem>
                            <asp:ListItem Value="4">信用卡还款银行接口</asp:ListItem>
                            <asp:ListItem Value="5">代扣银行接口</asp:ListItem>
                            <asp:ListItem Value="6">银行收款接口</asp:ListItem>
                           <%-- <asp:ListItem Value="7">实时提现接口</asp:ListItem>--%>
                            <asp:ListItem Value="8">退款接口</asp:ListItem>
						</asp:dropdownlist></TD>
                        	<TD style="WIDTH: 290px"><asp:label id="labQueryName" runat="server">银行类型编码</asp:label><asp:textbox id="txtBankType" runat="server" Width="130px"></asp:textbox>
						    </TD>
                           <TD><asp:button id="btnQuery" runat="server" Text="查询记录" onclick="btnQuery_Click"></asp:button>&nbsp;&nbsp;&nbsp;
                        <asp:button id="btadd" runat="server" Width="80px" Text="新增" onclick="btadd_Click"></asp:button></TD>
				</TR>
                </TABLE>
            <TABLE id="Table3" style="Z-INDEX: 103; LEFT: 16px; WIDTH: 1005px" cellSpacing="1" cellPadding="1"
				width="1005" align="center" border="1" runat="server">
				<TR>
					<TD><FONT face="宋体">
							<asp:datagrid id="Datagrid2" runat="server" Width="1032px"  PageSize="200"
								AutoGenerateColumns="False" CellPadding="3" BackColor="White" BorderWidth="1px" BorderStyle="None"
								BorderColor="#E7E7FF" GridLines="Horizontal">
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<Columns>
									<asp:BoundColumn DataField="bulletin_id" HeaderText="序号" Visible="false"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="banktype" HeaderText="银行编码"></asp:BoundColumn>
									<asp:BoundColumn DataField="FBank_TypeName" HeaderText="银行类型"></asp:BoundColumn>
									<asp:BoundColumn DataField="title" HeaderText="公告标题" Visible="false"></asp:BoundColumn>
									<asp:BoundColumn DataField="startime" HeaderText="开始时间"></asp:BoundColumn>
									<asp:BoundColumn DataField="endtime" HeaderText="结束时间"></asp:BoundColumn>
									<asp:BoundColumn DataField="createuser" HeaderText="创建人"></asp:BoundColumn>
									<asp:BoundColumn DataField="createtime" HeaderText="创建时间"></asp:BoundColumn>
									<asp:BoundColumn DataField="updateuser" HeaderText="修改人"></asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="updatetime" HeaderText="修改时间"></asp:BoundColumn>
									<asp:TemplateColumn HeaderText="操作">
										<ItemTemplate>
											<A href='<%# String.Format("BankInterfaceManage_Detail.aspx?sysid={0}&bulletinId={1}&opertype=2",DataBinder.Eval(Container, "DataItem.Fsysid").ToString(), DataBinder.Eval(Container, "DataItem.bulletin_id").ToString()) %>'>
												编辑/查看 </A>
										</ItemTemplate>
									</asp:TemplateColumn>
								</Columns>
								<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
							</asp:datagrid></FONT></TD>
				</TR>
                 <TR height="25">
					<TD><webdiyer:aspnetpager id="pager" runat="server" AlwaysShow="True" NumericButtonCount="5" ShowCustomInfoSection="left"
							PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager" HorizontalAlign="right" PageSize="8"
							SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]"></webdiyer:aspnetpager></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
