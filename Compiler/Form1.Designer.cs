﻿namespace Compiler
{
    partial class Compiler
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.codeTextBox = new System.Windows.Forms.TextBox();
            this.outputTextBox = new System.Windows.Forms.TextBox();
            this.compileButton = new System.Windows.Forms.Button();
            this.optionsTabControl = new System.Windows.Forms.TabControl();
            this.editorTab = new System.Windows.Forms.TabPage();
            this.fileSelectTab = new System.Windows.Forms.TabPage();
            this.selectFileButton = new System.Windows.Forms.Button();
            this.symbolsTable = new System.Windows.Forms.DataGridView();
            this.reservedKeywordsTable = new System.Windows.Forms.DataGridView();
            this.literalsTable = new System.Windows.Forms.DataGridView();
            this.optionsTabControl.SuspendLayout();
            this.editorTab.SuspendLayout();
            this.fileSelectTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.symbolsTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reservedKeywordsTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.literalsTable)).BeginInit();
            this.SuspendLayout();
            // 
            // codeTextBox
            // 
            this.codeTextBox.Location = new System.Drawing.Point(3, 3);
            this.codeTextBox.Multiline = true;
            this.codeTextBox.Name = "codeTextBox";
            this.codeTextBox.Size = new System.Drawing.Size(329, 196);
            this.codeTextBox.TabIndex = 0;
            // 
            // outputTextBox
            // 
            this.outputTextBox.Location = new System.Drawing.Point(424, 43);
            this.outputTextBox.Multiline = true;
            this.outputTextBox.Name = "outputTextBox";
            this.outputTextBox.ReadOnly = true;
            this.outputTextBox.Size = new System.Drawing.Size(364, 206);
            this.outputTextBox.TabIndex = 1;
            // 
            // compileButton
            // 
            this.compileButton.Location = new System.Drawing.Point(351, 271);
            this.compileButton.Name = "compileButton";
            this.compileButton.Size = new System.Drawing.Size(75, 23);
            this.compileButton.TabIndex = 2;
            this.compileButton.Text = "Compile";
            this.compileButton.UseVisualStyleBackColor = true;
            this.compileButton.Click += new System.EventHandler(this.CompileButton_Click);
            // 
            // optionsTabControl
            // 
            this.optionsTabControl.Controls.Add(this.editorTab);
            this.optionsTabControl.Controls.Add(this.fileSelectTab);
            this.optionsTabControl.Location = new System.Drawing.Point(12, 23);
            this.optionsTabControl.Name = "optionsTabControl";
            this.optionsTabControl.SelectedIndex = 0;
            this.optionsTabControl.Size = new System.Drawing.Size(343, 230);
            this.optionsTabControl.TabIndex = 3;
            // 
            // editorTab
            // 
            this.editorTab.Controls.Add(this.codeTextBox);
            this.editorTab.Location = new System.Drawing.Point(4, 24);
            this.editorTab.Name = "editorTab";
            this.editorTab.Padding = new System.Windows.Forms.Padding(3);
            this.editorTab.Size = new System.Drawing.Size(335, 202);
            this.editorTab.TabIndex = 0;
            this.editorTab.Text = "Editor";
            this.editorTab.UseVisualStyleBackColor = true;
            // 
            // fileSelectTab
            // 
            this.fileSelectTab.Controls.Add(this.selectFileButton);
            this.fileSelectTab.Location = new System.Drawing.Point(4, 24);
            this.fileSelectTab.Name = "fileSelectTab";
            this.fileSelectTab.Padding = new System.Windows.Forms.Padding(3);
            this.fileSelectTab.Size = new System.Drawing.Size(335, 202);
            this.fileSelectTab.TabIndex = 1;
            this.fileSelectTab.Text = "File Select";
            this.fileSelectTab.UseVisualStyleBackColor = true;
            // 
            // selectFileButton
            // 
            this.selectFileButton.Location = new System.Drawing.Point(120, 83);
            this.selectFileButton.Name = "selectFileButton";
            this.selectFileButton.Size = new System.Drawing.Size(75, 23);
            this.selectFileButton.TabIndex = 1;
            this.selectFileButton.Text = "Select file";
            this.selectFileButton.UseVisualStyleBackColor = true;
            this.selectFileButton.Click += new System.EventHandler(this.selectFileButton_Click);
            // 
            // symbolsTable
            // 
            this.symbolsTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.symbolsTable.Location = new System.Drawing.Point(19, 318);
            this.symbolsTable.Name = "symbolsTable";
            this.symbolsTable.RowTemplate.Height = 25;
            this.symbolsTable.Size = new System.Drawing.Size(336, 150);
            this.symbolsTable.TabIndex = 4;
            // 
            // reservedKeywordsTable
            // 
            this.reservedKeywordsTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.reservedKeywordsTable.Location = new System.Drawing.Point(424, 318);
            this.reservedKeywordsTable.Name = "reservedKeywordsTable";
            this.reservedKeywordsTable.RowTemplate.Height = 25;
            this.reservedKeywordsTable.Size = new System.Drawing.Size(364, 150);
            this.reservedKeywordsTable.TabIndex = 5;
            // 
            // literalsTable
            // 
            this.literalsTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.literalsTable.Location = new System.Drawing.Point(242, 513);
            this.literalsTable.Name = "literalsTable";
            this.literalsTable.RowTemplate.Height = 25;
            this.literalsTable.Size = new System.Drawing.Size(288, 150);
            this.literalsTable.TabIndex = 6;
            // 
            // Compiler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 704);
            this.Controls.Add(this.literalsTable);
            this.Controls.Add(this.reservedKeywordsTable);
            this.Controls.Add(this.symbolsTable);
            this.Controls.Add(this.optionsTabControl);
            this.Controls.Add(this.compileButton);
            this.Controls.Add(this.outputTextBox);
            this.Name = "Compiler";
            this.Text = "Compiler";
            this.optionsTabControl.ResumeLayout(false);
            this.editorTab.ResumeLayout(false);
            this.editorTab.PerformLayout();
            this.fileSelectTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.symbolsTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reservedKeywordsTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.literalsTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox codeTextBox;
        private System.Windows.Forms.TextBox outputTextBox;
        private System.Windows.Forms.Button compileButton;
        private System.Windows.Forms.TabControl optionsTabControl;
        private System.Windows.Forms.TabPage editorTab;
        private System.Windows.Forms.TabPage fileSelectTab;
        private System.Windows.Forms.Button selectFileButton;
        private System.Windows.Forms.DataGridView symbolsTable;
        private System.Windows.Forms.DataGridView reservedKeywordsTable;
        private System.Windows.Forms.DataGridView literalsTable;
    }
}

