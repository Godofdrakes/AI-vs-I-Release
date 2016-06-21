using System.Collections.Generic;
using AI_vs_I.Units;
using Newtonsoft.Json;


namespace AI_vs_I.CommandSystems.Commands {

    public class MoveCommand : BaseCommand, IAlteredUnits {

        [ JsonConstructor ]
        public MoveCommand( int subjectUnitIndex, int cellX, int cellY ) {
            SubjectUnitIndex = subjectUnitIndex;
            CellX = cellX;
            CellY = cellY;
        }

        [ JsonProperty ]
        public int SubjectUnitIndex { get; private set; }

        [ JsonProperty ]
        public int CellX { get; private set; }

        [ JsonProperty ]
        public int CellY { get; private set; }

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
            instance.Move( controller.GGGrid.Cells[CellX, CellY] );
        }

    }

}
