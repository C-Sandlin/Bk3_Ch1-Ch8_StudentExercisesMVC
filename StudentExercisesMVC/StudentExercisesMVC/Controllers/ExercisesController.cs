﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StudentExercisesMVC.Models;

namespace StudentExercisesMVC.Controllers
{
    public class ExercisesController : Controller
    {
        private readonly IConfiguration _config;

        public ExercisesController(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        // GET: Exercises
        public ActionResult Index()
        {
            var exercises = GetAllExercises();

            return View(exercises);
        }

        // GET: Exercises/Details/5
        public ActionResult Details(int id)
        {
            var exercise = GetOneExercise(id);
            return View(exercise);
        }

        // GET: Exercises/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Exercises/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Exercise exercise)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
                                          INSERT INTO Exercise
                                          ([Name], Language)
                                          VALUES (@name, @language)
                                          ";

                        cmd.Parameters.Add(new SqlParameter("@name", exercise.Name));
                        cmd.Parameters.Add(new SqlParameter("@language", exercise.Language));
                        cmd.ExecuteNonQuery();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Exercises/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Exercises/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Exercises/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Exercises/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private Exercise GetOneExercise(int id)
        {
            Exercise exercise = null;

            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                      SELECT Id, Name, Language
                                      FROM Exercise
                                      WHERE Id = @id
                                      ";

                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        exercise = new Exercise()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Language = reader.GetString(reader.GetOrdinal("Language"))
                        };
                    }
                    reader.Close();
                }
            }
            return exercise;
        }

        private List<Exercise> GetAllExercises()
        {
            var exercises = new List<Exercise>();

            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                      SELECT Id, Name, Language
                                      FROM Exercise
                                      ";

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        exercises.Add(new Exercise()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Language = reader.GetString(reader.GetOrdinal("Language"))
                        });
                    }
                    reader.Close();
                }
            }
            return exercises;
        }
    }
}