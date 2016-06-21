using AI_vs_I.Units;
using Newtonsoft.Json;
using Serializers;
using UnityEngine;


namespace AI_vs_I.Modules {

    /// <summary>
    ///     Represents an action a unit can perform.
    /// </summary>
    [ CreateAssetMenu( fileName = "NewActionModule", menuName = "AI vs I/Units/Action Module" )]
    public class ActionModule : BaseUnitModule {
        #region Inspector Fields

        [ SerializeField, Tooltip( "Can this action target enemy units?" ) ]
        private bool m_canEnemyTarget = true;

        [ SerializeField, Tooltip( "Can this action target friendly units?" ) ]
        private bool m_canFriendTarget = false;

        [ SerializeField, Tooltip( "Can this action target the unit using it?" ) ]
        private bool m_canSelfTarget = false;

        [ SerializeField ]
        private ActionEffect[] m_targetEffects = new ActionEffect[0];

        [ SerializeField ]
        private ActionEffect[] m_userEffects = new ActionEffect[0];

        [ SerializeField, Tooltip( "How far can this action reach?" ) ]
        private uint m_rangeValue = 1;

        #endregion


        #region Properties

        /// <summary>
        ///     Defines the range of the action.
        /// </summary>
        public uint RangeValue {
            get { return m_rangeValue; }
        }

        /// <summary>
        ///     Defines the effects of the action to the target.
        /// </summary>
        public ActionEffect[] TargetEffects {
            get { return m_targetEffects; }
        }

        /// <summary>
        ///     Defines the effects of the action to the user.
        /// </summary>
        public ActionEffect[] UserEffects {
            get { return m_userEffects; }
        }

        /// <summary>
        ///     Can the acting unit target itself?
        /// </summary>
        public bool CanSelfTarget {
            get { return m_canSelfTarget; }
        }

        /// <summary>
        ///     Can the acting unit target other units on the same team?
        /// </summary>
        public bool CanFriendTarget {
            get { return m_canFriendTarget; }
        }

        /// <summary>
        ///     Can the acting unit target other units on other teams?
        /// </summary>
        public bool CanEnemyTarget {
            get { return m_canEnemyTarget; }
        }

        #endregion
    }

}
