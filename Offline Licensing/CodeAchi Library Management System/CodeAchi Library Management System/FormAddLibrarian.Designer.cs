namespace CodeAchi_Library_Management_System
{
    partial class FormAddLibrarian
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAddLibrarian));
            this.chkbAll = new System.Windows.Forms.CheckBox();
            this.chkbEntryItems = new System.Windows.Forms.CheckBox();
            this.chkbIssueReturn = new System.Windows.Forms.CheckBox();
            this.chkbEntryMember = new System.Windows.Forms.CheckBox();
            this.chkbRemit = new System.Windows.Forms.CheckBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.txtbDesignation = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtbName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtbMailId = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtbContact = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtbAddress = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtbPassword = new System.Windows.Forms.TextBox();
            this.chkbActive = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblNotification = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpload = new System.Windows.Forms.Button();
            this.pcbUser = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.dgvUserList = new System.Windows.Forms.DataGridView();
            this.accnNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemSubject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.updateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label19 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcbUser)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUserList)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkbAll
            // 
            this.chkbAll.AutoSize = true;
            this.chkbAll.Location = new System.Drawing.Point(18, 21);
            this.chkbAll.Name = "chkbAll";
            this.chkbAll.Size = new System.Drawing.Size(41, 21);
            this.chkbAll.TabIndex = 0;
            this.chkbAll.Text = "All";
            this.chkbAll.UseVisualStyleBackColor = true;
            this.chkbAll.CheckedChanged += new System.EventHandler(this.chkbAll_CheckedChanged);
            // 
            // chkbEntryItems
            // 
            this.chkbEntryItems.AutoSize = true;
            this.chkbEntryItems.Location = new System.Drawing.Point(18, 48);
            this.chkbEntryItems.Name = "chkbEntryItems";
            this.chkbEntryItems.Size = new System.Drawing.Size(118, 21);
            this.chkbEntryItems.TabIndex = 1;
            this.chkbEntryItems.Text = "Entry/Edit Items";
            this.chkbEntryItems.UseVisualStyleBackColor = true;
            // 
            // chkbIssueReturn
            // 
            this.chkbIssueReturn.AutoSize = true;
            this.chkbIssueReturn.Location = new System.Drawing.Point(18, 75);
            this.chkbIssueReturn.Name = "chkbIssueReturn";
            this.chkbIssueReturn.Size = new System.Drawing.Size(148, 21);
            this.chkbIssueReturn.TabIndex = 2;
            this.chkbIssueReturn.Text = "Issue/Reissue/Return";
            this.chkbIssueReturn.UseVisualStyleBackColor = true;
            // 
            // chkbEntryMember
            // 
            this.chkbEntryMember.AutoSize = true;
            this.chkbEntryMember.Location = new System.Drawing.Point(18, 102);
            this.chkbEntryMember.Name = "chkbEntryMember";
            this.chkbEntryMember.Size = new System.Drawing.Size(137, 21);
            this.chkbEntryMember.TabIndex = 3;
            this.chkbEntryMember.Text = "Entry/Edit Member";
            this.chkbEntryMember.UseVisualStyleBackColor = true;
            // 
            // chkbRemit
            // 
            this.chkbRemit.AutoSize = true;
            this.chkbRemit.Location = new System.Drawing.Point(18, 129);
            this.chkbRemit.Name = "chkbRemit";
            this.chkbRemit.Size = new System.Drawing.Size(127, 21);
            this.chkbRemit.TabIndex = 4;
            this.chkbRemit.Text = "Remit Permission";
            this.chkbRemit.UseVisualStyleBackColor = true;
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.ForeColor = System.Drawing.Color.Black;
            this.Label2.Location = new System.Drawing.Point(9, 71);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(84, 17);
            this.Label2.TabIndex = 6054;
            this.Label2.Text = "Designation :";
            // 
            // txtbDesignation
            // 
            this.txtbDesignation.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbDesignation.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbDesignation.ForeColor = System.Drawing.Color.Black;
            this.txtbDesignation.Location = new System.Drawing.Point(12, 91);
            this.txtbDesignation.MaxLength = 500;
            this.txtbDesignation.Name = "txtbDesignation";
            this.txtbDesignation.Size = new System.Drawing.Size(220, 25);
            this.txtbDesignation.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(9, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 17);
            this.label1.TabIndex = 6056;
            this.label1.Text = "Name :";
            // 
            // txtbName
            // 
            this.txtbName.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbName.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbName.ForeColor = System.Drawing.Color.Black;
            this.txtbName.Location = new System.Drawing.Point(12, 41);
            this.txtbName.MaxLength = 500;
            this.txtbName.Name = "txtbName";
            this.txtbName.Size = new System.Drawing.Size(220, 25);
            this.txtbName.TabIndex = 0;
            this.txtbName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtbName_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(9, 125);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 17);
            this.label3.TabIndex = 6058;
            this.label3.Text = "E-mail Id :";
            // 
            // txtbMailId
            // 
            this.txtbMailId.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbMailId.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbMailId.ForeColor = System.Drawing.Color.Black;
            this.txtbMailId.Location = new System.Drawing.Point(12, 145);
            this.txtbMailId.MaxLength = 500;
            this.txtbMailId.Name = "txtbMailId";
            this.txtbMailId.Size = new System.Drawing.Size(220, 25);
            this.txtbMailId.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(9, 179);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 17);
            this.label4.TabIndex = 6060;
            this.label4.Text = "Contact No :";
            // 
            // txtbContact
            // 
            this.txtbContact.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbContact.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbContact.ForeColor = System.Drawing.Color.Black;
            this.txtbContact.Location = new System.Drawing.Point(12, 199);
            this.txtbContact.MaxLength = 15;
            this.txtbContact.Name = "txtbContact";
            this.txtbContact.Size = new System.Drawing.Size(220, 25);
            this.txtbContact.TabIndex = 3;
            this.txtbContact.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtbContact_KeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(261, 71);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 17);
            this.label5.TabIndex = 6062;
            this.label5.Text = "Address :";
            // 
            // txtbAddress
            // 
            this.txtbAddress.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbAddress.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbAddress.ForeColor = System.Drawing.Color.Black;
            this.txtbAddress.Location = new System.Drawing.Point(264, 90);
            this.txtbAddress.MaxLength = 500;
            this.txtbAddress.Multiline = true;
            this.txtbAddress.Name = "txtbAddress";
            this.txtbAddress.Size = new System.Drawing.Size(220, 48);
            this.txtbAddress.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(261, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 17);
            this.label6.TabIndex = 6064;
            this.label6.Text = "Password :";
            // 
            // txtbPassword
            // 
            this.txtbPassword.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbPassword.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbPassword.ForeColor = System.Drawing.Color.Black;
            this.txtbPassword.Location = new System.Drawing.Point(264, 41);
            this.txtbPassword.MaxLength = 500;
            this.txtbPassword.Name = "txtbPassword";
            this.txtbPassword.Size = new System.Drawing.Size(220, 25);
            this.txtbPassword.TabIndex = 4;
            this.txtbPassword.TextChanged += new System.EventHandler(this.txtbPassword_TextChanged);
            // 
            // chkbActive
            // 
            this.chkbActive.AutoSize = true;
            this.chkbActive.Location = new System.Drawing.Point(522, 181);
            this.chkbActive.Name = "chkbActive";
            this.chkbActive.Size = new System.Drawing.Size(116, 21);
            this.chkbActive.TabIndex = 6065;
            this.chkbActive.Text = "Active Librarian";
            this.chkbActive.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkbActive);
            this.groupBox1.Controls.Add(this.lblNotification);
            this.groupBox1.Controls.Add(this.lblMessage);
            this.groupBox1.Controls.Add(this.btnReset);
            this.groupBox1.Controls.Add(this.btnAdd);
            this.groupBox1.Controls.Add(this.btnUpload);
            this.groupBox1.Controls.Add(this.pcbUser);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.Label2);
            this.groupBox1.Controls.Add(this.txtbDesignation);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtbName);
            this.groupBox1.Controls.Add(this.txtbPassword);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtbMailId);
            this.groupBox1.Controls.Add(this.txtbAddress);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtbContact);
            this.groupBox1.Location = new System.Drawing.Point(6, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(696, 239);
            this.groupBox1.TabIndex = 6066;
            this.groupBox1.TabStop = false;
            // 
            // lblNotification
            // 
            this.lblNotification.AutoSize = true;
            this.lblNotification.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNotification.ForeColor = System.Drawing.Color.Red;
            this.lblNotification.Location = new System.Drawing.Point(348, 209);
            this.lblNotification.Name = "lblNotification";
            this.lblNotification.Size = new System.Drawing.Size(16, 20);
            this.lblNotification.TabIndex = 6113;
            this.lblNotification.Text = "*";
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.ForeColor = System.Drawing.Color.Black;
            this.lblMessage.Location = new System.Drawing.Point(362, 213);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(132, 15);
            this.lblMessage.TabIndex = 6112;
            this.lblMessage.Text = "Maximum file size 1 MB";
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(67)))), ((int)(((byte)(66)))));
            this.btnReset.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReset.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnReset.FlatAppearance.BorderSize = 2;
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReset.ForeColor = System.Drawing.Color.White;
            this.btnReset.Location = new System.Drawing.Point(605, 205);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(77, 30);
            this.btnReset.TabIndex = 6098;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(58)))), ((int)(((byte)(66)))));
            this.btnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdd.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnAdd.FlatAppearance.BorderSize = 2;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAdd.Location = new System.Drawing.Point(514, 205);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(82, 30);
            this.btnAdd.TabIndex = 6081;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnUpload
            // 
            this.btnUpload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpload.BackColor = System.Drawing.Color.Silver;
            this.btnUpload.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUpload.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnUpload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpload.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpload.Location = new System.Drawing.Point(393, 184);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(93, 26);
            this.btnUpload.TabIndex = 6;
            this.btnUpload.Text = "Upload Image";
            this.btnUpload.UseVisualStyleBackColor = false;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // pcbUser
            // 
            this.pcbUser.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pcbUser.Image = global::CodeAchi_Library_Management_System.Properties.Resources.blankBrrImage;
            this.pcbUser.Location = new System.Drawing.Point(264, 145);
            this.pcbUser.Name = "pcbUser";
            this.pcbUser.Size = new System.Drawing.Size(78, 84);
            this.pcbUser.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pcbUser.TabIndex = 6110;
            this.pcbUser.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.chkbAll);
            this.groupBox2.Controls.Add(this.chkbEntryItems);
            this.groupBox2.Controls.Add(this.chkbIssueReturn);
            this.groupBox2.Controls.Add(this.chkbRemit);
            this.groupBox2.Controls.Add(this.chkbEntryMember);
            this.groupBox2.Location = new System.Drawing.Point(516, 17);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(168, 158);
            this.groupBox2.TabIndex = 6067;
            this.groupBox2.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(8, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(90, 17);
            this.label7.TabIndex = 6082;
            this.label7.Text = "User Privilege";
            // 
            // dgvUserList
            // 
            this.dgvUserList.AllowUserToAddRows = false;
            this.dgvUserList.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvUserList.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvUserList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvUserList.BackgroundColor = System.Drawing.Color.Gainsboro;
            this.dgvUserList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvUserList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(82)))), ((int)(((byte)(90)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvUserList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvUserList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUserList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.accnNo,
            this.itemTitle,
            this.itemSubject,
            this.Column2,
            this.Column1});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.PowderBlue;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvUserList.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvUserList.EnableHeadersVisualStyles = false;
            this.dgvUserList.Location = new System.Drawing.Point(6, 256);
            this.dgvUserList.MultiSelect = false;
            this.dgvUserList.Name = "dgvUserList";
            this.dgvUserList.ReadOnly = true;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.ButtonShadow;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvUserList.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvUserList.RowHeadersVisible = false;
            this.dgvUserList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUserList.Size = new System.Drawing.Size(696, 159);
            this.dgvUserList.TabIndex = 6099;
            this.dgvUserList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgvUserList_MouseDown);
            // 
            // accnNo
            // 
            this.accnNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.accnNo.HeaderText = "User Name";
            this.accnNo.Name = "accnNo";
            this.accnNo.ReadOnly = true;
            this.accnNo.Width = 150;
            // 
            // itemTitle
            // 
            this.itemTitle.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.itemTitle.HeaderText = "Designation";
            this.itemTitle.Name = "itemTitle";
            this.itemTitle.ReadOnly = true;
            this.itemTitle.Width = 150;
            // 
            // itemSubject
            // 
            this.itemSubject.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.itemSubject.DefaultCellStyle = dataGridViewCellStyle3;
            this.itemSubject.HeaderText = "Contact No";
            this.itemSubject.Name = "itemSubject";
            this.itemSubject.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column2.HeaderText = "User Id";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 234;
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column1.HeaderText = "Status";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 60;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.updateToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(109, 48);
            // 
            // updateToolStripMenuItem
            // 
            this.updateToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(82)))), ((int)(((byte)(90)))));
            this.updateToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.updateToolStripMenuItem.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.updateToolStripMenuItem.Name = "updateToolStripMenuItem";
            this.updateToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.updateToolStripMenuItem.Text = "Edit";
            this.updateToolStripMenuItem.Click += new System.EventHandler(this.updateToolStripMenuItem_Click);
            this.updateToolStripMenuItem.MouseEnter += new System.EventHandler(this.updateToolStripMenuItem_MouseEnter);
            this.updateToolStripMenuItem.MouseLeave += new System.EventHandler(this.updateToolStripMenuItem_MouseLeave);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(82)))), ((int)(((byte)(90)))));
            this.deleteToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deleteToolStripMenuItem.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            this.deleteToolStripMenuItem.MouseEnter += new System.EventHandler(this.deleteToolStripMenuItem_MouseEnter);
            this.deleteToolStripMenuItem.MouseLeave += new System.EventHandler(this.deleteToolStripMenuItem_MouseLeave);
            // 
            // label19
            // 
            this.label19.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.ForeColor = System.Drawing.Color.Red;
            this.label19.Location = new System.Drawing.Point(3, 420);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(227, 15);
            this.label19.TabIndex = 6116;
            this.label19.Text = "* To Update/Delete a user right click on it.";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(126)))), ((int)(((byte)(176)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label11);
            this.panel1.Location = new System.Drawing.Point(6, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(696, 24);
            this.panel1.TabIndex = 456471;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(4, 3);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(111, 17);
            this.label11.TabIndex = 23;
            this.label11.Text = "User Information";
            // 
            // FormAddLibrarian
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(708, 440);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.dgvUserList);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAddLibrarian";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add Librarian";
            this.Load += new System.EventHandler(this.FormAdminSetting_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormAdminSetting_Paint);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcbUser)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUserList)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkbAll;
        private System.Windows.Forms.CheckBox chkbEntryItems;
        private System.Windows.Forms.CheckBox chkbIssueReturn;
        private System.Windows.Forms.CheckBox chkbEntryMember;
        private System.Windows.Forms.CheckBox chkbRemit;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.TextBox txtbDesignation;
        internal System.Windows.Forms.Label label1;
        internal System.Windows.Forms.TextBox txtbName;
        internal System.Windows.Forms.Label label3;
        internal System.Windows.Forms.TextBox txtbMailId;
        internal System.Windows.Forms.Label label4;
        internal System.Windows.Forms.TextBox txtbContact;
        internal System.Windows.Forms.Label label5;
        internal System.Windows.Forms.TextBox txtbAddress;
        internal System.Windows.Forms.Label label6;
        internal System.Windows.Forms.TextBox txtbPassword;
        private System.Windows.Forms.CheckBox chkbActive;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        internal System.Windows.Forms.Label label7;
        internal System.Windows.Forms.Button btnAdd;
        internal System.Windows.Forms.Button btnReset;
        internal System.Windows.Forms.DataGridView dgvUserList;
        private System.Windows.Forms.Label lblMessage;
        internal System.Windows.Forms.Button btnUpload;
        internal System.Windows.Forms.PictureBox pcbUser;
        private System.Windows.Forms.Label lblNotification;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem updateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        internal System.Windows.Forms.Label label19;
        private System.Windows.Forms.DataGridViewTextBoxColumn accnNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemSubject;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label11;
    }
}