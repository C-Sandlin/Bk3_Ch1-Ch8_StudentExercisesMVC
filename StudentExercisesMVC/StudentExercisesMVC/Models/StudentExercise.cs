﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercisesMVC.Models.ViewModels
{
    public class StudentExercise
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int InstructorId { get; set; }
        public int ExerciseId { get; set; }
    }
}
