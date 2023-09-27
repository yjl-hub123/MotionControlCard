using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace ADT_CARD_632XE
{
    class adt_card_632xe
    {
        //功  能：  控制卡初始化
        //参  数：  card_count  --  系统识别到的09系列运动控制卡数量
        //          mode        --  备用参数, 默认设置0即可
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_initial(out Int32 card_count, Int32 mode);
        
        //功  能：  控制卡资源释放
        [DllImport("adtemc.dll")]
        public static  extern Int32  adt_close_card();

        //功  能：  复位指定控制卡
        //参  数：  cardno  --  可用控制卡卡号
        //备  注：  1. 该接口将清空驱动指令数据、缓存事件数据、同步关系数据
        //          2. 紧急停止信号触发后, 须成功执行一次控制卡复位才能继续正常使用控制卡
        //          3. 控制卡复位不会影响基础设置, 包括限位模式、速度相关参数、编程模式、脉冲当量等
        [DllImport("adtemc.dll")] 
        public static  extern Int32  adt_reset_card(Int32 cardno);

        //功  能：  软件重启指定控制卡
        //参  数：  cardno  --  可用控制卡卡号
        //备  注：  1. 该接口将清空驱动指令数据、缓存事件数据、同步关系数据
        //          2. 该接口同样会将控制卡基础设置全部重置为初始状态
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_soft_reboot(Int32 cardno);

        //功  能：  获取当前系统可用控制卡数量及其拨码开关号(即卡号)
        //参  数：  card_count  --  系统识别到的09系列运动控制卡数量
        //          card_index  --  系统识别到的09系列运动控制卡可用索引
        //备  注：  1. card_index数组大小应>=card_count
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_card_index(out Int32 card_count, Int32[] card_index);

        //功  能：  获取指定控制卡版本信息
        //参  数：  cardno      --  可用控制卡卡号
        //          ver         --  版本号
        //备  注：  1. 接口依次为LIB库版本信息获取、MOTION程序版本信息获取、FPGA程序版本信息获取、接线板程序版本信息获取
        //          2. 版本信息是dll升级的依据, 客户提供的版本号信息格式为LIB.MOTION.FPGA
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_firmware_ver(Int32 cardno, out Int32 ver);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_motion_ver(Int32 cardno, out Int32 ver);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_lib_ver(Int32 cardno, out Int32 ver);

        //功  能：  获取指定控制卡系统错误号
        //参  数：  cardno      --  可用控制卡卡号
        //          errno       --  当前控制卡当前错误号
        //备  注：  1. 错误号描述可从adt_typedef.cs中查询
        //          2. 错误号不为0表示当前控制卡紧急停止信号触发或其他严重错误, 须复位或重启软件
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_motion_error(Int32 cardno, out Int32 errno);
        
        //功  能：  下发总线启动配置文件
        //参  数：  cardno      --  可用控制卡卡号
        //          path        --  总线启动配置文件路径, 由ADTWinCat配置工具生成
        //备  注：  1. 此函数下载配置文件到控制卡RAM中, 掉电不保存.用于PC端配置文件启动主站
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_ecat_download_cfg(Int32 cardno, String path);
        
        //功  能：  加载控制卡存储的总线配置参数
        //参  数：  cardno      --  可用控制卡卡号
        //备  注：  1. 此函数加载控制卡内存贮的配置文件, 用于控制卡内默认配置文件启动主站
        //          2. 配置文件下载到控制卡Flash内掉电保存, 需要使用AdtWinCat主站配置工具下载
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_ecat_load_flash_cfg(Int32 cardno);
        
        //功  能：  终止总线通信
        //参  数：  cardno      --  可用控制卡卡号
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_ecat_stop(Int32 cardno);
        
        //功  能：  启动总线通讯
        //参  数：  cardno      --  可用控制卡卡号
        //备  注：  1. 启动主站前需加载配置参数,阻塞式访问
        //          2. 注意启动前调用adt_ecat_load_flash_cfg或者adt_ecat_download_cfg启动配置文件
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_ecat_start(Int32 cardno);
        
        //功  能：  获取主站状态
        //参  数：  cardno      --  可用控制卡卡号
        //          cur_state   --  主站运行主流程状态
        //          sub_step    --  主站运行子流程状态
        //          ccl_time    --  总线通讯周期
        //          state_error --  错误码
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_ecat_get_master_status(Int32 cardno, out UInt32 cur_state, out UInt32 sub_step, out UInt32 ccl_time, out UInt32 state_error);
        
        //功  能：  获取从站状态
        //参  数：  cardno      --  可用控制卡卡号
        //          slv_index   --  从站索引号
        //          al_state    --  AL当前状态
        //          al_req      --  AL请求状态
        //          dl_state    --  DL状态
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_ecat_get_slave_status(Int32 cardno, Int32 slv_index, out UInt32 al_state, out UInt32 al_req, out UInt32 dl_state);
        
        //功  能：  Ethercat SDO数据读写操作  
        //参  数：  cardno      --  可用控制卡卡号
        //          node        --  从站节点号
        //          index       --  对象字典索引
        //          sub_index   --  AL对象字典子索引
        //          data        --  数据指针, 这里使用U8类型指针操作数据, 其他长度数据定义对应长度的U8数据即可
        //          len         --  数据长度, 只能是1,2,4
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_ecat_sdo_read(Int32 cardno, Int32 node, UInt32 index, UInt32 sub_index, byte[] data, UInt32 len);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_ecat_sdo_write(Int32 cardno, Int32 node, UInt32 index, UInt32 sub_index, byte[] data, UInt32 len);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_ecat_sdo_read_ex(Int32 cardno, Int32 node, Int32 index, Int32 subIndex, out Int32 pData, Int32 datalen);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_ecat_sdo_write_ex(Int32 cardno, Int32 node, Int32 index, Int32 subIndex, Int32 data, Int32 datalen);

        //旧邮箱读写接口, 暂时保留
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_read_EtherCAT_mailbox(Int32 cardno, Int32 slave, Int32 index, Int32 sub_index, Char[] data, Int32 length);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_write_EtherCAT_mailbox(Int32 cardno, Int32 slave, Int32 index, Int32 sub_index, Char[] data, Int32 length);
        
        //功  能：  Ethercat过程数据读取
        //参  数：  cardno      --  可用控制卡卡号
        //          addr        --  数据地址
        //          data        --  数据指针
        //          type        --  AL数据类型
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_ecat_pdo_read(Int32 cardno, UInt32 addr, Char[] data, UInt32 length);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_ecat_pdo_write(Int32 cardno, UInt32 addr, Char[] data, UInt32 length);
        
        //功  能：  设置规划轴资源数
        //参  数：  cardno      --  可用控制卡卡号
        //          axs_count   --  运动规划轴资源数, [1,32]
        //备  注：  1. 总线配置启动后, 会根据主站适配的轴资源改变
        //          2. 如果不连接从站, 仿真运动规划, 这里设置规划轴数后, 才能激活运动规划功能
        //          3. 可以使用adt_get_total_axis获取
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_axis_resource(Int32 cardno, Int32 axs_count);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_axis_resource(Int32 cardno, out Int32 axs_count);
        
        //功  能：  设置规划轴资源绑定输出通道
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  运动规划轴资源数, [1,32]
        //          type        --  轴类型, [0,2], 0:虚拟轴, 1:总线轴, 2:脉冲轴
        //          channel     --  输出通道号
        //备  注：  1. 绑定总线输出通道需要ECAT总线启动后, 确认底层总线轴通道实际存在才能绑定成功
        //          2. 注意规划轴号和输出通道号是单映射关系, 只能一对一绑定，重复绑定无效
        //          3. 重新绑定轴通道前, 先调用adt_reset_axis_bound函数复位底层绑定信息
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_axis_bound(Int32 cardno, Int32 axis, Int32 type, Int32 channel);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_axis_bound(Int32 cardno, Int32 axis, out Int32 type, out Int32 channel);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_reset_axis_bound(Int32 cardno);
        
        //功  能：  配置虚拟IO资源通道
        //参  数：  cardno      --  可用控制卡卡号
        //          cnl_count   --  虚拟IO资源通道数
        //备  注：  1. 虚拟IO资源开启后可以使用adt_read_busio_inbit等IO操作函数操作虚拟IO资源, 包括输入信号可设置修改
        //          2. 虚拟IO节点为实际总线资源节点后顺延, 比如实际总线配置10个节点, 这里配置1个虚拟IO通道, 虚拟IO节点为11
        //          3. 一个虚拟IO通道, 包含128个输入和128个输出资源
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_virtual_io_num(Int32 cardno, Int32 cnl_count);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_virtual_io_num(Int32 cardno, out Int32 cnl_count);
        
        //功  能：  获取Ethercat从站资源信息
        //参  数：  cardno      --  可用控制卡卡号
        //          slv_count   --  从站节点数
        //          srv_count   --  伺服节点数
        //          io_count    --  IO节点数, 非伺服都认为是IO设备
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_ecat_slave_resource(Int32 cardno, out Int32 slv_count, out Int32 srv_count, out Int32 io_count);
        
        //功  能：  获取Ethercat从站节点信息
        //参  数：  cardno      --  可用控制卡卡号
        //          node   --  节点号ID
        //          type   --  设备类型, [0,2], 0:空虚拟节点, 1:伺服设备, 2:非伺服设备
        //          axs_count    --  节点轴数, 用作伺服设备统计轴通道资源数, 用于获取多驱伺服轴数
        //          di_group    --  节点输入资源组数, 1组8位, 不满1组按1组计算
        //          do_group    --  节点输出资源组数, 1组8位, 不满1组按1组计算
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_ecat_slave_info(Int32 cardno, Int32 node, out Int32 type, out Int32 axs_count, out Int32 di_group, out Int32 do_group);
        
        //功  能：  获取轴号Ethercat从站节点映射
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号
        //          node        --  节点号
        //          channel     --  节点上通道号
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_axis_ecat_node(Int32 cardno, Int32 axis, out Int32 node, out Int32 channel);
        
        //功  能：  获取当前卡的轴资源数
        //参  数：  cardno      --  可用控制卡卡号
        //          axs_count   --  总轴数
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_total_axis(Int32 cardno, out Int32 axs_count);
        
        //功  能：  下载运动库系统配置文件
        //参  数：  cardno      --  可用控制卡卡号
        //          path        --  文件路径, 文件由配置工具生成, 可以提供多张卡拷贝使用 
        //备  注：  1. 文件参数, 包含控制卡安全限位等信息, 具体根据配置工具
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_load_nc_cfg(Int32 cardno, String path);
        
        //功  能：  导出运动库系统配置文件 
        //参  数：  cardno      --  可用控制卡卡号
        //          path        --  文件路径, 文件由配置工具生成, 可以提供多张卡拷贝使用 
        //备  注：  1. 文件参数, 包含控制卡安全限位等信息, 具体根据配置工具
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_export_nc_cfg(Int32 cardno, String path);

        //功  能：  设置与获取指定控制卡指定轴号的脉冲当量
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号, [1,32], 上限以当前系统实际连接总线伺服数为准
        //          equiv       --  脉冲当量, 1mm脉冲数, [50,25000]
        //备  注：  1. 脉冲当量的设置在脉冲当量编程模式下才有意义
        //          2. 默认配置为 equiv=1000 
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_pulse_equiv(Int32 cardno, Int32 axis, Double gear);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_pulse_equiv(Int32 cardno, Int32 axis, out Double gear);

        //功  能：  设置与获取指定控制卡指定轴号的编程模式
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号, [1,32], 上限以当前系统实际连接总线伺服数为准
        //          mode        --  编程模式, [0,1], 0: 脉冲当量编程模式  1: 脉冲编程模式
        //备  注：  1. 脉冲当量编程模式下, 速度与位置等驱动相关参数单位为mm; 脉冲编程模式下, 速度与位置等驱动相关参数单位为pulse
        //          2. 默认配置为 mode=0 
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_unit_mode(Int32 cardno, Int32 axis, Int32 mode);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_unit_mode(Int32 cardno, Int32 axis, out Int32 mode);

        //功  能：  设置与获取总线轴的电子齿轮比
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号, [1,32], 上限以当前系统实际连接总线伺服数为准
        //          encoder     --  编码器转动一圈发出的脉冲个数(编码器分辨率)
        //          elec        --  总线控制轴的电子齿轮比(大于1000)
        //备  注：  1. 默认值131072, encoder=elec
        //          2. 须满足elec<=encoder
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_bus_axis_gear_ratio(Int32 cardno, Int32 axis, Int32 encoder, Int32 elec);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_bus_axis_gear_ratio(Int32 cardno, Int32 axis, out Int32 encoder, out Int32 elec);

        //功  能：  设置与获取指定控制卡指定轴号设置生效的起始速度/终止速度/最大速度
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号, [1,32][60,63], 实体轴上限以当前系统实际连接总线伺服数为准
        //          speed       --  速度
        //备  注：  1. 点位驱动时, axis取[1,32], 插补驱动时,axis取[60,63]
        //          2. 基于脉冲当量编程模式时, 速度单位为mm/s, 取值[0, MAX], MAX与当前轴脉冲当量equiv须满足MAX*equiv<5M
        //             基于脉冲编程模式时, 尽管数据类型为Double, 速度单位仍为pulse/s, 取值[0, 5M)
        //          3. 若有非对称加减速规划需求, 终止速度须在起始速度之后设置, 减速度须在加速度之后设置
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_startv(Int32 cardno, Int32 axis, Double speed);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_startv(Int32 cardno, Int32 axis, out Double speed);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_endv(Int32 cardno, Int32 axis, Double speed);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_endv(Int32 cardno, Int32 axis, out Double speed);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_maxv(Int32 cardno, Int32 axis, Double speed);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_maxv(Int32 cardno, Int32 axis, out Double speed);

        //功  能：  设置与获取指定控制卡指定轴号设置生效的加速度/减速度
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号, [1,32][60,63], 实体轴上限以当前系统实际连接总线伺服数为准
        //          acc/dec     --  加速度/减速度
        //备  注：  1. 点位驱动时, axis取[1,32], 插补驱动时,axis取[60,63]
        //          2. 基于脉冲当量编程模式时, 速度单位为mm/s, 取值[0, MAX], MAX与当前轴脉冲当量equiv须满足MAX*equiv<100M
        //             基于脉冲编程模式时, 尽管数据类型为Double, 速度单位仍为pulse/s, 取值[0, 100M)
        //          3. 若有非对称加减速规划需求, 减速度须在加速度之后设置
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_acc(Int32 cardno, Int32 axis, Double acc);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_acc(Int32 cardno, Int32 axis, out Double acc);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_dec(Int32 cardno, Int32 axis, Double dec);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_dec(Int32 cardno, Int32 axis, out Double dec);

        //功  能：  设置与获取指定控制卡指定轴号的加减速模式
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号, [1,32][60,63], 实体轴上限以当前系统实际连接总线伺服数为准
        //          admode      --  加减速模式, [0,3], 0: S型  1: T型  2: EXP型  3: COS型
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_admode(Int32 cardno, Int32 axis, Int32 mode);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_admode(Int32 cardno, Int32 axis, out Int32 mode);
        
        //功  能：  设置轴运动参数  
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号, [1,32], 实体轴上限以当前系统实际连接总线伺服数为准
        //          admode      --  加减速模式, [0,3], 0: S型  1: T型  2: EXP型  3: COS型
        //          strv        --  起始速度
        //          maxv        --  最大速度
        //          acc/dec     --  加速度/减速度
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_axis_move_para_unit(Int32 cardno, Int32 axis, Int32 admode, Double strv, Double maxv, Double acc, Double dec);

        //功  能：  设置与获取指定控制卡指定轴号的速度倍率
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号, [0,32][60,63], 实体轴上限以当前系统实际连接总线伺服数为准, 轴号为0时为所有轴总倍率, 包括插补轴
        //          rate        --  倍率, [0,2.0]
        //备  注：  1. 倍率设置下发后, 总速度倍率会立即刷新，所以如果变化率过大，会导致速度产生阶跃, 建议分时设定
        //          2. 倍率为0时等同于设备立即暂停, 恢复倍率时, 已下发的运动数据将继续执行
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_rate(Int32 cardno, Int32 axis, Double rate);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_rate(Int32 cardno, Int32 axis, out Double rate);
        
        //功  能：  控制输出, 使能伺服电机 
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号, [0,32][60,63], 实体轴上限以当前系统实际连接总线伺服数为准, 轴号为0时为所有轴总倍率, 包括插补轴
        //          enable      --  使能标识, [0,1], 0:关闭使能, 1:使能
        //备  注：  1. 对于脉冲轴, 如果需要使用输出控制伺服电机上使能
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_axis_enable(Int32 cardno, Int32 axis, Int32 enable);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_axis_enable(Int32 cardno, Int32 axis, out Int32 enable);

        //功  能：  伺服报警清除
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号, [1,32]
        //备  注：  1. 通信连接异常及编码器报警不可清除, 须重新配置
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_clear_bus_axis_alarm(Int32 cardno, Int32 axis);
        
        //功  能：  设置编码器反馈分频比  
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号, [1,32]
        //          beta        --  分子(编码器实际值)
        //          alpha       --  分母(编码器当量值)
        //          mode        --  编码器返回值模式, 默认0返回编码器实际位置, 返回编码器分频后位置 
        //备  注：  1. 通信连接异常及编码器报警不可清除, 须重新配置
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_encoder_scale(Int32 cardno, Int32 axis, Int32 beta, Int32 alpha, Int32 mode);
        
        //功  能：  电机到位检测, 设置轴到位误差带  
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号, [1,32]
        //          band        --  误差带大小; 单位:pulse
        //          time        --  误差带保持时间; 单位(us)最小为一个指令周期
        //备  注：  1. 运动规划停止后, 逻辑位置和实际位置的误差小于设定误差带, 并且在误差带内保持设定时间后, 置起到位标志
        //          2. 注意使用adt_set_encoder_scale设置控制器同步伺服的电子齿轮比
        //          3. 注意保证编码器位置和规划位置, 方向相同
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_axis_band(Int32 cardno, Int32 axis, Int32 enable, Int32 band, Int32 time);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_axis_band(Int32 cardno, Int32 axis, out Int32 enable, out Int32 band, out Int32 time);

        //功  能：  设置轴最大跟随误差限制 
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号, [1,32]
        //          enable      --  使能标识, [0,1], 0:关闭, 1:使能
        //备  注：  1. 使能后, 伺服跟随滞后脉冲数大于设置的限制值时, 轴状态Bit9会置1提示, 但是不会限制轴运动功能
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_axis_pos_error(Int32 cardno, Int32 axis, out Int32 enable, Int32 pulse);

        //功  能：  获取指定控制卡指定轴号的速度
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号, [1,32][60,63], 实体轴上限以当前系统实际连接总线伺服数为准
        //          speed       --  速度
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_speed(Int32 cardno, Int32 axis, out Double speed);

        //功  能：  获取当前轴编码器速度
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号, [1,32], 实体轴上限以当前系统实际连接总线伺服数为准
        //          speed       --  轴编码器速度，单位(pulse/ms)       
        //备  注：  1. 如果设置了编码器分频输出, 获取的是分频后的速度  
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_encoder_speed(Int32 cardno, Int32 axis, out Double speed);

        //功  能：  设置与获取指定控制卡指定轴的逻辑位置
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号, [1,32], 上限以当前系统实际连接总线伺服数为准
        //          pos         --  逻辑位置, [-2147483648,2147483647]
        //备  注：  1. pos为整型数据, 即逻辑位置数据单位为pulse
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_command_pos(Int32 cardno, Int32 axis, Int32 pos);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_command_pos(Int32 cardno, Int32 axis, out Int32 pos);

        //功  能：  设置与获取指定控制卡指定轴的编码器实际位置
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号, [1,32], 上限以当前系统实际连接总线伺服数为准
        //          pos         --  编码器位置, [-2147483648,2147483647]
        //备  注：  1. pos为整型数据, 即编码器实际位置数据单位为pulse
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_actual_pos(Int32 cardno, Int32 axis, Int32 pos);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_actual_pos(Int32 cardno, Int32 axis, out Int32 pos);

        //功  能：  获取指定控制卡指定轴的绝对目标位置
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号, [1,32], 上限以当前系统实际连接总线伺服数为准
        //          pos         --  目标位置
        //备  注：  1. pos为Double数据类型时, 获取的位置单位为mm; pos为Int32数据类型时, 获取的位置单位为pulse
        //          2. 目标位置为绝对位置, 即相对于轴原点的位置, 通常在一次驱动完成后获取当前位置与目标位置并核对
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_target_pos_unit(int cardno, int axis, out Double pos);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_target_pos_pulse(int cardno, int axis, out Int32 pos);

        //功  能：  获取轴的驱动状态
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号, [0,32][60,63], 0:所有轴状态总结
        //          status      --  驱动状态, 0:所有轴都在静止, 1:至少有1个轴在运动规划中, 2:至少有1个轴在暂停中
        //备  注：  1. 该函数属于查询API, 如需连续查新, 两次查询中间建议插入一条Sleep(1)语句
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_axis_status(Int32 cardno, Int32 axis, out Int32 status);
        
        //功  能：  获取轴的驱动状态
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号, [1,32][60,63]
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_clear_axis_status(Int32 cardno, Int32 axis);

        //功  能：  获取轴的停止信息
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号, [1,32], 上限以当前系统实际连接总线伺服数为准
        //          value       --  停止信息, 多位二进制数据
        //                          0:正常停止
        //                          bit0=1:正限位触发停止
        //                          bit1=1:负限位触发停止
        //                          bit2=1:机械原点信号触发停止
        //                          bit3=1:编码器Z相信号触发停止
        //                          bit4=1:紧急停止信号触发停止
        //                          bit5=1:软件正限位触发停止
        //                          bit6=1:软件负限位触发停止
        //备  注：  1. 停止信息也可能组合出现, 故而查询时不判断是否相等, 而通过&运算判断是否发生指定信号触发
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_stopdata(Int32 cardno, Int32 axis, out Int32 value);

        //功  能：  获取总线轴的报警状态
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号, [1,32]
        //          alarm       --  报警状态
        //							_ERR_NONE: 无报警
        //        					_ERR_AXIS_ALARM: 轴报警
        //						    _ERR_EtherCAT_CONNECT: 总线轴通信故障
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_axis_alarm_info(Int32 cardno, Int32 axis, out Int32 alarm);
        
        //功  能：  获取所有轴运动状态
        //参  数：  cardno      --  可用控制卡卡号
        //          status      --  按位表示bit0-31表示, 轴1-MAXAXIS;  0停止, 1运动规划中;
        //备  注：  1. 到位状态需要设置并开启到位检测后有效
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_all_axis_move_status(Int32 cardno, Int32 axis, out UInt32 status);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_all_axis_reach_status(Int32 cardno, Int32 axis, out UInt32 status);

        //功  能：  轴停止驱动
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号, [0,32][60,63], 0表示停止所有轴(包括插补轴)驱动
        //          admode      --  停止模式, [0,1], 0:减速停止, 1:立即停止
        //备  注：  1. 该函数属于主动调用API, 发生调用一般属于非正常停止行为, 建议轴停止完全后, 调用一次adt_reset_card
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_axis_stop(Int32 cardno, Int32 axis, Int32 admode);

        //功  能：  设置PT FIFO模式
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号, [0,32], 0表示停止所有轴(包括插补轴)驱动
        //          mode        --  模式, [0,1], 0:静态, 不清除FIFO数据, 1:动态, 执行后清除FIFO数据
        //备  注：  1. 默认静态模式, 可以循环运行
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_pt_mode(Int32 cardno, Int32 axis, Int32 mode);
        
        //功  能：  发送PT数据  
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号, [0,32], 0表示停止所有轴(包括插补轴)驱动
        //          pos         --  位置, 单位mm, 相对量, 本段位置
        //          time        --  时间, 单位ms, 相对量, 本段时间
        //          type        --  模式, [0,2], 0:普通段, 两段直线首末速度相等, 默认为该类型   1:匀速段   2:减速段(速度减到0)
        //          count       --  PT数据段个数, 范围(1-1000)
        //备  注：  1. 需要先调用adt_set_pt_mode设置PT模式, 初始化PT运动参数
        //          2. 注意使用adt_set_pt_table, 需保证PT剩余缓存空间大于count;
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_pt_data(Int32 cardno, Int32 axis, Double pos, Int32 time, Int32 type);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_pt_table(Int32 cardno, Int32 axis, Int32 count, Double[] pos, Int32[] time, Int32[] type);

        //功  能：  清空PT缓存  
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号, [0,32], 0表示停止所有轴(包括插补轴)驱动
        //备  注：  1. 使用静态模式时, 缓存不会自动清空, 切换其他运动模式前必须先调用此函数
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_clr_pt_data(Int32 cardno, Int32 axis);
        
        //功  能：  获取PT缓冲区剩余段数 
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号, [0,32], 0表示停止所有轴(包括插补轴)驱动
        //          space       --  PT剩余缓存区大小
        //备  注：  1. PT最大缓存1000段数据, 下发PT数据前, 注意查询缓存区大小
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_pt_space(Int32 cardno, Int32 axis, out Int32 space);
        
        //功  能：  获取当前执行PT数据段索引 
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号, [0,32], 0表示停止所有轴(包括插补轴)驱动
        //          index       --  当前执行PT数据段索引    
        //备  注：  1. PT最大缓存1000段数据, 索引1-1000
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_pt_index(Int32 cardno, Int32 axis, out Int32 index);
 
        //功  能：  启动PT运动
        //参  数：  cardno      --  可用控制卡卡号
        //          axs_count   --  轴号, [0,32], 0表示停止所有轴(包括插补轴)驱动
        //          axs_list    --  当前执行PT数据段索引    
        //备  注：  1. 注意保证提前下发PT数据, PT数据为空, 命令不执行
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_pt_start(Int32 cardno, Int32 axs_count, Int32[] axs_list);
        
        //功  能：  设置循环运行(只有PT静态模式有效) 
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号, [0,32], 0表示停止所有轴(包括插补轴)驱动
        //          loop        --  循环次数, (0-100); 默认0, 即不循环, 只运行一次 	        
        //备  注：  1. PT启动前, 运动中, 设置有效
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_pt_loop(Int32 cardno, Int32 axis, Int32 loop);
        
        //功  能：  获取循环运行执行次数
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号, [0,32], 0表示停止所有轴(包括插补轴)驱动
        //          loop        --  当前循环执行次数     
        //备  注：  1. 注意不可死循环获取
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_pt_loop(Int32 cardno, Int32 axis, out Int32 loop);

        //功  能：  单轴JOG驱动
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号, [1,32], 上限以当前系统实际连接总线伺服数为准
        //          dir         --  驱动方向, [0,1], 0:正向, 1:负向
        //备  注：  1. 写入驱动命令前, 请正确地设定速度参数
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_continue_move(Int32 cardno, Int32 axis, Int32 dir);

        //功  能：  轴PTP点位驱动
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号, [1,32], 上限以当前系统实际连接总线伺服数为准
        //          pos         --  目标位置
        //          postype     --  位置模式, [0,1], 0:相对位置驱动, 1:绝对位置驱动
        //备  注：  1. pos为Double数据类型时, 获取的位置单位为mm; pos为Int32数据类型时, 获取的位置单位为pulse
        //          2. 不同编程模式下的轴请使用不同的PTP点位驱动接口, 基于脉冲当量编程的轴使用adt_pmove_unit,
        //             基于脉冲编程的轴使用adt_pmove_pulse
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_pmove_unit(Int32 cardno, Int32 axis, Double pos, Int32 postype);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_pmove_pulse(Int32 cardno, Int32 axis, Int32 pos, Int32 postype);

        //功  能：  运动中改变点位运动目标位置  
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号, [1,32], 上限以当前系统实际连接总线伺服数为准
        //          target      --  目标位置(绝对位置)  
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_change_pmove_pos_unit(Int32 cardno, Int32 axis, Double target);

        //功  能：  运动中改变点位运动目标位置  
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号, [1,32], 上限以当前系统实际连接总线伺服数为准
        //          target      --  目标位置
        //          postype     --  位置类型, [0,1], 0:相对位置, 1:绝对位置
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_change_pmove_pos(Int32 cardno, Int32 axis, Double target, Int32 postype);
        
        //功  能：  创建插补坐标系  
        //参  数：  cardno      --  可用控制卡卡号
        //          inp_axis    --  插补轴号, [60, 63]
        //          dim         --  插补轴数
        //          axs_list    --  实际物理轴列表
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_inp_coordinate(Int32 cardno, Int32 inp_axis, Int32 dim, Int32[] axs_list);
        
        //功  能：  设置插补组停止  
        //参  数：  cardno      --  可用控制卡卡号
        //          inp_axis    --  插补轴号, [60, 63]
        //          mode        --  停止模式, [0, 3], 0:减速停, 1:立即停, 2:暂停, 3:暂停恢复
        //备  注：  1. 插补暂停后, 可以单轴运动, 插补暂停恢复, 插补各轴需要回到暂停位置
        //          2. 可使用adt_get_inp_pause_pos获取最后一次插补暂停位置
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_inp_stop(Int32 cardno, Int32 inp_axis, Int32 mode);

        //功  能：  获取插补坐标系各轴暂停位置  
        //参  数：  cardno      --  可用控制卡卡号
        //          inp_axis    --  插补轴号, [60, 63]
        //          pos         --  坐标系各轴暂停位置 	
        //备  注：  1. 插补暂停位置只保存最后一次暂停位置
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_inp_pause_pos(Int32 cardno, Int32 inp_axis, Int32[] pos);

        //功  能：  获取插补轴插补缓存余量
        //参  数：  cardno      --  可用控制卡卡号
        //          inpaxis     --  插补轴号,[60,63]
        //          len         --  插补轴缓存余量
        //备  注：  1. 每个插补轴都有10000段插补缓存区, 插补缓存区满时, 须等待余量足够时才能继续插入缓存插补数据
        //          2. 插补缓存区不同于缓存事件区
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_inp_fifo_len(Int32 cardno, Int32 inpaxis, out Int32 len);

        //功  能：  获取当前插补驱动的索引信息
        //参  数：  cardno      --  可用控制卡卡号
        //          inpaxis     --  插补轴号,[60,63]
        //          index       --  插补驱动索引信息
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_inp_index(Int32 cardno, Int32 inpaxis, out Int32 index);

        //功  能：  可缓存式多轴直线插补
        //参  数：  cardno      --  可用控制卡卡号
        //          inpaxis     --  插补轴号, [60,63]
        //          index       --  插补索引信息, 用于标识当前驱动指令, 默认为0即可
        //          axs_count   --  参与插补的轴数, [1,32], 上限以当前系统实际连接总线伺服数为准
        //          axs_list    --  参与插补的轴号列表
        //          pos_list    --  参与插补的各轴位置列表
        //          postype     --  位置模式, [0,1], 0:相对位置驱动, 1:绝对位置驱动
        //备  注：  1. 参与插补的轴数不能少于1, 也不能大于当前控制卡可用轴数
        //          2. axs_list和pos_list数组大小须>=axs_count, 且轴列表中不能有重复的轴
        //          3. 不同编程模式下的轴请使用不同的直线插补驱动接口, 基于脉冲当量编程的轴使用adt_inp_move_unit,
        //             基于脉冲编程的轴使用adt_inp_move_pulse
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_inp_move_pulse(Int32 cardno, Int32 inpaxis, Int32 index, Int32 axs_count, Int32[] axs_list, Int32[] pos_list, Int32 postype);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_inp_move_unit(Int32 cardno, Int32 inpaxis, Int32 index, Int32 axs_count, Int32[] axs_list, Double[] pos_list, Int32 postype);

        //功  能：  可缓存式2轴平面圆弧插补
        //参  数：  cardno      --  可用控制卡卡号
        //          inpaxis     --  插补轴号, [60,63]
        //          index       --  插补索引信息, 用于标识当前驱动指令, 默认为0即可
        //          axs_list    --  参与插补的轴号列表
        //          pos_list    --  参与插补的各轴目标位置列表
        //          center_list --  参与插补的各轴圆弧圆心位置列表
        //          dir         --  圆弧方向, [0,1], 0:逆时针, 1:顺时针
        //          postype     --  位置模式, [0,1], 0:相对位置驱动, 1:绝对位置驱动
        //备  注：  1. axs_list、pos_list、center_list数组大小须>=2, 且轴列表中不能有重复的轴
        //          2. 2轴平面圆弧插补仅支持基于脉冲当量编程的轴
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_inp_arc2_unit(Int32 cardno, Int32 inpaxis, Int32 index, Int32[] axs_list, Double[] pos_list, Double[] center_list, Int32 dir, Int32 postype);

        //功  能：  可缓存式3轴空间圆弧插补
        //参  数：  cardno      --  可用控制卡卡号
        //          inpaxis     --  插补轴号, [60,63]
        //          index       --  插补索引信息, 用于标识当前驱动指令, 默认为0即可
        //          axs_list    --  参与插补的轴号列表
        //          pos2_list   --  参与插补的各轴圆弧上第2点位置列表
        //          pos3_list   --  参与插补的各轴圆弧上第3点位置列表
        //          flag        --  圆弧模式, [0,1], 0:圆弧, 1:整圆
        //          postype     --  位置模式, [0,1], 0:相对位置驱动, 1:绝对位置驱动
        //备  注：  1. axs_list、pos_list、center_list数组大小须>=3, 且轴列表中不能有重复的轴
        //          2. 3轴平面圆弧插补仅支持基于脉冲当量编程的轴
        //          3. 3轴圆弧插补也可以用在二维圆弧插补场合, 将第3个轴的轴号写为0, 相应的位移参数都写为0
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_inp_arc3_unit(Int32 cardno, Int32 inpaxis, Int32 index, Int32[] axs_list, Double[] pos2_list, Double[] pos3_list, Int32 flag, Int32 postype);

        //功  能：  任意两轴圆弧插补, 二轴跟随
        //参  数：  cardno      --  可用控制卡卡号
        //          inpaxis     --  插补轴号, [60,63]
        //          index       --  插补索引信息, 用于标识当前驱动指令, 默认为0即可
        //          axs_list    --  axs_list[0],axs_list[1] 为平面圆弧插补坐标系内轴号
        //                          axs_list[2],axs_list[3] 为随动轴坐标系内轴号, 为0则表示不跟随
        //          tgt_list    --  tgt_list[0],tgt_list[1]为圆弧的目标点坐标;  单位:unit
        //                          tgt_list[2],tgt_list[3]为随动轴的目标点坐标;单位:unit
        //          ctr_list    --  ctr_list[0],ctr_list[1]为圆弧的目标点坐标;  单位:unit
        //                          ctr_list[2],ctr_list[3]为随动轴的目标点坐标;单位:unit
        //          pos3_list   --  参与插补的各轴圆弧上第3点位置列表
        //          dir         --  圆弧方向, [0,1], 0:逆时针, 1:顺时针
        //          postype     --  位置模式, [0,1], 0:相对位置驱动, 1:绝对位置驱动
        //备  注：  1. 如果调用该函数来画一个封闭螺旋线, 则需启用速度前瞻, 否则走到一半时会出现加减速过程
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_inp_helix2_unit(Int32 cardno, Int32 inpaxis, Int32 index, Int32[] axs_list, Double[] tgt_list, Double[] pos3_list, Int32 dir, Int32 postype);

        //功  能：  任意三轴圆弧插补, 二轴跟随
        //参  数：  cardno      --  可用控制卡卡号
        //          inpaxis     --  插补轴号, [60,63]
        //          index       --  插补索引信息, 用于标识当前驱动指令, 默认为0即可
        //          axs_list    --  axs_list[0],axs_list[1],axs_list[2] 为平面圆弧插补坐标系内轴号
        //                          axs_list[3],axs_list[4] 为随动轴坐标系内轴号, 为0则表示不跟随
        //          pos2_list   --  pos2_list[0],pos2_list[1],pos2_list[2]为圆弧上第二点坐标;  单位:unit
        //          pos3_list   --  pos3_list[0],pos3_list[1],pos3_list[2]为圆弧上第三点坐标;  单位:unit
        //                          pos3_list[2],pos3_list[3]为随动轴的目标点坐标;单位:unit
        //          flag        --  圆弧类型, [0,1], 0:圆弧, 1:整圆
        //          postype     --  位置模式, [0,1], 0:相对位置驱动, 1:绝对位置驱动
        //备  注：  1. 如果调用该函数来画一个封闭螺旋线, 则需启用速度前瞻, 否则走到一半时会出现加减速过程
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_inp_helix3_unit(Int32 cardno, Int32 inpaxis, Int32 index, Int32[] axs_list, Double[] pos2_list, Double[] pos3_list, Int32 flag, Int32 postype);

        //功  能：  设置指定控制卡指定插补轴的圆弧速度约束
        //参  数：  cardno      --  可用控制卡卡号
        //          inpaxis     --  插补轴号, [60,63]
        //          radius      --  圆弧半径系数
        //          speed       --  约束速度系数, [0.01, 100000], 单位mm/s
        //备  注：  1. 平面圆弧或空间圆弧插补中生效, 由于圆弧插补仅支持基于脉冲当量编程模式, 故而该接口也仅用于基于脉冲当量编程模式
        //          2. 速度约束即圆弧工艺过程中, 插补轴驱动速度上限, 接口设定生效后, 若加工过程中实际圆弧半径<radius, 加工速度将以
        //             speed为基数等比约束, 若实际半径>radius, 加工速度将以speed为基数等比放大
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_arc_speed_clamp_unit(Int32 cardno, Int32 inpaxis, Double radius, Double speed);

        //功  能：  设置指定控制卡指定插补轴的速度前瞻模式
        //参  数：  cardno      --  可用控制卡卡号
        //          inpaxis     --  插补轴号, [60,63]
        //          enable      --  使能标识, [0,1], 0: 不使能, 1: 使能
        //备  注：  1. 该接口是是否启用缓存插补的关键接口
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_speed_pretreat_mode(Int32 cardno, Int32 inpaxis, Int32 enable);

        //功  能：  设置指定控制卡指定轴号的速度平滑等级
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号, [1,32], 上限以当前系统实际连接总线伺服数为准
        //          level       --  平滑等级, [1,100]
        //备  注：  1. 速度平滑仅在缓存插补过程, 即速度前瞻功能启用后生效
        //          2. 默认level=50, 能满足大部分工艺需求
        //          3. 增大拐角速度的平滑等级会提高机床整体的加工效率与运动稳定性, 但是也可能会引起拐角处单轴较大的启停速度
        //          4. 对应负载较大(较重)的轴建议适当调低平滑等级, 防止失步或者跟随滞后
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_corner_speed_smooth_level(Int32 cardno, Int32 axis, Int32 level);

        //功  能：  获取插补轴缓存事件余量
        //参  数：  cardno      --  可用控制卡卡号
        //          inpaxis     --  插补轴号,[60,63]
        //          len         --  插补轴缓存事件余量
        //备  注：  1. 每个插补轴都有1000段缓存事件区, 缓存事件区满时, 须等待余量足够时才能继续插入缓存事件数据
        //          2. 插补事件区不同于缓存插补区
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_fifo_event_len(Int32 cardno, Int32 inpaxis, out Int32 len);

        //功  能：  获取当前缓存事件的索引信息
        //参  数：  cardno      --  可用控制卡卡号
        //          inpaxis     --  插补轴号,[60,63]
        //          index       --  缓存事件索引信息
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_fifo_event_index(Int32 cardno, Int32 inpaxis, out Int32 index);
        
        //功  能：  设置提前滞后触发时间
        //参  数：  cardno      --  可用控制卡卡号
        //          inpaxis     --  插补轴号,[60,63]
        //          mstime      --  时间, 单位(ms), 大于0滞后触发, 小于0提前触发
        //备  注：  1. 只有一段插补缓存数据不做提前滞后判断
        //          2. 在设置缓存事件之前设置,设置后只对此缓存事件有效
        //          3. 提现或滞后时间, 不大于缓存数据前一段或后一段运动时间, 底层自动判断, 函数不会参数报错提示
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_fifo_adjust_time(Int32 cardno, Int32 inpaxis, Double mstime);
        
        //功  能：  设置提前滞后触发位置
        //参  数：  cardno      --  可用控制卡卡号
        //          inpaxis     --  插补轴号,[60,63]
        //          pos         --  插补合成位置, 大于0滞后触发, 小于0提前触发  
        //备  注：  1. 只有一段插补缓存数据不做提前滞后判断
        //          2. 在设置缓存事件之前设置,设置后只对此缓存事件有效
        //          3. 按时间调节优先级大于按位置调节, 如果设置了按时间调节, 则按位置调节不会生效
        //          4. 提现或滞后位置, 不大于缓存数据前一段或后一段轨迹距离, 底层自动判断, 函数不会参数报错提示
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_fifo_adjust_pos(Int32 cardno, Int32 inpaxis, Double pos);

        //功  能：  设置缓存输出控制
        //参  数：  cardno      --  可用控制卡卡号
        //          inpaxis     --  插补轴号, [60,63]
        //          index       --  插补索引信息, 用于标识当前驱动指令, 默认为0即可
        //          board       --  扩展IO板节点号, [1,32]
        //          port        --  输出端口号, [0,15]
        //          value       --  控制电平, [0,1], 0:低电平, 1:高电平
        //          speed       --  输出控制时的速度约束, 默认-1, 即不约束速度, 约束时[0.0-5000]mm/s
        //备  注：  1. 输出控制时的速度约束, 为控制开始前即达到速度约束值, 而非开始控制时降速或提速
        //          2. 缓存事件控制仅适用于基于脉冲当量编程的轴
        //          3. 目前缓存插补事件仅支持插补轴63, 后续会支持插补轴62
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_fifo_outbit(Int32 cardno, Int32 inpaxis, Int32 index, Int32 board, Int32 port, Int32 value, Double speed);
        
        //功  能：  设置缓存输出事件, 同时设置多个输出点电平
        //参  数：  cardno      --  可用控制卡卡号
        //          inpaxis     --  插补轴号, [60,63]
        //          index       --  插补索引信息, 用于标识当前驱动指令, 默认为0即可
        //          board       --  扩展IO板节点号, [1,32]
        //          group       --  输出端口组号, 8位1组
        //          en_map      --  按位(bit0～bit8)指定要操作的输出点, 位值为1则对相应的输出点进行操作, 位值为0的输出点不受影响
        //          out_map     --  控制电平, [0,1], 0:低电平, 1:高电平
        //          speed       --  输出控制时的速度约束, 默认-1, 即不约束速度, 约束时[0.0-5000]mm/s
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_fifo_multi_io(Int32 cardno, Int32 inpaxis, Int32 index, Int32 board, Int32 group, Int32 en_map, Int32 out_map, Double speed);

        //功  能：  设置缓存暂停事件
        //参  数：  cardno      --  可用控制卡卡号
        //          inpaxis     --  插补轴号, [60,63]
        //          index       --  插补索引信息, 用于标识当前驱动指令, 默认为0即可
        //          millisecond --  暂停时间, 单位ms
        //备  注：  1. 暂停时, 缓存插补等驱动将保持速度为0, 已作出的输出控制电平将保持
        //          2. 目前缓存插补事件仅支持插补轴63, 后续会支持插补轴62
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_fifo_delay(Int32 cardno, Int32 inpaxis, Int32 index, Int32 millisecond);

        //功  能：  清除尚未执行完毕的缓存事件
        //参  数：  cardno      --  可用控制卡卡号
        //          inpaxis     --  插补轴号, [60,63]
        //备  注：  1. 目前缓存插补事件仅支持插补轴63, 后续会支持插补轴62
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_clear_fifo_event(Int32 cardno, Int32 inpaxis);

        //功  能：  设置与获取原点/限位/急停信号输入端口映射
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号, [1,32], 上限以当前系统实际连接总线伺服数为准
        //          mode        --  信号类型, [0,4], 0:正限位, 1:负限位, 2:原点, 3:保留, 4:急停(此时axis无实际意义)
        //          board       --  节点号, [0,16], 0表示伺服本身IO, 1-16表示扩展IO节点号
        //          port        --  输入端口号, [0,15]
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_axis_io_map(Int32 cardno, Int32 axis, Int32 mode, Int32 board, Int32 port);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_axis_io_map(Int32 cardno, Int32 axis, Int32 mode, out Int32 board, out Int32 port);

        //功  能：  设置与获取原点/限位/急停信号模式与有效电平
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号, [1,32], 上限以当前系统实际连接总线伺服数为准
        //          mode        --  信号类型, [0,4], 0:正限位, 1:负限位, 2:原点, 3:保留, 4:急停(此时axis无实际意义)
        //          enable      --  使能标识, [0,1], 0:关闭, 1:使能
        //          logic       --  有效电平, [0,1], 0:低电平有效, 1:高电平有效
        //          admode      --  停止模式, [0,1], 0:减速停止, 1:立即停止
        //备  注：	1. 默认正负限位低电平有效, 原点/紧急停止信号无效
        //          2. 正负限位和紧急停止信号仅支持立即停止模式
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_hardlimit_mode(Int32 cardno, Int32 axis, Int32 mode, Int32 enable, Int32 logic, Int32 admode);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_hardlimit_mode(Int32 cardno, Int32 axis, Int32 mode, out Int32 enable, out Int32 logic, out Int32 admode);
        
        //功  能：  设置停止信号模式
        //参  数：  cardno      --  可用控制卡卡号
        //          enable      --  使能标识, [0,1], 0:关闭, 1:打开
        //          logic       --  有效电平, [0,1], 0:低电平有效, 1:高电平有效
        //备  注：	1. 急停生效后, 所有轴运动控制立即停止, 需要调用adt_reset_card后才能恢复使用
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_emergency_stop(Int32 cardno, Int32 enable, Int32 logic);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_emergency_stop(Int32 cardno, out Int32 enable, out Int32 logic);
        
        //功  能：  设置stop0(机械原点)信号
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号, [1,32], 上限以当前系统实际连接总线伺服数为准
        //          enable      --  使能标识, [0,1], 0:关闭, 1:打开
        //          logic       --  有效电平, [0,1], 0:低电平有效, 1:高电平有效
        //          admode      --  停止模式, [0,1], 0:减速停止, 1:立即停止
        //备  注：	1. 默认关闭
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_stop0_mode(Int32 cardno, Int32 axis, Int32 enable, Int32 logic);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_stop0_mode(Int32 cardno, Int32 axis, out Int32 enable, out Int32 logic, out Int32 admode);

        //功  能：  设置正负限位信号模式
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号, [1,32], 上限以当前系统实际连接总线伺服数为准
        //          pel_enable  --  正限位使能标识, [0,1], 0:关闭, 1:打开
        //          mel_enable  --  负限位使能标识, [0,1], 0:关闭, 1:打开
        //          logic       --  有效电平, [0,1], 0:低电平有效, 1:高电平有效
        //          admode      --  停止模式, [0,1], 0:减速停止, 1:立即停止
        //备  注：	1. 默认关闭
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_limit_mode(Int32 cardno, Int32 axis, Int32 pel_enable, Int32 mel_enable, Int32 logic);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_limit_mode(Int32 cardno, Int32 axis, out Int32 pel_enable, out Int32 mel_enable, out Int32 logic);

        //功  能：  设置软件限位模式(基于脉冲当量编程)
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号, [1,32], 上限以当前系统实际连接总线伺服数为准
        //          enable      --  软限位启禁标识, [0,1], 0:禁用, 1:启用
        //          ppos        --  软件正限位位置, 单位mm
        //          npos        --  软件负限位位置, 单位mm
        //          admode      --  软限位触发停止模式, [0,1], 0:减速停止, 1:立即停止
        //备  注：  1. 默认软限位无效
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_softlimit_mode(Int32 cardno, Int32 axis, Int32 enable, Double ppos, Double npos, Int32 admode);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_softlimit_mode(Int32 cardno, Int32 axis, out Int32 enable, out Double ppos, out Double npos, out Int32 admode);

        //功  能：  设置轴驱动跟随(开环龙门双驱)
        //参  数：  cardno      --  可用控制卡卡号
        //          slave       --  从轴号, [1,32]
        //          master      --  主轴号, [0,16], 0表示取消从轴跟随
        //备  注：  1. slave不能等于master
        //          2. 一个主轴可以有多个跟随轴, 但每个跟随轴有且只有最多一个主轴
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_follow_axis(Int32 cardno, Int32 slave, Int32 master);

        //功  能：  设置/获取指定控制卡指定轴号的逻辑位置可变环功能
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号, [1,32], 上限以当前系统实际连接总线伺服数为准
        //          enable      --  使能标识, [0,1], 0: 不使能, 1: 使能
        //          comppos     --  变环位置, (0,∞), 单位为pulse
        //备  注：  1. 默认不使能
        //          2. 逻辑变环多用于传送带这种同向周期性驱动, 逻辑位置到达comppos时将被清除并从0开始重新计数
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_pos_variable_loop(Int32 cardno, Int32 axis, Int32 enable, Int32 comppos);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_pos_variable_loop(Int32 cardno, Int32 axis, out Int32 enable, out Int32 comppos);

        //功  能：  位操作 —— 读取指定位的值/指定位写1/指定位写0/指定位取反
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_bit(Int32 value, Int32 bit);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_bit(ref Int32 value, Int32 bit);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_clr_bit(ref Int32 value, Int32 bit);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_cpl_bit(ref Int32 value, Int32 bit);
        
        //功  能：  根据轴号读取总线伺服IO输入状态
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号, [1,32], 上限以当前系统实际连接总线伺服数为准
        //          index       --  输入点索引, 从0开始
        //          state       --  0:常态  1:非常态 具体看伺服定义
        //          group       --  输入点组号, 8位一组, 从0开始
        //          inmap       --  输入状态, bit0--bit7有效, 按位映射
        //备  注：  1. 需要保证绑定轴号的伺服有IO资源映射
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_read_servo_inbit(Int32 cardno, Int32 axis, Int32 index, out Int32 state);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_read_servo_inport(Int32 cardno, Int32 axis, Int32 group, out Int32 inmap);
        
        //功  能：  操作总线伺服IO输出点
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号, [1,32], 上限以当前系统实际连接总线伺服数为准
        //          index       --  输入点索引, 从0开始
        //          value       --  0:关闭   1:打开
        //备  注：  1. 需要保证绑定轴号的伺服有IO资源映射
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_write_servo_outbit(Int32 cardno, Int32 axis, Int32 index, Int32 value);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_read_servo_outbit(Int32 cardno, Int32 axis, Int32 index, out Int32 value);
        
        //功  能：  按节点号读取总线IO输入点状态
        //参  数：  cardno      --  可用控制卡卡号
        //          node        --  扩展IO节点号, [1,32], 上限以当前系统实际连接总线伺服数为准
        //          index       --  输入点索引, 从0开始
        //          state       --  0:常态  1:非常态 具体看伺服定义
        //          group       --  输入点组号, 8位一组, 从0开始
        //          inmap       --  输入状态, bit0--bit7有效, 按位映射
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_read_busio_inbit(Int32 cardno, Int32 node, Int32 index, out Int32 state);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_read_busio_inport(Int32 cardno, Int32 node, Int32 group, out Int32 inmap);
        
        //功  能：  按节点号设置总线IO输入点状态
        //参  数：  cardno      --  可用控制卡卡号
        //          node        --  扩展IO节点号, [1,32], 上限以当前系统实际连接总线伺服数为准
        //          index       --  输入点索引, 从0开始
        //          state       --  0:常态  1:非常态 具体看伺服定义
        //          group       --  输入点组号, 8位一组, 从0开始
        //          inmap       --  输入状态, bit0--bit7有效, 按位映射
        //备  注：  1. 仅支持虚拟IO节点操作有效,虚拟IO节点, 输入128bit, 输出128bit
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_write_busio_inbit(Int32 cardno, Int32 node, Int32 index, Int32 state);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_write_busio_inport(Int32 cardno, Int32 node, Int32 group, Int32 inmap);

        //功  能：  按节点号操作总线IO板输出点
        //参  数：  cardno      --  可用控制卡卡号
        //          node        --  扩展IO节点号, [1,32], 上限以当前系统实际连接总线伺服数为准
        //          index       --  输入点索引, 从0开始
        //          value       --  0: 关闭	   1: 打开
        //          group       --  输入点组号, 8位一组, 从0开始
        //          outmap      --  输出点, bit0--bit7有效, 按位映射
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_write_busio_outbit(Int32 cardno, Int32 node, Int32 index, Int32 value);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_write_busio_outport(Int32 cardno, Int32 node, Int32 group, Int32 outmap);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_read_busio_outbit(Int32 cardno, Int32 node, Int32 index, out Int32 value);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_read_busio_outport(Int32 cardno, Int32 node, Int32 group, out Int32 outmap);

        //功  能：  修改/设置用户密码 
        //参  数：  cardno      --  可用控制卡卡号
        //          oldpassword --  旧密码
        //          newpassword --  新密码
        //返回值：  0: 成功     1: 密码长度大于128,或新密码和旧密码相同
        //备  注：  1. 出厂没有密码, 若要加密, 第一次使用则需要用户先进行密码设定, 设定时, oldpassword为空即可
        //          2. 如果忘记密码, 需要寄回原厂破解, 所以密码设定后请自行备份
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_user_password(Int32 cardno, String oldpassword, String newpassword);

        //功  能：  校验用户密码 
        //参  数：  cardno      --  可用控制卡卡号
        //          password    --  校验密码
        //返回值：  0: 校验通过     1: 校验不通过
        //备  注：  1. 密码校验失败3次后会锁卡, 再校验需要将电脑关机断电重启
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_check_password(Int32 cardno, String password);

        //功  能：  回零模式设置 
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号, [1,32]
        //          mode        --  回零模式, [0,3], 2位二进制数AB, 其中
        //                          低位D0, B表示回零方向, 0:负方向 1:正方向
        //                          中位D1, A表示回零类型, 0:直线回零, 即机械原点位置固定停留在机械原点传感器靠近正限位一侧
        //                                                 1:圆周回零, 即机械原点位置固定停留在首次接触机械原点传感器的位置
        //          stop0       --  机械原点有效电平, [0,1], 0:低电平有效  1:高电平有效
        //          limit       --  回零限位模式, [0,7], 三位二进制数ABC, 其中
        //                          低位D0, C表示限位有效电平, 0:低电平有效 1:高电平有效
        //                          中位D1, B表示负限位启禁标识, 0:负限位有效   1:负限位无效
        //                          高位D2, A表示正限位启禁标识, 0:负限位有效   1:负限位无效
        //          back_range  --  反向距离, (0,正限位与机械原点信号间距)
        //          offset      --  原点偏移
        //返回值：  0: 设置成功     -x: 第x个参数错误
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_home_mode(Int32 cardno, Int32 axis, Int32 mode, Int32 stop0, Int32 limit, Double back_range, Double offset);

        //功  能：  回零速度参数
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号, [1,32]
        //          startv      --  回零起始速度
        //          searchv     --  回零高速搜索速度
        //          homev       --  回零二次搜索低速速度
        //          acc         --  回零加速度
        //返回值：  0: 设置成功     -x: 第x个参数错误
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_home_speed(Int32 cardno, Int32 axis, Double startv, Double searchv, Double homev, Double acc);
        
        //功  能：  启动回零
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号, [1,32]
        //返回值：  0: 启动成功     -x: 第x个参数错误
        //备  注：  启动回零进程后需要不断调用adt_get_home_status获取回零状态并触发下一步回零动作, 直至回零完成
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_home_process(Int32 cardno, Int32 axis);

        //功  能：  获取回零状态
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号, 6轴卡[1,6], 8轴卡[1,8]
        //返回值：  0: 回零成功完成     
        //          >0:回零中
        //          -1:卡号参数错误
        //          -2:轴号参数错误
        //          -3:回零未启动
        //          -10xx:回零失败, 问题发生在第xx步骤. 通常-1004多发, 意为二次搜索机械原点之前并未成功退出原点信号, 原因多为如下两种
        //                a. 反退距离过小, 导致反退距离驱动完成后, 原点信号仍旧在触发当中
        //                b. 反退距离过大, 导致反退距离驱动过程中触发了硬件限位
        //          -1020:外部停止回零
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_home_status(Int32 cardno, Int32 axis);

        //功  能：  SDO控制伺服Z相参数设置
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号, [1,32], 上限以当前系统实际连接总线伺服数为准
        //          home_offset --  回零原点偏移, 单位:脉冲
        //          home_v1     --  高速搜索速度, 单位:脉冲/s
        //          home_v2     --  低速接近速度, 单位:脉冲/s
        //          home_acc    --  加速度, 单位:脉冲/s2
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_servo_home_para(Int32 cardno, Int32 axis, Int32 home_offset, Int32 home_v1, Int32 home_v2, Int32 home_acc);

        //功  能：  SDO控制伺服Z相回零
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号, [1,32], 上限以当前系统实际连接总线伺服数为准
        //          home_mode   --  回零模式, [1,35], 具体参考CIA402协议
        [DllImport("adtemc.dll")]  
        public static extern Int32 adt_set_servo_home(Int32 cardno, Int32 axis, Int32 home_mode);

        //功  能：  获取伺服Z回零状态  
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号, [1,32], 上限以当前系统实际连接总线伺服数为准
        //返回值：  0 -- 回零成功  1 -- 回零过程中  -1 -- 回零命令错误  -2 -- 回零过程出错或超时  -3 -- 回零伺服报警
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_servo_home_status(Int32 cardno, Int32 axis);
        
        //功  能：  停止伺服回零  
        //参  数：  cardno      --  可用控制卡卡号
        //          axis        --  轴号, [1,32], 上限以当前系统实际连接总线伺服数为准
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_stop_servo_home(Int32 cardno, Int32 axis);
        
        //功  能：  任意两轴或3轴样条（nurbs）曲线指令
        //参  数：  cardno      --  可用控制卡卡号
        //          inp_axis    --  插补轴号, [60,63]
        //          pos_type    --  0:相对位置插补  1:绝对位置插补
        //          index       --  索引信息, 用于标识本次运动信息, 一般设置为0即可
        //          axs_list    --  轴号列表, 若使用2轴平面插补, axs_list[0], axs_list[1]设置轴号, 第三轴设置为0.
        //							例如使用1,3轴进行平面nurbs曲线插补, 则axs_list[3]={1,3,0}
        //  		pos_data	--	用户输入拟合点坐标数组, 单位mm。数值范围[-10000,10000].3列分别表示第一轴，第二轴，第三轴坐标值
        //							例如pos_data[4][3]表示4个点的三维坐标值, 当axs_list设置为两轴平面曲线插补时, 这里的第三个轴坐标可以不用设置
        //          count       --	用户输入数据点的个数, 即指明PosData数组中坐标点的个数, PosData[][3]坐标点的个数必须与DataNum一致
        //		    vec_mode	--	起点和终点切线确定模式, [1,4], 1:指定导矢开曲线; 2:不指定导矢闭曲线, 使用默认导矢; 3:不指定导矢开曲线,使用默认导矢; 4:指定导矢闭曲线
        //          start_vec	--	起点切线方向, 是一个三维向量, 模必须大于0. 可以是一般向量或单位方向向量, 在使用模式1和模式4时, 必须指定起点和终点的切线方向
        //          end_vec		--	终点切线方向, 是一个三维向量, 模必须大于0. 可以是一般向量或单位方向向量, 在使用模式1和模式4时, 必须指定起点和终点的切线方向
        //          inpmode     --	插补模式, [3,4], 3:按指定步长插补, 插补轨迹不一定经过教导点; 4:按指定步长插补, 插补轨迹经过教导点
        //  		inp_para    --  inp_para[0], 样条曲线插补的步长, 单位mm, 需要设置合适的值（需大于0.1mm）,设置过大或过小有可能速度不顺滑, 发生振动
        // 		  					inp_para[1], 保留参数
        //备  注：  1. pos_data坐标点的个数必须>=count
        //          2. 在使用模式1和模式4时, 必须指定起点和终点的切线方向; 在模式2和模式3时, start_vec和end_vec参数不起作用, 元素的值设置为0即可
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_nurbs_curve(Int32 cardno, Int32 inp_axis, Int32 pos_type, Int32 index, Int32 count, Int32[] axs_list, Double[,] pos_data, Int32 vec_mode, Double start_vec, Double end_vec, Int32 inp_mode, Double[] inp_para);

        /************************************************************************/
        /*--------------------------圆弧过渡算法类接口--------------------------*/
        /************************************************************************/
        public struct arc_blend_ini_struct 
        {
            public double min_len; 			    //过渡前后直线或圆弧的长度下限, [0.001,10], 单位:mm
            public double min_R; 				    //过渡圆弧的半径长度下限, [0.001,10], 单位:mm(若求出的过渡圆弧半径小于此值则返回失败)
            public double max_inter_angle; 	    //过渡前直线段或圆弧间的夹角上限, [135,179.9], 单位:degree(度, 若夹角大于此值不过渡)
            public double min_inter_angle; 	        //过渡前直线段或圆弧间的夹角下限, [1,90], 单位:degree(度, 若夹角小于此值不过渡)
            public double tolerance; 			    //圆弧过渡精度值(过渡后最大误差), [0.001,10], 单位:mm
        };
        public enum ARC_BLEND_TYPE
        {
            BLEND_LINE_LINE,	//直线到直线
            BLEND_LINE_ARC,		//直线到圆弧
            BLEND_ARC_LINE,		//圆弧到直线
            BLEND_ARC_ARC,		//圆弧到圆弧
        }
        public struct arc_blend_xyz
        {
            public double x;		//1轴坐标
            public double y;		//2轴坐标
            public double z;		//3轴坐标
        };
        public struct arc_blend_inout_struct
        {
            public ARC_BLEND_TYPE type;         //输入曲线类型, 有四种, 具体参照枚举 ARC_BLEND_TYPE 定义
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public arc_blend_xyz[] seg1;		//输入第一条曲线(直线或圆弧)坐标数据
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public arc_blend_xyz[] seg2; 		//输入第二条曲线(直线或圆弧)坐标数据
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public arc_blend_xyz[] blendok_seg1;//过渡成功后, 输出第一条曲线(直线或圆弧)坐标数据
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public arc_blend_xyz[] blendok_seg2;//过渡成功后, 输出过渡圆弧坐标数据
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public arc_blend_xyz[] blendok_seg3;//过渡成功后, 输出第三条曲线(直线或圆弧)坐标数据
            public double BlendR;				//过渡成功后, 输出过渡圆弧的半径
            //备注： 1.对于结构体arc_blend_xyz定义的变量, 例如:第一段线段, 若是直线, 则seg1[0]为直线起点, seg1[1]为直线终点
    		//         若是圆弧段, 则seg1[0]为弧起点, seg1[1]为弧任意中间点(第二点), seg1[2]为弧终点
            //       2.所有坐标位置均为绝对位置, 故而中间的圆弧过渡必须要走绝对圆弧
        };

        //功  能：  对过渡圆弧处理时的参数阈值进行初始化
        //参  数：  cardno      --  可用控制卡卡号
        //          blend       --  初始化arc_blend_ini_struct圆弧过渡参数
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_arc_blend_parameter_initial_unit(Int32 cardno, ref arc_blend_ini_struct blend);

        //功  能：  计算相连曲线间的过渡圆弧
        //参  数：  blend       --  计算出来的arc_blend_inout_struct圆弧过渡参数
        //备  注：  1. 该接口仅仅负责计算, 不负责驱动
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_arc_blend_process_unit(IntPtr blend);
        
        /************************************************************************/
        /*---------------------------ADT-9188控制接口---------------------------*/
        /*    ADT-9188是ADT-6329E系列运动控制卡的高速IO扩展板, 使用ADT-6329E时以*/
        /*下接口才有意义                                                        */
        /************************************************************************/
        
        //功  能：  9188本地输出点操作  
        //参  数：  cardno      --  可用控制卡卡号
        //          index       --  输出点索引号(0-7)
        //          value       --  0:关闭   1:打开
        //备  注：  1. 默认状态为0(关闭)
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_write_outbit(Int32 cardno, Int32 index, int value);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_read_outbit(Int32 cardno, Int32 index, out int value);
        
        //功  能：  9188本地输入点状态获取  
        //参  数：  cardno      --  可用控制卡卡号
        //          index       --  输出点索引号(0-7)
        //          status      --  输入点状态, 0:低电平   1:高电平
        //          inmap       --  8bit电平状态, 0:低电平   1:高电平
        //备  注：  1. 输入点状态默认为 1(高电平)
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_read_inport(Int32 cardno, out Int32 inmap);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_read_inbit(Int32 cardno, Int32 index, out int status);

        //功  能：  设置输入信号滤波时间
        //参  数：  cardno      --  可用控制卡卡号
        //          msec        --  滤波时间, [0,15], 0-15ms  默认1ms  
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_input_filter(Int32 cardno, Int32 msec);
        
        //功  能：  获取位置比较器余量
        //参  数：  cardno      --  可用控制卡卡号
        //          cmp         --  比较器索引, [0,3]  
        //          len         --  比较器余量
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_compare_len(Int32 cardno, Int32 cmp, out int len);

        //功  能：  获取比较触发状态
        //参  数：  cardno      --  可用控制卡卡号
        //          cmp         --  比较器索引, [0,3]  
        //          len         --  比较器触发个数
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_compare_status(Int32 cardno, Int32 cmp, out int count);
        
        //功  能：  清除位置比较器的所有比较点,复位比较器
        //参  数：  cardno      --  可用控制卡卡号
        //          cmp         --  比较器索引, [0,3]  
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_clear_compare_point(Int32 cardno, Int32 cmp);
        
        //功  能：  获取比较触发状态
        //参  数：  cardno      --  可用控制卡卡号
        //          cmp         --  比较器索引, [0,3]  
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_reset_compare(Int32 cardno, Int32 cmp);
        
        //功  能：  设置位置比较器模式  
        //参  数：  cardno      --  可用控制卡卡号
        //          cmp         --  比较器索引, [0,3]  
        //          enable      --  比较器启禁标识, [0,1]  
        //          xd          --  比较器维度, [1,3]  
        //          axis        --  参与位置比较的轴号列表  
        //          source      --  比较源头, [0,1], 0:逻辑位置 1:实际位置(编码器位置)
        //          mode        --  比较器触发IO输出模式, [1,3], 1:翻转, 2:买款输出, 3:PWM  
        //          level       --  输出点常态, [0,1], 0:关闭 1:打开
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_compare_xd_mode(Int32 cardno, Int32 cmp, Int32 enable, Int32 xd, Int32[] axis, Int32 source, Int32 mode, Int32 level);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_compare_xd_mode(Int32 cardno, Int32 cmp, out Int32 enable, out Int32 xd, Int32[] axis, out Int32 source, out Int32 mode, out Int32 level);

        //功  能：  设定2/3D比较区域范围  
        //参  数：  cardno      --  可用控制卡卡号
        //          cmp         --  比较器索引, [0,3]  
        //          area        --  比较器区域(pulse), [0,1]  
        //                      --  xd = 2时, 0:dx; 1:dy; [x-dx, y+dy]  [x+dx, y+dy] [x-dx, y-dy]  [x+dx, y-dy]
        //                      --  xd = 3时, 0:dx; 1:dy; 2:dz; 
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_compare_xd_area(Int32 cardno, Int32 cmp, Int32[] area);

        //功  能：  添加位置比较位置点  
        //参  数：  cardno      --  可用控制卡卡号
        //          cmp         --  比较器索引, [0,3]  
        //          pos         --  比较位置点,下标0,1,2和模式设置的轴号映射, 位置模式为绝对位置 单位：pulse, 
        //          time_h      --  PWM脉冲输出,OUT打开时间,单位(ms); 脉宽输出模式,OUT打开时间  
        //          time_l      --  PWM脉冲输出,OUT关闭时间,单位(ms); 只有在mode=2;PWM输出模式下,此参数有效  
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_add_compare_xd_point(Int32 cardno, Int32 cmp, Int32 enable, Int32[] pos, Double time_h, Double time_l);
        
        //功  能：  设置编码器工作方式  
        //参  数：  cardno      --  可用控制卡卡号
        //          encd        --  编码器反馈路号, [1, 8]  
        //          type        --  [0,1], 0: A/B脉冲输入	1: 上/下（PPIN/PMIN）脉冲输入 
        //          dir         --  [0,1], 0: 正逻辑方向     1: 负逻辑方向
        //备  注：  1. 默认A/B相脉冲输入,正逻辑方向
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_endoder_mode(Int32 cardno, Int32 cmp, Int32 type, Int32 dir);
        
        //功  能：  设置编码器滤波等级  
        //参  数：  cardno      --  可用控制卡卡号
        //          grade       --  滤波等级, [1, 8] 
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_encoder_filter(Int32 cardno, Int32 grade);
        
        //功  能：  读写编码器位置  
        //参  数：  cardno      --  可用控制卡卡号
        //          encd        --  编码器反馈路号, [1, 8]  
        //          pos         --  位置
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_get_encoder_pos(Int32 cardno, Int32 grade, out Int32 pos);
        [DllImport("adtemc.dll")]
        public static extern Int32 adt_set_encoder_pos(Int32 cardno, Int32 grade, Int32 pos);
    }
}
