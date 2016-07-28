<%@ Page language="c#" Codebehind="CFTUserCheck.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.CFTUserCheck" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>CFTUserCheck</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); .style2 { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }

        .watermark_wrap span {
            z-index: 999;
            position: absolute;
            color:#808080;
            font-size:14px;
            -moz-transform: rotate(0deg) scale(1.00,1.00) translate(0px,0px) skew(0deg,-12deg);
            -moz-transform-origin: 0% 0%;
            -webkit-transform: rotate(0deg) scale(1.00,1.00) translate(0px,0px) skew(0deg,-12deg);
            -webkit-transform-origin: 0% 0%;
            -o-transform: rotate(0deg) scale(1.00,1.00) translate(0px,0px) skew(0deg,-12deg);
            -o-transform-origin: 0% 0%;
            transform: rotate(0deg) scale(1.00,1.00) translate(0px,0px) skew(0deg,-12deg);
            transform-origin: 0% 0%;
            -moz-user-select: none;
            -webkit-user-select: none;
            -ms-user-select: none;
            -khtml-user-select: none;
            user-select: none;
            -moz-opacity:0.5;
			-khtml-opacity: 0.5;
			opacity: 0.5;
        }
		</style>
		<script type="text/javascript">
			function showTips_2()
			{
				if(document.getElementById("cb_2").checked)
				{
					document.getElementById("div_cbxl2").style.display = "";
				}
				else
				{
					document.getElementById("div_cbxl2").style.display = "none";
				}
			}
			
			function shotTips_3()
			{
				if(document.getElementById("cb_3").checked)
				{
					document.getElementById("div_cbx_3").style.display = "";
				}
				else
				{
					document.getElementById("div_cbx_3").style.display = "none";
				}
			}
			
			function Init()
			{
				showTips_2();
				shotTips_3();
				watermark("watermark_wrap", "<%=uid%>");
			}

			function watermark(wrapClass, text) {
			    var wrap = document.createElement("div");
			    wrap.className = wrapClass;
			    var page_width = Math.max(document.body.scrollWidth, document.body.clientWidth);
			    var page_height = Math.max(document.body.scrollHeight, document.body.clientHeight);
			    for (var i = 20; i < page_width; i += 300) {
			        for (var j = 20; j < page_height; j += 100) {
			            var node = document.createElement("span");
			            node.style.top = j;
			            node.style.left = i;
			            node.textContent = node.innerText = text;
			            wrap.appendChild(node);
			        }
			    }
			    document.body.appendChild(wrap);
			    window.onresize = window.onload = function () {
			        wrap.parentElement.removeChild(wrap);
			        watermark(wrapClass, text);
			    };
			}


		</script>
	</HEAD>
	<body onload="Init()" MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table border="0" cellSpacing="1" cellPadding="0" width="100%" bgColor="#000000">
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee" height="20" background="IMAGES/Page/bk_white.gif" width="50%"
						colSpan="2" align="center">��̨����(
						<asp:label id="lblfid" runat="server" ForeColor="Red"></asp:label>)</td>
					<td bgColor="#eeeeee" background="IMAGES/Page/bk_white.gif" width="50%" colSpan="2"
						align="center">�û��ύ����(
						<asp:label id="labFTypeName" runat="server" ForeColor="Red"></asp:label>)</td>
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee" height="20" width="25%">&nbsp;&nbsp;&nbsp;&nbsp;�û��ʺ�</td>
					<td width="25%">&nbsp;
						<asp:label id="labFQQid" runat="server" ForeColor="#8080FF"></asp:label></td>
					<td width="25%">&nbsp;
						<asp:label id="labIsAnswer" runat="server" ForeColor="#00C000"></asp:label></td>
					<td bgColor="#eeeeee" width="25%">&nbsp;&nbsp;&nbsp;&nbsp;ԭ�ܱ���</td>
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;֤������</td>
					<td>&nbsp;
						<asp:label id="labFcreid" runat="server" ForeColor="#8080FF"></asp:label></td>
					<td>&nbsp;
						<asp:label id="cre_id" runat="server" ForeColor="#00C000"></asp:label></td>
					<td bgColor="#eeeeee">&nbsp;&nbsp;&nbsp;&nbsp;֤������</td>
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;֤������</td>
					<td>&nbsp;
						<asp:label id="labFcre_type" runat="server" ForeColor="#8080FF"></asp:label></td>
					<td>&nbsp;
						<asp:label id="cre_type" runat="server" ForeColor="#00C000"></asp:label></td>
					<td bgColor="#eeeeee">&nbsp;&nbsp;&nbsp;&nbsp;֤������</td>
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;�����ַ</td>
					<td>&nbsp;
						<asp:label id="labFEmail" runat="server" ForeColor="#8080FF"></asp:label></td>
					<td>&nbsp;
						<asp:label id="email" runat="server" ForeColor="#00C000"></asp:label></td>
					<td bgColor="#eeeeee">&nbsp;&nbsp;&nbsp;&nbsp;�����ַ</td>
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;����</td>
					<td>&nbsp;
						<asp:label id="labFtruename" runat="server" ForeColor="#8080FF"></asp:label></td>
					<td>&nbsp;
						<asp:label id="old_name" runat="server" ForeColor="#00C000"></asp:label></td>
					<td bgColor="#eeeeee">&nbsp;&nbsp;&nbsp;&nbsp;����</td>
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;�ܽ��</td>
					<td>&nbsp;
						<asp:label id="labFbalance" runat="server" ForeColor="#8080FF"></asp:label></td>
					<td>&nbsp;
						<asp:label id="new_name" runat="server" ForeColor="#00C000"></asp:label></td>
					<td bgColor="#eeeeee">&nbsp;&nbsp;&nbsp;&nbsp;������</td>
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;������</td>
					<td>&nbsp;
						<asp:label id="labFCon" runat="server" ForeColor="#8080FF"></asp:label></td>
					<td>&nbsp;
						<asp:label id="clear_pps" runat="server" ForeColor="#00C000"></asp:label></td>
					<td bgColor="#eeeeee">&nbsp;&nbsp;&nbsp;&nbsp;�Ƿ�����ܱ�����</td>
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;�����ʺ�</td>
					<td>&nbsp;
						<asp:label id="labFBankAcc" runat="server" ForeColor="#8080FF"></asp:label></td>
					<td>&nbsp;
						<asp:label id="lblBindMobileUser" runat="server" ForeColor="#00C000"></asp:label></td>
					<td bgColor="#eeeeee">&nbsp;&nbsp;&nbsp;&nbsp;�ֻ�</td>
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;����˱�׼��</td>
					<td>&nbsp;
						<asp:label id="lblstandard_score" runat="server" ForeColor="#8080FF"></asp:label></td>
					<td>&nbsp;
						<asp:label id="lblBindMailUser" runat="server" ForeColor="#00C000"></asp:label></td>
					<td bgColor="#eeeeee">&nbsp;&nbsp;&nbsp;&nbsp;������</td>
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;ʵ�ʵ÷�</td>
					<td>&nbsp;
						<asp:label id="lblscore" runat="server" ForeColor="#8080FF"></asp:label></td>
					<td>&nbsp;
						<asp:label id="labFstatename" runat="server" ForeColor="#00C000"></asp:label></td>
					<td bgColor="#eeeeee">&nbsp;&nbsp;&nbsp;&nbsp;������ǰ״̬</td>
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;�÷���ϸ</td>
					<td>&nbsp;
						<asp:label id="lbldetail_score" runat="server" ForeColor="#8080FF"></asp:label></td>
					<td>&nbsp;
						<asp:label id="new_cre_id" runat="server" ForeColor="#00C000"></asp:label></td>
					<td bgColor="#eeeeee">&nbsp;&nbsp;&nbsp;&nbsp;��֤������</td>
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;��ر��</td>
					<td>&nbsp;
						<asp:label id="lblrisk_result" runat="server" ForeColor="#8080FF"></asp:label></td>
                    <td>&nbsp;
						<asp:label id="lbauthenState" runat="server" ForeColor="#00C000"></asp:label></td>
					<td bgColor="#eeeeee">&nbsp;&nbsp;&nbsp;&nbsp;ʵ����֤</td>
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee" height="20">&nbsp;&nbsp;&nbsp;&nbsp;IVR���н��</td>
					<td colSpan="3">&nbsp;
						<asp:label id="lblivrresult" runat="server" ForeColor="#8080FF"></asp:label></td>
				</tr>
			</table>
			<hr SIZE="1">
			<table border="0" cellSpacing="1" cellPadding="0" width="100%" bgColor="#000000">
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee" height="20" background="IMAGES/Page/bk_white.gif" width="50%"
						align="center">���֤ɨ���</td>
					<td bgColor="#eeeeee" background="IMAGES/Page/bk_white.gif" width="50%" align="center">�ܾ�ԭ��</td>
				</tr>
				<tr bgColor="#ffffff">
					<td vAlign="top">
						<table style="WIDTH: 589px; HEIGHT: 248px" border="0" cellSpacing="1" cellPadding="0" width="589">
							<tr align="center">
								<td bgColor="#eeeeee" height="20" width="50%">�� ��</td>
								<td bgColor="#eeeeee"><asp:label id="lbImageSpecial" runat="server">�� ��</asp:label></td>
							</tr>
							<tr>
								<td style="PADDING-BOTTOM: 3px; PADDING-LEFT: 3px; PADDING-RIGHT: 3px; PADDING-TOP: 3px"
									height="20" align="center"><asp:image id="Image1" runat="server" Width="200px" Height="150px"></asp:image></td>
								<td style="PADDING-BOTTOM: 3px; PADDING-LEFT: 3px; PADDING-RIGHT: 3px; PADDING-TOP: 3px"
									align="center"><asp:image id="Image2" runat="server" Width="200px" Height="150px"></asp:image></td>
							</tr>
						</table>
					</td>
					<td rowSpan="3">
						<table>
							<tr>
								<td width="300">
									<p style="MARGIN: 0px 0px 30px; FONT-SIZE: 15px"><asp:checkbox id="cb_1" runat="server" Text="δ��ʾͼƬ"></asp:checkbox></p>
									<p style="MARGIN: 0px 0px 30px; FONT-SIZE: 15px"><asp:checkbox id="cb_5" runat="server" Text="δ�ṩ���֤ɨ���"></asp:checkbox></p>
									<p style="MARGIN: 0px 0px 30px; FONT-SIZE: 15px"><asp:checkbox id="cb_2" onclick="showTips_2()" runat="server" Text="�ϴ���ɨ���������������������Ч"></asp:checkbox></p>
									<p style="MARGIN: 0px 0px 30px; FONT-SIZE: 15px"><asp:checkbox id="cb_3" onclick="shotTips_3()" runat="server" Text="֤��������ԭע��֤�����벻��"></asp:checkbox></p>
									<p style="MARGIN: 0px 0px 30px; FONT-SIZE: 15px"><asp:checkbox id="cb_4" runat="server" Text="ʵ����֤��Ч����Ϊ16��80����"></asp:checkbox></p>
									<p style="MARGIN: 0px 0px 30px; FONT-SIZE: 15px"><asp:checkbox id="cb_0" runat="server" Text="����ԭ��"></asp:checkbox></p>
								</td>
								<td>
									<div style="DISPLAY: none" id="div_cbxl2" runat="server"><asp:checkboxlist id="cbxl_2" Runat="server" CellSpacing="0" CellPadding="0" CssClass="MARGIN: 0px"
											Font-Size="10">
											<asp:ListItem Value="ɨ������岻����"></asp:ListItem>
											<asp:ListItem Value="ɨ���������������"></asp:ListItem>
											<asp:ListItem Value="ɨ�����֤�����벻����"></asp:ListItem>
											<asp:ListItem Value="ɨ����ϵ�ַģ��"></asp:ListItem>
											<asp:ListItem Value="ɨ�����ͷ����Ƭ������"></asp:ListItem>
											<asp:ListItem Value="ɨ���������"></asp:ListItem>
											<asp:ListItem Value="ɨ����ǲ�ɫ"></asp:ListItem>
											<asp:ListItem Value="ɨ�����Ϊ������Ƭ"></asp:ListItem>
											<asp:ListItem Value="ɨ�������Ϳ����Ч"></asp:ListItem>
											<asp:ListItem Value="ɨ���������������޸Ĺ�"></asp:ListItem>
											<asp:ListItem Value="ɨ���������Ч���ڣ��ѹ��ڣ�"></asp:ListItem>
											<asp:ListItem Value="ɨ�����ɫ�нϴ�ɫ��"></asp:ListItem>
											<asp:ListItem Value="ɨ����Ƿ���"></asp:ListItem>
										</asp:checkboxlist></div>
									<div style="DISPLAY: none" id="div_cbx_3" runat="server"><asp:checkbox id="cbx_detail_cbx3" Text="���������ύ������ʱ���ϴ��˻��ʽ���Դ��ͼ����ο������������һء�ָ����http://help.tenpay.com/cgi-bin/helpcenter/help_center.cgi?id=2232&amp;type=0"
											Runat="server"></asp:checkbox></div>
								</td>
							</tr>
						</table>
						<!--
						<asp:checkboxlist id="RejectReason" Runat="server">
							<asp:ListItem Value="1">δ�ṩ���֤ɨ���</asp:ListItem>
							<asp:ListItem Value="2">�ϴ���ɨ���������������������Ч</asp:ListItem>
							<asp:ListItem Value="3">֤��������ԭע��֤�����벻��</asp:ListItem>
							<asp:ListItem Value="4">�ϴ���ɨ�����Ƹ�ͨע�����ϲ���</asp:ListItem>
							<asp:ListItem Value="5">��֧�ֽ������޸�Ϊ�س�</asp:ListItem>
							<asp:ListItem Value="6">�ʻ��������</asp:ListItem>
							<asp:ListItem Value="7">��δ��ɵĽ��׵�</asp:ListItem>
							<asp:ListItem Value="8">��δʹ����δ���ڵĲƸ�ȯ</asp:ListItem>
							<asp:ListItem Value="0">����ԭ��</asp:ListItem>
						</asp:checkboxlist>
						--><asp:textbox id="tbFCheckInfo" runat="server" Width="100%" Height="74px" TextMode="MultiLine"></asp:textbox></td>
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee" height="20" background="IMAGES/Page/bk_white.gif" align="center">����ԭ��</td>
				</tr>
				<tr bgColor="#ffffff">
					<td height="20"><asp:textbox id="tbReason" runat="server" Width="100%" Height="74px" TextMode="MultiLine" Enabled="False"
							BorderStyle="Groove" ReadOnly="True"></asp:textbox></td>
				</tr>
				<tr bgColor="#ffffff">
					<td bgColor="#eeeeee" background="IMAGES/Page/bk_white.gif" width="100%" colSpan="2"
						align="center">��ע</td>
				</tr>
				<tr bgColor="#ffffff">
					<td width="100%" colSpan="2"><asp:textbox id="tbComment" runat="server" Width="100%" Height="74px" TextMode="MultiLine" BorderStyle="Groove"></asp:textbox></td>
				</tr>
			</table>
			<table border="0" cellSpacing="0" cellPadding="0" width="100%">
				<tr>
					<td height="30" align="center"><asp:button id="btSetRealName" Width="85px" Text="ͨ������ʵ��" Runat="server" Visible="False" onclick="btSetRealName_Click"></asp:button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						<asp:button id="btOK" runat="server" Width="85px" Text="ͨ��" Visible="False" onclick="btOK_Click"></asp:button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						<asp:button id="btCancel" runat="server" Width="85px" Text="�ܾ�" onclick="btCancel_Click"></asp:button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						<asp:button id="btnDel" runat="server" Width="85px" Text="ɾ��" Visible="False" onclick="btnDel_Click"></asp:button></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
