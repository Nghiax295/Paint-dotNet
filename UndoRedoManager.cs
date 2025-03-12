using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paint_App_102230308
{
    public class UndoRedoManager
    {
        private Stack<Bitmap> undoStack = new Stack<Bitmap>();
        private Stack<Bitmap> redoStack = new Stack<Bitmap>();

        public void SaveState(Bitmap bitmap)
        {
            undoStack.Push(new Bitmap(bitmap));
            redoStack.Clear();
        }

        public Bitmap Undo(Bitmap bitmap)
        {
            if (undoStack.Count > 0)
            {
                redoStack.Push(new Bitmap(bitmap));
                return new Bitmap(undoStack.Pop());
            }
            return bitmap;
        }

        public Bitmap Redo(Bitmap bitmap)
        {
            if (redoStack.Count > 0)
            {
                undoStack.Push(new Bitmap(bitmap));
                return new Bitmap(redoStack.Pop());
            }
            return bitmap;
        }

        public bool canRedo()
        {
            return redoStack.Count > 0;
        }

        public bool canUndo()
        {
            return undoStack.Count > 0;
        }
    }
}
