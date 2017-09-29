namespace App
{
    partial class Form1
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panelColor = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonCalculateEquivalentColor = new System.Windows.Forms.Button();
            this.buttonLoadFile = new System.Windows.Forms.Button();
            this.openImageFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.buttonAnalyzeFiles = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelColor, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 366F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(623, 366);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(492, 360);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // panelColor
            // 
            this.panelColor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelColor.Location = new System.Drawing.Point(501, 3);
            this.panelColor.Name = "panelColor";
            this.panelColor.Size = new System.Drawing.Size(119, 100);
            this.panelColor.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.buttonAnalyzeFiles);
            this.panel2.Controls.Add(this.buttonCalculateEquivalentColor);
            this.panel2.Controls.Add(this.buttonLoadFile);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 366);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(623, 48);
            this.panel2.TabIndex = 0;
            // 
            // buttonCalculateEquivalentColor
            // 
            this.buttonCalculateEquivalentColor.Location = new System.Drawing.Point(453, 13);
            this.buttonCalculateEquivalentColor.Name = "buttonCalculateEquivalentColor";
            this.buttonCalculateEquivalentColor.Size = new System.Drawing.Size(75, 23);
            this.buttonCalculateEquivalentColor.TabIndex = 1;
            this.buttonCalculateEquivalentColor.Text = "Calculate";
            this.buttonCalculateEquivalentColor.UseVisualStyleBackColor = true;
            this.buttonCalculateEquivalentColor.Click += new System.EventHandler(this.button2_Click);
            // 
            // buttonLoadFile
            // 
            this.buttonLoadFile.Location = new System.Drawing.Point(529, 13);
            this.buttonLoadFile.Name = "buttonLoadFile";
            this.buttonLoadFile.Size = new System.Drawing.Size(75, 23);
            this.buttonLoadFile.TabIndex = 0;
            this.buttonLoadFile.Text = "Load File";
            this.buttonLoadFile.UseVisualStyleBackColor = true;
            this.buttonLoadFile.Click += new System.EventHandler(this.button1_Click);
            // 
            // openImageFileDialog
            // 
            this.openImageFileDialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG";
            // 
            // buttonAnalyzeFiles
            // 
            this.buttonAnalyzeFiles.Location = new System.Drawing.Point(372, 13);
            this.buttonAnalyzeFiles.Name = "buttonAnalyzeFiles";
            this.buttonAnalyzeFiles.Size = new System.Drawing.Size(75, 23);
            this.buttonAnalyzeFiles.TabIndex = 2;
            this.buttonAnalyzeFiles.Text = "Analyze";
            this.buttonAnalyzeFiles.UseVisualStyleBackColor = true;
            this.buttonAnalyzeFiles.Click += new System.EventHandler(this.buttonAnalyzeFiles_Click);
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.ShowNewFolderButton = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(623, 414);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.panel2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panelColor;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button buttonLoadFile;
        private System.Windows.Forms.OpenFileDialog openImageFileDialog;
        private System.Windows.Forms.Button buttonCalculateEquivalentColor;
        private System.Windows.Forms.Button buttonAnalyzeFiles;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    }
}

