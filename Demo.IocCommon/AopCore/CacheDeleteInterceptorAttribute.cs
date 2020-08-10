using System;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.IocCommon.AopCore
{
    public class CacheDeleteInterceptorAttribute : AbstractInterceptorAttribute
    {
        private readonly Type[] _types;
        private readonly string[] _methods;

        /// <summary>
        /// 需传入相同数量的Types跟Methods，同样位置的Type跟Method会组合成一个缓存key，进行删除
        /// </summary>
        /// <param name="types">传入要删除缓存的类</param>
        /// <param name="methods">传入要删除缓存的方法名称，必须与Types数组对应</param>
        public CacheDeleteInterceptorAttribute(Type[] types, string[] methods)
        {
            if (types == null || methods == null)
            {
                throw new Exception("不能传入空值");
            }
            if (types.Length != methods.Length)
            {
                throw new Exception("Types必须跟Methods数量一致");
            }
            _types = types;
            _methods = methods;
        }

        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            //var cache = context.ServiceProvider.GetService<ICacheHelper>();
            await next(context);
            //for (int i = 0; i < _types.Length; i++)
            //{
            //    var type = _types[i];
            //    var method = _methods[i];
            //    string key = "Methods:" + type.FullName + "." + method;
            //    cache.Delete(key);
            //}
        }
    }
}