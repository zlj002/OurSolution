﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurHelper.Api.Param
{
    /// <summary>
    /// 游客分页参数
    /// </summary>
    [Serializable]
    public class PagingParameter : ApiParameter
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 页数
        /// </summary>
        public int PageSize { get; set; }

        public PagingParameter()
        {
            this.PageIndex = 1;
            this.PageSize = 10;
        }
    }
}
