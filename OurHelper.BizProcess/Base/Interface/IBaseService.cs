using OurHelper;
using OurHelper.Api.Param;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OurHelper.BizProcess.Base.Interface
{
    public interface IBaseService
    {
        /*
         * 关于验证的层次
             
         * 第一层 UI层验证表单数据格式有效性，增加用户体验，友好提示
            
         * =========================================================
            
         * 第二层 控制转发层进行以下验证  
            
         * 1--身份（是否登录） 
            
         * 2--真实性 防止篡改
            
         * 3--防止重复
            
         * =========================================================
            
         * 第三层 逻辑层进行参数的基本验证和数据逻辑关系验证          本层控制
            
         * 1--格式（是否符合规定）
            
         * 2--数据逻辑关系验证
            
         * =========================================================
           第四层 数据库系统约束物理数据的有效性
         */
    }
}
