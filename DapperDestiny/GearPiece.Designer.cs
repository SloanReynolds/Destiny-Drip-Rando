namespace DapperDestiny {
	partial class GearPiece {
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
			this.gearbox = new System.Windows.Forms.GroupBox();
			this.itemImageSmallTop = new DapperDestiny.ItemImageBox();
			this.itemImageSmallBottom = new DapperDestiny.ItemImageBox();
			this.itemImageBig = new DapperDestiny.ItemImageBox();
			this.gearbox.SuspendLayout();
			this.SuspendLayout();
			// 
			// gearbox
			// 
			this.gearbox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gearbox.Controls.Add(this.itemImageSmallTop);
			this.gearbox.Controls.Add(this.itemImageSmallBottom);
			this.gearbox.Controls.Add(this.itemImageBig);
			this.gearbox.Location = new System.Drawing.Point(3, 3);
			this.gearbox.Name = "gearbox";
			this.gearbox.Size = new System.Drawing.Size(157, 116);
			this.gearbox.TabIndex = 0;
			this.gearbox.TabStop = false;
			this.gearbox.Text = "Huckleberry";
			// 
			// itemImageShader
			// 
			this.itemImageSmallTop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.itemImageSmallTop.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.itemImageSmallTop.Location = new System.Drawing.Point(5, 14);
			this.itemImageSmallTop.MinimumSize = new System.Drawing.Size(46, 46);
			this.itemImageSmallTop.Name = "itemImageShader";
			this.itemImageSmallTop.Size = new System.Drawing.Size(46, 46);
			this.itemImageSmallTop.TabIndex = 2;
			// 
			// itemImageOrnament
			// 
			this.itemImageSmallBottom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.itemImageSmallBottom.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.itemImageSmallBottom.Location = new System.Drawing.Point(5, 64);
			this.itemImageSmallBottom.MinimumSize = new System.Drawing.Size(46, 46);
			this.itemImageSmallBottom.Name = "itemImageOrnament";
			this.itemImageSmallBottom.Size = new System.Drawing.Size(46, 46);
			this.itemImageSmallBottom.TabIndex = 1;
			// 
			// itemImageMain
			// 
			this.itemImageBig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.itemImageBig.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.itemImageBig.Location = new System.Drawing.Point(55, 14);
			this.itemImageBig.MinimumSize = new System.Drawing.Size(46, 46);
			this.itemImageBig.Name = "itemImageMain";
			this.itemImageBig.Size = new System.Drawing.Size(96, 96);
			this.itemImageBig.TabIndex = 0;
			// 
			// GearPiece
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.gearbox);
			this.Name = "GearPiece";
			this.Size = new System.Drawing.Size(163, 122);
			this.Load += new System.EventHandler(this.GearPiece_Load);
			this.gearbox.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private GroupBox gearbox;
		private ItemImageBox itemImageBig;
		private ItemImageBox itemImageSmallTop;
		private ItemImageBox itemImageSmallBottom;
	}
}
