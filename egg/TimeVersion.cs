using System;
using System.Collections.Generic;
using System.Text;

namespace egg {

    /// <summary>
    /// 一套根据时间线自动增长的版本号，使用此版本号可有效体现项目的消耗时间
    /// 格式为 年份序号.月份序号.年月标记.创建序号
    /// 年份序号：代表项目的年份跨度，如1.0.1908.3中的1代表项目为现年建立的，而3.4.1908.23中的3则代表从项目建立后有三个不同年份参与到项目的修订当中
    /// 月份序号：代表项目的本年度参与月份跨度，如1.0.1908.3中的0代表项目为本年的当月第一次修改，而3.4.1908.23中的4则代表本年的有4个月份均有参与到项目的修订当中
    /// 年月标记：代表项目最后一次修订的年月
    /// 创建序号：代表项目的总修订数
    /// </summary>
    public class TimeVersion : egg.BasicObject {

        /// <summary>
        /// 年份序号
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 月份序号
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// 年月标记
        /// </summary>
        public int Sign { get; set; }

        /// <summary>
        /// 版本序号
        /// </summary>
        public int Build { get; set; }

        // 设置默认版本
        private void SetDefaultVersion() {
            var t = eggs.Time.GetNow();
            this.Year = 1;
            this.Month = 0;
            this.Sign = (t.Year % 100) * 100 + t.Month;
            this.Build = 0;
        }

        /// <summary>
        /// 获取下一个版本
        /// </summary>
        /// <returns></returns>
        public TimeVersion GetNextVersion() {
            var t = eggs.Time.GetNow();
            int y = this.Year;
            int m = this.Month;

            // 获取标识中的年月信息
            int yy = this.Sign / 100;
            int mm = this.Sign % 100;

            // 获取当前年月信息
            int ny = t.Year % 100;
            int nm = t.Month;

            if (yy != ny) {
                // 跨年
                y++;
                m = 0;
            } else if (mm != nm) {
                // 跨月
                m++;
            }

            // 新建一个版本并赋值
            TimeVersion timeVersion = new TimeVersion() {
                Year = y,
                Month = m,
                Sign = ny * 100 + nm,
                Build = this.Build + 1
            };
            return timeVersion;
        }

        /// <summary>
        /// 新建一个
        /// </summary>
        /// <param name="ver"></param>
        public TimeVersion(string ver = null) {
            if (!ver.IsEmpty()) {
                string[] vers = ver.Split('.');
                if (vers.Length == 4) {
                    this.Year = vers[0].ToInteger();
                    this.Month = vers[1].ToInteger();
                    this.Sign = vers[2].ToInteger();
                    this.Build = vers[3].ToInteger();
                } else {
                    // 设置默认版本
                    SetDefaultVersion();
                }
            } else {
                // 设置默认版本
                SetDefaultVersion();
            }
        }

        /// <summary>
        /// 获取字符串表示形式
        /// </summary>
        /// <returns></returns>
        protected override string OnParseString() {
            return String.Format("{0}.{1}.{2}.{3}", this.Year, this.Month, this.Sign, this.Build);
        }

    }
}
