# egg.Lark2

Egg开发套件中关于百灵鸟二代函数式脚本引擎(Lark Script Engine 2)支持的可选组件

该脚本配合SIR中间语言/SEVM虚拟机工作

## 语法规则

采用括号嵌套进行操作定义，比如：

```
    # 定义变量
    var(pi, r, area, str),
    # 设置内容
    set(pi, 3.1416),
    set(r, read()),
    # 计算面积
    calc(area, [pi*(r*r)]),
    # 连接字符串
    join(str, pi, "*(", r, "*", r, ")=", area),
    # 输出结果
    print(str)
```