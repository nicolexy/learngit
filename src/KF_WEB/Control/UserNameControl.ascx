<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserNameControl.ascx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.Control.UserControl" %>
<asp:TextBox ID="TextBox1_InputQQ" runat="server"></asp:TextBox>
<asp:RadioButton ID="WeChatId" GroupName="IDType" runat="server" Text="微信帐号" Checked="true" /> 
<asp:RadioButton ID="WeChatQQ" GroupName="IDType" runat="server" Text="微信绑定QQ"/>
<asp:RadioButton ID="WeChatMobile" GroupName="IDType" runat="server" Text="微信绑定手机"/>
<asp:RadioButton ID="WeChatEmail" GroupName="IDType" runat="server" Text="微信绑定邮箱"/>
<asp:RadioButton ID="WeChatUid" GroupName="IDType" runat="server" Text="微信内部ID"/>
<asp:RadioButton ID="WeChatCft" GroupName="IDType" runat="server" Text="微信财付通账号"/> 