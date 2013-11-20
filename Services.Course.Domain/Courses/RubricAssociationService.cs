using System;
using BpeProducts.Services.Course.Contract;

namespace BpeProducts.Services.Course.Domain.Courses
{
	public interface IRubricAssociationService
	{
		RubricAssociationInfo Get(Guid courseId, Guid segmentId, Guid learningActivityId, Guid rubricassociationId);
		RubricAssociationInfo AddRubric(Guid courseId, Guid segmentId, Guid learningActivityId, RubricAssociationRequest request);
	}

	public class RubricAssociationService
	{

	}
}
