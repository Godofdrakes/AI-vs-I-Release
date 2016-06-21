using AI_vs_I.Units;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;


namespace AI_vs_I.UserInterface.Unit_Crafting {

    public class UnitCraftingPreview : MonoBehaviour {
        #region Functions 

        public void UpdateUnitPreview( UnitDefinition unit ) {
            m_costText.text = unit.TotalCost.ToString();
            m_moveText.text = unit.Movement.ToString();
            m_healthText.text = unit.MaxHealth.ToString();
            m_actionText.text = unit.ActionModules.Length.ToString();
        }

        #endregion


        #region Inspector Fields

        [ SerializeField, UsedImplicitly ]
        private Text m_costText;

        [ SerializeField, UsedImplicitly ]
        private Text m_moveText;

        [ SerializeField, UsedImplicitly ]
        private Text m_healthText;

        [ SerializeField, UsedImplicitly ]
        private Text m_actionText;

        #endregion
    }

}
