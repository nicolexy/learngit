<%@ Import Namespace="TENCENT.OSS.CFT.KF.KF_Web.classLibrary" %>
<%@ Page language="c#" Codebehind="ModifyContactInfo.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.ModifyContactInfo" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>ModifyContactInfo</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
		<script src="../SCRIPTS/Local.js"></script>
		<script language="javascript">
		    function ShowBankCard() {
		        var txtBankCard = document.getElementById('txtBankCard');
		        var btnBankCardCheck = document.getElementById('btnBankCardCheck');
		        txtBankCard.disabled = false;
		        btnBankCardCheck.disabled = false;
		    }

		    function HideBankCard() {
		        var txtBankCard = document.getElementById('txtBankCard');
		        var btnBankCardCheck = document.getElementById('btnBankCardCheck');
		        txtBankCard.disabled = true;
		        btnBankCardCheck.disabled = true;
		    }
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE style="LEFT: 5%; POSITION:relative; TOP: 5%" cellSpacing="1" cellPadding="1" width="900"
				border="1">
				<TR>
					<TD style="WIDTH: 100%" bgColor="#e4e5f7" colSpan="5"><FONT face="宋体"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">&nbsp;&nbsp;商户资料修改</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" Width="73px" ForeColor="Red"></asp:label></SPAN></TD>
				</TR>
				<TR>
					<TD align="right"><asp:label id="Label2" runat="server">商户号</asp:label></TD>
					<TD><asp:textbox id="txtSpid" runat="server"></asp:textbox></TD>
                    <TD align="center" colSpan="2"><FONT face="宋体"><asp:button id="btnSearch" runat="server" Width="80px" Text="查 询" onclick="btnSearch_Click"></asp:button></FONT></TD>
				</TR>
			</TABLE>
			<TABLE id="Table1" style="LEFT: 5%; POSITION: relative;top:50px;" cellSpacing="1" cellPadding="1" border="1"
				width="900px" runat="server">
				<TR height="35px">
					<TD align="right" width="150"><asp:label id="Label12" runat="server">联系人姓名</asp:label></TD>
					<TD width="300"><asp:textbox id="name1" runat="server" Width="200px"></asp:textbox></TD>
					<TD align="right" width="150"><asp:label id="Label14" runat="server">退款失败Email</asp:label></TD>
					<TD width="300"><asp:textbox id="standbya1" runat="server" Width="200px"></asp:textbox></TD>
				</TR>
              <TR height="35px">
					<TD align="right" width="150"><asp:label id="Label3" runat="server">联系人电话</asp:label></TD>
					<TD width="300"><asp:textbox id="tele1" runat="server" Width="200px"></asp:textbox></TD>
					<TD align="right" width="150"><asp:label id="Label4" runat="server">联系人手机</asp:label></TD>
					<TD width="300"><asp:textbox id="mobile1" runat="server" Width="200px"></asp:textbox></TD>
				</TR>
               <TR height="35px">
					<TD align="right" width="150"><asp:label id="Label9" runat="server">业务联系人姓名</asp:label></TD>
					<TD width="300"><asp:textbox id="name2" runat="server" Width="200px"></asp:textbox></TD>
					<TD align="right" width="150" ><asp:label id="Label10" runat="server">业务联系电话</asp:label></TD>
					<TD width="300"><asp:textbox id="tele2" runat="server"  Width="200px"></asp:textbox></TD>
				</TR>

               <TR height="35px">
					<TD align="right" width="150"><asp:label id="Label17" runat="server">业务联系QQ号</asp:label></TD>
					<TD width="300"><asp:textbox id="qqnum2" runat="server" Width="200px"></asp:textbox></TD>
					<TD align="right" width="150" ><asp:label id="Label18" runat="server">业务联系Email</asp:label></TD>
					<TD width="300"><asp:textbox id="email2" runat="server"  Width="200px"></asp:textbox></TD>
				</TR>

               <TR height="35px">
					<TD align="right" width="150"><asp:label id="Label23" runat="server">技术联系人姓名</asp:label></TD>
					<TD width="300"><asp:textbox id="name3" runat="server" Width="200px"></asp:textbox></TD>
					<TD align="right" width="150" ><asp:label id="Label24" runat="server">技术联系电话</asp:label></TD>
					<TD width="300"><asp:textbox id="tele3" runat="server"  Width="200px"></asp:textbox></TD>
				</TR>
              
               <TR height="35px">
					<TD align="right" width="150"><asp:label id="Label29" runat="server">技术联系QQ号</asp:label></TD>
					<TD width="300"><asp:textbox id="qqnum3" runat="server" Width="200px"></asp:textbox></TD>
					<TD align="right" width="150" ><asp:label id="Label30" runat="server">技术联系Email</asp:label></TD>
					<TD width="300"><asp:textbox id="email3" runat="server" Width="200px" ></asp:textbox></TD>
				</TR>
             
               <TR height="35px">
					<TD align="right" width="150"><asp:label id="Label35" runat="server">结算联系人姓名</asp:label></TD>
					<TD width="300"><asp:textbox id="name4" runat="server" Width="200px"></asp:textbox></TD>
					<TD align="right" width="150" ><asp:label id="Label36" runat="server">结算联系电话</asp:label></TD>
					<TD width="300"><asp:textbox id="tele4" runat="server"  Width="200px"></asp:textbox></TD>
				</TR>
              
               <TR height="35px">
					<TD align="right" width="150"><asp:label id="Label41" runat="server">结算联系QQ号<</asp:label></TD>
					<TD width="300"><asp:textbox id="qqnum4" runat="server" Width="200px"></asp:textbox></TD>
					<TD align="right" width="150" ><asp:label id="Label42" runat="server">结算联系Email</asp:label></TD>
					<TD width="300"><asp:textbox id="email4" runat="server"  Width="200px"></asp:textbox></TD>
				</TR>
              
               <TR height="35px">
					<TD align="right" width="150"><asp:label id="Label47" runat="server">客服联系人姓名</asp:label></TD>
					<TD width="300"><asp:textbox id="name5" runat="server" Width="200px"></asp:textbox></TD>
					<TD align="right" width="150" ><asp:label id="Label48" runat="server">客服联系电话</asp:label></TD>
					<TD width="300"><asp:textbox id="tele5" runat="server"  Width="200px"></asp:textbox></TD>
				</TR>
             
               <TR height="35px">
					<TD align="right" width="150"><asp:label id="Label53" runat="server">客服联系QQ号</asp:label></TD>
					<TD width="300"><asp:textbox id="qqnum5" runat="server" Width="200px"></asp:textbox></TD>
					<TD align="right" width="150" ><asp:label id="Label54" runat="server">客服联系Email</asp:label></TD>
					<TD width="300"><asp:textbox id="email5" runat="server"  Width="200px"></asp:textbox></TD>
				</TR>
              
               <TR height="35px">
					<TD align="right" width="150"><asp:label id="Label59" runat="server">运营联系人姓名</asp:label></TD>
					<TD width="300"><asp:textbox id="name6" runat="server" Width="200px"></asp:textbox></TD>
					<TD align="right" width="150" ><asp:label id="Label60" runat="server">运营联系电话</asp:label></TD>
					<TD width="300"><asp:textbox id="tele6" runat="server"  Width="200px"></asp:textbox></TD>
				</TR>
              
               <TR height="35px">
					<TD align="right" width="150"><asp:label id="Label65" runat="server">运营联系QQ号</asp:label></TD>
					<TD width="300"><asp:textbox id="qqnum6" runat="server" Width="200px"></asp:textbox></TD>
					<TD align="right" width="150" ><asp:label id="Label66" runat="server">运营联系Email</asp:label></TD>
					<TD width="300"><asp:textbox id="email6" runat="server" Width="200px" ></asp:textbox></TD>
				</TR>
             
               <TR height="35px">
					<TD align="right" width="150"><asp:label id="Label71" runat="server">风控联系人姓名</asp:label></TD>
					<TD width="300"><asp:textbox id="name7" runat="server" Width="200px"></asp:textbox></TD>
					<TD align="right" width="150" ><asp:label id="Label72" runat="server">风控联系电话</asp:label></TD>
					<TD width="300"><asp:textbox id="tele7" runat="server"  Width="200px"></asp:textbox></TD>
				</TR>
            
               <TR height="35px">
					<TD align="right" width="150"><asp:label id="Label77" runat="server">风控联系QQ号</asp:label></TD>
					<TD width="300"><asp:textbox id="qqnum7" runat="server" Width="200px"></asp:textbox></TD>
					<TD align="right" width="150" ><asp:label id="Label78" runat="server">风控联系Email</asp:label></TD>
					<TD width="300"><asp:textbox id="email7" runat="server"  Width="200px"></asp:textbox></TD>
				</TR>
               <TR height="80px">
					<td colspan="4" align="center"> 
                    <asp:button id="btnSave" runat="server" Width="80px" Text="保   存" onclick="btnSave_Click"></asp:button>
                    </td>
				</TR>
			</TABLE>
           
		</form>
	</body>
</HTML>
