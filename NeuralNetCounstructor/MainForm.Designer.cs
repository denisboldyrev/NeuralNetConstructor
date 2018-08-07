namespace NeuralNetCounstructor
{
    partial class MainForm
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
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbNumLayers = new System.Windows.Forms.TextBox();
            this.cbSelectLayer = new System.Windows.Forms.ComboBox();
            this.btnSetNetType = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tbRightB = new System.Windows.Forms.TextBox();
            this.tbLeftA = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.tbSigmA = new System.Windows.Forms.TextBox();
            this.tbTanB = new System.Windows.Forms.TextBox();
            this.tbTanA = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.cbActFuncSelect = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnSetLayerType = new System.Windows.Forms.Button();
            this.tbNumNeurons = new System.Windows.Forms.TextBox();
            this.cbSelectNetType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tbNumIn = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tbAccuracy = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tbNumEpochs = new System.Windows.Forms.TextBox();
            this.tbLearnRate = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnCreateNet = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.создатьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.открытьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитьToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.выходToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.справкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.помощьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.оПрограммеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.cbTypeTask = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 16);
            this.label2.TabIndex = 8;
            this.label2.Text = "Слои";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 152);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 16);
            this.label1.TabIndex = 7;
            this.label1.Text = "Число слоев";
            // 
            // tbNumLayers
            // 
            this.tbNumLayers.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbNumLayers.Location = new System.Drawing.Point(115, 153);
            this.tbNumLayers.Name = "tbNumLayers";
            this.tbNumLayers.Size = new System.Drawing.Size(125, 22);
            this.tbNumLayers.TabIndex = 6;
            this.tbNumLayers.Text = "0";
            // 
            // cbSelectLayer
            // 
            this.cbSelectLayer.FormattingEnabled = true;
            this.cbSelectLayer.Location = new System.Drawing.Point(20, 38);
            this.cbSelectLayer.Name = "cbSelectLayer";
            this.cbSelectLayer.Size = new System.Drawing.Size(185, 24);
            this.cbSelectLayer.TabIndex = 5;
            this.cbSelectLayer.SelectedIndexChanged += new System.EventHandler(this.cbSelectLayer_SelectedIndexChanged);
            // 
            // btnSetNetType
            // 
            this.btnSetNetType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnSetNetType.Location = new System.Drawing.Point(135, 209);
            this.btnSetNetType.Name = "btnSetNetType";
            this.btnSetNetType.Size = new System.Drawing.Size(105, 36);
            this.btnSetNetType.TabIndex = 9;
            this.btnSetNetType.Text = "Задать";
            this.btnSetNetType.UseVisualStyleBackColor = true;
            this.btnSetNetType.Click += new System.EventHandler(this.btnSetNetType_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 16);
            this.label3.TabIndex = 10;
            this.label3.Text = "Число нейронов";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.cbActFuncSelect);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.btnSetLayerType);
            this.groupBox1.Controls.Add(this.tbNumNeurons);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cbSelectLayer);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.Location = new System.Drawing.Point(12, 251);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(516, 276);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Параметры слоя";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.tbRightB);
            this.groupBox4.Controls.Add(this.tbLeftA);
            this.groupBox4.Location = new System.Drawing.Point(18, 96);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(200, 130);
            this.groupBox4.TabIndex = 17;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Диапазон изменения начальных значений весовых коэффициентов";
            // 
            // tbRightB
            // 
            this.tbRightB.Location = new System.Drawing.Point(95, 61);
            this.tbRightB.Name = "tbRightB";
            this.tbRightB.Size = new System.Drawing.Size(70, 22);
            this.tbRightB.TabIndex = 1;
            this.tbRightB.Text = "0.0";
            this.tbRightB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbTanA_KeyPress);
            // 
            // tbLeftA
            // 
            this.tbLeftA.Location = new System.Drawing.Point(19, 61);
            this.tbLeftA.Name = "tbLeftA";
            this.tbLeftA.Size = new System.Drawing.Size(70, 22);
            this.tbLeftA.TabIndex = 0;
            this.tbLeftA.Text = "0.0";
            this.tbLeftA.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbTanA_KeyPress);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.tbSigmA);
            this.groupBox3.Controls.Add(this.tbTanB);
            this.groupBox3.Controls.Add(this.tbTanA);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox3.Location = new System.Drawing.Point(224, 96);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(281, 130);
            this.groupBox3.TabIndex = 16;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Параметры функции активации";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(127, 90);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(16, 16);
            this.label13.TabIndex = 20;
            this.label13.Text = "b";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 90);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(16, 16);
            this.label12.TabIndex = 19;
            this.label12.Text = "a";
            // 
            // tbSigmA
            // 
            this.tbSigmA.Location = new System.Drawing.Point(149, 28);
            this.tbSigmA.Name = "tbSigmA";
            this.tbSigmA.Size = new System.Drawing.Size(70, 22);
            this.tbSigmA.TabIndex = 0;
            this.tbSigmA.Text = "0.0";
            this.tbSigmA.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbTanA_KeyPress);
            // 
            // tbTanB
            // 
            this.tbTanB.Location = new System.Drawing.Point(149, 87);
            this.tbTanB.Name = "tbTanB";
            this.tbTanB.Size = new System.Drawing.Size(70, 22);
            this.tbTanB.TabIndex = 18;
            this.tbTanB.Text = "0.0";
            this.tbTanB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbTanA_KeyPress);
            // 
            // tbTanA
            // 
            this.tbTanA.Location = new System.Drawing.Point(28, 87);
            this.tbTanA.Name = "tbTanA";
            this.tbTanA.Size = new System.Drawing.Size(70, 22);
            this.tbTanA.TabIndex = 17;
            this.tbTanA.Text = "0.0";
            this.tbTanA.TextChanged += new System.EventHandler(this.tbTanA_TextChanged);
            this.tbTanA.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbTanA_KeyPress);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 68);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(271, 16);
            this.label11.TabIndex = 16;
            this.label11.Text = "Параметры гиперболического тангенса";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 31);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(137, 16);
            this.label10.TabIndex = 15;
            this.label10.Text = "Крутизна сигмоиды";
            // 
            // cbActFuncSelect
            // 
            this.cbActFuncSelect.FormattingEnabled = true;
            this.cbActFuncSelect.Items.AddRange(new object[] {
            "Сигмоидальная",
            "Гиперболический тангенс",
            "Линейная"});
            this.cbActFuncSelect.Location = new System.Drawing.Point(224, 41);
            this.cbActFuncSelect.Name = "cbActFuncSelect";
            this.cbActFuncSelect.Size = new System.Drawing.Size(219, 24);
            this.cbActFuncSelect.TabIndex = 14;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(221, 22);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(138, 16);
            this.label9.TabIndex = 13;
            this.label9.Text = "Функция активации";
            // 
            // btnSetLayerType
            // 
            this.btnSetLayerType.Location = new System.Drawing.Point(400, 232);
            this.btnSetLayerType.Name = "btnSetLayerType";
            this.btnSetLayerType.Size = new System.Drawing.Size(105, 35);
            this.btnSetLayerType.TabIndex = 12;
            this.btnSetLayerType.Text = "Применить";
            this.btnSetLayerType.UseVisualStyleBackColor = true;
            this.btnSetLayerType.Click += new System.EventHandler(this.btnSetLayerType_Click);
            // 
            // tbNumNeurons
            // 
            this.tbNumNeurons.Location = new System.Drawing.Point(138, 68);
            this.tbNumNeurons.Name = "tbNumNeurons";
            this.tbNumNeurons.Size = new System.Drawing.Size(67, 22);
            this.tbNumNeurons.TabIndex = 11;
            // 
            // cbSelectNetType
            // 
            this.cbSelectNetType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbSelectNetType.FormattingEnabled = true;
            this.cbSelectNetType.Items.AddRange(new object[] {
            "Линейный нейрон",
            "Персептрон",
            "Многослойный персептрон"});
            this.cbSelectNetType.Location = new System.Drawing.Point(12, 71);
            this.cbSelectNetType.Name = "cbSelectNetType";
            this.cbSelectNetType.Size = new System.Drawing.Size(228, 24);
            this.cbSelectNetType.TabIndex = 12;
            this.cbSelectNetType.SelectedIndexChanged += new System.EventHandler(this.cbSelectNetType_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(9, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(141, 16);
            this.label4.TabIndex = 13;
            this.label4.Text = "Тип нейронной сети";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(12, 181);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 16);
            this.label5.TabIndex = 14;
            this.label5.Text = "Число входов";
            // 
            // tbNumIn
            // 
            this.tbNumIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbNumIn.Location = new System.Drawing.Point(115, 181);
            this.tbNumIn.Name = "tbNumIn";
            this.tbNumIn.Size = new System.Drawing.Size(125, 22);
            this.tbNumIn.TabIndex = 15;
            this.tbNumIn.Text = "0";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tbAccuracy);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.tbNumEpochs);
            this.groupBox2.Controls.Add(this.tbLearnRate);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox2.Location = new System.Drawing.Point(246, 52);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(282, 193);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Параметры обучения";
            // 
            // tbAccuracy
            // 
            this.tbAccuracy.Location = new System.Drawing.Point(166, 81);
            this.tbAccuracy.Name = "tbAccuracy";
            this.tbAccuracy.Size = new System.Drawing.Size(95, 22);
            this.tbAccuracy.TabIndex = 5;
            this.tbAccuracy.Text = "0.0";
            this.tbAccuracy.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbTanA_KeyPress);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(23, 84);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(136, 16);
            this.label8.TabIndex = 4;
            this.label8.Text = "Точность обучения";
            // 
            // tbNumEpochs
            // 
            this.tbNumEpochs.Location = new System.Drawing.Point(165, 53);
            this.tbNumEpochs.Name = "tbNumEpochs";
            this.tbNumEpochs.Size = new System.Drawing.Size(96, 22);
            this.tbNumEpochs.TabIndex = 3;
            this.tbNumEpochs.Text = "0";
            // 
            // tbLearnRate
            // 
            this.tbLearnRate.Location = new System.Drawing.Point(165, 24);
            this.tbLearnRate.Name = "tbLearnRate";
            this.tbLearnRate.Size = new System.Drawing.Size(95, 22);
            this.tbLearnRate.TabIndex = 2;
            this.tbLearnRate.Text = "0.0";
            this.tbLearnRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbTanA_KeyPress);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(24, 56);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(85, 16);
            this.label7.TabIndex = 1;
            this.label7.Text = "Кол-во эпох";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(24, 27);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(135, 16);
            this.label6.TabIndex = 0;
            this.label6.Text = "Скорость обучения";
            // 
            // btnCreateNet
            // 
            this.btnCreateNet.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnCreateNet.Location = new System.Drawing.Point(315, 533);
            this.btnCreateNet.Name = "btnCreateNet";
            this.btnCreateNet.Size = new System.Drawing.Size(104, 35);
            this.btnCreateNet.TabIndex = 18;
            this.btnCreateNet.Text = "Создать";
            this.btnCreateNet.UseVisualStyleBackColor = true;
            this.btnCreateNet.Click += new System.EventHandler(this.создатьToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem,
            this.справкаToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(545, 24);
            this.menuStrip1.TabIndex = 20;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.создатьToolStripMenuItem,
            this.открытьToolStripMenuItem,
            this.сохранитьToolStripMenuItem1,
            this.выходToolStripMenuItem});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // создатьToolStripMenuItem
            // 
            this.создатьToolStripMenuItem.Name = "создатьToolStripMenuItem";
            this.создатьToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.создатьToolStripMenuItem.Text = "Создать";
            this.создатьToolStripMenuItem.Click += new System.EventHandler(this.создатьToolStripMenuItem_Click);
            // 
            // открытьToolStripMenuItem
            // 
            this.открытьToolStripMenuItem.Name = "открытьToolStripMenuItem";
            this.открытьToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.открытьToolStripMenuItem.Text = "Открыть";
            this.открытьToolStripMenuItem.Click += new System.EventHandler(this.открытьToolStripMenuItem_Click);
            // 
            // сохранитьToolStripMenuItem1
            // 
            this.сохранитьToolStripMenuItem1.Name = "сохранитьToolStripMenuItem1";
            this.сохранитьToolStripMenuItem1.Size = new System.Drawing.Size(145, 22);
            this.сохранитьToolStripMenuItem1.Text = "Сохранить";
            this.сохранитьToolStripMenuItem1.Click += new System.EventHandler(this.сохранитьToolStripMenuItem1_Click);
            // 
            // выходToolStripMenuItem
            // 
            this.выходToolStripMenuItem.Name = "выходToolStripMenuItem";
            this.выходToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.выходToolStripMenuItem.Text = "Выход";
            this.выходToolStripMenuItem.Click += new System.EventHandler(this.выходToolStripMenuItem_Click);
            // 
            // справкаToolStripMenuItem
            // 
            this.справкаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.помощьToolStripMenuItem,
            this.оПрограммеToolStripMenuItem});
            this.справкаToolStripMenuItem.Name = "справкаToolStripMenuItem";
            this.справкаToolStripMenuItem.Size = new System.Drawing.Size(76, 20);
            this.справкаToolStripMenuItem.Text = "Справка";
            // 
            // помощьToolStripMenuItem
            // 
            this.помощьToolStripMenuItem.Name = "помощьToolStripMenuItem";
            this.помощьToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.помощьToolStripMenuItem.Text = "Помощь";
            this.помощьToolStripMenuItem.Click += new System.EventHandler(this.помощьToolStripMenuItem_Click);
            // 
            // оПрограммеToolStripMenuItem
            // 
            this.оПрограммеToolStripMenuItem.Name = "оПрограммеToolStripMenuItem";
            this.оПрограммеToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.оПрограммеToolStripMenuItem.Text = "О программе...";
            this.оПрограммеToolStripMenuItem.Click += new System.EventHandler(this.оПрограммеToolStripMenuItem_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(425, 533);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(104, 35);
            this.button1.TabIndex = 21;
            this.button1.Text = "Выход";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.выходToolStripMenuItem_Click);
            // 
            // cbTypeTask
            // 
            this.cbTypeTask.Enabled = false;
            this.cbTypeTask.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.cbTypeTask.FormattingEnabled = true;
            this.cbTypeTask.Items.AddRange(new object[] {
            "Прогнозирование",
            "Классификация"});
            this.cbTypeTask.Location = new System.Drawing.Point(12, 117);
            this.cbTypeTask.Name = "cbTypeTask";
            this.cbTypeTask.Size = new System.Drawing.Size(228, 24);
            this.cbTypeTask.TabIndex = 23;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.label14.Location = new System.Drawing.Point(9, 98);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(84, 16);
            this.label14.TabIndex = 24;
            this.label14.Text = "Тип задачи";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(545, 578);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.cbTypeTask);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnCreateNet);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.tbNumIn);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbSelectNetType);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnSetNetType);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbNumLayers);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Конструктор нейронных сетей";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbNumLayers;
        private System.Windows.Forms.ComboBox cbSelectLayer;
        private System.Windows.Forms.Button btnSetNetType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSetLayerType;
        private System.Windows.Forms.TextBox tbNumNeurons;
        private System.Windows.Forms.ComboBox cbSelectNetType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbNumIn;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbAccuracy;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbNumEpochs;
        private System.Windows.Forms.TextBox tbLearnRate;
        private System.Windows.Forms.Button btnCreateNet;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem справкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem создатьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem открытьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранитьToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem выходToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem помощьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem оПрограммеToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox cbActFuncSelect;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tbSigmA;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox tbRightB;
        private System.Windows.Forms.TextBox tbLeftA;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox tbTanB;
        private System.Windows.Forms.TextBox tbTanA;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cbTypeTask;
        private System.Windows.Forms.Label label14;
    }
}