namespace Repo_Manager_Deluxe
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class config : Form
    {
        private IContainer components = null;
        private Label label1;
        private Label label2;
        private RichTextBox richTextBox1;

        public config()
        {
            this.InitializeComponent();
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
            this.richTextBox1 = new RichTextBox();
            base.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Font = new Font("Microsoft Sans Serif", 32f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label1.Location = new Point(230, 0x1c);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x161, 0x49);
            this.label1.TabIndex = 1;
            this.label1.Text = "SETTINGS";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x1c, 0x88);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0xa9, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Default depiction page";
            this.richTextBox1.Enabled = false;
            this.richTextBox1.Location = new Point(0x20, 0xb2);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new Size(0x2ea, 260);
            this.richTextBox1.TabIndex = 3;
            this.richTextBox1.Text = "SOON";
            base.AutoScaleDimensions = new SizeF(9f, 20f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(800, 0x1d5);
            base.Controls.Add(this.richTextBox1);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Name = "config";
            base.ShowIcon = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}

