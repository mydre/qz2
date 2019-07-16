using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinAppDemo.Forms;
using WinAppDemo.Db.Base;
using WinAppDemo.Db.Model;
using System.Data.SQLite;
using WinAppDemo.tools;
using System.Collections;
namespace WinAppDemo.Controls
{
    public partial class UcZjzs : UserControl
    {

        private List<TreeNodeTypes> Types;
        public UcZjzs()
        {
            InitializeComponent();
        }

        private void UcZjzs_Load(object sender, EventArgs e)
        {
            //获取数据文件的路径
            // string dbPath = "Data Source =" + Program.m_mainform.g_workPath + "\\AppData\\Weixin\\ca9529dc14475dbcc7e8553e77ad7d0b" + "\\midwxtrans.db";
            string dbPath = "Data Source =D:\\手机取证工作路径设置\\案件20190707093739\\HONORV2020190701094546\\AppData\\Weixin\\ca9529dc14475dbcc7e8553e77ad7d0b\\midwxtrans.db";// + Program.m_mainform.g_workPath+"\\AppData\\Weixin\\ca9529dc14475dbcc7e8553e77ad7d0b" + "\\midwxtrans.db";

            //创建数据库实例，指定文件位置
            Program.m_mainform.g_conn = new SQLiteConnection(dbPath);
            Program.m_mainform.g_conn.Open();
            SQLiteDataAdapter mAdapter = new SQLiteDataAdapter("select * from WXAccount", Program.m_mainform.g_conn);

            DataTable dt = new DataTable();
            mAdapter.Fill(dt);
            //绑定数据到DataGridView
            dataGridView3.DataSource = dt;
            panel1.Hide();
            panel2.Hide();
            panel3.Hide();
            dataGridView3.Show();

            Types = new List<TreeNodeTypes>()
            {
                new TreeNodeTypes() {Id = 1, Name = @"证据（47\13433\13480）", Value = "1", ParentId = 0},
                new TreeNodeTypes() {Id = 2, Name = @"手机信息（47\13433\13480）", Value = "2", ParentId = 1},
                new TreeNodeTypes() {Id = 3, Name = @"即时通讯（47\13433\13480）", Value = "3", ParentId = 1},
                new TreeNodeTypes() {Id = 4, Name = @"基本信息（47\13433\13480）", Value = "4", ParentId = 2},
                new TreeNodeTypes() {Id = 5, Name = @"通讯录（47\13433\13480）", Value = "5", ParentId = 2},
                new TreeNodeTypes() {Id = 6, Name = @"微信（47\13433\13480）", Value = "6", ParentId = 3},
            };

            //string Name = "";
            using (SqliteDbContext context = new SqliteDbContext())
            {//ca9529dc14475dbcc7e8553e77ad7d0b
             //   context.Database.Connection.ConnectionString = "Data Source="+ "D:\\手机取证工作路径设置\\案件20190707093739\\HONORV2020190701094546\\AppData\\Weixin\\a12788cdac6ba28270d03cc2df9a0122" + "\\midwxtrans.db";
                context.WxAccounts.ToList().ForEach(acc =>
                {
                    int index = Types.Count + 1;
                    Types.Add(new TreeNodeTypes() { Id = index, Name = acc.Sign, ParentId = 6, Value = acc.WxId });
                    Types.Add(new TreeNodeTypes() { Id = index + 1, Name = "通讯录", ParentId = index, Value = "通讯录" });
                    Types.Add(new TreeNodeTypes() { Id = index + 2, Name = "公众号", ParentId = index, Value = "公众号" });
                    Types.Add(new TreeNodeTypes() { Id = index + 3, Name = "聊天记录", ParentId = index, Value = "聊天记录" });
                    Types.Add(new TreeNodeTypes() { Id = index + 4, Name = "群聊天记录", ParentId = index, Value = "群聊天记录" });
                    Types.Add(new TreeNodeTypes() { Id = index + 5, Name = "应用程序", ParentId = index, Value = "应用程序" });
                    Types.Add(new TreeNodeTypes() { Id = index + 6, Name = "朋友圈", ParentId = index, Value = "朋友圈" });
                    Types.Add(new TreeNodeTypes() { Id = index + 7, Name = "新朋友", ParentId = index, Value = "新朋友" });
                });
            }


            var topNode = new TreeNode();
            topNode.Name = "0";
            topNode.Text = Program.m_mainform.g_ajName + @"（已删除\未删除\总共）"; // @"案件（已删除\未删除\总共）";
            treeView1.Nodes.Add(topNode);
            Bind(topNode, Types, 0);

            treeView1.ExpandAll();

            //添加根节点
            TreeNode nodeAJ = new TreeNode();
            nodeAJ.Text = Program.m_mainform.g_ajName + @"（已删除\未删除\总共）";
            treeView2.Nodes.Add(nodeAJ);

            TreeNode nodeZJ = new TreeNode();
            nodeZJ.Text = Program.m_mainform.g_zjName + @"（ \ \ ）";
            nodeAJ.Nodes.Add(nodeZJ);           //添加证据结点

            TreeNode nodeBase = new TreeNode("基础信息");
            nodeZJ.Nodes.Add(nodeBase);

            int BaseNum = Program.m_mainform.checkBaseList.Count;
            for (int i = 0; i < BaseNum; i++)
            {
                TreeNode node = new TreeNode(Program.m_mainform.checkBaseList[i]);
                nodeBase.Nodes.Add(node);
            }

            int FileNum = Program.m_mainform.checkFileList.Count;
            if (FileNum > 0)
            {
                TreeNode nodeFile = new TreeNode("文件信息");
                nodeZJ.Nodes.Add(nodeFile);
                for (int i = 0; i < FileNum; i++)
                {
                    TreeNode node = new TreeNode(Program.m_mainform.checkFileList[i]);
                    nodeFile.Nodes.Add(node);
                }
            }

            int AppNum = Program.m_mainform.checkAppList.Count;

            if (AppNum > 0)
            {
                TreeNode nodeApp = new TreeNode("APP列表");
                nodeZJ.Nodes.Add(nodeApp);
                for (int i = 0; i < AppNum; i++)
                {
                    TreeNode node = new TreeNode(Program.m_mainform.checkAppList[i]);
                    nodeApp.Nodes.Add(node);
                    if (node.Text == "微信")
                    {
                        TreeNode node1 = new TreeNode("账号1");
                        node.Nodes.Add(node1);
                        TreeNode node11 = new TreeNode("账号信息");
                        node1.Nodes.Add(node11);
                        TreeNode node12 = new TreeNode("通讯录");
                        node1.Nodes.Add(node12);
                        TreeNode node121 = new TreeNode("好友");
                        node12.Nodes.Add(node121);
                        TreeNode node122 = new TreeNode("公众号");
                        node12.Nodes.Add(node122);
                        TreeNode node123 = new TreeNode("群聊");
                        node12.Nodes.Add(node123);
                        TreeNode node124 = new TreeNode("应用程序");
                        node12.Nodes.Add(node124);
                        TreeNode node125 = new TreeNode("其他");
                        node12.Nodes.Add(node125);


                        TreeNode node13 = new TreeNode("聊天记录");
                        node1.Nodes.Add(node13);
                        TreeNode node131 = new TreeNode("好友");
                        node13.Nodes.Add(node131);
                        TreeNode node132 = new TreeNode("群聊");
                        node13.Nodes.Add(node132);
                        TreeNode node133 = new TreeNode("公众号");
                        node13.Nodes.Add(node133);

                        TreeNode node14 = new TreeNode("朋友圈");
                        node1.Nodes.Add(node14);
                        TreeNode node141 = new TreeNode("本人的朋友圈");
                        node14.Nodes.Add(node141);
                        TreeNode node142 = new TreeNode("好友的朋友圈");
                        node14.Nodes.Add(node142);
                    }
                }
            }
            treeView2.ExpandAll();

        }

        private void Bind(TreeNode parNode, List<TreeNodeTypes> list, int nodeId)
        {
            var childList = list.FindAll(t => t.ParentId == nodeId).OrderBy(t => t.Id);

            foreach (var urlTypese in childList)
            {
                var node = new TreeNode();
                node.Name = urlTypese.Id.ToString();
                node.Text = urlTypese.Name;
                node.Tag = urlTypese.Value;
                parNode.Nodes.Add(node);
                Bind(node, list, urlTypese.Id);
            }
        }

        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string wxid = e.Node.Tag as string;
            string id = e.Node.Name;

            switch (wxid)
            {
                case "通讯录":
                    {
                        panel1.Hide();
                        panel3.Hide();
                        panel2.Show();
                        panel2.Dock = DockStyle.Fill;

                        this.dataGridView1.DataSource = null;
                        using (SqliteDbContext context = new SqliteDbContext())
                        {
                            this.dataGridView1.DataSource = context.WxFriends
                                .Where(friend => friend.Type == 3)
                                .ToList();
                        }

                        break;
                    }
                case "公众号":
                    {
                        panel1.Hide();
                        panel3.Hide();
                        panel2.Show();
                        panel2.Dock = DockStyle.Fill;

                        this.dataGridView1.DataSource = null;
                        using (SqliteDbContext context = new SqliteDbContext())
                        {
                            this.dataGridView1.DataSource = context.WxFriends
                                .Where(friend => friend.Type == 0)
                                .ToList();
                        }

                        break;
                    }
                case "应用程序":
                    {
                        panel1.Hide();
                        panel3.Hide();
                        panel2.Show();
                        panel2.Dock = DockStyle.Fill;

                        this.dataGridView1.DataSource = null;
                        using (SqliteDbContext context = new SqliteDbContext())
                        {
                            this.dataGridView1.DataSource = context.WxFriends
                                .Where(friend => friend.Type == 33)
                                .ToList();
                        }

                        break;
                    }
                case "聊天记录":
                    {
                        Name = "聊天记录";
                        panel1.Hide();
                        panel2.Hide();
                        panel3.Show();
                        dataGridView2.Show();
                        panel3.Dock = DockStyle.Fill;

                        this.dataGridView2.Rows.Clear();
                        this.richTextBoxEx1.Clear();
                        using (SqliteDbContext context = new SqliteDbContext())
                        {
                            this.dataGridView2.Rows.AddRange(
                                context.WxFriends
                                .Where(friend => friend.Type == 3)
                                .Select(new Func<WxFriend, DataGridViewRow>((f) =>
                                {
                                    var row = new DataGridViewRow();
                                    row.CreateCells(this.dataGridView2, new[] { f.NickName, context.WxMessages.Count(m => m.WxId == f.WxId).ToString() });
                                    return row;
                                }))
                                .ToArray());
                        }

                        break;
                    }
                case "群聊天记录":
                    {
                        panel1.Hide();
                        panel2.Hide();
                        panel3.Show();
                        dataGridView2.Show();
                        panel3.Dock = DockStyle.Fill;

                        this.dataGridView2.Rows.Clear();
                        this.richTextBoxEx1.Clear();
                        using (SqliteDbContext context = new SqliteDbContext())
                        {
                            this.dataGridView2.Rows.AddRange(
                                context.WxFriends
                                .Where(friend => friend.Type == 4)
                                .Select(new Func<WxFriend, DataGridViewRow>((f) =>
                                {
                                    var row = new DataGridViewRow();
                                    row.CreateCells(this.dataGridView2, new[] { f.NickName, context.WxMessages.Count(m => m.WxId == f.WxId).ToString() });
                                    return row;
                                }))
                                .ToArray());
                        }

                        break;
                    }
                case "朋友圈":
                    {
                        Name = "朋友圈";
                        panel1.Hide();
                        panel2.Hide();
                        panel3.Show();
                        dataGridView2.Show();
                        panel3.Dock = DockStyle.Fill;

                        this.dataGridView2.Rows.Clear();
                        this.richTextBoxEx1.Clear();
                        using (SqliteDbContext context = new SqliteDbContext())
                        {
                            this.dataGridView2.Rows.AddRange(
                                context.WxFriends
                                .Where(friend => friend.Type == 3)
                                .Select(new Func<WxFriend, DataGridViewRow>((f) =>
                                {
                                    var row = new DataGridViewRow();
                                    row.CreateCells(this.dataGridView2, new[] { f.NickName, context.WxSns.Count(s => s.WxId == f.WxId).ToString() });
                                    return row;
                                }))
                                .ToArray());
                        }

                        break;
                    }
                case "新朋友":
                    {
                        panel1.Hide();
                        panel3.Hide();
                        panel2.Show();
                        panel2.Dock = DockStyle.Fill;

                        this.dataGridView1.DataSource = null;
                        using (SqliteDbContext context = new SqliteDbContext())
                        {
                            this.dataGridView1.DataSource = context.WxNewFriend.ToList();
                        }

                        break;
                    }
                default:
                    {
                        panel2.Hide();
                        panel3.Hide();
                        panel1.Show();
                        panel1.Dock = DockStyle.Fill;
                        WxAccount acc = null;
                        using (SqliteDbContext context = new SqliteDbContext())
                        {
                            acc = context.WxAccounts.FirstOrDefault(a => a.WxId == wxid);

                        }

                        if (acc == null)
                        {
                            if (Types != null && Types.Count > 0)
                            {
                                foreach (TreeNodeTypes tnt in Types)
                                {
                                    if (Convert.ToString(tnt.Id) == id)
                                    {
                                        lblCode.Text = tnt.Value;
                                        lblName.Text = tnt.Name;
                                        label8.Text = string.Empty;
                                        label9.Text = string.Empty;
                                        label10.Text = string.Empty;
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            lblCode.Text = acc.WxId;
                            lblName.Text = acc.Sign;
                            label8.Text = acc.Phone;
                            label9.Text = acc.NickName;
                            label10.Text = acc.District;
                        }

                        break;
                    }
            }
        }
        private void TreeView2_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }
        private void DataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            try
            {
                DataGridViewCheckBoxCell dgcc = (DataGridViewCheckBoxCell)this.dataGridView2.Rows[e.RowIndex].Cells[0];
                Boolean flag = Convert.ToBoolean(dgcc.Value);
                dgcc.Value = flag == true ? false : true;
            }
            catch (Exception)
            {
            }

            if (e.RowIndex < 0)
            {
                return;
            }

            using (SqliteDbContext context = new SqliteDbContext())
            {
                richTextBoxEx1.Clear();
                string nickName = this.dataGridView2.Rows[e.RowIndex].Cells[0].Value as string;
                WxFriend friend = context.WxFriends.FirstOrDefault(f => f.NickName == nickName);//仅查找一条数据
                WxAccount account = context.WxAccounts.FirstOrDefault(a => a.Id == 1);

                if (friend == null)//如果没有找到
                {
                    return;
                }
                if (Name == "聊天记录")
                {
                    //（treeView1.selectedNode.Name == “name1”）
                    var messages = context.WxMessages //
                        .Where(m => m.WxId == friend.WxId)
                        .OrderBy(m => m.CreateTime)
                        .ToList();

                    messages.ForEach(m =>
                    {
                        if (m.IsSend == 1)
                        {
                            richTextBoxEx1.SelectionAlignment = HorizontalAlignment.Right;
                            richTextBoxEx1.SelectionColor = Color.DimGray;
                            richTextBoxEx1.SelectionBackColor = Color.WhiteSmoke;
                            richTextBoxEx1.AppendText("(");
                            richTextBoxEx1.AppendText($"{account.NickName}");
                            richTextBoxEx1.AppendText(")");
                            richTextBoxEx1.AppendText($"{account.WxId}");
                            System.Drawing.Image img = System.Drawing.Image.FromFile(@"D:\UDisk\avator" + account.AvatarPath);
                            Bitmap bmp = new Bitmap(img, 25, 22);
                            Clipboard.SetDataObject(bmp);
                            DataFormats.Format dataFormat =
                            DataFormats.GetFormat(DataFormats.Bitmap);
                            if (richTextBoxEx1.CanPaste(dataFormat))
                                richTextBoxEx1.Paste(dataFormat);
                            richTextBoxEx1.AppendText("\n");
                            richTextBoxEx1.SelectionColor = Color.Red;
                        }
                        else
                        {
                            richTextBoxEx1.SelectionAlignment = HorizontalAlignment.Left;
                            richTextBoxEx1.SelectionColor = Color.DimGray;
                            richTextBoxEx1.SelectionBackColor = Color.Blue;
                            System.Drawing.Image img = System.Drawing.Image.FromFile(@"D:\UDisk\avator" + friend.AvatarPath);
                            Bitmap bmp = new Bitmap(img, 25, 22);
                            Clipboard.SetDataObject(bmp);
                            DataFormats.Format dataFormat =
                            DataFormats.GetFormat(DataFormats.Bitmap);
                            if (richTextBoxEx1.CanPaste(dataFormat))
                                richTextBoxEx1.Paste(dataFormat);
                            richTextBoxEx1.AppendText("(");
                            richTextBoxEx1.AppendText($"{friend.NickName}");
                            richTextBoxEx1.AppendText(")");
                            richTextBoxEx1.AppendText($"{m.WxId}\n");
                        }
                        //在sqlite的message表里面，有的对话的path字段含有空格，需要使用Replace进行去除
                        if (string.IsNullOrEmpty(m.Path.Replace(" ", "")))//message是单纯的对话消息 或者 是含有url链接的消息
                        {
                            SelectUrl.print_msg_or_url(m.Content, richTextBoxEx1);//该方法在打印message内容的同时能够识别url
                            //Console.WriteLine("是对话或者链接："+m.Content);
                        }
                        else//说明message是文件类型的消息，这时候应当能够点击链接并打开文件
                        {
                            //Console.WriteLine("文件类型：" + m.Content);
                            SelectUrl.print_file(m.Path,richTextBoxEx1);
                        }
                        richTextBoxEx1.AppendText($"{m.CreateTime}\n\n\n");
                    });
                }
                else if (Name == "朋友圈")
                {
                    var sns = context.WxSns
                          .Where(s => s.WxId == friend.WxId)
                          .OrderBy(s => s.CreateTime)
                          .ToList();

                    sns.ForEach(s =>
                    {
                        richTextBoxEx1.SelectionAlignment = HorizontalAlignment.Left;
                        richTextBoxEx1.SelectionColor = Color.DimGray;
                        System.Drawing.Image img = System.Drawing.Image.FromFile(@"D:\UDisk\avator" + friend.AvatarPath);
                        Bitmap bmp = new Bitmap(img, 25, 22);
                        Clipboard.SetDataObject(bmp);
                        DataFormats.Format dataFormat =
                        DataFormats.GetFormat(DataFormats.Bitmap);
                        if (richTextBoxEx1.CanPaste(dataFormat))
                            richTextBoxEx1.Paste(dataFormat);
                        richTextBoxEx1.AppendText("(");
                        richTextBoxEx1.AppendText($"{friend.NickName}");
                        richTextBoxEx1.AppendText(")");
                        richTextBoxEx1.AppendText($"{s.WxId}\n");

                        richTextBoxEx1.SelectionColor = Color.Blue;
                        richTextBoxEx1.AppendText($"{s.Content}\n");
                        richTextBoxEx1.AppendText($"{s.CreateTime}\n\n\n\n");


                        richTextBoxEx1.SelectionBackColor = Color.WhiteSmoke;

                    });
                }
            }

        }

        private void dgV1CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewCheckBoxCell dgcc = (DataGridViewCheckBoxCell)this.dataGridView1.Rows[e.RowIndex].Cells[0];
                Boolean flag = Convert.ToBoolean(dgcc.Value);
                dgcc.Value = flag == true ? false : true;
            }
            catch (Exception)
            {
            }
        }
        //private void dgV3CellClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    try
        //    {
        //        DataGridViewCheckBoxCell dgcc = (DataGridViewCheckBoxCell)this.dataGridView3.Rows[e.RowIndex].Cells[0];
        //        Boolean flag = Convert.ToBoolean(dgcc.Value);
        //        dgcc.Value = flag == true ? false : true;
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}

        private void button1_Click(object sender, EventArgs e)
        {
            FormGjglZzss form = new FormGjglZzss();
            form.ShowDialog();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ////添加根节点
            //TreeNode nodeAJ = new TreeNode();
            //nodeAJ.Text = Program.m_mainform.g_ajName + @"（已删除\未删除\总共）";
            //treeView2.Nodes.Add(nodeAJ);

            //TreeNode nodeZJ = new TreeNode();
            //nodeZJ.Text = Program.m_mainform.g_zjName + @"（ \ \ ）";
            //nodeAJ.Nodes.Add(nodeZJ);           //添加证据结点

            //TreeNode nodeBase = new TreeNode("基础信息");
            //nodeZJ.Nodes.Add(nodeBase);

            //int BaseNum = Program.m_mainform.checkBaseList.Count;
            //for (int i = 0; i < BaseNum; i++)
            //{
            //    TreeNode node = new TreeNode(Program.m_mainform.checkBaseList[i]);
            //    nodeBase.Nodes.Add(node);
            //}

            //int FileNum = Program.m_mainform.checkFileList.Count;
            //if (FileNum > 0)
            //{
            //    TreeNode nodeFile = new TreeNode("文件信息");
            //    nodeZJ.Nodes.Add(nodeFile);
            //    for (int i = 0; i < FileNum; i++)
            //    {
            //        TreeNode node = new TreeNode(Program.m_mainform.checkFileList[i]);
            //        nodeFile.Nodes.Add(node);
            //    }
            //}

            //int AppNum = Program.m_mainform.checkAppList.Count;
            //if (AppNum > 0)
            //{
            //    TreeNode nodeApp = new TreeNode("APP列表");
            //    nodeZJ.Nodes.Add(nodeApp);
            //    for (int i = 0; i < AppNum; i++)
            //    {
            //        TreeNode node = new TreeNode(Program.m_mainform.checkAppList[i]);
            //        nodeApp.Nodes.Add(node);
            //        if (node.Text == "微信")
            //        {
            //            TreeNode node1 = new TreeNode("账号1");
            //            node.Nodes.Add(node1);
            //            TreeNode node11 = new TreeNode("账号信息");
            //            node1.Nodes.Add(node11);
            //            TreeNode node12 = new TreeNode("通讯录");
            //            node1.Nodes.Add(node12);
            //            TreeNode node121 = new TreeNode("好友");
            //            node12.Nodes.Add(node121);
            //            TreeNode node122 = new TreeNode("公众号");
            //            node12.Nodes.Add(node122);
            //            TreeNode node123 = new TreeNode("群聊");
            //            node12.Nodes.Add(node123);
            //            TreeNode node124 = new TreeNode("应用程序");
            //            node12.Nodes.Add(node124);
            //            TreeNode node125 = new TreeNode("其他");
            //            node12.Nodes.Add(node125);


            //            TreeNode node13 = new TreeNode("聊天记录");
            //            node1.Nodes.Add(node13);
            //            TreeNode node131 = new TreeNode("好友");
            //            node13.Nodes.Add(node131);
            //            TreeNode node132 = new TreeNode("群聊");
            //            node13.Nodes.Add(node132);
            //            TreeNode node133 = new TreeNode("公众号");
            //            node13.Nodes.Add(node133);

            //            TreeNode node14 = new TreeNode("朋友圈");
            //            node1.Nodes.Add(node14);
            //            TreeNode node141 = new TreeNode("本人的朋友圈");
            //            node14.Nodes.Add(node141);
            //            TreeNode node142 = new TreeNode("好友的朋友圈");
            //            node14.Nodes.Add(node142);
            //        }
            //    }
            //}

            //  treeView2.ExpandAll();


            //SQLiteDataAdapter mAdapter = new SQLiteDataAdapter("select * from WXAccount", Program.m_mainform.g_conn);
            //DataTable dt = new DataTable();
            //mAdapter.Fill(dt);
            ////绑定数据到DataGridView
            //dataGridView3.DataSource = dt;

            //panel1.Hide();
            //panel2.Hide();
            //panel3.Hide(); 
            //dataGridView3.Show();
            //dataGridView3.Dock = DockStyle.Fill;

        }

        public System.Diagnostics.Process p = new System.Diagnostics.Process();

        private void richTextBoxEx1_LinkClicked(object sender, LinkClickedEventArgs e)//RichTextBox的点击事件
        {

            /*
             设置根目录，从message表的path字段中取出的值诸如：
             \com.tencent.mm\MicroMsg\9a444f2bffa22236d9e4313dc93683c8\video\1652293005198de450297040.mp4
             \com.tencent.mm\MicroMsg\9a444f2bffa22236d9e4313dc93683c8\voice2\e1\7a\msg_2216490530198de4502faaa104.amr
             \tencent\MicroMsg\Download\rcontact.txt
             但是，这只是一个相对的路径(可以把它称为绝对路径的后一部分)，需要进行拼接形成完整的路径
             */
            string baseDir = "D:";//假设baseDir是绝对路径的前一部分
            string like = e.LinkText;
            int index = like.IndexOf('\\');//如果链接中含有反斜杠，说明这是一个文件
            if(index != -1)//如果是文件
            {//文件
                string file_dir = string.Format("{0}{1}", baseDir, like.Substring(index));//把前一部分和后一部分拼接成绝对路径
                //Console.WriteLine(file_dir);
                string result = WechatLinkFile.display_file(file_dir);//打开文件
                if(result != "")
                {
                    MessageBox.Show(string.Format("{0}{1}\n{2}", "打开文件异常：",file_dir, result));
                }
            }else
            {//如果是url链接
                WechatLinkFile.display_url(like);
            }

        }
    }

    public class TreeNodeTypes
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public int ParentId { get; set; }
    }
}
