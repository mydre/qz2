using System;
using System.Collections.Generic;
using System.ComponentModel;
//using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinAppDemo.Db.Base;
using WinAppDemo.Db.Model;
//using System.Xml;
//using System.Reflection;

namespace WinAppDemo.Forms
{
    public partial class FormGjglZzxx : Form
    {
        public FormGjglZzxx()
        {
            InitializeComponent();

            textBox2.Text = Guid.NewGuid().ToString();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
                   
               ////方法1修改config的数据库路径
               //XmlDocument doc = new XmlDocument();
               ////获得配置文件的全路径 
               //string strFileName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName+".config";
               //MessageBox.Show(strFileName);
               //doc.Load(strFileName);
               ////找出名称为“add”的所有元素 
               //XmlNodeList nodes = doc.GetElementsByTagName("add");
               //XmlAttribute att;
               //for (int i = 0; i < nodes.Count; i++)
               //{
               //    //获得将当前元素的key属性 
               //    att = nodes[i].Attributes["name"];
               //    //根据元素的第一个属性来判断当前的元素是不是目标元素 
               //    if (att != null && att.Value == "sqlite_connection_string")
               //    {
               //        //对目标元素中的第二个属性赋值 
               //        att = nodes[i].Attributes["connectionString"];
               //        string strConn = "Data Source=" + Program.m_mainform.g_workPath + "\\midwxtrans.db;Pooling=true;FailIfMissing=false";
               //        att.Value = strConn;
               //        break;
               //    }
               //}
               ////保存上面的修改 
               //doc.Save(strFileName);

               //FieldInfo fieldInfo = typeof(ConfigurationManager).GetField("sqlite_connection_string", BindingFlags.NonPublic | BindingFlags.Static);
               //if (fieldInfo != null) fieldInfo.SetValue(null, 0);

               ////方法2修改config的数据库路径，均不能正常工作
               ////读app.config中的connectionStrings 
               //ModifyAppConfig mac = new ModifyAppConfig();
               //string strConnStrings = mac.GetConnectionStringsConfig("sqlite_connection_string");
               ////重写app.config中的connectionStrings 
               //ModifyAppConfig mac1 = new ModifyAppConfig();//"D:\手机取证工作路径设置\案件20190707093739\HONORV2020190701094546\AppData\Weixin\ca9529dc14475dbcc7e8553e77ad7d0b"
               //string strConn = "Data Source=" + Program.m_mainform.g_workPath + "\\midwxtrans.db;Pooling=true;FailIfMissing=false";
               //mac1.UpdateConnectionStringsConfig("sqlite_connection_string", strConn, "System.Data.SQLite.EF6");
               //strConnStrings = mac1.GetConnectionStringsConfig("sqlite_connection_string");

               Program.m_mainform.AddNewGjalZs();

            using (var context = new CaseContext())
            {
                Case @case = context.Cases.Find(AppConfig.getAppConfig().caseId_selected_working);
                Proof proof = new Proof()
                {
                    Case = @case,
                    ProofName = textBox1.Text,
                    ProofSerialNum = textBox2.Text,
                    PhoneNum = textBox6.Text,
                    phoneNum2 = textBox3.Text,
                    Holder = textBox4.Text,
                    ProofType = comboBox2.Text,
                    note = textBox8.Text,
                };
                @case.Proofs.Add(proof);

                context.SaveChanges();
            }

            this.Close();
            AppConfig.AppConfigAddAdvinceClear();
            AppContext.setInstanceNull();

                  

        }

        private void FormGjglZzxx_Load(object sender, EventArgs e)
        {
            textBox1.Text = Program.m_mainform.g_zjName;
        }
    }

    //class ModifyAppConfig
    //{
    //    /// 获取ConnectionStrings 
       
    //    public string GetConnectionStringsConfig(string connectionName)
    //    {
    //        string connectionString = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
    //        return connectionString;
    //    }
        
    //    /// 更新连接字符串 
        
    //    public void UpdateConnectionStringsConfig(string newName, string newConString, string newProviderName)
    //    {
    //        //bool isModified = false;    //记录该连接串是否已经存在           

    //        if (ConfigurationManager.ConnectionStrings[newName] != null)
    //        {
    //       //     isModified = true;

    //            // 打开可执行的配置文件*.exe.config
    //            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
    //            config.ConnectionStrings.ConnectionStrings.Remove(newName);
    //            //新建一个连接字符串实例 
    //            ConnectionStringSettings mySettings = new ConnectionStringSettings(newName, newConString, newProviderName);
            
    //            // 将新的连接串添加到配置文件中. 
    //            config.ConnectionStrings.ConnectionStrings.Add(mySettings);
    //            // 保存对配置文件所作的更改 
    //            config.Save(ConfigurationSaveMode.Modified);
    //            // 强制重新载入配置文件的ConnectionStrings配置节  
    //            ConfigurationManager.RefreshSection("ConnectionStrings");
    //        }
    //    }
    //}

}
