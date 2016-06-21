using System.Collections.Generic;
using AI_vs_I.Units;
using Newtonsoft.Json;


namespace AI_vs_I.CommandSystems.Commands
{

    public class ActionCommand : BaseCommand, IAlteredUnits
    {

        [JsonConstructor]
        public ActionCommand(int userUnitIndex, int targetUnitIndex, ActionEffect[] effects, ActionEffect[] userEffects/*, int bonusDamage = 0*/)
        {
            UserUnitIndex = userUnitIndex;
            TargetUnitIndex = targetUnitIndex;
            TargetEffects = effects;
            UserEffects = userEffects;
        }

        [JsonProperty]
        public int UserUnitIndex { get; private set; }

        [JsonProperty]
        public int TargetUnitIndex { get; private set; }

        [JsonProperty]
        public ActionEffect[] TargetEffects { get; private set; }

        [JsonProperty]
        public ActionEffect[] UserEffects { get; private set; }

        public List<UnitInstance> AlteredUnits
        {
            get
            {
                UnitController controller = UnitController.Instance;
                UnitInstance instance = controller.UnitInstances[TargetUnitIndex];
                return new List<UnitInstance> { instance };
            }
        }

        public override void Do()
        {
            UnitController controller = UnitController.Instance;
            UnitInstance target = controller.UnitInstances[TargetUnitIndex];
            UnitInstance user = controller.UnitInstances[UserUnitIndex];
            foreach (ActionEffect i in TargetEffects)
            {
                target.RecieveEffect(i);
            }
            foreach (ActionEffect i in UserEffects)
            {
                user.RecieveEffect(i);
            }
            //target.DrawBody();
            //user.DrawBody();
        }

    }

}
