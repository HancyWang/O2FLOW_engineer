using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BreathingMachine
{
    public partial class Form_engineer : Form
    {
        public Form_engineer()
        {
            InitializeComponent();
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            Form1.g_username = this.textBox_UserName.Text;
            Form1.g_password = this.textBox_Password.Text;
            if (this.textBox_UserName.Text != "eng004" || this.textBox_Password.Text!="eng004")
            {
                MessageBox.Show(LanguageMngr.pls_enter_right_usrname_pwd());
            }
            if (this.textBox_UserName.Text == "eng004" || this.textBox_Password.Text == "eng004")
            {
                this.Close();
            }
            
        }


        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form_engineer_Shown(object sender, EventArgs e)
        {
            if(LanguageMngr.m_language==LANGUAGE.CHINA)
            {
                this.label_username.Text = "用户名";
                this.label_password.Text = "密码";
                this.button_ok.Text = "确定";
                this.button_cancel.Text = "取消";
            }
            else if(LanguageMngr.m_language==LANGUAGE.ENGLISH)
            {
                this.label_username.Text = "User Name";
                this.label_password.Text = "Password";
                this.button_ok.Text = "OK";
                this.button_cancel.Text = "CANCEL";

            }
            else
            {

            }
        }
    }
}
