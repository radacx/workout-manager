using Redux;
using WorkoutManager.Contract.Models.Exercises;

namespace WorkoutManager.App.State.Actions
{
    internal class AddJointMotion : IAction
    {
        public JointMotion Motion { get; }

        public AddJointMotion(JointMotion motion)
        {
            Motion = motion;
        }
    }
}