<%@ Page language="c#" Codebehind="BankInterfaceManage_Detail.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.SysManage.BankInterfaceManage_Detail" %>
<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>BankInterfaceManage_Detail</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); .style4 { COLOR: #ff0000 }
	.style5 { FONT-WEIGHT: bold; COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>

	<script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			
            <TABLE id="Table4" style="position:relative;top:50px; WIDTH: 900px; HEIGHT: 293px" cellSpacing="1" cellPadding="1"
				align="center" border="1" runat="server" frame="box" rules="all">
				<TR>
					<TD style="HEIGHT: 25px" colSpan="3"><IMG height="16" src="../IMAGES/Page/post.gif" width="15">&nbsp;
						<asp:label id="labTitle" runat="server" ForeColor="Red" Width="272px">���нӿ�</asp:label></TD>
				</TR>
                <TR>
					<TD align="left" width="15%"><FONT face="����">&nbsp;&nbsp;&nbsp;&nbsp; ���� </FONT>
					</TD>
					<TD align="left" colSpan="2"><asp:textbox id="tbTitle" runat="server" Width="488px" Height="120px" TextMode="MultiLine">����ά������</asp:textbox></TD>
			    	<TD rowspan="18" align="left" valign="top">
							<asp:datagrid id="DatagridBank" runat="server" Width="300px"  PageSize="200"
								AutoGenerateColumns="False" CellPadding="3" BackColor="White" BorderWidth="1px" BorderStyle="None"
								BorderColor="#E7E7FF" GridLines="Horizontal">
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<Columns>
                                    <asp:BoundColumn ItemStyle-Width="100px" DataField="banktype" HeaderText="���б���"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="banktype_str" HeaderText="��������"></asp:BoundColumn>
								</Columns>
								<PagerStyle HorizontalAlign="Center" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
							</asp:datagrid>
				    </TD>
                </TR>
				<TR>
					<TD align="left" width="15%"><FONT face="����">&nbsp;&nbsp;&nbsp;&nbsp; ���� </FONT>
					</TD>
					<TD align="left"><asp:textbox id="tbInterfaceMainText" runat="server" Width="488px" Height="120px" TextMode="MultiLine">����ϵͳά���У�Ԥ��endtime�ָ���</asp:textbox><asp:requiredfieldvalidator id="Requiredfieldvalidator9" runat="server" ErrorMessage="������" ControlToValidate="tbInterfaceMainText"></asp:requiredfieldvalidator></TD>
				</TR>
             <%--   <TR>
					<TD align="left" width="15%"><FONT face="����">&nbsp;&nbsp;&nbsp;&nbsp; Ӱ��ӿ� </FONT>
					</TD>
					<TD align="left" colSpan="2"><asp:CheckBox id="cbForbid" runat="server" ForeColor="Red" Text="��ֹ������ʾ" Font-Bold="True" AutoPostBack="True"></asp:CheckBox></TD>
				</TR>--%>
                 <TR>
					<TD align="left" width="15%"><FONT face="����">&nbsp;&nbsp;&nbsp;&nbsp; Ӱ��ӿ� </FONT>
					</TD>
					<TD align="left"><asp:radiobuttonlist id="ForbidRadio" runat="server" RepeatDirection="Horizontal"  ForeColor="Red" Font-Bold="True"  AutoPostBack="True"  onselectedindexchanged="ForbidRadio_SelectedIndexChanged">
							<asp:ListItem Value="1" >Ӳ�ر�</asp:ListItem>
							<asp:ListItem Value="2" Selected="True">��ر�</asp:ListItem>
                            <asp:ListItem Value="3" >���Թر�</asp:ListItem>
                            </asp:radiobuttonlist>
                   </TD>
				</TR>
                <TR id="tcTextId">
					<TD align="left" width="15%"><FONT face="����">&nbsp;&nbsp;&nbsp;&nbsp; �������� </FONT>
					</TD>
					<TD align="left"><asp:textbox id="TextTCMainText" runat="server" Width="488px" Height="120px" TextMode="MultiLine">startime��endtime��������ϵͳά�������ڼ�������ʽ�Ԥ��XX��XX�յ���</asp:textbox><asp:requiredfieldvalidator id="Requiredfieldvalidator7" runat="server" ErrorMessage="������" ControlToValidate="TextTCMainText"></asp:requiredfieldvalidator></TD>
				</TR>
                <TR>
					<TD align="left" width="15%" style="HEIGHT: 17px">
					</TD>
					<TD style="HEIGHT: 17px">
<asp:CheckBox id="InterfaceOpen" runat="server" ForeColor="Red" Text="��������" Font-Bold="True" AutoPostBack="True"></asp:CheckBox>
                      <%--  <TD style="HEIGHT: 17px">�رղ��ԣ�<asp:radiobuttonlist id="InterfaceLong" runat="server" RepeatDirection="Horizontal"  ForeColor="Red" Font-Bold="True"  AutoPostBack="True"  onselectedindexchanged="InterfaceLong_SelectedIndexChanged">
							<asp:ListItem Value="1">����ÿ��</asp:ListItem>
							<asp:ListItem Value="0" Selected="True">ʱ���</asp:ListItem>
						</asp:radiobuttonlist></TD>--%>
                </TD>
				</TR>
                 <TR id="TR1">
					<TD align="left" width="15%"><font face="����" color="red" style="font-weight:bold;">&nbsp;&nbsp;&nbsp;&nbsp;�رչ��� </FONT>
					</TD>
					<TD align="left"><asp:radiobuttonlist id="closedRadio" runat="server" RepeatDirection="Horizontal"  ForeColor="Red" Font-Bold="True"  AutoPostBack="True" >
							<asp:ListItem Value="1" Selected="True">ȫ���ر�</asp:ListItem>
							<asp:ListItem Value="2" >���ǩԼ</asp:ListItem>
                            <asp:ListItem Value="3" >���֧��</asp:ListItem>
                            <%--<asp:ListItem Value="4" >�˿�</asp:ListItem>--%>
                            </asp:radiobuttonlist>
                    </TD>
				</TR>
                 <TR id="TRTimeSet">
					<TD align="left" width="15%"><FONT face="����" color="red" style="font-weight:bold;">&nbsp;&nbsp;&nbsp;&nbsp;�趨ʱ��</FONT>
					</TD>
					<TD align="left"><FONT face="����" color="red" style="font-weight:bold;">&nbsp;&nbsp;&nbsp;&nbsp;Ԥ���ر�ʱ��&nbsp;&nbsp;&nbsp;&nbsp;</FONT>
                    <asp:button id="btAddTime" runat="server" Width="80px" Font-Bold="True" ForeColor="Red" Text="����" onclick="btAddTime_Click"></asp:button>
                    </TD>
				</TR>
				<TR id="StartTime" runat="server">
					<TD align="left" width="15%"><FONT face="����">&nbsp;&nbsp;&nbsp;&nbsp; ��ʼʱ��</FONT></TD>
					<TD align="left"><asp:textbox id="tbStartTime" runat="server" Width="300px"  onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss' })" CssClass="Wdate"></asp:textbox></TD>
				</TR>
				<TR id="EndTime" runat="server">
					<TD align="left" width="15%"><FONT face="����">&nbsp;&nbsp;&nbsp;&nbsp; ����ʱ��</FONT></TD>
					<TD align="left">
                        <asp:textbox id="tbEndTime" runat="server" Width="300px" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss' })" CssClass="Wdate"></asp:textbox>
						<asp:TextBox id="tbcreateuserInterface" runat="server" Width="128px" Visible="False"></asp:TextBox>
                        <asp:TextBox id="tbarea" runat="server" Width="128px" Visible="False"></asp:TextBox>
                        <asp:TextBox id="tbcity" runat="server" Width="128px" Visible="False"></asp:TextBox>
                        <asp:TextBox id="tbbusinetype" runat="server" Width="128px" Visible="False"></asp:TextBox>
                        <asp:TextBox id="tbbulletin_id" runat="server" Width="128px" Visible="False"></asp:TextBox>
                         <asp:TextBox id="tbcreatetime" runat="server" Width="128px" Visible="False"></asp:TextBox>
                        </TD>
				</TR>
                <TR id="StartTime1" runat="server" visible="false">
					<TD align="left" width="15%"><FONT face="����">&nbsp;&nbsp;&nbsp;&nbsp; ��ʼʱ��1</FONT></TD>
					<TD align="left"><asp:textbox id="tbStartTime1" runat="server" Width="300px"  onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss' })" CssClass="Wdate"></asp:textbox></TD>
				</TR>
				<TR id="EndTime1" runat="server" visible="false">
					<TD align="left" width="15%"><FONT face="����">&nbsp;&nbsp;&nbsp;&nbsp; ����ʱ��1</FONT></TD>
					<TD align="left"><asp:textbox id="tbEndTime1" runat="server" Width="300px"  onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss' })" CssClass="Wdate"></asp:textbox></TD>
				</TR>
                <TR id="StartTime2" runat="server" visible="false">
					<TD align="left" width="15%"><FONT face="����">&nbsp;&nbsp;&nbsp;&nbsp; ��ʼʱ��2</FONT></TD>
					<TD align="left"><asp:textbox id="tbStartTime2" runat="server" Width="300px"  onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss' })" CssClass="Wdate"></asp:textbox></TD>
				</TR>
				<TR id="EndTime2" runat="server" visible="false">
					<TD align="left" width="15%"><FONT face="����">&nbsp;&nbsp;&nbsp;&nbsp; ����ʱ��2</FONT></TD>
					<TD align="left"><asp:textbox id="tbEndTime2" runat="server" Width="300px" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss' })" CssClass="Wdate"></asp:textbox></TD>
				</TR>
                <TR id="StartTime3" runat="server" visible="false">
					<TD align="left" width="15%"><FONT face="����">&nbsp;&nbsp;&nbsp;&nbsp; ��ʼʱ��3</FONT></TD>
					<TD align="left"><asp:textbox id="tbStartTime3" runat="server" Width="300px" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss' })" CssClass="Wdate"></asp:textbox></TD>
				</TR>
				<TR id="EndTime3" runat="server" visible="false">
					<TD align="left" width="15%"><FONT face="����">&nbsp;&nbsp;&nbsp;&nbsp; ����ʱ��3</FONT></TD>
					<TD align="left"><asp:textbox id="tbEndTime3" runat="server" Width="300px" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss' })" CssClass="Wdate"></asp:textbox></TD>
				</TR>
                <TR id="StartTime4" runat="server" visible="false">
					<TD align="left" width="15%"><FONT face="����">&nbsp;&nbsp;&nbsp;&nbsp; ��ʼʱ��4</FONT></TD>
					<TD align="left"><asp:textbox id="tbStartTime4" runat="server" Width="300px" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss' })" CssClass="Wdate"></asp:textbox></TD>
				</TR>
				<TR id="EndTime4" runat="server" visible="false">
					<TD align="left" width="15%"><FONT face="����">&nbsp;&nbsp;&nbsp;&nbsp; ����ʱ��4</FONT></TD>
					<TD align="left"><asp:textbox id="tbEndTime4" runat="server" Width="300px" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss' })" CssClass="Wdate"></asp:textbox>
                        </TD>
				</TR>
              <%--  <TR id="alwtime" runat="server">
					<TD align="left" width="15%"><FONT face="����">&nbsp;&nbsp;ʱ���</FONT></TD>
					<TD align="left" colSpan="2"><asp:textbox id="tbalwtime" runat="server" Width="300px">00:00:00-00:00:00</asp:textbox>(�밴00:00:00-00:00:00��ʽ����ʱ���)</TD>
				</TR>--%>
				<TR>
					<TD align="center" colspan="3"><asp:button id="btInterfaceAdd" runat="server" Width="80px" Text="����" onclick="btInterfaceAdd_Click"></asp:button>&nbsp;&nbsp;&nbsp;
						<asp:button id="btInterfaceUpdate" runat="server" Width="80px" CausesValidation="False" Text="�޸�" onclick="btInterfaceUpdate_Click"></asp:button>&nbsp;&nbsp;&nbsp;
						<asp:button id="btInterfaceBack" runat="server" Width="80px" CausesValidation="False" Text="����" onclick="btInterfaceBack_Click"></asp:button>&nbsp;
						<asp:hyperlink id="hlInterfaceBack" runat="server" ForeColor="Blue" Width="42px" NavigateUrl="javascript:history.go(-1)">����</asp:hyperlink></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
