<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page language="c#" Codebehind="BankCardUnbindNew.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.BankCardUnbindNew" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>BankCardUnbind</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		<script language="javascript">
		    function openModeBegin() {
		        var returnValue = window.showModalDialog("../Control/CalendarForm2.aspx", Form1.tbx_beginDate.value, 'dialogWidth:375px;DialogHeight=260px;status:no');
		        if (returnValue != null) Form1.tbx_beginDate.value = returnValue;
		    }

		    function openModeEnd() {
		        var returnValue = window.showModalDialog("../Control/CalendarForm2.aspx", Form1.tbx_endDate.value, 'dialogWidth:375px;DialogHeight=260px;status:no');
		        if (returnValue != null) Form1.tbx_endDate.value = returnValue;
		    }
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE style="margin-top:5px; margin-left:5px" id="Table1" border="1" cellSpacing="1"
				cellPadding="1" width="1000">
				<TR>
					<TD style="WIDTH: 1000px" bgColor="#e4e5f7" colSpan="4"><FONT face="宋体"><FONT color="red"><IMG src="../IMAGES/Page/post.gif" width="20" height="16">&nbsp;&nbsp;一点通业务</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>操作员代码: </FONT><asp:label id="Label1" runat="server" Width="73px" ForeColor="Red"></asp:label></TD>
				</TR>
                </table>
			<asp:panel id="PanelList" runat="server">
            <TABLE style="margin-top:5px; margin-left:5px" id="Table2" border="1" cellSpacing="1"
				cellPadding="1" width="1000">
					<TR>
						<TD width="150" align="right">查询类型:</TD>
						<TD>
							<P>
								<asp:radiobutton id="rbtn_all" AutoPostBack="True" Text="全部" Runat="server" Checked="false" GroupName="rbtnQueryType"></asp:radiobutton>
								<asp:radiobutton id="rbtn_ydt" AutoPostBack="True" Text="一点通" Runat="server" Checked="false" GroupName="rbtnQueryType"></asp:radiobutton>
								<asp:radiobutton id="rbtn_fastPay" AutoPostBack="True" Text="快捷支付" Runat="server" Checked="false"
									GroupName="rbtnQueryType"></asp:radiobutton></P>
						</TD>
						<TD width="150" colSpan="2" align="left">
							<%--<asp:checkbox id="cbx_showAbout" Text="显示查询条件相关结果" Runat="server" Checked="True"></asp:checkbox></TD>--%>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label2" runat="server">财付通账号</asp:label></TD>
						<TD width="250">
							<asp:textbox id="txtQQ" runat="server"></asp:textbox></TD>
						<TD width="150" align="right">
							<asp:label id="Label3" runat="server">内部ID</asp:label></TD>
						<TD width="250">
							<asp:textbox id="tbx_uid" runat="server" Width="200"></asp:textbox></TD>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label17" runat="server">银行类型</asp:label></TD>
						<TD width="450">
							<asp:DropDownList id="ddl_BankType" runat="server">
								<asp:ListItem Value="" Selected="True">全部</asp:ListItem>
							</asp:DropDownList>
							<asp:RadioButton id="rbtn_bkt_XYK" AutoPostBack="true" Text="信用卡" Runat="server" GroupName="bkt"></asp:RadioButton>
							<asp:RadioButton id="rbtn_bkt_JJK" AutoPostBack="true" Text="借记卡" Runat="server" GroupName="bkt"></asp:RadioButton>
							<asp:RadioButton id="rbtn_bkt_ALL" AutoPostBack="true" Text="全部" Runat="server" GroupName="bkt"></asp:RadioButton></TD>
						<TD width="150" align="right">
							<asp:label id="Label18" runat="server">银行卡号</asp:label></TD>
						<TD width="250">
							<asp:textbox id="tbx_bankID" runat="server" Width="200"></asp:textbox></TD>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label19" runat="server">证件类型</asp:label></TD>
						<TD width="250">
							<asp:DropDownList id="ddl_creType" runat="server">
								<asp:ListItem Value="" Selected="True">全部</asp:ListItem>
								<asp:ListItem Value="1">身份证</asp:ListItem>
								<asp:ListItem Value="2">护照</asp:ListItem>
								<asp:ListItem Value="3">军官证</asp:ListItem>
							</asp:DropDownList></TD>
						<TD width="150" align="right">
							<asp:label id="Label20" runat="server">证件号</asp:label></TD>
						<TD width="250">
							<asp:textbox id="tbx_creID" runat="server" Width="200"></asp:textbox></TD>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label21" runat="server">协议号</asp:label></TD>
						<TD width="250">
							<asp:textbox id="tbx_serNum" runat="server"></asp:textbox></TD>
						<TD width="150" align="right">
							<asp:label id="Label22" runat="server">手机号码</asp:label></TD>
						<TD width="250">
							<asp:textbox id="tbx_phone" runat="server" Width="200"></asp:textbox></TD>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label23" runat="server">起始日期</asp:label></TD>
						<TD width="250">
							<asp:textbox id="tbx_beginDate" runat="server"></asp:textbox>
							<asp:imagebutton id="ButtonBeginDate" runat="server" ImageUrl="../Images/Public/edit.gif" CausesValidation="False"></asp:imagebutton></TD>
						<TD width="150" align="right">
							<asp:label id="Label24" runat="server">结束日期</asp:label></TD>
						<TD width="250">
							<asp:textbox id="tbx_endDate" runat="server"></asp:textbox>
							<asp:imagebutton id="ButtonEndDate" runat="server" ImageUrl="../Images/Public/edit.gif" CausesValidation="False"></asp:imagebutton></TD>
					</TR>
					<TR>
						<TD width="150" align="right"><LABEL id="LABEL4" runat="server">绑定状态</LABEL></TD>
						<TD width="250">
							<asp:DropDownList id="ddl_bindStatue" runat="server">
								<asp:ListItem Value="99" Selected="True">所有</asp:ListItem>
								<asp:ListItem Value="0">未定义</asp:ListItem>
								<asp:ListItem Value="1">预期绑定状态</asp:ListItem>
								<asp:ListItem Value="2">绑定确认</asp:ListItem>
								<asp:ListItem Value="3">解除绑定</asp:ListItem>
							</asp:DropDownList></TD>
						<TD colSpan="2" align="center">
							<asp:button id="btnSearch" runat="server" Width="80px" Text="查 询" onclick="btnSearch_Click"></asp:button></TD>
					</TR>
					<TR>
						<TD colSpan="4" align="center">
							<%--<asp:datagrid id="Datagrid1" runat="server" Width="100%" BorderColor="#E7E7FF" BorderStyle="None"
								BorderWidth="1px" BackColor="White" CellPadding="1" GridLines="Horizontal" AutoGenerateColumns="False"
								HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="fuin" HeaderText="财付通帐号">
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="fcre_id" HeaderText="证件号">
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fbank_typeStr" HeaderText="银行类型">
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
									</asp:BoundColumn>
                                    <asp:BoundColumn DataField="fcard_tail" HeaderText="卡号后四位">
										<HeaderStyle Wrap="False"></HeaderStyle>
									</asp:BoundColumn>
									<asp:ButtonColumn HeaderText="详细" Text="详细" ButtonType="LinkButton" CommandName="query"></asp:ButtonColumn>
								</Columns>
							</asp:datagrid>
							<webdiyer:aspnetpager id="pager1" runat="server" HorizontalAlign="right" AlwaysShow="True" ShowCustomInfoSection="left"
								NumericButtonTextFormatString="[{0}]" SubmitButtonText="转到" CssClass="mypager" ShowInputBox="always"
								PagingButtonSpacing="0" NumericButtonCount="10"></webdiyer:aspnetpager>--%></TD>
					</TR>
					<TR>
						<TD colSpan="4" align="center">
							<asp:datagrid id="dgList" runat="server" Width="100%" BorderColor="#E7E7FF" BorderStyle="None"
								BorderWidth="1px" BackColor="White" CellPadding="1" GridLines="Horizontal" AutoGenerateColumns="False"
								HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="Findex" Visible="False"></asp:BoundColumn>
                             <%--       <asp:BoundColumn DataField="FBDIndex" Visible="False"></asp:BoundColumn>--%>
									<asp:BoundColumn DataField="Fbind_serialno" HeaderText="绑定序列号">
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fuin" HeaderText="财付通账号">
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fbank_typeStr" HeaderText="银行类型">
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
									</asp:BoundColumn>
                                    <asp:BoundColumn DataField="Fxyzf_typeStr" HeaderText="微信信用卡">
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fprotocol_no" HeaderText="协议编号">
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fbank_statusStr" HeaderText="银行绑定状态">
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fcard_tail" HeaderText="银行卡后四位">
										<HeaderStyle Wrap="False"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Ftruename" HeaderText="银行卡账户名">
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
									</asp:BoundColumn>
									<asp:TemplateColumn HeaderText="详细">
										<ItemTemplate>
											<a href='./BankCardUnbindNew.aspx?type=edit&Findex=<%# DataBinder.Eval(Container, "DataItem.Findex")%>&Fuid=<%# DataBinder.Eval(Container, "DataItem.Fuid")%>'>
												详细</a>
										</ItemTemplate>
									</asp:TemplateColumn>
								</Columns>
							</asp:datagrid>
							<webdiyer:aspnetpager id="pager" runat="server" HorizontalAlign="right" AlwaysShow="True" ShowCustomInfoSection="left"
								NumericButtonTextFormatString="[{0}]" SubmitButtonText="转到" CssClass="mypager" ShowInputBox="always"
								PagingButtonSpacing="0" NumericButtonCount="10"></webdiyer:aspnetpager></TD>
					</TR>
                </table>
			</asp:panel>
           <asp:panel id="PanelMod" runat="server"  Visible="False">
            <TABLE style="margin-top:5px; margin-left:5px" id="Table3" border="1" cellSpacing="1"
				cellPadding="1" width="1000">
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label6" runat="server">财付通账号</asp:label></TD>
						<TD width="250">
							<asp:label id="lblFuin" runat="server"></asp:label></TD>
						<TD width="150" align="right">
							<asp:label id="Label8" runat="server">银行类型</asp:label></TD>
						<TD width="250">
							<asp:label id="lblFbank_type" runat="server"></asp:label></TD>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label5" runat="server">绑定序列号</asp:label></TD>
						<TD width="250">
							<asp:label id="lblFbind_serialno" runat="server"></asp:label></TD>
						<TD width="150" align="right">
							<asp:label id="Label7" runat="server">协议编号</asp:label></TD>
						<TD width="250">
							<asp:label id="lblFprotocol_no" runat="server"></asp:label></TD>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label9" runat="server">银行绑定状态</asp:label></TD>
						<TD width="250">
							<asp:label id="lblFbank_status" runat="server"></asp:label></TD>
						<TD width="150" align="right">
							<asp:label id="Label10" runat="server">银行卡后四位</asp:label></TD>
						<TD width="250">
							<asp:label id="lblFcard_tail" runat="server"></asp:label></TD>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label11" runat="server">银行卡账户名</asp:label></TD>
						<TD width="250">
							<asp:label id="lblFtruename" runat="server"></asp:label></TD>
						<TD width="150" align="right">
							<asp:label id="Label12" runat="server">关联类型</asp:label></TD>
						<TD width="250">
							<asp:label id="lblFbind_type" runat="server"></asp:label></TD>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label14" runat="server">有效标识</asp:label></TD>
						<TD width="250">
							<asp:label id="lblFbind_flag" runat="server"></asp:label></TD>
						<TD width="150" align="right">
							<asp:label id="Label15" runat="server">关联的银行卡号</asp:label></TD>
						<TD width="250">
							<asp:label id="lblFbank_id" runat="server"></asp:label></TD>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label13" runat="server">关联状态</asp:label></TD>
						<TD width="250">
							<asp:label id="lblFbind_status" runat="server"></asp:label></TD>
						<TD>
							<asp:Label id="lblFindex" Runat="server" Visible="False"></asp:Label></TD>
						<TD>
							<asp:Label id="lblFuid" Runat="server" Visible="False"></asp:Label><asp:Label id="lblFbankType" Runat="server" Visible="False"></asp:Label>
                            <asp:Label id="lblFcard_tail_db" Runat="server" Visible="False"></asp:Label>
                            </TD>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label25" runat="server">证件类型</asp:label></TD>
						<TD width="250">
							<asp:label id="lblcreType" runat="server"></asp:label></TD>
						<TD width="150" align="right">
							<asp:label id="Label16" runat="server">证件号码</asp:label></TD>
						<TD width="250">
							<asp:label id="lblCreID" runat="server"></asp:label></TD>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label26" runat="server">手机号</asp:label></TD>
						<TD width="250">
							<asp:label id="lblPhone" runat="server"></asp:label></TD>
						<TD width="150" align="right">
							<asp:label id="Label28" runat="server">内部ID</asp:label></TD>
						<TD width="250">
							<asp:label id="lblUid" runat="server"></asp:label></TD>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label27" runat="server">开户名</asp:label></TD>
						<TD width="250">
							<asp:label id="lblTrueName" runat="server"></asp:label></TD>
						<TD width="150" align="right">
							<asp:label id="Label30" runat="server">创建时间</asp:label></TD>
						<TD width="250">
							<asp:label id="lblCreateTime" runat="server"></asp:label></TD>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label32" runat="server">绑定时间（本地）</asp:label></TD>
						<TD width="250">
							<asp:label id="lblbindTimeLocal" runat="server"></asp:label></TD>
						<TD width="150" align="right">
							<asp:label id="Label34" runat="server">绑定时间（银行）</asp:label></TD>
						<TD width="250">
							<asp:label id="lblbindTimeBank" runat="server"></asp:label></TD>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label36" runat="server">解绑时间（本地）</asp:label></TD>
						<TD width="250">
							<asp:label id="lblUnbindTimeLocal" runat="server"></asp:label></TD>
						<TD width="150" align="right">
							<asp:label id="Label38" runat="server">解绑时间（银行）</asp:label></TD>
						<TD width="250">
							<asp:label id="lblUnbindTimeBank" runat="server"></asp:label></TD>
					</TR>
                    <TR>
						<TD width="150" align="right">
							<asp:label id="Label29" runat="server">单次支付限额(元)</asp:label></TD>
						<TD width="250">
							<asp:label id="lblonce_quota" runat="server"></asp:label></TD>
						<TD width="150" align="right">
							<asp:label id="Label33" runat="server">单天支付限额（元）</asp:label></TD>
						<TD width="250">
							<asp:label id="lblday_quota" runat="server"></asp:label></TD>
					</TR>
                    <TR>
						<TD width="150" align="right">
							<asp:label id="Label31" runat="server">小额名短信通知</asp:label></TD>
						<TD width="250">
							<asp:label id="lbli_character2" runat="server"></asp:label></TD>
						<TD width="150" align="right">
							<asp:label id="Label37" runat="server"></asp:label></TD>
						<TD width="250">
                            <asp:label id="Label41" runat="server"></asp:label></TD>
					</TR>
                    <asp:panel id="PanelCheckBox" runat="server" HorizontalAlign="Center" Visible="true">
                      <TR>
						<TD width="150" align="right">
							<asp:label id="Label40" runat="server">调用特殊服务解绑</asp:label></TD>
						<TD width="250" colspan="3">
                            <asp:CheckBox ID="CheckBoxUnbind" runat="server" checked="false"/></TD>
					</TR>
                    </asp:panel>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label35" runat="server">备注</asp:label></TD>
						<TD colSpan="3">
							<asp:TextBox id="txtFmemo" Width="100%" Runat="server" TextMode="MultiLine"></asp:TextBox></TD>
					</TR>
					<TR>
						<TD colSpan="4" align="center">
							<asp:Button id="btnUnbind" Text="解除绑定" Runat="server" Enabled="False" onclick="btnUnbind_Click"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
							<asp:Button id="btnCancel" Text=" 取  消 " Runat="server" onclick="btnCancel_Click"></asp:Button>&nbsp;&nbsp;
                            <asp:Button id="btnSynchron" Text=" 同  步 " Runat="server" onclick="btnSynchron_Click"></asp:Button>
                        </TD>
					</TR>
                </TABLE>
		    </asp:panel>
		</form>
	</body>
</HTML>
