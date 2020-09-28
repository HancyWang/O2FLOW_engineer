using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics.Contracts;
using System.Windows.Forms.DataVisualization.Charting;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace BreathingMachine
{
    //将工作信息列表中的一行数据，封装成一个结构体
    
    public partial class Form1 : Form
    {
        public static string g_username;
        public static string g_password;
        public bool g_bEngineerMode;
        private List<ListViewItem> myCache;
        //private List<ListViewItem> myCache1;
        public Form1()
        {
            InitializeComponent();
            //myCache = new List<ListViewItem>();
            //myCache1 = new List<ListViewItem>();
        }

        enum DURATION
        {
            THREE_DAYS=3,
            SEVEN_DAYS=7,
            FOURTEEN_DAYS=14
        }

        enum BEGIN_END
        {
            BEGIN,
            END
        };

        public void PaintUsageChart(DateTime tmBegin, DateTime tmEnd)
        {
            DateTime tmFirstDay = DateTime.FromOADate(0); //获取系统默认的第一天，1899/12/30

            DateTime tm_begin = new DateTime(tmBegin.Year, tmBegin.Month, tmBegin.Day, 0, 0, 0);
            DateTime tm_end = new DateTime(tmEnd.Year, tmEnd.Month, tmEnd.Day, 0, 0, 0);

            double day_min = (tm_begin - tmFirstDay).TotalDays /*- 1*/; //x轴上的最小值 ,-1表示多放前一天，为了图表显示好看
            double day_max = (tm_end - tmFirstDay).TotalDays /*+ 1*/;   //y轴上的最大值,+1表示日期在后推一天

            //加一个时间范围限制，就显示2个月的数据
            if (day_max - day_min >= DataMngr.m_UsageChart_DateRange_Limit)
            {
                day_min = day_max - DataMngr.m_UsageChart_DateRange_Limit;
            }

            //var duration = (tmEnd - tmBegin).TotalDays;
            //画图
            this.chart_workData.Series.Clear();//清除所有图
            this.chart_workData.ChartAreas.Clear();
            this.chart_workData.Titles.Clear();

            Series usage = new Series("usage");
            ChartArea chartArea_usage = new ChartArea("chartArea_usage");

            #region
            //放大缩小功能
            //chartArea_usage.AxisX.ScaleView.Zoom(day_min - 100, day_max + 100);
            //chartArea_usage.CursorX.IsUserEnabled = true;
            //chartArea_usage.CursorX.IsUserSelectionEnabled = true;
            //chartArea_usage.AxisX.ScrollBar.IsPositionedInside = true;
            //chartArea_usage.AxisX.ScrollBar.Size = 10;
            ////chartArea_usage.AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.All;
            ////chartArea_usage.AxisX.ScaleView.SmallScrollMinSize = double.NaN;
            //chartArea_usage.AxisX.ScaleView.SmallScrollMinSize = 1;
            //chartArea_usage.AxisX.ScaleView.Position = usage.Points.Count - 5;
            #endregion

            usage.ChartArea = "chartArea_usage"; //绑定

            usage.XValueType = ChartValueType.Date;  //设置X,Y轴的坐标类型
            usage.YValueType = ChartValueType.Time;

            usage.ChartType = SeriesChartType.RangeColumn;//设置图标类型

            chartArea_usage.AxisX.LabelStyle.Format = "MM-dd";
            chartArea_usage.AxisY.IsReversed = true;
            chartArea_usage.AxisX.Minimum = day_min;
            chartArea_usage.AxisX.Maximum = day_max;
            if((this.dateTimePicker_End.Value-this.dateTimePicker_Begin.Value).TotalDays>=DataMngr.m_UsageChart_DateRange_Limit)
            {
                chartArea_usage.AxisX.Interval = 2;
            }
            else
            {
                chartArea_usage.AxisX.Interval = 1;
            }

            chartArea_usage.AxisY.LabelStyle.Format = "HH:mm";
            chartArea_usage.AxisY.Minimum = 0;
            chartArea_usage.AxisY.Maximum = 1;
            chartArea_usage.AxisY.Interval = 0.125;//00:00-24:00是按0-1来的，0.125等于3个小时

            this.chart_workData.ChartAreas.Add(chartArea_usage);
            this.chart_workData.Series.Add(usage);

            #region
            //debug，图表区矩形位置
            //chartArea_usage.AxisY.Title = "usage"; //设置y轴标题
            //chartArea_usage.Position.Auto = false;  //取消自动设置，改成自己设置chartArea_usage的大小
            //chartArea_usage.Position.X = 30;
            //chartArea_usage.Position.Y = 30;
            //chartArea_usage.Position.Height = 100;
            //chartArea_usage.Position.Width = 100;
            #endregion
            this.chart_workData.Legends.Clear(); //清除chart_workData的legend

            this.chart_workData.Location = new Point(0, 0);
            LanguageMngr lang = new LanguageMngr();
            this.chart_workData.Titles.Add(lang.usageTime());
            //这里要调整
            this.chart_workData.Width = DataMngr.m_chartSize.Width;    //按照显示天数的比例来调整图表的宽度
            this.chart_workData.Height = DataMngr.m_chartSize.Height;
            this.chart_workData.BorderlineColor = Color.Gray;
            this.chart_workData.BorderlineWidth = 1;
            this.chart_workData.BorderlineDashStyle = ChartDashStyle.Solid;

            #region
            //debug
            //var location=chart_workData.Location;
            //MessageBox.Show(location.X.ToString() + "," + location.Y.ToString());
            //var scr=PointToScreen(location);
            //MessageBox.Show(scr.X.ToString() + "," + scr.Y.ToString());
            //MessageBox.Show(this.dateTimePicker_Begin.Location.X.ToString() + "," + this.dateTimePicker_Begin.Location.Y.ToString());
            //.Series["usage"].Points.DataBindXY(DataMngr.m_usageTable_xAxis_list, DataMngr.m_usageTable_beginTime_list, DataMngr.m_usageTable_endTime_list);
            #endregion
           
            usage.Points.DataBindXY(DataMngr.m_usageTable_xAxis_list, DataMngr.m_usageTable_beginTime_list, DataMngr.m_usageTable_endTime_list);
            #region
            //动态增加控件
            //for(int i=0;i<30;i++)
            //{
            //    Button bt = new Button();
            //    bt.Location = new Point(this.panel_for_charts.Location.X + 100*i, this.panel_for_charts.Location.Y + 300);
            //    //MessageBox.Show(this.panel_for_charts.Location.X.ToString(), this.panel_for_charts.Location.Y.ToString());
            //    bt.BringToFront();
            //    this.panel_for_charts.Controls.Add(bt);
            //}   
            #endregion
        }

       
        
        public void ShowDetailCharts(DateTime tmEnd,double duration)
        {
            //一共4张图；病人温度，出气口温度，流量，氧浓度
            //先规定每张图的大小

            ShowChartByType(tmEnd, duration, CHARTTYPE.PATIENT_TMP);
            ShowChartByType(tmEnd, duration, CHARTTYPE.AIR_OUTLET_TMP);
            ShowChartByType(tmEnd, duration, CHARTTYPE.FLOW);
            ShowChartByType(tmEnd, duration, CHARTTYPE.OXY_CONCENTRATION);
            ShowChartByType(tmEnd, duration, CHARTTYPE.DEWPOINT_TMP);
        }
        public static byte Filte0xFF(byte bt)
        {
            if (bt == 0xFF)
            {
                return 0x00;
            }
            else
            {
                return bt;
            }
        }

        public void ShowChartByType(DateTime tmEnd, double duration, CHARTTYPE chartType)
        {
            //生成需要信息的链表
            #region
            DateTime tmEndOfDay= new DateTime(tmEnd.Year, tmEnd.Month, tmEnd.Day, 23, 59, 59);
            DateTime tmBeginOfDay = tmEndOfDay.AddDays(0 - duration);

            if (DataMngr.m_listInfo != null || DataMngr.m_listInfo.Count == 0)
            {
                DataMngr.m_listInfo.Clear();
            }
            //DataMngr.m_listInfo = new List<DETAIL_CHART_INFO>();
            
            foreach (KeyValuePair<WORK_INFO_HEAD, List<WORK_INFO_MESSAGE>> kv in FileMngr.m_workHead_Msg_Map)
            {
                //先判断,减少不必要的foreach (var workDataMsg in list)
                var list = kv.Value;

                WORK_INFO_MESSAGE msg=list[0];
                DateTime msgTm = new DateTime(100 * Convert.ToInt32(msg.YEAR1) + Convert.ToInt32(msg.YEAR2), 
                                                Convert.ToInt32(msg.MONTH), Convert.ToInt32(msg.DAY),23,59,59);
                if ((tmEndOfDay-msgTm).TotalDays>duration)
                {
                    continue;
                }

                foreach (var workDataMsg in list)
                {
                    DateTime tmFromMsg = new DateTime(100 * Convert.ToInt32(workDataMsg.YEAR1) + Convert.ToInt32(workDataMsg.YEAR2),
                                                        Convert.ToInt32(workDataMsg.MONTH),
                                                        Convert.ToInt32(workDataMsg.DAY),
                                                        Convert.ToInt32(workDataMsg.HOUR),
                                                        Convert.ToInt32(workDataMsg.MINUTE),
                                                        Convert.ToInt32(workDataMsg.SECOND));
                    if (tmFromMsg >= tmBeginOfDay && tmFromMsg <= tmEndOfDay)
                    {
                        DETAIL_CHART_INFO info = new DETAIL_CHART_INFO(); 
                        
                        info.tm = tmFromMsg;

                        info.patient_tmp = Convert.ToInt32(Filte0xFF(workDataMsg.DATA_PATIENT_TEMP));
                        info.air_outlet_tmp = Convert.ToInt32(Filte0xFF(workDataMsg.DATA_AIR_OUTLET_TEMP));
                        info.flow = Convert.ToInt32(Filte0xFF(workDataMsg.DATA_FLOW));
                        info.oxy_concentration = Convert.ToInt32(Filte0xFF(workDataMsg.DATA_OXYGEN_CONCENTRATION));
                        info.dewpoint_tmp = Convert.ToInt32(Filte0xFF(workDataMsg.DATA_DEWPOINT_TMP));  // 2018/7/10新增

                        DataMngr.m_listInfo.Add(info);
                    }
                }
            }
            //MessageBox.Show(DataMngr.m_listInfo.Count.ToString());
            #endregion

            #region
            DateTime tmFirstDay = DateTime.FromOADate(0); //获取系统默认的第一天，1899/12/30
            //用来生成x轴坐标
            DateTime tm_end = new DateTime(tmEnd.Year, tmEnd.Month, tmEnd.Day, 0, 0, 0);
            DateTime tm_begin = tm_end.AddDays(0 - duration);

            //生成数据
            #region
            DataTable table1 = new DataTable();
            table1.Columns.Add("时间", typeof(DateTime));
            table1.Columns.Add("数据", typeof(int));
            //获取最后一天的时间
            //MessageBox.Show(WorkDataList.m_WorkData_List.Count.ToString());

            DateTime tm_endOfDay=DateTime.Now;
            if (DataMngr.m_listInfo.Count == 0)
            {
                //do nothing
            }
            else
            {
                DateTime tmp = Convert.ToDateTime(DataMngr.m_listInfo[DataMngr.m_listInfo.Count - 1].tm);
                tm_endOfDay = new DateTime(tmp.Year, tmp.Month, tmp.Day, 23, 59, 59);
            }

            DateTime tm_start = tm_endOfDay.AddDays(0 - duration);
            DateTime tm_prev = tm_start;
            //将数据插入DataTable
            #region
            if (DataMngr.m_listInfo.Count == 0)
            {
                //如果m_listInfo为空，表示工作文件只有头，没有具体数据，这个时候给table1添加一个数据0即可
                #region
                switch (chartType)
                {
                    case CHARTTYPE.PATIENT_TMP:
                        table1.Rows.Add(Convert.ToDateTime(tm_prev), 0);
                        break;
                    case CHARTTYPE.AIR_OUTLET_TMP:
                        table1.Rows.Add(Convert.ToDateTime(tm_prev), 0);
                        break;
                    case CHARTTYPE.FLOW:
                        table1.Rows.Add(Convert.ToDateTime(tm_prev), 0);
                        break;
                    case CHARTTYPE.OXY_CONCENTRATION:
                        table1.Rows.Add(Convert.ToDateTime(tm_prev), 0);
                        break;
                    case CHARTTYPE.DEWPOINT_TMP:                               // 2018/7/20新增
                        table1.Rows.Add(Convert.ToDateTime(tm_prev), 0);
                        break;
                    default:
                        //MessageBox.Show("Unkonw chart type in common_series!");
                        break;
                }
                #endregion
            }
            else
            {
                #region
                for (int i = 0; i < DataMngr.m_listInfo.Count; i++)
                //for(int i=ndex;i<WorkDataList.m_WorkData_List.Count;i++)
                {
                    //int temperature = Convert.ToInt32(DataMngr.m_listInfo[i].patient_tmp);
                    while ((Convert.ToDateTime(DataMngr.m_listInfo[i].tm) - tm_prev).TotalMinutes >= 2)
                    {
                        table1.Rows.Add(tm_prev, 0);
                        tm_prev = tm_prev.AddMinutes(1);
                    }

                    switch (chartType)
                    {
                        case CHARTTYPE.PATIENT_TMP:
                            table1.Rows.Add(Convert.ToDateTime(tm_prev), Convert.ToInt32(DataMngr.m_listInfo[i].patient_tmp));
                            break;
                        case CHARTTYPE.AIR_OUTLET_TMP:
                            table1.Rows.Add(Convert.ToDateTime(tm_prev), Convert.ToInt32(DataMngr.m_listInfo[i].air_outlet_tmp));
                            break;
                        case CHARTTYPE.FLOW:
                            table1.Rows.Add(Convert.ToDateTime(tm_prev), Convert.ToInt32(DataMngr.m_listInfo[i].flow));
                            break;
                        case CHARTTYPE.OXY_CONCENTRATION:
                            table1.Rows.Add(Convert.ToDateTime(tm_prev), Convert.ToInt32(DataMngr.m_listInfo[i].oxy_concentration));
                            break;
                        case CHARTTYPE.DEWPOINT_TMP:                               // 2018/7/20新增
                            table1.Rows.Add(Convert.ToDateTime(tm_prev), Convert.ToInt32(DataMngr.m_listInfo[i].dewpoint_tmp));
                            break;
                        default:
                            //MessageBox.Show("Unkonw chart type in common_series!");
                            break;
                    }


                    tm_prev = Convert.ToDateTime(DataMngr.m_listInfo[i].tm);

                    if (i == DataMngr.m_listInfo.Count - 1)
                    {
                        table1.Rows.Add(tm_prev, 0);
                    }
                }
                #endregion
            }
            
            #endregion

            //table1.Rows.Add(new object[] { tm, rd, rd });
            #endregion
            //double day_min = (tm_begin - tmFirstDay).TotalDays - 1; //x轴上的最小值 ,-1表示多放前一天，为了图表显示好看
            double day_max = (tm_end - tmFirstDay).TotalDays + 1;   //y轴上的最大值,+1表示日期在后推一天
            double day_min = (day_max - duration) - 0;

            //画图
            #region

            //实例化serie和chartarea
            Series series_common = new Series("series_common");
            ChartArea chartArea_common = new ChartArea("chartArea_common");

            //这句话很重要，不加的话会认为（0，0）是可视的坐标原点，而不是真是控件的坐标原点
            //http://bbs.csdn.net/topics/390179469
            this.panel_detailCharts.VerticalScroll.Value = this.panel_detailCharts.VerticalScroll.Minimum;
            LanguageMngr lang = new LanguageMngr();
            switch (chartType)
            {
                case CHARTTYPE.PATIENT_TMP:
                    this.chart_patientTmp.Series.Clear();
                    this.chart_patientTmp.ChartAreas.Clear();
                    this.chart_patientTmp.Titles.Clear();

                    this.chart_patientTmp.Location = new Point(0, 0);
                    this.chart_patientTmp.Titles.Add(lang.data_patient_tmp());

                    this.chart_patientTmp.Width = Convert.ToInt32(duration) * DataMngr.m_chartSize.Width;    //按照显示天数的比例来调整图表的宽度
                    this.chart_patientTmp.Height = DataMngr.m_chartSize.Height;
                    this.chart_patientTmp.BorderlineColor = Color.Gray;
                    this.chart_patientTmp.BorderlineWidth = 1;
                    this.chart_patientTmp.BorderlineDashStyle = ChartDashStyle.Solid;


                    series_common = new Series("patientTmp");
                    chartArea_common = new ChartArea("chartArea_patientTmp");
                    series_common.ChartArea = "chartArea_patientTmp";
                    break;
                case CHARTTYPE.AIR_OUTLET_TMP:
                    this.chart_air_outlet_tmp.Series.Clear();
                    this.chart_air_outlet_tmp.ChartAreas.Clear();
                    this.chart_air_outlet_tmp.Titles.Clear();


                    this.chart_air_outlet_tmp.Location = new Point(0, DataMngr.m_chartSize.Height);
                    this.chart_air_outlet_tmp.Titles.Add(lang.data_air_outlet_tmp());

                    this.chart_air_outlet_tmp.Width = Convert.ToInt32(duration) * DataMngr.m_chartSize.Width;    //按照显示天数的比例来调整图表的宽度
                    this.chart_air_outlet_tmp.Height = DataMngr.m_chartSize.Height;
                    this.chart_air_outlet_tmp.BorderlineColor = Color.Gray;
                    this.chart_air_outlet_tmp.BorderlineWidth = 1;
                    this.chart_air_outlet_tmp.BorderlineDashStyle = ChartDashStyle.Solid;


                    series_common = new Series("airOutLetTmp");
                    chartArea_common = new ChartArea("chartArea_airOutLetTmp");
                    series_common.ChartArea = "chartArea_airOutLetTmp";
                    break;
                case CHARTTYPE.FLOW:
                    this.chart_flow.Series.Clear();
                    this.chart_flow.ChartAreas.Clear();
                    this.chart_flow.Titles.Clear();

                    this.chart_flow.Location = new Point(0, DataMngr.m_chartSize.Height * 2);
                    this.chart_flow.Titles.Add(lang.data_flow());

                    this.chart_flow.Width = Convert.ToInt32(duration) * DataMngr.m_chartSize.Width;    //按照显示天数的比例来调整图表的宽度
                    this.chart_flow.Height = DataMngr.m_chartSize.Height;
                    this.chart_flow.BorderlineColor = Color.Gray;
                    this.chart_flow.BorderlineWidth = 1;
                    this.chart_flow.BorderlineDashStyle = ChartDashStyle.Solid;

                    series_common = new Series("flow");
                    chartArea_common = new ChartArea("chartArea_flow");
                    series_common.ChartArea = "chartArea_flow";
                    break;
                case CHARTTYPE.OXY_CONCENTRATION:
                    this.chart_oxy_concentration.Series.Clear();
                    this.chart_oxy_concentration.ChartAreas.Clear();
                    this.chart_oxy_concentration.Titles.Clear();

                    this.chart_oxy_concentration.Location = new Point(0, DataMngr.m_chartSize.Height * 3);
                    this.chart_oxy_concentration.Titles.Add(lang.data_Oxy_concentration());

                    this.chart_oxy_concentration.Width = Convert.ToInt32(duration) * DataMngr.m_chartSize.Width;    //按照显示天数的比例来调整图表的宽度
                    this.chart_oxy_concentration.Height = DataMngr.m_chartSize.Height;
                    this.chart_oxy_concentration.BorderlineColor = Color.Gray;
                    this.chart_oxy_concentration.BorderlineWidth = 1;
                    this.chart_oxy_concentration.BorderlineDashStyle = ChartDashStyle.Solid;


                    series_common = new Series("oxyConcentration");
                    chartArea_common = new ChartArea("chartArea_oxyConcentration");
                    series_common.ChartArea = "chartArea_oxyConcentration";
                    break;
                case CHARTTYPE.DEWPOINT_TMP:                                        //2018/7/20新增
                    this.chart_dewpoint_tmp.Series.Clear();
                    this.chart_dewpoint_tmp.ChartAreas.Clear();
                    this.chart_dewpoint_tmp.Titles.Clear();

                    this.chart_dewpoint_tmp.Location = new Point(0, DataMngr.m_chartSize.Height * 4);
                    this.chart_dewpoint_tmp.Titles.Add(lang.data_dewpoint_tmp());

                    this.chart_dewpoint_tmp.Width = Convert.ToInt32(duration) * DataMngr.m_chartSize.Width;
                    this.chart_dewpoint_tmp.Height = DataMngr.m_chartSize.Height;
                    this.chart_dewpoint_tmp.BorderlineColor = Color.Gray;
                    this.chart_dewpoint_tmp.BorderlineWidth = 1;
                    this.chart_dewpoint_tmp.BorderlineDashStyle = ChartDashStyle.Solid;

                    series_common = new Series("dewpointTmp");
                    chartArea_common = new ChartArea("chartArea_dewpointTmp");
                    series_common.ChartArea = "chartArea_dewpointTmp";

                    break;
                default:
                    //MessageBox.Show("Unkonw chart type in common_series!");
                    break;
            }
            #region
            ////设置chart的长宽,外边框颜色和宽度
            //this.chart_patientTmp.Width = Convert.ToInt32(duration) * DataMngr.m_chartSize.Width;    //按照显示天数的比例来调整图表的宽度
            //this.chart_patientTmp.Height = DataMngr.m_chartSize.Height;
            //this.chart_patientTmp.BorderlineColor = Color.Black;
            //this.chart_patientTmp.BorderlineWidth = 1;
            //this.chart_patientTmp.BorderlineDashStyle = ChartDashStyle.Solid;

            ////实例化serie和chartarea
            //Series patientTmp = new Series("patientTmp");
            //ChartArea chartArea_patientTmp = new ChartArea("chartArea_patientTmp");

            //serie绑定chartarea
            //patientTmp.ChartArea = "chartArea_patientTmp";

            ////chartArea_patientTmp.AxisY.Title = "Patient Temperature";
            //this.chart_patientTmp.Titles.Add("Patient Temperature");
            #endregion
            //chartarea中设置是否显示虚线
            chartArea_common.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.NotSet;
            chartArea_common.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            #region
            // font=new System.Drawing.Font("Microsoft Sans Serif",3);
            ////var font = new System.Drawing.Font("Microsoft Sans Serif",5,FontStyle.Regular,System.Drawing.GraphicsUnit.Document,1,true);
            ////chartArea_patientTmp.AxisX.LabelStyle.Font = font;
            #endregion
            //series中设置x,y轴坐标类型
            series_common.XValueType = ChartValueType.DateTime;  //设置X,Y轴的坐标类型
            series_common.YValueType = ChartValueType.Int32;

            //设置图标类型，折线图
            series_common.ChartType = SeriesChartType.Line;
            series_common.MarkerSize = 1;
            #region
            //patientTmp.EmptyPointStyle.BorderWidth = 0;
            //patientTmp.EmptyPointStyle.CustomProperties = "EmptyPointValue = Zero";
            //patientTmp.EmptyPointStyle.BorderColor = Color.White;
            //patientTmp.EmptyPointStyle.MarkerColor = Color.White;
            //Chart1.Series["Series3"].EmptyPointStyle.CustomProperties = "EmptyPointValue = Zero";
            #endregion

            //chartarea中设置X轴显示格式，以及范围，步长
            chartArea_common.AxisX.LabelStyle.Format = "HH:mm\nMM-dd";
            //chartArea_patientTmp.AxisY.IsReversed = true;
            chartArea_common.AxisX.Minimum = day_min;
            chartArea_common.AxisX.Maximum = day_max;
            chartArea_common.AxisX.Interval = 0.125 / 3 * duration; //调整x轴坐标，特别注意这个！

            //chartarea中设置y轴显示范围，以及步长
            chartArea_common.AxisY.Minimum = 0;
            if (chartType == CHARTTYPE.FLOW)
            {
                chartArea_common.AxisY.Maximum = 90;
                chartArea_common.AxisY.Interval = 10;
            }
            else if (chartType == CHARTTYPE.DEWPOINT_TMP)  // 2020-09-28,露点温度"0-50"
            {
                chartArea_common.AxisY.Maximum = 50;
                chartArea_common.AxisY.Interval = 0;
            }
            else if (chartType == CHARTTYPE.OXY_CONCENTRATION) //2020-09-28,氧浓度"0-100"
            {
                chartArea_common.AxisY.Maximum = 100;
                chartArea_common.AxisY.Interval = 0;
            }
            else
            {
                chartArea_common.AxisY.Maximum = 50;
                chartArea_common.AxisY.Interval = 0;
            }



            switch (chartType)
            {
                case CHARTTYPE.PATIENT_TMP:
                    this.chart_patientTmp.Legends.Clear(); //清除chart_workData的legend
                    this.chart_patientTmp.ChartAreas.Add(chartArea_common);
                    this.chart_patientTmp.Series.Add(series_common);
                    this.chart_patientTmp.Series["patientTmp"].Points.DataBind(table1.AsEnumerable(), "时间", "数据", "");
                    break;
                case CHARTTYPE.AIR_OUTLET_TMP:
                    this.chart_air_outlet_tmp.Legends.Clear(); //清除chart_workData的legend
                    this.chart_air_outlet_tmp.ChartAreas.Add(chartArea_common);
                    this.chart_air_outlet_tmp.Series.Add(series_common);
                    this.chart_air_outlet_tmp.Series["airOutLetTmp"].Points.DataBind(table1.AsEnumerable(), "时间", "数据", "");
                    break;
                case CHARTTYPE.FLOW:
                    this.chart_flow.Legends.Clear(); //清除chart_workData的legend
                    this.chart_flow.ChartAreas.Add(chartArea_common);
                    this.chart_flow.Series.Add(series_common);
                    this.chart_flow.Series["flow"].Points.DataBind(table1.AsEnumerable(), "时间", "数据", "");
                    break;
                case CHARTTYPE.OXY_CONCENTRATION:
                    this.chart_oxy_concentration.Legends.Clear(); //清除chart_workData的legend
                    this.chart_oxy_concentration.ChartAreas.Add(chartArea_common);
                    this.chart_oxy_concentration.Series.Add(series_common);
                    this.chart_oxy_concentration.Series["oxyConcentration"].Points.DataBind(table1.AsEnumerable(), "时间", "数据", "");
                    break;
                case CHARTTYPE.DEWPOINT_TMP:                   // 2018/7/20新增
                    this.chart_dewpoint_tmp.Legends.Clear();
                    this.chart_dewpoint_tmp.ChartAreas.Add(chartArea_common);
                    this.chart_dewpoint_tmp.Series.Add(series_common);
                    this.chart_dewpoint_tmp.Series["dewpointTmp"].Points.DataBind(table1.AsEnumerable(), "时间", "数据", "");
                    break;
                default:
                    //MessageBox.Show("Unkonw chart type in common_series!");
                    break;
            }
            #endregion
            table1 = null;

            #endregion
          
            
        }

        public void ShowUsageChart(DateTime tmBegin,DateTime tmEnd)
        {
            //从FileMngr的Dictionary中得到信息放入DataMngr的链表中
            DataMngr.GetUsageInfo();

            //画usage图
            PaintUsageChart(tmBegin, tmEnd);
        }

        public void ShowBasicInfo()
        {
            //var workDataHead = FileMngr.m_lastWorkHead;
            //var workDataMsg = FileMngr.m_lastWorkMsg;
            //当报警文件和工作信息文件(经过过滤之后)都没有的话，需要将app面板的所有信息清空
            if (FileMngr.m_alarmFileName == null && FileMngr.m_workFileNameList.Count == 0)
            {
                this.label_equipType_Value.Text = "";
                this.label_SN_Value.Text = "";
                this.label_softwarVer_Value.Text = "";
                this.label_value_patient_name.Text = "";
                this.label_value_patient_age.Text = "";
                this.label_value_patient_gender.Text = "";
                this.label_value_patient_phoneNum.Text = "";
                this.label_value_patient_adress.Text = "";
                this.label_value_patient_height.Text = "";
                this.label_value_patient_weight.Text = "";
                this.label_value_added_patientName.Text = "";
                this.label_value_added_patientAge.Text = "";
                this.label_value_added_patientGender.Text = "";
                this.label_value_added_phoneNum.Text = "";
                return;
            }

            //workDataHead.MACHINETYPE;
            this.label_runningMode_value.Text = DataMngr.GetRunningMode(FileMngr.m_lastWorkMsg.SET_MODE);
            this.label_setTmp_Value.Text = DataMngr.GetSetting2Str(FileMngr.m_lastWorkMsg.SET_TEMP);
            this.label_setAdaultChild_Value.Text = DataMngr.GetAdaultChildSetting(FileMngr.m_lastWorkMsg.SET_ADULT_OR_CHILDE);

            this.label_setFlow_Value.Text = DataMngr.GetSetting2Str(FileMngr.m_lastWorkMsg.SET_FLOW);
            this.label_setHighOxyContAlarm_Value.Text = DataMngr.GetSetting2Str(FileMngr.m_lastWorkMsg.SET_HIGH_OXYGEN_ALARM);
            this.label_setLowOxyContAlarm_Value.Text = DataMngr.GetSetting2Str(FileMngr.m_lastWorkMsg.SET_LOW_OXYGEN_ALARM);
            this.label_setAtmoizLevel_Value.Text = DataMngr.GetSetting2Str(FileMngr.m_lastWorkMsg.SET_ATOMIZATION_LEVEL);
            this.label_setAtomizTime_Value.Text = DataMngr.GetSetting2Str(FileMngr.m_lastWorkMsg.SET_ATOMIZATION_TIME);

            if (FileMngr.m_workFileNameList.Count != 0)
            {
                //优先使用工作信息的
                //byte[] head = FileMngr.GetData(FileMngr.m_lastWorkHead);
                byte[] head = FileMngr.VM_transfer_workInfoHead2Buffer(FileMngr.m_lastWorkHead);
                this.label_equipType_Value.Text = DataMngr.GetMachineType(head, 1);
                this.label_SN_Value.Text = DataMngr.GetSN(head, 2);
                this.label_softwarVer_Value.Text = DataMngr.GetSoftwareVer(head, 3);
            }
            else
            {
                //如果工作信息文件没有，则使用报警文件的
                if (FileMngr.m_alarmFileName == null)
                {
                    return;
                }

                FileStream fs = null;
                try
                {
                    fs = new FileStream(FileMngr.m_dirPath + @"\" + FileMngr.m_alarmFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                }
                catch (IOException ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                finally
                {

                }
                BinaryReader br = new BinaryReader(fs, Encoding.ASCII);

                //读信息头
                ALARM_INFO_HEAD alarmHead = new ALARM_INFO_HEAD();
                //int len_head = Marshal.SizeOf(alarmHead);
                int len_head = 16 * 16; //16个数据，每个16字节
                byte[] head = new byte[len_head];
                br.Read(head, 0, len_head);

                this.label_equipType_Value.Text = DataMngr.GetMachineType(head, 1);
                this.label_SN_Value.Text = DataMngr.GetSN(head, 2);
                this.label_softwarVer_Value.Text = DataMngr.GetSoftwareVer(head, 3);

                br.Close();
                fs.Close();
            }

        }

        //这个函数废弃了
        //public void ShowWorkDataList(DateTime TmLow, DateTime TmHigh)
        //{
        //    LanguageMngr lang = new LanguageMngr();
        //    this.listView_workData.Items.Clear();
        //    //this.listView_workData.BeginUpdate();
        //    DateTime tmBegin = new DateTime(TmLow.Year, TmLow.Month, TmLow.Day, 0, 0, 0);
        //    DateTime tmEnd = new DateTime(TmHigh.Year, TmHigh.Month, TmHigh.Day, 23, 59, 59);
        //    #region
        //    int i = 1;
        //    foreach (KeyValuePair<WORK_INFO_HEAD, List<WORK_INFO_MESSAGE>> kv in FileMngr.m_workHead_Msg_Map)
        //    {
        //        //先判断,减少不必要的foreach (var workDataMsg in list)
        //        var list = kv.Value;
        //        WORK_INFO_MESSAGE msg = list[0];
        //        DateTime msgTm = new DateTime(100 * Convert.ToInt32(msg.YEAR1) + Convert.ToInt32(msg.YEAR2),
        //                                        Convert.ToInt32(msg.MONTH), Convert.ToInt32(msg.DAY), 23, 59, 59);
        //        if (msgTm < tmBegin || msgTm > tmEnd)
        //        {
        //            continue;
        //        }

        //        #region
        //        foreach (var workDataMsg in list)
        //        {
        //            //if(FileMngr.VerifyWorkDataMsg(FileMngr.GetData(workDataMsg)))
        //            {
        //                DateTime tmFromMsg = new DateTime(100 * Convert.ToInt32(workDataMsg.YEAR1) + Convert.ToInt32(workDataMsg.YEAR2),
        //                                                 Convert.ToInt32(workDataMsg.MONTH),
        //                                                 Convert.ToInt32(workDataMsg.DAY),
        //                                                 Convert.ToInt32(workDataMsg.HOUR),
        //                                                 Convert.ToInt32(workDataMsg.MINUTE),
        //                                                 Convert.ToInt32(workDataMsg.SECOND));
        //                //if (tmFromMsg > TmLow && tmFromMsg < TmHigh)
        //                {
        //                    //解析故障状态位,一共12位
        //                    int[] faultStates = new int[12];
        //                    #region
        //                    byte bt1 = workDataMsg.DATA_FAULT_STATUS_1;
        //                    byte bt2 = workDataMsg.DATA_FAULT_STATUS_2;

        //                    for (int j = 0; j < 8; j++)
        //                    {
        //                        if (Convert.ToInt32(bt1) % 2 == 1)
        //                        {
        //                            faultStates[j] = 1;
        //                        }
        //                        else
        //                        {
        //                            faultStates[j] = 0;
        //                        }
        //                        bt1 = (byte)(bt1 >> 1);    
        //                    }

        //                    for (int j = 0; j < 3; j++)
        //                    {
        //                        if (Convert.ToInt32(bt2) % 2 == 1)
        //                        {
        //                            faultStates[j + 8] = 1;
        //                        }
        //                        else
        //                        {
        //                            faultStates[j + 8] = 0;
        //                        }
        //                        bt1 = (byte)(bt2 >> 1);
        //                    }
        //                    #endregion

        //                    //添加一行数据，废弃了，不要这个代码
        //                    #region
        //                    //ListViewItem lvi = new ListViewItem();
        //                    //lvi.Text = i.ToString();  //第一列,No.
        //                    //lvi.SubItems.Add(tmFromMsg.ToString("yyyy-MM-dd HH:mm:ss")); //第二列，时间
        //                    //lvi.SubItems.Add(Convert.ToString(Convert.ToBoolean(workDataMsg.SET_MODE) ? "雾化" : "湿化"));  //第三列，运行模式
        //                    //lvi.SubItems.Add(Convert.ToString(Convert.ToString(workDataMsg.SET_TEMP)));                      //第四列，设定温度
        //                    //lvi.SubItems.Add(Convert.ToString(Convert.ToString(workDataMsg.SET_FLOW)));                      //第五列，设定流量
        //                    //lvi.SubItems.Add(Convert.ToString(Convert.ToString(workDataMsg.SET_HIGH_OXYGEN_ALARM)));                      //第六列，设定高氧浓度报警
        //                    //lvi.SubItems.Add(Convert.ToString(Convert.ToString(workDataMsg.SET_LOW_OXYGEN_ALARM)));                      //第七列，设定低氧浓度报警
        //                    //lvi.SubItems.Add(Convert.ToString(Convert.ToString(workDataMsg.SET_ATOMIZATION_LEVEL)));                      //第8列，设定雾化量档位
        //                    //lvi.SubItems.Add(Convert.ToString(Convert.ToString(workDataMsg.SET_ATOMIZATION_TIME)));                      //第9列，设定雾化时间
        //                    //lvi.SubItems.Add(Convert.ToString(Convert.ToString(workDataMsg.SET_ADULT_OR_CHILDE)));                      //第10列，设定成人儿童模式
        //                    //lvi.SubItems.Add(Convert.ToString(Convert.ToString(workDataMsg.DATA_PATIENT_TEMP)));                      //第11列，患者端温度
        //                    //lvi.SubItems.Add(Convert.ToString(Convert.ToString(workDataMsg.DATA_AIR_OUTLET_TEMP)));                      //第12列，出气口温度
        //                    //lvi.SubItems.Add(Convert.ToString(Convert.ToString(workDataMsg.DATA_HEATING_PLATE_ADC_H)));                      //第13列，加热盘温度
        //                    //lvi.SubItems.Add(Convert.ToString(Convert.ToString(workDataMsg.DATA_ENVIRONMENT_TMP)));                      //第14列，环境温度
        //                    //lvi.SubItems.Add(Convert.ToString(Convert.ToString(workDataMsg.DATA_DRIVERBOARD_TMP)));                      //第15列，驱动板温度
        //                    //lvi.SubItems.Add(Convert.ToString(Convert.ToString(workDataMsg.DATA_FLOW)));                      //第16列，流量
        //                    //lvi.SubItems.Add(Convert.ToString(Convert.ToString(workDataMsg.DATA_OXYGEN_CONCENTRATION)));                      //第17列，氧浓度
        //                    //lvi.SubItems.Add(Convert.ToString(Convert.ToString(workDataMsg.DATA_AIR_PRESSURE)));                      //第18列，气道压力
        //                    //lvi.SubItems.Add(Convert.ToString(Convert.ToString(workDataMsg.DATA_LOOP_TYPE)));                      //第19列，回路类型
        //                    //lvi.SubItems.Add(Convert.ToBoolean(faultStates[0]) ? "yes" : "no");                         //第20列，状态位
        //                    //lvi.SubItems.Add(Convert.ToBoolean(faultStates[1]) ? "yes" : "no");
        //                    //lvi.SubItems.Add(Convert.ToBoolean(faultStates[2]) ? "yes" : "no");
        //                    //lvi.SubItems.Add(Convert.ToBoolean(faultStates[3]) ? "yes" : "no");
        //                    //lvi.SubItems.Add(Convert.ToBoolean(faultStates[4]) ? "yes" : "no");
        //                    //lvi.SubItems.Add(Convert.ToBoolean(faultStates[5]) ? "yes" : "no");
        //                    //lvi.SubItems.Add(Convert.ToBoolean(faultStates[6]) ? "yes" : "no");
        //                    //lvi.SubItems.Add(Convert.ToBoolean(faultStates[7]) ? "yes" : "no");
        //                    //lvi.SubItems.Add(Convert.ToBoolean(faultStates[8]) ? "yes" : "no");
        //                    //lvi.SubItems.Add(Convert.ToBoolean(faultStates[9]) ? "yes" : "no");
        //                    //lvi.SubItems.Add(Convert.ToBoolean(faultStates[10]) ? "yes" : "no");
        //                    //lvi.SubItems.Add(Convert.ToBoolean(faultStates[11]) ? "yes" : "no");                      //第31列，状态位


        //                    ////新加的
        //                    //lvi.SubItems.Add(Convert.ToString(Convert.ToString(workDataMsg.DATA_ATOMIZ_DACVALUE_L)));
        //                    //lvi.SubItems.Add(Convert.ToString(Convert.ToString(workDataMsg.DATA_ATOMIZ_DACVALUE_H)));
        //                    //lvi.SubItems.Add(Convert.ToString(Convert.ToString(workDataMsg.DATA_ATOMIZ_ADCVALUE_L)));
        //                    //lvi.SubItems.Add(Convert.ToString(Convert.ToString(workDataMsg.DATA_ATOMIZ_ADCVALUE_H)));
        //                    //lvi.SubItems.Add(Convert.ToString(Convert.ToString(workDataMsg.DATA_LOOP_HEATING_PWM_L)));
        //                    //lvi.SubItems.Add(Convert.ToString(Convert.ToString(workDataMsg.DATA_LOOP_HEATING_PWM_H)));
        //                    //lvi.SubItems.Add(Convert.ToString(Convert.ToString(workDataMsg.DATA_LOOP_HEATING_ADC_L)));
        //                    //lvi.SubItems.Add(Convert.ToString(Convert.ToString(workDataMsg.DATA_LOOP_HEATING_ADC_H)));
        //                    //lvi.SubItems.Add(Convert.ToString(Convert.ToString(workDataMsg.DATA_HEATING_PLATE_PWM_L)));
        //                    //lvi.SubItems.Add(Convert.ToString(Convert.ToString(workDataMsg.DATA_HEATING_PLATE_PWM_H)));
        //                    //lvi.SubItems.Add(Convert.ToString(Convert.ToString(workDataMsg.DATA_HEATING_PLATE_ADC_L)));
        //                    //lvi.SubItems.Add(Convert.ToString(Convert.ToString(workDataMsg.DATA_HEATING_PLATE_ADC_H)));
        //                    //lvi.SubItems.Add(Convert.ToString(Convert.ToString(workDataMsg.DATA_MAIN_MOTOR_DRIVER_L)));
        //                    //lvi.SubItems.Add(Convert.ToString(Convert.ToString(workDataMsg.DATA_MAIN_MOTOR_DRIVER_H)));
        //                    //lvi.SubItems.Add(Convert.ToString(Convert.ToString(workDataMsg.DATA_MAIN_MOTOR_SPEED_L)));
        //                    //lvi.SubItems.Add(Convert.ToString(Convert.ToString(workDataMsg.DATA_MAIN_MOTOR_SPEED_H)));
        //                    //lvi.SubItems.Add(Convert.ToString(Convert.ToString(workDataMsg.DATA_PRESS_SENSOR_ADC_L)));
        //                    //lvi.SubItems.Add(Convert.ToString(Convert.ToString(workDataMsg.DATA_PRESS_SENSOR_ADC_H)));
        //                    //lvi.SubItems.Add(Convert.ToString(Convert.ToString(workDataMsg.DATA_WATERLEVEL_SENSOR_HADC_L)));
        //                    //lvi.SubItems.Add(Convert.ToString(Convert.ToString(workDataMsg.DATA_WATERLEVEL_SENSOR_HADC_H)));
        //                    //lvi.SubItems.Add(Convert.ToString(Convert.ToString(workDataMsg.DATA_WATERLEVEL_SENSOR_LADC_L)));
        //                    //lvi.SubItems.Add(Convert.ToString(Convert.ToString(workDataMsg.DATA_WATERLEVEL_SENSOR_LADC_H)));
        //                    //lvi.SubItems.Add(Convert.ToString(Convert.ToString(workDataMsg.DATA_FAN_DRIVER_L)));
        //                    //lvi.SubItems.Add(Convert.ToString(Convert.ToString(workDataMsg.DATA_FAN_DRIVER_H)));
        //                    //lvi.SubItems.Add(Convert.ToString(Convert.ToString(workDataMsg.DATA_FAN_SPEED_L)));
        //                    //lvi.SubItems.Add(Convert.ToString(Convert.ToString(workDataMsg.DATA_FAN_SPEED_H)));
        //                    #endregion

        //                    //添加到链表中
        //                    WorkData wd = new WorkData(); //实例化一个WorkData
        //                    //填充信息
        //                    #region
        //                    wd.No = i.ToString();
        //                    wd.tm=tmFromMsg.ToString("yyyy-MM-dd HH:mm:ss");
        //                    wd.set_mode=Convert.ToBoolean(workDataMsg.SET_MODE) ? lang.atomization() : lang.humidification();
        //                    wd.set_tmp=Convert.ToString(workDataMsg.SET_TEMP);
        //                    wd.set_flow=Convert.ToString(workDataMsg.SET_FLOW);
        //                    wd.set_high_oxy_alarm=Convert.ToString(workDataMsg.SET_HIGH_OXYGEN_ALARM);
        //                    wd.set_low_oxy_alrm=Convert.ToString(workDataMsg.SET_LOW_OXYGEN_ALARM);
        //                    wd.set_atomiz_level=Convert.ToString(workDataMsg.SET_ATOMIZATION_LEVEL);
        //                    wd.set_atomiz_time=Convert.ToString(workDataMsg.SET_ATOMIZATION_TIME);
        //                    wd.set_adault_or_child = Convert.ToBoolean(workDataMsg.SET_ADULT_OR_CHILDE) ? lang.child() : lang.adault();
        //                    wd.data_patient_tmp = Convert.ToString(workDataMsg.DATA_PATIENT_TEMP);
        //                    wd.data_air_outlet_tmp=Convert.ToString(workDataMsg.DATA_AIR_OUTLET_TEMP);
        //                    wd.data_heating_plate_tmp=Convert.ToString(workDataMsg.DATA_HEATING_PLATE_ADC_H);
        //                    wd.data_env_tmp=Convert.ToString(workDataMsg.DATA_ENVIRONMENT_TMP);
        //                    wd.data_driveboard_tmp=Convert.ToString(workDataMsg.DATA_DRIVERBOARD_TMP);
        //                    wd.data_flow=Convert.ToString(workDataMsg.DATA_FLOW);
        //                    wd.data_oxy_concentration=Convert.ToString(workDataMsg.DATA_OXYGEN_CONCENTRATION);
        //                    wd.data_air_pressure=Convert.ToString(workDataMsg.DATA_AIR_PRESSURE);
        //                    wd.data_loop_type=Convert.ToString(workDataMsg.DATA_LOOP_TYPE);
        //                    wd.data_faultstates_0=Convert.ToBoolean(faultStates[0]) ? "yes" : "no";
        //                    wd.data_faultstates_1=Convert.ToBoolean(faultStates[1]) ? "yes" : "no";
        //                    wd.data_faultstates_2=Convert.ToBoolean(faultStates[2]) ? "yes" : "no";
        //                    wd.data_faultstates_3=Convert.ToBoolean(faultStates[3]) ? "yes" : "no";
        //                    wd.data_faultstates_4=Convert.ToBoolean(faultStates[4]) ? "yes" : "no";
        //                    wd.data_faultstates_5=Convert.ToBoolean(faultStates[5]) ? "yes" : "no";
        //                    wd.data_faultstates_6=Convert.ToBoolean(faultStates[6]) ? "yes" : "no";
        //                    wd.data_faultstates_7=Convert.ToBoolean(faultStates[7]) ? "yes" : "no";
        //                    wd.data_faultstates_8=Convert.ToBoolean(faultStates[8]) ? "yes" : "no";
        //                    wd.data_faultstates_9=Convert.ToBoolean(faultStates[9]) ? "yes" : "no";
        //                    wd.data_faultstates_10=Convert.ToBoolean(faultStates[10]) ? "yes" : "no";
        //                    wd.data_faultstates_11=Convert.ToBoolean(faultStates[11]) ? "yes" : "no";
        //                    wd.data_atmoz_DAC_L = Convert.ToString(workDataMsg.DATA_ATOMIZ_DACVALUE_L);
        //                    wd.data_atmoz_DAC_H = Convert.ToString(workDataMsg.DATA_ATOMIZ_DACVALUE_H);
        //                    wd.data_atmoz_ADC_L = Convert.ToString(workDataMsg.DATA_ATOMIZ_ADCVALUE_L);
        //                    wd.data_atmoz_ADC_H = Convert.ToString(workDataMsg.DATA_ATOMIZ_ADCVALUE_H);
        //                    wd.data_loop_heating_PWM_L = Convert.ToString(workDataMsg.DATA_LOOP_HEATING_PWM_L);
        //                    wd.data_loop_heating_PWM_H = Convert.ToString(workDataMsg.DATA_LOOP_HEATING_PWM_H);
        //                    wd.data_loop_heating_ADC_L = Convert.ToString(workDataMsg.DATA_LOOP_HEATING_ADC_L);
        //                    wd.data_loop_heating_ADC_H = Convert.ToString(workDataMsg.DATA_LOOP_HEATING_ADC_H);
        //                    wd.data_loop_heating_plate_PWM_L = Convert.ToString(workDataMsg.DATA_HEATING_PLATE_PWM_L);
        //                    wd.data_loop_heating_plate_PWM_H = Convert.ToString(workDataMsg.DATA_HEATING_PLATE_PWM_H);
        //                    wd.data_loop_heating_plate_ADC_L = Convert.ToString(workDataMsg.DATA_HEATING_PLATE_ADC_L);
        //                    wd.data_loop_heating_plate_ADC_H = Convert.ToString(workDataMsg.DATA_HEATING_PLATE_ADC_H);
        //                    wd.data_main_motor_drive_L = Convert.ToString(workDataMsg.DATA_MAIN_MOTOR_DRIVER_L);
        //                    wd.data_main_motor_drive_H = Convert.ToString(workDataMsg.DATA_MAIN_MOTOR_DRIVER_H);
        //                    wd.data_main_motor_speed_L = Convert.ToString(workDataMsg.DATA_MAIN_MOTOR_SPEED_L);
        //                    wd.data_main_motor_speed_H = Convert.ToString(workDataMsg.DATA_MAIN_MOTOR_SPEED_H);
        //                    wd.data_press_sensor_ADC_L = Convert.ToString(workDataMsg.DATA_PRESS_SENSOR_ADC_L);
        //                    wd.data_press_sensor_ADC_H = Convert.ToString(workDataMsg.DATA_PRESS_SENSOR_ADC_H);
        //                    wd.data_waterlevel_sensor_HADC_L = Convert.ToString(workDataMsg.DATA_WATERLEVEL_SENSOR_HADC_L);
        //                    wd.data_waterlevel_sensor_HADC_H = Convert.ToString(workDataMsg.DATA_WATERLEVEL_SENSOR_HADC_H);
        //                    wd.data_waterlevel_sensor_LADC_L = Convert.ToString(workDataMsg.DATA_WATERLEVEL_SENSOR_LADC_L);
        //                    wd.data_waterlevel_sensor_LADC_H = Convert.ToString(workDataMsg.DATA_WATERLEVEL_SENSOR_LADC_H);
        //                    wd.data_fan_driver_L = Convert.ToString(workDataMsg.DATA_FAN_DRIVER_L);
        //                    wd.data_fan_driver_H = Convert.ToString(workDataMsg.DATA_FAN_DRIVER_H);
        //                    wd.data_fan_speed_L = Convert.ToString(workDataMsg.DATA_FAN_SPEED_L);
        //                    wd.data_fan_speed_H = Convert.ToString(workDataMsg.DATA_FAN_SPEED_H);
        //                    #endregion

        //                    WorkDataList.m_WorkData_List.Add(wd);
        //                    //myCache1.Add(lvi);
        //                    i++;
        //                }
        //            }
        //        }
        //        #endregion
        //    }
        //    #endregion
        //    //this.listView_workData.VirtualListSize = myCache1.Count;
        //    //this.listView_workData.EndUpdate();
        //}

        public static String ChangeAlarmCode2ASC(Byte bt)
        {
            if (bt == 0x00)  //错误0
            {
                //return "H001";   //0000,0001
                return "E1";
            }
            else if (bt == 0x01) //错误1
            {
                //return "H002";  //0000,0010
                return "E2";
            }
            else if (bt == 0x02) //错误2
            {
                //return "H004";  //0000,0100
                return "E3";
            }
            else if (bt == 0x03) //错误3
            {
                //return "H008";  //0000,1000
                return "E4";
            }
            else if (bt == 0x04) //错误4
            {
                //return "H010";  //0001,0000
                return "E5";
            }
            else if (bt == 0x05) //错误5
            {
                //return "H020";  //0010,0000
                return "E6";
            }
            else if (bt == 0x06) //错误6
            {
                //return "H040";  //0100,0000
                return "E7";
            }
            else if (bt == 0x07) //错误7
            {
                //return "H080";  //1000,0000
                return "E8";
            }
            else
            {
                return Convert.ToString(bt);
            }
        }

        public void ShowAlarmList(DateTime TmLow, DateTime TmHigh)
        {
            if (!DataMngr.m_bDateTimePicker_ValueChanged)
                return;

            this.listView_alarmInfo.Items.Clear();
            this.listView_alarmInfo.BeginUpdate();
            LanguageMngr lang = new LanguageMngr();
            #region
            int i = 1;
            DateTime tmBeing = new DateTime(TmLow.Year, TmLow.Month, TmLow.Day, 0, 0, 0);
            DateTime tmEnd = new DateTime(TmHigh.Year, TmHigh.Month, TmHigh.Day, 23, 59, 59);
            //debug
            //string str="";

            foreach (var alarmMsg in FileMngr.m_alarmMsgList)
            {
                //if(FileMngr.VerifyAlarmMsg(FileMngr.GetData(alarmMsg))) //校验
                {
                    DateTime tmFromMsg = new DateTime(100 * Convert.ToInt32(alarmMsg.YEAR1) + Convert.ToInt32(alarmMsg.YEAR2),
                                                         Convert.ToInt32(alarmMsg.MONTH),
                                                         Convert.ToInt32(alarmMsg.DAY),
                                                         Convert.ToInt32(alarmMsg.HOUR),
                                                         Convert.ToInt32(alarmMsg.MINUTE),
                                                         Convert.ToInt32(alarmMsg.SECOND));
                    
                    //12.26,先暂时屏蔽掉
                    if (tmFromMsg >= tmBeing && tmFromMsg <= tmEnd)
                    {
                        ListViewItem lvi = new ListViewItem();
                        lvi.Text = i.ToString();  //第一列,No.
                        lvi.SubItems.Add(tmFromMsg.ToString("yyyy-MM-dd HH:mm:ss")); //第二列，时间
                        if(DataMngr.m_machineType==2)
                        {
                            //str = Convert.ToBoolean(alarmMsg.RUNNIN_MODE) ? lang.atomization() : lang.humidification();
                            lvi.SubItems.Add(Convert.ToBoolean(alarmMsg.RUNNIN_MODE) ? lang.atomization() : lang.humidification());
                        }
                        //lvi.SubItems.Add(Convert.ToString(Convert.ToBoolean(alarmMsg.RUNNIN_MODE) ? "雾化" : "湿化"));  //第三列，运行模式
                        
                        //lvi.SubItems.Add(Convert.ToString(alarmMsg.ALARM_CODE));                      //第四列，错误码
                        lvi.SubItems.Add(ChangeAlarmCode2ASC(alarmMsg.ALARM_CODE));
                        lvi.SubItems.Add(Convert.ToString(FileMngr.AlarmCode2Str(alarmMsg.ALARM_CODE)));            //第五列，错误描述
                        lvi.SubItems.Add(Convert.ToString(alarmMsg.ALARM_DATA_L));                                      //第六列，数据值
                        lvi.SubItems.Add(Convert.ToString(alarmMsg.ALARM_DATA_H));                                      //第7列，数据值
                        //this.listView_alarmInfo.Items.Add(lvi);
                        myCache.Add(lvi);
                        i++;   
                    }
                }
            }
            #endregion
            //MessageBox.Show(str);
            this.listView_alarmInfo.VirtualListSize = myCache.Count;
            this.listView_alarmInfo.Invalidate(); //加了这个貌似点几次之后会释放内存
            this.listView_alarmInfo.EndUpdate();
        }

        private void 语言ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 中文ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //重复点击的时候直接返回
            if (LanguageMngr.m_language == LANGUAGE.CHINA)
            {
                return;
            }
            LanguageMngr.m_language = LANGUAGE.CHINA;
            ShowLabelNameByLanguageType(LANGUAGE.CHINA);
            InitListViewColumnHead_alarm();
            InitListViewColumnHead_workData();
            if (this.dateTimePicker_Begin.Enabled == true)
            {
                //显示报警数据
                if (myCache != null)
                {
                    myCache.Clear();
                }
                ShowAlarmList(this.dateTimePicker_Begin.Value, this.dateTimePicker_End.Value);
                //显示工作数据
                if (WorkDataList.m_WorkData_List == null)
                {
                    WorkDataList.InitWorkDataList(this.dateTimePicker_Begin.Value, this.dateTimePicker_End.Value);
                    ShowCurrentPage(WorkDataList.m_nCurrentPage);
                }
                else
                {
                    WorkDataList.m_WorkData_List.Clear();
                    WorkDataList.InitWorkDataList(this.dateTimePicker_Begin.Value, this.dateTimePicker_End.Value);
                    ShowCurrentPage(WorkDataList.m_nCurrentPage);
                }
                //显示图表
                if(this.treeView_detailChart.SelectedNode!=null)
                {
                    this.ShowDetailCharts(DataMngr.m_chart_selected_date, 1);
                }
                ShowUsageChart(this.dateTimePicker_Begin.Value, this.dateTimePicker_End.Value);
            }
            
        }

        private void ShowLabelNameByLanguageType(LANGUAGE type)
        {
            LanguageMngr.m_language = type;
            LanguageMngr lang = new LanguageMngr();

            //初始化app上的标签
            #region
            this.Text = lang.app_name();
            this.设置ToolStripMenuItem.Text = lang.set();
            this.帮助ToolStripMenuItem.Text = lang.help();
            this.软件版本ToolStripMenuItem.Text = lang.soft_ver_in_menu();
            this.语言设置ToolStripMenuItem.Text = lang.language();
            this.中文ToolStripMenuItem.Text = lang.chinese();
            this.英文ToolStripMenuItem.Text = lang.english();
            this.导入数据ToolStripMenuItem.Text = lang.importData();
            this.导出数据ToolStripMenuItem.Text = lang.exportData();

            this.高级模式ToolStripMenuItem.Text = lang.advanceMode();
            this.显示所有数据ToolStripMenuItem.Text = lang.showAllWorkData();

            this.groupBox_TimeSet.Text = lang.timePeriodSet();
            this.label_startTime.Text = lang.startDate();
            this.label_endTime.Text = lang.endDate();

            this.tabControl1.TabPages["tabPage_BasicInfo"].Text = lang.basicInfo();
            this.tabControl1.TabPages["tabPage_workDataChart"].Text = lang.usageTime();
            this.tabControl1.TabPages["tabPage_detailChart"].Text = lang.detailCharts();
            this.tabControl1.TabPages["tabPage_alarmList"].Text = lang.alarmInfo();
            this.tabControl1.TabPages["tabPage_workdatalist"].Text = lang.workInfo();


            //设备信息
            this.groupBox_equipInfo.Text = lang.equipInfo();
            this.label_equipType.Text = lang.machineType();
            this.label_softwarVer.Text = lang.soft_ver();
            this.label_SN.Text = "SN：";

            //时间段
            this.groupBox_time.Text = lang.timePeriod();
            #region
            //this.groupBox_equipInfo.Text = "Equipment Info.";
            //this.groupBox_time.Text = "Time Period";
            //this.groupBox_workingParam.Text = "Working Parameter";
            //this.label_equipType.Text = "Machine Type：";
            //this.label_softwarVer.Text = "Software Ver.：";
            //this.label_SN.Text = "SN：";
            //this.label_runningMode.Text = "Running Mode：";
            //this.label_setTmp.Text = "Temperature Setting：";
            //this.label_setFlow.Text = "Flow Setting：";
            //this.label_setHighOxyContAlarm.Text = "High Oxygen Content Alarm Setting：";
            //this.label_setLowOxyContAlarm.Text = "Low Oxygen Content Alarm Setting：";
            //this.label_setAtmoizLevel.Text = "Atomizatin Level Setting：";
            //this.label_setAtomizTime.Text = "Atomizatin time Setting：";
            //this.label_setAdaultChild.Text = "Adault or Child Setting：";
            #endregion

            #endregion

            #region
            //this.label_runningMode_value.Text = DataMngr.GetRunningMode(FileMngr.m_lastWorkMsg.SET_MODE);
            //this.label_setAdaultChild_Value.Text = DataMngr.GetAdaultChildSetting(FileMngr.m_lastWorkMsg.SET_ADULT_OR_CHILDE);

            //显示设备信息
            
            //byte[] head = FileMngr.GetData(FileMngr.m_lastWorkHead);
            //this.label_equipType_Value.Text = DataMngr.GetMachineType(head, 1);
            //this.label_SN_Value.Text = DataMngr.GetSN(head, 2);
            //this.label_softwarVer_Value.Text = DataMngr.GetSoftwareVer(head, 3);
            #endregion

            //病人信息编辑
            #region
            this.groupBox_add_patient_info.Text = lang.patient_info();
            this.label_added_patientName.Text = lang.name();
            this.label_added_patient_age.Text = lang.age();
            this.label_added_patient_gender.Text = lang.gender();
            this.label_added_patient_phoneNum.Text = lang.phoneNum();
            this.button_add_patientInfo.Text = lang.edit();
            #endregion

            //病人信息显示
            #region
            this.groupBox_Patientinfo.Text = lang.patient_info();
            this.label1_patient_name.Text = lang.name();
            this.label_patient_age.Text = lang.age();
            this.label_patient_gender.Text = lang.gender();
            this.label_patient_height.Text = lang.height();
            this.label_patient_weight.Text = lang.weight();
            this.label_patient_phoneNum.Text = lang.phoneNum();
            this.label_patient_adress.Text = lang.address();

            this.label_value_patient_gender.Text = lang.showGenderValue(DataMngr.m_old_PatientInfo.gender);
            this.label_value_added_patientGender.Text = lang.showGenderValue(DataMngr.m_old_PatientInfo.gender);
            #endregion
            this.button_generateReport.Text = lang.generateReport();

            //首页，上一页，下一页，尾页，跳转
            this.button_listview_toppage.Text = lang.top_page();
            this.button_listview_endpage.Text = lang.end_page();
            this.button_listview_prev.Text = lang.prev_page();
            this.button_listview_next.Text = lang.next_page();
            this.label_listview_jumpto.Text = lang.jump_to();
        }

        private void 英文ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //重复点击的时候直接返回
            if (LanguageMngr.m_language == LANGUAGE.ENGLISH)
            {
                return;
            }
            LanguageMngr.m_language = LANGUAGE.ENGLISH;
            ShowLabelNameByLanguageType(LANGUAGE.ENGLISH);
            InitListViewColumnHead_alarm();
            InitListViewColumnHead_workData();
            if(this.dateTimePicker_Begin.Enabled==true)
            {
                if (myCache != null)
                {
                    myCache.Clear();
                }
                myCache = new List<ListViewItem>();
                ShowAlarmList(this.dateTimePicker_Begin.Value, this.dateTimePicker_End.Value);
                //ShowWorkDataList(this.dateTimePicker_Begin.Value, this.dateTimePicker_End.Value);
                if (WorkDataList.m_WorkData_List == null)
                {
                    WorkDataList.InitWorkDataList(this.dateTimePicker_Begin.Value, this.dateTimePicker_End.Value);
                    ShowCurrentPage(WorkDataList.m_nCurrentPage);
                }
                else
                {
                    WorkDataList.m_WorkData_List.Clear();
                    WorkDataList.InitWorkDataList(this.dateTimePicker_Begin.Value, this.dateTimePicker_End.Value);
                    ShowCurrentPage(WorkDataList.m_nCurrentPage);
                }

                if (this.treeView_detailChart.SelectedNode != null)
                {
                    this.ShowDetailCharts(DataMngr.m_chart_selected_date, 1);
                }
                ShowUsageChart(this.dateTimePicker_Begin.Value, this.dateTimePicker_End.Value);
            }
            
        }
        private void ClearChartInfo()
        {
            //病人温度
            if (this.chart_patientTmp.Titles != null)
            {
                this.chart_patientTmp.Titles.Clear();
            }
            if (this.chart_patientTmp.Series != null)
            {
                this.chart_patientTmp.Series.Clear();
            }
            if (this.chart_patientTmp.ChartAreas != null)
            {
                this.chart_patientTmp.ChartAreas.Clear();
            }
            this.chart_patientTmp.BorderlineColor = Color.White;

            //出气口温度
            if (this.chart_air_outlet_tmp.Titles != null)
            {
                this.chart_air_outlet_tmp.Titles.Clear();
            }
            if (this.chart_air_outlet_tmp.Series != null)
            {
                this.chart_air_outlet_tmp.Series.Clear();
            }
            if (this.chart_air_outlet_tmp.ChartAreas != null)
            {
                this.chart_air_outlet_tmp.ChartAreas.Clear();
            }
            this.chart_air_outlet_tmp.BorderlineColor = Color.White;

            //流量
            if (this.chart_flow.Titles != null)
            {
                this.chart_flow.Titles.Clear();
            }
            if (this.chart_flow.Series != null)
            {
                this.chart_flow.Series.Clear();
            }
            if (this.chart_flow.ChartAreas != null)
            {
                this.chart_flow.ChartAreas.Clear();
            }
            this.chart_flow.BorderlineColor = Color.White;

            //氧浓度
            if (this.chart_oxy_concentration.Titles != null)
            {
                this.chart_oxy_concentration.Titles.Clear();
            }
            if (this.chart_oxy_concentration.Series != null)
            {
                this.chart_oxy_concentration.Series.Clear();
            }
            if (this.chart_oxy_concentration.ChartAreas != null)
            {
                this.chart_oxy_concentration.ChartAreas.Clear();
            }
            this.chart_oxy_concentration.BorderlineColor = Color.White;

            //露点温度
            if (this.chart_dewpoint_tmp.Titles != null)     //2018/7/20新增
            {
                this.chart_dewpoint_tmp.Titles.Clear();
            }
            if (this.chart_dewpoint_tmp.Series != null)
            {
                this.chart_dewpoint_tmp.Series.Clear();
            }
            if (this.chart_dewpoint_tmp.ChartAreas != null)
            {
                this.chart_dewpoint_tmp.ChartAreas.Clear();
            }
            this.chart_dewpoint_tmp.BorderlineColor = Color.White;

            //this.chart_patientTmp.Location = new Point(0, 0);
            //this.chart_air_outlet_tmp.Location = new Point(0, DataMngr.m_chartSize.Height);
            //this.chart_flow.Location=new Point(0, DataMngr.m_chartSize.Height*2);
            //this.chart_oxy_concentration.Location = new Point(0, DataMngr.m_chartSize.Height * 3);
        }

        private void 导入数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //加载文件内容到内存中，存放在FileMngr的链表中
            #region
            if (this.folderBrowserDialog_selectFolder.ShowDialog() == DialogResult.OK)
            {
                //规避bug,
                //bug描述：当页面停留在报警列表时，切换导入的文件夹，会触发虚模式错误
                //规避方法：先导入文件，触发时间控件后，才能操作tabpage
                DataMngr.m_bDateTimePicker_ValueChanged = false;
                this.tabControl1.SelectedIndex = 0;

                this.label_dateFrom_Value.Text = "NA";
                this.label_dateTo_Value.Text = "NA";
                //如果重复点击按钮，要先清除之前Msg链表的资源
                #region
                if (FileMngr.m_workFileNameList != null)
                {
                    FileMngr.m_workFileNameList.Clear();
                }
                if (FileMngr.m_workFileName_CanBeOpened_List != null)
                {
                    FileMngr.m_workFileName_CanBeOpened_List.Clear();
                }
                if (FileMngr.m_alarmMsgList != null)
                {
                    FileMngr.m_alarmMsgList.Clear();
                }
                if (FileMngr.m_workHead_Msg_Map != null)
                {
                    FileMngr.m_workHead_Msg_Map.Clear();
                }
                if (DataMngr.m_usageTable_beginTime_list != null)
                {
                    DataMngr.m_usageTable_beginTime_list.Clear();
                }
                if (DataMngr.m_usageTable_endTime_list != null)
                {
                    DataMngr.m_usageTable_endTime_list.Clear();
                }
                if (DataMngr.m_usageTable_xAxis_list != null)
                {
                    DataMngr.m_usageTable_xAxis_list.Clear();
                }
                if (myCache != null)
                {
                    myCache.Clear();
                }
                //清除图表信息
                ClearChartInfo();

                #endregion
                string strPath = folderBrowserDialog_selectFolder.SelectedPath;//获取打开的文件路径名
                //判断打开的文件夹是否有效
                if (!FileMngr.IsDirValidate(strPath))
                {
                    MessageBox.Show(LanguageMngr.pls_choose_the_right_folder());
                    return;
                }

                //1.得到所有文件名
                FileMngr.GetAllFilesName(); //获取所有文件的文件名，放入m_alarmFileName和m_workFileNameList中

                //2.得到文件名中最小时间和最大时间，做为DateTimePicker的起始时间,并且DateTimePicker不能超过起始时间
                FileMngr.GetMinMaxDateTime();

                //3.校验文件，并且得到信息头和信息体链表
                if (FileMngr.m_alarmFileName != null)
                {
                    if (!FileMngr.GetAlarmMsg())
                    {
                        MessageBox.Show(LanguageMngr.fail_to_get_alarmFile_info());
                    }
                }
                else
                {
                    MessageBox.Show(LanguageMngr.lack_of_alarm_file());
                }

                if (FileMngr.m_workFileNameList.Count != 0)
                {
                    if (!FileMngr.GetWorkMsg())
                    {
                        //MessageBox.Show("这里填什么好呢？");
                    }
                }
                else
                {
                    //FileMngr.m_dirPath = null; //丢弃得到的m_dirPath
                    MessageBox.Show(LanguageMngr.lack_of_work_file());
                    //return;
                }

                InitDateTimePicer();

                InitListViewColumnHead_alarm();
                InitListViewColumnHead_workData();
                ////4.初始化workdata链表，使其有数据
                //WorkDataList.InitWorkDataList();
                //MessageBox.Show("WorkDataList.m_WorkData_List："+WorkDataList.m_WorkData_List.Count.ToString());

                //初始化tree控件
                InitTree();

                //多次导入文件夹时,将高级模式里的子项全部设为uncheck状态
                显示所有数据ToolStripMenuItem.CheckState = CheckState.Unchecked;

                //5.显示app面板上基本信息的各个数据,显示最新的数据
                ShowBasicInfo();

            }
            else
            {
                return;
            }
            #endregion

            //清空控件列表,已经列表资源
            this.listView_alarmInfo.Items.Clear();
            this.listView_workData.Items.Clear();
            //if (FileMngr.m_alarmMsgList!=null)
            //{
            //    FileMngr.m_alarmMsgList.Clear();
            //}
            if (this.myCache != null)
            {
                this.myCache.Clear();
            }
            if(WorkDataList.m_WorkData_List!=null)
            {
                WorkDataList.m_WorkData_List.Clear();
            }

            this.dateTimePicker_Begin.Enabled = true;
            this.dateTimePicker_End.Enabled = true;

            //自动点一下datetimepicker控件？，加不加这个功能呢？
            //这段代码直接从datetimepicker 控件的close_up事件中复制过来的
            if(true)
            //if(false)
            {
                dateTimePicker_BeginOrEnd_CloseUp(BEGIN_END.BEGIN);
            }
        }

        public static String IsByteOxFF(byte bt)
        {
            if (bt == 0xFF)
            {
                return @"/";
            }
            else
            {
                return Convert.ToString(bt);
            }
        }

        private void 导出数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FileMngr.m_dirPath == null)
            {
                MessageBox.Show(LanguageMngr.no_data());
                return;
            }

            if (DataMngr.m_bDateTimePicker_ValueChanged == false)
            {
                MessageBox.Show(LanguageMngr.pls_choose_time());
                return;
            }
            DateTime tm_begin = this.dateTimePicker_Begin.Value;
            DateTime tm_end = this.dateTimePicker_End.Value;
            DateTime tmBegin = new DateTime(tm_begin.Year, tm_begin.Month, tm_begin.Day, 0, 0, 0);
            DateTime tmEnd = new DateTime(tm_end.Year, tm_end.Month, tm_end.Day, 23, 59, 59);

            //MessageBox.Show("请选择要保存文件的文件夹");
            //先将数据到到本地
            string strPath = "";
            if (this.folderBrowserDialog_saveFiles.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            
            strPath = folderBrowserDialog_saveFiles.SelectedPath;//获取打开的文件路径名
            //创建两个文件
            string alarm_str = "";
            string workdata_str = "";
            if (LanguageMngr.m_language == LANGUAGE.ENGLISH)
            {
                alarm_str = "_Alarm.csv";
                workdata_str = "_WorkData.csv";
            }
            else if (LanguageMngr.m_language == LANGUAGE.CHINA)
            {
                alarm_str = "_报警数据.csv";
                workdata_str = "_工作数据.csv";
            }

            FileStream fs_alarm = null;
            try
            {
                fs_alarm = new FileStream(strPath + @"\" + label_SN_Value.Text + alarm_str, FileMode.Create);
            }
            catch (IOException ex)
            {
                if (fs_alarm != null)
                    fs_alarm.Close();
                MessageBox.Show(ex.Message);
                return;
            }


            FileStream fs_workData = null;
            try
            {
                fs_workData = new FileStream(strPath + @"\" + label_SN_Value.Text + workdata_str, FileMode.Create);
            }
            catch (IOException ex)
            {
                if (fs_workData != null)
                    fs_workData.Close();
                MessageBox.Show(ex.Message);
                return;
            }

            StreamWriter sw_alarm = new StreamWriter(fs_alarm, Encoding.GetEncoding("gb2312"));
            StreamWriter sw_workData = new StreamWriter(fs_workData, Encoding.GetEncoding("gb2312"));

            LanguageMngr lang = new LanguageMngr();
            //写数据到两个文件中,先做个.csv格式的
            //先写alarm.csv
            int i = 1;
            if(FileMngr.m_alarmFileName!=null)
            {
                #region
                sw_alarm.WriteLine(lang.machineType() + "," + this.label_equipType_Value.Text);
                sw_alarm.WriteLine("SN：" + "," + this.label_SN_Value.Text);
                sw_alarm.WriteLine(lang.soft_ver() + "," + this.label_softwarVer_Value.Text);
                if (DataMngr.m_machineType == 2)
                {
                    sw_alarm.WriteLine("No." + "," + lang.date() + "," + lang.running_mode() + "," + lang.alarm_code()
                        + "," + lang.alarm_info() + "," + lang.alarm_value_L() + "," + lang.alarm_value_H());
                }
                else if (DataMngr.m_machineType == 1)
                {
                    //sw_alarm.WriteLine("No." + "," + "日期" + "," + "报警码" + "," + "报警信息" + "," + "报警数据值1" + "," + "报警数据值2");
                    sw_alarm.WriteLine("No." + "," + lang.date()  + "," + lang.alarm_code()
                        + "," + lang.alarm_info() + "," + lang.alarm_value_L() + "," + lang.alarm_value_H());
                }
                else
                {
                    //do nothing
                }
                //int i = 1;
                foreach (var alarmMsg in FileMngr.m_alarmMsgList)
                {
                    #region
                    string line = "";
                    DateTime tmFromMsg = new DateTime(100 * Convert.ToInt32(alarmMsg.YEAR1) + Convert.ToInt32(alarmMsg.YEAR2),
                                                            Convert.ToInt32(alarmMsg.MONTH),
                                                            Convert.ToInt32(alarmMsg.DAY),
                                                            Convert.ToInt32(alarmMsg.HOUR),
                                                            Convert.ToInt32(alarmMsg.MINUTE),
                                                            Convert.ToInt32(alarmMsg.SECOND));
                    //先屏蔽，先修改测试文件，改完在开 //已经改完
                    if(tmFromMsg>=tmBegin&&tmFromMsg<=tmEnd)
                    {
                        #region
                        if (DataMngr.m_machineType == 2)
                        {
                            line = i.ToString() + ","
                            + tmFromMsg.ToString("yyyy-MM-dd HH:mm:ss") + ","
                            + Convert.ToString(Convert.ToBoolean(alarmMsg.RUNNIN_MODE) ?  lang.atomization(): lang.humidification()) + ","
                           // + Convert.ToString(Convert.ToString(alarmMsg.ALARM_CODE)) + ","
                           + Convert.ToString(ChangeAlarmCode2ASC(alarmMsg.ALARM_CODE)) + ","
                            + Convert.ToString(FileMngr.AlarmCode2Str(alarmMsg.ALARM_CODE)) + ","
                            + Convert.ToString(alarmMsg.ALARM_DATA_L) + ","
                            + Convert.ToString(alarmMsg.ALARM_DATA_H);
                        }
                        else if (DataMngr.m_machineType == 1)
                        {
                            line = i.ToString() + ","
                            + tmFromMsg.ToString("yyyy-MM-dd HH:mm:ss") + ","
                            //+ Convert.ToString(Convert.ToString(alarmMsg.ALARM_CODE)) + ","
                            + Convert.ToString(ChangeAlarmCode2ASC(alarmMsg.ALARM_CODE)) + ","
                            + Convert.ToString(FileMngr.AlarmCode2Str(alarmMsg.ALARM_CODE)) + ","
                            + Convert.ToString(alarmMsg.ALARM_DATA_L) + ","
                            + Convert.ToString(alarmMsg.ALARM_DATA_H);
                        }
                        else
                        {
                            //do nothing
                        }
                        sw_alarm.WriteLine(line);
                        i++;
                        #endregion
                    }
                    #endregion
                }
                #endregion
            }
            
            //在写workdata.csv
            //备注说明:excel2007最多能存1048576行
            #region
            sw_workData.WriteLine(lang.machineType() + "," + this.label_equipType_Value.Text);
            sw_workData.WriteLine("SN：" + "," + this.label_SN_Value.Text);
            sw_workData.WriteLine(lang.soft_ver() + "," + this.label_softwarVer_Value.Text);
            i = 1;
            //写列头
            #region
            if(DataMngr.m_advanceMode==false)
            {
                //没有高级模式
                #region
                if (DataMngr.m_machineType == 2)
                {
                    //VNU002,有运行模式
                    //sw_workData.WriteLine("No." + ","+ "日期" + ","+ "运行模式" + ","
                    //                + "设定成人儿童模式" + ","+ "患者端温度" + ","
                    //                + "出气口温度" + ","+ "流量" + ","+ "氧浓度");
                    sw_workData.WriteLine("No." + "," + lang.date() + "," + lang.running_mode()+ ","
                                    + lang.set_adault_or_child() + "," + lang.data_patient_tmp() + ","
                                    + lang.data_air_outlet_tmp() + "," + lang.data_flow()+ "," + lang.data_Oxy_concentration()+","+lang.data_dewpoint_tmp());  //2018/7/20新增露点温度
                }
                else
                {
                    //VNU001,没有运行模式
                    //sw_workData.WriteLine("No." + ","+ "日期" + ","
                    //                + "设定成人儿童模式" + ","+ "患者端温度" + ","
                    //                + "出气口温度" + ","+ "流量" + ","+ "氧浓度");
                    sw_workData.WriteLine("No." + "," + lang.date()  + ","
                                    + lang.set_adault_or_child() + "," + lang.data_patient_tmp() + ","
                                    + lang.data_air_outlet_tmp() + "," + lang.data_flow() + "," + lang.data_Oxy_concentration() +","+ lang.data_dewpoint_tmp()); //2018/7/20新增露点温度
                }
                #endregion
            }
            else
            {
                #region
                //高级模式-所有数据
                if (显示所有数据ToolStripMenuItem.CheckState == CheckState.Checked)
                {
                    if(DataMngr.m_machineType==2)
                    {
                        
                        #region
                        //sw_workData.WriteLine("No." + ","
                        //                + "日期" + ","
                        //                + "运行模式" + ","
                        //                + "设定温度" + ","
                        //                + "设定流量" + ","
                        //                + "设定高氧浓度报警" + ","
                        //                + "设定低氧浓度报警" + ","
                        //                + "设定雾化量档位" + ","
                        //                + "设定雾化时间" + ","
                        //                + "设定成人儿童模式" + ","
                        //                + "患者端温度" + ","
                        //                + "出气口温度" + ","
                        //                + "加热盘温度" + ","
                        //                + "环境温度" + ","
                        //                + "驱动板温度" + ","
                        //                + "流量" + ","
                        //                + "氧浓度" + ","
                        //                + "气道压力" + ","
                        //                + "回路类型" + ","
                        //                + "故障1" + ","
                        //                + "故障2" + ","
                        //                + "故障3" + ","
                        //                + "故障4" + ","
                        //                + "故障5" + ","
                        //                + "故障6" + ","
                        //                + "故障7" + ","
                        //                + "故障8" + ","
                        //                + "故障9" + ","
                        //                + "故障10" + ","
                        //                + "故障11" + ","
                        //                + "故障12" + ","
                        //                + "雾化DAC数值L" + ","
                        //                + "雾化DAC数值H" + ","
                        //                + "雾化ADC数值L" + ","
                        //                + "雾化ADC数值H" + ","
                        //                + "回路加热PWM数值L" + ","
                        //                + "回路加热PWM数值H" + ","
                        //                + "回路加热ADC数值L" + ","
                        //                + "回路加热ADC数值H" + ","
                        //                + "加热盘加热PWM数值L" + ","
                        //                + "加热盘加热PWM数值H" + ","
                        //                + "加热盘加热ADC数值L" + ","
                        //                + "加热盘加热ADC数值H" + ","
                        //                + "主马达驱动数值L" + ","
                        //                + "主马达驱动数值H" + ","
                        //                + "主马达转速数值L" + ","
                        //                + "主马达转速数值H" + ","
                        //                + "压力传感器ADC值L" + ","
                        //                + "压力传感器ADC值H" + ","
                        //                + "水位传感器HADC值L" + ","
                        //                + "水位传感器HADC值H" + ","
                        //                + "水位传感器LADC值L" + ","
                        //                + "水位传感器LADC值H" + ","
                        //                + "散热风扇驱动数值L" + ","
                        //                + "散热风扇驱动数值H" + ","
                        //                + "散热风扇转速数值L" + ","
                        //                + "散热风扇转速数值H");
                        #endregion
                        //有运行模式
                        #region
                        sw_workData.WriteLine("No." + ","
                                        + lang.date() + ","
                                        + lang.running_mode() + ","
                                        + lang.set_temp() + ","
                                        + lang.set_flow() + ","
                                        + lang.set_high_oxy_alarm()+ ","
                                        + lang.set_low_oxy_alarm() + ","
                                        + lang.set_atomiz_level() + ","
                                        + lang.set_atomiz_time() + ","
                                        + lang.set_adault_or_child() + ","
                                        + lang.data_patient_tmp() + ","
                                        + lang.data_air_outlet_tmp()+ ","
                                        + lang.data_heating_plate_tmp() + ","
                                        + lang.data_enviroment_tmp() + ","
                                        + lang.data_driveboard_tmp() + ","
                                        + lang.data_flow() + ","
                                        + lang.data_Oxy_concentration() + ","
                                        + lang.data_air_pressure() + ","
                                        + lang.data_loop_type() + ","
                                        + lang.data_fault_statue1() + ","
                                        + lang.data_fault_statue2() + ","
                                        + lang.data_fault_statue3() + ","
                                        + lang.data_fault_statue4() + ","
                                        + lang.data_fault_statue5() + ","
                                        + lang.data_fault_statue6() + ","
                                        + lang.data_fault_statue7() + ","
                                        + lang.data_fault_statue8() + ","
                                        + lang.data_fault_statue9() + ","
                                        + lang.data_fault_statue10() + ","
                                        + lang.data_fault_statue11() + ","
                                        + lang.data_fault_statue12() + ","
                                        + lang.data_atomiz_DAC_L() + ","
                                        + lang.data_atomiz_DAC_H() + ","
                                        + lang.data_atomiz_ADC_L() + ","
                                        + lang.data_atomiz_ADC_H() + ","
                                        + lang.data_loop_heating_PWM_L()+ ","
                                        + lang.data_loop_heating_PWM_H() + ","
                                        + lang.data_loop_heating_ADC_L() + ","
                                        + lang.data_loop_heating_ADC_H() + ","
                                        + lang.data_heating_plate_PWM_L() + ","
                                        + lang.data_heating_plate_PWM_H() + ","
                                        + lang.data_heating_plate_ADC_L() + ","
                                        + lang.data_heating_plate_ADC_H() + ","
                                        + lang.data_main_motor_drive_L() + ","
                                        + lang.data_main_motor_drive_H() + ","
                                        + lang.data_main_motor_speed_L()+ ","
                                        + lang.data_main_motor_speed_H() + ","
                                        + lang.data_press_sensor_ADC_L() + ","
                                        + lang.data_press_sensor_ADC_H() + ","
                                        + lang.data_waterlevel_HADC_L() + ","
                                        + lang.data_waterlevel_HADC_H() + ","
                                        + lang.data_waterlevel_LADC_L() + ","
                                        + lang.data_waterlevel_LADC_H() + ","
                                        + lang.data_fan_driver_L() + ","
                                        + lang.data_fan_driver_H() + ","
                                        + lang.data_fan_speed_L() + ","
                                        + lang.data_fan_speed_H()+","
                                        + lang.data_main_motor_tmp_ADC_L() + ","     //2018/7/20补上遗漏的
                                        + lang.data_main_motor_tmp_ADC_H() + ","
                                        + lang.data_dewpoint_tmp()                   //2018/7/20新增露点温度
                                        );
                        #endregion
                    }
                    else
                    {
                        
                        #region
                        //sw_workData.WriteLine("No." + ","
                        //                + "日期" + ","
                        //                //+ "运行模式" + ","
                        //                + "设定温度" + ","
                        //                + "设定流量" + ","
                        //                + "设定高氧浓度报警" + ","
                        //                + "设定低氧浓度报警" + ","
                        //                + "设定雾化量档位" + ","
                        //                + "设定雾化时间" + ","
                        //                + "设定成人儿童模式" + ","
                        //                + "患者端温度" + ","
                        //                + "出气口温度" + ","
                        //                + "加热盘温度" + ","
                        //                + "环境温度" + ","
                        //                + "驱动板温度" + ","
                        //                + "流量" + ","
                        //                + "氧浓度" + ","
                        //                + "气道压力" + ","
                        //                + "回路类型" + ","
                        //                + "故障1" + ","
                        //                + "故障2" + ","
                        //                + "故障3" + ","
                        //                + "故障4" + ","
                        //                + "故障5" + ","
                        //                + "故障6" + ","
                        //                + "故障7" + ","
                        //                + "故障8" + ","
                        //                + "故障9" + ","
                        //                + "故障10" + ","
                        //                + "故障11" + ","
                        //                + "故障12" + ","
                        //                + "雾化DAC数值L" + ","
                        //                + "雾化DAC数值H" + ","
                        //                + "雾化ADC数值L" + ","
                        //                + "雾化ADC数值H" + ","
                        //                + "回路加热PWM数值L" + ","
                        //                + "回路加热PWM数值H" + ","
                        //                + "回路加热ADC数值L" + ","
                        //                + "回路加热ADC数值H" + ","
                        //                + "加热盘加热PWM数值L" + ","
                        //                + "加热盘加热PWM数值H" + ","
                        //                + "加热盘加热ADC数值L" + ","
                        //                + "加热盘加热ADC数值H" + ","
                        //                + "主马达驱动数值L" + ","
                        //                + "主马达驱动数值H" + ","
                        //                + "主马达转速数值L" + ","
                        //                + "主马达转速数值H" + ","
                        //                + "压力传感器ADC值L" + ","
                        //                + "压力传感器ADC值H" + ","
                        //                + "水位传感器HADC值L" + ","
                        //                + "水位传感器HADC值H" + ","
                        //                + "水位传感器LADC值L" + ","
                        //                + "水位传感器LADC值H" + ","
                        //                + "散热风扇驱动数值L" + ","
                        //                + "散热风扇驱动数值H" + ","
                        //                + "散热风扇转速数值L" + ","
                        //                + "散热风扇转速数值H");
                        #endregion
                        //没有运行模式
                        #region
                        sw_workData.WriteLine("No." + ","
                                        + lang.date() + ","
                                        //+ lang.running_mode() + ","
                                        + lang.set_temp() + ","
                                        + lang.set_flow() + ","
                                        + lang.set_high_oxy_alarm() + ","
                                        + lang.set_low_oxy_alarm() + ","
                                        + lang.set_atomiz_level() + ","
                                        + lang.set_atomiz_time() + ","
                                        + lang.set_adault_or_child() + ","
                                        + lang.data_patient_tmp() + ","
                                        + lang.data_air_outlet_tmp() + ","
                                        + lang.data_heating_plate_tmp() + ","
                                        + lang.data_enviroment_tmp() + ","
                                        + lang.data_driveboard_tmp() + ","
                                        + lang.data_flow() + ","
                                        + lang.data_Oxy_concentration() + ","
                                        + lang.data_air_pressure() + ","
                                        + lang.data_loop_type() + ","
                                        + lang.data_fault_statue1() + ","
                                        + lang.data_fault_statue2() + ","
                                        + lang.data_fault_statue3() + ","
                                        + lang.data_fault_statue4() + ","
                                        + lang.data_fault_statue5() + ","
                                        + lang.data_fault_statue6() + ","
                                        + lang.data_fault_statue7() + ","
                                        + lang.data_fault_statue8() + ","
                                        + lang.data_fault_statue9() + ","
                                        + lang.data_fault_statue10() + ","
                                        + lang.data_fault_statue11() + ","
                                        + lang.data_fault_statue12() + ","
                                        + lang.data_atomiz_DAC_L() + ","
                                        + lang.data_atomiz_DAC_H() + ","
                                        + lang.data_atomiz_ADC_L() + ","
                                        + lang.data_atomiz_ADC_H() + ","
                                        + lang.data_loop_heating_PWM_L() + ","
                                        + lang.data_loop_heating_PWM_H() + ","
                                        + lang.data_loop_heating_ADC_L() + ","
                                        + lang.data_loop_heating_ADC_H() + ","
                                        + lang.data_heating_plate_PWM_L() + ","
                                        + lang.data_heating_plate_PWM_H() + ","
                                        + lang.data_heating_plate_ADC_L() + ","
                                        + lang.data_heating_plate_ADC_H() + ","
                                        + lang.data_main_motor_drive_L() + ","
                                        + lang.data_main_motor_drive_H() + ","
                                        + lang.data_main_motor_speed_L() + ","
                                        + lang.data_main_motor_speed_H() + ","
                                        + lang.data_press_sensor_ADC_L() + ","
                                        + lang.data_press_sensor_ADC_H() + ","
                                        + lang.data_waterlevel_HADC_L() + ","
                                        + lang.data_waterlevel_HADC_H() + ","
                                        + lang.data_waterlevel_LADC_L() + ","
                                        + lang.data_waterlevel_LADC_H() + ","
                                        + lang.data_fan_driver_L() + ","
                                        + lang.data_fan_driver_H() + ","
                                        + lang.data_fan_speed_L() + ","
                                        + lang.data_fan_speed_H()+","
                                         + lang.data_main_motor_tmp_ADC_L() + ","     //2018/7/20补上遗漏的
                                        + lang.data_main_motor_tmp_ADC_H() + ","
                                        + lang.data_dewpoint_tmp()                   //2018/7/20新增露点温度
                                        );
                        #endregion
                    }
                    
                }
                else
                {
                    //高级模式-部分数据
                    if (DataMngr.m_machineType == 2)
                    {
                        #region
                        //sw_workData.WriteLine("No." + ","
                        //                + "日期" + ","
                        //                + "运行模式" + ","
                        //                + "设定温度" + ","
                        //                + "设定流量" + ","
                        //                + "设定高氧浓度报警" + ","
                        //                + "设定低氧浓度报警" + ","
                        //                + "设定雾化量档位" + ","
                        //                + "设定雾化时间" + ","
                        //                + "设定成人儿童模式" + ","
                        //                + "患者端温度" + ","
                        //                + "出气口温度" + ","
                        //                + "加热盘温度" + ","
                        //                + "环境温度" + ","
                        //                + "驱动板温度" + ","
                        //                + "流量" + ","
                        //                + "氧浓度" + ","
                        //                + "气道压力" + ","
                        //                + "回路类型" + ","
                        //                + "故障1" + ","
                        //                + "故障2" + ","
                        //                + "故障3" + ","
                        //                + "故障4" + ","
                        //                + "故障5" + ","
                        //                + "故障6" + ","
                        //                + "故障7" + ","
                        //                + "故障8" + ","
                        //                + "故障9" + ","
                        //                + "故障10" + ","
                        //                + "故障11" + ","
                        //                + "故障12");
                        #endregion
                        //有运行模式
                        #region
                        sw_workData.WriteLine("No." + ","
                                        + lang.date() + ","
                                        + lang.running_mode() + ","
                                        + lang.set_temp() + ","
                                        + lang.set_flow() + ","
                                        + lang.set_high_oxy_alarm() + ","
                                        + lang.set_low_oxy_alarm() + ","
                                        + lang.set_atomiz_level() + ","
                                        + lang.set_atomiz_time() + ","
                                        + lang.set_adault_or_child() + ","
                                        + lang.data_patient_tmp() + ","
                                        + lang.data_air_outlet_tmp() + ","
                                        + lang.data_heating_plate_tmp() + ","
                                        + lang.data_enviroment_tmp() + ","
                                        + lang.data_driveboard_tmp() + ","
                                        + lang.data_flow() + ","
                                        + lang.data_Oxy_concentration() + ","
                                        + lang.data_air_pressure() + ","
                                        + lang.data_loop_type() + ","
                                        + lang.data_fault_statue1() + ","
                                        + lang.data_fault_statue2() + ","
                                        + lang.data_fault_statue3() + ","
                                        + lang.data_fault_statue4() + ","
                                        + lang.data_fault_statue5() + ","
                                        + lang.data_fault_statue6() + ","
                                        + lang.data_fault_statue7() + ","
                                        + lang.data_fault_statue8() + ","
                                        + lang.data_fault_statue9() + ","
                                        + lang.data_fault_statue10() + ","
                                        + lang.data_fault_statue11() + ","
                                        + lang.data_fault_statue12()+","
                                         + lang.data_dewpoint_tmp()                   //2018/7/20新增露点温度
                                        );
                        #endregion
                    }
                    else
                    {
                        #region
                        //sw_workData.WriteLine("No." + ","
                        //                + "日期" + ","
                        //                //+ "运行模式" + ","
                        //                + "设定温度" + ","
                        //                + "设定流量" + ","
                        //                + "设定高氧浓度报警" + ","
                        //                + "设定低氧浓度报警" + ","
                        //                + "设定雾化量档位" + ","
                        //                + "设定雾化时间" + ","
                        //                + "设定成人儿童模式" + ","
                        //                + "患者端温度" + ","
                        //                + "出气口温度" + ","
                        //                + "加热盘温度" + ","
                        //                + "环境温度" + ","
                        //                + "驱动板温度" + ","
                        //                + "流量" + ","
                        //                + "氧浓度" + ","
                        //                + "气道压力" + ","
                        //                + "回路类型" + ","
                        //                + "故障1" + ","
                        //                + "故障2" + ","
                        //                + "故障3" + ","
                        //                + "故障4" + ","
                        //                + "故障5" + ","
                        //                + "故障6" + ","
                        //                + "故障7" + ","
                        //                + "故障8" + ","
                        //                + "故障9" + ","
                        //                + "故障10" + ","
                        //                + "故障11" + ","
                        //                + "故障12");
                        #endregion
                        //没有运行模式
                        #region
                        sw_workData.WriteLine("No." + ","
                                        + lang.date() + ","
                                        //+ lang.running_mode() + ","
                                        + lang.set_temp() + ","
                                        + lang.set_flow() + ","
                                        + lang.set_high_oxy_alarm() + ","
                                        + lang.set_low_oxy_alarm() + ","
                                        + lang.set_atomiz_level() + ","
                                        + lang.set_atomiz_time() + ","
                                        + lang.set_adault_or_child() + ","
                                        + lang.data_patient_tmp() + ","
                                        + lang.data_air_outlet_tmp() + ","
                                        + lang.data_heating_plate_tmp() + ","
                                        + lang.data_enviroment_tmp() + ","
                                        + lang.data_driveboard_tmp() + ","
                                        + lang.data_flow() + ","
                                        + lang.data_Oxy_concentration() + ","
                                        + lang.data_air_pressure() + ","
                                        + lang.data_loop_type() + ","
                                        + lang.data_fault_statue1() + ","
                                        + lang.data_fault_statue2() + ","
                                        + lang.data_fault_statue3() + ","
                                        + lang.data_fault_statue4() + ","
                                        + lang.data_fault_statue5() + ","
                                        + lang.data_fault_statue6() + ","
                                        + lang.data_fault_statue7() + ","
                                        + lang.data_fault_statue8() + ","
                                        + lang.data_fault_statue9() + ","
                                        + lang.data_fault_statue10() + ","
                                        + lang.data_fault_statue11() + ","
                                        + lang.data_fault_statue12()+","
                                         + lang.data_dewpoint_tmp()                   //2018/7/20新增露点温度
                                        );
                        #endregion
                    }
                }
                #endregion
            }

            #endregion
            foreach (KeyValuePair<WORK_INFO_HEAD, List<WORK_INFO_MESSAGE>> kv in FileMngr.m_workHead_Msg_Map)
            {
                var list = kv.Value;
                foreach (var workDataMsg in list)
                {
                    int[] faultStates = new int[12];

                    //将数据写入
                    #region
                    string line = "";
                    DateTime tmFromMsg = new DateTime(100 * Convert.ToInt32(workDataMsg.YEAR1) + Convert.ToInt32(workDataMsg.YEAR2),
                                                         Convert.ToInt32(workDataMsg.MONTH),
                                                         Convert.ToInt32(workDataMsg.DAY),
                                                         Convert.ToInt32(workDataMsg.HOUR),
                                                         Convert.ToInt32(workDataMsg.MINUTE),
                                                         Convert.ToInt32(workDataMsg.SECOND));
                    if (tmFromMsg >= tmBegin && tmFromMsg <= tmEnd)
                    {
                        //解析故障状态位
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

                        //填充数据
                        #region
                        if (DataMngr.m_advanceMode==false)
                        {
                            //只有基础数据
                            if (DataMngr.m_machineType == 2)
                            {
                                #region
                                line = i.ToString() + ","
                                        + tmFromMsg.ToString("yyyy-MM-dd HH:mm:ss") + ","
                                        + Convert.ToString(Convert.ToBoolean(workDataMsg.SET_MODE) ? lang.atomization() : lang.humidification()) + ","
                                        + Convert.ToString(Convert.ToBoolean(workDataMsg.SET_ADULT_OR_CHILDE) ? lang.child() : lang.adault()) + ","
                                        + Convert.ToString(IsByteOxFF(workDataMsg.DATA_PATIENT_TEMP)) + ","
                                        + Convert.ToString(IsByteOxFF(workDataMsg.DATA_AIR_OUTLET_TEMP)) + ","
                                        + Convert.ToString(IsByteOxFF(workDataMsg.DATA_FLOW)) + ","
                                        + Convert.ToString(IsByteOxFF(workDataMsg.DATA_OXYGEN_CONCENTRATION))+","
                                        + Convert.ToString(IsByteOxFF(workDataMsg.DATA_DEWPOINT_TMP));      //2018/7/20 新增
                                #endregion
                            }
                            else
                            {
                                #region
                                line = i.ToString() + ","
                                        + tmFromMsg.ToString("yyyy-MM-dd HH:mm:ss") + ","
                                        //+ Convert.ToString(Convert.ToBoolean(workDataMsg.SET_MODE) ? "雾化" : "湿化") + ","
                                        + Convert.ToString(Convert.ToBoolean(workDataMsg.SET_ADULT_OR_CHILDE) ? lang.child() : lang.adault()) + ","
                                        + Convert.ToString(IsByteOxFF(workDataMsg.DATA_PATIENT_TEMP)) + ","
                                        + Convert.ToString(IsByteOxFF(workDataMsg.DATA_AIR_OUTLET_TEMP)) + ","
                                        + Convert.ToString(IsByteOxFF(workDataMsg.DATA_FLOW)) + ","
                                        + Convert.ToString(IsByteOxFF(workDataMsg.DATA_OXYGEN_CONCENTRATION)) + ","
                                        + Convert.ToString(IsByteOxFF(workDataMsg.DATA_DEWPOINT_TMP));     //2018/7/20 新增
                                #endregion
                            }
                        }
                        else
                        {
                             if (显示所有数据ToolStripMenuItem.CheckState == CheckState.Checked)
                             {
                                 //显示所有
                                 if (DataMngr.m_machineType == 2)
                                 {
                                     #region
                                     line = i.ToString() + ","
                                             + tmFromMsg.ToString("yyyy-MM-dd HH:mm:ss") + ","
                                             + Convert.ToString(Convert.ToBoolean(workDataMsg.SET_MODE) ? lang.atomization() : lang.humidification()) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.SET_TEMP)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.SET_FLOW)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.SET_HIGH_OXYGEN_ALARM)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.SET_LOW_OXYGEN_ALARM)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.SET_ATOMIZATION_LEVEL)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.SET_ATOMIZATION_TIME)) + ","
                                             + Convert.ToString(Convert.ToBoolean(workDataMsg.SET_ADULT_OR_CHILDE) ? lang.child() : lang.adault() ) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_PATIENT_TEMP)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_AIR_OUTLET_TEMP)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_HEATING_PLATE_TMP)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_ENVIRONMENT_TMP)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_DRIVERBOARD_TMP)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_FLOW)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_OXYGEN_CONCENTRATION)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_AIR_OUTLET_TEMP)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_DEWPOINT_TMP)) + ","   //  2018/7/20 新增露点温度
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_LOOP_TYPE)) + ","
                                             + (Convert.ToBoolean(faultStates[0]) ? "yes" : "no") + ","
                                             + (Convert.ToBoolean(faultStates[1]) ? "yes" : "no") + ","
                                             + (Convert.ToBoolean(faultStates[2]) ? "yes" : "no") + ","
                                             + (Convert.ToBoolean(faultStates[3]) ? "yes" : "no") + ","
                                             + (Convert.ToBoolean(faultStates[4]) ? "yes" : "no") + ","
                                             + (Convert.ToBoolean(faultStates[5]) ? "yes" : "no") + ","
                                             + (Convert.ToBoolean(faultStates[6]) ? "yes" : "no") + ","
                                             + (Convert.ToBoolean(faultStates[7]) ? "yes" : "no") + ","
                                             + (Convert.ToBoolean(faultStates[8]) ? "yes" : "no") + ","
                                             + (Convert.ToBoolean(faultStates[9]) ? "yes" : "no") + ","
                                             + (Convert.ToBoolean(faultStates[10]) ? "yes" : "no") + ","
                                             + (Convert.ToBoolean(faultStates[11]) ? "yes" : "no") + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_ATOMIZ_DACVALUE_L)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_ATOMIZ_DACVALUE_H)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_ATOMIZ_ADCVALUE_L)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_ATOMIZ_ADCVALUE_H)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_LOOP_HEATING_PWM_L)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_LOOP_HEATING_PWM_H)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_LOOP_HEATING_ADC_L)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_LOOP_HEATING_ADC_H)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_HEATING_PLATE_PWM_L)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_HEATING_PLATE_PWM_H)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_HEATING_PLATE_ADC_L)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_HEATING_PLATE_ADC_H)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_MAIN_MOTOR_DRIVER_L)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_MAIN_MOTOR_DRIVER_H)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_MAIN_MOTOR_SPEED_L)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_MAIN_MOTOR_SPEED_H)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_PRESS_SENSOR_ADC_L)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_PRESS_SENSOR_ADC_H)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_WATERLEVEL_SENSOR_HADC_L)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_WATERLEVEL_SENSOR_HADC_H)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_WATERLEVEL_SENSOR_LADC_L)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_WATERLEVEL_SENSOR_LADC_H)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_FAN_DRIVER_L)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_FAN_DRIVER_H)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_FAN_SPEED_L)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_FAN_SPEED_H))+","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_MAIN_MOTOR_TMP_ADC_L))+","  // 2018/7/20补上遗漏的
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_MAIN_MOTOR_TMP_ADC_H));  
                                     #endregion
                                 }
                                 else
                                 {
                                     #region
                                     line = i.ToString() + ","
                                             + tmFromMsg.ToString("yyyy-MM-dd HH:mm:ss") + ","
                                             //+ Convert.ToString(Convert.ToBoolean(workDataMsg.SET_MODE) ? "雾化" : "湿化") + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.SET_TEMP)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.SET_FLOW)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.SET_HIGH_OXYGEN_ALARM)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.SET_LOW_OXYGEN_ALARM)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.SET_ATOMIZATION_LEVEL)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.SET_ATOMIZATION_TIME)) + ","
                                             + Convert.ToString(Convert.ToBoolean(workDataMsg.SET_ADULT_OR_CHILDE) ? lang.child() : lang.adault()) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_PATIENT_TEMP)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_AIR_OUTLET_TEMP)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_HEATING_PLATE_TMP)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_ENVIRONMENT_TMP)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_DRIVERBOARD_TMP)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_FLOW)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_OXYGEN_CONCENTRATION)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_AIR_OUTLET_TEMP)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_DEWPOINT_TMP)) + ","   //  2018/7/20 新增露点温度
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_LOOP_TYPE)) + ","
                                             + (Convert.ToBoolean(faultStates[0]) ? "yes" : "no") + ","
                                             + (Convert.ToBoolean(faultStates[1]) ? "yes" : "no") + ","
                                             + (Convert.ToBoolean(faultStates[2]) ? "yes" : "no") + ","
                                             + (Convert.ToBoolean(faultStates[3]) ? "yes" : "no") + ","
                                             + (Convert.ToBoolean(faultStates[4]) ? "yes" : "no") + ","
                                             + (Convert.ToBoolean(faultStates[5]) ? "yes" : "no") + ","
                                             + (Convert.ToBoolean(faultStates[6]) ? "yes" : "no") + ","
                                             + (Convert.ToBoolean(faultStates[7]) ? "yes" : "no") + ","
                                             + (Convert.ToBoolean(faultStates[8]) ? "yes" : "no") + ","
                                             + (Convert.ToBoolean(faultStates[9]) ? "yes" : "no") + ","
                                             + (Convert.ToBoolean(faultStates[10]) ? "yes" : "no") + ","
                                             + (Convert.ToBoolean(faultStates[11]) ? "yes" : "no") + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_ATOMIZ_DACVALUE_L)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_ATOMIZ_DACVALUE_H)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_ATOMIZ_ADCVALUE_L)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_ATOMIZ_ADCVALUE_H)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_LOOP_HEATING_PWM_L)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_LOOP_HEATING_PWM_H)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_LOOP_HEATING_ADC_L)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_LOOP_HEATING_ADC_H)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_HEATING_PLATE_PWM_L)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_HEATING_PLATE_PWM_H)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_HEATING_PLATE_ADC_L)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_HEATING_PLATE_ADC_H)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_MAIN_MOTOR_DRIVER_L)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_MAIN_MOTOR_DRIVER_H)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_MAIN_MOTOR_SPEED_L)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_MAIN_MOTOR_SPEED_H)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_PRESS_SENSOR_ADC_L)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_PRESS_SENSOR_ADC_H)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_WATERLEVEL_SENSOR_HADC_L)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_WATERLEVEL_SENSOR_HADC_H)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_WATERLEVEL_SENSOR_LADC_L)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_WATERLEVEL_SENSOR_LADC_H)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_FAN_DRIVER_L)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_FAN_DRIVER_H)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_FAN_SPEED_L)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_FAN_SPEED_H))+","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_MAIN_MOTOR_TMP_ADC_L)) + ","  // 2018/7/20补上遗漏的
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_MAIN_MOTOR_TMP_ADC_H)); 
                                     #endregion
                                 }
                             }
                             else
                             {
                                 if (DataMngr.m_machineType == 2)
                                 {
                                     #region
                                     line = i.ToString() + ","
                                             + tmFromMsg.ToString("yyyy-MM-dd HH:mm:ss") + ","
                                             + Convert.ToString(Convert.ToBoolean(workDataMsg.SET_MODE) ? lang.atomization() : lang.humidification()) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.SET_TEMP)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.SET_FLOW)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.SET_HIGH_OXYGEN_ALARM)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.SET_LOW_OXYGEN_ALARM)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.SET_ATOMIZATION_LEVEL)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.SET_ATOMIZATION_TIME)) + ","
                                             + Convert.ToString(Convert.ToBoolean(workDataMsg.SET_ADULT_OR_CHILDE) ? lang.child() : lang.adault()) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_PATIENT_TEMP)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_AIR_OUTLET_TEMP)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_HEATING_PLATE_TMP)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_ENVIRONMENT_TMP)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_DRIVERBOARD_TMP)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_FLOW)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_OXYGEN_CONCENTRATION)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_AIR_OUTLET_TEMP)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_DEWPOINT_TMP)) + ","   //  2018/7/20 新增露点温度
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_LOOP_TYPE)) + ","
                                             + (Convert.ToBoolean(faultStates[0]) ? "yes" : "no") + ","
                                             + (Convert.ToBoolean(faultStates[1]) ? "yes" : "no") + ","
                                             + (Convert.ToBoolean(faultStates[2]) ? "yes" : "no") + ","
                                             + (Convert.ToBoolean(faultStates[3]) ? "yes" : "no") + ","
                                             + (Convert.ToBoolean(faultStates[4]) ? "yes" : "no") + ","
                                             + (Convert.ToBoolean(faultStates[5]) ? "yes" : "no") + ","
                                             + (Convert.ToBoolean(faultStates[6]) ? "yes" : "no") + ","
                                             + (Convert.ToBoolean(faultStates[7]) ? "yes" : "no") + ","
                                             + (Convert.ToBoolean(faultStates[8]) ? "yes" : "no") + ","
                                             + (Convert.ToBoolean(faultStates[9]) ? "yes" : "no") + ","
                                             + (Convert.ToBoolean(faultStates[10]) ? "yes" : "no") + ","
                                             + (Convert.ToBoolean(faultStates[11]) ? "yes" : "no");      
                                     #endregion
                                 }
                                 else
                                 {
                                     #region
                                     line = i.ToString() + ","
                                             + tmFromMsg.ToString("yyyy-MM-dd HH:mm:ss") + ","
                                             //+ Convert.ToString(Convert.ToBoolean(workDataMsg.SET_MODE) ? "雾化" : "湿化") + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.SET_TEMP)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.SET_FLOW)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.SET_HIGH_OXYGEN_ALARM)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.SET_LOW_OXYGEN_ALARM)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.SET_ATOMIZATION_LEVEL)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.SET_ATOMIZATION_TIME)) + ","
                                             + Convert.ToString(Convert.ToBoolean(workDataMsg.SET_ADULT_OR_CHILDE) ? lang.child() : lang.adault()) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_PATIENT_TEMP)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_AIR_OUTLET_TEMP)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_HEATING_PLATE_TMP)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_ENVIRONMENT_TMP)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_DRIVERBOARD_TMP)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_FLOW)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_OXYGEN_CONCENTRATION)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_AIR_OUTLET_TEMP)) + ","
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_DEWPOINT_TMP)) + ","   //  2018/7/20 新增露点温度
                                             + Convert.ToString(Convert.ToString(workDataMsg.DATA_LOOP_TYPE)) + ","
                                             + (Convert.ToBoolean(faultStates[0]) ? "yes" : "no") + ","
                                             + (Convert.ToBoolean(faultStates[1]) ? "yes" : "no") + ","
                                             + (Convert.ToBoolean(faultStates[2]) ? "yes" : "no") + ","
                                             + (Convert.ToBoolean(faultStates[3]) ? "yes" : "no") + ","
                                             + (Convert.ToBoolean(faultStates[4]) ? "yes" : "no") + ","
                                             + (Convert.ToBoolean(faultStates[5]) ? "yes" : "no") + ","
                                             + (Convert.ToBoolean(faultStates[6]) ? "yes" : "no") + ","
                                             + (Convert.ToBoolean(faultStates[7]) ? "yes" : "no") + ","
                                             + (Convert.ToBoolean(faultStates[8]) ? "yes" : "no") + ","
                                             + (Convert.ToBoolean(faultStates[9]) ? "yes" : "no") + ","
                                             + (Convert.ToBoolean(faultStates[10]) ? "yes" : "no") + ","
                                             + (Convert.ToBoolean(faultStates[11]) ? "yes" : "no");
                                     #endregion
                                 }
                             }
                        }
                        #endregion
                        sw_workData.WriteLine(line);
                        i++;
                    }
                    #endregion 
                }
            }
            #endregion

            MessageBox.Show(LanguageMngr.file_export_completed());
            sw_alarm.Close();
            sw_workData.Close();
            fs_workData.Close();
            fs_alarm.Close();
        }

        private void 帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void InitListViewColumnHead_alarm()
        {
            LanguageMngr lang = new LanguageMngr();
            //获取机型号
            DataMngr.GetMachineTpye();
            this.listView_alarmInfo.Columns.Clear();

            this.listView_alarmInfo.Columns.Add("No.", 120, HorizontalAlignment.Left);
            this.listView_alarmInfo.Columns.Add(lang.date(), 180, HorizontalAlignment.Left);
            if (DataMngr.m_machineType == 2) //机型为VNU002是需要运行模式
            {
                this.listView_alarmInfo.Columns.Add(lang.running_mode(), 120, HorizontalAlignment.Left);
            } 
            else 
            {
                //do nothing
            }
            this.listView_alarmInfo.Columns.Add(lang.alarm_code(), 120, HorizontalAlignment.Left);
            this.listView_alarmInfo.Columns.Add(lang.alarm_info(), 120, HorizontalAlignment.Left);
            this.listView_alarmInfo.Columns.Add(lang.alarm_value_L(), 120, HorizontalAlignment.Left);
            this.listView_alarmInfo.Columns.Add(lang.alarm_value_H(), 120, HorizontalAlignment.Left);
        }

        private void InitTree()
        {
            #region
            ////测试用例
            //List<string> list_test = new List<string>();
            //list_test.Add("DATA20171101");
            //list_test.Add("DATA20171102");
            //list_test.Add("DATA20171102");
            //list_test.Add("DATA20161204");
            //list_test.Add("DATA20171205");
            //list_test.Add("DATA20181006");
            //list_test.Add("DATA20181007");
            //list_test.Add("DATA20181207");
            //list_test.Add("DATA20181209");
            #endregion
            #region
            if (this.treeView_detailChart != null)
            {
                this.treeView_detailChart.Nodes.Clear();
            }

            List<TreeNode> nodeList_year = new List<TreeNode>();
            List<TreeNode> nodeList_month = new List<TreeNode>();

            int i = 0;
            int j = 0;
            TreeNode node_year = new TreeNode();
            TreeNode node_month = new TreeNode();
            string prev_Year = "";
            string prev_Month = "";
            foreach(var fileName in FileMngr.m_workFileName_CanBeOpened_List)
            //foreach (var fileName in list_test)
            {
                //文件名举例：DATA20171210
                string strYear = fileName.Substring(4, 4);
                string strMonth = fileName.Substring(8, 2);
                string strDay = fileName.Substring(10, 2);

                DateTime tm_Begin = this.dateTimePicker_Begin.Value;
                DateTime tm_End = this.dateTimePicker_End.Value;
                DateTime tmBegin = new DateTime(tm_Begin.Year, tm_Begin.Month, tm_Begin.Day, 0, 0, 0);
                DateTime tmEnd = new DateTime(tm_End.Year, tm_End.Month, tm_End.Day, 23, 59, 59);

                DateTime tm = new DateTime(Convert.ToInt32(strYear), Convert.ToInt32(strMonth), Convert.ToInt32(strDay), 0, 0, 0);

                //if (tm >= tmBegin && tm <= tmEnd)
                {
                    #region
                    TreeNode node_day = new TreeNode();

                    node_year.Text = strYear;
                    node_month.Text = strMonth;
                    node_day.Text = strDay;

                    if (prev_Year != strYear)
                    {
                        i++;
                        TreeNode tmp = new TreeNode();
                        tmp.Text = strYear;
                        nodeList_year.Add(tmp);
                        this.treeView_detailChart.Nodes.Add(nodeList_year[i - 1]);
                        if (prev_Month == strMonth)
                        {
                            j++;
                            TreeNode tmp1 = new TreeNode();
                            tmp1.Text = strMonth;
                            nodeList_month.Add(tmp1);
                            nodeList_year[i - 1].Nodes.Add(nodeList_month[j - 1]);
                        }
                    }
                    if (prev_Month != strMonth)
                    {
                        j++;
                        TreeNode tmp1 = new TreeNode();
                        tmp1.Text = strMonth;
                        nodeList_month.Add(tmp1);
                        nodeList_year[i - 1].Nodes.Add(nodeList_month[j - 1]);
                    }

                    nodeList_month[j - 1].Nodes.Add(node_day);

                    prev_Year = strYear;
                    prev_Month = strMonth;
                    #endregion
                }
                
            }
            #endregion
        }

        private void InitListViewColumnHead_workData()
        {
            LanguageMngr lang=new LanguageMngr();
            //获取机型号
            DataMngr.GetMachineTpye();
            //MessageBox.Show(Convert.ToInt32(DataMngr.m_usageTable_xAxis_list).ToString());
            //根据机型号来初始化工作信息列表
            #region
            this.listView_workData.Columns.Clear();
            if (!DataMngr.m_advanceMode) //非高级模式时，给用户看，只需要部分数据
            {
                this.listView_workData.Columns.Add("No.", 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.date(), 180, HorizontalAlignment.Left);
                if (DataMngr.m_machineType == 2) //机型为VNU002是需要运行模式
                {
                    this.listView_workData.Columns.Add(lang.running_mode(), 120, HorizontalAlignment.Left);
                }
                this.listView_workData.Columns.Add(lang.set_adault_or_child(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_patient_tmp(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_air_outlet_tmp(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_flow(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_Oxy_concentration(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_dewpoint_tmp(), 120, HorizontalAlignment.Left);   //2018/7/20添加
            }
            else  //高级模式，给工程师看
            {
                #region
                this.listView_workData.Columns.Add("No.", 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.date(), 180, HorizontalAlignment.Left);
                if (DataMngr.m_machineType == 2)
                {
                    this.listView_workData.Columns.Add(lang.running_mode(), 120, HorizontalAlignment.Left);
                }
                this.listView_workData.Columns.Add(lang.set_temp(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.set_flow(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_dewpoint_tmp(), 120, HorizontalAlignment.Left);  //2018/7/20添加
                this.listView_workData.Columns.Add(lang.set_high_oxy_alarm(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.set_low_oxy_alarm(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.set_atomiz_level(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.set_atomiz_time(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.set_adault_or_child(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_patient_tmp(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_air_outlet_tmp(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_heating_plate_tmp(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_enviroment_tmp(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_driveboard_tmp(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_flow(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_Oxy_concentration(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_air_pressure(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_loop_type(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_fault_statue1(), 120, HorizontalAlignment.Left);   //报警码
                this.listView_workData.Columns.Add(lang.data_fault_statue2(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_fault_statue3(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_fault_statue4(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_fault_statue5(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_fault_statue6(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_fault_statue7(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_fault_statue8(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_fault_statue9(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_fault_statue10(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_fault_statue11(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_fault_statue12(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_atomiz_DAC_L(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_atomiz_DAC_H(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_atomiz_ADC_L(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_atomiz_ADC_L(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_loop_heating_PWM_L(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_loop_heating_PWM_H(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_loop_heating_ADC_L(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_loop_heating_ADC_H(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_heating_plate_PWM_L(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_heating_plate_PWM_H(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_heating_plate_ADC_L(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_heating_plate_ADC_H(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_main_motor_drive_L(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_main_motor_drive_H(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_main_motor_speed_L(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_main_motor_speed_H(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_press_sensor_ADC_L(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_press_sensor_ADC_H(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_waterlevel_HADC_L(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_waterlevel_HADC_H(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_waterlevel_LADC_L(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_waterlevel_LADC_H(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_fan_driver_L(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_fan_driver_H(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_fan_speed_L(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_fan_speed_H(), 120, HorizontalAlignment.Left);
                this.listView_workData.Columns.Add(lang.data_main_motor_tmp_ADC_L(), 120, HorizontalAlignment.Left);  //2018/7/20添加
                this.listView_workData.Columns.Add(lang.data_main_motor_tmp_ADC_H(), 120, HorizontalAlignment.Left);
                #endregion
                if(DataMngr.m_advanceMode)
                {
                    int start;
                    int end;
                    if (DataMngr.m_machineType == 2)
                    {
                        start = 31 + 1;   //2018/7/20添加
                        end = 56 + 1+2;   //2018/7/20添加
                    }
                    else 
                    {
                        start = 30+1;     //2018/7/20添加
                        end = 55 + 1+2;   //2018/7/20添加
                    }
                    for (int i = start; i <= end; i++)
                    {
                        this.listView_workData.Columns[i].Width = 0;
                    }
                }
            }
            #endregion
        }
        private void InitDetailChartsSize()
        {
            DataMngr.m_chartSize = new CHART_SIZE();
            DataMngr.m_chartSize.Width = 1100;
            DataMngr.m_chartSize.Height = 200;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //debug,产生测试文档
            #region
            FileMngr.m_bCreateTestFiles = false;
            //FileMngr.m_bCreateTestFiles = true;
            if (FileMngr.m_bCreateTestFiles)
            {
                if (Directory.Exists("C:/Users/Administrator/Desktop/SD_TEST/170000000001"))
                {
                    string[] strPathes = Directory.GetFiles("C:/Users/Administrator/Desktop/SD_TEST/170000000001", "*.vmf");
                    foreach(var path in strPathes)
                    {
                        File.Delete(path);
                    }
                    //Directory.Delete("C:/Users/Administrator/Desktop/SD_TEST/170000000001");
                }

                DateTime tmBegin=new DateTime(2017,1,1,0,0,0);
                //产生多少天数据
                Int32 duration = 365*2;
                FileMngr.CreateTestFiles(tmBegin, duration);
            }
            #endregion
            //工作参数不需要了，直接隐藏起来
            this.groupBox_workingParam.Visible = false;


            //初始化高级模式！！！
            DataMngr.m_advanceMode = false;  //默认开启高级模式，这里以后用户使用的话，改成false
            if (DataMngr.m_advanceMode==false)
            {
                this.高级模式ToolStripMenuItem.Visible = false;
            }

            this.用户模式ToolStripMenuItem.Visible = false;
            this.工程师模式ToolStripMenuItem.Visible = false;

            //初始化工程师模式
            this.g_bEngineerMode = true;
            //this.g_flag_alreadyInEngMode = false;
            //初始化，默认用户模式按钮不能点
            this.用户模式ToolStripMenuItem.Enabled = false;
            //初始化语言
            #region
            ConfigurMngr mngr = new ConfigurMngr();
            mngr.LoadCfg();
            LanguageMngr.m_language = mngr.m_lang;
            //初始化为客户保留的语言
            ShowLabelNameByLanguageType(LanguageMngr.m_language);
            #endregion

            //初始化基本信息中的各个参数值,都为空
            #region
            this.label_equipType_Value.Text = "";
            this.label_runningMode_value.Text = "";
            this.label_setAtmoizLevel_Value.Text = "";
            this.label_setAtomizTime_Value.Text = "";
            this.label_setFlow_Value.Text = "";
            this.label_setHighOxyContAlarm_Value.Text = "";
            this.label_setLowOxyContAlarm_Value.Text = "";
            this.label_setTmp_Value.Text = "";
            this.label_SN_Value.Text = "";
            this.label_softwarVer_Value.Text = "";
            this.label_setAdaultChild_Value.Text = "";
            #endregion

            //初始化详细图表的大小
            InitDetailChartsSize();

            //初始化FileMngr中的链表
            FileMngr.m_workFileNameList = new List<string>();
            FileMngr.m_workFileName_CanBeOpened_List = new List<string>();
            FileMngr.m_alarmMsgList = new List<ALARM_INFO_MESSAGE>();
            FileMngr.m_workHead_Msg_Map = new Dictionary<WORK_INFO_HEAD, List<WORK_INFO_MESSAGE>>();

            DataMngr.m_listInfo = new List<DETAIL_CHART_INFO>();

            //listview虚列表初始化
            this.listView_alarmInfo.View = View.Details;
            this.listView_alarmInfo.VirtualMode = true;
            this.listView_alarmInfo.RetrieveVirtualItem += new RetrieveVirtualItemEventHandler(listViewAlarmData_RetrieveVirtualItem);

            this.listView_workData.View = View.Details;
            //this.listView_workData.VirtualMode = true;
            //this.listView_workData.RetrieveVirtualItem += new RetrieveVirtualItemEventHandler(listViewWorkData_RetrieveVirtualItem);

          
        }
        private void listViewAlarmData_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            if (this.myCache == null || this.myCache.Count == 0)
            {
                return;
            }

            e.Item = this.myCache[e.ItemIndex];
            if (e.ItemIndex == this.myCache.Count)
            {
                this.myCache = null;
            }
            #region
            //if (myCache != null )
             //{
             //   e.Item = this.myCache[e.ItemIndex];
             //   if (e.ItemIndex == this.myCache.Count)
             //   {
             //       this.myCache = null;
             //   }
             //}
             //else
             //{
             //    //A cache miss, so create a new ListViewItem and pass it back.
             //    int x = e.ItemIndex * e.ItemIndex;
             //    e.Item = new ListViewItem(x.ToString());
            //}
            #endregion
        }

        //private void listViewWorkData_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        //{
        //    if (this.myCache1 == null || this.myCache1.Count == 0)
        //    {
        //        return;
        //    }

        //    e.Item = this.myCache1[e.ItemIndex];
        //    if (e.ItemIndex == this.myCache1.Count)
        //    {
        //        this.myCache1 = null;
        //    }
        //}
        private void InitDateTimePicer()
        {
            //修复一个bug,切换文件夹时，会判断MinDate和MaxDate，如果begin的MinDate和MaxDate
            //大于end的MinDate和MaxDate，就会报错
            this.dateTimePicker_Begin.MinDate = new DateTime(1900, 1, 1, 0, 0, 0);
            this.dateTimePicker_Begin.MaxDate = new DateTime(9998, 12, 31, 0, 0, 0);
            this.dateTimePicker_End.MinDate = new DateTime(1900, 1, 1, 0, 0, 0);
            this.dateTimePicker_End.MaxDate = new DateTime(9998, 12, 31, 0, 0, 0);

            //设置控件当前值
            this.dateTimePicker_Begin.Value = FileMngr.m_DateTime_min;
            this.dateTimePicker_End.Value = FileMngr.m_DateTime_max;

            //设置控件范围的时间选择范围
            this.dateTimePicker_Begin.MinDate = FileMngr.m_DateTime_min;
            this.dateTimePicker_End.MinDate = FileMngr.m_DateTime_min;
            
            this.dateTimePicker_Begin.MaxDate = FileMngr.m_DateTime_max;
            this.dateTimePicker_End.MaxDate = FileMngr.m_DateTime_max;
            
            //设置时间控件的显示范围，默认显示3个月
            if((this.dateTimePicker_End.Value-this.dateTimePicker_Begin.Value).TotalDays>=DataMngr.m_DateTimePicker_Range_Limit)
            {
                this.dateTimePicker_Begin.Value = this.dateTimePicker_End.Value.AddDays(0 - DataMngr.m_DateTimePicker_Range_Limit);
            }

        }


        private void Form1_Shown(object sender, EventArgs e)
        {
            //MessageBox.Show("I see U");
            FileMngr.m_dateTime_begin = this.dateTimePicker_Begin.Value;
            FileMngr.m_dateTime_end = this.dateTimePicker_End.Value;
            this.dateTimePicker_Begin.Enabled = false;
            this.dateTimePicker_End.Enabled = false;
            if (!this.g_bEngineerMode)
            {
                this.tabPage_alarmList.Parent = null;
                this.tabPage_workdatalist.Parent = null;
            }
        }

        private void groupBox5_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(this.BackColor);
        }



        private void 工程师模式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form_engineer = new Form_engineer();

            if (this.g_bEngineerMode == true)
            {
                MessageBox.Show(LanguageMngr.u_r_already_in_eng_mode());
                return;
            }
            form_engineer.ShowDialog();
            if (g_username == "eng004" && g_password == "eng004")
            {
                MessageBox.Show(LanguageMngr.welcom_enterinto_eng_mode());
                //this.g_flag_alreadyInEngMode = true;
                this.g_bEngineerMode = true;
                //先初始化报警信息和工作信息列表的语言
                #region
                if (LanguageMngr.m_language==LANGUAGE.CHINA)
                {
                    this.tabPage_alarmList.Text = "报警信息";
                    this.tabPage_workdatalist.Text = "工作信息";
                }
                else if (LanguageMngr.m_language == LANGUAGE.ENGLISH)
                {
                    this.tabPage_alarmList.Text = "Alarm Info.";
                    this.tabPage_workdatalist.Text = "Work Info";
                }
                else
                {

                }
                #endregion
                this.tabPage_alarmList.Parent = this.tabControl1;   //隐藏报警列表
                this.tabPage_workdatalist.Parent = this.tabControl1; //隐藏工作列表
                this.用户模式ToolStripMenuItem.Enabled = true;      //启动用户模式按钮
            }

        }

        private void 用户模式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.g_bEngineerMode = false;
            //this.g_flag_alreadyInEngMode = false;
            //if (this.用户模式ToolStripMenuItem.Enabled==false)
            //{
            //    if(LanguageMngr.m_language==LANGUAGE.CHINA)
            //    {
            //        MessageBox.Show("您已经处于用户模式！");
            //    }
            //    else if(LanguageMngr.m_language==LANGUAGE.ENGLISH)
            //    {
            //        MessageBox.Show("You are already in user mode!");
            //    }
            //    else
            //    {

            //    }
            //}
            this.tabPage_alarmList.Parent = null;
            this.tabPage_workdatalist.Parent = null;
            ////试一试,貌似没起什么作用，内存并没有释放，有可能是因为使用了虚模式
            ////可以虚模式下的内存并没有释放
            //myCache.Clear();
            //myCache1.Clear();
            //this.listView_alarmInfo.Items.Clear();
            //this.listView_workData.Items.Clear();
            //this.listView_alarmInfo.Dispose();
            //this.listView_workData.Dispose();
        }

        public void ShowWorkdataListPage(object list, int page)
        {
            if(list==null)
            {
                return;
            }
            #region
            if (WorkDataList.m_nCurrentPage <= 1)
            {
                WorkDataList.m_nCurrentPage = 1;
                page = WorkDataList.m_nCurrentPage;
            }

            if(WorkDataList.m_nCurrentPage>=WorkDataList.m_nPageCount)
            {
                WorkDataList.m_nCurrentPage = WorkDataList.m_nPageCount;
                page = WorkDataList.m_nCurrentPage;
            }
            #endregion

            int start = (page - 1) * WorkDataList.m_nPageSize;
            int end ;

            if (page == WorkDataList.m_nPageCount)
            {
                if (WorkDataList.m_nCount % WorkDataList.m_nPageSize == 0)
                {
                    end = start + WorkDataList.m_nPageSize;
                }
                else
                {
                    end = start + WorkDataList.m_nCount % WorkDataList.m_nPageSize;
                }
            }
            else
            {
                end = start + WorkDataList.m_nPageSize;
            }
            
            //显示
            this.textBox_listview_currentpage.Text = page.ToString() + "/" + WorkDataList.m_nPageCount.ToString();

            this.listView_workData.Items.Clear();
            this.listView_workData.BeginUpdate();

            
            LanguageMngr lang=new LanguageMngr();
            for (int i = start; i < end; ++i)
            {
                #region
                ListViewItem lvi = new ListViewItem();
                if(DataMngr.m_advanceMode)
                {
                    #region
                    lvi.Text = WorkDataList.m_WorkData_List[i].No;
                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].tm);
                    if(DataMngr.m_machineType==2)
                    {
                        lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].set_mode == "雾化" ? lang.atomization() : lang.humidification());
                    }
                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].set_tmp);
                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].set_flow);
                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].set_high_oxy_alarm);
                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].set_low_oxy_alrm);
                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].set_atomiz_level);
                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].set_atomiz_time);
                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].set_adault_or_child);

                    //患者端温度
                    if (WorkDataList.m_WorkData_List[i].data_patient_tmp == Convert.ToString(255))
                    {
                        lvi.SubItems.Add(@"/");
                    }
                    else
                    {
                        lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].data_patient_tmp);
                    }
                    //出气口温度
                    if (WorkDataList.m_WorkData_List[i].data_air_outlet_tmp == Convert.ToString(255))
                    {
                        lvi.SubItems.Add(@"/");
                    }
                    else
                    {
                        lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].data_air_outlet_tmp);
                    }

                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].data_heating_plate_tmp);
                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].data_env_tmp);
                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].data_driveboard_tmp);

                    //流量
                    if (WorkDataList.m_WorkData_List[i].data_flow == Convert.ToString(255))
                    {
                        lvi.SubItems.Add(@"/");
                    }
                    else
                    {
                        lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].data_flow);
                    }
                    //氧浓度
                    if (WorkDataList.m_WorkData_List[i].data_oxy_concentration == Convert.ToString(255))
                    {
                        lvi.SubItems.Add(@"/");
                    }
                    else
                    {
                        lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].data_oxy_concentration);
                    }
                    //露点温度
                    if (WorkDataList.m_WorkData_List[i].data_dewpoint_tmp == Convert.ToString(255))
                    {
                        lvi.SubItems.Add(@"/");
                    }
                    else
                    {
                        lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].data_dewpoint_tmp);     // 2018/7/20新增
                    }

                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].data_air_pressure);
                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].data_loop_type);
                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].data_faultstates_0);
                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].data_faultstates_1);
                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].data_faultstates_2);
                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].data_faultstates_3);
                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].data_faultstates_4);
                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].data_faultstates_5);
                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].data_faultstates_6);
                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].data_faultstates_7);
                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].data_faultstates_8);
                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].data_faultstates_9);
                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].data_faultstates_10);
                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].data_faultstates_11);
                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].data_atmoz_DAC_L);
                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].data_atmoz_DAC_H);
                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].data_atmoz_ADC_L);
                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].data_atmoz_ADC_H);
                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].data_loop_heating_PWM_L);
                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].data_loop_heating_PWM_H);
                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].data_loop_heating_ADC_L);
                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].data_loop_heating_ADC_H);
                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].data_loop_heating_plate_PWM_L);
                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].data_loop_heating_plate_PWM_H);
                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].data_loop_heating_plate_ADC_L);
                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].data_loop_heating_plate_ADC_H);
                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].data_main_motor_drive_L);
                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].data_main_motor_drive_H);
                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].data_main_motor_speed_L);
                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].data_main_motor_speed_H);
                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].data_press_sensor_ADC_L);
                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].data_press_sensor_ADC_H);
                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].data_waterlevel_sensor_HADC_L);
                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].data_waterlevel_sensor_HADC_H);
                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].data_waterlevel_sensor_LADC_L);
                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].data_waterlevel_sensor_LADC_H);
                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].data_fan_driver_L);
                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].data_fan_driver_H);
                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].data_fan_speed_L);
                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].data_fan_speed_H);
                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].data_main_motor_tmp_ADC_L);   //2018/7/20 补上遗漏的
                    lvi.SubItems.Add(WorkDataList.m_WorkData_List[i].data_main_motor_tmp_ADC_H);
                    #endregion
                }
                else
                {
                    #region
                    lvi.Text = WorkDataList.m_WorkData_Basic_List[i].No;
                    lvi.SubItems.Add(WorkDataList.m_WorkData_Basic_List[i].tm);
                    if(DataMngr.m_machineType==2)
                    {
                        lvi.SubItems.Add(WorkDataList.m_WorkData_Basic_List[i].set_mode);
                    }
                    lvi.SubItems.Add(WorkDataList.m_WorkData_Basic_List[i].set_adault_or_child);
                    //患者端温度
                    if (WorkDataList.m_WorkData_Basic_List[i].data_patient_tmp == "255")
                    {
                        lvi.SubItems.Add(@"/");
                    }
                    else
                    {
                        lvi.SubItems.Add(WorkDataList.m_WorkData_Basic_List[i].data_patient_tmp);
                    }
                    //lvi.SubItems.Add(WorkDataList.m_WorkData_Basic_List[i].data_patient_tmp);
                    //出气口温度
                    if (WorkDataList.m_WorkData_Basic_List[i].data_air_outlet_tmp == "255")
                    {
                        lvi.SubItems.Add(@"/");
                    }
                    else
                    {
                        lvi.SubItems.Add(WorkDataList.m_WorkData_Basic_List[i].data_air_outlet_tmp);
                    }
                    //lvi.SubItems.Add(WorkDataList.m_WorkData_Basic_List[i].data_air_outlet_tmp);
                    //流量
                    if (WorkDataList.m_WorkData_Basic_List[i].data_flow == "255")
                    {
                        lvi.SubItems.Add(@"/");
                    }
                    else
                    {
                        lvi.SubItems.Add(WorkDataList.m_WorkData_Basic_List[i].data_flow);
                    }
                    //lvi.SubItems.Add(WorkDataList.m_WorkData_Basic_List[i].data_flow);
                    //氧浓度
                    if (WorkDataList.m_WorkData_Basic_List[i].data_oxy_concentration == "255")
                    {
                        lvi.SubItems.Add(@"/");
                    }
                    else
                    {
                        lvi.SubItems.Add(WorkDataList.m_WorkData_Basic_List[i].data_oxy_concentration);
                    }
                    //露点温度
                    if (WorkDataList.m_WorkData_Basic_List[i].data_dewpoint_tmp == "255")
                    {
                        lvi.SubItems.Add(@"/");
                    }
                    else
                    {
                        lvi.SubItems.Add(WorkDataList.m_WorkData_Basic_List[i].data_dewpoint_tmp);   // 2018/7/20 新增
                    }
                    //lvi.SubItems.Add(WorkDataList.m_WorkData_Basic_List[i].data_oxy_concentration);
                    #endregion
                }

                this.listView_workData.Items.Add(lvi);
                #endregion
            }
            
            this.listView_workData.EndUpdate();   
        }

        private void 高级模式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            

        }

        private void button_listview_toppage_Click(object sender, EventArgs e)
        {
            ShowCurrentPage(1);   
        }

        

        private void dateTimePicker_BeginOrEnd_CloseUp(BEGIN_END begin_end)
        {
            #region
            //校验开始时间，结束时间的正确性
            DateTime tmp1 = this.dateTimePicker_Begin.Value;
            DateTime tmp2 = this.dateTimePicker_End.Value;
            #region
            //if (tmp1 < FileMngr.m_DateTime_min)
            //{
            //    this.dateTimePicker_Begin.Value = FileMngr.m_DateTime_min;
            //    MessageBox.Show("当前最小日期为:" + this.dateTimePicker_Begin.Value.ToString("yyyy-MM-dd"));
            //}
            #endregion

            if (tmp1 > tmp2)
            {
                this.dateTimePicker_Begin.Value = FileMngr.m_dateTime_begin;
                this.dateTimePicker_End.Value = FileMngr.m_dateTime_end;
                MessageBox.Show(LanguageMngr.startTime_begyond_endTime());
                return;
            }
            else
            {
                FileMngr.m_dateTime_begin = tmp1;
                FileMngr.m_dateTime_end = tmp2;
            }

            //设置时间范围，最多跨度为3个月
            if ((this.dateTimePicker_End.Value - this.dateTimePicker_Begin.Value).TotalDays >= DataMngr.m_DateTimePicker_Range_Limit)
            {
                if (begin_end == BEGIN_END.BEGIN)
                {
                    this.dateTimePicker_End.Value = this.dateTimePicker_Begin.Value.AddDays(DataMngr.m_DateTimePicker_Range_Limit);
                }
                else if (begin_end == BEGIN_END.END)
                {
                    this.dateTimePicker_Begin.Value = this.dateTimePicker_End.Value.AddDays(0 - DataMngr.m_DateTimePicker_Range_Limit);
                }
                else
                {
                    //do nothing
                }
            }

            //规避bug
            DataMngr.m_bDateTimePicker_ValueChanged = true;

            ////初始化treeView
            //InitTree();

            if (myCache != null)
            {
                myCache.Clear();
            }
            myCache = new List<ListViewItem>();


            //初始化app面板上，基本信息中的时间
            this.label_dateTo_Value.Text = this.dateTimePicker_End.Value.ToString("yyyy/MM/dd");
            this.label_dateFrom_Value.Text = this.dateTimePicker_Begin.Value.ToString("yyyy/MM/dd");

            tmp1 = this.dateTimePicker_Begin.Value;
            tmp2 = this.dateTimePicker_End.Value;
            DateTime TmLow = new DateTime(tmp1.Year, tmp1.Month, tmp1.Day, 0, 0, 0);
            DateTime TmHight = new DateTime(tmp2.Year, tmp2.Month, tmp2.Day, 23, 59, 59);

            //if (g_bEngineerMode)
            {
                //显示报警列表
                ShowAlarmList(TmLow, TmHight);

                //将工作列表存入m_WorkData_List中，不会显示，需要点首页来触发
                //为了实现分页功能
                if (WorkDataList.m_WorkData_List == null)
                {
                    WorkDataList.InitWorkDataList(TmLow, TmHight);
                }
                else
                {
                    WorkDataList.m_WorkData_List.Clear();
                    WorkDataList.InitWorkDataList(TmLow, TmHight);
                }
            }
            ShowUsageChart(this.dateTimePicker_Begin.Value, this.dateTimePicker_End.Value);

            //将工作信息的内容填充，相当于点了一下首页
            ShowCurrentPage(1);
            #endregion
        }

        private void dateTimePicker_Begin_CloseUp(object sender, EventArgs e)
        {
            #region
            ////校验开始时间，结束时间的正确性
            //DateTime tmp1 = this.dateTimePicker_Begin.Value;
            //DateTime tmp2 = this.dateTimePicker_End.Value;
            //#region
            ////if (tmp1 < FileMngr.m_DateTime_min)
            ////{
            ////    this.dateTimePicker_Begin.Value = FileMngr.m_DateTime_min;
            ////    MessageBox.Show("当前最小日期为:" + this.dateTimePicker_Begin.Value.ToString("yyyy-MM-dd"));
            ////}
            //#endregion

            //if (tmp1 > tmp2)
            //{
            //    this.dateTimePicker_Begin.Value = FileMngr.m_dateTime_begin;
            //    this.dateTimePicker_End.Value = FileMngr.m_dateTime_end;
            //    MessageBox.Show(LanguageMngr.startTime_begyond_endTime());
            //    return;
            //}
            //else
            //{
            //    FileMngr.m_dateTime_begin = tmp1;
            //    FileMngr.m_dateTime_end = tmp2;
            //}

            ////设置时间范围，最多跨度为3个月
            //if ((this.dateTimePicker_End.Value - this.dateTimePicker_Begin.Value).TotalDays>=DataMngr.m_DateTimePicker_Range_Limit)
            //{
            //    this.dateTimePicker_End.Value = this.dateTimePicker_Begin.Value.AddDays(DataMngr.m_DateTimePicker_Range_Limit);
                
            //}

            ////规避bug
            //DataMngr.m_bDateTimePicker_ValueChanged = true;

            //////初始化treeView
            ////InitTree();

            //if (myCache != null)
            //{
            //    myCache.Clear();
            //}
            //myCache = new List<ListViewItem>();


            ////初始化app面板上，基本信息中的时间
            //this.label_dateTo_Value.Text = this.dateTimePicker_End.Value.ToString("yyyy/MM/dd");
            //this.label_dateFrom_Value.Text = this.dateTimePicker_Begin.Value.ToString("yyyy/MM/dd");

            //tmp1 = this.dateTimePicker_Begin.Value;
            //tmp2 = this.dateTimePicker_End.Value;
            //DateTime TmLow = new DateTime(tmp1.Year, tmp1.Month, tmp1.Day, 0, 0, 0);
            //DateTime TmHight = new DateTime(tmp2.Year, tmp2.Month, tmp2.Day, 23, 59, 59);

            ////if (g_bEngineerMode)
            //{
            //    //显示报警列表
            //    ShowAlarmList(TmLow, TmHight);

            //    //将工作列表存入m_WorkData_List中，不会显示，需要点首页来触发
            //    //为了实现分页功能
            //    if (WorkDataList.m_WorkData_List == null )
            //    {
            //        WorkDataList.InitWorkDataList(TmLow, TmHight);
            //    }
            //    else
            //    {
            //        WorkDataList.m_WorkData_List.Clear();
            //        WorkDataList.InitWorkDataList(TmLow, TmHight);
            //    }
            //}
            //ShowUsageChart(this.dateTimePicker_Begin.Value, this.dateTimePicker_End.Value);

            ////将工作信息的内容填充，相当于点了一下首页
            //ShowCurrentPage(1);
            #endregion
            dateTimePicker_BeginOrEnd_CloseUp(BEGIN_END.BEGIN);
        }

        private void button_listview_prev_Click(object sender, EventArgs e)
        {
            ShowCurrentPage(WorkDataList.m_nCurrentPage - 1);
        }

        private void button_listview_next_Click(object sender, EventArgs e)
        {
            ShowCurrentPage(WorkDataList.m_nCurrentPage + 1);
        }

        private void ShowCurrentPage(int currentPage)
        {
            if (DataMngr.m_advanceMode)
            {
                if (WorkDataList.m_WorkData_List == null || WorkDataList.m_WorkData_List.Count == 0)
                {
                    textBox_listview_currentpage.Text = "";
                    listView_workData.Items.Clear();
                    MessageBox.Show(LanguageMngr.no_data_in_the_time_span());
                    return;
                }
                WorkDataList.m_nCurrentPage = currentPage;
                ShowWorkdataListPage(WorkDataList.m_WorkData_List, WorkDataList.m_nCurrentPage);
            }
            else
            {
                if (WorkDataList.m_WorkData_Basic_List == null || WorkDataList.m_WorkData_Basic_List.Count == 0)
                {
                    textBox_listview_currentpage.Text = "";
                    listView_workData.Items.Clear();
                    MessageBox.Show(LanguageMngr.no_data_in_the_time_span());
                    return;
                }
                WorkDataList.m_nCurrentPage = currentPage;
                ShowWorkdataListPage(WorkDataList.m_WorkData_Basic_List, WorkDataList.m_nCurrentPage);
            }
        }

        private void button_listview_endpage_Click(object sender, EventArgs e)
        {
            ShowCurrentPage(WorkDataList.m_nPageCount);
        }

        private void textBox_jumpto_TextChanged(object sender, EventArgs e)
        {
            var str = textBox_jumpto.Text;
            if(str.Length>=10)
            {
                return;
            }
            if(str=="")
            {
                return;
            }
            for(int i=0;i<str.Length;i++)
            {
                var ch=str[i];
                if(ch<'0'||ch>'9')
                {
                    return;
                }
            }

            var value = Convert.ToInt32(textBox_jumpto.Text);
            if (value >= 1 && value <= WorkDataList.m_nPageCount)
            {
                WorkDataList.m_nCurrentPage = value;
                if(DataMngr.m_advanceMode)
                {
                    ShowWorkdataListPage(WorkDataList.m_WorkData_List, value);
                }
                else
                {
                    ShowWorkdataListPage(WorkDataList.m_WorkData_Basic_List, value);
                }
            }
        }

        private void dateTimePicker_End_CloseUp(object sender, EventArgs e)
        {
            #region
            ////校验开始时间，结束时间的正确性
            //DateTime tmp1 = this.dateTimePicker_Begin.Value;
            //DateTime tmp2 = this.dateTimePicker_End.Value;

            //#region
            //if (tmp1 > tmp2)
            //{
            //    this.dateTimePicker_Begin.Value = FileMngr.m_dateTime_begin;
            //    this.dateTimePicker_End.Value = FileMngr.m_dateTime_end;
            //    MessageBox.Show(LanguageMngr.startTime_begyond_endTime());
            //    return;
            //}
            //else
            //{
            //    FileMngr.m_dateTime_begin = tmp1;
            //    FileMngr.m_dateTime_end = tmp2;
            //}

            ////设置时间范围，最多跨度为3个月
            //if ((this.dateTimePicker_End.Value - this.dateTimePicker_Begin.Value).TotalDays >= DataMngr.m_DateTimePicker_Range_Limit)
            //{
            //    this.dateTimePicker_Begin.Value = this.dateTimePicker_End.Value.AddDays(0-DataMngr.m_DateTimePicker_Range_Limit);
            //}
            //#endregion

            ////规避bug,虚模式问题
            //DataMngr.m_bDateTimePicker_ValueChanged = true;

            //////初始化treeView
            ////InitTree();

            //if (myCache != null)
            //{
            //    myCache.Clear();
            //}
            //myCache = new List<ListViewItem>();


            ////初始化app面板上，基本信息中的时间
            //this.label_dateFrom_Value.Text = this.dateTimePicker_Begin.Value.ToString("yyyy/MM/dd");
            //this.label_dateTo_Value.Text = this.dateTimePicker_End.Value.ToString("yyyy/MM/dd");
            //tmp1 = this.dateTimePicker_Begin.Value;
            //tmp2 = this.dateTimePicker_End.Value;
            //DateTime TmLow = new DateTime(tmp1.Year, tmp1.Month, tmp1.Day, 0, 0, 0);
            //DateTime TmHight = new DateTime(tmp2.Year, tmp2.Month, tmp2.Day, 23, 59, 59);

            ////if (g_bEngineerMode)
            //{
            //    ShowAlarmList(TmLow, TmHight);

            //    //将工作列表存入m_WorkData_List中，不会显示，需要点首页来触发
            //    if (WorkDataList.m_WorkData_List == null)
            //    {
            //        WorkDataList.InitWorkDataList(TmLow, TmHight);
            //    }
            //    else
            //    {
            //        WorkDataList.m_WorkData_List.Clear();
            //        WorkDataList.InitWorkDataList(TmLow, TmHight);
            //    }
            //}
            //ShowUsageChart(this.dateTimePicker_Begin.Value, this.dateTimePicker_End.Value);
            ////将工作信息的内容填充，相当于点了一下首页
            //ShowCurrentPage(1);
            #endregion
            dateTimePicker_BeginOrEnd_CloseUp(BEGIN_END.END);
        }

        private void button_add_patientInfo_Click(object sender, EventArgs e)
        {
            Form_PatientInfo fm = new Form_PatientInfo();
            fm.PatientInfo += new PatientInfoHandler(ParsePatientInfo2appPanel);
            fm.ShowDialog();
            if(fm.DialogResult==DialogResult.OK)
            {
                DataMngr.m_bPatientInfo_Geted = true;
            }
            //fm.StartPosition = FormStartPosition.CenterScreen;
            //DataMngr.m_bPatientInfo_Geted = true;
        }
        
        void ParsePatientInfo2appPanel(PATIENT_INFO info)
        {
            LanguageMngr lang = new LanguageMngr();
            label_value_added_patientName.Text = info.name;
            label_value_patient_name.Text = info.name;

            label_value_added_patientAge.Text = info.age;
            label_value_patient_age.Text = info.age;

            label_value_added_patientGender.Text = lang.showGenderValue(info.gender);

            label_value_patient_gender.Text = lang.showGenderValue(info.gender);

            label_value_added_phoneNum.Text = info.phoneNum;
            label_value_patient_phoneNum.Text = info.phoneNum;

            label_value_patient_height.Text = info.height+" cm";
            label_value_patient_weight.Text = info.weight+" kg";
            label_value_patient_adress.Text = info.adress;
        }

        private void button_generateReport_Click(object sender, EventArgs e)
        {
            if(!DataMngr.m_bPatientInfo_Geted)
            {
                MessageBox.Show(LanguageMngr.pls_complete_patient_info());
                return;
            }
            if (this.folderBrowserDialog_save2PDF.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            //创建一个pdf文件
            string strPath = this.folderBrowserDialog_save2PDF.SelectedPath;//获取打开的文件路径名
            //报告的名字格式为：姓名_电话号码.pdf
            string reportPath = strPath + @"\" + label_value_added_patientName.Text + "_"
                                 + label_value_added_phoneNum.Text + ".pdf";
            FileStream fs = new FileStream(reportPath, FileMode.Create);

            ////创建A4纸、横向PDF文档  
            //Document document = new Document(PageSize.A4.Rotate());
            Document document = new Document(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);    //将PDF文档写入创建的文件中  
            document.Open();

            //要在PDF文档中写入中文必须指定中文字体，否则无法写入中文  
            //Environment.CurrentDirectory为当前app的路径
            BaseFont bftitle = BaseFont.CreateFont(Environment.CurrentDirectory + @"\simhei.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font fonttitle = new iTextSharp.text.Font(bftitle, 20);     //标题字体，大小
            BaseFont bf1 = BaseFont.CreateFont(Environment.CurrentDirectory + @"\simhei.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font nullparagraph = new iTextSharp.text.Font(bf1, 8);          //单元格中的字体，大小12  
            iTextSharp.text.Font fonttitle2 = new iTextSharp.text.Font(bf1, 12);        //副标题字体，大小15  

            //填写基本信息
            LanguageMngr lang = new LanguageMngr();
            #region
            //设备信息标头 
            string strContent = lang.equipInfo();
            Paragraph line = new Paragraph(strContent, fonttitle);     //添加段落，第二个参数指定使用fonttitle格式的字体，写入中文必须指定字体否则无法显示中文  
            line.Alignment = iTextSharp.text.Rectangle.ALIGN_LEFT;       //设置居中  
            document.Add(line);        //将标题段加入PDF文档中  

            ////空一行  
            Paragraph nullp = new Paragraph(" ", nullparagraph);
            //nullp.Leading = 10;
            document.Add(nullp);

            //设备信息：设备型号 + SN
            strContent = lang.machineType() + this.label_equipType_Value.Text;
            strContent = strContent.PadRight(43, Convert.ToChar(" ")) + "SN：" + this.label_SN_Value.Text;
            line = new Paragraph(strContent, fonttitle2);
            document.Add(line);

            //设备信息：  软件版本 
            document.Add(nullp);
            strContent = lang.soft_ver() + this.label_softwarVer_Value.Text;
            line = new Paragraph(strContent, fonttitle2);
            document.Add(line);

            document.Add(nullp);
            document.Add(nullp);
            document.Add(nullp);

            //使用时间标头
            strContent = lang.usageTime();
            line = new Paragraph(strContent, fonttitle);
            document.Add(line);

            //使用时间： 开始时间 --- 结束时间
            document.Add(nullp);
            strContent = this.label_dateFrom_Value.Text + "        ---       " + this.label_dateTo_Value.Text;
            line = new Paragraph(strContent, fonttitle2);
            document.Add(line);

            document.Add(nullp);
            document.Add(nullp);
            document.Add(nullp);

            //病人信息标头
            strContent = lang.patient_info();
            line = new Paragraph(strContent, fonttitle);
            document.Add(line);

            //病人信息： 姓名 + 身高
            document.Add(nullp);
            strContent = lang.name() + this.label_value_patient_name.Text;
            strContent = strContent.PadRight(45, Convert.ToChar(" ")) + lang.height() + this.label_value_patient_height.Text;
            line = new Paragraph(strContent, fonttitle2);
            document.Add(line);

            //病人信息： 年龄 + 体重
            document.Add(nullp);
            strContent = lang.age() + this.label_value_patient_age.Text;
            strContent = strContent.PadRight(45, Convert.ToChar(" ")) + lang.weight() + this.label_value_patient_weight.Text;
            line = new Paragraph(strContent, fonttitle2);
            document.Add(line);

            //病人信息： 性别
            document.Add(nullp);
            strContent = lang.gender() + this.label_value_patient_gender.Text;
            line = new Paragraph(strContent, fonttitle2);
            document.Add(line);

            //病人信息： 电话号码
            document.Add(nullp);
            strContent = lang.phoneNum() + this.label_value_patient_phoneNum.Text;
            line = new Paragraph(strContent, fonttitle2);
            document.Add(line);

            //病人信息： 家庭住址
            document.Add(nullp);
            strContent = lang.address() + this.label_value_patient_adress.Text;
            line = new Paragraph(strContent, fonttitle2);
            document.Add(line);

            document.Add(nullp);
            document.Add(nullp);
            document.Add(nullp);
            #endregion

            //定义图片在PDF中宽和高
            int chart_Height_inPDF = 250;
            int chart_Width_inPDF = 500;

            //图表,大标题
            strContent = lang.charts();
            line = new Paragraph(strContent, fonttitle);
            document.Add(line);
            document.Add(nullp);
            //图表：工作信息-使用时间
            #region
            strContent = lang.title_usageTime();
            line = new Paragraph(strContent, fonttitle2);
            document.Add(line);
            document.Add(nullp);

            string workDataChart_image = Environment.CurrentDirectory + "\\" + "workData.png";
            //先获取原图的宽和高
            int old_width = this.chart_workData.Width;
            int old_height = this.chart_workData.Height;
            //设置保存成图片是的宽和高
            this.chart_workData.Width = chart_Width_inPDF;
            this.chart_workData.Height = chart_Height_inPDF;
            this.chart_workData.SaveImage(workDataChart_image, ChartImageFormat.Png);
            iTextSharp.text.Image png = iTextSharp.text.Image.GetInstance(workDataChart_image);
            document.Add(png);
            //还原chart的原来宽和高
            this.chart_workData.Width = old_width;
            this.chart_workData.Height = old_height;
            document.Add(nullp);
            #endregion

            //图表：病人温度
            #region
            strContent = lang.title_patient_tmp();
            line = new Paragraph(strContent, fonttitle2);
            document.Add(line);
            document.Add(nullp);

            string patientTmp_image = Environment.CurrentDirectory + "\\" + "PatientTmp.png";
            //先获取原图的宽和高
            old_width = this.chart_patientTmp.Width;
            old_height = this.chart_patientTmp.Height;
            //设置保存成图片是的宽和高
            this.chart_patientTmp.Width = chart_Width_inPDF;
            this.chart_patientTmp.Height = chart_Height_inPDF;

            this.chart_patientTmp.SaveImage(patientTmp_image, ChartImageFormat.Png);
            png = iTextSharp.text.Image.GetInstance(patientTmp_image);
            document.Add(png);
            //还原chart的原来宽和高
            this.chart_patientTmp.Width = old_width;
            this.chart_patientTmp.Height = old_height;
            document.Add(nullp);
            #endregion

            //图表：出气口温度
            #region
            strContent = lang.title_air_outlet_tmp();
            line = new Paragraph(strContent, fonttitle2);
            document.Add(line);
            document.Add(nullp);

            string airOutLetTmp_image = Environment.CurrentDirectory + "\\" + "airOutLetTmp_images.png";
            //先获取原图的宽和高
            old_width = this.chart_air_outlet_tmp.Width;
            old_height = this.chart_air_outlet_tmp.Height;
            //设置保存成图片是的宽和高
            this.chart_air_outlet_tmp.Width = chart_Width_inPDF;
            this.chart_air_outlet_tmp.Height = chart_Height_inPDF;
            this.chart_air_outlet_tmp.SaveImage(airOutLetTmp_image, ChartImageFormat.Png);
            png = iTextSharp.text.Image.GetInstance(airOutLetTmp_image);
            document.Add(png);
            //还原chart的原来宽和高
            this.chart_air_outlet_tmp.Width = old_width;
            this.chart_air_outlet_tmp.Height = old_height;
            document.Add(nullp);
            #endregion

            document.Add(nullp);
            document.Add(nullp);
            document.Add(nullp);
            document.Add(nullp);
            document.Add(nullp);
            document.Add(nullp);
            document.Add(nullp);
            document.Add(nullp);
            document.Add(nullp);
            document.Add(nullp);
            document.Add(nullp);
            document.Add(nullp);
            document.Add(nullp);
            document.Add(nullp);


            //图表：流量
            #region
            strContent = lang.title_flow();
            line = new Paragraph(strContent, fonttitle2);
            document.Add(line);
            document.Add(nullp);

            string flow_image = Environment.CurrentDirectory + "\\" + "flow.png";
            //先获取原图的宽和高
            old_width = this.chart_flow.Width;
            old_height = this.chart_flow.Height;
            //设置保存成图片是的宽和高
            this.chart_flow.Width = chart_Width_inPDF;
            this.chart_flow.Height = chart_Height_inPDF;
            this.chart_flow.SaveImage(flow_image, ChartImageFormat.Png);
            png = iTextSharp.text.Image.GetInstance(flow_image);
            document.Add(png);
            //还原chart的原来宽和高
            this.chart_flow.Width = old_width;
            this.chart_flow.Height = old_height;
            document.Add(nullp);
            #endregion

            //图表：氧浓度
            #region
            strContent = lang.title_oxy_concentration();
            line = new Paragraph(strContent, fonttitle2);
            document.Add(line);
            document.Add(nullp);

            string oxyConcentration_image = Environment.CurrentDirectory + "\\" + "oxyConcentration.png";
            //先获取原图的宽和高
            old_width = this.chart_oxy_concentration.Width;
            old_height = this.chart_oxy_concentration.Height;
            //设置保存成图片是的宽和高
            this.chart_oxy_concentration.Width = chart_Width_inPDF;
            this.chart_oxy_concentration.Height = chart_Height_inPDF;
            this.chart_oxy_concentration.SaveImage(oxyConcentration_image, ChartImageFormat.Png);
            png = iTextSharp.text.Image.GetInstance(oxyConcentration_image);
            document.Add(png);
            //还原chart的原来宽和高
            this.chart_oxy_concentration.Width = old_width;
            this.chart_oxy_concentration.Height = old_height;
            #endregion

            //增加空行，为了排版好看
            document.Add(nullp);         // 2018/7/20新增
            document.Add(nullp);
            document.Add(nullp);
            document.Add(nullp);
            document.Add(nullp);
            document.Add(nullp);
            document.Add(nullp);
            document.Add(nullp);
            document.Add(nullp);
            document.Add(nullp);
            document.Add(nullp);
            document.Add(nullp);
            document.Add(nullp);
            document.Add(nullp);

            //图表: 露点温度                  // 2018/7/20新增
            #region
            strContent = lang.title_dewpoint_tmp();
            line = new Paragraph(strContent, fonttitle2);
            document.Add(line);
            document.Add(nullp);

            string dewpointTmp_image = Environment.CurrentDirectory + "\\" + "dewpointTmp.png";
            //先获取原图的宽和高
            old_width = this.chart_dewpoint_tmp.Width;
            old_height = this.chart_dewpoint_tmp.Height;
            //设置保存成图片是的宽和高
            this.chart_dewpoint_tmp.Width = chart_Width_inPDF;
            this.chart_dewpoint_tmp.Height = chart_Height_inPDF;
            this.chart_dewpoint_tmp.SaveImage(dewpointTmp_image, ChartImageFormat.Png);
            png = iTextSharp.text.Image.GetInstance(dewpointTmp_image);
            document.Add(png);
            //还原chart的原来宽和高
            this.chart_dewpoint_tmp.Width = old_width;
            this.chart_dewpoint_tmp.Height = old_height;
            #endregion


            //删除照片
            File.Delete(workDataChart_image);
            File.Delete(patientTmp_image);
            File.Delete(airOutLetTmp_image);
            File.Delete(flow_image);
            File.Delete(oxyConcentration_image);
            File.Delete(dewpointTmp_image);              // 2018/7/20 新增

            document.Close();
            fs.Close();
            MessageBox.Show(LanguageMngr.generate_report_successful());
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            
            if(e.TabPageIndex==1||e.TabPageIndex==2||e.TabPageIndex==3||e.TabPageIndex==4)
            {
                if(FileMngr.m_dirPath==null)
                {
                    MessageBox.Show(LanguageMngr.pls_import_data());
                    e.Cancel = true;
                    return;
                }

                if (DataMngr.m_bDateTimePicker_ValueChanged)
                {
                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;
                    MessageBox.Show(LanguageMngr.pls_choose_time());
                } 
            }
        }

        

        private void treeView_detailChart_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if(e.Node.Parent==null)
            {
                //MessageBox.Show("没有父节点");
                return;
            }
            else
            {
                if(e.Node.Parent.Parent!=null)
                {
                    //找到了最终的子节点
                    string strDate = e.Node.Parent.Parent.Text +"/"+ e.Node.Parent.Text +"/"+ e.Node.Text + " 23:59:59";
                    DataMngr.m_chart_selected_date = Convert.ToDateTime(strDate);
                    this.ShowDetailCharts(DataMngr.m_chart_selected_date, 1);
                }
                //else
                //{
                //    MessageBox.Show("月");
                //}
            }
        }

        private void 显示所有数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            if (FileMngr.m_dirPath == null || !DataMngr.m_bDateTimePicker_ValueChanged)
            {
                显示所有数据ToolStripMenuItem.CheckState = CheckState.Unchecked;
                MessageBox.Show(LanguageMngr.pls_ensure_import_data_and_choose_time());
                return;
            }

            if (显示所有数据ToolStripMenuItem.CheckState==CheckState.Checked)
            {
                if (DataMngr.m_machineType == 2)
                {
                    //VNU002的时候有56列，这里将31~56列全部显示出来
                    for (int i = 31+1; i <= 56+1+2; i++)  //2018/7/20修改
                    {
                        this.listView_workData.Columns[i].Width = 150;
                    }
                }
                else if (DataMngr.m_machineType == 1)
                {
                    //VNU001的时候有55列，这里将30~56列全部显示出来
                    for (int i = 30 + 1; i <= 55 + 1 + 2; i++) //2018/7/20修改
                    {
                        this.listView_workData.Columns[i].Width = 150;
                    }
                }
                else
                {
                    //do nothing
                }
            }
            else
            {
                if (DataMngr.m_machineType == 2)
                {
                    //VNU002的时候有56列，这里将31~56列全部显示出来
                    for (int i = 31 + 1; i <= 56 + 1 + 2; i++)  //2018/7/20修改
                    {
                        this.listView_workData.Columns[i].Width = 0;
                    }
                }
                else if (DataMngr.m_machineType == 1)
                {
                    //VNU001的时候有55列，这里将30~56列全部显示出来
                    for (int i = 30 + 1; i <= 55 + 1 + 2; i++) //2018/7/20修改
                    {
                        this.listView_workData.Columns[i].Width = 0;
                    }
                }
                else
                {
                    //do nothing
                }
            } 
        }

        private void 软件版本ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(LanguageMngr.m_language==LANGUAGE.CHINA)
            {
                Form_About fm = new Form_About();
                fm.ShowDialog();
            }
            else if (LanguageMngr.m_language == LANGUAGE.ENGLISH)
            {
                Form_AboutUs_English fm = new Form_AboutUs_English();
                fm.ShowDialog();
            }
            else
            { 
                //其它语言
            }

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ConfigurMngr mngr = new ConfigurMngr();
            mngr.SaveCfg(LanguageMngr.m_language);
        }
    }

    public class ConfigurMngr 
    {
        public LANGUAGE m_lang;
        public String m_cfgfilePath;

        public ConfigurMngr()
        {
            m_cfgfilePath = Environment.CurrentDirectory + @"\Config.bin";
        }

        public ConfigurMngr(LANGUAGE lang)
        {
            m_cfgfilePath = Environment.CurrentDirectory + @"\Config.bin";
            m_lang = lang;
        }

        public LANGUAGE GetLangure()
        {
            return m_lang;
        }

        public void LoadCfg()
        {
            if (File.Exists(m_cfgfilePath))
            {
                FileStream fs = new FileStream(m_cfgfilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                BinaryReader br = new BinaryReader(fs, Encoding.ASCII);

                byte[] bt = new byte[1];
                while (br.Read(bt, 0, 1) > 0)
                {
                    m_lang = (LANGUAGE)(bt[0]);
                }

                fs.Close();
                br.Close();
            }
            else
            {
                m_lang = LANGUAGE.ENGLISH;   //不存cfg文件，就创建一个,默认为英文
                SaveCfg(LANGUAGE.ENGLISH);
            }
        }
        public void SaveCfg(LANGUAGE lang)
        {
            m_lang = lang;
            FileStream fs = new FileStream(m_cfgfilePath, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs, Encoding.ASCII);

            //Int32 bt = (Int32)(m_lang);
            //MessageBox.Show(bt.ToString());
            bw.Write(Convert.ToByte((Int32)m_lang));

            bw.Close();
            fs.Close();
        }
        
    }




    public class FileMngr
    {
        public static bool m_bCreateTestFiles;    //这个仅仅是为了产生测试文件，正式版本将它改成false
        public static WORK_INFO_HEAD m_lastWorkHead;            //这个是为了显示app上的基本信息，才保留的一个信息
        public static WORK_INFO_MESSAGE m_lastWorkMsg;          //这个是为了显示app上的基本信息，才保留的一个信息
        public static DateTime m_dateTime_begin;
        public static DateTime m_dateTime_end;

        public static DateTime m_DateTime_min;                  //根据导入的文件，得到最大最小时间值
        public static DateTime m_DateTime_max;

        public const int ALARM_MSG_LEN = 16;                     //报警信息长度
        public const int WORKDATA_MSG_LEN = 64;                //工作信息长度
        public static string m_dirPath;                         //打开文件夹时的路径
        public static string m_alarmFileName;                   //报警文件名
        public static List<string> m_workFileNameList;              //工作文件路径的链表(工作文件有很多，每天一个)
        public static List<string> m_workFileName_CanBeOpened_List; //能被打开的工作文件链表

        public static ALARM_INFO_HEAD m_alarmHead;              //alarm的消息头
        public static List<ALARM_INFO_MESSAGE> m_alarmMsgList;  //alarm消息体链表

        public static Dictionary<WORK_INFO_HEAD, List<WORK_INFO_MESSAGE>> m_workHead_Msg_Map;   //每天的工作信息头和Msg链表放到Map中

        public static void CreateWorkDataFiles(DateTime tmBegin, Int32 duration)
        {
            #region
            //产生工作文件
            for (int i = 0; i < duration; i++)
            {
                DateTime tmp = tmBegin.AddDays(i);

                string strFilePath = "C:/Users/Administrator/Desktop/SD_TEST/170000000001/DATA";
                string strData = Convert.ToString(tmp.Year) + Convert.ToString(tmp.Month).PadLeft(2, '0') + Convert.ToString(tmp.Day).PadLeft(2, '0');
                strFilePath += strData + ".vmf";
                FileStream fs = new FileStream(strFilePath, FileMode.Create);
                BinaryWriter bw = new BinaryWriter(fs, Encoding.ASCII);

                //填充工作信息头
                #region
                //WORK_INFO_HEAD workHead = new WORK_INFO_HEAD();
                //workHead.WORK_FLAG = "8DATA" + strData;//没加校验
                //workHead.WORK_FLAG.PadRight(64, '0');
                //workHead.MACHINETYPE = (Convert.ToChar(0x06)+"VUN002000").PadRight(64, '0');
                //workHead.SN = (Convert.ToChar(0x09)+"1700002342").PadRight(64, '0');
                //workHead.SOFTWAR_VER = (""+Convert.ToChar(0x03)+Convert.ToChar(0x01)+Convert.ToChar(0x01)+Convert.ToChar(0x01)).PadRight(64, '0');
                ////workHead.SOFTWAR_VER = ("" + Convert.ToChar(0x03) + "111").PadRight(64, '0');
                //workHead.RESERVE_0 = "".PadRight(64, '0');
                //workHead.RESERVE_1 = "".PadRight(64, '0');
                //workHead.RESERVE_2 = "".PadRight(64, '0');
                //workHead.RESERVE_3 = "".PadRight(64, '0');
                //workHead.RESERVE_4 = "".PadRight(64, '0');
                //workHead.RESERVE_5 = "".PadRight(64, '0');
                //workHead.RESERVE_6 = "".PadRight(64, '0');
                //workHead.RESERVE_7 = "".PadRight(64, '0');
                //workHead.RESERVE_8 = "".PadRight(64, '0');
                //workHead.RESERVE_9 = "".PadRight(64, '0');
                //workHead.RESERVE_10 = "".PadRight(64, '0');
                //workHead.WORKDATA_NUM = "45678912".PadRight(64, '0');

                //var buff = GetData(workHead);
                //bw.Write(buff, 0, Marshal.SizeOf(workHead));
                #endregion

                Random rnd = new Random();
                //写入信息体
                int m = 0;
                bool bflag_runMode = false;
                for (int j = 0; j < 1000 + rnd.Next(0, 360); j++)
                {
                    //int runMode = 0;
                    m++;
                    if (m == 30)
                    {
                        //runMode = 1;
                        m = 0;
                        bflag_runMode = !bflag_runMode;
                    }
                    DateTime tmp1 = tmp.AddMinutes(j);

                    byte[] bt = new byte[64]{
                    #region
                        Convert.ToByte(tmp1.Year/100),
                        Convert.ToByte(tmp1.Year%100),
                        Convert.ToByte(tmp1.Month),
                        Convert.ToByte(tmp1.Day),
                        Convert.ToByte(tmp1.Hour),
                        Convert.ToByte(tmp1.Minute),
                        Convert.ToByte(tmp1.Second),
                        Convert.ToByte(Convert.ToInt32(bflag_runMode)), //运行模式
                        Convert.ToByte(30),   //设定温度
                        Convert.ToByte(60),   //设定流量
                        Convert.ToByte(95),   //设定高氧浓度报警
                        Convert.ToByte(21),   //设定低氧浓度报警
                        Convert.ToByte(5),   //设定雾化量档位
                        Convert.ToByte(30),   //设定雾化时间
                        rnd.Next(0,2)==1?Convert.ToByte(0x01):Convert.ToByte(0x00), //成人儿童
                        //Convert.ToByte(SetAdaultOrChild(rnd.Next(0,2))),
                        //Convert.ToByte(0x01),
                        //Convert.ToByte(rnd.Next(0,5)+32), //患者端温度
                        //Convert.ToByte(rnd.Next(0,5)+30), //出气口温度
                        Convert.ToByte(255), //患者端温度
                        Convert.ToByte(255), //出气口温度
                        Convert.ToByte(100), //加热盘温度
                        Convert.ToByte(26), //环境温度
                        Convert.ToByte(41), //驱动板温度
                        Convert.ToByte(rnd.Next(0,5)+60), //流量
                        Convert.ToByte(rnd.Next(0,5)+30), //氧浓度
                        Convert.ToByte(2), //气道压力
                        Convert.ToByte(rnd.Next(0,5)), //回路类型
                        Convert.ToByte(161), //故障状态位 A1
                        Convert.ToByte(162), //故障状态位 A2
                        Convert.ToByte(0), //雾化DAC数值L
                        Convert.ToByte(0), //雾化DAC数值H
                        Convert.ToByte(0), //雾化ADC数值L
                        Convert.ToByte(0), //雾化ADC数值H
                        Convert.ToByte(0), //回路加热PWM数值L
                        Convert.ToByte(0), //回路加热PWM数值H
                        Convert.ToByte(0), //回路加热ADC数值L
                        Convert.ToByte(0), //回路加热ADC数值H
                        Convert.ToByte(0), //加热盘加热PWM数值L
                        Convert.ToByte(0), //加热盘加热PWM数值H
                        Convert.ToByte(0), //加热盘加热ADC数值L
                        Convert.ToByte(0), //加热盘加热ADC数值H
                        Convert.ToByte(0), //主马达驱动数值L
                        Convert.ToByte(0), //主马达驱动数值H
                        Convert.ToByte(0), //主马达转数数值L
                        Convert.ToByte(0), //主马达转数数值H
                        Convert.ToByte(0), //压力传感器ADC值L
                        Convert.ToByte(0), //压力传感器ADC值H
                        Convert.ToByte(0), //水位传感器HADC值L
                        Convert.ToByte(0), //水位传感器HADC值H
                        Convert.ToByte(0), //水位传感器LADC值L
                        Convert.ToByte(0), //水位传感器LADC值H
                        Convert.ToByte(0), //散热风扇驱动数值L
                        Convert.ToByte(0), //散热风扇驱动数值H
                        Convert.ToByte(0), //散热风扇转速数值L
                        Convert.ToByte(0), //散热风扇转速数值H
                        Convert.ToByte(22), //主马达温度ADC值L
                        Convert.ToByte(33), //主马达温度ADC值H
                        Convert.ToByte(rnd.Next(0,60)), //露点温度
                        Convert.ToByte(0), //保留0
                        Convert.ToByte(0), //保留1
                        Convert.ToByte(0), //保留2
                        Convert.ToByte(0), //保留3
                        Convert.ToByte(0), //保留4
                        Convert.ToByte(0), //保留5
                        Convert.ToByte(0), //保留6
                        Convert.ToByte(30), //
                        Convert.ToByte(89), //
                        #endregion
                    };
                    bw.Write(bt, 0, 64);
                }
                bw.Close();
                fs.Close();
            }
            #endregion
        }

        public static void CreateAlarmFile(DateTime tmBegin, Int32 duration)
        {
            #region
            //产生工作文件
                string strFilePath = "C:/Users/Administrator/Desktop/SD_TEST/170000000001/ALARM.vmf";
                string strData = Convert.ToString(tmBegin.Year) + Convert.ToString(tmBegin.Month).PadLeft(2, '0') + Convert.ToString(tmBegin.Day).PadLeft(2, '0');
               
                FileStream fs = new FileStream(strFilePath, FileMode.Create);
                BinaryWriter bw = new BinaryWriter(fs, Encoding.ASCII);

                //填充报警信息头
                #region
                //ALARM_INFO_HEAD alarmHead = new ALARM_INFO_HEAD();
                //alarmHead.ALARM_FLAG = (Convert.ToChar(0x05)+"ALARM89").PadRight(16, '0');
                //alarmHead.MACHINETYPE = (Convert.ToChar(0x06)+"VUN002").PadRight(16, '0');
                //alarmHead.SN = (Convert.ToChar(0x09)+"1700002342").PadRight(16, '0');
                //alarmHead.SOFTWAR_VER = ("" + Convert.ToChar(0x03) + Convert.ToChar(0x01) + Convert.ToChar(0x01) + Convert.ToChar(0x01)).PadRight(16, '0');
                ////alarmHead.SOFTWAR_VER = ("" + Convert.ToChar(0x03) + "111").PadRight(16, '0');    
                //alarmHead.RESERVE_0 = "".PadRight(16, '0');
                //alarmHead.RESERVE_1 = "".PadRight(16, '0');
                //alarmHead.RESERVE_2 = "".PadRight(16, '0');
                //alarmHead.RESERVE_3 = "".PadRight(16, '0');
                //alarmHead.RESERVE_4 = "".PadRight(16, '0');
                //alarmHead.RESERVE_5 = "".PadRight(16, '0');
                //alarmHead.RESERVE_6 = "".PadRight(16, '0');
                //alarmHead.RESERVE_7 = "".PadRight(16, '0');
                //alarmHead.RESERVE_8 = "".PadRight(16, '0');
                //alarmHead.RESERVE_9 = "".PadRight(16, '0');
                //alarmHead.RESERVE_10 = "".PadRight(16, '0');
                //alarmHead.ALARM_NUM = "9874545".PadRight(16, '0');

                //var buff = GetData(alarmHead);
                //bw.Write(buff, 0, Marshal.SizeOf(alarmHead));
                #endregion

                Random rnd = new Random();
                //写入信息体
                #region
                int m = 0;
                bool bflag_runMode = false;
                for (int j = 0; j < 100000; j++)
                {
                    //int runMode = 0;
                    m++;
                    if (m == 30)
                    {
                        //runMode = 1;
                        m = 0;
                        bflag_runMode = !bflag_runMode;
                    }
                    DateTime tmp = tmBegin.AddMinutes(10 * j);

                    byte[] bt = new byte[16]{
                    #region
                        Convert.ToByte(tmp.Year/100),
                        Convert.ToByte(tmp.Year%100),
                        Convert.ToByte(tmp.Month),
                        Convert.ToByte(tmp.Day),
                        Convert.ToByte(tmp.Hour),
                        Convert.ToByte(tmp.Minute),
                        Convert.ToByte(tmp.Second),
                        Convert.ToByte(Convert.ToInt32(bflag_runMode)),
                        Convert.ToByte(rnd.Next(0,30)), //报警代码
                        Convert.ToByte(rnd.Next(0,100)), //报警数据L
                        Convert.ToByte(rnd.Next(0,100)), //报警数据H
                        Convert.ToByte(0), //保留1
                        Convert.ToByte(0), //保留2
                        Convert.ToByte(0), //保留3
                        Convert.ToByte(12), //checksum1
                        Convert.ToByte(23), //checksum2
                        #endregion
                    };
                    bw.Write(bt, 0, 16);
                }
                #endregion
                bw.Close();
                fs.Close();
          
            #endregion
        }

        public static void CreateTestFiles(DateTime tmBegin,int duration)
        {
            //定义产生测试文件的时间范围
            CreateAlarmFile(tmBegin, duration);
            CreateWorkDataFiles(tmBegin, duration);

        }

        public static void GetMinMaxDateTime()
        {
            FileMngr.m_DateTime_min = new DateTime(1900, 1, 1, 0, 0, 0);
            FileMngr.m_DateTime_max = new DateTime(1900, 1, 1, 0, 0, 0);
            #region
            int n = 1;
            if (FileMngr.m_workFileNameList.Count != 0)
            {
                //工作信息文件不缺失时，优先使用工作信息的
                #region
                foreach (var fileName in m_workFileNameList)
                {
                    //MessageBox.Show(fileName);
                    int begin = fileName.IndexOf("DATA") + 4;
                    int end = fileName.IndexOf(".vmf");
                    string strDate = fileName.Substring(begin, end - begin);

                    Int32 year = Convert.ToInt32(strDate.Substring(0, 4));
                    Int32 month = Convert.ToInt32(strDate.Substring(4, 2));
                    Int32 day = Convert.ToInt32(strDate.Substring(6, 2));
                    DateTime tm = new DateTime(year, month, day, 0, 0, 0);
                    if (n == 1)
                    {
                        FileMngr.m_DateTime_min = tm; //给FileMngr.m_DateTime_min赋初值
                        //MessageBox.Show(FileMngr.m_DateTime_min.ToString());
                        n++;
                    }

                    if (tm < FileMngr.m_DateTime_min)
                    {
                        FileMngr.m_DateTime_min = tm;
                    }

                    if (tm > FileMngr.m_DateTime_max)
                    {
                        FileMngr.m_DateTime_max = tm;
                    }
                }
                #endregion
            }
            else
            {
                //工作信息文件缺失时，使用报警文件的
                #region
                if (m_alarmFileName == null)
                {
                    return;
                }

                FileStream fs = null;
                try
                {
                    fs = new FileStream(m_dirPath + @"\" + m_alarmFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                }
                catch (IOException ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                finally
                {

                }
                BinaryReader br = new BinaryReader(fs, Encoding.ASCII);

                //读信息头
                 ALARM_INFO_HEAD alarmHead = new ALARM_INFO_HEAD();
                //int len_head = Marshal.SizeOf(alarmHead);
                int len_head = 16 * 16; //16个数据，每个16字节
                byte[] buffer = new byte[len_head];
                br.Read(buffer, 0, len_head);

                //准备一个buffer_msg
                ALARM_INFO_MESSAGE alarmMsg = new ALARM_INFO_MESSAGE();
                int len_msg = Marshal.SizeOf(alarmMsg);
                byte[] buffer_msg = new byte[len_msg];

                if (br.Read(buffer_msg, 0, len_msg) == 0)
                {
                    //如果只有报警信息头
                    FileMngr.m_DateTime_min = DateTime.Now;
                    FileMngr.m_DateTime_max = DateTime.Now;
                    MessageBox.Show(LanguageMngr.no_data_info_in_alarm_file());
                }
                else
                {
                    while (br.Read(buffer_msg, 0, len_msg) > 0)
                    {
                        DateTime tmp = new DateTime(Convert.ToInt32(buffer_msg[0] * 100 + buffer_msg[1]), Convert.ToInt32(buffer_msg[2]),
                            buffer_msg[3], 0, 0, 0);

                        if (FileMngr.m_DateTime_min >= tmp)
                        {
                            FileMngr.m_DateTime_min = tmp;
                        }
                        if (FileMngr.m_DateTime_max <= tmp)
                        {
                            FileMngr.m_DateTime_max = tmp;
                        }
                    }
                }
                
                fs.Close();
                br.Close();
                #endregion
            }
            #endregion
        }

        public static bool IsDirValidate(string strPath)                      //判断打开的文件夹是否有效
        {
            var alarmFilePath = Directory.GetFiles(strPath, "ALARM.vmf");      //获取"ALARM.vmf"文件的路径名
            var workDateFilePathes = Directory.GetFiles(strPath, "DATA*.vmf");
            if (alarmFilePath.Length == 0 && workDateFilePathes.Length == 0)
            {
                return false;
            }
            m_dirPath = strPath;
            return true;
        }

        public static void GetAllFilesName()
        {
            string[] alarmFilePath;
            string filePath = FileMngr.m_dirPath + @"\ALARM.vmf";
            
            if (File.Exists(filePath))
            {
                alarmFilePath = Directory.GetFiles(m_dirPath, "ALARM.vmf");      //获取"ALARM.vmf"文件的路径名
                m_alarmFileName = alarmFilePath[0].Substring(alarmFilePath[0].LastIndexOf(@"\") + 1);
            }
            else
            {
                //alarmFilePath = null;
                m_alarmFileName = null;
            }
            
            var workDataFilePathes = Directory.GetFiles(m_dirPath, "DATA*.vmf");
            
            foreach (var file in workDataFilePathes)
            {
                var name = file;
                name = name.Substring(name.LastIndexOf(@"\") + 1);
                if (name.Length != 16)
                    continue;
                System.IO.FileInfo fileInfo = new FileInfo(file);
                if (fileInfo.Length < 1088)
                    continue;
                m_workFileNameList.Add(file.Substring(file.LastIndexOf(@"\") + 1)); //将工作文件名添加到链表中
            }
        }

        public static bool IsErrCodeInRange(ref byte[] buffer_msg)
        {
            if (buffer_msg[8] < 8 || (buffer_msg[8] >= 20 && buffer_msg[8] <= 255))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool GetAlarmMsg()
        {
            //https://bbs.csdn.net/topics/20483369
            FileStream fs = new FileStream(m_dirPath + @"\" + m_alarmFileName, FileMode.Open,FileAccess.Read,FileShare.Read);
            BinaryReader br = new BinaryReader(fs, Encoding.ASCII);

            ALARM_INFO_HEAD alarmHead = new ALARM_INFO_HEAD();
            //int len_head = Marshal.SizeOf(alarmHead);
            int len_head = 16 * 16; //16个数据，每个16字节
            byte[] buffer = new byte[len_head];

            if (fs == null)
            {
                MessageBox.Show(LanguageMngr.open_alarmFile_fail());
                fs.Close();
                br.Close();
                return false;
            }

            br.Read(buffer, 0, len_head);

            //对alarmMsg的第一个字段进行校验

            //if (VerifyField(buffer))   //这里校验是失败的，为了打开文件将条件屏蔽
            if (true)
            {
                //m_alarmHead = GetObject<ALARM_INFO_HEAD>(buffer, len_head); //将信息头放入m_alarmHead中
                m_alarmHead = VM_transfer_alarmInfoHead(buffer);
                //debug
                //MessageBox.Show("校验成功！");

                ALARM_INFO_MESSAGE alarmMsg = new ALARM_INFO_MESSAGE();
                int len_msg = Marshal.SizeOf(alarmMsg);
                byte[] buffer_msg = new byte[len_msg];

                //m_alarmMsgList = new List<ALARM_INFO_MESSAGE>();
                while (br.Read(buffer_msg, 0, len_msg) > 0)
                {
                    //if (VerifyAlarmMsg(buffer_msg))         //这里为了能读取字段，屏蔽条件
                    {
                        if (IsErrCodeInRange(ref buffer_msg))
                        {
                            alarmMsg = GetObject<ALARM_INFO_MESSAGE>(buffer_msg, len_msg);
                            m_alarmMsgList.Add(alarmMsg);
                        }
                    }

                }
                //debug
                //MessageBox.Show(m_alarmMsgList.Count.ToString());
                br.Close();
                fs.Close();
                return true;
            }
            else
            {
                br.Close();
                fs.Close();
                return false;
            }
        }

        public static T GetObject<T>(byte[] data, int size)
        {
            Contract.Assume(size == Marshal.SizeOf(typeof(T)));
            IntPtr ptr = Marshal.AllocHGlobal(size);

            try
            {
                Marshal.Copy(data, 0, ptr, size);
                return (T)Marshal.PtrToStructure(ptr, typeof(T));
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }

        }
        public static WORK_INFO_HEAD VM_transfer_workInfoHead(byte[] buffer)
        {
            WORK_INFO_HEAD dst = new WORK_INFO_HEAD();
            int CNT = 64;
            for (int i = 0; i < CNT; i++)
            {
                dst.WORK_FLAG[i] = buffer[i + CNT * 0];
            }
            for (int i = 0; i < CNT; i++)
            {
                dst.MACHINETYPE[i] = buffer[i + CNT * 1];
            }
            for (int i = 0; i < CNT; i++)
            {
                dst.SN[i] = buffer[i + CNT * 2];
            }
            for (int i = 0; i < CNT; i++)
            {
                dst.SOFTWAR_VER[i] = buffer[i + CNT * 3];
            }
            for (int i = 0; i < CNT; i++)
            {
                dst.RESERVE_0[i] = buffer[i + CNT * 4];
            }
            for (int i = 0; i < CNT; i++)
            {
                dst.RESERVE_1[i] = buffer[i + CNT * 5];
            }
            for (int i = 0; i < CNT; i++)
            {
                dst.RESERVE_2[i] = buffer[i + CNT * 6];
            }
            for (int i = 0; i < CNT; i++)
            {
                dst.RESERVE_3[i] = buffer[i + CNT * 7];
            }
            for (int i = 0; i < CNT; i++)
            {
                dst.RESERVE_4[i] = buffer[i + CNT * 8];
            }
            for (int i = 0; i < CNT; i++)
            {
                dst.RESERVE_5[i] = buffer[i + CNT * 9];
            }
            for (int i = 0; i < CNT; i++)
            {
                dst.RESERVE_6[i] = buffer[i + CNT * 10];
            }
            for (int i = 0; i < CNT; i++)
            {
                dst.RESERVE_7[i] = buffer[i + CNT * 11];
            }
            for (int i = 0; i < CNT; i++)
            {
                dst.RESERVE_8[i] = buffer[i + CNT * 12];
            }
            for (int i = 0; i < CNT; i++)
            {
                dst.RESERVE_9[i] = buffer[i + CNT * 13];
            }
            for (int i = 0; i < CNT; i++)
            {
                dst.RESERVE_10[i] = buffer[i + CNT * 14];
            }
            for (int i = 0; i < CNT; i++)
            {
                dst.WORKDATA_NUM[i] = buffer[i + CNT * 15];
            }
            return dst;
        }

        public static ALARM_INFO_HEAD VM_transfer_alarmInfoHead(byte[] buffer)
        {
            ALARM_INFO_HEAD dst = new ALARM_INFO_HEAD();
            int CNT = 16;
            for (int i = 0; i < CNT; i++)
            {
                dst.ALARM_FLAG[i] = buffer[i + CNT * 0];
            }
            for (int i = 0; i < CNT; i++)
            {
                dst.MACHINETYPE[i] = buffer[i + CNT * 1];
            }
            for (int i = 0; i < CNT; i++)
            {
                dst.SN[i] = buffer[i + CNT * 2];
            }
            for (int i = 0; i < CNT; i++)
            {
                dst.SOFTWAR_VER[i] = buffer[i + CNT * 3];
            }
            for (int i = 0; i < CNT; i++)
            {
                dst.RESERVE_0[i] = buffer[i + CNT * 4];
            }
            for (int i = 0; i < CNT; i++)
            {
                dst.RESERVE_1[i] = buffer[i + CNT * 5];
            }
            for (int i = 0; i < CNT; i++)
            {
                dst.RESERVE_2[i] = buffer[i + CNT * 6];
            }
            for (int i = 0; i < CNT; i++)
            {
                dst.RESERVE_3[i] = buffer[i + CNT * 7];
            }
            for (int i = 0; i < CNT; i++)
            {
                dst.RESERVE_4[i] = buffer[i + CNT * 8];
            }
            for (int i = 0; i < CNT; i++)
            {
                dst.RESERVE_5[i] = buffer[i + CNT * 9];
            }
            for (int i = 0; i < CNT; i++)
            {
                dst.RESERVE_6[i] = buffer[i + CNT * 10];
            }
            for (int i = 0; i < CNT; i++)
            {
                dst.RESERVE_7[i] = buffer[i + CNT * 11];
            }
            for (int i = 0; i < CNT; i++)
            {
                dst.RESERVE_8[i] = buffer[i + CNT * 12];
            }
            for (int i = 0; i < CNT; i++)
            {
                dst.RESERVE_9[i] = buffer[i + CNT * 13];
            }
            for (int i = 0; i < CNT; i++)
            {
                dst.RESERVE_10[i] = buffer[i + CNT * 14];
            }
            for (int i = 0; i < CNT; i++)
            {
                dst.ALARM_NUM[i] = buffer[i + CNT * 15];
            }
            return dst;
        }


        public static byte[] VM_transfer_workInfoHead2Buffer(WORK_INFO_HEAD head)
        {
            if (head == null)
            {
                return null;
            }
            byte[] buffer = new byte[Marshal.SizeOf(head) * 16];
            int CNT = 64;
            for (int i = 0; i < CNT; i++)
            {
                buffer[i + CNT * 0] = head.WORK_FLAG[i];
            }
            for (int i = 0; i < CNT; i++)
            {
                buffer[i + CNT * 1] = head.MACHINETYPE[i];
            }
            for (int i = 0; i < CNT; i++)
            {
                buffer[i + CNT * 2] = head.SN[i];
            }
            for (int i = 0; i < CNT; i++)
            {
                buffer[i + CNT * 3] = head.SOFTWAR_VER[i];
            }
            for (int i = 0; i < CNT; i++)
            {
                buffer[i + CNT * 4] = head.RESERVE_0[i];
            }
            for (int i = 0; i < CNT; i++)
            {
                buffer[i + CNT * 5] = head.RESERVE_1[i];
            }
            for (int i = 0; i < CNT; i++)
            {
                buffer[i + CNT * 6] = head.RESERVE_2[i];
            }
            for (int i = 0; i < CNT; i++)
            {
                buffer[i + CNT * 7] = head.RESERVE_3[i];
            }
            for (int i = 0; i < CNT; i++)
            {
                buffer[i + CNT * 8] = head.RESERVE_4[i];
            }
            for (int i = 0; i < CNT; i++)
            {
                buffer[i + CNT * 9] = head.RESERVE_5[i];
            }
            for (int i = 0; i < CNT; i++)
            {
                buffer[i + CNT * 10] = head.RESERVE_6[i];
            }
            for (int i = 0; i < CNT; i++)
            {
                buffer[i + CNT * 11] = head.RESERVE_7[i];
            }
            for (int i = 0; i < CNT; i++)
            {
                buffer[i + CNT * 12] = head.RESERVE_8[i];
            }
            for (int i = 0; i < CNT; i++)
            {
                buffer[i + CNT * 13] = head.RESERVE_9[i];
            }
            for (int i = 0; i < CNT; i++)
            {
                buffer[i + CNT * 14] = head.RESERVE_10[i];
            }
            for (int i = 0; i < CNT; i++)
            {
                buffer[i + CNT * 15] = head.WORKDATA_NUM[i];
            }

            return buffer;
        }
        //public static byte[] GetData(object obj)
        //{
        //    int size = Marshal.SizeOf(obj.GetType());
        //    byte[] data = new byte[size];
        //    IntPtr ptr = Marshal.AllocHGlobal(size);

        //    try
        //    {
        //        Marshal.StructureToPtr(obj, ptr, true);
        //        Marshal.Copy(ptr, data, 0, size);
        //        return data;
        //    }
        //    finally
        //    {
        //        Marshal.FreeHGlobal(ptr);
        //    }
        //}

        public static bool IsByte0x00(byte[] buffer_msg)
        {
            if (buffer_msg[15] == 0x00 || buffer_msg[16] == 0x00 || buffer_msg[20] == 0x00 || buffer_msg[21] == 0x00)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool GetWorkMsg()
        {
            #region
            //MessageBox.Show(m_workFileNameList.Count.ToString());
            foreach (var workFile in m_workFileNameList)
            {
                List<WORK_INFO_MESSAGE> list = new List<WORK_INFO_MESSAGE>();
                FileStream fs = new FileStream(m_dirPath + @"\" + workFile, FileMode.Open,FileAccess.Read,FileShare.Read);
                BinaryReader br = new BinaryReader(fs, Encoding.ASCII);
                
                WORK_INFO_HEAD alarmHead = new WORK_INFO_HEAD();
                //int len_head = Marshal.SizeOf(alarmHead);
                int len_head = 64 * 16; //16个数据，每个64字节
                byte[] buffer = new byte[len_head];
                #region
                if (fs == null)
                {
                    MessageBox.Show(LanguageMngr.open_alarmFile_fail());
                    fs.Close();
                    br.Close();
                    return false;
                }
                #endregion
                //if (VerifyField(buffer))   //暂时屏蔽
                if (true)
                {
                    
                    m_workFileName_CanBeOpened_List.Add(workFile);
                    br.Read(buffer, 0, len_head);
                    //alarmHead = GetObject<WORK_INFO_HEAD>(buffer, len_head);
                    //m_lastWorkHead = alarmHead;         //保留最后一个工作信息头，作为最新的信息头，刷新到app基本信息中
                    m_lastWorkHead = VM_transfer_workInfoHead(buffer);
                    #region
                    //记得到时候取消屏蔽
                    //if (!VerifyField(buffer))   //为了打开文件将校验屏蔽掉
                    //{
                    //    fs.Close();
                    //    br.Close();
                    //    return false;           
                    //}
                    #endregion
                    WORK_INFO_MESSAGE workDataMsg = new WORK_INFO_MESSAGE();
                    int len_msg = Marshal.SizeOf(workDataMsg);
                    byte[] buffer_msg = new byte[len_msg];

                    #region
                    //if (br.Read(buffer_msg, 0, len_msg) == 0)
                    //{
                    //    //do nothing
                    //    fs.Close();
                    //    br.Close();
                    //}
                    //else
                    //{
                    //    while (br.Read(buffer_msg, 0, len_msg) > 0)
                    //    {
                    //        //校验每一个字段 
                    //        //if (VerifyWorkDataMsg(buffer_msg))           //这里为了能读取字段，将校验屏蔽掉
                    //        {
                    //            //CheckEachByte(buffer_msg);
                    //            //下位机会出现0，0，0，0，下位机尚未修复这个问题，在这里先过滤掉
                    //            //if (IsByte0x00(buffer_msg))
                    //            //{
                    //            //    continue;
                    //            //}
                    //            workDataMsg = GetObject<WORK_INFO_MESSAGE>(buffer_msg, len_msg);
                    //            //2018.11.06发现文件数据中年月日不对，先给下位机做过滤
                    //            if (workDataMsg.YEAR1 > 99 || workDataMsg.YEAR2 > 99 || workDataMsg.MONTH > 12 || workDataMsg.DAY > 31 ||
                    //                workDataMsg.HOUR > 23 || workDataMsg.MINUTE > 59 || workDataMsg.SECOND > 59)
                    //            {
                    //                continue;
                    //            }
                    //            list.Add(workDataMsg);
                    //            m_lastWorkMsg = workDataMsg;

                    //            //workDataMsg = GetObject<WORK_INFO_MESSAGE>(buffer_msg, len_msg);
                    //            //list.Add(workDataMsg);
                    //            //m_lastWorkMsg = workDataMsg;//保留最后一个工作信息，作为最新的信息，刷新到app基本信息中
                    //        }
                    //    }
                    //    //如果经过过滤之后，List的count不为0，才添加到链表中
                    //    if (list.Count != 0)
                    //    {
                    //        m_workHead_Msg_Map[alarmHead] = list;
                    //    }
                    //    //m_workHead_Msg_Map.Add(alarmHead,list);
                    //    fs.Close();
                    //    br.Close();
                    //}
                    #endregion
                    int read_len = 0;
                    for (; ; )
                    {
                        read_len = br.Read(buffer_msg, 0, len_msg);
                        if (read_len == 0)
                        {
                            fs.Close();
                            br.Close();
                            break;
                        }
                        else
                        {
                            //校验每一个字段 
                            //if (VerifyWorkDataMsg(buffer_msg))           //这里为了能读取字段，将校验屏蔽掉
                            {
                                //CheckEachByte(buffer_msg);
                                //下位机会出现0，0，0，0，下位机尚未修复这个问题，在这里先过滤掉
                                //if (IsByte0x00(buffer_msg))
                                //{
                                //    continue;
                                //}
                                ////debugCnt++;
                                // if (debugCnt == 158)
                                {
                                    workDataMsg = GetObject<WORK_INFO_MESSAGE>(buffer_msg, len_msg);
                                    //2018.11.06发现文件数据中年月日不对，先给下位机做过滤
                                    if (workDataMsg.YEAR1 > 99 || workDataMsg.YEAR2 > 99 || workDataMsg.MONTH > 12 || workDataMsg.DAY > 31 ||
                                        workDataMsg.HOUR > 23 || workDataMsg.MINUTE > 59 || workDataMsg.SECOND > 59)
                                    {
                                        continue;
                                    }
                                    list.Add(workDataMsg);
                                    m_lastWorkMsg = workDataMsg;
                                }

                                //workDataMsg = GetObject<WORK_INFO_MESSAGE>(buffer_msg, len_msg);
                                //list.Add(workDataMsg);
                                //m_lastWorkMsg = workDataMsg;//保留最后一个工作信息，作为最新的信息，刷新到app基本信息中
                            }
                        }
                    }
                    //如果经过过滤之后，List的count不为0，才添加到链表中
                    if (list.Count != 0)
                    {
                        m_workHead_Msg_Map[alarmHead] = list;
                    }
                    //m_workHead_Msg_Map.Add(alarmHead,list);
                    fs.Close();
                    br.Close();

                }
                else
                {
                    fs.Close();
                    br.Close();

                }
            }
            #endregion

            return true;
        }

        //检查每一个对应的字节数据是否在规定的范围内
        //public static void CheckEachByte(ref byte[] buffer_msg)
        //{
        //    if (buffer_msg[15] == 0xFF)
        //    {
        //        buffer_msg[15] = 0x47;
        //    }
        //    for (int i = 26; i <= 61; i++)
        //    {
        //        if (buffer_msg[i] == 0xFF)
        //        {
                    
        //        }
        //    }
        //}

        public static bool VerifyField(byte[] buffer)
        {
            int len = Convert.ToInt32(buffer[0]);
            len -= 48;
            //MessageBox.Show(len.ToString());
            if (len > buffer.Length)
            {
                //MessageBox.Show("出事了!");
                return false;
            }
            int sum = 256 * Convert.ToInt32(buffer[len + 1]) + Convert.ToInt32(buffer[len + 2]); //sum1+sum2转成和
            int tmp = 0;
            for (int i = 0; i <= len; i++)
            {
                tmp += Convert.ToInt32(buffer[i]);
            }
            if (tmp == sum)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool VerifyAlarmMsg(byte[] buffer)
        {
            int len = buffer.Length;
            int tmp = 0;
            int sum = 256 * Convert.ToInt32(buffer[len - 2]) + Convert.ToInt32(buffer[len - 1]);
            if (len != ALARM_MSG_LEN)  //alarmMsg长度为16字节
            {
                return false;
            }
            for (int i = 0; i <= len - 3; i++)
            {
                tmp += Convert.ToInt32(buffer[i]);
            }
            if (sum == tmp)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //检测每条工作信息，使用checksum来校验
        public static bool VerifyWorkDataMsg(byte[] buffer)
        {
            int len = buffer.Length;
            int tmp = 0;
            int sum = 256 * Convert.ToInt32(buffer[len - 2]) + Convert.ToInt32(buffer[len - 1]);
            if (len != WORKDATA_MSG_LEN)  //workDataMsg长度为64字节
            {
                return false;
            }
            for (int i = 0; i <= len - 3; i++)
            {
                tmp += Convert.ToInt32(buffer[i]);
            }
            if (sum == tmp)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string AlarmCode2Str(byte code)
        {
            int nCode = Convert.ToInt32(code);
            string str = "";
            LanguageMngr lang=new LanguageMngr();
            #region
            //switch (nCode)
            //{
            //    case 0:
            //        //str = "氧浓度传感器故障";
            //        str = lang.oxy_concentration_sensor_fault();
            //        break;
            //    case 1:
            //        //str = "流量传感器故障";
            //        str = lang.flow_sensor_fault();
            //        break;
            //    case 2:
            //        //str = "环境温度传感器故障";
            //        str = lang.enviroment_tmp_sensor_fault();
            //        break;
            //    case 3:
            //        //str = "驱动板温度传感器故障";
            //        str = lang.driverBoard_tmp_sensor_fault();
            //        break;
            //    case 4:
            //        //str = "加热盘温度传感器故障";
            //        str = lang.heating_plate_tmp_sensor_fault();
            //        break;
            //    case 5:
            //        //str = "散热风扇故障";
            //        str = lang.fan_fault();
            //        break;
            //    case 6:
            //        //str = "EEPROM校验失败";
            //        str = lang.EEPROM_verify_fail();
            //        break;
            //    case 7:
            //        //str = "出气口温度传感器故障";
            //        str = lang.air_outlet_sensor_fault();
            //        break;
            //    case 8:
            //        //str = "患者端温度传感器故障";
            //        str = lang.patient_tmp_sensor_fault();
            //        break;
            //    default:
            //        //str = "未识别的错误";
            //        str = lang.unknow_err();
            //        break;
            //}
            #endregion


            switch (nCode)
            {
                case 0:
                    str = lang.system_failure_E1();
                    break;
                case 1:
                    str = lang.system_failure_E2();
                    break;
                case 2:
                    str = lang.system_failure_E3();
                    break;
                case 3:
                    str = lang.system_failure_E4();
                    break;
                case 4:
                    str = lang.system_failure_E5();
                    break;
                case 5:
                    str = lang.system_failure_E6();
                    break;
                case 6:
                    str = lang.system_failure_E7();
                    break;
                case 7:
                    str = lang.system_failure_E8();
                    break;
                //case 8:
                //    str = lang.system_failure();
                        //break;
                case 20:  //超温
                    str = lang.overheat();
                    break;
                case 21://运行中电源断开
                    str = lang.power_off();
                    break;
                case 22:  //检查水罐
                    str = lang.check_chamber();
                    break;
                case 23://缺水
                    str = lang.change_water_bag();
                    break;
                case 24://温度探头未安装
                    str = lang.check_temp_probe();
                    break;
                case 25://管路未安装
                    str = lang.check_tube();
                    break;
                case 26:////管路不匹配
                    str = lang.tube_not_match();
                    break;
                case 27://堵塞
                    str = lang.check_blockages();
                    break;
                case 28://高氧浓度
                    str = lang.high_O2();
                    break;
                case 29://低氧浓度
                    str = lang.low_O2();
                    break;
                case 30://流量超范围
                    str = lang.flow_overrange();
                    break;
                case 31://温度超范围
                    str = lang.temp_overrange();
                    break;
                case 32://温度探头脱落
                    str = lang.prob_out();
                    break;
                case 33://SD卡未安装
                    str = lang.sdCard_not_install();
                    break;
                case 34:
                    str = lang.Circuit_failure_data_cable_uninstalled();
                    break;
                case 35:
                    str = lang.Check_for_leaks();
                    break;
                default:
                    //str = "未识别的错误";
                    str = lang.unknow_err();
                    break;
            }

            return str;
        }
    }
   

}
