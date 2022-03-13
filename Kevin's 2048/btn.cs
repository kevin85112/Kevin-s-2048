using System.Windows.Forms;

namespace Kevin_s_2048
{
    public class btn : Button
    {
        protected override bool IsInputKey(Keys keyData)
        {
            if (keyData == Keys.Up || keyData == Keys.Down || keyData == Keys.Left || keyData == Keys.Right)
            {
                return true;
            }
            else
            {
                return base.IsInputKey(keyData);
            }
        }
    }
}
