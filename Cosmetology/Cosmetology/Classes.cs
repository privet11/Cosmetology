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
    public partial class Classes : Form
    {
        public Classes()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Newitem f1 = new Newitem();
            f1.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Edit f2 = new Edit();
            f2.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
    [DataContract]
    public class Material
    {
        [DataMember]
        public string name;
        [DataMember]
        public double price = 0;
        [DataMember]
        public double count = 0;
        [DataMember]
        public string faza;
        [DataMember]
        public double pricePerGram = 0;
        public Material(string name1, double price1, double count1, string faza1)
        {
            name = name1;
            price = price1;
            count = count1;
            faza = faza1;
            pricePerGram = Math.Round(price / count,3);
        }

    }
    [DataContract]
    public class Recipe
    {
        [DataMember]
        public string name;
        [DataMember]
        public DateTime date=new DateTime();
        [DataMember]
        public double mass = 0;
        [DataMember]
        public double[] fazaPercent = new double[3];
        [DataMember]

        public List<Material> materials = new List<Material>() { };
        [DataMember]
        public List<double> countiMat = new List<double>() { };
        [DataMember]
        public List<double> priceiMat = new List<double>() { };
        [DataMember]
        public double price = 0;

        public Recipe(string name1, DateTime date1, double mass1,double price1, List<Material> materials1, List<double> countiMat1, List<double> priceiMat1, double[] fazaPercent1)
        {
            name = name1;
            date = date1;
            materials = materials1;
            countiMat = countiMat1;
            priceiMat = priceiMat1;
            mass = mass1;
            price = price1;
            fazaPercent = fazaPercent1;
        }


        public double priceOfRecipe(byte[] procents)
        {
            for (int i = 0; i < materials.Count; i++)
            {
                countiMat[i] = (mass * procents[i]);
                priceiMat[i] = materials[i].pricePerGram * countiMat[i];
                price += priceiMat[i];

            }
            return price;
        }
     
    }
    public class Serialisation
    {
        public static void Serialise<T>(T obj, string address)
        {
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(List<T>));
            using (FileStream fs = new FileStream(address, FileMode.OpenOrCreate))
            {
                if (fs.Length != 0)
                {
                    List<T> objects = (List<T>)jsonFormatter.ReadObject(fs);
                    objects.Add(obj);
                    fs.SetLength(0);
                    jsonFormatter.WriteObject(fs, objects);
                }
                else
                {
                    List<T> object1 = new List<T>() { obj };
                    jsonFormatter.WriteObject(fs, object1);
                }
            }

            
        }
        public static List<T> GetList<T>(string address) //Отримання списку об'єктів з файлу
        {
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(List<T>));
            List<T> objects=new List<T>() { };
            using (FileStream fs = new FileStream(address, FileMode.OpenOrCreate))
            {
                if (fs.Length != 0)
                {
                   objects = (List<T>)jsonFormatter.ReadObject(fs);
                }
                
            }
            return objects;
        }
        public static List<string> GetNames(List<Material> objects1) //Отримання списку імен з файлу
        {
            List<string> names= new List<string>() { };
            
            foreach (Material obj in objects1)
            {
                names.Add(obj.name);
            }

            return names;
        }
    }
}