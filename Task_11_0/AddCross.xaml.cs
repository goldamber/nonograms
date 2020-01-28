using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Task_11_0
{
    public partial class AddCross : Window
    {
        SolidColorBrush _prev = new SolidColorBrush();
        int _height = 0;
        int _width = 0;
        int _col = 0;
        int _row = 0;

        public AddCross()
        {
            InitializeComponent();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            grStart.Visibility = Visibility.Collapsed;
            grPictureSet.Visibility = Visibility.Visible;
        }
        private void txtHeight_TextChanged(object sender, TextChangedEventArgs e)
        {
            Regex reg = new Regex("^[0-9]+$");
            if (!reg.IsMatch((sender as TextBox).Text))
            {
                (sender as TextBox).BorderBrush = Brushes.Red;
                (sender as TextBox).BorderThickness = new Thickness(1);
                (sender as TextBox).Text = "";
            }
            else
            {
                (sender as TextBox).BorderBrush = Brushes.Silver;
                (sender as TextBox).BorderThickness = new Thickness(1);
            }

            try
            {
                if (Convert.ToInt32(txtHeight.Text) != 0 && Convert.ToInt32(txtWidth.Text) != 0)
                {
                    _height = Convert.ToInt32(txtHeight.Text);
                    _width = Convert.ToInt32(txtWidth.Text);
                    btnNext.IsEnabled = true;
                }
            }
            catch { }
        }
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            grPictureSet.Visibility = Visibility.Collapsed;
            grCrossGenerate.Visibility = Visibility.Visible;

            GenerateField();
        }

        void GenerateField()
        {
            Grid tmp = new Grid();

            for (int i = 0; i < _height; i++)
            {
                tmp.RowDefinitions.Add(new RowDefinition());
            }
            for (int j = 0; j < _width; j++)
            {
                tmp.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
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
            grBody.Children.Add(tmp);
        }

        private void Lb_MouseLeave(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                (sender as Label).BorderBrush = Brushes.Gray;
            else
                (sender as Label).Background = _prev;
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
            txtName_TextChanged(null, null);
        }
        private void Lb_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _col = Grid.GetColumn(sender as Label);
            _row = Grid.GetRow(sender as Label);
            (sender as Label).Background = (e.ChangedButton == MouseButton.Left) ? Brushes.Gray : Brushes.Transparent;
            txtName_TextChanged(null, null);
        }

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool empty = true;

            foreach (Label item in (grBody.Children[0] as Grid).Children)
            {
                if (item.Background == Brushes.Gray)
                {
                    empty = false;
                    break;
                }
            }

            btnApply.IsEnabled = txtName.Text.Length > 0 && !empty;
        }
        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            List<Cross> lst = null;
            if (File.Exists("Cross.xml"))
            {
                XmlSerializer xmlFormat = new XmlSerializer(typeof(List<Cross>));

                try
                {
                    using (Stream fStream = File.OpenRead("Cross.xml"))
                    {
                        lst = (List<Cross>)xmlFormat.Deserialize(fStream);
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
            else
                lst = new List<Cross>();

            Cross result = new Cross { Name = txtName.Text, CGroup = (CrossGroup)Enum.Parse(typeof(CrossGroup), cmbGroup.Text) };
            int count;

            if (lst.Contains(result))
                MessageBox.Show("This name is already exists!", "Wrong data");
            else
            {
                for (int i = 0; i < _width; i++)
                {
                    List<int> tmp = new List<int>();
                    count = 0;

                    foreach (Label item in (grBody.Children[0] as Grid).Children)
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
                for (int i = 0; i < _height; i++)
                {
                    List<int> tmp = new List<int>();
                    count = 0;

                    foreach (Label item in (grBody.Children[0] as Grid).Children)
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
                lst.Add(result);

                try
                {
                    XmlSerializer xmlFormat = new XmlSerializer(typeof(List<Cross>));
                    using (Stream fStream = File.Create("Cross.xml"))
                    {
                        xmlFormat.Serialize(fStream, lst);
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }

                Close();
            }
        }
    }
}