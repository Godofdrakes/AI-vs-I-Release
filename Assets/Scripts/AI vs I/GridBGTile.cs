using SmallTools;
using UnityEngine;


namespace AI_vs_I {

    [ RequireComponent( typeof( SpriteRenderer ) ) ]
    public class GridBGTile : CachedMonoBehaviour {

        private GGCell m_linkedCell;

        public GGCell LinkedCell {
            get { return m_linkedCell; }
            set {
                m_linkedCell = value;
                transform.position = new Vector3( value.CenterPoint2D.x,
                                                  value.CenterPoint2D.y,
                                                  transform.position.z );
                transform.localScale = new Vector3( value.Grid.CellSize,
                                                    value.Grid.CellSize,
                                                    value.Grid.CellSize );
            }
        }

        public SpriteRenderer SpriteRenderer {
            get { return GetCachedComponent<SpriteRenderer>(); }
        }

        private void Update() { SpriteRenderer.enabled = LinkedCell.IsPathable; }

    }

}
