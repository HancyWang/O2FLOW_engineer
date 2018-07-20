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
    public partial class Form_About : Form
    {
        public Form_About()
        {
            InitializeComponent();
        }

        private void Form_About_Load(object sender, EventArgs e)
        {
            //if(LanguageMngr.m_language==LANGUAGE.CHINA)
            //{
            //    this.Text = "关于我们";
            //    label_AboutUs_appName.Text = "永胜宏基数据分析软件";
            //    label_AboutUs_appVersion.Text = "软件版本：" + DataMngr.m_App_Version;
            //    label_AboutUs_CompanyName.Text = "@2018-2026 东莞永胜宏基医疗器械有限公司版权所有";
            //}
            //else if(LanguageMngr.m_language==LANGUAGE.ENGLISH)
            //{
            //    this.Text = "About us";
            //    label_AboutUs_appName.Text = "Vincent Medical Data Analyser Software";
            //    label_AboutUs_appVersion.Text = "Software Version:" + DataMngr.m_App_Version;
            //    label_AboutUs_CompanyName.Text = "@2018-2026 by DongGuan Vincent Medical Co.,limited";
            //}
            //else
            //{
            //    //其他语言
            //}

        }
    }
}
