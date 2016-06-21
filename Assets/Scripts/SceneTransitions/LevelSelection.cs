using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SmallTools;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = TypeSafe.Scene;
using Newtonsoft.Json;

using AI_vs_I.Levels;

namespace SceneTransitions
{
    [JsonObject( MemberSerialization = MemberSerialization.OptIn )]
    public class LevelSelection : Singleton<LevelSelection>
    {
        [SerializeField]
        private LevelEditor m_selectedLevel;
        
        /*[SerializeField, JsonProperty]
        private List<LevelEditor> m_levels;*/

        public LevelDefinition SelectedLevel
        {
            get
            {
                return m_selectedLevel.LevelData;
            }
        }

        public void SetSelectedLevel(LevelEditor level)
        {
            m_selectedLevel = level;
        }

        /*public LevelKey? FindKey(string name)
        {
            LevelKey? level = null;

            foreach (LevelKey i in m_levels)
            {
                if (i.name == name)
                {
                    level = i;
                    break;
                }
            }
            return level;
        }*/

        /*public LevelDefinition GetLevel(string name)
        {
            foreach (LevelEditor i in m_levels)
            {
                if (i.gameObject.name == name)
                {
                    return i.LevelData;
                }
            }
            return null;
        }*/

        /*public void AddNewLevel(LevelKey levelKey)
        {
            m_levels.Add(levelKey);
        }*/

        /*public string SelectedLevel
        {
            get
            {
                return m_selectedLevel;
            }
            set
            {
                m_selectedLevel = value;
            }
        }*/
        
    }

}