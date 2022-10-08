using System;
using System.Collections.Generic;
using System.Text;

namespace Egg.Text {
    /// <summary>
    /// 拼音信息
    /// </summary>
    public class PinyinInfo {

        /// <summary>
        /// 拼音
        /// </summary>
        public string Pinyin { get; set; }

        /// <summary>
        /// 声调
        /// </summary>
        public int Tone { get; set; }

        /// <summary>
        /// 带声调拼音
        /// </summary>
        public string PinyinWithTone {
            get {
                if (this.Tone <= 0) return this.Pinyin;
                if (this.Pinyin.IndexOf("a") >= 0) {
                    switch (this.Tone) {
                        case 1: return this.Pinyin.Replace("a", "ā");
                        case 2: return this.Pinyin.Replace("a", "á");
                        case 3: return this.Pinyin.Replace("a", "ǎ");
                        case 4: return this.Pinyin.Replace("a", "à");
                        default: return this.Pinyin;
                    }
                } else if (this.Pinyin.IndexOf("o") >= 0) {
                    switch (this.Tone) {
                        case 1: return this.Pinyin.Replace("o", "ō");
                        case 2: return this.Pinyin.Replace("o", "ó");
                        case 3: return this.Pinyin.Replace("o", "ǒ");
                        case 4: return this.Pinyin.Replace("o", "ò");
                        default: return this.Pinyin;
                    }
                } else if (this.Pinyin.IndexOf("e") >= 0) {
                    switch (this.Tone) {
                        case 1: return this.Pinyin.Replace("e", "ē");
                        case 2: return this.Pinyin.Replace("e", "é");
                        case 3: return this.Pinyin.Replace("e", "ě");
                        case 4: return this.Pinyin.Replace("e", "è");
                        default: return this.Pinyin;
                    }
                } else if (this.Pinyin.IndexOf("iu") >= 0) {
                    switch (this.Tone) {
                        case 1: return this.Pinyin.Replace("iu", "iū");
                        case 2: return this.Pinyin.Replace("iu", "iú");
                        case 3: return this.Pinyin.Replace("iu", "iǔ");
                        case 4: return this.Pinyin.Replace("iu", "iù");
                        default: return this.Pinyin;
                    }
                } else if (this.Pinyin.IndexOf("i") >= 0) {
                    switch (this.Tone) {
                        case 1: return this.Pinyin.Replace("i", "ī");
                        case 2: return this.Pinyin.Replace("i", "í");
                        case 3: return this.Pinyin.Replace("i", "ǐ");
                        case 4: return this.Pinyin.Replace("i", "ì");
                        default: return this.Pinyin;
                    }
                } else if (this.Pinyin.IndexOf("u") >= 0) {
                    switch (this.Tone) {
                        case 1: return this.Pinyin.Replace("u", "ū");
                        case 2: return this.Pinyin.Replace("u", "ú");
                        case 3: return this.Pinyin.Replace("u", "ǔ");
                        case 4: return this.Pinyin.Replace("u", "ù");
                        default: return this.Pinyin;
                    }
                } else if (this.Pinyin.IndexOf("v") >= 0) {
                    switch (this.Tone) {
                        case 1: return this.Pinyin.Replace("v", "ǖ");
                        case 2: return this.Pinyin.Replace("v", "ǘ");
                        case 3: return this.Pinyin.Replace("v", "ǚ");
                        case 4: return this.Pinyin.Replace("v", "ǜ");
                        default: return this.Pinyin;
                    }
                } else {
                    return this.Pinyin;
                }
            }
        }

        /// <summary>
        /// 对象实例化
        /// </summary>
        /// <param name="py"></param>
        /// <param name="tone"></param>
        public PinyinInfo(string py, int tone) {
            this.Pinyin = py;
            this.Tone = tone;
        }
    }
}
