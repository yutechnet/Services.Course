using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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

	public virtual LearningMaterial AddLearningMaterial(Guid libraryItemId, string description)
        {
            var learningMaterial = new LearningMaterial {Id = Guid.NewGuid(), LibraryItemId = libraryItemId, TenantId = TenantId, Description = description};

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

			var associationCheck = _rubricAssociations.SingleOrDefault(r => r.RubricId == request.RubricId);
			if (associationCheck != null)
			{
				throw new BadRequestException(string.Format("RubricId {0} is already associated with learningActivity {1} and thus cannot be added again.", request.RubricId, Id));
			}

			//TODO: Add validation of rubric here (necessitates GET on assessmentSvc)

			_rubricAssociations.Add(rubricAssociation);
			return rubricAssociation;
		}

		public virtual void DeleteRubricAssociation(Guid rubricId)
		{
			var rubricAssociation = _rubricAssociations.SingleOrDefault(r => r.RubricId == rubricId);

			if (rubricAssociation == null)
			{
				throw new NotFoundException(string.Format("RubricId {0} is not associated with learningActivity {1} and thus cannot be deleted.", rubricId, Id));
			}

			_rubricAssociations.Remove(rubricAssociation);
		}
    }
}
