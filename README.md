# Lycium.Authentication
Token颁发和认证组件

## 工作流程

### 用户请求流程 

    ![时序图](https://github.com/night-moon-studio/Lycium.Authentication/blob/master/images/Token%E6%97%B6%E5%BA%8F%E5%9B%BE.png)
    
    客户端使用的仅仅是 LoginService 服务类，该类提供了登录与登出两个 API , 其他刷新以及分发 Token 均由中间件自己完成。
    
<br/>

### 客户端注册流程

    ![时序图](https://github.com/night-moon-studio/Lycium.Authentication/blob/master/images/%E5%AE%A2%E6%88%B7%E7%AB%AF%E6%97%B6%E5%BA%8F%E5%9B%BE.png)


  - 只有分配了 SecretKey 的客户端主机才能与服务端进行通信，因此每个主机在接入之前需要在服务端进行配置，
  该配置存储在表 LyciumHost 中： 
  
    - 客户端名称: "FreesqlClient"	
    - 客户端URL: "https://localhost:6001"	
    - 分配给客户端的 SecretKey: "fd4685a2-b78f-4dda-9d59-5d420410cf96"	
    - 客户端通信时分配的 Token: "d247990b-b0fe-47b1-8c4e-ba6ca36cdeea"
    - 客户端配置策略 ：1
  
  - 此时需要接入的客户端需要在 Starup 里添加服务配置，如后面的代码。  
  - 客户端开启之后会根据配置的 ServerUrl 自动寻找服务主机，并为自己申请一个通信的 Token； 
  - 申请到 Token 之后便开始扫描上传本地路由资源，服务端检测和添加之前没有存储的路由资源同时将本地的白名单资源回发给客户端主机。  
  
<br/>


## 服务端使用

### 准备工作

- 引入 `Lycium.Authentication.Server.PgFreeSql` 包

- 添加服务 `services.AddLyciumAuthenticationPgSql();`

- 注入IFreeSql `services.AddSingleton(p => new FreeSql.FreeSqlBuilder().xxx 配置;`

- 注入 HttpClient   `services.AddHttpClient();`



### 创建表

![表图](https://github.com/night-moon-studio/Lycium.Authentication/blob/master/images/ServerTables.png)  

Server 端需要 6 张表，可以通过使用 FreeSql 进行自动生成，如果出错一般都是主键自增的问题。

 - LyciumConfig : 可以为每个客户端分配关于客户端通信的配置，目前仅有客户端凭证的过期时间。

 - LyciumHost : 记录的客户端(接入的系统主机)的信息，系统接入之前需要配置该表。
 
 - LyciumHostGroup : 客户端系统分组，系统在不同业务范围需要进行分组，例如同一个系统服务可能参与到了 APP的业务范畴，也可能参与到了WEB后台的业务。
 
 - LyciumHostRelation : 记录了系统ID与组ID的关联关系。
 
 - LyciumResource ： 客户端（接入的系统）在使用 LyciumClient 中间件后会扫描本地路由信息，并上传给服务端，改表记录了客户端的路由资源。
 
 - LyciumToken : 记录了 用户在分组下的Token。
 
 <br/> 
 
 
 ## 客户端使用
 
 ### 准备工作
 

- 引入 `Lycium.Authentication.Client.PgFreeSql` 包

- 对接服务端

```C# 

services.AddLyciumAuthenticationPgSql(item => item
        .SetServerUrl("https://localhost:8001")
        .SetSecretKey("fd4685a2-b78f-4dda-9d59-5d420410cf96"));
```

- 注入IFreeSql `services.AddSingleton(p => new FreeSql.FreeSqlBuilder().xxx 配置;`

- 注入 HttpClient `services.AddHttpClient();`
 
<br/>

### 创建表

Ciient 端仅需 1 张表，可以通过使用 FreeSql 进行自动生成，如果出错一般都是主键自增的问题。
 
 - LyciumToken : 记录了 用户在该客户端下及分组下的Token 。
 
 <br/> 

 
 ### 申请 Token
 
  - 使用 LoginService 
  ```C#
  public class XXXController
  {
      private readonly LoginService _loginService;
      public XXXXController(LoginService _loginService)
      
  }
  
  //根据不同的组获取不同界限下的 Token
  _loginService.Login(uid,"APP组");
  _loginService.Login(uid,"WEB组");
  ```
