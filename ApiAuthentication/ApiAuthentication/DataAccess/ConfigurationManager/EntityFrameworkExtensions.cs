using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ConfigurationManager
{
    //File này đóng vai trò tạo ra các Extension method cho IConfigurationBuilder
    public static class EntityFrameworkExtensions
    {
        public static IConfigurationBuilder AddEntityFrameWork(this IConfigurationBuilder builder, Action<DbContextOptionsBuilder> setup)
        {
            return builder.Add(new EFConfigSource(setup));
        }
    }
}
