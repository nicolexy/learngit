<%@ Page Language="c#" CodeBehind="FastReplyManagePage.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.FreezeManage.FastReplyManagePage" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>FastReplyManagePage</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <style type="text/css">
        @import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> );
    </style>
</head>
<body ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
        <div>
            <table width="1100">
                <tr>
                    <td width="50%">
                        <label>财付通客服系统快捷回复管理面板</label></td>
                    <td width="50%">
                        <label>操作人员：</label><asp:Label ID="lb_operatorID" runat="server"></asp:Label></td>
                </tr>
            </table>
            <div>
                <p>
                    <label>快捷回复面板：</label>
                    <asp:DropDownList ID="ddl_fastReplyBlock" runat="server" AutoPostBack="True">
                        <asp:ListItem Value="0" Selected="True">风控冻结审核</asp:ListItem>
                    </asp:DropDownList>
                </p>
            </div>
            <div>
                <p>
                    <label>快捷回复内容列表：</label>
                    <asp:DropDownList runat="server" ID="ddl_fastReplyContent">
                    </asp:DropDownList>
                </p>
            </div>
            <div>
                <p>
                    <label>快捷回复内容编辑：</label>
                    <asp:TextBox runat="server" ID="tbx_fastReplyContent" TextMode="MultiLine"></asp:TextBox>
                </p>
            </div>
            <div>
                <p>
                    <asp:Button runat="server" ID="btn_addFastReply" Text="添加快捷回复" OnClick="btn_addFastReply_Click"></asp:Button>
                    <asp:Button runat="server" ID="btn_modifyFastReply" Text="修改快捷回复" OnClick="btn_modifyFastReply_Click"></asp:Button>
                    <asp:Button runat="server" ID="btn_deleteFastReply" Text="删除快捷回复" OnClick="btn_deleteFastReply_Click"></asp:Button>
                    <asp:Button runat="server" ID="btn_updateFastReply" Text="更新快捷回复列表" OnClick="btn_updateFastReply_Click"></asp:Button>
                </p>
            </div>
        </div>
    </form>
</body>
</html>
