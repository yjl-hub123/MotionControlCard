using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ADT_CARD_632XE;

namespace DEMO632XE
{
    class adt_global
    {
        public static string adt_decode_error_code(Int32 retn)
        {
            string err_text = "";
            switch ((ERR_CODE)retn)
            {
                case ERR_CODE.ERR_NONE:
                case ERR_CODE.ERR_PARAMETER:
                    err_text = String.Format("参数错误! 错误码：{0:D}", retn);
                    break;
                case ERR_CODE.ERR_SEM_CREATE:
                    err_text = String.Format("信号量创建错误! 错误码：{0:D}", retn);
                    break;
                case ERR_CODE.ERR_ARC_AXIS_MAP:
                    err_text = String.Format("圆弧轴选位错误! 错误码：{0:D}", retn);
                    break;
                case ERR_CODE.ERR_ARC_NO_EXIST:
                    err_text = String.Format("圆弧不存在! 错误码：{0:D}", retn);
                    break;
                case ERR_CODE.ERR_RESPONSE_TIMEOUT:
                    err_text = String.Format("运动库响应超时! 错误码：{0:D}", retn);
                    break;
                case ERR_CODE.ERR_ARC_PARALLEL:
                    err_text = String.Format("圆弧上点平行! 错误码：{0:D}", retn);
                    break;
                case ERR_CODE.ERR_PCI_RETURN_DATA:
                    err_text = String.Format("PCI返回数据错误! 错误码：{0:D}", retn);
                    break;
                case ERR_CODE.ERR_DATA_SIZE:
                    err_text = String.Format("通讯数据量异常! 错误码：{0:D}", retn);
                    break;
                case ERR_CODE.ERR_INI_WINIO:
                    err_text = String.Format("WinIO初始化失败! 错误码：{0:D}", retn);
                    break;
                case ERR_CODE.ERR_PCI_BRIDGE:
                    err_text = String.Format("PCI桥存在故障! 错误码：{0:D}", retn);
                    break;
                case ERR_CODE.ERR_CREATE_MUTEX:
                    err_text = String.Format("创建互斥量失败! 错误码：{0:D}", retn);
                    break;
                case ERR_CODE.ERR_OPEN_MUTEX:
                    err_text = String.Format("打开互斥量失败! 错误码：{0:D}", retn);
                    break;
                case ERR_CODE.ERR_SWITCH_REPEAT:
                    err_text = String.Format("拨码开关重复! 错误码：{0:D}", retn);
                    break;
                case ERR_CODE.ERR_NONE_CARD:
                    err_text = String.Format("未识别到运动控制卡! 错误码：{0:D}", retn);
                    break;
                case ERR_CODE.ERR_PCI_CRC:
                    err_text = String.Format("通信CRC校验失败! 错误码：{0:D}", retn);
                    break;

                case ERR_CODE.ERR_REBOOT:
                    err_text = String.Format("PCI驱动重启失败, 需关机重启! 错误码：{0:D}", retn);
                    break;
                case ERR_CODE.ERR_FAIL:
                    err_text = String.Format("程序更新失败! 错误码：{0:D}", retn);
                    break;
                case ERR_CODE.ERR_OVERTIME:
                    err_text = String.Format("程序更新超时! 错误码：{0:D}", retn);
                    break;
                case ERR_CODE.ERR_DSP_CRC_FAIL:
                    err_text = String.Format("DSP程序校验不通过! 错误码：{0:D}", retn);
                    break;
                case ERR_CODE.ERR_DSP_VERSION_FAIL:
                    err_text = String.Format("固件程序版本和dll不匹配! 错误码：{0:D}", retn);
                    break;

                case ERR_CODE.SERVO_HOMEING:
                    err_text = String.Format("伺服回零中! 错误码：{0:D}", retn);
                    break;
                case ERR_CODE.ERR_SERVO_HOME:
                    err_text = String.Format("伺服端回零超时或偏差过大! 错误码：{0:D}", retn);
                    break;

                case ERR_CODE.ERR_NURBS_MAXDATNUM:
                    err_text = String.Format("NURBS输入数据数量有误, 超过1000或低于2! 错误码：{0:D}", retn);
                    break;
                case ERR_CODE.ERR_NURBS_MOD:
                    err_text = String.Format("NURBS输入模式错误! 错误码：{0:D}", retn);
                    break;
                case ERR_CODE.ERR_NURBS_MAX_POS_VALUE:
                    err_text = String.Format("NURBS输入坐标值超出范围, 或指明的数据大小和实际意义的数组大小不一致! 错误码：{0:D}", retn);
                    break;
                case ERR_CODE.ERR_NURBS_ZERO_TanVec_VALUE:
                    err_text = String.Format("NURBS输入切矢的模为0! 错误码：{0:D}", retn);
                    break;
                case ERR_CODE.ERROR_NURBS_LowLengthLimit:
                    err_text = String.Format("NURBS步长小于0.1! 错误码：{0:D}", retn);
                    break;
                case ERR_CODE.ERR_NURBS_KNOTSLOWLIMITS:
                    err_text = String.Format("NURBS节点小于0! 错误码：{0:D}", retn);
                    break;

                case ERR_CODE.ERR_ECAT_RT_RETURN:
                    err_text = String.Format(" 错误码：{0:D}", retn);
                    break;
                case ERR_CODE.ERR_ECAT_CFG_PATH:
                    err_text = String.Format(" 错误码：{0:D}", retn);
                    break;
                case ERR_CODE.ERR_ECAT_CFG_FILE:
                    err_text = String.Format(" 错误码：{0:D}", retn);
                    break;
                case ERR_CODE.ERR_ECAT_LOAD_CFG:
                    err_text = String.Format(" 错误码：{0:D}", retn);
                    break;
                case ERR_CODE.ERR_ECAT_DOWNLOAD_CFG:
                    err_text = String.Format(" 错误码：{0:D}", retn);
                    break;
                case ERR_CODE.ERR_ECAT_START:
                    err_text = String.Format("通讯启动失败, 错误码：{0:D}", retn);
                    break;
                case ERR_CODE.ERR_ECAT_STOP:
                    err_text = String.Format(" 错误码：{0:D}", retn);
                    break;

                case ERR_CODE._ERR_PARAMETER:
                    err_text = String.Format("参数错误! 错误码：{0:D}", retn);
                    break;
                case ERR_CODE._ERR_DATA_SIZE:
                    err_text = String.Format("Motion库通信数据量异常! 错误码：{0:D}", retn);
                    break;
                case ERR_CODE._ERR_AXIS_STOP:
                    err_text = String.Format("对应轴处于停止状态! 错误码：{0:D}", retn);
                    break;
                case ERR_CODE._ERR_MOTION_CONFLICT:
                    err_text = String.Format("运动冲突! 错误码：{0:D}", retn);
                    break;
                case ERR_CODE._ERR_PARA_SET_FORBIT:
                    err_text = String.Format("当前状态不允许修改参数! 错误码：{0:D}", retn);
                    break;
                case ERR_CODE._ERR_DATA_MODE:
                    err_text = String.Format("输入数据模式错误! 错误码：{0:D}", retn);
                    break;
                case ERR_CODE._ERR_ADMODE:
                    err_text = String.Format("加减速模式设置错误! 错误码：{0:D}", retn);
                    break;
                case ERR_CODE._ERR_S_SPEED_CALC:
                    err_text = String.Format("S型速度规划时计算错误! 错误码：{0:D}", retn);
                    break;
                case ERR_CODE._ERR_EMERGENCY:
                    err_text = String.Format("紧急停止信号生效中，请先复位运动控制卡! 错误码：{0:D}", retn);
                    break;
                case ERR_CODE._ERR_MOTION_DATA:
                    err_text = String.Format("底层运动目标位置数据异常! 错误码：{0:D}", retn);
                    break;
                case ERR_CODE._ERR_CMD_NULL:
                    err_text = String.Format("空指令或无效指令! 错误码：{0:D}", retn);
                    break;
                case ERR_CODE._ERR_FPGA_WR:
                    err_text = String.Format("FPGA读写错误! 错误码：{0:D}", retn);
                    break;
                case ERR_CODE._ERR_INP_FIFO_FULL:
                    err_text = String.Format("插补运动指令缓冲区已满! 错误码：{0:D}", retn);
                    break;
                case ERR_CODE._ERR_PTP_FIFO_FULL:
                    err_text = String.Format("点位运动指令缓冲区已满! 错误码：{0:D}", retn);
                    break;
                case ERR_CODE._ERR_POS_INCONSISTENT:
                    err_text = String.Format("恢复倍率时存在位置偏差! 错误码：{0:D}", retn);
                    break;
                case ERR_CODE._ERR_FIRMWARE_NOT_SUPPORT:
                    err_text = String.Format("当前固件不支持! 错误码：{0:D}", retn);
                    break;
                case ERR_CODE._ERR_AXIS_TYPE:
                    err_text = String.Format("轴类型错误! 错误码：{0:D}", retn);
                    break;
                case ERR_CODE._ERR_EtherCAT_CONNECT:
                    err_text = String.Format("总线通信故障! 错误码：{0:D}", retn);
                    break;
                case ERR_CODE._ERR_AXIS_ALARM:
                    err_text = String.Format("轴报警! 错误码：{0:D}", retn);
                    break;
                case ERR_CODE._ERR_MAILBOX_WR:
                    err_text = String.Format("邮箱命令通信错误! 错误码：{0:D}", retn);
                    break;
                case ERR_CODE._ERR_CRC_CHECK:
                    err_text = String.Format("数据发送CRC校验错误 错误码：{0:D}", retn);
                    break;
                default:
                    err_text = String.Format("其它错误 错误码：{0:D}", retn);
                    break;
            }
               
            return err_text;
        }
    }
}
