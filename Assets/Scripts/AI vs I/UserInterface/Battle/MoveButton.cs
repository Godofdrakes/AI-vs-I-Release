using System.Collections.Generic;
using AI_vs_I.Units;
using UnityEngine;
using UnityEngine.EventSystems;


namespace AI_vs_I.UserInterface {

    [ RequireComponent( typeof( GGObject ) ) ]
    public class MoveButton : MonoBehaviour {

        [SerializeField]
        private Sprite eligableSprite;
        [SerializeField]
        private Sprite ineligableSprite;

        private bool isDisplay = false;

        private GGObject m_ggObject = null;

        public GGObject GGObject {
            get { return m_ggObject; }
        }

        private void Reset() {
            m_ggObject = GetComponent<GGObject>();
            m_ggObject.occupiesCell = false;
        }

        void Update()
        {
            if (!IsDisplay)
            {
                GetComponent<SpriteRenderer>().color = Color.white;
                GetComponent<SpriteRenderer>().sprite = eligableSprite;
            }
            else
            {
                GetComponent<SpriteRenderer>().color = Color.white;
                //GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 0.5f);
                GetComponent<SpriteRenderer>().sprite = ineligableSprite;
            }
        }

        public bool IsDisplay
        {
            get
            {
                return isDisplay;
            }

            set
            {
                isDisplay = value;
            }
        }

    }

}
