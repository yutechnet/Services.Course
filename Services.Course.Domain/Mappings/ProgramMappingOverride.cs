using BpeProducts.Services.Course.Domain.ProgramAggregates;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace BpeProducts.Services.Course.Domain.Mappings
{
    public class ProgramMappingOverride : IAutoMappingOverride<Program>
    {
        public void Override(AutoMapping<Program> mapping)
        {
            mapping
                .HasManyToMany(x => x.Courses)
                .ParentKeyColumn("ProgramId")
                .ChildKeyColumn("CourseId")
                .Table("CourseProgram")
                .Inverse();
        }
    }
}

