namespace VANEK_3
{
    partial class SalesForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnViewDetails;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblFilterDateFrom;
        private System.Windows.Forms.DateTimePicker dtpFilterDateFrom;
        private System.Windows.Forms.Label lblFilterDateTo;
        private System.Windows.Forms.DateTimePicker dtpFilterDateTo;
        private System.Windows.Forms.Label lblFilterCustomer;
        private System.Windows.Forms.TextBox txtFilterCustomer;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.Button btnClearFilter;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnViewDetails = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblFilterDateFrom = new System.Windows.Forms.Label();
            this.dtpFilterDateFrom = new System.Windows.Forms.DateTimePicker();
            this.lblFilterDateTo = new System.Windows.Forms.Label();
            this.dtpFilterDateTo = new System.Windows.Forms.DateTimePicker();
            this.lblFilterCustomer = new System.Windows.Forms.Label();
            this.txtFilterCustomer = new System.Windows.Forms.TextBox();
            this.btnFilter = new System.Windows.Forms.Button();
            this.btnClearFilter = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            
            // dataGridView
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Location = new System.Drawing.Point(12, 80);
            this.dataGridView.MultiSelect = false;
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.Size = new System.Drawing.Size(760, 332);
            this.dataGridView.TabIndex = 0;
            
            // lblFilterDateFrom
            this.lblFilterDateFrom.AutoSize = true;
            this.lblFilterDateFrom.Location = new System.Drawing.Point(12, 15);
            this.lblFilterDateFrom.Name = "lblFilterDateFrom";
            this.lblFilterDateFrom.Size = new System.Drawing.Size(45, 13);
            this.lblFilterDateFrom.TabIndex = 7;
            this.lblFilterDateFrom.Text = "Дата с:";
            
            // dtpFilterDateFrom
            this.dtpFilterDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFilterDateFrom.Location = new System.Drawing.Point(63, 12);
            this.dtpFilterDateFrom.Name = "dtpFilterDateFrom";
            this.dtpFilterDateFrom.Size = new System.Drawing.Size(120, 20);
            this.dtpFilterDateFrom.TabIndex = 8;
            this.dtpFilterDateFrom.Value = System.DateTime.Now.AddMonths(-1);
            
            // lblFilterDateTo
            this.lblFilterDateTo.AutoSize = true;
            this.lblFilterDateTo.Location = new System.Drawing.Point(200, 15);
            this.lblFilterDateTo.Name = "lblFilterDateTo";
            this.lblFilterDateTo.Size = new System.Drawing.Size(52, 13);
            this.lblFilterDateTo.TabIndex = 9;
            this.lblFilterDateTo.Text = "Дата по:";
            
            // dtpFilterDateTo
            this.dtpFilterDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFilterDateTo.Location = new System.Drawing.Point(258, 12);
            this.dtpFilterDateTo.Name = "dtpFilterDateTo";
            this.dtpFilterDateTo.Size = new System.Drawing.Size(120, 20);
            this.dtpFilterDateTo.TabIndex = 10;
            this.dtpFilterDateTo.Value = System.DateTime.Now;
            
            // lblFilterCustomer
            this.lblFilterCustomer.AutoSize = true;
            this.lblFilterCustomer.Location = new System.Drawing.Point(12, 45);
            this.lblFilterCustomer.Name = "lblFilterCustomer";
            this.lblFilterCustomer.Size = new System.Drawing.Size(46, 13);
            this.lblFilterCustomer.TabIndex = 11;
            this.lblFilterCustomer.Text = "Клиент:";
            
            // txtFilterCustomer
            this.txtFilterCustomer.Location = new System.Drawing.Point(64, 42);
            this.txtFilterCustomer.Name = "txtFilterCustomer";
            this.txtFilterCustomer.Size = new System.Drawing.Size(314, 20);
            this.txtFilterCustomer.TabIndex = 12;
            
            // btnFilter
            this.btnFilter.Location = new System.Drawing.Point(400, 40);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(100, 25);
            this.btnFilter.TabIndex = 13;
            this.btnFilter.Text = "Применить";
            this.btnFilter.UseVisualStyleBackColor = true;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            
            // btnClearFilter
            this.btnClearFilter.Location = new System.Drawing.Point(510, 40);
            this.btnClearFilter.Name = "btnClearFilter";
            this.btnClearFilter.Size = new System.Drawing.Size(100, 25);
            this.btnClearFilter.TabIndex = 14;
            this.btnClearFilter.Text = "Очистить";
            this.btnClearFilter.UseVisualStyleBackColor = true;
            this.btnClearFilter.Click += new System.EventHandler(this.btnClearFilter_Click);
            
            // btnAdd
            this.btnAdd.Location = new System.Drawing.Point(12, 430);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(100, 30);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "Добавить";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            
            // btnEdit
            this.btnEdit.Location = new System.Drawing.Point(130, 430);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(100, 30);
            this.btnEdit.TabIndex = 2;
            this.btnEdit.Text = "Редактировать";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            
            // btnDelete
            this.btnDelete.Location = new System.Drawing.Point(248, 430);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(100, 30);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "Удалить";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            
            // btnViewDetails
            this.btnViewDetails.Location = new System.Drawing.Point(366, 430);
            this.btnViewDetails.Name = "btnViewDetails";
            this.btnViewDetails.Size = new System.Drawing.Size(120, 30);
            this.btnViewDetails.TabIndex = 4;
            this.btnViewDetails.Text = "Просмотр деталей";
            this.btnViewDetails.UseVisualStyleBackColor = true;
            this.btnViewDetails.Click += new System.EventHandler(this.btnViewDetails_Click);
            
            // btnRefresh
            this.btnRefresh.Location = new System.Drawing.Point(500, 430);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 30);
            this.btnRefresh.TabIndex = 5;
            this.btnRefresh.Text = "Обновить";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            
            // btnClose
            this.btnClose.Location = new System.Drawing.Point(672, 430);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 30);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Закрыть";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            
            // SalesForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 472);
            this.Controls.Add(this.btnClearFilter);
            this.Controls.Add(this.btnFilter);
            this.Controls.Add(this.txtFilterCustomer);
            this.Controls.Add(this.lblFilterCustomer);
            this.Controls.Add(this.dtpFilterDateTo);
            this.Controls.Add(this.lblFilterDateTo);
            this.Controls.Add(this.dtpFilterDateFrom);
            this.Controls.Add(this.lblFilterDateFrom);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnViewDetails);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.dataGridView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "SalesForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Продажи";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}

