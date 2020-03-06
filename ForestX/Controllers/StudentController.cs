using DAL;
using DAL.Repo;
using LoginModule.Logic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ForestX.Controllers
{
    public class StudentController : Controller
    {

        StudentRepo studentRepo = new StudentRepo();

        // GET: Student
        public ActionResult List()
        {
            LoginLogic login = new LoginLogic();

            if (Request.Cookies["UserName"] != null)
            {
                ViewBag.UserName = Request.Cookies["UserName"].Value;
            }
            if (login.CheckLogin(Convert.ToBoolean(Session["LoginStatus"]), Request.Cookies["LoginCookie"]))
            {
                List<StudentDetails> studentList = studentRepo.GetList();
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        public JsonResult GetStudentById(Guid StudentID)
        {
            StudentDetails student = studentRepo.GetStudentById(StudentID);

            string value = string.Empty;
            value = JsonConvert.SerializeObject(student, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetList()
        {
            List<StudentDetails> studentList = studentRepo.GetList();
            foreach (var item in studentList)
            {
                if (item.StudentEmail != null)
                {
                    item.StudentEmail = item.StudentEmail.Replace("\r\n", "<br/>");
                    item.StudentEmail = item.StudentEmail.Replace(",", "<br/>");
                }
            }

            return Json(studentList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveDataInDatabase(Student model)
        {
            string user = Request.Cookies["FSUserName"].Value;
            var result = false;

            if (model.StudentID == Guid.Empty)
            {
                studentRepo.Save(model);
                result = true;
            }
            else
            {
                Student studentDetails = studentRepo.GetStudent(model.StudentID);

                if (model.StudentName == null)
                    model.StudentName = studentDetails.StudentName;
                if (model.StudentEmail == null)
                    model.StudentEmail = studentDetails.StudentEmail;
                if (model.Course == null)
                    model.Course = studentDetails.Course;
                if (model.Department == null)
                    model.Department = studentDetails.Department;

                studentRepo.Update(model.StudentID, model);
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteStudentRecord(Guid StudentID)
        {
            string user = Request.Cookies["FSUserName"].Value;
            bool result = false;
            Student studentDetails = studentRepo.GetStudent(StudentID);
            if (studentDetails != null)
            {
                studentRepo.Delete(StudentID);
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}