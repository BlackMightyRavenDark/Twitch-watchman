﻿
namespace Twitch_watchman
{
    partial class FormItemEditor
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
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.textBoxChannelName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDownCopiesCount = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBoxImportant = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCopiesCount)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(145, 99);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(60, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.Location = new System.Drawing.Point(12, 99);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(60, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // textBoxChannelName
            // 
            this.textBoxChannelName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxChannelName.Location = new System.Drawing.Point(12, 23);
            this.textBoxChannelName.Name = "textBoxChannelName";
            this.textBoxChannelName.Size = new System.Drawing.Size(193, 20);
            this.textBoxChannelName.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(193, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Введите название канала: на Твиче:";
            // 
            // numericUpDownCopiesCount
            // 
            this.numericUpDownCopiesCount.Location = new System.Drawing.Point(120, 49);
            this.numericUpDownCopiesCount.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericUpDownCopiesCount.Name = "numericUpDownCopiesCount";
            this.numericUpDownCopiesCount.Size = new System.Drawing.Size(34, 20);
            this.numericUpDownCopiesCount.TabIndex = 4;
            this.numericUpDownCopiesCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Количество копий:";
            // 
            // checkBoxImportant
            // 
            this.checkBoxImportant.AutoSize = true;
            this.checkBoxImportant.Location = new System.Drawing.Point(12, 76);
            this.checkBoxImportant.Name = "checkBoxImportant";
            this.checkBoxImportant.Size = new System.Drawing.Size(100, 17);
            this.checkBoxImportant.TabIndex = 6;
            this.checkBoxImportant.Text = "Важный канал";
            this.checkBoxImportant.UseVisualStyleBackColor = true;
            // 
            // FormEditing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(217, 127);
            this.Controls.Add(this.checkBoxImportant);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numericUpDownCopiesCount);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxChannelName);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormEditing";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Добавить канал";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCopiesCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox textBoxChannelName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDownCopiesCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBoxImportant;
    }
}