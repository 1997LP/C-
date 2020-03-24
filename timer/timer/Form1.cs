using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace timer
{
    public partial class Form1 : Form
    {
        int p_count = 0;    //定时器到一次，加1
        int p_timer = 0;    //记录定时时间
        int p_ret = 0;      
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Text = "时长";
            label2.Text = "剩余时长";
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;  //设置文本框不可编辑，只能下拉选择

            for (int i=0;i<101;i++)
            {
                comboBox1.Items.Add(i.ToString()+" 秒");  //添加下拉框选项
            }

            label3.Text = "0 秒";
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
          
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void 开始_Click(object sender, EventArgs e)
        {
            System.Media.SystemSounds.Beep.Play();  //系统提示音
            string timer = comboBox1.Text;
            if (timer == "")    //判断下拉框是否为空，即是否选择了定时时间
            {
                MessageBox.Show("请选择定时时间");
            }
            else
            {
                string data = timer.Substring(0, 2);  //读取字符串的前两个字符

                p_timer = p_ret = Convert.ToInt16(data);    //将前两个字符转换为int16的类型
                p_count = 0;

                progressBar1.Value = progressBar1.Minimum;  //设置进度条的初始值
                progressBar1.Maximum = p_timer;             //设置进度条的最大值
                progressBar1.Minimum = 0;                   //设置进度条的最小值

                label3.Text = comboBox1.Text;

                timer1.Start();         //打开定时器

            }
            

        }

        private void timer1_Tick(object sender, EventArgs e)//定时器处理函数
        {
            p_count++;
            p_ret--;
            if (p_count >= p_timer)  //如果定时时间到
            {
                progressBar1.Value = progressBar1.Maximum;  //设置进度条为最大值
                label3.Text = "0 秒";
                timer1.Stop();      //关闭定时器
                System.Media.SystemSounds.Asterisk.Play();//系统提示音
            
                MessageBox.Show("定时时间到"); //它会使程序阻塞，所以应该写在后面

            }
            if (p_count < p_timer)  //定时时间没有到
            {
                progressBar1.Value = p_count;  //修改进度条
                label3.Text = p_ret.ToString() + " 秒"; //显示剩余时间
            }
           
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }
    }
}
