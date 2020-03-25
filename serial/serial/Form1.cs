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
                                           //等价于
                                           // textBox1.Text += ret;
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
                MessageBox.Show("请先配置串口，再使能串口");
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
                    int i = 1;
                    BIT[0] = (byte)str[0];
                    while (BIT[0] != '\n')  //循环发送数据，字节的方式，不发送新行
                    {
                        serialPort1.Write(BIT, 0, 1);  //发送数组下标为0的元素
                        BIT[0] = (byte)str[i++];
                    }
                    textBox4.Text = "";  //清除发送区
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

    }
}
