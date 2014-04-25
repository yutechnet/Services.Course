using BpeProducts.Services.Course.Contract;

namespace BpeProducts.Services.Course.Domain.Courses
{
	public interface ICourseFactory
	{
		Courses.Course Build(SaveCourseRequest request);
	    Courses.Course Build(CreateCourseFromTemplateRequest request);
    }
}