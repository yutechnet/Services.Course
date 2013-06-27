using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Query;
using System.Web.Http.Routing;
using AutoMapper;
using Autofac;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;
using BpeProducts.Services.Course.Host.App_Start;
using BpeProducts.Services.Course.Host.Controllers;
using Moq;
using NHibernate;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Unit
{
    [TestFixture]
    public class CoursesControllerTests
    {
        private Mock<IRepository> _mockCourseRepository;

        private CoursesController _coursesController;
	    private Mock<IDomainEvents> _mockDomainEvents;
	    private Mock<ICourseFactory> _mockCourseFactory;
        private Guid[] _organizationIds;

	    [SetUp]
        public void SetUp()
        {
            MapperConfig.ConfigureMappers();

            _mockCourseRepository = new Mock<IRepository>();
	        _mockDomainEvents = new Mock<IDomainEvents>();
		    _mockCourseFactory = new Mock<ICourseFactory>();
			_coursesController = new CoursesController(_mockCourseRepository.Object, _mockDomainEvents.Object,_mockCourseFactory.Object);
            _organizationIds = new[] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
        }

        [Test]
        public void can_create_a_new_course()
        {
            SetUpApiController();

            var organizationId = Guid.NewGuid();

             var saveCourseRequest = new SaveCourseRequest
                {
                    Id = Guid.NewGuid(),
                    Code = "PSY101",
                    Description = "Psych!",
                    Name = "Psychology 101", 
                    OrganizationId = organizationId,
                    TemplateCourseId = null,
                    CourseType = ECourseType.Traditional,
                    IsTemplate = false
                };

             var course = new Domain.Entities.Course
                 {
                     ActiveFlag = true,
                     Code = saveCourseRequest.Code,
                     Description = saveCourseRequest.Description,
                     Name = saveCourseRequest.Name,
                     Id = saveCourseRequest.Id,
                     OrganizationId = saveCourseRequest.OrganizationId,
                     CourseType = ECourseType.Traditional,
                     IsTemplate = false
                 };

            _mockCourseFactory.Setup(c => c.Create(It.IsAny<SaveCourseRequest>())).Returns(course);

            var response = _coursesController.Post(saveCourseRequest);
            var actual = response.Content.ReadAsAsync<CourseInfoResponse>().Result;
	        
			_mockDomainEvents.Verify(c => c.Raise<CourseCreated>(
		        It.Is<CourseCreated>(p => p.Code.Equals("PSY101") &&
		                                  p.Description.Equals("Psych!") &&
		                                  p.Name.Equals("Psychology 101") &&
                                          p.OrganizationId.Equals(saveCourseRequest.OrganizationId) &&
                                          p.CourseType.Equals(saveCourseRequest.CourseType) &&
                                          p.IsTemplate.Equals(saveCourseRequest.IsTemplate))), Times.Once());
			
        }

        [Test]
        public void can_create_a_course_from_template()
        {
            SetUpApiController();

            var organizationId = Guid.NewGuid();
            var templateId = Guid.NewGuid();
            var template = new Domain.Entities.Course
                {
                    Id = templateId
                };
            var saveCourseRequest = new SaveCourseRequest
            {
                Id = Guid.NewGuid(),
                Code = "PSY101",
                Description = "Psych!",
                Name = "Psychology 101",
                OrganizationId = organizationId,
                TemplateCourseId = templateId,
                CourseType = ECourseType.Traditional,
                IsTemplate = false
            };

            var course = new Domain.Entities.Course
            {
                ActiveFlag = true,
                Code = saveCourseRequest.Code,
                Description = saveCourseRequest.Description,
                Name = saveCourseRequest.Name,
                Id = saveCourseRequest.Id,
                Template = template,
                OrganizationId = saveCourseRequest.OrganizationId,
                CourseType = ECourseType.Traditional,
                IsTemplate = false
            };

            _mockCourseFactory.Setup(c => c.Reconstitute(It.IsAny<Guid>()))
                              .Returns(template);
            _mockCourseFactory.Setup(c => c.BuildFromTemplate(It.IsAny<Domain.Entities.Course>())).Returns(course);

            var response = _coursesController.Post(saveCourseRequest);
            var actual = response.Content.ReadAsAsync<CourseInfoResponse>().Result;

            _mockCourseFactory.Verify(c => c.Reconstitute(templateId));
            _mockCourseFactory.Verify(c => c.BuildFromTemplate(template));
            _mockDomainEvents.Verify(c => c.Raise<CourseCreated>(
                It.Is<CourseCreated>(p => p.Code.Equals("PSY101") &&
                                          p.Description.Equals("Psych!") &&
                                          p.Name.Equals("Psychology 101") &&
                                          p.OrganizationId.Equals(saveCourseRequest.OrganizationId) &&
                                          p.CourseType.Equals(saveCourseRequest.CourseType) &&
                                          p.Template.Equals(template) &&
                                          p.IsTemplate.Equals(saveCourseRequest.IsTemplate))), Times.Once());

        }
        [Test]
        public void can_update_an_existing_course()
        {
            Mapper.CreateMap<Domain.Entities.Course, SaveCourseRequest>();

            var courses = SetUpExistingCourse();
            _mockCourseFactory.Setup(c => c.Reconstitute(It.IsAny<Guid>())).Returns(courses.First());

            var saveCourseRequest = Mapper.Map<SaveCourseRequest>(courses.First());

            _coursesController.Put(saveCourseRequest.Id, saveCourseRequest);

            _mockDomainEvents.Verify(c => c.Raise<CourseUpdated>(
				It.IsAny<CourseUpdated>()),Times.Once());
        }

        [Test]
        public void cannot_update_non_existing_course()
        {
            Mapper.CreateMap<Domain.Entities.Course, SaveCourseRequest>();

            var saveCourseRequest = new SaveCourseRequest();

            var response =
                Assert.Throws<HttpResponseException>(() => _coursesController.Put(Guid.NewGuid(), saveCourseRequest)).Response;
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

        }

        [Test]
        public void can_get_list_of_course()
        {
	        var criteria = new Mock<ICriteria>();
			var courses = SetUpExistingCourse().ToList();
			criteria.Setup(c => c.List<Domain.Entities.Course>()).Returns(courses);
            
			_coursesController.Request = new HttpRequestMessage(HttpMethod.Get,"http://locahost/courses");
	        _mockCourseRepository.Setup(c => c.ODataQuery<Domain.Entities.Course>(It.IsAny<String>()))
	                             .Returns(criteria.Object);
	        var courseInfoList = _coursesController.Get(GetMockODataQueryOptions(_coursesController.Request));
            
			Assert.That(courseInfoList.Count() == courses.Count());
        }

	    private ODataQueryOptions GetMockODataQueryOptions(HttpRequestMessage request)
	    {
			ODataModelBuilder modelBuilder = new ODataConventionModelBuilder();
			modelBuilder.EntitySet<Domain.Entities.Course>("Courses");
			var opts = new ODataQueryOptions<Domain.Entities.Course>(new ODataQueryContext(modelBuilder.GetEdmModel(), typeof(Domain.Entities.Course)), request);
		    return opts;
	    }

	    [Test]
        public void can_delete_a_course()
        {
            _mockCourseFactory.Setup(c => c.Reconstitute(It.IsAny<Guid>())).Returns(new Domain.Entities.Course());
            var courses = SetUpExistingCourse();

            var id = courses.FirstOrDefault().Id;
            _coursesController.Delete(id);

            _mockDomainEvents.Verify(c => c.Raise<CourseDeleted>(It.Is<CourseDeleted>(d => d.AggregateId == id)), Times.Once());
        }

        [Test]
        public void should_throw_exception_deleting_non_existing_course()
        {
            SetUpExistingCourse();
            var id = Guid.NewGuid();

            var response =
                Assert.Throws<HttpResponseException>(() => _coursesController.Delete(id)).Response;
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public void can_search_course_by_name_name()
        {
            var courses = SetUpExistingCourse();
            var course = courses.FirstOrDefault(c => c.Name.Equals("Psychology 101"));

            var courseInfo = _coursesController.GetByName(course.Name);
            Assert.That(courseInfo.Name.Equals(course.Name));
            Assert.That(courseInfo.Code.Equals(course.Code));
        }

        [Test]
        public void can_search_course_by_code()
        {
            var courses = SetUpExistingCourse();
            var course = courses.FirstOrDefault(c => c.Code.Equals("PSY102"));

            var courseInfo = _coursesController.GetByName(course.Name);
            Assert.That(courseInfo.Name.Equals(course.Name));
            Assert.That(courseInfo.Code.Equals(course.Code));
        }

        private void SetUpApiController()
        {
            var configuration = new HttpConfiguration();
            // Register the route
            WebApiConfig.Register(configuration);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("http://localhost/Courses"),
                Content = new HttpMessageContent(new HttpRequestMessage(HttpMethod.Post, "http://localhost/courses"))
            };

            _coursesController.Request = request;
            _coursesController.Request.Properties["MS_HttpConfiguration"] = configuration;
            _coursesController.Url = new UrlHelper(_coursesController.Request);
        }

        private IEnumerable<Domain.Entities.Course> SetUpExistingCourse()
        {

            var courses = new List<Domain.Entities.Course>
                {
                    new Domain.Entities.Course
                        {
                            Id = Guid.NewGuid(),
                            Code = "PSY101",
                            Description = "Psych!",
                            Name = "Psychology 101",
                            ActiveFlag = true,
                            OrganizationId = _organizationIds[1],
                            Template = new Domain.Entities.Course {Id = Guid.NewGuid()},
                            CourseType = ECourseType.Traditional,
                            IsTemplate = false
                        },
                    new Domain.Entities.Course
                        {
                            Id = Guid.NewGuid(),
                            Code = "PSY102",
                            Description = "Psych!",
                            Name = "Psychology 102",
                            ActiveFlag = true,
                            OrganizationId = _organizationIds[2],
                            Template = new Domain.Entities.Course {Id = Guid.NewGuid()},
                             CourseType = ECourseType.Traditional,
                             IsTemplate = false
                        },
                    new Domain.Entities.Course
                        {
                            Id = Guid.NewGuid(),
                            Code = "PSY103",
                            Description = "Psych!",
                            Name = "Psychology 103",
                            ActiveFlag = true,
                            OrganizationId = _organizationIds[3],
                            Template = new Domain.Entities.Course {Id = Guid.NewGuid()},
                             CourseType = ECourseType.Traditional,
                             IsTemplate = false
                        }
                };
            _mockCourseRepository.Setup(t => t.Query<Domain.Entities.Course>()).Returns(courses.AsQueryable());

            return courses;
        }
    }
}
