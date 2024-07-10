using Microsoft.EntityFrameworkCore;
using FitnessTrackerAPI.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace FitnessTrackerAPI.Data
{
    public class FitnessTrackerContext : DbContext
    {
        public FitnessTrackerContext(DbContextOptions<FitnessTrackerContext> options) : base(options) { }

        public DbSet<Workout> Workouts { get; set; }
        public DbSet<Exercise> Exercises { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Workout>().HasData(
                new Workout { WorkoutId = 1, Name = "Morning Routine", Description = "Morning workout routine", Date = DateTime.Now },
                new Workout { WorkoutId = 2, Name = "Evening Routine", Description = "Evening workout routine", Date = DateTime.Now }
            );

            modelBuilder.Entity<Exercise>().HasData(
                new Exercise { ExerciseId = 1, Name = "Push Ups", Repetitions = 15, Sets = 3, WorkoutId = 1 },
                new Exercise { ExerciseId = 2, Name = "Squats", Repetitions = 20, Sets = 3, WorkoutId = 1 },
                new Exercise { ExerciseId = 3, Name = "Plank", Repetitions = 1, Sets = 3, WorkoutId = 2 }
            );
        }
    }
}
