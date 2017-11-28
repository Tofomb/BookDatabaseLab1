using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookDatabaseLab1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        SqlDataReader reader = null;
        SqlDataReader bookReader = null;
        private const string connectionString = (@"
Data Source=(localdb)\MSSQLLocalDB;
Initial Catalog=BookLibrary;
Integrated Security=True;
Connect Timeout=30;
Encrypt=False;
TrustServerCertificate=True;
ApplicationIntent=ReadWrite;
MultiSubnetFailover=False");

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadMainFrame(object sender, RoutedEventArgs e)
        {
            ReadAuthorTabel();

        }

        private void ReadAuthorTabel()
        {
            SqlConnection conn = new SqlConnection(connectionString);

            AuthorListBox.Items.Clear();
            string sqlQuery = "SELECT * FROM Author;";
            SqlCommand command = new SqlCommand(sqlQuery, conn);
            try
            {
                conn.Open();  // kan kasta exception
                reader = command.ExecuteReader();
            }
            catch (SqlException ex) { }

            while (reader.Read())
            {
                AuthorListBox.Items.Add(reader[1]);
                string authurString = "";
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    authurString += "," + reader[i];
                }
            }
        }

        private void ReadAndWriteBookTabel(string idToSelect = "")
        {

            SqlConnection conn = new SqlConnection(@"
Data Source=(localdb)\MSSQLLocalDB;
Initial Catalog=BookLibrary;
Integrated Security=True;
Connect Timeout=30;
Encrypt=False;
TrustServerCertificate=True;
ApplicationIntent=ReadWrite;
MultiSubnetFailover=False");

            BookListBox.Items.Clear();
            string sqlQuery = $"SELECT * FROM Books WHERE Id = '{idToSelect}'";
            SqlCommand command = new SqlCommand(sqlQuery, conn);
            try
            {
                conn.Open();  // kan kasta exception
                bookReader = command.ExecuteReader();
            }
            catch (SqlException ex) { }

            while (bookReader.Read())
            {
                BookListBox.Items.Add(bookReader["Title"]);
            }
        }

        private void CreateAuthorButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AuthorSelection(object sender, SelectionChangedEventArgs e)
        {
            if (AuthorListBox.SelectedIndex != -1)
            {
                DeleteAuthorButton.IsEnabled = true;
                EditAuthorButton.IsEnabled = true;
                CreateBookButton.IsEnabled = true;
                DeleteBookButton.IsEnabled = false;
                EditBookButton.IsEnabled = false;

                SqlConnection conn = new SqlConnection(@"
Data Source=(localdb)\MSSQLLocalDB;
Initial Catalog=BookLibrary;
Integrated Security=True;
Connect Timeout=30;
Encrypt=False;
TrustServerCertificate=True;
ApplicationIntent=ReadWrite;
MultiSubnetFailover=False");

                string SelectedAuthorId = "";
                string sqlQuery = "Select * FROM Author;";
                SqlCommand command = new SqlCommand(sqlQuery, conn);
                try
                {
                    conn.Open();  // kan kasta exception
                    reader = command.ExecuteReader();
                }
                catch (SqlException ex) { }

                int ii = 0;
                while (AuthorListBox.SelectedIndex >= ii)
                {
                    reader.Read();
                    ii++;
                    SelectedAuthorId = reader["Id"].ToString();
                    NameTextBlock.Text = reader["Name"].ToString();
                    BirthDateTextBlock.Text = reader["BirthDate"].ToString();
                    CountryTextBlock.Text = reader["Country"].ToString();
                }
                conn.Close();
                ReadAndWriteBookTabel(SelectedAuthorId);
            }
        }

        private void CreateAuthorButton_Click_1(object sender, RoutedEventArgs e)
        {

            DeleteAuthorButton.IsEnabled = false;
            EditAuthorButton.IsEnabled = false;
            CreateBookButton.IsEnabled = false;

            SqlConnection conn = new SqlConnection(@"
Data Source=(localdb)\MSSQLLocalDB;
Initial Catalog=BookLibrary;
Integrated Security=True;
Connect Timeout=30;
Encrypt=False;
TrustServerCertificate=True;
ApplicationIntent=ReadWrite;
MultiSubnetFailover=False");


            Guid guid = Guid.NewGuid();
            string sqlQuery = $"INSERT INTO Author (Id, Name, BirthDate, Country) VALUES ('{guid.ToString()}', '{NameTextBlock.Text}', '{BirthDateTextBlock.Text}', '{CountryTextBlock.Text}')";
            SqlCommand command = new SqlCommand(sqlQuery, conn);

            try
            {
                conn.Open();
                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {

            }
            ReadAuthorTabel();
        }

        private void DeleteAuthorButton_Click(object sender, RoutedEventArgs e)
        {


            //


            SqlConnection conn = new SqlConnection(@"
Data Source=(localdb)\MSSQLLocalDB;
Initial Catalog=BookLibrary;
Integrated Security=True;
Connect Timeout=30;
Encrypt=False;
TrustServerCertificate=True;
ApplicationIntent=ReadWrite;
MultiSubnetFailover=False");


            //
            string IdToDelete = "";
            string sqlQuery = "Select * FROM Author";
            SqlCommand command = new SqlCommand(sqlQuery, conn);
            try
            {
                conn.Open();  // kan kasta exception
                reader = command.ExecuteReader();
            }
            catch (SqlException ex) { }

            int ii = 0;
            while (AuthorListBox.SelectedIndex >= ii)
            {
                reader.Read();
                ii++;
            }
            IdToDelete = reader["Id"].ToString();
            conn.Close();
            //
            //

            //

            sqlQuery = $"Select Count (Id) FROM Books WHERE Id = '{IdToDelete}'";

            command = new SqlCommand(sqlQuery, conn);
            conn.Open();
            /*   try
               {
                   conn.Open();  // kan kasta exception
                   reader = command.ExecuteReader();
               }
               catch (SqlException ex) { }*/



            Int32 count = Convert.ToInt32(command.ExecuteScalar());
            conn.Close();

            if (count == 0)

            {

                //


                DeleteAuthorButton.IsEnabled = false;
                EditAuthorButton.IsEnabled = false;
                CreateBookButton.IsEnabled = false;

                //

                //



                sqlQuery = $"DELETE FROM Author WHERE Id = '{IdToDelete}'";
                command = new SqlCommand(sqlQuery, conn);
                try
                {
                    conn.Open();
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {

                }
                ReadAuthorTabel();
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Can't Delete Author With Books");
            }
        }

        private void EditAuthorButton_Click(object sender, RoutedEventArgs e)
        {

            DeleteAuthorButton.IsEnabled = false;
            EditAuthorButton.IsEnabled = false;
            CreateBookButton.IsEnabled = false;

            SqlConnection conn = new SqlConnection(@"
Data Source=(localdb)\MSSQLLocalDB;
Initial Catalog=BookLibrary;
Integrated Security=True;
Connect Timeout=30;
Encrypt=False;
TrustServerCertificate=True;
ApplicationIntent=ReadWrite;
MultiSubnetFailover=False");

            //
            string IdToUpdate = "";
            string sqlQuery = "Select * FROM Author;";
            SqlCommand command = new SqlCommand(sqlQuery, conn);
            try
            {
                conn.Open();  // kan kasta exception
                reader = command.ExecuteReader();
            }
            catch (SqlException ex) { }

            int ii = 0;
            while (AuthorListBox.SelectedIndex >= ii)
            {
                reader.Read();
                ii++;
            }
            IdToUpdate = reader["Id"].ToString();
            conn.Close();
            //

            sqlQuery = $"UPDATE Author SET Name = '{NameTextBlock.Text}', " +
                $"BirthDate = '{BirthDateTextBlock.Text}', " +
                $"Country = '{CountryTextBlock.Text}' " +
                $"WHERE Id = '{IdToUpdate}'";
            command = new SqlCommand(sqlQuery, conn);
            try
            {
                conn.Open();
                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {

            }

            ReadAuthorTabel();
        }

        private void BookSelection(object sender, SelectionChangedEventArgs e)
        {


            if (BookListBox.SelectedIndex != -1)
            {
                EditBookButton.IsEnabled = true;
                DeleteBookButton.IsEnabled = true;

                SqlConnection conn = new SqlConnection(@"
Data Source=(localdb)\MSSQLLocalDB;
Initial Catalog=BookLibrary;
Integrated Security=True;
Connect Timeout=30;
Encrypt=False;
TrustServerCertificate=True;
ApplicationIntent=ReadWrite;
MultiSubnetFailover=False");

                //
                string SelectedAuthorId = "";
                string sqlQuery = "Select * FROM Author;";
                SqlCommand command = new SqlCommand(sqlQuery, conn);
                try
                {
                    conn.Open();  // kan kasta exception
                    reader = command.ExecuteReader();
                }
                catch (SqlException ex) { }

                int ii = 0;
                while (AuthorListBox.SelectedIndex >= ii)
                {
                    reader.Read();
                    ii++;
                }

                SelectedAuthorId = reader["Id"].ToString();
                conn.Close();

                //

                sqlQuery = $"Select * FROM Books WHERE Id = '{SelectedAuthorId}'";
                command = new SqlCommand(sqlQuery, conn);
                try
                {
                    conn.Open();  // kan kasta exception
                    bookReader = command.ExecuteReader();
                }
                catch (SqlException ex) { }

                ii = 0;
                while (BookListBox.SelectedIndex >= ii)
                {
                    bookReader.Read();
                    ii++;
                }
                TitleTextBlock.Text = bookReader["Title"].ToString();
                ReleaseDateTextBlock.Text = bookReader["RealeseDate"].ToString();
                WordCountTextBlock.Text = bookReader["WordCount"].ToString();
                conn.Close();
            }

        }
        
        private void CreateBookButton_Click(object sender, RoutedEventArgs e)
        {

            EditBookButton.IsEnabled = false;
            DeleteBookButton.IsEnabled = false;

            SqlConnection conn = new SqlConnection(@"
Data Source=(localdb)\MSSQLLocalDB;
Initial Catalog=BookLibrary;
Integrated Security=True;
Connect Timeout=30;
Encrypt=False;
TrustServerCertificate=True;
ApplicationIntent=ReadWrite;
MultiSubnetFailover=False");

            string SelectedAuthorId = "";
            string sqlQuery = "Select * FROM Author;";
            SqlCommand command = new SqlCommand(sqlQuery, conn);
            try
            {
                conn.Open();  // kan kasta exception
                reader = command.ExecuteReader();
            }
            catch (SqlException ex) { }

            int ii = 0;
            while (AuthorListBox.SelectedIndex >= ii)
            {
                reader.Read();
                ii++;
            }

            SelectedAuthorId = reader["Id"].ToString();
            conn.Close();

            //

            sqlQuery = $"Select Count (Id) FROM Books WHERE Id = '{SelectedAuthorId}' AND Title = '{TitleTextBlock.Text}'";

            command = new SqlCommand(sqlQuery, conn);
            conn.Open();


            Int32 count = Convert.ToInt32(command.ExecuteScalar());
            conn.Close();

            if (count == 0)
            {

                //

                sqlQuery = $"INSERT INTO Books (Id, Title, RealeseDate, WordCount) VALUES ('{SelectedAuthorId}', '{TitleTextBlock.Text}', '{ReleaseDateTextBlock.Text}', '{WordCountTextBlock.Text}')";
                command = new SqlCommand(sqlQuery, conn);


                try
                {
                    conn.Open();
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {

                }

                ReadAndWriteBookTabel(SelectedAuthorId);
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("The Author Already Contains A Book With That Title");
            }


        }

        private void DeleteBookButton_Click(object sender, RoutedEventArgs e)
        {

            EditBookButton.IsEnabled = false;
            DeleteBookButton.IsEnabled = false;

            SqlConnection conn = new SqlConnection(@"
Data Source=(localdb)\MSSQLLocalDB;
Initial Catalog=BookLibrary;
Integrated Security=True;
Connect Timeout=30;
Encrypt=False;
TrustServerCertificate=True;
ApplicationIntent=ReadWrite;
MultiSubnetFailover=False");

            //
            string SelectedAuthorId = "";
            string sqlQuery = "Select * FROM Author;";
            SqlCommand command = new SqlCommand(sqlQuery, conn);
            try
            {
                conn.Open();  // kan kasta exception
                reader = command.ExecuteReader();
            }
            catch (SqlException ex) { }

            int ii = 0;
            while (AuthorListBox.SelectedIndex >= ii)
            {
                reader.Read();
                ii++;
            }

            SelectedAuthorId = reader["Id"].ToString();
            conn.Close();

            //

            /*   string IdToDelete = "";
               string sqlQuery = $"Select * FROM Books WHERE id = {}";
               SqlCommand command = new SqlCommand(sqlQuery, conn);
               try
               {
                   conn.Open();  // kan kasta exception
                   reader = command.ExecuteReader();
               }
               catch (SqlException ex) { }

               int ii = 0;
               while (BookListBox.SelectedIndex >= ii)
               {
                   reader.Read();
                   ii++;
               }
               IdToDelete = reader["Id"].ToString();
               conn.Close();
               */

            sqlQuery = $"DELETE FROM Books WHERE Id = '{SelectedAuthorId}' AND Title = '{BookListBox.SelectedItem}'";
            command = new SqlCommand(sqlQuery, conn);
            try
            {
                conn.Open();
                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {

            }
            ReadAndWriteBookTabel(SelectedAuthorId);
        }

        private void EditBookButton_Click(object sender, RoutedEventArgs e)
        {

            EditBookButton.IsEnabled = false;
            DeleteBookButton.IsEnabled = false;

            SqlConnection conn = new SqlConnection(@"
Data Source=(localdb)\MSSQLLocalDB;
Initial Catalog=BookLibrary;
Integrated Security=True;
Connect Timeout=30;
Encrypt=False;
TrustServerCertificate=True;
ApplicationIntent=ReadWrite;
MultiSubnetFailover=False");

            string IdToUpdate = "";
            string sqlQuery = "Select * FROM Author;";
            SqlCommand command = new SqlCommand(sqlQuery, conn);
            try
            {
                conn.Open();  // kan kasta exception
                reader = command.ExecuteReader();
            }
            catch (SqlException ex) { }

            int ii = 0;
            while (AuthorListBox.SelectedIndex >= ii)
            {
                reader.Read();
                ii++;
            }
            IdToUpdate = reader["Id"].ToString();
            conn.Close();

            //

            sqlQuery = $"Select Count (Id) FROM Books WHERE Id = '{IdToUpdate}' AND Title = '{TitleTextBlock.Text}'";

            command = new SqlCommand(sqlQuery, conn);
            conn.Open();

            Int32 count = Convert.ToInt32(command.ExecuteScalar());
            conn.Close();

            //  if (count == 0)
            // {

            //

            sqlQuery = $"UPDATE Books SET Title = '{TitleTextBlock.Text}', " +
            $"RealeseDate = '{ReleaseDateTextBlock.Text}', " +
            $"WordCount = '{WordCountTextBlock.Text}' " +
            $"WHERE Id = '{IdToUpdate}' AND Title = '{BookListBox.SelectedItem}'";

            command = new SqlCommand(sqlQuery, conn);
            try
            {
                conn.Open();
                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {

            }

            ReadAndWriteBookTabel(IdToUpdate);
            // }
            //  else
            // {
            //    MessageBoxResult result = MessageBox.Show("The Author Already Contains A Book With That Title");
            // }
        }



        // Focus Clear
        private void NameTextBlockFocus(object sender, RoutedEventArgs e)
        {
            NameTextBlock.Clear();
        }
        private void BirthDateTextBlockFocus(object sender, RoutedEventArgs e)
        {
            BirthDateTextBlock.Clear();
        }

        private void CountryTextBlockFocus(object sender, RoutedEventArgs e)
        {
            CountryTextBlock.Clear();
        }

        private void TitleTextBlockFocus(object sender, RoutedEventArgs e)
        {
            TitleTextBlock.Clear();
        }

        private void RealeaseDateTextBlockFocus(object sender, RoutedEventArgs e)
        {
            ReleaseDateTextBlock.Clear();
        }

        private void WordCountTextBlockFocus(object sender, RoutedEventArgs e)
        {
            WordCountTextBlock.Clear();
        }
    }

}
