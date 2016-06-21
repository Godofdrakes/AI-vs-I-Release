using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using AI_vs_I.Modules;
using Newtonsoft.Json;


namespace AI_vs_I.Units {

    [ Serializable, JsonObject( MemberSerialization.OptIn ) ]
    public struct UnitStats {

        [ JsonProperty ]
        public int cost;

        [ JsonProperty ]
        public int health;

        [ JsonProperty ]
        public int move;

        [ JsonProperty ]
        public List<ActionModule> actions;

        public override string ToString() {
            return string.Format( "UnitStats {{ Cost: {0}, Health: {1}, Move {2}, Actions: {3} }}",
                                  cost,
                                  health,
                                  move,
                                  actions.Count );
        }

        public static UnitStats FromModules( params BaseUnitModule[] modules ) {
            UnitStats stats = new UnitStats();
            stats.actions = modules.OfType<ActionModule>().ToList();
            stats.cost = modules.Sum( module=>module.ModuleCost );
            stats.health = modules.OfType<MaxHealthModule>().Sum( module=>module.MaxHealthValue );
            stats.move = modules.OfType<MovementModule>().Sum( module=>module.MoveValue );
            return stats;
        }

    }

    /// <summary>
    /// Container for 
    /// </summary>
    [ Serializable, JsonObject( MemberSerialization.OptIn ) ]
    public struct UnitMods {

        [ JsonProperty ]
        private BaseUnitModule[] m_modules;

        private int m_cost;

        private int m_health;

        private int m_move;

        private ActionModule[] m_actions;

        public BaseUnitModule[] Modules {
            get { return m_modules; }
        }

        /// <summary>
        ///     The combined cost of the contained mods.
        /// </summary>
        public int Cost {
            get { return m_cost; }
        }

        /// <summary>
        ///     The combined health of the contained mods.
        /// </summary>
        public int Health {
            get { return m_health; }
        }

        /// <summary>
        ///     The combined move of the contained mods.
        /// </summary>
        public int Move {
            get { return m_move; }
        }

        /// <summary>
        ///     The available actions of the contained mods.
        /// </summary>
        public ActionModule[] Actions {
            get { return m_actions; }
        }

        public UnitMods( params BaseUnitModule[] modules ) : this() {
            m_modules = modules.Clone() as BaseUnitModule[];
            Calc();
        }

        /// <summary>
        ///     Recalculates the values based on the currently contained mods.
        /// </summary>
        [ OnDeserialized ]
        private void Calc() {
            m_cost = m_modules.Sum( module=>module.ModuleCost );
            m_health = m_modules.OfType<MaxHealthModule>().Sum( module=>module.MaxHealthValue );
            m_move = m_modules.OfType<MovementModule>().Sum( module=>module.MoveValue );
            m_actions = m_modules.OfType<ActionModule>().Take( 4 ).ToArray();
        }

    }

}
