<%@ Page language="c#" Codebehind="MobileRechargeConfig.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.TradeManage.MobileRechargeConfig" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>MobileRechargeConfig</title>
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
					function modify(cardType, cardValue, title)
					{
						var div = document.getElementById("modifyDiv");
						div.style.display = "block";
						div.src = "MobileRechargeConfigEdit.aspx?cardtype="+cardType+"&cardvalue="+cardValue+"&title="+title;
					}
					function cancel()
					{
						var div = document.getElementById("modifyDiv");
						div.style.display = "none";
					}
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form style="FONT-FAMILY: 宋体" id="Form1" method="post" runat="server">
			<iframe style="BORDER-BOTTOM: #aebbca 2px solid; POSITION: absolute; BORDER-LEFT: #aebbca 2px solid; WIDTH: 400px; DISPLAY: none; HEIGHT: 300px; BORDER-TOP: #aebbca 2px solid; TOP: 25%; BORDER-RIGHT: #aebbca 2px solid; LEFT: 25%; zindex: 999"
				id="modifyDiv" runat="server" src=""></iframe>
			<table style="MARGIN-TOP: 3%; MARGIN-LEFT: 3%" id="contentTable" border="1" cellSpacing="1"
				cellPadding="1" width="900" runat="server">
				<TBODY>
					<tr style="BACKGROUND-COLOR: #e4e5f7">
						<td colSpan="4"><FONT color="red"><IMG src="../IMAGES/Page/post.gif" width="20" height="16">&nbsp;&nbsp;分流配置</FONT>
						</td>
					</tr>
					<tr>
						<td colSpan="4">产品配置
						</td>
					</tr>
					<tr>
						<th style="WIDTH:50px">
							运营商</th>
						<th style="WIDTH:50px">
							面值</th>
						<th>
							当前方案</th>
						<th style="WIDTH:50px">
							操作</th>
					</tr>
				</TBODY>
			</table>
		</form>
	</body>
</HTML>
