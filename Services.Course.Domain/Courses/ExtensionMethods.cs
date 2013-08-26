using Newtonsoft.Json;

namespace BpeProducts.Services.Course.Domain.Courses
{
	public static class ExtensionMethods
	{
		
		public static T DeepClone<T>(this T a)
		{
			var s = JsonConvert.SerializeObject(a);
			var o= (T)JsonConvert.DeserializeObject(s,typeof(T));
			return o;
		}
	}
}