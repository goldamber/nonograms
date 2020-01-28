using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace Task_11_0
{
    public partial class MainWindow : Window
    {
        List<Cross> crossLst;
        Cross chCross = new Cross();
        SolidColorBrush _prev = new SolidColorBrush();
        int _col = 0;
        int _row = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Cross_MouseDown(object sender, MouseButtonEventArgs e)
        {
            grStart.Visibility = Visibility.Collapsed;
            grCrossList.Visibility = Visibility.Visible;
            GenerateList();
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddCross frm = new AddCross();
            frm.ShowDialog();
            GenerateList();
        }

        void GenerateList()
        {
            crossLst = Read("Cross");
            wrList.Children.Clear();

            foreach (Cross item in crossLst)
            {
                Border br = new Border { BorderBrush = Brushes.Silver, BorderThickness = new Thickness(2), Margin = new Thickness(5) };
                Grid gr = new Grid();
                StackPanel sp = new StackPanel { Width = 70 };

                sp.Children.Add(new Image { Width = 30, Margin = new Thickness(0, 15, 0, 5), Source = new BitmapImage(new Uri($"pack://application:,,,/Icons/{item.CGroup.ToString()}.png")) });
                sp.Children.Add(new Label { Content = item.Name, FontWeight = FontWeights.Bold, FontSize = 16, HorizontalContentAlignment = HorizontalAlignment.Center });
                sp.Children.Add(new Label { Content = $"{item.vArr.Count} x {item.hArr.Count}", HorizontalContentAlignment = HorizontalAlignment.Center });

                Grid close = new Grid { Visibility = Visibility.Collapsed };
                Ellipse el = new Ellipse { Fill = Brushes.Red, Margin = new Thickness(5), Width = 20, Height = 20, VerticalAlignment = VerticalAlignment.Top, HorizontalAlignment = HorizontalAlignment.Right };
                Label clLab = new Label { Content = "X", Margin = new Thickness(0, 2, 6, 0), Foreground = Brushes.White, FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Top, HorizontalAlignment = HorizontalAlignment.Right };
                clLab.MouseDown += El_MouseDown;
                close.Children.Add(el);
                close.Children.Add(clLab);

                gr.Children.Add(sp);
                gr.Children.Add(new Image { Width = 50, Source = new BitmapImage(new Uri("pack://application:,,,/Icons/Checked.png")), Visibility = item.Sloved ? Visibility.Visible : Visibility.Collapsed });
                gr.Children.Add(close);
                gr.MouseEnter += Sp_MouseEnter;
                gr.MouseLeave += Sp_MouseLeave;
                gr.MouseDown += Gr_MouseDown;
                br.Child = gr;
                wrList.Children.Add(br);
            }
        }
        void GenerateCrossword()
        {
            grCrossBody.Children.Clear();
            grVNumb.Children.Clear();
            grHNumb.Children.Clear();

            Grid tmp = new Grid();

            for (int i = 0; i < chCross.vArr.Count; i++)
            {
                tmp.RowDefinitions.Add(new RowDefinition());
            }
            for (int j = 0; j < chCross.hArr.Count; j++)
            {
                tmp.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < chCross.hArr.Count; i++)
            {
                for (int j = 0; j < chCross.vArr.Count; j++)
                {
                    Label lb = new Label { BorderBrush = Brushes.Gray };
                    lb.BorderThickness = new Thickness((i % 5 == 0 && i != 0) ? 3 : 0.5, (j % 5 == 0 && j != 0) ? 3 : 0.5, 0.5, 0.5);
                    lb.MouseDown += Lb_MouseDown;
                    lb.MouseEnter += Lb_MouseMove;
                    lb.MouseLeave += Lb_MouseLeave;
                    lb.MouseUp += Lb_MouseUp;

                    Grid.SetColumn(lb, i);
                    Grid.SetRow(lb, j);
                    tmp.Children.Add(lb);
                }
            }
            grCrossBody.Children.Add(tmp);

            int numHeight = 0;
            foreach (List<int> item in chCross.vArr)
            {
                if (item.Count > numHeight)
                    numHeight = item.Count;
            }
            tmp = new Grid();
            for (int i = 0; i < chCross.vArr.Count; i++)
            {
                tmp.RowDefinitions.Add(new RowDefinition());
            }
            for (int i = 0; i < numHeight; i++)
            {
                tmp.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < numHeight; i++)
            {
                for (int j = 0; j < chCross.vArr.Count; j++)
                {
                    try
                    {
                        Label lb = new Label { BorderBrush = Brushes.Sienna, Background = Brushes.LightYellow, VerticalContentAlignment = VerticalAlignment.Center, HorizontalContentAlignment = HorizontalAlignment.Center, Content = new TextBlock { FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, Text = chCross.vArr[j][i].ToString() } };
                        lb.BorderThickness = new Thickness(0.5, (j % 5 == 0 && j != 0) ? 2 : 0.5, 0.5, 0.5);

                        Grid.SetColumn(lb, i);
                        Grid.SetRow(lb, j);

                        tmp.Children.Add(lb);
                    }
                    catch
                    {
                        Label lb = new Label { BorderBrush = Brushes.Sienna, Background = Brushes.LightYellow, VerticalContentAlignment = VerticalAlignment.Center, HorizontalContentAlignment = HorizontalAlignment.Center, Content = new TextBlock { FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center } };
                        lb.BorderThickness = new Thickness(0.5, (j % 5 == 0 && j != 0) ? 2 : 0.5, 0.5, 0.5);

                        Grid.SetColumn(lb, i);
                        Grid.SetRow(lb, j);

                        tmp.Children.Add(lb);
                    }
                }
            }
            grHNumb.Children.Add(tmp);

            int numWidth = 0;
            foreach (List<int> item in chCross.hArr)
            {
                if (item.Count > numWidth)
                    numWidth = item.Count;
            }
            tmp = new Grid();
            for (int i = 0; i < chCross.hArr.Count; i++)
            {
                tmp.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < numWidth; i++)
            {
                tmp.RowDefinitions.Add(new RowDefinition());
            }
            for (int i = 0; i < chCross.hArr.Count; i++)
            {
                for (int j = 0; j < numWidth; j++)
                {
                    try
                    {
                        Label lb = new Label { BorderBrush = Brushes.Sienna, Background = Brushes.LightYellow, VerticalContentAlignment = VerticalAlignment.Center, HorizontalContentAlignment = HorizontalAlignment.Center, Content = new TextBlock { FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, Text = chCross.hArr[i][j].ToString() } };
                        lb.BorderThickness = new Thickness((i % 5 == 0 && i != 0) ? 2 : 0.5, 0.5, 0.5, 0.5);

                        Grid.SetColumn(lb, i);
                        Grid.SetRow(lb, j);
                        tmp.Children.Add(lb);
                    }
                    catch
                    {
                        Label lb = new Label { BorderBrush = Brushes.Sienna, Background = Brushes.LightYellow, VerticalContentAlignment = VerticalAlignment.Center, HorizontalContentAlignment = HorizontalAlignment.Center, Content = new TextBlock { FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center } };
                        lb.BorderThickness = new Thickness((i % 5 == 0 && i != 0) ? 2 : 0.5, 0.5, 0.5, 0.5);

                        Grid.SetColumn(lb, i);
                        Grid.SetRow(lb, j);
                        tmp.Children.Add(lb);
                    }
                }
            }
            grVNumb.Children.Add(tmp);
        }

        private void Gr_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            for (int i = 0; i < crossLst.Count; i++)
            {
                if (crossLst[i].Name == (((sender as Grid).Children[0] as StackPanel).Children[1] as Label).Content.ToString())
                {
                    chCross = crossLst[i];
                    break;
                }
            }

            if (chCross.Name != "")
            {
                grCrossList.Visibility = Visibility.Collapsed;
                grCrossSlove.Visibility = Visibility.Visible;
                GenerateCrossword();
            }
        }
        private void El_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            int ind = -1;
            for (int i = 0; i < crossLst.Count; i++)
            {
                if (crossLst[i].Name == (((((sender as Label).Parent as Grid).Parent as Grid).Children[0] as StackPanel).Children[1] as Label).Content.ToString())
                {
                    ind = i;
                    break;
                }
            }

            if (ind >= 0)
            {
                crossLst.RemoveAt(ind);
                Write("Cross");
                GenerateList();
            }
        }
        private void Sp_MouseLeave(object sender, MouseEventArgs e)
        {
            e.Handled = true;
            ((sender as Grid).Children[2] as Grid).Visibility = Visibility.Collapsed;
            (sender as Grid).Background = Brushes.Transparent;
        }
        private void Sp_MouseEnter(object sender, MouseEventArgs e)
        {
            e.Handled = true;
            ((sender as Grid).Children[2] as Grid).Visibility = Visibility.Visible;
            (sender as Grid).Background = Brushes.LightBlue;
        }

        private void Lb_MouseLeave(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                (sender as Label).BorderBrush = Brushes.Gray;
            else
                (sender as Label).Background = _prev;

            foreach (Label item in (grVNumb.Children[0] as Grid).Children)
            {
                item.Background = Brushes.LightYellow;
            }
            foreach (Label item in (grHNumb.Children[0] as Grid).Children)
            {
                item.Background = Brushes.LightYellow;
            }
        }
        private void Lb_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (_col == Grid.GetColumn(sender as Label))
                {
                    foreach (Label item in ((sender as Label).Parent as Grid).Children)
                    {
                        if (Grid.GetColumn(item) == _col)
                        {
                            if (Grid.GetRow(sender as Label) < _row)
                            {
                                if (Grid.GetRow(item) < _row && Grid.GetRow(item) >= Grid.GetRow(sender as Label))
                                    item.BorderBrush = Brushes.OrangeRed;
                            }
                            else
                            {
                                if (Grid.GetRow(item) > _row && Grid.GetRow(item) <= Grid.GetRow(sender as Label))
                                    item.BorderBrush = Brushes.OrangeRed;
                            }
                        }
                    }
                }
                else if (_row == Grid.GetRow(sender as Label))
                {
                    foreach (Label item in ((sender as Label).Parent as Grid).Children)
                    {
                        if (Grid.GetRow(item) == _row)
                        {
                            if (Grid.GetColumn(sender as Label) < _col)
                            {
                                if (Grid.GetColumn(item) < _col && Grid.GetColumn(item) >= Grid.GetColumn(sender as Label))
                                    item.BorderBrush = Brushes.OrangeRed;
                            }
                            else
                            {
                                if (Grid.GetColumn(item) > _col && Grid.GetColumn(item) <= Grid.GetColumn(sender as Label))
                                    item.BorderBrush = Brushes.OrangeRed;
                            }
                        }
                    }
                }
            }
            else
            {
                _prev = ((sender as Label).Background as SolidColorBrush);
                (sender as Label).Background = Brushes.LightBlue;

                foreach (Label item in (grVNumb.Children[0] as Grid).Children)
                {
                    if (Grid.GetColumn(item) == Grid.GetColumn((sender as Label)))
                        item.Background = Brushes.LightBlue;
                }
                foreach (Label item in (grHNumb.Children[0] as Grid).Children)
                {
                    if (Grid.GetRow(item) == Grid.GetRow((sender as Label)))
                        item.Background = Brushes.LightBlue;
                }
            }
        }
        private void Lb_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_col == Grid.GetColumn(sender as Label))
            {
                foreach (Label item in ((sender as Label).Parent as Grid).Children)
                {
                    if (Grid.GetColumn(item) == _col)
                    {
                        if (Grid.GetRow(sender as Label) < _row)
                        {
                            if(Grid.GetRow(item) < _row && Grid.GetRow(item) >= Grid.GetRow(sender as Label))
                                item.Background = (item.Background == Brushes.Transparent)? Brushes.Gray: Brushes.Transparent;
                        }
                        else
                        {
                            if (Grid.GetRow(item) > _row && Grid.GetRow(item) <= Grid.GetRow(sender as Label))
                                item.Background = (item.Background == Brushes.Transparent) ? Brushes.Gray : Brushes.Transparent;
                        }
                    }
                }
            }
            else if (_row == Grid.GetRow(sender as Label))
            {
                foreach (Label item in ((sender as Label).Parent as Grid).Children)
                {
                    if (Grid.GetRow(item) == _row)
                    {
                        if (Grid.GetColumn(sender as Label) < _col)
                        {
                            if (Grid.GetColumn(item) < _col && Grid.GetColumn(item) >= Grid.GetColumn(sender as Label))
                                item.Background = (item.Background == Brushes.Transparent) ? Brushes.Gray : Brushes.Transparent;
                        }
                        else
                        {
                            if (Grid.GetColumn(item) > _col && Grid.GetColumn(item) <= Grid.GetColumn(sender as Label))
                                item.Background = (item.Background == Brushes.Transparent) ? Brushes.Gray : Brushes.Transparent;
                        }
                    }
                }
            }

            foreach (Label item in ((sender as Label).Parent as Grid).Children)
            {
                item.BorderBrush = Brushes.Gray;
            }
            _prev = ((sender as Label).Background as SolidColorBrush);

            if(CheckWin())
            {
                SetMark();
                MessageBox.Show("Puzzle sloved.", "Well Done!");
                btnBack_Click(null, null);
            }
        }
        private void Lb_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _col = Grid.GetColumn(sender as Label);
            _row = Grid.GetRow(sender as Label);
            (sender as Label).Background = (e.ChangedButton == MouseButton.Left) ? Brushes.Gray : Brushes.Transparent;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            grCrossList.Visibility = Visibility.Visible;
            grCrossSlove.Visibility = Visibility.Collapsed;
            GenerateList();
        }

        List<Cross> Read(string filename)
        {
            List<Cross> lst = null;
            if (File.Exists($"{filename}.xml"))
            {
                XmlSerializer xmlFormat = new XmlSerializer(typeof(List<Cross>));

                try
                {
                    using (Stream fStream = File.OpenRead($"{filename}.xml"))
                    {
                        lst = (List<Cross>)xmlFormat.Deserialize(fStream);
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
            else
                lst = new List<Cross>();

            return lst;
        }
        void Write(string filename)
        {
            try
            {
                XmlSerializer xmlFormat = new XmlSerializer(typeof(List<Cross>));
                using (Stream fStream = File.Create($"{filename}.xml"))
                {
                    xmlFormat.Serialize(fStream, crossLst);
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        bool CheckWin()
        {
            Cross result = new Cross();
            int count;
            bool same = true;

            for (int i = 0; i < chCross.hArr.Count; i++)
            {
                List<int> tmp = new List<int>();
                count = 0;

                foreach (Label item in (grCrossBody.Children[0] as Grid).Children)
                {
                    if (Grid.GetColumn(item) == i)
                    {
                        if (item.Background != Brushes.Transparent)
                            count++;
                        else
                        {
                            if (count != 0)
                                tmp.Add(count);
                            count = 0;
                        }
                    }
                }

                if (count != 0)
                    tmp.Add(count);
                result.hArr.Add(tmp);
            }
            for (int i = 0; i < chCross.vArr.Count; i++)
            {
                List<int> tmp = new List<int>();
                count = 0;

                foreach (Label item in (grCrossBody.Children[0] as Grid).Children)
                {
                    if (Grid.GetRow(item) == i)
                    {
                        if (item.Background != Brushes.Transparent)
                            count++;
                        else
                        {
                            if (count != 0)
                                tmp.Add(count);
                            count = 0;
                        }
                    }
                }

                if (count != 0)
                    tmp.Add(count);
                result.vArr.Add(tmp);
            }    

            try
            {
                for (int i = 0; i < chCross.hArr.Count; i++)
                {
                    for (int j = 0; j < chCross.hArr[i].Count; j++)
                    {
                        if (chCross.hArr[i][j] != result.hArr[i][j])
                            same = false;
                    }
                }
            }
            catch { }
            try
            {
                for (int i = 0; i < chCross.vArr.Count; i++)
                {
                    for (int j = 0; j < chCross.vArr[i].Count; j++)
                    {
                        if (chCross.vArr[i][j] != result.vArr[i][j])
                            same = false;
                    }
                }

                return same;
            }
            catch { }

            return false;
        }
        void SetMark()
        {
            foreach (Cross item in crossLst)
            {
                if (item.Name == chCross.Name)
                {
                    chCross.Sloved = true;
                    break;
                }
            }
            Write("Cross");
        }
    }
}