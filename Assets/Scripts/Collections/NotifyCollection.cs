using System;
using System.Collections.ObjectModel;
using System.Linq;


namespace Collections {

    public class NotifyCollection<T> : Collection<T> {

        protected override void ClearItems() {
            T[] items = this.ToArray();
            base.ClearItems();
            ElementsRemoved( items );
        }

        protected override void InsertItem( int index, T item ) {
            base.InsertItem( index, item );
            ElementsAdded( item );
        }

        protected override void RemoveItem( int index ) {
            T item = this[index];
            base.RemoveItem( index );
            ElementsRemoved( item );
        }

        protected override void SetItem( int index, T item ) {
            base.SetItem( index, item );
            ElementsSet( item );
        }

        public event Action<T[]> OnElementsAdded;

        public event Action<T[]> OnElementsRemoved;

        public event Action<T[]> OnElementsSet;

        protected virtual void ElementsAdded( params T[] obj ) {
            if( OnElementsAdded != null ) { OnElementsAdded( obj ); }
        }

        protected virtual void ElementsRemoved( params T[] obj ) {
            if( OnElementsRemoved != null ) { OnElementsRemoved( obj ); }
        }

        protected virtual void ElementsSet( params T[] obj ) {
            if( OnElementsSet != null ) { OnElementsSet( obj ); }
        }

    }

}
