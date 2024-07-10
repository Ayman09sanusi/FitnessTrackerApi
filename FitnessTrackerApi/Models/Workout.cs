using System;
using System.Collections.Generic;

namespace FitnessTrackerAPI.Models
{
    public class Workout
    {
        public int WorkoutId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public List<Exercise> Exercises { get; set; }
    }
}
