<%@ Page language="c#" Codebehind="MobileRechargeConfigValidDateEdit.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.MobileRechargeConfigValidDateEdit" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>MobileRechargeConfigValidDateEdit</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="VBScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		<script src="../SCRIPTS/Local.js"></script>
		<script language="javascript">
					function openModeBegin()
					{
						var tbx_beginDate = document.getElementById("tbx_beginDate");
						var returnValue=window.showModalDialog("../Control/CalendarForm2.aspx",tbx_beginDate.value,'dialogWidth:375px;DialogHeight=260px;status:no');
						if(returnValue != null) tbx_beginDate.value=returnValue;
					}
					function openModeEnd()
					{
						var tbx_endDate = document.getElementById("tbx_endDate");
						var returnValue=window.showModalDialog("../Control/CalendarForm2.aspx",tbx_endDate.value,'dialogWidth:375px;DialogHeight=260px;status:no');
						if(returnValue != null) tbx_endDate.value=returnValue;
					}
					function cancel()
					{
						window.parent.cancel();
					}
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout" style="BACKGROUND: #fff;">
		<form id="Form1" method="post" runat="server" style="BACKGROUND: #fff; width:100%;">
			<div style="WIDTH: 392px; BACKGROUND: #a9e2f3; HEIGHT: 45px; PADDING-TOP: 15px">
				<span style="MARGIN-LEFT: 10px">商户有效期设置 </span>
			</div>
			<div style="PADDING-LEFT: 80px; PADDING-TOP: 15px">
				<span>生效时间: </span>
				<asp:TextBox Runat="server" ID="tbx_beginDate" Width="120" ReadOnly="True"></asp:TextBox>
				<asp:imagebutton id="btnBeginDate" runat="server" ImageUrl="../Images/Public/edit.gif" CausesValidation="False"></asp:imagebutton>
			</div>
			<div style="PADDING-LEFT: 80px; PADDING-TOP: 5px">
				<span>失效时间: </span>
				<asp:TextBox Runat="server" ID="tbx_endDate" Width="120" ReadOnly="True"></asp:TextBox>
				<asp:imagebutton id="btnEndDate" runat="server" ImageUrl="../Images/Public/edit.gif" CausesValidation="False"></asp:imagebutton>
			</div>
			<div style="PADDING-LEFT: 100px; PADDING-TOP: 25px">
				<input type="button" runat="server" id="modifyBtn" style="WIDTH: 80px" value="提交" onserverclick="modifyBtn_Click" /></asp:button><input style="WIDTH: 80px" onclick="cancel();" value="取消" type="button">
			</div>
		</form>
	</body>
</HTML>
