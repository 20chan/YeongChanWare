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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;

namespace YeongchanWare
{
    public struct Q
    {
        public string Answer;
        public string Wrong1;
        public string Wrong2;
        public string Wrong3;
        public string Question;
        public string Image;

        public Q(string question, string answer, string w1, string w2, string w3, string img = "")
        {
            Question = question;
            Answer = answer;
            Wrong1 = w1;
            Wrong2 = w2;
            Wrong3 = w3;
            Image = img;
        }
    }

    /// <summary>
    /// Question.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Question : Window, INotifyPropertyChanged
    {
        Random r;
        public string QuestionText { get; set; } = "que";
        public string Text1 { get; set; } = "text1";
        public string Text2 { get; set; } = "text2";
        public string Text3 { get; set; } = "text3";
        public string Text4 { get; set; } = "text4";

        public string QuestionImg { get; set; } = @"성식.jpg";

        public string LeftCount { get; set; } = "-1";

        public int Answer = -1;

        bool readyForClose = false;

        public static Q[] Questions = new Q[]
        {
            new Q("이거는 무엇일까요?", "갓성식", "다람쥐", "트럼프", "무릎연골줄기세포", "성식.jpg"),
            new Q("1 + 2 + 3 + 4 = ?", "10", "3.1415", "잘 모르겠고 초콜릿 먹고 싶다", "36"),
            new Q("우리 학교 교장선생님 성함은 무엇일까요?", "최부영", "김정은", "조규석", "마부장", "dsm.png"),
            new Q("프로그램의 제작자 이름은 무엇인가요?", "이영찬", "01영찬", "김수한무 거북이와 두루미 삼천갑자 동방삭", "나성식", "영찬.png"),
        };

        Stack<Q> questionStack;

        public event PropertyChangedEventHandler PropertyChanged;

        public Question()
        {
            InitializeComponent();
            r = new Random();
            this.Closing += Question_Closing;
            this.Closed += Question_Closed;
            LoadQuestions();
        }

        private void Question_Closing(object sender, CancelEventArgs e)
        {
            if (!readyForClose)
                e.Cancel = true;
        }

        void LoadQuestions()
        {
            var a = new List<Q>(Questions);
            a.Shuffle();
            questionStack = new Stack<Q>(a);

            Display();
        }

        void Display()
        {
            var p = questionStack.Pop();
            var ranged = Enumerable.Range(0, 4).ToList();
            ranged.Shuffle();
            Answer = ranged[0];
            SetText(p.Answer, Answer);
            SetText(p.Wrong1, ranged[1]);
            SetText(p.Wrong2, ranged[2]);
            SetText(p.Wrong3, ranged[3]);
            this.QuestionImg = p.Image;
            this.QuestionText = p.Question;
            this.LeftCount = $"{questionStack.Count - 1} 문제 남았습니다 ~~!!";

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Text1)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Text2)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Text3)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Text4)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(QuestionText)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(QuestionImg)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LeftCount)));
        }

        void SetText(string text, int index)
        {
            switch (index)
            {
                case 0:
                    Text1 = $"1. {text}";
                    break;
                case 1:
                    Text2 = $"2. {text}";
                    break;
                case 2:
                    Text3 = $"3. {text}";
                    break;
                case 3:
                    Text4 = $"4. {text}";
                    break;
            }
        }

        private void Question_Closed(object sender, EventArgs e)
        {
            MainWindow.hook.HookEnd();
            MainWindow.TaskManager(true);
            MainWindow.back.Close();
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock from = sender as TextBlock;
            int index = -1;
            if (from == TxtBlock1)
                index = 0;
            else if (from == TxtBlock2)
                index = 1;
            else if (from == TxtBlock3)
                index = 2;
            else if (from == TxtBlock4)
                index = 3;

            if (Answer == index)
                Correct();
            else Wrong();
        }

        void Correct()
        {
            if (questionStack.Count == 0)
            {
                readyForClose = true;
                MessageBox.Show("모두 정답입니다 `!~!~!!~!~!!\n앞으로는 노트북 보안에 더 신경을 써주세요~~!!!!", "ㅊㅋㅊㅋ");
                this.Close();
            }
            else
            {
                label.Foreground = Brushes.Black;
                var appear = Resources["appear"] as Storyboard;
                appear.Begin(label);
                Display();
            }
        }

        void Wrong()
        {
            var appear = Resources["wrong"] as Storyboard;
            appear.Begin(label);
            LoadQuestions();
        }
    }
}
