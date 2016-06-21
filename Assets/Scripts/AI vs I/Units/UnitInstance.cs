using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AI_vs_I.Alterations;
using AI_vs_I.Player;
using Newtonsoft.Json;
using UnityEngine;


namespace AI_vs_I.Units {

    [ RequireComponent( typeof( SpriteRenderer ), typeof( GGObject ) ),
      JsonObject( MemberSerialization.OptIn ), Serializable ]
    public class UnitInstance : ITakeDamage {
        #region Creation

        public UnitInstance( Players owner, UnitDefinition definition ) {
            m_owner = owner;
            m_definition = definition;
            m_body = new List<UnitBody>();
            m_moveHistory = new List<GGCell>();
            m_bodyHistory = new List<GGCell>();
            m_alterations = new AlterationContainer();
        }

        #endregion


        #region ITakeDamage

        public void TakeDamage( int value ) { CurrentHealth -= value; }

        public void RecieveEffect( ActionEffect effect )
        {
            if (m_currentStatus == StatusEffect.Freeze)
                return;

            switch (effect.EffectType)
            {
                case ActionEffectType.Damage:
                    TakeDamage(effect.EffectStrength);
                    break;
                    
                case ActionEffectType.HPBoost:
                    m_alterations.Add(new MaxHealthAlteration(effect.EffectDuration, effect.EffectStrength));
                    break;

                case ActionEffectType.SpeedBoost:
                    m_alterations.Add(new MovementAlteration(effect.EffectDuration, effect.EffectStrength));
                    break;


                case ActionEffectType.Corrupt:
                    m_currentStatus = StatusEffect.Corrupt;
                    m_statusDuration = effect.EffectDuration;
                    break;

                case ActionEffectType.Freeze:
                    m_currentStatus = StatusEffect.Freeze;
                    m_statusDuration = effect.EffectDuration;
                    break;
            }
            DrawBody();
        }

        #endregion


        public override string ToString() {
            return string.Format( "#UnitInstance# Health {0}/{1}, Body {2}",
                                  CurrentHealth,
                                  MaxHealth,
                                  m_body.Count );
        }


        #region Functions
        // Also an Updates Body
        public void DrawBody() {
            if (CurrentHealth > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }

            List<UnitBody> body = Body.Where( unitBody=>unitBody.gameObject.activeSelf ).ToList();

            // Get move history in reverse (newest to oldest)
            List<GGCell> reverseHistory = MoveHistory.Reverse()
                                                     .ToList();

            // Move active parts into position
            for( int index = 0; index < body.Count && index < reverseHistory.Count; index++ ) {
                // Update body
                // Draw Body
                if (body[index].CoreRenderer.gameObject.activeInHierarchy)
                {
                    body[index].CoreAnimator.runtimeAnimatorController = Definition.CoreAnimation;
                }
                body[index].SpriteRenderer.color = UnitColor;
                body[index].CoreRenderer.color = UnitColor;

                body[index].CoreRenderer.gameObject.SetActive(index == 0);
                
                body[index].transform.position = reverseHistory[index].CenterPoint2D;
                body[index].GGObject.UpdateCell();
                
            }

            foreach (UnitBody index in Body.Where(bod => bod.gameObject.activeSelf == true).ToList())
            {
                index.SpriteRenderer.sprite = Definition.BodySprites.Sprites[GetBodyLink(index)];
            }
            
            BuildBodyHistory();
        }
        private void BuildBodyHistory()
        {
            m_bodyHistory.Clear();
            List<GGCell> reverseHistory = MoveHistory.Reverse()
                                                     .ToList();
            for (int index = 0; index < CurrentHealth && index < reverseHistory.Count; index++)
            {
                m_bodyHistory.Add(reverseHistory[index]);
            }
        }

        public int GetBodyLink(UnitBody body)
        {
            body.GGObject.Update();
            body.GGObject.UpdateCell();

            int retVal = 0;
            GGDirection[] directions = new GGDirection[4] { GGDirection.Up, GGDirection.Right, GGDirection.Down, GGDirection.Left };
            for (int i = 0; i < 4; i++)
            {
                GGCell targetf = body.GGObject.Cell.GetCellInDirection(directions[i]);
                if (targetf != null)
                {
                    if (targetf.IsOccupied && targetf.GetObjectsOfType<UnitBody>().FirstOrDefault() != null)
                    {
                        if (targetf.GetObjectsOfType<UnitBody>().FirstOrDefault().UnitInstance == body.UnitInstance)
                        {
                            switch (i)
                            {
                                case 0:
                                    retVal += 1;
                                    break;
                                case 1:
                                    retVal += 2;
                                    break;
                                case 2:
                                    retVal += 4;
                                    break;
                                case 3:
                                    retVal += 8;
                                    break;
                            }
                        }
                    }
                }
            }
            return retVal;
        }

        public void Spawn( GGCell destination ) {
            Debug.Log("UnitInstance, Spawn.");
            m_moveHistory.Add( destination );
            //m_bodyHistory.Add(MoveHistory[0]);
            m_currentHealth = 1;
            DrawBody();
            //BuildBodyHistory();
            Body.FirstOrDefault().RandomAnimate();
        }

        public void Move( GGCell destination ) {
            if( m_moveHistory.Contains( destination ) ) {
                //Debug.Log("Dest had History");
                m_moveHistory.Remove( destination );
            }
            m_moveHistory.Add( destination );

            if( !m_bodyHistory.Contains( destination ) && m_currentStatus != StatusEffect.Corrupt ) {
                CurrentHealth++;
                //DrawBody();
            }
            /*else {
                DrawBody();
            }*/

            DrawBody();
        }
        
        public static int GetDistanceBetweenCells( GGCell start, GGCell end ) {
            int lat = Mathf.Abs( start.GridX - end.GridX );
            int lon = Mathf.Abs( start.GridY - end.GridY );
            return lat + lon;
        }

        public void TurnHasPassed() {
            foreach( IStatAlteration alteration in m_alterations ) {
                if( alteration.Permanent ) {
                    continue;
                }

                alteration.TurnsRemaining -= 1;
            }

            m_alterations.RemoveAll(
                                    alteration=>
                                    alteration.Permanent == false && alteration.TurnsRemaining < 1 );

            if (m_statusDuration <= 1)
            {
                m_currentStatus = StatusEffect.Normal;
            }
            if (m_currentStatus != StatusEffect.Normal)
            {
                m_statusDuration--;
                
            }
            if (m_currentStatus == StatusEffect.Freeze)
            {
                IsExausted = true;
            }

            DrawBody();
        }

        public UnitRevertState BuildRevertState() {
            return new UnitRevertState( m_body, m_currentHealth, m_moveHistory );
        }

        public void RevertTo( UnitRevertState revert ) {
            m_body = revert.m_body;
            m_currentHealth = revert.m_currentHealth;
            m_moveHistory = revert.m_moveHistory;
            DrawBody();
        }

        
        #endregion


        #region JSON

        public static UnitInstance NewFromJson( string json ) {
            UnitInstance instance = new UnitInstance( Players.None, null );
            JsonConvert.PopulateObject( json, instance );
            return instance;
        }

        public string ToJson( Formatting formatting = Formatting.None ) {
            return JsonConvert.SerializeObject( this, formatting );
        }

        #endregion


        #region Inspector Fields

        [ SerializeField, JsonProperty ]
        private UnitDefinition m_definition;

        [ SerializeField, JsonProperty ]
        private Color m_unitColor = Color.white;

        [ SerializeField, JsonProperty ]
        private Players m_owner;

        #endregion


        #region Runtime Fields
        
        private List<UnitBody> m_body;

        private AlterationContainer m_alterations;

        [SerializeField]
        private StatusEffect m_currentStatus;
        [SerializeField]
        private int m_statusDuration = 0;

        [ SerializeField ]
        private int m_currentHealth;

        private List<GGCell> m_moveHistory = new List<GGCell>();
        private List<GGCell> m_bodyHistory = new List<GGCell>();

        [ SerializeField ]
        private bool m_isExausted;

        /*[SerializeField]
        private UnitBodySprites m_bodySprites;*/

        #endregion


        #region Properties

        public int MaxHealth {
            get {
                return m_definition.MaxHealth
                             +
                             m_alterations.MaxHealthAlterations.Sum(alteration => alteration.Value);
            }
        }

        public int MaxMove {
            get {
                return m_definition.Movement + m_alterations.MovementAlterations.Sum(alteration => alteration.Value);
            }
        }

        public List<UnitBody> Body {
            get {
                if( MaxHealth > m_body.Count ) {
                    int missingBody = MaxHealth - m_body.Count;
                    //Debug.Log( this );
                    //Debug.LogFormat( "Creating {0} new body parts.", missingBody );
                    for( int index = 0; index < missingBody; index++ ) {
                        m_body.Add(
                                   UnitBody.CreateNewUnitBody(
                                                              UnitController.Instance.UnitBodyPrefab,
                                                              this ) );
                    }
                }

                for( int index = 0; index < m_body.Count; index++ ) {
                    m_body[index].gameObject.SetActive( index < CurrentHealth );
                }

                return m_body;
            }
        }

        public UnitDefinition Definition {
            get { return m_definition; }
            set { m_definition = value; }
        }

        public Players Owner {
            get { return m_owner; }
            set { m_owner = value; }
        }

        public StatusEffect CurrentStatus
        {
            get
            {
                return m_currentStatus;
            }
        }
        public int StatusDuration
        {
            get
            {
                return m_statusDuration;
            }
        }

        public int CurrentHealth {
            get { return m_currentHealth; }
            private set {
                m_currentHealth = Mathf.Clamp( value, 0, MaxHealth );
                //BuildBodyHistory();
                DrawBody();
            }
        }

        public bool IsDead {
            get { return m_currentHealth < 1; }
        }

        public GGObject Head {
            get { return !IsDead ? Body[0].GGObject : null; }
        }

        public ReadOnlyCollection<GGCell> MoveHistory {
            get { return m_moveHistory.AsReadOnly(); }
        }

        public ReadOnlyCollection<GGCell> BodyHistory {
            get { return m_moveHistory.AsReadOnly(); }
        }

        public bool IsExausted {
            get {
                if( IsDead ) { m_isExausted = true; }
                return m_isExausted;
            }
            set { m_isExausted = value; }
        }

        public Color UnitColor {
            get { return m_unitColor; }
            set { m_unitColor = value; }
        }

        #endregion
    }

}
