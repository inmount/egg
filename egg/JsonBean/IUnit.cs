﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace egg.JsonBean {
    /// <summary>
    /// 存储单元接口
    /// </summary>
    public interface IUnit {

        /// <summary>
        /// 获取Json表示形式
        /// </summary>
        /// <returns></returns>
        string ToJson();

        /// <summary>
        /// 获取单元类型
        /// </summary>
        UnitTypes GetUnitType();

        /// <summary>
        /// 获取父对象
        /// </summary>
        IUnit GetParent();

        /// <summary>
        /// 设置父对象
        /// </summary>
        void SetParent(IUnit p);

    }
}
