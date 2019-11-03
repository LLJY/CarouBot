using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

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
            if (string.IsNullOrEmpty(title_box.Text) || string.IsNullOrEmpty(brand_box.Text) || string.IsNullOrEmpty(price_box.Text) ||
                string.IsNullOrEmpty(location_box.Text) || string.IsNullOrEmpty(desc_box.Text))
            {
                MessageBox.Show("ONE OR MORE FIELDS ARE EMPTY, CANNOT CONTINUE!", "well.. shit", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                PostInfo postinfo = new PostInfo(photos_combo.SelectedIndex + 1, title_box.Text, brand_box.Text, desc_box.Text, used_check.Checked,
                    Int32.Parse(price_box.Text), multiple_check.Checked, meetup_check.Checked, delivery_check.Checked, location_box.Text);
                string json = JsonConvert.SerializeObject(postinfo, Formatting.Indented);
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "json files (*.json)|*.json";
                saveFileDialog1.FilterIndex = 1;
                saveFileDialog1.RestoreDirectory = true;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(saveFileDialog1.FileName, json);
                }
            }
        }

        private void load_button_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.Filter = "json files (*.json)|*.json";
            openfile.FilterIndex = 1;
            openfile.RestoreDirectory = true;
            if (openfile.ShowDialog() == DialogResult.OK)
            {
                string serialized = File.ReadAllText(openfile.FileName);
                PostInfo postinfo = JsonConvert.DeserializeObject<PostInfo>(serialized);
                photos_combo.SelectedIndex = postinfo.NumberofPhotos - 1;
                title_box.Text = postinfo.Title;
                brand_box.Text = postinfo.Brand;
                desc_box.Text = postinfo.Description;
                used_check.Checked = postinfo.Used;
                price_box.Text = postinfo.Price.ToString();
                multiple_check.Checked = postinfo.MultipleUnits;
                meetup_check.Checked = postinfo.MeetUp;
                delivery_check.Checked = postinfo.Delivery;
                location_box.Text = postinfo.Location;
            }
        }
    }
}
