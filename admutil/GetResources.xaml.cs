using System;
using System.Collections;
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
    /// Interaction logic for GetResources.xaml
    /// </summary>
    public partial class GetResources : Window
    {
        public GetResources()
        {
            InitializeComponent();
            foreach (Dictionary<string, dynamic> ADC in Globals.ADCs)
            {
                if (ADC["instance_mode"] == "Primary")
                {
                    ListViewItem ADCItem = new ListViewItem
                    {
                        Content = ADC["hostname"],
                        Tag = ADC["display_name"] + "," + ADC["ipv4_address"]
                    };
                    if (Globals.SelectedADCs.Contains(ADC["hostname"]))
                    {
                        ADCItem.IsSelected = true;
                    }
                    ADCList.Items.Add(ADCItem);
                }
            }
            foreach (Dictionary<string, dynamic> Group in Globals.Groups)
            {
                ListViewItem GroupItem = new ListViewItem
                {
                    Content = Group["name"]
                };
                GroupList.Items.Add(GroupItem);
            }
        }

        private void GetResources_Click(object sender, RoutedEventArgs e)
        {
            foreach (ListViewItem SelectedGroup in GroupList.SelectedItems)
            {
                foreach (Dictionary<string, dynamic> Group in Globals.Groups)
                {
                    if (SelectedGroup.Content == Group["name"])
                    {
                        string[] devices = Group["static_device_list"].Split(',');
                        foreach (ListViewItem ADCItem in ADCList.Items)
                        {
                            if (devices.Contains(ADCItem.Tag.ToString().Split(',')[0]) && !ADCItem.IsSelected)
                            {
                                ADCItem.IsSelected = true;
                            }
                        }
                    }
                }
            }
            if (ADCList.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select at least one ADC or device group");
            }
            else
            {
                Globals.ADCList = ADCList;
                Globals.NitroObjects = NitroObjects.Text.Split(',');
                this.Close();
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Globals.ADCList = null;
            this.Close();
        }
    }
}