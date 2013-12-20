using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BpeProducts.Services.Course.Domain.Validation
{
    public interface IValidatable<out T>
    {
        bool Validate(IValidator<T> validator, out IEnumerable<string> brokenRules);
    }
}
