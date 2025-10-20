using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesManagementSystem
{
    internal class ControlForm
    {
        public static void KeyControl(Form frm, object sender, KeyEventArgs e, Control previous, Control next)
        {
            bool forward = (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Down);
            bool backward = (e.KeyCode == Keys.Up);

            if (forward)
            {
                next.Focus();
            }
            else if (backward)
            {
                previous.Focus();
            }
        }


        public static void ClearData(Form frm)
        {
            ControlClearData(frm.Controls);
        }

        private static void ControlClearData(Control.ControlCollection controls)
        {
            foreach (Control ct in controls)
            {
                if (ct is TextBox || ct is MaskedTextBox)
                {
                    if (ct.Tag == null)
                    {
                        ct.Text = null;
                    }
                }
                else if (ct is RadioButton)
                {
                    ((RadioButton)ct).Checked = false;
                }
                //else if (ct is DateTimePicker)
                //{
                //    ((DateTimePicker)ct).CustomFormat = " ";
                //}
                else if (ct is PictureBox)
                {
                    ((PictureBox)ct).Image = null;
                }
                // Recursively check child controls
                if (ct.HasChildren)
                {
                    ControlClearData(ct.Controls);
                }
            }
        }
    }
}
