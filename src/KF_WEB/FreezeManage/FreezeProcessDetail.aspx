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
                    //身份证正面
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
					//身份证反面
					function cbxSfzf() {
					    var sfzf = document.getElementById("cbBt_sfzf");
					    var sfz2 = document.getElementById("cbBt_sfz2");
					    if (sfz2.checked) {
					        sfzf.checked = true;
					    } else {
					        sfzf.checked = false;
					    }
					}
		            //银行卡照片
					function cbxYhkzp() {
					    var yhkzp = document.getElementById("cbBt_yhkzp");
					    var yhk1 = document.getElementById("cbBt_yhk1");
					    if (yhk1.checked) {
					        yhkzp.checked = true;
					    } else {
					        yhkzp.checked = false;
					    }
					}
		            //资金来源截图
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
					//补充其他证件照片
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
					//补充手持身份证半身照
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
					//补充户籍证明照片
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
					//补充资料截图
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
					<TD style="WIDTH: 443px; HEIGHT: 20px" bgColor="#e4e5f7" colSpan="2"><FONT color="red" face="宋体"><IMG src="../IMAGES/Page/post.gif" width="20" height="16"><asp:label id="lb_pageTitle" Runat="server">风控解冻审核(新)</asp:label></FONT></TD>
					<td style="HEIGHT: 20px" colSpan="2"></FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="lb_operatorID" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></td>
				</TR>
				<tr>
					<td><label style="WIDTH: 110px; HEIGHT: 20px; VERTICAL-ALIGN: middle">财付通帐号：</label><asp:textbox id="tbx_payAccount" Runat="server" Width="140px" Enabled="False"></asp:textbox></td>
					<td><label style="WIDTH: 110px; HEIGHT: 20px; VERTICAL-ALIGN: middle">用户提交证件号码：</label><asp:textbox id="tbx_cerNO" Runat="server" Width="140px" Enabled="False"></asp:textbox></td>
					<td><label style="WIDTH: 110px; HEIGHT: 20px; VERTICAL-ALIGN: middle">用户提交姓名：</label><asp:textbox id="tbx_subUserName" Runat="server" Width="140px" Enabled="False"></asp:textbox></td>
				</tr>
				<tr>
					<td><label style="WIDTH: 110px; HEIGHT: 20px; VERTICAL-ALIGN: middle">账户余额：</label><asp:textbox id="tbx_restFin" Runat="server" Width="140px" Enabled="False"></asp:textbox></td>
				    <td><label style="WIDTH: 110px; HEIGHT: 20px; VERTICAL-ALIGN: middle">注册证件号码：</label><asp:textbox id="tbx_regCreNO" Runat="server" Width="140px" Enabled="False"></asp:textbox></td>
                    <td><label style="WIDTH: 110px; HEIGHT: 20px; VERTICAL-ALIGN: middle">注册姓名：</label><asp:textbox id="tbx_userName" Runat="server" Width="140px" Enabled="False"></asp:textbox></td>
                     </tr>
				<tr>
					
					<td><label style="WIDTH: 110px; HEIGHT: 20px; VERTICAL-ALIGN: middle">联系邮箱：</label><asp:textbox id="tbx_email" Runat="server" Width="140px" Enabled="False"></asp:textbox></td>
					<td><label style="WIDTH: 110px; HEIGHT: 20px; VERTICAL-ALIGN: middle">联系电话：</label><asp:textbox id="tbx_phoneNo" Runat="server" Width="140px" Enabled="False"></asp:textbox></td>
                    <td style="color:red"><label style="WIDTH: 110px; HEIGHT: 20px; VERTICAL-ALIGN: middle;">冻结原因：</label><asp:textbox id="tbx_freezeReason" Runat="server" Width="140px" Enabled="False"></asp:textbox></td>
				</tr>
                <tr runat="server" id="TR1">
					<td><label style="WIDTH: 110px; HEIGHT: 20px; VERTICAL-ALIGN: middle">免审核标准分：</label><asp:textbox id="lblstandard_score" Runat="server" Width="140px" Enabled="False"></asp:textbox></td>
					<td><label style="WIDTH: 110px; HEIGHT: 20px; VERTICAL-ALIGN: middle">风控标记：</label><asp:textbox id="lblrisk_result" Runat="server" Width="140px" Enabled="False"></asp:textbox></td>
					<td><label style="WIDTH: 110px; HEIGHT: 20px; VERTICAL-ALIGN: middle">是否清空密保资料：</label><asp:textbox id="clear_pps" Runat="server" Width="140px" Enabled="False"></asp:textbox></td>
				</tr>
                  <tr runat="server" id="TR2">
					<td><label style="WIDTH: 110px; HEIGHT: 20px; VERTICAL-ALIGN: middle">实际得分：</label><asp:textbox id="lblscore" Runat="server" Width="140px" Enabled="False"></asp:textbox></td>
					<td colspan="2"><label style="WIDTH: 110px; HEIGHT: 20px; VERTICAL-ALIGN: middle">实名认证：</label><asp:textbox id="lbauthenState" Runat="server" Width="140px" Enabled="False"></asp:textbox></td>
				</tr>
                   <tr runat="server" id="TR3">
					<td colspan="3"><label style="WIDTH: 110px; HEIGHT: 20px; VERTICAL-ALIGN: middle">得分明细：</label><asp:textbox id="lbldetail_score" Runat="server" Width="100%" Enabled="False"></asp:textbox></td>
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
									<td id="td_pic1" bgColor="#eeeeee" height="20">身份证正面照片</td>
									<td id="td_pic2" bgColor="#eeeeee">身份证反面照片</td>
									<td id="td_pic3" bgColor="#eeeeee" height="20" runat="server">银行卡照片</td>
									<td id="td_pic4" bgColor="#eeeeee" height="20" runat="server">资金来源截图</td>
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
									<td id="tdBt_pic1" bgColor="#eeeeee" height="20" width="20%" runat="server"><asp:checkbox id="cbBt_sfzz" Text="身份证正面照片" Runat="server"></asp:checkbox></td>
									<td id="tdBt_pic2" bgColor="#eeeeee" height="20" width="20%" runat="server"><asp:checkbox id="cbBt_sfzf" Text="身份证反面照片" Runat="server"></asp:checkbox></td>
									<td id="tdBt_pic3" bgColor="#eeeeee" height="20" width="20%" runat="server"><asp:checkbox id="cbBt_yhkzp" Text="银行卡照片" Runat="server"></asp:checkbox></td>
                                    <td id="tdBt_pic4" bgColor="#eeeeee" height="20" width="20%" runat="server"><asp:checkbox id="cbBt_zjlyjt" Text="资金来源截图" Runat="server"></asp:checkbox></td>
								</tr>
								<tr>
									<td style="PADDING-BOTTOM: 3px; PADDING-LEFT: 3px; PADDING-RIGHT: 3px; PADDING-TOP: 3px"
										height="20" align="center"><asp:checkbox id="cbBt_sfz1" Text="请上传财付通注册身份证正面照片" Runat="server"></asp:checkbox></td>
									<td style="PADDING-BOTTOM: 3px; PADDING-LEFT: 3px; PADDING-RIGHT: 3px; PADDING-TOP: 3px"
										height="20" align="center"><asp:checkbox id="cbBt_sfz2" Text="请上传财付通注册身份证反面照片" Runat="server"></asp:checkbox></td>
									<td style="PADDING-BOTTOM: 3px; PADDING-LEFT: 3px; PADDING-RIGHT: 3px; PADDING-TOP: 3px"
										height="20" align="center"><asp:checkbox id="cbBt_yhk1" Text="请上传银行卡照片或扫描件" Runat="server"></asp:checkbox></td>
                                    <td style="PADDING-BOTTOM: 3px; PADDING-LEFT: 3px; PADDING-RIGHT: 3px; PADDING-TOP: 3px" height="20" align="center">
                                       <table width="100%">
                                         <tr>
                                             <td align="left"><asp:checkbox id="cbBt_zjly1" Text="请提供网上银行扣费明细，包含订单号、时间、金额、交易对方为财付通的截图" Runat="server"></asp:checkbox></td>
                                         </tr>
                                         <tr>
                                             <td align="left"><asp:checkbox id="cbBt_zjly2" Text="请提供网上银行扣费明细，包含完整银行卡号、时间、金额、交易对方为财付通的截图" Runat="server"></asp:checkbox></td>
                                         </tr>
                                         <tr>
                                             <td align="left"><asp:checkbox id="cbBt_zjly3" Text="请提供付款方转账所在电脑的IP地址截图及联系电话" Runat="server"></asp:checkbox></td>
                                         </tr>
                                         <tr>
                                             <td align="left"><asp:checkbox id="cbBt_zjly4" Text="请提供发货快递物流单扫描件或照片" Runat="server"></asp:checkbox></td>
                                         </tr>
                                         <tr>
                                             <td align="left"><asp:checkbox id="cbBt_zjly5" Text="请提供帐户充值成功的凭条扫描件或照片" Runat="server"></asp:checkbox></td>
                                         </tr>
                                         <tr>
                                             <td align="left"><asp:checkbox id="cbBt_zjly6" Text="请提供带有商城网址的提现记录截图" Runat="server"></asp:checkbox></td>
                                         </tr>
                                         <tr>
                                             <td align="left"><asp:checkbox id="cbBt_zjly7" Text="请提供商城交易订单中的支付记录截图" Runat="server"></asp:checkbox></td>
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
					<td bgColor="#eeeeee" height="20" width="50%"><asp:checkbox id="cbBt_yhms1" Text="用户描述" Runat="server"></asp:checkbox></td>
					<td bgColor="#eeeeee" height="20" width="50%">客服处理结果</td>
				</tr>
				<tr>
					<td bgColor="#eeeeee" width="50%"><asp:textbox id="tbx_userQA" Runat="server" Width="400" Enabled="false" Height="82" TextMode="MultiLine"></asp:textbox></td>
					<td bgColor="#eeeeee" width="50%"><asp:textbox id="tbx_handleResult" Runat="server" Width="430" Height="82" TextMode="MultiLine"></asp:textbox><br>
						<label style="MARGIN-BOTTOM: 50px">快捷回复：</label>
						<asp:dropdownlist id="ddl_fastReply1" Runat="server" Width="350" AutoPostBack="True"></asp:dropdownlist>&nbsp;&nbsp;&nbsp;&nbsp;
						<asp:button id="btn_manageFastReply" Runat="server" Text="管理快捷回复" onclick="btn_manageFastReply_Click"></asp:button><br>
					</td>
				</tr>
				<tr>
					<td bgColor="#ffffff" height="10" colSpan="2"></td>
				</tr>
				<tr id="FreezeOperateTR" runat="server">
					<td bgColor="#ffffff" colSpan="2" align="center"><span style="MARGIN: 0px 30px 0px 0px"><asp:button id="btn_hangUp" runat="server" Width="100" Text="挂 起" onclick="btn_hangUp_Click"></asp:button></span><span style="MARGIN: 0px 30px 0px 0px"><asp:button id="btn_Finish1" runat="server" Width="100" Text="结单（已解冻）" onclick="btn_Finish1_Click"></asp:button></span><span style="MARGIN: 0px 30px 0px 0px"><asp:button id="btn_Finish2" runat="server" Width="100" Text="补充资料" onclick="btn_Finish2_Click"></asp:button></span><span style="MARGIN: 0px 30px 0px 0px"><asp:button id="btn_Del" runat="server" Width="100" Text="作 废" onclick="btn_Del_Click"></asp:button></span><span style="MARGIN: 0px 30px 0px 0px"><asp:button id="btnSynCredit" runat="server" Width="100" Text="同步身份证号" onclick="btn_synCreid_Click"></asp:button></span></td>
				</tr>
                <tr id="SpecialOperateTR" runat="server">
					<td bgColor="#ffffff" colSpan="2" align="center"><span style="MARGIN: 0px 30px 0px 0px">
                        <asp:button id="btn_OK" runat="server" Width="100" Text="通过" onclick="btn_OK_Click"></asp:button></span><span style="MARGIN: 0px 30px 0px 0px">
                        <asp:button id="btn_Cancel" runat="server" Width="100" Text="拒绝" onclick="btn_Cancel_Click"></asp:button></span><span style="MARGIN: 0px 30px 0px 0px">
                        <asp:button id="btn_Delete" runat="server" Width="100" Text="删除" onclick="btn_Delete_Click"></asp:button></span><span style="MARGIN: 0px 30px 0px 0px">
                       <asp:button id="btn_Complement" runat="server" Width="100" Text="补充资料" onclick="btn_Complement_Click"></asp:button></span>
					</td>
				</tr>
              
			</table>
            <table border="1" cellSpacing="1" cellPadding="1" width="1200">
				<tr>
					<td bgColor="#eeeeee" height="20" width="20%"><asp:checkbox id="cbBt_bcqtzjzp" Text="" Runat="server"></asp:checkbox><asp:TextBox ID="tbx_bcqtzjzp_zdy" Text="补充其他证件照片" Runat="server" Width="140px"></asp:TextBox></td>
					<td bgColor="#eeeeee" height="20" width="20%"><asp:checkbox id="cbBt_bcsfzsczp" Text="" Runat="server"></asp:checkbox><asp:TextBox ID="tbx_bcsfzsczp_zdy" Text="补充的手持身份证半身照" Runat="server" Width="140px"></asp:TextBox></td>
                    <td bgColor="#eeeeee" height="20" width="20%"><asp:checkbox id="cbBt_bchjzmzp" Text="" Runat="server"></asp:checkbox><asp:TextBox ID="tbx_bchjzmzp_zdy" Text="补充户籍证明照片" Runat="server" Width="140px"></asp:TextBox></td>
                    <td bgColor="#eeeeee" height="20" width="20%"><asp:checkbox id="cbBt_bcjljtzp" Text="" Runat="server"></asp:checkbox><asp:TextBox ID="tbx_bcjljtzp_zdy" Text="补充资料截图" Runat="server" Width="140px"></asp:TextBox></td>
				</tr>
				<tr>
					<td>
                      <asp:checkbox id="cbBt_qtzp1" name="cb_qtzp" Text="请上传拍拍卖家认证身份证的照片或扫描件" Runat="server"></asp:checkbox><br />
                      <asp:checkbox id="cbBt_qtzp2" name="cb_qtzp" Text="请上传交易对方身份证的照片或扫描件" Runat="server"></asp:checkbox><br />
                      <asp:checkbox id="cbBt_qtzp3" name="cb_qtzp" Text="请上传转账方身份证的照片或扫描件" Runat="server"></asp:checkbox><br />
                      <asp:checkbox id="cbBt_qtzp_zdy" Text="" Runat="server"></asp:checkbox><asp:TextBox ID="tbx_qtzp_zdy" Runat="server" Width="140px"></asp:TextBox><br />
                    </td>
                    <td>
                      <asp:checkbox id="cbBt_scbs1" Text="请上传您本人手持身份证的上半身照片" Runat="server"></asp:checkbox><br />
                      <asp:checkbox id="cbBt_scbs2" Text="请上传您本人手持身份证和当天报纸的上半身照片" Runat="server"></asp:checkbox><br />
                      <asp:checkbox id="cbBt_scbs_zdy" Text="" Runat="server"></asp:checkbox><asp:TextBox ID="tbx_scbs_zdy" Runat="server" Width="140px"></asp:TextBox><br />
                    </td>
                    <td>
                      <asp:checkbox id="cbBt_hjzm1" Text="请上传户籍证明照片" Runat="server"></asp:checkbox><br />
                      <asp:checkbox id="cbBt_hjzm2" Text="请上传户口本首页和本人信息页的照片或扫描件" Runat="server"></asp:checkbox><br />
                      <asp:checkbox id="cbBt_hjzm_zdy" Text="" Runat="server"></asp:checkbox><asp:TextBox ID="tbx_hjzm_zdy" Runat="server" Width="140px"></asp:TextBox><br />
                    </td>
                    <td>
                      <asp:checkbox id="cbBt_bczl1" Text="请上传话费充值卡照片" Runat="server"></asp:checkbox><br />
                      <asp:checkbox id="cbBt_bczl2" Text="请上传有效完整的聊天记录截图" Runat="server"></asp:checkbox><br />
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
                    <td bgColor="#eeeeee" align="right">客服处理备注：<font color="red">备注最多只能输入80个字符</font></td>
					<td bgColor="#eeeeee" width="80%" colspan="3" ><asp:textbox id="tbx_comment" Runat="server" Width="600" Height="82" TextMode="MultiLine"></asp:textbox></td>
				</tr>
				
			</table>
		</form>
	</body>
</HTML>
