using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADT_MotionControlCard
{
    public class AxisInformation
    {
        //轴号
        public int Axis { get; set; }
        //是否选中
        public bool IsSelected { get; set; }
        //逻辑位置
        public int LogicalPosition { get; set; }
        //编码器位置
        public int EncoderPosition { get; set; }
        //逻辑速度
        public double LogicalSpeed { get; set; }
        //编码器速度
        public double EncoderSpeed { get; set; }
        //驱动状态
        public int DriveStatus { get; set; }
        //目标位置
        public double TargetPosition { get; set; }
        //使能
        public string Enable { get; set; }
        //正限位
        public int PositiveLimit { get; set; }
        //负限位
        public int NegativeLimit { get; set; }
        //原点
        public int Origin { get; set; }
        //停止信息
        public int StopSignal { get; set; }
    }
}
