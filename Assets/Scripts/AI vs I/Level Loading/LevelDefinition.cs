using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AI_vs_I.Player;
using Newtonsoft.Json;
using Serializers;
using AI_vs_I.Units;

namespace AI_vs_I.Levels
{
    /*[System.Serializable, JsonObject(MemberSerialization = MemberSerialization.OptOut)]
    public struct LevelKey
    {
        public string name;
        public LevelDefinition data;
    }*/

    [System.Serializable, JsonObject( MemberSerialization = MemberSerialization.OptOut )]
    public struct SpawnPointDefinition
    {
        public Vector2 position;
        public UnitDefinitionObject enforcedUnit;
        public Players owner;
    }

    [System.Serializable, JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class LevelDefinition
    {
        [SerializeField, JsonProperty]
        private Vector2 m_gridDimensions;

        [SerializeField, JsonProperty]
        private List<Vector2> m_walls;

        [SerializeField, JsonProperty]
        private List<SpawnPointDefinition> m_spawns;


        public Vector2 GridDimensions
        {
            get
            {
                return m_gridDimensions;
            }
            set
            {
                m_gridDimensions = value;
            }
        }

        public List<Vector2> Walls
        {
            get
            {
                return m_walls;
            }
            /*set
            {
                m_walls = value;
            }*/
        }

        public List<SpawnPointDefinition> Spawns
        {
            get
            {
                return m_spawns;
            }
            /*set
            {
                m_spawns = value;
            }*/
        }

        public void Clean()
        {
            m_walls.Clear();
            m_spawns.Clear();
        }

        public string ToJson( IEnumerable<LevelDefinition> levels )
        {
            return JsonConvert.SerializeObject(levels);
        }
    }

}