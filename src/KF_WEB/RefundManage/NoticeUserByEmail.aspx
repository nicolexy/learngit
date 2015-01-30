<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NoticeUserByEmail.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.RefundManage.NoticeUserByEmail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            height: 38px;
        }
        BODY { BACKGROUND-IMAGE: url(../IMAGES/Page/bg01.gif) }
    </style>
    
</head>
<body>
    <form id="form1" runat="server">
    <div>   
        <table height="108" cellSpacing="1" cellPadding="0" width="860" align="center" 
	    border="0">
	    <tr background="../IMAGES/Page/bg_bl.gif">
		    <td style="HEIGHT: 16px" vAlign="middle" colSpan="3" height="16">
			    <table height="25" cellSpacing="0" cellPadding="1" width="859" border="0">
				    <tr>
					    <td style="HEIGHT: 20px" width="60%" background="../IMAGES/Page/bg_bl.gif"><font color="#ff0000"><STRONG>&nbsp;</STRONG><IMG height="16" src="../IMAGES/Page/post.gif" width="20">
							    通知用户</font>
					    </td>
					    <td style="HEIGHT: 20px" width="40%" background="../IMAGES/Page/bg_bl.gif">操作员代码: <span class="style5">
							    <asp:label id="operatorID" runat="server">Label</asp:label></span></td>
				    </tr>
			    </table>
		    </td>
	    </tr>

        <tr bgColor="#eeeeee">
            <td ><FONT face="宋体">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 邮箱地址：</FONT></td>				
            <td><asp:TextBox ID="txtEmailID" runat="server" width="200px" height="20" BorderWidth="2px" ></asp:TextBox><FONT face="宋体" color="red">&nbsp;*</FONT>
                <asp:RequiredFieldValidator ID="EmailValidator" runat="server" ErrorMessage="不能为空" ControlToValidate="txtEmailID" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="EmailExpressionValidator" runat="server" ErrorMessage="邮箱格式不正确！" ControlToValidate="txtEmailID"  ValidationExpression ="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
            </td>
            </tr>
            <tr align ="right">
                <td   colspan = "2"  rowspan = "3" align = "right"><asp:Button ID="btnNotice" runat="server" Text="确认通知" Width="100px"  Height="35px" OnClick="BtnNoticeUser_Click" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<td>
            </tr>  

        </table>
       </div>       
    </form>
</body>
</html>
