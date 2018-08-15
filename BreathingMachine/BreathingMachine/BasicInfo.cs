using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreathingMachine
{
    //全部的工作信息

    public struct WorkData
    {
        //构造函数
        public WorkData(
        #region
            string no,
            string time,
            string setmode,
            string settmp,
            string setflow,
            string sethighoxyalarm,
            string setlowoxyalrm,
            string setatomizlevel,
            string setatomiztime,
            string setadaultorchild,
            string datapatienttmp,
            string dataairoutlettmp,
            string dataheatingplatetmp,
            string dataenvtmp,
            string datadriveboardtmp,
            string dataflow,
            string dataoxyconcentration,
            string dataairpressure,
            string datalooptype,
            string datafaultstates0,  //故障状态位bit0-bit7,bit0-bit3
            string datafaultstates1,
            string datafaultstates2,
            string datafaultstates3,
            string datafaultstates4,
            string datafaultstates5,
            string datafaultstates6,
            string datafaultstates7,
            string datafaultstates8,
            string datafaultstates9,
            string datafaultstates10,
            string datafaultstates11,
            string dataatmozDACL,
            string dataatmozDACH,
            string dataatmozADCL,
            string dataatmozADCH,
            string dataloopheatingPWML,
            string dataloopheatingPWMH,
            string dataloopheatingADCL,
            string dataloopheatingADCH,
            string dataloopheatingplatePWML,
            string dataloopheatingplatePWMH,
            string dataloopheatingplateADCL,
            string dataloopheatingplateADCH,
            string datamainmotordriveL,
            string datamainmotordriveH,
            string datamainmotorspeedL,
            string datamainmotorspeedH,
            string datapresssensorADCL,
            string datapresssensorADCH,
            string datawaterlevelsensorHADCL,
            string datawaterlevelsensorHADCH,
            string datawaterlevelsensorLADCL,
            string datawaterlevelsensorLADCH,
            string datafandriverL,
            string datafandriverH,
            string datafanspeedL,
            string datafanspeedH,
            string dataMainMotorTmpADCL,  //2018/7/20,这两个遗漏了，补上
            string dataMainMotorTmpADCH,
            string dataDewpointTmp       //2018/7/20，新增露点温度
        #endregion
                )
        {
            #region
            No =no;
            tm=time;
            set_mode=setmode;
            set_tmp=settmp;
            set_flow=setflow;
            set_high_oxy_alarm=sethighoxyalarm;
            set_low_oxy_alrm=setlowoxyalrm;
            set_atomiz_level=setatomizlevel;
            set_atomiz_time=setatomiztime;
            set_adault_or_child=setadaultorchild;
            data_patient_tmp=datapatienttmp;
            data_air_outlet_tmp=dataairoutlettmp;
            data_heating_plate_tmp=dataheatingplatetmp;
            data_env_tmp=dataenvtmp;
            data_driveboard_tmp=datadriveboardtmp;
            data_flow=dataflow;
            data_oxy_concentration=dataoxyconcentration;
            data_air_pressure=dataairpressure;
            data_loop_type=datalooptype;
            data_faultstates_0=datafaultstates0;
            data_faultstates_1=datafaultstates1;
            data_faultstates_2=datafaultstates2;
            data_faultstates_3=datafaultstates3;
            data_faultstates_4=datafaultstates4;
            data_faultstates_5=datafaultstates5;
            data_faultstates_6=datafaultstates6;
            data_faultstates_7=datafaultstates7;
            data_faultstates_8=datafaultstates8;
            data_faultstates_9=datafaultstates9;
            data_faultstates_10=datafaultstates10;
            data_faultstates_11=datafaultstates11;

            data_atmoz_DAC_L=dataatmozDACL;
            data_atmoz_DAC_H=dataatmozDACH;
            data_atmoz_ADC_L=dataatmozADCL;
            data_atmoz_ADC_H=dataatmozADCH;
            data_loop_heating_PWM_L=dataloopheatingPWML;
            data_loop_heating_PWM_H=dataloopheatingPWMH;
            data_loop_heating_ADC_L = dataloopheatingADCL;
            data_loop_heating_ADC_H = dataloopheatingADCH;
            data_loop_heating_plate_PWM_L = dataloopheatingplatePWML;
            data_loop_heating_plate_PWM_H = dataloopheatingplatePWMH;
            data_loop_heating_plate_ADC_L = dataloopheatingplateADCL;
            data_loop_heating_plate_ADC_H = dataloopheatingplateADCH;
            data_main_motor_drive_L = datamainmotordriveL;
            data_main_motor_drive_H = datamainmotordriveH;
            data_main_motor_speed_L = datamainmotorspeedL;
            data_main_motor_speed_H = datamainmotorspeedH;
            data_press_sensor_ADC_L = datapresssensorADCL;
            data_press_sensor_ADC_H = datapresssensorADCH;
            data_waterlevel_sensor_HADC_L = datawaterlevelsensorHADCL;
            data_waterlevel_sensor_HADC_H = datawaterlevelsensorHADCH;
            data_waterlevel_sensor_LADC_L = datawaterlevelsensorLADCL;
            data_waterlevel_sensor_LADC_H = datawaterlevelsensorLADCH;
            data_fan_driver_L = datafandriverL;
            data_fan_driver_H = datafandriverH;
            data_fan_speed_L = datafanspeedL;
            data_fan_speed_H = datafanspeedH;
            data_main_motor_tmp_ADC_L = dataMainMotorTmpADCL; //2018/7/20,这两个遗漏了，补上
            data_main_motor_tmp_ADC_H = dataMainMotorTmpADCH;
            data_dewpoint_tmp = dataDewpointTmp;  //2018/7/20，新增露点温度
            #endregion
        }

        #region
        public string No;
        public string tm;
        public string set_mode;
        public string set_tmp;
        public string set_flow;
        public string set_high_oxy_alarm;
        public string set_low_oxy_alrm;
        public string set_atomiz_level;
        public string set_atomiz_time;
        public string set_adault_or_child;
        public string data_patient_tmp;
        public string data_air_outlet_tmp;
        public string data_heating_plate_tmp;
        public string data_env_tmp;
        public string data_driveboard_tmp;
        public string data_flow;
        public string data_oxy_concentration;
        public string data_air_pressure;
        public string data_loop_type;
        public string data_faultstates_0;
        public string data_faultstates_1;
        public string data_faultstates_2;
        public string data_faultstates_3;
        public string data_faultstates_4;
        public string data_faultstates_5;
        public string data_faultstates_6;
        public string data_faultstates_7;
        public string data_faultstates_8;
        public string data_faultstates_9;
        public string data_faultstates_10;
        public string data_faultstates_11;

        public string data_atmoz_DAC_L;
        public string data_atmoz_DAC_H;
        public string data_atmoz_ADC_L;
        public string data_atmoz_ADC_H;
        public string data_loop_heating_PWM_L;
        public string data_loop_heating_PWM_H;
        public string data_loop_heating_ADC_L;
        public string data_loop_heating_ADC_H;
        public string data_loop_heating_plate_PWM_L;
        public string data_loop_heating_plate_PWM_H;
        public string data_loop_heating_plate_ADC_L;
        public string data_loop_heating_plate_ADC_H;
        public string data_main_motor_drive_L;
        public string data_main_motor_drive_H;
        public string data_main_motor_speed_L;
        public string data_main_motor_speed_H;
        public string data_press_sensor_ADC_L;
        public string data_press_sensor_ADC_H;
        public string data_waterlevel_sensor_HADC_L;
        public string data_waterlevel_sensor_HADC_H;
        public string data_waterlevel_sensor_LADC_L;
        public string data_waterlevel_sensor_LADC_H;
        public string data_fan_driver_L;
        public string data_fan_driver_H;
        public string data_fan_speed_L;
        public string data_fan_speed_H;
        public string data_main_motor_tmp_ADC_L;   //2018/7/20,这两个遗漏了，补上
        public string data_main_motor_tmp_ADC_H;
        public string data_dewpoint_tmp;           //2018/7/20，新增露点温度
        #endregion
    }

    //基本的工作信息
    public struct WorkData_Basic
    {
        public WorkData_Basic(string no,string time,string setmode,string setAdaultOrChild,
            string dataPatientTmp,string dataAirOutletTmp,string dataFlow,string dataOxyConcentration,string dewpointTmp)
        {
            No=no;
            tm=time;
            set_mode = setmode;
            set_adault_or_child = setAdaultOrChild;
            data_patient_tmp = dataPatientTmp;
            data_air_outlet_tmp = dataAirOutletTmp;
            data_flow = dataFlow;
            data_oxy_concentration = dataOxyConcentration;
            data_dewpoint_tmp = dewpointTmp;  //  2018/7/20新增露点温度

        }
            
        #region
        public string No;
        public string tm;
        public string set_mode;
        public string set_adault_or_child;
        public string data_patient_tmp;
        public string data_air_outlet_tmp;
        public string data_flow;
        public string data_oxy_concentration;
        public string data_dewpoint_tmp; //  2018/7/20新增露点温度
        #endregion
    }

    public struct DETAIL_CHART_INFO
    {
        public DateTime tm;
        public int patient_tmp;
        public int air_outlet_tmp;
        public int flow;
        public int oxy_concentration;
        public int dewpoint_tmp;  //  2018/7/20新增露点温度
    }

    public struct CHART_SIZE
    {
        public int Height;
        public int Width;
    }

    public enum CHARTTYPE
    {
        PATIENT_TMP,
        AIR_OUTLET_TMP,
        FLOW,
        OXY_CONCENTRATION,
        DEWPOINT_TMP
    }

    public class BasicInfo
    {

    }


    //以下是从C++头文件通过软件转换过来的

    public partial class NativeConstants
    {
        /// ALARM_MSG_LEN -> 16
        public const int ALARM_MSG_LEN = 16;

        /// WORKDATA_MSG_LEN -> 64
        public const int WORKDATA_MSG_LEN = 64;

        /// FILE_PATH_MAX -> 512
        public const int FILE_PATH_MAX = 512;

        /// BMP_UNSELECTED -> "vincent_medical.bmp"
        public const string BMP_UNSELECTED = "vincent_medical.bmp";

        /// BMP_SELECTED -> "selected.bmp"
        public const string BMP_SELECTED = "selected.bmp";

        /// ALARM_RUNNING_MODE_HUMIDIFY -> 0
        public const int ALARM_RUNNING_MODE_HUMIDIFY = 0;

        /// ALARM_RUNNING_MODE_AUTOMIZATION -> 1
        public const int ALARM_RUNNING_MODE_AUTOMIZATION = 1;

        /// ALARM_ERR_CODE_DATA_0 -> "氧浓度传感器故障"
        public const string ALARM_ERR_CODE_DATA_0 = "氧浓度传感器故障";

        /// ALARM_ERR_CODE_DATA_1 -> "流量传感器故障"
        public const string ALARM_ERR_CODE_DATA_1 = "流量传感器故障";

        /// ALARM_ERR_CODE_DATA_2 -> "环境温度传感器故障"
        public const string ALARM_ERR_CODE_DATA_2 = "环境温度传感器故障";

        /// ALARM_ERR_CODE_DATA_3 -> "驱动板温度传感器故障"
        public const string ALARM_ERR_CODE_DATA_3 = "驱动板温度传感器故障";

        /// ALARM_ERR_CODE_DATA_4 -> "加热盘温度传感器故障"
        public const string ALARM_ERR_CODE_DATA_4 = "加热盘温度传感器故障";

        /// ALARM_ERR_CODE_DATA_5 -> "散热风扇故障"
        public const string ALARM_ERR_CODE_DATA_5 = "散热风扇故障";

        /// ALARM_ERR_CODE_DATA_6 -> "EEPROM校验失败"
        public const string ALARM_ERR_CODE_DATA_6 = "EEPROM校验失败";

        /// ALARM_ERR_CODE_DATA_7 -> "出气口温度传感器故障"
        public const string ALARM_ERR_CODE_DATA_7 = "出气口温度传感器故障";

        /// ALARM_ERR_CODE_DATA_8 -> "患者端温度传感器故障"
        public const string ALARM_ERR_CODE_DATA_8 = "患者端温度传感器故障";

        /// ALARM_ERR_CODE_DATA_9 -> ""
        public const string ALARM_ERR_CODE_DATA_9 = "";

        /// ALARM_ERR_CODE_DATA_10 -> ""
        public const string ALARM_ERR_CODE_DATA_10 = "";

        /// ALARM_ERR_CODE_DATA_11 -> ""
        public const string ALARM_ERR_CODE_DATA_11 = "";

        /// ALARM_ERR_CODE_DATA_12 -> ""
        public const string ALARM_ERR_CODE_DATA_12 = "";

        /// ALARM_ERR_CODE_DATA_13 -> ""
        public const string ALARM_ERR_CODE_DATA_13 = "";

        /// ALARM_ERR_CODE_DATA_14 -> ""
        public const string ALARM_ERR_CODE_DATA_14 = "";

        /// ALARM_ERR_CODE_DATA_15 -> ""
        public const string ALARM_ERR_CODE_DATA_15 = "";

        /// ALARM_ERR_CODE_DATA_16 -> ""
        public const string ALARM_ERR_CODE_DATA_16 = "";

        /// ALARM_ERR_CODE_DATA_17 -> ""
        public const string ALARM_ERR_CODE_DATA_17 = "";

        /// ALARM_ERR_CODE_DATA_18 -> ""
        public const string ALARM_ERR_CODE_DATA_18 = "";

        /// ALARM_ERR_CODE_DATA_19 -> ""
        public const string ALARM_ERR_CODE_DATA_19 = "";

        /// ALARM_ERR_CODE_DATA_20 -> "人体端超温"
        public const string ALARM_ERR_CODE_DATA_20 = "人体端超温";

        /// ALARM_ERR_CODE_DATA_21 -> "缺水"
        public const string ALARM_ERR_CODE_DATA_21 = "缺水";

        /// ALARM_ERR_CODE_DATA_22 -> "加热线脱落"
        public const string ALARM_ERR_CODE_DATA_22 = "加热线脱落";

        /// ALARM_ERR_CODE_DATA_23 -> "探头脱落"
        public const string ALARM_ERR_CODE_DATA_23 = "探头脱落";

        /// ALARM_ERR_CODE_DATA_24 -> "高氧浓度"
        public const string ALARM_ERR_CODE_DATA_24 = "高氧浓度";

        /// ALARM_ERR_CODE_DATA_25 -> "低氧浓度"
        public const string ALARM_ERR_CODE_DATA_25 = "低氧浓度";

        /// ALARM_ERR_CODE_DATA_26 -> "泄露"
        public const string ALARM_ERR_CODE_DATA_26 = "泄露";

        /// ALARM_ERR_CODE_DATA_27 -> "堵塞"
        public const string ALARM_ERR_CODE_DATA_27 = "堵塞";

        /// ALARM_ERR_CODE_DATA_28 -> "达不到流量"
        public const string ALARM_ERR_CODE_DATA_28 = "达不到流量";

        /// ALARM_ERR_CODE_DATA_29 -> "达不到温度"
        public const string ALARM_ERR_CODE_DATA_29 = "达不到温度";

        /// ALARM_ERR_CODE_DATA_30 -> "湿化罐未安装"
        public const string ALARM_ERR_CODE_DATA_30 = "湿化罐未安装";

        /// ALARM_ERR_CODE_DATA_31 -> "运行中电源断开"
        public const string ALARM_ERR_CODE_DATA_31 = "运行中电源断开";

        /// ALARM_ERR_CODE_DATA_32 -> "运行中SD卡拔出"
        public const string ALARM_ERR_CODE_DATA_32 = "运行中SD卡拔出";
    }

    public enum LANGUAGE
    {

        CHINA,

        ENGLISH,
        

        JAPAN,
    }

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Ansi)]
    public struct ALARM_INFO_HEAD
    {

        /// char[16]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 16)]
        public string ALARM_FLAG;

        /// char[16]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 16)]
        public string MACHINETYPE;

        /// char[16]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 16)]
        public string SN;

        /// char[16]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 16)]
        public string SOFTWAR_VER;

        /// char[16]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 16)]
        public string RESERVE_0;

        /// char[16]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 16)]
        public string RESERVE_1;

        /// char[16]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 16)]
        public string RESERVE_2;

        /// char[16]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 16)]
        public string RESERVE_3;

        /// char[16]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 16)]
        public string RESERVE_4;

        /// char[16]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 16)]
        public string RESERVE_5;

        /// char[16]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 16)]
        public string RESERVE_6;

        /// char[16]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 16)]
        public string RESERVE_7;

        /// char[16]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 16)]
        public string RESERVE_8;

        /// char[16]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 16)]
        public string RESERVE_9;

        /// char[16]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 16)]
        public string RESERVE_10;

        /// char[16]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 16)]
        public string ALARM_NUM;
    }

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct ALARM_INFO_MESSAGE
    {

        /// char
        public byte YEAR1;

        /// char
        public byte YEAR2;

        /// char
        public byte MONTH;

        /// char
        public byte DAY;

        /// char
        public byte HOUR;

        /// char
        public byte MINUTE;

        /// char
        public byte SECOND;

        /// char
        public byte RUNNIN_MODE;

        /// char
        public byte ALARM_CODE;

        /// char
        public byte ALARM_DATA_L;

        /// char
        public byte ALARM_DATA_H;

        /// char
        public byte RESERVE_0;

        /// char
        public byte RESERVE_1;

        /// char
        public byte RESERVE_2;

        /// char
        public byte CHECKSUM_L;

        /// char
        public byte CHECHSUM_H;
    }

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Ansi)]
    public struct WORK_INFO_HEAD
    {

        /// char[]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 64)]
        public string WORK_FLAG;

        /// char[]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 64)]
        public string MACHINETYPE;

        /// char[]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 64)]
        public string SN;

        /// char[]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 64)]
        public string SOFTWAR_VER;

        /// char[]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 64)]
        public string RESERVE_0;

        /// char[]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 64)]
        public string RESERVE_1;

        /// char[]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 64)]
        public string RESERVE_2;

        /// char[]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 64)]
        public string RESERVE_3;

        /// char[]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 64)]
        public string RESERVE_4;

        /// char[]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 64)]
        public string RESERVE_5;

        /// char[]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 64)]
        public string RESERVE_6;

        /// char[]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 64)]
        public string RESERVE_7;

        /// char[]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 64)]
        public string RESERVE_8;

        /// char[]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 64)]
        public string RESERVE_9;

        /// char[]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 64)]
        public string RESERVE_10;

        /// char[]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 64)]
        public string WORKDATA_NUM;
    }

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct WORK_INFO_MESSAGE
    {

        /// char
        public byte YEAR1;

        /// char
        public byte YEAR2;

        /// char
        public byte MONTH;

        /// char
        public byte DAY;

        /// char
        public byte HOUR;

        /// char
        public byte MINUTE;

        /// char
        public byte SECOND;

        /// char
        public byte SET_MODE;

        /// char
        public byte SET_TEMP;

        /// char
        public byte SET_FLOW;

        /// char
        public byte SET_HIGH_OXYGEN_ALARM;

        /// char
        public byte SET_LOW_OXYGEN_ALARM;

        /// char
        public byte SET_ATOMIZATION_LEVEL;

        /// char
        public byte SET_ATOMIZATION_TIME;

        /// char
        public byte SET_ADULT_OR_CHILDE;

        /// char
        public byte DATA_PATIENT_TEMP;

        /// char
        public byte DATA_AIR_OUTLET_TEMP;

        /// char
        public byte DATA_HEATING_PLATE_TMP;

        /// char
        public byte DATA_ENVIRONMENT_TMP;

        /// char
        public byte DATA_DRIVERBOARD_TMP;

        /// char
        public byte DATA_FLOW;

        /// char
        public byte DATA_OXYGEN_CONCENTRATION;

        /// char
        public byte DATA_AIR_PRESSURE;

        /// char
        public byte DATA_LOOP_TYPE;

        /// char
        public byte DATA_FAULT_STATUS_1;

        /// char
        public byte DATA_FAULT_STATUS_2;

        /// char
        public byte DATA_ATOMIZ_DACVALUE_L;

        /// char
        public byte DATA_ATOMIZ_DACVALUE_H;

        /// char
        public byte DATA_ATOMIZ_ADCVALUE_L;

        /// char
        public byte DATA_ATOMIZ_ADCVALUE_H;

        /// char
        public byte DATA_LOOP_HEATING_PWM_L;

        /// char
        public byte DATA_LOOP_HEATING_PWM_H;

        /// char
        public byte DATA_LOOP_HEATING_ADC_L;

        /// char
        public byte DATA_LOOP_HEATING_ADC_H;

        /// char
        public byte DATA_HEATING_PLATE_PWM_L;

        /// char
        public byte DATA_HEATING_PLATE_PWM_H;

        /// char
        public byte DATA_HEATING_PLATE_ADC_L;

        /// char
        public byte DATA_HEATING_PLATE_ADC_H;

        /// char
        public byte DATA_MAIN_MOTOR_DRIVER_L;

        /// char
        public byte DATA_MAIN_MOTOR_DRIVER_H;

        /// char
        public byte DATA_MAIN_MOTOR_SPEED_L;

        /// char
        public byte DATA_MAIN_MOTOR_SPEED_H;

        /// char
        public byte DATA_PRESS_SENSOR_ADC_L;

        /// char
        public byte DATA_PRESS_SENSOR_ADC_H;

        /// char
        public byte DATA_WATERLEVEL_SENSOR_HADC_L;

        /// char
        public byte DATA_WATERLEVEL_SENSOR_HADC_H;

        /// char
        public byte DATA_WATERLEVEL_SENSOR_LADC_L;

        /// char
        public byte DATA_WATERLEVEL_SENSOR_LADC_H;

        /// char
        public byte DATA_FAN_DRIVER_L;

        /// char
        public byte DATA_FAN_DRIVER_H;

        /// char
        public byte DATA_FAN_SPEED_L;

        /// char
        public byte DATA_FAN_SPEED_H;

        /// char
        public byte DATA_MAIN_MOTOR_TMP_ADC_L;   //  2018/7/20补上遗漏的

        /// char
        public byte DATA_MAIN_MOTOR_TMP_ADC_H;

        /// char
        public byte DATA_DEWPOINT_TMP;  //  2018/7/20新增露点温度

        /// char
        public byte RESERVE_0;

        /// char
        public byte RESERVE_1;

        /// char
        public byte RESERVE_2;

        /// char
        public byte RESERVE_3;

        /// char
        public byte RESERVE_4;

        /// char
        public byte RESERVE_5;

        /// char
        public byte RESERVE_6;

        /// char
        public byte CHECKSUM_L;

        /// char
        public byte CHECKSUM_H;
    }




}
