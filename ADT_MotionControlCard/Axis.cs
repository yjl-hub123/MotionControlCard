using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADT_MotionControlCard
{
    public class Axis
    {
        //轴号
        public int axiss { get; set; }
        //是否选中
        public bool IsSelected { get; set; }
        //起始速度
        public int InitialSpeed { get; set; }
        //最大速度
        public int MaxSpeed { get; set; }
        //加速度
        public int Acceleration { get; set; }
        //减速度
        public int Deceleration { get; set; }
        //加减速模式
        public string AccelerationDecelerationMode { get; set; }
        //目标位置
        public int TargetPosition { get; set; }
    }
}
