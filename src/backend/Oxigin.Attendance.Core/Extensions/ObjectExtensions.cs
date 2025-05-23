using Newtonsoft.Json;

namespace Oxigin.Attendance.Core.Extensions
{
    /// <summary>
    /// A collection of object extensions.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Clone an object in a deep way.
        /// </summary>
        /// <typeparam name="TObjectType">The type of the object to clone.</typeparam>
        /// <param name="obj">The object context.</param>
        /// <returns>The cloned/detatched object of the desired type.</returns>
        public static TObjectType DeepClone<TObjectType>(this TObjectType obj)
        {
            var clonedObj = JsonConvert.DeserializeObject<TObjectType>(JsonConvert.SerializeObject(obj));

            return clonedObj;
        }
    }
}
