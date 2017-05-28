using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.InitialDirectory = "c:\\";
                openFileDialog1.Filter = "db files (*.db)|*.db|All files (*.*)|*.*";
                openFileDialog1.FilterIndex = 2;
                openFileDialog1.RestoreDirectory = true;
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    connect_To_Sql(openFileDialog1.FileName);
                }
                MessageBox.Show("Database has been connected");

            }
            else if (File.Exists(textBox1.Text))
            {
                connect_To_Sql(textBox1.Text);
                MessageBox.Show("Database has been connected");
            }
            else
            {
                TableProccessor.connection = DatabaseConnection.CreateEmptyDb(textBox1.Text);
                MessageBox.Show("Database has been created");
            }
        }

        private void connect_To_Sql(string dir)
        {
            TableProccessor.connection = new DatabaseConnection(dir);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog3 = new OpenFileDialog();
            openFileDialog3.InitialDirectory = "c:\\";
            openFileDialog3.Filter = "db files (*.db)|*.db|All files (*.*)|*.*";
            openFileDialog3.FilterIndex = 2;
            openFileDialog3.RestoreDirectory = true;
            if (openFileDialog3.ShowDialog() == DialogResult.OK)
            {
                connect_To_SqlMeta(openFileDialog3.FileName);
                MessageBox.Show("Connection succeeded");
                MetaDbFiller.LoadMetaDB();
            }
        }

        private void connect_To_SqlMeta(string dir)
        {
            MetaDbFiller.dbConnection = new DatabaseConnection(dir);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string ceq = textBox2.Text;
            Proccessor.LoadAndProccessCeq(ceq);
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
