using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Kevin_s_2048
{
    public enum BlockNum : int
    {
        _2 = 2, _4 = 4, _8 = 8, _16 = 16, _32 = 32, _64 = 64, _128 = 128, _256 = 256, _512 = 512, _1024 = 1024, _2048 = 2048
    };

    public enum MoveDirection
    {
        Up, Down, Left, Right
    };

    public class Block : Label
    {
        /// <summary>
        /// 取得背景Label控制項
        /// </summary>
        public Label BackGround { get; private set; }

        /// <summary>
        /// 取得框線寬度
        /// </summary>
        public int FrameWidth { get; private set; }

        /// <summary>
        /// 取得或設定數字
        /// </summary>
        public BlockNum Number { get; private set; }

        /// <summary>
        /// 取得座標值
        /// </summary>
        public Point Coordinate { get; private set; }

        /// <summary>
        /// 取得座標值
        /// </summary>
        public Point TargetCoordinate { get; set; }

        /// <summary>
        /// 取得或設定自訂值, 
        /// 預設為0
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// 取的或設定移動時的速度, 
        /// 預設為1
        /// </summary>
        public int Speed { get; set; }

        /// <summary>
        /// 初始建構式
        /// </summary>
        /// <param name="backgorund"></param>
        /// <param name="frame_width"></param>
        public Block(Label backgorund, int frame_width)
        {
            BackGround = backgorund;
            FrameWidth = frame_width;
            Font = new Font("Segoe UI", 32, FontStyle.Bold);
            TextAlign = ContentAlignment.MiddleCenter;
            BackColor = Color.FromArgb(204, 191, 179);
            Size = new Size((backgorund.Size.Width - 5 * frame_width) / 4,
                (backgorund.Size.Height - 5 * frame_width) / 4);
            Value = 0;
            Speed = 0;
        }

        /// <summary>
        /// 轉換成數字Block
        /// </summary>
        public void ToNumBlock(BlockNum num)
        {
            Number = num;
            ForeColor = Color.White;
            Text = ((int)(num)).ToString();


            if ((int)num < 100)
            {
                Font = new Font("Segoe UI", 32, FontStyle.Bold);
            }
            else if ((int)num < 1000)
            {
                Font = new Font("Segoe UI", 24, FontStyle.Bold);
            }
            else if ((int)num < 10000)
            {
                Font = new Font("Segoe UI", 20, FontStyle.Bold);
            }

            switch (num)
            {
                case BlockNum._2:
                    ForeColor = Color.FromArgb(118, 109, 101);
                    BackColor = Color.FromArgb(237, 227, 217);
                    break;
                case BlockNum._4:
                    ForeColor = Color.FromArgb(118, 109, 101);
                    BackColor = Color.FromArgb(236, 223, 199);
                    break;
                case BlockNum._8:
                    BackColor = Color.FromArgb(241, 176, 121);
                    break;
                case BlockNum._16:
                    BackColor = Color.FromArgb(244, 148, 99);
                    break;
                case BlockNum._32:
                    BackColor = Color.FromArgb(244, 125, 95);
                    break;
                case BlockNum._64:
                    BackColor = Color.FromArgb(233, 89, 55);
                    break;
                case BlockNum._128:
                    BackColor = Color.FromArgb(242, 216, 106);
                    break;
                case BlockNum._256:
                    BackColor = Color.FromArgb(242, 208, 75);
                    break;
                case BlockNum._512:
                    BackColor = Color.FromArgb(228, 192, 42);
                    break;
                case BlockNum._1024:
                    BackColor = Color.FromArgb(230, 182, 22);
                    break;
                case BlockNum._2048:
                    BackColor = Color.FromArgb(236, 196, 0);
                    break;
            }
        }

        /// <summary>
        /// 轉換成背景Block
        /// </summary>
        public void ToBackBlock()
        {
            Text = "";
            BackColor = Color.FromArgb(204, 191, 179);
        }

        public Point AccLoc(Point target)
        {
            int x = target.X;
            int y = target.Y;
            return new Point(BackGround.Location.X + FrameWidth + (FrameWidth + Size.Width) * x,
                BackGround.Location.Y + FrameWidth + (FrameWidth + Size.Height) * y);
        }

        /// <summary>
        /// 瞬間移動至指定座標位置
        /// </summary>
        public void MoveToXY(int x, int y)
        {
            Coordinate = new Point(x, y);
            Location = new Point(BackGround.Location.X + FrameWidth + (FrameWidth + Size.Width) * x,
                BackGround.Location.Y + FrameWidth + (FrameWidth + Size.Height) * y);
        }
        public void MoveToXY(Point target)
        {
            MoveToXY(target.X, target.Y);
        }

        /// <summary>
        /// 朝指定方位移動
        /// </summary>
        public void MoveToDir(MoveDirection dir)
        {
            switch (dir)
            {
                case MoveDirection.Up:
                    Location = new Point(Location.X, Location.Y - Speed);
                    break;
                case MoveDirection.Down:
                    Location = new Point(Location.X, Location.Y + Speed);
                    break;
                case MoveDirection.Left:
                    Location = new Point(Location.X - Speed, Location.Y);
                    break;
                case MoveDirection.Right:
                    Location = new Point(Location.X + Speed, Location.Y);
                    break;
            }
        }
    }
}
