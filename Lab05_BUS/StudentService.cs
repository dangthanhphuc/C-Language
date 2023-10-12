using Lab05_DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab05_BUS
{
    public class StudentService
    {
        StudentModel context;

        public StudentService()
        {
            context = new StudentModel();
        }

        public List<Student> GetAll()
        {
            return context.Students.ToList();
        }

        public List<Student> GetAllHasNoMajor() { 
            return context.Students.Where(p => p.MajorID == null).ToList();
        }

        public List<Student> GetAllHasNoMajor(int facultyID)
        {
            return context.Students.Where(p => p.MajorID == null && p.FacultyID == facultyID).ToList();
        }

        public Student FindById(string studentId)
        {
            return context.Students.Find(studentId);
        }

        public void InsertUpdate (Student student)
        {
            context.Students.AddOrUpdate(student);
            context.SaveChanges();
        }

        public void DeleteById(string studentId)
        {
            Student st = FindById(studentId);
            context.Students.Remove(st);
            context.SaveChanges();
        }
    }
}
