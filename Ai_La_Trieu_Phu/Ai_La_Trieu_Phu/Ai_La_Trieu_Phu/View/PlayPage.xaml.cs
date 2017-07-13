using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Reactive;
using System.IO.IsolatedStorage;
using Microsoft.Xna.Framework.Media;
using Ai_La_Trieu_Phu.Model;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System.Threading;
using System.Windows.Threading;
using Microsoft.Phone.Tasks;
using Coding4Fun.Toolkit.Controls;
using System.Windows.Media;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;

namespace Ai_La_Trieu_Phu.View
{
    public partial class PlayPage : PhoneApplicationPage
    {
        // define variables
        #region
        private int number = 1;
        private double ans_numb = 0;
        private double choice = 0;
        private static int second;
        private Random rand = new Random();
        private IList<Database.Question> list1 = null;
        private DispatcherTimer countdown_timer = new DispatcherTimer();
        private List<int> mien_gia_tri;
        private int n1, n2,tmp;
        private string mute;
        private int t1, t2, t3, t4;
        private Button btA1 = new Button();
        private Button btB1 = new Button();
        private Button btC1 = new Button();
        private Button btD1 = new Button();
        TextBlock tbA1 = new TextBlock();
        TextBlock tbB1 = new TextBlock();
        TextBlock tbC1 = new TextBlock();
        TextBlock tbD1 = new TextBlock();
        AboutPrompt prompt = new AboutPrompt();
        AboutPrompt prompt1 = new AboutPrompt();
        AboutPrompt prompt2 = new AboutPrompt();
        SoundEffect effect;
        SoundEffectInstance ins;
        Image im1 = new Image();
        Image im2 = new Image();
        Image im3 = new Image();
        Image im4 = new Image();
        Image im5 = new Image();
        Image im6 = new Image();
        ListBox lb = new ListBox();
        RadioButton rd2 = new RadioButton();
        RadioButton rd1 = new RadioButton();
        List<int> money = new List<int> {100000,200000,300000,500000,1000000,2000000,3000000,6000000,9000000,15000000,25000000,35000000,50000000,80000000,120000000 };
        #endregion
        public PlayPage()
        {           
            InitializeComponent();
            Storyboard_dongho.Begin();
            mute = IsolatedStorageSettings.ApplicationSettings["key"].ToString();
        }

        private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {
            
            Sound("loichao.wav");
            if (mute == "0")
            {
                PlaySound();
            }
            else
            {
                StopSound();
            }
            countdown_timer.Interval = TimeSpan.FromSeconds(1);
            countdown_timer.Tick += countdown_timer_Tick;
            Storyboard_moccauhoi.Begin();

            prompt2.Title = "Ai Là Triệu Phú";
            ListBox li = new ListBox();
            TextBlock txt = new TextBlock();
            txt.Text = "Bạn có muốn bỏ qua phần giới thiệu không?";
            
            rd1.Content = "Đồng ý";
            
            rd2.Content = "Không";
            li.Items.Add(txt);
            li.Items.Add(rd1);
            li.Items.Add(rd2);
            prompt2.Body = li;
            prompt2.Show();
            prompt2.Completed += prompt2_Completed;
            Scheduler.Dispatcher.Schedule(hidepromt_start, TimeSpan.FromSeconds(16));
        }

        private void hidepromt_start()
        {
            if (prompt2.IsOpen)
            {
                prompt2.Hide();
                MessageBoxResult result = MessageBox.Show("Bạn đã sẵn sàng chưa?", "", MessageBoxButton.OK);
                if (result == MessageBoxResult.OK)
                {
                    if (mute == "0")
                    {
                        batdau.Play();
                    }
                    Scheduler.Dispatcher.Schedule(load_cauhoi_1, TimeSpan.FromSeconds(0.5));
                }
            }  
        }

        void prompt2_Completed(object sender, PopUpEventArgs<object, PopUpResult> e)
        {
            if (rd1.IsChecked == true)
            {
                ins.Stop();
                Scheduler.Dispatcher.Schedule(readyForPlay, TimeSpan.FromSeconds(0.5));
            }
            else if (rd2.IsChecked == true)
            {
                Storyboard_trogiup.Begin();
                Scheduler.Dispatcher.Schedule(readyForPlay, TimeSpan.FromSeconds(15));
            }
            else if (rd1.IsChecked == false && rd2.IsChecked == false)
            {
                Scheduler.Dispatcher.Schedule(readyForPlay, TimeSpan.FromSeconds(15));
            }
        }

        private void readyForPlay()
        {
            MessageBoxResult result = MessageBox.Show("Bạn đã sẵn sàng chưa?","",MessageBoxButton.OK);
            if (result == MessageBoxResult.OK)
            {
                if (mute == "0")
                {
                    batdau.Play();
                }
                Scheduler.Dispatcher.Schedule(load_cauhoi_1, TimeSpan.FromSeconds(0.5));
            }
        }

        private void load_cauhoi_1()
        {
            if (number == 16)
            {
                Sound("chienthang.wav");
                if (mute == "0")
                {
                    PlaySound();
                }
                else
                {
                    StopSound();
                }
                Scheduler.Dispatcher.Schedule(thangCuoc,TimeSpan.FromSeconds(8));
            }
            else
            {
                list1 = ConnectDatabase.GetQuestionLevel(number);
                var t = list1.ElementAt(rand.Next(0, list1.Count));
                tbl1.Text = "Câu số " + number + ":" + t.QUESTION_CONTENT;
                daA.Text = "A: " + t.QUESTION_1;
                daB.Text = "B: " + t.QUESTION_2;
                daC.Text = "C: " + t.QUESTION_3;
                daD.Text = "D: " + t.QUESTION_4;
                ans_numb = t.ANSWER_CONTENT.Value;
                Sound("ques0" + number + ".wav");
                if (mute == "0")
                {
                    PlaySound();
                }
                else
                {
                    StopSound();
                }
                second = 59;
                if (mute == "0")
                {
                    nhacnen.Volume = 1;
                }
                else
                {
                    nhacnen.Volume = 0;
                }
                nhacnen.Play();
                nhacnen.MediaEnded += nhacnen_MediaEnded;
                countdown_timer.Start();

                second = 59;
                dapan_A2.Visibility = Visibility.Collapsed;
                dapan_B2.Visibility = Visibility.Collapsed;
                dapan_C2.Visibility = Visibility.Collapsed;
                dapan_D2.Visibility = Visibility.Collapsed;
                daA.IsHitTestVisible = true;
                daB.IsHitTestVisible = true;
                daC.IsHitTestVisible = true;
                daD.IsHitTestVisible = true;
                daA.Opacity = 1;
                daB.Opacity = 1;
                daC.Opacity = 1;
                daD.Opacity = 1;
                nammuoi_nammuoi1.IsHitTestVisible = true;
                goidien1.IsHitTestVisible = true;
                hoikhangia1.IsHitTestVisible = true;
                back1.IsHitTestVisible = true;
                if (number == 9 || number == 14)
                {
                    Scheduler.Dispatcher.Schedule(mocQuanTrong, TimeSpan.FromSeconds(2));
                }
            }
            
         }

        private void mocQuanTrong()
        {
            Sound("important.wav");
            if (mute == "0")
            {
                PlaySound();
            }
            else
            {
                StopSound();
            }
        }

        void countdown_timer_Tick(object sender, EventArgs e)
        {
            dongho.Text = second.ToString();
            second--;
            if (dongho.Text == "0")
            {
                Sound("out_of_time.wav");
                if (mute == "0")
                {
                    PlaySound();
                }
                else
                {
                    StopSound();
                }
                countdown_timer.Stop();
                thuaCuoc();
            }
        }

        void nhacnen_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (mute == "0")
            {
                nhacnen.Play();
            }
            else
                nhacnen.Volume = 0;
        }

        private SoundEffectInstance Sound(string soundFile)
        {
            using (var stream = TitleContainer.OpenStream(soundFile))
            {
                effect = SoundEffect.FromStream(stream);
                ins = effect.CreateInstance();
                FrameworkDispatcher.Update();
            }
            return ins;
        }

        private void PlaySound()
        {
            ins.Play();
        }

        private void StopSound()
        {
            ins.Stop();
        }

        private void OnTimeDone(object state)
        {
            if (number < 5)
            {
                Sound("ans_now1.wav");
                if (mute == "0")
                {
                    PlaySound();
                }
                else
                {
                    StopSound();
                }
            }
            else if (number == 5)
            {
                Sound("duaraketqua.wav");
                if (mute == "0")
                {
                    PlaySound();
                }
                else
                {
                    StopSound();
                }
            }
            else if (number > 5 && number < 10)
            {
                Sound("duaraketqua2.wav");
                if (mute == "0")
                {
                    PlaySound();
                }
                else
                {
                    StopSound();
                }
            }
            else if (number == 10)
            {
                Sound("duaraketqua3.wav");
                if (mute == "0")
                {
                    PlaySound();
                }
                else
                {
                    StopSound();
                }
            }
            else
            {
                Sound("duaraketqua4.wav");
                if (mute == "0")
                {
                    PlaySound();
                }
                else
                {
                    StopSound();
                }
            }
        }

        //answer event
        #region
        private void daA_MouseEnter_1(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Sound("ping.wav");
            if (mute == "0")
            {
                PlaySound();
            }
            else
            {
                StopSound();
            }
            MessageBoxResult result = MessageBox.Show("Đáp án cuối cùng của bạn là A?", "Lựa chọn đáp án", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                countdown_timer.Stop();
                dapan_A2.Visibility = Visibility.Visible;
                daA.IsHitTestVisible = false;
                daB.IsHitTestVisible = false;
                daC.IsHitTestVisible = false;
                daD.IsHitTestVisible = false;
                nammuoi_nammuoi1.IsHitTestVisible = false;
                goidien1.IsHitTestVisible = false;
                hoikhangia1.IsHitTestVisible = false;
                back1.IsHitTestVisible = false;
                choice = 0;
                Sound("ans_a.wav");
                if (mute == "0")
                {
                    PlaySound();
                }
                else
                {
                    StopSound();
                }
                Scheduler.Dispatcher.Schedule(OnTimeDone, TimeSpan.FromSeconds(4));
                Scheduler.Dispatcher.Schedule(CorrectAnswerA, TimeSpan.FromSeconds(8));
            }
            else
            {
                return;
            }        
        }

        private void CorrectAnswerA(object state)
        {
            if (ans_numb == choice)
            {
                Sound("d_1.wav");
                if (mute == "0")
                {
                    PlaySound();
                }
                else
                {
                    StopSound();
                }
                if (number == 5)
                {
                    Scheduler.Dispatcher.Schedule(sound_correctAnswer5,TimeSpan.FromSeconds(2));
                }
                else if (number == 9)
                {
                    Scheduler.Dispatcher.Schedule(sound_correctAnswer9, TimeSpan.FromSeconds(3));
                }
                else if (number == 10)
                {
                    Scheduler.Dispatcher.Schedule(sound_correctAnswer10, TimeSpan.FromSeconds(2));
                }
                else if (number == 11)
                {
                    Scheduler.Dispatcher.Schedule(sound_correctAnswer11, TimeSpan.FromSeconds(2));
                }
                else if (number == 12)
                {
                    Scheduler.Dispatcher.Schedule(sound_correctAnswer12, TimeSpan.FromSeconds(2));
                }
                else if (number == 13)
                {
                    Scheduler.Dispatcher.Schedule(sound_correctAnswer13, TimeSpan.FromSeconds(2));
                }
                else if (number == 14)
                {
                    Scheduler.Dispatcher.Schedule(sound_correctAnswer14, TimeSpan.FromSeconds(2));
                }

                else
                {
                    number++;
                    load_cauhoi_1();
                }
            }
            else
            {
                if (ans_numb == 1)
                {
                    Sound("lose_b.wav");
                    if (mute == "0")
                    {
                        PlaySound();
                    }
                    else
                    {
                        StopSound();
                    }
                    Storyboard_dapanB.Begin();
                    dapan_B2.Visibility = Visibility.Visible;
                    dapan_A2.Visibility = Visibility.Collapsed;
                    Scheduler.Dispatcher.Schedule(thuaCuoc, TimeSpan.FromSeconds(5));
                }
                else if (ans_numb == 2)
                {
                    Sound("lose_c.wav");
                    if (mute == "0")
                    {
                        PlaySound();
                    }
                    else
                    {
                        StopSound();
                    }
                    Storyboard_dapanC.Begin();
                    dapan_C2.Visibility = Visibility.Visible;
                    dapan_A2.Visibility = Visibility.Collapsed;
                    Scheduler.Dispatcher.Schedule(thuaCuoc, TimeSpan.FromSeconds(5));
                }
                else
                {
                    Sound("lose_d.wav");
                    if (mute == "0")
                    {
                        PlaySound();
                    }
                    else
                    {
                        StopSound();
                    }
                    Storyboard_dapanD.Begin();
                    dapan_D2.Visibility = Visibility.Visible;
                    dapan_A2.Visibility = Visibility.Collapsed;
                    Scheduler.Dispatcher.Schedule(thuaCuoc, TimeSpan.FromSeconds(5));
                }
            }
        }

        private void sound_correctAnswer13()
        {
            number++;
            Scheduler.Dispatcher.Schedule(load_cauhoi_1, TimeSpan.FromSeconds(2));
        }

        private void sound_correctAnswer12()
        {
            number++;
            Scheduler.Dispatcher.Schedule(load_cauhoi_1, TimeSpan.FromSeconds(2));
        }

        private void sound_correctAnswer11()
        {
            number++;
            Scheduler.Dispatcher.Schedule(load_cauhoi_1, TimeSpan.FromSeconds(2));
        }

        private void sound_correctAnswer10()
        {
            nhacnen.Stop();
            Sound("hetcau10.wav");
            if (mute == "0")
            {
                PlaySound();
            }
            else
            {
                StopSound();
            }
            number++;
            Scheduler.Dispatcher.Schedule(load_cauhoi_1, TimeSpan.FromSeconds(15));
        }

        private void sound_correctAnswer14()
        {
            nhacnen.Stop();
            Sound("hetcau14.wav");
            if (mute == "0")
            {
                PlaySound();
            }
            else
            {
                StopSound();
            }
            number++;
            Scheduler.Dispatcher.Schedule(load_cauhoi_1, TimeSpan.FromSeconds(8));
        }

        private void sound_correctAnswer9()
        {
            nhacnen.Stop();
            Sound("hetcau9_2.wav");
            if (mute == "0")
            {
                PlaySound();
            }
            else
            {
                StopSound();
            }
            number++;
            Scheduler.Dispatcher.Schedule(load_cauhoi_1, TimeSpan.FromSeconds(10));
        }

        private void sound_correctAnswer5()
        {
            nhacnen.Stop();
            Sound("het_moc_5.wav");
            if (mute == "0")
            {
                PlaySound();
            }
            else
            {
                StopSound();
            }
            number++;
            Scheduler.Dispatcher.Schedule(load_cauhoi_1, TimeSpan.FromSeconds(19));
        }

        private void daB_MouseEnter_1(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Sound("ping.wav");
            if (mute == "0")
            {
                PlaySound();
            }
            else
            {
                StopSound();
            }
            MessageBoxResult result = MessageBox.Show("Đáp án cuối cùng của bạn là B?", "Lựa chọn đáp án", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                countdown_timer.Stop();
                dapan_B2.Visibility = Visibility.Visible;
                daA.IsHitTestVisible = false;
                daB.IsHitTestVisible = false;
                daC.IsHitTestVisible = false;
                daD.IsHitTestVisible = false;
                nammuoi_nammuoi1.IsHitTestVisible = false;
                goidien1.IsHitTestVisible = false;
                hoikhangia1.IsHitTestVisible = false;
                back1.IsHitTestVisible = false;
                choice = 1;
                Sound("ans_b.wav");
                if (mute == "0")
                {
                    PlaySound();
                }
                else
                {
                    StopSound();
                }
                Scheduler.Dispatcher.Schedule(OnTimeDone, TimeSpan.FromSeconds(4));
                Scheduler.Dispatcher.Schedule(CorrectAnswerB, TimeSpan.FromSeconds(8));
            }
            else
                return;
        }

        private void CorrectAnswerB()
        {
            if (ans_numb == choice)
            {
                Sound("d_2.wav");
                if (mute == "0")
                {
                    PlaySound();
                }
                else
                {
                    StopSound();
                }
                if (number == 5)
                {
                    Scheduler.Dispatcher.Schedule(sound_correctAnswer5, TimeSpan.FromSeconds(2));
                }
                else if (number == 9)
                {
                    Scheduler.Dispatcher.Schedule(sound_correctAnswer9, TimeSpan.FromSeconds(3));
                }
                else if (number == 10)
                {
                    Scheduler.Dispatcher.Schedule(sound_correctAnswer10, TimeSpan.FromSeconds(2));
                }
                else if (number == 11)
                {
                    Scheduler.Dispatcher.Schedule(sound_correctAnswer11, TimeSpan.FromSeconds(2));
                }
                else if (number == 12)
                {
                    Scheduler.Dispatcher.Schedule(sound_correctAnswer12, TimeSpan.FromSeconds(2));
                }
                else if (number == 13)
                {
                    Scheduler.Dispatcher.Schedule(sound_correctAnswer13, TimeSpan.FromSeconds(2));
                }
                else if (number == 14)
                {
                    Scheduler.Dispatcher.Schedule(sound_correctAnswer14, TimeSpan.FromSeconds(2));
                }

                else
                {
                    number++;
                    load_cauhoi_1();
                }
            }
            else
            {
                if (ans_numb == 0)
                {
                    Sound("lose_a.wav");
                    if (mute == "0")
                    {
                        PlaySound();
                    }
                    else
                    {
                        StopSound();
                    }
                    Storyboard_dapanA.Begin();
                    dapan_A2.Visibility = Visibility.Visible;
                    dapan_B2.Visibility = Visibility.Collapsed;
                    Scheduler.Dispatcher.Schedule(thuaCuoc, TimeSpan.FromSeconds(5));
                }
                else if (ans_numb == 2)
                {
                    Sound("lose_c.wav");
                    if (mute == "0")
                    {
                        PlaySound();
                    }
                    else
                    {
                        StopSound();
                    }
                    Storyboard_dapanC.Begin();
                    dapan_C2.Visibility = Visibility.Visible;
                    dapan_B2.Visibility = Visibility.Collapsed;
                    Scheduler.Dispatcher.Schedule(thuaCuoc, TimeSpan.FromSeconds(5));
                }
                else
                {
                    Sound("lose_d.wav");
                    if (mute == "0")
                    {
                        PlaySound();
                    }
                    else
                    {
                        StopSound();
                    }
                    Storyboard_dapanD.Begin();
                    dapan_D2.Visibility = Visibility.Visible;
                    dapan_B2.Visibility = Visibility.Collapsed;
                    Scheduler.Dispatcher.Schedule(thuaCuoc, TimeSpan.FromSeconds(5));
                }
            }
        }

        private void daC_MouseEnter_1(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Sound("ping.wav");
            if (mute == "0")
            {
                PlaySound();
            }
            else
            {
                StopSound();
            }
            MessageBoxResult result = MessageBox.Show("Đáp án cuối cùng của bạn là C?", "Lựa chọn đáp án", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                countdown_timer.Stop();
                dapan_C2.Visibility = Visibility.Visible;
                daA.IsHitTestVisible = false;
                daB.IsHitTestVisible = false;
                daC.IsHitTestVisible = false;
                daD.IsHitTestVisible = false;
                nammuoi_nammuoi1.IsHitTestVisible = false;
                goidien1.IsHitTestVisible = false;
                hoikhangia1.IsHitTestVisible = false;
                back1.IsHitTestVisible = false;
                choice = 2;
                Sound("ans_c.wav");
                if (mute == "0")
                {
                    PlaySound();
                }
                else
                {
                    StopSound();
                }
                Scheduler.Dispatcher.Schedule(OnTimeDone, TimeSpan.FromSeconds(4));
                Scheduler.Dispatcher.Schedule(CorrectAnswerC, TimeSpan.FromSeconds(8));
            }
            else
                return;
        }

        private void CorrectAnswerC()
        {
            if (ans_numb == choice)
            {
                Sound("d_3.wav");
                if (mute == "0")
                {
                    PlaySound();
                }
                else
                {
                    StopSound();
                }
                if (number == 5)
                {
                    Scheduler.Dispatcher.Schedule(sound_correctAnswer5, TimeSpan.FromSeconds(2));
                }
                else if (number == 9)
                {
                    Scheduler.Dispatcher.Schedule(sound_correctAnswer9, TimeSpan.FromSeconds(3));
                }
                else if (number == 10)
                {
                    Scheduler.Dispatcher.Schedule(sound_correctAnswer10, TimeSpan.FromSeconds(2));
                }
                else if (number == 11)
                {
                    Scheduler.Dispatcher.Schedule(sound_correctAnswer11, TimeSpan.FromSeconds(2));
                }
                else if (number == 12)
                {
                    Scheduler.Dispatcher.Schedule(sound_correctAnswer12, TimeSpan.FromSeconds(2));
                }
                else if (number == 13)
                {
                    Scheduler.Dispatcher.Schedule(sound_correctAnswer13, TimeSpan.FromSeconds(2));
                }
                else if (number == 14)
                {
                    Scheduler.Dispatcher.Schedule(sound_correctAnswer14, TimeSpan.FromSeconds(2));
                }

                else
                {
                    number++;
                    load_cauhoi_1();
                }
            }
            else
            {
                if (ans_numb == 0)
                {
                    Sound("lose_a.wav");
                    if (mute == "0")
                    {
                        PlaySound();
                    }
                    else
                    {
                        StopSound();
                    }
                    Storyboard_dapanA.Begin();
                    dapan_A2.Visibility = Visibility.Visible;
                    dapan_C2.Visibility = Visibility.Collapsed;
                    Scheduler.Dispatcher.Schedule(thuaCuoc, TimeSpan.FromSeconds(5));
                }
                else if (ans_numb == 1)
                {
                    Sound("lose_b.wav");
                    if (mute == "0")
                    {
                        PlaySound();
                    }
                    else
                    {
                        StopSound();
                    }
                    Storyboard_dapanB.Begin();
                    dapan_B2.Visibility = Visibility.Visible;
                    dapan_C2.Visibility = Visibility.Collapsed;
                    Scheduler.Dispatcher.Schedule(thuaCuoc, TimeSpan.FromSeconds(5));
                }
                else
                {
                    Sound("lose_d.wav");
                    if (mute == "0")
                    {
                        PlaySound();
                    }
                    else
                    {
                        StopSound();
                    }
                    Storyboard_dapanD.Begin();
                    dapan_D2.Visibility = Visibility.Visible;
                    dapan_C2.Visibility = Visibility.Collapsed;
                    Scheduler.Dispatcher.Schedule(thuaCuoc, TimeSpan.FromSeconds(5));
                }
            }
        }

        private void daD_MouseEnter_1(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Sound("ping.wav");
            if (mute == "0")
            {
                PlaySound();
            }
            else
            {
                StopSound();
            }
            MessageBoxResult result = MessageBox.Show("Đáp án cuối cùng của bạn là D?", "Lựa chọn đáp án", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                countdown_timer.Stop();
                dapan_D2.Visibility = Visibility.Visible;
                daA.IsHitTestVisible = false;
                daB.IsHitTestVisible = false;
                daC.IsHitTestVisible = false;
                daD.IsHitTestVisible = false;
                nammuoi_nammuoi1.IsHitTestVisible = false;
                goidien1.IsHitTestVisible = false;
                hoikhangia1.IsHitTestVisible = false;
                back1.IsHitTestVisible = false;
                choice = 3;
                Sound("ans_d.wav");
                if (mute == "0")
                {
                    PlaySound();
                }
                else
                {
                    StopSound();
                }
                Scheduler.Dispatcher.Schedule(OnTimeDone, TimeSpan.FromSeconds(4));
                Scheduler.Dispatcher.Schedule(CorrectAnswerD, TimeSpan.FromSeconds(8));
            }
            else
                return;
        }

        private void CorrectAnswerD()
        {
            if (ans_numb == choice)
            {
                Sound("d_4.wav");
                if (mute == "0")
                {
                    PlaySound();
                }
                else
                {
                    StopSound();
                }
                if (number == 5)
                {
                    Scheduler.Dispatcher.Schedule(sound_correctAnswer5, TimeSpan.FromSeconds(2));
                }
                else if (number == 9)
                {
                    Scheduler.Dispatcher.Schedule(sound_correctAnswer9, TimeSpan.FromSeconds(3));
                }
                else if (number == 10)
                {
                    Scheduler.Dispatcher.Schedule(sound_correctAnswer10, TimeSpan.FromSeconds(2));
                }
                else if (number == 11)
                {
                    Scheduler.Dispatcher.Schedule(sound_correctAnswer11, TimeSpan.FromSeconds(2));
                }
                else if (number == 12)
                {
                    Scheduler.Dispatcher.Schedule(sound_correctAnswer12, TimeSpan.FromSeconds(2));
                }
                else if (number == 13)
                {
                    Scheduler.Dispatcher.Schedule(sound_correctAnswer13, TimeSpan.FromSeconds(2));
                }
                else if (number == 14)
                {
                    Scheduler.Dispatcher.Schedule(sound_correctAnswer14, TimeSpan.FromSeconds(2));
                }

                else
                {
                    number++;
                    load_cauhoi_1();
                }
            }
            else
            {
                if (ans_numb == 0)
                {
                    Sound("lose_a.wav");
                    if (mute == "0")
                    {
                        PlaySound();
                    }
                    else
                    {
                        StopSound();
                    }
                    Storyboard_dapanA.Begin();
                    dapan_A2.Visibility = Visibility.Visible;
                    dapan_D2.Visibility = Visibility.Collapsed;
                    Scheduler.Dispatcher.Schedule(thuaCuoc, TimeSpan.FromSeconds(5));
                }
                else if (ans_numb == 1)
                {
                    Sound("lose_b.wav");
                    PlaySound(); if (mute == "0")
                    {
                        PlaySound();
                    }
                    else
                    {
                        StopSound();
                    } Storyboard_dapanB.Begin();
                    dapan_B2.Visibility = Visibility.Visible;
                    dapan_D2.Visibility = Visibility.Collapsed;
                    Scheduler.Dispatcher.Schedule(thuaCuoc, TimeSpan.FromSeconds(5));
                }
                else
                {
                    Sound("lose_c.wav");
                    if (mute == "0")
                    {
                        PlaySound();
                    }
                    else
                    {
                        StopSound();
                    }
                    Storyboard_dapanC.Begin();
                    dapan_C2.Visibility = Visibility.Visible;
                    dapan_D2.Visibility = Visibility.Collapsed;
                    Scheduler.Dispatcher.Schedule(thuaCuoc, TimeSpan.FromSeconds(5));
                }
            }
        }
        #endregion

        private void thuaCuoc()
        {
            Sound("dungcuocchoi.wav");
            if (mute == "0")
            {
                PlaySound();
            }
            else
            {
                StopSound();
            }
            if (number == 1)
            {
                MessageBoxResult lose = MessageBox.Show("Quay trở về Menu", "Thua cuộc", MessageBoxButton.OK);
                NavigationService.GoBack();
            }
            else
            {
                MessageBoxResult end = MessageBox.Show("Bạn có muốn ghi điểm không?", "Thua cuộc", MessageBoxButton.OKCancel);
                if (end == MessageBoxResult.OK)
                {
                    var input = new InputPrompt();
                    input.Title = "Ghi danh";
                    input.Message = "Nhập tên của bạn ";
                    input.Show();
                    input.Completed += input_Completed;
                }
                else
                    NavigationService.GoBack();
            }
        }

        private void thangCuoc()
        {
            MessageBoxResult end = MessageBox.Show("Bạn có muốn ghi điểm không?", "Thắng cuộc", MessageBoxButton.OKCancel);
            if (end == MessageBoxResult.OK)
            {
                var input = new InputPrompt();
                input.Title = "Ghi danh";
                input.Message = "Nhập tên của bạn ";
                input.Show();
                input.Completed += input_Completed;
            }
            else
                NavigationService.GoBack();
        }

        void input_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            var h = new Database.HighScore();
            h.Id = ConnectDatabase.GetHighScore().Count + 1;
            h.Name = e.Result;
            h.Score = money.ElementAt(number - 2);
            ConnectDatabase.SaveScore(h);
            NavigationService.GoBack();
        }

        private void nammuoi_nammuoi1_MouseEnter_1(object sender, System.Windows.Input.MouseEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Bạn có muốn sử dụng quyền trợ giúp này","Trợ giúp năm mươi năm mươi", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                Sound("sound5050.wav");
                if (mute == "0")
                {
                    PlaySound();
                }
                else
                {
                    StopSound();
                }
                countdown_timer.Stop();
                nammuoi_nammuoi2.Opacity = 1;
                nammuoi_nammuoi1.IsHitTestVisible = false;
                Scheduler.Dispatcher.Schedule(help_50_50, TimeSpan.FromSeconds(4));
            }
            else
            {
                return;
            }
        }

        private void help_50_50()
        {
            nammuoi_nammuoi1.Opacity = 0;
            nammuoi_nammuoi2.Opacity = 0;
            nammuoi_nammuoi3.Opacity = 1;
            countdown_timer.Start();
            #region
            if (ans_numb == 0)
            {
                mien_gia_tri = new List<int> { 1, 2, 3 };
                n1 = rand.Next(1, 3);
                mien_gia_tri.RemoveAt(n1);
                n2 = rand.Next(mien_gia_tri[0], mien_gia_tri[1]);
                if ((n1 == 1 && n2 == 2) || (n2 == 1 && n1 == 2))
                {
                    Sound("cb.wav");
                    if (mute == "0")
                    {
                        PlaySound();
                    }
                    else
                    {
                        StopSound();
                    }
                    daB.Opacity = 0;
                    daB.IsHitTestVisible = false;
                    daC.Opacity = 0;
                    daC.IsHitTestVisible = false;
                }
                else if ((n1 == 1 && n2 == 3) || (n1 == 3 && n2 == 1))
                {
                    Sound("bd.wav");
                    if (mute == "0")
                    {
                        PlaySound();
                    }
                    else
                    {
                        StopSound();
                    }
                    daB.Opacity = 0;
                    daB.IsHitTestVisible = false;
                    daD.Opacity = 0;
                    daD.IsHitTestVisible = false;
                }
                else
                {
                    Sound("cd.wav");
                    if (mute == "0")
                    {
                        PlaySound();
                    }
                    else
                    {
                        StopSound();
                    }
                    daC.Opacity = 0;
                    daC.IsHitTestVisible = false;
                    daD.Opacity = 0;
                    daD.IsHitTestVisible = false;
                }
                    
            }
            else if (ans_numb == 1)
            {
                mien_gia_tri = new List<int> { 0, 2, 3 };
                n1 = rand.Next(1, 3);
                tmp = mien_gia_tri.ElementAt(n1);
                mien_gia_tri.RemoveAt(n1);
                n2 = rand.Next(mien_gia_tri[0], mien_gia_tri[1]);
                if ((tmp == 0 && n2 == 2) || (tmp == 2 && n2 == 0))
                {
                    Sound("ac.wav");
                    if (mute == "0")
                    {
                        PlaySound();
                    }
                    else
                    {
                        StopSound();
                    }
                    daA.Opacity = 0;
                    daA.IsHitTestVisible = false;
                    daC.Opacity = 0;
                    daC.IsHitTestVisible = false;
                }
                else if (tmp == 0 && n2 == 3 || tmp == 3 && n2 == 0)
                {
                    Sound("ad.wav");
                    if (mute == "0")
                    {
                        PlaySound();
                    }
                    else
                    {
                        StopSound();
                    }
                    daA.Opacity = 0;
                    daA.IsHitTestVisible = false;
                    daD.Opacity = 0;
                    daD.IsHitTestVisible = false;
                }
                else
                {
                    Sound("cd.wav");
                    if (mute == "0")
                    {
                        PlaySound();
                    }
                    else
                    {
                        StopSound();
                    }
                    daC.Opacity = 0;
                    daC.IsHitTestVisible = false;
                    daD.Opacity = 0;
                    daD.IsHitTestVisible = false;
                }
                
            }
            else if (ans_numb == 2)
            {
                mien_gia_tri = new List<int> { 0, 1, 3 };
                n1 = rand.Next(1, 3);
                tmp = mien_gia_tri.ElementAt(n1);
                mien_gia_tri.RemoveAt(n1);
                n2 = rand.Next(mien_gia_tri[0], mien_gia_tri[1]);
                if (tmp == 0 && n2 == 1 || tmp == 1 && n2 == 0 )
                {
                    Sound("ab.wav");
                    if (mute == "0")
                    {
                        PlaySound();
                    }
                    else
                    {
                        StopSound();
                    }
                    daA.Opacity = 0;
                    daA.IsHitTestVisible = false;
                    daB.Opacity = 0;
                    daB.IsHitTestVisible = false;
                }
                else if (tmp == 0 && n2 == 3 || tmp == 3 && n2 == 0)
                {
                    Sound("ad.wav");
                    if (mute == "0")
                    {
                        PlaySound();
                    }
                    else
                    {
                        StopSound();
                    }
                    daA.Opacity = 0;
                    daA.IsHitTestVisible = false;
                    daD.Opacity = 0;
                    daD.IsHitTestVisible = false;
                }
                else
                {
                    Sound("bd.wav");
                    if (mute == "0")
                    {
                        PlaySound();
                    }
                    else
                    {
                        StopSound();
                    }
                    daB.Opacity = 0;
                    daB.IsHitTestVisible = false;
                    daD.Opacity = 0;
                    daD.IsHitTestVisible = false;
                }
            
            }
            else if (ans_numb == 3)
            {
                mien_gia_tri = new List<int> { 0, 1, 2 };
                n1 = rand.Next(1, 3);
                tmp = mien_gia_tri.ElementAt(n1);
                mien_gia_tri.RemoveAt(n1);
                n2 = rand.Next(mien_gia_tri[0], mien_gia_tri[1]);
                if (tmp == 0 && n2 == 1 || tmp == 1 && n2 == 0)
                {
                    Sound("ab.wav");
                    if (mute == "0")
                    {
                        PlaySound();
                    }
                    else
                    {
                        StopSound();
                    }
                    daA.Opacity = 0;
                    daA.IsHitTestVisible = false;
                    daB.Opacity = 0;
                    daB.IsHitTestVisible = false;
                }
                else if (tmp == 0 && n2 == 2 || tmp == 2 && n2 == 0)
                {
                    Sound("ac.wav");
                    if (mute == "0")
                    {
                        PlaySound();
                    }
                    else
                    {
                        StopSound();
                    }
                    daA.Opacity = 0;
                    daA.IsHitTestVisible = false;
                    daC.Opacity = 0;
                    daC.IsHitTestVisible = false;
                }
                else
                {
                    Sound("cb.wav");
                    if (mute == "0")
                    {
                        PlaySound();
                    }
                    else
                    {
                        StopSound();
                    }
                    daB.Opacity = 0;
                    daB.IsHitTestVisible = false;
                    daC.Opacity = 0;
                    daC.IsHitTestVisible = false;
                }
            #endregion  
            }
        }

        private void hoikhangia1_MouseEnter_1(object sender, System.Windows.Input.MouseEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Bạn có muốn sử dụng quyền trợ giúp này", "Trợ giúp từ khán giả", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                hoikhangia2.Opacity = 1;
                hoikhangia1.IsHitTestVisible = false;
                Sound("audience.wav");
                if (mute == "0")
                {
                    PlaySound();
                }
                else
                {
                    StopSound();
                }
                countdown_timer.Stop();
                prompt1.Body = "*Khán giả đang trả lời*";
                prompt1.IsEnabled = false;
                prompt1.Show();
                Scheduler.Dispatcher.Schedule(help_khan_gia, TimeSpan.FromSeconds(9));
            }
            else
                return;
        }

        private void help_khan_gia()
        {
            hoikhangia1.Opacity = 0;
            hoikhangia2.Opacity = 0;
            hoikhangia3.Opacity = 1;

            prompt1.Title = "Ý kiến khán giả";
            prompt1.VersionNumber = "";

            tbA1.Width = 300;
            tbA1.Text = "Đáp án A: ";
            btA1.Background = new SolidColorBrush(System.Windows.Media.Colors.Red);

            tbB1.Width = 300;
            tbB1.Text = "Đáp án B: ";
            btB1.Background = new SolidColorBrush(System.Windows.Media.Colors.Red);

            tbC1.Width = 300;
            tbC1.Text = "Đáp án C: ";
            btC1.Background = new SolidColorBrush(System.Windows.Media.Colors.Red);

            tbD1.Width = 300;
            tbD1.Text = "Đáp án D: ";
            btD1.Background = new SolidColorBrush(System.Windows.Media.Colors.Red);

            if (ans_numb == 0)
            {
                t1 = rand.Next(60, 82);
                btA1.Width = t1 * 5;
                tbA1.Text += t1.ToString() + "%";

                t2 = rand.Next(1, 100 - t1);
                btB1.Width = t2 * 5;
                tbB1.Text += t2.ToString() + "%";

                t3 = rand.Next(1, 100 - t1 - t2);
                btC1.Width = t3 * 5;
                tbC1.Text += t3.ToString() + "%";

                t4 = rand.Next(1, 100 - t1 - t2 - t3);
                btD1.Width = t4 * 5;
                tbD1.Text += t4.ToString() + "%";
            }
            else if (ans_numb == 1)
            {
                t2 = rand.Next(60, 82);
                btB1.Width = t2 * 5;
                tbB1.Text += t2.ToString() + "%";

                t1 = rand.Next(1, 100 - t2);
                btA1.Width = t1 * 5;
                tbA1.Text += t1.ToString() + "%";

                t3 = rand.Next(1, 100 - t1 - t2);
                btC1.Width = t3 * 5;
                tbC1.Text += t3.ToString() + "%";

                t4 = rand.Next(1, 100 - t1 - t2 - t3);
                btD1.Width = t4 * 5;
                tbD1.Text += t4.ToString() + "%";
            }
            else if (ans_numb == 2)
            {
                t3 = rand.Next(60, 82);
                btC1.Width = t3 * 5;
                tbC1.Text += t3.ToString() + "%";

                t2 = rand.Next(1, 100 - t3);
                btB1.Width = t2 * 5;
                tbB1.Text += t2.ToString() + "%";

                t1 = rand.Next(1, 100 - t3 - t2);
                btA1.Width = t1 * 5;
                tbA1.Text += t1.ToString() + "%";

                t4 = rand.Next(1, 100 - t1 - t2 - t3);
                btD1.Width = t4 * 5;
                tbD1.Text += t4.ToString() + "%";
            }
            else
            {
                t4 = rand.Next(60, 82);
                btD1.Width = t4 * 5;
                tbD1.Text += t4.ToString() + "%";

                t2 = rand.Next(1, 100 - t4);
                btB1.Width = t2 * 5;
                tbB1.Text += t2.ToString() + "%";

                t3 = rand.Next(1, 100 - t4 - t2);
                btC1.Width = t3 * 5;
                tbC1.Text += t3.ToString() + "%";

                t1 = rand.Next(1, 100 - t4 - t2 - t3);
                btA1.Width = t1 * 5;
                tbA1.Text += t1.ToString() + "%";
            }

            ListBox lb1 = new ListBox();
            lb1.Height = 300;
            lb1.Width = 400;
            lb1.Items.Add(tbA1);
            lb1.Items.Add(btA1);
            lb1.Items.Add(tbB1);
            lb1.Items.Add(btB1);
            lb1.Items.Add(tbC1);
            lb1.Items.Add(btC1);
            lb1.Items.Add(tbD1);
            lb1.Items.Add(btD1);
            prompt1.Body = lb1;
            prompt1.IsEnabled = true;
            prompt1.Completed += prompt1_Completed;
        }

        void prompt1_Completed(object sender, PopUpEventArgs<object, PopUpResult> e)
        {
            countdown_timer.Start();
        }

        private void goidien1_MouseEnter_1(object sender, System.Windows.Input.MouseEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Bạn có muốn sử dụng quyền trợ giúp này?","Hỏi ý kiến chuyên gia",MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                goidien2.Opacity = 1;
                goidien1.IsHitTestVisible = false;
                countdown_timer.Stop();
                Scheduler.Dispatcher.Schedule(help_goi_dien, TimeSpan.FromSeconds(0.5));
            }
        }

        private void help_goi_dien()
        {
            goidien1.Opacity = 0;
            goidien2.Opacity = 0;
            goidien3.Opacity = 1;
            im1.Source = new BitmapImage(new Uri("/Images/help_1.png", UriKind.Relative));
            im1.MouseEnter += im1_MouseEnter;
            im2.Source = new BitmapImage(new Uri("/Images/help_2.png", UriKind.Relative));
            im2.MouseEnter += im2_MouseEnter;
            im3.Source = new BitmapImage(new Uri("/Images/help_3.png", UriKind.Relative));
            im3.MouseEnter += im3_MouseEnter;
            im4.Source = new BitmapImage(new Uri("/Images/help_4.png", UriKind.Relative));
            im4.MouseEnter += im4_MouseEnter;
            im5.Source = new BitmapImage(new Uri("/Images/help_5.png", UriKind.Relative));
            im5.MouseEnter += im5_MouseEnter;
            im6.Source = new BitmapImage(new Uri("/Images/help_6.png", UriKind.Relative));
            im6.MouseEnter += im6_MouseEnter;
            
            StackPanel stack = new StackPanel();
            stack.Orientation = System.Windows.Controls.Orientation.Horizontal;
            stack.Children.Add(im1);
            stack.Children.Add(im2);
            stack.Children.Add(im3);
            StackPanel stack1 = new StackPanel();
            stack1.Orientation = System.Windows.Controls.Orientation.Horizontal;
            stack1.Children.Add(im4);
            stack1.Children.Add(im5);
            stack1.Children.Add(im6);
            lb.Items.Add(stack);
            lb.Items.Add(stack1);
            prompt.FontSize = 20;
            prompt.Title = "Hỏi ý kiến chuyên gia";
            prompt.VersionNumber = "";
            prompt.Body = lb;
            prompt.Show();
        }

        void im6_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            prompt.Title = "Chuyên gia trợ giúp";
            Scheduler.Dispatcher.Schedule(help_expert, TimeSpan.FromSeconds(1));
        }

        void im5_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            prompt.Title = "Chuyên gia trợ giúp";
            Scheduler.Dispatcher.Schedule(help_expert, TimeSpan.FromSeconds(1));
        }

        void im4_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            prompt.Title = "Chuyên gia trợ giúp";
            Scheduler.Dispatcher.Schedule(help_expert,TimeSpan.FromSeconds(1));
        }

        void im3_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            prompt.Title = "Chuyên gia trợ giúp";
            Scheduler.Dispatcher.Schedule(help_expert, TimeSpan.FromSeconds(1));
        }

        void im2_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            prompt.Title = "Chuyên gia trợ giúp";
            Scheduler.Dispatcher.Schedule(help_expert, TimeSpan.FromSeconds(1));   
        }

        void im1_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            prompt.Title = "Chuyên gia trợ giúp";
            Scheduler.Dispatcher.Schedule(help_expert, TimeSpan.FromSeconds(1));
        }

        private void help_expert()
        {
            prompt.Body = "*Vui lòng đợi trong giây lát*";
            prompt.IsEnabled = false;
            Sound("call.wav");
            PlaySound();
            Scheduler.Dispatcher.Schedule(expert_answer, TimeSpan.FromSeconds(10));
        }

        private void expert_answer()
        {
            if (ans_numb == 0)
            {
                prompt.Body = "Tôi xin trợ giúp cho bạn phương án A";
                prompt.IsEnabled = true;
            }
            else if (ans_numb == 1)
            {
                prompt.Body = "Tôi xin trợ giúp cho bạn phương án B";
                prompt.IsEnabled = true;
            }
            else if (ans_numb == 2)
            {
                prompt.Body = "Tôi xin trợ giúp cho bạn phương án C";
                prompt.IsEnabled = true;
            }
            else
            {
                prompt.Body = "Tôi xin trợ giúp cho bạn phương án D";
                prompt.IsEnabled = true;
            }
                
            countdown_timer.Start();
            StopSound();
        }

        private void back1_MouseEnter_1(object sender, System.Windows.Input.MouseEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Bạn có muốn sử dụng quyền trợ giúp này không?", "Đổi câu hỏi khác",MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                back1.Opacity = 0;
                back2.Opacity = 0;
                load_cauhoi_1();
                back3.Opacity = 1;
            }
            else
                return;
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult res = MessageBox.Show("Bạn có muốn quay trở về Menu?", "Exit?", MessageBoxButton.OKCancel);
            if (res == MessageBoxResult.OK)
            {
                NavigationService.GoBack();
                countdown_timer.Stop();
                base.OnBackKeyPress(e);
            }
            else
                return;
        }
    }
}