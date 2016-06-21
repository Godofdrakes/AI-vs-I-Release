using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;


namespace AI_vs_I.Alterations {

    [ JsonObject( MemberSerialization.OptOut ) ]
    public class AlterationContainer : List<IStatAlteration> {

        [ JsonIgnore ]
        public MaxHealthAlteration[] MaxHealthAlterations {
            get {
                return this.OfType<MaxHealthAlteration>()
                           .ToArray();
            }
        }

        [JsonIgnore]
        public MovementAlteration[] MovementAlterations
        {
            get
            {
                return this.OfType<MovementAlteration>()
                           .ToArray();
            }
        }

    }

}
