namespace SHSyncTool
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        // UI Components
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.CheckBox chkInternet;
        private System.Windows.Forms.CheckBox chkVPN;
        private System.Windows.Forms.ComboBox cmbSelectJob;
        private System.Windows.Forms.Button btnTakePhoto;
        private System.Windows.Forms.Button btnStartVideo;
        private System.Windows.Forms.Button btnStopVideo;
        private System.Windows.Forms.PictureBox livePreviewBox;
        private System.Windows.Forms.Label lblTimer;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblUploadStatus;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.lblTitle = new System.Windows.Forms.Label();
            this.chkInternet = new System.Windows.Forms.CheckBox();
            this.chkVPN = new System.Windows.Forms.CheckBox();
            this.cmbSelectJob = new System.Windows.Forms.ComboBox();
            this.btnTakePhoto = new System.Windows.Forms.Button();
            this.btnStartVideo = new System.Windows.Forms.Button();
            this.btnStopVideo = new System.Windows.Forms.Button();
            this.livePreviewBox = new System.Windows.Forms.PictureBox();
            this.lblTimer = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblUploadStatus = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.livePreviewBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(436, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(180, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Media Sync Tool";
            // 
            // chkInternet
            // 
            this.chkInternet.AutoSize = true;
            this.chkInternet.Enabled = false;
            this.chkInternet.Location = new System.Drawing.Point(195, 48);
            this.chkInternet.Name = "chkInternet";
            this.chkInternet.Size = new System.Drawing.Size(117, 17);
            this.chkInternet.TabIndex = 1;
            this.chkInternet.Text = "Internet Connected";
            // 
            // chkVPN
            // 
            this.chkVPN.AutoSize = true;
            this.chkVPN.Enabled = false;
            this.chkVPN.Location = new System.Drawing.Point(327, 48);
            this.chkVPN.Name = "chkVPN";
            this.chkVPN.Size = new System.Drawing.Size(103, 17);
            this.chkVPN.TabIndex = 2;
            this.chkVPN.Text = "VPN Connected";
            // 
            // cmbSelectJob
            // 
            this.cmbSelectJob.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSelectJob.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbSelectJob.FormattingEnabled = true;
            this.cmbSelectJob.Items.AddRange(new object[] {
            "Please Select Job"});
            this.cmbSelectJob.Location = new System.Drawing.Point(195, 9);
            this.cmbSelectJob.Name = "cmbSelectJob";
            this.cmbSelectJob.Size = new System.Drawing.Size(235, 33);
            this.cmbSelectJob.TabIndex = 3;
            // 
            // btnTakePhoto
            // 
            this.btnTakePhoto.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTakePhoto.Location = new System.Drawing.Point(195, 71);
            this.btnTakePhoto.Name = "btnTakePhoto";
            this.btnTakePhoto.Size = new System.Drawing.Size(82, 65);
            this.btnTakePhoto.TabIndex = 4;
            this.btnTakePhoto.Text = "Take Photo";
            // 
            // btnStartVideo
            // 
            this.btnStartVideo.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartVideo.Location = new System.Drawing.Point(364, 71);
            this.btnStartVideo.Name = "btnStartVideo";
            this.btnStartVideo.Size = new System.Drawing.Size(127, 65);
            this.btnStartVideo.TabIndex = 5;
            this.btnStartVideo.Text = "Start Video Record";
           
            // 
            // btnStopVideo
            // 
            this.btnStopVideo.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStopVideo.Location = new System.Drawing.Point(497, 71);
            this.btnStopVideo.Name = "btnStopVideo";
            this.btnStopVideo.Size = new System.Drawing.Size(128, 65);
            this.btnStopVideo.TabIndex = 6;
            this.btnStopVideo.Text = "Stop Video Record";
            this.btnStopVideo.Click += new System.EventHandler(this.btnStopVideo_Click);
            // 
            // livePreviewBox
            // 
            this.livePreviewBox.BackColor = System.Drawing.Color.Black;
            this.livePreviewBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.livePreviewBox.Location = new System.Drawing.Point(25, 142);
            this.livePreviewBox.Name = "livePreviewBox";
            this.livePreviewBox.Size = new System.Drawing.Size(600, 400);
            this.livePreviewBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.livePreviewBox.TabIndex = 7;
            this.livePreviewBox.TabStop = false;
            
            // 
            // lblTimer
            // 
            this.lblTimer.AutoSize = true;
            this.lblTimer.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblTimer.Location = new System.Drawing.Point(21, 545);
            this.lblTimer.Name = "lblTimer";
            this.lblTimer.Size = new System.Drawing.Size(70, 21);
            this.lblTimer.TabIndex = 8;
            this.lblTimer.Text = "00:00:00";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(25, 569);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(600, 20);
            this.progressBar.TabIndex = 9;
            // 
            // lblUploadStatus
            // 
            this.lblUploadStatus.AutoSize = true;
            this.lblUploadStatus.Location = new System.Drawing.Point(432, 551);
            this.lblUploadStatus.Name = "lblUploadStatus";
            this.lblUploadStatus.Size = new System.Drawing.Size(193, 13);
            this.lblUploadStatus.TabIndex = 10;
            this.lblUploadStatus.Text = "Upload Status: 0 uploaded, 0 remaining";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.InitialImage")));
            this.pictureBox1.Location = new System.Drawing.Point(25, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(164, 124);
            this.pictureBox1.TabIndex = 11;
            this.pictureBox1.TabStop = false;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(631, 510);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 32);
            this.button1.TabIndex = 12;
            this.button1.Text = "Exit";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(852, 604);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.chkInternet);
            this.Controls.Add(this.chkVPN);
            this.Controls.Add(this.cmbSelectJob);
            this.Controls.Add(this.btnTakePhoto);
            this.Controls.Add(this.btnStartVideo);
            this.Controls.Add(this.btnStopVideo);
            this.Controls.Add(this.livePreviewBox);
            this.Controls.Add(this.lblTimer);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.lblUploadStatus);
            this.Name = "Form1";
            this.Text = "Media Sync Tool";
            this.Load += new System.EventHandler(this.Form1_Load_1);
            ((System.ComponentModel.ISupportInitialize)(this.livePreviewBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button1;
    }
}
