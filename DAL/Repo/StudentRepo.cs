using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repo
{
    public class StudentRepo : BaseRepo
    {
        //List of student in student table
        public List<Student> GetAllList()
        {
            return (from c in Context.Students select c).ToList();
        }

        //List of student for display
        public List<StudentDetails> GetList()
        {
            List<StudentDetails> objStudent = new List<StudentDetails>();

            objStudent = (from s in Context.Students
                          where s.StudentEmail == null

                          select new
                          {
                              s.StudentID,
                              s.StudentName,
                              s.StudentEmail,
                              s.Course,
                              s.Department
                          }).ToList().Select(Rec => new
                            StudentDetails(Rec.StudentID, Rec.StudentName, Rec.StudentEmail, Rec.Course, Rec.Department)).ToList();

            return objStudent;
        }

        public StudentDetails GetStudentById(Guid id)
        {
            StudentDetails student = new StudentDetails();
            List<StudentDetails> objStudent = new List<StudentDetails>();
            objStudent = GetList();
            student = objStudent.Where(s => s.StudentID == id).FirstOrDefault();

            return student;
        }

        public Student GetStudent(Guid id)
        {
            Student student = new Student();
            List<Student> studentList = new List<Student>();
            studentList = GetAllList();
            student = studentList.Where(s => s.StudentID == id).FirstOrDefault();
            return student;
        }

        public void Save(Student student)
        {
            student.StudentID = Guid.NewGuid();
            Context.Students.Add(student);
            Context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            Student delete = (from c in Context.Students where c.StudentID == id select c).FirstOrDefault();
            delete.StudentEmail = null;
            Context.SaveChanges();
        }

        public void Update(Guid id, Student update)
        {
            Student student = (from c in Context.Students where c.StudentID == id select c).FirstOrDefault();

            student.StudentName = update.StudentName;

            student.StudentEmail = update.StudentEmail;

            student.Course = update.Course;

            student.Department = update.Department;

            Context.SaveChanges();
        }
    }
    public class StudentDetails
    {
        public StudentDetails(Guid _id, string _StudentName, string _StudentEmail, string _Course, string _Department)
        {
            StudentID = _id;
            StudentName = _StudentName;
            StudentEmail = _StudentEmail;
            Course = _Course;
            Department = _Department;
        }
        public StudentDetails()
        {

        }

        public Guid StudentID { get; set; }
        public string StudentName { get; set; }
        public string StudentEmail { get; set; }
        public string Course { get; set; }
        public string Department { get; set; }
    }

}
