using System.Collections.Generic;
using AI_vs_I.Units;
using Newtonsoft.Json;


namespace AI_vs_I.CommandSystems.Commands {

    public class DamageCommand : BaseCommand, IAlteredUnits {

        [ JsonConstructor ]
        public DamageCommand( int subjectUnitIndex, int damageValue ) {
            SubjectUnitIndex = subjectUnitIndex;
            DamageValue = damageValue;
        }

        [ JsonProperty ]
        public int SubjectUnitIndex { get; private set; }

        [ JsonProperty ]
        public int DamageValue { get; private set; }

        public List<UnitInstance> AlteredUnits {
            get {
                UnitController controller = UnitController.Instance;
                UnitInstance instance = controller.UnitInstances[SubjectUnitIndex];
                return new List<UnitInstance> { instance };
            }
        }

        public override void Do() {
            UnitController controller = UnitController.Instance;
            UnitInstance instance = controller.UnitInstances[SubjectUnitIndex];
            instance.TakeDamage( DamageValue );
        }

    }

}
