﻿using IITAcademicAutomationSystem.DAL;
using IITAcademicAutomationSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IITAcademicAutomationSystem.Areas.One.Services
{
    public interface IStudentService
    {
        Student ViewStudent(int id);
        Student GetStudentByUserId(string userId);
        IEnumerable<Student> GetAllStudents();
        int CreateStudent(Student student);
        bool EditStudent(Student student);
        void Dispose();
    }
    public class StudentService : IStudentService
    {
        private ModelStateDictionary modelState;
        public UnitOfWork unitOfWork;
        public StudentService(ModelStateDictionary modelState, UnitOfWork unitOfWork)
        {
            this.modelState = modelState;
            this.unitOfWork = unitOfWork;
        }

        // View student
        public Student ViewStudent(int id)
        {
            return unitOfWork.StudentRepository.GetStudentById(id);
        }

        public Student GetStudentByUserId(string userId)
        {
            return unitOfWork.StudentRepository.GetStudentByUserId(userId);
        }
        public int CreateStudent(Student student)
        {
            try
            {
                unitOfWork.StudentRepository.CreateStudent(student);
                unitOfWork.Save();
                return student.Id;
            }
            catch (DataException)
            {
                modelState.AddModelError("", "Unable to save, try again.");
                return -1;
            }
        }

        public bool EditStudent(Student student)
        {
            try
            {
                unitOfWork.StudentRepository.EditStudent(student);
                unitOfWork.Save();
                return true;
            }
            catch (Exception)
            {
                modelState.AddModelError("", "Unable to save student, please try again.");
                return false;
            }
        }

        public IEnumerable<Student> GetAllStudents()
        {
            return unitOfWork.StudentRepository.GetAllStudents();
        }
        public void Dispose()
        {
            unitOfWork.Dispose();
        }
    }
}