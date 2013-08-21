﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BpeProducts.Common.Exceptions;
using BpeProducts.Common.NHibernate.Version;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Entities;

namespace BpeProducts.Services.Course.Domain.Courses
{
    public class Course : VersionableEntity, ISupportingEntity
    {
        #region Properties

        private string _name;
        private string _code;
        private string _description;
        private ECourseType _courseType;
        private IList<CourseSegment> _segments = new List<CourseSegment>();
        private IList<Program> _programs = new List<Program>();
        private IList<LearningOutcome> _supportedOutcomes = new List<LearningOutcome>();
        private IList<Course> _prerequisites = new List<Course>();

        public virtual Course Template { get; protected internal set; }
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

        public virtual string Description
        {
            get { return _description; }
            set
            {
                CheckPublished();
                _description = value;
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
            protected internal set { _programs = value; }
        }

        public virtual IList<CourseSegment> Segments
        {
            get { return _segments; }
            protected internal set { _segments = value; }
        }

        public virtual IList<LearningOutcome> SupportedOutcomes
        {
            get { return _supportedOutcomes; }
            protected internal set { _supportedOutcomes = value; }
        }

        public virtual IList<Course> Prerequisites
        {
            get { return _prerequisites; }
            protected internal set { _prerequisites = value; }
        }

        #endregion

        public virtual void SupportOutcome(LearningOutcome outcome)
        {
            CheckPublished();

            _supportedOutcomes.Add(outcome);
            outcome.SupportingEntities.Add(this);
        }

        public virtual void UnsupportOutcome(LearningOutcome outcome)
        {
            CheckPublished();

            _supportedOutcomes.Remove(outcome);
            outcome.SupportingEntities.Remove(this);
        }

        public virtual void BuildIndex(IList<CourseSegment> segments, ref Dictionary<Guid, CourseSegment> index)
        {
            foreach (var courseSegment in segments)
            {
                index.Add(courseSegment.Id, courseSegment);
                if (courseSegment.ChildSegments.Count > 0) BuildIndex(courseSegment.ChildSegments, ref index);
            }
        }

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
				throw new ForbiddenException("Prerequisite item " + prerequisite.Id + " - " + prerequisite.Name + "is not yet published, and thus cannot be used as a prerequisite to this course.");
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

        public virtual CourseSegment AddSegment(Guid segmentId, Guid parentSegmentId, SaveCourseSegmentRequest request)
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
                    Id = segmentId,
                    Name = request.Name,
                    Description = request.Description,
                    DisplayOrder = request.DisplayOrder,
                    Type = request.Type,
                    //Content = request.Content ?? new List<Content>(),
                    TenantId = TenantId
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
            segment.Content = request.Content ?? new List<Content>();
            segment.DisplayOrder = request.DisplayOrder;

            return segment;
        }

        public virtual void DeleteSegment(Guid segmentId)
        {
            var segment = _segments.FirstOrDefault(s => s.Id == segmentId);

            segment.Delete();
        }

        protected override VersionableEntity Clone()
        {
            var course = AutoMapper.Mapper.Map<Course>(this);
            course.Id = Guid.NewGuid();
            return course;
        }

        #region CourseLearningActivity
        private CourseSegment GetSegmentOrThrow(Guid segmentId)
        {
            var segment = _segments.FirstOrDefault(s => s.Id == segmentId);

            if(segment == null)
                throw new NotFoundException(string.Format("Course {0} does not have segment with Id {1}", this.Id, segmentId));

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
        
        public virtual CourseLearningActivity AddLearningActivity(Guid segmentId, SaveCourseLearningActivityRequest request, Guid learningActivityId)
        {
            CheckPublished();

            CourseSegment segment = GetSegmentOrThrow(segmentId);

            var courseLearningActivity = new CourseLearningActivity()
            {
                Id = learningActivityId,
                Name = request.Name,
                Type = request.Type,
                IsExtraCredit = request.IsExtraCredit,
                IsGradeable = request.IsGradeable,
                MaxPoint = request.MaxPoint,
                Weight = request.Weight,
                ObjectId = request.ObjectId,
                TenantId = TenantId
            };

            if (courseLearningActivity != null)
            {
                segment.CourseLearningActivities.Add(courseLearningActivity);
            }

            return courseLearningActivity;
        }

        public virtual CourseLearningActivity UpdateLearningActivity(Guid segmentId, Guid learningActivityId, SaveCourseLearningActivityRequest request)
        {

            CheckPublished();

            var learningActivity = GetCourseLearningActivityOrThrow(segmentId, learningActivityId);

            learningActivity.Name = request.Name;
            learningActivity.Type = request.Type;
            learningActivity.IsExtraCredit = request.IsExtraCredit;
            learningActivity.IsGradeable = request.IsGradeable;
            learningActivity.MaxPoint = request.MaxPoint;
            learningActivity.Weight = request.Weight;
            learningActivity.ObjectId = request.ObjectId;

            return learningActivity;
        }

        public virtual CourseLearningActivity GetLearningActivity(Guid segmentId, Guid learningActivityId)
        {
            return GetCourseLearningActivityOrThrow(segmentId, learningActivityId);
        }

        public virtual IEnumerable<CourseLearningActivity> GetLearningActivity(Guid segmentId)
        {
            CourseSegment segment = GetSegmentOrThrow(segmentId);

            return AutoMapper.Mapper.Map<IList<CourseLearningActivity>>(segment.CourseLearningActivities.Where(c=>c.ActiveFlag.Equals(true)));
        }

        public virtual CourseLearningActivity DeleteLearningActivity(Guid segmentId, Guid learningActivityId)
        {

            CheckPublished();

            CourseLearningActivity learningActivity = GetCourseLearningActivityOrThrow(segmentId, learningActivityId);

            learningActivity.ActiveFlag=false;

            return learningActivity;
        }
        #endregion
    }
}
