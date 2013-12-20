using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BpeProducts.Services.Course.Domain.Validation
{
    public static class Validator
    {
        private static Dictionary<Type, object> _validators = new Dictionary<Type, object>();

        public static void RegisterValidatorFor<T>(T entity, IValidator<T> validator)
            where T : IValidatable<T>
        {
            _validators.Add(entity.GetType(), validator);
        }

        public static IValidator<T> GetValidatorFor<T>(T entity)
            where T : IValidatable<T>
        {
            return _validators[entity.GetType()] as IValidator<T>;
        }

        public static bool Validate<T>(this T entity, out IEnumerable<string> brokenRules)
            where T : IValidatable<T>
        {
            IValidator<T> validator = Validator.GetValidatorFor(entity);

            return entity.Validate(validator, out brokenRules);
        }
    }
}
