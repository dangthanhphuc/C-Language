using Lab05_BUS;
using Lab05_DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab05_GUI
{
    public partial class frmDangKyChuyenNganh : Form
    {
        private readonly StudentService studentService = new StudentService();
        private readonly FacultyService facultyService = new FacultyService();
        private readonly MajorService majorService = new MajorService();

        public frmDangKyChuyenNganh()
        {
            InitializeComponent();
        }

        private void frmRegister_Load(object sender, EventArgs e)
        {
            try
            {
                var listFacultys = facultyService.GetAll();
                Faculty selectedFaculty = cmbKhoa.SelectedItem as Faculty;
                if (selectedFaculty != null)
                {
                    var listMajor = majorService.GetAllByFaculty(selectedFaculty.FacultyID);
                }
                FillFalcultyCombobox(listFacultys);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FillMajorCombobox(List<Major> listMajor)
        {
            this.cmbChuyenNganh.DataSource = listMajor;
            this.cmbChuyenNganh.DisplayMember = "Name";
            this.cmbChuyenNganh.ValueMember = "MajorID";
        }

        private void FillFalcultyCombobox(List<Faculty> listFacultys)
        {
            this.cmbKhoa.DataSource = listFacultys;
            this.cmbKhoa.DisplayMember = "FacultyName";
            this.cmbKhoa.ValueMember = "FacultyID";
        }

        private void cmbChuyenNganh_SelectedIndexChanged(object sender, EventArgs e)
        {
            Faculty selectedFaculty = cmbKhoa.SelectedItem as Faculty;
            if(selectedFaculty != null)
            {
                var listMajor = majorService.GetAllByFaculty(selectedFaculty.FacultyID);
                FillMajorCombobox(listMajor);
                var listStudents = studentService.GetAllHasNoMajor(selectedFaculty.FacultyID);
                BindGrid(listStudents);
            }
        }

        

        private void BindGrid(List<Student> listStudents)
        {

            dgvDKChuyenNganh.Rows.Clear();
            foreach (var item in listStudents)
            {
                int index = dgvDKChuyenNganh.Rows.Add();
                dgvDKChuyenNganh.Rows[index].Cells[1].Value = item.StudentID;
                dgvDKChuyenNganh.Rows[index].Cells[2].Value = item.FullName;

                if (item.Faculty != null)
                    dgvDKChuyenNganh.Rows[index].Cells[3].Value = item.Faculty.FacultyName;

                dgvDKChuyenNganh.Rows[index].Cells[4].Value = item.AverageScore + "";

                if (item.MajorID != null)
                    dgvDKChuyenNganh.Rows[index].Cells[5].Value = item.Major.Name + "";

            }
        }
    }
}
