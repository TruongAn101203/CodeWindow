using Lab05.BUS;
using Lab05.DAL.Entities;
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

namespace Lab05.GUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private readonly StudentService studentService = new StudentService();
        private readonly FacultyService facultyService = new FacultyService();
        private string selectedFilePath;

        StudentContextDB db = new StudentContextDB();
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {

                var listFacultys = facultyService.GetAll();
                var listStudents = studentService.GetAll();
                FillFacultyCombobox(listFacultys);
                BindGrid(listStudents);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void FillFacultyCombobox(List<Faculty> listFacultys)
        {

            this.cmbKhoa.DataSource = listFacultys;
            this.cmbKhoa.DisplayMember = "FacultyName";
            this.cmbKhoa.ValueMember = "FacultyID";

        }
        private void BindGrid(List<Student> liststudent)
        {
            dgvStudent.Rows.Clear();
            foreach (var item in liststudent)
            {
                int index = dgvStudent.Rows.Add();
                dgvStudent.Rows[index].Cells[0].Value = item.StudentID;
                dgvStudent.Rows[index].Cells[1].Value = item.FullName;
                if (item.Faculty != null)
                    dgvStudent.Rows[index].Cells[2].Value = item.Faculty.FacultyName;
                dgvStudent.Rows[index].Cells[3].Value = item.AverageScore + "";
                if (item.MajorID != null)
                    dgvStudent.Rows[index].Cells[4].Value = item.Major.MajorName + "";
                ShowAvata(item.Avata);

            }
        }


        public void setGridViewStyle(DataGridView dgvview)
        {
            dgvStudent.BorderStyle = BorderStyle.None;
            dgvStudent.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise;
            dgvStudent.BackgroundColor = Color.White;
            dgvStudent.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }



        private void refresh()
        {
            List<Student> dssv = db.Students.ToList();
            BindGrid(dssv);
            dgvStudent.Update();
            dgvStudent.Refresh();
        }



        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            var listStudents = new List<Student>();
            if (this.checkBox1.Checked)
                listStudents = studentService.GetAllHasNoMajor();
            else
                listStudents = studentService.GetAll();
            BindGrid(listStudents);
        }
        Student GetStudent()
        {
            Student s = new Student();
            s.StudentID = txtMSSV.Text.Trim();
            s.FullName = txtHoTen.Text.Trim();
            if (cmbKhoa.Text != "")
                s.FacultyID = int.Parse(cmbKhoa.SelectedValue.ToString());
            if (txtDTB.Text != "")
                s.AverageScore = double.Parse(txtDTB.Text);
            return s;
        }

        //THÊM SỬA
        private void btnAddOrUpdate_Click(object sender, EventArgs e)
        {

             try
              {
                  if (txtMSSV.Text == "" || txtHoTen.Text == "" || cmbKhoa.Text == "" || txtDTB.Text == "")
                      throw new Exception("Vui lòng nhập đầy đủ các thông tin!");
                  StudentService studentService = new StudentService();
                  studentService.InsertUpdate(GetStudent());                         
                  MessageBox.Show("Insert update thành công!");
                  // = $"{selectedFilePath}";
                  refresh();
                  Form1_Load(sender, e);
              }
              catch (Exception ex)
              {
                  MessageBox.Show(ex.Message);
              }
           
        
    
        }
        // XÓA
        private void btnDelete_Click(object sender, EventArgs e)
        {
            var rowData = dgvStudent.SelectedRows[0].Cells["Column1"].Value.ToString();
            Student st = db.Students.Find(rowData);
            DialogResult result = MessageBox.Show($"ban co dong y xoa sinh vien{st.FullName}", "thong bao", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                db.Students.Remove(st);
                MessageBox.Show($"xoa sinh vien {st.FullName}thanh cong", "thong bao ", MessageBoxButtons.OK);

                db.SaveChanges();
                refresh();
            }
        }
        //ẢNH
        private void ShowAvata(string Avata)
        {
            if (string.IsNullOrEmpty(Avata))
            {
                pictureBox1.Image = null;
            }
            else
            {
                string parentDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName;
                string imagPath = Path.Combine(parentDirectory, "Images", Avata);
                pictureBox1.Image = Image.FromFile(imagPath);
                pictureBox1.Refresh();
            }
        }
        private void btnImage_Click(object sender, EventArgs e)
        {
            selectedFilePath = LoadImage();

            if (selectedFilePath != null)
            {
                ShowAvata(selectedFilePath);
            }
        }
        private string LoadImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Set initial directory and filter for image files
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp|All Files|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
               selectedFilePath = openFileDialog.FileName;
                return selectedFilePath;
            }

            // If the user canceled, return null
            return null;
        }



     
        

        private void dangKiChuyenNganhToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRegister frmRegister = new frmRegister();
            frmRegister.Show();
            Refresh();
        }

        private void thoatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvStudent_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvStudent.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvStudent.SelectedRows[0];
                txtMSSV.Text = row.Cells["Column1"].Value.ToString();
                txtHoTen.Text = row.Cells["Column2"].Value.ToString();
                cmbKhoa.SelectedItem = row.Cells["Column3"].Value.ToString();
                txtDTB.Text = row.Cells["Column4"].Value.ToString();

            }
        }
    }
}
