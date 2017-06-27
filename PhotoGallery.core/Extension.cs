using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoGallery
{
    public static class Extension
    {
        #region Generic Type Extensions
        /// <summary>
        /// Update properties with properties of the object Supplied (typically anonymous)
        /// </summary>
        /// <typeparam name="T">Type of Source Object</typeparam>
        /// <param name="destination">Object whose property you want to update</param>
        /// <param name="source">destination object (typically anonymous) you want to take values from</param>
        /// <returns>Update reference to same Object</returns>
        [System.Diagnostics.DebuggerStepThrough]
        public static T Assign<T>(this T destination, object source)
        {
            if (destination != null && source != null)
            {
                var query = from sourceProperty in source.GetType().GetProperties()
                            join destProperty in destination.GetType().GetProperties()
                            on sourceProperty.Name.ToLower() equals destProperty.Name.ToLower()             //Case Insensitive Match
                            where destProperty.PropertyType.IsAssignableFrom(sourceProperty.PropertyType)   //Properties can be Assigned
                            where destProperty.GetSetMethod() != null                                       //Destination Property is not Readonly
                            select new { sourceProperty, destProperty };


                foreach (var pair in query)
                {
                    //Go ahead and Assign the value on the destination
                    pair.destProperty
                        .SetValue(destination,
                            value: pair.sourceProperty.GetValue(obj: source, index: new object[] { }),
                            index: new object[] { });
                }
            }
            return destination;
        }

        public static U Pipe<T, U>(this T value, Func<T, U> projection) => projection(value);
        #endregion;
    }
}
