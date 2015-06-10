using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OurHelper.Entity.Extend
{
    /// <summary>
    /// 数据库字段的用途。
    /// </summary>
    public enum EnumEntityMemberUsage
    {
        /// <summary>
        /// 不生成对应的sql列（一般用于批量操作生成sql时）
        /// </summary>
        NoDBField = 0
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class EntityMemberAttribute : Attribute
    {
        public EnumEntityMemberUsage eemu { get; set; }
        public EntityMemberAttribute(EnumEntityMemberUsage eemu)
        {
            this.eemu = eemu;
        }
    }
}
