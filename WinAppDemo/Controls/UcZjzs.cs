using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinAppDemo.Db.Base;
using WinAppDemo.Db.Model;

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
            Types = new List<TreeNodeTypes>()
            {
                new TreeNodeTypes() {Id = 1, Name = @"努比亚降级（47\13433\13480）", Value = "1", ParentId = 0},
                new TreeNodeTypes() {Id = 2, Name = @"手机信息（47\13433\13480）", Value = "2", ParentId = 1},
                new TreeNodeTypes() {Id = 3, Name = @"即时通讯（47\13433\13480）", Value = "3", ParentId = 1},
                new TreeNodeTypes() {Id = 4, Name = @"基本信息（47\13433\13480）", Value = "4", ParentId = 2},
                new TreeNodeTypes() {Id = 5, Name = @"通讯录（47\13433\13480）", Value = "5", ParentId = 2},
                new TreeNodeTypes() {Id = 6, Name = @"微信（47\13433\13480）", Value = "6", ParentId = 3},
            };

            string Name = "";
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
            topNode.Text = @"努比亚备份（已删除\未删除\总共）";
            treeView1.Nodes.Add(topNode);
            Bind(topNode, Types, 0);

            treeView1.ExpandAll();
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
                        this.richTextBox1.Clear();
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
                        this.richTextBox1.Clear();
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
                        this.richTextBox1.Clear();
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
                            richTextBox1.SelectionAlignment = HorizontalAlignment.Right;
                            richTextBox1.SelectionColor = Color.DimGray;

                            richTextBox1.AppendText("(");
                            richTextBox1.AppendText($"{account.NickName}");
                            richTextBox1.AppendText(")");
                            richTextBox1.AppendText($"{account.WxId}");

                            System.Drawing.Image img = System.Drawing.Image.FromFile(@"D:\UDisk\avator" + account.AvatarPath);
                            Bitmap bmp = new Bitmap(img, 25, 22);
                            Clipboard.SetDataObject(bmp);
                            DataFormats.Format dataFormat =
                            DataFormats.GetFormat(DataFormats.Bitmap);
                            if (richTextBox1.CanPaste(dataFormat))
                                richTextBox1.Paste(dataFormat);
                            richTextBox1.AppendText("\n");


                            richTextBox1.SelectionColor = Color.Red;
                            richTextBox1.AppendText($"{m.Content}\n");
                            richTextBox1.AppendText($"{m.CreateTime}\n\n\n");

                        }
                        else
                        {
                            richTextBox1.SelectionAlignment = HorizontalAlignment.Left;
                            richTextBox1.SelectionColor = Color.DimGray;

                            System.Drawing.Image img = System.Drawing.Image.FromFile(@"D:\UDisk\avator" + friend.AvatarPath);
                            Bitmap bmp = new Bitmap(img, 25, 22);
                            Clipboard.SetDataObject(bmp);
                            DataFormats.Format dataFormat =
                            DataFormats.GetFormat(DataFormats.Bitmap);
                            if (richTextBox1.CanPaste(dataFormat))
                                richTextBox1.Paste(dataFormat);

                            richTextBox1.AppendText("(");
                            richTextBox1.AppendText($"{friend.NickName}");
                            richTextBox1.AppendText(")");
                            richTextBox1.AppendText($"{m.WxId}\n");

                            richTextBox1.SelectionColor = Color.Blue;
                            richTextBox1.AppendText($"{m.Content}\n");
                            richTextBox1.AppendText($"{m.CreateTime}\n\n\n");

                        }

                        richTextBox1.SelectionBackColor = Color.WhiteSmoke;
                        // richTextBox1.AppendText($"{m.Content}\n\n");
                    });
                }
                else
                if (Name == "朋友圈")
                {

                    var sns = context.WxSns
                          .Where(s => s.WxId == friend.WxId)
                          .OrderBy(s => s.CreateTime)
                          .ToList();

                    sns.ForEach(s =>
                    {
                        richTextBox1.SelectionAlignment = HorizontalAlignment.Left;
                        richTextBox1.SelectionColor = Color.DimGray;
                        System.Drawing.Image img = System.Drawing.Image.FromFile(@"D:\UDisk\avator" + friend.AvatarPath);
                        Bitmap bmp = new Bitmap(img, 25, 22);
                        Clipboard.SetDataObject(bmp);
                        DataFormats.Format dataFormat =
                        DataFormats.GetFormat(DataFormats.Bitmap);
                        if (richTextBox1.CanPaste(dataFormat))
                            richTextBox1.Paste(dataFormat);
                        richTextBox1.AppendText("(");
                        richTextBox1.AppendText($"{friend.NickName}");
                        richTextBox1.AppendText(")");
                        richTextBox1.AppendText($"{s.WxId}\n");

                        richTextBox1.SelectionColor = Color.Blue;
                        richTextBox1.AppendText($"{s.Content}\n");
                        richTextBox1.AppendText($"{s.CreateTime}\n\n\n\n");


                        richTextBox1.SelectionBackColor = Color.WhiteSmoke;

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
    }

    public class TreeNodeTypes
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public int ParentId { get; set; }
    }
}
