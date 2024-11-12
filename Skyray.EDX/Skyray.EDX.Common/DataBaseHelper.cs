using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skyray.EDXRFLibrary;
using Skyray.EDXRFLibrary.Define;
using System.Data;
using System.Data.SQLite;
using System.Text.RegularExpressions;
using Skyray.EDXRFLibrary.Spectrum;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using System.IO;
using Skyray.EDX.Common.App;
using Skyray.EDX.Common.IApplication;
using System.Xml;

namespace Skyray.EDX.Common
{
    public class DataBaseHelper
    {

        public static void CheckTable(Lephone.Data.DbContext ConnectionString,ref bool isUpSpectrumData)
        {
            var lstTables = ConnectionString.GetTableNames();
            #region 其它
            if (!lstTables.Contains("Oxide"))
            {
                ConnectionString.Create(typeof(Oxide));
            }
            if (!lstTables.Contains("Atom"))
            {
                ConnectionString.Create(typeof(Atom));
            }
            if (!lstTables.Contains("ElementSpectrum"))
            {
                ConnectionString.Create(typeof(ElementSpectrum));
            }
            if (!lstTables.Contains("ExcitationConditions"))
            {
                ConnectionString.Create(typeof(ExcitationConditions));
            }
            if (!lstTables.Contains("Peak"))
            {
                ConnectionString.Create(typeof(Peak));
            }
            if (!lstTables.Contains("PreHeatParams"))
            {
                ConnectionString.Create(typeof(PreHeatParams));
            }
            if (!lstTables.Contains("QualeElement"))
            {
                ConnectionString.Create(typeof(QualeElement));
            }
            if (!lstTables.Contains("Role"))
            {
                ConnectionString.Create(typeof(Role));
            }
            if (!lstTables.Contains("Shape"))
            {
                ConnectionString.Create(typeof(Shape));
            }
            if (!lstTables.Contains("SurfaceSource"))
            {
                ConnectionString.Create(typeof(SurfaceSourceLight));
            }
            if (!lstTables.Contains("SysConfig"))
            {
                ConnectionString.Create(typeof(SysConfig));
            }
            if (!lstTables.Contains("User"))
            {
                ConnectionString.Create(typeof(User));
            }
             if (!lstTables.Contains("PWDLock"))
            {
                ConnectionString.Create(typeof(PWDLock));
                PWDLock pwd = PWDLock.New.Init("skyray");
                pwd.Save();
            }
            #endregion
            #region 元素
            if (!lstTables.Contains("StandSample"))
            {
                ConnectionString.Create(typeof(StandSample));
            }
            if (!lstTables.Contains("ReferenceElement"))
            {
                ConnectionString.Create(typeof(ReferenceElement));
            }
            if (!lstTables.Contains("Expunction"))
            {
                ConnectionString.Create(typeof(Expunction));
            }
            if (!lstTables.Contains("Optimiztion"))
            {
                ConnectionString.Create(typeof(Optimiztion));
            }
            if (!lstTables.Contains("ElementRef"))
            {
                ConnectionString.Create(typeof(ElementRef));
            }
            if (!lstTables.Contains("CustomField"))
            {
                ConnectionString.Create(typeof(CustomField));
            }
            if (!lstTables.Contains("CustomStandard"))
            {
                ConnectionString.Create(typeof(CustomStandard));
            }
            if (!lstTables.Contains("CurveElement"))
            {
                ConnectionString.Create(typeof(CurveElement));
            }
            if (!lstTables.Contains("StandardData"))
            {
                ConnectionString.Create(typeof(StandardData));
            }
            #endregion
            #region 历史记录
            if (!lstTables.Contains("ContinuousResult"))
            {
                ConnectionString.Create(typeof(ContinuousResult));
            }
            if (!lstTables.Contains("HistoryElement"))
            {
                ConnectionString.Create(typeof(HistoryElement));
            }
            if (!lstTables.Contains("CompanyOthersInfo"))
            {
                ConnectionString.Create(typeof(CompanyOthersInfo));
            }
            if (!lstTables.Contains("CompanyOthersListInfo"))
            {
                ConnectionString.Create(typeof(CompanyOthersListInfo));
            }
            if (!lstTables.Contains("HistoryCompanyOtherInfo"))
            {
                ConnectionString.Create(typeof(HistoryCompanyOtherInfo));
            }
            if (!lstTables.Contains("HistoryRecord"))
            {
                ConnectionString.Create(typeof(HistoryRecord));
            }
            #endregion
            #region 设备
            if (!lstTables.Contains("Detector"))
            {
                ConnectionString.Create(typeof(Detector));
            }
            if (!lstTables.Contains("Tubes"))
            {
                ConnectionString.Create(typeof(Tubes));
            }
            if (!lstTables.Contains("Collimator"))
            {
                ConnectionString.Create(typeof(Collimator));
            }
            if (!lstTables.Contains("Filter"))
            {
                ConnectionString.Create(typeof(Filter));
            }
            if (!lstTables.Contains("FPGAParams"))
            {
                ConnectionString.Create(typeof(FPGAParams));
            }
            if (!lstTables.Contains("Target"))
            {
                ConnectionString.Create(typeof(Target));
            }
            if (!lstTables.Contains("Chamber"))
            {
                ConnectionString.Create(typeof(Chamber));
            }
            if (!lstTables.Contains("Device"))
            {
                ConnectionString.Create(typeof(Device));
            }
            #endregion
            #region 工作曲线
            if (!lstTables.Contains("ElementList"))
            {
                ConnectionString.Create(typeof(ElementList));
            }
            if (!lstTables.Contains("Compounds"))
            {
                ConnectionString.Create(typeof(Compounds));
            }
            if (!lstTables.Contains("IntensityCalibration"))
            {
                ConnectionString.Create(typeof(IntensityCalibration));
            }
            if (!lstTables.Contains("PureSpecParam"))
            {
                ConnectionString.Create(typeof(PureSpecParam));
            }
            if (!lstTables.Contains("MultiPoints"))
            {
                ConnectionString.Create(typeof(MultiPoints));
            }
            if (!lstTables.Contains("WorkRegion"))
            {
                ConnectionString.Create(typeof(WorkRegion));
            }
            if (!lstTables.Contains("CalibrationParam"))
            {
                ConnectionString.Create(typeof(CalibrationParam));
            }
            if (!lstTables.Contains("Condition"))
            {
                ConnectionString.Create(typeof(Condition));
            }
            if (!lstTables.Contains("InitParameter"))
            {
                ConnectionString.Create(typeof(InitParameter));
            }
            if (!lstTables.Contains("DeviceParameter"))
            {
                ConnectionString.Create(typeof(DeviceParameter));
            }
            if (!lstTables.Contains("DemarcateEnergy"))
            {
                ConnectionString.Create(typeof(DemarcateEnergy));
            }
            if (!lstTables.Contains("WorkCurve"))
            {
                ConnectionString.Create(typeof(WorkCurve));
            }
            #endregion
            if (!lstTables.Contains("SpectrumData"))
            {
                ConnectionString.Create(typeof(SpectrumData));
                isUpSpectrumData = true;
                
            }
            if (!lstTables.Contains("AreaDensityUnit"))
            {
                ConnectionString.Create(typeof(AreaDensityUnit));
            }
            if (!lstTables.Contains("SpecialRemoveItem"))
            {
                ConnectionString.Create(typeof(SpecialRemoveItem));
            }
            if (!lstTables.Contains("EncodeXY"))
            {
                ConnectionString.Create(typeof(EncodeXY));
            }

            if (!lstTables.Contains("DefinePureElement"))
            {
                ConnectionString.Create(typeof(DefinePureElement));
            }

          
        }

        private static DataTable GetData(string strSql, string connectionString)
        {
            DataTable dt = new DataTable();
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                using (SQLiteCommand command = new SQLiteCommand(strSql, connection))
                {
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                    adapter.Fill(dt);
                }
            }
            return dt;
        }

        //private static bool isUpSpectrumData = false;//是否更新SpectrumData数据库；

        /// <summary>
        /// 更新数据库功能
        /// </summary>
        public static void UpdateDatabase(Lephone.Data.DbContext ConnectionString)
        {

            bool isUpSpectrumData = false;//是否更新SpectrumData数据库；
            bool isExcelModeTarget = false;//是否更新CompanyOthersInfo数据库的ExcelModeTarget字段
            CheckTable(ConnectionString, ref isUpSpectrumData);
            string sqlTable = @"select tbl_name,sql,type from sqlite_master where (tbl_name='CustomStandard' " +
                                   " or tbl_name='HistoryElement' " +
                                   " or tbl_name='FPGAParams' " +
                                   " or tbl_name='InitParameter' " +
                                   " or tbl_name='IntensityCalibration' " +
                                    " or tbl_name='PureSpecParam' " +
                                     " or tbl_name='MultiPoints' " +
                                   " or tbl_name='PreHeatParams' " +
                                   " or tbl_name='WorkRegion'"+
                                    " or tbl_name='DemarcateEnergy' " +
                                    " or tbl_name='Tubes' " +
                                    " or tbl_name='Device' " +
                                    " or tbl_name='DeviceParameter' " +
                                    " or tbl_name='SurfaceSource' " + " or tbl_name='Optimiztion'";
                                    if (ConnectionString.GetTableNames().Contains("Spec"))
                                                sqlTable += " or tbl_name='Spec' ";
                                    sqlTable+=" or tbl_name='CalibrationParam'" +
                                    " or tbl_name='HistoryRecord' " +
                                    " or tbl_name='CompanyOthersInfo' " +
                                    " or tbl_name='Atom' " +
                                    " or tbl_name='SpecAdditional' " +
                                    " or tbl_name='StandardData' " +
                                    " or tbl_name='CurveElement' " +
                                    " or tbl_name='ElementList'" +
                                    " or tbl_name='StandSample'" +
                                    " or tbl_name='Collimator'" +
                                    " or tbl_name='Chamber'" +
                                    " or tbl_name='WorkCurve'" +
                                     " or tbl_name='SpectrumData'" +
                                    " or tbl_name='ContinuousResult'";
                                     if (ConnectionString.GetTableNames().Contains("SpecList"))
                                        sqlTable += " or tbl_name='SpecList'";
                                   sqlTable+=" )and type='table'";
            
           
            //获取指定的表结构
            DataTable dt = GetData(sqlTable, ConnectionString.Driver.ConnectionString);
            //获取指定的触发器
            //DataTable dt_trigger = GetData("select * from sqlite_master  where  type='trigger' and name='INSERT_copuuu_NAME'", ConnectionString.Driver.ConnectionString);
            if (dt == null || dt.Rows.Count == 0) return;
            string sql = "";

            #region
            foreach (DataRow row in dt.Rows)
            {

                switch (row["tbl_name"].ToString())
                {
                    case "SpectrumData":
                        if (row["sql"].ToString().IndexOf("Height") == -1) sql += " alter table SpectrumData add Height  float default 0;";
                        if (row["sql"].ToString().IndexOf("CalcAngleHeight") == -1) sql += " alter table SpectrumData add CalcAngleHeight  float default 0; update  SpectrumData set CalcAngleHeight =Height;";
                        
                        break;
                    case "CustomStandard":
                        if (row["sql"].ToString().IndexOf("TotalContentStandard") == -1) sql += " alter table CustomStandard add TotalContentStandard float ;alter table CustomStandard add IsSelectTotal bool;  update customstandard set TotalContentStandard=0,IsSelectTotal=0 ; ";
                        if (row["sql"].ToString().IndexOf("StandardThick") == -1) sql += " alter table CustomStandard add StandardThick  float default 0;";
                        if (row["sql"].ToString().IndexOf("StandardThickMax") == -1) sql += " alter table CustomStandard add StandardThickMax  float default 0;";
                       
                        break;
                    case "HistoryElement":

                        #region 全国花提供过来的数据库发现customstandard_Id和unitValue类型不正确，进行修改
                        string strsql_idandunitValue = "";
                        if (row["sql"].ToString().IndexOf("[customstandard_Id] NTEXT") != -1)
                            strsql_idandunitValue = row["sql"].ToString().Replace("[customstandard_Id] NTEXT", "[customstandard_Id] BIGINT");

                        if (row["sql"].ToString().IndexOf("[unitValue] NTEXT") != -1)
                            strsql_idandunitValue = strsql_idandunitValue.Replace("[unitValue] NTEXT", "[unitValue] INT");
                        if (strsql_idandunitValue != "") sql += "  drop table if exists HistoryElementOld; ALTER TABLE HistoryElement RENAME TO HistoryElementOld;" + strsql_idandunitValue + " ;INSERT INTO HistoryElement SELECT * FROM HistoryElementOld;DROP TABLE HistoryElementOld;";

                        #endregion


                        if (row["sql"].ToString().IndexOf("customstandard_Id") == -1)
                            sql += " alter table HistoryElement add customstandard_Id begint ;alter table HistoryElement add unitValue int;";
                        else sql += @" update  historyelement set customstandard_id=0 where  customstandard_id='' or  customstandard_id=null ;
                            update  historyelement set unitValue=0 where  unitValue='' or  unitValue=null ; ";



                        if (row["sql"].ToString().IndexOf("ElementIntensity") == -1) sql += " alter table HistoryElement add ElementIntensity float default 0;";

                        if (row["sql"].ToString().IndexOf("thickunitValue") == -1) sql += " alter table HistoryElement add thickunitValue int default 0;";

                        if (row["sql"].ToString().IndexOf("Error") == -1) sql += " alter table HistoryElement add Error FLOAT default 0;";
                        if (row["sql"].ToString().IndexOf("CaculateIntensity") == -1)
                            sql += " alter table HistoryElement add CaculateIntensity FLOAT default 0;";


                        if (row["sql"].ToString().IndexOf("AverageValue") == -1) sql += " alter table HistoryElement add AverageValue FLOAT default 0;";
                        //追加面密度保存
                        if (row["sql"].ToString().IndexOf("densityelementValue") == -1) sql += " alter table HistoryElement add densityelementValue ntext default '0';";
                        if (row["sql"].ToString().IndexOf("densityunitValue") == -1) sql += " alter table HistoryElement add densityunitValue ntext default 'g/cm^2';";
                        if (row["sql"].ToString().IndexOf("contentRealelemValue") == -1) sql += " alter table HistoryElement add contentRealelemValue ntext default '0';";
                       
                        break;
                    case "FPGAParams":
                        if (row["sql"].ToString().IndexOf("FastLimit") == -1) sql += " alter table FPGAParams add FastLimit int default 100000;";
                        if (row["sql"].ToString().IndexOf("Intercept") == -1) sql += " alter table FPGAParams add Intercept float default 0;";
                        break;
                    case "ElementList":
                        if (row["sql"].ToString().IndexOf("[SpecListId] BIGINT NOT NULL") != -1)
                        {
                            string strsql = row["sql"].ToString().Replace("[SpecListId] BIGINT NOT NULL ,", "[SpecListName] NVARCHAR (100) NULL ,");
                            sql += " drop table if exists ElementOld; ALTER TABLE ElementList RENAME TO ElementOld;" + strsql + @" ;insert into ElementList([Id],[IsUnitary],[UnitaryValue],[TubeWindowThickness],[RhIsLayer],[RhLayerFactor],[IsAbsorption],[ThCalculationWay],[DBlLimt],[IsRemoveBk],[SpecListName],[IsReportCategory],[PureAsInfinite],[WorkCurve_Id]) 
                                    Select [Id],[IsUnitary],[UnitaryValue],[TubeWindowThickness],[RhIsLayer],[RhLayerFactor],[IsAbsorption],[ThCalculationWay],[DBlLimt],[IsRemoveBk],[SpecListId],[IsReportCategory],[PureAsInfinite],[WorkCurve_Id] From ElementOld;;DROP TABLE ElementOld;";
                        }
                        if (row["sql"].ToString().IndexOf("MatchSpecListIdStr") == -1) sql += " alter table ElementList add MatchSpecListIdStr ntext default '';";
                        else if (row["sql"].ToString().IndexOf("MatchSpecListIdStr") != -1) sql += " update elementlist set matchspeclistidstr='' where  matchspeclistidstr isnull ;";

                        if (row["sql"].ToString().IndexOf("RefSpecListIdStr") == -1) sql += " alter table ElementList add RefSpecListIdStr ntext default '';";
                        else if (row["sql"].ToString().IndexOf("RefSpecListIdStr") != -1) sql += " update elementlist set RefSpecListIdStr='' where  RefSpecListIdStr isnull ;";

                        if (row["sql"].ToString().IndexOf("RhIsMainElementInfluence") == -1) sql += " alter table ElementList add RhIsMainElementInfluence bool default 0;";
                        if (row["sql"].ToString().IndexOf("NoStandardAlert") == -1) sql += " alter table ElementList add NoStandardAlert bool default 0;";
                        if (row["sql"].ToString().IndexOf("AreaDensity") == -1) sql += " alter table ElementList add AreaDensity FLOAT default 0;";
                        if (row["sql"].ToString().IndexOf("MainElementToCalcKarat") == -1) sql += " alter table ElementList add MainElementToCalcKarat ntext default 'Au';";
                        if (row["sql"].ToString().IndexOf("LayerElemsInAnalyzer") == -1) sql += " alter table ElementList add LayerElemsInAnalyzer ntext default 'Rh';";
                        break;
                    case "InitParameter":
                        if (row["sql"].ToString().IndexOf("Target") == -1) sql += " alter table InitParameter add Target int default 1;";
                        if (row["sql"].ToString().IndexOf("TargetMode") == -1) sql += " alter table InitParameter add TargetMode int default 0;";
                        if (row["sql"].ToString().IndexOf("CurrentRate") == -1) sql += " alter table InitParameter add CurrentRate int default 1;";

                        if (row["sql"].ToString().IndexOf("IsAdjustRate") == -1) sql += " alter table InitParameter add IsAdjustRate bool default 0;";
                        if (row["sql"].ToString().IndexOf("MinRate") == -1) sql += " alter table InitParameter add MinRate float default 0;";
                        if (row["sql"].ToString().IndexOf("MaxRate") == -1) sql += " alter table InitParameter add MaxRate float default 0;";
                        if (row["sql"].ToString().IndexOf("IsJoinInit") == -1) sql += " alter table InitParameter add IsJoinInit bool default 0;";
                        if (row["sql"].ToString().IndexOf("ExpressionFineGain") == -1) sql += " alter table InitParameter add ExpressionFineGain ntext default 'x';";
                        if (row["sql"].ToString().IndexOf("InitFistCount") == -1) sql += " alter table InitParameter add InitFistCount float default 0;";
                        if (row["sql"].ToString().IndexOf("InitCalibrateRatio") == -1) sql += " alter table InitParameter add InitCalibrateRatio  float default 1;";

                        
                        break;
                    case "PreHeatParams":
                        if (row["sql"].ToString().IndexOf("Target") == -1) sql += " alter table PreHeatParams add Target int default 1;";
                        if (row["sql"].ToString().IndexOf("TargetMode") == -1) sql += " alter table PreHeatParams add TargetMode int default 0;";
                        if (row["sql"].ToString().IndexOf("CurrentRate") == -1) sql += " alter table PreHeatParams add CurrentRate int default 1;";
                        break;
                    case "WorkRegion":
                        if (row["sql"].ToString().IndexOf("Caption") == -1) sql += " alter table WorkRegion add Caption ntext default '';";
                        else if (row["sql"].ToString().IndexOf("Caption") != -1 && row["sql"].ToString().IndexOf("string") != -1)
                        {
                            string strsql = row["sql"].ToString().Replace("string", "ntext").Replace("\n\t", "").Replace("\n", "").Replace("\t", "");

                            sql += " drop table if exists WorkRegionOld; ALTER TABLE WorkRegion RENAME TO WorkRegionOld;" + strsql + " ;INSERT INTO WorkRegion SELECT * FROM WorkRegionOld;DROP TABLE WorkRegionOld;";
                        }
                        break;
                    case "Spec":
                        bool isOne = false;
                        if (row["sql"].ToString().IndexOf(",\n\t[Name] NVARCHAR (100) NOT NULL ,") != -1)
                        {
                            string strsql = row["sql"].ToString().Replace(",\n\t[Name] NVARCHAR (100) NOT NULL ,", ",\n\t[Name] NVARCHAR (100) NULL ,");
                            strsql = strsql.Replace("\n\t", "").Replace("\n", "");

                            sql += " drop table if exists SpecOld; ALTER TABLE Spec RENAME TO SpecOld;" + strsql + " ;INSERT INTO Spec SELECT * FROM SpecOld;DROP TABLE SpecOld;";
                            isOne = true;
                        }
                        if (row["sql"].ToString().IndexOf("[UsedTime] INT NOT NULL") != -1
                            ||row["sql"].ToString().IndexOf("[TubVoltage] INT NOT NULL") != -1
                            ||row["sql"].ToString().IndexOf("[TubCurrent] INT NOT NULL") != -1)
                        {
                            string strsql=string.Empty;
                            if(row["sql"].ToString().IndexOf("[UsedTime] INT NOT NULL") != -1)
                                 strsql   = row["sql"].ToString().Replace(",\n\t[UsedTime] INT NOT NULL ,", ",\n\t[UsedTime] FLOAT NOT NULL ,");

                            if(row["sql"].ToString().IndexOf("[TubVoltage] INT NOT NULL") != -1&& strsql==string.Empty)
                                 strsql = row["sql"].ToString().Replace(",\n\t[TubVoltage] INT NOT NULL ,", ",\n\t[TubVoltage] FLOAT NOT NULL ,");
                            else strsql = strsql.Replace(",\n\t[TubVoltage] INT NOT NULL ,", ",\n\t[TubVoltage] FLOAT NOT NULL ,");

                            if (row["sql"].ToString().IndexOf("[TubCurrent] INT NOT NULL") != -1 && strsql == string.Empty)
                                strsql = row["sql"].ToString().Replace(",\n\t[TubCurrent] INT NOT NULL ,", ",\n\t[TubCurrent] FLOAT NOT NULL ,");
                            else strsql = strsql.Replace(",\n\t[TubCurrent] INT NOT NULL ,", ",\n\t[TubCurrent] FLOAT NOT NULL ,");

                            if (isOne) strsql = strsql.Replace(",\n\t[Name] NVARCHAR (100) NOT NULL ,", ",\n\t[Name] NVARCHAR (100) NULL ,");

                            sql += " drop table if exists SpecOld; ALTER TABLE Spec RENAME TO SpecOld;" + strsql + " ;INSERT INTO Spec SELECT * FROM SpecOld;DROP TABLE SpecOld;";
                        }
                        if (row["sql"].ToString().IndexOf("IsSmooth") == -1) { sql += " alter table Spec add IsSmooth bool default 1;"; }

                        //if (row["sql"].ToString().IndexOf("SpecOrignialData") == -1) { sql += " alter table Spec add SpecOrignialData NTEXT default ''; update Spec set SpecOrignialData=SpecData;"; }
                        break;
                    case "DemarcateEnergy":
                        if (row["sql"].ToString().IndexOf("[Channel] FLOAT NOT NULL") == -1)
                        {
                            string strsql = row["sql"].ToString().Replace("[Channel] INT NOT NULL", "[Channel] FLOAT NOT NULL");
                            sql += " drop table if exists DemarcateEnergyOld; ALTER TABLE DemarcateEnergy RENAME TO DemarcateEnergyOld;" + strsql + " ;INSERT INTO DemarcateEnergy SELECT * FROM DemarcateEnergyOld;DROP TABLE DemarcateEnergyOld;";
                        }
                        break;
                    //case "HistoryElement":
                    //    if (row["sql"].ToString().IndexOf("[customstandard_Id] bigint") == -1)
                    //    {
                    //        string strsql = row["sql"].ToString().Replace("[customstandard_Id] ntext", "[customstandard_Id] bigint");
                    //        if (row["sql"].ToString().IndexOf("[unitValue] int") == -1)
                    //            strsql = row["sql"].ToString().Replace("[unitValue] ntext", "[unitValue] int");
                    //        sql += " ALTER TABLE HistoryElement RENAME TO HistoryElementOld;" + strsql + " ;INSERT INTO HistoryElement SELECT * FROM HistoryElementOld;DROP TABLE HistoryElementOld;";
                    //    }
                    //    break;
                    case "Tubes":
                        if (row["sql"].ToString().IndexOf("TubVoltage") == -1) sql += " alter table Tubes add TubVoltage float default 40;";
                        if (row["sql"].ToString().IndexOf("PercentVoltage") == -1) sql += " alter table Tubes add PercentVoltage float default 0;";
                        break;
                    case "Device":
                        if (row["sql"].ToString().IndexOf("HasTarget") == -1) sql += " alter table Device add HasTarget BOOL default 0 ;alter table Device add TargetElectricalCode int default 0;alter table Device add TargetElectricalDirect int default 0;alter table Device add TargetMaxNum int default 0;alter table Device add TargetSpeed int default 0;";
                        if (row["sql"].ToString().IndexOf("IsAutoDetection") == -1) sql += " alter table Device add IsAutoDetection BOOL NOT NULL DEFAULT 0;";
                        if (row["sql"].ToString().IndexOf("HasMotorY1") == -1) sql += " alter table Device add HasMotorY1 BOOL default 0 ;alter table Device add MotorY1Code int default 0;alter table Device add MotorY1Direct int default 0;alter table Device add MotorY1Speed int default 0;alter table Device add MotorY1MaxStep int default 0;";
                        if (row["sql"].ToString().IndexOf("MotorZDutyRatioUp") == -1) sql += "alter table Device add MotorZDutyRatioUp float default 0 ;alter table Device add MotorZDutyRatioDown float default 0;";
                        if (row["sql"].ToString().IndexOf("HasMotorLight") == -1) sql += " alter table Device add HasMotorLight BOOL default 0 ;alter table Device add MotorLightCode int default 0;alter table Device add MotorLightDirect int default 0;alter table Device add MotorLightSpeed int default 0;alter table Device add MotorLightMaxStep int default 0;";
                        if (row["sql"].ToString().IndexOf("Dp5Version") == -1) sql += " alter table Device add Dp5Version int default 0 ;";
                        if (row["sql"].ToString().IndexOf("MotorResetX") == -1) sql += " alter table Device add MotorResetX int default 0 ;";
                        if (row["sql"].ToString().IndexOf("MotorResetY") == -1) sql += " alter table Device add MotorResetY int default 0 ;";
                        if (row["sql"].ToString().IndexOf("EncoderFormula") == -1) sql += " alter table Device add EncoderFormula NTEXT default 'x';"; 
                        break;
                    case "DeviceParameter":
                        if (row["sql"].ToString().IndexOf("TargetIdx") == -1) sql += " alter table DeviceParameter add TargetIdx int default 0;";
                        if (row["sql"].ToString().IndexOf("IsFaceTubVoltage") == -1) sql += " alter table DeviceParameter add IsFaceTubVoltage bool default 0;alter table DeviceParameter add FaceTubVoltage int default 0;";
                        if (row["sql"].ToString().IndexOf("TargetMode") == -1) sql += " alter table DeviceParameter add TargetMode int default 0;";
                        if (row["sql"].ToString().IndexOf("CurrentRate") == -1) sql += " alter table DeviceParameter add CurrentRate int default 1;";
                        break;
                    case "SurfaceSource":
                        if (row["sql"].ToString().IndexOf("ThirdLight") == -1) sql += "alter table SurfaceSource add ThirdLight SMALLINT default 1000;";
                        if (row["sql"].ToString().IndexOf("FourthLight") == -1) sql += "alter table SurfaceSource add FourthLight SMALLINT default 1000;";
                        break;
                    case "Optimiztion":
                        if (row["sql"].ToString().IndexOf("OptimizetionType") == -1) sql += " alter table Optimiztion add OptimizetionType int default 0;";
                        if (row["sql"].ToString().IndexOf("OptimiztionMax") == -1) sql += " alter table Optimiztion add OptimiztionMax float default 0;";
                        if (row["sql"].ToString().IndexOf("OptimiztionMin") == -1) sql += " alter table Optimiztion add OptimiztionMin float default 0;";
                        if (row["sql"].ToString().IndexOf("OptExpression") == -1) sql += " alter table Optimiztion add OptExpression NTEXT default 'x';";
                        if (row["sql"].ToString().IndexOf("IsJoinIntensity") == -1) sql += " alter table Optimiztion add IsJoinIntensity BOOL default 1;";
                        
                        else sql += @" update Optimiztion set OptimiztionMax=OptimiztionRange, OptimiztionMin=-OptimiztionRange,OptimiztionRange=0 where OptimiztionRange <>0;";
                        break;
                    case "CalibrationParam"://校正参数中，添加扣本底4功能
                        if (row["sql"].ToString().IndexOf("IsRemoveBackGroundFour") == -1) sql += " alter table CalibrationParam add IsRemoveBackGroundFour BOOL default 0; alter table CalibrationParam add RemoveFourTimes int default 1 ;alter table CalibrationParam add RemoveFourLeft int default 0;alter table CalibrationParam add RemoveFourRight int default 0;";
                        //if (row["sql"].ToString().IndexOf("IsRemoveBackGroundFive") == -1) sql += " alter table CalibrationParam add IsRemoveBackGroundFive BOOL default 0; alter table CalibrationParam add BackGroundPointFive NTEXT default '';";
                        break;
                    case "SpecList":
                        if (row["sql"].ToString().IndexOf("PeakChannel int") != -1)
                        {
                            string strsql_idandunit = "";
                            strsql_idandunit = row["sql"].ToString().Replace("PeakChannel int", "PeakChannel float");
                            sql += " drop table if exists SpecListOld; ALTER TABLE SpecList RENAME TO SpecListOld;" + strsql_idandunit + " ;INSERT INTO SpecList SELECT * FROM SpecListOld;DROP TABLE SpecListOld;";
                        }
                        if (row["sql"].ToString().IndexOf("ActualVoltage") == -1) sql += " alter table SpecList add ActualVoltage float default 0;";
                        if (row["sql"].ToString().IndexOf("ActualCurrent") == -1) sql += " alter table SpecList add ActualCurrent float default 0;";
                        if (row["sql"].ToString().IndexOf("CountRate") == -1) sql += " alter table SpecList add CountRate float default 0;";
                        if (row["sql"].ToString().IndexOf("PeakChannel") == -1) sql += " alter table SpecList add PeakChannel float default 0;";
                        if (row["sql"].ToString().IndexOf("Resole") == -1) sql += " alter table SpecList add Resole float default 0;";
                        if (row["sql"].ToString().IndexOf("TotalCount") == -1) sql += " alter table SpecList add TotalCount int default 0;";
                        if (row["sql"].ToString().IndexOf("ImageShow") == -1) sql += " alter table SpecList add ImageShow bool default 0;";
                        if (row["sql"].ToString().IndexOf("Loss") == -1) sql += " alter table SpecList add Loss float default  0;";
                        if (row["sql"].ToString().IndexOf("NameType") == -1) sql += " alter table SpecList add NameType int default 0;";
                        break;
                    case "HistoryRecord":
                        if (row["sql"].ToString().IndexOf("PeakChannel int") != -1 || row["sql"].ToString().IndexOf("TotalCount int")!=-1)
                        {
                            string strsql_idandunit = "";
                            strsql_idandunit = row["sql"].ToString().Replace("PeakChannel int", "PeakChannel float");
                            strsql_idandunit = strsql_idandunit.Replace("TotalCount int", "TotalCount bigint");
                            sql += " drop table if exists HistoryRecordOld; ALTER TABLE HistoryRecord RENAME TO HistoryRecordOld;" + strsql_idandunit + " ;INSERT INTO HistoryRecord SELECT * FROM HistoryRecordOld;DROP TABLE HistoryRecordOld;";
                        }
                        if (row["sql"].ToString().IndexOf("ActualVoltage") == -1) sql += " alter table HistoryRecord add ActualVoltage float default 0;";
                        if (row["sql"].ToString().IndexOf("ActualCurrent") == -1) sql += " alter table HistoryRecord add ActualCurrent float default 0;";
                        if (row["sql"].ToString().IndexOf("CountRate") == -1) sql += " alter table HistoryRecord add CountRate float default 0;";
                        if (row["sql"].ToString().IndexOf("PeakChannel") == -1) sql += " alter table HistoryRecord add PeakChannel float default 0;";
                        if (row["sql"].ToString().IndexOf("Resole") == -1) sql += " alter table HistoryRecord add Resole float default 0;";
                        if (row["sql"].ToString().IndexOf("TotalCount") == -1) sql += " alter table HistoryRecord add TotalCount bigint default 0;";
                        //else
                        //{
                        //    int indexTmp=row["sql"].ToString().IndexOf("TotalCount");
                        //    string strTmp=row["sql"].ToString().IndexOf(",",indexTmp)==-1?row["sql"].ToString().Substring(row["sql"].ToString().IndexOf(",",indexTmp)):row["sql"].ToString().Substring(indexTmp, row["sql"].ToString().IndexOf(",",indexTmp)-indexTmp);
                        //    if (!strTmp.Contains("long") || !strTmp.Contains("bigint"))
                        //        sql += " alter table HistoryRecord alter column TotalCount bigint;";
                        //}
                        if (row["sql"].ToString().IndexOf("SpecListId") == -1) sql += " alter table HistoryRecord add SpecListId bigint default 0;";
                        if (row["sql"].ToString().IndexOf("HistoryRecordCode") == -1) sql += " alter table HistoryRecord add HistoryRecordCode ntext default '';";
                        if (row["sql"].ToString().IndexOf("IsScan") == -1) sql += " alter table HistoryRecord add IsScan BOOL default 0;";
                        if (row["sql"].ToString().IndexOf("FilePath") == -1) sql += " alter table HistoryRecord add FilePath NTEXT default '';";
                        if (row["sql"].ToString().IndexOf("EditionType") == -1) sql += " alter table HistoryRecord add EditionType int default 0;";
                        if (row["sql"].ToString().IndexOf("StockStatus") == -1) sql += " alter table HistoryRecord add StockStatus int default 0;";
                        if (row["sql"].ToString().IndexOf("Specifications") == -1) sql += " alter table HistoryRecord add Specifications NTEXT default '';";
                        if (row["sql"].ToString().IndexOf("AreaDensity") == -1) sql += " alter table HistoryRecord add AreaDensity FLOAT default 0;";
                        if (row["sql"].ToString().IndexOf("Height") == -1) sql += " alter table HistoryRecord add Height FLOAT default 0;";
                        if (row["sql"].ToString().IndexOf("CalcAngleHeight") == -1) sql += " alter table HistoryRecord add CalcAngleHeight  float default 0; update  HistoryRecord set CalcAngleHeight =Height;";
                        
                        
                        break;
                    //case "SpecAdditional":
                    //    if (row["sql"].ToString().IndexOf("XrfChartPath") == -1) sql += " alter table SpecAdditional add XrfChartPath NTEXT default '';";
                    //    break;
                    case "Atom":
                        if (row["sql"].ToString().IndexOf("AtomNameEN") == -1) sql += " alter table Atom add AtomNameEN NTEXT default '';";
                        break;
                    case "CompanyOthersInfo":
                        if (row["sql"].ToString().IndexOf("WorkCurveId") != -1)//删除WorkCurveId列
                        {
                            string strsql = row["sql"].ToString().Replace("\n\t[WorkCurveId] BIGINT NOT NULL ,", "");
                            strsql = strsql.Replace("\n\t", "").Replace("\n", "");

                            string sTableColmun = GetColmun("CompanyOthersInfo", strsql);

                            sql += " drop table if exists CompanyOthersInfoOld; ALTER TABLE CompanyOthersInfo RENAME TO CompanyOthersInfoOld;" + strsql + " ;INSERT INTO CompanyOthersInfo select " + sTableColmun + " FROM CompanyOthersInfoOld;DROP TABLE CompanyOthersInfoOld;";
                        }

                        if (row["sql"].ToString().IndexOf("DefaultValue") == -1) sql += " alter table CompanyOthersInfo add DefaultValue NTEXT default '';";
                        if (row["sql"].ToString().IndexOf("ExcelModeType") == -1) sql += " alter table CompanyOthersInfo add ExcelModeType NTEXT default '';";
                        if (row["sql"].ToString().IndexOf("ExcelModeTarget") == -1) { sql += " alter table CompanyOthersInfo add ExcelModeTarget NTEXT default '';"; isExcelModeTarget = true; }
                        break;
                    case "StandardData":

                        if (row["sql"].ToString().IndexOf("StartStandardContent") == -1) sql += " alter table StandardData add StartStandardContent  float default 0;";
                        if (row["sql"].ToString().IndexOf("StandardThick") == -1) sql += " alter table StandardData add StandardThick  float default 0;";
                        if (row["sql"].ToString().IndexOf("StandardThickMax") == -1) sql += " alter table StandardData add StandardThickMax  float default 0;";
                        break;
                    case "CurveElement":
                        if (row["sql"].ToString().IndexOf("BgIntensity") == -1) sql += " alter table CurveElement add BgIntensity float default 0;";
                        if (row["sql"].ToString().IndexOf("IsOxide") == -1) sql += " alter table CurveElement add IsOxide bool default 0;";
                        if (row["sql"].ToString().IndexOf("CumulativeValue") == -1) sql += " alter table CurveElement add CumulativeValue float default 0;";
                        if (row["sql"].ToString().IndexOf("BaseIntensityWay") == -1) sql += " alter table CurveElement add BaseIntensityWay int default 1;";
                        if (row["sql"].ToString().IndexOf("IsShowElement") == -1) sql += " alter table CurveElement add IsShowElement bool default 1;";
                        if (row["sql"].ToString().IndexOf("IsShowContent") == -1) sql += " alter table CurveElement add IsShowContent bool default 1;";
                        if (row["sql"].ToString().IndexOf("IsAlert") == -1) sql += " alter table CurveElement add IsAlert bool default 0;";
                        if (row["sql"].ToString().IndexOf("Contentcoeff") == -1) sql += "alter table CurveElement add Contentcoeff float default 1;";
                        if (row["sql"].ToString().IndexOf("ContentRealValue") == -1) sql += "alter table CurveElement add ContentRealValue float default 0;";
                        if (row["sql"].ToString().IndexOf("ElementEncoderSpecName") == -1) sql += "alter table CurveElement add ElementEncoderSpecName NTEXT default ''; update  CurveElement set ElementEncoderSpecName =Caption;";
                        if (row["sql"].ToString().IndexOf("IsBorderlineElem") == -1) sql += "alter table CurveElement add IsBorderlineElem bool default 0;";
                        if (row["sql"].ToString().IndexOf("ElementSpecNameNoFilter") == -1) sql += "alter table CurveElement add ElementSpecNameNoFilter NTEXT default '';";
                        if (row["sql"].ToString().IndexOf("SSpectrumDataNotFilter") == -1) sql += "alter table CurveElement add SSpectrumDataNotFilter NTEXT default ''; ";
                        if (row["sql"].ToString().IndexOf("IsShowDefineName") == -1) sql += "alter table CurveElement add IsShowDefineName bool default 0;";
                        if (row["sql"].ToString().IndexOf("DefineElemName") == -1) sql += "alter table CurveElement add DefineElemName NTEXT default ''; update  CurveElement set DefineElemName =Caption;";
                       
                        break;
                    case "Collimator":
                        if (row["sql"].ToString().IndexOf("Diameter") == -1) sql += " alter table Collimator add Diameter float default 0;";
                        break;
                    case "Chamber":
                        if (row["sql"].ToString().IndexOf("StepCoef1") == -1) sql += " alter table Chamber add StepCoef1 int default 0;";
                        if (row["sql"].ToString().IndexOf("StepCoef2") == -1) sql += " alter table Chamber add StepCoef2 int default 0;";
                        break;
                    case "WorkCurve":
                        if (row["sql"].ToString().IndexOf("SimilarCurveId") == -1) sql += " alter table WorkCurve add SimilarCurveId int default 0;";
                        if (row["sql"].ToString().IndexOf("SimilarCurveName") == -1) sql += " alter table WorkCurve add SimilarCurveName NTEXT default '';";
                        if (row["sql"].ToString().IndexOf("IsThickShowAreaThick") == -1) sql += " alter table WorkCurve add IsThickShowAreaThick bool default 0;";
                        if (row["sql"].ToString().IndexOf("AreaThickType") == -1) sql += " alter table WorkCurve add AreaThickType NTEXT default 'g/cm^2';";
                        if (row["sql"].ToString().IndexOf("MainElements") == -1) sql += " alter table WorkCurve add MainElements NTEXT default '';";
                        if (row["sql"].ToString().IndexOf("IsCurrentNormalize") == -1) sql += " alter table WorkCurve add IsCurrentNormalize bool default 0;";
                        if (row["sql"].ToString().IndexOf("InCalType") == -1) sql += " alter table WorkCurve add InCalType int default 1;";
                        if (row["sql"].ToString().IndexOf("InCalSampName") == -1) sql += " alter table WorkCurve add InCalSampName NTEXT default '';";
                        if (row["sql"].ToString().IndexOf("InCalSampNameL") == -1) sql += " alter table WorkCurve add InCalSampNameL NTEXT default '';";
                        if (row["sql"].ToString().IndexOf("RemarkName") == -1) sql += " alter table WorkCurve add RemarkName NTEXT default '';";
                        if (row["sql"].ToString().IndexOf("ThickStandardName") == -1) sql += " alter table WorkCurve add ThickStandardName NTEXT default '';";
                        if (row["sql"].ToString().IndexOf("TestTime") == -1) sql += " alter table WorkCurve add TestTime int default '40';";
                        if (row["sql"].ToString().IndexOf("IsShowMain") == -1) sql += " alter table WorkCurve add IsShowMain bool default 1;";
                        if (row["sql"].ToString().IndexOf("IsNiP2") == -1) sql += " alter table WorkCurve add IsNiP2 bool default 0;";
                        if (row["sql"].ToString().IndexOf("IsBaseAdjust") == -1) sql += " alter table WorkCurve add IsBaseAdjust bool default 0;";
                       
                        
                        
                        break;
                    case "StandSample":
                        string strsqlResult = string.Empty;
                        if (row["sql"].ToString().IndexOf("[X] REAL") != -1)
                        {
                            strsqlResult = row["sql"].ToString().Replace("[X] REAL", "[X] NVARCHAR(100)");
                            strsqlResult = strsqlResult.Replace("[Y] REAL", "[Y] NVARCHAR(100)");
                            strsqlResult = strsqlResult.Replace("[Z] REAL", "[Z] NVARCHAR(100)");
                            sql += " drop table if exists StandSampleOld; ALTER TABLE StandSample RENAME TO StandSampleOld;" + strsqlResult + " ;INSERT INTO StandSample SELECT * FROM StandSampleOld;DROP TABLE StandSampleOld;";
                        }
                        if (row["sql"].ToString().IndexOf("[SpecListId] BIGINT NOT NULL") != -1)
                        {
                            strsqlResult = row["sql"].ToString().Replace("\n\t[SpecListId] BIGINT NOT NULL ,", "");
                            if (strsqlResult.IndexOf("[X] REAL") != -1)
                            {
                                strsqlResult = strsqlResult.Replace("[X] REAL", "[X] NVARCHAR(100)");
                                strsqlResult = strsqlResult.Replace("[Y] REAL", "[Y] NVARCHAR(100)");
                                strsqlResult = strsqlResult.Replace("[Z] REAL", "[Z] NVARCHAR(100)");
                            }
                            strsqlResult = strsqlResult.Replace("\n\t", "").Replace("\n", "");
                            string sTableColmun = GetColmun("StandSample", strsqlResult);
                            sql += " drop table if exists StandSampleOld; ALTER TABLE StandSample RENAME TO StandSampleOld;" + strsqlResult + " ;INSERT INTO StandSample select " + sTableColmun + " FROM StandSampleOld;DROP TABLE StandSampleOld;";
                        }
                        if (row["sql"].ToString().IndexOf("[MatchSpecListId] BIGINT NOT NULL") != -1)
                        {
                            if (string.IsNullOrEmpty(strsqlResult))
                                strsqlResult = row["sql"].ToString().Replace("\n\t[MatchSpecListId] BIGINT NOT NULL ,", "");
                            else
                                strsqlResult = strsqlResult.Replace("[MatchSpecListId] BIGINT NOT NULL ,", "");
                            if (strsqlResult.IndexOf("[X] REAL") != -1)
                            {
                                strsqlResult = strsqlResult.Replace("[X] REAL", "[X] NVARCHAR(100)");
                                strsqlResult = strsqlResult.Replace("[Y] REAL", "[Y] NVARCHAR(100)");
                                strsqlResult = strsqlResult.Replace("[Z] REAL", "[Z] NVARCHAR(100)");
                            }
                            strsqlResult = strsqlResult.Replace("\n\t", "").Replace("\n", "");
                            string sTableColmun = GetColmun("StandSample", strsqlResult);
                            sql += " drop table if exists StandSampleOld; ALTER TABLE StandSample RENAME TO StandSampleOld;" + strsqlResult + " ;INSERT INTO StandSample select " + sTableColmun + " FROM StandSampleOld;DROP TABLE StandSampleOld;";
                        }
                        //追加不确定度
                        if (row["sql"].ToString().IndexOf("Uncertainty") == -1) sql += " alter table StandSample add Uncertainty NTEXT default '0';";
                        if (row["sql"].ToString().IndexOf("Height") == -1) sql += " alter table StandSample add Height NTEXT default '0';";
                        if (row["sql"].ToString().IndexOf("CalcAngleHeight") == -1) sql += " alter table StandSample add CalcAngleHeight  NTEXT default '0'; update  StandSample set CalcAngleHeight =Height;";
                        
                        break;
                    case "ContinuousResult":
                        if (row["sql"].ToString().IndexOf("HistoryRecordCode") == -1) sql += " alter table ContinuousResult add HistoryRecordCode NTEXT default '';";
                        if (row["sql"].ToString().IndexOf("Supplier") == -1) sql += " alter table ContinuousResult add Supplier NTEXT default '';";
                        break;
                    case "IntensityCalibration":
                        if (row["sql"].ToString().IndexOf("PeakLeft") == -1) sql += " alter table IntensityCalibration add PeakLeft int default 0;";
                        if (row["sql"].ToString().IndexOf("PeakRight") == -1) sql += " alter table IntensityCalibration add PeakRight int default 0;";
                       // if (row["sql"].ToString().IndexOf("ConSampleName") == -1) sql += " alter table IntensityCalibration add ConSampleName NTEXT default '';";
                        if (row["sql"].ToString().IndexOf("OriginalBaseIn") == -1) sql += " alter table IntensityCalibration add OriginalBaseIn float default 0;";
                        if (row["sql"].ToString().IndexOf("CalibrateBaseIn") == -1) sql += " alter table IntensityCalibration add CalibrateBaseIn float default 0;";
                        if (row["sql"].ToString().IndexOf("InCalType") == -1) sql += " alter table IntensityCalibration add InCalType int default 1;";
                        break;
                    case "PureSpecParam":
                        if (row["sql"].ToString().IndexOf("StandSampleName") == -1) sql += " alter table PureSpecParam add StandSampleName NTEXT default '';";
                        if (row["sql"].ToString().IndexOf("ElementName") == -1) sql += " alter table PureSpecParam add ElementName NTEXT default '';  update PureSpecParam set ElementName = SampleName;";  //之前的数据库samplename=纯元素
                        if (row["sql"].ToString().IndexOf("Current") == -1) sql += " alter table PureSpecParam add Current float default 1;";
                        if (row["sql"].ToString().IndexOf("CurrentUnifyCount") == -1) sql += " alter table PureSpecParam add CurrentUnifyCount float default 0;update PureSpecParam set CurrentUnifyCount = TotalCount;";
                        
                        break;

                }


            }

            #endregion
            if (sql != "") dt = GetData(sql, ConnectionString.Driver.ConnectionString);


            #region 添加表索引
            sqlTable = @"select tbl_name,sql,type from sqlite_master where (tbl_name='HistoryElement' " +
            " or tbl_name='HistoryRecord' " +
            " or tbl_name='Atom' " +
            " or tbl_name='CurveElement' " +
            " or tbl_name='ElementList'" +
            " or tbl_name='Oxide'" +
            " or tbl_name='SpectrumData'";
            sqlTable += ") and type='index'";
            dt = GetData(sqlTable, ConnectionString.Driver.ConnectionString);
            sql = "";
            if (dt.Select("tbl_name='Atom'").Length == 0)
            {
                sql += "CREATE INDEX [AtomName_Atom] On [Atom] ([AtomName] ); CREATE INDEX [AtomNameCN_Atom] On [Atom] ([AtomNameCN] ); CREATE INDEX [AtomNameEN_Atom] On [Atom] ([AtomNameEN] );";
            }

            if (dt.Select("tbl_name='HistoryRecord'").Length == 0)
            {
                sql += "CREATE INDEX [SampleName_HistoryRecord] On [HistoryRecord] ([SampleName] ); CREATE INDEX [SpecListName_HistoryRecord] On [HistoryRecord] ([SpecListName] ); ";
            }

            if (dt.Select("tbl_name='HistoryElement'").Length == 0)
            {
                sql += "CREATE INDEX [elementName_HistoryElement] On [HistoryElement] ([elementName] );  ";
            }

            if (dt.Select("tbl_name='CurveElement'").Length == 0)
            {
                sql += "CREATE INDEX [Caption_CurveElement] On [CurveElement] ([Caption] ); CREATE INDEX [Formula_CurveElement] On [CurveElement] ([Formula] ); ";
            }

            if (dt.Select("tbl_name='ElementList'").Length == 0)
            {
                sql += "CREATE INDEX [SpecListName_ElementList] On [ElementList] ([SpecListName] );";
            }

            if (dt.Select("tbl_name='Oxide'").Length == 0)
            {
                sql += "CREATE INDEX [OxideName_Oxide] On [Oxide] ([OxideName] ); CREATE INDEX [OxideNameCN_Oxide] On [Oxide] ([OxideNameCN] ); CREATE INDEX [OxideNameEN_Oxide] On [Oxide] ([OxideNameEN] );";
            }

            if (dt.Select("tbl_name='SpectrumData'").Length == 0)
            {
                sql += "CREATE INDEX [Name_SpectrumData] On [SpectrumData] ([Name] ); CREATE INDEX [DeviceName_SpectrumData] On [SpectrumData] ([DeviceName] ); CREATE INDEX [SampleName_SpectrumData] On [SpectrumData] ([SampleName] );";
            }
            if (sql != "")  GetData(sql, ConnectionString.Driver.ConnectionString);
            #endregion





            SetElementAllName(ConnectionString);

            if (isUpSpectrumData) UpSpectrumData();


            if (isExcelModeTarget) UpisExcelModeTarget();

            #region 修正数据库元素中文名称@20210406
            var atom = Atom.FindOne(con => con.Id == 44 && con.AtomNameCN == "镣");
            if (atom != null)
            {
                atom.AtomNameCN = "钌";
                atom.Save();
            }
            #endregion

        }


        #region CompanyOthersInfo 数据库升级
        private static void UpisExcelModeTarget()
        {
            List<CompanyOthersInfo> companyOthersInfoList = CompanyOthersInfo.FindBySql("select * from companyothersinfo where isreport=1");
            if(companyOthersInfoList.Count==0) return;
            GetReportInfo();
            foreach (CompanyOthersInfo comotherinfo in companyOthersInfoList)
            {
                string sTarget = string.Empty;
                GetTarget(comotherinfo.Name, ref sTarget);
                comotherinfo.ExcelModeTarget = sTarget;
            }

            foreach (CompanyOthersInfo comotherinfo in companyOthersInfoList)
                comotherinfo.Save();
        }

        private static void GetTarget(string sCombReportInfo, ref string sTarget)
        {
            if (xmlnodelist != null && xmlnodelist.Count > 0)
            {
                string strWhere = "lable[@EN = '" + sCombReportInfo + "']";
                XmlNodeList childxmlnodelist = xmlnodelist[0].SelectNodes(strWhere);
                if (childxmlnodelist == null || childxmlnodelist.Count == 0)
                {
                    strWhere = "lable[@CN = '" + sCombReportInfo + "']";
                    childxmlnodelist = xmlnodelist[0].SelectNodes(strWhere);
                    if (childxmlnodelist == null || childxmlnodelist.Count == 0) return;
                    sTarget = childxmlnodelist[0].Attributes["Target"].Value;
                }
                else
                sTarget = childxmlnodelist[0].Attributes["Target"].Value;
            }
        }


        private static XmlNodeList xmlnodelist;
        private static void GetReportInfo()
        {
            string sReportPath = AppDomain.CurrentDomain.BaseDirectory + "//printxml//CompanyInfo.xml";
            XmlDocument xmlDocReport = new XmlDocument();
            xmlDocReport.Load(sReportPath);

            string strWhere = "";
            if (ReportTemplateHelper.ExcelModeType != 2)
                strWhere = "/Data/template[@Name = '" + ReportTemplateHelper.ExcelModeType + "']";
            else
                strWhere = "/Data/template[@Name = '" + ReportTemplateHelper.LoadReportName() + "']";


            xmlnodelist = xmlDocReport.SelectNodes(strWhere);
        }

        #endregion


        private static void UpSpectrumData()
        {
            Lephone.Data.DbContext ConnectionString = Lephone.Data.DbEntry.Context;

            string sSQL="select speclist.*,workcurve.Name as workcurveName,device.Name as deviceName  from speclist "+ 
                        " left outer join workcurve on workcurve.id=speclist.WorkCurveId   "+ 
                        " left outer join  condition on condition.Id=speclist.condition_id   "+  
                        " left outer join  device on  device.Id=condition.Device_Id";

            DataTable dt = GetData(sSQL, ConnectionString.Driver.ConnectionString);

        
            Dictionary<long, SpecListEntity> dSpecListEntity = new Dictionary<long, SpecListEntity>();
            foreach (DataRow row in dt.Rows)
            {
                SpecListEntity splistentity = new SpecListEntity();
                splistentity.ActualCurrent = double.Parse(row["ActualCurrent"].ToString());
                splistentity.ActualVoltage = double.Parse(row["ActualVoltage"].ToString());
                splistentity.Color = int.Parse(row["Color"].ToString());
                splistentity.CountRate = double.Parse(row["CountRate"].ToString());

                List<DemarcateEnergy> demarcateEnergy = DemarcateEnergy.FindBySql("select * from demarcateenergy where condition_id=" + row["condition_id"].ToString());
                splistentity.DemarcateEnergys = Default.ConvertFormOldToNew(demarcateEnergy,SpecLength.Normal);
                splistentity.DeviceName = row["deviceName"].ToString();
                splistentity.ImageShow = bool.Parse(row["ImageShow"].ToString());
                splistentity.Loss = double.Parse(row["Loss"].ToString());
                splistentity.Name = row["Name"].ToString();
                splistentity.NameType = int.Parse(row["NameType"].ToString());
                splistentity.Operater = row["Operater"].ToString();
                splistentity.PeakChannel = double.Parse(row["PeakChannel"].ToString());
                splistentity.Resole = double.Parse(row["Resole"].ToString());
                splistentity.SampleName = row["SampleName"].ToString();
                splistentity.Shape = row["Shape"].ToString();
                splistentity.SpecDate = (row["SpecDate"].ToString()=="")?DateTime.Now: DateTime.Parse(row["SpecDate"].ToString());
                splistentity.SpecSummary = row["SpecSummary"].ToString();
                splistentity.SpecType = (SpecType)int.Parse(row["SpecType"].ToString()) ;
                splistentity.Supplier = row["Supplier"].ToString();
                //splistentity.TotalCount = int.Parse(row["TotalCount"].ToString());
                splistentity.TotalCount = long.Parse(row["TotalCount"].ToString());
                splistentity.VirtualColor = int.Parse(row["VirtualColor"].ToString());
                splistentity.Weight = (row["Weight"].ToString()=="")?0:double.Parse(row["Weight"].ToString());
                splistentity.WorkCurveName = (row["workcurveName"] == null) ? "" : row["workcurveName"].ToString();

                dSpecListEntity.Add(long.Parse(row["Id"].ToString()), splistentity);
            }

            sSQL = "select spec.*,deviceparameter.Name as deviceparameterName, deviceparameter.PrecTime, deviceparameter.TubCurrent, deviceparameter.TubVoltage, deviceparameter.FilterIdx, deviceparameter.CollimatorIdx, deviceparameter.TargetIdx, deviceparameter.IsVacuum, deviceparameter.VacuumTime, deviceparameter.IsVacuumDegree, deviceparameter.VacuumDegree, deviceparameter.IsAdjustRate, deviceparameter.MinRate, deviceparameter.MaxRate, deviceparameter.BeginChann, deviceparameter.EndChann, deviceparameter.IsDistrubAlert, deviceparameter.IsPeakFloat, deviceparameter.PeakFloatLeft, deviceparameter.PeakFloatRight, deviceparameter.PeakFloatChannel, deviceparameter.PeakFloatError, deviceparameter.PeakCheckTime, deviceparameter.TargetMode, deviceparameter.CurrentRate from   spec  " +
                                        " left outer join deviceparameter   on deviceparameter.Id=spec.deviceparameter_id where spec.deviceparameter_id <> 0";
            DataTable dt_Spec = GetData(sSQL, ConnectionString.Driver.ConnectionString);

            foreach (DataRow row in dt_Spec.Rows)
            {
                SpecEntity spec = new SpecEntity();
                spec.DeviceParameter = new DeviceParameterEntity(row["deviceparameterName"].ToString(),
                    int.Parse(row["PrecTime"].ToString() == string.Empty ? "100" : row["PrecTime"].ToString()),
                    int.Parse(row["TubCurrent"].ToString() == string.Empty ? "100" : row["TubCurrent"].ToString()),
                    int.Parse(row["TubVoltage"].ToString() == string.Empty ? "45" : row["TubVoltage"].ToString()),
                    int.Parse(row["FilterIdx"].ToString() == string.Empty ? "1" : row["FilterIdx"].ToString()),
                    int.Parse(row["CollimatorIdx"].ToString() == string.Empty ? "1" : row["CollimatorIdx"].ToString()),
                    int.Parse(row["TargetIdx"].ToString() == string.Empty ? "1" : row["TargetIdx"].ToString()),
                    bool.Parse(row["IsVacuum"].ToString() == string.Empty ? "false" : row["IsVacuum"].ToString()),
                    int.Parse(row["VacuumTime"].ToString() == string.Empty ? "0" : row["VacuumTime"].ToString()),
                    bool.Parse(row["IsVacuumDegree"].ToString() == string.Empty ? "false" : row["IsVacuumDegree"].ToString()),
                    double.Parse(row["VacuumDegree"].ToString() == string.Empty ? "0" : row["VacuumDegree"].ToString()),
                    bool.Parse(row["IsAdjustRate"].ToString() == string.Empty ? "false" : row["IsAdjustRate"].ToString()),
                    double.Parse(row["MinRate"].ToString() == string.Empty ? "0" : row["MinRate"].ToString()),
                    double.Parse(row["MaxRate"].ToString() == string.Empty ? "0" : row["MaxRate"].ToString()),
                    int.Parse(row["BeginChann"].ToString() == string.Empty ? "50" : row["BeginChann"].ToString()),
                    int.Parse(row["EndChann"].ToString() == string.Empty ? "2000" : row["EndChann"].ToString()),
                    bool.Parse(row["IsDistrubAlert"].ToString() == string.Empty ? "false" : row["IsDistrubAlert"].ToString()),
                    bool.Parse(row["IsPeakFloat"].ToString() == string.Empty ? "false" : row["IsPeakFloat"].ToString()),
                    int.Parse(row["PeakFloatLeft"].ToString() == string.Empty ? "0" : row["PeakFloatLeft"].ToString()),
                    int.Parse(row["PeakFloatRight"].ToString() == string.Empty ? "0" : row["PeakFloatRight"].ToString()),
                    int.Parse(row["PeakFloatChannel"].ToString()==string.Empty?"0":row["PeakFloatChannel"].ToString()),
                    int.Parse(row["PeakFloatError"].ToString()==string.Empty?"0":row["PeakFloatError"].ToString()),
                    int.Parse(row["PeakCheckTime"].ToString() == string.Empty ? "0" : row["PeakCheckTime"].ToString()),
                    ((row["TargetMode"].ToString() == "0" || row["TargetMode"].ToString()==string.Empty) ? TargetMode.OneTarget : TargetMode.TwoTarget),
                    int.Parse(row["CurrentRate"].ToString()));
                spec.IsSmooth = bool.Parse(row["IsSmooth"].ToString() == string.Empty ? "false" : row["IsSmooth"].ToString());
                spec.Name = row["Name"].ToString();
                spec.RemarkInfo = row["RemarkInfo"].ToString();
                spec.SpecData = row["SpecData"].ToString();
                //spec.SpecDatas = specs.SpecDatas;
                spec.SpecTime = double.Parse(row["SpecTime"].ToString() == string.Empty ? "100" : row["SpecTime"].ToString());
                spec.TubCurrent = int.Parse(row["TubCurrent"].ToString() == string.Empty ? "100" : row["TubCurrent"].ToString());
                spec.TubVoltage = int.Parse(row["TubVoltage"].ToString() == string.Empty ? "45" : row["TubVoltage"].ToString());
                spec.UsedTime = double.Parse(row["UsedTime"].ToString() == string.Empty ? "100" : row["TubVoltage"].ToString());
                //SpecEntityList.Add(spec);

                SpecListEntity specListEntity = null;
                dSpecListEntity.TryGetValue(long.Parse(row["speclist_Id"].ToString()), out specListEntity);
                if (specListEntity != null)
                {
                    if (specListEntity.Specs != null)
                    {
                        List<SpecEntity> lSpecEntity = specListEntity.Specs.ToList();
                        lSpecEntity.Add(spec);

                        specListEntity.Specs = lSpecEntity.ToArray();
                    }
                    else
                    {
                        specListEntity.Specs = new SpecEntity[1];
                        specListEntity.Specs[0] = spec;
                    }
                }

            }

            foreach (long keys in dSpecListEntity.Keys)
            {

                SpecListEntity model = dSpecListEntity[keys];
                byte[] obj = SerializeHelper.SerializeObj(model);
                //sql = "insert into spectrumdata(name,devicename,workcurvename,nametype,spectypevalue,samplename,specdate,data) " +
                //    " Values('" + model.Name + "','" + model.DeviceName + "','" + model.WorkCurveName + "','" + model.NameType + "','" + model.SpecType + "','" + model.SampleName + "','" + model.SpecDate + "','" + obj + "'); ";

                InsertSpectrumdata(model, obj);

            }

        }


        public static void InsertSpectrumdata(SpecListEntity model, byte[] obj)
        {
            string sql = string.Empty;
            //保存当前谱数据
            using (SQLiteConnection cnn = new SQLiteConnection(Lephone.Data.DbEntry.Context.Driver.ConnectionString))
            {
                cnn.Open();
                using (SQLiteCommand cmd = cnn.CreateCommand())
                {

                    cmd.CommandText = "insert into spectrumdata(name,devicename,workcurvename,nametype,spectypevalue,samplename,specdate,data) " +
                    " Values(@Name,@DeviceName,@WorkCurveName,@NameType,@SpecType,@SampleName,@SpecDate,@obj)";
                    SQLiteParameter paraName = new SQLiteParameter("@Name", DbType.String);
                    SQLiteParameter paraDeviceName = new SQLiteParameter("@DeviceName", DbType.String);
                    SQLiteParameter paraWorkCurveName = new SQLiteParameter("@WorkCurveName", DbType.String);
                    SQLiteParameter paraNameType = new SQLiteParameter("@NameType", DbType.Int32);
                    SQLiteParameter paraSpecType = new SQLiteParameter("@SpecType", DbType.Int32);
                    SQLiteParameter paraSampleName = new SQLiteParameter("@SampleName", DbType.String);
                    SQLiteParameter paraSpecDate = new SQLiteParameter("@SpecDate", DbType.DateTime);
                    SQLiteParameter paraobj = new SQLiteParameter("@obj", DbType.Binary);

                    paraName.Value = model.Name;
                    paraDeviceName.Value = model.DeviceName;
                    paraWorkCurveName.Value = model.WorkCurveName;
                    paraNameType.Value = model.NameType;
                    paraSpecType.Value = model.SpecType;
                    paraSampleName.Value = model.SampleName;
                    paraSpecDate.Value = model.SpecDate;
                    paraobj.Value = obj;

                    cmd.Parameters.Add(paraName);
                    cmd.Parameters.Add(paraDeviceName);
                    cmd.Parameters.Add(paraWorkCurveName);
                    cmd.Parameters.Add(paraNameType);
                    cmd.Parameters.Add(paraSpecType);
                    cmd.Parameters.Add(paraSampleName);
                    cmd.Parameters.Add(paraSpecDate);
                    cmd.Parameters.Add(paraobj);
                    cmd.ExecuteNonQuery();
                }
            }

        }

        /// <summary>
        /// 获取表中的字段信息
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="strinfo"></param>
        /// <returns></returns>
        private static string GetColmun(string tablename, string strinfo)
        {
            string strRetColmun = "";
            Regex reg = new Regex(@"(?<=\[)[^\[\]]+(?=\])");
            MatchCollection mc = reg.Matches(strinfo);
            foreach (Match m in mc)
            {
                if (m.Value.ToLower() != tablename.ToLower())
                    strRetColmun += m.Value + ",";
            }
            if (strRetColmun != "") strRetColmun = strRetColmun.Substring(0, strRetColmun.Length - 1);
            return strRetColmun;
        }


        private static void SetElementAllName(Lephone.Data.DbContext ConnectionString)
        {
            string strsql = "select count(*) from atom where atomNameen=''";
            DataTable dt = GetData(strsql, ConnectionString.Driver.ConnectionString);
            if (int.Parse(dt.Rows[0][0].ToString()) > 0)
            {
                strsql = "update atom set atomNameen=case when atomname='H' then 'Hydrogen'when atomname='He' then 'Helium'when atomname='Li' then 'Lithium'when atomname='Be' then 'Beryllium'when atomname='B' then 'Boron'when atomname='C' then 'Carbon'when atomname='N' then 'Nitrogen'when atomname='O' then 'Oxygen'when atomname='F' then 'Flourine'when atomname='Ne' then 'Neon'when atomname='Na' then 'Sodium'when atomname='Mg' then 'Magnesium'when atomname='Al' then 'Aluminum'when atomname='Si' then 'Silicon'when atomname='P' then 'Phosphorous'when atomname='S' then 'Sulfur'when atomname='Cl' then 'Chlorine'when atomname='Ar' then 'Argon'when atomname='K' then 'Potassium'when atomname='Ca' then 'Calcium'when atomname='Sc' then 'Scandium'when atomname='Ti' then 'Titanium'when atomname='V' then 'Vandium'when atomname='Cr' then 'Chromium'when atomname='Mn' then 'Manganese'when atomname='Fe' then 'Iron'when atomname='Co' then 'Cobalt'when atomname='Ni' then 'Nickel'when atomname='Cu' then 'Copper'when atomname='Zn' then 'Zinc'  " +
                         " when atomname='Ga' then 'Gallium'when atomname='Ge' then 'Germanium'when atomname='As' then 'Arsenic'when atomname='Se' then 'Selenium'when atomname='Br' then 'Bromine'when atomname='Kr' then 'Krypton'when atomname='Rb' then 'Rubidium'when atomname='Sr' then 'Strontium'when atomname='Y' then 'Yttrium'when atomname='Zr' then 'Zirconium'when atomname='Nb' then 'Niobium'when atomname='Mo' then 'Molybdenum'when atomname='Tc' then 'Technetium'when atomname='Ru' then 'Ruthenium'when atomname='Rh' then 'Rhodium'when atomname='Pd' then 'Palladium'when atomname='Ag' then 'Silver'when atomname='Cd' then 'Cadmium'when atomname='In' then 'Indium'when atomname='Sn' then 'Tin'when atomname='Sb' then 'Antimony'when atomname='Te' then 'Tellurium'when atomname='I' then 'Iodine'when atomname='Xe' then 'Xenon'when atomname='Cs' then 'Cesium'when atomname='Ba' then 'Barium'when atomname='La' then 'Lanthanum'when atomname='Ce' then 'Cerium'when atomname='Pr' then 'Praseodymium'when atomname='Nd' then 'Neodymium'  " +
                         " when atomname='Pm' then 'Promethium'when atomname='Sm' then 'Samarium'when atomname='Eu' then 'Europium'when atomname='Gd' then 'Gadolinium'when atomname='Tb' then 'Terbium'when atomname='Dy' then 'Dysprosium'when atomname='Ho' then 'Holmium'when atomname='Er' then 'Erbium'when atomname='Tm' then 'Thulium'when atomname='Yb' then 'Ytterbium'when atomname='Lu' then 'Lutetium'when atomname='Hf' then 'Hafnium'when atomname='Ta' then 'Tantalum'when atomname='W' then 'Tungsten'when atomname='Re' then 'Rhenium'when atomname='Os' then 'Osmium'when atomname='Ir' then 'Iridium'when atomname='Pt' then 'Platinum'when atomname='Au' then 'Gold'when atomname='Hg' then 'Mercury'when atomname='Tl' then 'Thallium'when atomname='Pb' then 'Lead'when atomname='Bi' then 'Bismuth'when atomname='Po' then 'Polonium'when atomname='At' then 'Astatine'when atomname='Rn' then 'Radon'when atomname='Fr' then 'Francium'when atomname='Ra' then 'Radium'when atomname='Ac' then 'Actinium'when atomname='Th' then 'Thorium'  " +
                         " when atomname='Pa' then 'Proactinium'when atomname='U' then 'Uranium'when atomname='y' then 'Neptunium' end";

                dt = GetData(strsql, ConnectionString.Driver.ConnectionString);
            }

        }

        public static SpecListEntity QueryByEdition(string name,string path,TotalEditionType editionType)
        {
            if (string.IsNullOrEmpty(name))
                return null;
            List<SpecListEntity> entityes = new List<SpecListEntity>();
            SpecListEntity etity = new SpecListEntity();
            if (string.IsNullOrEmpty(path))
            {
                etity = WorkCurveHelper.DataAccess.Query(name);
                return etity;
            }
            else
            {
                FileInfo info = new FileInfo(path);
                if (info.Exists)
                {
                    TotalEditionType type = editionType;
                    IFactory factory = null;
                    if (type == TotalEditionType.EDXRF)
                        factory = new EDXRFImplement();
                    else if (type == TotalEditionType.ROHS4)
                        factory = new ROHSImplement();
                    else if (type == TotalEditionType.ROHS3)
                        factory = new RoHS3Implement();
                    else if (type == TotalEditionType.FPThick)
                        factory = new FPThickImplement();
                    else if (type == TotalEditionType.Thick800A)
                        factory = new Thick800AImplement();
                    else if (type == TotalEditionType.XFP2)
                        factory = new XRFImplement();
                    else if (type == TotalEditionType.XRF)
                        factory = new XRFDelphiImp();
                    if (factory != null)
                    {
                        SpecListEntity temp = factory.LoadSpecFactory(info.FullName);
                        return temp;
                    }
                }
            }
            return null;
        }


    }
}
