using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Linq;

namespace BankApplication.Data
{
    public class DataProvider
    {
        string _JsonFilePath;

        public DataProvider(string JsonFilePath)
        {
            _JsonFilePath = JsonFilePath;
        }

        // Get all objects of type T from JSON.
        public IList<T> GetAllObjects<T>()
        {
            try
            {
                JArray json = JObject.Parse(File.ReadAllText(_JsonFilePath))[typeof(T).Name + 's'].Value<JArray>();
                return json.ToObject<IList<T>>();
            }
            catch
            {
                return default;
            }
        }

        // Save all objects of type T to JSON.
        public bool SaveAllObjects<T>( IList<T> list)
        {
            try
            {
                JObject Json = JObject.Parse(File.ReadAllText(_JsonFilePath)) as JObject;
                Json[typeof(T).Name + 's'] = JArray.FromObject(list);
                File.WriteAllText(_JsonFilePath, Json.ToString());
                return true;
            }
            catch
            {
                return false;
            }
            
        }

        // Get Object by ID.
        public T GetObjectByID<T>(string ID)
        {
            try
            {
                JArray json = JObject.Parse(File.ReadAllText(_JsonFilePath))[typeof(T).Name + 's'].Value<JArray>();
                IList<T> tList = json.ToObject<IList<T>>();
                return tList.First(t => (string)typeof(T).GetProperty("ID").GetValue(t) == ID);
            }
            catch(Exception E)
            {
                Console.WriteLine(E.Message);
                return default;
            }
         
        }

        // Update a object. 
        public bool UpdateObject<T>( T genericObject)
        {
            try
            {
                // Converting json file to json object.
                JObject Json = JObject.Parse(File.ReadAllText(_JsonFilePath)) as JObject;
                // Retrieving array of object.
                JArray jArray = Json[typeof(T).Name + 's'] as JArray;
                // Removing the old object.
                jArray.First(element => (string)element["ID"] == (string)typeof(T).GetProperty("ID").GetValue(genericObject)).Remove();
                // Adding the new object into it.
                jArray.Add(JToken.FromObject(genericObject));
                // Updating orginal Json object with updated Jarray.
                Json[typeof(T).Name + 's'] = jArray;
                // Writing the updated Json back to file.
                File.WriteAllText(_JsonFilePath, Json.ToString());
                return true;
            }
            catch
            {
                return false;
            }
       
        }

        // Add a object.
        public string AddObject<T>(T genericObject)
        {
            try
            {
                string GuidValue = Guid.NewGuid().ToString();
                // Adding GUID for the generic object.
                typeof(T).GetProperty("ID").SetValue(genericObject, GuidValue);
                // Converting json file to json object.
                JObject Json = JObject.Parse(File.ReadAllText(_JsonFilePath)) as JObject;
                // Retrieving array of object.
                JArray jArray = Json[typeof(T).Name + 's'] as JArray;
                // Adding the new object into it.
                jArray.Add(JToken.FromObject(genericObject));
                // Updating orginal Json object with updated Jarray.
                Json[typeof(T).Name + 's'] = jArray;
                // Writing the updated Json back to file.
                File.WriteAllText(_JsonFilePath,Json.ToString());
                return GuidValue;
            }
            catch
            {
                return "";
            }


        }



    }
}
