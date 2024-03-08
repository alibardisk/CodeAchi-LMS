﻿namespace CodeAchi_Library_Management_System
{
    partial class FormBorrowerId
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBorrowerId));
            this.txtbInfo = new System.Windows.Forms.TextBox();
            this.rdbPdf = new System.Windows.Forms.RadioButton();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.lblNotification = new System.Windows.Forms.Label();
            this.dgvBrrList = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.accnNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rdbPrinter = new System.Windows.Forms.RadioButton();
            this.txtbPrinterName = new System.Windows.Forms.TextBox();
            this.btnPrinter = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblMessage = new System.Windows.Forms.Label();
            this.lblBlink = new System.Windows.Forms.Label();
            this.chkbAll = new System.Windows.Forms.CheckBox();
            this.btnPrint = new System.Windows.Forms.Button();
            this.txtbPath = new System.Windows.Forms.TextBox();
            this.panelDate = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.lblInfo = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkbFooter = new System.Windows.Forms.CheckBox();
            this.chkbQr = new System.Windows.Forms.CheckBox();
            this.label14 = new System.Windows.Forms.Label();
            this.lblTtlRecord = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbOption = new System.Windows.Forms.ComboBox();
            this.numUpBlock = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbPaper = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.rdbCategory = new System.Windows.Forms.RadioButton();
            this.rdbDate = new System.Windows.Forms.RadioButton();
            this.rdbManual = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBrrList)).BeginInit();
            this.panelDate.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpBlock)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtbInfo
            // 
            this.txtbInfo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtbInfo.Location = new System.Drawing.Point(452, 55);
            this.txtbInfo.Name = "txtbInfo";
            this.txtbInfo.Size = new System.Drawing.Size(198, 25);
            this.txtbInfo.TabIndex = 6086;
            this.txtbInfo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtbInfo_KeyDown);
            // 
            // rdbPdf
            // 
            this.rdbPdf.AutoSize = true;
            this.rdbPdf.Location = new System.Drawing.Point(9, -1);
            this.rdbPdf.Name = "rdbPdf";
            this.rdbPdf.Size = new System.Drawing.Size(96, 21);
            this.rdbPdf.TabIndex = 6095;
            this.rdbPdf.TabStop = true;
            this.rdbPdf.Text = "Save as PDF";
            this.rdbPdf.UseVisualStyleBackColor = true;
            this.rdbPdf.CheckedChanged += new System.EventHandler(this.rdbPdf_CheckedChanged);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(58)))), ((int)(((byte)(66)))));
            this.btnBrowse.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBrowse.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnBrowse.FlatAppearance.BorderSize = 2;
            this.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowse.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowse.ForeColor = System.Drawing.Color.White;
            this.btnBrowse.Location = new System.Drawing.Point(175, 60);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(89, 30);
            this.btnBrowse.TabIndex = 6094;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = false;
            this.btnBrowse.EnabledChanged += new System.EventHandler(this.btnBrowse_EnabledChanged);
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(511, 88);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(137, 15);
            this.label7.TabIndex = 6090;
            this.label7.Text = "Please press enter to add";
            // 
            // lblNotification
            // 
            this.lblNotification.AutoSize = true;
            this.lblNotification.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNotification.ForeColor = System.Drawing.Color.Red;
            this.lblNotification.Location = new System.Drawing.Point(499, 82);
            this.lblNotification.Name = "lblNotification";
            this.lblNotification.Size = new System.Drawing.Size(17, 21);
            this.lblNotification.TabIndex = 6089;
            this.lblNotification.Text = "*";
            // 
            // dgvBrrList
            // 
            this.dgvBrrList.AllowUserToAddRows = false;
            this.dgvBrrList.AllowUserToResizeRows = false;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvBrrList.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle9;
            this.dgvBrrList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvBrrList.BackgroundColor = System.Drawing.Color.Gainsboro;
            this.dgvBrrList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvBrrList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(82)))), ((int)(((byte)(90)))));
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvBrrList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.dgvBrrList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBrrList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.accnNo,
            this.itemTitle});
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.Color.PowderBlue;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvBrrList.DefaultCellStyle = dataGridViewCellStyle11;
            this.dgvBrrList.EnableHeadersVisualStyles = false;
            this.dgvBrrList.Location = new System.Drawing.Point(293, 110);
            this.dgvBrrList.MultiSelect = false;
            this.dgvBrrList.Name = "dgvBrrList";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.ButtonShadow;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvBrrList.RowHeadersDefaultCellStyle = dataGridViewCellStyle12;
            this.dgvBrrList.RowHeadersVisible = false;
            this.dgvBrrList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBrrList.Size = new System.Drawing.Size(393, 167);
            this.dgvBrrList.TabIndex = 6088;
            this.dgvBrrList.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvBrrList_CellMouseClick);
            this.dgvBrrList.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBrrList_CellValueChanged);
            this.dgvBrrList.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvBrrList_RowsAdded);
            this.dgvBrrList.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dgvBrrList_RowsRemoved);
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column1.HeaderText = "";
            this.Column1.Name = "Column1";
            this.Column1.Width = 30;
            // 
            // accnNo
            // 
            this.accnNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.accnNo.FillWeight = 143.7126F;
            this.accnNo.HeaderText = "Borrower Id";
            this.accnNo.Name = "accnNo";
            this.accnNo.ReadOnly = true;
            this.accnNo.Width = 120;
            // 
            // itemTitle
            // 
            this.itemTitle.FillWeight = 56.28743F;
            this.itemTitle.HeaderText = "Borrower Name";
            this.itemTitle.Name = "itemTitle";
            this.itemTitle.ReadOnly = true;
            // 
            // rdbPrinter
            // 
            this.rdbPrinter.AutoSize = true;
            this.rdbPrinter.Checked = true;
            this.rdbPrinter.Location = new System.Drawing.Point(9, -2);
            this.rdbPrinter.Name = "rdbPrinter";
            this.rdbPrinter.Size = new System.Drawing.Size(102, 21);
            this.rdbPrinter.TabIndex = 6096;
            this.rdbPrinter.TabStop = true;
            this.rdbPrinter.Text = "Select Printer";
            this.rdbPrinter.UseVisualStyleBackColor = true;
            this.rdbPrinter.CheckedChanged += new System.EventHandler(this.rdbPrinter_CheckedChanged);
            // 
            // txtbPrinterName
            // 
            this.txtbPrinterName.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbPrinterName.Enabled = false;
            this.txtbPrinterName.Location = new System.Drawing.Point(9, 29);
            this.txtbPrinterName.Name = "txtbPrinterName";
            this.txtbPrinterName.Size = new System.Drawing.Size(255, 25);
            this.txtbPrinterName.TabIndex = 6;
            // 
            // btnPrinter
            // 
            this.btnPrinter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrinter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(58)))), ((int)(((byte)(66)))));
            this.btnPrinter.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPrinter.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnPrinter.FlatAppearance.BorderSize = 2;
            this.btnPrinter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrinter.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrinter.ForeColor = System.Drawing.Color.White;
            this.btnPrinter.Location = new System.Drawing.Point(175, 59);
            this.btnPrinter.Name = "btnPrinter";
            this.btnPrinter.Size = new System.Drawing.Size(89, 30);
            this.btnPrinter.TabIndex = 6077;
            this.btnPrinter.Text = "Printer";
            this.btnPrinter.UseVisualStyleBackColor = false;
            this.btnPrinter.EnabledChanged += new System.EventHandler(this.btnPrinter_EnabledChanged);
            this.btnPrinter.Click += new System.EventHandler(this.btnPrinter_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.ForeColor = System.Drawing.Color.Black;
            this.lblMessage.Location = new System.Drawing.Point(549, 304);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(137, 15);
            this.lblMessage.TabIndex = 6100;
            this.lblMessage.Text = "Please press enter to add";
            // 
            // lblBlink
            // 
            this.lblBlink.AutoSize = true;
            this.lblBlink.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBlink.ForeColor = System.Drawing.Color.Red;
            this.lblBlink.Location = new System.Drawing.Point(537, 298);
            this.lblBlink.Name = "lblBlink";
            this.lblBlink.Size = new System.Drawing.Size(17, 21);
            this.lblBlink.TabIndex = 6099;
            this.lblBlink.Text = "*";
            // 
            // chkbAll
            // 
            this.chkbAll.AutoSize = true;
            this.chkbAll.Location = new System.Drawing.Point(302, 116);
            this.chkbAll.Name = "chkbAll";
            this.chkbAll.Size = new System.Drawing.Size(15, 14);
            this.chkbAll.TabIndex = 6098;
            this.chkbAll.UseVisualStyleBackColor = true;
            this.chkbAll.CheckedChanged += new System.EventHandler(this.chkbAll_CheckedChanged);
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(58)))), ((int)(((byte)(66)))));
            this.btnPrint.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPrint.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnPrint.FlatAppearance.BorderSize = 2;
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrint.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrint.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Location = new System.Drawing.Point(594, 324);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(92, 30);
            this.btnPrint.TabIndex = 6097;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.EnabledChanged += new System.EventHandler(this.btnPrint_EnabledChanged);
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // txtbPath
            // 
            this.txtbPath.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbPath.Enabled = false;
            this.txtbPath.Location = new System.Drawing.Point(9, 29);
            this.txtbPath.Name = "txtbPath";
            this.txtbPath.Size = new System.Drawing.Size(255, 25);
            this.txtbPath.TabIndex = 6093;
            // 
            // panelDate
            // 
            this.panelDate.Controls.Add(this.label9);
            this.panelDate.Controls.Add(this.label11);
            this.panelDate.Controls.Add(this.dtpTo);
            this.panelDate.Controls.Add(this.dtpFrom);
            this.panelDate.Location = new System.Drawing.Point(301, 51);
            this.panelDate.Name = "panelDate";
            this.panelDate.Size = new System.Drawing.Size(380, 52);
            this.panelDate.TabIndex = 6087;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Open Sans", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Image = ((System.Drawing.Image)(resources.GetObject("label9.Image")));
            this.label9.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label9.Location = new System.Drawing.Point(210, 7);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 18);
            this.label9.TabIndex = 6092;
            this.label9.Text = "        To :";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Open Sans", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Image = ((System.Drawing.Image)(resources.GetObject("label11.Image")));
            this.label11.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label11.Location = new System.Drawing.Point(3, 7);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(70, 18);
            this.label11.TabIndex = 6091;
            this.label11.Text = "        From :";
            // 
            // dtpTo
            // 
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTo.Location = new System.Drawing.Point(270, 4);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(101, 25);
            this.dtpTo.TabIndex = 6090;
            this.dtpTo.ValueChanged += new System.EventHandler(this.dtpTo_ValueChanged);
            this.dtpTo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dtpFrom_KeyDown);
            // 
            // dtpFrom
            // 
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrom.Location = new System.Drawing.Point(83, 4);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(101, 25);
            this.dtpFrom.TabIndex = 6088;
            this.dtpFrom.ValueChanged += new System.EventHandler(this.dtpFrom_ValueChanged);
            this.dtpFrom.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dtpFrom_KeyDown);
            // 
            // timer2
            // 
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(315, 58);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(118, 17);
            this.lblInfo.TabIndex = 6085;
            this.lblInfo.Text = "Enter Borrower Id :";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkbFooter);
            this.groupBox1.Controls.Add(this.chkbQr);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.lblTtlRecord);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cmbOption);
            this.groupBox1.Controls.Add(this.numUpBlock);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cmbPaper);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.panelDate);
            this.groupBox1.Controls.Add(this.cmbCategory);
            this.groupBox1.Controls.Add(this.lblMessage);
            this.groupBox1.Controls.Add(this.lblBlink);
            this.groupBox1.Controls.Add(this.chkbAll);
            this.groupBox1.Controls.Add(this.btnPrint);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.lblNotification);
            this.groupBox1.Controls.Add(this.dgvBrrList);
            this.groupBox1.Controls.Add(this.txtbInfo);
            this.groupBox1.Controls.Add(this.lblInfo);
            this.groupBox1.Controls.Add(this.rdbCategory);
            this.groupBox1.Controls.Add(this.rdbDate);
            this.groupBox1.Controls.Add(this.rdbManual);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Location = new System.Drawing.Point(12, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(692, 361);
            this.groupBox1.TabIndex = 6081;
            this.groupBox1.TabStop = false;
            // 
            // chkbFooter
            // 
            this.chkbFooter.AutoSize = true;
            this.chkbFooter.Location = new System.Drawing.Point(293, 330);
            this.chkbFooter.Name = "chkbFooter";
            this.chkbFooter.Size = new System.Drawing.Size(149, 21);
            this.chkbFooter.TabIndex = 6151;
            this.chkbFooter.Text = "Turn On Footer Note";
            this.chkbFooter.UseVisualStyleBackColor = true;
            // 
            // chkbQr
            // 
            this.chkbQr.AutoSize = true;
            this.chkbQr.Location = new System.Drawing.Point(293, 306);
            this.chkbQr.Name = "chkbQr";
            this.chkbQr.Size = new System.Drawing.Size(131, 21);
            this.chkbQr.TabIndex = 6150;
            this.chkbQr.Text = "Turn On QR Code";
            this.chkbQr.UseVisualStyleBackColor = true;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(290, 280);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(114, 17);
            this.label14.TabIndex = 6148;
            this.label14.Text = "Borrower Found :";
            // 
            // lblTtlRecord
            // 
            this.lblTtlRecord.AutoSize = true;
            this.lblTtlRecord.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTtlRecord.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblTtlRecord.Location = new System.Drawing.Point(400, 281);
            this.lblTtlRecord.Name = "lblTtlRecord";
            this.lblTtlRecord.Size = new System.Drawing.Size(15, 17);
            this.lblTtlRecord.TabIndex = 6147;
            this.lblTtlRecord.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(109, 17);
            this.label4.TabIndex = 6108;
            this.label4.Text = "Print Id/Barcode :";
            // 
            // cmbOption
            // 
            this.cmbOption.BackColor = System.Drawing.Color.Gainsboro;
            this.cmbOption.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOption.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbOption.ForeColor = System.Drawing.Color.Black;
            this.cmbOption.FormattingEnabled = true;
            this.cmbOption.Items.AddRange(new object[] {
            "Please choose a type...",
            "Print Id Card",
            "Print Barcode"});
            this.cmbOption.Location = new System.Drawing.Point(124, 26);
            this.cmbOption.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbOption.Name = "cmbOption";
            this.cmbOption.Size = new System.Drawing.Size(148, 25);
            this.cmbOption.TabIndex = 6107;
            this.cmbOption.SelectedIndexChanged += new System.EventHandler(this.cmbOption_SelectedIndexChanged);
            // 
            // numUpBlock
            // 
            this.numUpBlock.BackColor = System.Drawing.Color.Gainsboro;
            this.numUpBlock.Location = new System.Drawing.Point(191, 328);
            this.numUpBlock.Name = "numUpBlock";
            this.numUpBlock.ReadOnly = true;
            this.numUpBlock.Size = new System.Drawing.Size(89, 25);
            this.numUpBlock.TabIndex = 6106;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 330);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(163, 17);
            this.label3.TabIndex = 6105;
            this.label3.Text = "No of block already used :";
            // 
            // cmbPaper
            // 
            this.cmbPaper.BackColor = System.Drawing.Color.Gainsboro;
            this.cmbPaper.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPaper.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbPaper.ForeColor = System.Drawing.Color.Black;
            this.cmbPaper.FormattingEnabled = true;
            this.cmbPaper.Items.AddRange(new object[] {
            "A4_24L (3x8)",
            "A4_30L (3x10)",
            "Barcode Printer (5x2.5 Cm_1x1)",
            "Barcode Printer (5x2.5 Cm_2x1)"});
            this.cmbPaper.Location = new System.Drawing.Point(17, 293);
            this.cmbPaper.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbPaper.Name = "cmbPaper";
            this.cmbPaper.Size = new System.Drawing.Size(262, 25);
            this.cmbPaper.TabIndex = 6104;
            this.cmbPaper.SelectedIndexChanged += new System.EventHandler(this.cmbPaper_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 270);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 17);
            this.label2.TabIndex = 6103;
            this.label2.Text = "Select Paper Layout :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(6, -1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 20);
            this.label1.TabIndex = 6102;
            // 
            // cmbCategory
            // 
            this.cmbCategory.BackColor = System.Drawing.Color.White;
            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbCategory.ForeColor = System.Drawing.Color.Black;
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Items.AddRange(new object[] {
            "3x8",
            "3x10",
            "4x10",
            "5x13"});
            this.cmbCategory.Location = new System.Drawing.Point(452, 55);
            this.cmbCategory.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(198, 25);
            this.cmbCategory.TabIndex = 6101;
            this.cmbCategory.SelectedIndexChanged += new System.EventHandler(this.cmbCategory_SelectedIndexChanged);
            // 
            // rdbCategory
            // 
            this.rdbCategory.AutoSize = true;
            this.rdbCategory.Location = new System.Drawing.Point(520, 19);
            this.rdbCategory.Name = "rdbCategory";
            this.rdbCategory.Size = new System.Drawing.Size(108, 21);
            this.rdbCategory.TabIndex = 6084;
            this.rdbCategory.Text = "Category wise";
            this.rdbCategory.UseVisualStyleBackColor = true;
            this.rdbCategory.CheckedChanged += new System.EventHandler(this.rdbCategory_CheckedChanged);
            // 
            // rdbDate
            // 
            this.rdbDate.AutoSize = true;
            this.rdbDate.Location = new System.Drawing.Point(387, 19);
            this.rdbDate.Name = "rdbDate";
            this.rdbDate.Size = new System.Drawing.Size(114, 21);
            this.rdbDate.TabIndex = 6083;
            this.rdbDate.Text = "Entry date wise";
            this.rdbDate.UseVisualStyleBackColor = true;
            this.rdbDate.CheckedChanged += new System.EventHandler(this.rdbDate_CheckedChanged);
            // 
            // rdbManual
            // 
            this.rdbManual.AutoSize = true;
            this.rdbManual.Checked = true;
            this.rdbManual.Location = new System.Drawing.Point(301, 19);
            this.rdbManual.Name = "rdbManual";
            this.rdbManual.Size = new System.Drawing.Size(69, 21);
            this.rdbManual.TabIndex = 6082;
            this.rdbManual.TabStop = true;
            this.rdbManual.Text = "Manual";
            this.rdbManual.UseVisualStyleBackColor = true;
            this.rdbManual.CheckedChanged += new System.EventHandler(this.rdbManual_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gainsboro;
            this.panel1.Location = new System.Drawing.Point(286, 9);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1, 384);
            this.panel1.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rdbPdf);
            this.groupBox3.Controls.Add(this.btnBrowse);
            this.groupBox3.Controls.Add(this.txtbPath);
            this.groupBox3.Location = new System.Drawing.Point(8, 57);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(271, 99);
            this.groupBox3.TabIndex = 6096;
            this.groupBox3.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rdbPrinter);
            this.groupBox2.Controls.Add(this.txtbPrinterName);
            this.groupBox2.Controls.Add(this.btnPrinter);
            this.groupBox2.Location = new System.Drawing.Point(8, 164);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(271, 99);
            this.groupBox2.TabIndex = 6095;
            this.groupBox2.TabStop = false;
            // 
            // FormBorrowerId
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(716, 376);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormBorrowerId";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Id Card Printing";
            this.Load += new System.EventHandler(this.FormBorrowerId_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormBorrowerId_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBrrList)).EndInit();
            this.panelDate.ResumeLayout(false);
            this.panelDate.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpBlock)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtbInfo;
        private System.Windows.Forms.RadioButton rdbPdf;
        internal System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblNotification;
        internal System.Windows.Forms.DataGridView dgvBrrList;
        private System.Windows.Forms.RadioButton rdbPrinter;
        private System.Windows.Forms.TextBox txtbPrinterName;
        internal System.Windows.Forms.Button btnPrinter;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Label lblBlink;
        private System.Windows.Forms.CheckBox chkbAll;
        internal System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.TextBox txtbPath;
        private System.Windows.Forms.Panel panelDate;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdbCategory;
        private System.Windows.Forms.RadioButton rdbDate;
        private System.Windows.Forms.RadioButton rdbManual;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        internal System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown numUpBlock;
        private System.Windows.Forms.Label label3;
        internal System.Windows.Forms.ComboBox cmbPaper;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        internal System.Windows.Forms.ComboBox cmbOption;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn accnNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemTitle;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label lblTtlRecord;
        private System.Windows.Forms.CheckBox chkbFooter;
        private System.Windows.Forms.CheckBox chkbQr;
    }
}