<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="QueryAuthenStateInfoPage.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.NewQueryInfoPages.QueryAuthenStateInfoPage" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <HEAD>
		<title>QueryAuthenStateInfoPage</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="VBScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
	</style>
		<script src="../SCRIPTS/Local.js"></script>
		<script language="javascript">
					function openModeDate()
					{
					    var returnValue = window.showModalDialog("../Control/CalendarForm2.aspx", Form1.tbx_cerDate.value, 'dialogWidth:375px;DialogHeight=260px;status:no');
					    if (returnValue != null) Form1.tbx_cerDate.value = returnValue;
					}
		</script>
</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form style="FONT-FAMILY: 宋体" id="Form1" method="post" runat="server">
			<table  runat="server" style="LEFT: 5%; POSITION:relative;top:30px;" id="Table1" border="1" cellSpacing="1"
				cellPadding="1" width="800">
				<TBODY>
					<tr style="BACKGROUND-COLOR: #e4e5f7">
						<td colSpan="3"><FONT color="red"><IMG src="../IMAGES/Page/post.gif" width="20" height="16">&nbsp;&nbsp;证件状态查询</FONT>
						</td>
					</tr>
					<tr>
						<td width="150"><label>证件类型：身份证</label>&nbsp;&nbsp;&nbsp;<label>证件号:</label>
						</td>
						<td><asp:textbox id="tb_acc" Width="250px" Runat="server"></asp:textbox></td>
						<td><asp:button id="btn_query" Width="100px" Runat="server" Text="查询" onclick="btn_query_Click"></asp:button></td>
					</tr>
					<tr>
						<td colSpan="3"><asp:datagrid id="DataGrid_QueryResult" runat="server" Width="800px" ItemStyle-HorizontalAlign="Center"
								HeaderStyle-HorizontalAlign="Center" HorizontalAlign="Center" PageSize="5" AutoGenerateColumns="False"
								GridLines="Horizontal" CellPadding="1" BackColor="White" BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF">
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<Columns>
                                    <asp:BoundColumn DataField="Fimage_cre1" Visible="false" FooterStyle-HorizontalAlign="Center">
									</asp:BoundColumn>
                                      <asp:BoundColumn DataField="Fimage_cre2" Visible="false"  FooterStyle-HorizontalAlign="Center">
									</asp:BoundColumn>
                                      <asp:BoundColumn DataField="Fimage_evidence" Visible="false" FooterStyle-HorizontalAlign="Center">
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fcre_id" HeaderText="身份证号" FooterStyle-HorizontalAlign="Center">
										<HeaderStyle Width="200px"></HeaderStyle>
									</asp:BoundColumn>
                                    <asp:BoundColumn DataField="Fname_old" HeaderText="旧姓名" FooterStyle-HorizontalAlign="Center">
										<HeaderStyle Width="200px"></HeaderStyle>
									</asp:BoundColumn>
                                     <asp:BoundColumn DataField="Fuid" HeaderText="账号" FooterStyle-HorizontalAlign="Center">
										<HeaderStyle Width="200px"></HeaderStyle>
									</asp:BoundColumn>
                                     <asp:BoundColumn DataField="Fsubmit_user" HeaderText="操作人" FooterStyle-HorizontalAlign="Center">
										<HeaderStyle Width="200px"></HeaderStyle>
									</asp:BoundColumn>
                                     <asp:BoundColumn DataField="Fsubmit_time" HeaderText="操作时间" FooterStyle-HorizontalAlign="Center">
										<HeaderStyle Width="200px"></HeaderStyle>
									</asp:BoundColumn>
                                    <asp:ButtonColumn Text="查看" CommandName="Select">
										<HeaderStyle Width="30px"></HeaderStyle>
									</asp:ButtonColumn>
								</Columns>
								<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
							</asp:datagrid></td>
					</tr>
					<%--<tr height="25">
						<td colspan="4"><webdiyer:aspnetpager id="pager" runat="server" HorizontalAlign="right" AlwaysShow="True" ShowCustomInfoSection="left"
								NumericButtonTextFormatString="[{0}]" SubmitButtonText="转到" CssClass="mypager" ShowInputBox="always"
								PagingButtonSpacing="0" NumericButtonCount="10"></webdiyer:aspnetpager>
						</td>
					</tr>--%>
				</TBODY>
			</table>

            <TABLE id="tableCreate" visible="true" runat="server" style="LEFT: 5%; POSITION:relative;top:40px;" cellSpacing="1" cellPadding="1" width="800"
				border="1" frame="box">
                 <tr>
                    <td style="width: 100%" bgcolor="#e4e5f7" colspan="2">
                        <font color="red">
                            <img src="../IMAGES/Page/post.gif" width="20" height="16">实名认证置失效</font>
                    </td>
                </tr>
                 <TR>
                    <TD align="left" colspan="2"><asp:label id="Label3" runat="server"><Font color="red">原实名认证信息：</Font></asp:label></TD>
                  </TR>
                 <TR>
                     <TD align="right" width="100"><asp:label id="Label5" runat="server">证件号：</asp:label></TD>
                     <TD><asp:label id="tb_cre_id" runat="server"></asp:label></TD>
				</TR>
                 <TR>
                     <TD align="right" width="100"><asp:label id="Label4" runat="server">实名认证uid：</asp:label></TD>
                     <TD><asp:label id="tb_uid" runat="server"></asp:label></TD>
				</TR>
                 <TR>
                     <TD align="right" width="100"><asp:label id="Label7" runat="server">实名认证姓名：</asp:label></TD>
                      <TD><asp:label id="tb_name_old" runat="server"></asp:label></TD>
				</TR>
                  <TR>
                    <TD align="left" colspan="2"><asp:label id="Label10" runat="server"><font color="red">置失效凭证：</font></asp:label></TD>
                  </TR>
               <TR>
                    <TD align="right" width="100"><asp:label id="Label23" runat="server">身份证正面：</asp:label></TD>
					   <td style="HEIGHT: 23px">&nbsp;<FONT face="宋体"> </FONT><INPUT id="imageF" style="WIDTH: 241px; HEIGHT: 21px" type="file" size="21" name="imageF"
							runat="server">&nbsp;<SPAN class="style5"><Font color="red">*</Font></SPAN>
					</td>
                </TR>
                <TR>
                     <TD align="right" width="100"><asp:label id="Label1" runat="server">身份证反面：</asp:label></TD>
					   <td style="HEIGHT: 23px">&nbsp;<FONT face="宋体"> </FONT><INPUT id="imageR" style="WIDTH: 241px; HEIGHT: 21px" type="file" size="21" name="imageR"
							runat="server">&nbsp;<SPAN class="style5"><Font color="red">*</Font></SPAN>
					</td>
			 </TR>
             <TR>
                    <TD align="right" width="100"><asp:label id="Label2" runat="server">改名凭证：</asp:label></TD>
					   <td style="HEIGHT: 23px">&nbsp;<FONT face="宋体"> </FONT><INPUT id="imageO" style="WIDTH: 241px; HEIGHT: 21px" type="file" size="21" name="imageO"
							runat="server">&nbsp;<SPAN class="style5"><Font color="red">*</Font></SPAN>
					</td>
			</TR>
             <TR>
                   <TD align="center" colspan="2"><asp:button id="ButtonSubmit" runat="server" Width="100px" Text="确认" onclick="btnSubmit_Click"></asp:button></TD>
			</TR>
			</TABLE>
            <TABLE id="tableDetail"  visible="false" runat="server" style="LEFT: 5%; POSITION:relative;top:50px;" cellSpacing="1" cellPadding="1" width="800"
				border="1" frame="box">
                 <tr>
                    <td style="width: 100%" bgcolor="#e4e5f7" colspan="4" align="center">
                        <font size="4">
                           详情表</font>
                    </td>
                </tr>
                 <TR>
                    <TD align="right" width="100"><asp:label id="Label6" runat="server">证件号：</asp:label></TD>
                     <TD width="100"><asp:label id="lb_cre_id" runat="server"></asp:label></TD>
                     <TD align="right" width="100"><asp:label id="Label8" runat="server">旧姓名：</asp:label></TD>
                     <TD width="100"><asp:label id="lb_name_old" runat="server"></asp:label></TD>
				</TR>
                 <TR>
                    <TD align="right" width="100"><asp:label id="Label9" runat="server">uid：</asp:label></TD>
                     <TD colspan="3"><asp:label id="lb_uin" runat="server"></asp:label></TD>
				</TR>
               <TR>
                    <TD align="left" colspan="4"><asp:label id="Label17" runat="server"><font color="red">身份证正面：</font></asp:label></TD>
				</TR>
               <TR> 
                     <td colspan="4" style="PADDING-BOTTOM: 3px; PADDING-LEFT: 3px; PADDING-RIGHT: 3px; PADDING-TOP: 3px"
									height="20" align="center"><asp:image id="Image1" runat="server"></asp:image>
                    </td>
                </TR>
                <TR>
                     <TD align="left" colspan="4"><asp:label id="Label18" runat="server"><font color="red">身份证反面：</font></asp:label></TD>
						</TR>
               <TR> 
                     <td colspan="4" style="PADDING-BOTTOM: 3px; PADDING-LEFT: 3px; PADDING-RIGHT: 3px; PADDING-TOP: 3px"
									height="20" align="center"><asp:image id="Image2" runat="server"></asp:image>
                    </td>
			 </TR>
             <TR>
                    <TD align="left" colspan="4"><asp:label id="Label19" runat="server"><font color="red">改名凭证：</font></asp:label></TD>
				 </TR>
                <TR>	 
                 <td colspan="4" style="PADDING-BOTTOM: 3px; PADDING-LEFT: 3px; PADDING-RIGHT: 3px; PADDING-TOP: 3px"
									height="20" align="center"><asp:image id="Image3" runat="server"></asp:image>
                    </td>
			</TR>
             <TR>
                   <TD align="center" colspan="4">
                       <asp:button id="ButtonOK" runat="server" Width="200px" Text="提交" onclick="btnOK_Click"></asp:button>
                   </TD>
			</TR>
			</TABLE>
		</form>
	</body>
</HTML>
