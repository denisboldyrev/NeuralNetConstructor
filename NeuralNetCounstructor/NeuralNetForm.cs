using System;
using System.Xml;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Neuron;
using System.Threading;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;
using System.Globalization;

namespace NeuralNetCounstructor
{
    public partial class NeuralNetForm : Form
    {
        AppSettings NeuralNetSettings = AppSettings.GetInstance();

        public NeuralNetForm(AppSettings NeuralNetSettings)
        {
            this.NeuralNetSettings = NeuralNetSettings;
            InitializeComponent();
        }

        Layer[] layers;
        
        double outErr;

        CancellationTokenSource source;
        CancellationToken cancelToken;
        CancellationToken cancelToken1;
        double sumMSE;
        DateTime date = new DateTime();
        private void TickTimer(object sender, EventArgs e)
        {

            long tick = DateTime.Now.Ticks - date.Ticks;
            DateTime stopWatch = new DateTime();
            stopWatch = stopWatch.AddTicks(tick);
            toolStripStatusLabel1.Text = String.Format("{0:HH:mm:ss:ff}", stopWatch);
        }
        

        private void btnLearn_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 10;
            


            source = new CancellationTokenSource();
               
              
                //chart1.Series[0].Points.Clear();
                //chart1.Series[1].Points.Clear();
                //chart2.Series[0].Points.Clear();
                //chart3.Series[0].Points.Clear();

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


                // *******************************************************************
                // масштабирование
                // *******************************************************************

                double temp;
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
                if (cbOutConvert.Checked)
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
            Task.Run(() =>
            {
                this.Invoke((Action)delegate
                {
                    timer.Start();
                });
                try
                {

                    int count = 0;
                    double sumError = 0;
                    double output = 0.0;
                    bool flag = true;

                    double checkerr = 2;
                    //for (; checkerr >= NeuralNetSettings.Accuracy;) // изменено 04.07
                    //    {
                    Random r = new Random();
                    int rand = 0;
                    for (; flag == true;)
                    {
                      
                        
                        sumError = 0;
                        NeuralNetSettings.MSE = 0;
                        sumMSE = 0;
                        for (int k = 0; k < NeuralNetSettings.Epoch; k++)
                        {
                            this.Invoke((MethodInvoker)delegate
                            {
                                timer.Tick += new EventHandler(TickTimer);
                            });
                            //                           rand  = r.Next(0, NeuralNetSettings.Epoch);
                            if (cancelToken.IsCancellationRequested)
                            {
                                timer.Stop();
                                MessageBox.Show("Обучение прервано!\nСреднеквадратическая ошибка: " + NeuralNetSettings.AvgMSE
                                    + "\nТекущая ошибка:" + checkerr);
                                return;
                            }
                            layers[0].X = NeuralNetSettings.X[k];

                            // *********************************************************************************************************
                            if (NeuralNetSettings.ActFuncType[0] == "Сигмоидальная")
                                layers[0].GetLayerOut(NeuralNetSettings.SigmoidA[0]);
                            if (NeuralNetSettings.ActFuncType[0] == "Гиперболический тангенс")
                                layers[0].GetTangLayerOut(NeuralNetSettings.TanA[0], NeuralNetSettings.TanB[0]);
                            if(NeuralNetSettings.ActFuncType[0] == "Линейная")
                                layers[0].GetLinearOut();

                            for (int i = 0; i < NeuralNetSettings.NumLayers - 1; i++)
                            {
                                layers[i + 1].X = layers[i].Output;
                                if (NeuralNetSettings.ActFuncType[i + 1] == "Сигмоидальная")
                                    layers[i + 1].GetLayerOut(NeuralNetSettings.SigmoidA[i + 1]);
                                else if (NeuralNetSettings.ActFuncType[i + 1] == "Линейная")
                                    layers[i + 1].GetLinearOut();
                                else if (NeuralNetSettings.ActFuncType[i + 1] == "Гиперболический тангенс")
                                    layers[i + 1].GetTangLayerOut(NeuralNetSettings.TanA[i + 1], NeuralNetSettings.TanB[i + 1]);
                            }
                            if (NeuralNetSettings.ActFuncType[NeuralNetSettings.NumLayers - 1] == "Сигмоидальная")
                                layers[NeuralNetSettings.NumLayers - 1].CalcWeightsOutLayer(
                                     NeuralNetSettings.LearningRate, NeuralNetSettings.Y[k],
                                     NeuralNetSettings.SigmoidA[NeuralNetSettings.NumLayers - 1]); // вычислить ошибку выходного слоя
                            else if (NeuralNetSettings.ActFuncType[NeuralNetSettings.NumLayers - 1] == "Гиперболический тангенс")
                                layers[NeuralNetSettings.NumLayers - 1].CalcWeightsTangOutLayer(
                                    NeuralNetSettings.LearningRate, NeuralNetSettings.Y[k],
                                    NeuralNetSettings.TanA[NeuralNetSettings.NumLayers - 1],
                                    NeuralNetSettings.TanB[NeuralNetSettings.NumLayers - 1]); // вычислить ошибку выходного слоя
                            else
                                layers[NeuralNetSettings.NumLayers - 1].CalcWeightsLinearOutLayer(
                                    NeuralNetSettings.LearningRate, NeuralNetSettings.Y[k]); // вычислить ошибку выходного слоя

                            for (int i = NeuralNetSettings.NumLayers - 2; i >= 0; i--)
                            {
                                if (NeuralNetSettings.ActFuncType[i] == "Сигмоидальная")
                                    layers[i].CalcWeightsHiddenLayer(NeuralNetSettings.LearningRate,
                                                                     layers[i + 1].LocalGrad,
                                                                     NeuralNetSettings.SigmoidA[i]);
                                else if (NeuralNetSettings.ActFuncType[i] == "Гиперболический тангенс")
                                    layers[i].CalcWeightsHiddenTangLayer(NeuralNetSettings.LearningRate,
                                         layers[i + 1].LocalGrad, NeuralNetSettings.TanA[i],
                                         NeuralNetSettings.TanB[i]);

                            }
                            // Обратное шкалирование
                            // **************************************************************
                            if (cbOutConvert.Checked)
                            {

                                //    output = NeuralNetSettings.AvgY[0] + ((2 * NeuralNetSettings.SigmaY[0] * Math.Log(Math.Sqrt(-1 + Math.Tanh(layers[NeuralNetSettings.NumLayers - 1].Output[0]))) / Math.PI));
                                output = NeuralNetSettings.AvgY[0] + ((2 * NeuralNetSettings.SigmaY[0] * Math.Log(Math.Sqrt(-1 + (1 / (1 - layers[NeuralNetSettings.NumLayers - 1].Output[0])))) / Math.PI));
                            }
                            else
                            {
                                output = layers[NeuralNetSettings.NumLayers - 1].Output[0];
                            }
                            outErr = double.Parse(dataGridView2[0, k].Value.ToString()) - output;
                            //NeuralNetSettings.MSE += 0.5 * (double.Parse(dataGridView2[0, k].Value.ToString()) - output) * (double.Parse(dataGridView2[0, k].Value.ToString()) - output);
                            NeuralNetSettings.MSE = 0.5 * outErr * outErr;
                            sumMSE += NeuralNetSettings.MSE;
                            NeuralNetSettings.AvgMSE = sumMSE / (k + 1);
                            sumError += Math.Abs(outErr);
                            checkerr = sumError / (k + 1);


                            if (chart1.InvokeRequired)
                            {
                                chart1.Invoke((MethodInvoker)delegate
                                {
                                    chart1.Series[0].Points.AddXY(count, double.Parse(dataGridView2[0, k].Value.ToString()));
                                    chart1.Series[1].Points.AddXY(count, output);
                                    chart1.Series[2].Points.AddXY(count, double.Parse(dataGridView1[0, k+1].Value.ToString()));

                                        if (chart1.ChartAreas[0].AxisX.Maximum < count)
                                    {
                                        chart1.ChartAreas[0].AxisX.Maximum = count;
                                    }
                                    while (chart1.Series[0].Points.Count > 100)
                                    {
                                        // Remove data points on the left side
                                        while (chart1.Series[0].Points.Count > 99)
                                        {

                                            chart1.Series[0].Points.RemoveAt(0);
                                            chart1.Series[1].Points.RemoveAt(0);
                                            chart1.Series[2].Points.RemoveAt(0);
                                            // Adjust X axis scale     
                                        }
                                        chart1.ChartAreas[0].AxisX.Minimum = count - 99;
                                        chart1.ChartAreas[0].AxisX.Maximum = chart1.ChartAreas[0].AxisX.Minimum + 100;

                                    }
                          //          chart1.Update();
                                });
                            }
                            else
                            {
                                chart1.Series[0].Points.AddXY(count, double.Parse(dataGridView2[0, k].Value.ToString()));
                                chart1.Series[1].Points.AddXY(count, output);
                                chart1.Series[2].Points.AddXY(count, double.Parse(dataGridView1[0, k+1].Value.ToString())); //k+1
                                if (chart1.ChartAreas[0].AxisX.Maximum < count)
                                {
                                    chart1.ChartAreas[0].AxisX.Maximum = count;
                                }
                                while (chart1.Series[0].Points.Count > 100)
                                {
                                    // Remove data points on the left side
                                    while (chart1.Series[0].Points.Count > 99)
                                    {
                                        chart1.Series[0].Points.RemoveAt(0);
                                        chart1.Series[1].Points.RemoveAt(0);
                                        chart1.Series[2].Points.RemoveAt(0);
                                        // Adjust X axis scale     
                                    }
                                    chart1.ChartAreas[0].AxisX.Minimum = count - 99;
                                    chart1.ChartAreas[0].AxisX.Maximum = chart1.ChartAreas[0].AxisX.Minimum + 100;
                                }
                   //             chart1.Update();
                            }

                            if (chart2.InvokeRequired)
                            {
                                chart2.Invoke((MethodInvoker)delegate
                                {
                                        //chart2.Series[0].Points.AddXY(count, NeuralNetSettings.AvgMSE);
                                        if (cbMSE.Checked)
                                    {
                                        chart2.Series[0].Points.AddXY(count, NeuralNetSettings.MSE);
                                            //    cbAvgMSE.Checked = false;
                                        }
                                    if (cbAvgMSE.Checked)
                                    {
                                        chart2.Series[0].Points.AddXY(count, NeuralNetSettings.AvgMSE);
                                            //   cbMSE.Checked = false;
                                        }

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
   //                                 chart2.Update();
                                });
                            }
                            else
                            {
                                chart2.Invoke((MethodInvoker)delegate
                                {
                                        //chart2.Series[0].Points.AddXY(count, NeuralNetSettings.AvgMSE);
                                        if (cbMSE.Checked)
                                    {
                                        chart2.Series[0].Points.AddXY(count, NeuralNetSettings.MSE);
                                            //    cbAvgMSE.Checked = false;
                                        }
                                    if (cbAvgMSE.Checked)
                                    {
                                        chart2.Series[0].Points.AddXY(count, NeuralNetSettings.AvgMSE);
                                            //   cbAvgMSE.Checked = false;
                                        }
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
                                        //                   chart2.ChartAreas[0].AxisY.Minimum = Math.Round(NeuralNetSettings.AvgMSE, 2);
                                    }
                                });
                            }


                            if (chart3.InvokeRequired)
                            {
                                chart3.Invoke((MethodInvoker)delegate
                                {

                                    if (cbError.Checked)
                                    {
                                        chart3.Series[0].Points.AddXY(count, outErr);
                                            //      cbAvgError.Checked = false;
                                        }
                                    if (cbAvgError.Checked)
                                    {
                                        chart3.Series[0].Points.AddXY(count, checkerr);
                                            //     cbError.Checked = false;
                                        }

                                    if (chart1.ChartAreas[0].AxisX.Maximum < count)
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
                      //              chart3.Update();
                                });
                            }
                            else
                            {
                                if (cbError.Checked)
                                {
                                    chart3.Series[0].Points.AddXY(count, outErr);
                                    //          cbAvgError.Checked = false;
                                }
                                if (cbAvgError.Checked)
                                {
                                    chart3.Series[0].Points.AddXY(count, checkerr);
                                    //          cbError.Checked = false;
                                }
                                if (chart3.ChartAreas[0].AxisX.Maximum < count)
                                {
                                    chart3.ChartAreas[0].AxisX.Maximum = count;
                                }
                                while (chart3.Series[0].Points.Count > 100)
                                {
                                    // Remove data points on the left side
                                    while (chart3.Series[0].Points.Count > 99)
                                    {
                                        chart1.Series[0].Points.RemoveAt(0);
                                        // Adjust X axis scale     
                                    }
                                    chart3.ChartAreas[0].AxisX.Minimum = count - 99;
                                    chart3.ChartAreas[0].AxisX.Maximum = chart1.ChartAreas[0].AxisX.Minimum + 00;
                                }
                       //         chart3.Update();
                            }
                            count++;
                        }
                        if (checkerr <= NeuralNetSettings.Accuracy)
                            flag = false;
                    }
                    MessageBox.Show("Сеть обучена! За " + count + " итераций!\nСреднеквадратическая ошибка: " + NeuralNetSettings.AvgMSE
                                    + "\nТекущая ошибка:" + checkerr);
                    //source.Dispose();
                    dataGridView1.Dispose();
                    dataGridView2.Dispose();
 //                   timer.Stop();

                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
            });
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
//            NeuralNetSettings.X = new double[NeuralNetSettings.Epoch][];

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                //try
                //{
                                        string[] inputs = File.ReadAllLines(openFile.FileName);
                    double[] dout;
                    dataGridView1.ColumnCount = NeuralNetSettings.NumIn;
                                       dataGridView1.RowCount = inputs.Length;
                  //  dataGridView1.RowCount = NeuralNetSettings.Epoch;
                    //                    NeuralNetSettings.X = new double[NeuralNetSettings.Epoch][];
                    string strs;
                    using (StreamReader str = new StreamReader(openFile.FileName))
                    {


                    for (int i = 0; i < inputs.Length; i++)
                    {
                        strs = str.ReadLine();
                        //if(CultureInfo.CurrentUICulture.Name == "en-US")
                        //dout = Array.ConvertAll(strs.Replace(',', '.').Split('\t', ' '), Double.Parse); // конвертируем строку в массив double
                        //else

                        if (CultureInfo.CurrentUICulture.Name == "en-US")
                            dout = Array.ConvertAll(strs.TrimEnd().Replace(',', '.').Split('\t', ' '), Double.Parse); // конвертируем строку в массив double
                        else
                            dout = Array.ConvertAll(strs.TrimEnd().Replace('.', ',').Split('\t', ' '), Double.Parse); // конвертируем строку в массив double



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
                    //for (int i = 0; i < NeuralNetSettings.Epoch; i++)
                    //{
                    //    NeuralNetSettings.X[i] = new double[NeuralNetSettings.NumIn];
                    //}
                    //for (int i = 0; i < NeuralNetSettings.Epoch; i++)
                    //{
                    //    for (int j = 0; j < NeuralNetSettings.NumIn; j++)
                    //    {
                    //        this.Invoke((Action)delegate
                    //        {
                    //            NeuralNetSettings.X[i][j] = double.Parse(dataGridView1[j, i].Value.ToString());
                    //        });
                    //    }
                    //}
                //}
                //catch (Exception)
                //{
                //    MessageBox.Show("Ошибка при открытии файла!");
                //}
           
            // ***************************************************************
            // Масштабирование
            // ****************************************************************
            //double temp;
            //NeuralNetSettings.AvgX = new double[NeuralNetSettings.NumIn];
            //NeuralNetSettings.Sigma = new double[NeuralNetSettings.NumIn];
            //for (int i = 0; i < NeuralNetSettings.NumIn; i++)
            //{
            //    temp = 0;
            //    for (int j = 0; j < NeuralNetSettings.Epoch; j++)
            //    {
            //        temp += NeuralNetSettings.X[j][i];
            //    }
            //    NeuralNetSettings.AvgX[i] = temp / NeuralNetSettings.Epoch;
            //    temp = 0;
            //    for (int j = 0; j < NeuralNetSettings.Epoch; j++)
            //    {
            //        temp += Math.Pow((NeuralNetSettings.X[j][i] - NeuralNetSettings.AvgX[i]), 2);
            //    }
            //    NeuralNetSettings.Sigma[i] = Math.Sqrt(temp / NeuralNetSettings.Epoch);

            //}
            //for (int i = 0; i < NeuralNetSettings.Epoch; i++)
            //{
            //    for (int j = 0; j < NeuralNetSettings.NumIn; j++)
            //    {
            //        NeuralNetSettings.X[i][j] = 1 / (1 + Math.Exp(-((NeuralNetSettings.X[i][j] - NeuralNetSettings.AvgX[j]) / ((2 * NeuralNetSettings.Sigma[j]) / (2 * Math.PI)))));
            //    }
            //}
            // **************************************************************
            // Обратное шкалирование
            // **************************************************************
            //for (int i = 0; i < NeuralNetSettings.Epoch; i++)
            //{
            //    for (int j = 0; j < NeuralNetSettings.NumIn; j++)
            //    {
            //        NeuralNetSettings.X[i][j] = NeuralNetSettings.AvgX[j] + ((2 * NeuralNetSettings.Sigma[j] * Math.Log(Math.Sqrt(-1 + (1 / (1 - NeuralNetSettings.X[i][j])))) / Math.PI));
            //    }
            //}

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
                    dataGridView2.ColumnCount = 1;
                    dataGridView2.RowCount = etalon.Length;
                    //NeuralNetSettings.Y = new double[NeuralNetSettings.Epoch][];

                        for (int i = 0; i < etalon.Length; i++)
                        {
                        if (CultureInfo.CurrentUICulture.Name == "en-US")
                            dout = Array.ConvertAll(etalon[i].TrimEnd().Replace(',', '.').Split('\t', ' '), Double.Parse); // конвертируем строку в массив double
                        else
                            dout = Array.ConvertAll(etalon[i].TrimEnd().Replace('.', ',').Split('\t', ' '), Double.Parse); // конвертируем строку в массив double
                        for (int j = 0; j < dout.Length; j++)
                            {
                                //Application.DoEvents();
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

                        //for (int i = 0; i < NeuralNetSettings.Epoch; i++)
                        //{
                        //    NeuralNetSettings.Y[i] = new double[dataGridView2.ColumnCount];
                        //}
                        //for (int i = 0; i < NeuralNetSettings.Epoch; i++)
                        //{
                        //    for (int j = 0; j < dataGridView2.ColumnCount; j++)
                        //    {
                        //        this.Invoke((Action)delegate
                        //        {
                        //            NeuralNetSettings.Y[i][j] = double.Parse(dataGridView2[j, i].Value.ToString());
                        //        });
                        //    }
                        //}
                }
                catch
                {
                    MessageBox.Show("Ошибка при открытии файла!");
                }
            }
           

            // ************************************************************************
            // Масштабирование эталонов
            // ************************************************************************
            //double temp;
            //NeuralNetSettings.AvgY = new double[dataGridView2.ColumnCount];
            //NeuralNetSettings.SigmaY = new double[dataGridView2.ColumnCount];
            //for (int i = 0; i < dataGridView2.ColumnCount; i++)
            //{
            //    temp = 0;
            //    for (int j = 0; j < NeuralNetSettings.Epoch; j++)
            //    {
            //        temp += NeuralNetSettings.Y[j][i];
            //    }
            //    NeuralNetSettings.AvgY[i] = temp / NeuralNetSettings.Epoch;
            //    temp = 0;
            //    for (int j = 0; j < NeuralNetSettings.Epoch; j++)
            //    {
            //        temp += Math.Pow((NeuralNetSettings.Y[j][i] - NeuralNetSettings.AvgY[i]), 2);
            //    }
            //    NeuralNetSettings.SigmaY[i] = Math.Sqrt(temp / NeuralNetSettings.Epoch);

            //}
            //for (int i = 0; i < NeuralNetSettings.Epoch; i++)
            //{
            //    for (int j = 0; j < dataGridView2.ColumnCount; j++)
            //    {
            //        NeuralNetSettings.Y[i][j] = 1 / (1 + Math.Exp(-((NeuralNetSettings.Y[i][j] - NeuralNetSettings.AvgY[j]) / ((2 * NeuralNetSettings.SigmaY[j]) / (2 * Math.PI)))));
            //    }
            //}
            //for (int i = 0; i < NeuralNetSettings.Epoch; i++)
            //{
            //    for (int j = 0; j < dataGridView2.ColumnCount; j++)
            //    {
            //        Application.DoEvents();
            //        //this.Invoke((Action)delegate
            //        //{
            //        //    dataGridView1[j, i].Value = dout[j];
            //        //    dataGridView1.Columns[j].Width = 60;
            //        //});
            //        if (dataGridView2.InvokeRequired)
            //        {
            //            dataGridView1.Invoke((MethodInvoker)delegate
            //            {
            //                dataGridView2[j, i].Value = NeuralNetSettings.Y[i][j];
            //                dataGridView2.Columns[j].Width = 60;
            //            });
            //        }
            //        else
            //        {
            //            dataGridView2[j, i].Value = NeuralNetSettings.Y[i][j];
            //            dataGridView2.Columns[j].Width = 60;
            //        }
            //    }
            //}
        }
        private void сохранитьToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "xml files (*.xml) | *.xml";
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                NeuralNetSettings.Weights = new DataSet();

                DataTable[] arrTables = new DataTable[NeuralNetSettings.NumLayers];
                arrTables[0] = new DataTable("Layer1");
                DataColumn[][] c = new DataColumn[NeuralNetSettings.NumLayers][];
                c[0] = new DataColumn[NeuralNetSettings.NumIn];
                DataRow r;
                for (int i = 0; i < NeuralNetSettings.NumIn; i++)
                {
                    c[0][i] = new DataColumn("W1" + (i + 1));
                    arrTables[0].Columns.Add(c[0][i]);
                }
                for (int i = 0; i < NeuralNetSettings.NumNeurons[0]; i++)
                {
                    r = arrTables[0].NewRow();
                    for (int j = 0; j < NeuralNetSettings.NumIn; j++)
                    {
                        r[j] = Convert.ToDouble(NeuralNetSettings.W[0][i, j]);
                    }
                    arrTables[0].Rows.Add(r);
                }
                for (int i = 1; i < NeuralNetSettings.NumLayers; i++)
                {
                    arrTables[i] = new DataTable("Layer" + (i + 1));
                    c[i] = new DataColumn[NeuralNetSettings.NumNeurons[i - 1]];
                    for (int j = 0; j < NeuralNetSettings.NumNeurons[i - 1]; j++)
                    {
                        c[i][j] = new DataColumn("W" + (i + 1) + "" + (j + 1));
                        arrTables[i].Columns.Add(c[i][j]);
                    }
                }
                for (int i = 1; i < NeuralNetSettings.NumLayers; i++)
                {
                    for (int j = 0; j < NeuralNetSettings.NumNeurons[i]; j++)
                    {
                        r = arrTables[i].NewRow();
                        for (int k = 0; k < NeuralNetSettings.NumNeurons[i - 1]; k++)
                        {
                            r[k] = Convert.ToDouble(NeuralNetSettings.W[i][j, k]);
                        }
                        arrTables[i].Rows.Add(r);
                    }
                }
                for (int i = 0; i < NeuralNetSettings.NumLayers; i++)
                {
                    NeuralNetSettings.Weights.Tables.Add(arrTables[i]);
                }
                // ************************************************************************
                //
                // ************************************************************************
                NeuralNetSettings.Biases = new DataSet();
                DataTable[] biasTables = new DataTable[NeuralNetSettings.NumLayers];
                biasTables[0] = new DataTable("LayerBias1");
                c[0] = new DataColumn[NeuralNetSettings.NumNeurons[0]];
                for (int i = 0; i < NeuralNetSettings.NumNeurons[0]; i++)
                {
                    c[0][i] = new DataColumn("Bias1" + (i + 1));
                    biasTables[0].Columns.Add(c[0][i]);
                }
                for (int i = 0; i < NeuralNetSettings.NumNeurons[0]; i++)
                {
                    r = biasTables[0].NewRow();
                    r[i] = Convert.ToDouble(NeuralNetSettings.Bias[0][i]);
                    biasTables[0].Rows.Add(r);
                }
                for (int i = 1; i < NeuralNetSettings.NumLayers; i++)
                {
                    biasTables[i] = new DataTable("LayerBias" + (i + 1));
                    c[i] = new DataColumn[NeuralNetSettings.NumNeurons[i]];
                    for (int j = 0; j < NeuralNetSettings.NumNeurons[i]; j++)
                    {
                        c[i][j] = new DataColumn("Bias" + (i + 1) + "" + (j + 1));
                        biasTables[i].Columns.Add(c[i][j]);
                    }
                }
                for (int i = 1; i < NeuralNetSettings.NumLayers; i++)
                {
                    for (int j = 0; j < NeuralNetSettings.NumNeurons[i]; j++)
                    {
                        r = biasTables[i].NewRow();
                        r[j] = Convert.ToDouble(NeuralNetSettings.Bias[i][j]);
                        biasTables[i].Rows.Add(r);
                    }
                }
                for (int i = 0; i < NeuralNetSettings.NumLayers; i++)
                {
                    NeuralNetSettings.Biases.Tables.Add(biasTables[i]);
                }
                // ************************************************************************
                //
                // ************************************************************************
                System.Xml.Serialization.XmlSerializer writer =
                new System.Xml.Serialization.XmlSerializer(typeof(AppSettings));

                System.IO.FileStream file = System.IO.File.Create(saveFileDialog.FileName);
                writer.Serialize(file, NeuralNetSettings);
                file.Close();

            }
            else
            {
                Exception exc = new Exception(String.Format("Ошибка при сохранении файла!"));
                MessageBox.Show(exc.Message);
                throw exc;
            }
        }

        private void btnStartTest_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                toolStripStatusLabel2.Text = "Время тестирования";
                date = DateTime.Now;
                System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
                timer.Interval = 10;

                chart1.Invoke((MethodInvoker)delegate
                {
                    chart1.Series[0].Points.Clear();
                    chart1.Series[1].Points.Clear();
                });
                chart2.Invoke((MethodInvoker)delegate
                {
                    chart2.Series[0].Points.Clear();
                });
                chart3.Invoke((MethodInvoker)delegate
                {
                    chart3.Series[0].Points.Clear();
                });
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
                int count = 0;
                double error = 0;
                double output = 0.0;
//                timer.Start();
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

                    Parallel.Invoke(async () =>
                    {
                        source = new CancellationTokenSource();

                        for (int k = 0; k < Convert.ToInt32(tbCountTest.Text); k++)
                        {
                            if (cancelToken1.IsCancellationRequested)
                            {
                                timer.Stop();
                                MessageBox.Show("Тестирование прервано!");
                                return;
                            }
                            timer.Tick += new EventHandler(TickTimer);
                            layers[0].X = NeuralNetSettings.X[k];
                            // *********************************************************************************************************
                            //if (NeuralNetSettings.ActFuncType[0] == "Сигмоидальная")
                            //    layers[0].GetLayerOut(NeuralNetSettings.SigmoidA[0]);
                            //else
                            //    layers[0].GetTangLayerOut(NeuralNetSettings.TanA[0], NeuralNetSettings.TanB[0]);
                            //for (int i = 0; i < NeuralNetSettings.NumLayers - 1; i++)
                            //{
                            //    layers[i + 1].X = layers[i].Output;
                            //    if (NeuralNetSettings.ActFuncType[i + 1] == "Сигмоидальная")
                            //        layers[i + 1].GetLayerOut(NeuralNetSettings.SigmoidA[i + 1]);
                            //    else
                            //        layers[i + 1].GetTangLayerOut(NeuralNetSettings.TanA[i + 1], NeuralNetSettings.TanB[i + 1]);
                            //}
                            if (NeuralNetSettings.ActFuncType[0] == "Сигмоидальная")
                                layers[0].GetLayerOut(NeuralNetSettings.SigmoidA[0]);
                            if (NeuralNetSettings.ActFuncType[0] == "Гиперболический тангенс")
                                layers[0].GetTangLayerOut(NeuralNetSettings.TanA[0], NeuralNetSettings.TanB[0]);
                            if (NeuralNetSettings.ActFuncType[0] == "Линейная")
                                layers[0].GetLinearOut();

                            for (int i = 0; i < NeuralNetSettings.NumLayers - 1; i++)
                            {
                                layers[i + 1].X = layers[i].Output;
                                if (NeuralNetSettings.ActFuncType[i + 1] == "Сигмоидальная")
                                    layers[i + 1].GetLayerOut(NeuralNetSettings.SigmoidA[i + 1]);
                                else if (NeuralNetSettings.ActFuncType[i + 1] == "Линейная")
                                    layers[i + 1].GetLinearOut();
                                else if (NeuralNetSettings.ActFuncType[i + 1] == "Гиперболический тангенс")
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
                            if (chart1.InvokeRequired)
                            {
                                chart1.Invoke((MethodInvoker)delegate
                                {
                                    chart1.Series[0].Points.AddXY(count, double.Parse(dataGridView2[0, k].Value.ToString()));
                                    chart1.Series[1].Points.AddXY(count, output);
                                    //chart1.Series[2].Points.AddXY(count, double.Parse(dataGridView1[0, k + 1].Value.ToString()));

                                    if (chart1.ChartAreas[0].AxisX.Maximum < count)
                                    {
                                        chart1.ChartAreas[0].AxisX.Maximum = count;
                                    }
                                    while (chart1.Series[0].Points.Count > 100)
                                    {
                                        // Remove data points on the left side
                                        while (chart1.Series[0].Points.Count > 99)
                                        {

                                            chart1.Series[0].Points.RemoveAt(0);
                                            chart1.Series[1].Points.RemoveAt(0);
                                            //chart1.Series[2].Points.RemoveAt(0);
                                            // Adjust X axis scale     
                                        }
                                        chart1.ChartAreas[0].AxisX.Minimum = count - 99;
                                        chart1.ChartAreas[0].AxisX.Maximum = chart1.ChartAreas[0].AxisX.Minimum + 100;

                                    }
                                    chart1.Update();
                                });
                            }
                            else
                            {
                                chart1.Series[0].Points.AddXY(count, double.Parse(dataGridView2[0, k].Value.ToString()));
                                chart1.Series[1].Points.AddXY(count, output);
                                //chart1.Series[2].Points.AddXY(count, double.Parse(dataGridView1[0, k + 1].Value.ToString())); //k+1
                                if (chart1.ChartAreas[0].AxisX.Maximum < count)
                                {
                                    chart1.ChartAreas[0].AxisX.Maximum = count;
                                }
                                while (chart1.Series[0].Points.Count > 100)
                                {
                                    // Remove data points on the left side
                                    while (chart1.Series[0].Points.Count > 99)
                                    {
                                        chart1.Series[0].Points.RemoveAt(0);
                                        chart1.Series[1].Points.RemoveAt(0);
                                        chart1.Series[2].Points.RemoveAt(0);
                                        // Adjust X axis scale     
                                    }
                                    chart1.ChartAreas[0].AxisX.Minimum = count - 99;
                                    chart1.ChartAreas[0].AxisX.Maximum = chart1.ChartAreas[0].AxisX.Minimum + 100;
                                }
                                chart1.Update();
                            }

                            if (chart3.InvokeRequired)
                            {
                                chart3.Invoke((MethodInvoker)delegate
                                {
                                    chart3.Series[0].Points.AddXY(count + 1, Math.Abs(error));
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
                            count++;
                            if (count == Convert.ToInt32(tbCountTest.Text))
                            {
                                timer.Stop();
                                MessageBox.Show("Тестирование завершено!");
                            }
                        }
                    });
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
            });
        }

        private void button3_Click(object sender, EventArgs e)
        {
            double[][] x = new double[dataGridView1.ColumnCount][];
            double[] y = new double[2000];
            double[] maxx = new double[dataGridView1.ColumnCount];
            double[] minx = new double[dataGridView1.ColumnCount];
            double ymax;
            double ymin;
            x[0] = new double[2000];
            x[1] = new double[2000];
            x[2] = new double[2000];
            x[3] = new double[2000];
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                for (int j = 0; j < 2000; j++)
                {
                    x[i][j] = double.Parse(dataGridView1[i, j].Value.ToString());
                }
            }
            for (int i = 0; i < 2000; i++)
            {
                y[i] = double.Parse(dataGridView2[0, i].Value.ToString());
            }
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                Array.Sort(x[i]);
                maxx[i] = x[i][0];
                minx[i] = x[i][1999];
            }
            Array.Sort(y);
            ymax = y[1999];
            ymin = y[0];
        }
        
        private void Form2_Load(object sender, EventArgs e)
        {
            dataGridView1.ColumnCount = NeuralNetSettings.NumIn;
            dataGridView1.RowCount = NeuralNetSettings.Epoch;

            dataGridView2.ColumnCount = NeuralNetSettings.NumNeurons[NeuralNetSettings.NumLayers-1];
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
        private void button6_Click(object sender, EventArgs e)
        {
            //CancellationToken ct = cancelToken.Token;

            //cancelToken.Cancel();
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            source.Cancel();
            cancelToken = source.Token;
            source.Dispose();
            //    th.Suspend();
        }
        bool flag = false;
        private void button4_Click_1(object sender, EventArgs e)
        {
            flag = true;

        }
        private void button3_Click_1(object sender, EventArgs e)
        {
            source.Cancel();
            cancelToken1 = source.Token;
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //NeuralNetSettings.W = new double[NeuralNetSettings.NumLayers][,];
            //NeuralNetSettings.Bias = new double[NeuralNetSettings.NumLayers][];
            //XmlTextReader xmlTextReader = new XmlTextReader("w.xml");

            //NeuralNetSettings.W[0] = new double[NeuralNetSettings.NumNeurons[0], NeuralNetSettings.NumIn];
            //NeuralNetSettings.Bias[0] = new double[NeuralNetSettings.NumNeurons[0]];

            //for (int i = 1; i < NeuralNetSettings.NumLayers; i++)
            //{
            //    NeuralNetSettings.W[i] = new double[NeuralNetSettings.NumNeurons[i], NeuralNetSettings.NumNeurons[i - 1]];
            //    NeuralNetSettings.Bias[i] = new double[NeuralNetSettings.NumNeurons[i]];
            //}


            //while (xmlTextReader.Read())
            //{

            //    switch (xmlTextReader.NodeType)
            //    {
            //        case XmlNodeType.Text:
            //            for (int i = 0; i < NeuralNetSettings.NumNeurons[0]; i++)
            //            {


            //                for (int j = 0; j < NeuralNetSettings.NumIn; j++)
            //                {
            //                    NeuralNetSettings.Bias[0][i] = Convert.ToDouble(xmlTextReader.Value);
            //                    NeuralNetSettings.W[0][i, j] = Convert.ToDouble(xmlTextReader.Value);

            //                }
            //            }
            //            for (int i = 1; i < NeuralNetSettings.NumLayers; i++)
            //            {
            //                for (int j = 0; j < NeuralNetSettings.NumNeurons[i]; j++)
            //                {



            //                    for (int k = 0; k < NeuralNetSettings.NumNeurons[i - 1]; k++)
            //                    {

            //                        NeuralNetSettings.Bias[i][j] = Convert.ToDouble(xmlTextReader.Value);
            //                        NeuralNetSettings.W[i][j, k] = Convert.ToDouble(xmlTextReader.Value);
            //                    }

            //                }
            //            }
            //            break;
            //    }
            //}
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.InitialDirectory = Application.StartupPath;
            openFile.Filter = "xml files (*.xml) | *.xml";
            openFile.FilterIndex = 2;
            openFile.RestoreDirectory = true;
            //            NeuralNetSettings.X = new double[NeuralNetSettings.Epoch][];

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    System.Xml.Serialization.XmlSerializer serializer =
               new System.Xml.Serialization.XmlSerializer(typeof(AppSettings));

                    FileStream fs = new FileStream(openFile.FileName, FileMode.Open);
                    XmlReader reader = XmlReader.Create(fs);
                    NeuralNetSettings = (AppSettings)serializer.Deserialize(fs);
                    fs.Close();
                }
                catch
                {
                    MessageBox.Show("Ошибка при открытии файла!");
                }
            }
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

        private void cbMSE_CheckedChanged(object sender, EventArgs e)
        {
            if (cbMSE.Checked)
                cbAvgMSE.Checked = false;
            if(!cbMSE.Checked)
                cbAvgMSE.Checked = true;
        }

        private void cbError_CheckedChanged(object sender, EventArgs e)
        {
            if (cbError.Checked)
                cbAvgError.Checked = false;
            if (!cbError.Checked)
                cbAvgError.Checked = true;
        }

        private void cbAvgMSE_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAvgMSE.Checked)
                cbMSE.Checked = false;
            if (!cbAvgMSE.Checked)
                cbMSE.Checked = true;
        }

        private void cbAvgError_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAvgError.Checked)
                cbError.Checked = false;
            if (!cbAvgError.Checked)
                cbError.Checked = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = "Время тестирования";
            date = DateTime.Now;
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 10;


            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            chart2.Series[0].Points.Clear();
            chart3.Series[0].Points.Clear();
            double[] y = new double[1];
            double[] x = new double[dataGridView1.ColumnCount];

            //INeuron[] inL = new INeuron[NeuralNetSettings.NumNeurons[0]];
            //for (int i = 0; i < NeuralNetSettings.NumNeurons[0]; i++)
            //{
            //    inL[i] = new Perceptron(NeuralNetSettings.NumIn, Perceptron.ActivationFunctionType.Sigmoid);
            //}
            //InputLayer inputLayer = new InputLayer(inL, NeuralNetSettings.NumIn , InputLayer.ActivationFunctionType.Sigmoid);

            //INeuron[] hL = new INeuron[NeuralNetSettings.NumNeurons[1]];
            //for (int i = 0; i < NeuralNetSettings.NumNeurons[1]; i++)
            //{
            //    hL[i] = new Perceptron(NeuralNetSettings.NumNeurons[0], Perceptron.ActivationFunctionType.Sigmoid);
            //}
            //HiddenLayer hiddenLayer = new HiddenLayer(inL, NeuralNetSettings.NumNeurons[0]);

            //INeuron[] oL = new INeuron[NeuralNetSettings.NumNeurons[2]];
            //for (int i = 0; i < NeuralNetSettings.NumNeurons[2]; i++)
            //{
            //    oL[i] = new Perceptron(NeuralNetSettings.NumNeurons[1], Perceptron.ActivationFunctionType.Sigmoid);
            //}
            //OutputLayer outputLayer = new OutputLayer(, NeuralNetSettings.NumNeurons[1]);


            //    inputLayer.W = NeuralNetSettings.W[0];
            //hiddenLayer.W = NeuralNetSettings.W[1];
            //outputLayer.W = NeuralNetSettings.W[2];

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

            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            chart2.Series[0].Points.Clear();
            chart3.Series[0].Points.Clear();
            int count = 0;
            double error = 0;
            double output = 0.0;
 //           timer.Start();
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
                            if (chart1.InvokeRequired)
                            {
                                chart1.Invoke((MethodInvoker)delegate
                                {
                                    chart1.Series[0].Points.AddXY(count + 1, output);
                                    chart1.Series[1].Points.AddXY(count + 1, double.Parse(dataGridView2[0, k].Value.ToString()));

                                    if (chart1.ChartAreas[0].AxisX.Maximum < count)
                                    {
                                        chart1.ChartAreas[0].AxisX.Maximum = count;
                                    }
                                    while (chart1.Series[0].Points.Count > 200)
                                    {
                                        // Remove data points on the left side
                                        while (chart1.Series[0].Points.Count > 199)
                                        {
                                            chart1.Series[0].Points.RemoveAt(0);
                                            chart1.Series[1].Points.RemoveAt(0);
                                            // Adjust X axis scale     
                                        }
                                        chart1.ChartAreas[0].AxisX.Minimum = count - 199;
                                        chart1.ChartAreas[0].AxisX.Maximum = chart1.ChartAreas[0].AxisX.Minimum + 200;

                                    }
                                    chart1.Update();
                                });
                            }
                            else
                            {
                                chart1.Series[0].Points.AddXY(count + 1, output);
                                chart1.Series[1].Points.AddXY(count + 1, double.Parse(dataGridView2[0, k].Value.ToString()));
                                if (chart1.ChartAreas[0].AxisX.Maximum < count)
                                {
                                    chart1.ChartAreas[0].AxisX.Maximum = count;
                                }
                                while (chart1.Series[0].Points.Count > 200)
                                {
                                    // Remove data points on the left side
                                    while (chart1.Series[0].Points.Count > 199)
                                    {
                                        chart1.Series[0].Points.RemoveAt(0);
                                        chart1.Series[1].Points.RemoveAt(0);
                                        // Adjust X axis scale     
                                    }
                                    chart1.ChartAreas[0].AxisX.Minimum = count - 199;
                                    chart1.ChartAreas[0].AxisX.Maximum = chart1.ChartAreas[0].AxisX.Minimum + 200;
                                }
                                chart1.Update();
                            }
                            if (chart3.InvokeRequired)
                            {
                                chart3.Invoke((MethodInvoker)delegate
                                {
                                    chart3.Series[0].Points.AddXY(count + 1, Math.Abs(error));
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
                            count++;
                            if (count == Convert.ToInt32(tbCountTest.Text))
                            {
                                timer.Stop();
                                MessageBox.Show("Тестирование завершено!");
                            }
                        }
                    });
                });
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }


    }
}

