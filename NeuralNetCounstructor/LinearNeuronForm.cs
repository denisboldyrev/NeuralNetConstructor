using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Neuron;
using System.Globalization;
using System.Threading;

namespace NeuralNetCounstructor
{

    public partial class LinearNeuronForm : Form
    {
        AppSettings NeuralNetSettings = AppSettings.GetInstance();
        public LinearNeuronForm(AppSettings NeuralNetSettings)
        {
            this.NeuralNetSettings = NeuralNetSettings;
            InitializeComponent();
        }

        private void загрузитьНаборЭталоноToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void загрузитьОбучающийНаборToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        LinearNeuron linearNeuron;
        Task tsk;

        CancellationTokenSource source = new CancellationTokenSource();
        CancellationToken cancelToken;
        DateTime date;
        private void TickTimer(object sender, EventArgs e)
        {
            long tick = DateTime.Now.Ticks - date.Ticks;
            DateTime stopWatch = new DateTime();
            stopWatch = stopWatch.AddTicks(tick);
            toolStripStatusLabel1.Text = String.Format("{0:HH:mm:ss:ff}", stopWatch);
        }

        private void btnStart_Click_1(object sender, EventArgs e)
        {
            date = DateTime.Now;
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 10;

            NeuralNetSettings.W = new double[1][,];
            NeuralNetSettings.W[0] = new double[1, NeuralNetSettings.NumIn];
            double y = 0.0;
            double[] x = new double[dataGridView1.ColumnCount];
            linearNeuron = new LinearNeuron(NeuralNetSettings.NumIn);
            int count = 0;
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();

            timer.Start();
            tsk = new Task(() =>
            {
                double checkerr = 2;
                for (; checkerr >= NeuralNetSettings.Accuracy;)
                {
                    for (int k = 0; k < NeuralNetSettings.Epoch; k++)
                    {
                        Application.DoEvents();
                        for (int i = 0; i < dataGridView1.ColumnCount; i++)
                        {
                            if (dataGridView1.InvokeRequired)
                            {
                                dataGridView1.Invoke((MethodInvoker)delegate
                                {
                                    linearNeuron.X[i] = double.Parse(dataGridView1[i, k].Value.ToString());
                                });
                            }
                            else
                            {
                                linearNeuron.X[i] = double.Parse(dataGridView1[i, k].Value.ToString());
                            }
                        }
                        linearNeuron.GetOutput();
                        if (dataGridView2.InvokeRequired)
                        {
                            dataGridView2.Invoke((MethodInvoker)delegate
                            {
                                y = double.Parse(dataGridView2[0, k].Value.ToString());
                            });
                        }
                        else
                        {
                            y = double.Parse(dataGridView2[0, k].Value.ToString());
                        }
                        
                        linearNeuron.CalcWeights(NeuralNetSettings.LearningRate, y);
                        if (chart1.InvokeRequired)
                        {
                            chart1.Invoke((MethodInvoker)delegate
                            {
                                chart1.Series[0].Points.AddXY(count, linearNeuron.Output);
                                chart1.Series[1].Points.AddXY(count, double.Parse(dataGridView2[0, k].Value.ToString()));

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
                            chart1.Series[0].Points.AddXY(count, linearNeuron.Output);
                            chart1.Series[1].Points.AddXY(count, double.Parse(dataGridView2[0, k].Value.ToString()));
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
                                    // Adjust X axis scale     
                                }
                                chart1.ChartAreas[0].AxisX.Minimum = count - 99;
                                chart1.ChartAreas[0].AxisX.Maximum = chart1.ChartAreas[0].AxisX.Minimum + 100;
                            }
                            chart1.Update();
                        }

                        if (chart2.InvokeRequired)
                        {
                            chart2.Invoke((MethodInvoker)delegate
                            {
                                chart2.Series[0].Points.AddXY(count, NeuralNetSettings.AvgMSE);
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
                                    //                  chart2.ChartAreas[0].AxisY.Minimum = Math.Round(NeuralNetSettings.AvgMSE, 2);
                                }
                                chart2.Update();
                            });
                        }
                        else
                        {
                            chart2.Invoke((MethodInvoker)delegate
                            {
                                chart2.Series[0].Points.AddXY(count, NeuralNetSettings.AvgMSE);
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
                                chart2.Update();
                            });
                        }
                        count++;

                        NeuralNetSettings.MSE += 0.5 * (y - linearNeuron.Output) * (y - linearNeuron.Output);
                        checkerr = Math.Abs(linearNeuron.Error);
                        NeuralNetSettings.AvgMSE = NeuralNetSettings.MSE / (count + 1);
                    }     
                }
                timer.Stop();
                MessageBox.Show("Сеть обучена! За " + count + " итераций! Ошибка равна: " + NeuralNetSettings.AvgMSE);
                source.Dispose();

            }, cancelToken);
            tsk.Start();
        }

        private void загрузитьОбучающийНаборToolStripMenuItem_Click_1(object sender, EventArgs e)
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
                try
                {
                    //                    string[] inputs = File.ReadAllLines(openFile.FileName);
                    double[] dout;
                    dataGridView1.ColumnCount = NeuralNetSettings.NumIn;
                    //                    dataGridView1.RowCount = inputs.Length;
                    dataGridView1.RowCount = NeuralNetSettings.Epoch;
                    //                    NeuralNetSettings.X = new double[NeuralNetSettings.Epoch][];
                    string strs;
                    using (StreamReader str = new StreamReader(openFile.FileName))
                    {


                        for (int i = 0; i < NeuralNetSettings.Epoch; i++)
                        {
                            strs = str.ReadLine();
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

                }
                catch (Exception)
                {
                    MessageBox.Show("Ошибка при открытии файла!");
                }
            }

        }

        private void загрузитьНаборЭталоноToolStripMenuItem_Click_1(object sender, EventArgs e)
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

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < NeuralNetSettings.NumIn; i++)
            {
                NeuralNetSettings.W[0][0, i] = linearNeuron.W[i];
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "xml files (*.xml) | *.xml";
            saveFileDialog.RestoreDirectory = true;
            NeuralNetSettings.Weights = new DataSet();
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                DataTable[] arrTables = new DataTable[1];
                arrTables[0] = new DataTable("Neuron");
                DataColumn[][] c = new DataColumn[1][];
                c[0] = new DataColumn[NeuralNetSettings.NumIn];
                DataRow r;
                for (int i = 0; i < NeuralNetSettings.NumIn; i++)
                {
                    c[0][i] = new DataColumn("W" + (i + 1));
                    arrTables[0].Columns.Add(c[0][i]);
                }

                r = arrTables[0].NewRow();
                for (int j = 0; j < NeuralNetSettings.NumIn; j++)
                {
                    r[j] = Convert.ToDouble(NeuralNetSettings.W[0][0, j]);
                }
                arrTables[0].Rows.Add(r);
            NeuralNetSettings.Weights.Tables.Add(arrTables[0]);
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

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LinearNeuronForm_Load(object sender, EventArgs e)
        {
            dataGridView1.ColumnCount = NeuralNetSettings.NumIn;
            dataGridView1.RowCount = NeuralNetSettings.Epoch;

            dataGridView2.ColumnCount = 1;
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
