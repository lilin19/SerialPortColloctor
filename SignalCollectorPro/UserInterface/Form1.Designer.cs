namespace SignalCollectorPro
{
    partial class Form1
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.datagroup = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.Save = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.RealTemp = new System.Windows.Forms.RichTextBox();
            this.Temp = new System.Windows.Forms.RichTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.RealMea = new System.Windows.Forms.RichTextBox();
            this.Mea = new System.Windows.Forms.RichTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.start = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.Ports = new System.Windows.Forms.ComboBox();
            this.Content = new System.Windows.Forms.Label();
            this.SignalContent = new System.Windows.Forms.RichTextBox();
            this.Time = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Length = new System.Windows.Forms.RichTextBox();
            this.SignalLength = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.clear = new System.Windows.Forms.Button();
            this.Screen = new System.Windows.Forms.ListView();
            this.button6 = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.datagroup.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(776, 435);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.datagroup);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(768, 409);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "串口信息录入";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // datagroup
            // 
            this.datagroup.Controls.Add(this.button3);
            this.datagroup.Controls.Add(this.Save);
            this.datagroup.Controls.Add(this.groupBox3);
            this.datagroup.Controls.Add(this.groupBox2);
            this.datagroup.Location = new System.Drawing.Point(6, 196);
            this.datagroup.Name = "datagroup";
            this.datagroup.Size = new System.Drawing.Size(745, 207);
            this.datagroup.TabIndex = 7;
            this.datagroup.TabStop = false;
            this.datagroup.Text = "数据录入";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(619, 104);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 35);
            this.button3.TabIndex = 5;
            this.button3.Text = "查看温度曲线";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // Save
            // 
            this.Save.Location = new System.Drawing.Point(619, 49);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(75, 23);
            this.Save.TabIndex = 2;
            this.Save.Text = "保存数据";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.RealTemp);
            this.groupBox3.Controls.Add(this.Temp);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Location = new System.Drawing.Point(331, 19);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(270, 162);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "℃";
            // 
            // RealTemp
            // 
            this.RealTemp.Location = new System.Drawing.Point(116, 93);
            this.RealTemp.Name = "RealTemp";
            this.RealTemp.Size = new System.Drawing.Size(130, 25);
            this.RealTemp.TabIndex = 6;
            this.RealTemp.Text = "";
            // 
            // Temp
            // 
            this.Temp.Location = new System.Drawing.Point(116, 37);
            this.Temp.Name = "Temp";
            this.Temp.ReadOnly = true;
            this.Temp.Size = new System.Drawing.Size(130, 25);
            this.Temp.TabIndex = 5;
            this.Temp.Text = "";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(34, 96);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "实际温度：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(34, 40);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "当前温度：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.RealMea);
            this.groupBox2.Controls.Add(this.Mea);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(6, 19);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(283, 162);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "CM";
            // 
            // RealMea
            // 
            this.RealMea.Location = new System.Drawing.Point(100, 93);
            this.RealMea.Name = "RealMea";
            this.RealMea.Size = new System.Drawing.Size(130, 25);
            this.RealMea.TabIndex = 4;
            this.RealMea.Text = "";
            // 
            // Mea
            // 
            this.Mea.Location = new System.Drawing.Point(100, 37);
            this.Mea.Name = "Mea";
            this.Mea.ReadOnly = true;
            this.Mea.Size = new System.Drawing.Size(130, 25);
            this.Mea.TabIndex = 3;
            this.Mea.Text = "";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(27, 96);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "实际测数：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "当前测数：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 13);
            this.label3.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.button6);
            this.groupBox1.Controls.Add(this.start);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.Ports);
            this.groupBox1.Controls.Add(this.Content);
            this.groupBox1.Controls.Add(this.SignalContent);
            this.groupBox1.Controls.Add(this.Time);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.Length);
            this.groupBox1.Controls.Add(this.SignalLength);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(745, 184);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "当前串口信息：";
            // 
            // start
            // 
            this.start.Location = new System.Drawing.Point(458, 34);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(75, 23);
            this.start.TabIndex = 14;
            this.start.Text = "打开串口";
            this.start.UseVisualStyleBackColor = true;
            this.start.Click += new System.EventHandler(this.start_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(270, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "串口号：";
            // 
            // Ports
            // 
            this.Ports.FormattingEnabled = true;
            this.Ports.Location = new System.Drawing.Point(331, 36);
            this.Ports.Name = "Ports";
            this.Ports.Size = new System.Drawing.Size(121, 21);
            this.Ports.TabIndex = 12;
            // 
            // Content
            // 
            this.Content.AutoSize = true;
            this.Content.Location = new System.Drawing.Point(0, 118);
            this.Content.Name = "Content";
            this.Content.Size = new System.Drawing.Size(91, 13);
            this.Content.TabIndex = 11;
            this.Content.Text = "十六进制内容：";
            // 
            // SignalContent
            // 
            this.SignalContent.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SignalContent.Location = new System.Drawing.Point(92, 104);
            this.SignalContent.Name = "SignalContent";
            this.SignalContent.ReadOnly = true;
            this.SignalContent.Size = new System.Drawing.Size(643, 44);
            this.SignalContent.TabIndex = 10;
            this.SignalContent.Text = "";
            // 
            // Time
            // 
            this.Time.Location = new System.Drawing.Point(92, 64);
            this.Time.Multiline = false;
            this.Time.Name = "Time";
            this.Time.ReadOnly = true;
            this.Time.Size = new System.Drawing.Size(130, 22);
            this.Time.TabIndex = 9;
            this.Time.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "接收时间：";
            // 
            // Length
            // 
            this.Length.Location = new System.Drawing.Point(92, 36);
            this.Length.Multiline = false;
            this.Length.Name = "Length";
            this.Length.ReadOnly = true;
            this.Length.Size = new System.Drawing.Size(130, 22);
            this.Length.TabIndex = 7;
            this.Length.Text = "";
            // 
            // SignalLength
            // 
            this.SignalLength.AutoSize = true;
            this.SignalLength.Location = new System.Drawing.Point(33, 39);
            this.SignalLength.Name = "SignalLength";
            this.SignalLength.Size = new System.Drawing.Size(67, 13);
            this.SignalLength.TabIndex = 6;
            this.SignalLength.Text = "信号长度：";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.button5);
            this.tabPage2.Controls.Add(this.button4);
            this.tabPage2.Controls.Add(this.button1);
            this.tabPage2.Controls.Add(this.button2);
            this.tabPage2.Controls.Add(this.clear);
            this.tabPage2.Controls.Add(this.Screen);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(768, 409);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "数据信息";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(20, 264);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 32);
            this.button5.TabIndex = 6;
            this.button5.Text = "回归参数";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(20, 209);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 35);
            this.button4.TabIndex = 5;
            this.button4.Text = "查看测数曲线";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(20, 61);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 32);
            this.button1.TabIndex = 3;
            this.button1.Text = "刷新";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(20, 112);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 30);
            this.button2.TabIndex = 2;
            this.button2.Text = "生成Excel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // clear
            // 
            this.clear.Location = new System.Drawing.Point(20, 158);
            this.clear.Name = "clear";
            this.clear.Size = new System.Drawing.Size(75, 31);
            this.clear.TabIndex = 1;
            this.clear.Text = "清空";
            this.clear.UseVisualStyleBackColor = true;
            this.clear.Click += new System.EventHandler(this.clear_Click);
            // 
            // Screen
            // 
            this.Screen.Location = new System.Drawing.Point(101, 32);
            this.Screen.Name = "Screen";
            this.Screen.Size = new System.Drawing.Size(617, 331);
            this.Screen.TabIndex = 0;
            this.Screen.UseCompatibleStateImageBehavior = false;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(539, 34);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 23);
            this.button6.TabIndex = 15;
            this.button6.Text = "采集指令";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(654, 39);
            this.label8.MaximumSize = new System.Drawing.Size(13, 13);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(13, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "██ ";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControl1);
            this.DoubleBuffered = true;
            this.Name = "Form1";
            this.Text = "串口接收工具";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.datagroup.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox Ports;
        private System.Windows.Forms.Label Content;
        private System.Windows.Forms.RichTextBox SignalContent;
        private System.Windows.Forms.RichTextBox Time;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox Length;
        private System.Windows.Forms.Label SignalLength;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button start;
        private System.Windows.Forms.GroupBox datagroup;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.RichTextBox Mea;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RichTextBox RealTemp;
        private System.Windows.Forms.RichTextBox Temp;
        private System.Windows.Forms.RichTextBox RealMea;
        private System.Windows.Forms.Button Save;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button clear;
        private System.Windows.Forms.ListView Screen;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Label label8;
    }
}

