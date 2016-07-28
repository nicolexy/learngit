<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="NameAbnormal.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.NameAbnormal" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <HEAD>
		<title>NameAbnormal</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="VBScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
	</style>
		<script src="../SCRIPTS/Local.js"></script>
        <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form style="FONT-FAMILY: 宋体" id="Form1" method="post" runat="server">
			<table style="POSITION: absolute;TOP: 5%; LEFT: 5%" id="Table1" border="1" cellSpacing="1"
				cellPadding="1" width="800">
				<TBODY>
					<tr style="BACKGROUND-COLOR: #e4e5f7">
						<td colSpan="3"><FONT color="red"><IMG src="../IMAGES/Page/post.gif" width="20" height="16">&nbsp;&nbsp;姓名异常查询</FONT>
						</td>
					</tr>
					<tr>
						<td width="80"><label>用户账号:</label>
						</td>
						<td><asp:textbox id="tbx_acc" Width="250px" Runat="server"></asp:textbox></td>
						<td><asp:button id="btn_query" Width="100px" Runat="server" Text="查询" onclick="btn_query_Click"></asp:button></td>
					</tr>
					<tr>
						<td colSpan="3"><asp:datagrid id="DataGrid_QueryResult" runat="server" Width="800px" ItemStyle-HorizontalAlign="Center"
								HeaderStyle-HorizontalAlign="Center" HorizontalAlign="Center" PageSize="5" AutoGenerateColumns="False"
								GridLines="Horizontal" CellPadding="1" BackColor="White" BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF"
                                OnItemDataBound="dg_ItemDataBound" >
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="cname" HeaderText="实名认证姓名" FooterStyle-HorizontalAlign="Center">
										<HeaderStyle Width="200px"></HeaderStyle>
									</asp:BoundColumn>
                                    <asp:BoundColumn DataField="cre_id_tail" HeaderText="实名认证证件号" FooterStyle-HorizontalAlign="Center">
										<HeaderStyle Width="200px"></HeaderStyle>
									</asp:BoundColumn>
                                     <asp:TemplateColumn>
									    <ItemTemplate>
										    <asp:Button id="CreateButton" Visible="false" runat="server" CommandName="CREATE" Text="创建工单"></asp:Button>
									    </ItemTemplate>
								   </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="审批/查看">
							            <ItemTemplate>
								             <asp:LinkButton id="QueryButton" href = '<%# DataBinder.Eval(Container, "DataItem.URL")%>' target=_blank Visible="true" runat="server" Text="详情"></asp:LinkButton>
							            </ItemTemplate>
					              </asp:TemplateColumn>
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

            <TABLE id="tableCreate" visible="false" runat="server" style="LEFT: 5%; POSITION:relative;top:200px;" cellSpacing="1" cellPadding="1" width="800"
				border="1" frame="box">
                 <tr>
                    <td style="width: 100%" bgcolor="#e4e5f7" colspan="2">
                        <font color="red">
                            <img src="../IMAGES/Page/post.gif" width="20" height="16">实名认证异常登记</font>
                    </td>
                </tr>
                 <TR>
                    <TD align="right"><asp:label id="Label3" runat="server">原姓名：</asp:label></TD>
                     <TD><asp:label id="tb_nameOld" runat="server"></asp:label></TD>
                  </TR>
                 <TR>
                     <TD align="right"><asp:label id="Label5" runat="server">原证件号：</asp:label></TD>
                     <TD><asp:label id="tb_certifyNoOld" runat="server"></asp:label></TD>
				</TR>
                 <TR>
                    <TD align="right"><asp:label id="Label7" runat="server">新姓名：</asp:label></TD>
                     <TD><asp:textbox id="tb_name" runat="server"></asp:textbox>&nbsp;<SPAN class="style5"><Font color="red">*</Font></SPAN></TD>
                  </TR>
                 <TR>
                     <TD align="right"><asp:label id="Label9" runat="server">新证件号：</asp:label></TD>
                     <TD><asp:textbox id="tb_certifyNo" runat="server"></asp:textbox>&nbsp;<SPAN class="style5"><Font color="red">*</Font></SPAN></TD>
				</TR>
                 <TR>
                    <TD align="right"><asp:label id="Label11" runat="server">证件类型：</asp:label></TD>
                     <TD><asp:label id="tb_cer_type" runat="server">身份证</asp:label></TD>
                  </TR>
                 <TR>
                     <TD align="right"><asp:label id="Label13" runat="server">身份证版本：</asp:label></TD>
                     <TD><asp:dropdownlist id="ddlCreVis" runat="server" Width="152px">
							<asp:ListItem Value="1">一代</asp:ListItem>
							<asp:ListItem Value="2">二代</asp:ListItem>
							<asp:ListItem Value="3">临时</asp:ListItem>
						</asp:dropdownlist>&nbsp;<SPAN class="style5"><Font color="red">*</Font></SPAN></TD>
				</TR>
                  <TR>
                    <TD align="right"><asp:label id="Label15" runat="server">身份证有效期至：</asp:label></TD>
                   <td>
                       <asp:textbox id="tbx_cerDate" Width="120" Runat="server"  onclick="WdatePicker()" CssClass="Wdate"></asp:textbox>&nbsp;<SPAN class="style5"><Font color="red">*</Font></SPAN></td>
                  </TR>
                 <TR>
                     <TD align="right"><asp:label id="Label17" runat="server">常用联系地址：</asp:label></TD>
                     <TD><asp:textbox id="tb_address" runat="server"></asp:textbox></TD>
				</TR>
               <TR>
                    <TD align="right"><asp:label id="Label23" runat="server"><font color="red">身份证正面：</font></asp:label></TD>
					   <td style="HEIGHT: 23px" colSpan="2">&nbsp;<FONT face="宋体"> </FONT><INPUT id="imageF" style="WIDTH: 241px; HEIGHT: 21px" type="file" size="21" name="imageF"
							runat="server">&nbsp;<SPAN class="style5"><Font color="red">*</Font></SPAN>
					</td>
                </TR>
                <TR>
                     <TD align="right"><asp:label id="Label1" runat="server"><font color="red">身份证反面：</font></asp:label></TD>
					   <td style="HEIGHT: 23px" colSpan="2">&nbsp;<FONT face="宋体"> </FONT><INPUT id="imageR" style="WIDTH: 241px; HEIGHT: 21px" type="file" size="21" name="imageR"
							runat="server">&nbsp;<SPAN class="style5"><Font color="red">*</Font></SPAN>
					</td>
			 </TR>
             <TR>
                    <TD align="right"><asp:label id="Label2" runat="server"><font color="red">改名凭证：</font></asp:label></TD>
					   <td style="HEIGHT: 23px" colSpan="2">&nbsp;<FONT face="宋体"> </FONT><INPUT id="imageO" style="WIDTH: 241px; HEIGHT: 21px" type="file" size="21" name="imageO"
							runat="server">&nbsp;<SPAN class="style5"><Font color="red">*</Font></SPAN>
					</td>
			</TR>
             <TR>
                   <TD align="center" colspan="4"><asp:button id="ButtonSubmit" runat="server" Width="200px" Text="确认无误后，提交申请" onclick="btnSubmitApply_Click"></asp:button></TD>
			</TR>
			</TABLE>
		</form>
	</body>
</HTML>
