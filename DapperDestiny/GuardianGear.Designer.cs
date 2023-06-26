namespace DapperDestiny {
	partial class GuardianGear {
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
			this.gearKinetic = new DapperDestiny.GearPiece();
			this.gearEnergy = new DapperDestiny.GearPiece();
			this.gearPower = new DapperDestiny.GearPiece();
			this.gearHead = new DapperDestiny.GearPiece();
			this.gearClass = new DapperDestiny.GearPiece();
			this.gearArms = new DapperDestiny.GearPiece();
			this.gearChest = new DapperDestiny.GearPiece();
			this.gearLegs = new DapperDestiny.GearPiece();
			this.SuspendLayout();
			// 
			// gearKinetic
			// 
			this.gearKinetic.Location = new System.Drawing.Point(3, 3);
			this.gearKinetic.Name = "gearKinetic";
			this.gearKinetic.Size = new System.Drawing.Size(163, 125);
			this.gearKinetic.TabIndex = 0;
			// 
			// gearEnergy
			// 
			this.gearEnergy.Location = new System.Drawing.Point(3, 134);
			this.gearEnergy.Name = "gearEnergy";
			this.gearEnergy.Size = new System.Drawing.Size(163, 125);
			this.gearEnergy.TabIndex = 1;
			// 
			// gearPower
			// 
			this.gearPower.Location = new System.Drawing.Point(3, 265);
			this.gearPower.Name = "gearPower";
			this.gearPower.Size = new System.Drawing.Size(163, 125);
			this.gearPower.TabIndex = 2;
			// 
			// gearHead
			// 
			this.gearHead.Location = new System.Drawing.Point(172, 3);
			this.gearHead.Name = "gearHead";
			this.gearHead.Size = new System.Drawing.Size(163, 125);
			this.gearHead.TabIndex = 3;
			// 
			// gearClass
			// 
			this.gearClass.Location = new System.Drawing.Point(3, 396);
			this.gearClass.Name = "gearClass";
			this.gearClass.Size = new System.Drawing.Size(163, 125);
			this.gearClass.TabIndex = 4;
			// 
			// gearArms
			// 
			this.gearArms.Location = new System.Drawing.Point(172, 134);
			this.gearArms.Name = "gearArms";
			this.gearArms.Size = new System.Drawing.Size(163, 125);
			this.gearArms.TabIndex = 5;
			// 
			// gearChest
			// 
			this.gearChest.Location = new System.Drawing.Point(172, 265);
			this.gearChest.Name = "gearChest";
			this.gearChest.Size = new System.Drawing.Size(163, 125);
			this.gearChest.TabIndex = 6;
			// 
			// gearLegs
			// 
			this.gearLegs.Location = new System.Drawing.Point(172, 396);
			this.gearLegs.Name = "gearLegs";
			this.gearLegs.Size = new System.Drawing.Size(163, 125);
			this.gearLegs.TabIndex = 7;
			// 
			// GuardianGear
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.gearLegs);
			this.Controls.Add(this.gearChest);
			this.Controls.Add(this.gearArms);
			this.Controls.Add(this.gearClass);
			this.Controls.Add(this.gearHead);
			this.Controls.Add(this.gearPower);
			this.Controls.Add(this.gearEnergy);
			this.Controls.Add(this.gearKinetic);
			this.Name = "GuardianGear";
			this.Size = new System.Drawing.Size(339, 524);
			this.ResumeLayout(false);

		}

		#endregion

		private GearPiece gearKinetic;
		private GearPiece gearEnergy;
		private GearPiece gearPower;
		private GearPiece gearHead;
		private GearPiece gearClass;
		private GearPiece gearArms;
		private GearPiece gearChest;
		private GearPiece gearLegs;
	}
}
