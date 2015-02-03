CFT.Apollo.Common C#基础公共函数库
==========================================
[可选功能]若需要使用CFT.Apollo.Common.Diagnostics.LogHelper日志功能，需要安装CFT.Apollo.Log4net

==========================================
若不能正常打印日志，请检查系统日志

==========================================

==========================================
版本冲突，请加程序集跳转配置
<configuration>
  <runtime>
    <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
      <dependentAssembly>
        <assemblyIdentity name="CFT.Apollo.Common" publicKeyToken="" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-65535.65535.65535.65535" newVersion="3.1.5494.24493" />
      </dependentAssembly>
    </assemblyBinding>
  </runtime>
 </configuration>

其他问题，请访问up