using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO; // 讀取寫入文字檔 
using System.Diagnostics;//debug msg在『輸出』視窗觀看

namespace c_sche_with_py
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string py_addr = @"E:\DaysBetweenDates.py"; //給予要執行的檔案的路徑 
            Option1_ExecProcess(py_addr);
        }
        #region c# call py
        static void Option1_ExecProcess(string script)
        {
            // 1) Create Process Info
            var psi = new ProcessStartInfo(); //創造一個處理程序 
            psi.FileName = @"C:\Users\johnny\AppData\Local\Programs\Python\Python36\python.exe";

            // 2) 提供路境以及變數 
            script = @"C:\Users\johnny\source\repos\c_sche_with_py\c_sche_with_py\python\micro_grid_milp_with_c_sharp.py";//(因為我沒放debug下，所以直接寫的絕對路徑,替換掉上面的路徑了)

            var start = "2019-1-1";
            var end = "2019-1-22";

            //psi.Arguments = $"\"{script}\" \"{start}\" \"{end}\"";  //輸入的變數總共包含了 程式碼的路徑 程式碼需要的變數兩個 
            psi.Arguments = $"\"{script}\"";
            // 3) Process configuration
            psi.UseShellExecute = false;  //不需要使用shell
            psi.CreateNoWindow = true; //不需要顯示互動視窗 
            psi.RedirectStandardOutput = true; //重新導向輸出 這樣子才可以拿到輸出 
            psi.RedirectStandardError = true;  //錯誤同理 需要重新導向 

            // 4) Execute process and get output
            var errors = "";
            var results = "";

            using (var process = Process.Start(psi)) //啟動處理程序但是不知道在幹嘛 
                                                     //using 陳述式 執行完後會釋放資源 
            {
                errors = process.StandardError.ReadToEnd(); //
                results = process.StandardOutput.ReadToEnd();  //接收輸出 
            }

            // 5) Display output



            Debug.Print("ERRORS:");
            Debug.Print(errors);
            Debug.Print("");
            Debug.Print("Results:");
            Debug.Print(results);
            //Debug.Print(results);

        }
        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            //讀取每一行 放到string array 
            string[] DG = System.IO.File.ReadAllLines(@"C:\Users\johnny\source\repos\c_sche_with_py\c_sche_with_py\python\0DG.txt");
            string[] ESS = System.IO.File.ReadAllLines(@"C:\Users\johnny\source\repos\c_sche_with_py\c_sche_with_py\python\0ESS.txt");
            string[] PV = System.IO.File.ReadAllLines(@"C:\Users\johnny\source\repos\c_sche_with_py\c_sche_with_py\python\0PV.txt");
            string[] SOC = System.IO.File.ReadAllLines(@"C:\Users\johnny\source\repos\c_sche_with_py\c_sche_with_py\python\0SOC.txt");
            
            float[] dg_int = new float[DG.Count()];
            float[] ESS_int = new float[ESS.Count()];
            float[] PV_int = new float[PV.Count()];
            float[] SOC_int = new float[SOC.Count()];
            
            for (int i = 0; i < 24; i++)
            {
                dg_int[i] = float.Parse(DG[i]);
                ESS_int[i] = float.Parse(ESS[i]);
                PV_int[i] = float.Parse(PV[i]);
                SOC_int[i] = float.Parse(SOC[i]);
            }

            Debug.Print("字串轉換成矩陣" + dg_int.ToString() + dg_int[0].ToString());
        }
        #region 排程輸出測試 1
        DateTime time1 = DateTime.Now;
        //做 start end物件 
        public static List<Start_End> sche_obj = new List<Start_End>();

        #endregion
        private void button3_Click(object sender, EventArgs e)
        {

            #region 排程輸出測試 2
            timer_sche.Enabled = true;
            //讀取文字檔案 
            //讀取每一行 放到float array  
            
            string[] DG = System.IO.File.ReadAllLines(@"C:\Users\johnny\source\repos\c_sche_with_py\c_sche_with_py\python\0DG.txt");
            string[] ESS = System.IO.File.ReadAllLines(@"C:\Users\johnny\source\repos\c_sche_with_py\c_sche_with_py\python\0ESS.txt");
            string[] PV = System.IO.File.ReadAllLines(@"C:\Users\johnny\source\repos\c_sche_with_py\c_sche_with_py\python\0PV.txt");
            string[] SOC = System.IO.File.ReadAllLines(@"C:\Users\johnny\source\repos\c_sche_with_py\c_sche_with_py\python\0SOC.txt");

            float[] dg_int = new float[DG.Count()];
            float[] ESS_int = new float[ESS.Count()];
            float[] PV_int = new float[PV.Count()];
            float[] SOC_int = new float[SOC.Count()];

            //把讀到的每一個功率排程 存放到list，每一個相差10秒 
            DateTime starttime = time1;
            DateTime endtime = starttime.AddSeconds(3);

            for (int i = 0; i < 24; i++)
            {
                dg_int[i] = float.Parse(DG[i]);
                ESS_int[i] = float.Parse(ESS[i]);
                PV_int[i] = float.Parse(PV[i]);
                SOC_int[i] = float.Parse(SOC[i]);
                sche_obj.Add(new Start_End(starttime, endtime, 0, dg_int[i], ESS_int[i], PV_int[i], SOC_int[i], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
                starttime = starttime.AddSeconds(3);
                endtime = endtime.AddSeconds(3);
            }

            


            #endregion
        }
        #region  排程的物件 可以比較 排程輸出測試 4
        /////定義class 

        /* 增加排程的方法 
         * Start_List.Add(new Start_End(starttime, endtime, mode, fixed_p, fixed_q, steady_value, smooth_limit, back_p, soc_max, soc_min, ramp_up, ramp_down, p_base, q_base, fp_base, combine_mode, c_parameters));
                    _Hour.SelectedIndex = endtime.Hour;
                    _Minute.SelectedIndex = endtime.Minute;
            //自動排序
                Start_List.Sort((x, y) => { return x.Start_time.CompareTo(y.Start_time); });
            /////比較新排程是否有重疊 如果沒有重疊就加入排程 
                IEnumerable<Start_End> filteringQuery =
                from c in Start_List
                where (c.End_Time > starttime && starttime >= c.Start_time) || (c.Start_time < endtime && c.End_Time >= endtime)
                select c;
                ////若無重疊排程，加入新排程，排程時間自動延續
                int repeat_time = filteringQuery.Count();
                if (repeat_time == 0)
                {
                    Start_List.Add(new Start_End(starttime, endtime, mode, fixed_p, fixed_q, steady_value, smooth_limit, back_p, soc_max, soc_min, ramp_up, ramp_down, p_base, q_base, fp_base, combine_mode, c_parameters));
                    _Hour.SelectedIndex = endtime.Hour;
                    _Minute.SelectedIndex = endtime.Minute;
                }
                else
                {
                    MessageBox.Show("Error");
                }
         */
        public class Start_End : IComparable<Start_End>
        {

            private DateTime start_time;
            private DateTime end_time;
            private int mode;
            private double smooth_limit;
            private double steady_value;
            private double fixed_p;
            private double back_p;
            private double soc_max;
            private double soc_min;
            private double ramp_up;
            private double ramp_down;
            private double p_base;
            private double fp_base;
            private double q_base;
            private double combine_mode;
            private double c_parameters;
            private double fixed_q;

            #region 變數 get set 
            public DateTime Start_time
            {
                get { return start_time; }
                set { start_time = value; }
            }
            public DateTime End_Time
            {
                get { return end_time; }
                set { end_time = value; }
            }
            public int Mode
            {
                get { return mode; }
                set { mode = value; }
            }
            public double Fixed_P
            {
                get { return fixed_p; }
                set { fixed_p = value; }
            }
            public double Fixed_Q
            {
                get { return fixed_q; }
                set { fixed_q = value; }
            }
            public double P_Base
            {
                get { return p_base; }
                set { p_base = value; }
            }
            public double FP_Base
            {
                get { return fp_base; }
                set { fp_base = value; }
            }
            public double Q_Base
            {
                get { return q_base; }
                set { q_base = value; }
            }
            public double Steady_Value
            {
                get { return steady_value; }
                set { steady_value = value; }
            }
            public double Smooth_Limit
            {
                get { return smooth_limit; }
                set { smooth_limit = value; }
            }
            public double Back_P
            {
                get { return back_p; }
                set { back_p = value; }
            }
            public double SOC_max
            {
                get { return soc_max; }
                set { soc_min = value; }
            }
            public double SOC_min
            {
                get { return soc_min; }
                set { soc_min = value; }
            }
            public double Ramp_Up
            {
                get { return ramp_up; }
                set { ramp_up = value; }
            }
            public double Ramp_Down
            {
                get { return ramp_down; }
                set { ramp_down = value; }
            }
            public double Combine_mode
            {
                get { return combine_mode; }
                set { combine_mode = value; }
            }

            public double C_parameters
            {
                get { return c_parameters; }
                set { c_parameters = value; }
            }
            #endregion
            //建購子
            public Start_End(DateTime start_time, DateTime end_time, int mode, double fixed_p, double fixed_q, double steady_value, double smooth_limit, double back_p, double soc_max, double soc_min, double ramp_up, double ramp_down, double p_base, double q_base, double fp_base, double combine_mode, double c_parameters)
            {
                this.start_time = start_time;
                this.end_time = end_time;
                this.mode = mode;
                this.fixed_p = fixed_p;
                this.fixed_q = fixed_q;
                this.steady_value = steady_value;
                this.smooth_limit = smooth_limit;
                this.back_p = back_p;
                this.ramp_up = ramp_up;
                this.ramp_down = ramp_down;
                this.soc_max = soc_max;
                this.soc_min = soc_min;
                this.p_base = p_base;
                this.q_base = q_base;
                this.c_parameters = c_parameters;
                this.combine_mode = combine_mode;
                this.fp_base = fp_base;
            }
            //這個物件的功能  可以查看開始結束時間跟功能 
            public override string ToString()
            {
                return start_time.ToString("HH:mm") + "~" + end_time.ToString("HH:mm");
            }
            public string ToMode()
            {
                return Mode.ToString();
            }

            int IComparable<Start_End>.CompareTo(Start_End other)
            {
                throw new NotImplementedException();
            }
        }

        public class SchComparerby1 : IComparer<Start_End>
        {
            //實作Compare方法
            //依Speed由小排到大。
            public int Compare(Start_End x, Start_End y)
            {
                if (x.Start_time < y.Start_time)
                    return -1;
                if (x.Start_time > y.Start_time)
                    return 1;
                //該段為Speed相等時才會由Power比較
                //依power由小排到大
                return 0;//兩個開始的時間一樣，平等 
            }


        }
        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void timer_sche_Tick(object sender, EventArgs e)
        {
            #region 排程輸出測試 3
            foreach (var item1 in sche_obj)
            {
                //if 時間對 就輸出 
                if (item1.End_Time > DateTime.Now && DateTime.Now >= item1.Start_time)
                {

                    Debug.Print(DateTime.Now.ToString()+" "+ item1.Fixed_P.ToString());
                    
                }


            }
            #endregion
        }
    }
}
