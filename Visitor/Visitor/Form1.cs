using System;
using System.Windows.Forms;

namespace Visitor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            radioButton1.CheckedChanged += RadioButton1_CheckedChanged;
            radioButton2.CheckedChanged += RadioButton2_CheckedChanged;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string textToAdd = textBox1.Text;
            if (!string.IsNullOrWhiteSpace(textToAdd))
            {
                string scaleType = radioButton2.Checked ? "12-бальна" : "5-бальна";
                int min = radioButton2.Checked ? 0 : 0;
                int max = radioButton2.Checked ? 12 : 5;
                int num;
                if (int.TryParse(textToAdd, out num) && num >= min && num <= max)
                {
                    int convertedScore = radioButton2.Checked ? Convert.ToInt32(Math.Round(100.0 * num / 12)) : num * 20;
                    dataGridView1.Rows.Add(scaleType, textToAdd, convertedScore.ToString());
                    textBox1.Clear();
                }
                else
                {
                    MessageBox.Show($"Введіть число від {min} до {max}.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Введіть число для додавання.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                label1.Text = "Введіть число від 0 до 5:";
            }
        }

        private void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                label1.Text = "Введіть число від 0 до 12:";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            IVisitor visitor = new DataGridViewVisitor();
            Element element = new Element();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                element.Accept(visitor, row);
            }
        }
    }

    interface IVisitor
    {
        void Visit(DataGridViewRow row);
    }

    class Element
    {
        public void Accept(IVisitor visitor, DataGridViewRow row)
        {
            visitor.Visit(row);
        }
    }

    class DataGridViewVisitor : IVisitor
    {
        public void Visit(DataGridViewRow row)
        {
            string scaleType = row.Cells["Column1"].Value.ToString();
            int userScore = Convert.ToInt32(row.Cells["Column2"].Value);
            int convertedScore = ConvertToHundredPointScale(userScore, scaleType == "12-бальна");
            row.Cells["Column3"].Value = convertedScore;
        }

        private int ConvertToHundredPointScale(int num, bool isTwelvePointScale)
        {
            if (isTwelvePointScale)
            {
                return num + (int)Math.Round(88.0 * num / 12);
            }
            else
            {
                return num * 20;
            }
        }
    }
}
