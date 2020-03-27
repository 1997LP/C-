using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace serial
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;//只读下拉组合框
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;//只读下拉组合框
            comboBox4.DropDownStyle = ComboBoxStyle.DropDownList;//只读下拉组合框

            for (int i=1; i<20;i++)
            {
                comboBox1.Items.Add("COM" + i.ToString());  //添加元素
            }
            CheckForIllegalCrossThreadCalls = false; //去掉线程间操作无效的检查
            comboBox2.Items.Add( "9600");
            comboBox2.Items.Add( "115200");

            for (int i = 5; i < 9; i++)
            {
                comboBox4.Items.Add(i.ToString());
            }

            textBox1.ReadOnly = true; //将文本框设置为只读，接收区
            textBox4.Text = "";
            serialPort1.PortName = "COM11";  //串口初始化
            serialPort1.BaudRate = 115200;
            serialPort1.DataBits = 8;
           
            //为串口添加接收事件函数
            serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(port_DateReceived) ;
        }

        //串口数据接收事件
        private void port_DateReceived(object sender,System.IO.Ports.SerialDataReceivedEventArgs e )
        {
            if (radioButton4.Checked) //判断是否以字符串的方式读取
            {
                string ret = serialPort1.ReadExisting();//读取输入缓冲区的所有字节
                textBox1.AppendText(ret);  //将读取到的数据追加到文本框的末尾        
            }
            else if(radioButton3.Checked)  //读取到的字符对应的ACill以16进制显示
            {
                byte ret = (byte)serialPort1.ReadByte();  //读取输入缓冲区的一个字节  10 --对应 A
                string str = ret.ToString("x");            //将byte对应的16进制，转换为16进制的字符串0-ff;
                textBox1.Text += (str.Length == 1) ? "0x0" + str + " " : "0x" + str + " ";     //追加到文末
            }
        }

        private void button1_Click(object sender, EventArgs e)  //start按钮
        {
            try
            {
                serialPort1.PortName = comboBox1.Text;
                serialPort1.BaudRate = Convert.ToInt32(comboBox2.Text);
                serialPort1.DataBits = Convert.ToInt32(comboBox4.Text);
            }
            catch
            {
                MessageBox.Show("请重新配置串口，此为系统默认端口设置");
            }

            try
            {
                serialPort1.Open();  //打开串口
                button1.Enabled = false;
                button2.Enabled = true;
                timer1.Start();
            }
            catch
            {
                if (serialPort1.IsOpen)
                {
                    MessageBox.Show("串口连接错误");
                }
                else
                {
                    MessageBox.Show("串口不存在，请重新连接");

                }

            }

        }


        private void button3_Click(object sender, EventArgs e)  //清除屏幕
        {
            textBox1.Text = "";
        }

        private void timer1_Tick(object sender, EventArgs e)   //定时器处理函数，判断是否要发送数据
        {

            byte[] ret = new byte[1];

            if (serialPort1.IsOpen && textBox4.Text != "")
            {
                string str = textBox4.Text;
                byte[] BIT = new byte[1];

                if (textBox4.Text.Substring(textBox4.Text.Length - 1, 1) == "\n")  //按下回车，发送数据
                {
                    if (radioButton1.Checked)  //以字符方式发送
                    {
                        int i = 1;
                        BIT[0] = (byte)str[0];
                        if (radioButton5.Checked)  //判断是否发送新行
                        {
                            while (BIT[0] != '\n')  //循环发送数据，字节的方式，发送回车\r,按下回车键会发送"\r\n"
                            {
                                serialPort1.Write(BIT, 0, 1);  //发送数组下标为0的元素
                                BIT[0] = (byte)str[i++];
                            }
                        }
                        else  //不发送回车\r
                        {
                            while (BIT[0] != '\r') //循环发送数据，字节的方式，不发送回车\r
                            {
                                serialPort1.Write(BIT, 0, 1);  //发送数组下标为0的元素
                                BIT[0] = (byte)str[i++];
                            } 

                        }
                        textBox4.Text = "";  //清除发送区
                    }

                    else if (radioButton2.Checked)  //字符以对应的ASCII的16进制进行发送
                    {
                        int length = textBox4.Text.Length -2;
                        byte[] buf = new byte[2];
                        string str_test;
                        try
                        {
                            for (int i = 0; i < length; i += 3)
                            {
                                str_test = textBox4.Text.Substring(i,2);         //字符串6c 对应  整数108
                                buf[0] =  (byte)Convert.ToInt32(str_test, 16);//字符串形式的十六进制，转换为整形
                                serialPort1.Write(buf, 0, 1);  //发送数组下标为0的元素
                            }

                            if (radioButton5.Checked)//发送回车  :\r
                            {
                                buf[0] = 13;
                                serialPort1.Write(buf, 0, 1);  //发送数组下标为0的元素
                            }
                        }
                        catch
                        {
                            //MessageBox.Show("sh");
                        }
                       
                        textBox4.Text = "";  //清除发送区
                    }
                  
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)  //关闭串口。
        {
            if (serialPort1.IsOpen)
            {
                button2.Enabled = false;
                button1.Enabled = true;
                serialPort1.Close();
            }
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {

        }


        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }
        

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            byte[] buf = new byte[1];
            buf[0] = 13;//发送回车  :\r
            serialPort1.Write(buf, 0, 1);  //发送数组下标为0的元素
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
