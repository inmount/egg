# Egg开发套件

一个基于.Net Standard 2.1的综合性辅助开发套件基础件。

A comprehensive auxiliary development kit based on .Net Standard 2.1.

## 支持框架

| 组件 | .Net版本 | 描述 |
| ---- | ----- | ---- |
| Egg | .Net Standard 2.1 | 基础组件 |
| Egg.BarCode | .Net Standard 2.1 | 条码组件 |
| Egg.EFCore | .Net Standard 2.1 | EFCore组件 |
| Egg.EFCore.PostgreSQL | .Net Standard 2.1 | PostgreSQL组件 |
| Egg.EFCore.Sqlite | .Net Standard 2.1 | Sqlite组件 |
| Egg.Lark | .Net Standard 2.1 | 百灵鸟脚本组件 |
| Egg.Log | .Net Standard 2.1 | 日志组件 |
| Egg.Security | .Net Standard 2.1 | 安全组件 |
| Egg.VirtualDisk | .Net Standard 2.1 | 虚拟存储组件 |

## Nuget包下载

所有的组件均可以访问<https://www.nuget.org/profiles/inmount>进行下载

## 更新说明

### v16.1.2301.8 升级

+ 增加了百灵鸟脚本组件(Egg.Lark)
+ 基础组件中增加了对可空字符串的兼容处理

### v16 升级

+ 自2022年10月开始，重建整个开发包，版本号改为16.x所有组件，版本号统一发布
+ 自2022年5月开始，为了增加套件的兼容性，大多数基础类将往低版本迁移
