using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperwave.LabelEditor.DataModel
{
    class EditUndoItem : UndoItem
    {
        public EditUndoItem(LabelItem item,string old_text,string new_text)
        {
            Item = item;
            OldText = old_text;
            NewText = new_text;
        }

        public LabelItem Item { get; private set; }
        public string NewText { get; private set; }
        public string OldText { get; private set; }

        public override UndoType Type
        {
            get
            {
                return UndoType.Edit;
            }
        }

        public override void Redo(IList<LabelItem> items)
        {
            Item.Name = NewText;
        }

        public override void Undo(IList<LabelItem> items)
        {
            Item.Name = OldText;
        }
    }
}
