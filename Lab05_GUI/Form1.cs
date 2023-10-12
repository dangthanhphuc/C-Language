using Lab05_BUS;
using Lab05_DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab05_GUI
{
    public partial class frmQuanLySinhVien : Form
    {
        private readonly StudentService studentService = new StudentService();
        private readonly FacultyService facultyService = new FacultyService();

        public frmQuanLySinhVien()
        {
            InitializeComponent();
        }

        private void frmQuanLySinhVien_Load(object sender, EventArgs e)
        {
            try
            {
                setGridViewStyle(dgvDanhSach);
                var listFacultys = facultyService.GetAll();
                var listStudents = studentService.GetAll();
                FillFalcultyCombobox(listFacultys);
                BindGrid(listStudents);
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

           
        }

        private void FillFalcultyCombobox(List<Faculty> listFacultys)
        {
            listFacultys.Insert(0, new Faculty());
            this.cmbKhoa.DataSource = listFacultys;
            this.cmbKhoa.DisplayMember = "FacultyName";
            this.cmbKhoa.ValueMember = "FacultyID";
        }

        private void BindGrid(List<Student> listStudents)
        {
            
            dgvDanhSach.Rows.Clear();
            foreach (var item in listStudents)
            {
                int index = dgvDanhSach.Rows.Add();
                dgvDanhSach.Rows[index].Cells[0].Value = item.StudentID;
                dgvDanhSach.Rows[index].Cells[1].Value = item.FullName;

                if (item.Faculty != null)
                    dgvDanhSach.Rows[index].Cells[2].Value = item.Faculty.FacultyName;

                dgvDanhSach.Rows[index].Cells[3].Value = item.AverageScore + "";

                if (item.MajorID != null)
                    dgvDanhSach.Rows[index].Cells[4].Value = item.Major.Name + "";

/*                ShowAvatar(item.Avatar);
*/          }
        }

        private void ShowAvatar(string ImageName)
        {
            if (string.IsNullOrEmpty(ImageName))
            {
                pibAnhDaiDien.Image = null;
            }
            else
            {
                string parentDirectory =
                Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                string imagePath = Path.Combine(parentDirectory, "Images", ImageName);
                pibAnhDaiDien.Image = Image.FromFile(imagePath);
                pibAnhDaiDien.Refresh();
            }
        }

        private void setGridViewStyle(DataGridView dgvDanhSach)
        {
            dgvDanhSach.BorderStyle = BorderStyle.None;
            dgvDanhSach.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise;
            dgvDanhSach.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvDanhSach.BackgroundColor = Color.White;
            dgvDanhSach.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void ckbDkChuyenNganh_CheckedChanged(object sender, EventArgs e)
        {
            var listStudents = new List<Student>();
            if (this.ckbDkChuyenNganh.Checked)
                listStudents = studentService.GetAllHasNoMajor();
            else
                listStudents = studentService.GetAll();
            BindGrid(listStudents);
            
        }
        private bool RangBuoc()
        {
            if( int.TryParse(txtMaSinhVien.Text, out int vl) == false)
            {
                MessageBox.Show("MSSV chỉ được nhập số");
                return false;
            }
            if( double.TryParse(txtDiemTrungBinh.Text, out double vld) == false)
            {
                MessageBox.Show("Điểm trung bình chỉ được nhập số thực");
                return false;
            }

            return true;
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if(RangBuoc() == false)
            {
                return;
            }

            Student student = new Student()
            {
                StudentID = txtMaSinhVien.Text,
                FullName = txtHoTen.Text,
                FacultyID = int.Parse(cmbKhoa.SelectedValue.ToString()),
                AverageScore = double.Parse(txtDiemTrungBinh.Text)
            };

            studentService.InsertUpdate(student);
            var listStudents = studentService.GetAll();
            BindGrid(listStudents);

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (RangBuoc() == false)
            {
                return;
            }

            Student student = new Student()
            {
                StudentID = txtMaSinhVien.Text,
                FullName = txtHoTen.Text,
                FacultyID = int.Parse(cmbKhoa.SelectedValue.ToString()),
                AverageScore = double.Parse(txtDiemTrungBinh.Text)
            };

            studentService.InsertUpdate(student);
            var listStudents = studentService.GetAll();
            BindGrid(listStudents);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (RangBuoc() == false)
            {
                return;
            }

            studentService.DeleteById(txtMaSinhVien.Text);
            var listStudents = studentService.GetAll();
            BindGrid(listStudents);
        }

        private void dgvDanhSach_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            txtMaSinhVien.Text = dgvDanhSach.Rows[rowIndex].Cells[0].Value.ToString();
            txtHoTen.Text = dgvDanhSach.Rows[rowIndex].Cells[1].Value.ToString();
            cmbKhoa.SelectedIndex = cmbKhoa.FindString(dgvDanhSach.Rows[rowIndex].Cells[2].Value.ToString());
            txtDiemTrungBinh.Text = dgvDanhSach.Rows[rowIndex].Cells[3].Value.ToString();
        }

        private void đăngKíChuyênNgànhToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDangKyChuyenNganh frm = new frmDangKyChuyenNganh();
            frm.ShowDialog();
        }
    }
}
