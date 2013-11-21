using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BpeProducts.Common.Exceptions;
using BpeProducts.Common.NHibernate;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Entities;

namespace BpeProducts.Services.Course.Domain.Courses
{
    public class CourseLearningActivity : TenantEntity
    {
        private IList<LearningMaterial> _learningMaterials = new List<LearningMaterial>();
		private IList<RubricAssociation> _rubricAssociations = new List<RubricAssociation>();

        [Required]
        public virtual string Name { get; set; }

        [Required]
        public virtual CourseLearningActivityType Type { get; set; }

        public virtual Boolean IsGradeable { get; set; }

        public virtual Boolean IsExtraCredit { get; set; }

        public virtual int MaxPoint { get; set; }

        public virtual int Weight { get; set; }

        public virtual Guid ObjectId { get; set; }

        public virtual string CustomAttribute { get; set; }

        public virtual int? ActiveDate { get; set; }

        public virtual int? InactiveDate { get; set; }

        public virtual int? DueDate { get; set; }

        public virtual IList<LearningMaterial> LearningMaterials
        {
            get { return _learningMaterials; }
            set { _learningMaterials = value; }
        }

		public virtual IList<RubricAssociation> RubricAssociations
		{
			get { return _rubricAssociations; }
			set { _rubricAssociations = value; }
		}

        public virtual LearningMaterial AddLearningMaterial(LearningMaterialRequest request)
        {
            var learningMaterial = new LearningMaterial {Id = Guid.NewGuid(), TenantId = TenantId, Name = request.Name};

            _learningMaterials.Add(learningMaterial);
            return learningMaterial;
        }

		public virtual RubricAssociation AddRubricAssociation(RubricAssociationRequest request)
		{
			var rubricAssociation = new RubricAssociation { Id = Guid.NewGuid(), RubricId = request.RubricId, TenantId = TenantId };

			if (Type != CourseLearningActivityType.Custom)
			{
				throw new BadRequestException("Rubrics may only be associated with LearningActivities of type CUSTOM. To associate rubrics to non-custom types supported by the platform, please consult the documentation.");
			}

			//TODO: Add validation of rubric here (necessitates GET on assessmentSvc)

			_rubricAssociations.Add(rubricAssociation);
			return rubricAssociation;
		}
    }
}
