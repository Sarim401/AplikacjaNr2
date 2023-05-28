using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using APkaaa;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        List<Danie> dostepnePrzepisy = new List<Danie>();
        List<string> listaProdukt = new List<string>();
        StringBuilder sb = new StringBuilder();
        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-NCH96Q8\SQLEXPRESS;Initial Catalog=Aplikacja;Integrated Security=True");

        private void OnDodajClick(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-NCH96Q8\SQLEXPRESS;Initial Catalog=Aplikacja;Integrated Security=True");
            // Tworzenie nowego okna
            Form noweOkno = new Form();

            // Ustawienie tytułu okna
            noweOkno.Text = "Dodaj Przepis";
            TextBox textBoxWartosci = new TextBox();

            // Ustawienie rozmiaru okna
            noweOkno.AutoSize = true;

            // Utworzenie opisu i pola tekstowego 1
            Label nazwaLabel1 = new Label();
            nazwaLabel1.Text = "Nazwa Dania:";
            nazwaLabel1.Location = new Point(10, 10);
            noweOkno.Controls.Add(nazwaLabel1);

            TextBox textBox1 = new TextBox();
            textBox1.Location = new Point(10, 40);
            textBox1.Width = 250;
            noweOkno.Controls.Add(textBox1);

            // Utworzenie opisu i pola tekstowego 2
            Label opisLabel2 = new Label();
            opisLabel2.Text = "Opis:";
            opisLabel2.Location = new Point(10, 70);
            noweOkno.Controls.Add(opisLabel2);

            TextBox textBox2 = new TextBox();
            textBox2.Location = new Point(10, 100);
            textBox2.Width = 250;
            noweOkno.Controls.Add(textBox2);

            // Utworzenie opisu i pola tekstowego 3
            Label produktyLabel3 = new Label();
            produktyLabel3.Text = "Podaj produkty:";
            produktyLabel3.Location = new Point(10, 130);
            noweOkno.Controls.Add(produktyLabel3);

            TextBox textBox3 = new TextBox();
            textBox3.Location = new Point(10, 160);
            textBox3.Width = 250;
            noweOkno.Controls.Add(textBox3);
            Button button3 = new Button();
            button3.Text = "Dodaj";
            button3.Location = new Point(textBox3.Right + 10, textBox3.Top);
            button3.Click += (s, ev) =>
            {
                if (!string.IsNullOrWhiteSpace(textBox3.Text))
                {
                    textBoxWartosci.Text += textBox3.Text + Environment.NewLine;
                    listaProdukt.Add(textBox3.Text);
                    textBox3.Text = "";
                }

            };
            noweOkno.Controls.Add(button3);

            Button buttonUsun = new Button();
            buttonUsun.Text = "Usuń";
            buttonUsun.Location = new Point(270, 220);
            buttonUsun.Click += (s, ev) =>
            {
                if (listaProdukt.Count > 0)
                {
                    listaProdukt.RemoveAt(listaProdukt.Count - 1);
                    textBoxWartosci.Clear();

                    foreach (string produkt in listaProdukt)
                    {
                        textBoxWartosci.AppendText(produkt + Environment.NewLine);
                    }
                }
            };
            noweOkno.Controls.Add(buttonUsun);

            Label OpisLabel = new Label();
            OpisLabel.Text = "Podgląd Produktów:";
            OpisLabel.Location = new Point(10, 190);
            noweOkno.Controls.Add(OpisLabel);
            textBoxWartosci.Multiline = true;
            textBoxWartosci.ScrollBars = ScrollBars.Vertical;
            textBoxWartosci.Location = new Point(10, 220);
            textBoxWartosci.Width = 250;
            textBoxWartosci.Height = 100;
            noweOkno.Controls.Add(textBoxWartosci);

            Button buttonDodajPrzepis = new Button();
            buttonDodajPrzepis.Text = "Dodaj Przepis";
            buttonDodajPrzepis.Location = new Point(10, 320);
            buttonDodajPrzepis.Click += (s, ev) =>
            {
                string nazwaDania = textBox1.Text;
                string opisDania = textBox2.Text;
                List<string> skladniki = new List<string>(listaProdukt);

                if (string.IsNullOrWhiteSpace(nazwaDania) || string.IsNullOrWhiteSpace(opisDania) || skladniki.Count == 0)
                {
                    MessageBox.Show("Uzupełnij wszystkie pola przed dodaniem przepisu.");
                    return;
                }
                sb.Clear();
                foreach (string skladnik in listaProdukt)
                {
                    sb.Append(skladnik);
                    sb.Append(" ");
                }
                string Skladniki = sb.ToString();

                String querry = $"INSERT INTO Potrawa (Danie, Opis, Skladniki) VALUES ('{nazwaDania}', '{opisDania}', '{Skladniki}')";
                SqlDataAdapter sda = new SqlDataAdapter(querry, conn);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                Danie noweDanie = new Danie(nazwaDania, opisDania, skladniki);
                dostepnePrzepisy.Add(noweDanie);
                textBoxWartosci.Clear();
                textBox1.Text = "";
                textBox2.Text = "";
                // Tutaj możesz wykorzystać noweDanie i zrobić coś z nim, na przykład dodać do innej listy itp.
                MessageBox.Show("Dodano pomyślnie");
                listaProdukt.Clear();
            };

            noweOkno.Controls.Add(buttonDodajPrzepis);

            // Wyświetlenie nowego okna
            noweOkno.ShowDialog();
        }

        private void OnPrzegladajClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(conn.ConnectionString))
            {
                // Zainicjuj ConnectionString
                conn.ConnectionString = @"Data Source=DESKTOP-NCH96Q8\SQLEXPRESS;Initial Catalog=Aplikacja;Integrated Security=True";
            }
            // Tworzenie nowego okna
            Form przegladajOkno = new Form();
            przegladajOkno.Text = "Przeglądaj przepisy";
            przegladajOkno.AutoSize = true;


            // Utworzenie ListBox do wyboru przepisu
            ListBox listBoxPrzepisy = new ListBox();
            listBoxPrzepisy.Location = new Point(10, 10);
            listBoxPrzepisy.Width = 250;
            listBoxPrzepisy.Height = 300;

            WczytajDaneZBazy();
            listBoxPrzepisy.Items.AddRange(dostepnePrzepisy.Select(przepis => przepis.Nazwa).ToArray());

            // Dodanie ListBox do okna

            //dostepnePrzepisy.Clear();
            //try
            //{
            //    using (conn)
            //    {
            //        conn.Open();
            //        string query = "SELECT Danie, Opis, Skladniki FROM Potrawa";

            //        SqlCommand command = new SqlCommand(query, conn);

            //        using (SqlDataReader reader = command.ExecuteReader())
            //        {
            //            while (reader.Read())
            //            {
            //                string nazwa = (string)reader["Danie"];
            //                string opis = (string)reader["Opis"];
            //                string skladniki = (string)reader["Skladniki"];
            //                List<string> listaSkladnikow = skladniki.Split(' ').ToList();

            //                Danie danie = new Danie(nazwa, opis, listaSkladnikow);
            //                dostepnePrzepisy.Add(danie);

            //                listBoxPrzepisy.Items.Add(nazwa);
            //            }
            //        }
            //    }

                // Dodanie ListBox do okna
                przegladajOkno.Controls.Add(listBoxPrzepisy);

                // Utworzenie TextBox do wyświetlania informacji o przepisie
                TextBox textBoxInformacje = new TextBox();
                textBoxInformacje.Location = new Point(listBoxPrzepisy.Right + 10, listBoxPrzepisy.Top);
                textBoxInformacje.Width = 250;
                textBoxInformacje.Height = 300;
                textBoxInformacje.Multiline = true;
                textBoxInformacje.ScrollBars = ScrollBars.Vertical;
                textBoxInformacje.ReadOnly = true;

                // Dodanie TextBox do okna
                przegladajOkno.Controls.Add(textBoxInformacje);

                // Dodanie obsługi zdarzenia po wyborze przepisu z ListBox
                listBoxPrzepisy.SelectedIndexChanged += (s, ev) =>
                {
                    int selectedIndex = listBoxPrzepisy.SelectedIndex;
                    if (selectedIndex >= 0)
                    {
                        Danie wybraneDanie = dostepnePrzepisy[selectedIndex];
                        textBoxInformacje.Text = "Nazwa: " + Environment.NewLine + wybraneDanie.Nazwa + Environment.NewLine +
                                                  "Opis: " + Environment.NewLine + wybraneDanie.Opis + Environment.NewLine +
                                                  "Składniki: " + Environment.NewLine + string.Join(", ", wybraneDanie.Skladniki);
                    }
                };

                // Wyświetlenie nowego okna
                przegladajOkno.ShowDialog();
            }
        //    catch (Exception ex)
        //    {
        //        // Obsłuż wyjątek
        //        MessageBox.Show("Wystąpił błąd podczas przeglądania przepisów: " + ex.Message);
        //    }
        //    finally
        //    {
        //        // Zamknij połączenie
        //        conn.Close();
        //    }
        //}

        private void OnUsunClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(conn.ConnectionString))
            {
                // Zainicjuj ConnectionString
                conn.ConnectionString = @"Data Source=DESKTOP-NCH96Q8\SQLEXPRESS;Initial Catalog=Aplikacja;Integrated Security=True";
            }
            WczytajDaneZBazy();
            // Tworzenie nowego okna
            Form usunOkno = new Form();
            usunOkno.Text = "Usuń Przepis";
            usunOkno.AutoSize = true;

            // Tworzenie listy dostępnych przepisów
            ListBox listBoxPrzepisy = new ListBox();
            listBoxPrzepisy.Items.AddRange(dostepnePrzepisy.Select(przepis => przepis.Nazwa).ToArray());
            listBoxPrzepisy.Location = new Point(10, 10);
            listBoxPrzepisy.Width = 200;
            listBoxPrzepisy.Height = 200;
            usunOkno.Controls.Add(listBoxPrzepisy);



            // Tworzenie przycisku "Usuń"
            Button buttonUsun = new Button();
            buttonUsun.Text = "Usuń";
            buttonUsun.Location = new Point(listBoxPrzepisy.Right + 10, listBoxPrzepisy.Top);
            buttonUsun.Click += (s, ev) =>
            {
                if (listBoxPrzepisy.SelectedItem != null)
                {
                    string nazwaPrzepisu = listBoxPrzepisy.SelectedItem.ToString();
                    Danie przepis = dostepnePrzepisy.FirstOrDefault(p => p.Nazwa == nazwaPrzepisu);
                    if (przepis != null)
                    {
                        dostepnePrzepisy.Remove(przepis);
                        listBoxPrzepisy.Items.Remove(nazwaPrzepisu);
                        UsunDanieZBazyDanych(przepis.Nazwa);
                    }
                }
            };
            usunOkno.Controls.Add(buttonUsun);

            // Wyświetlanie okna
            usunOkno.ShowDialog();
        }
        private void UsunDanieZBazyDanych(string NazwaDania)
        {
            using (SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-NCH96Q8\SQLEXPRESS;Initial Catalog=Aplikacja;Integrated Security=True"))
            {
                // Otwórz połączenie
                conn.Open();

                // Wykonaj zapytanie SQL
                string query = "DELETE FROM Potrawa WHERE Danie = @Nazwa";
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@Nazwa", NazwaDania);
                    command.ExecuteNonQuery();
                }

                // Zamknij połączenie
                conn.Close();
            }
        }
        private void WczytajDaneZBazy()
        {
            dostepnePrzepisy.Clear();
            try
            {
                using (conn)
                {
                    conn.Open();
                    string query = "SELECT Danie, Opis, Skladniki FROM Potrawa";

                    SqlCommand command = new SqlCommand(query, conn);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string nazwa = (string)reader["Danie"];
                            string opis = (string)reader["Opis"];
                            string skladniki = (string)reader["Skladniki"];
                            List<string> listaSkladnikow = skladniki.Split(' ').ToList();

                            Danie danie = new Danie(nazwa, opis, listaSkladnikow);
                            dostepnePrzepisy.Add(danie);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Obsłuż wyjątek
                MessageBox.Show("Wystąpił błąd podczas wczytywania przepisów: " + ex.Message);
            }
            finally
            {
                // Zamknij połączenie
                conn.Close();
            }
        }

    }
}
