using System;
using System.Collections.Generic;
using System.Linq;
using WorkoutManager.Contract.Models.Exercises;
using WorkoutManager.Contract.Models.ExerciseSet;
using WorkoutManager.Contract.Models.Sessions;

namespace WorkoutManager.Service.Services
{
    public class ProgressResult
    {
        public DateTime Date { get; set; }
        
        public double Volume { get; set; }
    }
    
    public class ProgressFilter
    {    
        public Exercise Exercise { get; set; }
        
        public DateTime? DateFrom { get; set; }
        
        public DateTime? DateTo { get; set; }
    }
    
    public class ProgressService
    {
        public IEnumerable<ProgressResult> GetTotalVolume(IEnumerable<TrainingSession> sessions, ProgressFilter filter)
        {
            var filteredSessions = sessions;
            
            if (filter.DateFrom != null)
            {
                filteredSessions = filteredSessions.Where(session => session.Date >= filter.DateFrom);
            }
            
            if (filter.DateTo != null)
            {
                filteredSessions = filteredSessions.Where(session => session.Date <= filter.DateTo);
            }

            return GetTotalVolume(filteredSessions, filter.Exercise).Where(result => result.Volume > 0);
        }

        private static IEnumerable<ProgressResult> GetTotalVolume(
            IEnumerable<TrainingSession> sessions,
            Exercise exercise
        ) => sessions.Select(
            session => new ProgressResult()
            {
                Date = session.Date,
                Volume = GetTrainingSessionVolume(session, exercise)
            }
        );

        private static double GetExerciseVolume(SessionExercise exercise, double bodyweightVolume) => exercise.Sets.OfType<DynamicExerciseSet>()
            .Aggregate(
                0d,
                (exerciseVolume, set) => exerciseVolume + set.Reps * (set.Weight + bodyweightVolume)
            );

        private static double GetTrainingSessionVolume(TrainingSession session, Exercise givenExercise)
        {
            var bodyweight = session.Bodyweight;

            return session.Exercises.Aggregate(
                0d,
                (sessionVolume, exercise) =>
                {
                    if (!exercise.Exercise.Equals(givenExercise))
                    {
                        return sessionVolume;
                    }
                    
                    var bodyweightVolume = exercise.Exercise.RelativeBodyweight / 100 * bodyweight;

                    var exerciseVolume = GetExerciseVolume(exercise, bodyweightVolume);

                    return sessionVolume + exerciseVolume;
                }
            );
        }
    }
}