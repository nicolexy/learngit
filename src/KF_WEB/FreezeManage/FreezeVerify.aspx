<%@ Page language="c#" Codebehind="FreezeVerify.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.FreezeManage.FreezeVerify1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>FreezeVerify</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
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
        <script type="text/javascript" src="../SCRIPTS/My97DatePicker/WdatePicker.js"></script>
		<script language="javascript">
					function Init() {
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
	<body MS_POSITIONING="GridLayout" onload="Init()">
		<form id="Form1" method="post" runat="server">
			<table border="1" cellSpacing="1" cellPadding="1" width="1100">
				<TR>
					<TD style="WIDTH: 443px; HEIGHT: 20px" bgColor="#e4e5f7" colSpan="2"><FONT color="red" face="����"><IMG src="../IMAGES/Page/post.gif" width="20" height="16"><asp:label id="lb_pageTitle" Runat="server">��ؽⶳ���</asp:label></FONT></TD>
					<td style="HEIGHT: 20px"></FONT>����Ա����: </FONT><SPAN class="style3"><asp:label id="lb_operatorID" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></td>
				</TR>
				<tr>
					<td><label style="WIDTH: 110px; HEIGHT: 20px; VERTICAL-ALIGN: middle">�Ƹ�ͨ�ʺţ�</label><asp:textbox id="tbx_payAccount" Runat="server" Width="140px" Enabled="False"></asp:textbox></td>
					<td><label style="WIDTH: 110px; HEIGHT: 20px; VERTICAL-ALIGN: middle">�û��ύ֤�����룺</label><asp:textbox id="tbx_cerNO" Runat="server" Width="140px" Enabled="False"></asp:textbox></td>
					<td><label style="WIDTH: 110px; HEIGHT: 20px; VERTICAL-ALIGN: middle">�û��ύ���ֻ���</label><asp:textbox id="tbx_userSubBindMobile" Runat="server" Width="140px" Enabled="False"></asp:textbox></td>
					<td><label style="WIDTH: 130px; HEIGHT: 20px; VERTICAL-ALIGN: middle">���һ��ʹ�õĵ�ַ��</label><asp:textbox id="tbx_lastAddr" Runat="server" Width="140px" Enabled="False"></asp:textbox></td>
				</tr>
				<tr>
					<td><label style="WIDTH: 110px; HEIGHT: 20px; VERTICAL-ALIGN: middle">ע��������</label><asp:textbox id="tbx_userName" Runat="server" Width="140px" Enabled="False"></asp:textbox></td>
					<td><label style="WIDTH: 110px; HEIGHT: 20px; VERTICAL-ALIGN: middle">ע��֤�����룺</label><asp:textbox id="tbx_regCreNO" Runat="server" Width="140px" Enabled="False"></asp:textbox></td>
					<td><label style="WIDTH: 110px; HEIGHT: 20px; VERTICAL-ALIGN: middle">���ֻ���</label><asp:textbox id="tbx_bindMobile" Runat="server" Width="140px" Enabled="False"></asp:textbox></td>
					<td><label style="WIDTH: 130px; HEIGHT: 20px; VERTICAL-ALIGN: middle">���ڰ�װ����֤�飺</label><asp:textbox id="tbx_DC" Runat="server" Width="140px" Enabled="False"></asp:textbox></td>
				</tr>
				<tr>
					<td><label style="WIDTH: 110px; HEIGHT: 20px; VERTICAL-ALIGN: middle">�˻���</label><asp:textbox id="tbx_restFin" Runat="server" Width="140px" Enabled="False"></asp:textbox></td>
					<td><label style="WIDTH: 110px; HEIGHT: 20px; VERTICAL-ALIGN: middle">��ϵ���䣺</label><asp:textbox id="tbx_email" Runat="server" Width="140px" Enabled="False"></asp:textbox></td>
					<td><label style="WIDTH: 110px; HEIGHT: 20px; VERTICAL-ALIGN: middle">��ϵ�绰��</label><asp:textbox id="tbx_phoneNo" Runat="server" Width="140px" Enabled="False"></asp:textbox></td>
				</tr>
			</table>
			<br>
			<br>
			<table id="table_images" border="1" cellSpacing="1" cellPadding="1" width="1200" runat="server">
				<tr bgColor="#ffffff">
					<td vAlign="top" colSpan="3">
						<p>
							<table style="WIDTH: 100%; HEIGHT: 248px" border="0" cellSpacing="1" cellPadding="0">
								<tr align="center">
									<td bgColor="#eeeeee" height="20">���֤ɨ���</td>
									<td bgColor="#eeeeee">���п�ɨ���</td>
									<td id="td_pic1" bgColor="#eeeeee" height="20" runat="server">����֧����ͼ1</td>
									<td id="td_pic2" bgColor="#eeeeee" height="20" runat="server">����֧����ͼ2</td>
								</tr>
								<tr>
									<td style="PADDING-BOTTOM: 3px; PADDING-LEFT: 3px; PADDING-RIGHT: 3px; PADDING-TOP: 3px"
										height="20" align="center"><asp:image id="Image1" runat="server" Width="200px" Height="150px"></asp:image></td>
									<td style="PADDING-BOTTOM: 3px; PADDING-LEFT: 3px; PADDING-RIGHT: 3px; PADDING-TOP: 3px"
										height="20" align="center"><asp:image id="Image2" runat="server" Width="200px" Height="150px"></asp:image></td>
									<td style="PADDING-BOTTOM: 3px; PADDING-LEFT: 3px; PADDING-RIGHT: 3px; PADDING-TOP: 3px"
										height="20" align="center"><asp:image id="Image3" runat="server" Width="200" Height="150px"></asp:image></td>
									<td style="PADDING-BOTTOM: 3px; PADDING-LEFT: 3px; PADDING-RIGHT: 3px; PADDING-TOP: 3px"
										height="20" align="center"><asp:image id="Image4" runat="server" Width="200" Height="150px"></asp:image></td>
								</tr>
							</table>
							<table style="Z-INDEX: 0; WIDTH: 100%; DISPLAY: none; HEIGHT: 248px" id="table_bPics" border="0"
								cellSpacing="1" cellPadding="0" runat="server">
								<tr align="center">
									<td id="td_bpic1" bgColor="#eeeeee" height="20" width="20%" runat="server">����ͼ1</td>
									<td id="td_bpic2" bgColor="#eeeeee" height="20" width="20%" runat="server">����ͼ2</td>
									<td id="td_bpic3" bgColor="#eeeeee" height="20" width="20%" runat="server">����ͼ3</td>
								</tr>
								<tr>
									<td style="PADDING-BOTTOM: 3px; PADDING-LEFT: 3px; PADDING-RIGHT: 3px; PADDING-TOP: 3px"
										height="20" align="center"><asp:image id="Image5" runat="server" Width="200" Height="150px" Visible="False"></asp:image></td>
									<td style="PADDING-BOTTOM: 3px; PADDING-LEFT: 3px; PADDING-RIGHT: 3px; PADDING-TOP: 3px"
										height="20" align="center"><asp:image id="Image6" runat="server" Width="200" Height="150px" Visible="False"></asp:image></td>
									<td style="PADDING-BOTTOM: 3px; PADDING-LEFT: 3px; PADDING-RIGHT: 3px; PADDING-TOP: 3px"
										height="20" align="center"><asp:image id="Image7" runat="server" Width="200" Height="150px" Visible="False"></asp:image></td>
								</tr>
							</table>
						</p>
					</td>
				</tr>
			</table>
			<table border="1" cellSpacing="1" cellPadding="1" width="1200">
				<tr>
					<td bgColor="#eeeeee" height="20" width="50%">�û�������������</td>
					<td bgColor="#eeeeee" height="20" width="50%">�ͷ�������</td>
				</tr>
				<tr>
					<td bgColor="#eeeeee" width="50%"><asp:textbox id="tbx_userQA" Runat="server" Width="400" Enabled="false" Height="82" TextMode="MultiLine"></asp:textbox></td>
					<td bgColor="#eeeeee" width="50%"><asp:textbox id="tbx_handleResult" Runat="server" Width="430" Height="82" TextMode="MultiLine"></asp:textbox><br>
						<label style="MARGIN-BOTTOM: 50px">��ݻظ���</label>
						<asp:dropdownlist id="ddl_fastReply1" Runat="server" Width="350" AutoPostBack="True"></asp:dropdownlist>&nbsp;&nbsp;&nbsp;&nbsp;
						<asp:button id="btn_manageFastReply" Runat="server" Text="�����ݻظ�" onclick="btn_manageFastReply_Click"></asp:button><br>
					</td>
				</tr>
				<tr>
					<td bgColor="#ffffff" height="10" colSpan="2"></td>
				</tr>
				<tr>
					<td bgColor="#ffffff" colSpan="5" align="center"><span style="MARGIN: 0px 30px 0px 0px"><asp:button id="btn_hangUp" runat="server" Width="100" Text="�� ��" onclick="btn_hangUp_Click"></asp:button></span><span style="MARGIN: 0px 30px 0px 0px"><asp:button id="btn_Finish1" runat="server" Width="100" Text="�ᵥ���ѽⶳ��" onclick="btn_Finish1_Click"></asp:button></span><span style="MARGIN: 0px 30px 0px 0px"><asp:button id="btn_Finish2" runat="server" Width="100" Text="�ᵥ��δ�ⶳ��" onclick="btn_Finish2_Click"></asp:button></span><span style="MARGIN: 0px 30px 0px 0px"><asp:button id="btn_Del" runat="server" Width="100" Text="�� ��" onclick="btn_Del_Click"></asp:button></span><span style="MARGIN: 0px 30px 0px 0px"><asp:button id="btn_addRecord" runat="server" Width="100" Text="���䴦����" onclick="btn_addRecord_Click"></asp:button></span></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
