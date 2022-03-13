using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Kevin_s_2048
{
    public partial class Form1 : Form
    {
        // Recode the Block on the field
        LinkedList<Block> NowBlock = new LinkedList<Block>();
        // Recode BackBlock
        Block[,] BackBlocks;
        // Recode field's size
        Size size;
        // Recode press state
        public static bool IsPressing = false;
        // Recode ForeBlocks
        Block[,] ForeBlocks;

        LinkedList<Block>[][] ListAry;

        Timer _TimersTimer = null;
        MoveDirection Dir;
        LinkedList<Block> movelist = new LinkedList<Block>();
        LinkedList<Block>[] temL;

        public Form1()
        {
            InitializeComponent();
            // Register KeyDown event
            KeyDown += KD;
            KeyPreview = true;
            // Set BackGround's color
            BackGround.BackColor = Color.FromArgb(186, 172, 159);
            // Set field's size
            size = new Size(4, 4);
            // Create BackBlock
            BackBlocks = new Block[size.Width, size.Height];
            // Create ForeBlocks
            ForeBlocks = new Block[size.Width, size.Height];
            // Create BackBlock
            for (int i = 0, j; i < BackBlocks.GetLength(0); i++)
            {
                for (j = 0; j < BackBlocks.GetLength(1); j++)
                {
                    BackBlocks[i, j] = new Block(BackGround, 12);
                    BackBlocks[i, j].ToBackBlock();
                    BackBlocks[i, j].MoveToXY(i, j);
                    Controls.Add(BackBlocks[i, j]);
                    BackBlocks[i, j].BringToFront();
                }
            }

            ListAry = new LinkedList<Block>[2][];
            ListAry[0] = new LinkedList<Block>[size.Width];
            ListAry[1] = new LinkedList<Block>[size.Height];

            for (int i = 0, j; i < ListAry.GetLength(0); i++)
            {
                for (j = 0; j < ListAry[i].GetLength(0); j++)
                {
                    ListAry[i][j] = new LinkedList<Block>();
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CreateNumBlock();
            CreateNumBlock();
        }

        private void CreateNumBlock()
        {
            if (NowBlock.Count >= size.Width * size.Height)
                return;

            Block block;
            Random rand;
            List<int> listLinq = new List<int>(Enumerable.Range(0, size.Width * 2));
            int x, y;
            bool recover;

            do
            {
                recover = false;
                rand = new Random(Guid.NewGuid().GetHashCode());
                listLinq = listLinq.OrderBy(num => rand.Next()).ToList<int>();
                x = listLinq[0];
                x = x >= size.Width ? x - size.Width : x;
                y = listLinq[1];
                y = y >= size.Height ? y - size.Height : y;
                foreach (var item in NowBlock)
                {
                    if (x == item.Coordinate.X && y == item.Coordinate.Y)
                    {
                        recover = true;
                    }
                }
            } while (recover);

            block = new Block(BackBlocks[0, 0].BackGround, BackBlocks[0, 0].FrameWidth);
            block.ToNumBlock(BlockNum._2);
            block.MoveToXY(x, y);
            Controls.Add(block);
            block.BringToFront();
            NowBlock.AddFirst(block);
        }

        // KeyDown action
        private void KD(object sender, KeyEventArgs e)
        {
            if (IsPressing)
                return;
            IsPressing = true;

            for (int i = 0, j; i < ListAry.GetLength(0); i++)
            {
                for (j = 0; j < ListAry[i].GetLength(0); j++)
                {
                    ListAry[i][j].Clear();
                }
            }

            foreach (var item in NowBlock)
            {
                for (int i = 0; i < size.Width; i++)
                {
                    if (item.Coordinate.X == i)
                    {
                        ListAry[1][i].AddFirst(item);
                    }
                }
                for (int j = 0; j < size.Height; j++)
                {
                    if (item.Coordinate.Y == j)
                    {
                        ListAry[0][j].AddFirst(item);
                    }
                }
            }

            Block[] tem = new Block[size.Width];

            if (e.KeyCode == Keys.Left)
            {
                for (int j = 0, i, k; j < ListAry[0].GetLength(0); j++)
                {
                    if (ListAry[0][j].Count == 0)
                        continue;
                    foreach (var item in ListAry[0][j])
                    {
                        tem[item.Coordinate.X] = item;
                    }
                    for (i = 0; i < tem.GetLength(0); i++)
                    {
                        if (tem[i] == null)
                            continue;

                        tem[i].TargetCoordinate = tem[i].Coordinate;
                        if (i != 0)
                        {
                            for (k = i - 1; k >= 0; k--)
                            {
                                if (tem[k] != null)
                                {
                                    if (tem[k].Value == 0 && tem[k].Number == tem[i].Number)
                                    {
                                        tem[k].Value = 1;
                                        tem[i].Value = 2;
                                        tem[i].TargetCoordinate = tem[k].TargetCoordinate;
                                        tem[i] = null;
                                        break;
                                    }
                                    else
                                        break;
                                }
                            }
                            if (tem[i] != null)
                            {
                                for (k = 0; k < i; k++)
                                {
                                    if (tem[k] == null)
                                    {
                                        tem[i].TargetCoordinate = new Point(k, tem[i].Coordinate.Y);
                                        tem[k] = tem[i];
                                        tem[i] = null;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    foreach (var item in ListAry[0][j])
                    {
                        item.Speed = Math.Abs(item.TargetCoordinate.X - item.Coordinate.X) * 10;
                        if (item.Speed > 0)
                            movelist.AddLast(item);
                    }
                    for (i = 0; i < tem.GetLength(0); i++)
                        tem[i] = null;
                }
                temL = ListAry[0];
                MoveAction(MoveDirection.Left);
            }
            else if (e.KeyCode == Keys.Right)
            {
                int max = tem.GetUpperBound(0);
                for (int j = 0, i, k; j < ListAry[0].GetLength(0); j++)
                {
                    if (ListAry[0][j].Count == 0)
                        continue;
                    foreach (var item in ListAry[0][j])
                    {
                        tem[item.Coordinate.X] = item;
                    }

                    for (i = max; i >= 0; i--)
                    {
                        if (tem[i] == null)
                            continue;

                        tem[i].TargetCoordinate = tem[i].Coordinate;
                        if (i != max)
                        {
                            for (k = i + 1; k <= max; k++)
                            {
                                if (tem[k] != null)
                                {
                                    if (tem[k].Value == 0 && tem[k].Number == tem[i].Number)
                                    {
                                        tem[k].Value = 1;
                                        tem[i].Value = 2;
                                        tem[i].TargetCoordinate = tem[k].TargetCoordinate;
                                        tem[i] = null;
                                        break;
                                    }
                                    else
                                        break;
                                }
                            }
                            if (tem[i] != null)
                            {
                                for (k = max; k > i; k--)
                                {
                                    if (tem[k] == null)
                                    {
                                        tem[i].TargetCoordinate = new Point(k, tem[i].Coordinate.Y);
                                        tem[k] = tem[i];
                                        tem[i] = null;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    foreach (var item in ListAry[0][j])
                    {
                        item.Speed = Math.Abs(item.TargetCoordinate.X - item.Coordinate.X) * 10;
                        if (item.Speed > 0)
                            movelist.AddLast(item);
                    }
                    for (i = 0; i < tem.GetLength(0); i++)
                        tem[i] = null;
                }
                temL = ListAry[0];
                MoveAction(MoveDirection.Right);
            }
            else if (e.KeyCode == Keys.Up)
            {
                for (int j = 0, i, k; j < ListAry[1].GetLength(0); j++)
                {
                    if (ListAry[1][j].Count == 0)
                        continue;
                    foreach (var item in ListAry[1][j])
                    {
                        tem[item.Coordinate.Y] = item;
                    }
                    for (i = 0; i < tem.GetLength(0); i++)
                    {
                        if (tem[i] == null)
                            continue;

                        tem[i].TargetCoordinate = tem[i].Coordinate;
                        if (i != 0)
                        {
                            for (k = i - 1; k >= 0; k--)
                            {
                                if (tem[k] != null)
                                {
                                    if (tem[k].Value == 0 && tem[k].Number == tem[i].Number)
                                    {
                                        tem[k].Value = 1;
                                        tem[i].Value = 2;
                                        tem[i].TargetCoordinate = tem[k].TargetCoordinate;
                                        tem[i] = null;
                                        break;
                                    }
                                    else
                                        break;
                                }
                            }
                            if (tem[i] != null)
                            {
                                for (k = 0; k < i; k++)
                                {
                                    if (tem[k] == null)
                                    {
                                        tem[i].TargetCoordinate = new Point(tem[i].Coordinate.X, k);
                                        tem[k] = tem[i];
                                        tem[i] = null;
                                        break;
                                    }
                                }
                            }

                        }
                    }
                    foreach (var item in ListAry[1][j])
                    {
                        item.Speed = Math.Abs(item.TargetCoordinate.Y - item.Coordinate.Y) * 10;
                        if (item.Speed > 0)
                            movelist.AddLast(item);
                    }
                    for (i = 0; i < tem.GetLength(0); i++)
                        tem[i] = null;
                }
                temL = ListAry[1];
                MoveAction(MoveDirection.Up);
            }
            else if (e.KeyCode == Keys.Down)
            {
                int max = tem.GetUpperBound(0);
                for (int j = 0, i, k; j < ListAry[1].GetLength(0); j++)
                {
                    if (ListAry[1][j].Count == 0)
                        continue;
                    foreach (var item in ListAry[1][j])
                    {
                        tem[item.Coordinate.Y] = item;
                    }

                    for (i = max; i >= 0; i--)
                    {
                        if (tem[i] == null)
                            continue;

                        tem[i].TargetCoordinate = tem[i].Coordinate;
                        if (i != max)
                        {
                            for (k = i + 1; k <= max; k++)
                            {
                                if (tem[k] != null)
                                {
                                    if (tem[k].Value == 0 && tem[k].Number == tem[i].Number)
                                    {
                                        tem[k].Value = 1;
                                        tem[i].Value = 2;
                                        tem[i].TargetCoordinate = tem[k].TargetCoordinate;
                                        tem[i] = null;
                                        break;
                                    }
                                    else
                                        break;
                                }
                            }
                            if (tem[i] != null)
                            {
                                for (k = max; k > i; k--)
                                {
                                    if (tem[k] == null)
                                    {
                                        tem[i].TargetCoordinate = new Point(tem[i].Coordinate.X, k);
                                        tem[k] = tem[i];
                                        tem[i] = null;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    foreach (var item in ListAry[1][j])
                    {
                        item.Speed = Math.Abs(item.TargetCoordinate.Y - item.Coordinate.Y) * 10;
                        if (item.Speed > 0)
                            movelist.AddLast(item);
                    }
                    for (i = 0; i < tem.GetLength(0); i++)
                        tem[i] = null;
                }
                temL = ListAry[1];
                MoveAction(MoveDirection.Down);
            }

        }

        private void MoveAction(MoveDirection dir)
        {
            if (movelist.Count == 0)
            {
                IsPressing = false;
                return;
            }
            Dir = dir;
            _TimersTimer = new Timer();
            _TimersTimer.Interval = 8;
            _TimersTimer.Tick += new EventHandler(_TimersTimer_Elapsed);
            _TimersTimer.Start();
        }

        int Disss(Point p1, Point p2)
        {
            return (p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y);
        }

        void _TimersTimer_Elapsed(Object myObject, EventArgs myEventArgs)
        {

            foreach (var item in movelist)
            {
                item.MoveToDir(Dir);
            }
            if (Disss(movelist.First.Value.Location, movelist.First.Value.AccLoc(movelist.First.Value.TargetCoordinate)) < 800)
            {
                foreach (var item in temL)
                {
                    foreach (var xx in item)
                    {
                        xx.MoveToXY(xx.TargetCoordinate);
                        if (xx.Value == 1)
                        {
                            xx.ToNumBlock((BlockNum)((int)xx.Number * 2));
                        }
                        else if (xx.Value == 2)
                        {
                            Controls.Remove(xx);
                            NowBlock.Remove(xx);
                        }
                        xx.Value = 0;
                    }
                }

                movelist.Clear();
                CreateNumBlock();
                IsPressing = false;
                _TimersTimer.Tick -= new EventHandler(_TimersTimer_Elapsed);
                _TimersTimer.Dispose();
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            // Clear all Block on the field and reload
            foreach (var item in NowBlock)
            {
                Controls.Remove(item);
            }
            NowBlock.Clear();
            Form1_Load(sender, e);
        }
    }
}
