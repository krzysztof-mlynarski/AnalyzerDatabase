using System;
using AnalyzerDatabase.Interfaces;
using Newtonsoft.Json;

namespace AnalyzerDatabase.Services
{
    public class DeserializeJsonService : IDeserializeJsonService
    {
        public T GetObjectFromJson<T>(string jsonToDeserialize)
        {
            if (!String.IsNullOrEmpty(jsonToDeserialize))
            {
                try
                {
                    T tagObject = JsonConvert.DeserializeObject<T>(jsonToDeserialize);
                    return tagObject;
                }
                catch (Exception)
                {
                    return default(T);
                }
            }
            else
            {
                return default(T);
            }
        }
    }
}