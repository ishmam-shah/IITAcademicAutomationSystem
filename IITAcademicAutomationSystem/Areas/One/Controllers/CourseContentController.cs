﻿using IITAcademicAutomationSystem.Areas.One.Models;
using IITAcademicAutomationSystem.Areas.One.Services;
using IITAcademicAutomationSystem.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IITAcademicAutomationSystem.Areas.One.Controllers
{
    public class CourseContentController : Controller
    {
        private ICourseContentService courseContentService;
        private ICourseService courseService;


        public CourseContentController()
        {
            courseContentService = new CourseContentService(ModelState, new DAL.UnitOfWork());
            courseService = new CourseService(ModelState, new DAL.UnitOfWork());

        }
        public CourseContentController(ICourseContentService courseContentService)
        {
            this.courseContentService = courseContentService;
            courseService = new CourseService(ModelState, new DAL.UnitOfWork());

        }

        // GET: One/CourseContent
        public ActionResult IndexStudent(int courseId)
        {
            return View();
        }

        public ActionResult IndexTeacher(int courseId)
        {
            var teacherId = User.Identity.GetUserId();
            var contents = courseContentService.GetCourseContents(courseId, teacherId);
            var course = courseService.ViewCourse(courseId);

            List<CourseContentViewModel> model = new List<CourseContentViewModel>();
            CourseContentViewModel vm = new CourseContentViewModel();

            foreach (var item in contents)
            {
                vm.Id = item.Id;
                vm.CourseId = item.CourseId;
                vm.TeacherId = teacherId;
                vm.ContentTitle = item.ContentTitle;
                vm.ContentDescription = item.ContentDescription;
                vm.UploadDate = item.UploadDate;
                vm.FilePath = item.FilePath;

                model.Add(vm);
                vm = new CourseContentViewModel();
            }

            ViewBag.CourseCode = course.CourseCode;
            ViewBag.CourseTitle = course.CourseTitle;
            return View(model);
        }

        public FileResult Download(string fileName, string contentTitle)
        {
            return File("~/Areas/One/Content/CourseContent/" + fileName,
                System.Net.Mime.MediaTypeNames.Application.Octet, contentTitle);
        }

        public ActionResult Upload(int courseId)
        {
            Course course = courseService.ViewCourse(courseId);
            CourseContentViewModel model = new CourseContentViewModel();
            model.CourseId = courseId;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(CourseContentViewModel model, HttpPostedFileBase file)
        {
            string fileName = "";

            if (file != null && file.ContentLength > 0)
            {
                //string ext = System.IO.Path.GetExtension(this.File.PostedFile.FileName);
                //fileName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".png";

                fileName = Path.GetFileName(file.FileName);
                file.SaveAs(HttpContext.Server.MapPath("~/Areas/One/Content/CourseContent/")
                    + fileName);

                CourseContent content = new CourseContent();
                content.CourseId = model.CourseId;
                content.TeacherId = User.Identity.GetUserId();
                content.ContentTitle = fileName;
                content.UploadDate = DateTime.Now;
                content.FilePath = fileName;

                if (courseContentService.Create(content))
                {
                    return RedirectToAction("IndexTeacher", "CourseContent", new { area = "One", courseId = model.CourseId });
                }

                return View(model);
            }

            ModelState.AddModelError("ContentTitle", "There is a problem, try again.");

            return View(model);
        }
    }
}