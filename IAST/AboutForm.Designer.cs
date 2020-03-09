namespace IAST
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.reference1RichTextBox = new System.Windows.Forms.RichTextBox();
            this.contexttMenuStrip_ref1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.reference2RichTextBox = new System.Windows.Forms.RichTextBox();
            this.contextMenuStrip_ref2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label2 = new System.Windows.Forms.Label();
            this.acknowledgementsRichTextBox = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.contexttMenuStrip_ref1.SuspendLayout();
            this.contextMenuStrip_ref2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(225, 448);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Ok";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(7, 7);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(500, 200);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // reference1RichTextBox
            // 
            this.reference1RichTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.reference1RichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.reference1RichTextBox.ContextMenuStrip = this.contexttMenuStrip_ref1;
            this.reference1RichTextBox.Font = new System.Drawing.Font("Abadi MT Condensed Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.reference1RichTextBox.Location = new System.Drawing.Point(7, 239);
            this.reference1RichTextBox.Name = "reference1RichTextBox";
            this.reference1RichTextBox.Size = new System.Drawing.Size(508, 55);
            this.reference1RichTextBox.TabIndex = 4;
            this.reference1RichTextBox.Text = "Reference 1, line 1\nReference 2, line 2";
            // 
            // contexttMenuStrip_ref1
            // 
            this.contexttMenuStrip_ref1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem1});
            this.contexttMenuStrip_ref1.Name = "contexttMenuStrip_ref1";
            this.contexttMenuStrip_ref1.Size = new System.Drawing.Size(103, 26);
            // 
            // copyToolStripMenuItem1
            // 
            this.copyToolStripMenuItem1.Name = "copyToolStripMenuItem1";
            this.copyToolStripMenuItem1.Size = new System.Drawing.Size(102, 22);
            this.copyToolStripMenuItem1.Text = "Copy";
            this.copyToolStripMenuItem1.Click += new System.EventHandler(this.copyToolStripMenuItem1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(5, 217);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "References:";
            // 
            // reference2RichTextBox
            // 
            this.reference2RichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.reference2RichTextBox.ContextMenuStrip = this.contextMenuStrip_ref2;
            this.reference2RichTextBox.Font = new System.Drawing.Font("Abadi MT Condensed Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.reference2RichTextBox.Location = new System.Drawing.Point(7, 298);
            this.reference2RichTextBox.Name = "reference2RichTextBox";
            this.reference2RichTextBox.ReadOnly = true;
            this.reference2RichTextBox.Size = new System.Drawing.Size(508, 60);
            this.reference2RichTextBox.TabIndex = 6;
            this.reference2RichTextBox.Text = "Reference 2, line 1\nReference 2, line 2";
            // 
            // contextMenuStrip_ref2
            // 
            this.contextMenuStrip_ref2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem});
            this.contextMenuStrip_ref2.Name = "contextMenuStrip_ref2";
            this.contextMenuStrip_ref2.Size = new System.Drawing.Size(103, 26);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(5, 371);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(146, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "Acknowledgements:";
            // 
            // acknowledgementsRichTextBox
            // 
            this.acknowledgementsRichTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.acknowledgementsRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.acknowledgementsRichTextBox.Font = new System.Drawing.Font("Abadi MT Condensed Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.acknowledgementsRichTextBox.Location = new System.Drawing.Point(7, 391);
            this.acknowledgementsRichTextBox.Name = "acknowledgementsRichTextBox";
            this.acknowledgementsRichTextBox.ReadOnly = true;
            this.acknowledgementsRichTextBox.Size = new System.Drawing.Size(508, 53);
            this.acknowledgementsRichTextBox.TabIndex = 8;
            this.acknowledgementsRichTextBox.Text = "Acknowledgements";
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 480);
            this.Controls.Add(this.acknowledgementsRichTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.reference2RichTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.reference1RichTextBox);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AboutForm";
            this.Text = "About the IsoCalc Calculator";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.contexttMenuStrip_ref1.ResumeLayout(false);
            this.contextMenuStrip_ref2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.RichTextBox reference1RichTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox reference2RichTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox acknowledgementsRichTextBox;
        private System.Windows.Forms.ContextMenuStrip contexttMenuStrip_ref1;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_ref2;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
    }
}