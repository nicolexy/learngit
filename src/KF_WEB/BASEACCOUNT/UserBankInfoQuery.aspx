<%@ Page language="c#" Codebehind="UserAccountQuery.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.UserAccountQuery1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>UserAccountQuery</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); .style2 { COLOR: #ff0000; FONT-WEIGHT: bold }
	.style3 { COLOR: #000000 }
	.style4 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
        <script language="javascript" src="../js/common.js"></script>
		<SCRIPT language="javascript">
		    function IsNumber(string,sign) 
		{ 
			var number; 
			if (string==null) 
				return false; 
 
			number = new Number(string); 
			if (isNaN(number)) 
			{ 
				return false; 
			} 
			else
			{ 
				return true; 
			} 
		}
			function checkvlid()
			{
				with(Form1)
				{
					if(TextBox1_QQID.value=="")
					{
						alert("用户帐户不能为空!!");
						TextBox1_QQID.focus();
						return false;
					}	
				}
				return true;
			}
			function setProvCity(dProv, dCity) {
			    var detailPanel = document.getElementById("PanelDetail");
			    if (detailPanel != undefined && detailPanel != null) {
			        if (Number(dProv) > 0) {
			            document.getElementById("spanProv").innerHTML = where[dProv].loca;
			            var cityIds = where[dProv].locacityids.split('|');
			            for (var i = 0; i < cityIds.length; i++) {
			                if (dCity == cityIds[i]) {
			                    document.getElementById("spanCity").innerHTML = where[dProv].locacity.split('|')[i];
			                }
			            }			            
			        }
			    }
			}
		</SCRIPT>
	</HEAD>
	<body onload="setProvCity('<%=dProv%>','<%=dCity%>')" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT>
			<br>
			<table cellSpacing="1" cellPadding="0" width="90%" align="center" bgColor="#666666" border="0">
				<tr bgColor="#e4e5f7">
					<td vAlign="middle" colSpan="2" height="20">
						<table height="90%" cellSpacing="0" cellPadding="1" width="97%" border="0">
							<tr>
								<td width="80%" height="18"><font color="#ff0000"><STRONG><FONT color="#ff0000">&nbsp;</FONT></STRONG><IMG height="16" src="../IMAGES/Page/post.gif" width="20">
										用户查询</font>
									<div align="right"></div>
								</td>
								<td width="20%">操作员代码: <span class="style4">
										<asp:Label id="Label_uid" runat="server">0755688</asp:Label></span></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr bgColor="#ffffff">
					<td>
						<div align="center"></div>
						<div align="left">
							<table height="100%" cellSpacing="0" cellPadding="1" width="100%" border="0">
								<tr>
									<td width="19%">&nbsp;</td>
									<td width="78%">输入：&nbsp;
										<asp:textbox id="TextBox1_QQID" runat="server" BorderWidth="1px" BorderStyle="Solid"></asp:textbox>
										&nbsp;&nbsp;
										<asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" ControlToValidate="TextBox1_QQID" Width="117px"
											ErrorMessage="RequiredFieldValidator">请输入QQ号码</asp:requiredfieldvalidator></td>
									<TD width="3%">&nbsp;</TD>
								</tr>
							</table>
						</div>
						<div align="left"></div>
						<div align="center"></div>
					</td>
					<td width="25%">
						<div align="center">&nbsp;
							<asp:button id="Button1" runat="server" Text="查 询" onclick="Button1_Click"></asp:button></div>
					</td>
				</tr>
			</table>
			<br>
			<TABLE cellSpacing="0" cellPadding="0" width="95%" align="center" border="0">
				<TR>
					<TD bgColor="#666666">
						<TABLE cellSpacing="1" cellPadding="0" width="100%" align="center" border="0">
							<TR bgColor="#e4e5f7">
								<TD background="../IMAGES/Page/bg_bl.gif" height="20"><font color="#ff0000"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;<FONT face="宋体">详细</FONT>
									</font>
								</TD>
							</TR>
							<TR>
								<TD bgColor="#ffffff" height="12">
                                    <asp:panel id="PanelList" runat="server">
										<P>
											<asp:DataGrid id="DGData" runat="server" BorderStyle="None" BorderWidth="1px" Width="100%" GridLines="Horizontal"
												CellPadding="3" BackColor="White" BorderColor="#E7E7FF" AutoGenerateColumns="False" OnItemDataBound="DGData_ItemDataBound">
												<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
												<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
												<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
												<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
												<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
												<Columns>
													<asp:BoundColumn DataField="Fbank_name" HeaderText="银行名称">
														<HeaderStyle Width="100px"></HeaderStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="Fcard_tail" HeaderText="卡尾号">
														<HeaderStyle Width="150px"></HeaderStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="FstateName" HeaderText="帐户状态">
														<HeaderStyle Width="200px"></HeaderStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="FcurtypeName" HeaderText="币种">
														<HeaderStyle Width="150px"></HeaderStyle>
													</asp:BoundColumn>
													<asp:BoundColumn DataField="Fprimary_flagName" HeaderText="是否主卡"></asp:BoundColumn>
													<asp:BoundColumn Visible="False" DataField="Farea" HeaderText="开户地区"></asp:BoundColumn>
													<asp:BoundColumn Visible="False" DataField="Fcity" HeaderText="开户城市"></asp:BoundColumn>
													<asp:BoundColumn Visible="False" DataField="Fbankid_str" HeaderText="银行帐号"></asp:BoundColumn>
													<asp:BoundColumn Visible="False" DataField="Flogin_ip" HeaderText="IP地址"></asp:BoundColumn>
													<asp:BoundColumn Visible="False" DataField="Fbank_type" HeaderText="银行类型"></asp:BoundColumn>
													<asp:BoundColumn Visible="False" DataField="Fmemo" HeaderText="备注"></asp:BoundColumn>
													<asp:BoundColumn Visible="False" DataField="Fcreate_time" HeaderText="开户日期"></asp:BoundColumn>
													<asp:BoundColumn Visible="False" DataField="Fmodify_time" HeaderText="更新时间"></asp:BoundColumn>
													
                                                    <asp:TemplateColumn HeaderText="操作">
                                                      <ItemTemplate>
                                                        <asp:LinkButton id="lbDetail" runat="server" CommandName="DETAIL">详细</asp:LinkButton>
                                                        <asp:LinkButton id="lbChange" Visible="false" runat="server" CommandName="CHANGE">解冻</asp:LinkButton>
                                                      </ItemTemplate>
                                                    </asp:TemplateColumn>
													<asp:BoundColumn Visible="False" DataField="fqqid" HeaderText="QQ号"></asp:BoundColumn>
													<asp:BoundColumn Visible="False" DataField="Ftruename" HeaderText="开户名"></asp:BoundColumn>
													<asp:BoundColumn Visible="False" DataField="Fcompany_name" HeaderText="公司名称"></asp:BoundColumn>
                                                    <asp:BoundColumn Visible="False" DataField="Fcurtype" HeaderText="币"></asp:BoundColumn>
												</Columns>
												<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
											</asp:DataGrid></P>
									</asp:panel>

                                    <asp:panel id="PanelDetail" runat="server" Height="128px" Visible="False">
										<TABLE cellSpacing="1" cellPadding="0" width="100%" align="center" border="0">
											<TR>
												<TD>
                                                    <TABLE height="116" cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
				                                        <TR>
					                                        <TD bgColor="#666666" style="HEIGHT: 136px"><TABLE height="148" cellSpacing="1" cellPadding="0" width="100%" align="center" border="0">
							                                        <TR>
								                                        <TD bgColor="#eeeeee" height="20" style="HEIGHT: 20px">&nbsp;账户状态:
								                                        </TD>
								                                        <TD width="19%" height="20" bgColor="#ffffff" style="HEIGHT: 20px">&nbsp;
									                                        <asp:Label id="Label1_State" runat="server">正常</asp:Label></TD>
								                                        <TD width="7%" bgColor="#eeeeee" style="HEIGHT: 20px">&nbsp;
									                                        <asp:LinkButton id="lkb_acc" runat="server" Visible="False" onclick="lkb_acc_Click"></asp:LinkButton></TD>
								                                        <TD bgColor="#eeeeee" height="20" style="HEIGHT: 20px"><P>&nbsp;银行帐户号码</P>
								                                        </TD>
								                                        <TD bgColor="#ffffff" height="20" style="HEIGHT: 20px">&nbsp;<FONT face="宋体"> </FONT>
									                                        <asp:Label id="Label2_BankID" runat="server">4203038256578965</asp:Label></TD>
							                                        </TR>
							                                        <TR>
								                                        <TD width="23%" height="20" bgColor="#eeeeee">&nbsp;QQ 账号:</TD>
								                                        <TD height="10" colspan="2" bgColor="#ffffff">&nbsp;
									                                        <asp:Label id="Label3_QQID" runat="server">42030</asp:Label></TD>
								                                        <TD width="26%" height="10" bgColor="#eeeeee">&nbsp;银行类型:</TD>
								                                        <TD width="25%" height="10" bgColor="#ffffff">&nbsp;<FONT face="宋体">&nbsp;
										                                        <asp:Label id="Label13_BankType" runat="server">工商银行</asp:Label></FONT></TD>
							                                        </TR>
							                                        <TR>
								                                        <TD bgColor="#eeeeee" height="22" style="HEIGHT: 22px">&nbsp;开户姓名:</TD>
								                                        <TD height="22" colspan="2" bgColor="#ffffff" style="HEIGHT: 22px">&nbsp;
									                                        <asp:Label id="Label4_TrueName" runat="server">Ray</asp:Label></TD>
								                                        <TD bgColor="#eeeeee" height="22" style="HEIGHT: 22px">&nbsp;银行名称/开户行名称:</TD>
								                                        <TD bgColor="#ffffff" height="22" style="HEIGHT: 22px">&nbsp;<FONT face="宋体"> </FONT>
									                                        <asp:Label id="Label8_BankName" runat="server">深圳市工商银行南山支行</asp:Label></TD>
							                                        </TR>
							                                        <TR>
								                                        <TD style="HEIGHT: 22px" bgColor="#eeeeee" height="22"><FONT face="宋体">&nbsp;公司名称:</FONT></TD>
								                                        <TD style="HEIGHT: 22px" bgColor="#ffffff" colSpan="2" height="22"><FONT face="宋体">&nbsp;
										                                        <asp:Label id="lbCompay" runat="server">Label</asp:Label></FONT></TD>
								                                        <TD style="HEIGHT: 22px" bgColor="#eeeeee" height="22"><FONT face="宋体">&nbsp;开户时间:</FONT></TD>
								                                        <TD style="HEIGHT: 22px" bgColor="#ffffff" height="22"><FONT face="宋体">&nbsp;
										                                        <asp:Label id="lbAccCreate" runat="server">Label</asp:Label></FONT></TD>
							                                        </TR>
							                                        <TR>
								                                        <TD bgColor="#eeeeee" height="19" style="HEIGHT: 19px">&nbsp;开户地区/省级</TD>
								                                        <TD height="19" colspan="2" bgColor="#ffffff" style="HEIGHT: 19px">&nbsp; 
                                                                            <span id="spanProv"></span>
										                                    <%--<select id="area" style="WIDTH: 100px" onchange="select()" name="area" runat="server"></select>--%>
								                                        </TD>
								                                        <TD bgColor="#eeeeee" height="19" style="HEIGHT: 19px">&nbsp;开户城市:</TD>
								                                        <TD bgColor="#ffffff" height="19" style="HEIGHT: 19px">&nbsp;
                                                                            <span id="spanCity"></span>
										                                    <%--<select id="city" style="WIDTH: 100px" onchange="select()" name="city" runat="server"></select>--%>
								                                        </TD>
							                                        </TR>
							                                        <TR>
								                                        <TD height="18" bgColor="#eeeeee" style="HEIGHT: 18px">&nbsp;最后登陆IP地址:</TD>
								                                        <TD height="18" colspan="2" bgColor="#ffffff" style="HEIGHT: 18px">&nbsp;
									                                        <asp:Label id="Label6_LastIP" runat="server">202.103.24.68</asp:Label></TD>
								                                        <TD bgColor="#eeeeee" height="18" style="HEIGHT: 18px"><FONT face="宋体">&nbsp;最后更新时间:</FONT></TD>
								                                        <TD bgColor="#ffffff" height="18" style="HEIGHT: 18px">&nbsp;<FONT face="宋体">
										                                        <asp:Label id="Label11_Modify_Time" runat="server">2005-05-02 16:40 </asp:Label>
									                                        </FONT>
								                                        </TD>
							                                        </TR>
							                                        <TR>
								                                        <TD height="20" bgColor="#eeeeee">&nbsp;备注:</TD>
								                                        <TD height="-1" colspan="4" bgColor="#ffffff">&nbsp;
									                                        <asp:Label id="Label12_Memo" runat="server">用户于2005年3月1日在广东省深圳市工商银行南山支行开户，预存款5000元。</asp:Label></TD>
							                                        </TR>
						                                        </TABLE>
					                                        </TD>
				                                        </TR>
			                                        </TABLE>
												</TD>
											</TR>
										</TABLE>
										<DIV align="center">
                                            <FONT face="宋体"><asp:Button id="btBack" runat="server" BorderStyle="Groove" Width="71px" Text="  返 回  " onclick="btBack_Click"></asp:Button></FONT>
										</DIV>
									</asp:panel>
                                    <INPUT id="Harea" type="hidden" name="Harea" runat="server"><INPUT id="Hcity" type="hidden" name="Hcity" runat="server">
								</TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
