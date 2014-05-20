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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace ListViewSample
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Refresh_Button_Click(object sender, RoutedEventArgs e)
        {
            string json = await RequestJson();
            this.ParseJson(json);
        }

        private async Task<string> RequestJson()
        {
            string url = "http://www.redmine.org/issues.json";
            HttpClient client = new HttpClient();
            Task<string> getStringTask = client.GetStringAsync(url);
            string result = await getStringTask;
            return result;
        }

        private void ParseJson(String json)
        {
            List<Issue> issues = new List<Issue>();
            JObject obj = JObject.Parse(json);
            JArray array = JArray.Parse(obj["issues"].ToString());
            foreach (JObject itemObj in array)
            {
                Issue issue = new Issue();
                issue.Subject = itemObj["subject"].ToString();
                issue.Done = itemObj["done_ratio"].ToString();
                issue.Author = itemObj["author"]["name"].ToString();
                issues.Add(issue);
            }

            IssueListView.ItemsSource = issues;
        }


        private void Clear_Button_Click(object sender, RoutedEventArgs e)
        {
            IssueListView.ItemsSource = null;
        }
    }
}
