using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using tdb.framework.webapi.IocAutofac;

namespace tdb.account.webapi
{
    /// <summary>
    /// autofac注册模块
    /// </summary>
    public class AutofacModuleRegister : Autofac.Module
    {
        /// <summary>
        /// 重写Autofac管道Load方法，在这里注册注入
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            //所有要实现依赖注入的借口都要继承该接口
            var baseType = typeof(IAutofacDependency);

            //获取需要注册的程序集名称集合
            var lstAssemblyName = this.GetRegisterAssemblyNames();

            foreach(var assemblyName in lstAssemblyName)
            {
                //注册程序集中的对象
                builder.RegisterAssemblyTypes(GetAssemblyByName(assemblyName)).Where(m => baseType.IsAssignableFrom(m) && m != baseType)
                       .AsImplementedInterfaces()//表示注册的类型，以接口的方式注册
                       //.EnableInterfaceInterceptors()//引用Autofac.Extras.DynamicProxy,使用接口的拦截器，在使用特性 [Attribute] 注册时，注册拦截器可注册到接口(Interface)上或其实现类(Implement)上。使用注册到接口上方式，所有的实现类都能应用到拦截器。
                       .InstancePerLifetimeScope();//同一个Lifetime生成的对象是同一个实例
            }

            #region 一个接口多处实现时注册

            ////单独注册
            //builder.RegisterType<WxPayService>().Named<IPayService>(typeof(WxPayService).Name);
            //builder.RegisterType<AliPayService>().Named<IPayService>(typeof(AliPayService).Name);

            #endregion
        }

        /// <summary>
        /// 根据程序集名称获取程序集
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <returns></returns>
        private Assembly GetAssemblyByName(string assemblyName)
        {
            return Assembly.Load(assemblyName);
        }

        /// <summary>
        /// 获取需要注册的程序集名称集合
        /// </summary>
        /// <returns></returns>
        protected virtual List<string> GetRegisterAssemblyNames()
        {
            return new List<string>() { "tdb.account.dal", "tdb.account.bll" };
        }
    }

    #region 一个接口多处实现时使用

    //public class HomeController : Controller
    //{
    //    private IPersonService _personService;
    //    private IPayService _wxPayService;
    //    private IPayService _aliPayService;
    //    private IComponentContext _componentContext;//Autofac上下文
    //    //通过构造函数注入Service
    //    public HomeController(IPersonService personService, IComponentContext componentContext)
    //    {
    //        _personService = personService;
    //        _componentContext = componentContext;
    //        //解释组件
    //        _wxPayService = _componentContext.ResolveNamed<IPayService>(typeof(WxPayService).Name);
    //        _aliPayService = _componentContext.ResolveNamed<IPayService>(typeof(AliPayService).Name);
    //    }
    //    public IActionResult Index()
    //    {
    //        ViewBag.eat = _personService.Eat();
    //        ViewBag.wxPay = _wxPayService.Pay();
    //        ViewBag.aliPay = _aliPayService.Pay();
    //        return View();
    //    }
    //}

    #endregion
}
