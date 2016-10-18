<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KFWebTest.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.KFWebTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:GridView ID="GridView1" runat="server">
        </asp:GridView>
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
    
    </div>
        <div>
        <asp:Label ID="Label2" runat="server" Text="修改个人信息，qq账户属性："></asp:Label>
        <asp:FileUpload ID="fileqqid" runat="server" />
        <asp:Button ID="Button1" runat="server" Text="执行22" OnClick="Button1_Click" />
            </div>
    </form>
</body>
</html>
