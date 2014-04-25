using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace BpeProducts.Services.Course.Domain.Mappings
{
    public class ProgramMappingOverride : IAutoMappingOverride<Entities.Program>
    {
        public void Override(AutoMapping<Entities.Program> mapping)
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

