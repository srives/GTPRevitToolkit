using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GTP.UI
{
    public partial class ConfirmationWindow : Form
    {
        public ConfirmationWindow(string windowName, string confirmationText)
        {
            WindowName = windowName;
            ConfirmationText = confirmationText;
            
            InitializeComponent();
        }

        private void ConfirmationWindow_Load(object sender, EventArgs e)
        {
            this.Text = WindowName;
            ConfirmationDialog.Text = ConfirmationText;
        }

        public string WindowName { get; set; }
        public string ConfirmationText { get; set; }

        private void ConfirmationDialog_Click(object sender, EventArgs e){}
    }
}
