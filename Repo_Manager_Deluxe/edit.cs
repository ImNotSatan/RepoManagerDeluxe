namespace Repo_Manager_Deluxe
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    public class edit : Form
    {
        private string stweak;
        private string packagename = "no.package.name";
        private IContainer components = null;
        private GroupBox groupBox1;
        private ListView tweakdata;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private Label label2;
        private Label label1;
        private PictureBox tweak_icon;
        private GroupBox groupBox2;
        private Button change_icon;
        private GroupBox groupBox3;
        private RichTextBox richTextBox1;
        private Button update_tweak;
        private Label label3;

        public edit(string tweak)
        {
            this.stweak = tweak;
            this.InitializeComponent();
            this.tweakdata.Columns[0].Width = (this.tweakdata.Width / 2) - 10;
            this.tweakdata.Columns[1].Width = (this.tweakdata.Width / 2) - 10;
            this.get_data();
            if (File.Exists(Settings.repo + @"\depictions\" + this.packagename + ".html"))
            {
                this.richTextBox1.Text = File.ReadAllText(Settings.repo + @"\depictions\" + this.packagename + ".html");
            }
            if (!File.Exists(Settings.repo + @"\icons\" + this.packagename + ".png"))
            {
                this.default_tweak_icon();
            }
            else
            {
                using (FileStream stream = new FileStream(Settings.repo + @"\icons\" + this.packagename + ".png", FileMode.Open, FileAccess.Read))
                {
                    Image original = Image.FromStream(stream);
                    if ((original.Width != 0x100) || (original.Height != 0x100))
                    {
                        MessageBox.Show("The size of this image is not 100x100 so its cropped to 100x100 now.");
                        original = new Bitmap(original, new Size(0x100, 0x100));
                    }
                    this.tweak_icon.Image = original;
                }
            }
            this.groupBox2.Enabled = true;
            this.groupBox3.Enabled = true;
            this.update_tweak.Enabled = true;
            this.label3.Text = tweak;
        }

        public void default_tweak_icon()
        {
            Image image = new Bitmap(0x100, 0x100);
            Graphics graphics = Graphics.FromImage(image);
            graphics.Clear(Color.Cyan);
            Brush brush = new SolidBrush(Color.White);
            graphics.DrawString("Repo", new Font("Microsoft Sans Serif", 45f, FontStyle.Regular, GraphicsUnit.Point), brush, (float) 0f, (float) 20f);
            graphics.DrawString("Manager", new Font("Microsoft Sans Serif", 45f, FontStyle.Regular, GraphicsUnit.Point), brush, (float) 3f, (float) 100f);
            graphics.DrawString("DELUXE", new Font("Microsoft Sans Serif", 44f, FontStyle.Bold, GraphicsUnit.Point), brush, (float) 3f, (float) 180f);
            graphics.Save();
            brush.Dispose();
            graphics.Dispose();
            this.tweak_icon.Image = image;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void get_data()
        {
            string destFileName = Settings.repo + @"\temp\" + this.stweak + ".deb";
            foreach (string str2 in Directory.GetFiles(Settings.repo + @"\temp"))
            {
                File.Delete(str2);
            }
            File.Copy(Settings.repo + @"\debs\" + this.stweak + ".deb", destFileName);
            Extract.deb(this.stweak + ".deb", Settings.repo);
            Extract.deb("control.tar.gz", Settings.repo);
            this.packagename = "no.package.name";
            foreach (string str3 in File.ReadAllLines(Settings.repo + @"\temp\control"))
            {
                if (str3.Contains("Package:"))
                {
                    this.packagename = str3.Replace("Package: ", "Package:".Replace("Package:", ""));
                }
                char[] separator = new char[] { ':' };
                string[] strArray3 = str3.Split(separator);
                ListViewItem item = new ListViewItem(strArray3[0]) {
                    SubItems = { strArray3[1] }
                };
                this.tweakdata.Items.Add(item);
            }
        }

        private void InitializeComponent()
        {
            this.groupBox1 = new GroupBox();
            this.tweakdata = new ListView();
            this.columnHeader1 = new ColumnHeader();
            this.columnHeader2 = new ColumnHeader();
            this.label2 = new Label();
            this.label1 = new Label();
            this.tweak_icon = new PictureBox();
            this.groupBox2 = new GroupBox();
            this.change_icon = new Button();
            this.groupBox3 = new GroupBox();
            this.richTextBox1 = new RichTextBox();
            this.update_tweak = new Button();
            this.label3 = new Label();
            this.groupBox1.SuspendLayout();
            ((ISupportInitialize) this.tweak_icon).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            base.SuspendLayout();
            this.groupBox1.Controls.Add(this.tweakdata);
            this.groupBox1.Location = new Point(0x1a, 0xbc);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(600, 210);
            this.groupBox1.TabIndex = 0x27;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tweak info";
            this.tweakdata.BorderStyle = BorderStyle.FixedSingle;
            ColumnHeader[] values = new ColumnHeader[] { this.columnHeader1, this.columnHeader2 };
            this.tweakdata.Columns.AddRange(values);
            this.tweakdata.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.tweakdata.FullRowSelect = true;
            this.tweakdata.GridLines = true;
            this.tweakdata.HideSelection = false;
            this.tweakdata.ImeMode = ImeMode.Close;
            this.tweakdata.Location = new Point(13, 30);
            this.tweakdata.Margin = new Padding(3, 5, 3, 5);
            this.tweakdata.Name = "tweakdata";
            this.tweakdata.Size = new Size(0x23b, 0xac);
            this.tweakdata.TabIndex = 0x20;
            this.tweakdata.UseCompatibleStateImageBehavior = false;
            this.tweakdata.View = View.Details;
            this.columnHeader1.Text = "Data";
            this.columnHeader1.Width = 250;
            this.columnHeader2.Text = "Value";
            this.columnHeader2.Width = 250;
            this.label2.AutoSize = true;
            this.label2.Font = new Font("Microsoft Sans Serif", 16f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label2.Location = new Point(0x13, 0x80);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x89, 0x25);
            this.label2.TabIndex = 0x25;
            this.label2.Text = "TWEAK";
            this.label1.AutoSize = true;
            this.label1.Font = new Font("Microsoft Sans Serif", 32f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label1.Location = new Point(0xe1, 0x16);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0xb2, 0x49);
            this.label1.TabIndex = 0x24;
            this.label1.Text = "EDIT";
            this.tweak_icon.Location = new Point(13, 0x22);
            this.tweak_icon.Name = "tweak_icon";
            this.tweak_icon.Size = new Size(0x100, 0x100);
            this.tweak_icon.SizeMode = PictureBoxSizeMode.StretchImage;
            this.tweak_icon.TabIndex = 0x20;
            this.tweak_icon.TabStop = false;
            this.groupBox2.Controls.Add(this.change_icon);
            this.groupBox2.Controls.Add(this.tweak_icon);
            this.groupBox2.Enabled = false;
            this.groupBox2.Location = new Point(0x1a, 0x1a8);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(600, 0x135);
            this.groupBox2.TabIndex = 40;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Tweak Icon (256x256)";
            this.change_icon.BackColor = Color.White;
            this.change_icon.FlatStyle = FlatStyle.Flat;
            this.change_icon.Location = new Point(0x127, 0x22);
            this.change_icon.Name = "change_icon";
            this.change_icon.Size = new Size(0x121, 0x100);
            this.change_icon.TabIndex = 0x23;
            this.change_icon.Text = "CHANGE ICON";
            this.change_icon.UseVisualStyleBackColor = false;
            this.groupBox3.Controls.Add(this.richTextBox1);
            this.groupBox3.Enabled = false;
            this.groupBox3.Location = new Point(0x1a, 0x2f1);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new Size(600, 0x135);
            this.groupBox3.TabIndex = 0x29;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Depiction";
            this.richTextBox1.Location = new Point(13, 0x23);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new Size(0x23b, 0x101);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "<h1>Repo Manager Deluxe Default Tweak Depiction</h1>";
            this.update_tweak.BackColor = Color.White;
            this.update_tweak.Enabled = false;
            this.update_tweak.FlatStyle = FlatStyle.Flat;
            this.update_tweak.Location = new Point(0x1a, 0x441);
            this.update_tweak.Name = "update_tweak";
            this.update_tweak.Size = new Size(600, 0x41);
            this.update_tweak.TabIndex = 0x2a;
            this.update_tweak.Text = "UPDATE TWEAK";
            this.update_tweak.UseVisualStyleBackColor = false;
            this.update_tweak.Click += new EventHandler(this.update_tweak_Click);
            this.label3.AutoSize = true;
            this.label3.Font = new Font("Microsoft Sans Serif", 16f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label3.Location = new Point(0xa2, 0x80);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x94, 0x25);
            this.label3.TabIndex = 0x2b;
            this.label3.Text = "loading...";
            base.AutoScaleDimensions = new SizeF(9f, 20f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x285, 0x495);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.groupBox2);
            base.Controls.Add(this.groupBox3);
            base.Controls.Add(this.update_tweak);
            base.Name = "edit";
            base.ShowIcon = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.groupBox1.ResumeLayout(false);
            ((ISupportInitialize) this.tweak_icon).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void update_tweak_Click(object sender, EventArgs e)
        {
            if (File.Exists(Settings.repo + "/icons/" + this.packagename + ".png"))
            {
                File.Delete(Settings.repo + "/icons/" + this.packagename + ".png");
            }
            this.tweak_icon.Image.Save(Settings.repo + "/icons/" + this.packagename + ".png");
            File.WriteAllText(Settings.repo + @"\depictions\" + this.stweak + ".html", this.richTextBox1.Text);
            base.Close();
        }
    }
}

