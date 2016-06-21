using System.Collections.Generic;
using AI_vs_I.Units;


namespace AI_vs_I.CommandSystems.Commands {

    public interface IAlteredUnits {

        List<UnitInstance> AlteredUnits { get; }

    }

}