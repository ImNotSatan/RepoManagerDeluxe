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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tweakdata = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tweak_icon = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.change_icon = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.update_tweak = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tweak_icon)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tweakdata);
            this.groupBox1.Location = new System.Drawing.Point(17, 122);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(400, 136);
            this.groupBox1.TabIndex = 39;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tweak info";
            // 
            // tweakdata
            // 
            this.tweakdata.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tweakdata.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.tweakdata.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tweakdata.FullRowSelect = true;
            this.tweakdata.GridLines = true;
            this.tweakdata.HideSelection = false;
            this.tweakdata.ImeMode = System.Windows.Forms.ImeMode.Close;
            this.tweakdata.Location = new System.Drawing.Point(9, 19);
            this.tweakdata.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tweakdata.Name = "tweakdata";
            this.tweakdata.Size = new System.Drawing.Size(381, 112);
            this.tweakdata.TabIndex = 32;
            this.tweakdata.UseCompatibleStateImageBehavior = false;
            this.tweakdata.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Data";
            this.columnHeader1.Width = 250;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Value";
            this.columnHeader2.Width = 250;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(13, 83);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 26);
            this.label2.TabIndex = 37;
            this.label2.Text = "TWEAK";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(150, 14);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 51);
            this.label1.TabIndex = 36;
            this.label1.Text = "EDIT";
            // 
            // tweak_icon
            // 
            this.tweak_icon.Location = new System.Drawing.Point(9, 22);
            this.tweak_icon.Margin = new System.Windows.Forms.Padding(2);
            this.tweak_icon.Name = "tweak_icon";
            this.tweak_icon.Size = new System.Drawing.Size(171, 166);
            this.tweak_icon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.tweak_icon.TabIndex = 32;
            this.tweak_icon.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.change_icon);
            this.groupBox2.Controls.Add(this.tweak_icon);
            this.groupBox2.Enabled = false;
            this.groupBox2.Location = new System.Drawing.Point(17, 276);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(400, 201);
            this.groupBox2.TabIndex = 40;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Tweak Icon (256x256)";
            // 
            // change_icon
            // 
            this.change_icon.BackColor = System.Drawing.Color.White;
            this.change_icon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.change_icon.Location = new System.Drawing.Point(197, 22);
            this.change_icon.Margin = new System.Windows.Forms.Padding(2);
            this.change_icon.Name = "change_icon";
            this.change_icon.Size = new System.Drawing.Size(193, 166);
            this.change_icon.TabIndex = 35;
            this.change_icon.Text = "CHANGE ICON";
            this.change_icon.UseVisualStyleBackColor = false;
            this.change_icon.Click += new System.EventHandler(this.change_icon_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.richTextBox1);
            this.groupBox3.Enabled = false;
            this.groupBox3.Location = new System.Drawing.Point(17, 489);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox3.Size = new System.Drawing.Size(400, 201);
            this.groupBox3.TabIndex = 41;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Depiction";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(9, 23);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(2);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(382, 168);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "<h1>Repo Manager Deluxe Default Tweak Depiction</h1>";
            // 
            // update_tweak
            // 
            this.update_tweak.BackColor = System.Drawing.Color.White;
            this.update_tweak.Enabled = false;
            this.update_tweak.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.update_tweak.Location = new System.Drawing.Point(17, 708);
            this.update_tweak.Margin = new System.Windows.Forms.Padding(2);
            this.update_tweak.Name = "update_tweak";
            this.update_tweak.Size = new System.Drawing.Size(400, 42);
            this.update_tweak.TabIndex = 42;
            this.update_tweak.Text = "UPDATE TWEAK";
            this.update_tweak.UseVisualStyleBackColor = false;
            this.update_tweak.Click += new System.EventHandler(this.update_tweak_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(108, 83);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 26);
            this.label3.TabIndex = 43;
            this.label3.Text = "loading...";
            // 
            // edit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(430, 764);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.update_tweak);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "edit";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tweak_icon)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

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

        private void change_icon_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                FileName = "",
                Filter = "PNG FILES (*.PNG) | *.png"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                using (FileStream stream = new FileStream(dialog.FileName, FileMode.Open, FileAccess.Read))
                {
                    Image original = Image.FromStream(stream);
                    if ((original.Width != 256) || (original.Height != 256))
                    {
                        original = new Bitmap(original, new Size(256, 256));
                        MessageBox.Show("The size of this image is not 256x256 so its cropped to 256x256 now.");
                    }
                    this.tweak_icon.Image = original;
                }
            }
        }
    }
}

