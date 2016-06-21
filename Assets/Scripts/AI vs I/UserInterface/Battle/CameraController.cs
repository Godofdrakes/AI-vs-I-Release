using System.Collections.Generic;
using AI_vs_I.Units;
using UnityEngine;
using UnityEngine.EventSystems;
//using SmallTools;


namespace AI_vs_I.UserInterface
{

    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {

        private Camera m_camera;

        [SerializeField]
        private float m_sizeBase = 5;

        [SerializeField]
        private float m_zoom = 1;

        [SerializeField]
        private float m_ease = 4;

        [SerializeField]
        private Vector2 m_target;
        
        void Start()
        {
            m_camera = GetComponent<Camera>();
        }
        
        void Update()
        {
            m_camera.orthographicSize = m_sizeBase / m_zoom;

            Vector2 targetDif = Target - Position;
            Position += targetDif / m_ease;
        }


        

        public Vector2 Position
        {
            get
            {
                return (Vector2)transform.position;
            }
            set
            {
                transform.position = new Vector3(value.x, value.y, -3);
            }
        }

        public Vector2 Target
        {
            get { return m_target; }
            set
            {
                m_target.x = Mathf.Clamp(value.x, FindObjectOfType<GGGrid>().MinPoint2D.x, FindObjectOfType<GGGrid>().MaxPoint2D.x);
                m_target.y = Mathf.Clamp(value.y, FindObjectOfType<GGGrid>().MinPoint2D.y, FindObjectOfType<GGGrid>().MaxPoint2D.y);
            }
        }
    }

}
