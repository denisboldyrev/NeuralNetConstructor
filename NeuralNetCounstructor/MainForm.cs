using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Neuron;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Globalization;

namespace NeuralNetCounstructor
{

    public partial class MainForm : Form

    {
        AppSettings NeuralNetSettings = AppSettings.GetInstance();
        public Layer[] layers;
        public MainForm()
        {
            InitializeComponent();
        }
        private void btnSetNetType_Click(object sender, EventArgs e)
        {
            if (cbSelectNetType.Text == "Многослойный персептрон")
            {
                NeuralNetSettings.NetType = cbSelectNetType.Text;
                cbTypeTask.Enabled = true;
                if (cbTypeTask.Text == "Прогнозирование" || cbTypeTask.Text == "Классификация")
                {
                    cbSelectLayer.Items.Clear();
                    NeuralNetSettings.NumLayers = Convert.ToInt32(tbNumLayers.Text);

                    for (int i = 0; i < NeuralNetSettings.NumLayers; i++)
                    {
                        cbSelectLayer.Items.Insert(i, "Слой " + (i + 1));
                    }
                    NeuralNetSettings.NumNeurons = new int[NeuralNetSettings.NumLayers];
                    NeuralNetSettings.ActFuncType = new string[Convert.ToInt32(tbNumLayers.Text)];
                    NeuralNetSettings.NumIn = Convert.ToInt32(tbNumIn.Text);
                    NeuralNetSettings.LeftA = new double[NeuralNetSettings.NumLayers];
                    NeuralNetSettings.RightB = new double[NeuralNetSettings.NumLayers];
                    NeuralNetSettings.SigmoidA = new double[NeuralNetSettings.NumLayers];
                    NeuralNetSettings.TanA = new double[NeuralNetSettings.NumLayers];
                    NeuralNetSettings.TanB = new double[NeuralNetSettings.NumLayers];
                    NeuralNetSettings.W = new double[NeuralNetSettings.NumLayers][,];
                    NeuralNetSettings.Bias = new double[NeuralNetSettings.NumLayers][];
                }
            }

            if (cbSelectNetType.Text == "Линейный нейрон")
            {
                tbNumLayers.Enabled = false;
                NeuralNetSettings.NumIn = Convert.ToInt32(tbNumIn.Text);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            NeuralNetSettings.NumNeurons[cbSelectLayer.SelectedIndex] = Convert.ToInt32(tbNumNeurons.Text);

            // this.numNeurons[comboBox1.SelectedIndex] = Convert.ToInt32(textBox2.Text);
            //            numLayers = new int[Convert.ToInt32(textBox1.Text)];
            layers = new Layer[NeuralNetSettings.NumLayers]; // массив в котором хранятся объекты слоя сети. Число объектов равно числу слоев numLayers
                                                                           //double[] x = { 0.1, 0.9 }; // Входы сети. Вектор подается на первый(входной) слой layers[0] для того, чтобы вычислить выход первого слоя.
                                                                           //                           // Выход первого слоя необходим для того, чтобы подать его на вход второго слоя и т.д.
                                                                           //int[] numNeurons = { 2, 2, 1 }; // массив, который хранить число нейронов для каждого слоя
                                                                           //layers[0] = new Layer(2, 2); // создаем первый входной слой. Подаем ему на вход вектор входов и число нейронов
                                                                           //layers[0].X = x;
                                                                           //layers[0].W[0, 0] = -2;
                                                                           //layers[0].W[0, 1] = -2;
        }
        private void cbSelectNetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSelectNetType.SelectedItem.ToString() == "Многослойный персептрон")
            {
                cbTypeTask.Enabled = true;
                label1.Enabled = true;
                tbNumLayers.Enabled = true;
                groupBox1.Enabled = true;
            }
            else
            {
                cbTypeTask.Enabled = false;
            }
            if (cbSelectNetType.SelectedItem.ToString() == "Линейный нейрон" || cbSelectNetType.SelectedItem.ToString() == "Персептрон")
            {
                label1.Enabled = false;
                tbNumLayers.Enabled = false;
                groupBox1.Enabled = false;
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                //if (cbSelectNetType.Text == "Многослойный персептрон")
                //{
                //    NeuralNetSettings.getInstance().NetType = cbSelectNetType.Text;
                //    NeuralNetSettings.getInstance().NumLayers = Convert.ToInt32(tbNumLayers.Text);
                //    NeuralNetSettings.getInstance().NumIn = Convert.ToInt32(tbNumIn.Text);

                    // передать параметры обучения
                    if (tbLearnRate.Text != "" && Convert.ToDouble(tbLearnRate.Text) > 0 && Convert.ToDouble(tbLearnRate.Text) < 1)
                        NeuralNetSettings.LearningRate = Convert.ToDouble(tbLearnRate.Text); // 
                    else
                    {
                        Exception exc = new Exception(String.Format("Неверно задана скорость обучения!"));

                        MessageBox.Show(exc.Message);
                        throw exc;
                    }
                    for (int i = 0; i < NeuralNetSettings.NumLayers; i++)
                    {
                        if (NeuralNetSettings.NumNeurons[i] == 0)
                        {
                            Exception exc = new Exception(String.Format("В слое {0} не указано количество нейронов!", i + 1));
                            MessageBox.Show(exc.Message);
                            throw exc;
                        }
                    }
                NeuralNetSettings.Epoch = Convert.ToInt32(tbNumEpochs.Text); // 
                NeuralNetSettings.Accuracy = Convert.ToDouble(tbAccuracy.Text);
                if (cbSelectNetType.SelectedItem.ToString() == "Многослойный персептрон" && cbTypeTask.Text == "Прогнозирование")
                {
                    
                    NeuralNetForm form1 = new NeuralNetForm(NeuralNetSettings);
                    form1.ShowDialog();
                }
                if (cbSelectNetType.SelectedItem.ToString() == "Многослойный персептрон" && cbTypeTask.Text == "Классификация")
                {
                    ClassificationForm form1 = new ClassificationForm(NeuralNetSettings);
                    form1.ShowDialog();
                }
            }
            catch(Exception exc)
            {
                MessageBox.Show("Ошибка при вводе данных!", exc.Message);
            }
            if (cbSelectNetType.Text == "Линейный нейрон")
            {
                NeuralNetSettings.NetType = cbSelectNetType.Text;
                NeuralNetSettings.NumIn = Convert.ToInt32(tbNumIn.Text);
                // передать параметры обучения
                NeuralNetSettings.LearningRate = Convert.ToDouble(tbLearnRate.Text); // 
                NeuralNetSettings.Epoch = Convert.ToInt32(tbNumEpochs.Text); // 
                NeuralNetSettings.Accuracy = Convert.ToDouble(tbAccuracy.Text);
                LinearNeuronForm lnForm = new LinearNeuronForm(NeuralNetSettings);
                lnForm.ShowDialog();
            }
            if (cbSelectNetType.Text == "Персептрон")
            {
                NeuralNetSettings.NetType = cbSelectNetType.Text;
                NeuralNetSettings.NumIn = Convert.ToInt32(tbNumIn.Text);
                // передать параметры обучения
                NeuralNetSettings.LearningRate = Convert.ToDouble(tbLearnRate.Text); // 
                NeuralNetSettings.Epoch = Convert.ToInt32(tbNumEpochs.Text); // 
                NeuralNetSettings.Accuracy = Convert.ToDouble(tbAccuracy.Text);
                NonLinearNeuronForm form = new NonLinearNeuronForm(NeuralNetSettings);
                form.ShowDialog();
            }
        }
        private void btnSetLayerType_Click(object sender, EventArgs e)
        {
            
            Random rand = new Random();
                NeuralNetSettings.ActFuncType[cbSelectLayer.SelectedIndex] = cbActFuncSelect.Text;
            try
            {
                
                if (Convert.ToInt32(tbNumNeurons.Text) <= 0)
                {
                    Exception exc = new Exception(String.Format("Число нейронов не может быть отрицательным или равно нулю!"));
                    MessageBox.Show(exc.Message);
                    throw exc;
                }
                else
                    NeuralNetSettings.NumNeurons[cbSelectLayer.SelectedIndex] = Convert.ToInt32(tbNumNeurons.Text);

                if ((Convert.ToDouble(tbLeftA.Text) > -1.0) && (Convert.ToDouble(tbRightB.Text) < 1.0) && (Convert.ToDouble(tbLeftA.Text) < Convert.ToDouble(tbRightB.Text)))
                {
                    NeuralNetSettings.LeftA[cbSelectLayer.SelectedIndex] = Convert.ToDouble(tbLeftA.Text);
                    NeuralNetSettings.RightB[cbSelectLayer.SelectedIndex] = Convert.ToDouble(tbRightB.Text);
                    // ****************************************************************************************** //
                    if (cbSelectLayer.SelectedIndex == 0)
                    {
                        NeuralNetSettings.W[cbSelectLayer.SelectedIndex] = new double[NeuralNetSettings.NumNeurons[0], NeuralNetSettings.NumIn];
                        NeuralNetSettings.Bias[cbSelectLayer.SelectedIndex] = new double[NeuralNetSettings.NumNeurons[0]];

                        for (int i = 0; i < NeuralNetSettings.NumNeurons[cbSelectLayer.SelectedIndex]; i++)
                        {
                            for (int j = 0; j < NeuralNetSettings.NumIn; j++)
                            {
                                {
                                   NeuralNetSettings.W[0][i, j] = rand.NextDouble() * (NeuralNetSettings.RightB[0] - NeuralNetSettings.LeftA[0]) + NeuralNetSettings.LeftA[0]; // при создании слоя его весовым коэффициентам присваиваются случайные значения из диапазона (-1;0)
                                }
                               NeuralNetSettings.Bias[0][i] = rand.NextDouble() * (NeuralNetSettings.RightB[0] - NeuralNetSettings.LeftA[0]) + NeuralNetSettings.LeftA[0]; // при создании слоя его весовым коэффициентам присваиваются случайные значения из диапазона (-1;0)
                            }
                        }
                    }
                    else
                    {
                        //NeuralNetSettings.W = new double[NeuralNetSettings.NumLayers][,];
                        //NeuralNetSettings.Bias = new double[NeuralNetSettings.NumLayers][];
                        NeuralNetSettings.W[cbSelectLayer.SelectedIndex] = new double[NeuralNetSettings.NumNeurons[cbSelectLayer.SelectedIndex], NeuralNetSettings.NumNeurons[cbSelectLayer.SelectedIndex - 1]];
                        NeuralNetSettings.Bias[cbSelectLayer.SelectedIndex] = new double[NeuralNetSettings.NumNeurons[cbSelectLayer.SelectedIndex]];
                        
                        for (int i = 0; i < NeuralNetSettings.NumNeurons[cbSelectLayer.SelectedIndex]; i++)
                        {
                            for (int j = 0; j < NeuralNetSettings.NumNeurons[cbSelectLayer.SelectedIndex - 1]; j++)
                            {
                                {
                                    NeuralNetSettings.W[cbSelectLayer.SelectedIndex][i, j] = rand.NextDouble() * (NeuralNetSettings.RightB[cbSelectLayer.SelectedIndex] - NeuralNetSettings.LeftA[cbSelectLayer.SelectedIndex]) + NeuralNetSettings.LeftA[cbSelectLayer.SelectedIndex]; // при создании слоя его весовым коэффициентам присваиваются случайные значения из диапазона (-1;0)
                                }
                                NeuralNetSettings.Bias[cbSelectLayer.SelectedIndex][i] = rand.NextDouble() * (NeuralNetSettings.RightB[cbSelectLayer.SelectedIndex] - NeuralNetSettings.LeftA[cbSelectLayer.SelectedIndex]) + NeuralNetSettings.LeftA[cbSelectLayer.SelectedIndex]; // при создании слоя его весовым коэффициентам присваиваются случайные значения из диапазона (-1;0)
                            }
                        }
                    }
                }
                else
                {
                    Exception exc = new Exception(String.Format("Неверный диапазон изменения начальных значений весовых коэффициентов!"));
                    MessageBox.Show(exc.Message);
                    throw exc;
                }
                switch (cbActFuncSelect.Text)
                {
                    case "Сигмоидальная":
                        {
                            if (Convert.ToDouble(tbSigmA.Text) != 0.0 && tbSigmA.Text != "")
                            {
                                NeuralNetSettings.SigmoidA[cbSelectLayer.SelectedIndex] = Convert.ToDouble(tbSigmA.Text);
                            }
                            else
                            {
                                Exception exc = new Exception(String.Format("Неверно задан параметр крутизны сигмоиды!"));
                                MessageBox.Show(exc.Message);
                                throw exc;
                            }
                            break;
                        }
                    case "Гиперболический тангенс":
                        {

                            if (Convert.ToDouble(tbTanB.Text) != 0.0 && tbTanB.Text != "")
                            {
                                NeuralNetSettings.TanA[cbSelectLayer.SelectedIndex] = Convert.ToDouble(tbTanA.Text);
                                NeuralNetSettings.TanB[cbSelectLayer.SelectedIndex] = Convert.ToDouble(tbTanB.Text);
                            }
                            else
                            {
                                Exception exc = new Exception(String.Format("Неверно заданы параметры гиперболического тангенса!"));
                                MessageBox.Show(exc.Message);
                                throw exc;
                            }
                            break;
                        }

                    default:
                        break;
                }
                
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void tbSigmA_TextChanged(object sender, EventArgs e)
        {
        }

        private void tbTanA_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;

            if (!(char.IsDigit(c) || c == '.' || c == '-' || c == '\b'))
            {
                e.Handled = true;
            }
            else
            {
                string text = (sender as TextBox).Text ?? "";

                if (c == '.' && text.Contains('.'))
                {
                    e.Handled = true;
                }
                else if (c == '-' && text.Contains('-'))
                {
                    e.Handled = true;
                }
            }

        }

        private void tbTanA_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            //Layer[] layers1 = new Layer[3];
            //layers1[0] = new Layer(2, 2, 0, 0.1);
            //for (int i = 1; i < 2; i++)
            //{
            //    layers1[i] = new Layer(2, 2, 0, 0.1);
            //}
            //layers1[2] = new Layer(2, 1, 0, 0.1);
            //double[] l0b = { 2, 2 };
            //layers1[0].Bias = l0b;
            //double[] l0x = { 0.1, 0.9 };
            //layers1[0].X = l0x;
            //double[,] l0w = { { -2, -2 }, { 3, 3 } };
            //layers1[0].W = l0w;
            //layers1[0].GetLayerOut(1);

            //double[] l1b = { 3, -2 };
            //layers1[1].Bias = l1b;
            //layers1[1].X = layers1[0].GetLayerOut(1);
            //double[,] l1w = { { -2, -4 }, { 2, 2 } };
            //layers1[1].W = l1w;
            //layers1[1].GetLayerOut(1);

            //layers1[2].W[0, 0] = 3;
            //layers1[2].W[0, 1] = 1;
            //layers1[2].Bias[0] = -2;
            //layers1[2].X = layers1[1].GetLayerOut(1);

            //layers1[2].GetLayerOut(1);
            //double[] d = { 0.9 };
            ////layers1[2].GetOutLayerError(d);
            ////layers1[2].GetHiddenLayerError(layers1[2].OutError);
            ////layers1[1].GetHiddenLayerError(layers1[2].OutHError);

            ////layers1[2].CalcWeightsOutLayer(0.8);
            //////            layers1[2].CalcWeightsHiddenLayer(0.8, layers1[2].OutError);
            //layers1[2].CalcWeightsOutLayer(0.8, d, 1);
            //layers1[1].CalcWeightsHiddenLayer(0.8, layers1[2].LocalGrad, 1);
            //layers1[0].CalcWeightsHiddenLayer(0.8, layers1[1].LocalGrad, 1);

        }

        private void сохранитьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "xml files (*.xml) | *.xml";
            saveFileDialog.RestoreDirectory = true;
            
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                NeuralNetSettings.LearningRate = Convert.ToDouble(tbLearnRate.Text);
                NeuralNetSettings.Epoch = Convert.ToInt32(tbNumEpochs.Text);
                NeuralNetSettings.Accuracy = Convert.ToDouble(tbAccuracy.Text);
                NeuralNetSettings.Weights = new DataSet();

                if (NeuralNetSettings.NetType == "Линейный нейрнон" || NeuralNetSettings.NetType == "Персептрон")
                {
                    DataTable[] arrTables = new DataTable[NeuralNetSettings.NumLayers];
                    arrTables[0] = new DataTable("Neuron");
                    DataColumn[][] c = new DataColumn[NeuralNetSettings.NumLayers][];
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
              
                }
                if (NeuralNetSettings.NetType == "Многослойный персептрон")
                {
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
 //               throw exc;
            }
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Xml.Serialization.XmlSerializer writer =
            new System.Xml.Serialization.XmlSerializer(typeof(AppSettings));

            OpenFileDialog openFile = new OpenFileDialog();
            openFile.InitialDirectory = Application.StartupPath;
            openFile.Filter = "xml files (*.xml) | *.xml";
            openFile.FilterIndex = 2;
            openFile.RestoreDirectory = true;

            if (openFile.ShowDialog() == DialogResult.OK)
            {

                System.IO.FileStream file = System.IO.File.Open(openFile.FileName, FileMode.Open);

                NeuralNetSettings = (AppSettings)writer.Deserialize(file);

                file.Close();
                // ***********************************************************************************
                if (NeuralNetSettings.NetType == "Персептрон" || NeuralNetSettings.NetType == "Линейный нейрон")
                {
                    NeuralNetSettings.W = new double[1][,];
                    NeuralNetSettings.W[0] = new double[1, NeuralNetSettings.NumIn];

                    for (int j = 0; j < NeuralNetSettings.NumIn; j++)
                    {
                        NeuralNetSettings.W[0][0, j] = Convert.ToDouble(NeuralNetSettings.Weights.Tables["Neuron"].Rows[0][j]);
                    }
                    cbSelectNetType.Text = NeuralNetSettings.NetType;
                    tbNumIn.Text = NeuralNetSettings.NumIn.ToString();

                    tbLearnRate.Text = NeuralNetSettings.LearningRate.ToString();
                    tbNumEpochs.Text = NeuralNetSettings.Epoch.ToString();
                    tbAccuracy.Text = NeuralNetSettings.Accuracy.ToString();
                }
                else
                {
                    NeuralNetSettings.W = new double[NeuralNetSettings.NumLayers][,];
                    NeuralNetSettings.Bias = new double[NeuralNetSettings.NumLayers][];
                    // ************************************************************************************
                    NeuralNetSettings.W[0] = new double[NeuralNetSettings.NumNeurons[0], NeuralNetSettings.NumIn];
                    NeuralNetSettings.Bias[0] = new double[NeuralNetSettings.NumNeurons[0]];

                    for (int i = 1; i < NeuralNetSettings.NumLayers; i++)
                    {
                        NeuralNetSettings.W[i] = new double[NeuralNetSettings.NumNeurons[i], NeuralNetSettings.NumNeurons[i - 1]];
                        NeuralNetSettings.Bias[i] = new double[NeuralNetSettings.NumNeurons[i]];
                    }
                    if (CultureInfo.CurrentUICulture.Name == "en-US")
                    {
                        for (int i = 0; i < NeuralNetSettings.NumNeurons[0]; i++)
                        {
                            NeuralNetSettings.Bias[0][i] = Convert.ToDouble(NeuralNetSettings.Biases.Tables["LayerBias1"].Rows[i][i]);
                            for (int j = 0; j < NeuralNetSettings.NumIn; j++)
                            {
                                NeuralNetSettings.W[0][i, j] = Convert.ToDouble(NeuralNetSettings.Weights.Tables["Layer1"].Rows[i][j]);
                            }
                        }
                        for (int i = 1; i < NeuralNetSettings.NumLayers; i++)
                        {
                            for (int j = 0; j < NeuralNetSettings.NumNeurons[i]; j++)
                            {
                                NeuralNetSettings.Bias[i][j] = Convert.ToDouble(NeuralNetSettings.Biases.Tables["LayerBias" + (i + 1)].Rows[j][j]);
                                for (int k = 0; k < NeuralNetSettings.NumNeurons[i - 1]; k++)
                                {
                                    NeuralNetSettings.W[i][j, k] = Convert.ToDouble(NeuralNetSettings.Weights.Tables["Layer" + (i + 1)].Rows[j][k]);
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < NeuralNetSettings.NumNeurons[0]; i++)
                        {
                            NeuralNetSettings.Bias[0][i] = Convert.ToDouble(NeuralNetSettings.Biases.Tables["LayerBias1"].Rows[i][i].ToString().Replace('.', ','));
                            for (int j = 0; j < NeuralNetSettings.NumIn; j++)
                            {
                                NeuralNetSettings.W[0][i, j] = Convert.ToDouble(NeuralNetSettings.Weights.Tables["Layer1"].Rows[i][j].ToString().Replace('.', ','));
                            }
                        }
                        for (int i = 1; i < NeuralNetSettings.NumLayers; i++)
                        {
                            for (int j = 0; j < NeuralNetSettings.NumNeurons[i]; j++)
                            {
                                NeuralNetSettings.Bias[i][j] = Convert.ToDouble(NeuralNetSettings.Biases.Tables["LayerBias" + (i + 1)].Rows[j][j].ToString().Replace('.', ','));
                                for (int k = 0; k < NeuralNetSettings.NumNeurons[i - 1]; k++)
                                {
                                    NeuralNetSettings.W[i][j, k] = Convert.ToDouble(NeuralNetSettings.Weights.Tables["Layer" + (i + 1)].Rows[j][k].ToString().Replace('.', ','));
                                }
                            }
                        }
                    }
                    // ************************************************************************************************
                    cbSelectNetType.Text = NeuralNetSettings.NetType;
                    tbNumLayers.Text = NeuralNetSettings.NumLayers.ToString();
                    tbNumIn.Text = NeuralNetSettings.NumIn.ToString();
                    for (int i = 0; i < NeuralNetSettings.NumLayers; i++)
                    {
                        cbSelectLayer.Items.Insert(i, "Слой " + (i + 1));
                    }
                    cbSelectLayer.Text = "Слой 1";
                    if (CultureInfo.CurrentUICulture.Name == "en-US")
                    {
                        tbNumNeurons.Text = NeuralNetSettings.NumNeurons[0].ToString();
                        tbLeftA.Text = NeuralNetSettings.LeftA[0].ToString().Replace(',', '.');
                        tbRightB.Text = NeuralNetSettings.RightB[0].ToString().Replace(',', '.');
                        cbActFuncSelect.Text = NeuralNetSettings.ActFuncType[0].ToString();
                        tbSigmA.Text = NeuralNetSettings.SigmoidA[0].ToString().Replace(',', '.');
                        tbTanA.Text = NeuralNetSettings.TanA[0].ToString().Replace(',', '.');
                        tbTanB.Text = NeuralNetSettings.TanB[0].ToString().Replace(',', '.');
                        tbLearnRate.Text = NeuralNetSettings.LearningRate.ToString().Replace(',', '.');
                        tbNumEpochs.Text = NeuralNetSettings.Epoch.ToString(); // 
                        tbAccuracy.Text = NeuralNetSettings.Accuracy.ToString().Replace(',', '.');
                    }
                    else
                    {
                        tbNumNeurons.Text = NeuralNetSettings.NumNeurons[0].ToString();
                        tbLeftA.Text = NeuralNetSettings.LeftA[0].ToString().Replace('.', ',');
                        tbRightB.Text = NeuralNetSettings.RightB[0].ToString().Replace('.', ',');
                        cbActFuncSelect.Text = NeuralNetSettings.ActFuncType[0].ToString();
                        tbSigmA.Text = NeuralNetSettings.SigmoidA[0].ToString().Replace('.', ',');
                        tbTanA.Text = NeuralNetSettings.TanA[0].ToString().Replace('.', ',');
                        tbTanB.Text = NeuralNetSettings.TanB[0].ToString().Replace('.', ',');
                        tbLearnRate.Text = NeuralNetSettings.LearningRate.ToString().Replace('.', ',');
                        tbNumEpochs.Text = NeuralNetSettings.Epoch.ToString(); // 
                        tbAccuracy.Text = NeuralNetSettings.Accuracy.ToString().Replace('.', ',');
                    }
                }
            }
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbSelectLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbNumNeurons.Text = NeuralNetSettings.NumNeurons[cbSelectLayer.SelectedIndex].ToString();
            //tbLeftA.Text = NeuralNetSettings.LeftA[cbSelectLayer.SelectedIndex].ToString();
            //tbRightB.Text = NeuralNetSettings.RightB[cbSelectLayer.SelectedIndex].ToString();
            //cbActFuncSelect.Text = NeuralNetSettings.ActFuncType[cbSelectLayer.SelectedIndex].ToString();
            //tbSigmA.Text = NeuralNetSettings.SigmoidA[cbSelectLayer.SelectedIndex].ToString();
            //tbTanA.Text = NeuralNetSettings.TanB[cbSelectLayer.SelectedIndex].ToString();
            //tbTanB.Text = NeuralNetSettings.TanB[cbSelectLayer.SelectedIndex].ToString();
        }

        private void создатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //if (cbSelectNetType.Text == "Многослойный персептрон")
                //{
                //    NeuralNetSettings.getInstance().NetType = cbSelectNetType.Text;
                //    NeuralNetSettings.getInstance().NumLayers = Convert.ToInt32(tbNumLayers.Text);
                //    NeuralNetSettings.getInstance().NumIn = Convert.ToInt32(tbNumIn.Text);

                // передать параметры обучения
                if (tbLearnRate.Text != "" && Convert.ToDouble(tbLearnRate.Text) > 0 && Convert.ToDouble(tbLearnRate.Text) < 1)
                    NeuralNetSettings.LearningRate = Convert.ToDouble(tbLearnRate.Text); // 
                else
                {
                    Exception exc = new Exception(String.Format("Неверно задана скорость обучения!"));

                    MessageBox.Show(exc.Message);
                    throw exc;
                }
                for (int i = 0; i < NeuralNetSettings.NumLayers; i++)
                {
                    if (NeuralNetSettings.NumNeurons[i] == 0)
                    {
                        Exception exc = new Exception(String.Format("В слое {0} не указано количество нейронов!", i + 1));
                        MessageBox.Show(exc.Message);
                        throw exc;
                    }
                }
                NeuralNetSettings.Epoch = Convert.ToInt32(tbNumEpochs.Text); // 
                NeuralNetSettings.Accuracy = Convert.ToDouble(tbAccuracy.Text);
                if (cbSelectNetType.SelectedItem.ToString() == "Многослойный персептрон" && cbTypeTask.Text == "Прогнозирование")
                {

                    NeuralNetForm form1 = new NeuralNetForm(NeuralNetSettings);
                    form1.ShowDialog();
                }
                if (cbSelectNetType.SelectedItem.ToString() == "Многослойный персептрон" && cbTypeTask.Text == "Классификация")
                {
                    ClassificationForm form1 = new ClassificationForm(NeuralNetSettings);
                    form1.ShowDialog();
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("Ошибка при вводе данных!", exc.Message);
            }
            if (cbSelectNetType.Text == "Линейный нейрон")
            {
                NeuralNetSettings.NetType = cbSelectNetType.Text;
                NeuralNetSettings.NumIn = Convert.ToInt32(tbNumIn.Text);
                // передать параметры обучения
                NeuralNetSettings.LearningRate = Convert.ToDouble(tbLearnRate.Text); // 
                NeuralNetSettings.Epoch = Convert.ToInt32(tbNumEpochs.Text); // 
                NeuralNetSettings.Accuracy = Convert.ToDouble(tbAccuracy.Text);
                LinearNeuronForm lnForm = new LinearNeuronForm(NeuralNetSettings);
                lnForm.ShowDialog();
            }
            if (cbSelectNetType.Text == "Персептрон")
            {
                NeuralNetSettings.NetType = cbSelectNetType.Text;
                NeuralNetSettings.NumIn = Convert.ToInt32(tbNumIn.Text);
                // передать параметры обучения
                NeuralNetSettings.LearningRate = Convert.ToDouble(tbLearnRate.Text); // 
                NeuralNetSettings.Epoch = Convert.ToInt32(tbNumEpochs.Text); // 
                NeuralNetSettings.Accuracy = Convert.ToDouble(tbAccuracy.Text);
                NonLinearNeuronForm form = new NonLinearNeuronForm(NeuralNetSettings);
                form.ShowDialog();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (CultureInfo.CurrentUICulture.Name == "en-US")
            {
                tbAccuracy.Text = "0.0";
                tbLearnRate.Text = "0.0";
                tbLeftA.Text = "0.0";
                tbRightB.Text = "0.0";
                tbSigmA.Text = "0.0";
                tbTanA.Text = "0.0";
                tbTanB.Text = "0.0";
            }
            else
            {
                tbAccuracy.Text = "0,0";
                tbLearnRate.Text = "0,0";
                tbLeftA.Text = "0,0";
                tbRightB.Text = "0,0";
                tbSigmA.Text = "0,0";
                tbTanA.Text = "0,0";
                tbTanB.Text = "0,0";
            }
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox about = new AboutBox();
            about.ShowDialog();
        }

        private void помощьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormHelp help = new FormHelp();
            help.Show();
        }
    }
    // статичный класс, который содержит настройки для нейронной сети.
    // Служит для обмена между формами и/или функциямиы
    //public static class NeuralNetSettings
    //{
    //    public static string netType; // тип нейронной сети  
    //    // ***********************************************************************************************
    //    // Параметры нейронной сети
    //    // ***********************************************************************************************
    //    public static string NetType { get; set; } // тип нейрнонной сети
    //    public static int NumLayers { get; set; } // число слоев нейронной сети
    //    public static int NumIn { get; set; } // число входов
    //    public static double[][,] W { get; set; } // массив массивов весовых коэффициентов нейронной сети
    //    public static double[][] X { get; set; }
    //    public static double[][] Y { get; set; }
    //    public static double[][] Bias { get; set; } // массисив массивов смещений сети
    //    public static int[] NumNeurons { get; set; } // массив, содержащий число нейроновдля каждого слоя
    //    public static string[] ActFuncType { get; set; } // тип активационной функции
    //    public static double[] SigmoidA { get; set; } // параметр крутизны сигмоиды
    //    public static double[] TanA { get; set; }
    //    public static double[] TanB { get; set; }
    //    public static double[] LeftA { get; set; } // левый предел диапазона изменения начальных значений весовых коэффициентов
    //    public static double[] RightB { get; set; } // ghfdsq предел диапазона изменения начальных значений весовых коэффициентов
    //    // **********************************************************************************************
    //    // Параметры обучения
    //    // **********************************************************************************************
    //    public static string PathIn { get; set; } // строка, которая содержит путь к файлу с входными данными
    //    public static string PathOut { get; set; } // путь к файлу с эталонами
    //    public static string PathTestIn { get; set; } // пусть к файлу с набором входных тестовых данных
    //    public static string PathTestOut { get; set; } // путь к выходным тестовым данным
    //    public static double LearningRate { get; set; } // скорость обучения
    //    public static int Epoch { get; set; } // число эпох обучения
    //    public static double MSE { get; set; } // среднеквадратическая ошибка
    //    public static double AvgMSE { get; set; }
    //    public static double Error { get; set; } // фактичская ошибка, разность между выходным значением сети и эталонным значением
    //    public static double Accuracy { get; set; } // точность обучения



    //}

    public class AppSettings
    {

        // ***********************************************************************************************
        // Параметры нейронной сети
        // ***********************************************************************************************
        public DataSet Weights { get; set; }
        public DataSet Biases { get; set; }
        public string NetType { get; set; } // тип нейрнонной сети
        public string WorkType { get; set; }
        public int NumLayers { get; set; } // число слоев нейронной сети
        public int NumIn { get; set; } // число входов
        [XmlIgnore]
        public double[][,] W { get; set; } // массив массивов весовых коэффициентов нейронной сети
        [XmlElement]

        [XmlIgnore]
        public double[][] X { get; set; }
        [XmlIgnore]
        public double[] AvgX { get; set; }
        public double[] AvgY { get; set; }
        public double[] Sigma { get; set; } // стандартное отклоение
        public double[] SigmaY { get; set; } // стандартное отклоение

        [XmlIgnore]
        public double[][] Y { get; set; }
        [XmlIgnore]
        public double[][] Bias { get; set; } // массисив массивов смещений сети
        public int[] NumNeurons { get; set; } // массив, содержащий число нейроновдля каждого слоя
        public string[] ActFuncType { get; set; } // тип активационной функции
        public double[] SigmoidA { get; set; } // параметр крутизны сигмоиды
        public double[] TanA { get; set; }
        public double[] TanB { get; set; }
        public double[] LeftA { get; set; } // левый предел диапазона изменения начальных значений весовых коэффициентов
        public double[] RightB { get; set; } // ghfdsq предел диапазона изменения начальных значений весовых коэффициентов
                                             // **********************************************************************************************
                                             // Параметры обучения
                                             // **********************************************************************************************

        [XmlIgnore]
        public string PathIn { get; set; } // строка, которая содержит путь к файлу с входными данными

        [XmlIgnore]
        public string PathOut { get; set; } // путь к файлу с эталонами

        [XmlIgnore]
        public string PathTestIn { get; set; } // пусть к файлу с набором входных тестовых данных

        [XmlIgnore]
        public string PathTestOut { get; set; } // путь к выходным тестовым данным

        public double LearningRate { get; set; } // скорость обучения
        public int Epoch { get; set; } // число эпох обучения
        [XmlIgnore]
        public double MSE { get; set; } // среднеквадратическая ошибка
        [XmlIgnore]
        public double AvgMSE { get; set; }
        [XmlIgnore]
        public double Error { get; set; } // фактичская ошибка, разность между выходным значением сети и эталонным значением
        public double Accuracy { get; set; } // точность обучения

        static AppSettings instance;

       AppSettings()
        {
    
        }
        public static AppSettings GetInstance()
        {
            if (instance == null)
                instance = new AppSettings();
            return instance;
        }
        
    }


}
