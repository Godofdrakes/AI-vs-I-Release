using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TypeSafe;
using Object = UnityEngine.Object;


namespace Serializers {

    // TODO : Fix inability to get reference to sprite resource.
    // MAYBE : Serialize Sprite by serializeing reference to texture along with dimentions.
    //         Deserialize by making new sprite at runtime based on texture and dimentions.

    /// <summary>
    ///     Serializes and deserializes references to UnityEngine <see cref="UnityEngine.Object" />s stored as resources.
    /// </summary>
    public class UnityResourceConverter : JsonConverter {

        public override bool CanConvert( Type objectType ) {
            return typeof( Object ).IsAssignableFrom( objectType );
        }

        // REQUIRES TypeSafe
        /// <exception cref="FileNotFoundException">
        ///     No resource that matches the deserialized resource information can be found.
        /// </exception>
        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer ) {
            Object finalResource = null;

            JObject jObject = JObject.Load( reader );
            string resourceName = jObject["ResourceName"].Value<string>();

            if( string.IsNullOrEmpty( resourceName ) ) { return null; }

            IResource foundResource =
                SRResources.GetContentsRecursive()
                           .FirstOrDefault(
                                           resource=>
                                           objectType.IsAssignableFrom( resource.Type ) &&
                                           resource.Name == resourceName );

            if( foundResource != null ) { finalResource = foundResource.Load(); }

            if( finalResource == null ) {
                throw new FileNotFoundException(
                    string.Format( "Failed to find resource {0} of type {1}.",
                                   resourceName,
                                   objectType.FullName ),
                    resourceName );
            }

            return finalResource;
        }

        public override void WriteJson(
            JsonWriter writer,
            object value,
            JsonSerializer serializer ) {
            Object resource = value as Object;
            JObject jObject = new JObject();

            // CanConvert will ensure that value is a UnityEngine.Object
            // Json.Net handles null serialization itself adn won't pass it to a custom JsonConverter

            // ReSharper disable once PossibleNullReferenceException
            jObject["ResourceName"] = resource != null ? resource.name : string.Empty;

            jObject.WriteTo( writer );
        }

    }

}
