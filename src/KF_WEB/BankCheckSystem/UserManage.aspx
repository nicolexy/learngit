<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserManage.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BankCheckSystem.UserManage" %>


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
        .auto-style1 {
            height: 22px;
        }
        .auto-style2 {
            width: 30%;
            height: 22px;
        }
        .auto-style3 {
            width: 20%;
            height: 22px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div style="margin-top:10px;margin-left:10px">
            <table class="tb_detail" width="90%" cellspacing="1" cellpadding="1">
                <tr>
                   <TD style=" background-color:#e4e5f7" colspan="4"><FONT face="宋体"><FONT color="red"><IMG height="16" src="../IMAGES/Page/post.gif" width="20">登录名信息查询</FONT>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</FONT>操作员代码: </FONT><SPAN class="style3"><asp:label id="Label1" runat="server" ForeColor="Red" Width="73px"></asp:label></SPAN></TD>
                </tr>
                <tr>
                    <td style="text-align:right;width:120px">登录名：</td>
                    <td><asp:TextBox runat="server" ID="txt_userBindEmail"></asp:TextBox>
                       </td>
                     <td style="text-align:right;width:120px">
                      银行类型：</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddl_Fbank_id" Width="134px"></asp:DropDownList></td>
                </tr>
                   <tr>
                    <td style="text-align:right;width:120px">申请人姓名：</td>
                    <td>
                        <asp:TextBox runat="server" ID="txt_Fuser_name"></asp:TextBox></td>
                     <td style="text-align:right;width:120px">
                       申请人证件号码：</td>
                    <td>
                       <asp:TextBox runat="server" ID="txt_Fuser_id_no"></asp:TextBox></td>
                </tr>
                <tr>
                    <td colspan="4" style="text-align:right">
                         <asp:Button ID="btnSerach" runat="server" Text="查 询" OnClick="btnSerach_Click"  />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
            </table>
            <br />
            <asp:datagrid id="DataGrid1" runat="server" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px"
							BackColor="White" CellPadding="3" GridLines="Horizontal" AutoGenerateColumns="False" Width="90%" OnItemCommand="DataGrid1_ItemCommand">
							<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
							<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
							<Columns>
                                <asp:BoundColumn DataField="Fuser_id" Visible="false" ></asp:BoundColumn>
								<asp:BoundColumn DataField="Fuser_bind_email" HeaderText="登录号"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fbank_id_str" HeaderText="银行类型"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fcontact_name" HeaderText="联系人"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fcontact_manager" HeaderText="接口人"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fcreate_time" HeaderText="提交时间"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Fuser_status_str" HeaderText="当前状态"></asp:BoundColumn>
								<asp:ButtonColumn Text="详细" HeaderText="查看" CommandName="detail"></asp:ButtonColumn>
							</Columns>
                            <PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
						</asp:datagrid>
     <webdiyer:aspnetpager id="Aspnetpager1" runat="server" AlwaysShow="True" ShowCustomInfoSection="left" NumericButtonTextFormatString="[{0}]"
							SubmitButtonText="转到" HorizontalAlign="right" CssClass="mypager" ShowInputBox="always" PagingButtonSpacing="0" Width="90%"
							NumericButtonCount="10" OnPageChanged="ChangePage"></webdiyer:aspnetpager>

           
            <br />
             <table id="tbDetail" runat="server" visible="false" class="tb_detail"  style="width:90%"  cellspacing="1" cellpadding="1">
            
                <tr>
                    <td style="text-align:right;width:20%">登录名：</td>
                    <td >
                        <asp:Label ID="lbl_Fuser_bind_email" runat="server"></asp:Label>
                    </td>
                     <td style="text-align:right;width:20%">
                         
                         银行类型：</td>
                    <td>
                      <asp:Label ID="lbl_Fbank_id_str" runat="server"></asp:Label>
                    </td>
                </tr>
                     <tr>
                    <td style="text-align:right;">申请人姓名：</td>
                    <td style="width:30%">
                        <asp:Label ID="lbl_Fuser_name" runat="server"></asp:Label>
                    </td>
                     <td style="text-align:right;width:20%">
                         申请人身份证号码：</td>
                    <td>
                      <asp:Label ID="lbl_Fuser_id_no" runat="server"></asp:Label>
                    </td>
                </tr>  <tr>
                    <td style="text-align:right;">联系人姓名：</td>
                    <td style="width:30%">
                        <asp:TextBox ID="txt_Fcontact_name" runat="server"></asp:TextBox>
                    </td>
                     <td style="text-align:right;width:20%">
                         
                         申请人电话：</td>
                    <td>
                      <asp:Label ID="lbl_Fcontact_tel" runat="server"></asp:Label>
                    </td>
                </tr>  <tr>
                    <td style="text-align:right;">联系人手机：</td>
                    <td style="width:30%">
                        <asp:TextBox ID="txt_Fcontact_mobile" runat="server"></asp:TextBox>
                    </td>
                     <td style="text-align:right;width:20%">
                         
                         联系人QQ：</td>
                    <td>
                      <asp:TextBox ID="txt_Fcontact_qq" runat="server"></asp:TextBox>
                    </td>
                </tr>  <tr>
                    <td style="text-align:right;" class="auto-style1">联系人Email：</td>
                    <td class="auto-style2">
                        <asp:TextBox ID="txt_Fcontact_email" runat="server"></asp:TextBox>
                    </td>
                     <td style="text-align:right;" class="auto-style3">
                         接口人：</td>
                    <td class="auto-style1">
                      <asp:TextBox ID="txt_Fcontact_manager" runat="server"></asp:TextBox>
                    </td>
                </tr>  <tr>
                    <td style="text-align:right;">修改备注：</td>
                    <td colspan="3">
                        <asp:TextBox ID="txt_Fremark" runat="server"></asp:TextBox>
                    </td>
                    
                </tr>
                  <tr>
                    <td style="text-align:right;">权限：</td>
                    <td  colspan="3">
                        <asp:CheckBoxList runat="server" ID="chk_right" RepeatDirection="Horizontal">

                        </asp:CheckBoxList>
                    </td>
                    
                </tr>
                  <tr>
                    <td style="text-align:center" colspan="4">
                                                 <asp:Button ID="btnSave" runat="server" Text="保 存" OnClick="btnSave_Click" />
                    
                    </td>
                     
                </tr>
                 </table>
        </div>
        
        
        
    </form>
</body>
</html>