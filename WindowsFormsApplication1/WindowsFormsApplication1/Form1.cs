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

        /// <summary>
        /// either runs the sql statement in the chatbox or loads in the .sql statement file chosen from the windows file system
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click_1(object sender, System.EventArgs e)
        {
            //if text is empty open file dialog
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
            {//is not used for exercise but could come of use for testing purposes
                TableProccessor.connection.QueryDatabase(textBox2.Text, false);
            }
        }

        /// <summary>
        /// either creates an empty db or opens a db
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        private void button3_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// loads workload
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "All files (*.*)|*.*";
            openFileDialog1.RestoreDirectory = true; if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Stream myStream;
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            StreamReader reader = new StreamReader(myStream);
                            PreProcessor.ProccessWorkload(reader);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }


        }
        
        private void button4_Click(object sender, EventArgs e)
        {
            PreProcessor.LoadTableAndPreprocess();
            MessageBox.Show("Process succeeded");
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            string ceq = textBox4.Text;
            Proccessor.LoadAndProccessCeq(ceq);
        }

        private void connect_To_SqlMeta(string dir)
        {
            MetaDbFiller.dbConnection = new DatabaseConnection(dir);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (textBox5.Text == "")
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
                    MetaDbFiller.CreateMetaTables();
                    MetaDbFiller.FillMetaDb();
                    MessageBox.Show("MetaDB filling Process succeeded");
                }

            }
            else if (File.Exists(textBox5.Text))
            {
                connect_To_SqlMeta(textBox5.Text);
                MessageBox.Show("Connection succeeded");
                MetaDbFiller.LoadMetaDB();
                MessageBox.Show("MetaDB filling Process succeeded");
            }
            else
            {
                MetaDbFiller.dbConnection = DatabaseConnection.CreateEmptyDb(textBox5.Text);
                MessageBox.Show("New metaDB file made");
                MetaDbFiller.CreateMetaTables();
                MetaDbFiller.FillMetaDb();
                MessageBox.Show("MetaDB filling Process succeeded");
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
