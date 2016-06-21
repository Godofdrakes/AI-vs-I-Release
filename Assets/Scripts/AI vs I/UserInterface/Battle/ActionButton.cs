using System.Collections.Generic;
using AI_vs_I.Units;
using UnityEngine;
using UnityEngine.EventSystems;


namespace AI_vs_I.UserInterface
{

    [RequireComponent(typeof(GGObject))]
    public class ActionButton : MonoBehaviour
    {
        [SerializeField]
        private Sprite eligableSprite;
        [SerializeField]
        private Sprite ineligableSprite;

        private bool isTargetValid = false;


        private GGObject m_ggObject = null;

        public GGObject GGObject
        {
            get { return m_ggObject; }
        }

        public bool IsTargetValid
        {
            get
            {
                return isTargetValid;
            }

            set
            {
                isTargetValid = value;
            }
        }

        private void Reset()
        {
            m_ggObject = GetComponent<GGObject>();
            m_ggObject.occupiesCell = false;
        }


        void Update()
        {
            if (IsTargetValid)
            {
                GetComponent<SpriteRenderer>().color = Color.red;
                GetComponent<SpriteRenderer>().sprite = eligableSprite;
            }
            else
            {
                GetComponent<SpriteRenderer>().color = Color.red;
                //GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 0.5f);
                GetComponent<SpriteRenderer>().sprite = ineligableSprite;
            }
        }
    }

}
