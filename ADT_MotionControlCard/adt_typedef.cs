using System;
using System.Collections.Generic;
using System.Text;

namespace ADT_CARD_632XE
{
    public enum ERR_CODE
    {
        ERR_NONE = 0,                           // 成功
        ERR_PARAMETER = 1,                      // 参数错误
        ERR_SEM_CREATE = 2,                     // 信号量创建错误
        ERR_ARC_AXIS_MAP = 3,                   // 圆弧轴选位错误
        ERR_ARC_NO_EXIST = 4,                   // 圆弧不存在
        ERR_RESPONSE_TIMEOUT = 5,               // 运动库响应超时
        ERR_ARC_PARALLEL = 6,                   // 圆弧上的点平行
        ERR_PCI_RETURN_DATA = 7,                // PCI返回数据错误
        ERR_DATA_SIZE = 8,                      // 通信数据量异常
        ERR_INI_WINIO = 9,                      // WinIo初始化失败
        ERR_PCI_BRIDGE = 10,                    // PCI桥存在故障
        ERR_CREATE_MUTEX = 11,                  // 创建互斥量失败
        ERR_OPEN_MUTEX = 12,                    // 打开互斥量失败
        ERR_SWITCH_REPEAT = 13,                 // 使用多张卡时，卡号重复
        ERR_NONE_CARD = 14,                     // 没有识别到控制卡，卡安装有误或者驱动安装失败
        ERR_PCI_CRC = 15,                       // 通信CRC校验失败
        ERR_REBOOT = 20,                        //PCI驱动重启失败，需要关机重启
        ERR_FAIL = 21,                          //程序更新失败
        ERR_OVERTIME = 22,                      //超时
        ERR_DSP_CRC_FAIL = 23,                  //DSP程序校验不通过
        ERR_DSP_VERSION_FAIL = 24,              //固件程序版本和dll不匹配,或者固件异常,无法识别,需要送厂升级,或使用老版本dll
        ERR_SLV_NOT_EXIST = 25,			        //从站配置文件不存在
        ERR_SLV_INVALID = 26,			        //从站配置文件格式有误
        
        ERR_ARCBLEND_INPUT_SEG = 30,			// 非相连曲线段(圆弧过渡处理)
        ERR_ARCBLEND_INPUT_PARA = 31,			// 过渡精度或者参数阈值要求过高(圆弧过渡处理)
        ERR_ARCBLEND_INCOPLANARITY = 32,		// 存在一条圆弧段时输入曲线不共面(圆弧过渡处理)
        ERR_ARCBLEND_INPUT_ANGLE = 33,			// 输入曲线夹角超出设定阈值范围(圆弧过渡处理)
        ERR_ARCBLEND_TOO_SHORT = 34,			// 输入曲线长度太小(圆弧过渡处理)
        ERR_ARCBLEND_OUTPUT_SEG	= 35,			// 过渡后, 原线段的长度小于min_len(圆弧过渡处理)
        ERR_ARCBLEND_BLENDARC_R	= 36,			// 过渡后, 圆半径过大或者圆半径小于min_R(圆弧过渡处理)
        ERR_ARCBLEND_BLENDARC_len = 37,			// 过渡后, 过渡圆弧长小于min_len(圆弧过渡处理)
        ERR_ARCBLEND_EXECUTE_FAIL = 38,			// 求解过渡圆过程中无解(圆弧过渡处理)
        ERR_ARCBLEND_INCOP_ARCARC = 39,			// 存在两条圆弧段时输入曲线不共面(圆弧过渡处理)
            
        SERVO_HOMEING = 40,                     //伺服回零中
        ERR_SERVO_HOME = 41,                    //伺服端回零超时，或偏差过大
        ERR_NURBS_MAXDATNUM = 50,               //输入数据个数错误，超过1000或少于2。
        ERR_NURBS_MOD = 51,                     //<输入模式mod错误。
        ERR_NURBS_MAX_POS_VALUE = 52,           //<输入坐标值超出范围。或者指明的数据大小和实际定义的数组大小不一致
        ERR_NURBS_ZERO_TanVec_VALUE = 53,       //<输入切矢的模接近0
        ERROR_NURBS_LowLengthLimit = 54,        //步长小于0.1
        ERR_NURBS_KNOTSLOWLIMITS = 55,          //<节点小于0

		ERR_ECAT_RT_RETURN = 60,
		ERR_ECAT_CFG_PATH =	61,
		ERR_ECAT_CFG_FILE =	62,
		ERR_ECAT_LOAD_CFG =	63,
		ERR_ECAT_DOWNLOAD_CFG =	64,
		ERR_ECAT_START = 65,
		ERR_ECAT_STOP =	66,

        _ERR_NONE = 0,                          // 成功
        _ERR_PARAMETER = 101,                   // 参数错误
        _ERR_DATA_SIZE = 102,                   // 通信数据量异常
        _ERR_AXIS_STOP = 103,                   // 对应轴处于限位停止状态
        _ERR_MOTION_CONFLICT = 104,             // 运动冲突
        _ERR_PARA_SET_FORBIT = 105,             // 当前状态不允许修改参数
        _ERR_DATA_MODE = 106,			        // 输入数据模式错误
        _ERR_ADMODE = 107,			            // 加减速模式设置错误
        _ERR_S_SPEED_CALC = 108,			    // S型速度规划时计算错误
        _ERR_EMERGENCY = 109,			        // 外部紧急停止信号生效中
        _ERR_MOTION_DATA = 110,			        // 底层运动目标位置数据异常
        _ERR_CMD_NULL = 111,			        // 空指令或无效指令
        _ERR_FPGA_WR = 112,			            // FPGA读写错误
        _ERR_INP_FIFO_FULL = 113,			    // 插补运动指令缓冲区已满
        _ERR_PTP_FIFO_FULL = 114,			    // 点位运动指令缓冲区已满
        _ERR_POS_INCONSISTENT = 115,			// 恢复倍率时存在位置偏差
        _ERR_FIRMWARE_NOT_SUPPORT = 116,		// 当前固件不支持
        _ERR_AXIS_TYPE = 117,			        // 轴类型错误
        _ERR_EtherCAT_CONNECT = 118,			// 总线通信故障
        _ERR_AXIS_ALARM = 119,			        // 轴报警
        _ERR_MAILBOX_WR = 120,			        // 邮箱命令通信错误
        _ERR_CRC_CHECK = 121,                   // 数据发送CRC校验错误

        AXIS_1 = 1,			                    // 第1轴轴号
        AXIS_2 = 2,			                    // 第2轴轴号
        AXIS_3 = 3,			                    // 第3轴轴号
        AXIS_4 = 4,			                    // 第4轴轴号
        AXIS_5 = 5,			                    // 第5轴轴号
        AXIS_6 = 6,			                    // 第6轴轴号
        AXIS_7 = 7,			                    // 第7轴轴号
        AXIS_8 = 8,			                    // 第8轴轴号

        MAX_INP_AXIS = 8,
        INPA_AXIS = 63,			                // A组插补轴轴号(常用)
        INPB_AXIS = 62,			                // B组插补轴轴号
        INPC_AXIS = 61,			                // C组插补轴轴号
        INPD_AXIS = 60,			                // D组插补轴轴号

        /******************************坐标定义******************************/
        REL_POS = 0,		// 相对坐标
        ABS_POS = 1,	    // 绝对坐标

        /******************************回零错误******************************/
        ERR_HOME_PARA1 = -1,        //参数1错误
        ERR_HOME_PARA2 = -2,        //参数2错误
        ERR_HOME_PARA3 = -3,        //参数3错误
        ERR_HOME_PARA4 = -4,        //参数4错误
        ERR_HOME_PARA5 = -5,        //参数5错误
        ERR_HOME_PARA6 = -6,        //参数6错误
        ERR_HOME_PARA7 = -7,        //参数7错误
        ERR_HOME_PARA8 = -8,        //参数8错误
        ERR_HOME_PARA9 = -9,        //参数9错误

        ERR_HOME_STEP1 = -1001,     //步骤1错误
        ERR_HOME_STEP2 = -1002,     //步骤2错误
        ERR_HOME_STEP3 = -1003,     //步骤3错误
        ERR_HOME_STEP4 = -1004,     //步骤4错误
        ERR_HOME_STEP5 = -1005,     //步骤5错误
        ERR_HOME_STEP6 = -1006,     //步骤6错误
        ERR_HOME_STEP7 = -1007,     //步骤7错误
        ERR_HOME_STEP8 = -1008,     //步骤8错误
        ERR_HOME_STEP9 = -1009,     //步骤9错误
        ERR_HOME_STEP10 = -1010,    //步骤10错误
        ERR_HOME_STEP11 = -1011,    //步骤11错误
        ERR_HOME_STEP12 = -1012,    //步骤12错误
        ERR_HOME_STEP13 = -1013,    //步骤13错误
        ERR_HOME_STEP14 = -1014,    //步骤14错误
        ERR_HOME_STEP15 = -1015,    //步骤15错误
        ERR_HOME_STEP16 = -1016,    //步骤16错误
        ERR_HOME_STEP17 = -1017,    //步骤17错误
        ERR_HOME_STEP18 = -1018,    //步骤18错误
        ERR_HOME_STEP19 = -1019,    //步骤19错误
        ERR_HOME_TERMINATE = -1020, //外部终止回零
        ERR_HOME_INNER = -1030,     //其他回零错误
    }
}
