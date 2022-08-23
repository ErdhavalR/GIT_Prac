using System;
using System.IO;
//using Aarohi_Modbus;
using System.Windows.Forms;
using DataBaseClass;
//using Aarohi_Devices;
using System.Drawing;
namespace App_Globals               
{
	//==========================Class Defination===========================//
	/*!
	*  Application Gloable Variable and general setting Variable  
	*/
	class Globals
	{
		public static string  SoftwareNameForDefaultJsonFile	= "AutoTesterV2";
		public static string  SoftwareVersionNo					= "V0.0.220";
		public static string  SoftwareTitle						= "AutoTesterV2 " + SoftwareVersionNo.Trim() + " - Powered By Aarohi Embedded Systems Pvt.Ltd.";//display software version or form name 
		//public static modbus  ModObj							= new modbus(); //mod object
		public static DBClass DbObj								= new DBClass();    //Database Object
		//public static DeviceSettings DeviceSettingsObj			= new DeviceSettings();/// DeviceSettingsObj object create
		public static string AppPath							= Path.GetDirectoryName(Application.ExecutablePath);//application path
		public static string GroupId							= "0";
		public static string CategoryNameId						= "0";
		public static string GroupName  						= "0";
		public static string CategoryName					    = "0";
		public static string Phase					            = "0";
		public static string HV_KV					            = "0";
		public static string ModelName					        = "0";
		public static string SerialNumberInputType				= "Normal";
		public static int    MultipaleTable						= 0;
		public static bool   RegisteredSuccessfullyFlag			= false;
		public static bool   isMainRead							= false;
		public static short  ReportGenerateIndex				= 0;//Export Report  index 1=pdf,2=excle,3=csv
		public static string TotalLogs							= "0";///Total logs display 
		public static int	 SelectTestOldOrNew					= 0;///1=New test,2=oldtest
		public static string SelectedTestSrNO					= "0";
		public static string SelectedProfile					= "0";
		public static string SelectedModelForReport				= "0";
		public static string PDFFileName						= "0";
		public static bool[] ViewParameter						= new bool[30];       
        public static string DBServerName                       = "localhost";
        public static string DBName                             = "autotest";
        public static string DBPort                             = "3306";
        public static string DBUserName                         = "root";
        public static string DBPassword                         = "";
        public static DateTime StartDate                        = new DateTime();       
        public static DateTime EndDate                          = new DateTime();
        public static string TestResultForReport                = "0";
        public static int ModelIndex                            = 0;
        public static int TotalModels                           = 0;
        public static string[] ModelIdSave                      = new string[50000];
        public static string ReportTitle                        = "";
        public static bool IsThemeSelection                     = false;
        public static string UserName                           = "";
        public static bool isPasswordForgot                     = false;
        public static bool isRegisteredSuccessfully             = false;
        public static bool isLogin                              = false;
        public static int LoginUserId                           = 0;
        public static int TimerCounter                          = 0;
        public static bool isCloaseLoadingIndicator             = false;
        public static bool isAutoTest                           = false;
        public static bool isExcelReportProgressbar             = false;
        public static bool isPDFReportProgressbar               = false;
        public static bool isDatabaseCreationProgressbar = false;
        public static bool isReportSelection                    = false;
        public static bool[] isReport                           = new bool[4];
        public static bool isProgressbarClose                   = false;
        public static bool isRoutineCheckDeviceStatues          = false;
        public static bool isRapidCheckDeviceStatues            = false;
        public static bool isRoutineAllTestComplate             = false;
        public static bool isSessionExpired                     = false;
        public static string RemarkAferTestFinish               = "";
        public static string RejectionEntries                   = "";
        public static string[] RapidTestFaliList                = new string[9];
		public static string YesterdayDate					    = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
		public static string TodayDate = "";
		public static string ReportPath = "";
		// live message for all test
		public static string[] MsgResistanceTest                = { "Resistance Test Load", "RY Test Start", "RY Test is Running", "RY Test Finished", "YB Test Start", "YB Test is Running", "YB Test Finished", "BR Test Start", "BR Test is Running", "BR Test Finished" };
        public static string[] MsgLCTTest                       = { "LCT Test Load", "LCT Test Start", "LCT Test is Running", "LCT Test Finished", "Passed" };
        public static string[] MsgHVTest                        = { "HV Test Load", "HV Test Start", "HV Test is Running", "HV Test Finished", "Passed" };
        public static string[] MsgIRTest                        = { "IR Test Load", "IR Test Start", "IR Test is Running", "IR Test Finished", "IR Test Passed" };
        public static string[] MsgNoLoadTest                    = { "NoLoad Test Load", "NoLoad Test Start", "NoLoad Test is Running", "NoLoad Test Finished", "Passed" };
        public static string[] MsgStartingVoltageTest           = { "Starting Voltage Test Load", "Starting Voltage Test Start", "Starting Voltage Test Running", "Starting Voltage Test Finished", "Passed " };
        public static string[] MsgLRTest                        = { "Lock Rotor Test Load", "Lock Rotor Test Start", "Lock Rotor Test is Running ", "Lock Rotor Test Finished", "Passed" };
        public static string[] MsgMechanicalTest                = { "Mechanical Test Load", "Mechanical Test Start", "Mechanical Test is Running", "Mechanical Test Finished", "Passed" };
        public static string[] MsgReduceVoltageTest             = { "Reduce Voltage Test Load", "Reduce Voltage Test Start", "Reduce Voltage Test is Running", "Reduce Voltage Test Finished", "Passed" };
		public static double TotalTest = 0;
		public static double MasterWindowGroupId = 0;
		public static double MasterWindowCategoryId = 0;
        public static int TotalPassTest;
        public static int TotalFailTest;
        public static int TotalNoOfTest;
        public static int TestCounter;
        public static bool isDataStoreEnable = false;
        public static bool isRejectionEntryLocked = false;
        //public static string BarcodeModel_Name = "";
        public struct CustomMsgBox
        {
            public static bool Yes = false;
        }
        /// <summary>
        /// Application path given
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public static string GetExePath(string FileName)//get file path using this methode
		{
			return AppPath.Trim() + "\\" + FileName.Trim();
		}
		/// <summary>
		/// Selecte Report Style
		/// </summary>
		public enum ReportStyle
		{
			PDF		= 1,
			EXCEL	= 2,
			CSV		= 3
		}

        public enum ModeType
        {
            ROUTINE_AUTO=0,
            ROUTINE_MANUAL,
            AUTOCAP_AUTO,
            AUTOCAP_MANUAL,
            RAPID_AUTO,
            RAPID_MANUAL,            
            STATOR_AUTO,
            STATOR_MANUAL           
        }
        
        /// <summary>
		/// general Setting
		/// </summary>
		public struct CapacitorSetup
        {
            public static bool[]    RunningCapacitorChk                 = new bool[30];
            public static bool[]    StartingCapacitorChk                = new bool[30];
            public static string[]  StartingCapacitorTxtValue           = new string[30];

            public static string[]  RunningCapacitorTxtValue            = new string[30];
            public static string[]  Run_Cap_CombValue_With_Duplicate    = new string[500];
            public static int[]     Run_Cap_DispValue_With_Duplicate    = new int[500];
            public static int[]     Run_Cap_OutputOn;
            public static int       Run_Cap_Index = 0;
        }
        public struct VoltageIOSetup_RapidMode
        {
            public static bool[] VoltageIOChk           = new bool[30];
            public static string[] VoltageIOTxtValue    = new string[30];
            public static int  RatedVoltage_OutputOn;
            public static int StartingVoltage_OutputOn;
            public static int LRVoltage_OutputOn;
            public static int ReduceVoltage_OutputOn;
            public static int NoOfOutPuts=19;
        }
        /// <summary>
        /// general Setting
        /// </summary>
        public struct generalSetting
		{
			public static string LogTime				= "1";
			public static bool	 ShowallTestSrNoFlag	= true;
			public static string TestSrNo				= "YES";
			public static string ReportPath				= "0";
			public static string ProfileTestTime		= "0";
			public static string GaugeDistance			= "0";
			public static string ReportFlowUnit			= "0";
			public static string LivetFlowUnit			= "0";
			public static string[] TestSelection        = new string[11];
			public static int    NoOftestPerform        = 0;
            public static bool isSelectTestTimeinMaster = false;
		}
		public struct ResistanceTestPhase
		{
			public static bool isRYPhase = false;
			public static bool isYBPhase = false;
			public static bool isBRPhase = false;
		}
		/// <summary>
		/// general Setting
		/// </summary>
		public struct TestSettings
		{
			#region #region//----------------------------------------------------- GeneralesettingsPara-----------------------------------------------------//
			public static string PumpType = "";
			public static bool isShowTestPassOrFail				= false;
			public static bool isShowCompulsoryMessages			= false;
			public static bool isAutoSaveTestResult				= false;
			public static bool isAttachedSAPServer				= false;
			public static bool isAttachedMSSQLServer			= false;
			public static bool isNextTestContinueAfterFailTest	= false;
			public static bool isEnableBarcodeScanner			= false;
			public static bool isExportImportMasterdataExcel	= false;
			public static bool isLineIdSelectionOption			= false;
			public static bool isUpdateExistingRecord			= false;
			public static bool isCustomRemark					= false;
			public static bool isPTStatus_inReport				= false;
			public static bool isRTStatus_inReport				= false;
			public static bool isShowPumpSetNumberforResultSave = false;
			public static bool isAutoGeneratedPumpsetSrNo		= false;
			public static bool isResultType             		= false;
			public static bool isRejectionEntry                 = false;
			public static bool isVerify_IOBoard					= false;
			public static bool isNLandLCTSeparate			    = false;

			#endregion
			#region #region//----------------------------------------------------- Voltage Test Settings -----------------------------------------------------//
			public static bool TorqueTestLog						        = false;
			public static bool RPMTestLog = false;
            public static string LRTCycleCount_RapidTest			        = "0";
			public static string ArmLength_mtr_forTorqueMeter		        = "0";
			public static string Phase3tartupVoltage				        = "0";
			public static string Phase1StartupVoltage				        = "0";
			public static bool isDisplayFillWaterMessageBeforeLRTest        = false;
			public static bool isDisplayFillWaterMessageAfterLRTest         = false;		
			public static bool isAddRPMDirection					        = false;
			public static bool isContinousVoltageTest				        = false;
			public static bool isStartTestfromStartupVoltage		        = false;
			public static bool isTestCurrentimbalanceforAllVoltageTests		= false;
			public static bool isApplyRunningStartingCapacitorsDuringLRTest = false;
			public static bool istoggHVkVTestCheck                          = false;
			#endregion
			#region #region//----------------------------------------------------- HVT,IR&Resistance Test Settings -----------------------------------------------------//
		//	public static string SerialNumberInputType  			= "0"; 
            public static string ResistanceLimitTest	= "0";
			public static bool isMeggaTestValue			= false;
			public static bool isHVTestValue			= false;
			#endregion
			#region #region//----------------------------------------------------- Panel Devices Settings -----------------------------------------------------//
			public static string RPMDeviceType				= "0";
			public static string MeggarDeviceType			= "0";
			public static string ResistanceShowOverValue	= "0";
			public static string DimmerTimerMultiplier		= "0";
			public static string VoltageVariationDelta		= "0";
			public static string CurrentVariationDelta		= "0";
			public static string DriveNoTrials				= "0";
			public static string AverageSample				= "0";
			public static bool[] ModeSelection				= new bool[10];
            public static string TotalRevolutipon           = "0";
            public static string MaxVolt                    = "0";
            public static string PanelID                    = "0";
			public static string BacodeScannerType			= "";
            #endregion
            public static bool LRTestettings = false;
            // Devices communiction for Check Device Statues from.
            public static bool isPowerCommunication = false;
            public static bool isREFCommunication = false;
            public static bool isHVCommunication = false;
            public static bool isMaggerCommunication = false;
            public static bool isResistCommunication = false;
            public static bool isCommunication = false;
            public static bool isLCTCommunication = false;
            public static bool isTorqueCommunication = false;
            public static bool isAmperereCommunication = false;
            public static bool isIOBCommunication = false;
            public static bool isDiractionCommunication = false;     
        }
        public struct UserManagement 
        {
            // For Only One User's Fetch
            public static bool[] isFirstUser                   = new bool[100];
            /// <summary>
            /// For All Data Fatch
            /// </summary>
            public static bool[] isPerformTestAllow             = new bool[100];
            public static bool[] isRoutineAuto                  = new bool[100];
            public static bool[] isRoutineManual                = new bool[100];
            public static bool[] isStatorAuto                   = new bool[100];
            public static bool[] isStatorManual                 = new bool[100];
            public static bool[] isAutoCapAuto                  = new bool[100];
            public static bool[] isAutoCapManual                = new bool[100];
            public static bool[] isRapidManual                  = new bool[100];
            public static bool[] isRapidAuto                    = new bool[100];
            public static bool[] isMasterParamatersAllow        = new bool[100];
            public static bool[] isMasterDelete                 = new bool[100];
            public static bool[] isMasterEdit                   = new bool[100];
            public static bool[] isStatorMaster                 = new bool[100];
            public static bool[] isSettings                     = new bool[100];
            public static bool[] isUserSettings                 = new bool[100];
            public static bool[] isDeviceSettings               = new bool[100];
            public static bool[] isTestSettings                 = new bool[100];
            public static bool[] isPanelSettings                = new bool[100];
            public static bool[] isTestSequence                 = new bool[100];
            public static bool[] isReportsAllow                 = new bool[100];
            public static bool[] isUserManagement               = new bool[100];
            //public static bool[] isLogin                        = new bool[100];
            public static bool[] isAddUser                      = new bool[100];
            public static bool[] isDeleteUser                   = new bool[100];
            public static bool[] isChangePassword               = new bool[100];
            public static bool[] isLoginUserSelection           = new bool[100];

            public static bool isDeviceSettingManagementsAllow  = false;
            public static bool isPanelSettingsManagementsAllow  = false;
            public static bool isAddnewUserManagementsAllow  = false;
            public static bool isDeleteUserManagementsAllow  = false;
        }
        /// <summary>
        /// Company  info
        /// </summary>
        public struct CompanyInfo
		{
			public static string CompanyName = "AAROHI EMBEDDED SYSTEMS PVT.LTD";
			public static string CompanyAddrerss = "Plot No. G-(1004-8)A, Kishan Gate No-3 Main road, Nr. Durga Weigh Bridge,GIDC Metoda, Village Lodhik, Kalawad RoadRajkot ,Gujarat(India.) - 360021.";
		}
		/// <summary>
		/// Set DatagridView Default settings
		/// </summary>
		public static void SetFontAndColorsDataGridView(DataGridView dataGridView1)
		{
			dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
			dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
			dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;            
            dataGridView1.AutoResizeColumns();
			// Configure the details DataGridView so that its columns automatically
			// adjust their widths when the data changes.
			dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
			dataGridView1.DefaultCellStyle.Font = new Font("Tahoma", 10);
			dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold);
			dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
			dataGridView1.DefaultCellStyle.BackColor = Color.White;
			dataGridView1.DefaultCellStyle.SelectionForeColor = Color.Black;
			dataGridView1.DefaultCellStyle.SelectionBackColor = Color.LightBlue;
			dataGridView1.ReadOnly = true;           
		}
		/// <summary>
		/// Stored master Data Dispaly Using This Structure Variables
		/// </summary>
		public struct SelectedModel
		{
			public static bool SelectModelFlag              = false;
            public static string TestResult                 = "PASS";
            public static string TestSrNo                   = "";
            public static string TestAutoSrNoIncrementString   = "";
            public static int    TestAutoSrNoIncrement      = 0;
            public static int   SrNoformat                  = 0;
            public static string ModelParaId                = "0";
            public static string TestTime                   = "00:00";
            public static string[] SaveResultInDB           = new string[50];
            public static string[] Model_Name               = new String[2] { "Model_Name ", " " };
			public static string[] Motor_Rating_Kw          = new String[2] { "Motor_Rating_Kw ", "0" };
			public static string[] Motor_Rating_HP          = new String[2] { "Motor_Rating_HP ", "0" };
			public static string[] Phase_Select             = new String[2] { "Phase_Select ", "0" };
			public static string[] Rated_Voltage            = new String[2] { "Rated_Voltage ", "0" };
			public static string[] Rated_Speed              = new String[2] { "Rated_Speed ", "0" };
			public static string[] Rated_Freq               = new String[2] { "Rated_Freq ", "0" };
			public static string[] NLIMin                   = new String[2] { "NLCurrentMin ", "0" };
			public static string[] NLIMax                   = new String[2] { "NLCurrentMax ", "0" };
			public static string[] NLPowerMin               = new String[2] { "NLPowerMin ", "0" };
			public static string[] NLPowerMax               = new String[2] { "NLPowerMax ", "0" };
			public static string[] LRVoltage                = new String[2] { "LockRotorVoltage ", "0" };
			public static string[] LRIMin                   = new String[2] { "LockRotorCurrentMin ", "0" };
			public static string[] LRIMax                   = new String[2] { "LockRotorCurrentMax ", "0" };
			public static string[] LRPowerMin               = new String[2] { "LockRotorPowerMin ", "0" };
			public static string[] LRPowerMax               = new String[2] { "LockRotorPowerMax ", "0" };
			public static string[] ResMainMin               = new String[2] { "ResistanceMainMin ", "0" };
			public static string[] ResMainMax               = new String[2] { "ResistanceMainMax ", "0" };
			public static string[] ResAuxMin                = new String[2] { "ResistanceAuxMin ", "0" };
			public static string[] ResAuxMax                = new String[2] { "ResistanceAuxMax ", "0" };
			public static string[] ResPhaseMin              = new String[2] { "ResistancePhaseMin ", "0" };
			public static string[] ResPhaseMax              = new String[2] { "ResistancePhaseMax ", "0" };
			public static string[] LCT                      = new String[2] { "LCT ", "0" };
			public static string[] StartingVoltage          = new String[2] { "Starting Voltage ", "0" };
			public static string[] HVCurrent                = new String[2] { "HV(mA)", "0" };
			public static string[] HVVoltage                = new String[2] { "HV(kV)", "0" };
			public static string[] MeggerValueMohm          = new String[2] { "Megger Value Mohm ", "0" };
			public static string[] Acceleration             = new String[2] { "Acceleration ", "0" };
			public static string[] Deacceleration           = new String[2] { "Deacceleration ", "0" };

			public static string[] ResCorrectedMin          = new String[2] { "ResistanceCorrectedMin ", "0" };
			public static string[] ResCorrectedMax          = new String[2] { "ResistanceCorrectedMax ", "0" };
			public static string[] ResCorrectedBRMin        = new String[2] { "ResistanceCorrectedBRMin ", "0" };
			public static string[] ResCorrectedBRMax        = new String[2] { "ResistanceCorrectedBRMax ", "0" };
			public static string[] MotorRotation            = new String[2] { "MotorRotation ", "0" };
			public static string[] Stage                    = new String[2] { "Stage ", "0" };
			public static string[] ISINoISI                 = new String[2] { "ISI_NOISI ", "0" };
			public static string[] StarRating               = new String[2] { "StarRating ", "0" };
			public static string[] ReduceVoltage            = new String[2] { "ReduceVoltage ", "0" };
			public static string[] HVTestTime               = new String[2] { "HV Test Time ", "0" };
			public static string[] MeggerTestTime           = new String[2] { "Megger Test Time ", "0" };
			public static string[] LRTestTime               = new String[2] { "Lock Rotor Test Time ", "0" };
			public static string[] LVTestTime               = new String[2] { "LV Test Time ", "0" };
			public static string[] NLTestTime               = new String[2] { "NL Test Time ", "0" };
			public static string[] ResTestTime              = new String[2] { "Resistance Test Time ", "0" };
			public static string[] LCTTestTime              = new String[2] { "LCT Test Time ", "0" };
			public static string[] MechanicalTestTime       = new String[2] { "Mechanical Test Time ", "0" };
			public static string[] RevFTestTime             = new String[2] { "Rev F Test Time ", "0" };
			public static string[] RevRTestTime             = new String[2] { "Rev R Test Time ", "0" };
			public static string[] CategoryId               = new String[2] { "CategoryId ", "0" };
            public static string[] Start_Capacitor          = new String[2] { "Start_Capacitor ", "0" };
            public static string[] Running_Capacitor        = new String[2] { "Running_Capacitor ", "0" };
            public static string[] Barcode_Code				= new String[2] { "Barcode ", "0" };
            public static int  Start_Capacitor_OutPut       = 1;
            public static bool[] AllTestResult              = new bool[12];
            public static string[] AllTestResultPassFail    = new string[12];
        }
	}
}
