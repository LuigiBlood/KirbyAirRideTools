namespace KirbyAirRideTools
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportCollisionOBJToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportPartitionOBJToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exportCollisionDAEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(284, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportCollisionOBJToolStripMenuItem,
            this.exportPartitionOBJToolStripMenuItem,
            this.toolStripSeparator1,
            this.exportCollisionDAEToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exportCollisionOBJToolStripMenuItem
            // 
            this.exportCollisionOBJToolStripMenuItem.Name = "exportCollisionOBJToolStripMenuItem";
            this.exportCollisionOBJToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.exportCollisionOBJToolStripMenuItem.Text = "Export Collision OBJ";
            this.exportCollisionOBJToolStripMenuItem.Click += new System.EventHandler(this.exportCollisionOBJToolStripMenuItem_Click);
            // 
            // exportPartitionOBJToolStripMenuItem
            // 
            this.exportPartitionOBJToolStripMenuItem.Name = "exportPartitionOBJToolStripMenuItem";
            this.exportPartitionOBJToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.exportPartitionOBJToolStripMenuItem.Text = "Export Partition OBJ";
            this.exportPartitionOBJToolStripMenuItem.Click += new System.EventHandler(this.exportPartitionOBJToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(178, 6);
            // 
            // exportCollisionDAEToolStripMenuItem
            // 
            this.exportCollisionDAEToolStripMenuItem.Name = "exportCollisionDAEToolStripMenuItem";
            this.exportCollisionDAEToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.exportCollisionDAEToolStripMenuItem.Text = "Export Collision DAE";
            this.exportCollisionDAEToolStripMenuItem.Click += new System.EventHandler(this.exportCollisionDAEToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportCollisionOBJToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportPartitionOBJToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exportCollisionDAEToolStripMenuItem;
    }
}

