<%@ Page language="c#" Codebehind="PaymentAbnormalQueryNotify.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.RefundManage.PaymentAbnormalQueryNotify" %>
<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>PaymentAbnormalQueryNotify</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); .style2 { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		    .auto-style1
            {
                width: 83px;
                height: 25px;
            }
            .auto-style2
            {
                height: 25px;
            }
		</style>
        <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE style="Z-INDEX: 101; POSITION: absolute;  LEFT: 5%" id="Table1" border="1"
				cellSpacing="1" cellPadding="1" width="85%" >
				<TR>
					<TD bgColor="#e4e5f7" colSpan="5"><FONT color="red" face="����"><IMG src="../IMAGES/Page/post.gif" width="20" height="16">
							&nbsp;&nbsp;&nbsp;�����ӳ��쳣��ѯ</FONT> </FONT></TD>
					<TD bgColor="#e4e5f7" align="right"><FONT face="����">����Ա����: <SPAN class="style3">
								<asp:label id="Label1" runat="server" Width="73px"></asp:label></SPAN></FONT></TD>
				</TR>
				<TR>
					<TD align="right"><asp:label id="Label2" runat="server">����</asp:label></TD>
					<TD>
                        <input type="text" runat="server" id="TextBoxDate" onclick="WdatePicker()" />
                        <img onclick="TextBoxDate.click()" src="../SCRIPTS/My97DatePicker/skin/datePicker.gif" width="16" height="22" style="width:16px;height:22px; cursor:pointer;" alt="ѡ������" />
					</TD>
					<TD align="right" class="auto-style1"><asp:label id="Label5" runat="server">����</asp:label></TD>
					<TD class="auto-style2"><asp:textbox id="tbBatchID" runat="server"></asp:textbox></TD>
                    <TD align="right" class="auto-style1"><asp:label id="Label3" runat="server">��ID</asp:label></TD>
					<TD class="auto-style2"><asp:textbox id="tbPackageID" runat="server"></asp:textbox></TD>
                </TR>
                <TR>
                    <TD align="right" class="auto-style1"><asp:label id="Label4" runat="server">ҵ�񵥺�</asp:label></TD>
					<TD class="auto-style2"><asp:textbox id="tblistid" runat="server" Width="200px"></asp:textbox></TD>
					<TD align="right" class="auto-style2"><asp:label id="Label6" runat="server">ҵ������</asp:label></TD>
					<TD class="auto-style2">
                        <asp:dropdownlist id="ddltype" runat="server">
							<%--<asp:ListItem Value="">����</asp:ListItem>
							<asp:ListItem Value="1"  Selected="True">����</asp:ListItem>--%>
							<%--<asp:ListItem Value="3">�˿�</asp:ListItem>--%>
						</asp:dropdownlist>
					</TD>
                    <TD align="right" class="auto-style2"><asp:label id="Label7" runat="server">��ҵ������</asp:label></TD>
					<TD class="auto-style2">
                        <asp:dropdownlist id="ddlSubTypePay" runat="server"><%--������ҵ������--%>
							<%--<asp:ListItem Value="" Selected="True">����</asp:ListItem>
							<asp:ListItem Value="60">��Q������Ƹ�ͨ</asp:ListItem>
							<asp:ListItem Value="61">��Q��������п�</asp:ListItem>
                            <asp:ListItem Value="62">��Q��������п�</asp:ListItem>
                            <asp:ListItem Value="63">��Q��������п�</asp:ListItem>
                            <asp:ListItem Value="71">΢������T+0</asp:ListItem>
                            <asp:ListItem Value="72">΢������T+1</asp:ListItem>
                            <asp:ListItem Value="81">���ͨ����T+0</asp:ListItem>
                            <asp:ListItem Value="82">���ͨ����T+1</asp:ListItem>
                            <asp:ListItem Value="91">���ÿ�����-΢�Ż���</asp:ListItem>
                            <asp:ListItem Value="92">���ÿ�����-��Q����</asp:ListItem>
                            <asp:ListItem Value="93">���ÿ�����-��վ����</asp:ListItem>--%>
						</asp:dropdownlist>
                    </TD>
			    </TR>
                 <TR>
					<TD align="right" class="auto-style2"><asp:label id="Label9" runat="server">֪ͨ״̬</asp:label></TD>
					<TD class="auto-style2">
                        <asp:dropdownlist id="ddlNotityStatus" runat="server">
							<%--<asp:ListItem Value="">����</asp:ListItem>
                            <asp:ListItem Value="0" Selected="True">��ʼ״̬</asp:ListItem>
							<asp:ListItem Value="1">������</asp:ListItem>
							<asp:ListItem Value="2">�ѷ���</asp:ListItem>--%>
						</asp:dropdownlist>
					</TD>
                     <TD align="right" class="auto-style2"><asp:label id="Label12" runat="server">֪ͨ���</asp:label></TD>
					<TD class="auto-style2">
                        <asp:dropdownlist id="ddlNotityResult" runat="server">
							<%--<asp:ListItem Value="" Selected="True">����</asp:ListItem>
                            <asp:ListItem Value="0">��ʼ״̬</asp:ListItem>
							<asp:ListItem Value="1">�ɹ�</asp:ListItem>
							<asp:ListItem Value="2">ʧ��</asp:ListItem>--%>
						</asp:dropdownlist>
					</TD>
                    <TD align="right" class="auto-style2"><asp:label id="Label10" runat="server">��������</asp:label></TD>
					<TD class="auto-style2">
                        <asp:dropdownlist id="ddlBankType" runat="server">
						</asp:dropdownlist>
                    </TD>
                    
			    </TR>
                 <TR>
                      <TD align="right" class="auto-style2"><asp:label id="Label8" runat="server">��������</asp:label></TD>
					<TD class="auto-style2">
                        <asp:dropdownlist id="ddlErrorType" runat="server">
							<%--<asp:ListItem Value="" Selected="True">����</asp:ListItem>
							<asp:ListItem Value="1">�����쳣</asp:ListItem>
							<asp:ListItem Value="2">��Ȩ�쳣</asp:ListItem>
                            <asp:ListItem Value="3">����ά��</asp:ListItem>
							<asp:ListItem Value="4">��ȡ����ӳ�</asp:ListItem>--%>
						</asp:dropdownlist>
                    </TD>
					<TD align="right" class="auto-style2"><asp:label id="Label11" runat="server">�˻�����</asp:label></TD>
					<TD class="auto-style2">
                        <asp:dropdownlist id="ddlAccType" runat="server">
							<%--<asp:ListItem Value="" Selected="True">����</asp:ListItem>
							<asp:ListItem Value="1">΢��</asp:ListItem>
							<asp:ListItem Value="2">QQ</asp:ListItem>
                            <asp:ListItem Value="3">Email</asp:ListItem>
							<asp:ListItem Value="4">�ֻ���</asp:ListItem>--%>
						</asp:dropdownlist>
					</TD>
                     <TD align="center" colspan="2">
						<asp:button id="Button1" runat="server" Width="80px" Text="�� ѯ" onclick="ButtonQuery_Click"></asp:button>
                     </TD>
                    
			    </TR>
			</TABLE>

			<TABLE id="Table2" style="Z-INDEX: 102; POSITION: absolute; WIDTH: 1400px; TOP: 25%; LEFT: 1%"
				cellSpacing="1" cellPadding="1" border="1" runat="server">
				<TR>
					<TD vAlign="top"><asp:datagrid id="DataGrid1" runat="server" Width="1400px" AutoGenerateColumns="False" GridLines="Horizontal"
							CellPadding="3" BackColor="White" BorderWidth="1px" BorderStyle="None" BorderColor="#E7E7FF">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
                                  <asp:TemplateColumn HeaderText="ѡ��">
									<ItemTemplate>
										<asp:CheckBox id="CheckBox2" runat="server"></asp:CheckBox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="Fabnormal_time" HeaderText="�쳣ʱ��" HeaderStyle-HorizontalAlign="Center"></asp:BoundColumn>
								<asp:BoundColumn DataField="FBatchID" HeaderText="����" HeaderStyle-HorizontalAlign="Center"></asp:BoundColumn>
								<asp:BoundColumn DataField="FPackageID" HeaderText="��ID" HeaderStyle-HorizontalAlign="Center"></asp:BoundColumn>
								<asp:BoundColumn DataField="Flistid" HeaderText="ҵ�񵥺�"></asp:BoundColumn>
								<asp:BoundColumn DataField="Ftype_str" HeaderText="ҵ������"></asp:BoundColumn>
								<asp:BoundColumn DataField="subType_str" HeaderText="��ҵ������"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fbank_type_str" HeaderText="��������"></asp:BoundColumn>
								<asp:BoundColumn DataField="Faccount_type_str" HeaderText="�˻�����"></asp:BoundColumn>
                              <%--  <asp:BoundColumn DataField="Fspid" HeaderText="�̻�ID"></asp:BoundColumn>--%>
								<asp:BoundColumn DataField="Ferror_type_str" HeaderText="��������"></asp:BoundColumn>
                                 <asp:BoundColumn DataField="Fnotify_type_str" HeaderText="����֪ͨ����"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fnotify_status_str" HeaderText="����֪ͨ״̬"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fnotify_result_str" HeaderText="����֪ͨ���"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fnotify_history_str" HeaderText="��ʷ֪ͨ����"></asp:BoundColumn>
							</Columns>
							<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD height="25">
                        <div style="position:relative;">
                            <span style="color:red;position:absolute;right:250px">����:<span id="lb_conut" runat="server">0</span></span>
                            <webdiyer:aspnetpager id="pager" runat="server" NumericButtonTextFormatString="[{0}]" SubmitButtonText="ת��" PageSize="10"
							    OnPageChanged="ChangePage" HorizontalAlign="right" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0"
							    ShowCustomInfoSection="left" NumericButtonCount="5" AlwaysShow="True"></webdiyer:aspnetpager>
                        </div>
					</TD>
				</TR>
                <TR>
                <td height="40px">
                    <asp:radiobuttonlist id="ChooseRadio" runat="server" RepeatDirection="Horizontal"  ForeColor="Red" Font-Bold="True"  AutoPostBack="True"  onselectedindexchanged="Choose_SelectedIndexChanged">
							<asp:ListItem Value="1" Selected="True">��ѡ</asp:ListItem>
							<asp:ListItem Value="2" >ȫѡ��ҳ</asp:ListItem>
                             <asp:ListItem Value="3" >ȫѡ����</asp:ListItem>
                            </asp:radiobuttonlist>
                    <asp:Button id="btSendWX" runat="server" Text ="��΢��Ϣ" Width="80px" OnClick="SendWX_Click"></asp:Button>
                    <%--<asp:Button id="btSendQQ" runat="server" Text ="��QQ��Ϣ" Width="80px" OnClick="SendQQ_Click"></asp:Button>
                    <asp:Button id="btSendEmail" runat="server" Text ="��Email" Width="80px" OnClick="SendEmail_Click"></asp:Button>--%>
                    <asp:Button id="btSendMES" runat="server" Text ="������" Width="80px" OnClick="SendMES_Click"></asp:Button>
                   <%-- <asp:Button id="btSendTips" runat="server" Text ="��Tips" Width="80px" OnClick="SendTips_Click"></asp:Button>
                    <asp:Button id="btSendWallet" runat="server" Text ="��СǮ����Ϣ" Width="100px" OnClick="SendWallet_Click"></asp:Button>--%>
                     </td>
                </TR>
			</TABLE>
		</form>
	</body>
</HTML>
