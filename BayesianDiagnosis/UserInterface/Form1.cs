using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetworkParser;
using BayesianNetwork;

namespace UserInterface
{
    public partial class Form1 : Form
    {
        static Network network = Parser.parse("disease.xml");
        
        public Form1()
        {
            InitializeComponent();
        }

        void sortare(List<string> list)
        {
            list.Sort();
            list.Reverse();
        }

        void afisare(List<string> list)
        {
            foreach (string i in list)
            {
                Afisare.Items.Add(i);
            }
        }

        public void TestAskingInferentialQuestion(List<string> list)
        {
            List<Fact> evidence = new List<Fact>();
            int k = 0;
            foreach (string i in list)
            {
                evidence.Add(new Fact(list[k].Split('|')[0], list[k].Split('|')[1], network));
                k++;
            }
            foreach (string i in checkedListBox3.CheckedItems)
            {
                evidence.Add( new Fact(i, "true", network));
            }
            List<string> list_node = new List<string> {"cold","lung cancer","flu","pneumonia","asthma" };
            
            List<string> list_answer = new List<string>();
            foreach (string i in list_node)
            {
                Node disease = network.Nodes[i];
                Query question = new Query(disease, "true", evidence);
                list_answer.Add(string.Format("{0} => {1}", network.answer(question), disease.ToString()));
            }
            sortare(list_answer);
            afisare(list_answer);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Afisare.Items.Clear();
            List<CheckedListBox> checkbox_list = new List<CheckedListBox> { checkedListBox1, checkedListBox2, checkedListBox4 };
            string[] a={ label1.Text, label2.Text, label3.Text };
            List<string> list = new List<string>();
            int k = 0;
            foreach (CheckedListBox s in checkbox_list)
            {
                foreach (string i in s.CheckedItems)
                {
                    list.Add(string.Format("{0}|{1}",a[k],i));
                }
                k++;
            }
            afisare(list);
            TestAskingInferentialQuestion(list);
        }
    }
}
