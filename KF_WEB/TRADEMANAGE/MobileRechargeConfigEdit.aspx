<%@ Page language="c#" Codebehind="MobileRechargeConfigEdit.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.MobileRechargeConfigEdit" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>MobileRechargeConfigEdit</title>
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
					function cancel()
					{
						window.parent.cancel();
					}
		</script>
	</HEAD>
	<body style="BACKGROUND: #fff" MS_POSITIONING="GridLayout">
		<form style="WIDTH: 100%; BACKGROUND: #fff" id="Form1" method="post" runat="server">
			<div style="WIDTH: 392px; BACKGROUND: #a9e2f3; HEIGHT: 45px; PADDING-TOP: 15px"><span style="MARGIN-LEFT: 20px" id="title" runat="server"></span><span style="MARGIN-LEFT: 10px">流量分配方案
				</span>
			</div>
			<table style="PADDING-LEFT: 80px; PADDING-TOP: 15px" id="contentTable" runat="server">
			</table>
			<div style="PADDING-LEFT: 100px; PADDING-TOP: 25px"><input style="WIDTH: 80px" id="modifyBtn" value="提交" type="button" name="modifyBtn" runat="server"
					onserverclick="modifyBtn_Click"></ASP:BUTTON><input style="WIDTH: 80px" onclick="cancel();" value="取消" type="button">
			</div>
		</form>
	</body>
</HTML>
