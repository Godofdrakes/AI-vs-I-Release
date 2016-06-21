using System.Collections;
using System.Linq;
using SmallTools;
using UnityEngine;


namespace AI_vs_I.Units {

    [ RequireComponent( typeof( GGObject ), typeof( SpriteRenderer ) ) ]
    public class UnitBody : CachedMonoBehaviour {

        [ SerializeField ]
        private SpriteRenderer m_coreRenderer;

        private UnitInstance m_unitInstance;

        public SpriteRenderer CoreRenderer {
            get { return m_coreRenderer; }
        }

        public Animator CoreAnimator
        {
            get { return GetComponentInChildren<Animator>(); }
        }

        public UnitInstance UnitInstance {
            get { return m_unitInstance; }
        }

        public SpriteRenderer SpriteRenderer {
            get { return GetCachedComponent<SpriteRenderer>(); }
        }

        public GGObject GGObject {
            get { return GetCachedComponent<GGObject>(); }
        }

        public static UnitBody CreateNewUnitBody( UnitBody prefab,
                                                  UnitInstance isInstanceOf ) {
            UnitBody body = Instantiate( prefab );
            body.m_unitInstance = isInstanceOf;
            body.transform.SetParent( null, false );
            body.gameObject.SetActive( false );
            body.SpriteRenderer.sprite = isInstanceOf.Definition.BodySprite;
            return body;
        }

        public void RandomAnimate() {
            StopCoroutine( "CorRandomAnimate" );
            StartCoroutine( CorRandomAnimate( 2.0f, 5.0f ) );
        }

        private IEnumerator CorRandomAnimate( float minTime, float maxTime ) {
            Animator coreAnimator = m_coreRenderer.GetComponent<Animator>();
            // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
            while( coreAnimator != null ) {
                if( coreAnimator.gameObject.activeSelf ) {
                    m_coreRenderer.GetComponent<Animator>().SetTrigger( "animate" );
                }
                yield return new WaitForSeconds( Random.Range( minTime, maxTime ) );
            }
        }

        private void OnDisable() { StopAllCoroutines(); }

        //private void OnEnable() { RandomAnimate(); }

        private void Reset() {
            m_coreRenderer =
                GetComponentsInChildren<SpriteRenderer>()
                    .FirstOrDefault( spriteRenderer=>spriteRenderer.gameObject != gameObject );
        }

    }
    

}
