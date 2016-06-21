using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Serializers;
using AI_vs_I.Units;
using SceneTransitions;

namespace AI_vs_I.Levels
{
    [ExecuteInEditMode]
    public class LevelEditor : MonoBehaviour
    {
        [SerializeField]
        private string m_levelName;

        [SerializeField]
        private LevelDefinition m_level;

        private GGGrid m_grid;
        // Use this for initialization
        void Start()
        {
            m_grid = FindObjectOfType<GGGrid>();
        }

        //[ContextMenu("Update")]
        void Update()
        {
            m_level.Clean();
            m_level.GridDimensions = new Vector2(m_grid.GridWidth, m_grid.GridHeight);
            m_grid.Update();
            foreach (GGCell i in m_grid.Cells)
            {
                if (!i.IsPathable)
                {
                    Vector2 vecToAdd = i.CenterPoint2D + (m_level.GridDimensions / 2);
                    m_level.Walls.Add(vecToAdd);
                }
            }
            foreach (SpawnPoint i in FindObjectsOfType<SpawnPoint>())
            {
                SpawnPointDefinition newSD;
                newSD.position = (Vector2)i.transform.position + (m_level.GridDimensions / 2);
                newSD.enforcedUnit = i.EnforcedUnit;
                newSD.owner = i.PlayerOwner;
                m_level.Spawns.Add(newSD);
            }
        }

        /*[ContextMenu("Save Level")]
        public void SaveLevel()
        {
            
        }*/

        public LevelDefinition LevelData
        {
            get
            {
                return m_level;
            }
        }
    }

}
