using System;
using System.Collections.Generic;
using System.Text;

namespace SixKstep.Helpers
{
    
        public enum Goals
        {
            NonCompleted,
            HalfStepsCompleted,
            DalilyStepGoalCompleted
        };

        public static class GoalsChecker
        {
            public static Goals CheckForGoals(int steps)
            {
                if (steps > Settings.DailyStepGoal)
                    return Goals.DalilyStepGoalCompleted;

                else if (steps > Settings.DailyStepGoal / 2)
                    return Goals.HalfStepsCompleted;

                return Goals.NonCompleted;
            }
        }
   
}
