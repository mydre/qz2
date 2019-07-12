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

            using (SqliteDbContext context = new SqliteDbContext())
            {//ca9529dc14475dbcc7e8553e77ad7d0b
             //   context.Database.Connection.ConnectionString = "Data Source="+ "D:\\手机取证工作路径设置\\案件20190707093739\\HONORV2020190701094546\\AppData\\Weixin\\a12788cdac6ba28270d03cc2df9a0122" + "\\midwxtrans.db";
                context.WxAccounts.ToList().ForEach(acc =>
                {
                    int index = Types.Count + 1;
                    Types.Add(new TreeNodeTypes() { Id = index, Name = acc.Sign, ParentId = 6, Value = acc.WxId });
                    Types.Add(new TreeNodeTypes() { Id = index + 1, Name = "好友", ParentId = index, Value = "好友" });
                    Types.Add(new TreeNodeTypes() { Id = index + 2, Name = "公众号", ParentId = index, Value = "公众号" });
                    Types.Add(new TreeNodeTypes() { Id = index + 3, Name = "聊天记录", ParentId = index, Value = "聊天记录" });
                    Types.Add(new TreeNodeTypes() { Id = index + 4, Name = "应用程序", ParentId = index, Value = "应用程序" });
                    Types.Add(new TreeNodeTypes() { Id = index + 5, Name = "朋友圈", ParentId = index, Value = "朋友圈" });
                    Types.Add(new TreeNodeTypes() { Id = index + 6, Name = "新朋友", ParentId = index, Value = "新朋友" });
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
                case "好友":
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
                        panel1.Hide();
                        panel2.Hide();
                        panel3.Show();
                        panel3.Dock = DockStyle.Fill;

                        this.dataGridView2.Rows.Clear();
                        using (SqliteDbContext context = new SqliteDbContext())
                        {
                            this.dataGridView2.Rows.AddRange(
                                context.WxFriends
                                .Where(friend => friend.Type == 3)
                                .Select(new Func<WxFriend, DataGridViewRow>((f) =>
                                {
                                    var row = new DataGridViewRow();
                                    row.CreateCells(this.dataGridView2, new[] {null, f.NickName, context.WxMessages.Count(m => m.WxId == f.WxId).ToString() });
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
                richTextBox1.Clear();
                string nickName = this.dataGridView2.Rows[e.RowIndex].Cells[0].Value as string;
                WxFriend friend = context.WxFriends.FirstOrDefault(f => f.NickName == nickName);//仅查找一条数据
                if (friend == null)//如果没有找到
                {
                    return;
                }

                var messages = context.WxMessages //
                    .Where(m => m.WxId == friend.WxId)
                    .OrderBy(m => m.CreateTime)
                    .ToList();

                messages.ForEach(m =>
                {
                    if (m.IsSend == 1)
                    {
                        richTextBox1.SelectionAlignment = HorizontalAlignment.Right;
                        richTextBox1.SelectionColor = Color.DimGray;
                        richTextBox1.AppendText($"{m.CreateTime}\n");
                        richTextBox1.SelectionColor = Color.Red;
                    }
                    else
                    {
                        richTextBox1.SelectionAlignment = HorizontalAlignment.Left;
                        richTextBox1.SelectionColor = Color.DimGray;
                        richTextBox1.AppendText($"{m.CreateTime}\n");
                        richTextBox1.SelectionColor = Color.Blue;
                    }

                    richTextBox1.SelectionBackColor = Color.WhiteSmoke;
                    richTextBox1.AppendText($"{m.Content}\n\n");
                });
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
    }

    public class TreeNodeTypes
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public int ParentId { get; set; }
    }
}
