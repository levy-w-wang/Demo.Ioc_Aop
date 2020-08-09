using Demo.Data;
using Demo.Domain;
using Demo.IocCommon.AutofacDependency;
using System.Collections.Generic;

namespace Demo.Business
{
    public class UserBusiness : IScopedDependency
    {
        public UserData UserData { get; set; }
        public List<User> FindUser(int age)
        {
            return UserData.FindUser(age);
        }
    }
}
