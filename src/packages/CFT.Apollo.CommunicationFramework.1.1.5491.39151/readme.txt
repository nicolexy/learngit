CFT.Apollo.CommunicationFramework
==============================================
组件本身增加了以下配置
<appSettings>
    <!--Relay Server Configuration-->
    <add key="CFRelayServerIP" value="172.25.38.123" /><!--默认的Relay服务器ip-->    
    <add key="CFRelayServerPort" value="22000" /><!--默认的Relay服务器端口-->    
    <add key="CFRelayDefaultSPId" value="2000000000" /><!--默认的Relay接口的调用商户号-->    
    <!--ICE Server Configuration-->    
    <add key="CFICEServerIP" value="172.25.39.53" /><!--默认的ICE服务器ip-->    
    <add key="CFICEServerPort" value="6644" /><!--默认的ICE服务器ip-->    
    <add key="CFICEUsername" value="rayguo" /><!--默认的ICE服务器用户名-->    
    <add key="CFICEPassword" value="1111" /><!--默认的ICE服务器密码-->    
    <add key="CFTrace" value="true" /><!--开启CF日志-->
</appSettings>
===============================================
组件引用的CFT.Apollo.Logging添加了以下配置；若不使用日志，可以去掉以下配置
    <configSections>
        <section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler, log4net" />
    </configSections>
    <appSettings>
        <!--log4net-->
        <add key="log4net.Config" value="Configs\Log4net.config" />
        <add key="log4net.Config.Watch" value="False" />
    </appSettings>
 
 同时CFT.Apollo.Logging还添加了一个Configs文件夹，里面存放的Log4net的配置文件;若不使用日志，可以删掉
 ==============================================
 其他问题，请访问up