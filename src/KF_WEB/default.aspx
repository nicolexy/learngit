<%@ Page Language="c#" CodeBehind="Default.aspx.cs" AutoEventWireup="True" Inherits="TENCENT.OSS.CFT.KF.KF_Web.Default" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>财付通 客服系统</title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
</head>
<frameset border="0" framespacing="0" rows="61,89%" frameborder="0">
		<FRAME src="Top.aspx" frameBorder="0" noresize="noresize" scrolling="no">
		<frameset cols="118px,*">
            <FRAME src="Left.aspx" frameBorder="0" noresize="noresize">
			<frame name="WorkArea" src="<%=path%>" frameBorder=0 noresize="noresize"></frame>
		</frameset>
	</frameset>
</html>
