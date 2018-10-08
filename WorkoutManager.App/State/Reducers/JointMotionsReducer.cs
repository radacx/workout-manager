using System.Collections.Generic;
using System.Linq;
using Redux;
using WorkoutManager.App.State.Actions;
using WorkoutManager.Contract.Models.Exercises;

namespace WorkoutManager.App.State.Reducers
{
    internal class JointMotionsReducer
    {
        public static IEnumerable<JointMotion> Execute(IEnumerable<JointMotion> state, IAction action)
        {
            if (action is AddJointMotion addJointMotion)
            {
                return state.Concat(new[] { addJointMotion.Motion });
            }

            return state;
        }
    }
}