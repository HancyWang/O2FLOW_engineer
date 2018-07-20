using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;

//创建这个类用来将FileMngr里的链表数据转换成各种DataTable
namespace BreathingMachine
{
    class DataMngr
    {
        public static DateTime m_chart_selected_date;
        public static string m_App_Version = "1.1.0";
        public static PATIENT_INFO m_old_PatientInfo=new PATIENT_INFO();
        public static double m_DateTimePicker_Range_Limit = 60;//默认2个月的时间
        public static double m_UsageChart_DateRange_Limit = 60;//默认2个月的时间
        public static List<DETAIL_CHART_INFO> m_listInfo;
        public static CHART_SIZE m_chartSize;
        public static bool m_bPatientInfo_Geted;
        public static bool m_bDateTimePicker_ValueChanged;
        public static bool m_advanceMode;
        public static int m_machineType;
        private DateTime m_tmBegin;
        private DateTime m_tmEnd;
        public static List<DateTime> m_usageTable_xAxis_list;
        public static List<DateTime> m_usageTable_beginTime_list;
        public static List<DateTime> m_usageTable_endTime_list;
        //public DataTable m_usageTable;
        
        public static void GetMachineTpye()
        {
            byte[] head = FileMngr.GetData(FileMngr.m_lastWorkHead);
            var machineType = DataMngr.GetMachineType(head, 1);
            if(machineType=="VUN001")
            {
                m_machineType = 1;
            }
            else if (machineType=="VUN002")
            {
                m_machineType = 2;
            }
            else
            {
                //do nothing
            }
        }

        public static void GetUsageInfo()
        {
            //提取信息到DataMngr中的List<DateTime>链表中
            DataMngr.m_usageTable_xAxis_list = new List<DateTime>();
            DataMngr.m_usageTable_beginTime_list = new List<DateTime>();
            DataMngr.m_usageTable_endTime_list = new List<DateTime>();
            #region
            foreach (KeyValuePair<WORK_INFO_HEAD, List<WORK_INFO_MESSAGE>> kv in FileMngr.m_workHead_Msg_Map)
            {
                #region
                var list = kv.Value;
                //int len = list.Count;
                WORK_INFO_MESSAGE msg = list[0];//取链表的第一个消息，从而获得时间

                DateTime xAxis = new DateTime(100 * Convert.ToInt32(list[0].YEAR1) + Convert.ToInt32(list[0].YEAR2), Convert.ToInt32(list[0].MONTH), Convert.ToInt32(list[0].DAY), 0, 0, 0);

                DateTime tmPrev = new DateTime(100 * Convert.ToInt32(msg.YEAR1) + Convert.ToInt32(msg.YEAR2), Convert.ToInt32(msg.MONTH), Convert.ToInt32(msg.DAY), 0, 0, 0);

                DateTime tmEndOfDay = new DateTime(100 * Convert.ToInt32(msg.YEAR1) + Convert.ToInt32(msg.YEAR2), Convert.ToInt32(msg.MONTH), Convert.ToInt32(msg.DAY), 23, 59, 59);


                #endregion

                #region
                //bool bChanged = true;
                int i = 0;
                foreach (var workDataMsg in list)
                {
                    i++;

                    TimeSpan span;
                    DateTime tmFromMsg = new DateTime(100 * Convert.ToInt32(workDataMsg.YEAR1) + Convert.ToInt32(workDataMsg.YEAR2),
                                                        Convert.ToInt32(workDataMsg.MONTH),
                                                        Convert.ToInt32(workDataMsg.DAY),
                                                        Convert.ToInt32(workDataMsg.HOUR),
                                                        Convert.ToInt32(workDataMsg.MINUTE),
                                                        Convert.ToInt32(workDataMsg.SECOND));

                    span = tmFromMsg - tmPrev;
                    if(i==1)
                    {
                        if(span.TotalMinutes<=2.0)
                        {
                            DataMngr.m_usageTable_xAxis_list.Add(xAxis);
                            DataMngr.m_usageTable_beginTime_list.Add(tmFromMsg);
                        }
                    }

                    if (span.TotalMinutes > 2.0)
                    {
                        DateTime tmp = new DateTime(100 * Convert.ToInt32(list[0].YEAR1) + Convert.ToInt32(list[0].YEAR2), Convert.ToInt32(list[0].MONTH), Convert.ToInt32(list[0].DAY), 0, 0, 0);
                        if (tmPrev == tmp)  //这个只判断一次，就是最开始的时候
                        {
                            DataMngr.m_usageTable_xAxis_list.Add(xAxis);
                            DataMngr.m_usageTable_beginTime_list.Add(tmFromMsg);
                        }
                        else
                        {
                            DataMngr.m_usageTable_endTime_list.Add(tmPrev);
                            DataMngr.m_usageTable_xAxis_list.Add(xAxis);
                            DataMngr.m_usageTable_beginTime_list.Add(tmFromMsg);
                        }
                    }

                    if (i == list.Count)
                    {
                        tmPrev = tmFromMsg;
                        DataMngr.m_usageTable_endTime_list.Add(tmPrev);
                        break;
                    }
                    tmPrev = tmFromMsg;
                }
                #endregion
            }
            #endregion
  
        }

        //先试一个
        public static string GetRunningMode(byte bt)
        {
            if(FileMngr.m_dirPath==null)
            {
                return "";
            }
            string str = LanguageMngr.humidy_or_atomiz(bt);
            //if(LanguageMngr.m_language==LANGUAGE.CHINA)
            //{
            //    str = Convert.ToBoolean(bt) ? "雾化" : "湿化";
            //}
            //else if(LanguageMngr.m_language==LANGUAGE.ENGLISH)
            //{
            //    str = Convert.ToBoolean(bt) ? "Atomization" : "Humidifying";
            //}
            //else
            //{
            //    //等有其他语言再添加
            //}
            return str;
        }

        public static string GetAdaultChildSetting(byte bt)
        {
            if (FileMngr.m_dirPath == null)
            {
                return "";
            }
            string str = LanguageMngr.adault_or_child(bt);
            //if (LanguageMngr.m_language == LANGUAGE.CHINA)
            //{
            //    str = Convert.ToBoolean(bt) ? "儿童" : "成人";
            //}
            //else if (LanguageMngr.m_language == LANGUAGE.ENGLISH)
            //{
            //    str = Convert.ToBoolean(bt) ? "Child" : "Adault";
            //}
            //else
            //{
            //    //等有其他语言再添加
            //}
            return str;
        }

        public static string GetSetting2Str(byte bt)
        {
            if (FileMngr.m_dirPath == null)
            {
                return "";
            }
            return Convert.ToString(Convert.ToInt32(bt));
        }

        public static string GetMachineType(byte[] bt, int NoOfField)
        {
            //识别码  
            //机型号  解析这个字段，先要偏移64字节,也就是FileMngr.WORKDATA_MSG_LEN
            //SN
            //软件版本
            int begin;
            if (FileMngr.m_dirPath == null)
            {
                return "";
            }
            if (FileMngr.m_workFileNameList.Count != 0)
            {
                begin = NoOfField * FileMngr.WORKDATA_MSG_LEN;
            }
            else
            {
                begin = NoOfField * FileMngr.ALARM_MSG_LEN;
            }
                
            int len = Convert.ToInt32(bt[begin]);
            //len -= 48;  //第一个byte为二进制的36，十进制的54，减掉48才为6

            string tmp = "";
            for (int i = 1; i <= len; i++)
            {
                tmp += Convert.ToString((char)Convert.ToInt32(bt[begin + i]));
            }
            return tmp;
            
            
        }

        public static string GetSN(byte[] bt,int NoOfField)
        {

            //识别码  
            //机型号  
            //SN        解析这个字段，先要偏移2*64字节,也就是FileMngr.WORKDATA_MSG_LEN
            //软件版本
            return GetMachineType(bt, NoOfField);
        }

        public static string GetSoftwareVer(byte[] bt, int NoOfField)
        {
            //识别码  
            //机型号  
            //SN        
            //软件版本      解析这个字段，先要偏移2*64字节,也就是FileMngr.WORKDATA_MSG_LEN
            int begin;
            if (FileMngr.m_dirPath == null)
            {
                return "";
            }
            //int begin = NoOfField * FileMngr.WORKDATA_MSG_LEN;
            if (FileMngr.m_workFileNameList.Count != 0)
            {
                begin = NoOfField * FileMngr.WORKDATA_MSG_LEN;
            }
            else
            {
                begin = NoOfField * FileMngr.ALARM_MSG_LEN;
            }
            int len = Convert.ToInt32(bt[begin]);
            //len -= 48;

            string tmp = "";
            for (int i = 1; i <= len; i++)
            {
                //tmp += Convert.ToString((char)Convert.ToInt32(bt[begin + i]));
                tmp += Convert.ToString(Convert.ToInt32(bt[begin + i]));
                if(i==len)
                {
                    break;
                }
                tmp += ".";
            }
            return tmp;
        }

        public DataMngr(DateTime tmBegin,DateTime tmEnd)
        {
            m_tmBegin = tmBegin;
            m_tmEnd = tmEnd;
        }
        
    }
}
