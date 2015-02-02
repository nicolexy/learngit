CFT.Apollo.Log4net，只是包装了原Log4net.dll，打包了Log4net配置文件。

================================================
安装后
1、项目配置文件会新增
  <configSections>
    <section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler, log4net" />
  </configSections>
  <appSettings>
    <!--log4net-->
    <add key="log4net.Config" value="Configs\Log4net.config"/>
    <add key="log4net.Config.Watch" value="False"/>
  </appSettins>
2、新增一个文件夹Configs,里面存放是的Log4net的配置文件
3、默认配置下日志文件按天存放在Logs文件夹下

================================================
若项目中已经有包含Log4net的引用，请使用assembly重定向
<runtime>

    <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">

      <dependentAssembly>

        <assemblyIdentity name="log4net" publicKeyToken="669e0ddf0bb1aa2a" culture="neutral" />

        <bindingRedirect oldVersion="0.0.0.0-1.2.13.0" newVersion="1.2.13.0" />

      </dependentAssembly>

    </assemblyBinding>

  </runtime>
  
=================================================
有问题，请访问up