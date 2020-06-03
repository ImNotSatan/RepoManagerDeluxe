namespace Repo_Manager_Deluxe
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    public class addpackage : Form
    {
        private Main caller;
        private string filen = "";
        private string packagename = "no.package.name";
        private IContainer components = null;
        private Label label1;
        private Label label2;
        private Button open_tweak;
        private GroupBox groupBox1;
        private Label tweak_info;
        private ListView tweakdata;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private PictureBox tweak_icon;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private RichTextBox richTextBox1;
        private Button change_icon;
        private Button add_tweak;

        public addpackage(Main main)
        {
            this.caller = main;
            this.InitializeComponent();
            if(File.Exists("default.depiction"))
            {
                richTextBox1.Text = File.ReadAllText("default.depiction");
            }
            this.tweakdata.Columns[0].Width = (this.tweakdata.Width / 2) - 10;
            this.tweakdata.Columns[1].Width = (this.tweakdata.Width / 2) - 10;
        }

        private void add_tweak_Click(object sender, EventArgs e)
        {
            if (File.Exists(Settings.repo + "/debs/" + this.filen))
            {
                MessageBox.Show("Could not add tweak, it already exists.");
            }
            else
            {
                File.Copy(Settings.repo + "/temp/" + this.filen, Settings.repo + "/debs/" + this.filen);
                if (File.Exists(Settings.repo + "/icons/" + this.packagename + ".png"))
                {
                    File.Delete(Settings.repo + "/icons/" + this.packagename + ".png");
                }
                this.tweak_icon.Image.Save(Settings.repo + "/icons/" + this.packagename + ".png");
                File.WriteAllText(Settings.repo + "/depictions/" + this.packagename + ".html", this.richTextBox1.Text);
                this.caller.packagelist.Items.Add(this.filen.Replace(".deb", ""));
                base.Close();
            }
        }

        private void change_icon_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog {
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

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.open_tweak = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tweak_info = new System.Windows.Forms.Label();
            this.tweakdata = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tweak_icon = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.change_icon = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.add_tweak = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tweak_icon)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(47, 12);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(343, 51);
            this.label1.TabIndex = 0;
            this.label1.Text = "NEW PACKAGE";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(8, 83);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(174, 26);
            this.label2.TabIndex = 1;
            this.label2.Text = "TWEAK (.DEB)";
            // 
            // open_tweak
            // 
            this.open_tweak.BackColor = System.Drawing.Color.White;
            this.open_tweak.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.open_tweak.Location = new System.Drawing.Point(193, 83);
            this.open_tweak.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.open_tweak.Name = "open_tweak";
            this.open_tweak.Size = new System.Drawing.Size(219, 24);
            this.open_tweak.TabIndex = 30;
            this.open_tweak.Text = "CHOOSE";
            this.open_tweak.UseVisualStyleBackColor = false;
            this.open_tweak.Click += new System.EventHandler(this.open_tweak_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tweak_info);
            this.groupBox1.Controls.Add(this.tweakdata);
            this.groupBox1.Location = new System.Drawing.Point(13, 122);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Size = new System.Drawing.Size(400, 136);
            this.groupBox1.TabIndex = 31;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tweak info";
            // 
            // tweak_info
            // 
            this.tweak_info.AutoSize = true;
            this.tweak_info.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tweak_info.ForeColor = System.Drawing.Color.Red;
            this.tweak_info.Location = new System.Drawing.Point(125, 38);
            this.tweak_info.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.tweak_info.Name = "tweak_info";
            this.tweak_info.Size = new System.Drawing.Size(154, 13);
            this.tweak_info.TabIndex = 0;
            this.tweak_info.Text = "SELECT A TWEAK FIRST";
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
            this.tweakdata.Visible = false;
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
            // tweak_icon
            // 
            this.tweak_icon.Location = new System.Drawing.Point(9, 22);
            this.tweak_icon.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
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
            this.groupBox2.Location = new System.Drawing.Point(13, 276);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox2.Size = new System.Drawing.Size(400, 201);
            this.groupBox2.TabIndex = 33;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Tweak Icon (256x256)";
            // 
            // change_icon
            // 
            this.change_icon.BackColor = System.Drawing.Color.White;
            this.change_icon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.change_icon.Location = new System.Drawing.Point(197, 22);
            this.change_icon.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
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
            this.groupBox3.Location = new System.Drawing.Point(13, 489);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox3.Size = new System.Drawing.Size(400, 201);
            this.groupBox3.TabIndex = 34;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Depiction";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(9, 23);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(382, 168);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "<h1>Repo Manager Deluxe Default Tweak Depiction</h1>";
            // 
            // add_tweak
            // 
            this.add_tweak.BackColor = System.Drawing.Color.White;
            this.add_tweak.Enabled = false;
            this.add_tweak.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.add_tweak.Location = new System.Drawing.Point(13, 708);
            this.add_tweak.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.add_tweak.Name = "add_tweak";
            this.add_tweak.Size = new System.Drawing.Size(400, 42);
            this.add_tweak.TabIndex = 35;
            this.add_tweak.Text = "Add Tweak";
            this.add_tweak.UseVisualStyleBackColor = false;
            this.add_tweak.Click += new System.EventHandler(this.add_tweak_Click);
            // 
            // addpackage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(430, 761);
            this.Controls.Add(this.add_tweak);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.open_tweak);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "addpackage";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tweak_icon)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void open_tweak_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog {
                FileName = "",
                Filter = "TWEAK (*.DEB) | *.deb"
            };
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            else
            {
                this.filen = dialog.SafeFileName.Replace(" ", "");
                if (!File.Exists(Settings.repo + @"\debs\" + this.filen))
                {
                    string destFileName = Settings.repo + @"\temp\" + this.filen;
                    string[] files = Directory.GetFiles(Settings.repo + @"\temp");
                    int index = 0;
                    while (true)
                    {
                        if (index >= files.Length)
                        {
                            File.Copy(dialog.FileName, destFileName);
                            Extract.deb(this.filen, Settings.repo);
                            Extract.deb("control.tar.gz", Settings.repo);
                            this.packagename = "no.package.name";
                            string[] strArray2 = File.ReadAllLines(Settings.repo + @"\temp\control");
                            int num2 = 0;
                            while (true)
                            {
                                if (num2 >= strArray2.Length)
                                {
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
                                    break;
                                }
                                string str3 = strArray2[num2];
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
                                num2++;
                            }
                            break;
                        }
                        string path = files[index];
                        File.Delete(path);
                        index++;
                    }
                }
                else
                {
                    MessageBox.Show(dialog.SafeFileName + " is already in your tweaks.", "ERROR");
                    return;
                }
            }
            this.tweak_info.Visible = false;
            this.tweakdata.Visible = true;
            this.groupBox2.Enabled = true;
            this.groupBox3.Enabled = true;
            this.add_tweak.Enabled = true;
        }
    }
}

