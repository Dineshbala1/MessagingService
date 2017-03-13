using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
using MessagingServices_Mvvm_Sample.View;

namespace MessagingServices_Mvvm_Sample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Window newWindow;

        public MainWindow()
        {
            InitializeComponent();
            (listView.Items as INotifyCollectionChanged).CollectionChanged += MainWindow_CollectionChanged;
        }

        private void MainWindow_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            newWindow?.Close();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var studentView = new StudentControl();
            newWindow = new Window
            {
                Title = "Add new student",
                Content = studentView,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            newWindow.ShowDialog();
        }
    }
}
