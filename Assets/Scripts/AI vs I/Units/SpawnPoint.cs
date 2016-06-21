using AI_vs_I.Player;
using SmallTools;
using UnityEngine;


namespace AI_vs_I.Units {

    [ RequireComponent( typeof( GGObject ), typeof( SpriteRenderer ) ) ]
    public class SpawnPoint : CachedMonoBehaviour {

        /// <summary>
        ///     See <see cref="EnforcedUnit" />.
        /// </summary>
        [ SerializeField ]
        private UnitDefinitionObject m_enforcedUnit = null;

        /// <summary>
        ///     See <see cref="PlayerOwner" />.
        /// </summary>
        [ SerializeField ]
        private Players m_playerOwner = Players.None;

        /// <summary>
        ///     See <see cref="SelectedDefinition" /> and <see cref="EnforcedUnit" />.
        /// </summary>
        private UnitDefinition m_selectedDefinition;

        /// <summary>
        ///     The currently selected <see cref="UnitDefinition" /> or the <see cref="EnforcedUnit" /> if set.
        /// </summary>
        public UnitDefinition SelectedDefinition {
            get {
                return m_enforcedUnit != null ? m_enforcedUnit.UnitDefinition : m_selectedDefinition;
            }
            set {
                if( m_enforcedUnit != null ) return;

                m_selectedDefinition = value;
                SpriteRenderer.sprite = m_selectedDefinition.CoreSprite;
            }
        }

        public GGObject GGObject {
            get { return GetCachedComponent<GGObject>(); }
        }

        public SpriteRenderer SpriteRenderer {
            get { return GetCachedComponent<SpriteRenderer>(); }
        }

        /// <summary>
        ///     If set use of the provided unit is enforced and cannot be changed by the owner.
        /// </summary>
        public UnitDefinitionObject EnforcedUnit {
            get { return m_enforcedUnit; }
            set { m_enforcedUnit = value; }
        }

        /// <summary>
        ///     The player that owns this spawn point.
        /// </summary>
        /// <remarks>
        ///     Only the owner should be able to interact with the spawn point but anyone can view it.
        /// </remarks>
        public Players PlayerOwner {
            get { return m_playerOwner; }
            set { m_playerOwner = value; }
        }

        /*public UnitInstance SpawnInstance() {
            UnitInstance instance = null;
            //Debug.Log("SpawnPoint, Spawn Instance.");
            if( SelectedDefinition != null ) {
                instance = new UnitInstance( Players.One, SelectedDefinition );
                instance.Spawn( GGObject.Cell );
                instance.Owner = PlayerOwner;
            }
            else {
                Debug.LogWarningFormat( this,
                                        "#{0}# Tried to spawn unit but SelectedDefinition was null!",
                                        typeof( SpawnPoint ).Name );
            }
            return instance;
        }*/

        private void OnEnable() {
            if( SelectedDefinition != null ) {
                SpriteRenderer.sprite = SelectedDefinition.CoreSprite;
            }
        }

        private void Reset() { GGObject.occupiesCell = false; }

    }

}
