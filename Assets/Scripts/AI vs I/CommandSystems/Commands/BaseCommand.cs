using Newtonsoft.Json;


namespace AI_vs_I.CommandSystems.Commands {

    [ JsonObject( MemberSerialization.OptIn ) ]
    public abstract class BaseCommand {

        public virtual JsonSerializerSettings SerializerSettings {
            get { return new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto }; }
        }

        public abstract void Do();

        public string ToJson( Formatting formatting = Formatting.None ) {
            return JsonConvert.SerializeObject( this, formatting, SerializerSettings );
        }

    }

}
