using System.Collections;
using UnityEngine;


namespace SmallTools {

    public class RotatingIcon : MonoBehaviour {

        public float DegPerSecond = 1.0f;

        private void OnDisable() { StopAllCoroutines(); }

        private void OnEnable() { StartCoroutine( CorRotate() ); }

        private IEnumerator CorRotate() {
            RectTransform rectTransform = GetComponent<RectTransform>();
            while( true ) {
                rectTransform.Rotate( 0.0f,
                                      0.0f,
                                      Mathf.LerpUnclamped( 0,
                                                           DegPerSecond,
                                                           Time.deltaTime ) );
                yield return null;
            }
        }

    }

}
