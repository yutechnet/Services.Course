﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using BpeProducts.Common.Exceptions;
using BpeProducts.Common.NHibernate.Version;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.ProgramAggregates;
using BpeProducts.Services.Course.Domain.Validation;
using BpeProducts.Services.Section.Contracts;
using Castle.Core.Internal;
using Newtonsoft.Json;
using Services.Assessment.Contract;

namespace BpeProducts.Services.Course.Domain.CourseAggregates
{
    public class Course : VersionableEntity, IValidatable<Course>
    {
        #region Properties

        private string _name;
        private string _code;
        private string _description;
        private decimal _credit;
        private ECourseType _courseType;
        private IList<CourseSegment> _segments = new List<CourseSegment>();
        private IList<Program> _programs = new List<Program>();
        private IList<Course> _prerequisites = new List<Course>();
        private IList<LearningMaterial> _learningMaterials = new List<LearningMaterial>();

        [JsonProperty]
        public virtual Course Template { get; protected internal set; }

        [JsonProperty]
        public virtual bool IsTemplate { get; protected internal set; }

        [Required]
        public virtual string Name
        {
            get { return _name; }
            set
            {
                CheckPublished();
                _name = value;
            }
        }

        [Required]
        public virtual string Code
        {
            get { return _code; }
            set
            {
                CheckPublished();
                _code = value;
            }
        }
        public virtual IList<LearningMaterial> LearningMaterials
        {
            get { return _learningMaterials; }
            set { _learningMaterials = value; }
        }
        public virtual string Description
        {
            get { return _description; }
            set
            {
                CheckPublished();
                _description = value;
            }
        }

        public virtual decimal Credit
        {
            get { return _credit; }
            set
            {
                CheckPublished();
                _credit = value;
            }
        }

        public virtual ECourseType CourseType
        {
            get { return _courseType; }
            set
            {
                CheckPublished();
                _courseType = value;
            }
        }

        public virtual IList<Program> Programs
        {
            get { return _programs; }
            set { _programs = value; }
        }

        public virtual IList<CourseSegment> Segments
        {
            get { return _segments; }
            protected internal set { _segments = value; }
        }

        public virtual IList<Course> Prerequisites
        {
            get { return _prerequisites; }
            protected internal set { _prerequisites = value; }
        }

        #endregion

        public virtual void SetPrograms(IList<Program> programs)
        {
            CheckPublished();

            Programs.Clear();
            Programs = programs;
        }

        public virtual void AddPrerequisite(Course prerequisite)
        {
            CheckPublished();

            if (!prerequisite.IsPublished)
            {
                throw new ForbiddenException("Prerequisite item " + prerequisite.Id + " - " + prerequisite.Name +
                                             "is not yet published, and thus cannot be used as a prerequisite to this course.");
            }

            var prequisiteAlreadyExistsCheck = _prerequisites.FirstOrDefault(p => p.Id == prerequisite.Id);
            if (prequisiteAlreadyExistsCheck == null)
            {
                _prerequisites.Add(prerequisite);
            }
        }

        public virtual void RemovePrerequisite(Guid prerequisiteCourseId)
        {
            CheckPublished();

            var prequisiteToRemove = _prerequisites.FirstOrDefault(p => p.Id == prerequisiteCourseId);
            if (prequisiteToRemove != null)
            {
                _prerequisites.Remove(prequisiteToRemove);
            }
        }

        public virtual CourseSegment AddSegment(SaveCourseSegmentRequest request)
        {
            return AddSegment(Guid.Empty, request);
        }

        public virtual CourseSegment AddSegment(Guid parentSegmentId, SaveCourseSegmentRequest request)
        {
            CheckPublished();

            CourseSegment parentSegment = null;
            if (parentSegmentId != Guid.Empty)
            {
                parentSegment = _segments.FirstOrDefault(s => s.Id == parentSegmentId);

                if (parentSegment == null)
                    throw new BadRequestException(
                        string.Format(
                            "Cannot add segment to Course {0} with parent Segment SegmentId {1}. Parent segment SegmentId does not exists",
                            Id, parentSegmentId));
            }

            var newSegment = new CourseSegment
            {
                Course = this,
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                DisplayOrder = request.DisplayOrder,
                Type = request.Type,
                TenantId = TenantId,
                ActiveFlag = true,
                ActiveDate = request.ActiveDate,
                InactiveDate = request.InactiveDate
            };

            if (parentSegment != null)
            {
                parentSegment.AddSubSegment(newSegment);
            }

            _segments.Add(newSegment);

            return newSegment;
        }

        public virtual CourseSegment UpdateSegment(Guid segmentId, SaveCourseSegmentRequest request)
        {
            var segment = _segments.FirstOrDefault(s => s.Id == segmentId);

            if (segment == null)
                throw new NotFoundException(string.Format("Segment {0} for Course {1} is not found.", segmentId, Id));

            segment.Name = request.Name;
            segment.Description = request.Description;
            segment.Type = request.Type;
            segment.DisplayOrder = request.DisplayOrder;

            return segment;
        }

        public virtual CourseSegment ReorderSegment(Guid segmentId, UpdateCourseSegmentRequest request, int displayOrder)
        {
            var segment = _segments.FirstOrDefault(s => s.Id == segmentId);

            if (segment == null)
                throw new NotFoundException(string.Format("Segment {0} for Course {1} is not found.", segmentId, Id));

            segment.Name = request.Name;
            segment.Description = request.Description;
            segment.Type = request.Type;
            segment.DisplayOrder = displayOrder;

            return segment;
        }


        public virtual void DeleteSegment(Guid segmentId)
        {
            var segment = _segments.FirstOrDefault(s => s.Id == segmentId);
            if (segment != null)
            {
                segment.Delete();
            }
        }

        protected override VersionableEntity Clone()
        {
            var course = AutoMapper.Mapper.Map<Course>(this);
            course.Id = Guid.NewGuid();

            course.Programs = new List<Program>(this.Programs);
            course.Prerequisites = new List<Course>(this.Prerequisites);

            return course;
        }

        #region Create Section Request

        public virtual CreateSectionRequest GetSectionRequest(CourseSectionRequest request,
                                                              IAssessmentClient assessmentClient)
        {
            if (!IsPublished)
                throw new BadRequestException(
                    string.Format("Cannot create a section from course {0}. Course is not published.", Id));

            var translatedRequest = new CreateSectionRequest
            {
                Name = request.Name,
                CourseCode = request.CourseCode,
                SectionCode = request.SectionCode,
                OrganizationId = request.OrganizationId,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                TenantId = TenantId,
                CourseId = Id,
                LearningMaterials = BuildLearningMaterials(LearningMaterials),
                Segments = BuildSectionSegments(Segments.Where(s => s.ParentSegment == null)),
                Credit = Credit
            };

            return translatedRequest;
        }

        private List<SectionSegmentRequest> BuildSectionSegments(IEnumerable<CourseSegment> segments)
        {
            var sectionSegments = segments.Select(courseSegment => new SectionSegmentRequest
            {
                Name = courseSegment.Name,
                Description = courseSegment.Description,
                DisplayOrder = courseSegment.DisplayOrder,
                Type = courseSegment.Type,
                CourseSegmentId = courseSegment.Id,
                ChildSegments = BuildSectionSegments(courseSegment.ChildSegments),
                LearningActivities = BuildLearningActivities(courseSegment.CourseLearningActivities),
                LearningMaterials = BuildLearningMaterials(courseSegment.LearningMaterials)
            }).ToList();

            return sectionSegments;
        }

        private List<SectionLearningActivityRequest> BuildLearningActivities(
            IEnumerable<CourseLearningActivity> courseLearningActivities)
        {
            var sectionLearningActivities =
                courseLearningActivities.Select(courseLearningActivity => new SectionLearningActivityRequest
                {
                    Name = courseLearningActivity.Name,
                    CourseLearningActivityId = courseLearningActivity.Id,
                    Type = (SectionLearningActivityType)courseLearningActivity.Type,
                    ActiveDate = courseLearningActivity.ActiveDate,
                    InactiveDate = courseLearningActivity.InactiveDate,
                    DueDate = courseLearningActivity.DueDate,
                    IsExtraCredit = courseLearningActivity.IsExtraCredit,
                    IsGradeable = courseLearningActivity.IsGradeable,
                    MaxPoint = courseLearningActivity.MaxPoint,
                    Weight = courseLearningActivity.Weight,
                    ObjectId = courseLearningActivity.ObjectId,
                    CustomAttribute = courseLearningActivity.CustomAttribute,
                    AssessmentId = courseLearningActivity.AssessmentId,
                    AssessmentType = courseLearningActivity.AssessmentType.ToString(),
                    Description = courseLearningActivity.Description
                }).ToList();

            return sectionLearningActivities;
        }

        private List<SectionLearningMaterialRequest> BuildLearningMaterials(IEnumerable<LearningMaterial> courseLearningMaterials)
        {
            var sectionLearningMaterials =
                courseLearningMaterials.Select(courseLearningMaterial => new SectionLearningMaterialRequest
                {
                    CourseLearningMaterialId = courseLearningMaterial.Id,
                    Instruction = courseLearningMaterial.Instruction,
                    AssetId = courseLearningMaterial.AssetId,
                    IsRequired = courseLearningMaterial.IsRequired,
                    CustomAttribute = courseLearningMaterial.CustomAttribute
                }).ToList();

            return sectionLearningMaterials;
        }

        #endregion

        #region CourseLearningActivity

        private CourseSegment GetSegmentOrThrow(Guid segmentId)
        {
            var segment = _segments.FirstOrDefault(s => s.Id == segmentId);

            if (segment == null)
                throw new NotFoundException(string.Format("Course {0} does not have segment with Id {1}", this.Id,
                                                          segmentId));

            return segment;
        }

        private CourseLearningActivity GetCourseLearningActivityOrThrow(Guid segmentId, Guid learningActivityId)
        {
            CourseSegment segment = GetSegmentOrThrow(segmentId);

            CourseLearningActivity learningActivity =
                segment.CourseLearningActivities.FirstOrDefault(s => s.Id == learningActivityId);

            if (learningActivity == null || !learningActivity.ActiveFlag)
                throw new NotFoundException(string.Format("Learning Activity {0} for Segment {1} is not found.",
                                                          learningActivityId, segmentId));
            return learningActivity;
        }

        public virtual Program AddProgram(Program program)
        {
            CheckPublished();

            if (_programs.FirstOrDefault(x => x.Id == program.Id) == null)
            {
                program.Courses.Add(this);
                _programs.Add(program);
            }

            return program;
        }

        public virtual void RemoveProgram(Guid programId)
        {
            CheckPublished();

            var program = _programs.FirstOrDefault(x => x.Id == programId);
            if (program != null)
            {
                program.Courses.Remove(this);
                _programs.Remove(program);
            }
        }

        public virtual CourseLearningActivity AddLearningActivity(Guid segmentId, SaveCourseLearningActivityRequest request)
        {
            CheckPublished();

            CourseSegment segment = GetSegmentOrThrow(segmentId);

            // TODO: Consider using AutoMapper here
            var courseLearningActivity = new CourseLearningActivity()
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Type = request.Type,
                IsExtraCredit = request.IsExtraCredit,
                IsGradeable = request.IsGradeable,
                MaxPoint = request.MaxPoint,
                Weight = request.Weight,
                ObjectId = request.ObjectId,
                Description = request.Description,
                CustomAttribute = request.CustomAttribute,
                TenantId = TenantId,
                ActiveDate = request.ActiveDate,
                InactiveDate = request.InactiveDate,
                DueDate = request.DueDate,
                AssessmentId = request.AssessmentId,
                AssessmentType = string.IsNullOrEmpty(request.AssessmentType)
                        ? AssessmentType.Custom
                        : (AssessmentType)Enum.Parse(typeof(AssessmentType), request.AssessmentType),
                ActiveFlag = true
            };


            segment.CourseLearningActivities.Add(courseLearningActivity);

            return courseLearningActivity;
        }

        public virtual CourseLearningActivity UpdateLearningActivity(Guid segmentId, Guid learningActivityId,
                                                                     SaveCourseLearningActivityRequest request)
        {
            CheckPublished();

            var learningActivity = GetCourseLearningActivityOrThrow(segmentId, learningActivityId);

            // TODO: Consider using AutoMapper here
            learningActivity.Name = request.Name;
            learningActivity.Type = request.Type;
            learningActivity.IsExtraCredit = request.IsExtraCredit;
            learningActivity.IsGradeable = request.IsGradeable;
            learningActivity.MaxPoint = request.MaxPoint;
            learningActivity.Weight = request.Weight;
            learningActivity.ObjectId = request.ObjectId;
            learningActivity.Description = request.Description;
            learningActivity.CustomAttribute = request.CustomAttribute;
            learningActivity.ActiveDate = request.ActiveDate;
            learningActivity.InactiveDate = request.InactiveDate;
            learningActivity.DueDate = request.DueDate;
            learningActivity.AssessmentId = request.AssessmentId;
            learningActivity.AssessmentType = string.IsNullOrEmpty(request.AssessmentType)
                                                  ? AssessmentType.Custom
                                                  : (AssessmentType)
                                                    Enum.Parse(typeof(AssessmentType), request.AssessmentType);

            return learningActivity;
        }

        public virtual CourseLearningActivity GetLearningActivity(Guid segmentId, Guid learningActivityId)
        {
            return GetCourseLearningActivityOrThrow(segmentId, learningActivityId);
        }

        public virtual IEnumerable<CourseLearningActivity> GetLearningActivity(Guid segmentId)
        {
            CourseSegment segment = GetSegmentOrThrow(segmentId);

            return
                AutoMapper.Mapper.Map<IList<CourseLearningActivity>>(
                    segment.CourseLearningActivities.Where(c => c.ActiveFlag.Equals(true)));
        }

        public virtual CourseLearningActivity DeleteLearningActivity(Guid segmentId, Guid learningActivityId)
        {

            CheckPublished();

            CourseLearningActivity learningActivity = GetCourseLearningActivityOrThrow(segmentId, learningActivityId);

            learningActivity.ActiveFlag = false;

            return learningActivity;
        }

        #endregion

        #region CourseLearningMaterial
        public virtual LearningMaterial AddLearningMaterial(LearningMaterialRequest learningMaterialRequest)
        {
            CheckPublished();
            var learningMaterial = Mapper.Map<LearningMaterial>(learningMaterialRequest);
            learningMaterial.TenantId = TenantId;
            learningMaterial.Id = Guid.NewGuid();
            learningMaterial.Course = this;
            _learningMaterials.Add(learningMaterial);
            return learningMaterial;
        }
        public virtual void UpdateLearningMaterial(Guid learningMaterialId, UpdateLearningMaterialRequest updatelearningMaterialRequest)
        {
            CheckPublished();
            var learningMaterial = GetLearningMaterialOrThrow(learningMaterialId);
            Mapper.Map(updatelearningMaterialRequest, learningMaterial);
        }

        public virtual void DeleteLearningMaterial(Guid learningMaterialId)
        {
            CheckPublished();
            var learningMaterial = GetLearningMaterialOrThrow(learningMaterialId);
            learningMaterial.ActiveFlag = false;
        }

        private LearningMaterial GetLearningMaterialOrThrow(Guid learningMaterialId)
        {
            var learningMaterial = _learningMaterials.SingleOrDefault(l => l.Id == learningMaterialId);

            if (learningMaterial == null)
                throw new NotFoundException(string.Format("Learning Material {0} for Course Learning Material {1} is not found.", learningMaterialId, Id));

            return learningMaterial;
        }

        public virtual LearningMaterial AddLearningMaterial(Guid segmentId,
                                                         LearningMaterialRequest learningMaterialRequest)
        {
            CheckPublished();

            var segment = GetSegmentOrThrow(segmentId);
            return segment.AddLearningMaterial(learningMaterialRequest);
        }

        public virtual void DeleteLearningMaterial(Guid segmentId, Guid learningMaterialId)
        {
            CheckPublished();

            var segment = GetSegmentOrThrow(segmentId);
            segment.DeleteLearningMaterial(learningMaterialId);
        }

        public virtual void UpdateLearningMaterial(Guid segmentId, Guid learningMaterialId,
                                                   UpdateLearningMaterialRequest updatelearningMaterialRequest)
        {
            CheckPublished();

            var segment = GetSegmentOrThrow(segmentId);
            segment.UpdateLearningMaterial(learningMaterialId, updatelearningMaterialRequest);
        }
        #endregion

        // Base method is not supported
        public override void Publish(string publishNote)
        {
            throw new NotImplementedException("this overload is not supported for course");
        }

        public virtual void Publish(string publishNote, ICoursePublisher coursePublisher)
        {
            coursePublisher.Publish(this, publishNote);
            base.Publish(publishNote);
        }

        public virtual void CloneOutcomes(IAssessmentClient assessmentClient)
        {
            assessmentClient.CloneEntityOutcomes(SupportingEntityType.Course, OriginalEntity.Id, new CloneEntityOutcomeRequest()
            {
                EntityId = Id,
                Type = SupportingEntityType.Course
            });
            Segments.ForEach(courseSegment => courseSegment.CloneOutcomes(assessmentClient));
        }

        public virtual bool Validate(IValidator<Course> validator, out IEnumerable<string> brokenRules)
        {
            var isValid = validator.IsValid(this);
            brokenRules = validator.BrokenRules(this);
            return isValid;
        }

        public virtual void Delete()
        {
            if (IsPublished)
            {
                throw new BadRequestException(string.Format("Course {0} is published and cannot be deleted.", Id));
            }

            ActiveFlag = false;
        }
    }
}