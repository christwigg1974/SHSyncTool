using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;


namespace SHSyncTool
{
    public partial class Form1 : Form
    {
        private VideoCapture capture;
        private Mat frame;
        private Bitmap image;
        private Thread cameraThread;
        private bool isCameraRunning = false;

        private string currentSRO = "";
        private string currentCustNum = "";
        private string currentCustPO = "";

        private bool isRecording = false;
        private VideoWriter videoWriter;
        private System.Windows.Forms.Timer recordingTimer;
        private DateTime recordingStartTime;

        private System.Threading.Timer uploadTimer;
        private int uploadTotal = 0;
        private int uploadRemaining = 0;
        private int cameraIndex = 0;
        private readonly string photoPath = @"C:\sync\photos";
        private readonly string photoArchivePath = @"C:\sync\photos\archive";
        private readonly string videoPath = @"C:\sync\videos";
        private readonly string videoArchivePath = @"C:\sync\videos\archive";

        public Form1()
        {
            InitializeComponent();
            Load += Form1_Load;
            FormClosing += Form1_FormClosing;
           

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckConnectivity();
            LoadJobsFromDatabase();
            StartCamera();


            CleanOldArchives();
     

            recordingTimer = new System.Windows.Forms.Timer { Interval = 1000 };
            recordingTimer.Tick += (s, args) =>
            {
                TimeSpan elapsed = DateTime.Now - recordingStartTime;
                lblTimer.Text = elapsed.ToString(@"hh\:mm\:ss");
            };

            cmbSelectJob.SelectedIndexChanged += (s, args) =>
            {
                if (cmbSelectJob.SelectedIndex > 0)
                {
                    GetJobDetailsFromDatabase(cmbSelectJob.SelectedItem.ToString());
                }
            };

            btnTakePhoto.Click += (s, args) =>
            {
                if (string.IsNullOrWhiteSpace(currentSRO))
                {
                    MessageBox.Show("Please select a job before capturing a photo.");
                    return;
                }

                string timestamp = DateTime.Now.ToString("ddMMyyyyHHmmss");
                string filename = $"{currentSRO}-{currentCustNum}-{currentCustPO}-{timestamp}.jpg";
                string savePath = Path.Combine(photoPath, filename);

                Directory.CreateDirectory(photoPath);
                image?.Save(savePath);
                MessageBox.Show($"Photo saved:\n{savePath}", "Photo Captured", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            btnStartVideo.Click += (s, e2) =>
            {
                if (string.IsNullOrWhiteSpace(currentSRO))
                {
                    MessageBox.Show("Please select a job before recording.");
                    return;
                }

                if (isRecording) return;
                btnStartVideo.BackColor = Color.Red;
                btnStartVideo.ForeColor = Color.White;
                string timestamp = DateTime.Now.ToString("ddMMyyyyHHmmss");
                string filename = $"{currentSRO}-{currentCustNum}-{currentCustPO}-{timestamp}.avi";
                string savePath = Path.Combine(videoPath, filename);
                Directory.CreateDirectory(videoPath);

                int fourcc = VideoWriter.FourCC('M', 'J', 'P', 'G');
                videoWriter = new VideoWriter(savePath, fourcc, 20, new OpenCvSharp.Size(frame.Width, frame.Height));

                if (!videoWriter.IsOpened())
                {
                    MessageBox.Show("Failed to start video writer.", "Error");
                    return;
                }

                isRecording = true;
                recordingStartTime = DateTime.Now;
                recordingTimer.Start();
            };

            btnStopVideo.Click += (s, e3) =>
            {
                if (!isRecording) return;

                isRecording = false;
                recordingTimer.Stop();
                lblTimer.Text = "00:00:00";

                videoWriter?.Release();
                videoWriter = null;
                btnStartVideo.BackColor = SystemColors.Control;
                btnStartVideo.ForeColor = SystemColors.ControlText;


                MessageBox.Show("Video recording stopped and saved.", "Video Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            uploadTimer = new System.Threading.Timer((_) => ProcessPendingUploads(), null, 5000, 15000);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopCamera();
        }

        
   


        private CancellationTokenSource cancellationTokenSource;
        
        private void StartCamera()
        {
            capture = new VideoCapture(0, VideoCaptureAPIs.DSHOW);

            // Attempt to set an exaggeratedly high resolution
            capture.Set(VideoCaptureProperties.FrameWidth, 9999);
            capture.Set(VideoCaptureProperties.FrameHeight, 9999);
            frame = new Mat();
            isCameraRunning = true;

            cameraThread = new Thread(() =>
            {
                while (isCameraRunning)
                {
                    capture.Read(frame);
                    if (!frame.Empty())
                    {
                        
                        Cv2.Rotate(frame, frame, RotateFlags.Rotate180);
                        image = BitmapConverter.ToBitmap(frame);
                        livePreviewBox.Invoke(new Action(() =>
                        {
                            livePreviewBox.Image?.Dispose();
                            livePreviewBox.Image = new Bitmap(image);
                        }));

                        if (isRecording && videoWriter != null)
                        {
                            videoWriter.Write(frame);
                        }
                    }
                    Thread.Sleep(10);
                }

            });
            cameraThread.Start();
        }




        private void StopCamera()
        {
            isCameraRunning = false;
            Thread.Sleep(100);
            capture?.Release();
            Thread.Sleep(100);
            livePreviewBox.Image?.Dispose();
        }




        private void CheckConnectivity()
        {
            chkInternet.Checked = PingHost("8.8.8.8");
            chkVPN.Checked = PingHost("172.16.1.1");

            if (!chkInternet.Checked)
                MessageBox.Show("⚠ Internet not reachable. Please check your Internet Connection", "Warning");

            if (!chkVPN.Checked)
                MessageBox.Show("⚠ VPN not connected. Files may not be copied to the network share.", "Warning");
        }

        private void LogMessage(string message)
        {
            try
            {
                string logDir = @"C:\sync\logs";
                Directory.CreateDirectory(logDir);
                string logFile = Path.Combine(logDir, "sync-log.txt");

                string newEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {message}";

                // Auto-prune: remove lines older than 6 months
                var now = DateTime.Now;
                var validLines = new List<string>();

                if (File.Exists(logFile))
                {
                    foreach (var line in File.ReadLines(logFile))
                    {
                        if (DateTime.TryParse(line.Substring(0, 19), out DateTime logTime))
                        {
                            if (logTime > now.AddMonths(-6))
                                validLines.Add(line);
                        }
                    }
                }

                validLines.Add(newEntry);
                File.WriteAllLines(logFile, validLines);
            }
            catch
            {
                // Silently ignore logging failures
            }
        }

        private bool PingHost(string host)
        {
            try
            {
                using (var ping = new Ping())
                {
                    var reply = ping.Send(host, 1000);
                    return reply?.Status == IPStatus.Success;
                }
            }
            catch
            {
                return false;
            }
        }

        private void LoadJobsFromDatabase()
        {
            string connStr = @"Data Source=.\SQLEXPRESS;Initial Catalog=ServiceMobile;Integrated Security=True";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    string query = @"SELECT DISTINCT sro_num FROM fs_sro";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    cmbSelectJob.Items.Clear();
                    cmbSelectJob.Items.Add("Please Select Job");
                    while (reader.Read())
                    {
                        cmbSelectJob.Items.Add(reader["sro_num"].ToString());
                    }
                    cmbSelectJob.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database error:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CleanOldArchives()
        {
            string[] archiveFolders = { photoArchivePath, videoArchivePath };

            foreach (var folder in archiveFolders)
            {
                try
                {
                    if (!Directory.Exists(folder)) continue;

                    var files = Directory.GetFiles(folder);
                    foreach (var file in files)
                    {
                        DateTime lastModified = File.GetLastWriteTime(file);
                        if (lastModified < DateTime.Now.AddMonths(-6))
                        {
                            File.Delete(file);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Optional: log or show error
                    Console.WriteLine($"Cleanup failed in {folder}: {ex.Message}");
                }
            }
            LogMessage("Archive cleanup completed (files older than 6 months removed)");

        }


        private void GetJobDetailsFromDatabase(string sro)
        {
            string connStr = @"Data Source=.\SQLEXPRESS;Initial Catalog=ServiceMobile;Integrated Security=True";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string query = @"SELECT TOP 1 cust_num, LEFT(cust_po, CHARINDEX('-', cust_po + '-') - 1) AS cust_po FROM fs_sro WHERE sro_num = @sro";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@sro", sro);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    currentSRO = sro;
                    currentCustNum = reader["cust_num"].ToString();
                    currentCustPO = reader["cust_po"].ToString();
                }
            }
        }

        private void ProcessPendingUploads()
        {
            string[] photoFiles = Directory.GetFiles(photoPath, "*.jpg");
            string[] videoFiles = Directory.GetFiles(videoPath, "*.avi");

            uploadTotal = photoFiles.Length + videoFiles.Length;
            uploadRemaining = uploadTotal;

            if (uploadTotal == 0)
            {
                UpdateUploadStatus();
                return;
            }

            foreach (var file in photoFiles)
            {
                bool success = TryCopyFile(file, @"\\SMB\Upload_Location", photoArchivePath);
                if (success) uploadRemaining--;
                UpdateUploadStatus();
            }

            foreach (var file in videoFiles)
            {
                string custNum = ExtractCustNum(file);
                string dest = $@"\\SMB\Upload_locations\{custNum}\videos";
                bool success = TryCopyFile(file, dest, videoArchivePath);
                if (success) uploadRemaining--;
                UpdateUploadStatus();
            }

            CleanOldArchives();
        }

        private bool TryCopyFile(string sourcePath, string destFolder, string archiveFolder)
        {
            try
            {
                Directory.CreateDirectory(destFolder);
                Directory.CreateDirectory(archiveFolder);

                string name = Path.GetFileName(sourcePath);
                File.Copy(sourcePath, Path.Combine(destFolder, name), true);
                File.Move(sourcePath, Path.Combine(archiveFolder, name));
                LogMessage($"Uploaded: {Path.GetFileName(sourcePath)} → {destFolder}");


                return true;
            }
            catch
            {
                LogMessage($"Upload FAILED: {Path.GetFileName(sourcePath)} → {destFolder}");

                return false;
            }
        }

        private string ExtractCustNum(string filePath)
        {
            var parts = Path.GetFileNameWithoutExtension(filePath).Split('-');
            return parts.Length >= 2 ? parts[1] : "Unknown";
        }

        private void UpdateUploadStatus()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(UpdateUploadStatus));
                return;
            }

            int uploaded = uploadTotal - uploadRemaining;
            int percent = (uploadTotal == 0) ? 0 : (int)((uploaded * 100.0) / uploadTotal);
            percent = Math.Min(percent, 100);

            progressBar.Value = percent;
            lblUploadStatus.Text = $"Upload Status: {uploaded} uploaded, {uploadRemaining} remaining";
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            MaximizeBox = false;
            MinimizeBox = false;
            ControlBox = false;
        }

        private void btnStopVideo_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
         "Have all files been uploaded?",
         "Confirmation",
         MessageBoxButtons.YesNo,
         MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                StopCamera();
                Application.Exit();
            }
        }

       


       
    }
}