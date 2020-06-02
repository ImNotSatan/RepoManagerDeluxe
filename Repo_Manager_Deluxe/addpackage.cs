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
                    if ((original.Width == 0x100) && (original.Height == 0x100))
                    {
                        original.Save(Settings.repo + "/CydiaIcon.png");
                    }
                    else
                    {
                        original = new Bitmap(original, new Size(0x100, 0x100));
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
            this.label1 = new Label();
            this.label2 = new Label();
            this.open_tweak = new Button();
            this.groupBox1 = new GroupBox();
            this.tweak_info = new Label();
            this.tweakdata = new ListView();
            this.columnHeader1 = new ColumnHeader();
            this.columnHeader2 = new ColumnHeader();
            this.tweak_icon = new PictureBox();
            this.groupBox2 = new GroupBox();
            this.change_icon = new Button();
            this.groupBox3 = new GroupBox();
            this.richTextBox1 = new RichTextBox();
            this.add_tweak = new Button();
            this.groupBox1.SuspendLayout();
            ((ISupportInitialize) this.tweak_icon).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            base.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Font = new Font("Microsoft Sans Serif", 32f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label1.Location = new Point(70, 0x12);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x1fd, 0x49);
            this.label1.TabIndex = 0;
            this.label1.Text = "NEW PACKAGE";
            this.label2.AutoSize = true;
            this.label2.Font = new Font("Microsoft Sans Serif", 16f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label2.Location = new Point(12, 0x80);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0xf9, 0x25);
            this.label2.TabIndex = 1;
            this.label2.Text = "TWEAK (.DEB)";
            this.open_tweak.BackColor = Color.White;
            this.open_tweak.FlatStyle = FlatStyle.Flat;
            this.open_tweak.Location = new Point(290, 0x80);
            this.open_tweak.Name = "open_tweak";
            this.open_tweak.Size = new Size(0x149, 0x25);
            this.open_tweak.TabIndex = 30;
            this.open_tweak.Text = "CHOOSE";
            this.open_tweak.UseVisualStyleBackColor = false;
            this.open_tweak.Click += new EventHandler(this.open_tweak_Click);
            this.groupBox1.Controls.Add(this.tweak_info);
            this.groupBox1.Controls.Add(this.tweakdata);
            this.groupBox1.Location = new Point(0x13, 0xbc);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(600, 210);
            this.groupBox1.TabIndex = 0x1f;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tweak info";
            this.tweak_info.AutoSize = true;
            this.tweak_info.Font = new Font("Microsoft Sans Serif", 8f, FontStyle.Underline | FontStyle.Bold, GraphicsUnit.Point, 0);
            this.tweak_info.ForeColor = Color.Red;
            this.tweak_info.Location = new Point(0xbb, 0x3a);
            this.tweak_info.Name = "tweak_info";
            this.tweak_info.Size = new Size(0xd9, 20);
            this.tweak_info.TabIndex = 0;
            this.tweak_info.Text = "SELECT A TWEAK FIRST";
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
            this.tweakdata.Visible = false;
            this.columnHeader1.Text = "Data";
            this.columnHeader1.Width = 250;
            this.columnHeader2.Text = "Value";
            this.columnHeader2.Width = 250;
            this.tweak_icon.Location = new Point(13, 0x22);
            this.tweak_icon.Name = "tweak_icon";
            this.tweak_icon.Size = new Size(0x100, 0x100);
            this.tweak_icon.SizeMode = PictureBoxSizeMode.StretchImage;
            this.tweak_icon.TabIndex = 0x20;
            this.tweak_icon.TabStop = false;
            this.groupBox2.Controls.Add(this.change_icon);
            this.groupBox2.Controls.Add(this.tweak_icon);
            this.groupBox2.Enabled = false;
            this.groupBox2.Location = new Point(0x13, 0x1a8);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(600, 0x135);
            this.groupBox2.TabIndex = 0x21;
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
            this.change_icon.Click += new EventHandler(this.change_icon_Click);
            this.groupBox3.Controls.Add(this.richTextBox1);
            this.groupBox3.Enabled = false;
            this.groupBox3.Location = new Point(0x13, 0x2f1);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new Size(600, 0x135);
            this.groupBox3.TabIndex = 0x22;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Depiction";
            this.richTextBox1.Location = new Point(13, 0x23);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new Size(0x23b, 0x101);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "<h1>Repo Manager Deluxe Default Tweak Depiction</h1>";
            this.add_tweak.BackColor = Color.White;
            this.add_tweak.Enabled = false;
            this.add_tweak.FlatStyle = FlatStyle.Flat;
            this.add_tweak.Location = new Point(0x13, 0x441);
            this.add_tweak.Name = "add_tweak";
            this.add_tweak.Size = new Size(600, 0x41);
            this.add_tweak.TabIndex = 0x23;
            this.add_tweak.Text = "Add Tweak";
            this.add_tweak.UseVisualStyleBackColor = false;
            this.add_tweak.Click += new EventHandler(this.add_tweak_Click);
            base.AutoScaleDimensions = new SizeF(9f, 20f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x285, 0x495);
            base.Controls.Add(this.add_tweak);
            base.Controls.Add(this.groupBox3);
            base.Controls.Add(this.groupBox2);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.open_tweak);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Name = "addpackage";
            base.ShowIcon = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((ISupportInitialize) this.tweak_icon).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
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

