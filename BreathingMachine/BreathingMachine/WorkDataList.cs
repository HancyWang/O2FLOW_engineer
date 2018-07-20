using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreathingMachine
{
    class WorkDataList
    {
        //public static bool m_firstPageclicked;
        //public static bool m_endPageclicked;
        public static int m_nPageSize;//一页有多少行
        public static int m_nCurrentPage;//当前页
        public static int m_nCount;//一共有多少行
        public static int m_nPageCount;//一共有多少页
        public static List<WorkData> m_WorkData_List;
        public static List<WorkData_Basic> m_WorkData_Basic_List;

        public static void InitWorkDataList(DateTime tmLow,DateTime tmHigh)
        {
            DateTime tmBegin = new DateTime(tmLow.Year, tmLow.Month, tmLow.Day, 0, 0, 0);
            DateTime tmEnd = new DateTime(tmHigh.Year, tmHigh.Month, tmHigh.Day, 23, 59, 59);
            LanguageMngr lang = new LanguageMngr();
            if(DataMngr.m_advanceMode)
            {
                m_WorkData_List = new List<WorkData>();
                #region
                int i = 1;
                foreach (KeyValuePair<WORK_INFO_HEAD, List<WORK_INFO_MESSAGE>> kv in FileMngr.m_workHead_Msg_Map)
                {
                    //if (kv.Value.Count == 0)
                    //{
                    //    return;
                    //}
                    //先判断,减少不必要的foreach (var workDataMsg in list)
                    var list = kv.Value;
                    WORK_INFO_MESSAGE msg = list[0];
                    DateTime msgTm = new DateTime(100 * Convert.ToInt32(msg.YEAR1) + Convert.ToInt32(msg.YEAR2),
                                                    Convert.ToInt32(msg.MONTH), Convert.ToInt32(msg.DAY), 23, 59, 59);
                    if (msgTm < tmBegin || msgTm > tmEnd)
                    {
                        continue;
                    }

                    #region

                    foreach (var workDataMsg in list)
                    {
                        DateTime tmFromMsg = new DateTime(100 * Convert.ToInt32(workDataMsg.YEAR1) + Convert.ToInt32(workDataMsg.YEAR2),
                                                            Convert.ToInt32(workDataMsg.MONTH),
                                                            Convert.ToInt32(workDataMsg.DAY),
                                                            Convert.ToInt32(workDataMsg.HOUR),
                                                            Convert.ToInt32(workDataMsg.MINUTE),
                                                            Convert.ToInt32(workDataMsg.SECOND));
                        //if(tmFromMsg>=tmBegin&&tmFromMsg<=tmEnd)
                        {
                            //解析故障状态位,一共12位
                            int[] faultStates = new int[12];
                            #region
                            byte bt1 = workDataMsg.DATA_FAULT_STATUS_1;
                            byte bt2 = workDataMsg.DATA_FAULT_STATUS_2;

                            for (int j = 0; j < 8; j++)
                            {
                                if (Convert.ToInt32(bt1) % 2 == 1)
                                {
                                    faultStates[j] = 1;
                                }
                                else
                                {
                                    faultStates[j] = 0;
                                }
                                bt1 = (byte)(bt1 >> 1);
                            }

                            for (int j = 0; j < 3; j++)
                            {
                                if (Convert.ToInt32(bt2) % 2 == 1)
                                {
                                    faultStates[j + 8] = 1;
                                }
                                else
                                {
                                    faultStates[j + 8] = 0;
                                }
                                bt1 = (byte)(bt2 >> 1);
                            }
                            #endregion

                            //添加到链表中

                            WorkData wd = new WorkData(); //实例化一个WorkData
                            //填充信息,这里要改，要根据advanceMode和用户模式
                            #region
                            wd.No = i.ToString();
                            wd.tm = tmFromMsg.ToString("yyyy-MM-dd HH:mm:ss");
                            wd.set_mode = Convert.ToBoolean(workDataMsg.SET_MODE) ? lang.atomization() : lang.humidification();

                            wd.set_adault_or_child = Convert.ToBoolean(workDataMsg.SET_ADULT_OR_CHILDE) ? lang.child() : lang.adault();
                            wd.data_patient_tmp = Convert.ToString(workDataMsg.DATA_PATIENT_TEMP);
                            wd.data_air_outlet_tmp = Convert.ToString(workDataMsg.DATA_AIR_OUTLET_TEMP);

                            wd.data_flow = Convert.ToString(workDataMsg.DATA_FLOW);
                            wd.data_oxy_concentration = Convert.ToString(workDataMsg.DATA_OXYGEN_CONCENTRATION);

                            //工程师模式下的扩展信息
                            #region
                            wd.set_tmp = Convert.ToString(workDataMsg.SET_TEMP);
                            wd.set_flow = Convert.ToString(workDataMsg.SET_FLOW);
                            wd.set_high_oxy_alarm = Convert.ToString(workDataMsg.SET_HIGH_OXYGEN_ALARM);
                            wd.set_low_oxy_alrm = Convert.ToString(workDataMsg.SET_LOW_OXYGEN_ALARM);
                            wd.set_atomiz_level = Convert.ToString(workDataMsg.SET_ATOMIZATION_LEVEL);
                            wd.set_atomiz_time = Convert.ToString(workDataMsg.SET_ATOMIZATION_TIME);

                            wd.data_heating_plate_tmp = Convert.ToString(workDataMsg.DATA_HEATING_PLATE_ADC_H);
                            wd.data_env_tmp = Convert.ToString(workDataMsg.DATA_ENVIRONMENT_TMP);
                            wd.data_driveboard_tmp = Convert.ToString(workDataMsg.DATA_DRIVERBOARD_TMP);
                            wd.data_air_pressure = Convert.ToString(workDataMsg.DATA_AIR_PRESSURE);
                            wd.data_loop_type = Convert.ToString(workDataMsg.DATA_LOOP_TYPE);

                            wd.data_faultstates_0 = Convert.ToBoolean(faultStates[0]) ? "yes" : "no";
                            wd.data_faultstates_1 = Convert.ToBoolean(faultStates[1]) ? "yes" : "no";
                            wd.data_faultstates_2 = Convert.ToBoolean(faultStates[2]) ? "yes" : "no";
                            wd.data_faultstates_3 = Convert.ToBoolean(faultStates[3]) ? "yes" : "no";
                            wd.data_faultstates_4 = Convert.ToBoolean(faultStates[4]) ? "yes" : "no";
                            wd.data_faultstates_5 = Convert.ToBoolean(faultStates[5]) ? "yes" : "no";
                            wd.data_faultstates_6 = Convert.ToBoolean(faultStates[6]) ? "yes" : "no";
                            wd.data_faultstates_7 = Convert.ToBoolean(faultStates[7]) ? "yes" : "no";
                            wd.data_faultstates_8 = Convert.ToBoolean(faultStates[8]) ? "yes" : "no";
                            wd.data_faultstates_9 = Convert.ToBoolean(faultStates[9]) ? "yes" : "no";
                            wd.data_faultstates_10 = Convert.ToBoolean(faultStates[10]) ? "yes" : "no";
                            wd.data_faultstates_11 = Convert.ToBoolean(faultStates[11]) ? "yes" : "no";
                            wd.data_atmoz_DAC_L = Convert.ToString(workDataMsg.DATA_ATOMIZ_DACVALUE_L);
                            wd.data_atmoz_DAC_H = Convert.ToString(workDataMsg.DATA_ATOMIZ_DACVALUE_H);
                            wd.data_atmoz_ADC_L = Convert.ToString(workDataMsg.DATA_ATOMIZ_ADCVALUE_L);
                            wd.data_atmoz_ADC_H = Convert.ToString(workDataMsg.DATA_ATOMIZ_ADCVALUE_H);
                            wd.data_loop_heating_PWM_L = Convert.ToString(workDataMsg.DATA_LOOP_HEATING_PWM_L);
                            wd.data_loop_heating_PWM_H = Convert.ToString(workDataMsg.DATA_LOOP_HEATING_PWM_H);
                            wd.data_loop_heating_ADC_L = Convert.ToString(workDataMsg.DATA_LOOP_HEATING_ADC_L);
                            wd.data_loop_heating_ADC_H = Convert.ToString(workDataMsg.DATA_LOOP_HEATING_ADC_H);
                            wd.data_loop_heating_plate_PWM_L = Convert.ToString(workDataMsg.DATA_HEATING_PLATE_PWM_L);
                            wd.data_loop_heating_plate_PWM_H = Convert.ToString(workDataMsg.DATA_HEATING_PLATE_PWM_H);
                            wd.data_loop_heating_plate_ADC_L = Convert.ToString(workDataMsg.DATA_HEATING_PLATE_ADC_L);
                            wd.data_loop_heating_plate_ADC_H = Convert.ToString(workDataMsg.DATA_HEATING_PLATE_ADC_H);
                            wd.data_main_motor_drive_L = Convert.ToString(workDataMsg.DATA_MAIN_MOTOR_DRIVER_L);
                            wd.data_main_motor_drive_H = Convert.ToString(workDataMsg.DATA_MAIN_MOTOR_DRIVER_H);
                            wd.data_main_motor_speed_L = Convert.ToString(workDataMsg.DATA_MAIN_MOTOR_SPEED_L);
                            wd.data_main_motor_speed_H = Convert.ToString(workDataMsg.DATA_MAIN_MOTOR_SPEED_H);
                            wd.data_press_sensor_ADC_L = Convert.ToString(workDataMsg.DATA_PRESS_SENSOR_ADC_L);
                            wd.data_press_sensor_ADC_H = Convert.ToString(workDataMsg.DATA_PRESS_SENSOR_ADC_H);
                            wd.data_waterlevel_sensor_HADC_L = Convert.ToString(workDataMsg.DATA_WATERLEVEL_SENSOR_HADC_L);
                            wd.data_waterlevel_sensor_HADC_H = Convert.ToString(workDataMsg.DATA_WATERLEVEL_SENSOR_HADC_H);
                            wd.data_waterlevel_sensor_LADC_L = Convert.ToString(workDataMsg.DATA_WATERLEVEL_SENSOR_LADC_L);
                            wd.data_waterlevel_sensor_LADC_H = Convert.ToString(workDataMsg.DATA_WATERLEVEL_SENSOR_LADC_H);
                            wd.data_fan_driver_L = Convert.ToString(workDataMsg.DATA_FAN_DRIVER_L);
                            wd.data_fan_driver_H = Convert.ToString(workDataMsg.DATA_FAN_DRIVER_H);
                            wd.data_fan_speed_L = Convert.ToString(workDataMsg.DATA_FAN_SPEED_L);
                            wd.data_fan_speed_H = Convert.ToString(workDataMsg.DATA_FAN_SPEED_H);
                            #endregion

                            #endregion

                            ////debug,使用构造函数,速度还是不够快
                            #region
                            //WorkData wd = new WorkData(i.ToString(),
                            //                        tmFromMsg.ToString("yyyy-MM-dd HH:mm:ss"),
                            //                        Convert.ToBoolean(workDataMsg.SET_MODE) ? "雾化" : "湿化",
                            //                        Convert.ToString(workDataMsg.SET_TEMP),
                            //                        Convert.ToString(workDataMsg.SET_FLOW),
                            //                        Convert.ToString(workDataMsg.SET_HIGH_OXYGEN_ALARM),
                            //                        Convert.ToString(workDataMsg.SET_LOW_OXYGEN_ALARM),
                            //                        Convert.ToString(workDataMsg.SET_ATOMIZATION_LEVEL),
                            //                        Convert.ToString(workDataMsg.SET_ATOMIZATION_TIME),
                            //                        Convert.ToBoolean(workDataMsg.SET_ADULT_OR_CHILDE) ? "儿童" : "成人",
                            //                        Convert.ToString(workDataMsg.DATA_PATIENT_TEMP),
                            //                        Convert.ToString(workDataMsg.DATA_AIR_OUTLET_TEMP),
                            //                        Convert.ToString(workDataMsg.DATA_HEATING_PLATE_ADC_H),
                            //                        Convert.ToString(workDataMsg.DATA_ENVIRONMENT_TMP),
                            //                        Convert.ToString(workDataMsg.DATA_DRIVERBOARD_TMP),
                            //                        Convert.ToString(workDataMsg.DATA_FLOW),
                            //                        Convert.ToString(workDataMsg.DATA_OXYGEN_CONCENTRATION),
                            //                        Convert.ToString(workDataMsg.DATA_AIR_PRESSURE),
                            //                        Convert.ToString(workDataMsg.DATA_LOOP_TYPE),
                            //                        Convert.ToBoolean(faultStates[0]) ? "yes" : "no",
                            //                        Convert.ToBoolean(faultStates[1]) ? "yes" : "no",
                            //                        Convert.ToBoolean(faultStates[2]) ? "yes" : "no",
                            //                        Convert.ToBoolean(faultStates[3]) ? "yes" : "no",
                            //                        Convert.ToBoolean(faultStates[4]) ? "yes" : "no",
                            //                        Convert.ToBoolean(faultStates[5]) ? "yes" : "no",
                            //                        Convert.ToBoolean(faultStates[6]) ? "yes" : "no",
                            //                        Convert.ToBoolean(faultStates[7]) ? "yes" : "no",
                            //                        Convert.ToBoolean(faultStates[8]) ? "yes" : "no",
                            //                        Convert.ToBoolean(faultStates[9]) ? "yes" : "no",
                            //                        Convert.ToBoolean(faultStates[10]) ? "yes" : "no",
                            //                        Convert.ToBoolean(faultStates[11]) ? "yes" : "no",
                            //                        Convert.ToString(workDataMsg.DATA_ATOMIZ_DACVALUE_L),
                            //                        Convert.ToString(workDataMsg.DATA_ATOMIZ_DACVALUE_H),
                            //                        Convert.ToString(workDataMsg.DATA_ATOMIZ_ADCVALUE_L),
                            //                        Convert.ToString(workDataMsg.DATA_ATOMIZ_ADCVALUE_H),
                            //                        Convert.ToString(workDataMsg.DATA_LOOP_HEATING_PWM_L),
                            //                        Convert.ToString(workDataMsg.DATA_LOOP_HEATING_PWM_H),
                            //                        Convert.ToString(workDataMsg.DATA_LOOP_HEATING_ADC_L),
                            //                        Convert.ToString(workDataMsg.DATA_LOOP_HEATING_ADC_H),
                            //                        Convert.ToString(workDataMsg.DATA_HEATING_PLATE_PWM_L),
                            //                        Convert.ToString(workDataMsg.DATA_HEATING_PLATE_PWM_H),
                            //                        Convert.ToString(workDataMsg.DATA_HEATING_PLATE_ADC_L),
                            //                        Convert.ToString(workDataMsg.DATA_HEATING_PLATE_ADC_H),
                            //                        Convert.ToString(workDataMsg.DATA_MAIN_MOTOR_DRIVER_L),
                            //                        Convert.ToString(workDataMsg.DATA_MAIN_MOTOR_DRIVER_H),
                            //                        Convert.ToString(workDataMsg.DATA_MAIN_MOTOR_SPEED_L),
                            //                        Convert.ToString(workDataMsg.DATA_MAIN_MOTOR_SPEED_H),
                            //                        Convert.ToString(workDataMsg.DATA_PRESS_SENSOR_ADC_L),
                            //                        Convert.ToString(workDataMsg.DATA_PRESS_SENSOR_ADC_H),
                            //                        Convert.ToString(workDataMsg.DATA_WATERLEVEL_SENSOR_HADC_L),
                            //                        Convert.ToString(workDataMsg.DATA_WATERLEVEL_SENSOR_HADC_H),
                            //                        Convert.ToString(workDataMsg.DATA_WATERLEVEL_SENSOR_LADC_L),
                            //                        Convert.ToString(workDataMsg.DATA_WATERLEVEL_SENSOR_LADC_H),
                            //                        Convert.ToString(workDataMsg.DATA_FAN_DRIVER_L),
                            //                        Convert.ToString(workDataMsg.DATA_FAN_DRIVER_H),
                            //                        Convert.ToString(workDataMsg.DATA_FAN_SPEED_L),
                            //                        Convert.ToString(workDataMsg.DATA_FAN_SPEED_H));
                            #endregion

                            WorkDataList.m_WorkData_List.Add(wd);
                            //myCache1.Add(lvi);
                            i++;
                        }
                    }
                    #endregion
                }
                m_nCount = i - 1;  //一共有多少行
                m_nPageSize = 35; //每一页有多少行
                m_nPageCount = m_nCount / m_nPageSize;
                m_nPageCount += m_nCount % m_nPageSize != 0 ? 1 : 0; //一共有多少页
                m_nCurrentPage = 1;  //当前页为0
                //m_firstPageclicked = false;
                //m_endPageclicked = false;
                #endregion
            }
            else
            {
                m_WorkData_Basic_List = new List<WorkData_Basic>();
                #region
                int i = 1;
                foreach (KeyValuePair<WORK_INFO_HEAD, List<WORK_INFO_MESSAGE>> kv in FileMngr.m_workHead_Msg_Map)
                {
                    //先判断,减少不必要的foreach (var workDataMsg in list)
                    var list = kv.Value;
                    if (list.Count == 0)
                    {
                        return;
                    }
                    WORK_INFO_MESSAGE msg = list[0];
                    DateTime msgTm = new DateTime(100 * Convert.ToInt32(msg.YEAR1) + Convert.ToInt32(msg.YEAR2),
                                                    Convert.ToInt32(msg.MONTH), Convert.ToInt32(msg.DAY), 23, 59, 59);
                    if (msgTm < tmBegin || msgTm > tmEnd)
                    {
                        continue;
                    }

                    #region

                    foreach (var workDataMsg in list)
                    {
                        DateTime tmFromMsg = new DateTime(100 * Convert.ToInt32(workDataMsg.YEAR1) + Convert.ToInt32(workDataMsg.YEAR2),
                                                            Convert.ToInt32(workDataMsg.MONTH),
                                                            Convert.ToInt32(workDataMsg.DAY),
                                                            Convert.ToInt32(workDataMsg.HOUR),
                                                            Convert.ToInt32(workDataMsg.MINUTE),
                                                            Convert.ToInt32(workDataMsg.SECOND));
                        //if(tmFromMsg>=tmBegin&&tmFromMsg<=tmEnd)
                        {
                            //添加到链表中
                            //WorkData_Basic wd = new WorkData_Basic(); //实例化一个WorkData_Basic
                            //#region
                            //wd.No = i.ToString();
                            //wd.tm = tmFromMsg.ToString("yyyy-MM-dd HH:mm:ss");
                            //wd.set_mode = Convert.ToBoolean(workDataMsg.SET_MODE) ? "雾化" : "湿化";
                            //wd.set_adault_or_child = Convert.ToBoolean(workDataMsg.SET_ADULT_OR_CHILDE) ? "儿童" : "成人";
                            //wd.data_patient_tmp = Convert.ToString(workDataMsg.DATA_PATIENT_TEMP);
                            //wd.data_air_outlet_tmp = Convert.ToString(workDataMsg.DATA_AIR_OUTLET_TEMP);
                            //wd.data_flow = Convert.ToString(workDataMsg.DATA_FLOW);
                            //wd.data_oxy_concentration = Convert.ToString(workDataMsg.DATA_OXYGEN_CONCENTRATION);
                            //#endregion


                            WorkData_Basic wd = new WorkData_Basic(i.ToString(),
                                                        tmFromMsg.ToString("yyyy-MM-dd HH:mm:ss"),
                                                        Convert.ToBoolean(workDataMsg.SET_MODE) ? lang.atomization() : lang.humidification(),
                                                        Convert.ToBoolean(workDataMsg.SET_ADULT_OR_CHILDE) ? lang.child() : lang.adault(),
                                                        Convert.ToString(workDataMsg.DATA_PATIENT_TEMP),
                                                        Convert.ToString(workDataMsg.DATA_AIR_OUTLET_TEMP),
                                                        Convert.ToString(workDataMsg.DATA_FLOW),
                                                        Convert.ToString(workDataMsg.DATA_OXYGEN_CONCENTRATION),
                                                        Convert.ToString(workDataMsg.DATA_DEWPOINT_TMP));

                            WorkDataList.m_WorkData_Basic_List.Add(wd);
                            i++;
                        }
                    }
                    #endregion
                }
                m_nCount = i - 1;  //一共有多少行
                m_nPageSize = 35; //每一页有多少行
                m_nPageCount = m_nCount / m_nPageSize;
                m_nPageCount += m_nCount % m_nPageSize != 0 ? 1 : 0; //一共有多少页
                m_nCurrentPage = 1;  //当前页为0
                //m_firstPageclicked = false;
                //m_endPageclicked = false;
                #endregion
            }   
        }   

    }
}
