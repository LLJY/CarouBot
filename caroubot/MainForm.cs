using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace caroubot
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            Initialize();
        }
        public async void Initialize()
        {
            InitializeComponent();
            photos_combo.Items.Add(1);
            photos_combo.Items.Add(2);
            photos_combo.Items.Add(3);
            photos_combo.Items.Add(4);
        }

        private void save_button_Click(object sender, EventArgs e)
        {

        }
    }
}
