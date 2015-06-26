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
        <script src="../Scripts/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
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
					<TD colSpan="5"><IMG height="16" src="../IMAGES/Page/post.gif" width="15">&nbsp;<asp:label id="lbTitle" runat="server" ForeColor="Red">银行接口维护管理</asp:label>
						&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
						</FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" Width="73px" ForeColor="Red"></asp:label></SPAN>
					</TD>
				</TR>
				<TR>
					<TD><asp:dropdownlist id="ddlSysList" runat="server" AutoPostBack="True">
						</asp:dropdownlist>
					</TD>
                     <td>
                        开始日期：
                        <asp:TextBox ID="textBoxBeginDate" runat="server" Width="130px" onClick="WdatePicker()"  CssClass="Wdate"></asp:TextBox>
                        结束日期：
                        <asp:TextBox ID="textBoxEndDate" runat="server" Width="130px" onClick="WdatePicker()"  CssClass="Wdate"></asp:TextBox>
                      </TD>   
                    <TD><asp:label id="Label8" runat="server">公告类型</asp:label>
                        <asp:dropdownlist id="ddlBullType" runat="server" AutoPostBack="True">
						</asp:dropdownlist>
					</TD>
                    <TD><asp:label id="Label2" runat="server">状态</asp:label>
                        <asp:dropdownlist id="ddlBullState" runat="server" AutoPostBack="True">
						</asp:dropdownlist>
					</TD>
                    <TD>
                        <asp:button id="btnQuery" runat="server" Text="查询" onclick="btnQuery_Click"></asp:button>&nbsp;&nbsp;&nbsp;
                    </TD>
				</TR>
                <TR>
					<TD align="right" width="15%"><FONT face="宋体">银行类型编码 </FONT>
					</TD>
					<TD align="left" colSpan="4"><asp:textbox id="txtBankType" runat="server" Width="500px" Height="80px" TextMode="MultiLine"></asp:textbox>
                        <font color="red">新增可输入多个银行编码，请按分号“;”分割填写，例如：1001;1002</font></TD>
				</TR>
                 <TR>
					<TD align="right" colspan="5">
                        <asp:button id="btadd" runat="server" Width="80px" Text="新增" onclick="btadd_Click"></asp:button>
                        <asp:button id="Button1" runat="server" Width="80px" Text="开启" onclick="btOpen_Click"></asp:button>
                         <asp:button id="Button2" runat="server" Width="80px" Text="查询当前" onclick="btCurrent_Click"></asp:button>
                          <asp:button id="Button3" runat="server" Width="80px" Text="查询历史" onclick="btHistory_Click"></asp:button>
					</TD>
				</TR>
                </TABLE>
            <TABLE id="Table3" style="Z-INDEX: 103; LEFT: 16px; WIDTH: 1005px" cellSpacing="1" cellPadding="1"
				width="1005" align="center" border="1" runat="server">
				<TR>
					<TD><FONT face="宋体">
							<asp:datagrid id="Datagrid2" runat="server" Width="1032px"  PageSize="200"
								AutoGenerateColumns="False" CellPadding="3" BackColor="White" BorderWidth="1px" BorderStyle="None"
								BorderColor="#E7E7FF" GridLines="Horizontal" OnItemDataBound="dg_ItemDataBound">
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<Columns>
									<asp:BoundColumn DataField="bulletin_id" HeaderText="公告id" Visible="false"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="bull_type" Visible="false"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="bull_state" Visible="false"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="banktype" HeaderText="银行编码"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="banktype_str" HeaderText="银行类型"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="closetype_str" HeaderText="关闭方式"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="op_support_flag_str" HeaderText="关闭功能"></asp:BoundColumn>
									<asp:BoundColumn DataField="title" HeaderText="公告标题" Visible="false"></asp:BoundColumn>
									<asp:BoundColumn DataField="startime" HeaderText="开始时间"></asp:BoundColumn>
									<asp:BoundColumn DataField="endtime" HeaderText="结束时间"></asp:BoundColumn>
									<asp:BoundColumn DataField="createuser" HeaderText="创建人"></asp:BoundColumn>
									<asp:BoundColumn DataField="updatetime" HeaderText="更新时间"></asp:BoundColumn>
									<asp:BoundColumn DataField="updateuser" HeaderText="修改人"></asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="updatetime" HeaderText="修改时间"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="bull_type_str" HeaderText="公告类型"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="bull_state_str" HeaderText="状态"></asp:BoundColumn>
									<asp:TemplateColumn HeaderText="操作">
										<ItemTemplate>
											<%--<A href='<%# String.Format("BankInterfaceManage_Detail.aspx?sysid={0}&bulletinId={1}&opertype=2",DataBinder.Eval(Container, "DataItem.sysid").ToString(), DataBinder.Eval(Container, "DataItem.bulletin_id").ToString()) %>'>
												编辑/查看 </A>--%>
                                            <asp:LinkButton id="btupdate" href = '<%# DataBinder.Eval(Container, "DataItem.URLUpdate")%>' target=_blank runat="server" Text="编辑"></asp:LinkButton>
                                             <asp:LinkButton id="btquery" href = '<%# DataBinder.Eval(Container, "DataItem.URLQuery")%>' target=_blank runat="server" Text="查看"></asp:LinkButton>
										</ItemTemplate>
									</asp:TemplateColumn>
                                  <asp:TemplateColumn>
                                      <HeaderTemplate>
                                         <asp:CheckBox id="CheckBoxAll" runat="server" OnCheckedChanged = "OnCheckBox_CheckedSelect"  AutoPostBack = "true" ></asp:CheckBox>
                                          <asp:Label ID="selTxt" runat="server">全选</asp:Label>
                                      </HeaderTemplate>
									<ItemTemplate>
										<asp:CheckBox id="CheckBox2" runat="server"></asp:CheckBox>
									</ItemTemplate>
								</asp:TemplateColumn>
								</Columns>
								<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
							</asp:datagrid></FONT></TD>
				</TR>
                 <TR height="25">
					<TD><webdiyer:aspnetpager id="pager" runat="server" AlwaysShow="True" NumericButtonCount="5" ShowCustomInfoSection="left"
							PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager" HorizontalAlign="right" PageSize="40"
							SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]"></webdiyer:aspnetpager></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
