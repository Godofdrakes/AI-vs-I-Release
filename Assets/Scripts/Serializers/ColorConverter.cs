using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;


namespace Serializers {

    public class ColorConverter : JsonConverter {

        public override bool CanConvert( Type objectType ) { return typeof( Color ) == objectType; }

        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer ) {
            Color c = new Color( 0, 0, 0, 1 );
            JObject jObject = JObject.Load( reader );

            if( jObject["r"] != null ) { c.r = jObject["r"].Value<float>(); }
            if( jObject["g"] != null ) { c.g = jObject["g"].Value<float>(); }
            if( jObject["b"] != null ) { c.b = jObject["b"].Value<float>(); }
            if( jObject["a"] != null ) { c.a = jObject["a"].Value<float>(); }

            return c;
        }

        public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer ) {
            Color? c = value as Color?;
            JObject jObject = new JObject();
            if( c.HasValue ) {
                jObject["r"] = c.Value.r;
                jObject["g"] = c.Value.g;
                jObject["b"] = c.Value.b;
                jObject["a"] = c.Value.a;
            }
            else {
                jObject["r"] = 1.0f;
                jObject["g"] = 1.0f;
                jObject["b"] = 1.0f;
                jObject["a"] = 1.0f;
            }

            jObject.WriteTo( writer );
        }

    }

}
