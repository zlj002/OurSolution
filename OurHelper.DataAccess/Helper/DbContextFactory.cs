using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text; 

namespace OurHelper.DataAccess.Helper
{
    public abstract class DbContextFactory
    {
        /// <summary>
        /// 获得DbContext
        /// </summary>
        /// <returns></returns>
        public abstract DbContext GetDbContext();
    }
}
