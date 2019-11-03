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
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace caroubot
{
    public partial class MainForm : Form
    {
        string driverPath = Application.StartupPath + @"\geckodriver.exe";
        IWebDriver driver;
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
        public async void click(string xpath)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            IWebElement element = wait.Until(ExpectedConditions.ElementExists(By.XPath(xpath)));
            element.Click();
        }
        public async void fill(string xpath, string text)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            IWebElement element = wait.Until(ExpectedConditions.ElementExists(By.XPath(xpath)));
            element.SendKeys(text);
        }
        public async Task run(PostInfo pi)
        {
            var profileManager = new FirefoxProfileManager();
            FirefoxProfile myprofile = profileManager.GetProfile("default-release");
            FirefoxOptions options = new FirefoxOptions();
            options.Profile = myprofile;
            driver = new FirefoxDriver(Application.StartupPath, options);
            driver.Url = ("https://sg.carousell.com/sell/");
            for (int i = 0; i < pi.NumberofPhotos; i++)
            {
                string photoPath = Application.StartupPath + $@"\photos\photo{i}.jpg";
                //post photo
                driver.FindElement(By.Id($"photo{i}")).SendKeys(photoPath);
                click("//span[text()=\"Save\"]");
            }
            click("//button[text()=\"Next: Choose a category\"]");
            fill("//input[@placeholder=\"Search for a category\"]", "Mobile Phones");
            click("(//button[@class=\"el-_a el-m\"])[1]");
            //TITLE
            fill("//input[@placeholder=\"Name your listing\"]", pi.Title);
            /*BRAND
            fill("//input[@placeholder=\"Brand of the Item\"]", pi.Brand);
            */
            //CONDITION
            if (pi.Used)
            {
                click("//label[@for=\"condition_1\"]");
            }
            else
            {
                click("//label[@for=\"condition_0\"]");
            }
            //PRICE
            fill("//input[@placeholder=\"Price of your listing\"][@type=\"number\"]", pi.Price.ToString());
            //DESC
            fill("//textarea[@placeholder=\"Describe what you are selling and include any details a buyer might be interested in. People love items with stories!\"]", pi.Description);
            //WE DO NOT USE CAROUPAY
            click("//label[@for=\"field_4caroupay\"]");
            //MULTIPLE
            /*
            if (pi.MultipleUnits)
            {
                click("//input[@for=\"field_7multi_quantities\"]");
            }
            */
            if (!pi.Delivery)
            {
                click("//label[@for=\"field_10mailing\"]");
            }
            if (pi.MeetUp)
            {
                click("//label[@for=\"field_8meetup\"]");
                //may error out if place does not exist
                try
                {
                    fill("//input[@placeholder=\"Search for a location\"]", pi.Location);
                    click($"//div[text()=\"{pi.Location}\"]");
                    click("//span[text()=\"Save\"]");
                }
                catch
                {
                    MessageBox.Show("Location does not exist!!", "well.. shit", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
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

        private async void POST_Click(object sender, EventArgs e)
        {
            try
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
                    await run(postinfo);
                }
            }
            catch
            {
                MessageBox.Show("An Error Has Occured", "well.. shit", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
