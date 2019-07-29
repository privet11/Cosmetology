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
    public partial class Newitem : Form
    {
        public Newitem()
        {
            InitializeComponent();
            textBox1.KeyPress += textBox1_Fix;
        }

        double sum = 0;
        double sumMass = 0;
        double sumPrice = 0;
        bool radioButtonCheck=false;
        
        TextBox pricePerGram = new TextBox();
        GroupBox groupBox2 = new GroupBox();
        ListBox listBox2 = new ListBox();

        List<string> materialsList = new List<string>() { };
        List<double> pricesMat = new List<double>() { };
        List<double> countsMat = new List<double>() { };
        List<Material> m = new List<Material>() { };
        List<Recipe> r = new List<Recipe>() { };
        List<Material> selectedMaterials = new List<Material>() { };
        double[] fazaPercent = { 0,0,0};
        string faza;
        double[] fazes = new double[3];
        double procents = 0;
        List<string> materialsList2 = new List<string>() { };
        string[] materialsList3;

        
        public Newitem(double count1, Recipe selectedMaterials1)  //Конструктор, використовується під час копіювання рецептів
        {
            InitializeComponent();
            textBox5.Text = Convert.ToString(count1);
            textBox1.KeyPress += textBox1_Fix;
            tabControl1.SelectedIndex = 1;
            
            for (int i = 0; i < selectedMaterials1.materials.Count; i++)
            {
                procents = Math.Round((selectedMaterials1.countiMat[i] * 100) / selectedMaterials1.mass,3);
                dataGridView1.Rows.Add(selectedMaterials1.materials[i].name, selectedMaterials1.materials[i].price,
                    selectedMaterials1.materials[i].count, procents,selectedMaterials1.materials[i].faza/*, selectedMaterials1.faza[i]*/);
                selectedMaterials.Add(selectedMaterials1.materials[i]);
                pricesMat.Add(selectedMaterials1.materials[i].price);
                countsMat.Add(selectedMaterials1.materials[i].count);
              
            }
            for (int j = 0; j < 3; j++)
            {
                fazaPercent[j] = Math.Round(selectedMaterials1.fazaPercent[j],3);
            }
            label20.Text = Convert.ToString(selectedMaterials1.fazaPercent[0]);
            label21.Text = Convert.ToString(selectedMaterials1.fazaPercent[1]);
            label22.Text = Convert.ToString(selectedMaterials1.fazaPercent[2]);
        }

        private void Newitem_Load(object sender, EventArgs e)
        {
            TextBox mass = new TextBox();
            groupBox1.Hide();
            dataGridView1.Hide();
            comboBox1_SelectedIndexChanged(0, new EventArgs());
            
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }



        private void groupBox1_Enter(object sender, EventArgs e)
        {
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)  //Перехід між вкладками
        {
            
            materialsList1.Items.Clear();
            if (tabControl1.SelectedIndex==0)
            {
                groupBox1.Show();
                groupBox3.Hide();
                dataGridView1.Hide();
                this.Width = 301;
                button1.Show();
            }
            if (tabControl1.SelectedIndex == 1)
            {
                button1.Hide();
                groupBox1.Hide();
                groupBox3.Show();
                dataGridView1.Show();
                m = (Serialisation.GetList<Material>(Application.StartupPath + @"\people.json"));
                pricePerGram.Location = new Point(30, 150);
                for (int i = 0; i < m.Count; i++)
                {
                    materialsList1.Items.Add(m[i].name);
                }
                materialsList1.AutoCompleteMode = AutoCompleteMode.Suggest;
                materialsList1.AutoCompleteSource = AutoCompleteSource.CustomSource;
                materialsList1.SelectedIndex = 0;

                label3.Text = "Ціна за грам:";
                this.Width = 855;

            }
        }

        private void button1_Click(object sender, EventArgs e)  //Розрахунок матеріалу
        {
            if (textBox3.Text != String.Empty && textBox3.Text != String.Empty)
            {
                label3.Text = "";
                Material newMaterial = new Material(textBox2.Text, Convert.ToDouble(textBox3.Text), Convert.ToDouble(textBox4.Text), faza);
                pricePerGram.Text = newMaterial.pricePerGram.ToString();
                label24.Text += '\n' + pricePerGram.Text;
            }
        }

        private void button2_Click(object sender, EventArgs e)  //Збереження в файл
        {
            if (tabControl1.SelectedIndex == 0)
            {
                if (NameExist(textBox2, tabControl1.SelectedIndex)&&radioButtonCheck)
                {
                    DialogResult dialog = MessageBox.Show("Зберегти?","", MessageBoxButtons.OKCancel);
                    if (dialog == DialogResult.OK)
                    {
                        Material newMaterial = new Material(textBox2.Text, Convert.ToDouble(textBox3.Text), Convert.ToDouble(textBox4.Text), faza);
                        Serialisation.Serialise<Material>(newMaterial, Application.StartupPath + @"\people.json");
                        MessageBox.Show("Матеріал збережено!");
                    }
                }
                }
            if (tabControl1.SelectedIndex == 1)
            {
                if (NameExist(textBox7, tabControl1.SelectedIndex))
                {
                    DialogResult dialog = MessageBox.Show("Зберегти?", "", MessageBoxButtons.OKCancel);
                    if (dialog == DialogResult.OK)
                    {
                        Recipe newRecipe = new Recipe(textBox7.Text, textBox6.Value, Convert.ToDouble(textBox5.Text), 
                            Convert.ToDouble(label13.Text), selectedMaterials, countsMat, pricesMat, fazaPercent);
                        Serialisation.Serialise<Recipe>(newRecipe, Application.StartupPath + @"\recipe.json");
                        MessageBox.Show("Рецепт збережено!");
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)  //Перекидування вибраного матеріалу в список матеріалів рецепту
        {
            for (int i = 0; i < selectedMaterials.Count; i++)
            {
                if (selectedMaterials[i].name == materialsList1.Text)
                {
                    return;
                }
            }
            for (int i = 0; i < m.Count; i++)
            {
                if (materialsList1.Text == m[i].name)
                {
                    selectedMaterials.Add(m[i]);
                }
              
            }
            pricesMat.Add(Convert.ToDouble(pricelabel.Text));
            countsMat.Add(Convert.ToDouble(label1.Text));


            for (int i = 0; i < m.Count; i++)
            {
                if (m[i].name == materialsList1.Text)
                {
                    switch (m[i].faza)
                    {
                        case "Масляна":
                            fazaPercent[0] += Convert.ToDouble(textBox1.Text);
                            label20.Text = Convert.ToString(fazaPercent[0]);
                            faza = m[i].faza;
                            break;
                        case "Активна":
                            fazaPercent[1] += Convert.ToDouble(textBox1.Text);
                            label21.Text = Convert.ToString(fazaPercent[1]);
                            faza = m[i].faza;
                            break;
                        case "Водна":
                            fazaPercent[2] += Convert.ToDouble(textBox1.Text);
                            label22.Text = Convert.ToString(fazaPercent[2]);
                            faza = m[i].faza;
                            break;
                    }
                }

            }

            dataGridView1.Rows.Add(materialsList1.Text, pricelabel.Text, label1.Text, textBox1.Text, faza);

        }
        private void button4_Click(object sender, EventArgs e)  //Видалення вибраного матеріалу зі списку матеріалів рецепту
        {
           
            int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;
            if (Convert.ToString(dataGridView1[0, selectedRowIndex].Value) != String.Empty)
            {
                switch (selectedMaterials[selectedRowIndex].faza)
                {
                    
                    case "Масляна":
                        fazaPercent[0] -= Convert.ToDouble(dataGridView1[3, selectedRowIndex].Value);
                        label20.Text = Convert.ToString(fazaPercent[0]);
                        break;
                    case "Активна":
                        fazaPercent[1] -= Convert.ToDouble(dataGridView1[3, selectedRowIndex].Value);
                        label21.Text = Convert.ToString(fazaPercent[1]);
                        break;
                    case "Водна":
                        fazaPercent[2] -= Convert.ToDouble(dataGridView1[3, selectedRowIndex].Value);
                        label22.Text = Convert.ToString(fazaPercent[2]);
                        break;
                }
                selectedMaterials.RemoveAt(selectedRowIndex);
                dataGridView1.Rows.RemoveAt(selectedRowIndex);
                pricesMat.RemoveAt(selectedRowIndex);
                countsMat.RemoveAt(selectedRowIndex);
                label20.Text = Convert.ToString(Math.Round(fazaPercent[0], 3));
                label21.Text = Convert.ToString(Math.Round(fazaPercent[1], 3));
                label22.Text = Convert.ToString(Math.Round(fazaPercent[2], 3));
            
                textBox1.Text = "0";
            }
        }


        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            TextChanged();
        }

        private void textBox1_Fix(object sender, KeyPressEventArgs e)  //Перевірка текстбоксів
        {
            if (Char.IsDigit(e.KeyChar) || e.KeyChar == ',' || e.KeyChar == '\b')
            {
                return;
            }
            else
            {
                e.Handled = true;
            }
           
        }

        private void fazaFunc(Material mat)
        {
           
          
        }

        private void TextChanged()
        {
            m = (Serialisation.GetList<Material>(Application.StartupPath + @"\people.json"));
            pricelabel.Text = "0";
            if (textBox5.Text != "")
            {
                if (textBox1.Text.Length != 0)
                {
                    if (textBox1.Text == ",")
                    {
                        textBox1.Text = "0" + textBox1.Text;
                    }
                    label1.Text = Convert.ToString((Convert.ToDouble(textBox5.Text) * Convert.ToDouble(textBox1.Text)) / 100);


                    for (int i = 0; i < m.Count; i++)
                    {
                        if (materialsList1.Text == m[i].name)
                        {
                            label9.Text = Convert.ToString(Math.Round(m[i].pricePerGram,3));
                            pricelabel.Text = Convert.ToString(Math.Round(m[i].pricePerGram * Convert.ToDouble(label1.Text),3));
                        }
                    }
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        sum += Convert.ToDouble(dataGridView1[3, i].Value);

                    }
                    sum += Convert.ToDouble(textBox1.Text);
                    label12.Text = Convert.ToString(sum);
                    sum = 0;
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        sumMass += Convert.ToDouble(dataGridView1[2, i].Value);

                    }
                    sumMass += Convert.ToDouble(label1.Text);
                    label2.Text = Convert.ToString(Math.Round(sumMass,3));
                    sumMass = 0;
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        sumPrice += Convert.ToDouble(dataGridView1[1, i].Value);

                    }
                    
                    sumPrice += Convert.ToDouble(pricelabel.Text);
                    label13.Text = Convert.ToString(Math.Round(sumPrice,3));
                    sumPrice = 0;

                }
            }
        }

        private bool NameExist(TextBox name, int type)
        {
            if (type == 0)
            {
                m = (Serialisation.GetList<Material>(Application.StartupPath+@"\people.json"));
                for (int i = 0; i < m.Count; i++)
                {
                    if (name.Text == m[i].name)
                    {
                        MessageBox.Show("Назва вже існує");
                        return false;
                    }
                }
            }

            if (type == 1)
            {
                r = (Serialisation.GetList<Recipe>(Application.StartupPath + @"\recipe.json"));

                for (int i = 0; i < r.Count; i++)
                {
                    if (name.Text == r[i].name)
                    {
                        MessageBox.Show("Назва вже існує");
                        return false;
                    }
                }
            }
            return true;
        }


private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            materialsList1.Items.Clear();
            for (int i = 0; i < m.Count; i++)
            { 
                materialsList1.Items.Add(m[i].name);
            }

        }

        private void materialsList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            materialsList3 = new string[materialsList1.Items.Count];
            for (int i = 0; i < materialsList1.Items.Count; i++)
            {
                materialsList3[i] = Convert.ToString(materialsList1.Items[i]);
            }
            var values = new AutoCompleteStringCollection();
            values.AddRange(materialsList3);
            materialsList1.AutoCompleteCustomSource = values;
        }


        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked)
            {
                faza = "";
                faza += radioButton.Text;
                radioButtonCheck = true;
            }
        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void materialsList1_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void textBox6_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
    }
}
