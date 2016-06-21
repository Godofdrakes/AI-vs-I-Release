using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace ExtensionMethods {

    public static class GGCellEx {

        /// <summary>
        ///     Incrementaly searches outward from the target cell to find all cells in a range.
        /// </summary>
        /// <param name="ggCell">
        ///     The center of the search.
        /// </param>
        /// <param name="inclusiveRange">
        ///     The range of cells to get, zero being the the target cell.
        /// </param>
        /// <returns>
        ///     The list of found cells.
        /// </returns>
        public static List<GGCell> GetCellsInRange( this GGCell ggCell, int inclusiveRange ) {
            if( ggCell == null ) { throw new ArgumentNullException( "ggCell", "ggCell == null" ); }
            if( inclusiveRange < 0 ) {
                throw new ArgumentOutOfRangeException( "inclusiveRange",
                                                       inclusiveRange,
                                                       "inclusiveRange < 0" );
            }

            //Debug.LogFormat( "#{0}# Running grid search. Cell: {{ X = {1}, Y = {2} }}, Range: {3}",
            //                 typeof( GGCellEx ).Name,
            //                 ggCell.GridX,
            //                 ggCell.GridY,
            //                 inclusiveRange );

            IEnumerable<GGCell> foundCells;
            GGCell[] nullCell = { null }; // For easy filtering of null.

            switch( inclusiveRange ) {
                case 0 :
                    foundCells = new List<GGCell> { ggCell };
                    break;

                case 1 :
                    foundCells = new List<GGCell> {
                                                      ggCell,
                                                      ggCell.GetCellInDirection( GGDirection.Up ),
                                                      ggCell.GetCellInDirection( GGDirection.Down ),
                                                      ggCell.GetCellInDirection( GGDirection.Left ),
                                                      ggCell.GetCellInDirection( GGDirection.Right )
                                                  };
                    break;

                default :
                    Profiler.BeginSample( "Cell Range Search", ggCell.Grid );

                    HashSet<GGCell> foundSet = new HashSet<GGCell>();
                    HashSet<GGCell> workingSet = new HashSet<GGCell> { ggCell };

                    for( int index = 0; index <= inclusiveRange; index++ ) {
                        HashSet<GGCell> todoSet = new HashSet<GGCell>();
                        foreach( GGCell workingCell in workingSet ) {
                            if( workingCell == null ) {
                                continue;
                            }

                            int difX = Math.Abs( ggCell.GridX - workingCell.GridX );
                            int difY = Math.Abs( ggCell.GridY - workingCell.GridY );
                            if( difX + difY <= inclusiveRange ) { foundSet.Add( workingCell ); }
                            todoSet.Add( workingCell.GetCellInDirection( GGDirection.Up ) );
                            todoSet.Add( workingCell.GetCellInDirection( GGDirection.Down ) );
                            todoSet.Add( workingCell.GetCellInDirection( GGDirection.Left ) );
                            todoSet.Add( workingCell.GetCellInDirection( GGDirection.Right ) );
                        }

                        workingSet = todoSet;
                    }

                    foundCells = foundSet;

                    Profiler.EndSample();
                    break;
            }

            return foundCells.Except( nullCell ).ToList();
        }

        /// <summary>
        ///     Searches for <see cref="GGObject" />s that have the desired <see cref="MonoBehaviour" /> attached in the cell.
        /// </summary>
        /// <typeparam name="T">
        ///     The <see cref="MonoBehaviour" /> to search for.
        /// </typeparam>
        /// <param name="cell">
        ///     The target of the search.
        /// </param>
        /// <returns>
        ///     The list of <see cref="MonoBehaviour" />s found.
        /// </returns>
        public static T[] GetTypesInCell<T>( this GGCell cell ) where T : MonoBehaviour {
            return cell.Objects.Select( o=>o.GetComponent<T>() )
                       .Where( arg1=>arg1 != null )
                       .ToArray();
        }

    }

}
