﻿using System;
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
using System.Threading;

namespace WinAppDemo.Controls
{
    public partial class UcAjgl : UserControl
    {
        public UcAjgl()
        {
            InitializeComponent();
        }

        private void UcAjgl_SizeChanged(object sender, EventArgs e)
        {
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            //新建案件
            FormGjglNewAj form = new FormGjglNewAj();
            form.ShowDialog();
            Case @case = form.Case;

            using (var context = new CaseContext())
            {
                context.Cases.Add(@case);
                context.SaveChanges();
                AppContext.CaseID = @case.CaseId;
            }
        }

        private void UcAjgl_Load(object sender, EventArgs e)
        {
            using (var context = new CaseContext())
            {

                var cases = context.Cases.AsNoTracking().ToList();
                this.dataGridView1.DataSource = cases;
            }
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
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

        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                return;
            }

            var row = this.dataGridView1.SelectedRows[0];
            int caseId = (int)row.Cells[1].Value;
            this.label5.Text = caseId.ToString();
            this.label7.Text = row.Cells[1]?.Value?.ToString() ?? string.Empty;

            using (var context = new CaseContext())
            {
                var proofs = context.Proofs.AsNoTracking().Where(p => p.CaseID == caseId).ToList();
                dataGridView2.DataSource = proofs;
            }

        }



        private void clickAddEvidence(object sender, EventArgs e)
        {
            //Program.m_mainform.AddNewGjalZj();
            Console.WriteLine(this.dataGridView1.SelectedRows[0].Index);

            UcZjtq uc = new UcZjtq();
            uc.Dock = DockStyle.Fill;
            var p = this.Parent.Parent.Controls["WinContent"].Controls;
            this.Parent.Parent.Controls["WinContent"].Controls.Clear();
            p.Add(uc);


        }

        private void allCheckedChanged(object sender, EventArgs e)
        {
            CheckBox c = sender as CheckBox;
            bool allCheck = false;
            if(c.Checked == true)
            {
                allCheck = true;
            }
            foreach (DataGridViewRow dgvr in dataGridView1.Rows)
            {
                DataGridViewCheckBoxCell dgcc = (DataGridViewCheckBoxCell)dgvr.Cells[0];
                Boolean flag = Convert.ToBoolean(allCheck);
                dgcc.Value = flag;
            }
        }
    }
}
