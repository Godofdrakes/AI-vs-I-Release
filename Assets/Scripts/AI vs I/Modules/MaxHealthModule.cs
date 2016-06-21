using Newtonsoft.Json;
using Serializers;
using UnityEngine;


namespace AI_vs_I.Modules {

    /// <summary>
    ///     Represents the maximum health a unit can have.
    /// </summary>
    [ CreateAssetMenu( fileName = "NewHealthModule", menuName = "AI vs I/Units/Health Module" )]
    public class MaxHealthModule : BaseUnitModule {
        #region inspector Fields

        [ SerializeField ]
        protected int m_maxHealthValue = 1;

        #endregion


        #region Properties

        /// <summary>
        ///     Defines the unit's max health value.
        /// </summary>
        public int MaxHealthValue {
            get { return m_maxHealthValue; }
        }

        #endregion
    }

}
