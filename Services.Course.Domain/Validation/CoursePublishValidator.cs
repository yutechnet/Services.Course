using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Services.Course.Domain.Courses;

namespace BpeProducts.Services.Course.Domain.Validation
{
    public class CoursePublishValidator : IValidator<Courses.Course>
    {
        private readonly IValidator<CourseLearningActivity> _learningActivityValidator;
        private IEnumerable<string> _brokenRules;

        public CoursePublishValidator(IValidator<CourseLearningActivity> learningActivityValidator )
        {
            _learningActivityValidator = learningActivityValidator;
            _brokenRules = new List<string>();
        }

        public bool IsValid(Courses.Course course)
        {
            return !BrokenRules(course).Any();
        }

        public IEnumerable<string> BrokenRules(Courses.Course course)
        {
            foreach (var segment in course.Segments)
            {
                CheckForValidLearningActivity(segment);
            }
            return _brokenRules;
        }

        private void CheckForValidLearningActivity(CourseSegment segment )
        {
            foreach (var learningActivity in segment.CourseLearningActivities)
            {
                IEnumerable<string> brokenRules;
                learningActivity.Validate(_learningActivityValidator, out brokenRules);

                _brokenRules = _brokenRules.Union(brokenRules);
            }

            foreach (var childSegment in segment.ChildSegments)
            {
                CheckForValidLearningActivity(childSegment);
            }
          
        }
    }
}
