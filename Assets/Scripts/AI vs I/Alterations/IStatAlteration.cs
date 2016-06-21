using System.Collections.Generic;
using Newtonsoft.Json;


namespace AI_vs_I.Alterations {
    
    public interface IStatAlteration {

        bool Permanent { get; }
        int TurnsRemaining { get; set; }

    }

}
