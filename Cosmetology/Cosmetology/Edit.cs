using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;


namespace Cosmetology
{
    public partial class Edit : Form
    {
        public Edit()
        {
            InitializeComponent();
            dataGridView1.Columns[1].DefaultCellStyle.Format = "dd/MMMM/yyyy";

        }
        List<Material> m = new List<Material>() { };
        List<Material> mn = new List<Material>() { };
        List<Recipe> r = new List<Recipe>() { };
        Recipe r1;
        double procents = 0;
        double sum = 0;
        int firstElem = 0;
        List<int> selectedRowIndex1 = new List<int>() { };
        string radioButtonCheck;
        decimal numericValue = 0;
        double firstPrice = 0;
        string materialPath = Application.StartupPath + @"\people.json";
        string recipePath = Application.StartupPath + @"\recipe.json";


        private void Edit_Load(object sender, EventArgs e)
        {
            dataGridView2.Hide();
            dataGridView1.Hide();
            panel1.Hide();
            comboBox1_SelectedIndexChanged(0, new EventArgs());
            button2.Hide();
        }

        private void dataGridView1_CellContentClick(object sender, EventArgs e) //Вивід інформації про рецепт
        {

            sum = 0;
            int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;
            if (Convert.ToString(dataGridView1[0, selectedRowIndex].Value) != String.Empty)
            {
                for (int i = 0; i < r.Count; i++)
                {
                    for (int j = 0; j < r.Count; j++)
                    {
                        if (r[j].name == Convert.ToString(dataGridView1[0, i].Value))
                        {
                            r1 = r[i];
                            r[i] = r[j];
                            r[j] = r1;
                        }
                    }
                }
                if (selectedRowIndex >= 0 && Convert.ToString(dataGridView1[0, selectedRowIndex].Value) != "")
                {
                    dataGridView4.Rows.Clear();
                    label5.Text = Convert.ToString(r[selectedRowIndex].price);
                    label3.Text = Convert.ToString(r[selectedRowIndex].date.ToLongDateString());
                    label2.Text = r[selectedRowIndex].name;
                    label7.Text = Convert.ToString(r[selectedRowIndex].mass);
                    label11.Text = "Масляна: " + Convert.ToString(r[selectedRowIndex].fazaPercent[0]) + "%";
                    label18.Text = "Активна: " + Convert.ToString(r[selectedRowIndex].fazaPercent[1]) + "%";
                    label19.Text = "Водна: " + Convert.ToString(r[selectedRowIndex].fazaPercent[2]) + "%";
                    for (int i = 0; i < r[selectedRowIndex].materials.Count; i++)
                    {
                        procents = (r[selectedRowIndex].countiMat[i] * 100) / r[selectedRowIndex].mass;
                        dataGridView4.Rows.Add(r[selectedRowIndex].materials[i].name, r[selectedRowIndex].priceiMat[i], r[selectedRowIndex].countiMat[i],
                            procents, r[selectedRowIndex].materials[i].faza);
                    }
                    panel1.Show();
                    this.Width = 1183;
                }

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGridView1.SelectedCells.Count; j++)
                    {
                        if (i == dataGridView1.SelectedCells[j].RowIndex)
                        {
                            sum += Convert.ToDouble(dataGridView1[3, i].Value);
                        }
                    }
                }
                label9.Text = Convert.ToString(sum) + " грн.";
                firstPrice = Convert.ToDouble(label5.Text);
                numericUpDown1.Hide();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)  //Вивід списку матеріалів
            {
                panel1.Hide();
                this.Width = 526;
                dataGridView2.Rows.Clear();
                dataGridView1.Hide();
                label9.Hide();
                label10.Hide();
                dataGridView2.Show();
                button2.Hide();
                m = (Serialisation.GetList<Material>(materialPath));
                for (int i = 0; i < m.Count; i++)
                {
                    dataGridView2.Rows.Add(m[i].name, m[i].price + " грн.", m[i].count + " гр.", m[i].pricePerGram + " грн.", m[i].faza);
                }
            }
            if (tabControl1.SelectedIndex == 1)  //Вивід списку рецептів
            {
                panel1.Hide();
                dataGridView1.Rows.Clear();
                dataGridView2.Hide();
                dataGridView1.Show();
                label9.Show();
                label10.Show();
                button2.Show();
                r = (Serialisation.GetList<Recipe>(recipePath));
                for (int i = 0; i < r.Count; i++)
                {
                    dataGridView1.Rows.Add(r[i].name, r[i].date, r[i].mass, r[i].price);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Видалити?", "", MessageBoxButtons.OKCancel);
            if (dialog == DialogResult.OK)
            {
                int count = 0;
                int counter = 0;
                if (tabControl1.SelectedIndex == 0)
                {

                    m = (Serialisation.GetList<Material>(materialPath));
                    firstElem = dataGridView2.SelectedCells[0].RowIndex;
                    count = dataGridView2.SelectedCells.Count;
                    do
                    {
                        dataGridView2.Rows.RemoveAt(dataGridView2.SelectedCells[0].RowIndex);
                        counter++;
                    }
                    while (dataGridView2.SelectedCells[0].RowIndex >= firstElem && counter < count);

                    File.WriteAllText(Application.StartupPath + @"\people.json", string.Empty);
                    for (int i = 0; i < m.Count; i++)
                    {
                        for (int j = 0; j < dataGridView2.Rows.Count; j++)
                        {
                            if (m[i].name == Convert.ToString(dataGridView2[0, j].Value))
                            {
                                Serialisation.Serialise<Material>(m[i], materialPath);
                            }
                        }
                    }
                    MessageBox.Show("Видалено");
                }
                if (tabControl1.SelectedIndex == 1)
                {
                    r = (Serialisation.GetList<Recipe>(recipePath));
                    firstElem = dataGridView1.SelectedCells[0].RowIndex;
                    if (Convert.ToString(dataGridView1[0, firstElem].Value) != "")
                    {
                        count = dataGridView1.SelectedCells.Count;
                        do
                        {
                            dataGridView1.Rows.RemoveAt(dataGridView1.SelectedCells[0].RowIndex);
                            counter++;
                        }
                        while (dataGridView1.SelectedCells[0].RowIndex >= firstElem && counter < count);


                        File.WriteAllText(recipePath, string.Empty);
                        for (int i = 0; i < r.Count; i++)
                        {
                            for (int j = 0; j < dataGridView1.Rows.Count; j++)
                            {
                                if (r[i].name == Convert.ToString(dataGridView1[0, j].Value))
                                {
                                    Serialisation.Serialise<Recipe>(r[i], recipePath);
                                }
                            }
                        }
                    }
                    MessageBox.Show("Видалено");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e) //Копіювання рецептів
        {
            int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;
            if (Convert.ToString(dataGridView1[0, selectedRowIndex].Value) != String.Empty)
            {
                Newitem f1 = new Newitem(r[selectedRowIndex].mass, r[selectedRowIndex]);
                f1.Show();
            }

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }



        private void radioButton1_CheckedChanged(object sender, EventArgs e) //Вибір ємностей
        {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked)
            {
                radioButtonCheck = radioButton.Text;
                numericUpDown1.Show();
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e) //Додавання вартостей ємностей
        {
            if (numericUpDown1.Value > numericValue)
            {
                if (radioButtonCheck == radioButton1.Text)
                    label5.Text = Convert.ToString(firstPrice + 2.5 * Convert.ToDouble(numericUpDown1.Value));
                if (radioButtonCheck == radioButton2.Text)
                    label5.Text = Convert.ToString(firstPrice + 5 * Convert.ToDouble(numericUpDown1.Value));
                if (radioButtonCheck == radioButton3.Text)
                    label5.Text = Convert.ToString(firstPrice + 8.55 * Convert.ToDouble(numericUpDown1.Value));
                numericValue = numericUpDown1.Value;
                return;
            }
            if (numericUpDown1.Value < numericValue)
            {
                if (radioButtonCheck == radioButton1.Text)
                    label5.Text = Convert.ToString(Convert.ToDouble(label5.Text) - 2.5);
                if (radioButtonCheck == radioButton2.Text)
                    label5.Text = Convert.ToString(Convert.ToDouble(label5.Text) - 5);
                if (radioButtonCheck == radioButton3.Text)
                    label5.Text = Convert.ToString(Convert.ToDouble(label5.Text) - 8.55);
                numericValue = numericUpDown1.Value;
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void FirstToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Newitem f1 = new Newitem();
            f1.Show();
        }

        private void materialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Application.StartupPath;
                openFileDialog.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    materialPath = openFileDialog.FileName;
                    
                    MessageBox.Show(materialPath);
                }
                comboBox1_SelectedIndexChanged(0, new EventArgs());
            }
        }

     

        private void InfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
              using (StreamReader sr = new StreamReader(Application.StartupPath + @"\info.txt",System.Text.Encoding.Default))
              {
                MessageBox.Show(sr.ReadToEnd());
              }
        }

        private void RecipeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Application.StartupPath;
                openFileDialog.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    recipePath = openFileDialog.FileName;

                    MessageBox.Show(recipePath);
                }
                comboBox1_SelectedIndexChanged(0, new EventArgs());
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }
    }
    
}
