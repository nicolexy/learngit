CFT.Apollo.Data
通用数据库操作类库
=======================================
【可选功能】组件自身增加了集中获取字符串连接字符串的配置；若不需要此功能，可删除此配置项
  <configSections>
    <sectionGroup name="CFT.Apollo.Data">
      <section name="ConnectionStringAdapter" type="CFT.Apollo.Data.ConnectionString.ConnectionStringConfigurationSection, CFT.Apollo.Data"/>
    </sectionGroup>
  </configSections>
  
  <CFT.Apollo.Data>
    <ConnectionStringAdapter 
      [option]enable="true"
      [option]environmentNameInMapping = "Environment"
      systemNameInMapping ="lotteryoms"
      [option]systemNameInCore="CFT_ZW_ALL" 
      mappingDBGlobalKey="KeyMappingDB"
      relayIP="10.12.23.14"
      relayPort="35600"
      [option]relayDefaultSPId="2000000000"
      />
  </CFT.Apollo.Data>
  
  
【可选功能】组件引用的CFT.Apollo.CommunicationFramework添加了以下配置，若不单独使用CF，以下配置可删除（除CFTrace不可删除）
 <appSettings>
    <!--RelayServer Configuration-->
    <add key="CFRelayServerIP" value="172.25.38.123" />
    <add key="CFRelayServerPort" value="22000" />
    <add key="CFRelayDefaultSPId" value="2000000000" />
    <!--ICE Server Configuration-->
    <add key="CFICEServerIP" value="172.25.39.53" />
    <add key="CFICEServerPort" value="6644" />
    <add key="CFICEUsername" value="rayguo" />
    <add key="CFICEPassword" value="1111" />
    <!--CF Log-->   
    <add key="CFTrace" value="true" />
  </appSettings>
  
 

============================================
其他问题，请访问http://up.cf.com/index.php/CFT.Apollo.Data
