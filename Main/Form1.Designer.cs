namespace Main
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.level_max_button = new System.Windows.Forms.Button();
            this.level25_button = new System.Windows.Forms.Button();
            this.level5_button = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.msg_lable = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.yi_su_max_button = new System.Windows.Forms.Button();
            this.min_jie_max_button = new System.Windows.Forms.Button();
            this.li_liang_max_button = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // level_max_button
            // 
            this.level_max_button.Location = new System.Drawing.Point(329, 57);
            this.level_max_button.Name = "level_max_button";
            this.level_max_button.Size = new System.Drawing.Size(117, 44);
            this.level_max_button.TabIndex = 2;
            this.level_max_button.Text = "Max";
            this.level_max_button.UseVisualStyleBackColor = true;
            this.level_max_button.Click += new System.EventHandler(this.level_max_button_Click);
            // 
            // level25_button
            // 
            this.level25_button.Location = new System.Drawing.Point(183, 57);
            this.level25_button.Name = "level25_button";
            this.level25_button.Size = new System.Drawing.Size(117, 44);
            this.level25_button.TabIndex = 3;
            this.level25_button.Text = "25级";
            this.level25_button.UseVisualStyleBackColor = true;
            this.level25_button.Click += new System.EventHandler(this.level25_button_Click);
            // 
            // level5_button
            // 
            this.level5_button.Location = new System.Drawing.Point(38, 57);
            this.level5_button.Name = "level5_button";
            this.level5_button.Size = new System.Drawing.Size(117, 44);
            this.level5_button.TabIndex = 1;
            this.level5_button.Text = "5级";
            this.level5_button.UseVisualStyleBackColor = true;
            this.level5_button.Click += new System.EventHandler(this.level5_button_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.level_max_button);
            this.groupBox1.Controls.Add(this.level5_button);
            this.groupBox1.Controls.Add(this.level25_button);
            this.groupBox1.Location = new System.Drawing.Point(24, 35);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(495, 134);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "修改经验";
            // 
            // msg_lable
            // 
            this.msg_lable.AutoSize = true;
            this.msg_lable.Location = new System.Drawing.Point(237, 9);
            this.msg_lable.Name = "msg_lable";
            this.msg_lable.Size = new System.Drawing.Size(53, 12);
            this.msg_lable.TabIndex = 4;
            this.msg_lable.Text = "信息展示";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.yi_su_max_button);
            this.groupBox2.Controls.Add(this.min_jie_max_button);
            this.groupBox2.Controls.Add(this.li_liang_max_button);
            this.groupBox2.Location = new System.Drawing.Point(24, 238);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(497, 106);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "修改属性";
            // 
            // yi_su_max_button
            // 
            this.yi_su_max_button.Location = new System.Drawing.Point(329, 39);
            this.yi_su_max_button.Name = "yi_su_max_button";
            this.yi_su_max_button.Size = new System.Drawing.Size(117, 44);
            this.yi_su_max_button.TabIndex = 6;
            this.yi_su_max_button.Text = "移速Max";
            this.yi_su_max_button.UseVisualStyleBackColor = true;
            this.yi_su_max_button.Click += new System.EventHandler(this.yi_su_max_button_Click);
            // 
            // min_jie_max_button
            // 
            this.min_jie_max_button.Location = new System.Drawing.Point(183, 39);
            this.min_jie_max_button.Name = "min_jie_max_button";
            this.min_jie_max_button.Size = new System.Drawing.Size(117, 44);
            this.min_jie_max_button.TabIndex = 1;
            this.min_jie_max_button.Text = "敏捷Max";
            this.min_jie_max_button.UseVisualStyleBackColor = true;
            this.min_jie_max_button.Click += new System.EventHandler(this.min_jie_max_button_Click);
            // 
            // li_liang_max_button
            // 
            this.li_liang_max_button.Location = new System.Drawing.Point(38, 39);
            this.li_liang_max_button.Name = "li_liang_max_button";
            this.li_liang_max_button.Size = new System.Drawing.Size(117, 44);
            this.li_liang_max_button.TabIndex = 0;
            this.li_liang_max_button.Text = "力量Max";
            this.li_liang_max_button.UseVisualStyleBackColor = true;
            this.li_liang_max_button.Click += new System.EventHandler(this.li_liang_max_button_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 366);
            this.Controls.Add(this.msg_lable);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "七十二变修改器";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button level_max_button;
        private System.Windows.Forms.Button level25_button;
        private System.Windows.Forms.Button level5_button;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button yi_su_max_button;
        private System.Windows.Forms.Button min_jie_max_button;
        private System.Windows.Forms.Button li_liang_max_button;
        private System.Windows.Forms.Label msg_lable;
    }
}

