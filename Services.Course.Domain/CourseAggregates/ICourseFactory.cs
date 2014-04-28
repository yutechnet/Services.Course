using BpeProducts.Services.Course.Contract;

namespace BpeProducts.Services.Course.Domain.CourseAggregates
{
	public interface ICourseFactory
	{
		Course Build(SaveCourseRequest request);
	    Course Build(CreateCourseFromTemplateRequest request);
    }
}