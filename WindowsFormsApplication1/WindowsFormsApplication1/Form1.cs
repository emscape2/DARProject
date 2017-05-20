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
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void button2_Click_1(object sender, System.EventArgs e)
        {
            if (textBox2.Text == "")
            {

                Stream myStream = null;
                OpenFileDialog openFileDialog1 = new OpenFileDialog();

                openFileDialog1.InitialDirectory = "c:\\";
                openFileDialog1.Filter = "sql files (*.sql)|*.sql|All files (*.*)|*.*";
                openFileDialog1.FilterIndex = 2;
                openFileDialog1.RestoreDirectory = true;

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        if ((myStream = openFileDialog1.OpenFile()) != null)
                        {
                            using (myStream)
                            {
                                StreamReader reader = new StreamReader(myStream);
                                PreProcessor.ProcessTables(reader);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                    }
                }
            }
            else
            {
                TableProccessor.connection.QueryDatabase(textBox2.Text, false);
            }
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

            }
            else if (File.Exists(textBox1.Text))
            {
                connect_To_Sql(textBox1.Text);
            }
            else
            {
                TableProccessor.connection = DatabaseConnection.CreateEmptyDb(textBox1.Text);
            }
        }

        private void connect_To_Sql(string dir)
        {
            TableProccessor.connection = new DatabaseConnection(dir);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
