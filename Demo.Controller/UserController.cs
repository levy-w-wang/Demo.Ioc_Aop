using Autofac;
using Demo.Business;
using Demo.IocCommon;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Demo.Controller
{
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private UserBusiness UserBusinessConstructor { get; set; }
        public UserController(UserBusiness userBusiness)
        {
            this.UserBusinessConstructor = userBusiness;
        }
        public UserBusiness UserBusiness { get; set; }

        [AcceptVerbs("Get"), Route("user")]
        public ActionResult FindUser(int age)
        {
            var userBusinessResolve = IocCore.AutofacContainer.Resolve<UserBusiness>();
            var result1 = userBusinessResolve.FindUser(age); //通过容器实例化

            var result2 = UserBusiness.FindUser(age);//属性注入

            var result3 = UserBusinessConstructor.FindUser(age);//构造函数注入

            return Ok(UserBusiness.FindUser(age));
        }

        [HttpGet, Route("gettime")]
        public ActionResult GetTime()
        {
            return Ok(DateTime.Now);
        }
    }
}
