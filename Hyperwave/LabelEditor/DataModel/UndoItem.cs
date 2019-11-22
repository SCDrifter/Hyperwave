using Hyperwave.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hyperwave.LabelEditor.DataModel
{
    abstract class UndoItem : UIObject
    {
        public abstract UndoType Type { get; }
        public abstract void Undo(IList<LabelItem> items);
        public abstract void Redo(IList<LabelItem> items);
    }

    enum UndoType
    {
        Add,
        Delete,
        Edit
    }
}
