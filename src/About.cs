using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MouseCrank.src;

namespace MouseCrank.src
{
    public partial class About : Form {
        public About() {
            InitializeComponent();

            labelTitle.Text = "MouseCrank " + Program.GetVersion() + "\nfor Steel Beasts";
        }

        private void btnOK_Click(object sender, EventArgs e) {
            Close();
        }
    }
}
