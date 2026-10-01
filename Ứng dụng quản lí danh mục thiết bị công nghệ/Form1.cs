using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Ứng_dụng_quản_lí_danh_mục_thiết_bị_công_nghệ
{
    public partial class Form1 : Form
    {
        private BindingList<Product> products = new BindingList<Product>();
        private BindingSource bs = new BindingSource();
        private string currentImagePath = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var categories = new List<CategoryItem>
            {
                new CategoryItem("Điện thoại","Điện thoại"),
                new CategoryItem("Laptop","Laptop"),
                new CategoryItem("Phụ kiện","Phụ kiện")
            };
            cboCategory.DisplayMember = "Name";
            cboCategory.ValueMember = "Value";
            cboCategory.DataSource = categories;

            bs.DataSource = products;
            dgvProducts.DataSource = bs;

            dgvProducts.Columns.Clear();
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Mã SP", DataPropertyName = "ProductId", Width = 80 });
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Tên SP", DataPropertyName = "ProductName", Width = 200 });
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Danh mục", DataPropertyName = "Category", Width = 120 });
            var colPrice = new DataGridViewTextBoxColumn { HeaderText = "Đơn giá", DataPropertyName = "UnitPrice", Width = 120 };
            colPrice.DefaultCellStyle.Format = "N0";
            colPrice.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProducts.Columns.Add(colPrice);
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Số lượng", DataPropertyName = "Quantity", Width = 80 });

            UpdateStatus();
        }

        private void UpdateStatus()
        {
            toolStripStatusLabel1.Text = $"Tổng số sản phẩm: {products.Count}";
        }

        private bool ValidateInputs()
        {
            bool ok = true;
            errorProvider1.Clear();
            if (string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                errorProvider1.SetError(txtProductName, "Tên SP không được trống.");
                ok = false;
            }
            if (!decimal.TryParse(txtUnitPrice.Text, out decimal price) || price <= 0)
            {
                errorProvider1.SetError(txtUnitPrice, "Đơn giá phải là số lớn hơn 0.");
                ok = false;
            }
            if (!int.TryParse(txtQuantity.Text, out int qty) || qty < 0)
            {
                errorProvider1.SetError(txtQuantity, "Số lượng phải là số nguyên không âm.");
                ok = false;
            }
            return ok;
        }

        private void ClearInputs()
        {
            txtProductId.Clear();
            txtProductName.Clear();
            txtUnitPrice.Clear();
            txtQuantity.Clear();
            cboCategory.SelectedIndex = 0;
            picAvatar.Image = null;
            currentImagePath = null;
            errorProvider1.Clear();
        }

        private void btnChooseImage_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp;*.gif|All files|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var img = Image.FromFile(ofd.FileName);
                        picAvatar.Image = img;
                        currentImagePath = ofd.FileName;
                    }
                    catch
                    {
                        MessageBox.Show("Không thể mở file ảnh.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;
            var prod = new Product
            {
                ProductId = txtProductId.Text.Trim(),
                ProductName = txtProductName.Text.Trim(),
                Category = cboCategory.Text,
                UnitPrice = decimal.Parse(txtUnitPrice.Text),
                Quantity = int.Parse(txtQuantity.Text),
                ImagePath = currentImagePath
            };
            products.Add(prod);
            RefreshGridFilter();
            UpdateStatus();
            ClearInputs();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (bs.Current is Product sel && ValidateInputs())
            {
                sel.ProductId = txtProductId.Text.Trim();
                sel.ProductName = txtProductName.Text.Trim();
                sel.Category = cboCategory.Text;
                sel.UnitPrice = decimal.Parse(txtUnitPrice.Text);
                sel.Quantity = int.Parse(txtQuantity.Text);
                sel.ImagePath = currentImagePath;
                
                bs.ResetCurrentItem();
                UpdateStatus();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (bs.Current is Product sel)
            {
                var dr = MessageBox.Show($"Xác nhận xóa sản phẩm '{sel.ProductName}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    products.Remove(sel);
                    RefreshGridFilter();
                    UpdateStatus();
                    ClearInputs();
                }
            }
        }

        private void dgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            LoadSelectedProduct();
        }

        private void LoadSelectedProduct()
        {
            if (bs.Current is Product sel)
            {
                txtProductId.Text = sel.ProductId;
                txtProductName.Text = sel.ProductName;
                txtUnitPrice.Text = sel.UnitPrice.ToString();
                txtQuantity.Text = sel.Quantity.ToString();
                // select category
                for (int i = 0; i < cboCategory.Items.Count; i++)
                {
                    var it = cboCategory.Items[i] as CategoryItem;
                    if (it != null && it.Name == sel.Category)
                    {
                        cboCategory.SelectedIndex = i;
                        break;
                    }
                }
                currentImagePath = sel.ImagePath;
                try
                {
                    picAvatar.Image = string.IsNullOrEmpty(sel.ImagePath) ? null : Image.FromFile(sel.ImagePath);
                }
                catch { picAvatar.Image = null; }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            RefreshGridFilter();
        }

        private void RefreshGridFilter()
        {
            var q = txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(q))
            {
                bs.DataSource = products;
            }
            else
            {
                var filtered = products.Where(p => p.ProductName != null && p.ProductName.IndexOf(q, StringComparison.CurrentCultureIgnoreCase) >= 0).ToList();
                bs.DataSource = new BindingList<Product>(filtered);
            }
            dgvProducts.Refresh();
        }

        private void exportCSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV files|*.csv|All files|*.*";
                sfd.FileName = "products.csv";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (var sw = new StreamWriter(sfd.FileName, false, Encoding.UTF8))
                        {
                            sw.WriteLine("ProductId,ProductName,Category,UnitPrice,Quantity,ImagePath");
                            foreach (var p in products)
                            {
                                string line = $"{Quote(p.ProductId)},{Quote(p.ProductName)},{Quote(p.Category)},{p.UnitPrice},{p.Quantity},{Quote(p.ImagePath)}";
                                sw.WriteLine(line);
                            }
                        }
                        MessageBox.Show("Xuất CSV thành công.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xuất CSV: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private string Quote(string s)
        {
            if (s == null) return "";
            return '"' + s.Replace("\"", "\"\"") + '"';
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

    class CategoryItem
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public CategoryItem(string name, string value)
        {
            Name = name; Value = value;
        }
        public override string ToString() => Name;
    }
}
