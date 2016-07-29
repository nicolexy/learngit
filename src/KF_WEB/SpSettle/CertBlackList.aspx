<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CertBlackList.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.SpSettle.CertBlackList" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title></title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		    <script src="../Scripts/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form2" method="post" runat="server">
			<TABLE cellSpacing="1" cellPadding="1" width="900" style="margin-left:10px"
				border="1">
				<TR style="" bgColor="#e4e5f7">
                    <td style="" bgcolor="#e4e5f7" colspan="4">
                        <div style="display: inline; width: 50%">
                            <font color="red">
                                <img height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;新活动添加</font>
                        </div>
                        <div style="display: inline; width: 50%; text-align: right">
                            操作员代码: <span class="style3">
                                <asp:Label ID="Label2" runat="server" ForeColor="Red" Width="73px"></asp:Label></span>
                        </div>
                    </td>
                </TR>
                <TR>
                    <TD ><asp:label id="Label3" runat="server">商户号：</asp:label>
                        <asp:textbox id="txtSpid" style="WIDTH: 120px;" runat="server"></asp:textbox>
                    </TD>
					<td><asp:label id="Label4" runat="server">开始日期：</asp:label>
                        <asp:textbox id="txtStartDate" runat="server" onClick="WdatePicker()"></asp:textbox>
					</td>
                    <td> <asp:label id="Label5" runat="server">结束日期：</asp:label>
                        <asp:textbox id="txtEndDate"  runat="server" onClick="WdatePicker()"></asp:textbox>

                    </td>
                     <TD align="center" >
                        <asp:button id="btnAdd" runat="server" Width="80px" Text="新 增" OnClick="btnAdd_Click" ></asp:button>&nbsp;
                        <asp:button id="btnQuery" runat="server" Width="80px" Text="查 询" OnClick="btnQuery_Click" ></asp:button>
                    </TD>
				</TR>
				</TABLE>
            <TABLE id="Table2" cellSpacing="1" cellPadding="1" width="900" border="1" runat="server" style="margin-left:10px">
				<TR>
					<TD vAlign="top"><asp:datagrid id="DataGrid1" runat="server" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px"
							BackColor="White" CellPadding="3" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
                                <asp:BoundColumn DataField="Fid" HeaderText="FID" Visible="false"></asp:BoundColumn>
								<asp:BoundColumn DataField="FSpid" HeaderText="商户号"></asp:BoundColumn>
								<asp:BoundColumn DataField="FcompanyName" HeaderText="商户名称"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fcreate_time" HeaderText="录入时间"></asp:BoundColumn>
                                 <asp:BoundColumn DataField="Fmemo" HeaderText="录入原因"></asp:BoundColumn>
                                <asp:TemplateColumn HeaderText="操作">
                                    <ItemTemplate>
                                        <asp:LinkButton id="lbDel" runat="server" CommandName="DEL" OnClientClick="if(!confirm('确定删除？')) return false;">删除</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
							</Columns>
                            <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
                <TR>
				<TD><webdiyer:aspnetpager id="pager" runat="server" AlwaysShow="True" NumericButtonCount="10" ShowCustomInfoSection="left"
							PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager" HorizontalAlign="right" OnPageChanged="ChangePage"
							SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]"></webdiyer:aspnetpager>

				</TD>
				</TR>
			</TABLE>
            <br /><br />
            <table width="500px" runat="server" ID="TbEdit" visible="false" cellpadding="1" cellspacing="1" border="1" style="margin-left:10px">
                <tr>
                    <td style="text-align:right">商户号:&nbsp;&nbsp; </td>
                    <td>
                        <asp:TextBox ID="txtSpid_add" runat="server" style="WIDTH: 120px;"></asp:TextBox>
                    </td>
                </tr>
                 <tr>
                    <td  style="text-align:right">录入原因:&nbsp;&nbsp; </td>
                     <td><asp:TextBox ID="txtFMemo" runat="server" style="WIDTH: 200px; height:60px" TextMode="MultiLine"></asp:TextBox></td>
                </tr>
                  <tr>
                    <td>&nbsp;</td>
                     <td>
                         <asp:button id="btnSubmit" runat="server" Width="80px" Text="提 交" OnClick="btnSubmit_Click" ></asp:button>&nbsp;&nbsp;
                         <asp:button id="btnBack" runat="server" Width="80px" Text="返 回" OnClick="btnBack_Click" ></asp:button>
                     </td>
                </tr>
            </table>
		</form>
	</body>
</HTML>