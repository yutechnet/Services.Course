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

            mapping.Map(x => x.Description)
               .CustomType("StringClob")
               .Column("Description");

            mapping.Map(x => x.Name)
                   .Length(250);

            mapping.References<Program>(c => c.OriginalEntity);
            mapping.References<Program>(c => c.ParentEntity);
        }
    }
}

