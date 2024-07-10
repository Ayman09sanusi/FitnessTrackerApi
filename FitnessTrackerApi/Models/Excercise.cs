namespace FitnessTrackerAPI.Models
{
    public class Exercise
    {
        public int ExerciseId { get; set; }
        public string Name { get; set; }
        public int Repetitions { get; set; }
        public int Sets { get; set; }
        public int WorkoutId { get; set; }
        public Workout Workout { get; set; }
    }
}
