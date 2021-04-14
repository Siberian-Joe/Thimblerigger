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

namespace Thimbles
{
    /// <summary>
    /// Логика взаимодействия для Thimble.xaml
    /// </summary>
    public partial class Thimble : UserControl
    {
        public int position { get; set; }
        public static bool finish { get; set; }
        public bool ball { get; set; }
        public Thimble()
        {
            InitializeComponent();
            position = 0;
        }
    }
}
