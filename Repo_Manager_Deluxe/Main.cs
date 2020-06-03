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
            if(!File.Exists("index.html") && !File.Exists("index.php"))
            {
                File.WriteAllText("index.html", "<!--RDM--><h1><center><a href='" + repolink.Text + "'>Add my Repo</a><br>By Repo Manager Deluxe.</center></h1>");
            }
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
            //Extract.create_packages_bz2("Packages", Settings.repo); //soon
            if(File.Exists("bz2extention.exe") && File.Exists("ICSharpCode.SharpZipLib.dll"))
            {
                Process.Start("bz2extention.exe");
            }
            else
            {
                MessageBox.Show("bz2 extention not installed, did not create Packages.bz2");
            }
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.add_package = new System.Windows.Forms.Button();
            this.generate_packages = new System.Windows.Forms.Button();
            this.reload_packages = new System.Windows.Forms.Button();
            this.lock_repo = new System.Windows.Forms.Button();
            this.packagelist = new System.Windows.Forms.ListBox();
            this.package_options = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.showlogs = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.repolink = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.open_folder = new System.Windows.Forms.Button();
            this.change_icon = new System.Windows.Forms.Button();
            this.save_release = new System.Windows.Forms.Button();
            this.description = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.version = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.origin = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.codename = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.name = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.repo_icon = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.settings = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.package_options.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.repo_icon)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 100F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(373, 6);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(334, 153);
            this.label1.TabIndex = 0;
            this.label1.Text = "RDL";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(417, 141);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(229, 26);
            this.label2.TabIndex = 1;
            this.label2.Text = "Repo Manager Deluxe";
            // 
            // add_package
            // 
            this.add_package.BackColor = System.Drawing.Color.LightGreen;
            this.add_package.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.add_package.Location = new System.Drawing.Point(8, 231);
            this.add_package.Margin = new System.Windows.Forms.Padding(2);
            this.add_package.Name = "add_package";
            this.add_package.Size = new System.Drawing.Size(277, 122);
            this.add_package.TabIndex = 5;
            this.add_package.Text = "Add Package";
            this.add_package.UseVisualStyleBackColor = false;
            this.add_package.Click += new System.EventHandler(this.add_package_Click);
            // 
            // generate_packages
            // 
            this.generate_packages.BackColor = System.Drawing.Color.SkyBlue;
            this.generate_packages.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.generate_packages.Location = new System.Drawing.Point(299, 364);
            this.generate_packages.Margin = new System.Windows.Forms.Padding(2);
            this.generate_packages.Name = "generate_packages";
            this.generate_packages.Size = new System.Drawing.Size(277, 122);
            this.generate_packages.TabIndex = 6;
            this.generate_packages.Text = "Recreate Packages File";
            this.generate_packages.UseVisualStyleBackColor = false;
            this.generate_packages.Click += new System.EventHandler(this.generate_packages_Click);
            // 
            // reload_packages
            // 
            this.reload_packages.BackColor = System.Drawing.Color.LightYellow;
            this.reload_packages.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.reload_packages.Location = new System.Drawing.Point(299, 231);
            this.reload_packages.Margin = new System.Windows.Forms.Padding(2);
            this.reload_packages.Name = "reload_packages";
            this.reload_packages.Size = new System.Drawing.Size(277, 122);
            this.reload_packages.TabIndex = 7;
            this.reload_packages.Text = "Reload Packages";
            this.reload_packages.UseVisualStyleBackColor = false;
            this.reload_packages.Click += new System.EventHandler(this.reload_packages_Click);
            // 
            // lock_repo
            // 
            this.lock_repo.BackColor = System.Drawing.Color.MistyRose;
            this.lock_repo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lock_repo.Location = new System.Drawing.Point(8, 364);
            this.lock_repo.Margin = new System.Windows.Forms.Padding(2);
            this.lock_repo.Name = "lock_repo";
            this.lock_repo.Size = new System.Drawing.Size(277, 122);
            this.lock_repo.TabIndex = 8;
            this.lock_repo.Text = "Lock Repo";
            this.lock_repo.UseVisualStyleBackColor = false;
            this.lock_repo.Click += new System.EventHandler(this.lock_repo_Click);
            // 
            // packagelist
            // 
            this.packagelist.ContextMenuStrip = this.package_options;
            this.packagelist.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.packagelist.FormattingEnabled = true;
            this.packagelist.ItemHeight = 31;
            this.packagelist.Location = new System.Drawing.Point(591, 244);
            this.packagelist.Margin = new System.Windows.Forms.Padding(2);
            this.packagelist.Name = "packagelist";
            this.packagelist.Size = new System.Drawing.Size(509, 190);
            this.packagelist.TabIndex = 9;
            this.packagelist.DoubleClick += new System.EventHandler(this.packagelist_DoubleClick);
            // 
            // package_options
            // 
            this.package_options.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.package_options.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem,
            this.removeToolStripMenuItem});
            this.package_options.Name = "package_options";
            this.package_options.Size = new System.Drawing.Size(118, 48);
            this.package_options.Opening += new System.ComponentModel.CancelEventHandler(this.package_options_Opening);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(744, 6);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(2);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(324, 209);
            this.richTextBox1.TabIndex = 10;
            this.richTextBox1.Text = "";
            this.richTextBox1.Visible = false;
            // 
            // showlogs
            // 
            this.showlogs.BackColor = System.Drawing.Color.White;
            this.showlogs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.showlogs.Location = new System.Drawing.Point(147, 8);
            this.showlogs.Margin = new System.Windows.Forms.Padding(2);
            this.showlogs.Name = "showlogs";
            this.showlogs.Size = new System.Drawing.Size(119, 29);
            this.showlogs.TabIndex = 11;
            this.showlogs.Text = "Show logs";
            this.showlogs.UseVisualStyleBackColor = false;
            this.showlogs.Click += new System.EventHandler(this.showlogs_Click);
            // 
            // groupBox1
            // 
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
            this.groupBox1.Location = new System.Drawing.Point(8, 12);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(277, 215);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Your Repo";
            // 
            // repolink
            // 
            this.repolink.Location = new System.Drawing.Point(85, 187);
            this.repolink.Margin = new System.Windows.Forms.Padding(2);
            this.repolink.Name = "repolink";
            this.repolink.Size = new System.Drawing.Size(173, 20);
            this.repolink.TabIndex = 31;
            this.repolink.Text = "https://imnotsatan.github.io/";
            this.repolink.TextChanged += new System.EventHandler(this.repolink_TextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(22, 71);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(49, 13);
            this.label10.TabIndex = 30;
            this.label10.Text = "(64x64)";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(15, 190);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(65, 13);
            this.label12.TabIndex = 16;
            this.label12.Text = "Repo Link";
            // 
            // open_folder
            // 
            this.open_folder.BackColor = System.Drawing.Color.White;
            this.open_folder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.open_folder.Location = new System.Drawing.Point(4, 16);
            this.open_folder.Margin = new System.Windows.Forms.Padding(2);
            this.open_folder.Name = "open_folder";
            this.open_folder.Size = new System.Drawing.Size(123, 21);
            this.open_folder.TabIndex = 29;
            this.open_folder.Text = "OPEN REPO FOLDER";
            this.open_folder.UseVisualStyleBackColor = false;
            this.open_folder.Click += new System.EventHandler(this.open_folder_Click);
            // 
            // change_icon
            // 
            this.change_icon.BackColor = System.Drawing.Color.White;
            this.change_icon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.change_icon.Location = new System.Drawing.Point(14, 155);
            this.change_icon.Margin = new System.Windows.Forms.Padding(2);
            this.change_icon.Name = "change_icon";
            this.change_icon.Size = new System.Drawing.Size(67, 21);
            this.change_icon.TabIndex = 28;
            this.change_icon.Text = "CHANGE";
            this.change_icon.UseVisualStyleBackColor = false;
            this.change_icon.Click += new System.EventHandler(this.change_icon_Click);
            // 
            // save_release
            // 
            this.save_release.BackColor = System.Drawing.Color.White;
            this.save_release.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.save_release.Location = new System.Drawing.Point(164, 155);
            this.save_release.Margin = new System.Windows.Forms.Padding(2);
            this.save_release.Name = "save_release";
            this.save_release.Size = new System.Drawing.Size(94, 21);
            this.save_release.TabIndex = 27;
            this.save_release.Text = "SAVE";
            this.save_release.UseVisualStyleBackColor = false;
            this.save_release.Click += new System.EventHandler(this.save_release_Click);
            // 
            // description
            // 
            this.description.Location = new System.Drawing.Point(164, 72);
            this.description.Margin = new System.Windows.Forms.Padding(2);
            this.description.Name = "description";
            this.description.Size = new System.Drawing.Size(95, 20);
            this.description.TabIndex = 26;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(103, 74);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(60, 13);
            this.label9.TabIndex = 25;
            this.label9.Text = "Description";
            // 
            // version
            // 
            this.version.Location = new System.Drawing.Point(164, 135);
            this.version.Margin = new System.Windows.Forms.Padding(2);
            this.version.Name = "version";
            this.version.Size = new System.Drawing.Size(95, 20);
            this.version.TabIndex = 24;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(103, 136);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(42, 13);
            this.label8.TabIndex = 23;
            this.label8.Text = "Version";
            // 
            // origin
            // 
            this.origin.Location = new System.Drawing.Point(164, 114);
            this.origin.Margin = new System.Windows.Forms.Padding(2);
            this.origin.Name = "origin";
            this.origin.Size = new System.Drawing.Size(95, 20);
            this.origin.TabIndex = 22;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(103, 116);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(34, 13);
            this.label7.TabIndex = 21;
            this.label7.Text = "Origin";
            // 
            // codename
            // 
            this.codename.Location = new System.Drawing.Point(164, 93);
            this.codename.Margin = new System.Windows.Forms.Padding(2);
            this.codename.Name = "codename";
            this.codename.Size = new System.Drawing.Size(95, 20);
            this.codename.TabIndex = 20;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(103, 95);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 19;
            this.label5.Text = "Codename";
            // 
            // name
            // 
            this.name.Location = new System.Drawing.Point(164, 51);
            this.name.Margin = new System.Windows.Forms.Padding(2);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(95, 20);
            this.name.TabIndex = 18;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(103, 53);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Name";
            // 
            // repo_icon
            // 
            this.repo_icon.Location = new System.Drawing.Point(25, 99);
            this.repo_icon.Margin = new System.Windows.Forms.Padding(2);
            this.repo_icon.Name = "repo_icon";
            this.repo_icon.Size = new System.Drawing.Size(43, 42);
            this.repo_icon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.repo_icon.TabIndex = 14;
            this.repo_icon.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(10, 58);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "REPO ICON";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(588, 229);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(200, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Packages (Right click for options)";
            // 
            // settings
            // 
            this.settings.BackColor = System.Drawing.Color.White;
            this.settings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.settings.Location = new System.Drawing.Point(475, 175);
            this.settings.Margin = new System.Windows.Forms.Padding(2);
            this.settings.Name = "settings";
            this.settings.Size = new System.Drawing.Size(115, 29);
            this.settings.TabIndex = 14;
            this.settings.Text = "⚙ SETTINGS";
            this.settings.UseVisualStyleBackColor = false;
            this.settings.Click += new System.EventHandler(this.settings_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(630, 63);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(37, 13);
            this.label11.TabIndex = 15;
            this.label11.Text = "v0.0.4";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1107, 512);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.settings);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.showlogs);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.packagelist);
            this.Controls.Add(this.lock_repo);
            this.Controls.Add(this.reload_packages);
            this.Controls.Add(this.generate_packages);
            this.Controls.Add(this.add_package);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Main";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.package_options.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.repo_icon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
            if(File.Exists("index.html"))
            {
                if(File.ReadAllText("index.html").Contains("<!--RDM-->"))
                {
                    File.WriteAllText("index.html", "<!--RDM--><h1><center><a href='" + repolink.Text + "'>Add my Repo</a><br>By Repo Manager Deluxe.</center></h1>");
                }
            }
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

