namespace Repo_Manager_Deluxe
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;

    public class Main : Form
    {
        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;
        private IContainer components = null;
        private Label label1;
        private Label label2;
        private Button add_package;
        private Button generate_packages;
        private Button reload_packages;
        private Button lock_repo;
        public ListBox packagelist;
        private RichTextBox richTextBox1;
        private Button showlogs;
        private GroupBox groupBox1;
        private PictureBox repo_icon;
        private Label label3;
        private Label label4;
        private TextBox description;
        private Label label9;
        private TextBox version;
        private Label label8;
        private TextBox origin;
        private Label label7;
        private TextBox codename;
        private Label label5;
        private TextBox name;
        private Label label6;
        private Button save_release;
        private Button change_icon;
        private Button open_folder;
        private Label label10;
        private Button settings;
        private Label label11;
        private ContextMenuStrip package_options;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem removeToolStripMenuItem;
        private TextBox repolink;
        private Label label12;

        public Main()
        {
            this.InitializeComponent();
            Console.WriteLine("Loading debs.");
            this.loaddebs();
            Console.WriteLine("Done.");
            Console.Title = "Logging";
            this.hide_logs();
            if (File.Exists(Settings.repo + "/repo.link"))
            {
                this.repolink.Text = File.ReadAllText(Settings.repo + "/repo.link");
            }
            if (!File.Exists(Settings.repo + "/CydiaIcon.png"))
            {
                this.default_logo();
            }
            else
            {
                using (FileStream stream = new FileStream(Settings.repo + "/CydiaIcon.png", FileMode.Open, FileAccess.Read))
                {
                    this.repo_icon.Image = Image.FromStream(stream);
                }
            }
            if (!File.Exists(Settings.repo + "/Release"))
            {
                this.origin.Text = "Repo Manager Deluxe Default Origin";
                this.name.Text = "Repo Manager Deluxe Default Name";
                this.version.Text = "1.0";
                this.codename.Text = "iospackagemanagerdeluxe";
                this.description.Text = "Made with Repo Manager Deluxe";
                this.save_release_Click(this, new EventArgs());
            }
            else
            {
                foreach (string str in File.ReadAllLines(Settings.repo + "/Release"))
                {
                    if (str.ToLower().Contains("origin:"))
                    {
                        this.origin.Text = str.Replace(": ", ":").Replace("Origin:", "");
                    }
                    else if (str.ToLower().Contains("label:"))
                    {
                        this.name.Text = str.Replace(": ", ":").Replace("Label:", "");
                    }
                    else if (str.ToLower().Contains("version:"))
                    {
                        this.version.Text = str.Replace(": ", ":").Replace("Version:", "");
                    }
                    else if (str.ToLower().Contains("codename:"))
                    {
                        this.codename.Text = str.Replace(": ", ":").Replace("Codename:", "");
                    }
                    else if (str.ToLower().Contains("description:"))
                    {
                        this.description.Text = str.Replace(": ", ":").Replace("Description:", "");
                    }
                }
            }
            this.packagelist.Select();
        }

        private void add_package_Click(object sender, EventArgs e)
        {
            new addpackage(this).ShowDialog();
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
                    if ((original.Width == 0x40) && (original.Height == 0x40))
                    {
                        File.Delete(Settings.repo + "/CydiaIcon.png");
                        original.Save(Settings.repo + "/CydiaIcon.png");
                    }
                    else
                    {
                        original = new Bitmap(original, new Size(0x40, 0x40));
                        File.Delete(Settings.repo + "/CydiaIcon.png");
                        original.Save(Settings.repo + "/CydiaIcon.png");
                        MessageBox.Show("The size of this image is not 64x64 so its cropped to 64x64 now.");
                    }
                    this.repo_icon.Image = original;
                }
            }
        }

        public void default_logo()
        {
            Image image = new Bitmap(0x40, 0x40);
            Graphics graphics = Graphics.FromImage(image);
            graphics.Clear(Color.Cyan);
            Brush brush = new SolidBrush(Color.White);
            graphics.DrawString("Repo", new Font("Arial Rounded MT", 15f, FontStyle.Regular, GraphicsUnit.Point), brush, (float) -2f, (float) 6f);
            graphics.DrawString("Manager", new Font("Arial Rounded MT", 11f, FontStyle.Bold, GraphicsUnit.Point), brush, (float) -2f, (float) 25f);
            graphics.DrawString("DELUXE", new Font("Arial Rounded MT", 11f, FontStyle.Bold, GraphicsUnit.Point), brush, (float) -2f, (float) 42f);
            graphics.Save();
            brush.Dispose();
            graphics.Dispose();
            this.repo_icon.Image = image;
            image.Save(Settings.repo + "/CydiaIcon.png");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.packagelist.SelectedItem != null)
            {
                new edit(this.packagelist.SelectedItem.ToString()).ShowDialog();
            }
        }

        private void generate_packages_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Recreating package files...");
            if (!Directory.Exists(Settings.repo + @"\temp"))
            {
                Directory.CreateDirectory(Settings.repo + @"\temp");
            }
            StringBuilder builder = new StringBuilder();
            foreach (string str in this.packagelist.Items)
            {
                string file = str + ".deb";
                string destFileName = Settings.repo + @"\temp\" + file;
                string[] files = Directory.GetFiles(Settings.repo + @"\temp");
                int index = 0;
                while (true)
                {
                    if (index >= files.Length)
                    {
                        File.Copy(Settings.repo + @"\debs\" + file, destFileName);
                        Extract.deb(file, Settings.repo);
                        Extract.deb("control.tar.gz", Settings.repo);
                        string str4 = "no.package.name";
                        string str5 = "???";
                        string[] strArray2 = File.ReadAllLines(Settings.repo + @"\temp\control");
                        int num2 = 0;
                        while (true)
                        {
                            if (num2 >= strArray2.Length)
                            {
                                builder.AppendLine("Filename: debs/" + file);
                                string[] textArray1 = new string[5];
                                textArray1[0] = "Depiction: ";
                                char[] trimChars = new char[] { '/' };
                                textArray1[1] = this.repolink.Text.TrimEnd(trimChars);
                                textArray1[2] = "/depictions/";
                                textArray1[3] = str4;
                                textArray1[4] = ".html";
                                builder.AppendLine(string.Concat(textArray1));
                                if (!File.Exists(Settings.repo + @"\icons\" + str4 + ".png"))
                                {
                                    string[] textArray3 = new string[] { "[WARNING] > [MISSING ICON] >", str4, " (", file, ")" };
                                    this.warning(string.Concat(textArray3));
                                }
                                else
                                {
                                    string[] textArray2 = new string[5];
                                    textArray2[0] = "Icon: ";
                                    char[] chArray2 = new char[] { '/' };
                                    textArray2[1] = this.repolink.Text.TrimEnd(chArray2);
                                    textArray2[2] = "/icons/";
                                    textArray2[3] = str4;
                                    textArray2[4] = ".png";
                                    builder.AppendLine(string.Concat(textArray2));
                                }
                                builder.AppendLine("Size: " + new FileInfo(destFileName).Length.ToString());
                                builder.AppendLine("MD5sum: " + FileHash.getMD5(destFileName));
                                builder.AppendLine("SHA1: " + FileHash.getSHA1(destFileName));
                                builder.AppendLine("SHA256: " + FileHash.getSHA256(destFileName));
                                builder.AppendLine("SHA521: " + FileHash.getSHA521(destFileName));
                                builder.AppendLine("");
                                string[] textArray4 = new string[] { "[info] > ", str4, " (", str5, ") processed." };
                                this.log(string.Concat(textArray4));
                                break;
                            }
                            string str7 = strArray2[num2];
                            if (str7.Contains("Package:"))
                            {
                                str4 = str7.Replace("Package: ", "Package:".Replace("Package:", ""));
                            }
                            else if (str7.Contains("Version:"))
                            {
                                str5 = str7.Replace("Version: ", "Version:".Replace("Version:", ""));
                            }
                            builder.AppendLine(str7);
                            num2++;
                        }
                        break;
                    }
                    string path = files[index];
                    File.Delete(path);
                    index++;
                }
            }
            File.WriteAllText(Settings.repo + "/Packages", builder.ToString().TrimEnd(' '));
            this.success("[success] > Packages file created succesfully.");
        }

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();
        public void hide_logs()
        {
            ShowWindow(GetConsoleWindow(), 0);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.label1 = new Label();
            this.label2 = new Label();
            this.add_package = new Button();
            this.generate_packages = new Button();
            this.reload_packages = new Button();
            this.lock_repo = new Button();
            this.packagelist = new ListBox();
            this.package_options = new ContextMenuStrip(this.components);
            this.editToolStripMenuItem = new ToolStripMenuItem();
            this.removeToolStripMenuItem = new ToolStripMenuItem();
            this.richTextBox1 = new RichTextBox();
            this.showlogs = new Button();
            this.groupBox1 = new GroupBox();
            this.repolink = new TextBox();
            this.label10 = new Label();
            this.label12 = new Label();
            this.open_folder = new Button();
            this.change_icon = new Button();
            this.save_release = new Button();
            this.description = new TextBox();
            this.label9 = new Label();
            this.version = new TextBox();
            this.label8 = new Label();
            this.origin = new TextBox();
            this.label7 = new Label();
            this.codename = new TextBox();
            this.label5 = new Label();
            this.name = new TextBox();
            this.label6 = new Label();
            this.repo_icon = new PictureBox();
            this.label3 = new Label();
            this.label4 = new Label();
            this.settings = new Button();
            this.label11 = new Label();
            this.package_options.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((ISupportInitialize) this.repo_icon).BeginInit();
            base.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Font = new Font("Microsoft Sans Serif", 100f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label1.Location = new Point(560, 9);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x1ee, 0xe2);
            this.label1.TabIndex = 0;
            this.label1.Text = "RDL";
            this.label2.AutoSize = true;
            this.label2.Font = new Font("Microsoft Sans Serif", 16f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label2.Location = new Point(0x272, 0xd9);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x14d, 0x25);
            this.label2.TabIndex = 1;
            this.label2.Text = "Repo Manager Deluxe";
            this.add_package.BackColor = Color.LightGreen;
            this.add_package.FlatStyle = FlatStyle.Flat;
            this.add_package.Location = new Point(12, 0x163);
            this.add_package.Name = "add_package";
            this.add_package.Size = new Size(0x1a0, 0xbc);
            this.add_package.TabIndex = 5;
            this.add_package.Text = "Add Package";
            this.add_package.UseVisualStyleBackColor = false;
            this.add_package.Click += new EventHandler(this.add_package_Click);
            this.generate_packages.BackColor = Color.SkyBlue;
            this.generate_packages.FlatStyle = FlatStyle.Flat;
            this.generate_packages.Location = new Point(0x1c0, 560);
            this.generate_packages.Name = "generate_packages";
            this.generate_packages.Size = new Size(0x1a0, 0xbc);
            this.generate_packages.TabIndex = 6;
            this.generate_packages.Text = "Recreate Packages File";
            this.generate_packages.UseVisualStyleBackColor = false;
            this.generate_packages.Click += new EventHandler(this.generate_packages_Click);
            this.reload_packages.BackColor = Color.LightYellow;
            this.reload_packages.FlatStyle = FlatStyle.Flat;
            this.reload_packages.Location = new Point(0x1c0, 0x163);
            this.reload_packages.Name = "reload_packages";
            this.reload_packages.Size = new Size(0x1a0, 0xbc);
            this.reload_packages.TabIndex = 7;
            this.reload_packages.Text = "Reload Packages";
            this.reload_packages.UseVisualStyleBackColor = false;
            this.reload_packages.Click += new EventHandler(this.reload_packages_Click);
            this.lock_repo.BackColor = Color.MistyRose;
            this.lock_repo.FlatStyle = FlatStyle.Flat;
            this.lock_repo.Location = new Point(12, 560);
            this.lock_repo.Name = "lock_repo";
            this.lock_repo.Size = new Size(0x1a0, 0xbc);
            this.lock_repo.TabIndex = 8;
            this.lock_repo.Text = "Lock Repo";
            this.lock_repo.UseVisualStyleBackColor = false;
            this.lock_repo.Click += new EventHandler(this.lock_repo_Click);
            this.packagelist.ContextMenuStrip = this.package_options;
            this.packagelist.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.packagelist.FormattingEnabled = true;
            this.packagelist.ItemHeight = 0x2e;
            this.packagelist.Location = new Point(0x376, 0x177);
            this.packagelist.Name = "packagelist";
            this.packagelist.Size = new Size(0x2fa, 0x146);
            this.packagelist.TabIndex = 9;
            this.packagelist.DoubleClick += new EventHandler(this.packagelist_DoubleClick);
            this.package_options.ImageScalingSize = new Size(0x18, 0x18);
            ToolStripItem[] toolStripItems = new ToolStripItem[] { this.editToolStripMenuItem, this.removeToolStripMenuItem };
            this.package_options.Items.AddRange(toolStripItems);
            this.package_options.Name = "package_options";
            this.package_options.Size = new Size(0x95, 0x44);
            this.package_options.Opening += new CancelEventHandler(this.package_options_Opening);
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new Size(0x94, 0x20);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.Click += new EventHandler(this.editToolStripMenuItem_Click);
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new Size(0x94, 0x20);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new EventHandler(this.removeToolStripMenuItem_Click);
            this.richTextBox1.Location = new Point(0x45c, 9);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new Size(0x1e4, 0x13f);
            this.richTextBox1.TabIndex = 10;
            this.richTextBox1.Text = "";
            this.richTextBox1.Visible = false;
            this.showlogs.BackColor = Color.White;
            this.showlogs.FlatStyle = FlatStyle.Flat;
            this.showlogs.Location = new Point(220, 12);
            this.showlogs.Name = "showlogs";
            this.showlogs.Size = new Size(0xb2, 0x2d);
            this.showlogs.TabIndex = 11;
            this.showlogs.Text = "Show logs";
            this.showlogs.UseVisualStyleBackColor = false;
            this.showlogs.Click += new EventHandler(this.showlogs_Click);
            this.groupBox1.Controls.Add(this.repolink);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.open_folder);
            this.groupBox1.Controls.Add(this.change_icon);
            this.groupBox1.Controls.Add(this.save_release);
            this.groupBox1.Controls.Add(this.description);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.version);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.origin);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.codename);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.name);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.repo_icon);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new Point(12, 0x12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x1a0, 0x14b);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Your Repo";
            this.repolink.Location = new Point(0x80, 0x120);
            this.repolink.Name = "repolink";
            this.repolink.Size = new Size(0x102, 0x1a);
            this.repolink.TabIndex = 0x1f;
            this.repolink.Text = "https://imnotsatan.github.io/";
            this.repolink.TextChanged += new EventHandler(this.repolink_TextChanged);
            this.label10.AutoSize = true;
            this.label10.Font = new Font("Microsoft Sans Serif", 8f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label10.Location = new Point(0x21, 0x6d);
            this.label10.Name = "label10";
            this.label10.Size = new Size(0x45, 20);
            this.label10.TabIndex = 30;
            this.label10.Text = "(64x64)";
            this.label12.AutoSize = true;
            this.label12.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Underline | FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label12.Location = new Point(0x16, 0x124);
            this.label12.Margin = new Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new Size(0x5d, 20);
            this.label12.TabIndex = 0x10;
            this.label12.Text = "Repo Link";
            this.open_folder.BackColor = Color.White;
            this.open_folder.FlatStyle = FlatStyle.Flat;
            this.open_folder.Location = new Point(6, 0x19);
            this.open_folder.Name = "open_folder";
            this.open_folder.Size = new Size(0xb8, 0x20);
            this.open_folder.TabIndex = 0x1d;
            this.open_folder.Text = "OPEN REPO FOLDER";
            this.open_folder.UseVisualStyleBackColor = false;
            this.open_folder.Click += new EventHandler(this.open_folder_Click);
            this.change_icon.BackColor = Color.White;
            this.change_icon.FlatStyle = FlatStyle.Flat;
            this.change_icon.Location = new Point(0x15, 0xee);
            this.change_icon.Name = "change_icon";
            this.change_icon.Size = new Size(100, 0x20);
            this.change_icon.TabIndex = 0x1c;
            this.change_icon.Text = "CHANGE";
            this.change_icon.UseVisualStyleBackColor = false;
            this.change_icon.Click += new EventHandler(this.change_icon_Click);
            this.save_release.BackColor = Color.White;
            this.save_release.FlatStyle = FlatStyle.Flat;
            this.save_release.Location = new Point(0xf6, 0xee);
            this.save_release.Name = "save_release";
            this.save_release.Size = new Size(0x8d, 0x20);
            this.save_release.TabIndex = 0x1b;
            this.save_release.Text = "SAVE";
            this.save_release.UseVisualStyleBackColor = false;
            this.save_release.Click += new EventHandler(this.save_release_Click);
            this.description.Location = new Point(0xf6, 0x6f);
            this.description.Name = "description";
            this.description.Size = new Size(140, 0x1a);
            this.description.TabIndex = 0x1a;
            this.label9.AutoSize = true;
            this.label9.Location = new Point(0x9a, 0x72);
            this.label9.Name = "label9";
            this.label9.Size = new Size(0x59, 20);
            this.label9.TabIndex = 0x19;
            this.label9.Text = "Description";
            this.version.Location = new Point(0xf6, 0xd0);
            this.version.Name = "version";
            this.version.Size = new Size(140, 0x1a);
            this.version.TabIndex = 0x18;
            this.label8.AutoSize = true;
            this.label8.Location = new Point(0x9a, 0xd1);
            this.label8.Name = "label8";
            this.label8.Size = new Size(0x3f, 20);
            this.label8.TabIndex = 0x17;
            this.label8.Text = "Version";
            this.origin.Location = new Point(0xf6, 0xaf);
            this.origin.Name = "origin";
            this.origin.Size = new Size(140, 0x1a);
            this.origin.TabIndex = 0x16;
            this.label7.AutoSize = true;
            this.label7.Location = new Point(0x9a, 0xb2);
            this.label7.Name = "label7";
            this.label7.Size = new Size(50, 20);
            this.label7.TabIndex = 0x15;
            this.label7.Text = "Origin";
            this.codename.Location = new Point(0xf6, 0x8f);
            this.codename.Name = "codename";
            this.codename.Size = new Size(140, 0x1a);
            this.codename.TabIndex = 20;
            this.label5.AutoSize = true;
            this.label5.Location = new Point(0x9a, 0x92);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x57, 20);
            this.label5.TabIndex = 0x13;
            this.label5.Text = "Codename";
            this.name.Location = new Point(0xf6, 0x4e);
            this.name.Name = "name";
            this.name.Size = new Size(140, 0x1a);
            this.name.TabIndex = 0x12;
            this.label6.AutoSize = true;
            this.label6.Location = new Point(0x9a, 0x52);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x33, 20);
            this.label6.TabIndex = 0x10;
            this.label6.Text = "Name";
            this.repo_icon.Location = new Point(0x26, 0x98);
            this.repo_icon.Name = "repo_icon";
            this.repo_icon.Size = new Size(0x40, 0x41);
            this.repo_icon.SizeMode = PictureBoxSizeMode.StretchImage;
            this.repo_icon.TabIndex = 14;
            this.repo_icon.TabStop = false;
            this.label3.AutoSize = true;
            this.label3.Font = new Font("Microsoft Sans Serif", 8f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label3.Location = new Point(15, 0x59);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x6a, 20);
            this.label3.TabIndex = 13;
            this.label3.Text = "REPO ICON";
            this.label4.AutoSize = true;
            this.label4.Font = new Font("Microsoft Sans Serif", 8f, FontStyle.Underline | FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label4.Location = new Point(0x372, 0x160);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x116, 20);
            this.label4.TabIndex = 13;
            this.label4.Text = "Packages (Right click for options)";
            this.settings.BackColor = Color.White;
            this.settings.FlatStyle = FlatStyle.Flat;
            this.settings.Location = new Point(0x2c8, 0x10d);
            this.settings.Name = "settings";
            this.settings.Size = new Size(0xac, 0x2d);
            this.settings.TabIndex = 14;
            this.settings.Text = "⚙ SETTINGS";
            this.settings.UseVisualStyleBackColor = false;
            this.settings.Click += new EventHandler(this.settings_Click);
            this.label11.AutoSize = true;
            this.label11.Location = new Point(0x3b1, 0x61);
            this.label11.Name = "label11";
            this.label11.Size = new Size(0x33, 20);
            this.label11.TabIndex = 15;
            this.label11.Text = "v0.0.1";
            base.AutoScaleDimensions = new SizeF(9f, 20f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x67c, 0x314);
            base.Controls.Add(this.label11);
            base.Controls.Add(this.settings);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.showlogs);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.richTextBox1);
            base.Controls.Add(this.packagelist);
            base.Controls.Add(this.lock_repo);
            base.Controls.Add(this.reload_packages);
            base.Controls.Add(this.generate_packages);
            base.Controls.Add(this.add_package);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Name = "Main";
            base.ShowIcon = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.package_options.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((ISupportInitialize) this.repo_icon).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public void loaddebs()
        {
            if (!File.Exists(@"C:\Windows\System32\tar.exe"))
            {
                MessageBox.Show(@"You dont have TAR installed in: [C:\Windows\System32\], things can get problematic.");
            }
            if (!Directory.Exists(Settings.repo + @"\debs"))
            {
                Directory.CreateDirectory(Settings.repo + @"\debs");
            }
            if (!Directory.Exists(Settings.repo + @"\icons"))
            {
                Directory.CreateDirectory(Settings.repo + @"\icons");
            }
            if (!Directory.Exists(Settings.repo + @"\temp"))
            {
                Directory.CreateDirectory(Settings.repo + @"\temp");
            }
            if (!Directory.Exists(Settings.repo + @"\depictions"))
            {
                Directory.CreateDirectory(Settings.repo + @"\depictions");
            }
            this.packagelist.Items.Clear();
            foreach (string str in Directory.GetFiles(Settings.repo + @"\debs"))
            {
                this.packagelist.Items.Add(str.Replace(Settings.repo + @"\debs\", "").Replace(".deb", ""));
            }
        }

        private void lock_repo_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Next version wil have this.");
        }

        public void log(string text)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        private void open_folder_Click(object sender, EventArgs e)
        {
            Process.Start(Settings.repo);
        }

        private void package_options_Opening(object sender, CancelEventArgs e)
        {
            if (this.packagelist.SelectedItem == null)
            {
                e.Cancel = true;
            }
        }

        private void packagelist_DoubleClick(object sender, EventArgs e)
        {
            if (this.packagelist.SelectedItem != null)
            {
                new edit(this.packagelist.SelectedItem.ToString()).ShowDialog();
            }
        }

        private void reload_packages_Click(object sender, EventArgs e)
        {
            this.loaddebs();
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((this.packagelist.SelectedItem != null) && File.Exists(Settings.repo + "/debs/" + this.packagelist.SelectedItem.ToString() + ".deb"))
            {
                if (MessageBox.Show("Do you also want to delete the depiction and pictures?\r\nIf not you can remove them yourself from the folder manually.", "Depiction and Picture", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    string[] files = Directory.GetFiles(Settings.repo + @"\temp");
                    int index = 0;
                    while (true)
                    {
                        if (index >= files.Length)
                        {
                            File.Copy(Settings.repo + @"\debs\" + this.packagelist.SelectedItem.ToString() + ".deb", Settings.repo + @"\temp\" + this.packagelist.SelectedItem.ToString() + ".deb");
                            Extract.deb(this.packagelist.SelectedItem.ToString() + ".deb", Settings.repo);
                            Extract.deb("control.tar.gz", Settings.repo);
                            string str = "no.package.name";
                            foreach (string str3 in File.ReadAllLines(Settings.repo + @"\temp\control"))
                            {
                                if (str3.Contains("Package:"))
                                {
                                    str = str3.Replace("Package: ", "Package:").Replace("Package:", "");
                                    File.Delete(Settings.repo + "/icons/" + str + ".png");
                                    File.Delete(Settings.repo + "/depictions/" + str + ".html");
                                }
                            }
                            break;
                        }
                        string path = files[index];
                        File.Delete(path);
                        index++;
                    }
                }
                File.Delete(Settings.repo + "/debs/" + this.packagelist.SelectedItem.ToString() + ".deb");
                this.packagelist.Items.RemoveAt(this.packagelist.SelectedIndex);
            }
        }

        private void repolink_TextChanged(object sender, EventArgs e)
        {
            string text = this.repolink.Text;
            if (!text.Contains("://"))
            {
                char[] trimChars = new char[] { '/' };
                text = "https://" + text.Replace("https:/", "").Replace("http:/", "").Replace("//", "/").TrimEnd(trimChars) + "/";
            }
            File.WriteAllText(Settings.repo + "/repo.link", text);
        }

        private void save_release_Click(object sender, EventArgs e)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Origin: " + this.origin.Text);
            builder.AppendLine("Label: " + this.name.Text);
            builder.AppendLine("Suite: stable");
            builder.AppendLine("Version: " + this.version.Text);
            builder.AppendLine("Codename: " + this.codename.Text);
            builder.AppendLine("Architectures: iphoneos-arm");
            builder.AppendLine("Components: main");
            builder.AppendLine("Description: " + this.description.Text);
            File.WriteAllText(Settings.repo + "/Release", builder.ToString());
        }

        private void settings_Click(object sender, EventArgs e)
        {
            new config().ShowDialog();
        }

        public void show_logs()
        {
            ShowWindow(GetConsoleWindow(), 5);
        }

        private void showlogs_Click(object sender, EventArgs e)
        {
            if (this.showlogs.Text == "Show logs")
            {
                this.show_logs();
                this.showlogs.Text = "Hide logs";
            }
            else
            {
                this.hide_logs();
                this.showlogs.Text = "Show logs";
            }
        }

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        public void success(string text)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        public void warning(string text)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}

