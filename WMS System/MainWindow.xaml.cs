using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Npgsql;
using WMS_Class_Library;
using WMS_System.Views;


namespace WMS_System;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        MainContent.Content = new HomeView();
        // AdminWindow admin = new AdminWindow();
        // admin.Show();
    }
    private void Home_Click(object sender, RoutedEventArgs e)
    {
        MainContent.Content = new HomeView();
        FadeInContent();
    }

    private void Receive_Click(object sender, RoutedEventArgs e)
    {
        MainContent.Content = new ReceiveView();
        FadeInContent();
    }
    private void FadeInContent()
    {
        MainContent.Opacity = 0;
        var fadeIn = (Storyboard)this.Resources["FadeInStoryboard"];
        Storyboard.SetTarget(fadeIn, MainContent);
        fadeIn.Begin();
    }

    private void Transfer_Click(object sender, RoutedEventArgs e)
    {
        MainContent.Content = new TransferView();
        FadeInContent();
    }
    private void Order_Click(object sender, RoutedEventArgs e)
    {
        MainContent.Content = new OrderView();
        FadeInContent();
    }
}