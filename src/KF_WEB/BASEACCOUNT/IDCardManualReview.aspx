<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IDCardManualReview.aspx.cs" Inherits="TENCENT.OSS.CFT.KF.KF_Web.BaseAccount.IDCardManualReview" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <style type="text/css">
        @import url( ../STYLES/ossstyle.css?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]??DateTime.Now.ToString("yyyyMMddHHmmss") %> );
        .transp-block{background:#fff no-repeat right bottom;margin:0px auto;width:555px;height:320px;overflow:hidden}
        .transparent{filter:alpha(opacity=70);moz-opacity:.70;opacity:.70}

        .artZoom {
	padding:3px;
	background:#FFF;
	border:1px solid #EBEBEB;
}

img {
	border:0 none;
}
#demo {
	width:800px;
	padding:2px;
	background:#FBFCFD;
}

      
    </style>    
    <script src="../SCRIPTS/jquery-1.7.2/jquery.min.js"></script>    
    <link rel="stylesheet" type="text/css" href="css/normalize.css" />
    	

   
    <script type="text/javascript" src="../SCRIPTS/jquery-easyui-1.5/jquery.easyui.min.js" charset="utf-8"></script>
    <script type="text/javascript" src="../SCRIPTS/jquery-easyui-1.5/locale/easyui-lang-zh_CN.js" charset="utf-8"></script>
    <link href="../SCRIPTS/jquery-easyui-1.5/themes/default/easyui.css" rel="stylesheet" />
    <link href="../SCRIPTS/jquery-easyui-1.5/themes/color.css" rel="stylesheet" />
    <link href="../SCRIPTS/jquery-easyui-1.5/themes/icon.css" rel="stylesheet" />
    <script src="../SCRIPTS/KF.js"></script>
    <script src="../SCRIPTS/LoadControlsDataSource.js?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]%>"></script>         
    <script src="IDCardManualReview.js?v=<%=System.Configuration.ConfigurationManager.AppSettings["PageStyleVersion"]%>"></script>

    
	<%--<link rel="stylesheet" type="text/css" href="../SCRIPTS/viewerjs/css/default.css"/>--%>
	<%--<link rel="stylesheet" href="../SCRIPTS/viewerjs/assets/css/bootstrap.min.css"/>--%>
	<link rel="stylesheet" href="../SCRIPTS/viewerjs/dist/viewer.css"/>
	<link rel="stylesheet" href="../SCRIPTS/viewerjs/css/main.css"/>

    <script src="../SCRIPTS/viewerjs/assets/js/bootstrap.min.js"></script>
	  <script src="../SCRIPTS/viewerjs/dist/viewer.js"></script>
	  <script src="../SCRIPTS/viewerjs/assets/js/main.js"></script>
</head>
<body>
    <form id="Form1" method="post" runat="server">
        <table style="height: 15px; width: 100%; background-color: #e4e5f7; border: 0; padding: 1px;">
            <tr>
                <td width="80%" height="18"><font color="#ff0000"><STRONG><FONT color="#ff0000">&nbsp;</FONT></STRONG><IMG height="16" src="../IMAGES/Page/post.gif" width="20">
										身份证影印件客服人工审核</font>
                    <div align="right"></div>
                </td>
                <td width="20%">操作员代码: <span style="color: #ff0000">
                    <asp:Label ID="Label_uid" runat="server">Label</asp:Label></span></td>
            </tr>
        </table>
        <div id="toolbar" style="width: 100%">
            <table style="width: 100%">
                <tr>
                    <td style="width: 20%; text-align: right">
                        <label id="lab_StartDate">申请开始日期：</label></td>
                    <td style="width: 30%">
                        <input type="text" runat="server" id="tbx_beginDate" />
                    </td>
                    <td style="width: 20%; text-align: right">
                        <label id="lab_EndDate">申请结束日期：</label></td>
                    <td style="width: 30%">
                        <input type="text" runat="server" id="txt_EndDate" />
                    </td>
                </tr>
                 <tr>
                    <td style="width: 20%; text-align: right">
                        <label id="lab_ModifyBeginDate">审核开始日期：</label></td>
                    <td style="width: 30%">
                        <input type="text" runat="server" id="txt_ModifyBeginDate" />
                    </td>
                    <td style="width: 20%; text-align: right">
                        <label id="lab_ModifyEndDate">审核结束日期：</label></td>
                    <td style="width: 30%">
                        <input type="text" runat="server" id="txt_ModifyEndDate" />
                    </td>
                </tr>
                <tr>

                    <td style="width: 20%; text-align: right">
                        <label id="lab_status">审核状态：</label></td>
                    <td style="width: 30%">
                        <input id="ddl_ReviewStatus" type="text" />
                    </td>
                    <td style="width: 20%; text-align: right">
                        <label id="lab_ReviewResult">审核结果：</label></td>
                    <td style="width: 30%">
                        <input id="ddl_ReviewResult" type="text" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%; text-align: right">
                        <label id="lab_Foperator">审核人：</label></td>
                    <td style="width: 30%">
                        <input type="text" runat="server" id="txt_Foperator" />
                    </td>
                    <td  style="width: 20%; text-align: right">
                        <label id="lab_Fmemo">失败原因：</label></td>
                    <td  style="width: 30%">
                        <input type="text" id="ddl_Fmemo" />                                                
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%; text-align: right">
                        <label id="lab_uin">帐号：</label></td>
                    <td style="width: 30%">
                        <input type="text" runat="server" id="txt_uin" />
                         <input id="WeChatId" name="IDType" type="radio" value="WeChatId"  /><label>微信帐号</label>
                         <input id="WeChatQQ" name="IDType"  type="radio"  value="WeChatQQ" /><label>微信绑定QQ</label>
                         <input id="WeChatMobile" name="IDType"  type="radio" value="WeChatMobile" /><label>微信绑定手机</label>
                         <input id="WeChatCft" name="IDType" type="radio" value="WeChatCft" checked="checked" /><label>财付通账户</label>
                    </td>
                    <td id="td_ReviewCountName" style="width: 20%; text-align: right">
                        <label id="Label1">批处理数：</label></td>
                    <td id="td_ReviewCount" style="width: 30%">
                        <input type="text" id="txt_ReviewCount" />                        
                        <a href="javascript:void(0)" id="btn_ReceiveReview" class="easyui-linkbutton" iconcls="icon-set" plain="true">批量领单</a>
                    </td>
                </tr>
                 
                <tr>
                    <td style="width: 20%; text-align: right"></td>
                    <td style="width: 30%">
                        <span style="color: red;">注:不能跨月查询</span>
                    </td>
                    <td style="width: 20%; text-align: left">                        
                        <a href="javascript:void(0)" id="btn_Search" class="easyui-linkbutton" iconcls="icon-search" plain="true">查 询</a>                        
                        <a href="javascript:void(0)" id="a_DownloadReviewData" class="easyui-linkbutton" iconcls="icon-print" plain="true">导出</a>
                        <%--<asp:Button ID="btn_DownloadReviewData" runat="server" OnClick="btn_DownloadReviewData_Click" />--%>
                    </td>
                    <td style="width: 30%"></td>
                </tr>                 
            </table>
        </div>
        <br />
        <div id="div_IDCardManualReviewList" style="width: 100%">
            <table id="tb_IDCardManualReviewList"></table>
            <input id="hid_IsHaveRightForSeeDetail" type="hidden" value="False" />
        </div>
        <div id="div_ReveiwIdCard" class="easyui-dialog">
            <table border="0" style="width: 100%; text-align: left; line-height: 20px;">
                <tr style="text-align: left">
                    <td style="width: 10%; min-width:80px; text-align: right">
                        <b>用户帐号：</b>
                    </td>
                    <td style="width: 85%" colspan="3">
                        <label id="lab_Fuin"></label>
                    </td>
                </tr>

                <tr style="text-align: left">
                      <td style="width: 10%; min-width:80px; text-align: right">
                        <b>用户姓名：</b>
                    </td>
                    <td style="width: 85%" colspan="3">
                        <label id="lab_Fname"></label>
                    </td>
                </tr>
                <tr style="text-align: left" colspan="3">
                     <td style="width: 10%; min-width:80px; text-align: right">
                        <b>证件号码：</b>
                    </td>
                    <td style="width: 85%">
                        <label id="lab_Fidentitycard"></label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 10%; min-width:80px; text-align: right">
                        <b>失败原因：</b>
                    </td>
                    <td style="width: 85%" colspan="3">
                        <input type="text" id="txt_Fmemo" />
                    </td>
                </tr>
                 <tr>
                    <td style="width: 10%; min-width:80px; text-align: right">
                        <b>通过备注：</b>
                    </td>
                    <td style="width: 85%" colspan="3">  
                        <input type="radio" name="AgreeRemark" id="radio_RGSH" value="1" checked="checked" />需要人工审核
                        <input type="radio" name="AgreeRemark" id="radio_XTYH" value="2" /><label style="color:red">系统可优化</label>
                        
                        
                    </td>
                </tr>
                <tr>
                    <th colspan="4" style="width: 100%; text-align: center">身份证扫描件
                    </th>
                </tr>
                <tr>
                    <td colspan="4">
                        <table style="width: 100%">
                           <%-- <tr>
                                <td style="width: 50%; text-align: center;"><b>正面</b></td>
                                <td style="width: 50%; text-align: center;"><b>反面</b></td>
                            </tr>--%>
                            <tr>
                                <td style="width: 100%; text-align: center;">

                                    <div class="docs-galley">
		                              <ul class="docs-pictures clearfix">
		                                <li style="width:49%"> <b>正面</b><br />
                                            <div style="position:relative">
                                            <img id="ima_IDCardZ" data-original="" src="" style="width: 100%; height:500px; padding:2px;background:#FFF;border:1px solid #EBEBEB;"/>
                                            </div>
                                            <%--<div id="div_ima_IDCardZ"   style="position:absolute;z-index:2;width: 100%; height: 500px;left:2px;top:2px;"></div>--%>
		                                </li>		           
                                        <li style="width:49%"><b>反面</b><br />
                                            <div style="position:relative">
                                            <img id="ima_IDCardF" data-original="" src="" style="width: 100%; height:500px; padding:2px;background:#FFF;border:1px solid #EBEBEB;"/>
                                                </div>
                                            <%--<div id="div_ima_IDCardF" style="position:absolute;z-index:2;width: 100%; height: 500px;left:2px;top:2px"></div>--%>
                                        </li>		           
		                              </ul>
		                            </div>
                                    
                                    <%--<div id="demo" style="position:relative">
                                        <img id="ima_IDCardZ" class="artZoom"  src="" style="width: 100%; height:320px" />
                                        <div id="div_ima_IDCardZ"   style="position:absolute;z-index:2;width: 100%; height: 320px;left:2px;top:2px;"></div>
                                    </div>   --%>        

                                   <%-- <a id="a_IDCardZ" class="miniImg artZoom" href="" rel=""><img id="ima_IDCardZ" src="" style="width: 100%; height:600px" /></a>      --%>                           
                                </td>
                                <%--<td style="width: 50%; text-align: center;">
                                    
                                    <div style="position:relative">
                                       
                                       <img id="ima_IDCardF" class="small_02" src="" style="width: 100%; height: 320px" />
                                        <div id="div_ima_IDCardF" style="position:absolute;z-index:2;width: 100%; height: 320px;left:2px;top:2px"></div>
                                    </div>
                                </td>--%>

                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="color: red; text-align: center;">
                        <input id="hid_TableName" type="hidden" />
                        <input id="hid_Fid" type="hidden" />
                        <input id="hid_Fserial_number" type="hidden" />

                        <a href="javascript:void(0)" id="a_Yes" class="easyui-linkbutton" iconcls="icon-ok" plain="true">通过</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <a href="javascript:void(0)" id="a_No" class="easyui-linkbutton" iconcls="icon-no" plain="true">拒绝</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <a href="javascript:void(0)" id="a_ReSend" class="easyui-linkbutton" iconcls="icon-save" plain="true">重新提交</a>
                    </td>
                </tr>
            </table>
        </div>
        <%--<div id="movediv" style="Z-INDEX: 102; POSITION: absolute; VISIBILITY: hidden; TOP: 300px; LEFT: 100px">
			<img id="moveimgid" width="400" height="300"></div>--%>
        <input id="hid_IdCaredServerPath" type="hidden" runat="server" />

    </form>
</body>
</html>
