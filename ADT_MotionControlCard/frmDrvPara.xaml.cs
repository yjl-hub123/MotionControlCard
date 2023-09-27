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
using System.Windows.Shapes;

namespace ADT_MotionControlCard
{
    /// <summary>
    /// frmDrvPara.xaml 的交互逻辑
    /// </summary>
    public partial class frmDrvPara : Window
    {
        public bool dialogResult;
        public frmDrvPara(string startv, string maxv, string acc, string dec, string admode, string target)
        {
            InitializeComponent();

            textStartV.Text = startv;
            textMaxV.Text = maxv;
            textAcc.Text = acc;
            textDec.Text = dec;
            string[] strings = { "S型", "T型", "EXP型", "COS型" };
            cbAdmode.ItemsSource = strings;
            cbAdmode.SelectedItem = admode;
            textTarget.Text = target;

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

         private void Button_Click(object sender, RoutedEventArgs e)
        {
            dialogResult= true;
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
