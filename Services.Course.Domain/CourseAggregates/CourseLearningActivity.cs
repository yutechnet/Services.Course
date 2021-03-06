﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BpeProducts.Common.NHibernate;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Validation;
using BpeProducts.Services.Assessment.Contract;

namespace BpeProducts.Services.Course.Domain.CourseAggregates
{
    public class CourseLearningActivity : TenantEntity,IValidatable<CourseLearningActivity>
    {
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
				_type = value;
			}
	    }

	    public virtual Boolean IsGradeable
	    {
			get { return _isGradeable; }
			set
			{
				_isGradeable = value;
			}
	    }

        public virtual Boolean IsExtraCredit { get; set; }

        public virtual decimal MaxPoint { get; set; }

        public virtual int Weight { get; set; }

        public virtual Guid? ObjectId { get; set; }

		public virtual string Description { get; set; }

        public virtual string MetaData { get; set; }

        public virtual int? ActiveDate { get; set; }

        public virtual int? InactiveDate { get; set; }

        public virtual int? DueDate { get; set; }
      
        public virtual Guid? AssessmentId { get; set; }
        public virtual AssessmentType AssessmentType { get; set; }
        public virtual Guid SourceCourseLearningActivityId { get; set; }

        public virtual bool Validate(IValidator<CourseLearningActivity> validator, out IEnumerable<string> brokenRules)
        {
            brokenRules = validator.BrokenRules(this);
            return validator.IsValid(this);
        }

        public virtual void CloneOutcomes(IAssessmentClient assessmentClient)
        {
            assessmentClient.CloneEntityOutcomes(SupportingEntityType.LearningActivity, SourceCourseLearningActivityId, new CloneEntityOutcomeRequest()
            {
                EntityId = Id,
                Type = SupportingEntityType.LearningActivity
            });
        }
    }
}
