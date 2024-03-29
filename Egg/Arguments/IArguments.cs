﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Egg.Arguments {
    /// <summary>
    /// 命令行参数集合
    /// </summary>
    public interface IArguments {

        /// <summary>
        /// 设置命令行参数
        /// </summary>
        /// <param name="args"></param>
        void SetParams(string[] args);

        /// <summary>
        /// 设置参数值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void SetParam(string key, string value);

        /// <summary>
        /// 获取参数值
        /// </summary>
        /// <param name="key"></param>
        string GetParam(string key);

    }
}
