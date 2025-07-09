using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Npgsql;
using WMS_Class_Library;

namespace WMS_System;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    

    DatabaseConnection databaseConnection = new DatabaseConnection();
    public MainWindow()
    {
        InitializeComponent();
        // AdminWindow admin = new AdminWindow();
        // admin.Show();
    }
    private void Button_Click(object sender, EventArgs e)
    {
        try
        {
            databaseConnection.ReadAllData($"{query_database_textbox.Text}");
        }
        catch
        {
            Console.WriteLine("Empty Table Selected");
        }
    }

}