using SmallTools;
using UnityEngine;


namespace AI_vs_I {

    [ RequireComponent( typeof( GGGrid ) ) ]
    public class GridBackground : CachedMonoBehaviour {

        public GridBGTile m_tilePrefab;

        public GGGrid GGGrid {
            get { return GetCachedComponent<GGGrid>(); }
        }

        public void Reload() {
            foreach( GridBGTile i in GetComponentsInChildren<GridBGTile>() ) {
                Destroy( i.gameObject );
            }
            foreach( GGCell i in GGGrid.Cells ) {
                GridBGTile newtile = Instantiate( m_tilePrefab );
                newtile.transform.SetParent( transform );
                newtile.SpriteRenderer.color -= new Color(0, 0, 0, 0.5f);
                newtile.LinkedCell = i;
            }
        }

        private void Start() { Reload(); }

    }

}
