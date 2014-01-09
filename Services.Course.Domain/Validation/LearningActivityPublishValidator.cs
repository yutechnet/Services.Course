using System;
using System.Collections.Generic;
using System.Linq;
using BpeProducts.Services.Course.Domain.Courses;
using Services.Assessment.Contract;

namespace BpeProducts.Services.Course.Domain.Validation
{
    public class LearningActivityPublishValidator:IValidator<CourseLearningActivity>
    {
        public bool IsValid(CourseLearningActivity learningActivity)
        {
            return !BrokenRules(learningActivity).Any();
        }

        public IEnumerable<string> BrokenRules(CourseLearningActivity learningActivity)
        {
            // All learning activities, except those of custom type, must have assessment id.
            if (learningActivity.AssessmentType != AssessmentType.Custom && learningActivity.AssessmentId == Guid.Empty)
            {
                yield return string.Format(
                    "LeanringActivity {0} is not of custom type but does not have an assessment assigned.",
                    learningActivity.Id);
            }
        }
    }
}