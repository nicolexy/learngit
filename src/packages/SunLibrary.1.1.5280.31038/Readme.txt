SunLibrary Release Notes

1、添加SunLibrary，会自动引用Log4net.dll
2、配置文件app.config会增加Log4net的相关配置：
    2.1 新增了 name 为 log4net 的configsections
    2.2 新增了两个appsettings：log4net.config 指示log4net配置文件地址；log4net.config.watch 是否定时检查配置文件
3、配置文件app.config会增加邮件的默认参数
    <!--emailHelper-->
    <add key="SunSmtpServer" value=""/>
    <add key="SunSmtpServerPort" value=""/>
    <add key="SunEmailFromAddress" value=""/>
    <add key="SunEmailUserName" value=""/>
    <add key="SunEmailPassword" value=""/>
3、新增了configs文件夹，存放log4net的配置文件 log4net.config

配置相关的内容，需要合并到站点的web.config里