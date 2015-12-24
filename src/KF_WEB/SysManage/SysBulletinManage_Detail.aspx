<%@ Page language="c#" Codebehind="SysBulletinManage_Detail.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.SysManage.SysBulletinManage_Detail" %>
<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>SysBulletinManage_Detail</title>
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
			<TABLE id="Table1" cellSpacing="1" cellPadding="1" align="center" border="1" runat="server">
				<TR>
					<TD style="HEIGHT: 25px" colSpan="2"><IMG height="16" src="../IMAGES/Page/post.gif" width="15">&nbsp;
						<asp:label id="labTitle" runat="server" ForeColor="Red">修改余额支付状态</asp:label></TD>
				</TR>
				<TR>
					<TD align="left" width="15%"><asp:label id="Label2" runat="server">发布时间：</asp:label></TD>
					<TD align="left">
                        <asp:textbox id="tbIssueTime" runat="server" Width="300px"  onclick="WdatePicker()" CssClass="Wdate"></asp:textbox>
                        <asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" ErrorMessage="请输入" ControlToValidate="tbIssueTime"></asp:requiredfieldvalidator>
					</TD>
				</TR>
				<TR>
					<TD align="left" width="15%"><asp:label id="Label1" runat="server">标题：</asp:label></TD>
					<TD align="left"><asp:textbox id="tbTitle" runat="server" Width="300px"></asp:textbox><asp:requiredfieldvalidator id="RequiredFieldValidator2" runat="server" ErrorMessage="请输入" ControlToValidate="tbTitle"></asp:requiredfieldvalidator></TD>
				</TR>
				<TR>
					<TD align="left" width="15%"><asp:label id="Label3" runat="server">链接：</asp:label></TD>
					<TD align="left"><asp:textbox id="tbUrl" runat="server" Width="300px"></asp:textbox><asp:requiredfieldvalidator id="RequiredFieldValidator3" runat="server" ErrorMessage="请输入" ControlToValidate="tbUrl"></asp:requiredfieldvalidator></TD>
				</TR>
				<TR id="Isnew" runat="server">
					<TD align="left" width="15%"><asp:label id="Label4" runat="server">是否最新：</asp:label></TD>
					<TD align="left"><asp:radiobuttonlist id="rbIsnew" runat="server" Width="250px" RepeatDirection="Horizontal" Height="16px">
							<asp:ListItem Value="1" Selected="True">是</asp:ListItem>
							<asp:ListItem Value="2">否</asp:ListItem>
						</asp:radiobuttonlist></TD>
				</TR>
				<TR id="Isred" runat="server">
					<TD align="left" width="15%"><asp:label id="Label7" runat="server">是否红字：</asp:label></TD>
					<TD align="left"><asp:radiobuttonlist id="rbIsred" runat="server" Width="250px" RepeatDirection="Horizontal" Height="16px">
							<asp:ListItem Value="1">是</asp:ListItem>
							<asp:ListItem Value="0" Selected="True">否</asp:ListItem>
						</asp:radiobuttonlist></TD>
				</TR>
				<TR>
					<TD align="left" width="15%"><asp:label id="Label5" runat="server">到期时间：</asp:label></TD>
					<TD align="left">
                        <asp:textbox id="tbLastTime" runat="server" Width="300px" onclick="WdatePicker()" CssClass="Wdate"></asp:textbox>
                    </TD>
				</TR>
				<TR>
					<TD align="left" width="15%"><asp:label id="Label6" runat="server">发布人：</asp:label></TD>
					<TD align="left"><asp:textbox id="tbUserID" runat="server" Width="300px"></asp:textbox><asp:requiredfieldvalidator id="RequiredFieldValidator4" runat="server" ErrorMessage="请输入" ControlToValidate="tbUserID"></asp:requiredfieldvalidator></TD>
				</TR>
				<TR>
					<TD align="center" colSpan="2"><asp:button id="btnSave" runat="server" Width="80px" Text="提交" onclick="btnSave_Click">
                    </asp:button>&nbsp;<asp:button id="btnBack" runat="server" Width="80px" CausesValidation="False" Text="返回" onclick="btnBack_Click"></asp:button>
					</TD>
				</TR>
			</TABLE>
			<TABLE id="Table2" style="WIDTH: 632px; HEIGHT: 293px" cellSpacing="1" cellPadding="1"
				align="center" border="1" runat="server">
				<TR>
					<TD style="HEIGHT: 25px" colSpan="2"><IMG height="16" src="../IMAGES/Page/post.gif" width="15">&nbsp;
						<asp:label id="Label8" runat="server" ForeColor="Red" Width="272px">财付通银行维护公告</asp:label></TD>
				</TR>
				<TR>
					<TD align="left" width="15%"><FONT face="宋体">&nbsp;&nbsp;&nbsp;&nbsp;银行类型</FONT></TD>
					<TD align="left"><asp:dropdownlist id="ddlQueryBankType" runat="server" Width="160px" AutoPostBack="True"></asp:dropdownlist>&nbsp;&nbsp;&nbsp;&nbsp;<asp:label id="lbbanktype" runat="server" ForeColor="blue" Width="88px"></asp:label></TD>
				</TR>
				<TR>
					<TD align="left" width="15%"><FONT face="宋体">&nbsp;&nbsp;&nbsp;&nbsp; 标题</FONT></TD>
					<TD align="left"><asp:textbox id="tbbanktitle" runat="server" Width="488px" Height="80px" TextMode="MultiLine"></asp:textbox><asp:requiredfieldvalidator id="RequiredFieldValidator5" runat="server" ErrorMessage="请输入" ControlToValidate="tbbanktitle"></asp:requiredfieldvalidator></TD>
				</TR>
				<TR>
					<TD align="left" width="15%"><FONT face="宋体">&nbsp;&nbsp;&nbsp;&nbsp; 正文 </FONT>
					</TD>
					<TD align="left"><asp:textbox id="tbmaintext" runat="server" Width="488px" Height="120px" TextMode="MultiLine"></asp:textbox><asp:requiredfieldvalidator id="Requiredfieldvalidator6" runat="server" ErrorMessage="请输入" ControlToValidate="tbmaintext"></asp:requiredfieldvalidator></TD>
				</TR>
                <TR>
					<TD align="left" width="15%" style="HEIGHT: 17px">
					</TD>
					<TD align="left" style="HEIGHT: 17px">
<asp:CheckBox id="cbopen" runat="server" ForeColor="Red" Width="200px" Text="立即开启" Font-Bold="True" AutoPostBack="True"></asp:CheckBox></TD>
				</TR>
				<TR>
					<TD align="left" width="15%"><FONT face="宋体">&nbsp;&nbsp;开始时间</FONT></TD>
					<TD align="left">
                        <asp:textbox id="tbstarttime" runat="server" Width="300px"  onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss' })" CssClass="Wdate"></asp:textbox>
					</TD>
				</TR>
				<TR>
					<TD align="left" width="15%"><FONT face="宋体">&nbsp; 结束时间</FONT></TD>
					<TD align="left">
                        <asp:textbox id="tbendtime" runat="server" Width="300px"  onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss' })" CssClass="Wdate"></asp:textbox>
						<asp:TextBox id="tbcreateuser" runat="server" Width="128px" Visible="False"></asp:TextBox></TD>
				</TR>
				<TR>
					<TD align="center" colSpan="2"><asp:button id="btadd" runat="server" Width="80px" Text="新增" onclick="btadd_Click"></asp:button>&nbsp;&nbsp;&nbsp;
						<asp:button id="btupdate" runat="server" Width="80px" CausesValidation="False" Text="修改" onclick="btupdate_Click"></asp:button>&nbsp;&nbsp;&nbsp;
						<asp:button id="btback" runat="server" Width="80px" CausesValidation="False" Text="返回" onclick="btback_Click"></asp:button>&nbsp;
						<asp:hyperlink id="hlBack" runat="server" ForeColor="Blue" Width="42px" NavigateUrl="javascript:history.go(-1)">返回</asp:hyperlink></TD>
				</TR>
			</TABLE>
			<TABLE id="Table3" style="WIDTH: 632px; HEIGHT: 293px" cellSpacing="1" cellPadding="1"
				align="center" border="1" runat="server">
				<TR>
					<TD style="HEIGHT: 25px" colSpan="2"><IMG height="16" src="../IMAGES/Page/post.gif" width="15">&nbsp;
						<asp:label id="Label9" runat="server" ForeColor="Red" Width="272px">生活缴费维护公告</asp:label></TD>
				</TR>
				<TR>
					<TD align="left" width="15%"><FONT face="宋体">&nbsp;&nbsp;&nbsp;&nbsp;业务编号</FONT></TD>
					<TD align="left"><asp:TextBox id="tbServicecode" runat="server"></asp:TextBox>
						<asp:Label id="lbuctype" runat="server" ForeColor="Red"></asp:Label></TD>
				</TR>
				<TR>
					<TD align="left" width="15%"><FONT face="宋体">&nbsp;&nbsp;&nbsp;&nbsp; 正文 </FONT>
					</TD>
					<TD align="left"><asp:textbox id="tb_uc_text" runat="server" Width="488px" Height="120px" TextMode="MultiLine"></asp:textbox><asp:requiredfieldvalidator id="Requiredfieldvalidator8" runat="server" ErrorMessage="请输入" ControlToValidate="tbmaintext"></asp:requiredfieldvalidator></TD>
				</TR>
				<TR>
					<TD align="left" width="15%"><FONT face="宋体">&nbsp;&nbsp;开始时间</FONT></TD>
					<TD align="left">
                        <asp:textbox id="tb_ucstarttime" runat="server" Width="300px" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss' })" CssClass="Wdate"></asp:textbox>
                    </TD>
				</TR>
				<TR>
					<TD align="left" width="15%"><FONT face="宋体">&nbsp; 结束时间</FONT></TD>
					<TD align="left">
                        <asp:textbox id="tb_ucendtime" runat="server" Width="300px" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss' })" CssClass="Wdate"></asp:textbox>
                    </TD>
				</TR>
				<TR>
					<TD align="center" colSpan="2">&nbsp;&nbsp;&nbsp;
						<asp:button id="bt_ucupdate" runat="server" Width="80px" CausesValidation="False" Text="修改" onclick="bt_ucupdate_Click"></asp:button>&nbsp;&nbsp;&nbsp;
						<asp:button id="bt_ucback" runat="server" Width="80px" CausesValidation="False" Text="返回" onclick="bt_ucback_Click"></asp:button>&nbsp;
						<asp:hyperlink id="hl_ucback" runat="server" ForeColor="Blue" Width="42px" NavigateUrl="javascript:history.go(-1)">返回</asp:hyperlink></TD>
				</TR>
			</TABLE>
            <TABLE id="Table4" style="WIDTH: 632px; HEIGHT: 293px" cellSpacing="1" cellPadding="1"
				align="center" border="1" runat="server" frame="box" rules="all">
				<TR>
					<TD style="HEIGHT: 25px" colSpan="3"><IMG height="16" src="../IMAGES/Page/post.gif" width="15">&nbsp;
						<asp:label id="Label10" runat="server" ForeColor="Red" Width="272px">银行接口</asp:label></TD>
				</TR>
				<TR>
					<TD align="left" width="15%"><FONT face="宋体">&nbsp;&nbsp;&nbsp;&nbsp;银行类型</FONT></TD>
					<TD align="left" colSpan="2"><asp:dropdownlist id="ddlQueryBankTypeInterface" runat="server" Width="160px" AutoPostBack="True"></asp:dropdownlist>&nbsp;&nbsp;&nbsp;&nbsp;<asp:label id="lbbanktypeInterface" runat="server" ForeColor="blue" Width="88px"></asp:label></TD>
				</TR>
				<TR>
					<TD align="left" width="15%"><FONT face="宋体">&nbsp;&nbsp;&nbsp;&nbsp; 正文 </FONT>
					</TD>
					<TD align="left" colSpan="2"><asp:textbox id="tbInterfaceMainText" runat="server" Width="488px" Height="120px" TextMode="MultiLine">XX银行系统维护中，预计XX月XX日00：00恢复。</asp:textbox><asp:requiredfieldvalidator id="Requiredfieldvalidator9" runat="server" ErrorMessage="请输入" ControlToValidate="tbInterfaceMainText"></asp:requiredfieldvalidator></TD>
				</TR>
                <TR>
					<TD align="left" width="15%"><FONT face="宋体">&nbsp;&nbsp;&nbsp;&nbsp; 影响接口 </FONT>
					</TD>
					<TD align="left" colSpan="2"><asp:CheckBox id="cbForbid" runat="server" ForeColor="Red" Text="禁止付款提示" Font-Bold="True" AutoPostBack="True"></asp:CheckBox></TD>
				</TR>
                <TR id="tcTextId">
					<TD align="left" width="15%"><FONT face="宋体">&nbsp;&nbsp;&nbsp;&nbsp; 弹层正文 </FONT>
					</TD>
					<TD align="left" colSpan="2"><asp:textbox id="TextTCMainText" runat="server" Width="488px" Height="120px" TextMode="MultiLine">XX月XX日00:00至XX月XX日0：00，因XX银行系统维护，此期间操作的付款将延迟到XX月XX日到账。</asp:textbox><asp:requiredfieldvalidator id="Requiredfieldvalidator7" runat="server" ErrorMessage="请输入" ControlToValidate="TextTCMainText"></asp:requiredfieldvalidator></TD>
				</TR>
                <TR>
					<TD align="left" width="15%" style="HEIGHT: 17px">
					</TD>
					<TD style="HEIGHT: 17px"  colSpan="2">
<asp:CheckBox id="InterfaceOpen" runat="server" ForeColor="Red" Text="立即开启" Font-Bold="True" AutoPostBack="True"></asp:CheckBox>
                      <%--  <TD style="HEIGHT: 17px">关闭策略：<asp:radiobuttonlist id="InterfaceLong" runat="server" RepeatDirection="Horizontal"  ForeColor="Red" Font-Bold="True"  AutoPostBack="True"  onselectedindexchanged="InterfaceLong_SelectedIndexChanged">
							<asp:ListItem Value="1">长期每天</asp:ListItem>
							<asp:ListItem Value="0" Selected="True">时间段</asp:ListItem>
						</asp:radiobuttonlist></TD>--%>
                </TD>
				</TR>
				<TR id="StartTime" runat="server">
					<TD align="left" width="15%"><FONT face="宋体">&nbsp;&nbsp;开始时间</FONT></TD>
					<TD align="left" colSpan="2">
                        <asp:textbox id="tbInterfaceStartTime" runat="server" Width="300px" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss' })" CssClass="Wdate"></asp:textbox>
                    </TD>
				</TR>
				<TR id="EndTime" runat="server">
					<TD align="left" width="15%"><FONT face="宋体">&nbsp; 结束时间</FONT></TD>
					<TD align="left" colSpan="2">
                        <asp:textbox id="tbInterfaceEndTime" runat="server" Width="300px" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss' })" CssClass="Wdate"></asp:textbox>
						<asp:TextBox id="tbcreateuserInterface" runat="server" Width="128px" Visible="False"></asp:TextBox>
                        <asp:TextBox id="tbarea" runat="server" Width="128px" Visible="False"></asp:TextBox>
                        <asp:TextBox id="tbcity" runat="server" Width="128px" Visible="False"></asp:TextBox>
                        <asp:TextBox id="tbbusinetype" runat="server" Width="128px" Visible="False"></asp:TextBox>
                        <asp:TextBox id="tbFid" runat="server" Width="128px" Visible="False"></asp:TextBox>
                        </TD>
				</TR>
              <%--  <TR id="alwtime" runat="server">
					<TD align="left" width="15%"><FONT face="宋体">&nbsp;&nbsp;时间段</FONT></TD>
					<TD align="left" colSpan="2"><asp:textbox id="tbalwtime" runat="server" Width="300px">00:00:00-00:00:00</asp:textbox>(请按00:00:00-00:00:00格式输入时间段)</TD>
				</TR>--%>
				<TR>
					<TD align="center" colSpan="3"><asp:button id="btInterfaceAdd" runat="server" Width="80px" Text="新增" onclick="btInterfaceAdd_Click"></asp:button>&nbsp;&nbsp;&nbsp;
						<asp:button id="btInterfaceUpdate" runat="server" Width="80px" CausesValidation="False" Text="修改" onclick="btInterfaceUpdate_Click"></asp:button>&nbsp;&nbsp;&nbsp;
						<asp:button id="btInterfaceBack" runat="server" Width="80px" CausesValidation="False" Text="返回" onclick="btInterfaceBack_Click"></asp:button>&nbsp;
						<asp:hyperlink id="hlInterfaceBack" runat="server" ForeColor="Blue" Width="42px" NavigateUrl="javascript:history.go(-1)">返回</asp:hyperlink></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
