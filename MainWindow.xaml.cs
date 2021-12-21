using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using ListView = System.Windows.Controls.ListView;
using ListViewItem = System.Windows.Controls.ListViewItem;
using MessageBox = System.Windows.MessageBox;
using TreeView = System.Windows.Controls.TreeView;

namespace ADMUtil
{
    internal static class Globals
    {
        public static string RegistryKey = "Software\\Citrix\\ADM";
        public static string ADMHost;
        public static string ADMUser;
        public static string ADMPass;
        public static string PuttyPath;
        public static string BaseURL;
        public static string HttpType = "https://";
        public static string TreeViewMouseButton = null;
        public static string SelectedADC = null;
        public static string[] NitroObjects = null;
        public static string[] PriorityColumns = new string[] { "name", "Host Name", "ipv46", "NSIP", "Instance", "Up Since", "monitorname", "username", "groupname", "ipaddress", "network", "gateway", "netmask", "policyname", "hostname", "source", "category", "severity", "entity", "message", "failureobj", "id", "intftype", "dnsprofilename", "certkey", "ciphergroupname", "description" };
        public static ListView ADCList = new ListView();
        public static System.Data.DataTable LogTable = new System.Data.DataTable();
        public static Dictionary<string, dynamic> NitroResults = new Dictionary<string, dynamic>();
        public static bool Continue = true;
        public static TreeViewItem SelectedNode = new TreeViewItem();
        public static TreeViewItem ADCNode = new TreeViewItem();
        public static TreeViewItem ClickedNode = new TreeViewItem();
        public static CookieContainer ADMCookie;
        public static ArrayList ADCs;
        public static ArrayList Datacenters;
        public static ArrayList Groups;

        public static Hashtable RegistryValues = new Hashtable()
        {
            { "ADMHost", "adm.domain.local" },
            { "ADMUser", "username" },
            { "PuttyPath", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile).ToString() + "\\Downloads" }
        };

        public static Hashtable ADCNodeStructure = new Hashtable()
        {
            { "AppExpert", new Hashtable(){
                { "Rewrites", new Hashtable(){
                    {"Rewrite Policies", "rewritepolicy" },
                    {"Rewrite Actions", "rewriteaction" } }
                },
                { "Responders", new Hashtable(){
                    {"Responder Policies", "responderpolicy" },
                    {"Responder Actions", "responderaction" } }
                },
                }
            },
            { "System", new Hashtable(){
                { "Settings", new Hashtable(){
                    { "Modes", "nsmode" },
                    { "Features", "nsfeature" },
                    { "HTTP Params", "nshttpparam" },
                    { "System Params", "systemparameter" } }
                },
                { "User Administration", new Hashtable(){
                    { "Users", "systemuser"},
                    { "Groups", "systemgroup" },
                    { "Command Policies", "systemcmdpolicy" }}
                },
                { "Auditing", new Hashtable(){
                    { "Message Actions", "auditmessageaction" },
                    { "Syslog Policies", "auditsyslogpolicy" },
                    { "Syslog Servers", "auditsyslogaction" } }
                },
                { "Networking", new Hashtable(){
                    { "IP Addresses", "nsip"},
                    { "Interfaces", "Interface" },
                    { "VLANs", "vlan" },
                    { "Routes", "route" },
                    { "PBRs", "nspbr" } }
                },
                { "Profiles", new Hashtable(){
                    { "TCP Profiles", "nstcpprofile" },
                    { "HTTP Profiles", "nshttpprofile" },
                    { "SSL Profiles", "sslprofile" },
                    { "DTLS Profiles", "ssldtlsprofile" },
                    { "DNS Profiles", "dnsprofile" },
                    { "ICA Profiles", "nsicapprofile" },
                    { "Analytics Profiles", "analyticsprofile" } }
                } }
            },
            { "Security", new Hashtable() {
                { "AAA Application Traffic", new Hashtable(){
                    { "Authentication Policies", "authenticationpolicy" },
                    { "Authentication Labels", "authenticationpolicylabel" },
                    { "Authentication Profiles", "authenticationauthnprofile" },
                    { "LDAP Actions", "authenticationldapaction" },
                    { "nFactor Policies", "authenticationloginschemapolicy" },
                    { "nFactor Profiles", "authenticationloginschema" },
                    { "SAML IDP Policies", "authenticationsamlidppolicy" },
                    { "SAML IDP Profiles", "authenticationsamlidpprofile" },
                    { "Virtual Servers", "authenticationvserver"} }
                } }
            },
            { "Traffic Management", new Hashtable(){
                { "Load Balancing", new Hashtable(){
                    { "Virtual Servers", "lbvserver" },
                    { "Services", "service" },
                    { "Servers", "server" },
                    { "Monitors", "lbmonitor" } }
                },
                { "Content Switching", new Hashtable(){
                    { "Virtual Servers", "csvserver" },
                    { "Policies", "cspolicy" },
                    { "Actions", "csaction" } }
                },
                { "DNS", new Hashtable(){
                    {"DNS Policies", "dnspolicy"},
                    {"Name Servers", "dnsnameserver" },
                    {"DNS Suffixes", "dnssuffix" },
                    {"DNS Actions","dnsaction" } }
                },
                { "SSL", new Hashtable(){
                    { "Certificates", "sslcertkey" },
                    { "Key Files", "sslkeyfiles" },
                    {"CSRs", "sslcsrfiles"},
                    {"Cert Files", "sslcertfiles" },
                    {"Cipher Groups", "sslcipher" } }
                } }
            },
            { "Gateway", new Hashtable(){
                { "VPN Services", new Hashtable(){
                    { "Virtual Servers", "vpnvserver" },
                    { "VPN Parameters", "nsconfig" },
                    { "Secure Ticket Authority", "vpnglobal_staserver_binding" },
                    { "Portal Themes", "vpnportaltheme" },
                    { "Session Policies", "vpnsessionpolicy" },
                    { "Session Profiles", "vpnsessionaction" } }
                },
                { "User Administration", new Hashtable(){
                    { "AAA Users", "aaauser" },
                    { "AAA Groups", "aaagroup" } }
                },
                { "ICA Policies", new Hashtable(){
                    { "ICA Policies", "icapolicy" },
                    { "ICA Actions", "icaaction" },
                    { "ICA Access Profiles", "icaaccessprofile" } }
                },
                { "Authentication Policies", new Hashtable(){
                    { "LDAP Policies", "authenticationldappolicy" },
                    { "LDAP Actions", "authenticationldapaction" },
                    { "RADIUS Policies", "authenticationradiuspolicy" },
                    { "RADIUS Actions", "authenticationradiusaction" },
                    { "SAML Servers", "authenticationsamlidpprofile" },
                    { "Preauthentication", "aaapreauthenticationpolicy" },
                    { "Authorization", "authorizationpolicy" } }
                }
            }}
        };

        public static Hashtable TreeStructure = new Hashtable() {
            { "Inventory", new Hashtable(){
                { "Datacenters", "mps_datacenter,id,ns" },
                { "Device Groups", "device_group,id,ns" },
                { "Events", "ns_event" } }
            },
        };

        public static Hashtable ADCDetail = new Hashtable() {
            { "Host Name", "hostname" },
            { "Instance", "display_name" },
            { "NSIP", "mgmt_ip_address" },
            { "Serial", "encoded_serialnumber" },
            { "State", "instance_state" },
            { "Up Since", "upsince" },
            { "Firmware", "version" },
            { "Model", "system_hardwareversion" },
            { "HA Status", "instance_mode" }
        };

        public static Hashtable GroupDetail = new Hashtable() {
            { "Name", "name" },
            { "Instances", "static_device_list" },
            { "Type", "device_family" },
            { "ID", "id" }
        };

        public static Hashtable DatacenterDetail = new Hashtable() {
            { "Name", "name" },
            { "City", "city" },
            { "Country", "country" },
            { "Region", "region" },
            { "ZIP","zipcode" },
            { "Latitude", "latitude"},
            { "Longitude", "longitude" }
        };

        internal static Brush BadColor = Brushes.Tomato;
        internal static Brush WarnColor = Brushes.Orange;
        internal static Brush GoodColor = Brushes.LimeGreen;
        internal static Brush SecondaryColor = Brushes.LightBlue;
        public static Hashtable ExcelData { get; internal set; }
        public static string ExcelFile { get; internal set; }
        public static System.Data.DataTable DataGridTable { get; internal set; }
        public static ArrayList ArrayData { get; internal set; }
        public static Hashtable Columns { get; internal set; }
        public static Hashtable Filters { get; internal set; }
        public static Hashtable NodeTag { get; internal set; }
        public static dynamic ADCNodeHeader { get; internal set; }
        public static TreeViewItem ParentNode { get; internal set; }
        public static string TestResponse { get; internal set; }
        public static string version { get; internal set; }
        public static ArrayList Devices { get; internal set; }
        public static ArrayList EventSummary { get; internal set; }
        public static List<string> SelectedADCs { get; internal set; }
    }

    public partial class MainWindow : System.Windows.Window
    {
        private readonly BackgroundWorker ExcelWorker = new BackgroundWorker();
        private readonly BackgroundWorker DataGridWorker = new BackgroundWorker();

        private void ADMLogin()
        {
            if (Globals.ADMPass == null)
            {
                WriteLog("Info", "Getting login information");
                Authenticate AuthWindow = new Authenticate();
                AuthWindow.ShowDialog();
                Globals.BaseURL = (Globals.HttpType + Globals.ADMHost + "/nitro/v1");
            }
            if (Globals.Continue == true)
            {
                try
                {
                    WriteLog("Info", "Logging on to " + Globals.ADMHost + " with " + Globals.ADMUser);
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(Globals.BaseURL + "/config/login");
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";
                    httpWebRequest.CookieContainer = new CookieContainer();
                    var LoginPayload = new
                    {
                        login = new[] {
                        new
                        {
                            username = Globals.ADMUser ,
                            password = Globals.ADMPass
                        }
                    }
                    };
                    JavaScriptSerializer javascriptSerializer = new JavaScriptSerializer();
                    string json = "object=" + javascriptSerializer.Serialize(LoginPayload);
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        streamWriter.Write(json);
                    }
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                    }
                    for (int i = 0; i < httpResponse.Headers.Count; i++)
                    {
                        string name = httpResponse.Headers.GetKey(i);
                        if (name != "Set-Cookie")
                            continue;
                        string value = httpResponse.Headers.Get(i);
                        foreach (var singleCookie in value.Split(','))
                        {
                            Match match = Regex.Match(singleCookie, "(.+?)=(.+?);");
                            if (match.Captures.Count == 0)
                                continue;
                            httpResponse.Cookies.Add(
                                new Cookie(
                                    match.Groups[1].ToString(),
                                    match.Groups[2].ToString(),
                                    "/",
                                    httpWebRequest.Host.Split(':')[0]));
                        }
                    }
                    CookieCollection ADMCookies = httpResponse.Cookies;
                    CookieContainer ADMCookie = new CookieContainer();
                    foreach (Cookie Cookie in ADMCookies)
                    {
                        ADMCookie.Add(Cookie);
                    }
                    Globals.ADMCookie = ADMCookie;
                }
                catch (Exception LoginException)
                {
                    MessageBox.Show(LoginException.Message, "Login Failed:");
                }
                if (TestConnection() == false)
                {
                    WriteLog("Error", "Login failed for " + Globals.ADMHost + " using " + Globals.ADMUser);
                    MessageBox.Show(Globals.TestResponse, "Login Failed:");
                    Globals.ADMPass = null;
                    ADMLogin();
                }
            }
            else
            {
                this.Close();
            }
        }

        private void StartADMUtil()
        {
            DataRow LogLine = Globals.LogTable.NewRow();
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            string TimeStamp = TimeZoneInfo.ConvertTime(DateTime.Now, timeZone).ToString("HH:mm:ss");
            LogLine["Time"] = TimeStamp;
            LogLine["Type"] = "Info";
            LogLine["Message"] = "Starting ADMUtil";
            Globals.LogTable.Rows.Add(LogLine);
            LogGrid.DataContext = Globals.LogTable;
            LoadSettings();
            DetailGrid.RowBackground = Brushes.Black;
            WriteLog("Info", "Checking " + Globals.RegistryKey + " for saved settings");
            RegistryKey ADMKey = Registry.CurrentUser.OpenSubKey(Globals.RegistryKey, true);
            Globals.ADMHost = ADMKey.GetValue("ADMHost").ToString();
            Globals.ADMUser = ADMKey.GetValue("ADMUser").ToString();
            Globals.PuttyPath = ADMKey.GetValue("PuttyPath").ToString();
            ADMKey.Close();
            ADMLogin();
            RefreshADMUtil();
        }

        private void LoadSettings()
        {
            RegistryKey ADMKey = Registry.CurrentUser.OpenSubKey(Globals.RegistryKey, true);
            if (ADMKey == null)
            {
                RegistryKey NewADMKey;
                NewADMKey = Registry.CurrentUser.CreateSubKey(Globals.RegistryKey);
                ADMKey = NewADMKey;
            }
            ADMKey.Close();
            ADMKey = Registry.CurrentUser.OpenSubKey(Globals.RegistryKey, true);
            foreach (DictionaryEntry RegValue in Globals.RegistryValues)
            {
                try
                {
                    var RegistryValue = ADMKey.GetValue(RegValue.Key.ToString());
                    if (RegistryValue == null)
                    {
                        ADMKey.SetValue(RegValue.Key.ToString(), Globals.RegistryValues[RegValue.Key].ToString());
                    }
                }
                catch
                {
                    WriteLog("Error", "Unable to set " + RegValue.Key.ToString() + " in " + Globals.RegistryKey);
                }
            }
            foreach (string subKey in ADMKey.GetSubKeyNames())
            {
                Globals.RegistryValues[subKey] = ADMKey.GetValue(subKey);
            }
            ADMKey.Close();
        }

        private void PopulateTreeView()
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            foreach (DictionaryEntry TreeNode in Globals.TreeStructure)
            {
                TreeViewItem RootNode = new TreeViewItem
                {
                    Header = TreeNode.Key.ToString(),
                    Foreground = Brushes.WhiteSmoke,
                    FontWeight = FontWeights.Bold,
                    FontSize = 16
                };
                Hashtable TreeNodes = TreeNode.Value as Hashtable;
                foreach (DictionaryEntry RootItems in TreeNodes)
                {
                    if (TreeNode.Key.ToString() == "Inventory")
                    {
                        TreeViewItem RootNodeItem = new TreeViewItem
                        {
                            Header = RootItems.Key.ToString(),
                            Foreground = Brushes.WhiteSmoke,
                            FontWeight = FontWeights.Bold,
                            FontSize = 15
                        };
                        ArrayList RootItem = new ArrayList();

                        switch (RootItems.Key.ToString())
                        {
                            case "Datacenters":
                                RootItem = Globals.Datacenters;
                                break;

                            case "Device Groups":
                                RootItem = Globals.Groups;
                                break;

                            case "Events":
                                foreach (Dictionary<string, dynamic> EventSummary in Globals.EventSummary)
                                {
                                    TreeViewItem EventNode = new TreeViewItem();
                                    switch (EventSummary["severity"])
                                    {
                                        case "Critical":
                                            EventNode.Foreground = Globals.BadColor;
                                            break;

                                        case "Major":
                                            EventNode.Foreground = Globals.WarnColor;
                                            break;

                                        case "Minor":
                                            EventNode.Foreground = Brushes.Yellow;
                                            break;

                                        case "Clear":
                                            EventNode.Foreground = Globals.GoodColor;
                                            break;
                                    }
                                    EventNode.Tag = EventSummary["severity"];
                                    EventNode.FontSize = 14;
                                    EventNode.Header = EventSummary["severity"] + " (" + EventSummary["total_count"] + ")";
                                    RootNodeItem.Items.Add(EventNode);
                                    RootNodeItem.Items.IsLiveSorting = true;
                                    RootNodeItem.Items.SortDescriptions.Add(new SortDescription("Header", ListSortDirection.Ascending));
                                    RootNodeItem.IsExpanded = true;
                                }
                                break;
                        }
                        foreach (Dictionary<string, dynamic> TreeItem in RootItem)
                        {
                            if (RootItems.Key.ToString() != "Events")
                            {
                                if (RootItems.Value.ToString().Split(',')[2].Length > 0)
                                {
                                    TreeViewItem ChildItem = new TreeViewItem
                                    {
                                        Header = TreeItem["name"],
                                        Tag = TreeItem[RootItems.Value.ToString().Split(',')[1]],
                                        Foreground = Brushes.WhiteSmoke,
                                        FontWeight = FontWeights.Normal,
                                        FontSize = 14
                                    };
                                    if (RootItems.Value.ToString().Split(',')[2] == "ns")
                                    {
                                        foreach (Dictionary<string, dynamic> SubItem in Globals.ADCs)
                                        {
                                            if (SubItem["datacenter_id"] == TreeItem["id"])
                                            {
                                                TreeViewItem ADCItem = ColorTreeNode(SubItem);
                                                if (ADCItem.Foreground == Globals.BadColor)
                                                {
                                                    ChildItem.Foreground = Globals.BadColor;
                                                }
                                                ADCItem.Header = SubItem["hostname"];
                                                ADCItem.Tag = SubItem["id"];
                                                ChildItem.Items.Add(ADCItem);
                                            }
                                            else if (RootNodeItem.Header.ToString() == "Device Groups")
                                            {
                                                foreach (string device in TreeItem["static_device_list"].Split(','))
                                                {
                                                    if (SubItem["display_name"] == device)
                                                    {
                                                        TreeViewItem ADCItem = ColorTreeNode(SubItem);
                                                        if (ADCItem.Foreground == Globals.BadColor)
                                                        {
                                                            ChildItem.Foreground = Globals.BadColor;
                                                        }
                                                        ADCItem.Header = SubItem["hostname"];
                                                        ADCItem.Tag = SubItem["id"];
                                                        ChildItem.Items.Add(ADCItem);
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    RootNodeItem.Items.Add(ChildItem);
                                    RootNodeItem.Items.IsLiveSorting = true;
                                    RootNodeItem.Items.SortDescriptions.Add(new SortDescription("Header", ListSortDirection.Ascending));
                                    RootNodeItem.IsExpanded = true;
                                }
                            }
                        }
                        RootNode.Items.Add(RootNodeItem);
                        RootNode.Items.IsLiveSorting = true;
                        RootNode.Items.SortDescriptions.Add(new SortDescription("Header", ListSortDirection.Ascending));
                        RootNode.IsExpanded = true;
                    }
                }
                Inventory.Items.Add(RootNode);
            }
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
        }

        public Dictionary<string, dynamic> GetNitroURL(string url, string ADCIP)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = Globals.ADMCookie;
            try
            {
                using (var httpClient = new HttpClient(handler))
                {
                    if (ADCIP != null)
                    {
                        httpClient.DefaultRequestHeaders.Add("_MPS_API_PROXY_MANAGED_INSTANCE_IP", ADCIP);
                    }
                    var response = httpClient.GetStringAsync(new Uri(url)).Result;
                    var jss = new JavaScriptSerializer();
                    return jss.Deserialize<Dictionary<string, dynamic>>(response);
                }
            }
            catch (Exception e)
            {
                Dictionary<string, dynamic> ErrorMessage = new Dictionary<string, dynamic>();
                ErrorMessage.Add("Error", e.InnerException.Message);
                return ErrorMessage;
            }
        }

        public ArrayList GetADMObject(string APIType, string ResourceName, string ObjectName = null, string ADCIP = null, Hashtable Filters = null, Boolean Count = false, Hashtable Args = null)
        {
            string NitroURI = Globals.BaseURL + "/" + APIType + "/" + ResourceName;
            if (ObjectName != null)
            {
                NitroURI += "/" + ObjectName;
            }
            if (Filters != null)
            {
                NitroURI += "/" + "?filter=";
                List<string> FilterList = new List<string>();
                foreach (DictionaryEntry Filter in Filters)
                {
                    FilterList.Add(Filter.Key.ToString() + ":" + Filter.Value.ToString());
                }
                NitroURI += String.Join(",", FilterList.ToArray());
            }
            if (Args != null)
            {
                int count = 0;
                foreach (DictionaryEntry Arg in Args)
                {
                    NitroURI += "?" + Arg.Key + "=" + Arg.Value;
                    count++;
                    if (Args.Count > count)
                    {
                        NitroURI += "&";
                    }
                }
            }
            if (Count)
            {
                NitroURI += "?count=yes";
            }
            try
            {
                WriteLog("Info", "Getting " + NitroURI);
                Globals.NitroResults = GetNitroURL(NitroURI, ADCIP);
                if (Globals.NitroResults.ContainsKey("Error"))
                {
                    switch (Globals.NitroResults["Error"].ToString())
                    {
                        case "(400) Bad Request":
                            WriteLog("Error", "Failed to get " + NitroURI);
                            Globals.NitroResults = null;
                            break;

                        case "Response status code does not indicate success: 401 (Unauthorized).":

                            ADMLogin();
                            WriteLog("Info", "Getting " + NitroURI);
                            Globals.NitroResults = GetNitroURL(NitroURI, ADCIP);
                            break;
                    }
                }
                if (ResourceName == "managed_device")
                {
                    ArrayList ConvertedResults = new ArrayList();
                    foreach (Dictionary<string, dynamic> Entry in Globals.NitroResults[ResourceName])
                    {
                        Dictionary<string, dynamic> Result = new Dictionary<string, dynamic>();
                        foreach (KeyValuePair<string, dynamic> KVP in Entry)
                        {
                            if (KVP.Key == "entity_tag")
                            {
                                foreach (Dictionary<string, dynamic> Tags in KVP.Value)
                                {
                                    foreach (string Tag in Tags.Keys)
                                    {
                                        Result.Add(Tag, Tags[Tag]);
                                    }
                                }
                            }
                            else
                            {
                                Result.Add(KVP.Key, KVP.Value);
                            }
                        }
                        ConvertedResults.Add(Result);
                    }

                    return ConvertedResults;
                }
                if (ResourceName == "nsrunningconfig")
                {
                    string[] Results = Globals.NitroResults[ResourceName]["response"].Split('\n');
                    ArrayList ConvertedResults = new ArrayList(Results);
                    return ConvertedResults;
                }
                else if (ResourceName == "nsmode" || ResourceName == "nsfeature" || ResourceName.Contains("param"))
                {
                    ArrayList ConvertedResults = new ArrayList();
                    Dictionary<string, dynamic> Result = new Dictionary<string, dynamic>();
                    foreach (string KeyName in Globals.NitroResults[ResourceName].Keys)
                    {
                        if (KeyName != "mode" && KeyName != "feature")
                        {
                            Result.Add(KeyName, Globals.NitroResults[ResourceName][KeyName]);
                        }
                    }
                    ConvertedResults.Add(Result);
                    return ConvertedResults;
                }
                else if (ResourceName == "nssavedconfig")
                {
                    string[] Results = Globals.NitroResults[ResourceName]["textblob"].Split('\n');
                    ArrayList ConvertedResults = new ArrayList(Results);
                    return ConvertedResults;
                }
                else
                {
                    return Globals.NitroResults[ResourceName];
                }
            }
            catch (Exception e)
            {
                WriteLog("Error", e.Message + " " + NitroURI);
                return null;
            }
        }

        private bool TestConnection()
        {
            bool result = false;
            if (Globals.BaseURL != null)
            {
                string SessionURI = Globals.BaseURL + "/config/mpssession?count=yes";
                HttpWebRequest TestRequest = (HttpWebRequest)WebRequest.Create(SessionURI);
                TestRequest.ContentType = "application/json";
                TestRequest.Method = "GET";
                TestRequest.CookieContainer = Globals.ADMCookie;
                try
                {
                    var httpResponse = (HttpWebResponse)TestRequest.GetResponse();
                    result = true;
                }
                catch (Exception e)
                {
                    Globals.TestResponse = e.Message;
                    WriteLog("Error", e.Message);
                }
            }
            return result;
        }

        private TreeViewItem ColorTreeNode(Dictionary<string, dynamic> TreeNode)
        {
            TreeViewItem ColoredNode = new TreeViewItem();
            if (TreeNode["instance_state"] != "Up")
            {
                ColoredNode.Foreground = Globals.BadColor;
            }
            else if (TreeNode["instance_mode"] == "Primary")
            {
                ColoredNode.Foreground = Globals.GoodColor;
            }
            else
            {
                ColoredNode.Foreground = Globals.SecondaryColor;
            }
            return ColoredNode;
        }

        private void PopulateADCTreeView()
        {
            Globals.SelectedNode.Items.Clear();
            foreach (DictionaryEntry TreeNode in Globals.ADCNodeStructure)
            {
                TreeViewItem RootNode = new TreeViewItem
                {
                    Header = TreeNode.Key.ToString(),
                    Foreground = Brushes.WhiteSmoke,
                };
                Hashtable TreeNodes = TreeNode.Value as Hashtable;
                foreach (DictionaryEntry RootItems in TreeNodes)
                {
                    TreeViewItem RootNodeItem = new TreeViewItem
                    {
                        Header = RootItems.Key.ToString(),
                        Foreground = Brushes.WhiteSmoke
                    };
                    if (RootItems.Value.ToString().Contains(','))
                    {
                        RootNodeItem.Tag = RootItems.Value.ToString();
                    }
                    else
                    {
                        Hashtable ChildNodes = RootItems.Value as Hashtable;
                        if (ChildNodes != null)
                        {
                            foreach (DictionaryEntry ChildNode in ChildNodes)
                            {
                                TreeViewItem ChildNodeItem = new TreeViewItem
                                {
                                    Header = ChildNode.Key.ToString(),
                                    Tag = ChildNode.Value.ToString(),
                                    Foreground = Brushes.WhiteSmoke
                                };
                                RootNodeItem.Items.Add(ChildNodeItem);
                            }
                            RootNodeItem.Items.IsLiveSorting = true;
                            RootNodeItem.Items.SortDescriptions.Add(new SortDescription("Header", ListSortDirection.Ascending));
                        }
                    }
                    RootNode.Items.Add(RootNodeItem);
                    RootNode.Items.IsLiveSorting = true;
                    RootNode.Items.SortDescriptions.Add(new SortDescription("Header", ListSortDirection.Ascending));
                }
                Globals.SelectedNode.Items.Add(RootNode);
                Globals.SelectedNode.Items.IsLiveSorting = true;
                Globals.SelectedNode.Items.SortDescriptions.Add(new SortDescription("Header", ListSortDirection.Ascending));
                Globals.SelectedNode.IsExpanded = true;
            }
        }

        public ItemsControl GetSelectedTreeViewItemParent(TreeViewItem item)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(item);
            while (!(parent is TreeViewItem || parent is TreeView))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            return parent as ItemsControl;
        }

        private static T VisualUpwardSearch<T>(DependencyObject source) where T : DependencyObject
        {
            DependencyObject returnVal = source;
            while (returnVal != null && !(returnVal is T))
            {
                DependencyObject tempReturnVal = null;
                if (returnVal is Visual || returnVal is Visual3D)
                {
                    tempReturnVal = VisualTreeHelper.GetParent(returnVal);
                }
                if (tempReturnVal == null)
                {
                    returnVal = LogicalTreeHelper.GetParent(returnVal);
                }
                else returnVal = tempReturnVal;
            }
            return returnVal as T;
        }

        private void Inventory_LeftClick(object sender, MouseButtonEventArgs e)
        {
        }

        private void Inventory_RightClick(object sender, MouseButtonEventArgs e)
        {
            Globals.ClickedNode =
              VisualUpwardSearch<TreeViewItem>(e.OriginalSource as DependencyObject);

            if (Globals.ClickedNode != null)
            {
                TreeViewItem ClickedItem = Globals.ClickedNode as TreeViewItem;
                ClickedItem.IsSelected = true;
                ItemsControl ParentNode = GetSelectedTreeViewItemParent(ClickedItem);
                if (ParentNode is TreeViewItem ParentItem)
                {
                    string ParentText = ParentItem.Header.ToString();
                    ItemsControl GrandParentNode = GetSelectedTreeViewItemParent(ParentItem);
                    if (GrandParentNode is TreeViewItem GrandParentItem)
                    {
                        string GrandParentText = GrandParentItem.Header.ToString();
                        if (GrandParentText == "Datacenters" || GrandParentText == "Device Groups")
                        {
                            Inventory.ContextMenu = Inventory.Resources["ADCContext"] as System.Windows.Controls.ContextMenu;
                            //Show ADC Context Menu
                        }
                        else
                        {
                            Inventory.ContextMenu = Inventory.Resources["NoContext"] as System.Windows.Controls.ContextMenu;
                            //Future grandparent treeview nodes
                        }
                    }
                    switch (ParentText)
                    {
                        case "Device Groups":
                            Inventory.ContextMenu = Inventory.Resources["DeviceGroupContext"] as System.Windows.Controls.ContextMenu;
                            //Context menu for Instance Groups
                            break;

                        case "Datacenters":
                            Inventory.ContextMenu = Inventory.Resources["DatacenterContext"] as System.Windows.Controls.ContextMenu;
                            //Context menu for Datacenters
                            break;

                        case "Events":
                            if (Globals.ClickedNode.Header.ToString() != "Clear")
                            {
                                Inventory.ContextMenu = Inventory.Resources["EventContext"] as System.Windows.Controls.ContextMenu;
                            }
                            else
                            {
                                Inventory.ContextMenu = Inventory.Resources["NoContext"] as System.Windows.Controls.ContextMenu;
                            }
                            //Context menu for Events
                            break;
                    }
                }
            }
        }

        private void Inventory_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            Globals.SelectedNode = (sender as TreeView).SelectedItem as TreeViewItem;
            if (Globals.SelectedNode != null)
            {
                Globals.SelectedNode.IsExpanded = true;

                if (Globals.SelectedNode is TreeViewItem SelectedItem)
                {
                    ItemsControl ParentNode = GetSelectedTreeViewItemParent(Globals.SelectedNode);
                    if (ParentNode is TreeViewItem ParentItem)
                    {
                        string ParentText = ParentItem.Header.ToString();
                        ItemsControl GrandParentNode = GetSelectedTreeViewItemParent(ParentItem);
                        if (GrandParentNode is TreeViewItem GrandParentItem)
                        {
                            string GrandParentText = GrandParentItem.Header.ToString();
                            ItemsControl GGrandParentNode = GetSelectedTreeViewItemParent(GrandParentItem);
                            if (GrandParentText == "Datacenters" || GrandParentText == "Device Groups")
                            {
                                Hashtable Filter = new Hashtable() {
                                { "hostname", Globals.SelectedNode.Header}
                            };
                                UpdateDataGrid(Globals.ADCs, Globals.ADCDetail, Filter);
                                PopulateADCTreeView();
                                //Populate ADC child nodes
                            }
                            else
                            {
                                //Future grandparent treeview nodes
                            }
                            if (GGrandParentNode is TreeViewItem GGrandParentItem)
                            {
                                string GGrandParentText = GGrandParentItem.Header.ToString();
                                ItemsControl GGGrandParentNode = GetSelectedTreeViewItemParent(GGrandParentItem);
                                if (GGGrandParentNode is TreeViewItem GGGrandParentItem)
                                {
                                    string GGGrandParentText = GGGrandParentItem.Header.ToString();
                                    ItemsControl GGGGrandParentNode = GetSelectedTreeViewItemParent(GGGrandParentItem);
                                    if (GGGGrandParentNode is TreeViewItem GGGGrandParentItem)
                                    {
                                        string GGGGrandParentText = GGGGrandParentItem.Header.ToString();
                                        if (GGGGrandParentText == "Datacenters" || GGGGrandParentText == "Device Groups")
                                        {
                                            Globals.ADCNode = GGrandParentItem;
                                            UpdateDataGrid(null, null, null);
                                            //Populate ADC child nodes
                                        }
                                    }
                                }
                            }
                        }
                        switch (ParentText)
                        {
                            case "Device Groups":
                                Hashtable Filter = new Hashtable() {
                                { "id", Globals.SelectedNode.Tag.ToString()}
                            };
                                UpdateDataGrid(Globals.Groups, Globals.GroupDetail, Filter);
                                break;

                            case "Datacenters":
                                Hashtable DCFilter = new Hashtable() {
                                { "id", Globals.SelectedNode.Tag.ToString() }
                            };
                                UpdateDataGrid(Globals.Datacenters, Globals.DatacenterDetail, DCFilter);
                                break;

                            case "Events":
                                Hashtable NitroFilter = new Hashtable() {
                                    { "severity", Globals.SelectedNode.Tag.ToString() },
                                    { "usecomparator", "true" },
                                    { "reportname", "event_severity_report" },
                                    { "rpt_sample_time", "last_1_day" }
                                };
                                ArrayList Events = GetADMObject("config", "ns_event", Filters: NitroFilter);
                                UpdateDataGrid(Events, null, null);
                                break;
                        }
                    }
                    if (Globals.SelectedNode.Header.ToString() == "Inventory")
                    {
                        UpdateDataGrid(Globals.ADCs, Globals.ADCDetail, null);
                    }
                }
            }
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
        }

        private void ExportToExcel(string ExportType)
        {
            Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
            if (ExcelApp == null)
            {
                MessageBox.Show("Excel is not installed.");
                return;
            }
            else
            {
                ExcelApp.Visible = false;
                Microsoft.Office.Interop.Excel.Workbook xlsWorkbook = ExcelApp.Workbooks.Add(Type.Missing);
                if (ExportType == "DataTable")
                {
                    ExcelApp.DisplayAlerts = false;
                    ExcelApp.Visible = false;

                    Microsoft.Office.Interop.Excel.Worksheet xlsWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlsWorkbook.ActiveSheet;
                    xlsWorksheet.Name = "ADC Export";
                    System.Data.DataTable tempDt = Globals.DataGridTable;
                    //DetailGrid.ItemsSource = tempDt.DefaultView;
                    xlsWorksheet.Cells.Font.Size = 11;
                    int rowcount = 1;
                    for (int i = 1; i <= tempDt.Columns.Count; i++) //taking care of Headers.
                    {
                        xlsWorksheet.Cells[1, i] = tempDt.Columns[i - 1].ColumnName;
                    }
                    foreach (System.Data.DataRow row in tempDt.Rows) //taking care of each Row
                    {
                        rowcount += 1;
                        for (int i = 0; i < tempDt.Columns.Count; i++) //taking care of each column
                        {
                            xlsWorksheet.Cells[rowcount, i + 1] = row[i].ToString();
                        }
                    }
                    var ListObject = xlsWorksheet.ListObjects.AddEx(Microsoft.Office.Interop.Excel.XlListObjectSourceType.xlSrcRange, ExcelApp.ActiveCell.CurrentRegion, null, Microsoft.Office.Interop.Excel.XlYesNoGuess.xlYes);
                    ListObject.Name = "ADC_Export";
                    xlsWorksheet.Application.ActiveWindow.SplitColumn = 0;
                    xlsWorksheet.Application.ActiveWindow.SplitRow = 0;
                    ListObject.Range.Columns.AutoFit();
                    ExcelApp.DisplayAlerts = false;
                }
                else
                {
                    int SheetNumber = 1;
                    foreach (string SheetName in Globals.NitroObjects)
                    {
                        Microsoft.Office.Interop.Excel.Worksheet xlsWorksheet = xlsWorkbook.Worksheets.Add();
                        xlsWorksheet.Select();
                        string ShortSheetName = SheetName.Replace("_", "");
                        xlsWorksheet.Name = ShortSheetName;
                        SheetNumber++;
                        int Row = 1;
                        int Column = 1;
                        ArrayList ColumnNames = new ArrayList();
                        foreach (DictionaryEntry ADCData in Globals.ExcelData)
                        {
                            foreach (DictionaryEntry ObjectData in ADCData.Value as Hashtable)
                            {
                                if (ObjectData.Key.ToString() == SheetName)
                                {
                                    foreach (Dictionary<string, dynamic> RowData in ObjectData.Value as ArrayList)
                                    {
                                        foreach (string ColumnName in RowData.Keys)
                                        {
                                            if (!ColumnNames.Contains(ColumnName))
                                            {
                                                ColumnNames.Add(ColumnName);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        foreach (DictionaryEntry ADCData in Globals.ExcelData)
                        {
                            string ADCName = ADCData.Key.ToString();
                            if (Row == 1)
                            {
                                xlsWorksheet.Cells[Row, Column] = "ADC Name";
                                Column++;
                                foreach (string ColumnName in ColumnNames)
                                {
                                    xlsWorksheet.Cells[Row, Column] = ColumnName;
                                    Column++;
                                }
                                Row++;
                            }
                            Column = 1;
                            foreach (DictionaryEntry ObjectData in ADCData.Value as Hashtable)
                            {
                                if (ObjectData.Key.ToString() == SheetName)
                                {
                                    foreach (Dictionary<string, dynamic> RowData in ObjectData.Value as ArrayList)
                                    {
                                        xlsWorksheet.Cells[Row, Column] = ADCName;
                                        Column++;
                                        foreach (string ColumnName in ColumnNames)
                                        {
                                            try
                                            {
                                                xlsWorksheet.Cells[Row, Column] = RowData[ColumnName];
                                            }
                                            catch
                                            {
                                                xlsWorksheet.Cells[Row, Column] = "";
                                            }
                                            Column++;
                                        }
                                        Row++;
                                        Column = 1;
                                    }
                                }
                            }
                        }
                        var ListObject = xlsWorksheet.ListObjects.AddEx(Microsoft.Office.Interop.Excel.XlListObjectSourceType.xlSrcRange, ExcelApp.ActiveCell.CurrentRegion, null, Microsoft.Office.Interop.Excel.XlYesNoGuess.xlYes);
                        ListObject.Name = SheetName;
                        xlsWorksheet.Application.ActiveWindow.SplitColumn = 0;
                        xlsWorksheet.Application.ActiveWindow.SplitRow = 0;
                        ListObject.Range.Columns.AutoFit();
                    }
                }
                try
                {
                    xlsWorkbook.SaveAs(Globals.ExcelFile);
                }
                catch
                {
                }
                xlsWorkbook.Close(true);
                ExcelApp.DisplayAlerts = true;
                Marshal.ReleaseComObject(xlsWorkbook);
                Marshal.ReleaseComObject(ExcelApp);
            }
        }

        private void DetailGrid_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            DetailGrid.ContextMenu = DetailGrid.Resources["DetailGridContext"] as System.Windows.Controls.ContextMenu;
        }

        private void DetailGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ExportDGToExcel();
        }

        private void ExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            ExportDGToExcel();
        }

        private void ExportDGToExcel()
        {
            var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            Globals.ExcelFile = Globals.PuttyPath + "\\" + "ADCDetails_" + Timestamp + ".xlsx";
            BackgroundProgress.Visibility = Visibility.Visible;
            BackgroundProgress.IsEnabled = true;
            BackgroundLabel.Content = "Exporting to " + Globals.ExcelFile;
            BackgroundLabel.Visibility = Visibility.Visible;
            ExcelWorker.DoWork += new DoWorkEventHandler(ExcelWorker_DoWork);
            ExcelWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ExcelWorker_RunWorkerCompleted);
            BackgroundLabel.Content = "Creating Excel Spreadsheet..";
            ExcelWorker.RunWorkerAsync(argument: "DataTable");
        }

        private void MenuItem_GetADCResources(object sender, RoutedEventArgs e)
        {
            Globals.SelectedADCs = new List<string>();
            DoGetADCResources();
        }

        private void GetNitroResources(object sender, RoutedEventArgs e)
        {
            Globals.SelectedADCs = new List<string>();
            if (Globals.SelectedNode is TreeViewItem SelectedItem)
            {
                ItemsControl ParentNode = GetSelectedTreeViewItemParent(Globals.SelectedNode);
                if (ParentNode is TreeViewItem ParentItem)
                {
                    if (ParentItem.Header.ToString() == "Device Groups")
                    {
                        foreach (Dictionary<string, dynamic> Group in Globals.Groups)
                        {
                            if (Group["name"] == Globals.SelectedNode.Header.ToString())
                            {
                                foreach (Dictionary<string, dynamic> ADC in Globals.ADCs)
                                {
                                    if (Group["static_device_list"].Contains(ADC["display_name"]))
                                    {
                                        Globals.SelectedADCs.Add(ADC["hostname"]);
                                    }
                                }
                            }
                        }
                    }
                    else if (ParentItem.Header.ToString() == "Datacenters")
                    {
                        foreach (Dictionary<string, dynamic> Datacenter in Globals.Datacenters)
                        {
                            if (Datacenter["name"] == Globals.SelectedNode.Header.ToString())
                            {
                                foreach (Dictionary<string, dynamic> ADC in Globals.ADCs)
                                {
                                    if (ADC["datacenter_id"] == Datacenter["id"])
                                    {
                                        Globals.SelectedADCs.Add(ADC["hostname"]);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            DoGetADCResources();
        }

        private void DoGetADCResources()
        {
            Globals.ADCList = new ListView();
            BackgroundProgress.Visibility = Visibility.Visible;
            BackgroundProgress.IsEnabled = true;
            BackgroundLabel.Visibility = Visibility.Visible;
            BackgroundLabel.Content = "Getting ADC Resources..";
            GetResources getResources = new GetResources();
            getResources.ShowDialog();
            if (Globals.ADCList != null)
            {
                Hashtable ADCCollection = new Hashtable();
                foreach (ListViewItem ADCItem in Globals.ADCList.SelectedItems)
                {
                    Hashtable NitroCollection = new Hashtable();
                    string ADCIP = ADCItem.Tag.ToString().Split(',')[1].Replace(" ", "");
                    foreach (string NitroObject in Globals.NitroObjects)
                    {
                        var NitroResult = GetADMObject(APIType: "config", ResourceName: NitroObject, ObjectName: null, ADCIP: ADCIP);
                        NitroCollection.Add(NitroObject, NitroResult);
                    }
                    ADCCollection.Add(ADCItem.Content.ToString(), NitroCollection);
                }
                var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
                string FileName = Globals.PuttyPath + "\\" + "ADCResources_" + Timestamp + ".xlsx";
                Globals.ExcelData = ADCCollection;
                Globals.ExcelFile = FileName;
                ExcelWorker.DoWork += new DoWorkEventHandler(ExcelWorker_DoWork);
                ExcelWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ExcelWorker_RunWorkerCompleted);
                BackgroundLabel.Content = "Creating Excel Spreadsheet..";
                ExcelWorker.RunWorkerAsync(argument: "Hashtable");
            }
            else
            {
                BackgroundProgress.Visibility = Visibility.Hidden;
                BackgroundLabel.Visibility = Visibility.Hidden;
            }
        }

        private void DataGridWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            UpdateDataGridWork();
        }

        private void UpdateDataGridWork()
        {
            Globals.DataGridTable = new System.Data.DataTable();

            if (Globals.Columns == null)
            {
                if (Globals.ArrayData != null)
                {
                    Globals.Columns = new Hashtable();
                    foreach (Dictionary<string, dynamic> Entry in Globals.ArrayData)
                    {
                        foreach (string Key in Entry.Keys)
                        {
                            if (Globals.Columns[Key] == null)
                            {
                                Globals.Columns.Add(Key, Key);
                            }
                        }
                    }
                }
            }
            if (Globals.Columns != null)
            {
                foreach (string PriorityColumn in Globals.PriorityColumns)
                {
                    if (Globals.Columns.Contains(PriorityColumn))
                    {
                        Globals.DataGridTable.Columns.Add(PriorityColumn);
                    }
                }
                foreach (DictionaryEntry Column in Globals.Columns)
                {
                    if (!Globals.DataGridTable.Columns.Contains(Column.Key.ToString()))
                    {
                        Globals.DataGridTable.Columns.Add(Column.Key.ToString());
                    }
                }

                if (Globals.Filters != null)
                {
                    foreach (Dictionary<string, dynamic> Entry in Globals.ArrayData)
                    {
                        foreach (DictionaryEntry Filter in Globals.Filters)
                        {
                            if (Entry[(Filter.Key.ToString())] == Filter.Value)
                            {
                                DataRow NewRow = Globals.DataGridTable.NewRow();
                                foreach (DataColumn Column in Globals.DataGridTable.Columns)
                                {
                                    NewRow[Column.ColumnName] = Entry[Globals.Columns[Column.ColumnName].ToString()];
                                }
                                Globals.DataGridTable.Rows.Add(NewRow);
                            }
                        }
                    }
                }
                else
                {
                    if (Globals.ArrayData != null)
                    {
                        foreach (Dictionary<string, dynamic> ArrayObj in Globals.ArrayData)
                        {
                            DataRow NewRow = Globals.DataGridTable.NewRow();
                            foreach (DataColumn Column in Globals.DataGridTable.Columns)
                            {
                                try
                                {
                                    NewRow[Column.ColumnName] = ArrayObj[Globals.Columns[Column.ColumnName].ToString()];
                                }
                                catch
                                {
                                    NewRow[Column.ColumnName] = "";
                                }
                            }
                            Globals.DataGridTable.Rows.Add(NewRow);
                        }
                    }
                    else
                    {
                        Globals.DataGridTable = null;
                    }
                }
            }
        }

        private void DataGridWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            System.Windows.Forms.Application.DoEvents();
            DetailGrid.DataContext = Globals.DataGridTable;
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
            BackgroundProgress.Visibility = Visibility.Hidden;
            BackgroundLabel.Visibility = Visibility.Hidden;
        }

        private void UpdateDataGrid(ArrayList ArrayData, Hashtable Columns, Hashtable Filters)
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            BackgroundProgress.Visibility = Visibility.Visible;
            BackgroundProgress.IsEnabled = true;
            System.Windows.Forms.Application.DoEvents();
            BackgroundLabel.Content = "Getting results..";
            BackgroundLabel.Visibility = Visibility.Visible;
            DataGridWorker.DoWork += new DoWorkEventHandler(DataGridWorker_DoWork);
            DataGridWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(DataGridWorker_RunWorkerCompleted);
            string NitroResourceName = null;
            string ADCIP = null;
            if (ArrayData == null)
            {
                System.Windows.Forms.Application.DoEvents();
                string NodeTag = Globals.SelectedNode.Tag.ToString();
                NitroResourceName = NodeTag;
                foreach (Dictionary<string, dynamic> ADC in Globals.ADCs)
                {
                    System.Windows.Forms.Application.DoEvents();
                    if (ADC["hostname"] == Globals.ADCNode.Header.ToString())
                    {
                        ADCIP = ADC["ipv4_address"];
                    }
                }
                ItemsControl ParentNode = GetSelectedTreeViewItemParent(Globals.SelectedNode);
                if (ParentNode is TreeViewItem ParentItem)
                {
                    ArrayData = GetADMObject(APIType: "config", ResourceName: NitroResourceName, ADCIP: ADCIP);
                }
            }
            Globals.ArrayData = ArrayData;
            Globals.Columns = Columns;
            Globals.Filters = Filters;
            System.Windows.Forms.Application.DoEvents();
            DataGridWorker.RunWorkerAsync();
        }

        private void ExcelWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            ExportToExcel(e.Argument.ToString());
        }

        private void ExcelWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundProgress.Visibility = Visibility.Hidden;
            BackgroundLabel.Visibility = Visibility.Hidden;
            System.Diagnostics.Process.Start(Globals.ExcelFile);
        }

        private void MenuItem_ClickExit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SCPWithWinSCP(object sender, RoutedEventArgs e)
        {
            string SCPHost = Globals.SelectedNode.Header.ToString();
            foreach (Dictionary<string, dynamic> ADC in Globals.ADCs)
            {
                if (ADC["hostname"] == SCPHost)
                {
                    SCPHost = ADC["ipv4_address"];
                }
            }
            string Command = "(get-appvclientpackage | where-Object {$_.Name -match \"WinSCP\"}).packageID.guid";
            string Results = RunScript(Command, null).Split('\r')[0];
            string LocalAppData = Environment.GetEnvironmentVariable("LocalAppData");
            string ProcessPath = LocalAppData + "\\Microsoft\\Appv\\Client\\Integration\\" + Results + "\\root\\WinSCP.exe";
            string ArgumentList = "sftp://" + Globals.ADMUser + "@" + SCPHost;
            System.Diagnostics.Process.Start(ProcessPath, ArgumentList);
        }

        private void RemoveDatacenter(object sender, RoutedEventArgs e)
        {
        }

        private void CompareSaved(object sender, RoutedEventArgs e)
        {
            BackgroundProgress.Visibility = Visibility.Visible;
            BackgroundLabel.Content = "Comparing Configs..";
            BackgroundLabel.Visibility = Visibility.Visible;
            foreach (Dictionary<string, dynamic> ADC in Globals.ADCs)
            {
                if (ADC["hostname"] == Globals.ClickedNode.Header)
                {
                    string[] NitroObjects = { "nssavedconfig", "nsrunningconfig" };
                    string FileA = null;
                    string FileB = null;
                    string ADCIP = ADC["ipv4_address"];
                    foreach (string NitroObject in NitroObjects)
                    {
                        BackgroundLabel.Content = "Getting " + NitroObject + " From " + Globals.ClickedNode.Header;
                        var Config = GetADMObject("config", NitroObject, ADCIP: ADCIP);
                        var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
                        string FileName = Globals.PuttyPath + "\\" + Globals.ClickedNode.Header + "_" + NitroObject + "-" + Timestamp + ".conf";
                        try
                        {
                            File.WriteAllLines(@"" + FileName, Config.Cast<string>().ToArray());
                            if (NitroObject == "nssavedconfig")
                            {
                                FileA = FileName;
                            }
                            else
                            {
                                FileB = FileName;
                            }
                        }
                        catch
                        {
                            WriteLog("Error", "Unable to write running config to " + FileName);
                        }
                    }
                    System.Diagnostics.Process Code = new System.Diagnostics.Process();
                    Code.StartInfo.FileName = "Code";
                    Code.StartInfo.Arguments = "-d " + FileA + " " + FileB;
                    try
                    {
                        Code.Start();
                    }
                    catch
                    {
                        System.Diagnostics.Process.Start(@"" + Globals.PuttyPath);
                        MessageBox.Show("Install Visual Studio Code to get automatic diffs!");
                    }
                }
            }
            BackgroundProgress.Visibility = Visibility.Hidden;
            BackgroundLabel.Visibility = Visibility.Hidden;
        }

        private void CompareOther(object sender, RoutedEventArgs e)
        {
            SelectADC SelectADC = new SelectADC();
            SelectADC.ShowDialog();
            if (Globals.SelectedADC != null)
            {
                string FileA = null;
                string FileB = null;
                string[] ADCs = { Globals.SelectedADC, Globals.ClickedNode.Header.ToString() };
                foreach (string ADCName in ADCs)
                {
                    foreach (Dictionary<string, dynamic> ADC in Globals.ADCs)
                    {
                        if (ADC["hostname"] == ADCName)
                        {
                            string ADCIP = ADC["ipv4_address"];
                            var RunningConfig = GetADMObject("config", "nsrunningconfig", ADCIP: ADCIP);
                            var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
                            string FileName = Globals.PuttyPath + "\\" + ADCName + "-" + Timestamp + ".conf";
                            try
                            {
                                File.WriteAllLines(@"" + FileName, RunningConfig.Cast<string>().ToArray());
                                if (ADC["hostname"] == Globals.ClickedNode.Header.ToString())
                                {
                                    FileA = FileName;
                                }
                                else
                                {
                                    FileB = FileName;
                                }
                            }
                            catch
                            {
                                WriteLog("Error", "Unable to write running config to " + FileName);
                            }
                        }
                    }
                }
                System.Diagnostics.Process Code = new System.Diagnostics.Process();
                Code.StartInfo.FileName = "Code";
                Code.StartInfo.Arguments = "-d " + FileA + " " + FileB;
                try
                {
                    Code.Start();
                }
                catch
                {
                    System.Diagnostics.Process.Start(@"" + Globals.PuttyPath);
                    MessageBox.Show("Install Visual Studio Code to get automatic diffs!");
                }
            }
        }

        private void ExportToCSV(object sender, RoutedEventArgs e)
        {
            //;
        }

        private void ShowRun(object sender, RoutedEventArgs e)
        {
            foreach (Dictionary<string, dynamic> ADC in Globals.ADCs)
            {
                if (ADC["hostname"] == Globals.ClickedNode.Header)
                {
                    string ADCIP = ADC["ipv4_address"];
                    var RunningConfig = GetADMObject("config", "nsrunningconfig", ADCIP: ADCIP);
                    var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
                    string FileName = Globals.PuttyPath + "\\" + Globals.ClickedNode.Header + "-" + Timestamp + ".conf";
                    try
                    {
                        File.WriteAllLines(@"" + FileName, RunningConfig.Cast<string>().ToArray());
                        System.Diagnostics.Process.Start(@"" + FileName);
                    }
                    catch
                    {
                        WriteLog("Error", "Unable to write running config to " + FileName);
                    }
                }
            }
        }

        private void SSHWithPutty(object sender, RoutedEventArgs e)
        {
            string PuttyExe = Globals.PuttyPath + "\\putty.exe";
            if (!File.Exists(PuttyExe))
            {
                System.Windows.MessageBox.Show(PuttyExe + " not found!", "Please select the path where Putty.exe is located.");
                FolderBrowserDialog PuttyBrowse = new FolderBrowserDialog();
                DialogResult result = PuttyBrowse.ShowDialog();
                if (result.ToString() == "OK")
                {
                    Globals.PuttyPath = PuttyBrowse.SelectedPath;
                    PuttyExe = PuttyBrowse.SelectedPath + "\\putty.exe";
                }
                else
                {
                    PuttyExe = null;
                }
            }
            if (PuttyExe != null && File.Exists(PuttyExe))
            {
                string PuttyHost = Globals.ClickedNode.Header.ToString();
                foreach (Dictionary<string, dynamic> ADC in Globals.ADCs)
                {
                    if (ADC["hostname"] == PuttyHost)
                    {
                        PuttyHost = ADC["ipv4_address"];
                    }
                }
                RegistryKey ADMKey = Registry.CurrentUser.OpenSubKey(Globals.RegistryKey, true);
                ADMKey.SetValue("PuttyPath", Globals.PuttyPath);
                ADMKey.Close();
                System.Diagnostics.Process Putty = new System.Diagnostics.Process();
                Putty.StartInfo.FileName = PuttyExe;
                Putty.StartInfo.Arguments = (Globals.ADMUser.Replace("\\", "") + '@' + PuttyHost);
                Putty.Start();
            }
            else
            {
                System.Windows.MessageBox.Show(PuttyExe + " not found!", "Please get putty.exe and specify the path to continue.");
            }
        }

        private void OpenLomConsole(object sender, RoutedEventArgs e)
        {
            foreach (Dictionary<string, dynamic> ADC in Globals.ADCs)
            {
                if (ADC["hostname"] == Globals.ClickedNode.Header)
                {
                    foreach (Dictionary<string, dynamic> Device in Globals.Devices)
                    {
                        if (ADC["display_name"] == Device["display_name"])
                        {
                            var Tags = Device["entity_tag"];
                            var foo = Tags;
                            try
                            {
                            }
                            catch (Exception Error)
                            {
                                WriteLog("Error", Error.Message);
                                MessageBox.Show(Error.Message, "Define a lomips ADM tag");
                            }
                        }
                    }
                }
            }
        }

        private void OpenInBrowser(object sender, RoutedEventArgs e)
        {
            foreach (Dictionary<string, dynamic> ADC in Globals.ADCs)
            {
                if (ADC["hostname"] == Globals.ClickedNode.Header)
                {
                    string ADCIP = ADC["ipv4_address"];
                    string ADCURL = ADCIP;
                    try
                    {
                        IPHostEntry DNSLookup = Dns.GetHostEntry(ADC["hostname"]);
                        ADCURL = ADC["hostname"];
                    }
                    catch
                    {
                        WriteLog("Error", ("Unable to resolve " + ADC["hostname"] + " connecting to " + ADCIP + " instead."));
                    }
                    string LoginToken = null;
                    string LoginUser = null;
                    try
                    {
                        ArrayList DeviceLogin = new ArrayList(GetADMObject("config", "device_login", ObjectName: ADCIP));
                        foreach (Dictionary<string, dynamic> LoginDetails in DeviceLogin)
                        {
                            LoginToken = WebUtility.UrlEncode(LoginDetails["login_token"]);
                            LoginUser = LoginDetails["username"];
                        }
                        string URL = Globals.HttpType + ADCURL + "/menu/ss?sid=" + LoginToken + "&username=" + LoginUser;
                        System.Diagnostics.Process.Start(URL);
                    }
                    catch (Exception Error)
                    {
                        WriteLog("Error", Error.Message);
                        MessageBox.Show(Error.Message, "Unable to Browse");
                    }
                }
            }
        }

        private void MenuItem_EditSettings(object sender, RoutedEventArgs e)
        {
            Settings SettingsWindow = new Settings();
            SettingsWindow.ShowDialog();
        }

        private void MenuItem_ClickChangeADM(object sender, RoutedEventArgs e)
        {
            Globals.ADMPass = null;
            StartADMUtil();
        }

        private void MenuItem_ClickOpenADM(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(Globals.HttpType + Globals.ADMHost);
        }

        private void MenuItem_Refresh(object sender, RoutedEventArgs e)
        {
            RefreshADMUtil();
        }

        private void ClearEvents(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Feature not yet implemented");
        }

        private string RunScript(string scriptText, string[] param)
        {
            // create Powershell runspace
            Runspace runspace = RunspaceFactory.CreateRunspace();
            // open it
            //Invoke(new LogText(scriptText));
            runspace.Open();
            if (param != null)
            {
                if (param.Length == 1)
                {
                    runspace.SessionStateProxy.SetVariable("var1", param[0]);
                }
                if (param.Length > 1)
                {
                    int i = 1;
                    string arg;
                    foreach (string parm in param)
                    {
                        arg = "var" + i;
                        runspace.SessionStateProxy.SetVariable(arg, parm);
                        i++;
                    }
                }
            }
            // create a pipeline and feed it the script text
            Pipeline pipeline = runspace.CreatePipeline();
            pipeline.Commands.AddScript(scriptText);
            pipeline.Commands.Add("Out-String");
            // execute the script
            try
            {
                Collection<PSObject> results = pipeline.Invoke();
                // return results as string
                StringBuilder stringBuilder = new StringBuilder();
                foreach (PSObject obj in results)
                {
                    stringBuilder.AppendLine(obj.ToString());
                }
                return stringBuilder.ToString();
            }
            catch (Exception e)
            {
                WriteLog("Error", "Unhandled exception running PowerShell Script: " + e.Message.ToString());
                return "Not Found";
            }
        }

        private void RefreshADMUtil()
        {
            Inventory.Items.Clear();
            RegistryKey ADMKey = Registry.CurrentUser.OpenSubKey(Globals.RegistryKey, true);
            ADMKey.SetValue("ADMHost", Globals.ADMHost);
            ADMKey.SetValue("ADMUser", Globals.ADMUser);
            ADMKey.Close();
            if (Globals.ADMCookie != null)
            {
                WriteLog("Info", "Getting ADC inventory from " + Globals.ADMHost);
                Globals.ADCs = GetADMObject("config", "ns");
                Globals.Datacenters = GetADMObject("config", "mps_datacenter");
                Globals.EventSummary = GetADMObject("config", "event_severity_report", Args: new Hashtable() { { "duration", "last_1_day" } });
                Globals.Groups = GetADMObject("config", "device_group");
                Globals.Devices = GetADMObject("config", "managed_device");
                PopulateTreeView();
                UpdateDataGrid(Globals.ADCs, Globals.ADCDetail, null);
            }
            else
            {
                this.Close();
            }
        }

        private void RefreshNode(object sender, RoutedEventArgs e)
        {
            if (Globals.SelectedNode is TreeViewItem SelectedItem)
            {
                ItemsControl ParentNode = GetSelectedTreeViewItemParent(Globals.SelectedNode);
                if (ParentNode is TreeViewItem ParentItem)
                {
                    if (ParentItem.Header.ToString() == "Events")
                    {
                    }
                }
            }
        }

        private void LogGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            try
            {
                var row = (DataRowView)e.Row.Item;
                if (row.Row[1].ToString().Contains("Error"))
                {
                    e.Row.Background = Globals.BadColor;
                }
                LogGrid.ScrollIntoView(row);
            }
            catch
            {
            }
        }

        private void WriteLog(string LogType, string LogMessage)
        {
            DataRow LogLine = Globals.LogTable.NewRow();
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            string TimeStamp = TimeZoneInfo.ConvertTime(DateTime.Now, timeZone).ToString("HH:mm:ss");
            LogLine["Time"] = TimeStamp;
            LogLine["Type"] = LogType;
            LogLine["Message"] = LogMessage;
            Globals.LogTable.Rows.Add(LogLine);
        }

        public MainWindow()
        {
            InitializeComponent();
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            Globals.version = version.Major + "." + version.Minor + "." + version.Build + "." + version.Revision;
            this.Title = "ADM Utility v" + Globals.version;
            Globals.LogTable.Columns.Add("Time");
            Globals.LogTable.Columns.Add("Type");
            Globals.LogTable.Columns.Add("Message");
            StartADMUtil();
        }
    }
}