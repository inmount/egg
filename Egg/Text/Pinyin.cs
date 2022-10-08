using System;
using System.Collections.Generic;
using System.Text;

namespace Egg.Text {

    /// <summary>
    /// 拼音处理
    /// </summary>
    public class Pinyin : IDisposable {

        // 字典
        private Dictionary<PinyinInfo, string> dic;
        private Dictionary<char, PinyinInfo> dicSpecial;

        /// <summary>
        /// 都西昂实例化
        /// </summary>
        public Pinyin() {
            // 建立字典
            dic = new Dictionary<PinyinInfo, string>();

            #region [=====A=====]

            dic.Add(new PinyinInfo("a", 1), "阿啊腌吖锕");

            dic.Add(new PinyinInfo("ai", 1), "挨唉哎哀锿埃");
            dic.Add(new PinyinInfo("ai", 2), "挨皑癌");
            dic.Add(new PinyinInfo("ai", 3), "嗳毐欸矮蔼霭");
            dic.Add(new PinyinInfo("ai", 4), "嗳唉艾砹爱暧嫒叆碍隘");

            dic.Add(new PinyinInfo("an", 1), "安桉氨鞍庵鹌谙");
            dic.Add(new PinyinInfo("an", 2), "玵");
            dic.Add(new PinyinInfo("an", 3), "铵俺埯唵揞");
            dic.Add(new PinyinInfo("an", 4), "岸按案胺暗黯");

            dic.Add(new PinyinInfo("ang", 1), "肮");
            dic.Add(new PinyinInfo("ang", 2), "昂");
            dic.Add(new PinyinInfo("ang", 4), "盎");

            dic.Add(new PinyinInfo("ao", 1), "熬凹");
            dic.Add(new PinyinInfo("ao", 2), "熬敖遨嗷獒聱螯謷翱鏖");
            dic.Add(new PinyinInfo("ao", 3), "拗袄媪");
            dic.Add(new PinyinInfo("ao", 4), "拗坳傲骜奥澳懊");

            #endregion

            #region [=====B=====]

            dic.Add(new PinyinInfo("ba", 1), "吧扒八叭巴芭疤笆粑捌");
            dic.Add(new PinyinInfo("ba", 2), "拔茇妭菝跋魃");
            dic.Add(new PinyinInfo("ba", 3), "把钯靶");
            dic.Add(new PinyinInfo("ba", 4), "把罢耙坝爸霸灞");
            dic.Add(new PinyinInfo("ba", 0), "吧罢");

            dic.Add(new PinyinInfo("bai", 1), "掰");
            dic.Add(new PinyinInfo("bai", 2), "拜白");
            dic.Add(new PinyinInfo("bai", 3), "柏伯百佰捭摆");
            dic.Add(new PinyinInfo("bai", 4), "拜呗败稗");

            dic.Add(new PinyinInfo("ban", 1), "扳攽颁班斑般搬瘢");
            dic.Add(new PinyinInfo("ban", 3), "阪坂板版钣舨");
            dic.Add(new PinyinInfo("ban", 4), "办半伴拌绊扮瓣");

            dic.Add(new PinyinInfo("bang", 1), "邦帮梆浜");
            dic.Add(new PinyinInfo("bang", 3), "膀绑榜");
            dic.Add(new PinyinInfo("bang", 4), "蚌磅搒棒傍谤蒡镑");

            dic.Add(new PinyinInfo("bao", 1), "剥炮包苞孢枹胞龅煲褒");
            dic.Add(new PinyinInfo("bao", 2), "薄雹");
            dic.Add(new PinyinInfo("bao", 3), "堡饱宝保葆褓鸨");
            dic.Add(new PinyinInfo("bao", 4), "刨曝瀑报抱鲍趵豹暴爆");

            dic.Add(new PinyinInfo("bei", 1), "背陂杯卑椑碑悲");
            dic.Add(new PinyinInfo("bei", 3), "北");
            dic.Add(new PinyinInfo("bei", 4), "背贝狈钡悖褙备惫倍焙蓓碚被辈");
            dic.Add(new PinyinInfo("bei", 0), "臂呗");

            dic.Add(new PinyinInfo("ben", 1), "奔贲锛犇");
            dic.Add(new PinyinInfo("ben", 3), "本苯");
            dic.Add(new PinyinInfo("ben", 4), "奔坌倴笨");

            dic.Add(new PinyinInfo("beng", 1), "绷崩嘣");
            dic.Add(new PinyinInfo("beng", 2), "甭");
            dic.Add(new PinyinInfo("beng", 3), "绷");
            dic.Add(new PinyinInfo("beng", 4), "蚌泵迸镚蹦");

            dic.Add(new PinyinInfo("bi", 1), "屄逼");
            dic.Add(new PinyinInfo("bi", 2), "荸鼻");
            dic.Add(new PinyinInfo("bi", 3), "匕比芘吡沘妣秕彼笔俾鄙");
            dic.Add(new PinyinInfo("bi", 4), "臂辟裨秘贲币必苾泌毖铋馝婢毕荜哔闭庇陛毙狴梐诐痹萆婢髀敝蔽弊弼赑愎蓖篦滗薜壁避璧碧");

            dic.Add(new PinyinInfo("bian", 1), "边砭萹编煸蝙鳊鞭");
            dic.Add(new PinyinInfo("bian", 3), "扁贬匾褊藊");
            dic.Add(new PinyinInfo("bian", 4), "便卞抃苄汴忭变遍辨辩辫");

            dic.Add(new PinyinInfo("biao", 1), "骠标骉彪膘镖飙镳瀌");
            dic.Add(new PinyinInfo("biao", 3), "表婊裱");
            dic.Add(new PinyinInfo("biao", 4), "摽鳔");

            dic.Add(new PinyinInfo("bie", 1), "瘪憋鳖");
            dic.Add(new PinyinInfo("bie", 2), "别蹩");
            dic.Add(new PinyinInfo("bie", 3), "瘪");
            dic.Add(new PinyinInfo("bie", 4), "别");

            dic.Add(new PinyinInfo("bin", 1), "槟宾傧滨缤彬濒");
            dic.Add(new PinyinInfo("bin", 4), "摈殡髌鬓");

            dic.Add(new PinyinInfo("bing", 1), "槟并冰兵");
            dic.Add(new PinyinInfo("bing", 3), "屏丙邴柄炳秉饼禀");
            dic.Add(new PinyinInfo("bing", 4), "并摒病");

            dic.Add(new PinyinInfo("bo", 1), "剥拨波玻菠钵哱饽播蕃");
            dic.Add(new PinyinInfo("bo", 2), "柏薄伯泊帛铂舶箔驳勃脖鹁渤馞钹亳袯博搏馎膊薄礴踣");
            dic.Add(new PinyinInfo("bo", 3), "簸跛");
            dic.Add(new PinyinInfo("bo", 4), "柏薄簸檗擘");
            dic.Add(new PinyinInfo("bo", 0), "卜啵");

            dic.Add(new PinyinInfo("bu", 3), "卜堡卟补捕哺");
            dic.Add(new PinyinInfo("bu", 4), "埔不钚布怖步部埠簿");

            #endregion

            #region [=====C=====]

            dic.Add(new PinyinInfo("ca", 1), "嚓擦");

            dic.Add(new PinyinInfo("cai", 1), "猜");
            dic.Add(new PinyinInfo("cai", 2), "才材财裁");
            dic.Add(new PinyinInfo("cai", 3), "采彩睬踩");
            dic.Add(new PinyinInfo("cai", 4), "采菜蔡");

            dic.Add(new PinyinInfo("can", 1), "参餐");
            dic.Add(new PinyinInfo("can", 2), "残蚕惭");
            dic.Add(new PinyinInfo("can", 3), "惨穇黪");
            dic.Add(new PinyinInfo("can", 4), "孱灿粲璨");

            dic.Add(new PinyinInfo("cang", 1), "仓伧苍沧舱");
            dic.Add(new PinyinInfo("cang", 2), "藏");

            dic.Add(new PinyinInfo("cao", 1), "操糙");
            dic.Add(new PinyinInfo("cao", 2), "曹嘈漕槽");
            dic.Add(new PinyinInfo("cao", 3), "草螬");

            dic.Add(new PinyinInfo("ce", 4), "侧册厕测恻策");

            dic.Add(new PinyinInfo("cen", 1), "参");
            dic.Add(new PinyinInfo("cen", 2), "岑涔");

            dic.Add(new PinyinInfo("ceng", 1), "噌");
            dic.Add(new PinyinInfo("ceng", 2), "曾层嶒");
            dic.Add(new PinyinInfo("ceng", 4), "蹭");

            dic.Add(new PinyinInfo("cha", 1), "叉杈差嚓喳插锸馇");
            dic.Add(new PinyinInfo("cha", 2), "叉查茬茶搽嵖猹碴槎察檫");
            dic.Add(new PinyinInfo("cha", 3), "叉衩蹅镲");
            dic.Add(new PinyinInfo("cha", 4), "叉衩杈差刹汊岔侘诧姹");

            dic.Add(new PinyinInfo("chai", 1), "差拆钗");
            dic.Add(new PinyinInfo("chai", 2), "侪柴豺");
            dic.Add(new PinyinInfo("chai", 3), "茝");
            dic.Add(new PinyinInfo("chai", 4), "虿");

            dic.Add(new PinyinInfo("chan", 1), "觇掺搀襜");
            dic.Add(new PinyinInfo("chan", 2), "禅孱单婵蝉谗馋巉潺缠蟾");
            dic.Add(new PinyinInfo("chan", 3), "啴产浐铲谄阐冁蒇骣");
            dic.Add(new PinyinInfo("chan", 4), "忏颤羼韂");

            dic.Add(new PinyinInfo("chang", 1), "伥昌菖猖娼");
            dic.Add(new PinyinInfo("chang", 2), "长场肠尝偿徜常嫦裳");
            dic.Add(new PinyinInfo("chang", 3), "场厂昶惝敞");
            dic.Add(new PinyinInfo("chang", 4), "怅畅倡唱");

            dic.Add(new PinyinInfo("chao", 1), "绰焯抄钞怊超");
            dic.Add(new PinyinInfo("chao", 2), "朝晁巢嘲潮");
            dic.Add(new PinyinInfo("chao", 3), "吵炒");
            dic.Add(new PinyinInfo("chao", 4), "耖");

            dic.Add(new PinyinInfo("che", 1), "车砗");
            dic.Add(new PinyinInfo("che", 3), "扯");
            dic.Add(new PinyinInfo("che", 4), "彻坼掣撤澈瞮");

            dic.Add(new PinyinInfo("chen", 1), "抻郴嗔瞋");
            dic.Add(new PinyinInfo("chen", 2), "臣尘辰宸晨忱沉陈谌");
            dic.Add(new PinyinInfo("chen", 3), "碜");
            dic.Add(new PinyinInfo("chen", 4), "称衬龀趁");

            dic.Add(new PinyinInfo("cheng", 1), "称柽琤撑铛樘瞠");
            dic.Add(new PinyinInfo("cheng", 2), "乘盛澄成诚城铖丞呈程酲承惩橙");
            dic.Add(new PinyinInfo("cheng", 3), "逞骋");
            dic.Add(new PinyinInfo("cheng", 4), "秤");

            dic.Add(new PinyinInfo("chi", 1), "吃哧蚩嗤媸鸱笞魑痴");
            dic.Add(new PinyinInfo("chi", 2), "池弛驰迟茌持匙踟踟");
            dic.Add(new PinyinInfo("chi", 3), "尺齿侈耻豉褫");
            dic.Add(new PinyinInfo("chi", 4), "彳叱斥赤饬炽翅敕啻傺");

            dic.Add(new PinyinInfo("chong", 1), "冲忡充茺舂憧艟");
            dic.Add(new PinyinInfo("chong", 2), "种重虫崇");
            dic.Add(new PinyinInfo("chong", 3), "宠");
            dic.Add(new PinyinInfo("chong", 4), "冲铳");

            dic.Add(new PinyinInfo("chou", 1), "抽瘳");
            dic.Add(new PinyinInfo("chou", 2), "仇畴筹踌惆绸稠酬愁");
            dic.Add(new PinyinInfo("chou", 3), "丑瞅");
            dic.Add(new PinyinInfo("chou", 4), "臭");

            dic.Add(new PinyinInfo("chu", 1), "出初樗");
            dic.Add(new PinyinInfo("chu", 2), "刍雏除滁蜍厨橱蹰锄躇");
            dic.Add(new PinyinInfo("chu", 3), "处杵础储褚楚");
            dic.Add(new PinyinInfo("chu", 4), "处畜亍怵绌黜搐触憷矗");

            dic.Add(new PinyinInfo("chua", 1), "歘");

            dic.Add(new PinyinInfo("chuai", 1), "揣搋");
            dic.Add(new PinyinInfo("chuai", 2), "膗");
            dic.Add(new PinyinInfo("chuai", 3), "揣");
            dic.Add(new PinyinInfo("chuai", 4), "揣嘬踹");

            dic.Add(new PinyinInfo("chuan", 1), "川氚穿");
            dic.Add(new PinyinInfo("chuan", 2), "传舡船遄椽");
            dic.Add(new PinyinInfo("chuan", 3), "舛喘");
            dic.Add(new PinyinInfo("chuan", 4), "串钏");

            dic.Add(new PinyinInfo("chuang", 1), "创疮窗");
            dic.Add(new PinyinInfo("chuang", 2), "床");
            dic.Add(new PinyinInfo("chuang", 3), "闯");
            dic.Add(new PinyinInfo("chuang", 4), "创怆");

            dic.Add(new PinyinInfo("chui", 1), "吹炊");
            dic.Add(new PinyinInfo("chui", 2), "椎垂陲棰锤捶槌");

            dic.Add(new PinyinInfo("chun", 1), "春瑃椿蝽");
            dic.Add(new PinyinInfo("chun", 2), "纯莼唇淳鹑醇");
            dic.Add(new PinyinInfo("chun", 3), "蠢");

            dic.Add(new PinyinInfo("chuo", 1), "踔戳");
            dic.Add(new PinyinInfo("chuo", 4), "绰龊啜惙辍歠");

            dic.Add(new PinyinInfo("ci", 1), "跐差刺疵");
            dic.Add(new PinyinInfo("ci", 2), "兹词祠茈雌茨瓷慈磁鹚糍辞");
            dic.Add(new PinyinInfo("ci", 3), "跐此泚");
            dic.Add(new PinyinInfo("ci", 4), "刺伺次佽赐");

            dic.Add(new PinyinInfo("cong", 1), "匆葱苁枞囱骢聪");
            dic.Add(new PinyinInfo("cong", 2), "从丛淙悰琮");

            dic.Add(new PinyinInfo("cou", 4), "凑辏腠");

            dic.Add(new PinyinInfo("cu", 1), "粗");
            dic.Add(new PinyinInfo("cu", 2), "徂殂");
            dic.Add(new PinyinInfo("cu", 4), "猝促醋蔟簇踧蹙蹴");

            dic.Add(new PinyinInfo("cuan", 1), "汆撺镩蹿");
            dic.Add(new PinyinInfo("cuan", 2), "攒");
            dic.Add(new PinyinInfo("cuan", 4), "窜篡");

            dic.Add(new PinyinInfo("cui", 1), "崔催摧");
            dic.Add(new PinyinInfo("cui", 3), "璀");
            dic.Add(new PinyinInfo("cui", 4), "脆萃啐淬悴瘁粹翠毳");

            dic.Add(new PinyinInfo("cun", 1), "村皴");
            dic.Add(new PinyinInfo("cun", 2), "蹲存");
            dic.Add(new PinyinInfo("cun", 3), "忖");
            dic.Add(new PinyinInfo("cun", 4), "寸吋");

            dic.Add(new PinyinInfo("cuo", 1), "撮搓磋蹉");
            dic.Add(new PinyinInfo("cuo", 2), "嵯矬痤酇");
            dic.Add(new PinyinInfo("cuo", 3), "脞");
            dic.Add(new PinyinInfo("cuo", 4), "挫莝锉厝措错");

            #endregion

            #region [=====D=====]

            dic.Add(new PinyinInfo("da", 1), "答哒嗒咑耷搭褡");
            dic.Add(new PinyinInfo("da", 2), "答打沓瘩达荙怛妲笪鞑靼");
            dic.Add(new PinyinInfo("da", 3), "打");
            dic.Add(new PinyinInfo("da", 4), "大");
            dic.Add(new PinyinInfo("da", 0), "瘩垯跶");

            dic.Add(new PinyinInfo("dai", 1), "待呆呔");
            dic.Add(new PinyinInfo("dai", 3), "逮歹傣");
            dic.Add(new PinyinInfo("dai", 4), "逮待大代岱玳贷袋黛迨殆怠带叇戴襶");

            dic.Add(new PinyinInfo("dan", 1), "担单丹郸殚箪眈耽聃儋");
            dic.Add(new PinyinInfo("dan", 3), "胆疸掸赕亶");
            dic.Add(new PinyinInfo("dan", 4), "担石弹澹旦但诞惮瘅萏啖淡氮蛋");

            dic.Add(new PinyinInfo("dang", 1), "当裆");
            dic.Add(new PinyinInfo("dang", 3), "挡党谠");
            dic.Add(new PinyinInfo("dang", 4), "当挡凼档砀荡宕菪");

            dic.Add(new PinyinInfo("dao", 1), "叨刀忉舠氘");
            dic.Add(new PinyinInfo("dao", 2), "捯");
            dic.Add(new PinyinInfo("dao", 3), "倒导岛捣祷蹈");
            dic.Add(new PinyinInfo("dao", 4), "倒到焘盗悼道稻");

            dic.Add(new PinyinInfo("da", 1), "嘚");
            dic.Add(new PinyinInfo("da", 2), "得锝德");
            dic.Add(new PinyinInfo("da", 0), "得的地");

            dic.Add(new PinyinInfo("dei", 3), "得");

            dic.Add(new PinyinInfo("den", 4), "扽");

            dic.Add(new PinyinInfo("deng", 1), "蹬灯登噔");
            dic.Add(new PinyinInfo("deng", 3), "等戥");
            dic.Add(new PinyinInfo("deng", 4), "蹬澄邓僜凳嶝磴瞪镫");

            dic.Add(new PinyinInfo("di", 1), "的镝氐提低羝堤滴");
            dic.Add(new PinyinInfo("di", 2), "的镝狄荻迪笛籴敌涤觌嘀嫡翟");
            dic.Add(new PinyinInfo("di", 3), "氐邸诋抵底柢砥骶");
            dic.Add(new PinyinInfo("di", 4), "的地玓弟递娣睇第帝谛蒂缔碲棣");

            dic.Add(new PinyinInfo("dia", 3), "嗲");

            dic.Add(new PinyinInfo("dian", 1), "掂滇颠巅癫");
            dic.Add(new PinyinInfo("dian", 3), "典碘点踮");
            dic.Add(new PinyinInfo("dian", 4), "佃电甸钿阽坫玷店惦垫淀靛奠殿癫");

            dic.Add(new PinyinInfo("diao", 1), "刁叼汈凋碉雕鲷貂");
            dic.Add(new PinyinInfo("diao", 3), "屌");
            dic.Add(new PinyinInfo("diao", 4), "调吊钓窎掉铫");

            dic.Add(new PinyinInfo("die", 1), "爹跌");
            dic.Add(new PinyinInfo("die", 2), "迭耋谍喋碟牒蝶蹀鲽嵽叠");

            dic.Add(new PinyinInfo("ding", 1), "丁钉酊仃叮玎盯町耵");
            dic.Add(new PinyinInfo("ding", 3), "酊顶鼎");
            dic.Add(new PinyinInfo("ding", 4), "钉订定啶腚碇锭");

            dic.Add(new PinyinInfo("diu", 1), "丢铥");

            dic.Add(new PinyinInfo("dong", 1), "东鸫冬咚氡");
            dic.Add(new PinyinInfo("dong", 3), "董懂");
            dic.Add(new PinyinInfo("dong", 4), "恫动冻栋胨洞胴侗");

            dic.Add(new PinyinInfo("dou", 1), "都兜蔸篼");
            dic.Add(new PinyinInfo("dou", 3), "斗抖蚪陡");
            dic.Add(new PinyinInfo("dou", 4), "斗读豆逗痘窦");

            dic.Add(new PinyinInfo("du", 1), "都厾嘟督");
            dic.Add(new PinyinInfo("du", 2), "读毒独渎椟犊牍黩髑");
            dic.Add(new PinyinInfo("du", 3), "肚笃堵赌睹");
            dic.Add(new PinyinInfo("du", 4), "肚度杜妒渡镀蠹");

            dic.Add(new PinyinInfo("duan", 1), "端");
            dic.Add(new PinyinInfo("duan", 3), "短");
            dic.Add(new PinyinInfo("duan", 4), "段缎椴锻断簖");

            dic.Add(new PinyinInfo("dui", 1), "堆");
            dic.Add(new PinyinInfo("dui", 4), "队对怼兑憝");

            dic.Add(new PinyinInfo("dun", 1), "蹲吨惇敦墩墩礅");
            dic.Add(new PinyinInfo("dun", 3), "盹趸");
            dic.Add(new PinyinInfo("dun", 4), "囤沌炖砘钝顿盾遁");

            dic.Add(new PinyinInfo("duo", 1), "多哆咄掇裰");
            dic.Add(new PinyinInfo("duo", 2), "度夺踱");
            dic.Add(new PinyinInfo("duo", 3), "垛朵哚躲亸");
            dic.Add(new PinyinInfo("duo", 4), "垛驮剁跺舵堕惰");

            #endregion

            #region [=====E=====]

            dic.Add(new PinyinInfo("e", 1), "阿屙婀");
            dic.Add(new PinyinInfo("e", 2), "哦讹囮俄莪峨娥锇鹅蛾额");
            dic.Add(new PinyinInfo("e", 3), "恶");
            dic.Add(new PinyinInfo("e", 4), "恶厄扼苊呃垩饿鄂谔萼愕腭颚鳄遏噩");

            dic.Add(new PinyinInfo("en", 1), "恩蒽嗯");
            dic.Add(new PinyinInfo("en", 4), "摁");

            dic.Add(new PinyinInfo("eng", 1), "鞥");

            dic.Add(new PinyinInfo("er", 2), "儿而鸸");
            dic.Add(new PinyinInfo("er", 3), "尔迩耳饵洱铒");
            dic.Add(new PinyinInfo("er", 4), "二贰");

            #endregion

            #region [=====F=====]

            dic.Add(new PinyinInfo("fa", 1), "发");
            dic.Add(new PinyinInfo("fa", 2), "乏伐垡阀筏罚");
            dic.Add(new PinyinInfo("fa", 3), "法砝");
            dic.Add(new PinyinInfo("fa", 4), "发珐");

            dic.Add(new PinyinInfo("fan", 1), "番帆幡藩翻");
            dic.Add(new PinyinInfo("fan", 2), "凡矾钒烦燔蹯樊繁蘩");
            dic.Add(new PinyinInfo("fan", 3), "反返");
            dic.Add(new PinyinInfo("fan", 4), "犯范饭贩畈泛梵");

            dic.Add(new PinyinInfo("fang", 1), "坊方邡芳枋钫");
            dic.Add(new PinyinInfo("fang", 2), "坊防妨肪房鲂");
            dic.Add(new PinyinInfo("fang", 3), "彷仿访纺舫");
            dic.Add(new PinyinInfo("fang", 4), "放");

            dic.Add(new PinyinInfo("fei", 1), "菲飞妃非啡绯扉");
            dic.Add(new PinyinInfo("fei", 2), "肥淝腓");
            dic.Add(new PinyinInfo("fei", 3), "菲匪诽悱棐斐榧翡篚");
            dic.Add(new PinyinInfo("fei", 4), "肺吠狒沸费镄废痱");

            dic.Add(new PinyinInfo("fen", 1), "分芬吩纷氛酚");
            dic.Add(new PinyinInfo("fen", 2), "坟汾棼鼢焚");
            dic.Add(new PinyinInfo("fen", 3), "粉");
            dic.Add(new PinyinInfo("fen", 4), "分份忿奋偾愤粪");

            dic.Add(new PinyinInfo("feng", 1), "丰沣风枫砜疯封葑峰烽锋蜂酆");
            dic.Add(new PinyinInfo("feng", 2), "缝冯逢");
            dic.Add(new PinyinInfo("feng", 3), "讽唪");
            dic.Add(new PinyinInfo("feng", 4), "缝凤奉俸");

            dic.Add(new PinyinInfo("fo", 2), "佛");

            dic.Add(new PinyinInfo("fou", 3), "否缶");

            dic.Add(new PinyinInfo("fu", 1), "夫呋肤麸跗稃孵敷");
            dic.Add(new PinyinInfo("fu", 2), "佛夫服扶芙弗拂怫艴氟伏茯袱凫孚俘浮蜉苻符匐幅辐福蝠涪");
            dic.Add(new PinyinInfo("fu", 3), "脯父斧釜抚甫辅拊府俯腑腐");
            dic.Add(new PinyinInfo("fu", 4), "服父讣赴付附咐驸负妇阜复腹蝮覆馥副富赋傅缚赙");

            #endregion

            #region [=====G=====]

            dic.Add(new PinyinInfo("ga", 1), "夹伽咖嘎旮");
            dic.Add(new PinyinInfo("ga", 2), "轧钆尜");
            dic.Add(new PinyinInfo("ga", 3), "嘎尕");
            dic.Add(new PinyinInfo("ga", 4), "尬");

            dic.Add(new PinyinInfo("gai", 1), "该垓荄赅");
            dic.Add(new PinyinInfo("gai", 3), "改");
            dic.Add(new PinyinInfo("gai", 4), "芥盖丐钙溉概");

            dic.Add(new PinyinInfo("gan", 1), "干杆玕肝矸竿酐甘坩苷泔柑疳尴");
            dic.Add(new PinyinInfo("gan", 3), "杆秆赶擀敢澉橄感");
            dic.Add(new PinyinInfo("gan", 4), "干旰骭绀淦赣");

            dic.Add(new PinyinInfo("gang", 1), "钢扛冈刚纲冮肛缸罡");
            dic.Add(new PinyinInfo("gang", 3), "岗港");
            dic.Add(new PinyinInfo("gang", 4), "钢杠");

            dic.Add(new PinyinInfo("gao", 1), "膏皋高篙羔糕睾");
            dic.Add(new PinyinInfo("gao", 3), "镐杲搞槁稿");
            dic.Add(new PinyinInfo("gao", 4), "膏告郜诰锆");

            dic.Add(new PinyinInfo("ge", 1), "搁咯戈圪纥疙胳袼哥歌鸽割");
            dic.Add(new PinyinInfo("ge", 2), "搁蛤葛革阁格骼隔嗝膈镉");
            dic.Add(new PinyinInfo("ge", 3), "个葛盖舸");
            dic.Add(new PinyinInfo("ge", 4), "个各硌铬虼");

            dic.Add(new PinyinInfo("gei", 3), "给");

            dic.Add(new PinyinInfo("gen", 1), "根跟");
            dic.Add(new PinyinInfo("gen", 2), "哏");
            dic.Add(new PinyinInfo("gen", 3), "艮");
            dic.Add(new PinyinInfo("gen", 4), "艮亘茛");

            dic.Add(new PinyinInfo("geng", 1), "更浭庚赓耕羹");
            dic.Add(new PinyinInfo("geng", 3), "颈埂哽绠梗鲠耿");
            dic.Add(new PinyinInfo("geng", 4), "更");

            dic.Add(new PinyinInfo("gong", 1), "供工功攻弓躬公蚣龚肱宫恭塨觥");
            dic.Add(new PinyinInfo("gong", 3), "巩汞拱珙");
            dic.Add(new PinyinInfo("gong", 4), "供共贡");

            dic.Add(new PinyinInfo("gou", 1), "勾句沟钩佝枸篝");
            dic.Add(new PinyinInfo("gou", 3), "苟岣狗枸");
            dic.Add(new PinyinInfo("gou", 4), "勾构购诟垢够媾觏彀");

            dic.Add(new PinyinInfo("gu", 1), "呱骨估咕沽姑轱鸪菇蛄辜酤孤菰觚蓇箍");
            dic.Add(new PinyinInfo("gu", 3), "骨贾鹄古诂牯钴谷汩股榾鹘羖蛊鼓臌毂瀔");
            dic.Add(new PinyinInfo("gu", 4), "估故固崮锢痼顾梏雇");

            dic.Add(new PinyinInfo("gua", 1), "呱瓜胍刮鸹");
            dic.Add(new PinyinInfo("gua", 3), "剐寡");
            dic.Add(new PinyinInfo("gua", 4), "卦诖挂褂");

            dic.Add(new PinyinInfo("guai", 1), "掴乖");
            dic.Add(new PinyinInfo("guai", 3), "拐");
            dic.Add(new PinyinInfo("guai", 4), "怪");

            dic.Add(new PinyinInfo("guan", 1), "冠观纶关官倌棺蒄瘝鳏");
            dic.Add(new PinyinInfo("guan", 3), "莞馆琯管");
            dic.Add(new PinyinInfo("guan", 4), "冠观贯掼惯祼盥灌瓘鹳罐");

            dic.Add(new PinyinInfo("guang", 1), "桄光咣胱");
            dic.Add(new PinyinInfo("guang", 3), "广犷");
            dic.Add(new PinyinInfo("guang", 4), "桄逛");

            dic.Add(new PinyinInfo("gui", 1), "归圭闺硅鲑龟妫规皈瑰");
            dic.Add(new PinyinInfo("gui", 3), "氿宄轨匦庋诡姽鬼癸晷簋");
            dic.Add(new PinyinInfo("gui", 4), "桧柜炅刿刽贵桂跪鳜");

            dic.Add(new PinyinInfo("gun", 3), "衮滚磙绲辊鲧");
            dic.Add(new PinyinInfo("gun", 4), "棍");

            dic.Add(new PinyinInfo("guo", 1), "过埚涡锅郭崞啯蝈聒");
            dic.Add(new PinyinInfo("guo", 2), "掴国帼虢");
            dic.Add(new PinyinInfo("guo", 3), "果蜾裹椁");
            dic.Add(new PinyinInfo("guo", 4), "过");

            #endregion

            #region [=====H=====]

            dic.Add(new PinyinInfo("ha", 1), "哈铪");
            dic.Add(new PinyinInfo("ha", 2), "蛤");
            dic.Add(new PinyinInfo("ha", 3), "哈");
            dic.Add(new PinyinInfo("ha", 4), "哈");

            dic.Add(new PinyinInfo("hai", 1), "咳嗨");
            dic.Add(new PinyinInfo("hai", 2), "还孩骸");
            dic.Add(new PinyinInfo("hai", 3), "胲海醢");
            dic.Add(new PinyinInfo("hai", 4), "亥骇害嗐");

            dic.Add(new PinyinInfo("han", 1), "顸鼾蚶酣憨");
            dic.Add(new PinyinInfo("han", 2), "汗邗邯含晗焓函涵韩寒");
            dic.Add(new PinyinInfo("han", 3), "罕喊");
            dic.Add(new PinyinInfo("han", 4), "汗汉旱捍悍焊蔊菡撖撼憾翰瀚");

            dic.Add(new PinyinInfo("hang", 1), "夯");
            dic.Add(new PinyinInfo("hang", 2), "行吭绗杭航");
            dic.Add(new PinyinInfo("hang", 4), "巷沆");

            dic.Add(new PinyinInfo("hao", 1), "蒿嚆薅");
            dic.Add(new PinyinInfo("hao", 2), "号貉蚝毫嗥豪壕嚎濠");
            dic.Add(new PinyinInfo("hao", 3), "好郝");
            dic.Add(new PinyinInfo("hao", 4), "好号镐昊耗浩皓滈颢灏");

            dic.Add(new PinyinInfo("he", 1), "呵喝诃嗬");
            dic.Add(new PinyinInfo("he", 2), "和核荷貉禾合盒颌何河菏劾阂曷盍阖涸");
            dic.Add(new PinyinInfo("he", 4), "和喝荷吓贺鹤褐赫壑");

            dic.Add(new PinyinInfo("hei", 1), "黑嘿");

            dic.Add(new PinyinInfo("hen", 2), "痕");
            dic.Add(new PinyinInfo("hen", 3), "狠很");
            dic.Add(new PinyinInfo("hen", 4), "恨");

            dic.Add(new PinyinInfo("heng", 1), "哼亨啈");
            dic.Add(new PinyinInfo("heng", 2), "横恒珩衡蘅");
            dic.Add(new PinyinInfo("heng", 4), "横");
            dic.Add(new PinyinInfo("heng", 0), "哼");

            dic.Add(new PinyinInfo("hong", 1), "哄吽轰烘訇薨");
            dic.Add(new PinyinInfo("hong", 2), "红荭虹鸿闳宏弘竑洪");
            dic.Add(new PinyinInfo("hong", 3), "哄");
            dic.Add(new PinyinInfo("hong", 4), "哄讧");

            dic.Add(new PinyinInfo("hou", 1), "齁");
            dic.Add(new PinyinInfo("hou", 2), "侯猴喉瘊篌糇骺");
            dic.Add(new PinyinInfo("hou", 3), "吼");
            dic.Add(new PinyinInfo("hou", 4), "侯后郈垕逅厚候堠鲎");

            dic.Add(new PinyinInfo("hu", 1), "糊乎呼轷烀滹忽惚");
            dic.Add(new PinyinInfo("hu", 2), "糊和核鹄囫狐弧胡葫猢湖瑚鹕蝴醐壶斛槲觳");
            dic.Add(new PinyinInfo("hu", 3), "浒虎唬琥");
            dic.Add(new PinyinInfo("hu", 4), "糊互冱枑户护沪戽扈怙笏瓠鹱");

            dic.Add(new PinyinInfo("hua", 1), "哗花砉");
            dic.Add(new PinyinInfo("hua", 2), "哗华划骅铧猾滑");
            dic.Add(new PinyinInfo("hua", 4), "划华化桦画婳话觟");

            dic.Add(new PinyinInfo("huai", 2), "怀徊淮槐踝耲");
            dic.Add(new PinyinInfo("huai", 4), "划坏");

            dic.Add(new PinyinInfo("huan", 1), "欢獾");
            dic.Add(new PinyinInfo("huan", 2), "还环洹桓萑寰缳");
            dic.Add(new PinyinInfo("huan", 3), "缓");
            dic.Add(new PinyinInfo("huan", 4), "幻奂换唤涣焕痪宦浣鲩患漶豢擐");

            dic.Add(new PinyinInfo("huang", 1), "肓荒塃慌");
            dic.Add(new PinyinInfo("huang", 2), "皇凰隍喤遑徨湟惶媓煌锽蝗篁艎鳇黄潢璜磺癀蟥簧");
            dic.Add(new PinyinInfo("huang", 3), "晃恍幌谎");
            dic.Add(new PinyinInfo("huang", 4), "晃滉皝");

            dic.Add(new PinyinInfo("hui", 1), "灰诙恢挥晖辉麾徽隳");
            dic.Add(new PinyinInfo("hui", 2), "回茴洄蛔");
            dic.Add(new PinyinInfo("hui", 3), "悔毁");
            dic.Add(new PinyinInfo("hui", 4), "会溃桧卉汇荟浍绘烩讳诲晦恚贿彗慧秽惠蕙蟪喙");

            dic.Add(new PinyinInfo("hun", 1), "昏阍惛婚荤");
            dic.Add(new PinyinInfo("hun", 2), "混浑珲馄魂");
            dic.Add(new PinyinInfo("hun", 4), "混诨溷");

            dic.Add(new PinyinInfo("huo", 1), "豁嚄耠劐攉");
            dic.Add(new PinyinInfo("huo", 2), "和活");
            dic.Add(new PinyinInfo("huo", 3), "火伙钬夥");
            dic.Add(new PinyinInfo("huo", 4), "和豁或惑货获祸霍藿嚯镬");

            #endregion

            #region [=====J=====]

            dic.Add(new PinyinInfo("ji", 1), "几奇稽缉讥叽饥玑机肌矶击圾芨乩鸡笄剞犄畸跻唧积屐姬基箕嵇齑畿墼激羁");
            dic.Add(new PinyinInfo("ji", 2), "亟及岌汲级极笈吉佶即急疾蒺嫉棘集楫辑瘠藉籍");
            dic.Add(new PinyinInfo("ji", 3), "给几济纪虮麂己挤脊掎戟");
            dic.Add(new PinyinInfo("ji", 4), "济纪系荠计记忌伎技妓际季悸剂迹既暨绩觊继偈寄祭寂稷鲫髻冀骥");

            dic.Add(new PinyinInfo("jia", 1), "夹伽茄加枷痂袈嘉浃佳家镓");
            dic.Add(new PinyinInfo("jia", 2), "夹荚颊戛");
            dic.Add(new PinyinInfo("jia", 3), "贾假甲胛钾");
            dic.Add(new PinyinInfo("jia", 4), "假价驾架嫁稼");

            dic.Add(new PinyinInfo("jian", 1), "间监渐浅戋笺尖奸歼坚肩艰兼菅煎缄");
            dic.Add(new PinyinInfo("jian", 3), "锏拣茧柬俭捡检睑减碱剪简");
            dic.Add(new PinyinInfo("jian", 4), "间监渐锏见舰件涧饯贱践溅建健毽腱键荐剑鉴谏僭箭");

            dic.Add(new PinyinInfo("jiang", 1), "将浆江豇姜僵缰疆");
            dic.Add(new PinyinInfo("jiang", 3), "讲奖桨蒋");
            dic.Add(new PinyinInfo("jiang", 4), "将浆降强匠酱犟糨");

            dic.Add(new PinyinInfo("jiao", 1), "教交郊姣胶蛟跤浇娇骄椒焦蕉礁");
            dic.Add(new PinyinInfo("jiao", 2), "嚼矫");
            dic.Add(new PinyinInfo("jiao", 3), "矫角侥佼狡饺绞铰皎脚搅缴剿");
            dic.Add(new PinyinInfo("jiao", 4), "教嚼觉校叫轿较酵窖");

            dic.Add(new PinyinInfo("jie", 1), "节结阶皆秸接揭嗟街");
            dic.Add(new PinyinInfo("jie", 2), "节桔结孑讦诘洁杰捷睫碣竭截劫");
            dic.Add(new PinyinInfo("jie", 3), "解姐");
            dic.Add(new PinyinInfo("jie", 4), "芥介界疥戒诫届借");

            dic.Add(new PinyinInfo("jin", 1), "禁巾斤今矜金津筋荆襟");
            dic.Add(new PinyinInfo("jin", 3), "尽仅紧谨锦");
            dic.Add(new PinyinInfo("jin", 4), "尽禁劲烬进近晋浸噤觐");

            dic.Add(new PinyinInfo("jing", 1), "茎泾经京惊鲸荆菁睛精晶粳兢");
            dic.Add(new PinyinInfo("jing", 3), "颈井阱刭景儆警");
            dic.Add(new PinyinInfo("jing", 4), "劲径痉净静竞竟境镜靖敬");

            dic.Add(new PinyinInfo("jiong", 3), "炯迥窘");

            dic.Add(new PinyinInfo("jiu", 1), "纠赳鸠究阄揪啾");
            dic.Add(new PinyinInfo("jiu", 3), "氿九久玖灸韭酒");
            dic.Add(new PinyinInfo("jiu", 4), "旧臼舅咎柩救厩就鹫");

            dic.Add(new PinyinInfo("ju", 1), "车狙雎拘驹居掬鞠");
            dic.Add(new PinyinInfo("ju", 2), "桔局焗菊橘");
            dic.Add(new PinyinInfo("ju", 3), "矩咀沮龃举");
            dic.Add(new PinyinInfo("ju", 4), "句巨拒炬距具俱惧飓剧据锯踞聚");

            dic.Add(new PinyinInfo("juan", 1), "圈捐涓娟鹃");
            dic.Add(new PinyinInfo("juan", 3), "卷");
            dic.Add(new PinyinInfo("juan", 4), "圈卷倦眷隽绢");

            dic.Add(new PinyinInfo("jue", 1), "撅噘");
            dic.Add(new PinyinInfo("jue", 2), "蹶嚼角觉倔决诀抉绝掘崛厥蕨谲爵矍");
            dic.Add(new PinyinInfo("jue", 3), "蹶");
            dic.Add(new PinyinInfo("jue", 4), "倔");

            dic.Add(new PinyinInfo("jun", 1), "军皲均钧君菌");
            dic.Add(new PinyinInfo("jun", 4), "俊峻骏竣郡");

            #endregion

            #region [=====K=====]

            dic.Add(new PinyinInfo("ka", 1), "咖咔");
            dic.Add(new PinyinInfo("ka", 3), "卡咯");

            dic.Add(new PinyinInfo("kai", 1), "开揩");
            dic.Add(new PinyinInfo("kai", 3), "凯铠楷慨");
            dic.Add(new PinyinInfo("kai", 4), "忾");

            dic.Add(new PinyinInfo("kan", 1), "看刊勘堪龛");
            dic.Add(new PinyinInfo("kan", 3), "坎砍侃槛");
            dic.Add(new PinyinInfo("kan", 4), "看瞰");

            dic.Add(new PinyinInfo("kang", 1), "康慷糠");
            dic.Add(new PinyinInfo("kang", 2), "扛");
            dic.Add(new PinyinInfo("kang", 4), "亢伉抗炕");

            dic.Add(new PinyinInfo("kao", 3), "考拷烤");
            dic.Add(new PinyinInfo("kao", 4), "靠铐犒");

            dic.Add(new PinyinInfo("ke", 1), "苛轲科蝌棵稞窠颗磕瞌搕");
            dic.Add(new PinyinInfo("ke", 2), "咳壳");
            dic.Add(new PinyinInfo("ke", 3), "可渴坷");
            dic.Add(new PinyinInfo("ke", 4), "可克刻恪客课嗑");

            dic.Add(new PinyinInfo("ken", 3), "肯啃垦恳");

            dic.Add(new PinyinInfo("keng", 1), "吭坑铿");

            dic.Add(new PinyinInfo("kong", 1), "空");
            dic.Add(new PinyinInfo("kong", 3), "孔恐");
            dic.Add(new PinyinInfo("kong", 4), "空控");

            dic.Add(new PinyinInfo("kou", 1), "抠");
            dic.Add(new PinyinInfo("kou", 3), "口");
            dic.Add(new PinyinInfo("kou", 4), "叩扣寇蔻");

            dic.Add(new PinyinInfo("ku", 1), "枯骷哭窟");
            dic.Add(new PinyinInfo("ku", 3), "苦");
            dic.Add(new PinyinInfo("ku", 4), "库裤酷绔");

            dic.Add(new PinyinInfo("kua", 1), "夸");
            dic.Add(new PinyinInfo("kua", 3), "垮");
            dic.Add(new PinyinInfo("kua", 4), "挎胯跨");

            dic.Add(new PinyinInfo("kuai", 4), "会侩脍块快筷");

            dic.Add(new PinyinInfo("kuan", 1), "宽髋");
            dic.Add(new PinyinInfo("kuan", 3), "款");

            dic.Add(new PinyinInfo("kuang", 1), "筐诓哐");
            dic.Add(new PinyinInfo("kuang", 2), "狂诳");
            dic.Add(new PinyinInfo("kuang", 4), "旷矿况框眶");

            dic.Add(new PinyinInfo("kui", 1), "亏盔窥");
            dic.Add(new PinyinInfo("kui", 2), "葵魁");
            dic.Add(new PinyinInfo("kui", 3), "傀");
            dic.Add(new PinyinInfo("kui", 4), "溃匮馈溃愧");

            dic.Add(new PinyinInfo("kun", 1), "昆坤鲲");
            dic.Add(new PinyinInfo("kun", 3), "捆");
            dic.Add(new PinyinInfo("kun", 4), "困");

            dic.Add(new PinyinInfo("kuo", 4), "扩括阔廓");

            #endregion

            #region [=====L=====]

            dic.Add(new PinyinInfo("la", 1), "拉啦垃邋");
            dic.Add(new PinyinInfo("la", 2), "拉旯");
            dic.Add(new PinyinInfo("la", 3), "喇");
            dic.Add(new PinyinInfo("la", 4), "落腊蜡辣");
            dic.Add(new PinyinInfo("la", 0), "啦");

            dic.Add(new PinyinInfo("lai", 2), "来莱徕");
            dic.Add(new PinyinInfo("lai", 4), "睐赖籁癞");

            dic.Add(new PinyinInfo("lan", 2), "兰拦栏岚婪阑澜蓝褴篮");
            dic.Add(new PinyinInfo("lan", 3), "览揽缆榄懒");
            dic.Add(new PinyinInfo("lan", 4), "烂滥");

            dic.Add(new PinyinInfo("lang", 1), "啷");
            dic.Add(new PinyinInfo("lang", 2), "郎廊榔螂狼琅锒");
            dic.Add(new PinyinInfo("lang", 3), "朗");
            dic.Add(new PinyinInfo("lang", 4), "郎浪");

            dic.Add(new PinyinInfo("lao", 1), "捞");
            dic.Add(new PinyinInfo("lao", 2), "唠劳崂牢");
            dic.Add(new PinyinInfo("lao", 3), "老佬姥");
            dic.Add(new PinyinInfo("lao", 4), "唠落烙酪涝");

            dic.Add(new PinyinInfo("le", 4), "乐勒");
            dic.Add(new PinyinInfo("le", 0), "了");

            dic.Add(new PinyinInfo("lei", 1), "勒");
            dic.Add(new PinyinInfo("lei", 2), "累擂雷羸");
            dic.Add(new PinyinInfo("lei", 3), "累垒磊蕾儡");
            dic.Add(new PinyinInfo("lei", 4), "累擂肋泪类");
            dic.Add(new PinyinInfo("lei", 0), "嘞");

            dic.Add(new PinyinInfo("leng", 2), "棱");
            dic.Add(new PinyinInfo("leng", 3), "冷");
            dic.Add(new PinyinInfo("leng", 4), "楞睖");

            dic.Add(new PinyinInfo("li", 1), "哩");
            dic.Add(new PinyinInfo("li", 2), "丽鹂厘喱狸离漓篱梨犁黎罹蠡");
            dic.Add(new PinyinInfo("li", 3), "哩礼李里俚娌理鲤逦");
            dic.Add(new PinyinInfo("li", 4), "丽力荔历呖沥枥雳厉励蛎立莅笠粒吏利俐莉痢例戾唳隶砾栗");
            dic.Add(new PinyinInfo("li", 0), "哩璃蜊");

            dic.Add(new PinyinInfo("lia", 3), "俩");

            dic.Add(new PinyinInfo("lian", 2), "连莲涟鲢怜帘联廉镰");
            dic.Add(new PinyinInfo("lian", 3), "敛脸");
            dic.Add(new PinyinInfo("lian", 4), "练炼恋殓链");

            dic.Add(new PinyinInfo("liang", 2), "凉量良粮梁粱");
            dic.Add(new PinyinInfo("liang", 3), "俩两魉");
            dic.Add(new PinyinInfo("liang", 4), "凉量亮谅晾踉辆靓");

            dic.Add(new PinyinInfo("liao", 1), "撩");
            dic.Add(new PinyinInfo("liao", 2), "撩燎辽疗聊僚嘹獠潦缭寥");
            dic.Add(new PinyinInfo("liao", 3), "了燎");
            dic.Add(new PinyinInfo("liao", 4), "料撂瞭镣");

            dic.Add(new PinyinInfo("lie", 3), "咧");
            dic.Add(new PinyinInfo("lie", 4), "列冽冽烈裂趔劣猎");

            dic.Add(new PinyinInfo("lin", 1), "拎");
            dic.Add(new PinyinInfo("lin", 2), "淋邻林琳霖临粼潾嶙磷鳞麟");
            dic.Add(new PinyinInfo("lin", 3), "凛檩");
            dic.Add(new PinyinInfo("lin", 4), "淋吝赁躏");

            dic.Add(new PinyinInfo("ling", 2), "伶苓囹泠玲瓴铃聆蛉翎羚零龄灵棂凌陵菱绫");
            dic.Add(new PinyinInfo("ling", 3), "岭领");
            dic.Add(new PinyinInfo("ling", 4), "另令");

            dic.Add(new PinyinInfo("liu", 1), "溜熘溜");
            dic.Add(new PinyinInfo("liu", 2), "馏刘浏留榴瘤流琉硫");
            dic.Add(new PinyinInfo("liu", 3), "柳绺");
            dic.Add(new PinyinInfo("liu", 4), "溜馏碌六陆遛");

            dic.Add(new PinyinInfo("lo", 0), "咯");

            dic.Add(new PinyinInfo("long", 2), "笼龙咙珑胧聋隆窿");
            dic.Add(new PinyinInfo("long", 3), "笼陇拢垄");

            dic.Add(new PinyinInfo("lou", 1), "搂");
            dic.Add(new PinyinInfo("lou", 2), "喽偻娄楼髅");
            dic.Add(new PinyinInfo("lou", 3), "搂篓");
            dic.Add(new PinyinInfo("lou", 4), "露陋镂漏");
            dic.Add(new PinyinInfo("lou", 0), "喽");

            dic.Add(new PinyinInfo("lu", 1), "撸");
            dic.Add(new PinyinInfo("lu", 2), "卢泸鸬颅鲈芦庐炉");
            dic.Add(new PinyinInfo("lu", 3), "卤虏掳鲁橹");
            dic.Add(new PinyinInfo("lu", 4), "碌露绿陆录禄箓赂鹿漉麓路鹭戮");

            dic.Add(new PinyinInfo("lv", 2), "驴榈");
            dic.Add(new PinyinInfo("lv", 3), "偻捋吕侣铝旅膂屡缕褛履");
            dic.Add(new PinyinInfo("lv", 4), "绿率律虑滤氯");

            dic.Add(new PinyinInfo("luan", 2), "峦孪挛鸾");
            dic.Add(new PinyinInfo("luan", 3), "卵");
            dic.Add(new PinyinInfo("luan", 4), "乱");

            dic.Add(new PinyinInfo("lve", 4), "掠略");

            dic.Add(new PinyinInfo("lun", 1), "抡");
            dic.Add(new PinyinInfo("lun", 2), "抡纶论仑伦论囵沦轮");
            dic.Add(new PinyinInfo("lun", 4), "论");

            dic.Add(new PinyinInfo("luo", 1), "捋啰");
            dic.Add(new PinyinInfo("luo", 2), "啰罗萝逻锣箩骡螺");
            dic.Add(new PinyinInfo("luo", 3), "裸");
            dic.Add(new PinyinInfo("luo", 4), "落洛洛骆络落摞漯");
            dic.Add(new PinyinInfo("luo", 0), "啰");

            #endregion

            #region [=====M=====]

            dic.Add(new PinyinInfo("ma", 1), "抹蚂妈");
            dic.Add(new PinyinInfo("ma", 2), "吗犸玛码麻");
            dic.Add(new PinyinInfo("ma", 3), "蚂蚂吗马玛码");
            dic.Add(new PinyinInfo("ma", 4), "蚂蚂骂");
            dic.Add(new PinyinInfo("ma", 0), "吗么嘛蟆");

            dic.Add(new PinyinInfo("mai", 2), "埋霾");
            dic.Add(new PinyinInfo("mai", 3), "买");
            dic.Add(new PinyinInfo("mai", 4), "脉迈麦卖霡");

            dic.Add(new PinyinInfo("man", 2), "埋谩蔓蛮馒瞒");
            dic.Add(new PinyinInfo("man", 3), "满");
            dic.Add(new PinyinInfo("man", 4), "谩蔓曼幔漫慢");

            dic.Add(new PinyinInfo("mang", 1), "牤");
            dic.Add(new PinyinInfo("mang", 2), "芒忙盲氓茫");
            dic.Add(new PinyinInfo("mang", 3), "莽漭蟒");

            dic.Add(new PinyinInfo("mao", 1), "猫");
            dic.Add(new PinyinInfo("mao", 2), "猫毛牦髦矛茅蝥蟊锚");
            dic.Add(new PinyinInfo("mao", 3), "冇卯铆");
            dic.Add(new PinyinInfo("mao", 4), "貌茂冒帽贸袤");

            dic.Add(new PinyinInfo("me", 0), "么");

            dic.Add(new PinyinInfo("mei", 2), "没玫枚眉楣莓梅酶霉媒煤");
            dic.Add(new PinyinInfo("mei", 3), "每美镁");
            dic.Add(new PinyinInfo("mei", 4), "妹昧寐魅袂媚");

            dic.Add(new PinyinInfo("men", 1), "闷");
            dic.Add(new PinyinInfo("men", 2), "门扪");
            dic.Add(new PinyinInfo("men", 4), "闷焖懑");
            dic.Add(new PinyinInfo("men", 0), "们");

            dic.Add(new PinyinInfo("meng", 1), "蒙");
            dic.Add(new PinyinInfo("meng", 2), "蒙虻萌盟檬曚朦");
            dic.Add(new PinyinInfo("meng", 3), "蒙猛锰蜢懵");
            dic.Add(new PinyinInfo("meng", 4), "孟梦");

            dic.Add(new PinyinInfo("mi", 1), "眯咪");
            dic.Add(new PinyinInfo("mi", 2), "眯靡弥猕迷谜醚糜麋");
            dic.Add(new PinyinInfo("mi", 3), "靡米弭");
            dic.Add(new PinyinInfo("mi", 4), "秘觅泌密谧蜜幂");

            dic.Add(new PinyinInfo("mian", 2), "眠绵棉");
            dic.Add(new PinyinInfo("mian", 3), "免勉娩冕湎缅腼渑");
            dic.Add(new PinyinInfo("mian", 4), "面");

            dic.Add(new PinyinInfo("miao", 1), "喵");
            dic.Add(new PinyinInfo("miao", 2), "苗描瞄");
            dic.Add(new PinyinInfo("miao", 3), "秒渺缥藐");
            dic.Add(new PinyinInfo("miao", 4), "妙庙");

            dic.Add(new PinyinInfo("mie", 1), "乜咩");
            dic.Add(new PinyinInfo("mie", 4), "灭蔑篾");

            dic.Add(new PinyinInfo("min", 2), "民");
            dic.Add(new PinyinInfo("min", 3), "皿悯闽抿泯敏");

            dic.Add(new PinyinInfo("ming", 2), "名茗铭明鸣冥瞑螟");
            dic.Add(new PinyinInfo("ming", 3), "酩");
            dic.Add(new PinyinInfo("ming", 4), "命");

            dic.Add(new PinyinInfo("miu", 4), "谬");

            dic.Add(new PinyinInfo("mo", 1), "摸");
            dic.Add(new PinyinInfo("mo", 2), "模磨馍摹摩膜嬷蘑魔");
            dic.Add(new PinyinInfo("mo", 3), "抹");
            dic.Add(new PinyinInfo("mo", 4), "抹没磨脉末茉沫秣殁陌莫蓦漠寞墨默");

            dic.Add(new PinyinInfo("mou", 1), "哞");
            dic.Add(new PinyinInfo("mou", 2), "牟眸谋缪");
            dic.Add(new PinyinInfo("mou", 3), "某");

            dic.Add(new PinyinInfo("mu", 2), "模");
            dic.Add(new PinyinInfo("mu", 3), "母拇姆牡亩");
            dic.Add(new PinyinInfo("mu", 4), "牟木沐目苜钼牧募墓幕慕暮睦穆");

            #endregion

            #region [=====N=====]

            dic.Add(new PinyinInfo("na", 1), "那");
            dic.Add(new PinyinInfo("na", 2), "拿");
            dic.Add(new PinyinInfo("na", 3), "哪");
            dic.Add(new PinyinInfo("na", 4), "那娜呐纳钠衲捺");
            dic.Add(new PinyinInfo("na", 0), "哪");

            dic.Add(new PinyinInfo("nai", 3), "乃奶氖");
            dic.Add(new PinyinInfo("nai", 4), "奈耐");

            dic.Add(new PinyinInfo("nan", 1), "囡");
            dic.Add(new PinyinInfo("nan", 2), "难男南喃楠");
            dic.Add(new PinyinInfo("nan", 3), "赧腩");
            dic.Add(new PinyinInfo("nan", 4), "难");

            dic.Add(new PinyinInfo("nang", 1), "囔");
            dic.Add(new PinyinInfo("nang", 2), "囊");

            dic.Add(new PinyinInfo("nao", 1), "孬");
            dic.Add(new PinyinInfo("nao", 2), "呶挠");
            dic.Add(new PinyinInfo("nao", 3), "恼脑瑙");
            dic.Add(new PinyinInfo("nao", 4), "闹淖");

            dic.Add(new PinyinInfo("ne", 2), "哪");
            dic.Add(new PinyinInfo("ne", 4), "讷");
            dic.Add(new PinyinInfo("ne", 0), "呢");

            dic.Add(new PinyinInfo("nei", 3), "哪馁");
            dic.Add(new PinyinInfo("nei", 4), "那内");

            dic.Add(new PinyinInfo("nen", 4), "恁嫩");

            dic.Add(new PinyinInfo("neng", 2), "能");

            dic.Add(new PinyinInfo("ni", 1), "妮");
            dic.Add(new PinyinInfo("ni", 2), "呢泥尼怩倪霓");
            dic.Add(new PinyinInfo("ni", 3), "拟你旎");
            dic.Add(new PinyinInfo("ni", 4), "泥昵逆匿腻溺");

            dic.Add(new PinyinInfo("nian", 1), "拈蔫");
            dic.Add(new PinyinInfo("nian", 2), "年黏");
            dic.Add(new PinyinInfo("nian", 3), "捻辇撵碾");
            dic.Add(new PinyinInfo("nian", 4), "廿念");

            dic.Add(new PinyinInfo("niang", 2), "娘");
            dic.Add(new PinyinInfo("niang", 4), "酿");

            dic.Add(new PinyinInfo("niao", 3), "鸟袅");
            dic.Add(new PinyinInfo("niao", 4), "尿");

            dic.Add(new PinyinInfo("nie", 1), "捏");
            dic.Add(new PinyinInfo("nie", 4), "涅聂嗫镊蹑镍孽");

            dic.Add(new PinyinInfo("nin", 2), "您");

            dic.Add(new PinyinInfo("ning", 2), "宁拧咛狞柠凝");
            dic.Add(new PinyinInfo("ning", 3), "拧");
            dic.Add(new PinyinInfo("ning", 4), "宁拧泞佞");

            dic.Add(new PinyinInfo("niu", 1), "妞");
            dic.Add(new PinyinInfo("niu", 2), "牛");
            dic.Add(new PinyinInfo("niu", 3), "扭忸纽钮");
            dic.Add(new PinyinInfo("niu", 4), "拗");

            dic.Add(new PinyinInfo("nong", 2), "农哝浓脓");
            dic.Add(new PinyinInfo("nong", 4), "弄");

            dic.Add(new PinyinInfo("nu", 2), "奴驽");
            dic.Add(new PinyinInfo("nu", 3), "努弩");
            dic.Add(new PinyinInfo("nu", 4), "怒");

            dic.Add(new PinyinInfo("nv", 3), "女");

            dic.Add(new PinyinInfo("nuan", 3), "暖");

            dic.Add(new PinyinInfo("nue", 4), "疟虐");

            dic.Add(new PinyinInfo("nuo", 2), "娜挪");
            dic.Add(new PinyinInfo("nuo", 4), "诺喏懦糯");

            #endregion

            #region [=====O=====]

            dic.Add(new PinyinInfo("o", 1), "噢");
            dic.Add(new PinyinInfo("o", 2), "哦");
            dic.Add(new PinyinInfo("o", 3), "嚄");
            dic.Add(new PinyinInfo("o", 4), "哦");

            dic.Add(new PinyinInfo("ou", 1), "区讴瓯欧殴鸥");
            dic.Add(new PinyinInfo("ou", 3), "呕偶藕");
            dic.Add(new PinyinInfo("ou", 4), "怄");

            #endregion

            #region [=====P=====]

            dic.Add(new PinyinInfo("pa", 1), "趴葩啪");
            dic.Add(new PinyinInfo("pa", 2), "扒耙杷爬琶");
            dic.Add(new PinyinInfo("pa", 4), "帕怕");

            dic.Add(new PinyinInfo("pai", 1), "拍");
            dic.Add(new PinyinInfo("pai", 2), "排徘牌");
            dic.Add(new PinyinInfo("pai", 3), "迫");
            dic.Add(new PinyinInfo("pai", 4), "哌派湃");

            dic.Add(new PinyinInfo("pan", 1), "番潘攀");
            dic.Add(new PinyinInfo("pan", 2), "胖爿盘磐蟠蹒");
            dic.Add(new PinyinInfo("pan", 4), "判叛畔盼");

            dic.Add(new PinyinInfo("pang", 1), "膀乓滂");
            dic.Add(new PinyinInfo("pang", 2), "膀彷磅庞旁螃");
            dic.Add(new PinyinInfo("pang", 3), "耪");
            dic.Add(new PinyinInfo("pang", 4), "胖");

            dic.Add(new PinyinInfo("pao", 1), "泡抛");
            dic.Add(new PinyinInfo("pao", 2), "炮刨咆袍庖袍");
            dic.Add(new PinyinInfo("pao", 3), "跑");
            dic.Add(new PinyinInfo("pao", 4), "炮泡");

            dic.Add(new PinyinInfo("pei", 1), "呸胚");
            dic.Add(new PinyinInfo("pei", 2), "陪培赔裴");
            dic.Add(new PinyinInfo("pei", 4), "沛佩配");

            dic.Add(new PinyinInfo("pen", 1), "喷");
            dic.Add(new PinyinInfo("pen", 2), "盆");
            dic.Add(new PinyinInfo("pen", 4), "喷");

            dic.Add(new PinyinInfo("peng", 1), "抨怦砰烹嘭");
            dic.Add(new PinyinInfo("peng", 2), "搒朋棚硼鹏彭澎膨蓬篷");
            dic.Add(new PinyinInfo("peng", 3), "捧");
            dic.Add(new PinyinInfo("peng", 4), "碰");

            dic.Add(new PinyinInfo("pi", 1), "劈丕坯批纰砒披噼霹");
            dic.Add(new PinyinInfo("pi", 2), "裨陂皮疲枇毗蚍琵貔啤脾");
            dic.Add(new PinyinInfo("pi", 3), "否劈匹痞癖");
            dic.Add(new PinyinInfo("pi", 4), "辟屁媲睥僻譬");

            dic.Add(new PinyinInfo("pian", 1), "片扁偏篇翩");
            dic.Add(new PinyinInfo("pian", 2), "便骈蹁");
            dic.Add(new PinyinInfo("pian", 4), "片骗");

            dic.Add(new PinyinInfo("piao", 1), "漂剽飘");
            dic.Add(new PinyinInfo("piao", 2), "朴瓢");
            dic.Add(new PinyinInfo("piao", 3), "漂殍瞟");
            dic.Add(new PinyinInfo("piao", 4), "漂骠票");

            dic.Add(new PinyinInfo("pie", 1), "撇氕瞥");
            dic.Add(new PinyinInfo("pie", 3), "撇");

            dic.Add(new PinyinInfo("pin", 1), "拼姘");
            dic.Add(new PinyinInfo("pin", 2), "贫频颦嫔");
            dic.Add(new PinyinInfo("pin", 3), "品");
            dic.Add(new PinyinInfo("pin", 4), "聘");

            dic.Add(new PinyinInfo("ping", 1), "乒娉");
            dic.Add(new PinyinInfo("ping", 2), "屏平评坪苹萍凭瓶");

            dic.Add(new PinyinInfo("po", 1), "朴泊陂钋坡颇泼");
            dic.Add(new PinyinInfo("po", 2), "婆鄱");
            dic.Add(new PinyinInfo("po", 3), "叵");
            dic.Add(new PinyinInfo("po", 4), "迫朴珀粕魄破");

            dic.Add(new PinyinInfo("po", 1), "剖");
            dic.Add(new PinyinInfo("po", 2), "抔");

            dic.Add(new PinyinInfo("pu", 1), "仆铺扑噗");
            dic.Add(new PinyinInfo("pu", 2), "仆脯匍葡蒲菩璞");
            dic.Add(new PinyinInfo("pu", 3), "朴埔浦圃普谱蹼");
            dic.Add(new PinyinInfo("pu", 4), "曝铺堡瀑");

            #endregion

            #region [=====Q=====]

            dic.Add(new PinyinInfo("qi", 1), "栖蹊缉七柒沏妻凄萋戚嘁期欺漆");
            dic.Add(new PinyinInfo("qi", 2), "奇跂荠歧齐脐祈芪祇其琪棋旗麒崎骑俟鳍畦");
            dic.Add(new PinyinInfo("qi", 3), "稽跂乞岂杞起企启绮");
            dic.Add(new PinyinInfo("qi", 4), "亟气汽讫迄弃泣契砌葺器憩");

            dic.Add(new PinyinInfo("qia", 1), "掐");
            dic.Add(new PinyinInfo("qia", 3), "卡");
            dic.Add(new PinyinInfo("qia", 4), "洽恰");

            dic.Add(new PinyinInfo("qian", 1), "铅千仟阡芊迁钎签牵悭谦愆");
            dic.Add(new PinyinInfo("qian", 2), "黔前虔钱钳乾潜");
            dic.Add(new PinyinInfo("qian", 3), "浅遣谴");
            dic.Add(new PinyinInfo("qian", 4), "纤茜欠芡嵌倩堑歉");

            dic.Add(new PinyinInfo("qiang", 1), "呛枪羌蜣戕腔锵");
            dic.Add(new PinyinInfo("qiang", 2), "强墙蔷");
            dic.Add(new PinyinInfo("qiang", 3), "强呛抢羟襁");
            dic.Add(new PinyinInfo("qiang", 4), "炝跄");

            dic.Add(new PinyinInfo("qiao", 1), "雀悄跷敲锹橇");
            dic.Add(new PinyinInfo("qiao", 2), "翘乔侨荞桥憔瞧");
            dic.Add(new PinyinInfo("qiao", 3), "雀悄巧");
            dic.Add(new PinyinInfo("qiao", 4), "壳翘鞘俏诮峭窍撬");

            dic.Add(new PinyinInfo("qie", 1), "切");
            dic.Add(new PinyinInfo("qie", 2), "伽茄");
            dic.Add(new PinyinInfo("qie", 3), "且");
            dic.Add(new PinyinInfo("qie", 4), "切窃妾怯挈锲惬趄");

            dic.Add(new PinyinInfo("qin", 1), "亲钦侵衾");
            dic.Add(new PinyinInfo("qin", 2), "覃芹芩琴秦禽擒噙勤");
            dic.Add(new PinyinInfo("qin", 3), "寝");
            dic.Add(new PinyinInfo("qin", 4), "沁");

            dic.Add(new PinyinInfo("qing", 1), "青清蜻轻氢倾卿");
            dic.Add(new PinyinInfo("qing", 2), "情晴氰擎");
            dic.Add(new PinyinInfo("qing", 3), "顷请");
            dic.Add(new PinyinInfo("qing", 4), "亲庆罄");

            dic.Add(new PinyinInfo("qiong", 2), "穷穹琼");

            dic.Add(new PinyinInfo("qiu", 1), "丘蚯秋鳅");
            dic.Add(new PinyinInfo("qiu", 2), "仇囚泅求球裘虬酋遒");

            dic.Add(new PinyinInfo("qu", 1), "曲区岖驱躯蛐屈蛆黢趋");
            dic.Add(new PinyinInfo("qu", 2), "渠瞿");
            dic.Add(new PinyinInfo("qu", 3), "曲取娶龋");
            dic.Add(new PinyinInfo("qu", 4), "去趣觑");

            dic.Add(new PinyinInfo("quan", 1), "圈悛");
            dic.Add(new PinyinInfo("quan", 2), "权全诠痊醛泉拳蜷");
            dic.Add(new PinyinInfo("quan", 3), "犬");
            dic.Add(new PinyinInfo("quan", 4), "劝券");

            dic.Add(new PinyinInfo("que", 1), "阙炔缺");
            dic.Add(new PinyinInfo("que", 2), "瘸");
            dic.Add(new PinyinInfo("que", 4), "阙雀却确榷鹊");

            dic.Add(new PinyinInfo("qun", 1), "逡");
            dic.Add(new PinyinInfo("qun", 2), "裙群");

            #endregion

            #region [=====R=====]

            dic.Add(new PinyinInfo("ran", 2), "蚺然燃");
            dic.Add(new PinyinInfo("ran", 3), "冉苒染");

            dic.Add(new PinyinInfo("rang", 1), "嚷");
            dic.Add(new PinyinInfo("rang", 2), "瓤");
            dic.Add(new PinyinInfo("rang", 3), "嚷壤攘");
            dic.Add(new PinyinInfo("rang", 4), "让");

            dic.Add(new PinyinInfo("rao", 2), "娆荛饶桡");
            dic.Add(new PinyinInfo("rao", 3), "娆扰");
            dic.Add(new PinyinInfo("rao", 4), "绕");

            dic.Add(new PinyinInfo("re", 3), "惹");
            dic.Add(new PinyinInfo("re", 4), "热");

            dic.Add(new PinyinInfo("ren", 2), "任人壬仁");
            dic.Add(new PinyinInfo("ren", 3), "忍荏稔");
            dic.Add(new PinyinInfo("ren", 4), "任刃纫韧认饪");

            dic.Add(new PinyinInfo("reng", 1), "扔");
            dic.Add(new PinyinInfo("reng", 2), "仍");

            dic.Add(new PinyinInfo("ri", 4), "日");

            dic.Add(new PinyinInfo("rong", 2), "戎绒茸荣嵘容蓉榕熔融溶");
            dic.Add(new PinyinInfo("rong", 3), "冗氄");

            dic.Add(new PinyinInfo("rou", 2), "柔揉糅蹂鞣");
            dic.Add(new PinyinInfo("rou", 4), "肉");

            dic.Add(new PinyinInfo("ru", 2), "如茹铷儒蠕孺蠕");
            dic.Add(new PinyinInfo("ru", 3), "汝乳辱");
            dic.Add(new PinyinInfo("ru", 4), "入缛褥");

            dic.Add(new PinyinInfo("ruan", 3), "阮软");

            dic.Add(new PinyinInfo("rui", 3), "蕊");
            dic.Add(new PinyinInfo("rui", 4), "蚋锐瑞睿");

            dic.Add(new PinyinInfo("run", 4), "闰润");

            dic.Add(new PinyinInfo("ruo", 4), "若偌弱");

            #endregion

            #region [=====S=====]

            dic.Add(new PinyinInfo("sa", 1), "撒仨");
            dic.Add(new PinyinInfo("sa", 3), "撒洒");
            dic.Add(new PinyinInfo("sa", 4), "卅飒萨");

            dic.Add(new PinyinInfo("sai", 1), "塞腮");
            dic.Add(new PinyinInfo("sai", 4), "塞赛");

            dic.Add(new PinyinInfo("san", 1), "三叁");
            dic.Add(new PinyinInfo("san", 3), "散伞");
            dic.Add(new PinyinInfo("san", 4), "散");

            dic.Add(new PinyinInfo("sang", 1), "丧桑");
            dic.Add(new PinyinInfo("sang", 3), "搡嗓");
            dic.Add(new PinyinInfo("sang", 4), "丧");

            dic.Add(new PinyinInfo("sao", 1), "臊搔骚");
            dic.Add(new PinyinInfo("sao", 3), "扫嫂");
            dic.Add(new PinyinInfo("sao", 4), "臊");

            dic.Add(new PinyinInfo("se", 4), "塞色铯涩啬穑瑟");

            dic.Add(new PinyinInfo("sen", 1), "森");

            dic.Add(new PinyinInfo("seng", 1), "僧");

            dic.Add(new PinyinInfo("sha", 1), "刹沙杉莎杀纱砂");
            dic.Add(new PinyinInfo("sha", 2), "啥");
            dic.Add(new PinyinInfo("sha", 3), "傻");
            dic.Add(new PinyinInfo("sha", 4), "沙厦歃煞霎");

            dic.Add(new PinyinInfo("shai", 1), "筛");
            dic.Add(new PinyinInfo("shai", 3), "色");
            dic.Add(new PinyinInfo("shai", 0), "晒");

            dic.Add(new PinyinInfo("shan", 1), "扇苫栅杉山衫删姗珊煽潸膻");
            dic.Add(new PinyinInfo("shan", 3), "闪陕");
            dic.Add(new PinyinInfo("shan", 4), "扇苫禅单讪汕骟善缮膳鳝擅嬗赡");

            dic.Add(new PinyinInfo("shan", 1), "伤殇觞商熵");
            dic.Add(new PinyinInfo("shan", 3), "上晌赏");
            dic.Add(new PinyinInfo("shan", 4), "上尚");

            dic.Add(new PinyinInfo("shao", 1), "稍鞘烧捎梢艄");
            dic.Add(new PinyinInfo("shao", 2), "苕勺芍韶");
            dic.Add(new PinyinInfo("shao", 3), "少");
            dic.Add(new PinyinInfo("shao", 4), "少召稍绍邵哨潲");

            dic.Add(new PinyinInfo("she", 1), "奢赊");
            dic.Add(new PinyinInfo("she", 2), "折舌佘蛇");
            dic.Add(new PinyinInfo("she", 3), "舍");
            dic.Add(new PinyinInfo("she", 4), "舍拾设社射麝涉赦摄慑");

            dic.Add(new PinyinInfo("shen", 1), "参莘申伸绅砷呻身深");
            dic.Add(new PinyinInfo("shen", 2), "什神");
            dic.Add(new PinyinInfo("shen", 3), "沈审婶哂");
            dic.Add(new PinyinInfo("shen", 4), "肾甚葚渗蜃慎");

            dic.Add(new PinyinInfo("sheng", 1), "升昇生牲笙甥声");
            dic.Add(new PinyinInfo("sheng", 2), "绳");
            dic.Add(new PinyinInfo("sheng", 3), "省");
            dic.Add(new PinyinInfo("sheng", 4), "乘盛圣胜晟剩");

            dic.Add(new PinyinInfo("shi", 1), "嘘尸失师狮诗虱施湿");
            dic.Add(new PinyinInfo("shi", 2), "石识拾什十时实食蚀");
            dic.Add(new PinyinInfo("shi", 3), "史驶矢使始屎");
            dic.Add(new PinyinInfo("shi", 4), "似士仕氏舐示世市柿式试拭轼弑势事侍恃饰视是适室逝誓释谥嗜");
            dic.Add(new PinyinInfo("shi", 0), "殖");

            dic.Add(new PinyinInfo("shou", 1), "收");
            dic.Add(new PinyinInfo("shou", 3), "熟手守首");
            dic.Add(new PinyinInfo("shou", 4), "寿受授绶狩售兽瘦");

            dic.Add(new PinyinInfo("shu", 1), "殳书抒舒枢叔淑姝殊倏梳疏蔬输");
            dic.Add(new PinyinInfo("shu", 2), "熟秫孰塾赎");
            dic.Add(new PinyinInfo("shu", 3), "数属暑署薯曙黍蜀鼠");
            dic.Add(new PinyinInfo("shu", 4), "数术述戍束树竖恕庶墅漱");

            dic.Add(new PinyinInfo("shua", 1), "刷唰");
            dic.Add(new PinyinInfo("shua", 3), "耍");

            dic.Add(new PinyinInfo("shuai", 1), "衰摔");
            dic.Add(new PinyinInfo("shuai", 3), "甩");
            dic.Add(new PinyinInfo("shuai", 4), "率帅蟀");

            dic.Add(new PinyinInfo("shuan", 1), "闩拴栓");
            dic.Add(new PinyinInfo("shuan", 4), "涮");

            dic.Add(new PinyinInfo("shuang", 1), "双霜孀");
            dic.Add(new PinyinInfo("shuang", 3), "爽");

            dic.Add(new PinyinInfo("shui", 2), "谁");
            dic.Add(new PinyinInfo("shui", 3), "水");
            dic.Add(new PinyinInfo("shui", 4), "说税睡");

            dic.Add(new PinyinInfo("shun", 3), "吮");
            dic.Add(new PinyinInfo("shun", 4), "顺舜瞬");

            dic.Add(new PinyinInfo("shuo", 1), "说");
            dic.Add(new PinyinInfo("shuo", 4), "数烁铄朔硕");

            dic.Add(new PinyinInfo("si", 1), "私司丝咝思斯厮撕嘶");
            dic.Add(new PinyinInfo("si", 3), "死");
            dic.Add(new PinyinInfo("si", 4), "似伺巳祀四肆寺饲嗣");

            dic.Add(new PinyinInfo("song", 1), "松嵩");
            dic.Add(new PinyinInfo("song", 3), "怂耸悚");
            dic.Add(new PinyinInfo("song", 4), "讼颂宋送诵");

            dic.Add(new PinyinInfo("sou", 1), "搜嗖馊艘");
            dic.Add(new PinyinInfo("sou", 3), "擞叟");
            dic.Add(new PinyinInfo("sou", 4), "擞嗽");

            dic.Add(new PinyinInfo("su", 1), "苏酥");
            dic.Add(new PinyinInfo("su", 2), "俗");
            dic.Add(new PinyinInfo("su", 4), "宿夙诉肃素嗉速粟塑溯簌");

            dic.Add(new PinyinInfo("suan", 1), "酸");
            dic.Add(new PinyinInfo("suan", 4), "蒜算");

            dic.Add(new PinyinInfo("sui", 1), "虽睢");
            dic.Add(new PinyinInfo("sui", 2), "遂绥隋随");
            dic.Add(new PinyinInfo("sui", 3), "髓");
            dic.Add(new PinyinInfo("sui", 4), "遂岁碎祟隧穗");

            dic.Add(new PinyinInfo("sun", 1), "孙");
            dic.Add(new PinyinInfo("sun", 3), "损笋隼");

            dic.Add(new PinyinInfo("suo", 1), "莎唆梭羧蓑缩");
            dic.Add(new PinyinInfo("suo", 3), "所索唢琐锁");

            #endregion

            #region [=====D=====]

            dic.Add(new PinyinInfo("ta", 1), "踏他她它铊塌");
            dic.Add(new PinyinInfo("ta", 3), "拓塔獭");
            dic.Add(new PinyinInfo("ta", 4), "踏哒沓嗒挞榻蹋");

            dic.Add(new PinyinInfo("tai", 1), "台苔胎");
            dic.Add(new PinyinInfo("tai", 2), "台苔邰抬跆");
            dic.Add(new PinyinInfo("tai", 4), "太汰态钛酞泰");

            dic.Add(new PinyinInfo("tan", 1), "啴贪摊滩瘫");
            dic.Add(new PinyinInfo("tan", 2), "弹覃澹坛昙谈痰谭潭檀");
            dic.Add(new PinyinInfo("tan", 3), "坦袒毯");
            dic.Add(new PinyinInfo("tan", 4), "叹炭碳探");

            dic.Add(new PinyinInfo("tang", 1), "汤嘡羰");
            dic.Add(new PinyinInfo("tang", 2), "唐塘搪糖堂棠膛镗螳");
            dic.Add(new PinyinInfo("tang", 3), "倘淌躺");
            dic.Add(new PinyinInfo("tang", 4), "烫趟");

            dic.Add(new PinyinInfo("tao", 1), "叨涛绦掏滔韬饕");
            dic.Add(new PinyinInfo("tao", 2), "逃桃萄淘陶");
            dic.Add(new PinyinInfo("tao", 3), "讨");
            dic.Add(new PinyinInfo("tao", 4), "套");

            dic.Add(new PinyinInfo("te", 4), "特");

            dic.Add(new PinyinInfo("teng", 2), "疼腾誊滕藤");

            dic.Add(new PinyinInfo("ti", 1), "剔踢梯");
            dic.Add(new PinyinInfo("ti", 2), "提题醍啼蹄");
            dic.Add(new PinyinInfo("ti", 3), "体");
            dic.Add(new PinyinInfo("ti", 4), "屉剃涕悌惕替");

            dic.Add(new PinyinInfo("tian", 1), "天添");
            dic.Add(new PinyinInfo("tian", 2), "田恬甜填");
            dic.Add(new PinyinInfo("tian", 3), "佃忝舔殄");
            dic.Add(new PinyinInfo("tian", 4), "掭");

            dic.Add(new PinyinInfo("tiao", 1), "挑佻");
            dic.Add(new PinyinInfo("tiao", 2), "调苕条迢笤髫");
            dic.Add(new PinyinInfo("tiao", 3), "挑窕");
            dic.Add(new PinyinInfo("tiao", 4), "眺跳粜");

            dic.Add(new PinyinInfo("tie", 1), "帖贴");
            dic.Add(new PinyinInfo("tie", 3), "帖铁");
            dic.Add(new PinyinInfo("tie", 4), "帖餮");

            dic.Add(new PinyinInfo("ting", 1), "厅听烃");
            dic.Add(new PinyinInfo("ting", 2), "廷庭蜓霆亭停婷");
            dic.Add(new PinyinInfo("ting", 3), "挺铤艇");

            dic.Add(new PinyinInfo("tong", 1), "通恫嗵");
            dic.Add(new PinyinInfo("tong", 2), "同桐铜彤童潼瞳");
            dic.Add(new PinyinInfo("tong", 3), "筒统捅桶");
            dic.Add(new PinyinInfo("tong", 4), "通同痛恸");

            dic.Add(new PinyinInfo("tou", 1), "偷");
            dic.Add(new PinyinInfo("tou", 2), "头投骰");
            dic.Add(new PinyinInfo("tou", 4), "透");

            dic.Add(new PinyinInfo("tu", 1), "凸秃突");
            dic.Add(new PinyinInfo("tu", 2), "图荼途涂徒屠");
            dic.Add(new PinyinInfo("tu", 3), "吐土");
            dic.Add(new PinyinInfo("tu", 4), "吐兔菟");

            dic.Add(new PinyinInfo("tuan", 1), "湍");
            dic.Add(new PinyinInfo("tuan", 2), "团");
            dic.Add(new PinyinInfo("tuan", 3), "疃");
            dic.Add(new PinyinInfo("tuan", 4), "彖");

            dic.Add(new PinyinInfo("tui", 1), "推");
            dic.Add(new PinyinInfo("tui", 2), "颓");
            dic.Add(new PinyinInfo("tui", 3), "腿");
            dic.Add(new PinyinInfo("tui", 4), "退褪蜕");

            dic.Add(new PinyinInfo("tun", 1), "吞");
            dic.Add(new PinyinInfo("tun", 2), "囤屯饨豚臀");

            dic.Add(new PinyinInfo("tuo", 1), "托拖脱");
            dic.Add(new PinyinInfo("tuo", 2), "驮佗陀驼鸵");
            dic.Add(new PinyinInfo("tuo", 3), "妥椭");
            dic.Add(new PinyinInfo("tuo", 4), "拓唾");

            #endregion

            #region [=====W=====]

            dic.Add(new PinyinInfo("wa", 1), "哇挖洼蛙娲");
            dic.Add(new PinyinInfo("wa", 2), "娃");
            dic.Add(new PinyinInfo("wa", 3), "瓦佤");
            dic.Add(new PinyinInfo("wa", 4), "瓦袜");
            dic.Add(new PinyinInfo("wa", 0), "哇");

            dic.Add(new PinyinInfo("wai", 1), "歪");
            dic.Add(new PinyinInfo("wai", 3), "崴");
            dic.Add(new PinyinInfo("wai", 4), "外");

            dic.Add(new PinyinInfo("wan", 1), "弯湾剜蜿豌");
            dic.Add(new PinyinInfo("wan", 2), "丸纨完玩顽烷");
            dic.Add(new PinyinInfo("wan", 3), "莞宛惋婉碗皖挽晚绾");
            dic.Add(new PinyinInfo("wan", 4), "蔓万腕");

            dic.Add(new PinyinInfo("wang", 1), "汪");
            dic.Add(new PinyinInfo("wang", 2), "王亡");
            dic.Add(new PinyinInfo("wang", 3), "网罔惘枉往");
            dic.Add(new PinyinInfo("wang", 4), "王旺望妄忘");

            dic.Add(new PinyinInfo("wei", 1), "委崴危巍威偎煨微薇");
            dic.Add(new PinyinInfo("wei", 2), "为韦违围闱桅唯帷维");
            dic.Add(new PinyinInfo("wei", 3), "委尾伟苇纬伪娓诿萎痿猥");
            dic.Add(new PinyinInfo("wei", 4), "为遗卫未味位畏喂胃谓猬渭尉蔚慰魏");

            dic.Add(new PinyinInfo("wen", 1), "温瘟");
            dic.Add(new PinyinInfo("wen", 2), "文纹蚊雯闻");
            dic.Add(new PinyinInfo("wen", 3), "刎吻紊稳");
            dic.Add(new PinyinInfo("wen", 4), "问汶");

            dic.Add(new PinyinInfo("weng", 1), "翁嗡");
            dic.Add(new PinyinInfo("weng", 4), "瓮");

            dic.Add(new PinyinInfo("wo", 1), "挝莴窝涡蜗倭喔");
            dic.Add(new PinyinInfo("wo", 3), "我肟");
            dic.Add(new PinyinInfo("wo", 4), "沃卧握幄斡");

            dic.Add(new PinyinInfo("wu", 1), "恶乌邬呜钨污巫诬屋");
            dic.Add(new PinyinInfo("wu", 2), "唔无芜毋梧吴蜈");
            dic.Add(new PinyinInfo("wu", 3), "五伍午仵忤怃妩武鹉侮捂舞");
            dic.Add(new PinyinInfo("wu", 4), "恶乌兀勿物坞戊务雾误悟晤寤骛鹜");

            #endregion

            #region [=====X=====]

            dic.Add(new PinyinInfo("xi", 1), "茜栖蹊夕汐兮西牺吸希唏烯稀昔惜析晰皙蜥息熄奚溪悉蟋翕犀锡熙嘻嬉膝羲曦");
            dic.Add(new PinyinInfo("xi", 2), "习席袭媳");
            dic.Add(new PinyinInfo("xi", 3), "铣洗玺徙喜禧");
            dic.Add(new PinyinInfo("xi", 4), "系戏细隙");

            dic.Add(new PinyinInfo("xia", 1), "呷虾瞎");
            dic.Add(new PinyinInfo("xia", 2), "匣侠峡狭遐瑕暇霞辖黠");
            dic.Add(new PinyinInfo("xia", 4), "吓下夏罅");

            dic.Add(new PinyinInfo("xian", 1), "鲜纤仙氙先酰掀锨");
            dic.Add(new PinyinInfo("xian", 2), "弦舷闲娴贤咸涎衔嫌");
            dic.Add(new PinyinInfo("xian", 3), "鲜铣跣显险藓");
            dic.Add(new PinyinInfo("xian", 4), "苋现县限线宪陷馅羡腺献");

            dic.Add(new PinyinInfo("xiang", 1), "相乡厢湘箱香襄镶");
            dic.Add(new PinyinInfo("xiang", 2), "降详祥翔");
            dic.Add(new PinyinInfo("xiang", 3), "享响饷飨想");
            dic.Add(new PinyinInfo("xiang", 4), "相巷向项象像橡");

            dic.Add(new PinyinInfo("xiao", 1), "肖削逍消宵硝销霄魈枭哓骁萧潇箫嚣");
            dic.Add(new PinyinInfo("xiao", 2), "崤淆");
            dic.Add(new PinyinInfo("xiao", 3), "小晓");
            dic.Add(new PinyinInfo("xiao", 4), "肖校孝哮效笑啸");

            dic.Add(new PinyinInfo("xie", 1), "些揳楔歇蝎");
            dic.Add(new PinyinInfo("xie", 2), "邪协胁挟偕谐斜撷携鞋");
            dic.Add(new PinyinInfo("xie", 3), "血写");
            dic.Add(new PinyinInfo("xie", 4), "泄泻卸屑械亵谢邂懈蟹");

            dic.Add(new PinyinInfo("xin", 1), "芯莘心欣辛锌歆新薪馨鑫");
            dic.Add(new PinyinInfo("xin", 4), "芯信衅");

            dic.Add(new PinyinInfo("xing", 1), "兴星猩惺腥");
            dic.Add(new PinyinInfo("xing", 2), "行刑形型邢");
            dic.Add(new PinyinInfo("xing", 3), "省醒擤");
            dic.Add(new PinyinInfo("xing", 4), "兴杏幸悻性姓");

            dic.Add(new PinyinInfo("xiong", 1), "凶匈讻汹胸兄");
            dic.Add(new PinyinInfo("xiong", 2), "雄熊");

            dic.Add(new PinyinInfo("xiu", 1), "休咻修羞");
            dic.Add(new PinyinInfo("xiu", 3), "宿朽");
            dic.Add(new PinyinInfo("xiu", 4), "宿臭秀绣锈袖嗅");

            dic.Add(new PinyinInfo("xu", 1), "歘嘘吁戌须胥虚墟欻需魆");
            dic.Add(new PinyinInfo("xu", 2), "徐");
            dic.Add(new PinyinInfo("xu", 3), "浒许诩栩");
            dic.Add(new PinyinInfo("xu", 4), "畜旭序煦叙恤蓄酗勖绪续婿絮");

            dic.Add(new PinyinInfo("xuan", 1), "轩宣揎喧暄");
            dic.Add(new PinyinInfo("xuan", 2), "旋玄悬漩璇");
            dic.Add(new PinyinInfo("xuan", 3), "选癣");
            dic.Add(new PinyinInfo("xuan", 4), "旋炫眩绚渲");

            dic.Add(new PinyinInfo("xue", 1), "削靴薛");
            dic.Add(new PinyinInfo("xue", 2), "穴学噱");
            dic.Add(new PinyinInfo("xue", 3), "雪");
            dic.Add(new PinyinInfo("xue", 4), "血谑");

            dic.Add(new PinyinInfo("xun", 1), "熏勋薰醺");
            dic.Add(new PinyinInfo("xun", 2), "旬询荀寻巡循");
            dic.Add(new PinyinInfo("xun", 4), "熏训驯讯汛迅徇殉逊巽");

            #endregion

            #region [=====Y=====]

            dic.Add(new PinyinInfo("ya", 1), "丫压呀鸦押鸭桠");
            dic.Add(new PinyinInfo("ya", 2), "牙伢芽蚜崖涯睚衙");
            dic.Add(new PinyinInfo("ya", 3), "哑雅");
            dic.Add(new PinyinInfo("ya", 4), "轧亚娅氩讶揠");

            dic.Add(new PinyinInfo("yan", 1), "燕腌咽殷胭烟恹焉嫣阉湮淹");
            dic.Add(new PinyinInfo("yan", 2), "铅延蜒筵闫严言妍研岩炎沿盐阎颜檐");
            dic.Add(new PinyinInfo("yan", 3), "奄掩俨衍魇郾偃眼演鼹");
            dic.Add(new PinyinInfo("yan", 4), "咽燕厌餍砚彦谚艳滟晏宴唁堰验雁赝焰");

            dic.Add(new PinyinInfo("yang", 1), "央泱殃秧鸯");
            dic.Add(new PinyinInfo("yang", 2), "扬杨疡羊佯徉洋阳");
            dic.Add(new PinyinInfo("yang", 3), "仰养氧痒");
            dic.Add(new PinyinInfo("yang", 4), "怏样恙烊漾");

            dic.Add(new PinyinInfo("yao", 1), "要约幺吆夭妖腰邀");
            dic.Add(new PinyinInfo("yao", 2), "爻肴尧姚窑谣摇徭遥瑶");
            dic.Add(new PinyinInfo("yao", 3), "杳咬舀窈");
            dic.Add(new PinyinInfo("yao", 4), "要钥药鹞耀");

            dic.Add(new PinyinInfo("ye", 1), "掖耶椰噎");
            dic.Add(new PinyinInfo("ye", 2), "掖耶邪爷揶");
            dic.Add(new PinyinInfo("ye", 3), "也冶野");
            dic.Add(new PinyinInfo("ye", 4), "咽业页曳夜液腋谒叶");

            dic.Add(new PinyinInfo("yi", 1), "一壹伊咿衣依医漪揖噫");
            dic.Add(new PinyinInfo("yi", 2), "迤遗仪夷咦姨胰痍饴贻宜颐移疑彝");
            dic.Add(new PinyinInfo("yi", 3), "迤尾乙已以苡矣蚁倚椅旖");
            dic.Add(new PinyinInfo("yi", 4), "艾乂刈亿忆义议艺呓屹亦弈奕裔异抑邑轶役疫毅译驿绎易诣羿翌翳翼益溢缢谊逸肄意薏臆");

            dic.Add(new PinyinInfo("yin", 1), "殷因茵姻铟阴音喑愔殷");
            dic.Add(new PinyinInfo("yin", 2), "吟垠银龈淫霪寅");
            dic.Add(new PinyinInfo("yin", 3), "饮尹引蚓隐瘾");
            dic.Add(new PinyinInfo("yin", 4), "印荫");

            dic.Add(new PinyinInfo("ying", 1), "应英莺罂婴嘤缨樱鹦膺鹰");
            dic.Add(new PinyinInfo("ying", 2), "饮迎茔荧莹萤营萦盈楹蝇瀛赢");
            dic.Add(new PinyinInfo("ying", 3), "颍颖影");
            dic.Add(new PinyinInfo("ying", 4), "应映硬");

            dic.Add(new PinyinInfo("yo", 1), "唷");
            dic.Add(new PinyinInfo("yo", 0), "哟");

            dic.Add(new PinyinInfo("yong", 1), "佣拥庸雍壅臃");
            dic.Add(new PinyinInfo("yong", 3), "永咏泳甬勇涌恿蛹踊");
            dic.Add(new PinyinInfo("yong", 4), "佣用");

            dic.Add(new PinyinInfo("you", 1), "优忧攸悠呦幽");
            dic.Add(new PinyinInfo("you", 2), "柚尤犹鱿由邮油铀游");
            dic.Add(new PinyinInfo("you", 3), "友有酉莠黝");
            dic.Add(new PinyinInfo("you", 4), "柚又右佑幼囿鼬诱");

            dic.Add(new PinyinInfo("yu", 1), "於迂纡淤瘀");
            dic.Add(new PinyinInfo("yu", 2), "於予于盂竽余狳臾谀腴鱼渔隅愚俞揄逾渝愉瑜榆虞娱舆");
            dic.Add(new PinyinInfo("yu", 3), "语予雨与屿宇羽禹圄");
            dic.Add(new PinyinInfo("yu", 4), "雨与吁语玉驭芋浴欲裕妪郁育狱域预豫谕喻愈尉遇寓御鹬誉");

            dic.Add(new PinyinInfo("yuan", 1), "鸢鸳冤渊");
            dic.Add(new PinyinInfo("yuan", 2), "媛员元园圆垣援袁猿辕原源缘");
            dic.Add(new PinyinInfo("yuan", 3), "远");
            dic.Add(new PinyinInfo("yuan", 4), "媛苑怨院愿");

            dic.Add(new PinyinInfo("yue", 1), "约曰");
            dic.Add(new PinyinInfo("yue", 4), "乐钥月岳钺越阅悦跃粤");

            dic.Add(new PinyinInfo("yun", 1), "晕");
            dic.Add(new PinyinInfo("yun", 2), "云芸纭耘匀");
            dic.Add(new PinyinInfo("yun", 3), "允陨殒");
            dic.Add(new PinyinInfo("yun", 4), "晕员孕运酝愠蕴韵熨");

            #endregion

            #region [=====Z=====]

            dic.Add(new PinyinInfo("za", 1), "扎匝咂");
            dic.Add(new PinyinInfo("za", 2), "杂砸");
            dic.Add(new PinyinInfo("za", 3), "咋");

            dic.Add(new PinyinInfo("zai", 1), "灾甾哉栽");
            dic.Add(new PinyinInfo("zai", 3), "仔载宰崽");
            dic.Add(new PinyinInfo("zai", 4), "载再在");

            dic.Add(new PinyinInfo("zan", 1), "糌簪");
            dic.Add(new PinyinInfo("zan", 2), "咱");
            dic.Add(new PinyinInfo("zan", 3), "攒趱");
            dic.Add(new PinyinInfo("zan", 4), "暂錾赞");

            dic.Add(new PinyinInfo("zang", 1), "脏赃臧");
            dic.Add(new PinyinInfo("zang", 3), "驵");
            dic.Add(new PinyinInfo("zang", 4), "脏藏奘葬");

            dic.Add(new PinyinInfo("zao", 1), "遭糟");
            dic.Add(new PinyinInfo("zao", 2), "凿");
            dic.Add(new PinyinInfo("zao", 3), "早枣蚤澡藻");
            dic.Add(new PinyinInfo("zao", 4), "皂灶造噪燥躁");

            dic.Add(new PinyinInfo("ze", 2), "咋择则责啧帻泽");
            dic.Add(new PinyinInfo("ze", 4), "仄");

            dic.Add(new PinyinInfo("zei", 2), "贼");

            dic.Add(new PinyinInfo("zen", 3), "怎");
            dic.Add(new PinyinInfo("zen", 4), "谮");

            dic.Add(new PinyinInfo("zeng", 1), "曾增憎");
            dic.Add(new PinyinInfo("zeng", 4), "锃赠甑");

            dic.Add(new PinyinInfo("zha", 1), "扎咋查喳吒挓哳揸渣楂");
            dic.Add(new PinyinInfo("zha", 2), "扎轧炸札闸铡");
            dic.Add(new PinyinInfo("zha", 3), "拃砟眨");
            dic.Add(new PinyinInfo("zha", 4), "炸栅乍诈蚱榨咤");

            dic.Add(new PinyinInfo("zhai", 1), "侧斋摘");
            dic.Add(new PinyinInfo("zhai", 2), "择宅翟");
            dic.Add(new PinyinInfo("zhai", 3), "窄");
            dic.Add(new PinyinInfo("zhai", 4), "债砦寨");

            dic.Add(new PinyinInfo("zhan", 1), "占沾毡粘詹谵瞻");
            dic.Add(new PinyinInfo("zhan", 3), "斩崭盏展搌辗");
            dic.Add(new PinyinInfo("zhan", 4), "占战站栈湛绽蘸");

            dic.Add(new PinyinInfo("zhang", 1), "张章獐彰樟蟑");
            dic.Add(new PinyinInfo("zhang", 3), "长涨掌");
            dic.Add(new PinyinInfo("zhang", 4), "涨丈仗杖帐账胀障嶂瘴");

            dic.Add(new PinyinInfo("zhao", 1), "着朝钊招昭");
            dic.Add(new PinyinInfo("zhao", 2), "着");
            dic.Add(new PinyinInfo("zhao", 3), "爪找沼");
            dic.Add(new PinyinInfo("zhao", 4), "召诏照兆赵罩肇");

            dic.Add(new PinyinInfo("zhe", 1), "折蜇遮");
            dic.Add(new PinyinInfo("zhe", 2), "折蜇哲辄蛰谪辙");
            dic.Add(new PinyinInfo("zhe", 3), "者锗赭褶");
            dic.Add(new PinyinInfo("zhe", 4), "这浙蔗鹧");
            dic.Add(new PinyinInfo("zhe", 0), "着");

            dic.Add(new PinyinInfo("zhei", 4), "这");

            dic.Add(new PinyinInfo("zhen", 1), "贞侦帧针珍胗真砧箴榛臻斟甄");
            dic.Add(new PinyinInfo("zhen", 3), "诊疹枕缜");
            dic.Add(new PinyinInfo("zhen", 4), "阵鸩振赈震朕镇");

            dic.Add(new PinyinInfo("zhen", 1), "正症挣怔丁征争峥狰睁铮筝蒸");
            dic.Add(new PinyinInfo("zhen", 3), "拯整");
            dic.Add(new PinyinInfo("zhen", 4), "正症挣怔证政郑诤");

            dic.Add(new PinyinInfo("zhi", 1), "只之芝支吱枝肢织栀汁知蜘脂");
            dic.Add(new PinyinInfo("zhi", 2), "殖执直值植侄职跖踯");
            dic.Add(new PinyinInfo("zhi", 3), "只止址芷祉趾枳咫旨指酯纸");
            dic.Add(new PinyinInfo("zhi", 4), "识至桎致窒蛭志痣豸帜秩制质炙治栉峙痔挚掷智滞置雉稚");

            dic.Add(new PinyinInfo("zhong", 1), "中忠盅钟衷终");
            dic.Add(new PinyinInfo("zhong", 3), "种肿冢踵");
            dic.Add(new PinyinInfo("zhong", 4), "种中重仲众");

            dic.Add(new PinyinInfo("zhou", 1), "舟州洲诌周粥");
            dic.Add(new PinyinInfo("zhou", 2), "轴妯");
            dic.Add(new PinyinInfo("zhou", 3), "肘帚");
            dic.Add(new PinyinInfo("zhou", 4), "轴纣皱咒宙胄昼骤");

            dic.Add(new PinyinInfo("zhu", 1), "朱侏诛茱珠株铢蛛诸猪");
            dic.Add(new PinyinInfo("zhu", 2), "术竹竺逐烛躅");
            dic.Add(new PinyinInfo("zhu", 3), "属主拄煮嘱瞩");
            dic.Add(new PinyinInfo("zhu", 4), "伫苎贮助住注驻柱蛀祝著铸筑");

            dic.Add(new PinyinInfo("zhua", 1), "抓");
            dic.Add(new PinyinInfo("zhua", 3), "爪");

            dic.Add(new PinyinInfo("zhuai", 3), "跩");
            dic.Add(new PinyinInfo("zhuai", 4), "拽");

            dic.Add(new PinyinInfo("zhuan", 1), "专砖");
            dic.Add(new PinyinInfo("zhuan", 3), "转");
            dic.Add(new PinyinInfo("zhuan", 4), "转传赚啭篆撰");

            dic.Add(new PinyinInfo("zhuang", 1), "妆庄桩装");
            dic.Add(new PinyinInfo("zhuang", 3), "奘");
            dic.Add(new PinyinInfo("zhuang", 4), "壮状撞幢");

            dic.Add(new PinyinInfo("zhui", 1), "椎骓锥追");
            dic.Add(new PinyinInfo("zhui", 4), "坠缀惴赘");

            dic.Add(new PinyinInfo("zhun", 1), "谆");
            dic.Add(new PinyinInfo("zhun", 3), "准");

            dic.Add(new PinyinInfo("zhuo", 1), "焯拙捉桌");
            dic.Add(new PinyinInfo("zhuo", 2), "着琢灼酌茁卓斫浊镯啄擢");

            dic.Add(new PinyinInfo("zi", 1), "仔兹孜赀龇咨姿资兹嗞滋辎锱");
            dic.Add(new PinyinInfo("zi", 3), "仔子籽姊秭紫訾梓滓");
            dic.Add(new PinyinInfo("zi", 4), "自字恣眦渍");

            dic.Add(new PinyinInfo("zong", 1), "宗综棕踪鬃");
            dic.Add(new PinyinInfo("zong", 3), "总");
            dic.Add(new PinyinInfo("zong", 4), "纵粽");

            dic.Add(new PinyinInfo("zou", 1), "邹");
            dic.Add(new PinyinInfo("zou", 3), "走");
            dic.Add(new PinyinInfo("zou", 4), "奏揍");

            dic.Add(new PinyinInfo("zu", 1), "租");
            dic.Add(new PinyinInfo("zu", 2), "足卒族");
            dic.Add(new PinyinInfo("zu", 3), "诅阻组俎祖");

            dic.Add(new PinyinInfo("zuan", 1), "钻");
            dic.Add(new PinyinInfo("zuan", 3), "纂");
            dic.Add(new PinyinInfo("zuan", 4), "赚钻攥");

            dic.Add(new PinyinInfo("zui", 3), "嘴");
            dic.Add(new PinyinInfo("zui", 4), "最醉罪");

            dic.Add(new PinyinInfo("zun", 3), "尊遵樽鳟");

            dic.Add(new PinyinInfo("zuo", 1), "作嘬");
            dic.Add(new PinyinInfo("zuo", 2), "琢昨");
            dic.Add(new PinyinInfo("zuo", 3), "撮左佐");
            dic.Add(new PinyinInfo("zuo", 4), "作阼怍祚酢坐唑座做");

            #endregion

            // 建立特殊字符字典
            dicSpecial = new Dictionary<char, PinyinInfo>();

            #region [=====多音字设置默认读音=====]

            dicSpecial.Add('挨', new PinyinInfo("ai", 1));
            dicSpecial.Add('嗳', new PinyinInfo("ai", 4));
            dicSpecial.Add('唉', new PinyinInfo("ai", 1));
            dicSpecial.Add('熬', new PinyinInfo("ao", 2));
            dicSpecial.Add('拗', new PinyinInfo("niu", 4));
            dicSpecial.Add('把', new PinyinInfo("ba", 4));
            dicSpecial.Add('吧', new PinyinInfo("ba", 0));
            dicSpecial.Add('罢', new PinyinInfo("ba", 4));
            dicSpecial.Add('拜', new PinyinInfo("bai", 4));
            dicSpecial.Add('背', new PinyinInfo("bei", 4));
            dicSpecial.Add('呗', new PinyinInfo("bei", 0));
            dicSpecial.Add('奔', new PinyinInfo("ben", 1));
            dicSpecial.Add('绷', new PinyinInfo("beng", 1));
            dicSpecial.Add('蚌', new PinyinInfo("beng", 4));
            dicSpecial.Add('臂', new PinyinInfo("bi", 4));
            dicSpecial.Add('贲', new PinyinInfo("ben", 1));
            dicSpecial.Add('婢', new PinyinInfo("bi", 4));
            dicSpecial.Add('瘪', new PinyinInfo("bie", 3));
            dicSpecial.Add('别', new PinyinInfo("bie", 2));
            dicSpecial.Add('槟', new PinyinInfo("bing", 1));
            dicSpecial.Add('并', new PinyinInfo("bing", 4));
            dicSpecial.Add('剥', new PinyinInfo("bo", 1));
            dicSpecial.Add('柏', new PinyinInfo("bai", 3));
            dicSpecial.Add('薄', new PinyinInfo("bao", 2));
            dicSpecial.Add('伯', new PinyinInfo("bo", 2));
            dicSpecial.Add('簸', new PinyinInfo("bo", 3));
            dicSpecial.Add('卜', new PinyinInfo("bu", 3));
            dicSpecial.Add('堡', new PinyinInfo("bao", 3));
            dicSpecial.Add('采', new PinyinInfo("cai", 3));
            dicSpecial.Add('参', new PinyinInfo("can", 1));
            dicSpecial.Add('嚓', new PinyinInfo("cha", 1));
            dicSpecial.Add('叉', new PinyinInfo("cha", 1));
            dicSpecial.Add('衩', new PinyinInfo("cha", 3));
            dicSpecial.Add('杈', new PinyinInfo("cha", 3));
            dicSpecial.Add('差', new PinyinInfo("cha", 4));
            dicSpecial.Add('孱', new PinyinInfo("chan", 2));
            dicSpecial.Add('场', new PinyinInfo("chang", 3));
            dicSpecial.Add('称', new PinyinInfo("cheng", 4));
            dicSpecial.Add('踟', new PinyinInfo("chi", 2));
            dicSpecial.Add('冲', new PinyinInfo("chong", 1));
            dicSpecial.Add('处', new PinyinInfo("chu", 3));
            dicSpecial.Add('揣', new PinyinInfo("chuai", 4));
            dicSpecial.Add('创', new PinyinInfo("chuang", 4));
            dicSpecial.Add('绰', new PinyinInfo("chuo", 4));
            dicSpecial.Add('跐', new PinyinInfo("ci", 3));
            dicSpecial.Add('刺', new PinyinInfo("ci", 4));
            dicSpecial.Add('撮', new PinyinInfo("cuo", 1));
            dicSpecial.Add('答', new PinyinInfo("da", 2));
            dicSpecial.Add('打', new PinyinInfo("da", 3));
            dicSpecial.Add('瘩', new PinyinInfo("da", 0));
            dicSpecial.Add('逮', new PinyinInfo("dai", 3));
            dicSpecial.Add('待', new PinyinInfo("dai", 1));
            dicSpecial.Add('大', new PinyinInfo("da", 4));
            dicSpecial.Add('单', new PinyinInfo("dan", 1));
            dicSpecial.Add('担', new PinyinInfo("dan", 4));
            dicSpecial.Add('当', new PinyinInfo("dang", 1));
            dicSpecial.Add('挡', new PinyinInfo("dang", 3));
            dicSpecial.Add('倒', new PinyinInfo("dao", 4));
            dicSpecial.Add('得', new PinyinInfo("de", 2));
            dicSpecial.Add('蹬', new PinyinInfo("deng", 4));
            dicSpecial.Add('澄', new PinyinInfo("cheng", 2));
            dicSpecial.Add('的', new PinyinInfo("de", 0));
            dicSpecial.Add('镝', new PinyinInfo("di", 2));
            dicSpecial.Add('氐', new PinyinInfo("di", 3));
            dicSpecial.Add('地', new PinyinInfo("di", 4));
            dicSpecial.Add('癫', new PinyinInfo("dian", 1));
            dicSpecial.Add('酊', new PinyinInfo("ding", 3));
            dicSpecial.Add('钉', new PinyinInfo("ding", 1));
            dicSpecial.Add('斗', new PinyinInfo("dou", 4));
            dicSpecial.Add('都', new PinyinInfo("du", 1));
            dicSpecial.Add('读', new PinyinInfo("du", 2));
            dicSpecial.Add('肚', new PinyinInfo("du", 4));
            dicSpecial.Add('蹲', new PinyinInfo("dun", 1));
            dicSpecial.Add('墩', new PinyinInfo("dun", 1));
            dicSpecial.Add('度', new PinyinInfo("du", 4));
            dicSpecial.Add('垛', new PinyinInfo("duo", 4));
            dicSpecial.Add('阿', new PinyinInfo("a", 1));
            dicSpecial.Add('恶', new PinyinInfo("e", 4));
            dicSpecial.Add('发', new PinyinInfo("fa", 1));
            dicSpecial.Add('坊', new PinyinInfo("fang", 3));
            dicSpecial.Add('菲', new PinyinInfo("fei", 1));
            dicSpecial.Add('分', new PinyinInfo("fen", 1));
            dicSpecial.Add('缝', new PinyinInfo("feng", 2));
            dicSpecial.Add('佛', new PinyinInfo("fo", 2));
            dicSpecial.Add('夫', new PinyinInfo("fu", 1));
            dicSpecial.Add('服', new PinyinInfo("fu", 2));
            dicSpecial.Add('父', new PinyinInfo("fu", 4));
            dicSpecial.Add('嘎', new PinyinInfo("ga", 3));
            dicSpecial.Add('杆', new PinyinInfo("gan", 3));
            dicSpecial.Add('干', new PinyinInfo("gan", 4));
            dicSpecial.Add('钢', new PinyinInfo("gang", 1));
            dicSpecial.Add('膏', new PinyinInfo("gao", 1));
            dicSpecial.Add('搁', new PinyinInfo("ge", 1));
            dicSpecial.Add('葛', new PinyinInfo("ge", 3));
            dicSpecial.Add('盖', new PinyinInfo("gai", 4));
            dicSpecial.Add('个', new PinyinInfo("ge", 4));
            dicSpecial.Add('艮', new PinyinInfo("gen", 4));
            dicSpecial.Add('更', new PinyinInfo("geng", 4));
            dicSpecial.Add('供', new PinyinInfo("gong", 4));
            dicSpecial.Add('枸', new PinyinInfo("gou", 3));
            dicSpecial.Add('勾', new PinyinInfo("gou", 1));
            dicSpecial.Add('骨', new PinyinInfo("gu", 3));
            dicSpecial.Add('估', new PinyinInfo("gu", 1));
            dicSpecial.Add('呱', new PinyinInfo("gua", 1));
            dicSpecial.Add('冠', new PinyinInfo("guan", 4));
            dicSpecial.Add('观', new PinyinInfo("guan", 1));
            dicSpecial.Add('桄', new PinyinInfo("guang", 1));
            dicSpecial.Add('掴', new PinyinInfo("guo", 2));
            dicSpecial.Add('过', new PinyinInfo("guo", 4));
            dicSpecial.Add('蛤', new PinyinInfo("ha", 2));
            dicSpecial.Add('哈', new PinyinInfo("ha", 1));
            dicSpecial.Add('汗', new PinyinInfo("han", 4));
            dicSpecial.Add('好', new PinyinInfo("hao", 3));
            dicSpecial.Add('号', new PinyinInfo("hao", 4));
            dicSpecial.Add('镐', new PinyinInfo("gao", 3));
            dicSpecial.Add('貉', new PinyinInfo("hao", 2));
            dicSpecial.Add('和', new PinyinInfo("he", 2));
            dicSpecial.Add('喝', new PinyinInfo("he", 1));
            dicSpecial.Add('荷', new PinyinInfo("he", 2));
            dicSpecial.Add('横', new PinyinInfo("heng", 2));
            dicSpecial.Add('哼', new PinyinInfo("heng", 0));
            dicSpecial.Add('哄', new PinyinInfo("hong", 3));
            dicSpecial.Add('侯', new PinyinInfo("hou", 2));
            dicSpecial.Add('糊', new PinyinInfo("hu", 2));
            dicSpecial.Add('核', new PinyinInfo("he", 2));
            dicSpecial.Add('鹄', new PinyinInfo("hu", 2));
            dicSpecial.Add('哗', new PinyinInfo("hua", 1));
            dicSpecial.Add('划', new PinyinInfo("hua", 2));
            dicSpecial.Add('华', new PinyinInfo("hua", 2));
            dicSpecial.Add('还', new PinyinInfo("huan", 2));
            dicSpecial.Add('晃', new PinyinInfo("huang", 4));
            dicSpecial.Add('桧', new PinyinInfo("hui", 4));
            dicSpecial.Add('混', new PinyinInfo("hun", 2));
            dicSpecial.Add('豁', new PinyinInfo("huo", 4));
            dicSpecial.Add('给', new PinyinInfo("gei", 3));
            dicSpecial.Add('几', new PinyinInfo("ji", 3));
            dicSpecial.Add('济', new PinyinInfo("ji", 4));
            dicSpecial.Add('纪', new PinyinInfo("ji", 4));
            dicSpecial.Add('夹', new PinyinInfo("jia", 2));
            dicSpecial.Add('伽', new PinyinInfo("jia", 1));
            dicSpecial.Add('贾', new PinyinInfo("jia", 3));
            dicSpecial.Add('假', new PinyinInfo("jia", 4));
            dicSpecial.Add('间', new PinyinInfo("jian", 1));
            dicSpecial.Add('监', new PinyinInfo("jian", 1));
            dicSpecial.Add('渐', new PinyinInfo("jian", 4));
            dicSpecial.Add('锏', new PinyinInfo("jian", 4));
            dicSpecial.Add('将', new PinyinInfo("jiang", 1));
            dicSpecial.Add('浆', new PinyinInfo("jiang", 3));
            dicSpecial.Add('矫', new PinyinInfo("jiao", 2));
            dicSpecial.Add('教', new PinyinInfo("jiao", 1));
            dicSpecial.Add('嚼', new PinyinInfo("jiao", 2));
            dicSpecial.Add('节', new PinyinInfo("jie", 2));
            dicSpecial.Add('结', new PinyinInfo("jie", 2));
            dicSpecial.Add('芥', new PinyinInfo("jie", 4));
            dicSpecial.Add('尽', new PinyinInfo("jin", 4));
            dicSpecial.Add('禁', new PinyinInfo("jin", 4));
            dicSpecial.Add('荆', new PinyinInfo("jing", 1));
            dicSpecial.Add('颈', new PinyinInfo("jing", 3));
            dicSpecial.Add('劲', new PinyinInfo("jing", 4));
            dicSpecial.Add('氿', new PinyinInfo("gui", 3));
            dicSpecial.Add('车', new PinyinInfo("che", 1));
            dicSpecial.Add('桔', new PinyinInfo("ju", 2));
            dicSpecial.Add('句', new PinyinInfo("ju", 4));
            dicSpecial.Add('圈', new PinyinInfo("quan", 1));
            dicSpecial.Add('卷', new PinyinInfo("juan", 4));
            dicSpecial.Add('角', new PinyinInfo("jiao", 3));
            dicSpecial.Add('觉', new PinyinInfo("jue", 2));
            dicSpecial.Add('蹶', new PinyinInfo("jue", 2));
            dicSpecial.Add('倔', new PinyinInfo("jue", 4));
            dicSpecial.Add('咖', new PinyinInfo("ka", 1));
            dicSpecial.Add('咯', new PinyinInfo("ge", 1));
            dicSpecial.Add('看', new PinyinInfo("kan", 4));
            dicSpecial.Add('扛', new PinyinInfo("kang", 2));
            dicSpecial.Add('咳', new PinyinInfo("ke", 2));
            dicSpecial.Add('可', new PinyinInfo("ke", 3));
            dicSpecial.Add('吭', new PinyinInfo("keng", 1));
            dicSpecial.Add('空', new PinyinInfo("kong", 1));
            dicSpecial.Add('会', new PinyinInfo("hui", 4));
            dicSpecial.Add('溃', new PinyinInfo("kui", 4));
            dicSpecial.Add('拉', new PinyinInfo("la", 1));
            dicSpecial.Add('啦', new PinyinInfo("la", 0));
            dicSpecial.Add('郎', new PinyinInfo("lang", 2));
            dicSpecial.Add('唠', new PinyinInfo("lao", 4));
            dicSpecial.Add('落', new PinyinInfo("luo", 4));
            dicSpecial.Add('勒', new PinyinInfo("lei", 1));
            dicSpecial.Add('累', new PinyinInfo("lei", 4));
            dicSpecial.Add('擂', new PinyinInfo("lei", 4));
            dicSpecial.Add('哩', new PinyinInfo("li", 0));
            dicSpecial.Add('丽', new PinyinInfo("li", 4));
            dicSpecial.Add('俩', new PinyinInfo("liang", 3));
            dicSpecial.Add('凉', new PinyinInfo("liang", 4));
            dicSpecial.Add('量', new PinyinInfo("liang", 4));
            dicSpecial.Add('撩', new PinyinInfo("liao", 4));
            dicSpecial.Add('了', new PinyinInfo("le", 0));
            dicSpecial.Add('燎', new PinyinInfo("liao", 2));
            dicSpecial.Add('冽', new PinyinInfo("lie", 4));
            dicSpecial.Add('淋', new PinyinInfo("lin", 2));
            dicSpecial.Add('溜', new PinyinInfo("liu", 1));
            dicSpecial.Add('馏', new PinyinInfo("liu", 2));
            dicSpecial.Add('笼', new PinyinInfo("long", 2));
            dicSpecial.Add('搂', new PinyinInfo("lou", 3));
            dicSpecial.Add('喽', new PinyinInfo("lou", 0));
            dicSpecial.Add('碌', new PinyinInfo("lu", 4));
            dicSpecial.Add('露', new PinyinInfo("lu", 4));
            dicSpecial.Add('陆', new PinyinInfo("lu", 4));
            dicSpecial.Add('偻', new PinyinInfo("lv", 3));
            dicSpecial.Add('绿', new PinyinInfo("lv", 4));
            dicSpecial.Add('抡', new PinyinInfo("lun", 2));
            dicSpecial.Add('纶', new PinyinInfo("lun", 2));
            dicSpecial.Add('论', new PinyinInfo("lun", 4));
            dicSpecial.Add('捋', new PinyinInfo("luo", 1));
            dicSpecial.Add('啰', new PinyinInfo("luo", 0));
            dicSpecial.Add('洛', new PinyinInfo("luo", 4));
            dicSpecial.Add('蚂', new PinyinInfo("ma", 4));
            dicSpecial.Add('吗', new PinyinInfo("ma", 0));
            dicSpecial.Add('玛', new PinyinInfo("ma", 3));
            dicSpecial.Add('码', new PinyinInfo("ma", 3));
            dicSpecial.Add('埋', new PinyinInfo("mai", 2));
            dicSpecial.Add('谩', new PinyinInfo("man", 4));
            dicSpecial.Add('蔓', new PinyinInfo("man", 4));
            dicSpecial.Add('猫', new PinyinInfo("mao", 1));
            dicSpecial.Add('么', new PinyinInfo("me", 0));
            dicSpecial.Add('闷', new PinyinInfo("men", 4));
            dicSpecial.Add('蒙', new PinyinInfo("meng", 2));
            dicSpecial.Add('眯', new PinyinInfo("mi", 1));
            dicSpecial.Add('靡', new PinyinInfo("mi", 2));
            dicSpecial.Add('秘', new PinyinInfo("mi", 4));
            dicSpecial.Add('泌', new PinyinInfo("mi", 4));
            dicSpecial.Add('抹', new PinyinInfo("mo", 3));
            dicSpecial.Add('没', new PinyinInfo("mei", 2));
            dicSpecial.Add('磨', new PinyinInfo("mo", 4));
            dicSpecial.Add('脉', new PinyinInfo("mai", 4));
            dicSpecial.Add('模', new PinyinInfo("mo", 2));
            dicSpecial.Add('牟', new PinyinInfo("mu", 4));
            dicSpecial.Add('那', new PinyinInfo("na", 4));
            dicSpecial.Add('哪', new PinyinInfo("na", 3));
            dicSpecial.Add('难', new PinyinInfo("nan", 4));
            dicSpecial.Add('呢', new PinyinInfo("ni", 2));
            dicSpecial.Add('泥', new PinyinInfo("ni", 2));
            dicSpecial.Add('拧', new PinyinInfo("ning", 3));
            dicSpecial.Add('宁', new PinyinInfo("ning", 2));
            dicSpecial.Add('娜', new PinyinInfo("na", 4));
            dicSpecial.Add('哦', new PinyinInfo("o", 0));
            dicSpecial.Add('嚄', new PinyinInfo("o", 3));
            dicSpecial.Add('扒', new PinyinInfo("pa", 1));
            dicSpecial.Add('耙', new PinyinInfo("pa", 2));
            dicSpecial.Add('番', new PinyinInfo("fan", 1));
            dicSpecial.Add('膀', new PinyinInfo("pang", 2));
            dicSpecial.Add('彷', new PinyinInfo("pang", 2));
            dicSpecial.Add('磅', new PinyinInfo("bang", 4));
            dicSpecial.Add('胖', new PinyinInfo("pang", 4));
            dicSpecial.Add('炮', new PinyinInfo("pao", 4));
            dicSpecial.Add('刨', new PinyinInfo("pao", 2));
            dicSpecial.Add('袍', new PinyinInfo("pao", 2));
            dicSpecial.Add('泡', new PinyinInfo("pao", 4));
            dicSpecial.Add('喷', new PinyinInfo("pen", 1));
            dicSpecial.Add('搒', new PinyinInfo("peng", 2));
            dicSpecial.Add('裨', new PinyinInfo("bi", 4));
            dicSpecial.Add('陂', new PinyinInfo("po", 1));
            dicSpecial.Add('否', new PinyinInfo("fou", 3));
            dicSpecial.Add('劈', new PinyinInfo("pi", 1));
            dicSpecial.Add('辟', new PinyinInfo("pi", 4));
            dicSpecial.Add('扁', new PinyinInfo("pian", 3));
            dicSpecial.Add('便', new PinyinInfo("pian", 4));
            dicSpecial.Add('片', new PinyinInfo("pian", 4));
            dicSpecial.Add('漂', new PinyinInfo("piao", 1));
            dicSpecial.Add('骠', new PinyinInfo("Biao", 1));
            dicSpecial.Add('撇', new PinyinInfo("pie", 3));
            dicSpecial.Add('屏', new PinyinInfo("ping", 2));
            dicSpecial.Add('朴', new PinyinInfo("pu", 3));
            dicSpecial.Add('泊', new PinyinInfo("po", 1));
            dicSpecial.Add('迫', new PinyinInfo("po", 4));
            dicSpecial.Add('仆', new PinyinInfo("pu", 2));
            dicSpecial.Add('脯', new PinyinInfo("pu", 2));
            dicSpecial.Add('埔', new PinyinInfo("pu", 3));
            dicSpecial.Add('曝', new PinyinInfo("pu", 4));
            dicSpecial.Add('铺', new PinyinInfo("pu", 1));
            dicSpecial.Add('瀑', new PinyinInfo("pu", 4));
            dicSpecial.Add('缉', new PinyinInfo("qi", 1));
            dicSpecial.Add('奇', new PinyinInfo("qi", 2));
            dicSpecial.Add('荠', new PinyinInfo("qi", 2));
            dicSpecial.Add('稽', new PinyinInfo("ji", 1));
            dicSpecial.Add('跂', new PinyinInfo("qi", 3));
            dicSpecial.Add('亟', new PinyinInfo("qi", 4));
            dicSpecial.Add('卡', new PinyinInfo("ka", 3));
            dicSpecial.Add('浅', new PinyinInfo("qian", 3));
            dicSpecial.Add('强', new PinyinInfo("qiang", 2));
            dicSpecial.Add('呛', new PinyinInfo("qiang", 4));
            dicSpecial.Add('雀', new PinyinInfo("que", 4));
            dicSpecial.Add('悄', new PinyinInfo("qiao", 1));
            dicSpecial.Add('壳', new PinyinInfo("ke", 2));
            dicSpecial.Add('翘', new PinyinInfo("qiao", 4));
            dicSpecial.Add('茄', new PinyinInfo("qie", 2));
            dicSpecial.Add('切', new PinyinInfo("qie", 1));
            dicSpecial.Add('亲', new PinyinInfo("qin", 1));
            dicSpecial.Add('仇', new PinyinInfo("chou", 2));
            dicSpecial.Add('区', new PinyinInfo("qu", 1));
            dicSpecial.Add('曲', new PinyinInfo("qu", 3));
            dicSpecial.Add('阙', new PinyinInfo("que", 4));
            dicSpecial.Add('嚷', new PinyinInfo("rang", 3));
            dicSpecial.Add('娆', new PinyinInfo("rao", 2));
            dicSpecial.Add('任', new PinyinInfo("ren", 2));
            dicSpecial.Add('蠕', new PinyinInfo("ru", 2));
            dicSpecial.Add('撒', new PinyinInfo("sa", 3));
            dicSpecial.Add('塞', new PinyinInfo("sai", 1));
            dicSpecial.Add('散', new PinyinInfo("san", 4));
            dicSpecial.Add('丧', new PinyinInfo("sang", 4));
            dicSpecial.Add('臊', new PinyinInfo("sao", 4));
            dicSpecial.Add('刹', new PinyinInfo("sha", 1));
            dicSpecial.Add('沙', new PinyinInfo("sha", 1));
            dicSpecial.Add('色', new PinyinInfo("se", 4));
            dicSpecial.Add('杉', new PinyinInfo("shan", 1));
            dicSpecial.Add('扇', new PinyinInfo("shan", 4));
            dicSpecial.Add('苫', new PinyinInfo("shan", 4));
            dicSpecial.Add('禅', new PinyinInfo("chan", 2));
            dicSpecial.Add('上', new PinyinInfo("shan", 4));
            dicSpecial.Add('鞘', new PinyinInfo("shao", 1));
            dicSpecial.Add('少', new PinyinInfo("shao", 3));
            dicSpecial.Add('稍', new PinyinInfo("shao", 1));
            dicSpecial.Add('舍', new PinyinInfo("she", 4));
            dicSpecial.Add('乘', new PinyinInfo("cheng", 2));
            dicSpecial.Add('盛', new PinyinInfo("sheng", 4));
            dicSpecial.Add('石', new PinyinInfo("shi", 2));
            dicSpecial.Add('拾', new PinyinInfo("shi", 2));
            dicSpecial.Add('什', new PinyinInfo("shi", 2));
            dicSpecial.Add('熟', new PinyinInfo("shu", 2));
            dicSpecial.Add('数', new PinyinInfo("shu", 4));
            dicSpecial.Add('率', new PinyinInfo("shuai", 4));
            dicSpecial.Add('说', new PinyinInfo("shuo", 1));
            dicSpecial.Add('似', new PinyinInfo("si", 4));
            dicSpecial.Add('伺', new PinyinInfo("si", 4));
            dicSpecial.Add('擞', new PinyinInfo("sou", 4));
            dicSpecial.Add('遂', new PinyinInfo("sui", 4));
            dicSpecial.Add('莎', new PinyinInfo("sha", 1));
            dicSpecial.Add('踏', new PinyinInfo("ta", 4));
            dicSpecial.Add('哒', new PinyinInfo("ta", 4));
            dicSpecial.Add('沓', new PinyinInfo("ta", 4));
            dicSpecial.Add('嗒', new PinyinInfo("ta", 4));
            dicSpecial.Add('台', new PinyinInfo("tai", 2));
            dicSpecial.Add('苔', new PinyinInfo("tai", 2));
            dicSpecial.Add('啴', new PinyinInfo("tan", 1));
            dicSpecial.Add('弹', new PinyinInfo("tan", 2));
            dicSpecial.Add('覃', new PinyinInfo("tan", 2));
            dicSpecial.Add('澹', new PinyinInfo("tan", 2));
            dicSpecial.Add('叨', new PinyinInfo("tao", 1));
            dicSpecial.Add('提', new PinyinInfo("ti", 2));
            dicSpecial.Add('佃', new PinyinInfo("tian", 3));
            dicSpecial.Add('调', new PinyinInfo("tiao", 2));
            dicSpecial.Add('苕', new PinyinInfo("tiao", 2));
            dicSpecial.Add('挑', new PinyinInfo("tiao", 1));
            dicSpecial.Add('帖', new PinyinInfo("tie", 3));
            dicSpecial.Add('恫', new PinyinInfo("tong", 1));
            dicSpecial.Add('通', new PinyinInfo("tong", 1));
            dicSpecial.Add('同', new PinyinInfo("tong", 2));
            dicSpecial.Add('吐', new PinyinInfo("tu", 3));
            dicSpecial.Add('囤', new PinyinInfo("tun", 2));
            dicSpecial.Add('驮', new PinyinInfo("tuo", 2));
            dicSpecial.Add('拓', new PinyinInfo("ta", 4));
            dicSpecial.Add('瓦', new PinyinInfo("wa", 3));
            dicSpecial.Add('哇', new PinyinInfo("wa", 4));
            dicSpecial.Add('莞', new PinyinInfo("wan", 3));
            dicSpecial.Add('王', new PinyinInfo("wang", 2));
            dicSpecial.Add('崴', new PinyinInfo("wei", 1));
            dicSpecial.Add('委', new PinyinInfo("wei", 3));
            dicSpecial.Add('为', new PinyinInfo("wei", 2));
            dicSpecial.Add('涡', new PinyinInfo("wo", 1));
            dicSpecial.Add('乌', new PinyinInfo("wu", 4));
            dicSpecial.Add('茜', new PinyinInfo("xi", 1));
            dicSpecial.Add('栖', new PinyinInfo("xi", 1));
            dicSpecial.Add('蹊', new PinyinInfo("xi", 1));
            dicSpecial.Add('系', new PinyinInfo("xi", 4));
            dicSpecial.Add('吓', new PinyinInfo("xia", 4));
            dicSpecial.Add('纤', new PinyinInfo("xian", 1));
            dicSpecial.Add('鲜', new PinyinInfo("xian", 3));
            dicSpecial.Add('铣', new PinyinInfo("xian", 3));
            dicSpecial.Add('降', new PinyinInfo("xiang", 2));
            dicSpecial.Add('相', new PinyinInfo("xiang", 1));
            dicSpecial.Add('巷', new PinyinInfo("xiang", 4));
            dicSpecial.Add('肖', new PinyinInfo("xiao", 1));
            dicSpecial.Add('校', new PinyinInfo("xiao", 4));
            dicSpecial.Add('莘', new PinyinInfo("xin", 1));
            dicSpecial.Add('芯', new PinyinInfo("xin", 1));
            dicSpecial.Add('行', new PinyinInfo("xing", 2));
            dicSpecial.Add('省', new PinyinInfo("sheng", 3));
            dicSpecial.Add('兴', new PinyinInfo("xing", 4));
            dicSpecial.Add('宿', new PinyinInfo("shu", 4));
            dicSpecial.Add('臭', new PinyinInfo("chou", 4));
            dicSpecial.Add('歘', new PinyinInfo("xu", 1));
            dicSpecial.Add('嘘', new PinyinInfo("xu", 1));
            dicSpecial.Add('浒', new PinyinInfo("hu", 3));
            dicSpecial.Add('畜', new PinyinInfo("xu", 4));
            dicSpecial.Add('旋', new PinyinInfo("xuan", 2));
            dicSpecial.Add('削', new PinyinInfo("xiao", 1));
            dicSpecial.Add('血', new PinyinInfo("xue", 3));
            dicSpecial.Add('熏', new PinyinInfo("xun", 1));
            dicSpecial.Add('轧', new PinyinInfo("zha", 2));
            dicSpecial.Add('腌', new PinyinInfo("yan", 1));
            dicSpecial.Add('铅', new PinyinInfo("qian", 1));
            dicSpecial.Add('咽', new PinyinInfo("ye", 4));
            dicSpecial.Add('燕', new PinyinInfo("yan", 4));
            dicSpecial.Add('要', new PinyinInfo("yao", 4));
            dicSpecial.Add('掖', new PinyinInfo("ye", 2));
            dicSpecial.Add('耶', new PinyinInfo("ye", 2));
            dicSpecial.Add('邪', new PinyinInfo("xie", 2));
            dicSpecial.Add('遗', new PinyinInfo("yi", 2));
            dicSpecial.Add('迤', new PinyinInfo("yi", 3));
            dicSpecial.Add('尾', new PinyinInfo("wei", 3));
            dicSpecial.Add('艾', new PinyinInfo("ai", 4));
            dicSpecial.Add('殷', new PinyinInfo("yin", 1));
            dicSpecial.Add('饮', new PinyinInfo("ying", 3));
            dicSpecial.Add('应', new PinyinInfo("ying", 4));
            dicSpecial.Add('佣', new PinyinInfo("yong", 4));
            dicSpecial.Add('柚', new PinyinInfo("you", 4));
            dicSpecial.Add('於', new PinyinInfo("yu", 2));
            dicSpecial.Add('予', new PinyinInfo("yu", 2));
            dicSpecial.Add('雨', new PinyinInfo("yu", 3));
            dicSpecial.Add('与', new PinyinInfo("yu", 3));
            dicSpecial.Add('吁', new PinyinInfo("yu", 4));
            dicSpecial.Add('语', new PinyinInfo("yu", 2));
            dicSpecial.Add('尉', new PinyinInfo("wei", 4));
            dicSpecial.Add('媛', new PinyinInfo("yuan", 2));
            dicSpecial.Add('约', new PinyinInfo("yue", 1));
            dicSpecial.Add('乐', new PinyinInfo("le", 4));
            dicSpecial.Add('钥', new PinyinInfo("yao", 4));
            dicSpecial.Add('晕', new PinyinInfo("yun", 1));
            dicSpecial.Add('员', new PinyinInfo("yuan", 2));
            dicSpecial.Add('载', new PinyinInfo("zai", 1));
            dicSpecial.Add('攒', new PinyinInfo("zan", 3));
            dicSpecial.Add('脏', new PinyinInfo("zang", 1));
            dicSpecial.Add('藏', new PinyinInfo("cang", 2));
            dicSpecial.Add('咋', new PinyinInfo("zha", 3));
            dicSpecial.Add('曾', new PinyinInfo("zeng", 1));
            dicSpecial.Add('扎', new PinyinInfo("zha", 2));
            dicSpecial.Add('查', new PinyinInfo("cha", 2));
            dicSpecial.Add('喳', new PinyinInfo("zha", 1));
            dicSpecial.Add('炸', new PinyinInfo("zha", 4));
            dicSpecial.Add('栅', new PinyinInfo("zha", 4));
            dicSpecial.Add('侧', new PinyinInfo("ce", 4));
            dicSpecial.Add('择', new PinyinInfo("ze", 2));
            dicSpecial.Add('翟', new PinyinInfo("zhai", 2));
            dicSpecial.Add('占', new PinyinInfo("zhan", 4));
            dicSpecial.Add('长', new PinyinInfo("chang", 2));
            dicSpecial.Add('涨', new PinyinInfo("zhang", 3));
            dicSpecial.Add('朝', new PinyinInfo("chao", 2));
            dicSpecial.Add('着', new PinyinInfo("zhe", 0));
            dicSpecial.Add('召', new PinyinInfo("zhao", 4));
            dicSpecial.Add('折', new PinyinInfo("zhe", 2));
            dicSpecial.Add('蜇', new PinyinInfo("zhe", 2));
            dicSpecial.Add('这', new PinyinInfo("zhe", 4));
            dicSpecial.Add('丁', new PinyinInfo("ding", 1));
            dicSpecial.Add('正', new PinyinInfo("zhen", 4));
            dicSpecial.Add('症', new PinyinInfo("zhen", 4));
            dicSpecial.Add('挣', new PinyinInfo("zhen", 4));
            dicSpecial.Add('怔', new PinyinInfo("zhen", 4));
            dicSpecial.Add('殖', new PinyinInfo("zhi", 2));
            dicSpecial.Add('只', new PinyinInfo("zhi", 3));
            dicSpecial.Add('识', new PinyinInfo("shi", 2));
            dicSpecial.Add('种', new PinyinInfo("zhong", 4));
            dicSpecial.Add('中', new PinyinInfo("zhong", 1));
            dicSpecial.Add('重', new PinyinInfo("zhong", 4));
            dicSpecial.Add('轴', new PinyinInfo("zhou", 2));
            dicSpecial.Add('术', new PinyinInfo("shu", 4));
            dicSpecial.Add('属', new PinyinInfo("zhu", 3));
            dicSpecial.Add('爪', new PinyinInfo("zhua", 3));
            dicSpecial.Add('转', new PinyinInfo("zhuan", 4));
            dicSpecial.Add('传', new PinyinInfo("zhuan", 4));
            dicSpecial.Add('奘', new PinyinInfo("zang", 4));
            dicSpecial.Add('椎', new PinyinInfo("zhui", 1));
            dicSpecial.Add('焯', new PinyinInfo("zhuo", 1));
            dicSpecial.Add('仔', new PinyinInfo("zai", 3));
            dicSpecial.Add('兹', new PinyinInfo("zi", 1));
            dicSpecial.Add('赚', new PinyinInfo("zuan", 4));
            dicSpecial.Add('钻', new PinyinInfo("zuan", 4));
            dicSpecial.Add('嘬', new PinyinInfo("zuo", 1));
            dicSpecial.Add('琢', new PinyinInfo("zuo", 2));
            dicSpecial.Add('作', new PinyinInfo("zuo", 4));

            #endregion

        }

        /// <summary>
        /// 根据字符获取拼音
        /// </summary>
        /// <param name="chr"></param>
        /// <returns></returns>
        public string GetPinyin(char chr) {

            if (dicSpecial.ContainsKey(chr)) return dicSpecial[chr].Pinyin;

            foreach (var item in dic) {
                if (item.Value.IndexOf(chr) >= 0) {
                    return item.Key.Pinyin;
                }
            }

            return chr.ToString();
        }

        /// <summary>
        /// 根据字符获取拼音
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string GetPinyin(string str) {

            StringBuilder sb = new StringBuilder();

            // 遍历字符串
            for (int i = 0; i < str.Length; i++) {
                if (sb.Length > 0) sb.Append(' ');
                sb.Append(GetPinyin(str[i]));
            }

            return sb.ToString();
        }

        /// <summary>
        /// 根据字符获取拼音
        /// </summary>
        /// <param name="chr"></param>
        /// <returns></returns>
        public string GetPinyinWithTone(char chr) {

            if (dicSpecial.ContainsKey(chr)) return dicSpecial[chr].PinyinWithTone;

            foreach (var item in dic) {
                if (item.Value.IndexOf(chr) >= 0) {
                    return item.Key.PinyinWithTone;
                }
            }

            return chr.ToString();
        }

        /// <summary>
        /// 根据字符获取拼音
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string GetPinyinWithTone(string str) {

            StringBuilder sb = new StringBuilder();

            // 遍历字符串
            for (int i = 0; i < str.Length; i++) {
                if (sb.Length > 0) sb.Append(' ');
                sb.Append(GetPinyinWithTone(str[i]));
            }

            return sb.ToString();
        }

        /// <summary>
        /// 根据字符获取拼音
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string GetInitialLetters(string str) {

            StringBuilder sb = new StringBuilder();

            // 遍历字符串
            for (int i = 0; i < str.Length; i++) {
                sb.Append(GetPinyin(str[i])[0]);
            }

            return sb.ToString();
        }

        #region [=====静态方法=====]

        /// <summary>
        /// 获取拼音
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetPinyinFromString(string str) {
            string? res = null;
            using (Pinyin py = new Pinyin()) {
                res = py.GetPinyin(str);
            }
            return res ?? "";
        }

        /// <summary>
        /// 获取带音节拼音
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetPinyinToneFromString(string str) {
            string? res = null;
            using (Pinyin py = new Pinyin()) {
                res = py.GetPinyinWithTone(str);
            }
            return res ?? "";
        }

        /// <summary>
        /// 获取首字母
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetInitialLettersFromString(string str) {
            string? res = null;
            using (Pinyin py = new Pinyin()) {
                res = py.GetInitialLetters(str);
            }
            return res ?? "";
        }

        /// <summary>
        /// 释放对象
        /// </summary>
        public void Dispose() {
            dic.Clear();
        }

        #endregion

    }
}
