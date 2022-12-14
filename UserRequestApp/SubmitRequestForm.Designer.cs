// 
// Copyright © 2019, Patrick S. Seymour
// Licensed under the GNU General Public License, version 3.
// See the LICENSE file in the project root for full license information.  
//
// This file is part of PanFlip.
//
// PanFlip is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, version 3.
//
// PanFlip is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with PanFlip. If not, see <http://www.gnu.org/licenses/>.
//

namespace PanFlip
{
    internal partial class SubmitRequestForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// A tooltip to explain other controls.
        /// </summary>
        private System.Windows.Forms.ToolTip toolTip;


        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SubmitRequestForm));
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.statusImages = new System.Windows.Forms.ImageList(this.components);
            this.toggleButton = new System.Windows.Forms.Button();
            this.serviceButton = new System.Windows.Forms.Button();
            this.registryButton = new System.Windows.Forms.Button();
            this.appButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // statusImages
            // 
            this.statusImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("statusImages.ImageStream")));
            this.statusImages.TransparentColor = System.Drawing.Color.Transparent;
            this.statusImages.Images.SetKeyName(0, "green-dot");
            this.statusImages.Images.SetKeyName(1, "red-dot");
            this.statusImages.Images.SetKeyName(2, "yellow-dot");
            // 
            // toggleButton
            // 
            resources.ApplyResources(this.toggleButton, "toggleButton");
            this.toggleButton.ImageList = this.statusImages;
            this.toggleButton.Name = "toggleButton";
            this.toggleButton.UseVisualStyleBackColor = true;
            this.toggleButton.Click += new System.EventHandler(this.ToggleButtonClickHandler);
            // 
            // serviceButton
            // 
            resources.ApplyResources(this.serviceButton, "serviceButton");
            this.serviceButton.ImageList = this.statusImages;
            this.serviceButton.Name = "serviceButton";
            this.serviceButton.UseVisualStyleBackColor = true;
            this.serviceButton.Click += new System.EventHandler(this.ServiceButtonClickHandler);
            // 
            // registryButton
            // 
            resources.ApplyResources(this.registryButton, "registryButton");
            this.registryButton.ImageList = this.statusImages;
            this.registryButton.Name = "registryButton";
            this.registryButton.UseVisualStyleBackColor = true;
            this.registryButton.Click += new System.EventHandler(this.RegistryButtonClickHandler);
            // 
            // appButton
            // 
            resources.ApplyResources(this.appButton, "appButton");
            this.appButton.ImageList = this.statusImages;
            this.appButton.Name = "appButton";
            this.appButton.UseVisualStyleBackColor = true;
            this.appButton.Click += new System.EventHandler(this.ApplicationButtonClickHandler);
            // 
            // SubmitRequestForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.appButton);
            this.Controls.Add(this.registryButton);
            this.Controls.Add(this.serviceButton);
            this.Controls.Add(this.toggleButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SubmitRequestForm";
            this.Load += new System.EventHandler(this.FormLoad);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ImageList statusImages;
        private System.Windows.Forms.Button toggleButton;
        private System.Windows.Forms.Button serviceButton;
        private System.Windows.Forms.Button registryButton;
        private System.Windows.Forms.Button appButton;
    }
}

