using Lab05.BUS;
using Lab05.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab05.GUI
{
    public partial class frmRegister : Form
    {
        private readonly StudentService studentService = new StudentService();
        private readonly FacultyService facultyService = new FacultyService();
        private readonly MajorService majorService = new MajorService();
        
        public frmRegister()
        {
            InitializeComponent();
        }

        private void frmRegister_Load(object sender, EventArgs e)
        {
            try
            {

                var listFacultys = facultyService.GetAll();
                FillFacultyCombobox(listFacultys);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        //Hàm binding list dữ liệu khoa vào combobox có tên hiện thị là tên khoa, giá trị là Mã khoa         
        private void FillFacultyCombobox(List<Faculty> listFacultys)
        {
            
            this.cmbKhoa.DataSource = listFacultys;
            this.cmbKhoa.DisplayMember = "FacultyName";
            this.cmbKhoa.ValueMember = "FacultyID";

        }
        private void cmbKhoa_SelectedIndexChanged(object sender, EventArgs e)
        {
            Faculty selectedFaculty = cmbKhoa.SelectedItem as Faculty;
            if (selectedFaculty != null)
            {
                //get combobox for Major

                var listMajor = majorService.GetAllByFaculty(selectedFaculty.FacultyID);
                FillMajorCombobox(listMajor);
                var listStudents = studentService.GetAllHasNoMajor(selectedFaculty.FacultyID);
                BindGrid(listStudents);
            }
        }
        private void FillMajorCombobox(List<Major> listMajor)
        {
            this.cmbMajor.DataSource = listMajor;
            this.cmbMajor.DisplayMember = "MajorName";
            this.cmbMajor.ValueMember = "MajorID";
        }

        private void BindGrid(List<Student> listStudent)
        {

          dgvRegister.Rows.Clear();
            foreach (var item in listStudent)
            {
                int index = dgvRegister.Rows.Add();
                dgvRegister.Rows[index].Cells[1].Value = item.StudentID;
                dgvRegister.Rows[index].Cells[2].Value = item.FullName;
                if (item.Faculty != null)
                    dgvRegister.Rows[index].Cells[3].Value = item.Faculty.FacultyName;
                dgvRegister.Rows[index].Cells[4].Value = item.AverageScore + "";
                if (item.MajorID != null)
                    dgvRegister.Rows[index].Cells[5].Value = item.Major.MajorName + "";
            }

        }

    
        

        private void dgvRegister_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvRegister.Rows)
            {
                DataGridViewCheckBoxCell checkBoxCell = row.Cells[0] as DataGridViewCheckBoxCell;
                if (checkBoxCell.Value != null)
                {
                    bool isChecked = Convert.ToBoolean(checkBoxCell.Value);
                    string studentID = dgvRegister.CurrentRow.Cells[1].Value.ToString();
                    string cn = cmbMajor.SelectedValue != null ? cmbMajor.SelectedValue.ToString() : null;
                    // Find the student in the list by StudentID
                    if (studentService.FindByID(studentID) != null)
                    {
                        if (isChecked)
                        {
                            // Update the MajorName property of the selected tblStudent's tblMajor object
                            Student st = studentService.FindByID(studentID);
                           
                            st.MajorID = Convert.ToInt32(cn);
                            studentService.InsertUpdate(st);
                        }
                    }
                }
            }
        
        }
    }
}

