using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using Argus.Extensions;
using Argus.Memory;
using StackExchange.StacMan;

namespace WpfAppTestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Profile> Profiles = new ObservableCollection<Profile>();

        public MainWindow()
        {
            InitializeComponent();

            //TextChord.Text = System.IO.File.ReadAllText(@"C:\Users\blake\Documents\KB\Repository\Guitar\John Denver - Country Roads.md");
        }


        public class Person
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        private void ButtonLower_Click(object sender, RoutedEventArgs e)
        {
            int found = 0;
            var sb = new StringBuilder();
            var client = new StacManClient(version: "2.3");

            var response = client.Questions.GetAll("cooking",
                page: 1,
                pagesize: 50,                
                fromdate: Convert.ToDateTime("1/1/2020"),
                todate: Convert.ToDateTime("12/31/2020"),
                sort: StackExchange.StacMan.Questions.AllSort.Hot,
                order: Order.Desc,
                filter: "!T1gn2_dYO-SE)yAF_f").Result;

            foreach (var question in response.Data.Items)
            {
                var answerResp = client.Answers.GetAll("cooking", filter: "!*MZqiH2nLdWUlVNU").Result;
                var answer = question.Answers.FirstOrDefault(x => x.AnswerId == question.AcceptedAnswerId);

                if (answer != null)
                {
                    found++;

                    sb.Append("#### Question\r\n\r\n");
                    sb.Append(question.Title);

                    if (!question.BodyMarkdown.IsNullOrEmptyOrWhiteSpace())
                    {
                        sb.Append("\r\n\r\n");
                        sb.Append(question.BodyMarkdown);
                    }

                    sb.Append("\r\n\r\n#### Answer\r\n\r\n");
                    sb.Append(answer.BodyMarkdown ?? answer.Body);

                    if (question?.Tags != null)
                    {
                        sb.AppendFormat("\r\n\r\n > {0}\r\n\r\n", string.Join(' ', question.Tags));
                    }
                }
            }

            System.IO.File.WriteAllText(@"C:\Users\blake\Desktop\CookingQuestions.md", sb.ToString());
            TextChord.Text = sb.ToString();

            //TextChord.Text = Argus.Audio.Test.ChordTransposer.TransposeText(TextChord.Text, -1);
            //TextChord.Text = Chord.Transpose(TextChord.Text, -1);
        }

        private void ButtonHigher_Click(object sender, RoutedEventArgs e)
        {
            int found = 0;
            var sb = new StringBuilder();
            var client = new StacManClient(version: "2.3");

            var response = client.Search.GetMatches("cooking",
                page: 1,
                pagesize: 20,
                fromdate: Convert.ToDateTime("1/1/2020"),
                todate: Convert.ToDateTime("12/31/2021"),
                sort: StackExchange.StacMan.Questions.SearchSort.Votes,
                intitle: "herb",
                order: Order.Desc,
                filter: "!T1gn2_dYO-SE)yAF_f").Result;

            foreach (var question in response.Data.Items)
            {
                var answerResp = client.Answers.GetAll("cooking", filter: "!*MZqiH2nLdWUlVNU").Result;
                var answer = question.Answers.FirstOrDefault(x => x.AnswerId == question.AcceptedAnswerId);

                if (answer != null)
                {
                    found++;

                    sb.Append("#### Question\r\n\r\n");
                    sb.Append(question.Title);

                    if (!question.BodyMarkdown.IsNullOrEmptyOrWhiteSpace())
                    {
                        sb.Append("\r\n\r\n");
                        sb.Append(question.BodyMarkdown);
                    }

                    sb.Append("\r\n\r\n#### Answer\r\n\r\n");
                    sb.Append(answer.BodyMarkdown ?? answer.Body);

                    if (question?.Tags != null)
                    {
                        sb.AppendFormat("\r\n\r\n > {0}\r\n\r\n", string.Join(' ', question.Tags));
                    }
                }
            }

            System.IO.File.WriteAllText(@"C:\Users\blake\Desktop\CookingQuestions.md", sb.ToString());
            TextChord.Text = sb.ToString();
            //TextChord.Text = Argus.Audio.Test.ChordTransposer.TransposeText(TextChord.Text, 1);
        }
    }
}
