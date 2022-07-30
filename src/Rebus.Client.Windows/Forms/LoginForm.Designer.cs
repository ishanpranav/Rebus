// Ishan Pranav's REBUS: ConnectionForm.Designer.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Windows.Forms;

namespace Rebus.Client.Windows.Forms
{
    partial class LoginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.submitButton = new System.Windows.Forms.Button();
            this.registerTabPage = new System.Windows.Forms.TabPage();
            this.registerEntityGrid = new Rebus.Client.Windows.EntityGrid();
            this.loginTabPage = new System.Windows.Forms.TabPage();
            this.loginEntityGrid = new Rebus.Client.Windows.EntityGrid();
            this.myTabControl = new System.Windows.Forms.TabControl();
            this.aboutButton = new System.Windows.Forms.Button();
            this.registerTabPage.SuspendLayout();
            this.loginTabPage.SuspendLayout();
            this.myTabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // submitButton
            // 
            resources.ApplyResources(this.submitButton, "submitButton");
            this.submitButton.Name = "submitButton";
            this.submitButton.UseVisualStyleBackColor = true;
            this.submitButton.Click += new System.EventHandler(this.OnSubmitButtonClick);
            // 
            // registerTabPage
            // 
            this.registerTabPage.Controls.Add(this.registerEntityGrid);
            resources.ApplyResources(this.registerTabPage, "registerTabPage");
            this.registerTabPage.Name = "registerTabPage";
            this.registerTabPage.UseVisualStyleBackColor = true;
            // 
            // registerEntityGrid
            // 
            resources.ApplyResources(this.registerEntityGrid, "registerEntityGrid");
            this.registerEntityGrid.Name = "registerEntityGrid";
            this.registerEntityGrid.Saver = null;
            this.registerEntityGrid.SelectedObject = null;
            // 
            // loginTabPage
            // 
            this.loginTabPage.Controls.Add(this.loginEntityGrid);
            resources.ApplyResources(this.loginTabPage, "loginTabPage");
            this.loginTabPage.Name = "loginTabPage";
            this.loginTabPage.UseVisualStyleBackColor = true;
            // 
            // loginEntityGrid
            // 
            resources.ApplyResources(this.loginEntityGrid, "loginEntityGrid");
            this.loginEntityGrid.Name = "loginEntityGrid";
            this.loginEntityGrid.Saver = null;
            this.loginEntityGrid.SelectedObject = null;
            // 
            // myTabControl
            // 
            resources.ApplyResources(this.myTabControl, "myTabControl");
            this.myTabControl.Controls.Add(this.loginTabPage);
            this.myTabControl.Controls.Add(this.registerTabPage);
            this.myTabControl.Name = "myTabControl";
            this.myTabControl.SelectedIndex = 0;
            // 
            // aboutButton
            // 
            resources.ApplyResources(this.aboutButton, "aboutButton");
            this.aboutButton.Name = "aboutButton";
            this.aboutButton.UseVisualStyleBackColor = true;
            this.aboutButton.Click += new System.EventHandler(this.OnAboutButtonClick);
            // 
            // LoginForm
            // 
            this.AcceptButton = this.submitButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.Controls.Add(this.aboutButton);
            this.Controls.Add(this.myTabControl);
            this.Controls.Add(this.submitButton);
            this.MaximizeBox = false;
            this.Name = "LoginForm";
            this.registerTabPage.ResumeLayout(false);
            this.loginTabPage.ResumeLayout(false);
            this.myTabControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Button submitButton;
        private TabPage registerTabPage;
        private EntityGrid registerEntityGrid;
        private TabPage loginTabPage;
        private EntityGrid loginEntityGrid;
        private TabControl myTabControl;
        private Button aboutButton;
    }
}
