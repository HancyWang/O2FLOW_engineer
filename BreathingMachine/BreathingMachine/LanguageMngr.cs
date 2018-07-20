using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreathingMachine
{
    class LanguageMngr
    {
        public static LANGUAGE m_language=LANGUAGE.CHINA;  //0-中文,1-英语，，其它待添加
        public string set()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "设置";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Set";
            }
            else
            {
                return "";
            }
        }
        public string help()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "帮助";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Help";
            }
            else
            {
                return "";
            }
        }

        public string soft_ver()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "软件版本：";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Soft ver.：";
            }
            else
            {
                return "";
            }
        }

        public string language()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "语言";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Language";
            }
            else
            {
                return "";
            }
        }

        public string chinese()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "中文";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Chinese";
            }
            else
            {
                return "";
            }
        }

        public string english()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "英文";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "English";
            }
            else
            {
                return "";
            }
        }

        public string importData()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "导入数据";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Import Data";
            }
            else
            {
                return "";
            }
        }
        public string exportData()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "导出数据";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Export Data";
            }
            else
            {
                return "";
            }
        }

        public string advanceMode()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "高级模式";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Advance Mode";
            }
            else
            {
                return "";
            }
        }

        public string showAllWorkData()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "显示所有工作数据";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Show All Work Data";
            }
            else
            {
                return "";
            }
        }
        public string timePeriodSet()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "时段设置";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Period Setting";
            }
            else
            {
                return "";
            }
        }

        public string startDate()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "开始时间";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Startting Date";
            }
            else
            {
                return "";
            }
        }
        public string endDate()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "结束时间";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Ending Date";
            }
            else
            {
                return "";
            }
        }

        public string basicInfo()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "基本信息";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Basic Info.";
            }
            else
            {
                return "";
            }
        }

        public string usageTime()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "使用时间";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Usage Time";
            }
            else
            {
                return "";
            }
        }

        public string detailCharts()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "详细图表";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Detail Charts";
            }
            else
            {
                return "";
            }
        }
        public string alarmInfo()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "报警信息";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Alarm Info";
            }
            else
            {
                return "";
            }
        }

        public string workInfo()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "工作信息";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Work Info";
            }
            else
            {
                return "";
            }
        }

        public string name()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "姓名：";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Name：";
            }
            else
            {
                return "";
            }
        }

        public string age()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "年龄：";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Age：";
            }
            else
            {
                return "";
            }
        }
        public string gender()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "性别：";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Gender：";
            }
            else
            {
                return "";
            }
        }

        public string height()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "身高：";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Height：";
            }
            else
            {
                return "";
            }
        }

        public string weight()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "体重：";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Weight：";
            }
            else
            {
                return "";
            }
        }
        public string patient_info()
        {
            if(m_language==LANGUAGE.CHINA)
            {
                return "病人信息";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Patient Info";
            }
            else
            {
                return "";
            }
        }

        public string phoneNum()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "电话号码：";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Phone No.：";
            }
            else
            {
                return "";
            }
        }

        public string address()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "住址：";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Adress：";
            }
            else
            {
                return "";
            }
        }

        public string edit()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "编辑";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Edit";
            }
            else
            {
                return "";
            }
        }

        public string generateReport()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "生成报告";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Generate Report";
            }
            else
            {
                return "";
            }
        }

        public string machineType()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "设备型号：";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Machine Type：";
            }
            else
            {
                return "";
            }
        }

        public string equipInfo()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "设备信息";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Euipment Info";
            }
            else
            {
                return "";
            }
        }

        public string male()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "男";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "male";
            }
            else
            {
                return "";
            }
        }

        public string female()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "女";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "female";
            }
            else
            {
                return "";
            }
        }

        public string timePeriod()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "时间段";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Time Period";
            }
            else
            {
                return "";
            }
        }

        public static string pls_enter_right_usrname_pwd()
        {
            if(m_language==LANGUAGE.CHINA)
            {
                return "请输入正确的用户名和密码";
            }
            else if(m_language==LANGUAGE.ENGLISH)
            {
                return "Please enter the right username and password！";
            }
            else
            {
                return "";
            }
        }

        public string ok()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "确定";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "ok";
            }
            else
            {
                return "";
            }
        }

        public string cancle()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "取消";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "cancle";
            }
            else
            {
                return "";
            }
        }

        public static string no_data()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "无数据！";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "No data!";
            }
            else
            {
                return "";
            }
        }

        public static string pls_import_data()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "请导入数据！";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Please import data first!";
            }
            else
            {
                return "";
            }
        }

        public static string errAge()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "年龄";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "age";
            }
            else
            {
                return "";
            }
        }
        public static string errHeight()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "身高";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "height";
            }
            else
            {
                return "";
            }
        }
        public static string errWeight()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "体重";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "weight";
            }
            else
            {
                return "";
            }
        }
        public static string errPhoneNum()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "电话号码";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "phoneNo.";
            }
            else
            {
                return "";
            }
        }
        
        public static string pls_fill_up_name_and_age()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "姓名和年龄为必填信息，请将信息填完！";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Name and age are necessary information,please fill up!";
            }
            else
            {
                return "";
            }
        }
        public string showGenderValue(string gender)
        {
            //if(gender=="")
            //{
            //    return "NA";
            //}
            if(gender=="男"||gender=="male")
            {
                if (m_language == LANGUAGE.CHINA)
                {
                    return "男";
                }
                else if (m_language == LANGUAGE.ENGLISH)
                {
                    return "male";
                }
                else
                {
                    return "";
                }
            }
            else if (gender == "女" || gender == "female")
            {
                if (m_language == LANGUAGE.CHINA)
                {
                    return "女";
                }
                else if (m_language == LANGUAGE.ENGLISH)
                {
                    return "female";
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "NA";
            }
        }

        public string date()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "日期";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Date";
            }
            else
            {
                return "";
            }
        }
        public string alarm_info()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "报警信息";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Alarm Info";
            }
            else
            {
                return "";
            }
        }

        public string running_mode()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "运行模式";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Running Mode";
            }
            else
            {
                return "";
            }
        }

        public string alarm_code()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "报警码";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Alarm Code";
            }
            else
            {
                return "";
            }
        }

        public string alarm_value_L()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "报警数据1";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Alarm value 1";
            }
            else
            {
                return "";
            }
        }
        public string alarm_value_H()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "报警数据2";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Alarm value 2";
            }
            else
            {
                return "";
            }
        }

        public static string pls_fill_in_right()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "请填写正确的：";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Please fill in the right：";
            }
            else
            {
                return "";
            }
        }

        public static string generate_report_successful()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "生成报告成功！";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Generate report sucessful!";
            }
            else
            {
                return "";
            }
        }
        public static string pls_complete_patient_info()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "请先完善病人信息！";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Please complete patient informations!";
            }
            else
            {
                return "";
            }
        }

        public static string startTime_begyond_endTime()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "开始时间大于结束时间，请重选！";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Start date beyond end date,please choose again!";
            }
            else
            {
                return "";
            }
        }

        public static string file_export_completed()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "文件导出完毕！";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "File export completed!";
            }
            else
            {
                return "";
            }
        }
        public static string pls_choose_time()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "请选择时间！";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Please choose time!";
            }
            else
            {
                return "";
            }
        }
        public static string fail_to_get_alarmFile_info()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "获取报警文件信息失败！";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Fail to get alarm file info!";
            }
            else
            {
                return "";
            }
        }

        public static string pls_choose_the_right_folder()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "请选择正确的文件夹！";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Please choose the right folder!";
            }
            else
            {
                return "";
            }
        }
        public static string open_alarmFile_fail()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "打开报警文件失败！";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Open alarm file fail!";
            }
            else
            {
                return "";
            }
        }
        public static string pls_ensure_import_data_and_choose_time()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "请确认导入了数据，并且选择了时间！";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Please make sure already import data and choose time!";
            }
            else
            {
                return "";
            }
        }


        public static string u_r_already_in_eng_mode()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "您已经在工程师模式！";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "You are already in engineer mode!";
            }
            else
            {
                return "";
            }
        }

        public static string welcom_enterinto_eng_mode()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "欢迎进入工程师模式！";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Welcome to engineer mode!";
            }
            else
            {
                return "";
            }
        }

        public static string adault_or_child(byte bt)
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return Convert.ToBoolean(bt) ? "儿童" : "成人";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return Convert.ToBoolean(bt) ? "Child" : "Adault";
            }
            else
            {
                return "";
            }
        }

        //if(LanguageMngr.m_language==LANGUAGE.CHINA)
        //    {
        //        str = Convert.ToBoolean(bt) ? "雾化" : "湿化";
        //    }
        //    else if(LanguageMngr.m_language==LANGUAGE.ENGLISH)
        //    {
        //        str = Convert.ToBoolean(bt) ? "Atomization" : "Humidifying";
        //    }
        //    else
        //    {
        //        //等有其他语言再添加
        //    }
        public static String humidy_or_atomiz(byte bt)
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return Convert.ToBoolean(bt) ? "雾化" : "湿化";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return Convert.ToBoolean(bt) ? "Atomization" : "Humidifying";
            }
            else
            {
                return "";
            }
        }
        public  String set_temp()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "设定温度";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Set temperature";
            }
            else
            {
                return "";
            }
        }
        public  String set_flow()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "设定流量";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Set flow";
            }
            else
            {
                return "";
            }
        }
        public  String set_high_oxy_alarm()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "设定高氧浓度报警";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Set High Oxy Alarm";
            }
            else
            {
                return "";
            }
        }
        public  String set_low_oxy_alarm()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "设定低氧浓度报警";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Set Low Oxy Alarm";
            }
            else
            {
                return "";
            }
        }
        public  String set_atomiz_level()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "设定雾化量档位";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Set Atomiz Level";
            }
            else
            {
                return "";
            }
        }
        public  String set_atomiz_time()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "设定雾化时间";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Set Atomiz Time";
            }
            else
            {
                return "";
            }
        }
         public  String set_adault_or_child()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "设定成人儿童模式";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Set Adault or Child";
            }
            else
            {
                return "";
            }
        }
        public  String data_patient_tmp()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "患者端温度";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Patient Temperature";
            }
            else
            {
                return "";
            }
        }
        public  String data_air_outlet_tmp()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "出气口温度";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Air Outlet Temperature";
            }
            else
            {
                return "";
            }
        }
        public  String data_heating_plate_tmp()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "加热盘温度";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Heating Plate Temperature";
            }
            else
            {
                return "";
            }
        }
        public  String data_enviroment_tmp()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "环境温度";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Enviroment Temperature";
            }
            else
            {
                return "";
            }
        }

        public  String data_driveboard_tmp()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "驱动板温度";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Driving Board temperature";
            }
            else
            {
                return "";
            }
        }

        public  String data_flow()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "流量";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Flow";
            }
            else
            {
                return "";
            }
        }

        public String data_dewpoint_tmp()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "露点温度";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Dewpoint Temperature";
            }
            else
            {
                return "";
            }
        }

        public  String data_Oxy_concentration()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "氧浓度";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Oxy Concentration";
            }
            else
            {
                return "";
            }
        }
        public  String data_air_pressure()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "气道压力";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Air Pressure";
            }
            else
            {
                return "";
            }
        }
        public  String data_loop_type()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "回路类型";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Loop Type";
            }
            else
            {
                return "";
            }
        }
        public  String data_fault_statue1()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "故障状态位1";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Fault Statue 1";
            }
            else
            {
                return "";
            }
        }

        public  String data_fault_statue2()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "故障状态位2";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Fault Statue 2";
            }
            else
            {
                return "";
            }
        }
        public  String data_fault_statue3()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "故障状态位3";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Fault Statue 3";
            }
            else
            {
                return "";
            }
        }
        public  String data_fault_statue4()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "故障状态位4";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Fault Statue 4";
            }
            else
            {
                return "";
            }
        }
        public  String data_fault_statue5()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "故障状态位5";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Fault Statue 5";
            }
            else
            {
                return "";
            }
        }
        public  String data_fault_statue6()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "故障状态位6";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Fault Statue 6";
            }
            else
            {
                return "";
            }
        }
        public  String data_fault_statue7()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "故障状态位7";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Fault Statue 7";
            }
            else
            {
                return "";
            }
        }
        public  String data_fault_statue8()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "故障状态位8";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Fault Statue 8";
            }
            else
            {
                return "";
            }
        }
        public  String data_fault_statue9()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "故障状态位9";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Fault Statue 9";
            }
            else
            {
                return "";
            }
        }
        public  String data_fault_statue10()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "故障状态位10";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Fault Statue 10";
            }
            else
            {
                return "";
            }
        }
        public  String data_fault_statue11()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "故障状态位11";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Fault Statue 11";
            }
            else
            {
                return "";
            }
        }
        public  String data_fault_statue12()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "故障状态位12";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Fault Statue 12";
            }
            else
            {
                return "";
            }
        }
        public  String data_atomiz_DAC_L()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "雾化DAC数值L";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Atomiz DAC L";
            }
            else
            {
                return "";
            }
        }
        public  String data_atomiz_DAC_H()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "雾化DAC数值H";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Atomiz DAC H";
            }
            else
            {
                return "";
            }
        }
        public  String data_atomiz_ADC_L()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "雾化ADC数值L";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Atomiz ADC L";
            }
            else
            {
                return "";
            }
        }
        public  String data_atomiz_ADC_H()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "雾化ADC数值H";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Atomiz ADC H";
            }
            else
            {
                return "";
            }
        }
        public  String data_loop_heating_PWM_L()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "雾化ADC回路加热PWM数值L";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Loop Heating PWM L";
            }
            else
            {
                return "";
            }
        }
        public  String data_loop_heating_PWM_H()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "雾化ADC回路加热PWM数值H";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Loop Heating PWM H";
            }
            else
            {
                return "";
            }
        }
        public  String data_loop_heating_ADC_L()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "回路加热ADC数值L";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Loop Heating ADC L";
            }
            else
            {
                return "";
            }
        }
        public  String data_loop_heating_ADC_H()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "回路加热ADC数值H";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Loop Heating ADC H";
            }
            else
            {
                return "";
            }
        }
        public  String data_heating_plate_PWM_L()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "加热盘加热PWM数值L";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Heating Plate PWM L";
            }
            else
            {
                return "";
            }
        }
        public  String data_heating_plate_PWM_H()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "加热盘加热PWM数值H";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Heating Plate PWM H";
            }
            else
            {
                return "";
            }
        }
        public  String data_heating_plate_ADC_L()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "加热盘加热ADC数值L";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Heating Plate ADC L";
            }
            else
            {
                return "";
            }
        }
        public  String data_heating_plate_ADC_H()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "加热盘加热ADC数值H";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Heating Plate ADC H";
            }
            else
            {
                return "";
            }
        }
        public  String data_main_motor_drive_L()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "主马达驱动数值L";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Main motor driver L";
            }
            else
            {
                return "";
            }
        }
        public  String data_main_motor_drive_H()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "主马达驱动数值H";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Main motor driver H";
            }
            else
            {
                return "";
            }
        }

        public  String data_main_motor_speed_L()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "主马达转速数值L";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Main motor speed L";
            }
            else
            {
                return "";
            }
        }
        public  String data_main_motor_speed_H()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "主马达转速数值H";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Main motor speed H";
            }
            else
            {
                return "";
            }
        }
        public  String data_press_sensor_ADC_L()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "压力传感器ADC值L";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Press sensor ADC L";
            }
            else
            {
                return "";
            }
        }
        public  String data_press_sensor_ADC_H()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "压力传感器ADC值H";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Press sensor ADC H";
            }
            else
            {
                return "";
            }
        }
        public  String data_waterlevel_HADC_L()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "水位传感器HADC值L";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Water level HADC L";
            }
            else
            {
                return "";
            }
        }
        public  String data_waterlevel_HADC_H()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "水位传感器HADC值H";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Water level HADC H";
            }
            else
            {
                return "";
            }
        }
        public  String data_waterlevel_LADC_L()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "水位传感器LADC值L";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Water level LADC L";
            }
            else
            {
                return "";
            }
        }
        public  String data_waterlevel_LADC_H()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "水位传感器LADC值H";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Water level LADC H";
            }
            else
            {
                return "";
            }
        }
        public  String data_fan_driver_L()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "散热风扇驱动数值L";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Fan driver L";
            }
            else
            {
                return "";
            }
        }
        public  String data_fan_driver_H()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "散热风扇驱动数值H";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Fan driver H";
            }
            else
            {
                return "";
            }
        }
        public  String data_fan_speed_L()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "散热风扇转速数值L";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Fan speed L";
            }
            else
            {
                return "";
            }
        }
        public  String data_fan_speed_H()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "散热风扇转速数值H";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Fan speed H";
            }
            else
            {
                return "";
            }
        }

        public String data_main_motor_tmp_ADC_L()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "主马达温度ADC值L";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Main motor tmp value L";
            }
            else
            {
                return "";
            }
        }

        public String data_main_motor_tmp_ADC_H()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "主马达温度ADC值H";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Main motor tmp value H";
            }
            else
            {
                return "";
            }
        }

        public String atomization()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "雾化";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Atomization";
            }
            else
            {
                return "";
            }
        }
        public String humidification()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "湿化";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Humidification";
            }
            else
            {
                return "";
            }
        }

        public String overheat()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                //return "患者端超温";
                return "超温";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Overheat";
            }
            else
            {
                return "";
            }
        }

        public String power_off()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "电源断开";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
               // return "Power off";
                return "Power Out";
            }
            else
            {
                return "";
            }
        }

        //湿化罐(雾化罐)未安装
        public String check_chamber()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                //return "罐子未安装";
                return "检查水罐"; 
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Check Chamber";
            }
            else
            {
                return "";
            }
        }

        //缺水
        public String change_water_bag()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "缺水";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                //return "Change water bag";
                return "Lack of water";
            }
            else
            {
                return "";
            }
        }


        //温度探头未安装
        public String check_temp_probe()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                //return "连接数据";
                return "温度探头未安装";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                //return "Check temp.probe";
                return "Temp. Probe uninstalled";
            }
            else
            {
                return "";
            }
        }

        //管路未安装
        public String check_tube()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                //return "连接发热线";
                return "管路未安装";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                //return "Check tube";
                return "Tube uninstalled";
            }
            else
            {
                return "";
            }
        }

        //管路不匹配
        public String tube_not_match()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "管路不匹配";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "CTube not match";
            }
            else
            {
                return "";
            }
        }
        //堵塞
        public String check_blockages()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                //return "堵塞";
                return "检查堵塞";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                //return "Check blockages";
                return "Check for blockages";
            }
            else
            {
                return "";
            }
        }

         //高氧浓度
        public String high_O2()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "高氧浓度";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                //return "High O2";
                return "Oxygen too high";
            }
            else
            {
                return "";
            }
        }

        //低氧浓度
        public String low_O2()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "低氧浓度";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                //return "Low O2";
                return "Oxygen too low";
            }
            else
            {
                return "";
            }
        }

        //流量超范围
        public String flow_overrange()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                //return "达不到设定流量";
                return "流量超范围";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                //return "Flow overrange";
                return "Flow over range";
            }
            else
            {
                return "";
            }
        }

        //温度超范围
        public String temp_overrange()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                //return "温度达不到设定值";
                return "温度超范围";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                //return "Temp.overrange";
                return "Tempe. Over Range";
            }
            else
            {
                return "";
            }
        }

        //温度探头脱落
        public String prob_out()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "温度探头脱落";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Tem.Probe Out";
            }
            else
            {
                return "";
            }
        }

        //SD卡未安装
        public String sdCard_not_install()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "SD卡未安装";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "SD Card Uninstalled";
            }
            else
            {
                return "";
            }
        }
   
        //系统故障E1---氧浓度传感器故障
        public String system_failure_E1()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "系统故障E1";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "System failure E1";
            }
            else
            {
                return "";
            }
        }

        //系统故障E2---流量传感器故障
        public String system_failure_E2()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "系统故障E2";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "System failure E2";
            }
            else
            {
                return "";
            }
        }


        //系统故障E3---环境温度传感器故障
        public String system_failure_E3()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "系统故障E3";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "System failure E3";
            }
            else
            {
                return "";
            }
        }


        //系统故障E4---加热盘温度传感器故障
        public String system_failure_E4()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "系统故障E4";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "System failure E4";
            }
            else
            {
                return "";
            }
        }

        //系统故障E5---散热风扇故障
        public String system_failure_E5()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "系统故障E5";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "System failure E5";
            }
            else
            {
                return "";
            }
        }

        //系统故障E6---主风机过热故障
        public String system_failure_E6()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "系统故障E6";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "System failure E6";
            }
            else
            {
                return "";
            }
        }

        ////系统故障E7---散热器温度传感器故障(仅VUN-002)
        //public String system_failure_E7()
        //{
        //    if (m_language == LANGUAGE.CHINA)
        //    {
        //        return "系统故障E7";
        //    }
        //    else if (m_language == LANGUAGE.ENGLISH)
        //    {
        //        return "System failure E7";
        //    }
        //    else
        //    {
        //        return "";
        //    }
        //}


        //系统故障E7---温控开关开路
        public String system_failure_E7()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "系统故障E7";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "System failure E7";
            }
            else
            {
                return "";
            }
        }

        //系统故障E8---发热盘开路
        public String system_failure_E8()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "系统故障E8";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "System failure E8";
            }
            else
            {
                return "";
            }
        }


        public String oxy_concentration_sensor_fault()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "氧浓度传感器故障";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Oxygen concentration sensor fault";
            }
            else
            {
                return "";
            }
        }
        
        public String flow_sensor_fault()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "流量传感器故障";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Flow sensor fault";
            }
            else
            {
                return "";
            }
        }
        
        public String enviroment_tmp_sensor_fault()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "环境温度传感器故障";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Enviroment temperature fault";
            }
            else
            {
                return "";
            }
        }
        
        public String driverBoard_tmp_sensor_fault()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "驱动板温度传感器故障";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Driving board temperature fault";
            }
            else
            {
                return "";
            }
        }
        
        public String heating_plate_tmp_sensor_fault()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "加热盘温度传感器故障";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Heating plate temperature fault";
            }
            else
            {
                return "";
            }
        }
        
        public String fan_fault()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "散热风扇故障";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Fan fault";
            }
            else
            {
                return "";
            }
        }
        
        public String EEPROM_verify_fail()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "EEPROM校验失败";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "EEPROM verification failed";
            }
            else
            {
                return "";
            }
        }
        
        public String air_outlet_sensor_fault()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "出气口温度传感器故障";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Air outlet sensor fault";
            }
            else
            {
                return "";
            }
        }
        
        public String patient_tmp_sensor_fault()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "患者端温度传感器故障";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Patien temperature sensor fault";
            }
            else
            {
                return "";
            }
        }
        
        public String unknow_err()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "未识别的错误";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Unknow error";
            }
            else
            {
                return "";
            }
        }
        public String adault()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "成人";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "adault";
            }
            else
            {
                return "";
            }
        }
        public String child()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "儿童";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "child";
            }
            else
            {
                return "";
            }
        }
        public String top_page()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "首页";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Top Page";
            }
            else
            {
                return "";
            }
        }
        public String end_page()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "尾页";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "End Page";
            }
            else
            {
                return "";
            }
        }
        public String prev_page()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "上一页";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Prev Page";
            }
            else
            {
                return "";
            }
        }
        public String next_page()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "下一页";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Next Page";
            }
            else
            {
                return "";
            }
        }

        public String jump_to()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "跳转至:";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Jump to:";
            }
            else
            {
                return "";
            }
        }
        public String charts()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "图表";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Charts";
            }
            else
            {
                return "";
            }
        }

        public string title_usageTime()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "使用时间：";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Usage Time:";
            }
            else
            {
                return "";
            }
        }
        public String title_patient_tmp()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "病人温度";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Patient Temperature:";
            }
            else
            {
                return "";
            }
        }
        
        public String title_air_outlet_tmp()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "出气口温度：";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Air Outlet Temperature:";
            }
            else
            {
                return "";
            }
        }
        public String title_flow()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "流量：";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Flow:";
            }
            else
            {
                return "";
            }
        }
        public String title_oxy_concentration()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "氧浓度：";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Oxygen Concentration:";
            }
            else
            {
                return "";
            }
        }

        public String title_dewpoint_tmp()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "露点温度：";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Dewpoint Temperature:";
            }
            else
            {
                return "";
            }
        }

        public String soft_ver_in_menu()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "软件版本";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Soft version";
            }
            else
            {
                return "";
            }
        }
        public String app_name()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "永胜宏碁数据分析软件";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Vincent Medical Data Analyser Software";
            }
            else
            {
                return "";
            }
        }

        //"Lack of work information file!"
        public static String lack_of_work_file()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "缺少工作信息文件！";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Lack of work information file!";
            }
            else
            {
                return "";
            }
        }

        public static String lack_of_alarm_file()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "缺少报警信息文件！";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "Lack of alarm information file!";
            }
            else
            {
                return "";
            }
        }
        //No alarm information in alarm file
        public static String no_data_info_in_alarm_file()
        {
            if (m_language == LANGUAGE.CHINA)
            {
                return "报警信息文件中没有数据！";
            }
            else if (m_language == LANGUAGE.ENGLISH)
            {
                return "No data in alarm file!";
            }
            else
            {
                return "";
            }
        }
    }
}
