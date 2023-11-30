namespace WinFormsApp
{
    partial class AddBookingForm
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
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            txtBox_FullName = new TextBox();
            txtBox_Email = new TextBox();
            txtBox_PhoneNumber = new TextBox();
            txtBox_Notes = new TextBox();
            btn_OK = new Button();
            btn_Cancel = new Button();
            dtp_BookingDate = new DateTimePicker();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            cmb_AvailableTimeSlot = new ComboBox();
            btn_Add = new Button();
            btn_Remove = new Button();
            lbo_SelectedTime = new ListBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(30, 199);
            label1.Name = "label1";
            label1.Size = new Size(95, 15);
            label1.TabIndex = 0;
            label1.Text = "Full name . . . . . .";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(30, 236);
            label2.Name = "label2";
            label2.Size = new Size(96, 15);
            label2.TabIndex = 1;
            label2.Text = "Email . . . . . . . . . .";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(30, 274);
            label3.Name = "label3";
            label3.Size = new Size(122, 15);
            label3.TabIndex = 2;
            label3.Text = "Phone number . . . . . .";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(30, 313);
            label4.Name = "label4";
            label4.Size = new Size(108, 15);
            label4.TabIndex = 3;
            label4.Text = "Note . . . . . . . . . . . . ";
            // 
            // txtBox_FullName
            // 
            txtBox_FullName.Location = new Point(131, 194);
            txtBox_FullName.Margin = new Padding(3, 2, 3, 2);
            txtBox_FullName.Name = "txtBox_FullName";
            txtBox_FullName.Size = new Size(309, 23);
            txtBox_FullName.TabIndex = 4;
            txtBox_FullName.TextChanged += txtBox_FullName_TextChanged;
            // 
            // txtBox_Email
            // 
            txtBox_Email.Location = new Point(131, 230);
            txtBox_Email.Margin = new Padding(3, 2, 3, 2);
            txtBox_Email.Name = "txtBox_Email";
            txtBox_Email.Size = new Size(309, 23);
            txtBox_Email.TabIndex = 5;
            txtBox_Email.TextChanged += txtBox_Email_TextChanged;
            // 
            // txtBox_PhoneNumber
            // 
            txtBox_PhoneNumber.Location = new Point(131, 268);
            txtBox_PhoneNumber.Margin = new Padding(3, 2, 3, 2);
            txtBox_PhoneNumber.Name = "txtBox_PhoneNumber";
            txtBox_PhoneNumber.Size = new Size(309, 23);
            txtBox_PhoneNumber.TabIndex = 6;
            txtBox_PhoneNumber.TextChanged += txtBox_PhoneNumber_TextChanged;
            // 
            // txtBox_Notes
            // 
            txtBox_Notes.Location = new Point(131, 306);
            txtBox_Notes.Margin = new Padding(3, 2, 3, 2);
            txtBox_Notes.Name = "txtBox_Notes";
            txtBox_Notes.Size = new Size(309, 23);
            txtBox_Notes.TabIndex = 7;
            // 
            // btn_OK
            // 
            btn_OK.Enabled = false;
            btn_OK.Location = new Point(283, 340);
            btn_OK.Margin = new Padding(3, 2, 3, 2);
            btn_OK.Name = "btn_OK";
            btn_OK.Size = new Size(70, 22);
            btn_OK.TabIndex = 8;
            btn_OK.Text = "OK";
            btn_OK.UseVisualStyleBackColor = true;
            btn_OK.Click += btn_OK_Click;
            // 
            // btn_Cancel
            // 
            btn_Cancel.Location = new Point(358, 340);
            btn_Cancel.Margin = new Padding(3, 2, 3, 2);
            btn_Cancel.Name = "btn_Cancel";
            btn_Cancel.Size = new Size(82, 22);
            btn_Cancel.TabIndex = 9;
            btn_Cancel.Text = "Cancel";
            btn_Cancel.UseVisualStyleBackColor = true;
            btn_Cancel.Click += btn_Cancel_Click;
            // 
            // dtp_BookingDate
            // 
            dtp_BookingDate.Format = DateTimePickerFormat.Short;
            dtp_BookingDate.Location = new Point(30, 20);
            dtp_BookingDate.Margin = new Padding(3, 2, 3, 2);
            dtp_BookingDate.Name = "dtp_BookingDate";
            dtp_BookingDate.Size = new Size(114, 23);
            dtp_BookingDate.TabIndex = 10;
            dtp_BookingDate.ValueChanged += dtp_BookingDate_ValueChanged;
            // 
            // cmb_AvailableTimeSlot
            // 
            cmb_AvailableTimeSlot.FormattingEnabled = true;
            cmb_AvailableTimeSlot.Location = new Point(30, 56);
            cmb_AvailableTimeSlot.Margin = new Padding(3, 2, 3, 2);
            cmb_AvailableTimeSlot.Name = "cmb_AvailableTimeSlot";
            cmb_AvailableTimeSlot.Size = new Size(176, 23);
            cmb_AvailableTimeSlot.TabIndex = 11;
            cmb_AvailableTimeSlot.SelectedIndexChanged += cmb_AvailableTimeSlot_SelectedIndexChanged;
            cmb_AvailableTimeSlot.SelectedValueChanged += cmb_AvailableTimeSlot_SelectedValueChanged;
            // 
            // btn_Add
            // 
            btn_Add.Enabled = false;
            btn_Add.Location = new Point(30, 112);
            btn_Add.Margin = new Padding(3, 2, 3, 2);
            btn_Add.Name = "btn_Add";
            btn_Add.Size = new Size(69, 22);
            btn_Add.TabIndex = 12;
            btn_Add.Text = "Add";
            btn_Add.UseVisualStyleBackColor = true;
            btn_Add.Click += btn_Add_Click;
            // 
            // btn_Remove
            // 
            btn_Remove.Enabled = false;
            btn_Remove.Location = new Point(112, 112);
            btn_Remove.Margin = new Padding(3, 2, 3, 2);
            btn_Remove.Name = "btn_Remove";
            btn_Remove.Size = new Size(75, 22);
            btn_Remove.TabIndex = 13;
            btn_Remove.Text = "Remove";
            btn_Remove.UseVisualStyleBackColor = true;
            btn_Remove.Click += btn_Remove_Click;
            // 
            // lbo_SelectedTime
            // 
            lbo_SelectedTime.Enabled = false;
            lbo_SelectedTime.FormattingEnabled = true;
            lbo_SelectedTime.ItemHeight = 15;
            lbo_SelectedTime.Items.AddRange(new object[] { "No bookings selected...." });
            lbo_SelectedTime.Location = new Point(262, 40);
            lbo_SelectedTime.Margin = new Padding(3, 2, 3, 2);
            lbo_SelectedTime.Name = "lbo_SelectedTime";
            lbo_SelectedTime.Size = new Size(190, 94);
            lbo_SelectedTime.TabIndex = 14;
            lbo_SelectedTime.SelectedIndexChanged += lbo_SelectedTime_SelectedIndexChanged;
            lbo_SelectedTime.EnabledChanged += lbo_SelectedTime_EnabledChanged;
            // 
            // AddBookingForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(498, 385);
            Controls.Add(lbo_SelectedTime);
            Controls.Add(btn_Remove);
            Controls.Add(btn_Add);
            Controls.Add(cmb_AvailableTimeSlot);
            Controls.Add(dtp_BookingDate);
            Controls.Add(btn_Cancel);
            Controls.Add(btn_OK);
            Controls.Add(txtBox_Notes);
            Controls.Add(txtBox_PhoneNumber);
            Controls.Add(txtBox_Email);
            Controls.Add(txtBox_FullName);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "AddBookingForm";
            Text = "Add booking";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private TextBox txtBox_FullName;
        private TextBox txtBox_Email;
        private TextBox txtBox_PhoneNumber;
        private TextBox txtBox_Notes;
        private Button btn_OK;
        private Button btn_Cancel;
        private DateTimePicker dtp_BookingDate;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private ComboBox cmb_AvailableTimeSlot;
        private Button btn_Add;
        private Button btn_Remove;
        private ListBox lbo_SelectedTime;
    }
}