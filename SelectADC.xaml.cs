using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace ADMUtil
{
    /// <summary>
    /// Interaction logic for SelectADC.xaml
    /// </summary>
    public partial class SelectADC : Window
    {
        public SelectADC()
        {
            InitializeComponent();
            Globals.SelectedADC = null;
            foreach (Dictionary<string, dynamic> ADC in Globals.ADCs)
            {
                ListViewItem ADCItem = new ListViewItem
                {
                    Content = ADC["hostname"],
                    Tag = ADC["display_name"] + "," + ADC["ipv4_address"]
                };
                ADCList.Items.Add(ADCItem);                
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (ADCList.SelectedItems.Count > 0)
            {
                Globals.SelectedADC = (ADCList.SelectedItem as ListViewItem).Content.ToString();
                this.Close();
            }
            else
            {
                MessageBox.Show("Please select an ADC.");
            }
        }
    }
}
