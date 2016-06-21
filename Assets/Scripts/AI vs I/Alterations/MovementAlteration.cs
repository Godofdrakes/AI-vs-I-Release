using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;


namespace AI_vs_I.Alterations
{

    [System.Serializable, JsonObject(MemberSerialization.OptIn)]
    public class MovementAlteration : IStatAlteration
    {

        [SerializeField, JsonProperty]
        private int m_turnsRemaining;

        [SerializeField, JsonProperty]
        private int m_value;

        public int Value
        {
            get { return m_value; }
        }

        /// <summary>
        /// Creates a new <see cref="MaxHealthAlteration"/>.
        /// </summary>
        /// <param name="turnDuration">How many turns this buff should last. Anything less than 0 is an infinite duration.</param>
        public MovementAlteration(int turnDuration, int value)
        {
            m_turnsRemaining = turnDuration;
            m_value = value;
        }

        public bool Permanent
        {
            get { return m_turnsRemaining < 0; }
        }

        public int TurnsRemaining
        {
            get { return m_turnsRemaining; }
            set { m_turnsRemaining = value; }
        }

    }

}
