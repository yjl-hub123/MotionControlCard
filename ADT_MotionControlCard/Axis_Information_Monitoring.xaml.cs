using ADT_CARD_632XE;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Threading;

namespace ADT_MotionControlCard
{
    /// <summary>
    /// Axis_Information_Monitoring.xaml 的交互逻辑
    /// </summary>
    public partial class Axis_Information_Monitoring : Window
    {
        public ObservableCollection<AxisInformation> AxisInformation { get; set; }

        private DispatcherTimer timer;
        public Axis_Information_Monitoring()
        {
            InitializeComponent();
            DataContext = this;

            AxisInformation = new ObservableCollection<AxisInformation>();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.1);
            timer.Tick += Timer_Tick;

            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            int axs_count = 0;  //总轴数
            int cmd = 0, enc = 0, stt = 0, enb = 0;  //逻辑位置  编码器位置  轴状态  使能
            double c_spd = 0, e_spd = 0, tgt = 0;   //逻辑速度  编码器速度  目标位置
            int lmtp_board = 0, lmtp_port = 0, lmtp_now_level = 0;    //节点号   输入端口   
            int lmtm_board = 0, lmtm_port = 0, lmtm_now_level = 0;
            int stp0_board = 0, stp0_port = 0, stp0_now_level = 0;
            adt_card_632xe.adt_get_total_axis(MainWindow.m_iCardIndex, out axs_count);
            if (axs_count>=1)
            {
                for (int i = 1; i <= axs_count; i++)
                {
                    AxisInformation[i - 1].Axis = i;

                    adt_card_632xe.adt_get_command_pos(MainWindow.m_iCardIndex, i, out cmd);//逻辑位置
                    if (AxisInformation[i - 1].LogicalPosition != cmd)
                        AxisInformation[i - 1].LogicalPosition = cmd;

                    adt_card_632xe.adt_get_actual_pos(MainWindow.m_iCardIndex, i, out enc);  //编码器位置
                    if (AxisInformation[i - 1].EncoderPosition != enc)
                        AxisInformation[i - 1].EncoderPosition = enc;

                    adt_card_632xe.adt_get_speed(MainWindow.m_iCardIndex, i, out c_spd);  //逻辑速度
                    if (AxisInformation[i - 1].LogicalSpeed != c_spd)
                        AxisInformation[i - 1].LogicalSpeed = c_spd;

                    adt_card_632xe.adt_get_encoder_speed(MainWindow.m_iCardIndex, i, out e_spd);  //编码器速度
                    if (AxisInformation[i - 1].EncoderSpeed != e_spd)
                        AxisInformation[i - 1].EncoderSpeed = e_spd;

                    adt_card_632xe.adt_get_axis_status(MainWindow.m_iCardIndex, i, out stt);  //轴状态
                    if (AxisInformation[i - 1].DriveStatus != stt)
                        AxisInformation[i - 1].DriveStatus = stt;

                    adt_card_632xe.adt_get_target_pos_unit(MainWindow.m_iCardIndex, i, out tgt);  //目标位置
                    if (AxisInformation[i - 1].TargetPosition != tgt)
                        AxisInformation[i - 1].TargetPosition = tgt;

                    adt_card_632xe.adt_get_axis_enable(MainWindow.m_iCardIndex, i, out enb);//使能
                    AxisInformation[i - 1].Enable = enb == 0 ? "未使能" : "使能";

                    adt_card_632xe.adt_get_axis_io_map(MainWindow.m_iCardIndex, i, 0, out lmtp_board, out lmtp_port);
                    if(lmtp_board==0)
                    {
                        adt_card_632xe.adt_read_servo_inbit(MainWindow.m_iCardIndex, i, lmtp_port, out lmtp_now_level);//伺服IO使用为正限位, 当前电平
                    }
                    else
                    {
                        adt_card_632xe.adt_read_busio_inbit(MainWindow.m_iCardIndex, i, lmtp_port, out lmtp_now_level);//总线IO使用为正限位, 当前电平
                    }
                    if (AxisInformation[i - 1].PositiveLimit != lmtp_now_level)
                        AxisInformation[i - 1].PositiveLimit = lmtp_now_level;

                    adt_card_632xe.adt_get_axis_io_map(MainWindow.m_iCardIndex, i, 1, out lmtm_board, out lmtm_port);//负限位端口
                    if (0 == lmtm_board)
                        adt_card_632xe.adt_read_servo_inbit(MainWindow.m_iCardIndex, i, lmtm_port, out lmtm_now_level);//伺服IO使用为负限位, 当前电平
                    else
                        adt_card_632xe.adt_read_busio_inbit(MainWindow.m_iCardIndex, lmtm_board, lmtm_port, out lmtm_now_level);//总线IO使用为负限位, 当前电平
                    if (AxisInformation[i - 1].NegativeLimit != lmtm_now_level)
                        AxisInformation[i - 1].NegativeLimit = lmtm_now_level;

                    adt_card_632xe.adt_get_axis_io_map(MainWindow.m_iCardIndex, i, 2, out stp0_board, out stp0_port);//原点端口
                    if (0 == stp0_board)
                        adt_card_632xe.adt_read_servo_inbit(MainWindow.m_iCardIndex, i, stp0_port, out stp0_now_level);//伺服IO使用为原点, 当前电平
                    else
                        adt_card_632xe.adt_read_busio_inbit(MainWindow.m_iCardIndex, stp0_board, stp0_port, out stp0_now_level); //总线IO使用为原点, 当前电平
                    if (AxisInformation[i - 1].Origin != stp0_now_level)
                        AxisInformation[i - 1].Origin = stp0_now_level;

                    adt_card_632xe.adt_get_stopdata(MainWindow.m_iCardIndex, i, out stt);//停止信息
                    if (AxisInformation[i - 1].StopSignal != stt)
                        AxisInformation[i - 1].StopSignal = stt;
                }
            }
            lsvStatus.ItemsSource = AxisInformation;
            lsvStatus.CommitEdit();
            lsvStatus.Items.Refresh();
        }

        public void UpdateUI()
        {
            lsvStatus.ItemsSource = null;
            lsvStatus.Items.Clear();
            int axs_count = 0;  //总轴数
            adt_card_632xe.adt_get_total_axis(MainWindow.m_iCardIndex, out axs_count);
            for (int i = 0; i < axs_count; i++)
            {
                AxisInformation.Add(new AxisInformation() { Axis = 1, IsSelected = true, LogicalPosition = 0, EncoderPosition = 0, LogicalSpeed = 0, EncoderSpeed = 0, DriveStatus = 0, TargetPosition = 0, Enable = "未使能", PositiveLimit = 0, NegativeLimit = 0, Origin = 0, StopSignal = 0 });
            }

            lsvStatus.ItemsSource= AxisInformation;
        }

        private void Reset_Click(object sender, EventArgs e)

        {
            int axs_count = 0;
            adt_card_632xe.adt_get_total_axis(MainWindow.m_iCardIndex, out axs_count);
            for (int i = 1; i <= axs_count; i++)
            {
                //逻辑位置清零
                adt_card_632xe.adt_set_command_pos(MainWindow.m_iCardIndex, i, 0);
                //实际位置清零
                adt_card_632xe.adt_set_actual_pos(MainWindow.m_iCardIndex, i, 0);
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            int axs_count = 0;
            adt_card_632xe.adt_get_total_axis(MainWindow.m_iCardIndex, out axs_count);
            for (int index = 1; index <= axs_count; ++index)
                //清除轴的驱动状态
                adt_card_632xe.adt_clear_axis_status(MainWindow.m_iCardIndex, index);
        }
    }
}
