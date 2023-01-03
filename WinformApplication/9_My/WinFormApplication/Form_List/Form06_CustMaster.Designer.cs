namespace Form_List
{
    partial class Form06_CustMaster
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label3 = new System.Windows.Forms.Label();
            this.textbox3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cboCustType = new System.Windows.Forms.ComboBox();
            this.txtCustName = new System.Windows.Forms.TextBox();
            this.txtCustCode = new System.Windows.Forms.TextBox();
            this.dgvGrid = new System.Windows.Forms.DataGridView();
            this.groupBox11.SuspendLayout();
            this.groupBox22.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.label3);
            this.groupBox11.Controls.Add(this.textbox3);
            this.groupBox11.Controls.Add(this.label1);
            this.groupBox11.Controls.Add(this.cboCustType);
            this.groupBox11.Controls.Add(this.txtCustName);
            this.groupBox11.Controls.Add(this.txtCustCode);
            // 
            // groupBox22
            // 
            this.groupBox22.Controls.Add(this.dgvGrid);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(548, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "거래처구분";
            // 
            // textbox3
            // 
            this.textbox3.AutoSize = true;
            this.textbox3.Location = new System.Drawing.Point(288, 44);
            this.textbox3.Name = "textbox3";
            this.textbox3.Size = new System.Drawing.Size(41, 12);
            this.textbox3.TabIndex = 7;
            this.textbox3.Text = "업체명";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "업체코드";
            // 
            // cboCustType
            // 
            this.cboCustType.FormattingEnabled = true;
            this.cboCustType.Location = new System.Drawing.Point(619, 41);
            this.cboCustType.Name = "cboCustType";
            this.cboCustType.Size = new System.Drawing.Size(150, 20);
            this.cboCustType.TabIndex = 5;
            // 
            // txtCustName
            // 
            this.txtCustName.Location = new System.Drawing.Point(347, 39);
            this.txtCustName.Name = "txtCustName";
            this.txtCustName.Size = new System.Drawing.Size(150, 21);
            this.txtCustName.TabIndex = 3;
            // 
            // txtCustCode
            // 
            this.txtCustCode.Location = new System.Drawing.Point(89, 39);
            this.txtCustCode.Name = "txtCustCode";
            this.txtCustCode.Size = new System.Drawing.Size(150, 21);
            this.txtCustCode.TabIndex = 4;
            // 
            // dgvGrid
            // 
            this.dgvGrid.AllowUserToAddRows = false;
            this.dgvGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvGrid.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvGrid.Location = new System.Drawing.Point(3, 17);
            this.dgvGrid.Name = "dgvGrid";
            this.dgvGrid.RowTemplate.Height = 23;
            this.dgvGrid.Size = new System.Drawing.Size(794, 330);
            this.dgvGrid.TabIndex = 1;
            // 
            // Form06_CustMaster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "Form06_CustMaster";
            this.Text = "거래처마스터";
            this.Load += new System.EventHandler(this.Form06_CustMaster_Load);
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            this.groupBox22.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label textbox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboCustType;
        private System.Windows.Forms.TextBox txtCustName;
        private System.Windows.Forms.TextBox txtCustCode;
        private System.Windows.Forms.DataGridView dgvGrid;
    }
}