namespace Автомастерская
{
    partial class авторизация
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.labelcaptcha = new System.Windows.Forms.Label();
            this.textBoxcaptcha = new System.Windows.Forms.TextBox();
            this.loginTimer = new System.Windows.Forms.Timer(this.components);
            this.pictureBoxreplay = new System.Windows.Forms.PictureBox();
            this.pictureBoxcaptcha = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxreplay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxcaptcha)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.RoyalBlue;
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Location = new System.Drawing.Point(85, 52);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(369, 311);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.White;
            this.groupBox2.Controls.Add(this.checkBox1);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.textBox2);
            this.groupBox2.Controls.Add(this.textBox1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox2.Location = new System.Drawing.Point(7, 7);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(355, 296);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(280, 182);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(15, 14);
            this.checkBox1.TabIndex = 5;
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.RoyalBlue;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button1.Location = new System.Drawing.Point(100, 226);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(157, 41);
            this.button1.TabIndex = 4;
            this.button1.Text = "ВОЙТИ";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.White;
            this.textBox2.Location = new System.Drawing.Point(47, 174);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(256, 29);
            this.textBox2.TabIndex = 3;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.White;
            this.textBox1.Location = new System.Drawing.Point(47, 93);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(256, 29);
            this.textBox1.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label2.Location = new System.Drawing.Point(43, 138);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 21);
            this.label2.TabIndex = 1;
            this.label2.Text = "ПАРОЛЬ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label1.Location = new System.Drawing.Point(43, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "ЛОГИН";
            // 
            // labelcaptcha
            // 
            this.labelcaptcha.AutoSize = true;
            this.labelcaptcha.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelcaptcha.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.labelcaptcha.Location = new System.Drawing.Point(22, 390);
            this.labelcaptcha.Name = "labelcaptcha";
            this.labelcaptcha.Size = new System.Drawing.Size(178, 21);
            this.labelcaptcha.TabIndex = 6;
            this.labelcaptcha.Text = "ВВЕДИТЕ КАПТЧУ:";
            // 
            // textBoxcaptcha
            // 
            this.textBoxcaptcha.BackColor = System.Drawing.Color.White;
            this.textBoxcaptcha.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxcaptcha.Location = new System.Drawing.Point(206, 387);
            this.textBoxcaptcha.Name = "textBoxcaptcha";
            this.textBoxcaptcha.Size = new System.Drawing.Size(115, 29);
            this.textBoxcaptcha.TabIndex = 6;
            // 
            // loginTimer
            // 
            this.loginTimer.Interval = 1000;
            // 
            // pictureBoxreplay
            // 
            this.pictureBoxreplay.Image = global::Автомастерская.Properties.Resources.replay;
            this.pictureBoxreplay.Location = new System.Drawing.Point(460, 387);
            this.pictureBoxreplay.Name = "pictureBoxreplay";
            this.pictureBoxreplay.Size = new System.Drawing.Size(31, 29);
            this.pictureBoxreplay.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxreplay.TabIndex = 8;
            this.pictureBoxreplay.TabStop = false;
            this.pictureBoxreplay.Click += new System.EventHandler(this.pictureBoxreplay_Click);
            // 
            // pictureBoxcaptcha
            // 
            this.pictureBoxcaptcha.Location = new System.Drawing.Point(334, 387);
            this.pictureBoxcaptcha.Name = "pictureBoxcaptcha";
            this.pictureBoxcaptcha.Size = new System.Drawing.Size(120, 29);
            this.pictureBoxcaptcha.TabIndex = 7;
            this.pictureBoxcaptcha.TabStop = false;
            // 
            // авторизация
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(538, 450);
            this.Controls.Add(this.pictureBoxreplay);
            this.Controls.Add(this.pictureBoxcaptcha);
            this.Controls.Add(this.textBoxcaptcha);
            this.Controls.Add(this.labelcaptcha);
            this.Controls.Add(this.groupBox1);
            this.Name = "авторизация";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "авторизация";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.авторизация_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxreplay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxcaptcha)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label labelcaptcha;
        private System.Windows.Forms.TextBox textBoxcaptcha;
        private System.Windows.Forms.PictureBox pictureBoxcaptcha;
        private System.Windows.Forms.PictureBox pictureBoxreplay;
        private System.Windows.Forms.Timer loginTimer;
    }
}

