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
					<TD style="WIDTH: 1000px" bgColor="#e4e5f7" colSpan="4"><FONT face="����"><FONT color="red"><IMG src="../IMAGES/Page/post.gif" width="20" height="16">&nbsp;&nbsp;һ��ͨҵ��</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>����Ա����: </FONT><asp:label id="Label1" runat="server" Width="73px" ForeColor="Red"></asp:label></TD>
				</TR>
                </table>
			<asp:panel id="PanelList" runat="server">
            <TABLE style="margin-top:5px; margin-left:5px" id="Table2" border="1" cellSpacing="1"
				cellPadding="1" width="1000">
					<TR>
						<TD width="150" align="right">��ѯ����:</TD>
						<TD>
							<P>
								<asp:radiobutton id="rbtn_all" AutoPostBack="True" Text="ȫ��" Runat="server" Checked="false" GroupName="rbtnQueryType"></asp:radiobutton>
								<asp:radiobutton id="rbtn_ydt" AutoPostBack="True" Text="һ��ͨ" Runat="server" Checked="false" GroupName="rbtnQueryType"></asp:radiobutton>
								<asp:radiobutton id="rbtn_fastPay" AutoPostBack="True" Text="���֧��" Runat="server" Checked="false"
									GroupName="rbtnQueryType"></asp:radiobutton></P>
						</TD>
						<TD width="150" colSpan="2" align="left">
							<%--<asp:checkbox id="cbx_showAbout" Text="��ʾ��ѯ������ؽ��" Runat="server" Checked="True"></asp:checkbox></TD>--%>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label2" runat="server">�Ƹ�ͨ�˺�</asp:label></TD>
						<TD width="250">
							<asp:textbox id="txtQQ" runat="server"></asp:textbox></TD>
						<TD width="150" align="right">
							<asp:label id="Label3" runat="server">�ڲ�ID</asp:label></TD>
						<TD width="250">
							<asp:textbox id="tbx_uid" runat="server" Width="200"></asp:textbox></TD>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label17" runat="server">��������</asp:label></TD>
						<TD width="450">
							<asp:DropDownList id="ddl_BankType" runat="server">
								<asp:ListItem Value="" Selected="True">ȫ��</asp:ListItem>
							</asp:DropDownList>
							<asp:RadioButton id="rbtn_bkt_XYK" AutoPostBack="true" Text="���ÿ�" Runat="server" GroupName="bkt"></asp:RadioButton>
							<asp:RadioButton id="rbtn_bkt_JJK" AutoPostBack="true" Text="��ǿ�" Runat="server" GroupName="bkt"></asp:RadioButton>
							<asp:RadioButton id="rbtn_bkt_ALL" AutoPostBack="true" Text="ȫ��" Runat="server" GroupName="bkt"></asp:RadioButton></TD>
						<TD width="150" align="right">
							<asp:label id="Label18" runat="server">���п���</asp:label></TD>
						<TD width="250">
							<asp:textbox id="tbx_bankID" runat="server" Width="200"></asp:textbox></TD>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label19" runat="server">֤������</asp:label></TD>
						<TD width="250">
							<asp:DropDownList id="ddl_creType" runat="server">
								<asp:ListItem Value="" Selected="True">ȫ��</asp:ListItem>
								<asp:ListItem Value="1">���֤</asp:ListItem>
								<asp:ListItem Value="2">����</asp:ListItem>
								<asp:ListItem Value="3">����֤</asp:ListItem>
							</asp:DropDownList></TD>
						<TD width="150" align="right">
							<asp:label id="Label20" runat="server">֤����</asp:label></TD>
						<TD width="250">
							<asp:textbox id="tbx_creID" runat="server" Width="200"></asp:textbox></TD>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label21" runat="server">Э���</asp:label></TD>
						<TD width="250">
							<asp:textbox id="tbx_serNum" runat="server"></asp:textbox></TD>
						<TD width="150" align="right">
							<asp:label id="Label22" runat="server">�ֻ�����</asp:label></TD>
						<TD width="250">
							<asp:textbox id="tbx_phone" runat="server" Width="200"></asp:textbox></TD>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label23" runat="server">��ʼ����</asp:label></TD>
						<TD width="250">
							<asp:textbox id="tbx_beginDate" runat="server"></asp:textbox>
							<asp:imagebutton id="ButtonBeginDate" runat="server" ImageUrl="../Images/Public/edit.gif" CausesValidation="False"></asp:imagebutton></TD>
						<TD width="150" align="right">
							<asp:label id="Label24" runat="server">��������</asp:label></TD>
						<TD width="250">
							<asp:textbox id="tbx_endDate" runat="server"></asp:textbox>
							<asp:imagebutton id="ButtonEndDate" runat="server" ImageUrl="../Images/Public/edit.gif" CausesValidation="False"></asp:imagebutton></TD>
					</TR>
					<TR>
						<TD width="150" align="right"><LABEL id="LABEL4" runat="server">��״̬</LABEL></TD>
						<TD width="250">
							<asp:DropDownList id="ddl_bindStatue" runat="server">
								<asp:ListItem Value="99" Selected="True">����</asp:ListItem>
								<asp:ListItem Value="0">δ����</asp:ListItem>
								<asp:ListItem Value="1">Ԥ�ڰ�״̬</asp:ListItem>
								<asp:ListItem Value="2">��ȷ��</asp:ListItem>
								<asp:ListItem Value="3">�����</asp:ListItem>
							</asp:DropDownList></TD>
						<TD colSpan="2" align="center">
							<asp:button id="btnSearch" runat="server" Width="80px" Text="�� ѯ" onclick="btnSearch_Click"></asp:button></TD>
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
									<asp:BoundColumn DataField="fuin" HeaderText="�Ƹ�ͨ�ʺ�">
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="fcre_id" HeaderText="֤����">
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fbank_typeStr" HeaderText="��������">
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
									</asp:BoundColumn>
                                    <asp:BoundColumn DataField="fcard_tail" HeaderText="���ź���λ">
										<HeaderStyle Wrap="False"></HeaderStyle>
									</asp:BoundColumn>
									<asp:ButtonColumn HeaderText="��ϸ" Text="��ϸ" ButtonType="LinkButton" CommandName="query"></asp:ButtonColumn>
								</Columns>
							</asp:datagrid>
							<webdiyer:aspnetpager id="pager1" runat="server" HorizontalAlign="right" AlwaysShow="True" ShowCustomInfoSection="left"
								NumericButtonTextFormatString="[{0}]" SubmitButtonText="ת��" CssClass="mypager" ShowInputBox="always"
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
									<asp:BoundColumn DataField="Fbind_serialno" HeaderText="�����к�">
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fuin" HeaderText="�Ƹ�ͨ�˺�">
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fbank_typeStr" HeaderText="��������">
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
									</asp:BoundColumn>
                                    <asp:BoundColumn DataField="Fxyzf_typeStr" HeaderText="΢�����ÿ�">
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fprotocol_no" HeaderText="Э����">
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fbank_statusStr" HeaderText="���а�״̬">
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Fcard_tail" HeaderText="���п�����λ">
										<HeaderStyle Wrap="False"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Ftruename" HeaderText="���п��˻���">
										<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
									</asp:BoundColumn>
									<asp:TemplateColumn HeaderText="��ϸ">
										<ItemTemplate>
											<a href='./BankCardUnbindNew.aspx?type=edit&Findex=<%# DataBinder.Eval(Container, "DataItem.Findex")%>&Fuid=<%# DataBinder.Eval(Container, "DataItem.Fuid")%>'>
												��ϸ</a>
										</ItemTemplate>
									</asp:TemplateColumn>
								</Columns>
							</asp:datagrid>
							<webdiyer:aspnetpager id="pager" runat="server" HorizontalAlign="right" AlwaysShow="True" ShowCustomInfoSection="left"
								NumericButtonTextFormatString="[{0}]" SubmitButtonText="ת��" CssClass="mypager" ShowInputBox="always"
								PagingButtonSpacing="0" NumericButtonCount="10"></webdiyer:aspnetpager></TD>
					</TR>
                </table>
			</asp:panel>
           <asp:panel id="PanelMod" runat="server"  Visible="False">
            <TABLE style="margin-top:5px; margin-left:5px" id="Table3" border="1" cellSpacing="1"
				cellPadding="1" width="1000">
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label6" runat="server">�Ƹ�ͨ�˺�</asp:label></TD>
						<TD width="250">
							<asp:label id="lblFuin" runat="server"></asp:label></TD>
						<TD width="150" align="right">
							<asp:label id="Label8" runat="server">��������</asp:label></TD>
						<TD width="250">
							<asp:label id="lblFbank_type" runat="server"></asp:label></TD>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label5" runat="server">�����к�</asp:label></TD>
						<TD width="250">
							<asp:label id="lblFbind_serialno" runat="server"></asp:label></TD>
						<TD width="150" align="right">
							<asp:label id="Label7" runat="server">Э����</asp:label></TD>
						<TD width="250">
							<asp:label id="lblFprotocol_no" runat="server"></asp:label></TD>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label9" runat="server">���а�״̬</asp:label></TD>
						<TD width="250">
							<asp:label id="lblFbank_status" runat="server"></asp:label></TD>
						<TD width="150" align="right">
							<asp:label id="Label10" runat="server">���п�����λ</asp:label></TD>
						<TD width="250">
							<asp:label id="lblFcard_tail" runat="server"></asp:label></TD>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label11" runat="server">���п��˻���</asp:label></TD>
						<TD width="250">
							<asp:label id="lblFtruename" runat="server"></asp:label></TD>
						<TD width="150" align="right">
							<asp:label id="Label12" runat="server">��������</asp:label></TD>
						<TD width="250">
							<asp:label id="lblFbind_type" runat="server"></asp:label></TD>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label14" runat="server">��Ч��ʶ</asp:label></TD>
						<TD width="250">
							<asp:label id="lblFbind_flag" runat="server"></asp:label></TD>
						<TD width="150" align="right">
							<asp:label id="Label15" runat="server">���������п���</asp:label></TD>
						<TD width="250">
							<asp:label id="lblFbank_id" runat="server"></asp:label></TD>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label13" runat="server">����״̬</asp:label></TD>
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
							<asp:label id="Label25" runat="server">֤������</asp:label></TD>
						<TD width="250">
							<asp:label id="lblcreType" runat="server"></asp:label></TD>
						<TD width="150" align="right">
							<asp:label id="Label16" runat="server">֤������</asp:label></TD>
						<TD width="250">
							<asp:label id="lblCreID" runat="server"></asp:label></TD>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label26" runat="server">�ֻ���</asp:label></TD>
						<TD width="250">
							<asp:label id="lblPhone" runat="server"></asp:label></TD>
						<TD width="150" align="right">
							<asp:label id="Label28" runat="server">�ڲ�ID</asp:label></TD>
						<TD width="250">
							<asp:label id="lblUid" runat="server"></asp:label></TD>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label27" runat="server">������</asp:label></TD>
						<TD width="250">
							<asp:label id="lblTrueName" runat="server"></asp:label></TD>
						<TD width="150" align="right">
							<asp:label id="Label30" runat="server">����ʱ��</asp:label></TD>
						<TD width="250">
							<asp:label id="lblCreateTime" runat="server"></asp:label></TD>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label32" runat="server">��ʱ�䣨���أ�</asp:label></TD>
						<TD width="250">
							<asp:label id="lblbindTimeLocal" runat="server"></asp:label></TD>
						<TD width="150" align="right">
							<asp:label id="Label34" runat="server">��ʱ�䣨���У�</asp:label></TD>
						<TD width="250">
							<asp:label id="lblbindTimeBank" runat="server"></asp:label></TD>
					</TR>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label36" runat="server">���ʱ�䣨���أ�</asp:label></TD>
						<TD width="250">
							<asp:label id="lblUnbindTimeLocal" runat="server"></asp:label></TD>
						<TD width="150" align="right">
							<asp:label id="Label38" runat="server">���ʱ�䣨���У�</asp:label></TD>
						<TD width="250">
							<asp:label id="lblUnbindTimeBank" runat="server"></asp:label></TD>
					</TR>
                    <TR>
						<TD width="150" align="right">
							<asp:label id="Label29" runat="server">����֧���޶�(Ԫ)</asp:label></TD>
						<TD width="250">
							<asp:label id="lblonce_quota" runat="server"></asp:label></TD>
						<TD width="150" align="right">
							<asp:label id="Label33" runat="server">����֧���޶Ԫ��</asp:label></TD>
						<TD width="250">
							<asp:label id="lblday_quota" runat="server"></asp:label></TD>
					</TR>
                    <TR>
						<TD width="150" align="right">
							<asp:label id="Label31" runat="server">С��������֪ͨ</asp:label></TD>
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
							<asp:label id="Label40" runat="server">�������������</asp:label></TD>
						<TD width="250" colspan="3">
                            <asp:CheckBox ID="CheckBoxUnbind" runat="server" checked="false"/></TD>
					</TR>
                    </asp:panel>
					<TR>
						<TD width="150" align="right">
							<asp:label id="Label35" runat="server">��ע</asp:label></TD>
						<TD colSpan="3">
							<asp:TextBox id="txtFmemo" Width="100%" Runat="server" TextMode="MultiLine"></asp:TextBox></TD>
					</TR>
					<TR>
						<TD colSpan="4" align="center">
							<asp:Button id="btnUnbind" Text="�����" Runat="server" Enabled="False" onclick="btnUnbind_Click"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
							<asp:Button id="btnCancel" Text=" ȡ  �� " Runat="server" onclick="btnCancel_Click"></asp:Button>&nbsp;&nbsp;
                            <asp:Button id="btnSynchron" Text=" ͬ  �� " Runat="server" onclick="btnSynchron_Click"></asp:Button>
                        </TD>
					</TR>
                </TABLE>
		    </asp:panel>
		</form>
	</body>
</HTML>
