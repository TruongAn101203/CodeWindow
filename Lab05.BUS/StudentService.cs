using Lab05.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab05.BUS
{
    public class StudentService
    {
        public List<Student> GetAll()
        {
            StudentContextDB context = new StudentContextDB();
            return context.Students.ToList();
        }
        public List<Student> GetAllHasNoMajor()
        {
            StudentContextDB context = new StudentContextDB();
            return context.Students.Where(p => p.MajorID == null).ToList();
        }
        public List<Student> GetAllHasNoMajor(int faculyID)
        {
            StudentContextDB context = new StudentContextDB();
            return context.Students.Where(p => p.MajorID == null && p.FacultyID == faculyID).ToList();
        }
        public Student FindByID(string studentID)
        {
            StudentContextDB context = new StudentContextDB();
            return context.Students.FirstOrDefault(p => p.StudentID == studentID);
        }
        public void InsertUpdate(Student s)
        {
            StudentContextDB context = new StudentContextDB();
            context.Students.AddOrUpdate(s);
            context.SaveChanges();
        }
    }
}
