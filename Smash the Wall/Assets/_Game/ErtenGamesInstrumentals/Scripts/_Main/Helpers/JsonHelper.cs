#if NewtonsoftInstalled
using Newtonsoft.Json;
#endif

namespace Helpers
{
    public class JsonHelper
    {
        public static string ToJson<T>(T obj)
        {
#if NewtonsoftInstalled
            return JsonConvert.SerializeObject(obj);
#else
            return default(string);
#endif
        }

        public static T FromJson<T>(string obj)
        {
#if NewtonsoftInstalled
            return JsonConvert.DeserializeObject<T>(obj);
#else
            return default(T);
#endif
        }
    }
}
