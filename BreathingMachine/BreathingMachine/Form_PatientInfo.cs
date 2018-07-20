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
    //病人信息结构体
    public struct PATIENT_INFO
    {
        public string name;
        public string age;
        public string gender;
        public string height;
        public string weight;
        public string phoneNum;
        public string adress;
    }
    //定义委托
    public delegate void PatientInfoHandler(PATIENT_INFO info);
    public partial class Form_PatientInfo : Form
    {

        public Form_PatientInfo()
        {
            InitializeComponent();
        }
        //实例化病人信息委托
        public event PatientInfoHandler PatientInfo;

        private bool isNumberic(string message,ref  double result)
        {
            //如果第一个字符为0，返回false
            if(message[0]=='0')
            {
                return false;
            }
            int len = message.Length;
            //判断是否全部为数字
            int nDot_num = 0;
            for (int i = 0; i < len;i++ )
            {
                //判断有几个小数点
                if(message[i]=='.')
                {
                    nDot_num ++;
                    if(nDot_num>1)
                    {
                        return false;
                    }
                    continue;
                }
                //判断是否在0-9之间
                if ((message[i] >= '0' && message[i] <= '9') )
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            result = Convert.ToDouble(message);
            return true;
        }
        
        private bool isPhoneNum(string message)
        {
            for (int i = 0; i < message.Length; i++)
            {
                //判断是否在0-9之间,'-'认为是合法的
                if ((message[i] >= '0' && message[i] <= '9') || message[i]=='-')
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            PATIENT_INFO info = new PATIENT_INFO();
            info.name = this.textBox_name.Text.Trim();
            info.age = this.textBox_age.Text.Trim();
            info.gender = this.radioButton_male.Checked?"男":"女";
            info.height = this.textBox_height.Text.Trim();
            info.weight = this.textBox_weight.Text.Trim();
            info.phoneNum = this.textBox_phoneNum.Text.Trim();
            info.adress = this.textBox_adress.Text.Trim();

            DataMngr.m_old_PatientInfo = info;
            //姓名，年龄，性别为必填
            #region
            string str_errHeight = "";
            string str_errWeight = "";
            string str_errPhoneNum = "";
            bool b_HeightInfo_Right = true;
            bool b_WeightInfo_Right = true;
            bool b_PhoneNum_Right = true;

            //如果姓名年龄都填了
            if (info.name!="" && info.age!="" )
            {
                //判断年龄是否填正确，范围0-300
                #region
                double result = -1;
                string str_errAge = "";
                bool b_AgeInfo_Right = true;
                if (!isNumberic(info.age, ref result))
                {
                    str_errAge = LanguageMngr.errAge();
                    b_AgeInfo_Right = false;
                }
                else
                {
                    if(result<=0.0||result>=300.0)
                    {
                        str_errAge = LanguageMngr.errAge();
                        b_AgeInfo_Right = false;
                    }
                }
                #endregion

                //如果年龄填正确了，身高，体重，电话号码，地址都为空
                #region
                if (info.height == "" && info.weight == "" && info.phoneNum == "" && info.adress=="")
                {
                    if (b_HeightInfo_Right && b_WeightInfo_Right && b_PhoneNum_Right && b_AgeInfo_Right)
                    {
                        if (PatientInfo != null)//判断事件是否为空
                        {
                            PatientInfo(info);//执行委托实例  
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                    }
                    else
                    {
                        //str_errAge = LanguageMngr.errAge();
                        //str_errHeight = LanguageMngr.errHeight();
                        //str_errWeight = LanguageMngr.errWeight();
                        //str_errPhoneNum = LanguageMngr.errPhoneNum();
                        MessageBox.Show(LanguageMngr.pls_fill_in_right() + str_errAge + " " + str_errHeight + " " + str_errWeight + " " + str_errPhoneNum);
                        return;
                    }
                    #region
                    if (PatientInfo != null)//判断事件是否为空
                    {
                        PatientInfo(info);//执行委托实例  
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    #endregion
                }
                else
                {
                    //如果年龄填正确了，身高，体重，电话号码，地址有一个或多个不为空
                    #region
                    //如果填写了身高，对填写的信息进行判断
                    #region
                    if (info.height != "")
                    {
                        //先校验书写的是否是数字
                        double db_Heihgt_result=-1.0;
                        if (isNumberic(info.height, ref db_Heihgt_result))
                        {
                            //如果为数字，判断范围是否在0-500
                            if (db_Heihgt_result <= 0.0 || db_Heihgt_result >= 500.0)
                            {
                                b_HeightInfo_Right = false;
                                str_errHeight = LanguageMngr.errHeight();
                            }
                        }
                        else
                        {
                            b_HeightInfo_Right = false;
                            str_errHeight = LanguageMngr.errHeight();
                        }
                    }
                    #endregion

                    //如果填写了体重，对填写的信息进行判断,0-500kg
                    #region
                    if (info.weight != "")
                    {
                        //先校验书写的是否是数字
                        double db_Weight_result = -1.0;
                        if (isNumberic(info.weight, ref db_Weight_result))
                        {
                            if (db_Weight_result <= 0.0 || db_Weight_result >= 500.0)
                            {
                                b_WeightInfo_Right = false;
                                str_errWeight = LanguageMngr.errWeight();
                            }
                        }
                        else
                        {
                            b_WeightInfo_Right = false;
                            str_errWeight = LanguageMngr.errWeight();
                        }
                    }
                    #endregion

                    //如果填写了电话号码，对填写的信息进行判断
                    #region
                    if (info.phoneNum != "")
                    {
                        //先校验书写的是否是数字
                        if (!isPhoneNum(info.phoneNum))
                        {
                            b_PhoneNum_Right = false;
                            str_errPhoneNum = LanguageMngr.errPhoneNum();
                        }
                    }
                    #endregion

                    //如果身高，体重，年龄都正确，就执行委托
                    #region
                    if (b_HeightInfo_Right && b_WeightInfo_Right && b_PhoneNum_Right &&b_AgeInfo_Right)
                    {
                        if (PatientInfo != null)//判断事件是否为空
                        {
                            PatientInfo(info);//执行委托实例  
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                    }
                    else
                    {
                        //str_errAge = LanguageMngr.errAge();
                        //str_errHeight = LanguageMngr.errHeight();
                        //str_errWeight = LanguageMngr.errWeight();
                        //str_errPhoneNum = LanguageMngr.errPhoneNum();
                        MessageBox.Show(LanguageMngr.pls_fill_in_right() + str_errAge + " " + str_errHeight + " " + str_errWeight + " " + str_errPhoneNum);
                        return;
                    }
                    #endregion
                       
                    #endregion
                }
                #endregion
            }
            else
            {
                //姓名，年龄没有填完全
                MessageBox.Show(LanguageMngr.pls_fill_up_name_and_age());
            }
            #endregion
        }

        private void button_cancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form_PatientInfo_Load(object sender, EventArgs e)
        {
            LanguageMngr lang = new LanguageMngr();
            //初始化语言
            #region
            this.Text = lang.patient_info();
            this.label_name.Text = lang.name();
            this.label_age.Text = lang.age();
            this.label_gender.Text = lang.gender();
            this.radioButton_male.Text = lang.male();
            this.radioButton_femal.Text = lang.female();
            this.label_height.Text = lang.height();
            this.label_weight.Text = lang.weight();
            this.label_phoneNum.Text = lang.phoneNum();
            this.label_address.Text = lang.address();
            this.button_ok.Text = lang.ok();
            this.button_cancle.Text = lang.cancle();
            #endregion

            this.textBox_name.Text = DataMngr.m_old_PatientInfo.name;
            this.textBox_age.Text = DataMngr.m_old_PatientInfo.age;
            this.textBox_height.Text = DataMngr.m_old_PatientInfo.height;
            this.textBox_weight.Text = DataMngr.m_old_PatientInfo.weight;
            this.textBox_phoneNum.Text = DataMngr.m_old_PatientInfo.phoneNum;
            this.textBox_adress.Text = DataMngr.m_old_PatientInfo.adress;
            if(DataMngr.m_old_PatientInfo.gender=="女")
            {
                this.radioButton_femal.Checked = true;
            }
            else
            {
                this.radioButton_male.Checked = true;
            }
            this.StartPosition = FormStartPosition.CenterScreen;
        }
    }
}
