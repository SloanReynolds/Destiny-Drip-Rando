namespace DapperDestiny {
	partial class ItemImageBox {
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.icon = new System.Windows.Forms.PictureBox();
			this.badge = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.icon)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.badge)).BeginInit();
			this.SuspendLayout();
			// 
			// icon
			// 
			this.icon.BackColor = System.Drawing.Color.DimGray;
			this.icon.BackgroundImage = global::DapperDestiny.Properties.Resources.UntitledGear_Icon;
			this.icon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.icon.Dock = System.Windows.Forms.DockStyle.Fill;
			this.icon.Location = new System.Drawing.Point(0, 0);
			this.icon.Margin = new System.Windows.Forms.Padding(0);
			this.icon.Name = "icon";
			this.icon.Size = new System.Drawing.Size(192, 192);
			this.icon.TabIndex = 0;
			this.icon.TabStop = false;
			// 
			// badge
			// 
			this.badge.BackColor = System.Drawing.Color.Transparent;
			this.badge.BackgroundImage = global::DapperDestiny.Properties.Resources.UntitledGear_Badge;
			this.badge.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.badge.Dock = System.Windows.Forms.DockStyle.Fill;
			this.badge.Location = new System.Drawing.Point(0, 0);
			this.badge.Margin = new System.Windows.Forms.Padding(0);
			this.badge.Name = "badge";
			this.badge.Size = new System.Drawing.Size(192, 192);
			this.badge.TabIndex = 1;
			this.badge.TabStop = false;
			// 
			// ItemImageBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.badge);
			this.Controls.Add(this.icon);
			this.MinimumSize = new System.Drawing.Size(46, 46);
			this.Name = "ItemImageBox";
			this.Size = new System.Drawing.Size(192, 192);
			this.Load += new System.EventHandler(this.ItemImageBox_Load);
			((System.ComponentModel.ISupportInitialize)(this.icon)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.badge)).EndInit();
			this.ResumeLayout(false);
		}

		#endregion

		private PictureBox icon;
		private PictureBox badge;
	}
}
