# FitnessTrackerApi
Ayman Sanusi BU/22C/IT/7415

FitnessTrackerAPI is a simple ASP.NET Core Web API for managing workouts and exercises. This API supports basic CRUD operations and uses Entity Framework Core with an in-memory database for data storage.

 Getting Started

Prerequisites

- Visual Studio 2022
- .NET 6.0 SDK or later

Setting Up the Project

1. Create a New ASP.NET Core Web API Project**
    - Open Visual Studio 2022.
    - Create a new project: **ASP.NET Core Web API**.
    - Name it `FitnessTrackerAPI`.

2. Add Entity Framework Core In-Memory Database**
    - In the `FitnessTrackerAPI` project, add the necessary NuGet packages:
      
      dotnet add package Microsoft.EntityFrameworkCore.InMemory
      dotnet add package Microsoft.EntityFrameworkCore.Design
     

3. Define Your Models
    - Create a folder named `Models`.
    - Create `Workout.cs`:
      
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
     
    
    - Create `Exercise.cs`:   
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
     

4. Set Up the Data Context
    - Create a folder named `Data`.
    
    - Create `FitnessTrackerContext.cs`:
      using Microsoft.EntityFrameworkCore;
      using FitnessTrackerAPI.Models;

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
      

5. Configure the Database in `Program.cs`
    - In the `ConfigureServices` method:
      
      public void ConfigureServices(IServiceCollection services)
      {
          services.AddDbContext<FitnessTrackerContext>(options =>
              options.UseInMemoryDatabase("FitnessTracker"));
          services.AddControllers();
      }
     

 Implement CRUD Operations

1. Create Controllers
    - Create a folder named `Controllers`.

    - Create `WorkoutsController.cs`:
     
      using Microsoft.AspNetCore.Mvc;
      using Microsoft.EntityFrameworkCore;
      using FitnessTrackerAPI.Data;
      using FitnessTrackerAPI.Models;
      using System.Collections.Generic;
      using System.Linq;
      using System.Threading.Tasks;

      namespace FitnessTrackerAPI.Controllers
      {
          [Route("api/[controller]")]
          [ApiController]
          public class WorkoutsController : ControllerBase
          {
              private readonly FitnessTrackerContext _context;

              public WorkoutsController(FitnessTrackerContext context)
              {
                  _context = context;
              }

              [HttpGet]
              public async Task<ActionResult<IEnumerable<Workout>>> GetWorkouts()
              {
                  return await _context.Workouts.Include(w => w.Exercises).ToListAsync();
              }

              [HttpGet("{id}")]
              public async Task<ActionResult<Workout>> GetWorkout(int id)
              {
                  var workout = await _context.Workouts.Include(w => w.Exercises).FirstOrDefaultAsync(w => w.WorkoutId == id);

                  if (workout == null)
                  {
                      return NotFound();
                  }

                  return workout;
              }

              [HttpPost]
              public async Task<ActionResult<Workout>> PostWorkout(Workout workout)
              {
                  _context.Workouts.Add(workout);
                  await _context.SaveChangesAsync();

                  return CreatedAtAction(nameof(GetWorkout), new { id = workout.WorkoutId }, workout);
              }

              [HttpPut("{id}")]
              public async Task<IActionResult> PutWorkout(int id, Workout workout)
              {
                  if (id != workout.WorkoutId)
                  {
                      return BadRequest();
                  }

                  _context.Entry(workout).State = EntityState.Modified;

                  try
                  {
                      await _context.SaveChangesAsync();
                  }
                  catch (DbUpdateConcurrencyException)
                  {
                      if (!WorkoutExists(id))
                      {
                          return NotFound();
                      }
                      else
                      {
                          throw;
                      }
                  }

                  return NoContent();
              }

              [HttpDelete("{id}")]
              public async Task<IActionResult> DeleteWorkout(int id)
              {
                  var workout = await _context.Workouts.FindAsync(id);
                  if (workout == null)
                  {
                      return NotFound();
                  }

                  _context.Workouts.Remove(workout);
                  await _context.SaveChangesAsync();

                  return NoContent();
              }

              private bool WorkoutExists(int id)
              {
                  return _context.Workouts.Any(e => e.WorkoutId == id);
              }
          }
      }

    - Create `ExercisesController.cs`:
      using Microsoft.AspNetCore.Mvc;
      using Microsoft.EntityFrameworkCore;
      using FitnessTrackerAPI.Data;
      using FitnessTrackerAPI.Models;
      using System.Collections.Generic;
      using System.Linq;
      using System.Threading.Tasks;

      namespace FitnessTrackerAPI.Controllers
      {
          [Route("api/[controller]")]
          [ApiController]
          public class ExercisesController : ControllerBase
          {
              private readonly FitnessTrackerContext _context;

              public ExercisesController(FitnessTrackerContext context)
              {
                  _context = context;
              }

              [HttpGet]
              public async Task<ActionResult<IEnumerable<Exercise>>> GetExercises()
              {
                  return await _context.Exercises.ToListAsync();
              }

              [HttpGet("{id}")]
              public async Task<ActionResult<Exercise>> GetExercise(int id)
              {
                  var exercise = await _context.Exercises.FindAsync(id);

                  if (exercise == null)
                  {
                      return NotFound();
                  }

                  return exercise;
              }

              [HttpPost]
              public async Task<ActionResult<Exercise>> PostExercise(Exercise exercise)
              {
                  _context.Exercises.Add(exercise);
                  await _context.SaveChangesAsync();

                  return CreatedAtAction(nameof(GetExercise), new { id = exercise.ExerciseId }, exercise);
              }

              [HttpPut("{id}")]
              public async Task<IActionResult> PutExercise(int id, Exercise exercise)
              {
                  if (id != exercise.ExerciseId)
                  {
                      return BadRequest();
                  }

                  _context.Entry(exercise).State = EntityState.Modified;

                  try
                  {
                      await _context.SaveChangesAsync();
                  }
                  catch (DbUpdateConcurrencyException)
                  {
                      if (!ExerciseExists(id))
                      {
                          return NotFound();
                      }
                      else
                      {
                          throw;
                      }
                  }

                  return NoContent();
              }

              [HttpDelete("{id}")]
              public async Task<IActionResult> DeleteExercise(int id)
              {
                  var exercise = await _context.Exercises.FindAsync(id);
                  if (exercise == null)
                  {
                      return NotFound();
                  }

                  _context.Exercises.Remove(exercise);
                  await _context.SaveChangesAsync();

                  return NoContent();
              }

              private bool ExerciseExists(int id)
              {
                  return _context.Exercises.Any(e => e.ExerciseId == id);
              }
          }
      }
     

 Running the API

1. Run the API
    - Press `F5` in Visual Studio or use the `dotnet run` command in the terminal.
    - The API will be available at `https://localhost:<port>`.

2. Test the API**
    - Use tools like Postman or the built-in Swagger UI to test the endpoints.

 Endpoints

- Workouts
  - `GET /api/workouts` - Retrieve all workouts
  - `GET /api/workouts/{id}` - Retrieve a specific workout by ID
  - `POST /api/workouts` - Create a new workout
  - `PUT /api/workouts/{id}` - Update a workout
  - `DELETE /api/workouts/{id}` - Delete a workout

- Exercises
  - `GET /api/exercises` - Retrieve all exercises
  - `GET /api/exercises/{id}` - Retrieve a specific exercise by ID
  - `POST /api/exercises` - Create a new exercise
  - `PUT /api/exercises/{id}` - Update an exercise
  - `DELETE /api/exercises/{id}` - Delete an exercise
