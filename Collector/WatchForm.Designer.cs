namespace Collector
{
    partial class WatchForm
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btn_StopRefresh = new System.Windows.Forms.Button();
            this.btn_Clear = new System.Windows.Forms.Button();
            this.timer_refresh = new System.Windows.Forms.Timer(this.components);
            this.textBox_Search = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button_search = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(7, 82);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(642, 383);
            this.textBox1.TabIndex = 0;
            this.textBox1.WordWrap = false;
            // 
            // btn_StopRefresh
            // 
            this.btn_StopRefresh.BackColor = System.Drawing.Color.Green;
            this.btn_StopRefresh.Location = new System.Drawing.Point(294, 51);
            this.btn_StopRefresh.Name = "btn_StopRefresh";
            this.btn_StopRefresh.Size = new System.Drawing.Size(351, 27);
            this.btn_StopRefresh.TabIndex = 1;
            this.btn_StopRefresh.Text = "停止更新";
            this.btn_StopRefresh.UseVisualStyleBackColor = false;
            this.btn_StopRefresh.Click += new System.EventHandler(this.btn_StopRefresh_Click);
            // 
            // btn_Clear
            // 
            this.btn_Clear.Location = new System.Drawing.Point(7, 51);
            this.btn_Clear.Name = "btn_Clear";
            this.btn_Clear.Size = new System.Drawing.Size(266, 27);
            this.btn_Clear.TabIndex = 2;
            this.btn_Clear.Text = "清空显示";
            this.btn_Clear.UseVisualStyleBackColor = true;
            this.btn_Clear.Click += new System.EventHandler(this.btn_Clear_Click);
            // 
            // timer_refresh
            // 
            this.timer_refresh.Enabled = true;
            this.timer_refresh.Interval = 700;
            this.timer_refresh.Tick += new System.EventHandler(this.timer_refresh_Tick);
            // 
            // textBox_Search
            // 
            this.textBox_Search.Location = new System.Drawing.Point(334, 12);
            this.textBox_Search.Name = "textBox_Search";
            this.textBox_Search.Size = new System.Drawing.Size(150, 21);
            this.textBox_Search.TabIndex = 3;
            this.textBox_Search.TextChanged += new System.EventHandler(this.textBox_Search_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(275, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "搜索任务";
            // 
            // button_search
            // 
            this.button_search.Location = new System.Drawing.Point(490, 11);
            this.button_search.Name = "button_search";
            this.button_search.Size = new System.Drawing.Size(141, 23);
            this.button_search.TabIndex = 5;
            this.button_search.Text = "搜索并停止更新";
            this.button_search.UseVisualStyleBackColor = true;
            this.button_search.Click += new System.EventHandler(this.button_search_Click);
            // 
            // WatchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(657, 471);
            this.Controls.Add(this.button_search);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_Search);
            this.Controls.Add(this.btn_Clear);
            this.Controls.Add(this.btn_StopRefresh);
            this.Controls.Add(this.textBox1);
            this.Name = "WatchForm";
            this.Text = "监控窗口";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.WatchForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btn_StopRefresh;
        private System.Windows.Forms.Button btn_Clear;
        private System.Windows.Forms.Timer timer_refresh;
        private System.Windows.Forms.TextBox textBox_Search;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_search;
    }
}