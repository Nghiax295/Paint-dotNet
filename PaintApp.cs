using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Paint_App_102230308
{
    public partial class PaintApp: Form
    {
        private UndoRedoManager UndoRedo = new UndoRedoManager();
        private bool isDrawing = false;
        private Point previousPoint;
        private Point startPoint;
        private Point endPoint;
        private Graphics graphics;
        private Pen drawingPen = new Pen(Color.Black, 1);
        private Pen penEraser = new Pen(Color.White, 1);
        private Color selectedColor = Color.Black;
        private int penSize = 0;
        private string selectedTool = "Pen";
        private Shape shape = new Shape();
        private Bitmap copiedBitmap;
        private Bitmap bitmap;

        public PaintApp()
        {
            InitializeComponent();
            ChangeColor_Click();
            UndoRedo_Click();
            this.Resize += PaintAppResize;

            bitmap = new Bitmap(pictureBoxShowImage.Width, pictureBoxShowImage.Height);
            graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White);
            pictureBoxShowImage.Image = bitmap;
            trackBarSizePen.Value = penSize;

            pictureBoxShowImage.Paint += pictureBoxShowImage_Paint;
            newWorks();
        }

        private void newWorks()
        {
            graphics.Clear(Color.White);
            trackBarZoom.Value = 4;
            pictureBoxShowImage.Width = 700;
            pictureBoxShowImage.Height = 400;
            pictureBoxShowImage.Invalidate();
            CenterPictureBox();
        }

        private void openWorks()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
            openFileDialog1.Title = "Chọn hình ảnh để mở";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog1.FileName;
                bitmap = new Bitmap(filePath);
                graphics = Graphics.FromImage(bitmap);
                pictureBoxShowImage.Image = bitmap;
                pictureBoxShowImage.Visible = true;
            }
        }

        private void saveWorks()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PNG Image|*.png|JPEG Image|*.jpg|Bitmap Image|*.bmp";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                bitmap.Save(saveFileDialog.FileName);
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(UndoRedo.canUndo())
            {
                DialogResult result = MessageBox.Show("Do you want to save your works?",
                     "Notification",
                     MessageBoxButtons.YesNoCancel,
                     MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    saveWorks();
                    newWorks();
                }
                else if (result == DialogResult.No)
                {
                    newWorks();
                }
            }
            else
            {
                newWorks();
            }        
        }

        private void buttonNewfile_Click(object sender, EventArgs e)
        {
            if (UndoRedo.canUndo())
            {
                DialogResult result = MessageBox.Show("Do you want to save your works?",
                     "Notification",
                     MessageBoxButtons.YesNoCancel,
                     MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    saveWorks();
                    newWorks();
                }
                else if (result == DialogResult.No)
                {
                    newWorks();
                }
            }
            else
            {
                newWorks();
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UndoRedo.canUndo())
            {
                DialogResult result = MessageBox.Show("Do you want to save your works?",
                     "Notification",
                     MessageBoxButtons.YesNoCancel,
                     MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    saveWorks();
                    openWorks();
                }
                else if (result == DialogResult.No)
                {
                    newWorks();
                    openWorks();
                }
            } 
            else
            {
                openWorks();
            }
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            if (UndoRedo.canUndo())
            {
                DialogResult result = MessageBox.Show("Do you want to save your works?",
                     "Notification",
                     MessageBoxButtons.YesNoCancel,
                     MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    saveWorks();
                    openWorks();
                }
                else if (result == DialogResult.No)
                {
                    newWorks();
                    openWorks();
                }
            }
            else
            {
                openWorks();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveWorks();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            saveWorks();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UndoRedo.canUndo())
            {
                DialogResult result = MessageBox.Show("Do you want to save your works?",
                     "Notification",
                     MessageBoxButtons.YesNoCancel,
                     MessageBoxIcon.Question);
                if(result == DialogResult.Yes)
                {
                    saveWorks();
                    Application.Exit();
                } else if (result == DialogResult.No)
                {
                    Application.Exit();
                }
            }
        }

        private void buttonLine_Click(object sender, EventArgs e)
        {
            selectedTool = "Line";
        }

        private void buttonDiamond_Click(object sender, EventArgs e)
        {
            selectedTool = "Diamond";
        }

        private void buttonCircle_Click(object sender, EventArgs e)
        {
            selectedTool = "Circle";
        }

        private void buttonSquare_Click(object sender, EventArgs e)
        {
            selectedTool = "Square";
        }

        private void buttonTriangle_Click(object sender, EventArgs e)
        {
            selectedTool = "Triangle";
        }

        private void buttonRectangle_Click(object sender, EventArgs e)
        {
            selectedTool = "Rectangle";
        }

        private void buttonPen_Click(object sender, EventArgs e)
        {
            selectedTool = "Pen";
        }

        private void buttonEraser_Click(object sender, EventArgs e)
        {
            selectedTool = "Eraser";
        }

        private void buttonFillShape_Click(object sender, EventArgs e)
        {
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                using (SolidBrush brush = new SolidBrush(selectedColor))
                {
                    if (selectedTool == "Rectangle")
                    {
                        g.FillRectangle(brush, shape.GetRectangle(startPoint, endPoint));
                    }
                    else if (selectedTool == "Circle")
                    {
                        g.FillEllipse(brush, shape.GetRectangle(startPoint, endPoint));
                    }
                    else if (selectedTool == "Triangle")
                    {
                        Point[] points = shape.GetTrianglePoints(startPoint, endPoint);
                        g.FillPolygon(brush, points);
                    }
                    else if (selectedTool == "Square")
                    {
                        Rectangle rect = shape.GetSquare(startPoint, endPoint);
                        g.FillRectangle(brush, rect);
                    }
                    else if( selectedTool == "Diamond")
                    {
                        Point[] diamond = shape.GetDiamondPoints(startPoint, endPoint);
                        g.FillPolygon(brush, diamond);
                    }
                }
            }
            pictureBoxShowImage.Invalidate();
        }

        private void ChangeColor_Click()
        {
            pictureBox1.Click += PictureBoxColor_Click;
            pictureBox2.Click += PictureBoxColor_Click;
            pictureBox3.Click += PictureBoxColor_Click;
            pictureBox4.Click += PictureBoxColor_Click;
            pictureBox5.Click += PictureBoxColor_Click;
            pictureBox6.Click += PictureBoxColor_Click;
            pictureBox7.Click += PictureBoxColor_Click;
            pictureBox8.Click += PictureBoxColor_Click;
            pictureBox9.Click += PictureBoxColor_Click;
            pictureBox10.Click += PictureBoxColor_Click;
        }

        private void PictureBoxColor_Click(object sender, EventArgs e)
        {
            PictureBox clickedPictureBox = sender as PictureBox;

            if (clickedPictureBox != null)
            {
                selectedColor = clickedPictureBox.BackColor;
                drawingPen.Color = selectedColor;
                pictureBoxColorSelection.BackColor = selectedColor;
            }
        }

        private void pictureBoxColorSelection_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                selectedColor = colorDialog.Color;
                drawingPen.Color = selectedColor;
                pictureBoxColorSelection.BackColor = selectedColor;
            }
        }

        private void trackBarSizePen_Scroll(object sender, EventArgs e)
        {
            penSize = (trackBarSizePen.Value) * 5;
            drawingPen.Width = penSize;
        }

        private void trackBarZoom_Scroll(object sender, EventArgs e)
        {
            double zoomLevel = (double)trackBarZoom.Value / 5 + 0.2;
            pictureBoxShowImage.Width = (int)(700 * zoomLevel);
            pictureBoxShowImage.Height = (int)(400 * zoomLevel);
            pictureBoxShowImage.SizeMode = PictureBoxSizeMode.StretchImage;
            CenterPictureBox();
        }

        private void pictureBoxShowImage_MouseDown(object sender, MouseEventArgs e)
        {
            isDrawing = true;
            startPoint = realLocation(e.Location);
            previousPoint = startPoint;
            saveState();
        }

        private void pictureBoxShowImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                if (selectedTool == "Pen")
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.DrawLine(drawingPen, previousPoint, realLocation(e.Location));
                    }
                    previousPoint = realLocation(e.Location);
                }
                else if (selectedTool == "Eraser")
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        penEraser.Width = penSize;
                        g.DrawLine(penEraser, previousPoint, realLocation(e.Location));
                    }
                    previousPoint = realLocation(e.Location);
                }
                endPoint = e.Location;
                pictureBoxShowImage.Invalidate();
            }
        }

        private void pictureBoxShowImage_MouseUp(object sender, MouseEventArgs e)
        {
            isDrawing = false;
            endPoint = realLocation(e.Location);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                if (selectedTool == "Line")
                {
                    g.DrawLine(drawingPen, startPoint, endPoint);
                }
                else if (selectedTool == "Rectangle")
                {
                    g.DrawRectangle(drawingPen, shape.GetRectangle(startPoint, endPoint));
                }
                else if (selectedTool == "Circle")
                {
                    g.DrawEllipse(drawingPen, shape.GetRectangle(startPoint, endPoint));
                }
                else if (selectedTool == "Triangle")
                {
                    Point[] points = shape.GetTrianglePoints(startPoint, endPoint);
                    g.DrawPolygon(drawingPen, points);
                }
                else if (selectedTool == "Square")
                {
                    Rectangle rect = shape.GetSquare(startPoint, endPoint);
                    g.DrawRectangle(drawingPen, rect);
                }
                else if (selectedTool == "Diamond")
                {
                    Point[] points = shape.GetDiamondPoints(startPoint, endPoint);
                    g.DrawPolygon(drawingPen, points);
                }
                saveState();
            pictureBoxShowImage.Invalidate();
            }
        }
         
        private void CenterPictureBox()
        {
            int containerWidth = this.ClientSize.Width;
            int containerHeight = this.ClientSize.Height;

            int newX = (containerWidth - pictureBoxShowImage.Width) / 2;
            int newY = 150;

            pictureBoxShowImage.Location = new Point(newX, newY);
        }

        private void PaintAppResize(object sender, EventArgs e)
        {
            CenterPictureBox();
        }

        private void buttonUndo_Click(object sender, EventArgs e)
        {
            if (UndoRedo.canUndo())
            {
                bitmap = UndoRedo.Undo(bitmap);
                pictureBoxShowImage.Image = bitmap;
                graphics = Graphics.FromImage(bitmap);
                pictureBoxShowImage.Invalidate();
            }
        }

        private void buttonRedo_Click(object sender, EventArgs e)
        {
            if (UndoRedo.canRedo())
            {
                bitmap = UndoRedo.Redo(bitmap);
                pictureBoxShowImage.Image = bitmap;
                graphics = Graphics.FromImage(bitmap);
                pictureBoxShowImage.Invalidate();
            }
        }

        private void UndoRedo_Click()
        {
            buttonUndo.Click += buttonUndo_Click;
            buttonRedo.Click += buttonRedo_Click;
        }

        private Point realLocation(Point mousePoint)
        {
            double zoomX = (double)pictureBoxShowImage.Width / bitmap.Width;
            double zoomY = (double)pictureBoxShowImage.Height / bitmap.Height;

            int x = (int)(mousePoint.X / zoomX);
            int y = (int)(mousePoint.Y / zoomY);
            return new Point(x, y);
        }

        private void saveState()
        {
            UndoRedo.SaveState(bitmap);
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveState();
            copiedBitmap = new Bitmap(bitmap);
            graphics.Clear(Color.White);
            pictureBoxShowImage.Invalidate();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(bitmap != null)
            {
                copiedBitmap = new Bitmap(bitmap);
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(copiedBitmap != null)
            {
                saveState();
                graphics.DrawImage(copiedBitmap, new Point(0, 0));
                pictureBoxShowImage.Invalidate();
            }
        }

        private void fullScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBoxShowImage.Dock == DockStyle.Fill)
            {
                pictureBoxShowImage.Dock = DockStyle.None;
                pictureBoxShowImage.Size = new Size(700, 400);
                panel5.Visible = true;
                CenterPictureBox();
            }
            else
            {
                panel5.Visible = false;
                pictureBoxShowImage.Dock = DockStyle.Fill;
                pictureBoxShowImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            
        }

        private void pictureBoxShowImage_Paint(object sender, PaintEventArgs e)
        {
            if (isDrawing)
            {
                Pen previewPen = new Pen(selectedColor, drawingPen.Width) { DashStyle = DashStyle.Dash };

                if (selectedTool == "Line")
                {
                    e.Graphics.DrawLine(previewPen, startPoint, endPoint);
                }
                else if (selectedTool == "Rectangle")
                {
                    e.Graphics.DrawRectangle(previewPen, shape.GetRectangle(startPoint, endPoint));
                }
                else if (selectedTool == "Circle")
                {
                    e.Graphics.DrawEllipse(previewPen, shape.GetRectangle(startPoint, endPoint));
                }
                else if (selectedTool == "Triangle")
                {
                    e.Graphics.DrawPolygon(previewPen, shape.GetTrianglePoints(startPoint, endPoint));
                }
                else if (selectedTool == "Square")
                {
                    e.Graphics.DrawRectangle(previewPen, shape.GetSquare(startPoint, endPoint));
                }
                else if (selectedTool == "Diamond")
                {
                    e.Graphics.DrawPolygon(previewPen, shape.GetDiamondPoints(startPoint, endPoint));
                }
                previewPen.Dispose();
            }
        }

    }
}
