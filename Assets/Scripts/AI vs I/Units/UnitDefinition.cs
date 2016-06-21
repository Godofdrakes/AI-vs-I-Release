using System;
using System.Linq;
using AI_vs_I.Modules;
using Newtonsoft.Json;
using UnityEngine;


namespace AI_vs_I.Units {

    [ Serializable, JsonObject( MemberSerialization.OptIn ) ]
    public class UnitDefinition {
        #region Constructors

        public UnitDefinition() {
            m_name = string.Empty;
            m_artBundle = null;
            //m_coreSprite = null;
            //m_coreAnimation = null;
            //m_bodySprite = null;
            //m_bodySprites = null;
            m_modules = new BaseUnitModule[0];
            Movement = 0;
            MaxHealth = 0;
            ActionModules = new ActionModule[0];
            TotalCost = 0;
        }

        public UnitDefinition( UnitArtBundle artBundle, params BaseUnitModule[] modules ) : this() {
            m_artBundle = artBundle;
            m_modules = modules.Clone() as BaseUnitModule[];
        }

        #endregion


        #region Inspector Fields

        [ SerializeField, JsonProperty ]
        private string m_name;

        //[ SerializeField, JsonProperty, JsonConverter( typeof( UnityResourceConverter ) ) ]
        //private Sprite m_coreSprite;

        //[ SerializeField, JsonProperty, JsonConverter( typeof( UnityResourceConverter ) ) ]
        //private RuntimeAnimatorController m_coreAnimation;

        //[ SerializeField, JsonProperty, JsonConverter( typeof( UnityResourceConverter ) ) ]
        //private Sprite m_bodySprite;

        //[ SerializeField, JsonProperty, JsonConverter( typeof( UnityResourceConverter ) ) ]
        //private UnitBodySprites m_bodySprites;

        [ SerializeField, JsonProperty ]
        private UnitArtBundle m_artBundle;

        [ SerializeField, JsonProperty ]
        private BaseUnitModule[] m_modules;

        #endregion


        #region Properties

        public string Name {
            get { return m_name; }
            set { m_name = value; }
        }

        public BaseUnitModule[] Modules {
            get { return m_modules; }
        }

        public int Movement { get; private set; }

        public int MaxHealth { get; private set; }

        public ActionModule[] ActionModules { get; private set; }

        public int TotalCost { get; private set; }

        public Sprite CoreSprite {
            get { return m_artBundle.CoreSprite; }
        }

        public RuntimeAnimatorController CoreAnimation {
            get { return m_artBundle.Animator; }
        }

        public Sprite BodySprite {
            get { return m_artBundle.BodySprite; }
        }

        public UnitBodySprites BodySprites {
            get { return m_artBundle.BodySprites; }
        }

        #endregion


        #region Functions

        /*public static UnitDefinition PlaceHolder {
            get {
                return SRResources.PlaceHolder_Assets.PlaceHolderUnitDefinition.Load()
                                  .Data;
            }
        }*/

        public ActionModule GetActionModuleInclusive( int mod ) {
            if( mod <= ActionModules.Length && mod >= 1 ) { return ActionModules[mod - 1]; }

            return null;
        }

        public void RecalculateStats() {
            TotalCost = m_modules.Sum( module=>module.ModuleCost );
            MaxHealth = m_modules.OfType<MaxHealthModule>().Sum( module=>module.MaxHealthValue );
            Movement = m_modules.OfType<MovementModule>().Sum( module=>module.MoveValue );
            ActionModules = m_modules.OfType<ActionModule>().Take( 4 ).ToArray();
        }

        #endregion


        #region Json Save/Load

        public void LoadFromJson( string json ) { JsonConvert.PopulateObject( json, this ); }

        public static UnitDefinition NewFromJson( string json ) {
            return JsonConvert.DeserializeObject<UnitDefinition>( json );
        }

        public string ToJson( Formatting formatting = Formatting.None ) {
            return JsonConvert.SerializeObject( this, formatting );
        }

        #endregion
    }

}
