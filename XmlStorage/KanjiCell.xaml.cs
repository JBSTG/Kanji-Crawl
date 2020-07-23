using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace XmlStorage
{
    /// <summary>
    /// Interaction logic for KanjiCell.xaml
    /// </summary>
    public partial class KanjiCell : UserControl
    {
        public KanjiCell()
        {
            InitializeComponent();
        }
        public KanjiCell(string character)
        {
            InitializeComponent();
            value.Text = character;
        }


    }
}
