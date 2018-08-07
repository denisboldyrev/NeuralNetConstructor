using Neuron;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NeuralNetCounstructor
{
    public partial class ClassificationForm : Form
    {
        AppSettings NeuralNetSettings = AppSettings.GetInstance();
        public ClassificationForm(AppSettings NeuralNetSettings)
        {
            this.NeuralNetSettings = NeuralNetSettings;
            InitializeComponent();
        }
        private void загрузитьОбучающийНаборToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.InitialDirectory = Application.StartupPath;
            openFile.Filter = "txt files (*.txt) | *.txt";
            openFile.FilterIndex = 2;
            openFile.RestoreDirectory = true;
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    double[] dout;
                    dataGridView1.ColumnCount = NeuralNetSettings.NumIn;
                    dataGridView1.RowCount = NeuralNetSettings.Epoch;
                    string strs;
                    using (StreamReader str = new StreamReader(openFile.FileName))
                    {
                        for (int i = 0; i < NeuralNetSettings.Epoch; i++)
                        {
                            strs = str.ReadLine();
                            if (CultureInfo.CurrentUICulture.Name == "en-US")
                                dout = Array.ConvertAll(strs.Replace(',', '.').Split('\t', ' '), Double.Parse); // конвертируем строку в массив double
                            else
                                dout = Array.ConvertAll(strs.Replace('.', ',').Split('\t', ' '), Double.Parse); // конвертируем строку в массив double
                            for (int j = 0; j < dout.Length; j++)
                            {
                                Application.DoEvents();
                                if (dataGridView1.InvokeRequired)
                                {
                                    dataGridView1.Invoke((MethodInvoker)delegate
                                    {
                                        dataGridView1[j, i].Value = dout[j];
                                        dataGridView1.Columns[j].Width = 60;
                                    });
                                }
                                else
                                {
                                    dataGridView1[j, i].Value = dout[j];
                                    dataGridView1.Columns[j].Width = 60;
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Ошибка при открытии файла!");
                }
           }
        }
        private void загрузитьНаборЭталоновToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView2.Columns.Clear();
            dataGridView2.Rows.Clear();
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.InitialDirectory = Application.StartupPath;
            openFile.Filter = "txt files (*.txt) | *.txt";
            openFile.FilterIndex = 2;
            openFile.RestoreDirectory = true;
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string[] etalon = File.ReadAllLines(openFile.FileName);
                    double[] dout;
                    dataGridView2.ColumnCount = NeuralNetSettings.NumNeurons[NeuralNetSettings.NumLayers-1];
                    dataGridView2.RowCount = etalon.Length;
                    for (int i = 0; i < etalon.Length; i++)
                    {
                        if (CultureInfo.CurrentUICulture.Name == "en-US")
                            dout = Array.ConvertAll(etalon[i].Replace(',', '.').Split('\t', ' '), Double.Parse); // конвертируем строку в массив double
                        else
                            dout = Array.ConvertAll(etalon[i].Replace('.', ',').Split('\t', ' '), Double.Parse); // конвертируем строку в массив double
                        for (int j = 0; j < dout.Length; j++)
                        {
                            Application.DoEvents();
                            if (dataGridView2.InvokeRequired)
                            {
                                dataGridView2.Invoke((MethodInvoker)delegate
                                {
                                    dataGridView2[j, i].Value = dout[j];
                                    dataGridView2.Columns[j].Width = 60;
                                });
                            }
                            else
                            {
                                dataGridView2[j, i].Value = dout[j];
                                dataGridView2.Columns[j].Width = 60;
                            }
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Ошибка при открытии файла!");
                }
            }
        }
        Layer[] layers;
        Task tsk;
        CancellationTokenSource source = new CancellationTokenSource();
        CancellationToken cancelToken;
        DateTime date;
        bool flag = true;
        private void TickTimer(object sender, EventArgs e)
        {
            long tick = DateTime.Now.Ticks - date.Ticks;
            DateTime stopWatch = new DateTime();
            stopWatch = stopWatch.AddTicks(tick);
            toolStripStatusLabel1.Text = String.Format("{0:HH:mm:ss:ff}", stopWatch);
        }
        private void btnLearn_Click(object sender, EventArgs e)
        {
            date = DateTime.Now;
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 10;
            chart2.Series[0].Points.Clear();
            double[] y = new double[1];
            double[] x = new double[dataGridView1.ColumnCount];
            layers = new Layer[NeuralNetSettings.NumLayers];
            // Выход первого слоя необходим для того, чтобы подать его на вход второго слоя и т.д.
            layers[0] = new Layer(NeuralNetSettings.NumIn, NeuralNetSettings.NumNeurons[0]); // создаем первый входной слой. Подаем ему на вход вектор входов и число нейронов
            for (int i = 1; i < NeuralNetSettings.NumLayers; i++)
            {
                layers[i] = new Layer(NeuralNetSettings.NumNeurons[i - 1], NeuralNetSettings.NumNeurons[i]);
            }
            for (int i = 0; i < NeuralNetSettings.NumLayers; i++)
            {
                layers[i].W = NeuralNetSettings.W[i];
                layers[i].Bias = NeuralNetSettings.Bias[i];
            }
            NeuralNetSettings.X = new double[NeuralNetSettings.Epoch][];
            // загрузить данные из таблицы в массив
            // ****************************************************************************************
            for (int i = 0; i < NeuralNetSettings.Epoch; i++)
            {
                NeuralNetSettings.X[i] = new double[NeuralNetSettings.NumIn];
            }
            for (int i = 0; i < NeuralNetSettings.Epoch; i++)
            {
                for (int j = 0; j < NeuralNetSettings.NumIn; j++)
                {
                    this.Invoke((Action)delegate
                    {
                        NeuralNetSettings.X[i][j] = double.Parse(dataGridView1[j, i].Value.ToString());
                    });
                }
            }
            NeuralNetSettings.Y = new double[NeuralNetSettings.Epoch][];
            for (int i = 0; i < NeuralNetSettings.Epoch; i++)
            {
                NeuralNetSettings.Y[i] = new double[dataGridView2.ColumnCount];
            }
            for (int i = 0; i < NeuralNetSettings.Epoch; i++)
            {
                for (int j = 0; j < dataGridView2.ColumnCount; j++)
                {
                    this.Invoke((Action)delegate
                    {
                        NeuralNetSettings.Y[i][j] = double.Parse(dataGridView2[j, i].Value.ToString());
                    });
                }
            }
            // ******************************************************************************************
            // вычислить необходимые данные для мастабирования
            // **********************************************************************
            double temp;
            // *******************************************************************
            // масштабирование
            // *******************************************************************
            if (cbInConvert.Checked) // если выбрано масштабирование вх данных
            {
                NeuralNetSettings.AvgX = new double[NeuralNetSettings.NumIn];
                NeuralNetSettings.Sigma = new double[NeuralNetSettings.NumIn];
                for (int i = 0; i < NeuralNetSettings.NumIn; i++)
                {
                    temp = 0;
                    for (int j = 0; j < NeuralNetSettings.Epoch; j++)
                    {
                        temp += NeuralNetSettings.X[j][i];
                    }
                    NeuralNetSettings.AvgX[i] = temp / NeuralNetSettings.Epoch;
                    temp = 0;
                    for (int j = 0; j < NeuralNetSettings.Epoch; j++)
                    {
                        temp += Math.Pow((NeuralNetSettings.X[j][i] - NeuralNetSettings.AvgX[i]), 2);
                    }
                    NeuralNetSettings.Sigma[i] = Math.Sqrt(temp / NeuralNetSettings.Epoch);
                }
                for (int i = 0; i < NeuralNetSettings.Epoch; i++)
                {
                    for (int j = 0; j < NeuralNetSettings.NumIn; j++)
                    {
                        NeuralNetSettings.X[i][j] = 1 / (1 + Math.Exp(-((NeuralNetSettings.X[i][j] - NeuralNetSettings.AvgX[j]) / ((2 * NeuralNetSettings.Sigma[j]) / (2 * Math.PI)))));
                    }
                }
            }
            // **********************************************************************************
            // Масштабирование эталонов
            // **********************************************************************************
            if (cbInConvert.Checked)
            {
                temp = 0.0;
                NeuralNetSettings.AvgY = new double[dataGridView2.ColumnCount];
                NeuralNetSettings.SigmaY = new double[dataGridView2.ColumnCount];
                for (int i = 0; i < dataGridView2.ColumnCount; i++)
                {
                    temp = 0;
                    for (int j = 0; j < NeuralNetSettings.Epoch; j++)
                    {
                        temp += NeuralNetSettings.Y[j][i];
                    }
                    NeuralNetSettings.AvgY[i] = temp / NeuralNetSettings.Epoch;
                    temp = 0;
                    for (int j = 0; j < NeuralNetSettings.Epoch; j++)
                    {
                        temp += Math.Pow((NeuralNetSettings.Y[j][i] - NeuralNetSettings.AvgY[i]), 2);
                    }
                    NeuralNetSettings.SigmaY[i] = Math.Sqrt(temp / NeuralNetSettings.Epoch);
                }
                for (int i = 0; i < NeuralNetSettings.Epoch; i++)
                {
                    for (int j = 0; j < dataGridView2.ColumnCount; j++)
                    {
                        NeuralNetSettings.Y[i][j] = 1 / (1 + Math.Exp(-((NeuralNetSettings.Y[i][j] - NeuralNetSettings.AvgY[j]) / ((2 * NeuralNetSettings.SigmaY[j]) / (2 * Math.PI)))));
                    }
                }
            }
            // ***********************************************************************************
            int count = 0;
            double sumError = 0;
            double[] output = new double[NeuralNetSettings.NumNeurons[NeuralNetSettings.NumLayers-1]];
            timer.Start();
            try
            {
                tsk = new Task(() =>
                {
                    Parallel.Invoke(() =>
                    {

                     //   Application.DoEvents();
                        double checkerr = 2;
                        //for (; checkerr >= NeuralNetSettings.Accuracy;)
                        //{
                        for (; flag == true;)
                        {
                            timer.Tick += new EventHandler(TickTimer);
                            sumError = 0;
                            NeuralNetSettings.MSE = 0;
                            for (int k = 0; k < NeuralNetSettings.Epoch; k++)
                            {
                                if (cancelToken.IsCancellationRequested)
                                {
                                    timer.Stop();
                                    MessageBox.Show("Обучение прервано!\nСреднеквадратическая ошибка: " + NeuralNetSettings.AvgMSE
                                        + "\nТекущая ошибка:" + checkerr);
                                    //   source.Dispose();
                                    return;
                                }
                                layers[0].X = NeuralNetSettings.X[k];
                                // *********************************************************************************************************
                                if (NeuralNetSettings.ActFuncType[0] == "Сигмоидальная")
                                    layers[0].GetLayerOut(NeuralNetSettings.SigmoidA[0]);
                                else
                                    layers[0].GetTangLayerOut(NeuralNetSettings.TanA[0], NeuralNetSettings.TanB[0]);
                                for (int i = 0; i < NeuralNetSettings.NumLayers - 1; i++)
                                {
                                    layers[i + 1].X = layers[i].Output;
                                    if (NeuralNetSettings.ActFuncType[i + 1] == "Сигмоидальная")
                                        layers[i + 1].GetLayerOut(NeuralNetSettings.SigmoidA[i + 1]);
                                    else
                                        layers[i + 1].GetTangLayerOut(NeuralNetSettings.TanA[i + 1], NeuralNetSettings.TanB[i + 1]);
                                }
                                if (NeuralNetSettings.ActFuncType[NeuralNetSettings.NumLayers - 1] == "Сигмоидальная")
                                    layers[NeuralNetSettings.NumLayers - 1].CalcWeightsOutLayer(
                                        NeuralNetSettings.LearningRate, NeuralNetSettings.Y[k],
                                        NeuralNetSettings.SigmoidA[NeuralNetSettings.NumLayers - 1]); // вычислить ошибку выходного слоя
                                else
                                    layers[NeuralNetSettings.NumLayers - 1].CalcWeightsTangOutLayer(
                                        NeuralNetSettings.LearningRate, NeuralNetSettings.Y[k],
                                        NeuralNetSettings.TanA[NeuralNetSettings.NumLayers - 1],
                                        NeuralNetSettings.TanB[NeuralNetSettings.NumLayers - 1]); // вычислить ошибку выходного слоя

                                for (int i = NeuralNetSettings.NumLayers - 2; i >= 0; i--)
                                {
                                    if (NeuralNetSettings.ActFuncType[i] == "Сигмоидальная")
                                        layers[i].CalcWeightsHiddenLayer(NeuralNetSettings.LearningRate,
                                                                         layers[i + 1].LocalGrad,
                                                                         NeuralNetSettings.SigmoidA[i]);
                                    else
                                        layers[i].CalcWeightsHiddenTangLayer(NeuralNetSettings.LearningRate,
                                            layers[i + 1].LocalGrad, NeuralNetSettings.TanA[i],
                                            NeuralNetSettings.TanB[i]);
                                }
                               
                                // Обратное шкалирование
                                // **************************************************************
                                if (cbInConvert.Checked)
                                {
                                    for (int i = 0; i < layers[NeuralNetSettings.NumLayers - 1].Output.Length; i++)
                                    {
                                        output[i] = NeuralNetSettings.AvgY[i] + ((2 * NeuralNetSettings.SigmaY[i] * Math.Log(Math.Sqrt(-1 + (1 / (1 - layers[NeuralNetSettings.NumLayers - 1].Output[i])))) / Math.PI));
                                    }      
                                }
                                else
                                {
                                    for (int i = 0; i < output.Length; i++)
                                    {
                                        output[i] = layers[NeuralNetSettings.NumLayers - 1].Output[i];
                                    }                            
                                }
                                temp = 0;
                                for (int i = 0; i < output.Length; i++)
                                {
                                    temp += Math.Abs((NeuralNetSettings.Y[k][i] - output[i])/(i+1));
                                }
                                //NeuralNetSettings.MSE += 0.5 * temp * temp;
                                NeuralNetSettings.MSE = 0.5 * temp * temp;
                                //NeuralNetSettings.AvgMSE = NeuralNetSettings.MSE / (count + 1);
                                sumError += temp;
                                checkerr = sumError / (count + 1);
                                if (chart2.InvokeRequired)
                                {
                                    chart2.Invoke((MethodInvoker)delegate
                                    {
                                        //chart2.Series[0].Points.AddXY(count, NeuralNetSettings.AvgMSE);
                                        chart2.Series[0].Points.AddXY(count, NeuralNetSettings.MSE);
                                        if (chart2.ChartAreas[0].AxisX.Maximum < count)
                                        {
                                            chart2.ChartAreas[0].AxisX.Maximum = count;
                                        }
                                        while (chart2.Series[0].Points.Count > 100)
                                        {
                                            // Remove data points on the left side
                                            while (chart2.Series[0].Points.Count > 99)
                                            {
                                                chart2.Series[0].Points.RemoveAt(0);
                                            }
                                            chart2.ChartAreas[0].AxisX.Minimum = count - 99;
                                            chart2.ChartAreas[0].AxisX.Maximum = chart2.ChartAreas[0].AxisX.Minimum + 100;
                                        }
                                        chart2.Update();
                                    });
                                }
                                else
                                {
                                    chart2.Invoke((MethodInvoker)delegate
                                    {
                                        //chart2.Series[0].Points.AddXY(count, NeuralNetSettings.AvgMSE);
                                        chart2.Series[0].Points.AddXY(count, NeuralNetSettings.MSE);
                                        if (chart2.ChartAreas[0].AxisX.Maximum < count)
                                        {
                                            chart2.ChartAreas[0].AxisX.Maximum = count;
                                        }
                                        while (chart2.Series[0].Points.Count > 99)
                                        {
                                            // Remove data points on the left side
                                            while (chart2.Series[0].Points.Count > 99)
                                            {
                                                chart2.Series[0].Points.RemoveAt(0);
                                            }
                                            chart2.ChartAreas[0].AxisX.Minimum = count - 99;
                                            chart2.ChartAreas[0].AxisX.Maximum = chart2.ChartAreas[0].AxisX.Minimum + 100;
                                        }
                                        chart2.Update();
                                    });
                                }
                                if (chart3.InvokeRequired)
                                {
                                    chart3.Invoke((MethodInvoker)delegate
                                    {
                                        chart3.Series[0].Points.AddXY(count, checkerr);
                                        if (chart3.ChartAreas[0].AxisX.Maximum < count)
                                        {
                                            chart3.ChartAreas[0].AxisX.Maximum = count;
                                        }
                                        while (chart3.Series[0].Points.Count > 100)
                                        {
                                            // Remove data points on the left side
                                            while (chart3.Series[0].Points.Count > 99)
                                            {
                                                chart3.Series[0].Points.RemoveAt(0);
                                                // Adjust X axis scale     
                                            }
                                            chart3.ChartAreas[0].AxisX.Minimum = count - 99;
                                            chart3.ChartAreas[0].AxisX.Maximum = chart3.ChartAreas[0].AxisX.Minimum + 100;
                                        }
                                        chart3.Update();
                                    });
                                }
                                else
                                {
                                    chart3.Series[0].Points.AddXY(count, checkerr);
                                    if (chart3.ChartAreas[0].AxisX.Maximum < count)
                                    {
                                        chart3.ChartAreas[0].AxisX.Maximum = count;
                                    }
                                    while (chart3.Series[0].Points.Count > 100)
                                    {
                                        // Remove data points on the left side
                                        while (chart3.Series[0].Points.Count > 99)
                                        {
                                            chart3.Series[0].Points.RemoveAt(0);
                                            // Adjust X axis scale     
                                        }
                                        chart3.ChartAreas[0].AxisX.Minimum = count - 99;
                                        chart3.ChartAreas[0].AxisX.Maximum = chart3.ChartAreas[0].AxisX.Minimum + 00;
                                    }
                                    chart3.Update();
                                }
                                count++;
                            }
                            if (checkerr <= NeuralNetSettings.Accuracy)
                                flag = false;
                        }
                        MessageBox.Show("Сеть обучена! За " + count + " итераций!\nСреднеквадратическая ошибка: " + NeuralNetSettings.AvgMSE
                                        + "\nТекущая ошибка:" + checkerr);
                        source.Dispose();
                        timer.Stop();
                    });

                }, cancelToken);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            tsk.Start();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = "Время тестирования";
            date = DateTime.Now;
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 10;

            chart2.Series[0].Points.Clear();
            chart3.Series[0].Points.Clear();
            double[] y = new double[1];
            double[] x = new double[dataGridView1.ColumnCount];
            layers = new Layer[NeuralNetSettings.NumLayers];
            // Выход первого слоя необходим для того, чтобы подать его на вход второго слоя и т.д.
            layers[0] = new Layer(NeuralNetSettings.NumIn, NeuralNetSettings.NumNeurons[0]); // создаем первый входной слой. Подаем ему на вход вектор входов и число нейронов
            for (int i = 1; i < NeuralNetSettings.NumLayers; i++)
            {
                layers[i] = new Layer(NeuralNetSettings.NumNeurons[i - 1], NeuralNetSettings.NumNeurons[i]);
            }
            for (int i = 0; i < NeuralNetSettings.NumLayers; i++)
            {
                layers[i].W = NeuralNetSettings.W[i];
                layers[i].Bias = NeuralNetSettings.Bias[i];
            }
            NeuralNetSettings.X = new double[Convert.ToInt32(tbCountTest.Text)][];
            // загрузить данные из таблицы в массив
            // ****************************************************************************************
            for (int i = 0; i < Convert.ToInt32(tbCountTest.Text); i++)
            {
                NeuralNetSettings.X[i] = new double[NeuralNetSettings.NumIn];
            }
            for (int i = 0; i < Convert.ToInt32(tbCountTest.Text); i++)
            {
                for (int j = 0; j < NeuralNetSettings.NumIn; j++)
                {
                    this.Invoke((Action)delegate
                    {
                        NeuralNetSettings.X[i][j] = double.Parse(dataGridView1[j, i].Value.ToString());
                    });
                }
            }
            NeuralNetSettings.Y = new double[Convert.ToInt32(tbCountTest.Text)][];
            for (int i = 0; i < Convert.ToInt32(tbCountTest.Text); i++)
            {
                NeuralNetSettings.Y[i] = new double[dataGridView2.ColumnCount];
            }
            for (int i = 0; i < Convert.ToInt32(tbCountTest.Text); i++)
            {
                for (int j = 0; j < dataGridView2.ColumnCount; j++)
                {
                    this.Invoke((Action)delegate
                    {
                        NeuralNetSettings.Y[i][j] = double.Parse(dataGridView2[j, i].Value.ToString());
                    });
                }
            }
            // ******************************************************************************************
            // вычислить необходимые данные для мастабирования
            // **********************************************************************
            double temp;

            // *******************************************************************
            // масштабирование
            // *******************************************************************
            if (cbInConvert.Checked) // если выбрано масштабирование вх данных
            {
                NeuralNetSettings.AvgX = new double[NeuralNetSettings.NumIn];
                NeuralNetSettings.Sigma = new double[NeuralNetSettings.NumIn];
                for (int i = 0; i < NeuralNetSettings.NumIn; i++)
                {
                    temp = 0;
                    for (int j = 0; j < Convert.ToInt32(tbCountTest.Text); j++)
                    {
                        temp += NeuralNetSettings.X[j][i];
                    }
                    NeuralNetSettings.AvgX[i] = temp / Convert.ToInt32(tbCountTest.Text);
                    temp = 0;
                    for (int j = 0; j < Convert.ToInt32(tbCountTest.Text); j++)
                    {
                        temp += Math.Pow((NeuralNetSettings.X[j][i] - NeuralNetSettings.AvgX[i]), 2);
                    }
                    NeuralNetSettings.Sigma[i] = Math.Sqrt(temp / Convert.ToInt32(tbCountTest.Text));
                }
                for (int i = 0; i < Convert.ToInt32(tbCountTest.Text); i++)
                {
                    for (int j = 0; j < NeuralNetSettings.NumIn; j++)
                    {
                        NeuralNetSettings.X[i][j] = 1 / (1 + Math.Exp(-((NeuralNetSettings.X[i][j] - NeuralNetSettings.AvgX[j]) / ((2 * NeuralNetSettings.Sigma[j]) / (2 * Math.PI)))));
                    }
                }
            }
            // **********************************************************************************
            // Масштабирование эталонов
            // **********************************************************************************
            if (cbOutConvert.Checked)
            {
                temp = 0.0;
                NeuralNetSettings.AvgY = new double[dataGridView2.ColumnCount];
                NeuralNetSettings.SigmaY = new double[dataGridView2.ColumnCount];
                for (int i = 0; i < dataGridView2.ColumnCount; i++)
                {
                    temp = 0;
                    for (int j = 0; j < Convert.ToInt32(tbCountTest.Text); j++)
                    {
                        temp += NeuralNetSettings.Y[j][i];
                    }
                    NeuralNetSettings.AvgY[i] = temp / Convert.ToInt32(tbCountTest.Text);
                    temp = 0;
                    for (int j = 0; j < Convert.ToInt32(tbCountTest.Text); j++)
                    {
                        temp += Math.Pow((NeuralNetSettings.Y[j][i] - NeuralNetSettings.AvgY[i]), 2);
                    }
                    NeuralNetSettings.SigmaY[i] = Math.Sqrt(temp / Convert.ToInt32(tbCountTest.Text));

                }
                for (int i = 0; i < Convert.ToInt32(tbCountTest.Text); i++)
                {
                    for (int j = 0; j < dataGridView2.ColumnCount; j++)
                    {
                        NeuralNetSettings.Y[i][j] = 1 / (1 + Math.Exp(-((NeuralNetSettings.Y[i][j] - NeuralNetSettings.AvgY[j]) / ((2 * NeuralNetSettings.SigmaY[j]) / (2 * Math.PI)))));
                    }
                }
            }
            chart2.Series[0].Points.Clear();
            chart3.Series[0].Points.Clear();
            int count = 0;
            double error = 0;
            double output = 0.0;
            timer.Start();
            layers = new Layer[NeuralNetSettings.NumLayers];
            // Выход первого слоя необходим для того, чтобы подать его на вход второго слоя и т.д.
            layers[0] = new Layer(NeuralNetSettings.NumIn, NeuralNetSettings.NumNeurons[0]); // создаем первый входной слой. Подаем ему на вход вектор входов и число нейронов
            for (int i = 1; i < NeuralNetSettings.NumLayers; i++)
            {
                layers[i] = new Layer(NeuralNetSettings.NumNeurons[i - 1], NeuralNetSettings.NumNeurons[i]);
            }
            for (int i = 0; i < NeuralNetSettings.NumLayers; i++)
            {
                layers[i].W = NeuralNetSettings.W[i];
                layers[i].Bias = NeuralNetSettings.Bias[i];
            }
            try
            {
                Task.Run(() =>
                {
                    Parallel.Invoke(() =>
                    {

                        for (int k = 0; k < Convert.ToInt32(tbCountTest.Text); k++)
                        {
                            timer.Tick += new EventHandler(TickTimer);
                            layers[0].X = NeuralNetSettings.X[k];
                            // *********************************************************************************************************
                            if (NeuralNetSettings.ActFuncType[0] == "Сигмоидальная")
                                layers[0].GetLayerOut(NeuralNetSettings.SigmoidA[0]);
                            else
                                layers[0].GetTangLayerOut(NeuralNetSettings.TanA[0], NeuralNetSettings.TanB[0]);
                            for (int i = 0; i < NeuralNetSettings.NumLayers - 1; i++)
                            {
                                layers[i + 1].X = layers[i].Output;
                                if (NeuralNetSettings.ActFuncType[i + 1] == "Сигмоидальная")
                                    layers[i + 1].GetLayerOut(NeuralNetSettings.SigmoidA[i + 1]);
                                else
                                    layers[i + 1].GetTangLayerOut(NeuralNetSettings.TanA[i + 1], NeuralNetSettings.TanB[i + 1]);
                            }
                            // Обратное шкалирование
                            // **************************************************************
                            if (cbOutConvert.Checked)
                            {
                                output = NeuralNetSettings.AvgY[0] + ((2 * NeuralNetSettings.SigmaY[0] * Math.Log(Math.Sqrt(-1 + (1 / (1 - layers[NeuralNetSettings.NumLayers - 1].Output[0])))) / Math.PI));
                                error = double.Parse(dataGridView2[0, k].Value.ToString()) - output;
                            }
                            else
                            {
                                output = layers[NeuralNetSettings.NumLayers - 1].Output[0];
                                error = Math.Abs(NeuralNetSettings.Y[k][0] - output);
                            }

                            if (chart3.InvokeRequired)
                            {
                                chart3.Invoke((MethodInvoker)delegate
                                {
                                    chart3.Series[0].Points.AddXY(count, Math.Abs(error));
                                    if (chart3.ChartAreas[0].AxisX.Maximum < count)
                                    {
                                        chart3.ChartAreas[0].AxisX.Maximum = count;
                                    }
                                    while (chart3.Series[0].Points.Count > 200)
                                    {
                                        // Remove data points on the left side
                                        while (chart3.Series[0].Points.Count > 199)
                                        {
                                            chart3.Series[0].Points.RemoveAt(0);
                                            // Adjust X axis scale     
                                        }
                                        chart3.ChartAreas[0].AxisX.Minimum = count - 199;
                                        chart3.ChartAreas[0].AxisX.Maximum = chart3.ChartAreas[0].AxisX.Minimum + 200;
                                    }
                                    chart3.Update();
                                });
                            }
                            else
                            {
                                chart3.Series[0].Points.AddXY(count, Math.Abs(error));
                                if (chart3.ChartAreas[0].AxisX.Maximum < count)
                                {
                                    chart3.ChartAreas[0].AxisX.Maximum = count;
                                }
                                while (chart3.Series[0].Points.Count > 200)
                                {
                                    // Remove data points on the left side
                                    while (chart3.Series[0].Points.Count > 199)
                                    {
                                        chart3.Series[0].Points.RemoveAt(0);
                                        // Adjust X axis scale     
                                    }
                                    chart3.ChartAreas[0].AxisX.Minimum = count - 199;
                                    chart3.ChartAreas[0].AxisX.Maximum = chart3.ChartAreas[0].AxisX.Minimum + 200;
                                }
                                chart3.Update();
                            }
                            if (count == Convert.ToInt32(tbCountTest.Text))
                            {
                                timer.Stop();
                                MessageBox.Show("Тестирование завершено!");
                            }
                            count++;
                        }
                    });
                });

            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            source.Cancel();
            cancelToken = source.Token;
        }
  
        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();
            dataGridView2.Columns.Clear();
            dataGridView2.Rows.Clear();
            dataGridView1.ColumnCount = NeuralNetSettings.NumIn;
            dataGridView1.RowCount = Convert.ToInt32(tbCountTest.Text);

            dataGridView2.ColumnCount = NeuralNetSettings.NumNeurons[NeuralNetSettings.NumLayers - 1];
            dataGridView2.RowCount = Convert.ToInt32(tbCountTest.Text);

            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                for (int j = 0; j < dataGridView1.RowCount; j++)
                {
                    dataGridView1[i, j].Value = 0;
                }
            }
            for (int i = 0; i < dataGridView2.ColumnCount; i++)
            {
                for (int j = 0; j < dataGridView2.RowCount; j++)
                {
                    dataGridView2[i, j].Value = 0;
                }
            }
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ClassificationForm_Load(object sender, EventArgs e)
        {
            dataGridView1.ColumnCount = NeuralNetSettings.NumIn;
            dataGridView1.RowCount = NeuralNetSettings.Epoch;

            dataGridView2.ColumnCount = NeuralNetSettings.NumNeurons[NeuralNetSettings.NumLayers - 1];
            dataGridView2.RowCount = NeuralNetSettings.Epoch;

            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                for (int j = 0; j < dataGridView1.RowCount; j++)
                {
                    dataGridView1[i, j].Value = 0;
                }
            }
            for (int i = 0; i < dataGridView2.ColumnCount; i++)
            {
                for (int j = 0; j < dataGridView2.RowCount; j++)
                {
                    dataGridView2[i, j].Value = 0;
                }
            }
        }
    }
}
