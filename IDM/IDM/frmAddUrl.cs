using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDM
{
    public partial class frmAddUrl : Form
    {
        public frmAddUrl()
        {
            InitializeComponent();
            btnOk.DialogResult = DialogResult.OK;
        }
        public string Url { get; set; }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Url = txtAddUrl.Text;

            
        }
    }
}
