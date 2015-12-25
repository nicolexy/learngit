<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="CGIInfoQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.CGIInfoQuery" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>CGIInfoQuery</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">
		    @import url( ../STYLES/ossstyle.css );

		    UNKNOWN {
		        COLOR: #000000;
		    }

		    .style3 {
		        COLOR: #ff0000;
		    }

		    BODY {
		        BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif);
		    }
		</style>
		<script src="../SCRIPTS/Local.js"></script>
        <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" cellSpacing="1" cellPadding="1" align="center" 
				 Width="80%"  border="1">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="5"><FONT face="宋体"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;证书CGI查询</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" Width="73px" ForeColor="Red"></asp:label></SPAN></TD>
				</TR>
                <TR>
                    <TD align="right"><asp:label id="Label36" runat="server">开始时间：</asp:label></TD>
				    <TD><asp:textbox id="TextBoxStartTime" runat="server" onclick="WdatePicker()" CssClass="Wdate"></asp:textbox>
                        
				    </TD>
                    <TD align="right"><asp:label id="Label32" runat="server">结束时间：</asp:label></TD>
					 <TD>
                         <asp:textbox id="TextBoxEndTime" runat="server" onclick="WdatePicker()" CssClass="Wdate"></asp:textbox>
				    </TD>
				</TR>
                	<TR>
					<TD align="right"><asp:label id="Label2" runat="server">商户号：</asp:label></TD>
					<TD colspan="3"><asp:textbox id="txtSpid" runat="server"></asp:textbox></TD>
				</TR>
                <TR>
                    <TD align="center" colspan="6"><FONT face="宋体"><asp:button id="btnSearch" runat="server" Width="80px" Text="查 询" onclick="btnSearch_Click"></asp:button></FONT></TD>
				</TR>
			</TABLE>
		<%--	<div style="LEFT: 5%; OVERFLOW: auto; WIDTH:820px; POSITION: absolute; TOP: 200px; HEIGHT: 300px">--%>
				<table cellSpacing="0" cellPadding="0" border="0" Width="80%" align="center" runat="server" >
				<tr>
						<TD vAlign="top" align="left"><asp:datagrid id="Datagrid1" runat="server" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
								HorizontalAlign="left" AutoGenerateColumns="False" GridLines="Horizontal" CellPadding="1" BackColor="White"
								BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF"  Width="100%">
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="FSpid" HeaderText="商户号">
										<HeaderStyle Width="150px" HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle Width="150px" HorizontalAlign="Center"/>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="FCertNum" HeaderText="使用证书总个数">
									    <HeaderStyle Width="100px" HorizontalAlign="Center"></HeaderStyle>
                                         <ItemStyle Width="100px"  HorizontalAlign="Center"/>
									</asp:BoundColumn>
                                    <asp:BoundColumn DataField="FCertUsed_LastTime" HeaderText="旧证书最后使用时间"> 
									    <HeaderStyle Width="100px" HorizontalAlign="Center"></HeaderStyle>
                                         <ItemStyle Width="100px"  HorizontalAlign="Center"/>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FOldCertClinkIntTotal" HeaderText="旧证书使用次数">
										<HeaderStyle Width="100px" HorizontalAlign="Center"></HeaderStyle>
                                         <ItemStyle   Width="100px" HorizontalAlign="Center"/>
									</asp:BoundColumn>
                                    <asp:BoundColumn DataField="FCertValidTimeEnd" HeaderText="旧证书到期时间">
										<HeaderStyle Width="100px"></HeaderStyle>
                                         <ItemStyle  Width="100px" HorizontalAlign="Center"/>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FMixedStr" HeaderText="证书是否全部更换">
										<HeaderStyle Width="100px"></HeaderStyle>
                                         <ItemStyle  Width="100px" HorizontalAlign="Center"/>
									</asp:BoundColumn>
								</Columns>
								<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
							</asp:datagrid></TD>
					</tr>
                    <tr runat="server" id="cgiTR">
                        <td height="80px"><font color="red" size="4">旧证书使用的cgi信息如下：</font></td>
                    </tr>
                    	<tr>
						<TD vAlign="top" align="left"><asp:datagrid id="dgList" runat="server" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
								HorizontalAlign="left" AutoGenerateColumns="False" GridLines="Horizontal" CellPadding="1" BackColor="White"
								BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF"  Width="100%">
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="FCgiName" HeaderText="cgi">
										<HeaderStyle Width="150px" HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle Width="150px" HorizontalAlign="Center"/>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="FClientHostIp" HeaderText="ip">
									    <HeaderStyle Width="100px" HorizontalAlign="Center"></HeaderStyle>
                                         <ItemStyle Width="100px"  HorizontalAlign="Center"/>
									</asp:BoundColumn>
                                    <asp:BoundColumn DataField="FCgiClkInt" HeaderText="次数"> 
									    <HeaderStyle Width="100px" HorizontalAlign="Center"></HeaderStyle>
                                         <ItemStyle Width="100px"  HorizontalAlign="Center"/>
									</asp:BoundColumn>
								</Columns>
								<PagerStyle ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
							</asp:datagrid></TD>
					</tr>
                    <TR height="25">
					<TD><webdiyer:aspnetpager id="pager" runat="server" AlwaysShow="True" NumericButtonCount="5" ShowCustomInfoSection="left"
							PagingButtonSpacing="0" ShowInputBox="always" CssClass="mypager" HorizontalAlign="right" PageSize="10"
							SubmitButtonText="转到" NumericButtonTextFormatString="[{0}]"></webdiyer:aspnetpager></TD>
				   </TR>
				</table>
		<%--	</div>--%>
		</form>
	</body>
</HTML>
