// Ishan Pranav's REBUS: MainForm.Designer.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Windows.Forms;

namespace Rebus.Client.Windows.Forms
{
    partial class GameForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameForm));
            this.visionPictureBox = new System.Windows.Forms.PictureBox();
            this.myToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.usernameToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.wealthToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.messageToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.unitDataGridView = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.submitButton = new System.Windows.Forms.Button();
            this.destinationComboBox = new System.Windows.Forms.ComboBox();
            this.commandComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lensComboBox = new System.Windows.Forms.ComboBox();
            this.myNotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.visionPictureBox)).BeginInit();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.unitDataGridView)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // visionPictureBox
            // 
            this.visionPictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.visionPictureBox, "visionPictureBox");
            this.visionPictureBox.Name = "visionPictureBox";
            this.visionPictureBox.TabStop = false;
            this.visionPictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnVisionPictureBoxMouseClick);
            // 
            // myToolTip
            // 
            this.myToolTip.IsBalloon = true;
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.visionPictureBox);
            this.panel1.Name = "panel1";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.usernameToolStripLabel,
            this.wealthToolStripLabel,
            this.messageToolStripButton});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            // 
            // usernameToolStripLabel
            // 
            this.usernameToolStripLabel.Name = "usernameToolStripLabel";
            resources.ApplyResources(this.usernameToolStripLabel, "usernameToolStripLabel");
            // 
            // wealthToolStripLabel
            // 
            this.wealthToolStripLabel.Name = "wealthToolStripLabel";
            resources.ApplyResources(this.wealthToolStripLabel, "wealthToolStripLabel");
            // 
            // messageToolStripButton
            // 
            this.messageToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.messageToolStripButton, "messageToolStripButton");
            this.messageToolStripButton.Name = "messageToolStripButton";
            this.messageToolStripButton.Click += new System.EventHandler(this.OnMessageToolStripButtonClick);
            // 
            // unitDataGridView
            // 
            this.unitDataGridView.AllowUserToAddRows = false;
            this.unitDataGridView.AllowUserToDeleteRows = false;
            this.unitDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.unitDataGridView, "unitDataGridView");
            this.unitDataGridView.Name = "unitDataGridView";
            this.unitDataGridView.RowTemplate.Height = 25;
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.unitDataGridView);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.submitButton);
            this.groupBox2.Controls.Add(this.destinationComboBox);
            this.groupBox2.Controls.Add(this.commandComboBox);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // submitButton
            // 
            resources.ApplyResources(this.submitButton, "submitButton");
            this.submitButton.Name = "submitButton";
            this.submitButton.UseVisualStyleBackColor = true;
            this.submitButton.Click += new System.EventHandler(this.OnSubmitButtonClick);
            // 
            // destinationComboBox
            // 
            resources.ApplyResources(this.destinationComboBox, "destinationComboBox");
            this.destinationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.destinationComboBox.FormattingEnabled = true;
            this.destinationComboBox.Name = "destinationComboBox";
            this.destinationComboBox.Format += new System.Windows.Forms.ListControlConvertEventHandler(this.OnDestinationComboBoxFormat);
            // 
            // commandComboBox
            // 
            resources.ApplyResources(this.commandComboBox, "commandComboBox");
            this.commandComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.commandComboBox.FormattingEnabled = true;
            this.commandComboBox.Name = "commandComboBox";
            this.commandComboBox.SelectedIndexChanged += new System.EventHandler(this.OnCommandComboBoxSelectedIndexChanged);
            this.commandComboBox.Format += new System.Windows.Forms.ListControlConvertEventHandler(this.OnFormat);
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.lensComboBox);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // lensComboBox
            // 
            resources.ApplyResources(this.lensComboBox, "lensComboBox");
            this.lensComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lensComboBox.FormattingEnabled = true;
            this.lensComboBox.Name = "lensComboBox";
            this.lensComboBox.SelectedIndexChanged += new System.EventHandler(this.OnLensComboBoxSelectedIndexChanged);
            this.lensComboBox.Format += new System.Windows.Forms.ListControlConvertEventHandler(this.OnFormat);
            // 
            // myNotifyIcon
            // 
            resources.ApplyResources(this.myNotifyIcon, "myNotifyIcon");
            // 
            // GameForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.panel1);
            this.Name = "GameForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnMainFormClosing);
            this.Load += new System.EventHandler(this.OnMainFormLoad);
            ((System.ComponentModel.ISupportInitialize)(this.visionPictureBox)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.unitDataGridView)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PictureBox visionPictureBox;
        private ToolTip myToolTip;
        private Panel panel1;
        private ToolStrip toolStrip1;
        private ToolStripLabel usernameToolStripLabel;
        private ToolStripLabel wealthToolStripLabel;
        private ToolStripButton messageToolStripButton;
        private DataGridView unitDataGridView;
        private NotifyIcon myNotifyIcon;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Button submitButton;
        private ComboBox destinationComboBox;
        private ComboBox commandComboBox;
        private GroupBox groupBox3;
        private ComboBox lensComboBox;
    }
}
