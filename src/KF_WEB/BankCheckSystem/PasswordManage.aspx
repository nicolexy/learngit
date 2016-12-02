<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PasswordManage.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BankCheckSystem.PasswordManage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    	<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> ); UNKNOWN { COLOR: #000000 }
	.style3 { COLOR: #ff0000 }
	BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
		</style>
    <style>
        @import url( ../STYLES/style.css?);
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div style="margin-top:10px;margin-left:10px">
            <table class="tb_detail" width="90%" cellspacing="1" cellpadding="1">
                <tr>
                   <TD style=" background-color:#e4e5f7"   colspan="4"><FONT face="宋体"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">密码管理</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></TD>
                </tr>
                <tr>
                    <td style="text-align:right;width:120px">登陆账号：</td>
                    <td>
                       <asp:TextBox ID="txt_Fuser_login_account" runat="server"></asp:TextBox>
                    </td>
                     <td style="text-align:right;width:120px">
                       身份证后五位：
                    </td>
                    <td>
                       <asp:TextBox ID="txt_Fuser_id_no" runat="server"></asp:TextBox>
                        <asp:Button ID="btncheck" runat="server" Text="检 验" OnClick="btncheck_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="text-align:right">
                         <asp:Button ID="btnSerach" runat="server" Text="查 询" OnClick="btnSerach_Click" Visible="false" />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
                
            </table>

            <br /><br />
             <table runat="server" id="tb_detail" visible="false" class="tb_detail" width="90%" cellspacing="1" cellpadding="1">
                 <tr><td style="text-align:right;width:20%">发送邮箱：</td>
                     <td colspan="3">
                        <asp:TextBox ID="txt_email" runat="server"></asp:TextBox>
                     </td>
                 </tr>
                <tr>
                  <%--  <td style="text-align:right;">密码管理：</td>
                    <td style="width:30%">
                        <asp:LinkButton ID="lblreset" runat="server" OnClick="lblreset_Click">重置密码</asp:LinkButton>
                    </td>--%>
                     <td style="text-align:right;width:20%">
                         银行类型：
                    </td>
                    <td>
                        <asp:Label ID="lbl_Fbank_id" runat="server"></asp:Label>
                    </td>
                    <td></td><td></td>
                </tr>
                  <tr>
                    <td style="text-align:right;">联系人：</td>
                    <td>
                      <asp:Label ID="lbl_Fcontact_name" runat="server"></asp:Label>
                    </td>
                     <td style="text-align:right;">
                         最后一次修改密码时间：
                    </td>
                    <td>
                        <asp:Label ID="lbl_Flast_update_date" runat="server"></asp:Label>
                    </td>
                </tr>
                 </table>
        </div>
        
        
        
    </form>
</body>
</html>
