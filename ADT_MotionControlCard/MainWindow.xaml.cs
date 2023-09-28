using ADT_CARD_632XE;
using Control_Card;
using DEMO632XE;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Windows.Threading;
using System.Xml;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;
using Image = System.Windows.Controls.Image;

namespace ADT_MotionControlCard
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private CtrlCard my_Card; //当前控制的卡

        //运动控制卡的数量
        private int crd_count = 0;

        private int[] crd_index = new int[10];

        public static int m_iCardIndex = 0;//当前卡号

        //从站节点数 伺服节点数(轴号数)  IO节点数
        public int m_slaveNum, m_axisNum, m_ioCount;

        //节点号数
        private int[] cbxNode = new int[33];

        //端口号数
        private int[] cbxPort = new int[16];

        //有效电平  
        private string[] cbxLevel = new string[2] { "低电平", "高电平" };

        //停止模式
        private string[] cbxAdmode = new string[2] { "减速停止", "立即停止" };

        //电子齿轮比
        private int[] encode = new int[4] { 131072, 1048576, 8388608, 16777216 };

        public ObservableCollection<Axis> axis = new ObservableCollection<Axis>();

        //圆弧方向
        private string[] Dir1 = new string[2] { "逆时针", "顺时针" };

        //圆弧类型
        private string[] Dir2 = new string[2] { "圆弧", "整圆" };

        //位置模式
        private string[] PosMode = new string[2] { "相对位置", "绝对位置" };

        //轨迹类型
        private string[] Type = new string[2] { "圆弧", "整圆" };

        private string[] UnitMode = new string[2] { "基于脉冲当量", "基于脉冲" };

        private string[] HomeMode = new string[4] { "1 负方向直线回零", "2 负方向圆周回零", "3 正方向直线回零", "4 正方向圆周回零" };

        private int m_iPosMode = 0; //位置模式

        public ObservableCollection<Axis> axes = new ObservableCollection<Axis>();

        private int m_iAxsCount = 0;    //轴数
        public int[] m_alAxsList = new int[32]; //轴号列表参数
        public double[] m_plPosListDouble = new double[32]; //位置参数Double类型
        public int[] m_plPosListInt32 = new int[32]; //位置参数Int32类型

        private DispatcherTimer timer = new DispatcherTimer();

        private DispatcherTimer timer1 = new DispatcherTimer();

        private DispatcherTimer timer2 = new DispatcherTimer();

        private int m_iSlvType = 0; //当前从站类型
        private int m_iICount = 0, m_iOCount = 0; //当前从站IO组数

        public MainWindow()
        {
            InitializeComponent();

            for (int i = 0; i < cbxNode.Length; i++)
            {
                cbxNode[i] = i;
            }
            for (int i = 0; i < cbxPort.Length; i++)
            {
                cbxPort[i] = i;
            }
            cbxNodeLP.ItemsSource = cbxNode;
            cbxPortLP.ItemsSource = cbxPort;
            cbxLevelLP.ItemsSource = cbxLevel;
            cbxAdmodeLP.ItemsSource = cbxAdmode;
            cbxNodeLN.ItemsSource = cbxNode;
            cbxPortLN.ItemsSource = cbxPort;
            cbxLevelLN.ItemsSource = cbxLevel;
            cbxAdmodeLN.ItemsSource = cbxAdmode;
            cbxNodeSTOP0.ItemsSource = cbxNode;
            cbxPortSTOP0.ItemsSource = cbxPort;
            cbxLevelSTOP0.ItemsSource = cbxLevel;
            cbxAdmodeSTOP0.ItemsSource = cbxAdmode;
            cbxNodeEmg.ItemsSource = cbxNode;
            cbxPortEmg.ItemsSource = cbxPort;
            cbxLevelEmg.ItemsSource = cbxLevel;
            cbxAdmodeEmg.ItemsSource = cbxAdmode;
            cbxEncode.ItemsSource = encode;
            cbDir1.ItemsSource = Dir1;
            cbDir2.ItemsSource = Dir2;
            cbxPosMode.ItemsSource = PosMode;
            cbType.ItemsSource = Type;
            tbxAdmode.ItemsSource = cbxAdmode;
            cbxUnitMode.ItemsSource = UnitMode;
            homeMode.ItemsSource = HomeMode;


            timer1.Interval = TimeSpan.FromSeconds(1);
            timer1.Tick += timer1_Tick;
            timer2.Interval = TimeSpan.FromSeconds(1);
            timer2.Tick += timer2_Tick;
            timer2.Start();
        }

        //初始化
        private void Init(object sender, EventArgs e)
        {
            my_Card = new CtrlCard(this);
            cbbCard.SelectedIndex = 0;
            m_iCardIndex = CtrlCard.m_iCardIndex;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            int cmd = 0;
            adt_card_632xe.adt_get_command_pos(m_iCardIndex, 1, out cmd);
            double pos = Convert.ToDouble(cmd) / Convert.ToInt32(tbxEquiv.Text);
            LogicalPosition.Text= pos.ToString();
        }

        //索引更改
        private void cbbCard_SelectedIndexChanged(object sender, EventArgs e)
        {
            int fpga = 0, dll = 0, motion = 0;
            //获取版本信息
            adt_card_632xe.adt_get_firmware_ver(m_iCardIndex, out fpga);
            adt_card_632xe.adt_get_motion_ver(m_iCardIndex, out motion);
            adt_card_632xe.adt_get_lib_ver(m_iCardIndex, out dll);
            lblVersion.Text = fpga.ToString() + "." + motion.ToString() + "." + dll.ToString();
            //获取Ethercat从站资源信息
            adt_card_632xe.adt_get_ecat_slave_resource(m_iCardIndex, out m_slaveNum, out m_axisNum, out m_ioCount);

            //获取Ethercat从站节点信息
            //adt_card_632xe.adt_get_ecat_slave_info(m_iCardIndex,)
            Int32 axis_num = 0, io_num = 0;
            if (m_slaveNum > 0)
            {
                //对应的节点类型 伺服轴对应的节点号 io对应的节点号
                int[] m_nodetype = new Int32[m_slaveNum];
                int[] m_axisnode = new Int32[m_axisNum];
                int[] m_ionode = new Int32[m_ioCount];

                Int32 m_in, m_out, m_naxis;
                for (Int32 i = 1; i <= m_slaveNum; i++)
                {
                    adt_card_632xe.adt_get_ecat_slave_info(m_iCardIndex, i, out m_nodetype[i - 1], out m_naxis, out m_in, out m_out);
                    if (m_nodetype[i - 1] == 1)        //io节点
                    {
                        m_axisnode[axis_num] = i;
                        axis_num++;
                    }
                    else if (m_nodetype[i - 1] == 2)   //伺服节点
                    {
                        m_ionode[io_num] = i;
                        io_num++;
                    }
                }
            }

            AxisNum.Items.Clear();
            for (int i = 1; i <= m_axisNum; i++)
            {
                AxisNum.Items.Add(i.ToString());
            }
            AxisNum.SelectedIndex = 0;

            CircleAxis.Items.Clear();
            for (int i = 1; i <= m_axisNum; i++)
            {
                CircleAxis.Items.Add(i.ToString());
            }
            CircleAxis.SelectedIndex = 0;

            lineAxis.Items.Clear();
            for (int i = 1; i <= m_axisNum; i++)
            {
                lineAxis.Items.Add(i.ToString());
            }
            lineAxis.SelectedIndex = 0;


            //更新驱动参数列表
            UpdateAxis();

            GoHomeUpdateUI();

            IOUpdateUI();

            UpdateCtrl();

            //UpdateNode();
        }

        private void Apply_Click(object sender, EventArgs e)
        {
            my_Card.Apply_Click();
        }

        //保存配置
        private void WriteXml_Click(object sender, EventArgs e)
        {
            //获取当前应用程序的可执行文件路径
            string path = System.Reflection.Assembly.GetEntryAssembly().Location;
            path = path.Substring(0, path.LastIndexOf("\\"));
            string fl_name = path + "\\axis config.xml";
            int result = adt_card_632xe.adt_export_nc_cfg(m_iCardIndex, fl_name);
            if (result != 0)
            {
                MessageBox.Show("轴配置文件导入失败!");
            }
            else
            {
                MessageBox.Show("轴配置文件已导入, 路径为" + fl_name);
            }
        }

        //读取配置
        private void ReadXml_Click(object sender, EventArgs e)
        {
            //获取当前应用程序的可执行文件路径
            string path = System.Reflection.Assembly.GetEntryAssembly().Location;
            path = path.Substring(0, path.LastIndexOf("\\"));
            string fl_name = path + "\\axis config.xml";
            int result = adt_card_632xe.adt_load_nc_cfg(m_iCardIndex, fl_name);
            if (result != 0)
            {
                MessageBox.Show("轴配置文件读取失败!");
            }
            else
            {
                MessageBox.Show("轴配置文件已读取");
            }
            UpdateCtrl();
        }

        private void cbxAxis_SelectedIndexChanged(object sender, EventArgs e)
        {
            //UpdateCtrl();
        }


        //更新参数配置
        private void UpdateCtrl()
        {
            int axis = AxisNum.SelectedIndex + 1;

            cbxNodeLP.ItemsSource = null;
            cbxNodeLP.Items.Clear();
            for (int i = 0; i <= m_axisNum + 1; ++i)
                cbxNodeLP.Items.Add(i);
            cbxNodeLP.SelectedIndex = 0;

            cbxNodeLN.ItemsSource = null;
            cbxNodeLN.Items.Clear();
            for (int i = 0; i <= m_axisNum + 1; ++i)
                cbxNodeLN.Items.Add(i);
            cbxNodeLN.SelectedIndex = 0;

            cbxNodeSTOP0.ItemsSource = null;
            cbxNodeSTOP0.Items.Clear();
            for (int i = 0; i <= m_axisNum + 1; ++i)
                cbxNodeSTOP0.Items.Add(i);
            cbxNodeSTOP0.SelectedIndex = 0;

            cbxNodeEmg.ItemsSource = null;
            cbxNodeEmg.Items.Clear();
            for (int i = 0; i <= m_axisNum + 1; ++i)
                cbxNodeEmg.Items.Add(i);
            cbxNodeEmg.SelectedIndex = 0;

            //正限位
            int board = 0, port = 0, enble = 0, lvl = 0, admode = 0;

            //获取原点 / 限位 / 急停信号输入端口映射
            adt_card_632xe.adt_get_axis_io_map(m_iCardIndex, axis, 0, out board, out port);
            adt_card_632xe.adt_get_hardlimit_mode(m_iCardIndex, axis, 0, out enble, out lvl, out admode);

            cbxLmtp.IsChecked = enble == 1 ? true : false;
            cbxNodeLP.SelectedIndex = board;
            cbxPortLP.SelectedIndex = port;
            cbxLevelLP.SelectedIndex = lvl;
            cbxAdmodeLP.SelectedIndex = admode;

            //负限位
            adt_card_632xe.adt_get_axis_io_map(m_iCardIndex, axis, 1, out board, out port);
            adt_card_632xe.adt_get_hardlimit_mode(m_iCardIndex, axis, 1, out enble, out lvl, out admode);

            cbxLmtn.IsChecked = enble == 1 ? true : false;
            cbxNodeLN.SelectedIndex = board;
            cbxPortLN.SelectedIndex = port;
            cbxLevelLN.SelectedIndex = lvl;
            cbxAdmodeLN.SelectedIndex = admode;

            //原点
            adt_card_632xe.adt_get_axis_io_map(m_iCardIndex, axis, 2, out board, out port);
            adt_card_632xe.adt_get_hardlimit_mode(m_iCardIndex, axis, 2, out enble, out lvl, out admode);

            cbxStop0.IsChecked = enble == 1 ? true : false;
            cbxNodeSTOP0.SelectedIndex = board;
            cbxPortSTOP0.SelectedIndex = port;
            cbxLevelSTOP0.SelectedIndex = lvl;
            cbxAdmodeSTOP0.SelectedIndex = admode;

            //硬件停止信息
            adt_card_632xe.adt_get_axis_io_map(m_iCardIndex, axis, 4, out board, out port);
            adt_card_632xe.adt_get_hardlimit_mode(m_iCardIndex, axis, 4, out enble, out lvl, out admode);

            cbxEmgn.IsChecked = enble == 1 ? true : false;
            cbxNodeEmg.SelectedIndex = board;
            cbxPortEmg.SelectedIndex = port;
            cbxLevelEmg.SelectedIndex = lvl;
            cbxAdmodeEmg.SelectedIndex = admode;

            int mode = 0;
            //获取指定控制卡指定轴号的编程模式
            adt_card_632xe.adt_get_unit_mode(m_iCardIndex, axis, out mode);
            cbxUnitMode.SelectedIndex = mode;

            //编码器转动一圈发出的脉冲个数(编码器分辨率) 总线控制轴的电子齿轮比(大于1000)
            int encoder = 0, elec;
            //脉冲当量 正限位位置  负限位位置  单位mm
            double equiv = 0, softp = 0, softn = 0;
            adt_card_632xe.adt_get_pulse_equiv(m_iCardIndex, axis, out equiv);
            adt_card_632xe.adt_get_bus_axis_gear_ratio(m_iCardIndex, axis, out encoder, out elec);
            tbxEquiv.Text = equiv.ToString();
            cbxEncode.SelectedItem = encoder.ToString();

            adt_card_632xe.adt_get_softlimit_mode(m_iCardIndex, axis, out enble, out softp, out softn, out mode);
            cbxSoftlm.IsChecked = enble == 1 ? true : false;
            tbxSoftP.Text = softp.ToString();
            tbxSoftM.Text = softn.ToString();
            tbxAdmode.SelectedIndex = mode;



            UpdateAxis();
        }

        //更新节点号
        public void UpdateNode()
        {
            cbxNodeLP.Items.Clear();
            for (int i = 0; i <= m_axisNum + 1; ++i)
                cbxNodeLP.Items.Add(i);
            cbxNodeLP.SelectedIndex = 0;

            cbxNodeLN.Items.Clear();
            for (int i = 0; i <= m_axisNum + 1; ++i)
                cbxNodeLN.Items.Add(i);
            cbxNodeLN.SelectedIndex = 0;

            cbxNodeSTOP0.Items.Clear();
            for (int i = 0; i <= m_axisNum + 1; ++i)
                cbxNodeSTOP0.Items.Add(i);
            cbxNodeSTOP0.SelectedIndex = 0;

            cbxNodeEmg.Items.Clear();
            for (int i = 0; i <= m_axisNum + 1; ++i)
                cbxNodeEmg.Items.Add(i);
            cbxNodeEmg.SelectedIndex = 0;
        }

        //当前轴使能
        private void Enable_Click(object sender, EventArgs e)
        {
            int m_axis = AxisNum.SelectedIndex;
            int result = my_Card.Enable_Click(m_axis + 1, 1);
            if (result != 0)
            {
                MessageBox.Show("使能失败：" + result.ToString());
                return;
            }
        }

        private void cbDir1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        //当前轴关闭使能
        private void Disnable_Click(object sender, EventArgs e)
        {
            int m_axis = AxisNum.SelectedIndex;
            int result = my_Card.Enable_Click(m_axis + 1, 0);
            if (result != 0)
            {
                MessageBox.Show("断使能失败：" + result.ToString());
                return;
            }
        }

        //更新基本驱动参数列表
        public void UpdateAxis()
        {
            axis = new ObservableCollection<Axis>();
            lsvDrive.ItemsSource = null;
            lsvDrive.Items.Clear();
            for (int i = 1; i <= m_axisNum; i++)
            {
                axis.Add(new Axis() { axiss = i, IsSelected = false, InitialSpeed = 0, MaxSpeed = 0, Acceleration = 0, Deceleration = 0, AccelerationDecelerationMode = "S型", TargetPosition = 0 });
            }
            lsvDrive.ItemsSource = axis;

            cbDir1.SelectedIndex = 0;
            cbDir2.SelectedIndex = 0;
            cbxPosMode.SelectedIndex = 0;
            cbType.SelectedIndex = 0;
        }

        //减速停止
        private void DecStop_Click(object sender, EventArgs e)
        {
            adt_card_632xe.adt_set_axis_stop(m_iCardIndex, 0, 0);
        }

        //立即停止
        private void SudStop_Click(object sender, EventArgs e)
        {
            adt_card_632xe.adt_set_axis_stop(m_iCardIndex, 0, 1);
        }

        private void Drive_Click(object sender, EventArgs e)
        {
            int unt_mode; //编程模式   0: 脉冲当量编程模式  1: 脉冲编程模式
            for (int i = 0; i < m_iAxsCount; i++)
            {
                int result;
                adt_card_632xe.adt_get_unit_mode(m_iCardIndex, m_alAxsList[i], out unt_mode);
                if (unt_mode == 0)
                {
                    result = adt_card_632xe.adt_pmove_unit(m_iCardIndex, m_alAxsList[i], m_plPosListDouble[i], m_iPosMode);
                }
                else
                {
                    result = adt_card_632xe.adt_pmove_pulse(m_iCardIndex, m_alAxsList[i], m_plPosListInt32[i], m_iPosMode);
                }
                if (result != 0)
                {
                    MessageBox.Show(adt_global.adt_decode_error_code(result));
                    return;
                }
            }

        }

        private void GoHomeUpdateUI()
        {
            homeAxis.Items.Clear();
            for (int i = 1; i <= m_axisNum; i++)
            {
                homeAxis.Items.Add(i);
            }
            homeAxis.SelectedIndex = 0;
        }

        private void Home_Click(object sender, EventArgs e)
        {
            my_Card.Home_Click();
        }

        private void StopHome_Click(object sender, EventArgs e)
        {
            my_Card.StopHome_Click();
        }

        private void IOUpdateUI()
        {
            ioNode.Items.Clear();
            if (m_slaveNum <= 0)
                return;
            for (int i = 1; i <= m_slaveNum; i++)
            {
                ioNode.Items.Add(i.ToString());
            }
            ioNode.SelectedIndex = 0;
        }

        //io节点索引改变
        private void ioNode_SelectedIndexChanged(object sender, EventArgs e)
        {
            lvIn.Items.Clear();
            lvOut.Items.Clear();

            int slave = ioNode.SelectedIndex + 1;
            //设备类型   节点轴数   节点输入资源组数   节点输出资源组数
            int type = 0, axs_count = 0, i_count = 0, o_count = 0;
            adt_card_632xe.adt_get_ecat_slave_info(m_iCardIndex, slave, out type, out axs_count, out i_count, out o_count);

            m_iSlvType = type;
            m_iICount = i_count;
            m_iOCount = o_count;

            //IO输入输出列表
            for (int group = 1; group <= i_count; group++)
            {
                for (int i = 0; i < 8; i++)
                {
                    lvIn.Items.Add("IN" + ((group - 1) * 8 + i).ToString());
                }
            }
            for (int group = 1; group <= o_count; group++)
            {
                for (int i = 0; i < 8; i++)
                {
                    lvOut.Items.Add("OUT" + ((group - 1) * 8 + i).ToString());
                }
            }
        }

        private void AllOn_Click(object sender, EventArgs e)
        {
            int slave = ioNode.SelectedIndex + 1;
            for (int i = 0; i < lvOut.Items.Count; i++)
            {
                adt_card_632xe.adt_write_busio_outbit(m_iCardIndex, slave, i, 1);
            }
        }

        //输出模块双击事件
        private void ListViewItem_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int slave = ioNode.SelectedIndex + 1;
            var listViewItem = sender as ListViewItem;
            var item = listViewItem?.Content as string;
            if (item != null)
            {
                var numberRegex = new Regex(@"\d+");
                var match = numberRegex.Match(item);
                if (match.Success)
                {
                    var number = match.Value;
                    int parseNumber = int.Parse(number);
                    int result;
                    adt_card_632xe.adt_read_busio_outbit(m_iCardIndex, slave, parseNumber, out result);
                    if (result == 1)   //此时输出点打开
                    {
                        adt_card_632xe.adt_write_busio_outbit(m_iCardIndex, slave, parseNumber, 0);
                    }
                    else if (result == 0)
                    {
                        adt_card_632xe.adt_write_busio_outbit(m_iCardIndex, slave, parseNumber, 1);
                    }
                }
            }

        }

        private void AllEnable_Click(object sender, RoutedEventArgs e)
        {
            int result = 0;
            for (int i = 0; i < m_axisNum; i++)
            {
                result = my_Card.Enable_Click(i + 1, 1);
                if (result != 0)
                {
                    MessageBox.Show("使能失败：" + result.ToString());
                    return;
                }
            }
        }

        private void AllDisnable_Click(object sender, RoutedEventArgs e)
        {
            int result = 0;
            for (int i = 0; i < m_axisNum; i++)
            {
                result = my_Card.Enable_Click(i + 1, 0);
                if (result != 0)
                {
                    MessageBox.Show("使能失败：" + result.ToString());
                    return;
                }
            }
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            //复位运动控制卡
            my_Card.Reset_Click(m_iCardIndex);
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            my_Card.Close_Click();
        }

        private void JogP_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int axis = Convert.ToInt32(CircleAxis.Text.ToString());
            double startv = Convert.ToDouble(Acc_startv02.Text);
            double maxv = Convert.ToDouble(Acc_speed02.Text);
            double time=Convert.ToDouble(Acc_time02.Text);
            my_Card.MouseDown_Click(0, axis, startv, maxv, time);
        }

        private void JogP_MouseUp(object sender, MouseButtonEventArgs e)
        {
            my_Card.SudStop_Click();
        }

        private void JogN_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int axis = Convert.ToInt32(CircleAxis.Text.ToString());
            double startv = Convert.ToDouble(Acc_startv02.Text);
            double maxv = Convert.ToDouble(Acc_speed02.Text);
            double time = Convert.ToDouble(Acc_time02.Text);
            my_Card.MouseDown_Click(1, axis, startv, maxv, time);
        }

        private void JogN_MouseUp(object sender, MouseButtonEventArgs e)
        {
            my_Card.SudStop_Click();
        }

        private void AllOff_Click(object sender, EventArgs e)
        {
            int slave = ioNode.SelectedIndex + 1;
            for (int i = 0; i < lvOut.Items.Count; i++)
            {
                adt_card_632xe.adt_write_busio_outbit(m_iCardIndex, slave, i, 0);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //int slave = ioNode.SelectedIndex + 1;
            ////设备类型   节点轴数   节点输入资源组数   节点输出资源组数
            //int type = 0, axs_count = 0, i_count = 0, o_count = 0;
            //adt_card_632xe.adt_get_ecat_slave_info(m_iCardIndex, slave, out type, out axs_count, out i_count, out o_count);

            //for (int i = 0; i < lvIn.Items.Count; i++)
            //{
            //    int level;
            //    string imagePath = "";
            //    adt_card_632xe.adt_read_busio_inbit(m_iCardIndex, slave, i, out level);
            //    if (level == 0)
            //    {
            //        imagePath = "Images/IN/IN OFF.ico";
            //    }
            //    else if (level == 1)
            //    {
            //        imagePath = "Images/IN/IN ON.ico";
            //    }
            //    Image image = new Image
            //    {
            //        Source = new BitmapImage(new Uri(imagePath, UriKind.Relative))
            //    };
            //    Dispatcher.Invoke(() =>
            //    {
            //        lvIn.Items[i].
            //        // 在 UI 线程上添加图片项
            //        lvIn.Items.Add(image);
            //    });
            //}
        }

        //圆周旋转定值运动
        private void Drive1_Click(object sender, RoutedEventArgs e)
        {
            double startv, maxv, time, distance;
            startv = Convert.ToDouble(Acc_startv.Text);
            maxv = Convert.ToDouble(Acc_speed01.Text);
            time = Convert.ToDouble(Acc_time.Text);
            distance = Convert.ToDouble(Const_angle.Text);
            my_Card.Drive_Click(Convert.ToInt32(CircleAxis.Text),startv, maxv, time, distance);
        }

        private void StopImmediately_Click(object sender, RoutedEventArgs e)
        {
            my_Card.SudStop_Click();
        }

        private void Line_Drive1_Click(object sender, RoutedEventArgs e)
        {
            double startv, maxv, time, distance;
            int axis = Convert.ToInt32(lineAxis.Text);
            startv = Convert.ToDouble(line_startv.Text);
            maxv = Convert.ToDouble(line_speed01.Text);
            time = Convert.ToDouble(line_time.Text);
            distance = Convert.ToDouble(Const_Line.Text);
            my_Card.Drive_Click(axis,startv, maxv, time, distance);
        }

        private void Line_JogP_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int axis = Convert.ToInt32(lineAxis.Text.ToString());
            double startv = Convert.ToDouble(line_startv02.Text);
            double maxv = Convert.ToDouble(line_speed02.Text);
            double time = Convert.ToDouble(line_time02.Text);
            my_Card.MouseDown_Click(0, axis, startv, maxv, time);
        }

        private void Line_JogN_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int axis = Convert.ToInt32(lineAxis.Text.ToString());
            double startv = Convert.ToDouble(line_startv02.Text);
            double maxv = Convert.ToDouble(line_speed02.Text);
            double time = Convert.ToDouble(line_time02.Text);
            my_Card.MouseDown_Click(1, axis, startv, maxv, time);
        }

        //轴同步
        private void Follow_Click(object sender, RoutedEventArgs e)
        {
            my_Card.Follow_Click(2, 1);
        }

        private void StopFollow_Click(object sender, RoutedEventArgs e)
        {
            my_Card.Follow_Click(2, 0);
            MessageBox.Show("321");
        }

        private void Status_Click(object sender, EventArgs e)
        {
            Axis_Information_Monitoring axis_Status = new Axis_Information_Monitoring();
            axis_Status.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            axis_Status.Show();
            axis_Status.UpdateUI();
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = FindParent<DataGridRow>(e.OriginalSource as DependencyObject);
            if (row != null && row.Item is Axis item)
            {
                int axiss = item.axiss - 1;
                string startv = item.InitialSpeed.ToString();
                string maxv = item.MaxSpeed.ToString();
                string acc = item.Acceleration.ToString();
                string dec = item.Deceleration.ToString();
                string mode = item.AccelerationDecelerationMode.ToString();
                string target = item.TargetPosition.ToString();
                frmDrvPara frmDrvPara = new frmDrvPara(startv, maxv, acc, dec, mode, target);
                frmDrvPara.ShowDialog();
                if (frmDrvPara.dialogResult == true)
                {
                    axis[axiss].InitialSpeed = Convert.ToInt32(frmDrvPara.textStartV.Text);
                    axis[axiss].MaxSpeed = Convert.ToInt32(frmDrvPara.textMaxV.Text);
                    axis[axiss].Acceleration = Convert.ToInt32(frmDrvPara.textAcc.Text);
                    axis[axiss].Deceleration = Convert.ToInt32(frmDrvPara.textDec.Text);
                    axis[axiss].AccelerationDecelerationMode = frmDrvPara.cbAdmode.Text;
                    axis[axiss].TargetPosition = Convert.ToInt32(frmDrvPara.textTarget.Text);
                    // 退出当前的数据绑定事务过程
                    lsvDrive.CommitEdit();
                    lsvDrive.Items.Refresh();

                }
            }
        }

        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            while (true)
            {
                // 没有更多父元素
                if (child == null)
                {
                    return null;
                }

                // 找到所需类型的父元素
                if (child is T parent)
                {
                    return parent;
                }

                // 获取父元素
                child = VisualTreeHelper.GetParent(child);
            }
        }
    }
}
