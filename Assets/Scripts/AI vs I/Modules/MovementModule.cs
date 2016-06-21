using UnityEngine;


namespace AI_vs_I.Modules {

    /// <summary>
    ///     Represents the distance a unit can move.
    /// </summary>
    [ CreateAssetMenu( fileName = "NewMoveModule", menuName = "AI vs I/Units/Move Module" ) ]
    public class MovementModule : BaseUnitModule {
        #region Inspector Fields

        [ SerializeField ]
        protected int m_moveValue = 1;

        #endregion


        #region Properties

        /// <summary>
        ///     Defines the unit's move value.
        /// </summary>
        public int MoveValue {
            get { return m_moveValue; }
        }

        #endregion
    }

}
