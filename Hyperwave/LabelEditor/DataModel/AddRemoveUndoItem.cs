using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperwave.LabelEditor.DataModel
{
    abstract class AddRemoveUndoItem : UndoItem
    {

        public AddRemoveUndoItem(LabelItem item,int index)
        {
            IndexHint = index;
            Item = item;
        }

        public LabelItem Item { get; private set; }
        public int IndexHint { get; private set; }   
        
    }

    class AddUndoItem : AddRemoveUndoItem
    {
        public AddUndoItem(LabelItem item, int index)
            :base(item,index)
        {
        }

        public override UndoType Type
        {
            get
            {
                return UndoType.Add;
            }
        }

        public override void Redo(IList<LabelItem> items)
        {
            items.Insert(IndexHint, Item);
        }

        public override void Undo(IList<LabelItem> items)
        {
            items.Remove(Item);
        }
    }

    class RemoveUndoItem : AddRemoveUndoItem
    {
        public RemoveUndoItem(LabelItem item, int index)
            : base(item, index)
        {
        }

        public override UndoType Type
        {
            get
            {
                return UndoType.Delete;
            }
        }

        public override void Redo(IList<LabelItem> items)
        {
            items.Remove(Item);
        }

        public override void Undo(IList<LabelItem> items)
        {
            items.Insert(IndexHint, Item);
        }
    }
}
