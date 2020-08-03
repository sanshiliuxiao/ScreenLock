using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace ScreenAndScreen
{
    public class Setting
    {
        public string Tip1 { get; set; }
        public string Password { get; set; }
        public string Tip2 { get; set; }
        public bool StartUpStatus { get; set; }
        public string Tip3 { get; set; }
        public int StartLockAfterTime { get; set; }
        public Setting()
        {
            Tip1 = "Password 设置密码字符串";
            Password = "yksqscvgy";
            Tip2 = "StartUpStatus 是否开机启动 true 是 false 否";
            StartUpStatus = false;
            Tip3 = "StartLockAfterTime 开启开机启动功能后、多少秒后自动锁屏";
            StartLockAfterTime = 50;
        }
        public static Setting SaveSetting(String path, Setting setting)
        {
            string json = JsonConvert.SerializeObject(setting, Formatting.Indented);
            File.WriteAllText(path, json, Encoding.UTF8);
            return setting;
        }
        public static Setting LoadSetting(String path)
        {
            Setting setting;
            string settingJson = File.ReadAllText(path, Encoding.UTF8);
            setting = JsonConvert.DeserializeObject<Setting>(settingJson);
            return setting;
        }
    }
}
