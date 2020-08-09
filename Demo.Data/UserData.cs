using Demo.Domain;
using Demo.IocCommon.AutofacDependency;
using System.Collections.Generic;
using System.Linq;

namespace Demo.Data
{
    public class UserData : IScopedDependency
    {
        private static readonly List<User> Users = new List<User>()
        {
            new User()
            {
                Age = 1,
                Name = "levy",
            },
            new User()
            {
                Age = 2,
                Name = "le3vy",
            },
            new User()
            {
                Age = 3,
                Name = "43456",
            },
            new User()
            {
                Age = 4,
                Name = "le4444vy",
            },
            new User()
            {
                Age = 5,
                Name = "lev66y",
            },
        };
        public List<User> FindUser(int age)
        {
            return Users.Where(c => c.Age > age).ToList();
        }
    }
}
