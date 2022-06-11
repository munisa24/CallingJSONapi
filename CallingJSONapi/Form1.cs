using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Net;
using System.IO;


namespace CallingJSONapi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public string connectionString = (@"Data Source=DESKTOP-ON5DRGF;Initial Catalog=Jokes;Integrated Security=True");

        public string valueTextBox1 { get; private set; }
        public string valueTextBox2 { get; private set; }

        private void button1_Click(object sender, EventArgs e)
        {
            string valueTextBox1 = textBox1.Text.ToString();
            string valueTextBox2 = textBox2.Text.ToString();

            if (valueTextBox1 == "" && valueTextBox2 == "")
            {
                MessageBox.Show("Please enter some text");

                Application.Exit();
            }

            else { 
            WebRequest request = HttpWebRequest.Create("http://api.icndb.com/jokes/random?firstName=" + textBox1.Text + "&lastName=" + textBox2.Text);
            WebResponse response = request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());

            string Joke_JSON = reader.ReadToEnd();

            Joke myJoke = Newtonsoft.Json.JsonConvert.DeserializeObject<Joke>(Joke_JSON);
            string jokeValue = myJoke.value.joke;
            //MessageBox.Show(jokeValue);

            //string data = myJoke.value.ToString();
           
            MessageBox.Show("Value in data variable: " + jokeValue);
                //MessageBox.Show(valueTextBox1);

                try
                {

                    SqlConnection con = new SqlConnection(connectionString);
                    con.Open();
                    if (con.State == System.Data.ConnectionState.Open)
                    {

                        string query = "insert into [dbo].[jokes_test_table_2] (firstName, lastName, jokes) values ('" + valueTextBox1 + "','" + valueTextBox2 + "','" + myJoke.value.joke + "')";

                        SqlCommand cmd = new SqlCommand(query, con);

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Data submitted succesfully.");

                    }

                    con.Close();
                }

                catch (SqlException odbcEx)
                {
                    Console.Write("Error info:" + odbcEx.Message);
                }

            }

        }

    }

    public class Joke
    {
        public string type { get; set; }
        public Value value { get; set; }
    }

    public class Value
    {
        public int id { get; set; }
        public string joke { get; set; }
    }

}
