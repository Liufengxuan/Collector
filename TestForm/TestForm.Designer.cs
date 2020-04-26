namespace TestForm
{
    partial class TestForm
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
            this.btn_TCPStart = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btn_SPstart = new System.Windows.Forms.Button();
            this.btn_OpenConfig = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_Find = new System.Windows.Forms.Button();
            this.btn_RemoveTask = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btn_addtask = new System.Windows.Forms.Button();
            this.tb_Code = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tb_taskName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_data = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_Station = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.btn_close = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.button3 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_TCPStart
            // 
            this.btn_TCPStart.Location = new System.Drawing.Point(12, 12);
            this.btn_TCPStart.Name = "btn_TCPStart";
            this.btn_TCPStart.Size = new System.Drawing.Size(75, 23);
            this.btn_TCPStart.TabIndex = 2;
            this.btn_TCPStart.Text = "网口启动";
            this.btn_TCPStart.UseVisualStyleBackColor = true;
            this.btn_TCPStart.Click += new System.EventHandler(this.btn_TCPStart_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(3, 3);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(383, 226);
            this.textBox1.TabIndex = 0;
            // 
            // btn_SPstart
            // 
            this.btn_SPstart.Location = new System.Drawing.Point(93, 12);
            this.btn_SPstart.Name = "btn_SPstart";
            this.btn_SPstart.Size = new System.Drawing.Size(75, 23);
            this.btn_SPstart.TabIndex = 2;
            this.btn_SPstart.Text = "串口启动";
            this.btn_SPstart.UseVisualStyleBackColor = true;
            this.btn_SPstart.Click += new System.EventHandler(this.btn_SPstart_Click);
            // 
            // btn_OpenConfig
            // 
            this.btn_OpenConfig.Location = new System.Drawing.Point(311, 235);
            this.btn_OpenConfig.Name = "btn_OpenConfig";
            this.btn_OpenConfig.Size = new System.Drawing.Size(75, 23);
            this.btn_OpenConfig.TabIndex = 2;
            this.btn_OpenConfig.Text = "打开配置文件";
            this.btn_OpenConfig.UseVisualStyleBackColor = true;
            this.btn_OpenConfig.Click += new System.EventHandler(this.btn_OpenConfig_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btn_Find);
            this.panel1.Controls.Add(this.btn_RemoveTask);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.btn_addtask);
            this.panel1.Controls.Add(this.tb_Code);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.tb_taskName);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.tb_data);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.tb_Station);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.textBox2);
            this.panel1.Controls.Add(this.btn_OpenConfig);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Enabled = false;
            this.panel1.Location = new System.Drawing.Point(12, 41);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1220, 574);
            this.panel1.TabIndex = 3;
            // 
            // btn_Find
            // 
            this.btn_Find.Location = new System.Drawing.Point(207, 530);
            this.btn_Find.Name = "btn_Find";
            this.btn_Find.Size = new System.Drawing.Size(75, 23);
            this.btn_Find.TabIndex = 13;
            this.btn_Find.Text = "查找";
            this.btn_Find.UseVisualStyleBackColor = true;
            this.btn_Find.Click += new System.EventHandler(this.btn_Find_Click);
            // 
            // btn_RemoveTask
            // 
            this.btn_RemoveTask.Location = new System.Drawing.Point(207, 501);
            this.btn_RemoveTask.Name = "btn_RemoveTask";
            this.btn_RemoveTask.Size = new System.Drawing.Size(75, 23);
            this.btn_RemoveTask.TabIndex = 13;
            this.btn_RemoveTask.Text = "删除";
            this.btn_RemoveTask.UseVisualStyleBackColor = true;
            this.btn_RemoveTask.Click += new System.EventHandler(this.btn_RemoveTask_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(288, 472);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 12;
            this.button2.Text = "添加";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button2_MouseDown);
            this.button2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.button2_MouseUp);
            // 
            // btn_addtask
            // 
            this.btn_addtask.Location = new System.Drawing.Point(207, 472);
            this.btn_addtask.Name = "btn_addtask";
            this.btn_addtask.Size = new System.Drawing.Size(75, 23);
            this.btn_addtask.TabIndex = 12;
            this.btn_addtask.Text = "添加递增";
            this.btn_addtask.UseVisualStyleBackColor = true;
            this.btn_addtask.Click += new System.EventHandler(this.btn_addtask_Click);
            // 
            // tb_Code
            // 
            this.tb_Code.FormattingEnabled = true;
            this.tb_Code.Items.AddRange(new object[] {
            "03",
            "06",
            "16"});
            this.tb_Code.Location = new System.Drawing.Point(207, 298);
            this.tb_Code.Name = "tb_Code";
            this.tb_Code.Size = new System.Drawing.Size(121, 20);
            this.tb_Code.TabIndex = 11;
            this.tb_Code.Text = "03";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(134, 442);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "任务名称";
            // 
            // tb_taskName
            // 
            this.tb_taskName.Location = new System.Drawing.Point(205, 433);
            this.tb_taskName.Name = "tb_taskName";
            this.tb_taskName.Size = new System.Drawing.Size(100, 21);
            this.tb_taskName.TabIndex = 10;
            this.tb_taskName.Text = "t2";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(140, 395);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "数据(HEX)";
            // 
            // tb_data
            // 
            this.tb_data.Location = new System.Drawing.Point(205, 392);
            this.tb_data.Name = "tb_data";
            this.tb_data.Size = new System.Drawing.Size(100, 21);
            this.tb_data.TabIndex = 8;
            this.tb_data.Text = "00020001";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(140, 344);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "站号(HEX)";
            // 
            // tb_Station
            // 
            this.tb_Station.Location = new System.Drawing.Point(205, 341);
            this.tb_Station.Name = "tb_Station";
            this.tb_Station.Size = new System.Drawing.Size(100, 21);
            this.tb_Station.TabIndex = 6;
            this.tb_Station.Text = "0A";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(134, 301);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "指令码(HEX)";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(418, 7);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox2.Size = new System.Drawing.Size(788, 557);
            this.textBox2.TabIndex = 3;
            // 
            // btn_close
            // 
            this.btn_close.Location = new System.Drawing.Point(174, 12);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(75, 23);
            this.btn_close.TabIndex = 2;
            this.btn_close.Text = "关闭";
            this.btn_close.UseVisualStyleBackColor = true;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(265, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "释放资源";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(357, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "label1";
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(518, 6);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "打开配置文件";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(46, 642);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 5;
            this.label6.Text = "label6";
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1244, 725);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.btn_SPstart);
            this.Controls.Add(this.btn_TCPStart);
            this.Controls.Add(this.button3);
            this.Name = "TestForm";
            this.Text = "TestForm";
            this.Load += new System.EventHandler(this.TestForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btn_TCPStart;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btn_SPstart;
        private System.Windows.Forms.Button btn_OpenConfig;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tb_taskName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_data;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb_Station;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox tb_Code;
        private System.Windows.Forms.Button btn_addtask;
        private System.Windows.Forms.Button btn_RemoveTask;
        private System.Windows.Forms.Button btn_Find;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label6;
    }
}