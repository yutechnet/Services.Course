using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using BpeProducts.Common.Exceptions;
using BpeProducts.Common.NHibernate;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Entities;
using BpeProducts.Services.Course.Domain.Validation;
using Services.Assessment.Contract;

namespace BpeProducts.Services.Course.Domain.Courses
{
    public class CourseLearningActivity : TenantEntity,IValidatable<CourseLearningActivity>
    {
        private IList<CourseRubric> _courseRubrics = new List<CourseRubric>();

	    private CourseLearningActivityType _type;
	    private bool _isGradeable;

        [Required]
        public virtual string Name { get; set; }

	    [Required]
	    public virtual CourseLearningActivityType Type
	    {
			get { return _type; }
			set 
			{
				// Only LearningActivities of type 'Custom' may be associated to rubrics
				if ((CourseRubrics.Count > 0) && (value != CourseLearningActivityType.Custom))
				{
					throw new BadRequestException("This learning activity is associated with rubric(s). Since rubrics may only be associated with learning activities of type 'custom', this learning activity's type may not be changed.");
				}
				_type = value;
			}
	    }

	    public virtual Boolean IsGradeable
	    {
			get { return _isGradeable; }
			set
			{
				// Only gradable LearningActivities may be associated to rubrics
				if (CourseRubrics.Count > 0 && value == false)
				{
					throw new BadRequestException("This learning activity is associated with rubric(s). Since rubrics may only be associated with gradable learning activities, this learning activity's gradability may not be changed.");
				}
				_isGradeable = value;
			}
	    }

        public virtual Boolean IsExtraCredit { get; set; }

        public virtual int MaxPoint { get; set; }

        public virtual int Weight { get; set; }

        public virtual Guid ObjectId { get; set; }

        public virtual string CustomAttribute { get; set; }

        public virtual int? ActiveDate { get; set; }

        public virtual int? InactiveDate { get; set; }

        public virtual int? DueDate { get; set; }

        public virtual IList<CourseRubric> CourseRubrics
        {
            get { return _courseRubrics; }
            set { _courseRubrics = value; }
        }
      
        public virtual Guid AssessmentId { get; set; }
        public virtual AssessmentType AssessmentType { get; set; }

        public virtual CourseRubric AddCourseRubric(CourseRubricRequest request)
        {
            var courseRubric = new CourseRubric { Id = Guid.NewGuid(), RubricId = request.RubricId, TenantId = TenantId };

            if (Type != CourseLearningActivityType.Custom)
            {
                throw new BadRequestException("Rubrics may only be associated with LearningActivities of type CUSTOM. To associate rubrics to non-custom types supported by the platform, please consult the documentation.");
            }

			if (!IsGradeable)
			{
				throw new BadRequestException("Rubrics may only be associated with LearningActivities that are gradable.");
			}

            var rubricAlreadyLinkedCheck = _courseRubrics.SingleOrDefault(r => r.RubricId == request.RubricId);
            if (rubricAlreadyLinkedCheck != null)
            {
                throw new BadRequestException(string.Format("RubricId {0} is already associated with learningActivity {1} and thus cannot be added again.", request.RubricId, Id));
            }

            //TODO: Add validation of rubric here (necessitates GET on assessmentSvc)

            _courseRubrics.Add(courseRubric);
            return courseRubric;
        }

        public virtual void DeleteCourseRubric(Guid rubricId)
        {
            var courseRubric = _courseRubrics.SingleOrDefault(r => r.RubricId == rubricId);

            if (courseRubric == null)
            {
                throw new NotFoundException(string.Format("RubricId {0} is not associated with learningActivity {1} and thus cannot be deleted.", rubricId, Id));
            }
			courseRubric.ActiveFlag = false;
        }

        }

        public virtual bool Validate(IValidator<CourseLearningActivity> validator, out IEnumerable<string> brokenRules)
        {
            brokenRules = validator.BrokenRules(this);
            return validator.IsValid(this);
    }
}
