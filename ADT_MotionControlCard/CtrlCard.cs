using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Xml.Serialization;
using ADT_CARD_632XE;
using DEMO632XE;

namespace ADT_MotionControlCard
{
    public class CtrlCard
    {

        public static int m_iCardIndex = 0;//当前卡号

        private int[] crd_index = new int[10];

        int[] cbbCard;

        private int m_iPosMode = 0; //位置模式

        private MainWindow mainWindow;  //主界面

        public int crd_count = 0;  //可用控制卡数量

        private int m_iAxsCount = 0;    //轴数

        private DispatcherTimer timer = new DispatcherTimer();

        private DispatcherTimer timer1 = new DispatcherTimer();

        private DispatcherTimer timer2 = new DispatcherTimer();

        public CtrlCard(MainWindow mainWindow)
        {

            this.mainWindow = mainWindow;

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer1.Interval = TimeSpan.FromSeconds(1);
            timer1.Tick += tmrStt_Tick;
            int result = adt_card_632xe.adt_initial(out crd_count, 0);
            if (result != 0)
            {
                MessageBox.Show("控制卡初始化失败或者系统未安装运动控制卡！错误码为 " + result);
                return;
            }
            MessageBox.Show("控制卡初始化成功，系统有" + crd_count.ToString() + " 张ADT-6320E运动控制卡！");
            //可用卡索引
            mainWindow.Dispatcher.Invoke(() =>
            {
                mainWindow.cbbCard.Items.Clear();
            });
            //获取索引
            adt_card_632xe.adt_get_card_index(out crd_count, crd_index);
            for (int i = 0; i < crd_count; i++)
            {
                mainWindow.Dispatcher.Invoke(() =>
                {
                    mainWindow.cbbCard.Items.Add(crd_index[i]);
                });
                //先关闭通讯
                m_iCardIndex = crd_index[i];
                adt_card_632xe.adt_ecat_stop(m_iCardIndex);
                //加载配置
                result = adt_card_632xe.adt_ecat_load_flash_cfg(m_iCardIndex);
                if (result != 0)
                {
                    MessageBox.Show(adt_global.adt_decode_error_code(result));
                    return;
                }
                //启动总线通讯
                result = adt_card_632xe.adt_ecat_start(m_iCardIndex);
                if (result != 0)
                {
                    MessageBox.Show(adt_global.adt_decode_error_code(result));
                    return;
                }
            }
            m_iCardIndex = crd_index[0];
            MessageBox.Show("控制卡初始化成功, EtherCAT通讯已开启!");
        }

        public void Close_Click()
        {
            int result = adt_card_632xe.adt_close_card();
            if (result != 0)
            {
                MessageBox.Show("控制卡关闭失败！错误码为 " + result);
                return;
            }
            MessageBox.Show("控制卡关闭成功!");
        }

        public void Reset_Click(int cardno)
        {
            int result = adt_card_632xe.adt_reset_card(cardno);
            if (result != 0)
            {
                MessageBox.Show("控制卡复位失败！错误码为 " + result);
                return;
            }
            MessageBox.Show("控制卡复位成功!");
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            int axis = m_iCardIndex + 1;
            int status = adt_card_632xe.adt_get_home_status(m_iCardIndex, axis);

            if (status > 0)
            {
                return;
            }
            else if (status == 0)
            {
                timer.Stop();
                MessageBox.Show("回零成功！");
            }
            else
            {
                timer.Stop();
                MessageBox.Show("回零失败！" + status.ToString());
            }
        }

        /// <summary>
        /// 使能或断使能
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="enable">1：使能   0：断使能</param>
        /// <returns></returns>
        public int Enable_Click(int axis,int enable)
        {
            int result = adt_card_632xe.adt_set_axis_enable(m_iCardIndex, axis, enable);
            return result;
        }

        //设置限位参数
        public void Apply_Click()
        {
            //轴号
            int axis = mainWindow.AxisNum.SelectedIndex + 1;
            int enable = 0, board = 0, port = 0, level = 0, mode = 0;

            //设置正限位
            enable = mainWindow.cbxLmtp.IsChecked == true ? 1 : 0;
            board = mainWindow.cbxNodeLP.SelectedIndex;
            port = mainWindow.cbxPortLP.SelectedIndex;
            level = mainWindow.cbxLevelLP.SelectedIndex;
            mode = mainWindow.cbxAdmodeLP.SelectedIndex;

            //设置正限位输入端口映射
            adt_card_632xe.adt_set_axis_io_map(m_iCardIndex, axis, 0, board, port);
            //设置正限位 停止模式与有效电平
            adt_card_632xe.adt_set_hardlimit_mode(m_iCardIndex, axis, 0, enable, level, mode);

            //设置负限位
            enable = mainWindow.cbxLmtn.IsChecked == true ? 1 : 0;
            board = mainWindow.cbxNodeLN.SelectedIndex;
            port = mainWindow.cbxPortLN.SelectedIndex;
            level = mainWindow.cbxLevelLN.SelectedIndex;
            mode = mainWindow.cbxAdmodeLN.SelectedIndex;

            //设置负限位输入端口映射
            adt_card_632xe.adt_set_axis_io_map(m_iCardIndex, axis, 1, board, port);
            //设置负限位 停止模式与有效电平
            adt_card_632xe.adt_set_hardlimit_mode(m_iCardIndex, axis, 1, enable, level, mode);

            //设置原点
            enable = mainWindow.cbxStop0.IsChecked == true ? 1 : 0;
            board = mainWindow.cbxNodeSTOP0.SelectedIndex;
            port = mainWindow.cbxPortSTOP0.SelectedIndex;
            level = mainWindow.cbxLevelSTOP0.SelectedIndex;
            mode = mainWindow.cbxAdmodeSTOP0.SelectedIndex;

            //设置原点输入端口映射
            adt_card_632xe.adt_set_axis_io_map(m_iCardIndex, axis, 2, board, port);
            //设置原点 停止模式与有效电平
            adt_card_632xe.adt_set_hardlimit_mode(m_iCardIndex, axis, 2, enable, level, mode);

            //设置硬件停止信息
            enable = mainWindow.cbxEmgn.IsChecked == true ? 1 : 0;
            board = mainWindow.cbxNodeEmg.SelectedIndex;
            port = mainWindow.cbxPortEmg.SelectedIndex;
            level = mainWindow.cbxLevelEmg.SelectedIndex;
            mode = mainWindow.cbxAdmodeEmg.SelectedIndex;

            //先将硬件停止信号禁用, 以免当前生效时其他参数无法写入
            adt_card_632xe.adt_set_axis_io_map(m_iCardIndex, axis, 4, 0, 0);
            //清除一次硬件停止信息错误
            adt_card_632xe.adt_reset_card(m_iCardIndex);

            adt_card_632xe.adt_set_axis_io_map(m_iCardIndex, axis, 4, board, port);
            //设置停止 停止模式与有效电平
            adt_card_632xe.adt_set_hardlimit_mode(m_iCardIndex, axis, 4, enable, level, mode);

            adt_card_632xe.adt_set_emergency_stop(m_iCardIndex, 1, level);

            mode = mainWindow.cbxUnitMode.SelectedIndex;
            //设置指定控制卡指定轴号的编程模式
            adt_card_632xe.adt_set_unit_mode(m_iCardIndex, axis, mode);

            //编码器转动一圈发出的脉冲个数(编码器分辨率) 总线控制轴的电子齿轮比(大于1000)
            int encoder = 0, elec = 0;
            //脉冲当量 正限位位置  负限位位置  单位mm
            double equiv = 0, softp = 0, softn = 0;
            try
            {
                encoder = Convert.ToInt32(mainWindow.cbxEncode.Text);
                equiv = Convert.ToInt32(mainWindow.tbxEquiv.Text);
                elec = Convert.ToInt32(mainWindow.tbxEquiv.Text) * Convert.ToInt32(mainWindow.cbxLeadRatio.Text) * Convert.ToInt32(mainWindow.tbxReductionRatio.Text);
                softp = Convert.ToInt32(mainWindow.tbxSoftP.Text);
                softn = Convert.ToInt32(mainWindow.tbxSoftM.Text);

            }
            catch (Exception ex)
            {
                MessageBox.Show("数据错误," + ex);
            }
            //设置与获取指定控制卡指定轴号的脉冲当量
            adt_card_632xe.adt_set_pulse_equiv(m_iCardIndex, axis, equiv);
            //设置与获取总线轴的电子齿轮比
            adt_card_632xe.adt_set_bus_axis_gear_ratio(m_iCardIndex, axis, encoder, elec);

            enable = mainWindow.cbxSoftlm.IsChecked == true ? 1 : 0;
            mode = mainWindow.tbxAdmode.SelectedIndex;
            //设置软件限位模式(基于脉冲当量编程)
            adt_card_632xe.adt_set_softlimit_mode(m_iCardIndex, axis, enable, softp, softn, mode);

            mainWindow.Dispatcher.Invoke(() =>
            {
                mainWindow.tbxElec.Text = elec.ToString();
            });
        }

        //设置运动参数  sign  0  点动  1   插补
        private int setDriverParam(int sign, double Startv, double Maxv, double Time)
        {
            double startv = 0, maxv = 0, acc = 0, dec = 0;
            int admode = 0, axis = 0;
            double time = 0;
            m_iAxsCount = 0;

            if (sign == 0)
            {
                startv = Startv;
                maxv = Maxv;
                time = Time;
                acc = (maxv - startv) / time;
                dec = (maxv - startv) / time;
                //if (data.AccelerationDecelerationMode == "S型")
                //    admode = 0;
                //else if (data.AccelerationDecelerationMode == "T型")
                //    admode = 1;
                //else if (data.AccelerationDecelerationMode == "EXP型")
                //    admode = 2;
                //else if (data.AccelerationDecelerationMode == "COS型")
                //    admode = 3;
                admode = 0;
                int axisNum = mainWindow.AxisNum.SelectedIndex + 1;
                int result = adt_card_632xe.adt_set_axis_move_para_unit(m_iCardIndex, axisNum, admode, startv, maxv, acc, dec);
                if (result != 0)
                {
                    MessageBox.Show("adt_set_axis_move_para_unit:" + adt_global.adt_decode_error_code(result));
                    return 1;
                }
            }

            for (int i = 0; i < mainWindow.lsvDrive.Items.Count; i++)
            {
                var data = mainWindow.lsvDrive.Items[i] as Axis;
                if (data.IsSelected)
                {
                    mainWindow.Dispatcher.Invoke(() =>
                    {
                        mainWindow.m_alAxsList[m_iAxsCount] = data.axiss;
                        mainWindow.m_plPosListDouble[m_iAxsCount] = Convert.ToDouble(data.TargetPosition);
                        mainWindow.m_plPosListInt32[m_iAxsCount] = Convert.ToInt32(data.TargetPosition);
                        m_iAxsCount += 1;
                    });
                }
            }

            m_iPosMode = mainWindow.cbxPosMode.SelectedIndex;
            return 0;
        }

        //点位驱动
        public void Drive_Click(int Axis, double Startv,double Maxv,double Time,double Distance)
        {
            int axis = Axis ;

            int unt_mode; //编程模式   0: 脉冲当量编程模式  1: 脉冲编程模式
            int result = 0;
            double pos = Distance;
            setDriverParam(0, Startv, Maxv, Time);
            adt_card_632xe.adt_get_unit_mode(m_iCardIndex, axis, out unt_mode);
            if (unt_mode == 0)
            {
                result = adt_card_632xe.adt_pmove_unit(m_iCardIndex, axis, pos, m_iPosMode);
            }
            if (result != 0)
            {
                MessageBox.Show(adt_global.adt_decode_error_code(result));
                return;
            }
        }

        /// <summary>
        /// 连续模式鼠标按下   
        /// </summary>
        /// <param name="direction">0:正向  1：负向</param>
        public void MouseDown_Click(int direction,int Axis, double Startv, double Maxv, double Time)
        {
            int axis = Axis;
            setDriverParam(0, Startv, Maxv, Time);
            int result = adt_card_632xe.adt_continue_move(m_iCardIndex, axis, direction);
            if (result != 0)
            {
                MessageBox.Show(adt_global.adt_decode_error_code(result));
                return;
            }
        }

        //立即停止
        public void SudStop_Click()
        {
            adt_card_632xe.adt_set_axis_stop(m_iCardIndex, 0, 1);
        }

        //回零
        public void StopHome_Click()
        {
            int result = 0;
            result = adt_card_632xe.adt_stop_servo_home(MainWindow.m_iCardIndex, mainWindow.AxisNum.SelectedIndex + 1);
            if (result != 0)
            {
                MessageBox.Show(adt_global.adt_decode_error_code(result));
                return;
            }
            timer1.Stop();
        }

        /// <summary>
        /// 轴跟随
        /// </summary>
        /// <param name="slave">从轴号</param>
        /// <param name="master">主轴号  0表示取消从轴跟随</param>
        public void Follow_Click(int slave,int master)
        {
            adt_card_632xe.adt_set_follow_axis(MainWindow.m_iCardIndex, slave, master);
        }

        private void tmrStt_Tick(object sender, EventArgs e)
        {
            //查询回零状态
            int retn = adt_card_632xe.adt_get_servo_home_status(MainWindow.m_iCardIndex, mainWindow.AxisNum.SelectedIndex + 1);
            mainWindow.Dispatcher.Invoke(() =>
            {
                mainWindow.tbxStt.Text = retn.ToString();
            });
            if (retn == 0)
            {
                mainWindow.Dispatcher.Invoke(() =>
                {
                    mainWindow.tbxStt.Text = retn.ToString() + ":回零完成";
                });
                timer1.Stop();
            }
        }

        public void Home_Click()
        {
            int axis = mainWindow.homeAxis.SelectedIndex + 1;
            int mode = mainWindow.homeMode.SelectedIndex;
            double maxv = 0, lowv = 0, acc = 0, offset = 0, distance = 0;
            try
            {
                maxv = Convert.ToDouble(mainWindow.tbxMaxv.Text);
                lowv = Convert.ToDouble(mainWindow.tbxLowv.Text);
                acc = Convert.ToDouble(mainWindow.tbxAcc.Text);
                offset = Convert.ToDouble(mainWindow.tbxOffset.Text);
                distance = Convert.ToDouble(mainWindow.tbxBack.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("数据错误");
                return;
            }
            //设置stop0(机械原点)信号 使能标识 有效电平 停止模式
            int enable=0, logic1=0, admode=0;
            adt_card_632xe.adt_get_stop0_mode(m_iCardIndex, axis, out enable, out logic1, out admode);

            int plEnable = 0, nlEnable = 0, logic2 = 0;
            adt_card_632xe.adt_get_limit_mode(m_iCardIndex, axis, out plEnable, out nlEnable, out logic2);

            int limit = logic2;
            if (nlEnable == 0)
            {
                limit += 2;
            }
            if (plEnable == 0)
            {
                limit += 4;
            }

            int reslut = 0;
            // 回零模式设置
            reslut = adt_card_632xe.adt_set_home_mode(m_iCardIndex, axis, mode, logic1, limit, distance, offset);
            if (reslut != 0)
            {
                MessageBox.Show("adt_set_home_mode:" + adt_global.adt_decode_error_code(reslut));
                return;
            }
            //回零速度参数
            reslut = adt_card_632xe.adt_set_home_speed(m_iCardIndex, axis, 0, maxv, lowv, acc);
            if (reslut != 0)
            {
                MessageBox.Show("adt_set_home_speed:" + adt_global.adt_decode_error_code(reslut));
                return;
            }
            //启动
            reslut = adt_card_632xe.adt_set_home_process(m_iCardIndex, axis);
            if (reslut != 0)
            {
                MessageBox.Show("adt_set_home_process:" + adt_global.adt_decode_error_code(reslut));
                return;
            }

            timer.Start();
            timer1.Start();
        }
    }
}
