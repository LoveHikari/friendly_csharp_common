#if !NETSTANDARD2_0
using System.Collections.Generic;
using System.Windows.Forms;

/******************************************************************************************************************
 * 
 * 
 * 标  题： string 帮助类(版本：Version1.0.0)
 * 作  者： YuXiaoWei
 * 日  期： 2017/06/06
 * 修  改：
 * 参  考： http://blog.sina.com.cn/s/blog_45eaa01a0101c7ko.html
 * 说  明： 暂无...
 * 备  注： 暂无...
 * 调用示列：1.在需要自适应的Form中实例化全局变量
 *          AutoSizeForm asc = new AutoSizeForm();
 *          2.Form_Load事件中  
 *          asc.controllInitializeSize(this);
 *          3.Page_SizeChanged事件中
 *          asc.controlAutoSize(this);
 *
 * 
 * ***************************************************************************************************************/
namespace System.Forms
{
    /// <summary>
    /// WinForm窗体及其控件的自适应
    /// </summary>
    public class AutoSizeForm
    {
        //(1).声明结构,只记录窗体和其控件的初始位置和大小。
        private struct ControlRect
        {
            public int Left;
            public int Top;
            public int Width;
            public int Height;
        }
        //(2).声明 1个对象
        //注意这里不能使用控件列表记录 List nCtrl;，因为控件的关联性，记录的始终是当前的大小。
        private readonly List<ControlRect> _oldCtrl;

        private int _ctrlNo;
        /// <summary>
        /// 构造函数
        /// </summary>
        public AutoSizeForm()
        {
            _oldCtrl = new List<ControlRect>();
            _ctrlNo = 0;//1;
        }

        /// <summary>
        /// 记录窗体和其控件的初始位置和大小
        /// </summary>
        /// <param name="mForm">窗口对象</param>
        public void ControllInitializeSize(Form mForm)
        {
            ControlRect cR;
            cR.Left = mForm.Left; cR.Top = mForm.Top; cR.Width = mForm.Width; cR.Height = mForm.Height;
            _oldCtrl.Add(cR);//第一个为"窗体本身",只加入一次即可
            AddControl(mForm);//窗体内其余控件还可能嵌套控件(比如panel),要单独抽出,因为要递归调用
            //this.WindowState = (System.Windows.Forms.FormWindowState)(2);//记录完控件的初始位置和大小后，再最大化
            //0 - Normalize , 1 - Minimize,2- Maximize
        }
        private void AddControl(Control ctl)
        {
            foreach (Control c in ctl.Controls)
            {  //**放在这里，是先记录控件的子控件，后记录控件本身
                //if (c.Controls.Count > 0)
                //    AddControl(c);//窗体内其余控件还可能嵌套控件(比如panel),要单独抽出,因为要递归调用
                ControlRect objCtrl;
                objCtrl.Left = c.Left; objCtrl.Top = c.Top; objCtrl.Width = c.Width; objCtrl.Height = c.Height;
                _oldCtrl.Add(objCtrl);
                //**放在这里，是先记录控件本身，后记录控件的子控件
                if (c.Controls.Count > 0)
                    AddControl(c);//窗体内其余控件还可能嵌套控件(比如panel),要单独抽出,因为要递归调用
            }
        }
        /// <summary>
        /// 控件自适应大小
        /// </summary>
        /// <param name="mForm">窗口对象</param>
        public void ControlAutoSize(Form mForm)
        {
            if (_ctrlNo == 0)
            { //*如果在窗体的Form1_Load中，记录控件原始的大小和位置，正常没有问题，但要加入皮肤就会出现问题，因为有些控件如dataGridView的的子控件还没有完成，个数少
                //*要在窗体的Form1_SizeChanged中，第一次改变大小时，记录控件原始的大小和位置,这里所有控件的子控件都已经形成
                ControlRect cR;
                //  cR.Left = mForm.Left; cR.Top = mForm.Top; cR.Width = mForm.Width; cR.Height = mForm.Height;
                cR.Left = 0; cR.Top = 0; cR.Width = mForm.PreferredSize.Width; cR.Height = mForm.PreferredSize.Height;

                _oldCtrl.Add(cR);//第一个为"窗体本身",只加入一次即可
                AddControl(mForm);//窗体内其余控件可能嵌套其它控件(比如panel),故单独抽出以便递归调用
            }
            float wScale = (float)mForm.Width / (float)_oldCtrl[0].Width;//新旧窗体之间的比例，与最早的旧窗体
            float hScale = (float)mForm.Height / (float)_oldCtrl[0].Height;//.Height;
            _ctrlNo = 1;//进入=1，第0个为窗体本身,窗体内的控件,从序号1开始
            AutoScaleControl(mForm, wScale, hScale);//窗体内其余控件还可能嵌套控件(比如panel),要单独抽出,因为要递归调用
        }
        private void AutoScaleControl(Control ctl, float wScale, float hScale)
        {
            int ctrLeft0, ctrTop0, ctrWidth0, ctrHeight0;
            //int ctrlNo = 1;//第1个是窗体自身的 Left,Top,Width,Height，所以窗体控件从ctrlNo=1开始
            foreach (Control c in ctl.Controls)
            { //**放在这里，是先缩放控件的子控件，后缩放控件本身
                //if (c.Controls.Count > 0)
                //   AutoScaleControl(c, wScale, hScale);//窗体内其余控件还可能嵌套控件(比如panel),要单独抽出,因为要递归调用
                ctrLeft0 = _oldCtrl[_ctrlNo].Left;
                ctrTop0 = _oldCtrl[_ctrlNo].Top;
                ctrWidth0 = _oldCtrl[_ctrlNo].Width;
                ctrHeight0 = _oldCtrl[_ctrlNo].Height;
                //c.Left = (int)((ctrLeft0 - wLeft0) * wScale) + wLeft1;//新旧控件之间的线性比例
                //c.Top = (int)((ctrTop0 - wTop0) * h) + wTop1;
                c.Left = (int)((ctrLeft0) * wScale);//新旧控件之间的线性比例。控件位置只相对于窗体，所以不能加 + wLeft1
                c.Top = (int)((ctrTop0) * hScale);//
                c.Width = (int)(ctrWidth0 * wScale);//只与最初的大小相关，所以不能与现在的宽度相乘 (int)(c.Width * w);
                c.Height = (int)(ctrHeight0 * hScale);//
                _ctrlNo++;//累加序号
                //**放在这里，是先缩放控件本身，后缩放控件的子控件
                if (c.Controls.Count > 0)
                    AutoScaleControl(c, wScale, hScale);//窗体内其余控件还可能嵌套控件(比如panel),要单独抽出,因为要递归调用
            }
        }
    }
}
#endif
