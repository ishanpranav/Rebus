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
            this.creditsToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.unitListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.commandGroupBox = new System.Windows.Forms.GroupBox();
            this.submitButton = new System.Windows.Forms.Button();
            this.destinationComboBox = new System.Windows.Forms.ComboBox();
            this.commandComboBox = new System.Windows.Forms.ComboBox();
            this.lensGroupBox = new System.Windows.Forms.GroupBox();
            this.lensComboBox = new System.Windows.Forms.ComboBox();
            this.myNotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.visionPictureBox)).BeginInit();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.commandGroupBox.SuspendLayout();
            this.lensGroupBox.SuspendLayout();
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
            this.creditsToolStripLabel});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            // 
            // usernameToolStripLabel
            // 
            this.usernameToolStripLabel.Name = "usernameToolStripLabel";
            resources.ApplyResources(this.usernameToolStripLabel, "usernameToolStripLabel");
            // 
            // creditsToolStripLabel
            // 
            this.creditsToolStripLabel.Name = "creditsToolStripLabel";
            resources.ApplyResources(this.creditsToolStripLabel, "creditsToolStripLabel");
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.unitListView);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // unitListView
            // 
            this.unitListView.AllowColumnReorder = true;
            this.unitListView.CheckBoxes = true;
            this.unitListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            resources.ApplyResources(this.unitListView, "unitListView");
            this.unitListView.Name = "unitListView";
            this.unitListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.unitListView.UseCompatibleStateImageBehavior = false;
            this.unitListView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // columnHeader2
            // 
            resources.ApplyResources(this.columnHeader2, "columnHeader2");
            // 
            // columnHeader3
            // 
            resources.ApplyResources(this.columnHeader3, "columnHeader3");
            // 
            // commandGroupBox
            // 
            resources.ApplyResources(this.commandGroupBox, "commandGroupBox");
            this.commandGroupBox.Controls.Add(this.submitButton);
            this.commandGroupBox.Controls.Add(this.destinationComboBox);
            this.commandGroupBox.Controls.Add(this.commandComboBox);
            this.commandGroupBox.Name = "commandGroupBox";
            this.commandGroupBox.TabStop = false;
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
            // lensGroupBox
            // 
            resources.ApplyResources(this.lensGroupBox, "lensGroupBox");
            this.lensGroupBox.Controls.Add(this.lensComboBox);
            this.lensGroupBox.Name = "lensGroupBox";
            this.lensGroupBox.TabStop = false;
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
            this.Controls.Add(this.lensGroupBox);
            this.Controls.Add(this.commandGroupBox);
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
            this.groupBox1.ResumeLayout(false);
            this.commandGroupBox.ResumeLayout(false);
            this.lensGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PictureBox visionPictureBox;
        private ToolTip myToolTip;
        private Panel panel1;
        private ToolStrip toolStrip1;
        private ToolStripLabel usernameToolStripLabel;
        private ToolStripLabel creditsToolStripLabel;
        private NotifyIcon myNotifyIcon;
        private GroupBox groupBox1;
        private GroupBox commandGroupBox;
        private Button submitButton;
        private ComboBox destinationComboBox;
        private ComboBox commandComboBox;
        private GroupBox lensGroupBox;
        private ComboBox lensComboBox;
        private ListView unitListView;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
    }
}
