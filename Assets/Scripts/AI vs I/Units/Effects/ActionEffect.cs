using UnityEngine;
using System.Collections;

namespace AI_vs_I.Units
{

    public enum ActionEffectType
    {
        // Stat Change
        Damage = 0,
        HPBoost,
        SpeedBoost,
        // Major Status
        Corrupt,
        Freeze,
    }

    [System.Serializable]
    public struct ActionEffect
    {
        [SerializeField]
        private ActionEffectType m_effectType;

        [SerializeField]
        private int m_effectStrength;

        [SerializeField]
        private int m_effectDuration;

        public ActionEffectType EffectType
        {
            get
            {
                return m_effectType;
            }
        }

        public int EffectStrength
        {
            get
            {
                return m_effectStrength;
            }
        }

        public int EffectDuration
        {
            get
            {
                return m_effectDuration;
            }
        }
    }

}