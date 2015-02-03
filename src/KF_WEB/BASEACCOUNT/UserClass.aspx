<%@ Page language="c#" Codebehind="UserClass.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.UserClass" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>CFTUserPick</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); .style2 { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		<script src="../SCRIPTS/Local.js"></script>
	
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<div id="test" style="Z-INDEX: 102; LEFT: 100px; VISIBILITY: hidden; POSITION: absolute; TOP: 300px">
			<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD><INPUT type="button" value="放大" onclick="ZoomOut(imgid);"> <INPUT type="button" value="缩小" onclick="ZoomIn(imgid);">
						<INPUT type="button" value="顺时" onclick="RotaRight(imgid);"><INPUT type="button" value="关闭" onclick="HiddenImg(test);"></TD>
				</TR>
			</TABLE>
			<img id="imgid">
		</div>
		<div id="movediv" style="Z-INDEX: 102; LEFT: 100px; VISIBILITY: hidden; POSITION: absolute; TOP: 300px">
			<img id="moveimgid" width="400" height="300"></div>
		<form id="Form1" method="post" runat="server">
			<table cellSpacing="1" cellPadding="0" width="100%" bgColor="#000000" border="0">
				<tr bgColor="#ffffff">
					<td align="center" background="IMAGES/Page/bk_white.gif" bgColor="#eeeeee" colSpan="3"
						height="20"><font color="blue">财付通用户实名认证处理</font></td>
				</tr>
				<tr bgColor="#ffffff">
					<td align="center" width="25%" height="20"><asp:button id="btnPick" runat="server" Text="领  单" onclick="btnPick_Click"></asp:button></td>
					<td align="center" width="25%" height="20">
						<asp:HyperLink id="hlNewORder" runat="server" Target="_blank" NavigateUrl="UserClass.aspx?type=new"
							ForeColor="Blue">领取新单</asp:HyperLink></td>
					<td align="center" height="20">
						<asp:Label id="Label1" runat="server">Label</asp:Label></td>
				</tr>
			</table>
			<P></P>
			<table cellSpacing="1" cellPadding="0" width="100%" bgColor="#eeeeee" border="0">
				<tr bgColor="#eeeeee">
					<td vAlign="top">
						<table cellSpacing="1" cellPadding="0" width="100%" border="0">
							<tr align="center">
								<td bgColor="#eeeeee" height="20">申诉ID
								</td>
								<td><asp:label id="labid1" runat="server" ForeColor="Blue"></asp:label></td>
								<td bgColor="#eeeeee">证件类型</td>
								<td><asp:label id="labcardtype1" runat="server" ForeColor="Blue"></asp:label></td>
							</tr>
							<tr>
								<td style="PADDING-RIGHT: 3px; PADDING-LEFT: 3px; PADDING-BOTTOM: 3px; PADDING-TOP: 3px"
									align="center" colSpan="2"><asp:image id="Image1" runat="server" Width="200px" Height="150px"></asp:image>
								</td>
								<td colSpan="2">
									<table cellSpacing="1" cellPadding="0" width="100%" border="0">
										<tr>
											<td align="right"><asp:label id="Label4" runat="server">后五位证件号</asp:label></td>
											<td><asp:textbox id="tbidcard1" runat="server" Width="90px"></asp:textbox></td>
										</tr>
										<tr align="center">
											<td colSpan="2"><asp:radiobuttonlist id="rblist1" runat="server">
													<asp:ListItem Value="1">证件类型错或不可识别</asp:ListItem>
												</asp:radiobuttonlist></td>
										</tr>
										<tr>
											<td align="right"><asp:label id="Label3" runat="server" Visible="False">姓名</asp:label></td>
											<td><asp:textbox id="tbname1" runat="server" Width="90px" Visible="False"></asp:textbox></td>
										</tr>
									</table>
								</td>
							</tr>
						</table>
					</td>
					<td vAlign="top">
						<table cellSpacing="1" cellPadding="0" width="100%" border="0">
							<tr align="center">
								<td bgColor="#eeeeee" height="20">申诉ID
								</td>
								<td><asp:label id="labid2" runat="server" ForeColor="Blue"></asp:label></td>
								<td bgColor="#eeeeee">证件类型</td>
								<td><asp:label id="labcardtype2" runat="server" ForeColor="Blue"></asp:label></td>
							</tr>
							<tr>
								<td style="PADDING-RIGHT: 3px; PADDING-LEFT: 3px; PADDING-BOTTOM: 3px; PADDING-TOP: 3px"
									align="center" colSpan="2"><asp:image id="Image2" runat="server" Width="200px" Height="150px"></asp:image></td>
								<td colSpan="2">
									<table cellSpacing="1" cellPadding="0" width="100%" border="0">
										<tr>
											<td align="right"><asp:label id="Label8" runat="server">后五位证件号</asp:label></td>
											<td><asp:textbox id="tbidcard2" runat="server" Width="90px"></asp:textbox></td>
										</tr>
										<tr align="center">
											<td colSpan="2"><asp:radiobuttonlist id="rblist2" runat="server">
													<asp:ListItem Value="1">证件类型错或不可识别</asp:ListItem>
												</asp:radiobuttonlist></td>
										</tr>
										<tr>
											<td align="right"><asp:label id="Label7" runat="server" Visible="False">姓名</asp:label></td>
											<td><asp:textbox id="tbname2" runat="server" Width="90px" Visible="False"></asp:textbox></td>
										</tr>
									</table>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr bgColor="#ccccff">
					<td colSpan="2"></td>
				</tr>
				<tr bgColor="#eeeeee">
					<td vAlign="top">
						<table cellSpacing="1" cellPadding="0" width="100%" border="0">
							<tr align="center">
								<td bgColor="#eeeeee" height="20">申诉ID
								</td>
								<td><asp:label id="labid3" runat="server" ForeColor="Blue"></asp:label></td>
								<td bgColor="#eeeeee">证件类型</td>
								<td><asp:label id="labcardtype3" runat="server" ForeColor="Blue"></asp:label></td>
							</tr>
							<tr>
								<td style="PADDING-RIGHT: 3px; PADDING-LEFT: 3px; PADDING-BOTTOM: 3px; PADDING-TOP: 3px"
									align="center" colSpan="2"><asp:image id="Image3" runat="server" Width="200px" Height="150px"></asp:image></td>
								<td colSpan="2">
									<table cellSpacing="1" cellPadding="0" width="100%" border="0">
										<tr>
											<td align="right"><asp:label id="Label12" runat="server">后五位证件号</asp:label></td>
											<td><asp:textbox id="tbidcard3" runat="server" Width="90px"></asp:textbox></td>
										</tr>
										<tr align="center">
											<td colSpan="2"><asp:radiobuttonlist id="rblist3" runat="server">
													<asp:ListItem Value="1">证件类型错或不可识别</asp:ListItem>
												</asp:radiobuttonlist></td>
										</tr>
										<tr>
											<td align="right"><asp:label id="Label11" runat="server" Visible="False">姓名</asp:label></td>
											<td><asp:textbox id="tbname3" runat="server" Width="90px" Visible="False"></asp:textbox></td>
										</tr>
									</table>
								</td>
							</tr>
						</table>
					</td>
					<td vAlign="top">
						<table cellSpacing="1" cellPadding="0" width="100%" border="0">
							<tr align="center">
								<td bgColor="#eeeeee" height="20">申诉ID
								</td>
								<td><asp:label id="labid4" runat="server" ForeColor="Blue"></asp:label></td>
								<td bgColor="#eeeeee">证件类型</td>
								<td><asp:label id="labcardtype4" runat="server" ForeColor="Blue"></asp:label></td>
							</tr>
							<tr>
								<td style="PADDING-RIGHT: 3px; PADDING-LEFT: 3px; PADDING-BOTTOM: 3px; PADDING-TOP: 3px"
									align="center" colSpan="2"><asp:image id="Image4" runat="server" Width="200px" Height="150px"></asp:image></td>
								<td colSpan="2">
									<table cellSpacing="1" cellPadding="0" width="100%" border="0">
										<tr>
											<td align="right"><asp:label id="Label16" runat="server">后五位证件号</asp:label></td>
											<td><asp:textbox id="tbidcard4" runat="server" Width="90px"></asp:textbox></td>
										</tr>
										<tr align="center">
											<td colSpan="2"><asp:radiobuttonlist id="rblist4" runat="server">
													<asp:ListItem Value="1">证件类型错或不可识别</asp:ListItem>
												</asp:radiobuttonlist></td>
										</tr>
										<tr>
											<td align="right"><asp:label id="Label15" runat="server" Visible="False">姓名</asp:label></td>
											<td><asp:textbox id="tbname4" runat="server" Width="90px" Visible="False"></asp:textbox></td>
										</tr>
									</table>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr bgColor="#ccccff">
					<td colSpan="2" align="center"><asp:button id="btnCommit" runat="server" Text="提  交" onclick="btnCommit_Click"></asp:button></td>
				</tr>
				<tr bgColor="#ccccff">
					<td align="center">
						<asp:Label id="Label2" runat="server">输入有效日期</asp:Label>
						<asp:TextBox id="tbSumDate" runat="server"></asp:TextBox>
						<asp:Button id="btnSum" runat="server" Text="个人统计" onclick="btnSum_Click"></asp:Button></td>
					<td align="center">
						<asp:Label id="labSumInfo" runat="server" ForeColor="Blue"></asp:Label></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
