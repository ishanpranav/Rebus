// Ishan Pranav's REBUS: AboutForm.Designer.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

namespace Rebus.Client.Windows.Forms
{
    partial class AboutForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.myPictureBox = new System.Windows.Forms.PictureBox();
            this.titleLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.headerLabel = new System.Windows.Forms.Label();
            this.myTextBox = new System.Windows.Forms.RichTextBox();
            this.myListBox = new System.Windows.Forms.ListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.environmentVersionLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.myPictureBox)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // myPictureBox
            // 
            resources.ApplyResources(this.myPictureBox, "myPictureBox");
            this.myPictureBox.Name = "myPictureBox";
            this.myPictureBox.TabStop = false;
            // 
            // titleLabel
            // 
            resources.ApplyResources(this.titleLabel, "titleLabel");
            this.titleLabel.Name = "titleLabel";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.headerLabel);
            this.groupBox1.Controls.Add(this.myTextBox);
            this.groupBox1.Controls.Add(this.myListBox);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // headerLabel
            // 
            resources.ApplyResources(this.headerLabel, "headerLabel");
            this.headerLabel.AutoEllipsis = true;
            this.headerLabel.Name = "headerLabel";
            // 
            // myTextBox
            // 
            resources.ApplyResources(this.myTextBox, "myTextBox");
            this.myTextBox.Name = "myTextBox";
            this.myTextBox.ReadOnly = true;
            // 
            // myListBox
            // 
            resources.ApplyResources(this.myListBox, "myListBox");
            this.myListBox.FormattingEnabled = true;
            this.myListBox.Name = "myListBox";
            this.myListBox.SelectedIndexChanged += new System.EventHandler(this.OnListBoxSelectedIndexChanged);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // environmentVersionLabel
            // 
            resources.ApplyResources(this.environmentVersionLabel, "environmentVersionLabel");
            this.environmentVersionLabel.Name = "environmentVersionLabel";
            // 
            // AboutForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.environmentVersionLabel);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.myPictureBox);
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.Load += new System.EventHandler(this.OnAboutFormLoad);
            ((System.ComponentModel.ISupportInitialize)(this.myPictureBox)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox myPictureBox;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RichTextBox myTextBox;
        private System.Windows.Forms.ListBox myListBox;
        private System.Windows.Forms.Label headerLabel;
        private System.Windows.Forms.Label environmentVersionLabel;
    }
}
