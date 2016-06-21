using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AI_vs_I.Units
{
    [System.Serializable]
    public struct UnitRevertState
    {

        //public Vector3 m_position;

        public List<UnitBody> m_body;

        public int m_currentHealth;

        public List<GGCell> m_moveHistory;

        public UnitRevertState(/*Vector3 pos,*/ List<UnitBody> body, int health, List<GGCell> history)
        {
            /*m_position = pos;*/
            m_body = body.ToList();
            m_currentHealth = health;
            m_moveHistory = history.ToList();
        }
        /*public void StoreRevert(UnitInstance target)
        {
            m_body = target.Body;
            m_currentHealth = target.CurrentHealth;
            m_moveHistory = (List<GGCell>)target.MoveHistory;
        }*/
    }

}
