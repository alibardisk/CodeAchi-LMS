using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WIA;

namespace CodeAchi_Library_Management_System
{
    public partial class FormPicture : Form
    {
        public FormPicture()
        {
            InitializeComponent();
        }

        bool isCaptured = false;
        private FilterInfoCollection CaptureDevice; // list of webcam
        private VideoCaptureDevice FinalFrame;
        DeviceInfo firstScannerAvailable = null;

        int cropX;
        int cropY;
        int cropWidth;
        int cropHeight;
        public Pen cropPen;

        private void FormPicture_Load(object sender, EventArgs e)
        {
            lblScanning.Visible = false;
        }

        private void FormPicture_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                      this.DisplayRectangle);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog selectFile = new OpenFileDialog();
            selectFile.Title = Application.ProductName + " Select JPG/PNG File";
            selectFile.Filter = "JPG/PNG File|*.jpg;*.png";
            if (selectFile.ShowDialog() == DialogResult.OK)
            {
                txtbImagePath.Text = selectFile.FileName;
                pcbViewer.Image = Image.FromFile(selectFile.FileName);
                pcbViewer.ImageLocation = txtbImagePath.Text;
                btnCrop.Enabled = false;
                btnSave.Enabled = true;
                btnReset.Enabled = true;
                isCaptured = true;
            }
        }

        private void pcbViewer_MouseMove(object sender, MouseEventArgs e)
        {
            if (pcbViewer.Image == null)
                return;
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (rdbCamera.Checked)
                {
                    cropPen = new Pen(Color.WhiteSmoke, 1);
                    cropPen.DashStyle = DashStyle.Dot;
                }
                else if (rdbScanner.Checked)
                {
                    cropPen = new Pen(Color.Black, 1);
                    cropPen.DashStyle = DashStyle.Dot;
                }
                cropPen = new Pen(Color.WhiteSmoke, 1);
                cropPen.DashStyle = DashStyle.Dot;
                pcbViewer.Refresh();
                cropWidth = e.X - cropX;
                cropHeight = e.Y - cropY;
                pcbViewer.CreateGraphics().DrawRectangle(cropPen, cropX, cropY, cropWidth, cropHeight);
            }
        }

        private void pcbViewer_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                btnCrop.Enabled = false;
                cropX = e.X;
                cropY = e.Y;
                if (rdbCamera.Checked)
                {
                    cropPen = new Pen(Color.WhiteSmoke, 1);
                    cropPen.DashStyle = DashStyle.Dot;
                }
                else if (rdbScanner.Checked)
                {
                    cropPen = new Pen(Color.Black, 1);
                    cropPen.DashStyle = DashStyle.Dot;
                }

            }
            pcbViewer.Refresh();
        }

        private void pcbViewer_MouseUp(object sender, MouseEventArgs e)
        {
            if (isCaptured)
            {
                btnCrop.Enabled = true;
            }
        }

        private void btnCrop_Click(object sender, EventArgs e)
        {
            try
            {
                btnCrop.Enabled = false;
                Cursor = Cursors.Default;
                if (cropWidth < 1)
                {
                    return;
                }
                Rectangle rect = new Rectangle(cropX, cropY, cropWidth, cropHeight);
                //First we define a rectangle with the help of already calculated points  
                Bitmap OriginalImage = new Bitmap(pcbViewer.Image, pcbViewer.Width, pcbViewer.Height);
                //Original image  
                Bitmap _img = new Bitmap(cropWidth, cropHeight);
                // for cropinf image  
                Graphics graphics = Graphics.FromImage(_img);
                // create graphics  
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                //set image attributes  
                graphics.DrawImage(OriginalImage, 0, 0, rect, GraphicsUnit.Pixel);
                pcbViewer.Image = _img;
                pcbViewer.Width = _img.Width;
                pcbViewer.Height = _img.Height;
                pcbViewer.Location = new System.Drawing.Point((panelBack.Width / 2) - (pcbViewer.Width / 2), (panelBack.Height / 2) - (pcbViewer.Height / 2));

            }
            catch
            {

            }
        }

        private void pcbViewer_MouseEnter(object sender, EventArgs e)
        {
            if (pcbViewer.Image != null && isCaptured)
            {
                Cursor = Cursors.Cross;
            }
        }

        private void pcbViewer_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            isCaptured = false;
            pcbViewer.Size = new Size(373, 297);
            pcbViewer.Location = new Point(5, 5);
            if (rdbBrowse.Checked)
            {
                pcbViewer.Image = Image.FromFile(txtbImagePath.Text);
                isCaptured = true;
                btnCrop.Enabled = false;
            }
            else if (rdbCamera.Checked)
            {
                if (cmbCamera.Items.Count > 1)
                {
                    try
                    {
                        btnCapture_Click(null, null);
                        FinalFrame = new VideoCaptureDevice(CaptureDevice[cmbCamera.SelectedIndex - 1].MonikerString);// specified web cam and its filter moniker string
                        FinalFrame.NewFrame += new NewFrameEventHandler(FinalFrame_NewFrame);// click button event is fired, 
                        FinalFrame.Start();
                        btnCapture.Enabled = true;
                        btnCrop.Enabled = false;
                    }
                    catch
                    {

                    }
                }
                else
                {
                    cmbCamera.SelectedIndex = 0;
                    btnSave.Enabled = false;
                    btnCrop.Enabled = false;
                    btnCapture.Enabled = false;
                    btnReset.Enabled = false;
                }
            }
            else if (rdbScanner.Checked)
            {
                pcbViewer.Image = null;
                cmbScanner.SelectedIndex = 0;
                btnSave.Enabled = false;
                btnCrop.Enabled = false;
                btnCapture.Enabled = false;
                btnReset.Enabled = false;
            }
        }

        private void rdbCamera_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbCamera.Checked)
            {
                isCaptured = false;
                pcbViewer.Size = new Size(373, 297);
                pcbViewer.Location = new Point(5, 5);
                txtbImagePath.Clear();
                btnBrowse.Enabled = false;
                cmbScanner.Items.Clear();
                cmbScanner.Enabled = false;
                pcbViewer.Image = null;
                try
                {
                    cmbCamera.Items.Clear();
                    cmbCamera.Items.Add("Please select your camera..");
                    isCaptured = false;
                    CaptureDevice = new FilterInfoCollection(FilterCategory.VideoInputDevice);//constructor
                    foreach (AForge.Video.DirectShow.FilterInfo Device in CaptureDevice)
                    {
                        cmbCamera.Items.Add(Device.Name);
                    }
                    cmbCamera.SelectedIndex = 0; // default
                    cmbCamera.Enabled = true;
                }
                catch 
                {
                   
                }
                if (cmbCamera.Items.Count == 0)
                {
                    cmbCamera.Enabled = false;
                    MessageBox.Show("No webcam detected.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void rdbBrowse_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbBrowse.Checked)
            {
                isCaptured = false;
                pcbViewer.Size = new Size(373, 297);
                pcbViewer.Location = new Point(5, 5);
                btnBrowse.Enabled = true;
                pcbViewer.Image = null;
                btnCrop.Enabled = false;
                btnCapture.Enabled = false;
                btnSave.Enabled = false;
                btnReset.Enabled = false;
                cmbCamera.Enabled = false;
                cmbScanner.Enabled = false;
                cmbCamera.Items.Clear();
                cmbScanner.Items.Clear();
                try
                {
                    if (FinalFrame != null)
                    {
                        FinalFrame.Stop();
                    }
                    pcbViewer.Image = null;
                }
                catch
                {

                }
            }
        }

        private void cmbCamera_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cmbCamera.SelectedIndex>0)
            {
                try
                {
                    btnCapture_Click(null, null);
                    FinalFrame = new VideoCaptureDevice(CaptureDevice[cmbCamera.SelectedIndex-1].MonikerString);// specified web cam and its filter moniker string
                    FinalFrame.NewFrame += new NewFrameEventHandler(FinalFrame_NewFrame);// click button event is fired, 
                    FinalFrame.Start();
                    btnCapture.Enabled = true;
                }
                catch
                {

                }
            }
            else
            {
                btnCapture.Enabled = false;
                btnSave.Enabled = false;
                btnReset.Enabled = false;
                try
                {
                    if (FinalFrame != null)
                    {
                        FinalFrame.Stop();
                    }
                    pcbViewer.Image = null;
                }
                catch
                {

                }
            }
        }

        private void btnCapture_Click(object sender, EventArgs e)
        {
            if (rdbCamera.Checked)
            {
                try
                {
                    if (FinalFrame.IsRunning)
                    {
                        FinalFrame.Stop();
                        isCaptured = true;
                        btnCapture.Enabled = false;
                        btnSave.Enabled = true;
                        btnReset.Enabled = true;
                        isCaptured = true;
                    }
                }
                catch
                {
                   
                }
            }
        }

        void FinalFrame_NewFrame(object sender, NewFrameEventArgs eventArgs) // must be void so that it can be accessed everywhere.
        {
            pcbViewer.Image = (Bitmap)eventArgs.Frame.Clone();// clone the bitmap
        }

        private void rdbScanner_CheckedChanged(object sender, EventArgs e)
        {
            if(rdbScanner.Checked)
            {
                isCaptured = false;
                pcbViewer.Size = new Size(373, 297);
                pcbViewer.Location = new Point(5, 5);
                txtbImagePath.Clear();
                btnBrowse.Enabled = false;
                cmbCamera.Items.Clear();
                cmbCamera.Enabled = false;
                try
                {
                    if (FinalFrame!=null && FinalFrame.IsRunning)
                    {
                        FinalFrame.Stop();
                    }
                }
                catch
                {

                }
                pcbViewer.Image = null;

                var deviceManager = new DeviceManager();
                string deviceName = "";
                cmbScanner.Items.Clear();
                cmbScanner.Items.Add("Pelease select your scanner.");
                for (int i = 1; i <= deviceManager.DeviceInfos.Count; i++)
                {
                    if (deviceManager.DeviceInfos[i].Type == WiaDeviceType.ScannerDeviceType)
                    {
                        if (deviceManager.DeviceInfos[i].Type == WIA.WiaDeviceType.ScannerDeviceType)

                        {

                            foreach (WIA.Property p in deviceManager.DeviceInfos[i].Properties)
                            {

                                if (p.Name == "Name")
                                {
                                    deviceName = ((WIA.IProperty)p).get_Value().ToString();
                                    cmbScanner.Items.Add(deviceName);
                                    firstScannerAvailable = deviceManager.DeviceInfos[i];
                                }
                            }
                        }
                    }
                }
                cmbScanner.SelectedIndex = 0;
                cmbScanner.Enabled = true;
            }
        }

        private void cmbScanner_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cmbScanner.SelectedIndex>0)
            {
                lblScanning.Visible = true;
                Application.DoEvents();
                rdbBrowse.Enabled = false;
                rdbCamera.Enabled = false;
                try
                {
                    var device = firstScannerAvailable.Connect();

                    // Select the scanner
                    var scannerItem = device.Items[cmbScanner.SelectedIndex];

                    // Retrieve a image in JPEG format and store it into a variable
                    var imageFile = (ImageFile)scannerItem.Transfer(FormatID.wiaFormatJPEG);
                    var imageBytes = (byte[])imageFile.FileData.get_BinaryData();
                    var memoryStream = new MemoryStream(imageBytes);
                    pcbViewer.Image = Image.FromStream(memoryStream);
                    btnSave.Enabled = true;
                    btnReset.Enabled = true;
                    isCaptured = true;
                    rdbBrowse.Enabled = false;
                    rdbCamera.Enabled = false;
                    lblScanning.Visible = false;
                }
                catch
                {
                    pcbViewer.Image = Properties.Resources.uploadingFail;
                    lblScanning.Visible = false;
                }
            }
            else
            {
                btnSave.Enabled = false;
                btnReset.Enabled = false;
                rdbBrowse.Enabled = true;
                rdbCamera.Enabled = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!isCaptured)
            {
                MessageBox.Show("Please capture an image.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            globalVarLms.bimapImage = new Bitmap(pcbViewer.Image);
            this.Close();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                try
                {
                    Application.DoEvents();
                    var device = firstScannerAvailable.Connect();

                    // Select the scanner
                    var scannerItem = device.Items[cmbScanner.SelectedIndex];

                    // Retrieve a image in JPEG format and store it into a variable
                    var imageFile = (ImageFile)scannerItem.Transfer(FormatID.wiaFormatJPEG);
                    var imageBytes = (byte[])imageFile.FileData.get_BinaryData();
                    var memoryStream = new MemoryStream(imageBytes);
                    pcbViewer.Image = Image.FromStream(memoryStream);
                    Application.DoEvents();
                    btnSave.Enabled = true;
                    btnReset.Enabled = true;
                    isCaptured = true;
                    rdbBrowse.Enabled = false;
                    rdbCamera.Enabled = false;
                }
                catch
                {

                }
            }));
        }

        private void FormPicture_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (FinalFrame != null)
                {
                    FinalFrame.Stop();
                    FinalFrame.NewFrame -= new NewFrameEventHandler(FinalFrame_NewFrame);
                    FinalFrame = null;
                }
            }
            catch
            {

            }
        }
    }
}
