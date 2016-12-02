<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BankCheckUsersAdd.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BankCheckSystem.BankCheckUsersAdd" %>

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
                   <TD style=" background-color:#e4e5f7"  colspan="2"><FONT face="宋体"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">登录名新增</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></TD>
                </tr>
                  <tr>
                    <td style="text-align:right">绑定邮箱：</td>
                    <td>
                        <asp:TextBox runat="server" ID="txt_Fuser_bind_email" 
                            onchange="document.getElementById('txt_Fuser_login_account').value=document.getElementById('txt_Fuser_bind_email').value"></asp:TextBox>
                         <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*"
                              ControlToValidate="txt_Fuser_bind_email"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                             ValidationExpression="([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,5})+" 
                             ControlToValidate="txt_Fuser_bind_email" 
                             ErrorMessage="请输入正确的邮箱地址！"></asp:RegularExpressionValidator>
                    </td>
                </tr>
              
                <tr>
                    <td style="text-align:right">银行类型：</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddl_Fbank_id" Width="134px"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align:right">银行名称：</td>
                    <td>
                        <asp:TextBox runat="server" ID="txt_Bankname"></asp:TextBox>
                          <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*"
                              ControlToValidate="txt_Bankname"></asp:RequiredFieldValidator>
                    </td>
                </tr>
              
                <tr>
                    <td style="text-align:right">申请人姓名：</td>
                    <td>
                        <asp:TextBox runat="server" ID="txt_Fuser_name"></asp:TextBox>
                          <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="*"
                              ControlToValidate="txt_Fuser_name"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="text-align:right">申请人证件号：</td>
                    <td>
                        <asp:TextBox runat="server" ID="txt_Fuser_id_no"></asp:TextBox>
                          <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="*"
                              ControlToValidate="txt_Fuser_id_no"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="text-align:right">联系地址：</td>
                    <td>
                        <asp:TextBox runat="server" ID="txt_Fcontact_address"></asp:TextBox>
                          <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="*"
                              ControlToValidate="txt_Fcontact_address"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="text-align:right">联系人姓名：</td>
                    <td>
                        <asp:TextBox runat="server" ID="txt_Fcontact_name"></asp:TextBox>
                          <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="*"
                              ControlToValidate="txt_Fcontact_name"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="text-align:right">联系人电话：</td>
                    <td>
                        <asp:TextBox runat="server" ID="txt_Fcontact_tel"></asp:TextBox>
                          <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="*"
                              ControlToValidate="txt_Fcontact_tel"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="text-align:right">联系人手机：</td>
                    <td>
                        <asp:TextBox runat="server" ID="txt_Fcontact_mobile"></asp:TextBox>
                          <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="*"
                              ControlToValidate="txt_Fcontact_mobile"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="text-align:right">身份证件：</td>
                    <td><input id="up_Fuser_id_no_url" style="WIDTH:300px; HEIGHT: 21px" type="file" size="21" name="up_Fuser_id_no_url" runat="server">
                    </td>
                </tr>
                <tr>
                    <td style="text-align:right">联系QQ：</td>
                    <td>
                        <asp:TextBox runat="server" ID="txt_Fcontact_qq"></asp:TextBox>
                          <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="*"
                              ControlToValidate="txt_Fcontact_qq"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="text-align:right">备  注：</td>
                    <td>
                        <asp:TextBox runat="server" ID="txt_Fremark"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align:right">权限选择：</td>
                    <td>
                        <asp:CheckBoxList ID="radio_Fauth_level"  runat="server"  RepeatDirection="Horizontal" >
                        </asp:CheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align:center"><asp:Button ID="Button1" runat="server" Text="确认提交" OnClick="Button1_Click"  CausesValidation=""/></td>
                </tr>
            </table>
        </div>
        
        
        
    </form>
</body>
</html>
