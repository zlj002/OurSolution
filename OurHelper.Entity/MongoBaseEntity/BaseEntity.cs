using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurHelper.Entity.MongoBaseEntity
{
    /// <summary>
    /// 数据实体基类
    /// </summary>
    [Serializable]
    public abstract class BaseEntity
    {
        public BaseEntity()
        {
            this.ID = Guid.NewGuid();
            this.CreateTime = DateTime.Now;
            this.LastUpdateTime = DateTime.Now;
        }

        /// <summary>
        /// 数据库唯一ID
        /// </summary> 
        public Guid ID { get; set; }

        /// <summary>
        /// 当前记录的创建时间
        /// </summary> 
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 当前记录最近一次更新时间
        /// </summary> 
        public DateTime LastUpdateTime { get; set; }
    }
}
