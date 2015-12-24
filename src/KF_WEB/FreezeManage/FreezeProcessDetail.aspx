<%@ Page language="c#" Codebehind="FreezeProcessDetail.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.FreezeManage.FreezeProcessDetail" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>FreezeProcessDetail</title>
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
		<script language="javascript">
                    //���֤����
					function cbxSfzz()
					{
					    var sfzz = document.getElementById("cbBt_sfzz");
					    var sfz1 = document.getElementById("cbBt_sfz1");
					    if (sfz1.checked) {
					        sfzz.checked = true;
					    } else
					    {
					        sfzz.checked = false;
					    }
					}
					//���֤����
					function cbxSfzf() {
					    var sfzf = document.getElementById("cbBt_sfzf");
					    var sfz2 = document.getElementById("cbBt_sfz2");
					    if (sfz2.checked) {
					        sfzf.checked = true;
					    } else {
					        sfzf.checked = false;
					    }
					}
		            //���п���Ƭ
					function cbxYhkzp() {
					    var yhkzp = document.getElementById("cbBt_yhkzp");
					    var yhk1 = document.getElementById("cbBt_yhk1");
					    if (yhk1.checked) {
					        yhkzp.checked = true;
					    } else {
					        yhkzp.checked = false;
					    }
					}
		            //�ʽ���Դ��ͼ
					function cbxZjlyjt() {
					    var zjlyjt = document.getElementById("cbBt_zjlyjt");
					    var zjly1 = document.getElementById("cbBt_zjly1");
					    var zjly2 = document.getElementById("cbBt_zjly2");
					    var zjly3 = document.getElementById("cbBt_zjly3");
					    var zjly4 = document.getElementById("cbBt_zjly4");
					    var zjly5 = document.getElementById("cbBt_zjly5");
					    var zjly6 = document.getElementById("cbBt_zjly6");
					    var zjly7 = document.getElementById("cbBt_zjly7");
					    var i = 0;
					    if (zjly1.checked) {
					        i++;
					    }
					    if (zjly2.checked) {
					        i++;
					    }
					    if (zjly3.checked) {
					        i++;
					    }
					    if (zjly4.checked) {
					        i++;
					    }
					    if (zjly5.checked) {
					        i++;
					    }
					    if (zjly6.checked) {
					        i++;
					    }
					    if (zjly7.checked) {
					        i++;
					    }
					    if (i == 0) {
					        zjlyjt.checked = false;
					    }
					    else
					    {
					        zjlyjt.checked = true;
					    }
					}
					//��������֤����Ƭ
					function cbxBcqtzjzp() {
					    var bcqtzjzp = document.getElementById("cbBt_bcqtzjzp");
					    var qtzp1 = document.getElementById("cbBt_qtzp1");
					    var qtzp2 = document.getElementById("cbBt_qtzp2");
					    var qtzp3 = document.getElementById("cbBt_qtzp3");
					    var qtzp_zdy = document.getElementById("cbBt_qtzp_zdy");

					    var i = 0;
					    
					    if (qtzp1.checked) {
					        i++;
					        qtzp2.checked = false;
					        qtzp3.checked = false;
					        qtzp_zdy.checked = false;
					    }
					    if (qtzp2.checked) {
					        i++;
					        qtzp1.checked = false;
					        qtzp3.checked = false;
					        qtzp_zdy.checked = false;
					    }
					    if (qtzp3.checked) {
					        i++;
					        qtzp1.checked = false;
					        qtzp2.checked = false;
					        qtzp_zdy.checked = false;
					    }
					    if (qtzp_zdy.checked) {
					        i++;
					        qtzp1.checked = false;
					        qtzp2.checked = false;
					        qtzp3.checked = false;
					    }
                        
					    if (i == 0) {
					        bcqtzjzp.checked = false;
					    }
					    else {
					        bcqtzjzp.checked = true;
					    }
					    
					}
					//�����ֳ����֤������
					function cbxBcsfzsczp() {
					    var bcsfzsczp = document.getElementById("cbBt_bcsfzsczp");
					    var scbs1 = document.getElementById("cbBt_scbs1");
					    var scbs2 = document.getElementById("cbBt_scbs2");
					    var scbs_zdy = document.getElementById("cbBt_scbs_zdy");

					    var i = 0;
					    if (scbs1.checked) {
					        i++;
					        scbs2.checked = false;
					        scbs_zdy.checked = false;
					    }
					    if (scbs2.checked) {
					        i++;
					        scbs1.checked = false;
					        scbs_zdy.checked = false;
					    }
					    if (scbs_zdy.checked) {
					        i++;
					        scbs1.checked = false;
					        scbs2.checked = false;
					    }

					    if (i == 0) {
					        bcsfzsczp.checked = false;
					    }
					    else {
					        bcsfzsczp.checked = true;
					    }
					}
					//���仧��֤����Ƭ
					function cbxBchjzmzp() {
					    var bchjzmzp = document.getElementById("cbBt_bchjzmzp");
					    var hjzm1 = document.getElementById("cbBt_hjzm1");
					    var hjzm2 = document.getElementById("cbBt_hjzm2");
					    var hjzm_zdy = document.getElementById("cbBt_hjzm_zdy");

					    var i = 0;
					    if (hjzm1.checked) {
					        i++;
					        hjzm2.checked = false;
					        hjzm_zdy.checked = false;
					    }
					    if (hjzm2.checked) {
					        i++;
					        hjzm1.checked = false;
					        hjzm_zdy.checked = false;
					    }
					    if (hjzm_zdy.checked) {
					        i++;
					        hjzm1.checked = false;
					        hjzm2.checked = false;
					    }

					    if (i == 0) {
					        bchjzmzp.checked = false;
					    }
					    else {
					        bchjzmzp.checked = true;
					    }
					}
					//�������Ͻ�ͼ
					function cbxBcjljtzp() {
					    var bcjljtzp = document.getElementById("cbBt_bcjljtzp");
					    var bczl1 = document.getElementById("cbBt_bczl1");
					    var bczl2 = document.getElementById("cbBt_bczl2");
					    var bczl_zdy = document.getElementById("cbBt_bczl_zdy");

					    var i = 0;
					    if (bczl1.checked) {
					        i++;
					        bczl2.checked = false;
					        bczl_zdy.checked = false;
					    }
					    if (bczl2.checked) {
					        i++;
					        bczl1.checked = false;
					        bczl_zdy.checked = false;
					    }
					    if (bczl_zdy.checked) {
					        i++;
					        bczl1.checked = false;
					        bczl2.checked = false;
					    }

					    if (i == 0) {
					        bcjljtzp.checked = false;
					    }
					    else {
					        bcjljtzp.checked = true;
					    }
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

					function Init() {
					    watermark("watermark_wrap", "<%=uid%>");
            	    }
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout" onload="Init()">
		<form id="Form1" method="post" runat="server">
			<table border="1" cellSpacing="1" cellPadding="1" width="1200">
				<TR>
					<TD style="WIDTH: 443px; HEIGHT: 20px" bgColor="#e4e5f7" colSpan="2"><FONT color="red" face="����"><IMG src="../IMAGES/Page/post.gif" width="20" height="16"><asp:label id="lb_pageTitle" Runat="server">��ؽⶳ���(��)</asp:label></FONT></TD>
					<td style="HEIGHT: 20px" colSpan="2"></FONT>����Ա����: </FONT><SPAN class="style3"><asp:label id="lb_operatorID" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></td>
				</TR>
				<tr>
					<td><label style="WIDTH: 110px; HEIGHT: 20px; VERTICAL-ALIGN: middle">�Ƹ�ͨ�ʺţ�</label><asp:textbox id="tbx_payAccount" Runat="server" Width="140px" Enabled="False"></asp:textbox></td>
					<td><label style="WIDTH: 110px; HEIGHT: 20px; VERTICAL-ALIGN: middle">�û��ύ֤�����룺</label><asp:textbox id="tbx_cerNO" Runat="server" Width="140px" Enabled="False"></asp:textbox></td>
					<td><label style="WIDTH: 110px; HEIGHT: 20px; VERTICAL-ALIGN: middle">�û��ύ������</label><asp:textbox id="tbx_subUserName" Runat="server" Width="140px" Enabled="False"></asp:textbox></td>
				</tr>
				<tr>
					<td><label style="WIDTH: 110px; HEIGHT: 20px; VERTICAL-ALIGN: middle">�˻���</label><asp:textbox id="tbx_restFin" Runat="server" Width="140px" Enabled="False"></asp:textbox></td>
				    <td><label style="WIDTH: 110px; HEIGHT: 20px; VERTICAL-ALIGN: middle">ע��֤�����룺</label><asp:textbox id="tbx_regCreNO" Runat="server" Width="140px" Enabled="False"></asp:textbox></td>
                    <td><label style="WIDTH: 110px; HEIGHT: 20px; VERTICAL-ALIGN: middle">ע��������</label><asp:textbox id="tbx_userName" Runat="server" Width="140px" Enabled="False"></asp:textbox></td>
                     </tr>
				<tr>
					
					<td><label style="WIDTH: 110px; HEIGHT: 20px; VERTICAL-ALIGN: middle">��ϵ���䣺</label><asp:textbox id="tbx_email" Runat="server" Width="140px" Enabled="False"></asp:textbox></td>
					<td><label style="WIDTH: 110px; HEIGHT: 20px; VERTICAL-ALIGN: middle">��ϵ�绰��</label><asp:textbox id="tbx_phoneNo" Runat="server" Width="140px" Enabled="False"></asp:textbox></td>
                    <td style="color:red"><label style="WIDTH: 110px; HEIGHT: 20px; VERTICAL-ALIGN: middle;">����ԭ��</label><asp:textbox id="tbx_freezeReason" Runat="server" Width="140px" Enabled="False"></asp:textbox></td>
				</tr>
                <tr runat="server" id="TR1">
					<td><label style="WIDTH: 110px; HEIGHT: 20px; VERTICAL-ALIGN: middle">����˱�׼�֣�</label><asp:textbox id="lblstandard_score" Runat="server" Width="140px" Enabled="False"></asp:textbox></td>
					<td><label style="WIDTH: 110px; HEIGHT: 20px; VERTICAL-ALIGN: middle">��ر�ǣ�</label><asp:textbox id="lblrisk_result" Runat="server" Width="140px" Enabled="False"></asp:textbox></td>
					<td><label style="WIDTH: 110px; HEIGHT: 20px; VERTICAL-ALIGN: middle">�Ƿ�����ܱ����ϣ�</label><asp:textbox id="clear_pps" Runat="server" Width="140px" Enabled="False"></asp:textbox></td>
				</tr>
                  <tr runat="server" id="TR2">
					<td><label style="WIDTH: 110px; HEIGHT: 20px; VERTICAL-ALIGN: middle">ʵ�ʵ÷֣�</label><asp:textbox id="lblscore" Runat="server" Width="140px" Enabled="False"></asp:textbox></td>
					<td colspan="2"><label style="WIDTH: 110px; HEIGHT: 20px; VERTICAL-ALIGN: middle">ʵ����֤��</label><asp:textbox id="lbauthenState" Runat="server" Width="140px" Enabled="False"></asp:textbox></td>
				</tr>
                   <tr runat="server" id="TR3">
					<td colspan="3"><label style="WIDTH: 110px; HEIGHT: 20px; VERTICAL-ALIGN: middle">�÷���ϸ��</label><asp:textbox id="lbldetail_score" Runat="server" Width="100%" Enabled="False"></asp:textbox></td>
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
									<td id="td_pic1" bgColor="#eeeeee" height="20">���֤������Ƭ</td>
									<td id="td_pic2" bgColor="#eeeeee">���֤������Ƭ</td>
									<td id="td_pic3" bgColor="#eeeeee" height="20" runat="server">���п���Ƭ</td>
									<td id="td_pic4" bgColor="#eeeeee" height="20" runat="server">�ʽ���Դ��ͼ</td>
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
							<table style="Z-INDEX: 0; WIDTH: 100%; HEIGHT: 248px" id="table_bt1" border="0"
								cellSpacing="1" cellPadding="0" runat="server">
								<tr align="center">
									<td id="tdBt_pic1" bgColor="#eeeeee" height="20" width="20%" runat="server"><asp:checkbox id="cbBt_sfzz" Text="���֤������Ƭ" Runat="server"></asp:checkbox></td>
									<td id="tdBt_pic2" bgColor="#eeeeee" height="20" width="20%" runat="server"><asp:checkbox id="cbBt_sfzf" Text="���֤������Ƭ" Runat="server"></asp:checkbox></td>
									<td id="tdBt_pic3" bgColor="#eeeeee" height="20" width="20%" runat="server"><asp:checkbox id="cbBt_yhkzp" Text="���п���Ƭ" Runat="server"></asp:checkbox></td>
                                    <td id="tdBt_pic4" bgColor="#eeeeee" height="20" width="20%" runat="server"><asp:checkbox id="cbBt_zjlyjt" Text="�ʽ���Դ��ͼ" Runat="server"></asp:checkbox></td>
								</tr>
								<tr>
									<td style="PADDING-BOTTOM: 3px; PADDING-LEFT: 3px; PADDING-RIGHT: 3px; PADDING-TOP: 3px"
										height="20" align="center"><asp:checkbox id="cbBt_sfz1" Text="���ϴ��Ƹ�ͨע�����֤������Ƭ" Runat="server"></asp:checkbox></td>
									<td style="PADDING-BOTTOM: 3px; PADDING-LEFT: 3px; PADDING-RIGHT: 3px; PADDING-TOP: 3px"
										height="20" align="center"><asp:checkbox id="cbBt_sfz2" Text="���ϴ��Ƹ�ͨע�����֤������Ƭ" Runat="server"></asp:checkbox></td>
									<td style="PADDING-BOTTOM: 3px; PADDING-LEFT: 3px; PADDING-RIGHT: 3px; PADDING-TOP: 3px"
										height="20" align="center"><asp:checkbox id="cbBt_yhk1" Text="���ϴ����п���Ƭ��ɨ���" Runat="server"></asp:checkbox></td>
                                    <td style="PADDING-BOTTOM: 3px; PADDING-LEFT: 3px; PADDING-RIGHT: 3px; PADDING-TOP: 3px" height="20" align="center">
                                       <table width="100%">
                                         <tr>
                                             <td align="left"><asp:checkbox id="cbBt_zjly1" Text="���ṩ�������п۷���ϸ�����������š�ʱ�䡢�����׶Է�Ϊ�Ƹ�ͨ�Ľ�ͼ" Runat="server"></asp:checkbox></td>
                                         </tr>
                                         <tr>
                                             <td align="left"><asp:checkbox id="cbBt_zjly2" Text="���ṩ�������п۷���ϸ�������������п��š�ʱ�䡢�����׶Է�Ϊ�Ƹ�ͨ�Ľ�ͼ" Runat="server"></asp:checkbox></td>
                                         </tr>
                                         <tr>
                                             <td align="left"><asp:checkbox id="cbBt_zjly3" Text="���ṩ���ת�����ڵ��Ե�IP��ַ��ͼ����ϵ�绰" Runat="server"></asp:checkbox></td>
                                         </tr>
                                         <tr>
                                             <td align="left"><asp:checkbox id="cbBt_zjly4" Text="���ṩ�������������ɨ�������Ƭ" Runat="server"></asp:checkbox></td>
                                         </tr>
                                         <tr>
                                             <td align="left"><asp:checkbox id="cbBt_zjly5" Text="���ṩ�ʻ���ֵ�ɹ���ƾ��ɨ�������Ƭ" Runat="server"></asp:checkbox></td>
                                         </tr>
                                         <tr>
                                             <td align="left"><asp:checkbox id="cbBt_zjly6" Text="���ṩ�����̳���ַ�����ּ�¼��ͼ" Runat="server"></asp:checkbox></td>
                                         </tr>
                                         <tr>
                                             <td align="left"><asp:checkbox id="cbBt_zjly7" Text="���ṩ�̳ǽ��׶����е�֧����¼��ͼ" Runat="server"></asp:checkbox></td>
                                         </tr>
                                       </table>
                                    </td>								
                                </tr>
							</table>
						</p>
					</td>
				</tr>
			</table>
			<table border="1" cellSpacing="1" cellPadding="1" width="1200" runat="server">
				<tr>
					<td bgColor="#eeeeee" height="20" width="50%"><asp:checkbox id="cbBt_yhms1" Text="�û�����" Runat="server"></asp:checkbox></td>
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
				<tr id="FreezeOperateTR" runat="server">
					<td bgColor="#ffffff" colSpan="2" align="center"><span style="MARGIN: 0px 30px 0px 0px"><asp:button id="btn_hangUp" runat="server" Width="100" Text="�� ��" onclick="btn_hangUp_Click"></asp:button></span><span style="MARGIN: 0px 30px 0px 0px"><asp:button id="btn_Finish1" runat="server" Width="100" Text="�ᵥ���ѽⶳ��" onclick="btn_Finish1_Click"></asp:button></span><span style="MARGIN: 0px 30px 0px 0px"><asp:button id="btn_Finish2" runat="server" Width="100" Text="��������" onclick="btn_Finish2_Click"></asp:button></span><span style="MARGIN: 0px 30px 0px 0px"><asp:button id="btn_Del" runat="server" Width="100" Text="�� ��" onclick="btn_Del_Click"></asp:button></span><span style="MARGIN: 0px 30px 0px 0px"><asp:button id="btnSynCredit" runat="server" Width="100" Text="ͬ�����֤��" onclick="btn_synCreid_Click"></asp:button></span></td>
				</tr>
                <tr id="SpecialOperateTR" runat="server">
					<td bgColor="#ffffff" colSpan="2" align="center"><span style="MARGIN: 0px 30px 0px 0px">
                        <asp:button id="btn_OK" runat="server" Width="100" Text="ͨ��" onclick="btn_OK_Click"></asp:button></span><span style="MARGIN: 0px 30px 0px 0px">
                        <asp:button id="btn_Cancel" runat="server" Width="100" Text="�ܾ�" onclick="btn_Cancel_Click"></asp:button></span><span style="MARGIN: 0px 30px 0px 0px">
                        <asp:button id="btn_Delete" runat="server" Width="100" Text="ɾ��" onclick="btn_Delete_Click"></asp:button></span><span style="MARGIN: 0px 30px 0px 0px">
                       <asp:button id="btn_Complement" runat="server" Width="100" Text="��������" onclick="btn_Complement_Click"></asp:button></span>
					</td>
				</tr>
              
			</table>
            <table border="1" cellSpacing="1" cellPadding="1" width="1200">
				<tr>
					<td bgColor="#eeeeee" height="20" width="20%"><asp:checkbox id="cbBt_bcqtzjzp" Text="" Runat="server"></asp:checkbox><asp:TextBox ID="tbx_bcqtzjzp_zdy" Text="��������֤����Ƭ" Runat="server" Width="140px"></asp:TextBox></td>
					<td bgColor="#eeeeee" height="20" width="20%"><asp:checkbox id="cbBt_bcsfzsczp" Text="" Runat="server"></asp:checkbox><asp:TextBox ID="tbx_bcsfzsczp_zdy" Text="������ֳ����֤������" Runat="server" Width="140px"></asp:TextBox></td>
                    <td bgColor="#eeeeee" height="20" width="20%"><asp:checkbox id="cbBt_bchjzmzp" Text="" Runat="server"></asp:checkbox><asp:TextBox ID="tbx_bchjzmzp_zdy" Text="���仧��֤����Ƭ" Runat="server" Width="140px"></asp:TextBox></td>
                    <td bgColor="#eeeeee" height="20" width="20%"><asp:checkbox id="cbBt_bcjljtzp" Text="" Runat="server"></asp:checkbox><asp:TextBox ID="tbx_bcjljtzp_zdy" Text="�������Ͻ�ͼ" Runat="server" Width="140px"></asp:TextBox></td>
				</tr>
				<tr>
					<td>
                      <asp:checkbox id="cbBt_qtzp1" name="cb_qtzp" Text="���ϴ�����������֤���֤����Ƭ��ɨ���" Runat="server"></asp:checkbox><br />
                      <asp:checkbox id="cbBt_qtzp2" name="cb_qtzp" Text="���ϴ����׶Է����֤����Ƭ��ɨ���" Runat="server"></asp:checkbox><br />
                      <asp:checkbox id="cbBt_qtzp3" name="cb_qtzp" Text="���ϴ�ת�˷����֤����Ƭ��ɨ���" Runat="server"></asp:checkbox><br />
                      <asp:checkbox id="cbBt_qtzp_zdy" Text="" Runat="server"></asp:checkbox><asp:TextBox ID="tbx_qtzp_zdy" Runat="server" Width="140px"></asp:TextBox><br />
                    </td>
                    <td>
                      <asp:checkbox id="cbBt_scbs1" Text="���ϴ��������ֳ����֤���ϰ�����Ƭ" Runat="server"></asp:checkbox><br />
                      <asp:checkbox id="cbBt_scbs2" Text="���ϴ��������ֳ����֤�͵��챨ֽ���ϰ�����Ƭ" Runat="server"></asp:checkbox><br />
                      <asp:checkbox id="cbBt_scbs_zdy" Text="" Runat="server"></asp:checkbox><asp:TextBox ID="tbx_scbs_zdy" Runat="server" Width="140px"></asp:TextBox><br />
                    </td>
                    <td>
                      <asp:checkbox id="cbBt_hjzm1" Text="���ϴ�����֤����Ƭ" Runat="server"></asp:checkbox><br />
                      <asp:checkbox id="cbBt_hjzm2" Text="���ϴ����ڱ���ҳ�ͱ�����Ϣҳ����Ƭ��ɨ���" Runat="server"></asp:checkbox><br />
                      <asp:checkbox id="cbBt_hjzm_zdy" Text="" Runat="server"></asp:checkbox><asp:TextBox ID="tbx_hjzm_zdy" Runat="server" Width="140px"></asp:TextBox><br />
                    </td>
                    <td>
                      <asp:checkbox id="cbBt_bczl1" Text="���ϴ����ѳ�ֵ����Ƭ" Runat="server"></asp:checkbox><br />
                      <asp:checkbox id="cbBt_bczl2" Text="���ϴ���Ч�����������¼��ͼ" Runat="server"></asp:checkbox><br />
                      <asp:checkbox id="cbBt_bczl_zdy" Text="" Runat="server"></asp:checkbox><asp:TextBox ID="tbx_bczl_zdy" Runat="server" Width="140px"></asp:TextBox><br />
                    </td>
				</tr>
                <tr>
					<td>
                      <asp:image id="img_qtzp1" runat="server" Width="200" Height="150px"></asp:image>
                    </td>
                    <td>
                      <asp:image id="img_scbs1" runat="server" Width="200" Height="150px"></asp:image>
                    </td>
                    <td>
                      <asp:image id="img_hjzm1" runat="server" Width="200" Height="150px"></asp:image>
                    </td>
                    <td>
                      <asp:image id="img_zljt1" runat="server" Width="200" Height="150px"></asp:image>
                    </td>
				</tr>
				  <tr>
                    <td bgColor="#eeeeee" align="right">�ͷ�����ע��<font color="red">��ע���ֻ������80���ַ�</font></td>
					<td bgColor="#eeeeee" width="80%" colspan="3" ><asp:textbox id="tbx_comment" Runat="server" Width="600" Height="82" TextMode="MultiLine"></asp:textbox></td>
				</tr>
				
			</table>
		</form>
	</body>
</HTML>
