using Newtonsoft.Json;

namespace BpeProducts.Services.Course.Domain.CourseAggregates
{
	public static class ExtensionMethods
	{
		
		public static T DeepClone<T>(this T a)
		{
			var s = JsonConvert.SerializeObject(a, Formatting.None, new JsonSerializerSettings(){ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
			var o= (T)JsonConvert.DeserializeObject(s,typeof(T));
			return o;
		}
	}
}