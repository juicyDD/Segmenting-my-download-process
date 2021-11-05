
namespace IDM
{
    partial class frmDownload
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
            this.label1 = new System.Windows.Forms.Label();
            this.tbAddress = new System.Windows.Forms.TextBox();
            this.tbPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnResume = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cbSegments = new System.Windows.Forms.ComboBox();
            this.lbStopWatch = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(78, 89);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Address:";
            // 
            // tbAddress
            // 
            this.tbAddress.Location = new System.Drawing.Point(159, 84);
            this.tbAddress.Name = "tbAddress";
            this.tbAddress.Size = new System.Drawing.Size(381, 22);
            this.tbAddress.TabIndex = 1;
            // 
            // tbPath
            // 
            this.tbPath.Location = new System.Drawing.Point(159, 122);
            this.tbPath.Name = "tbPath";
            this.tbPath.Size = new System.Drawing.Size(381, 22);
            this.tbPath.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(78, 127);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Path:";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(380, 272);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 31);
            this.btnStart.TabIndex = 6;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(465, 272);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 31);
            this.btnStop.TabIndex = 7;
            this.btnStop.Text = "Cancel";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(557, 122);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(35, 23);
            this.btnBrowse.TabIndex = 8;
            this.btnBrowse.Text = "...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnPause
            // 
            this.btnPause.Location = new System.Drawing.Point(294, 272);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(80, 31);
            this.btnPause.TabIndex = 9;
            this.btnPause.Text = "Pause";
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnResume
            // 
            this.btnResume.Location = new System.Drawing.Point(213, 272);
            this.btnResume.Name = "btnResume";
            this.btnResume.Size = new System.Drawing.Size(75, 31);
            this.btnResume.TabIndex = 10;
            this.btnResume.Text = "Resume";
            this.btnResume.UseVisualStyleBackColor = true;
            this.btnResume.Click += new System.EventHandler(this.btnResume_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(78, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 17);
            this.label3.TabIndex = 11;
            this.label3.Text = "Segments:";
            // 
            // cbSegments
            // 
            this.cbSegments.FormattingEnabled = true;
            this.cbSegments.Location = new System.Drawing.Point(160, 45);
            this.cbSegments.Name = "cbSegments";
            this.cbSegments.Size = new System.Drawing.Size(121, 24);
            this.cbSegments.TabIndex = 12;
            this.cbSegments.SelectedIndexChanged += new System.EventHandler(this.cbSegments_SelectedIndexChanged);
            // 
            // lbStopWatch
            // 
            this.lbStopWatch.AutoSize = true;
            this.lbStopWatch.Location = new System.Drawing.Point(435, 48);
            this.lbStopWatch.Name = "lbStopWatch";
            this.lbStopWatch.Size = new System.Drawing.Size(157, 17);
            this.lbStopWatch.TabIndex = 13;
            this.lbStopWatch.Text = "time span here tehe :\"D";
            // 
            // frmDownload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lbStopWatch);
            this.Controls.Add(this.cbSegments);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnResume);
            this.Controls.Add(this.btnPause);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.tbPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbAddress);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "frmDownload";
            this.Text = "Download";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmDownload_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmDownload_FormClosed);
            this.Load += new System.EventHandler(this.frmDownload_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbAddress;
        private System.Windows.Forms.TextBox tbPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnResume;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbSegments;
        private System.Windows.Forms.Label lbStopWatch;
    }
}