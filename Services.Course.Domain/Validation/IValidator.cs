using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BpeProducts.Services.Course.Domain.Validation
{
    public interface IValidator<in T>
    {
        bool IsValid(T entity);
        IEnumerable<string> BrokenRules(T entity);
    }
}
