using App_Globals;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

namespace DataBaseClass
{
    //============================= Class Defination ==================================
    /*!
	*  Database Query and connection  
	*/
    class DBClass
    {
        /* connection      : will be used to open a connection to the database.
		/* server          : indicates where our server is hosted, in our case, it's localhost.
		/* database        : is the name of the database we will use, in our case it's the database we already created earlier which is connectcsharptomysql.
		/* uid             : is our MySQL username.
		/* password        : is our MySQL password.
		/* connectionString: contains the connection string to connect to the database, and will be assigned to the connection variable.*/
        #region //----------------------------------------------------- Form Variable -----------------------------------------------------//
        public static string DBServerName = "";
        public static string DBName = "";
        public static string DBPort = "";
        public static string DBUserName = "";
        public static string DBPassword = "";
        public static string ConnectionString = "";
        public static string G_ConnectionString = "";
        #endregion
        #region //----------------------------------------------------- Constructor  Destructor -----------------------------------------------------//
        public DBClass()
        {
        }

        public DBClass(string ServerName, string dbName, string PortNumber, string UserName, string Password)
        {
            G_ConnectionString = "Server=" + ServerName.Trim() + ";Database=" + dbName.Trim() + ";port=" + PortNumber.Trim() + ";username=" + UserName.Trim() + ";password=" + Password.Trim();
        }

        ~DBClass()
        {
        }
        #endregion
        #region //-----------------------------------------------------GetConnection -----------------------------------------------------//
        private void GetConnection()
        {
            DBServerName = Globals.DBServerName.Trim();
            DBName = Globals.DBName.Trim();
            DBPort = Globals.DBPort.Trim();
            DBUserName = Globals.DBUserName.Trim();
            DBPassword = Globals.DBPassword.Trim();
            ConnectionString = "Server=" + DBServerName + ";port=" + DBPort + ";username=" + DBUserName + ";password=" + DBPassword;
            G_ConnectionString = "Server=" + DBServerName + ";Database=" + DBName + ";port=" + DBPort + ";username=" + DBUserName + ";password=" + DBPassword;
        }
        #endregion
        #region //----------------------------------------------------- Store Image IN Database -----------------------------------------------------//
        public long StoreImageOnDatabase(string SQLQueery, string IMGFilePath)
        {
            long ID = 0;
            MySqlConnection con = new MySqlConnection(G_ConnectionString);
            MySqlCommand MyCommand;
            FileStream FileStr;
            BinaryReader BinaryRed;
            byte[] ImageData;
            FileStr = new FileStream(IMGFilePath, FileMode.Open, FileAccess.Read);
            BinaryRed = new BinaryReader(FileStr);
            ImageData = BinaryRed.ReadBytes((int)FileStr.Length);
            BinaryRed.Close();
            FileStr.Close();
            MyCommand = new MySqlCommand(SQLQueery, con);
            MyCommand.Parameters.Add("@CompanyLogo", MySqlDbType.LongBlob, ImageData.Length);
            MyCommand.Parameters["@CompanyLogo"].Value = ImageData;
            con.Open();
            int RowsAffected = MyCommand.ExecuteNonQuery();
            if (RowsAffected > 0)
            {
                MessageBox.Show("Image saved sucessfully!");
            }
            ID = MyCommand.LastInsertedId;
            con.Close();
            return ID;
        }
        public PictureBox FetchImageInDataBase(string SQLQueery)
        {
            GetConnection();
            MySqlConnection connection = new MySqlConnection(G_ConnectionString);
            connection.Open();
            MySqlCommand MyCommand2 = new MySqlCommand(SQLQueery, connection);
            MySqlDataAdapter MyAdapter = new MySqlDataAdapter(MyCommand2);
            DataSet Dataet = new DataSet();
            MyAdapter.Fill(Dataet);
            PictureBox picture = new PictureBox();
            if (Dataet.Tables[0].Rows.Count > 0)
            {
                MemoryStream MemoryStr = new MemoryStream((byte[])Dataet.Tables[0].Rows[0]["CompanyLogo"]);
                picture.Image = new Bitmap(MemoryStr);
            }
            connection.Close();
            return picture;
        }
        #endregion
        #region//-----------------------------------------------------Request2Response -----------------------------------------------------//
        public DataTable Request2Response(string SqlQuery)
        {
            GetConnection();
            string Query = SqlQuery.Trim();
            MySqlConnection MyConnection2 = new MySqlConnection(G_ConnectionString);
            MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConnection2);
            MySqlDataAdapter MyAdapter = new MySqlDataAdapter();
            MyAdapter.SelectCommand = MyCommand2;
            DataTable dTable = new DataTable();
            MyAdapter.Fill(dTable);
            //dataGridView1.DataSource = dTable;
            MyConnection2.Close();
            return dTable;
        }
        public long Request2Insert(string SqlQuery)
        {
            GetConnection();
            string Query = SqlQuery.Trim();
            long Id = 0;
            MySqlConnection MyConnection2 = new MySqlConnection(G_ConnectionString);
            MyConnection2.Open();
            MySqlCommand MyCommand2 = MyConnection2.CreateCommand();
            MyCommand2.CommandText = Query;
            MyCommand2.ExecuteNonQuery();
            Id = MyCommand2.LastInsertedId;
            //dataGridView1.DataSource = dTable;
            MyConnection2.Close();
            return Id;
        }
        #endregion
        #region //----------------------------------------------------- DBBackupExport-----------------------------------------------------//
        public void DBBackupExport(string AppPath)
        {
            GetConnection();
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        cmd.Connection = conn;
                        conn.Open();
                        mb.ExportToFile(AppPath);//This line will export the file to given path.
                        conn.Close();
                        MessageBox.Show("Backup Completed...!!!");
                    }
                }
            }
        }
        #endregion
        #region //----------------------------------------------------- DBRestoreImport-----------------------------------------------------//
        public void DBRestoreImport(string AppPath)
        {
            GetConnection();
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    //cmd.ExecuteNonQuery();
                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        cmd.Connection = conn;
                        conn.Open();
                        mb.ImportFromFile(AppPath);//This line will export the file to given path.
                        conn.Close();
                        MessageBox.Show("DataBase Imported!!!");
                    }
                }
            }
        }
        #endregion
        #region //----------------------------------------------------- DataBaseCreate-----------------------------------------------------//
        public void DataBaseCreate()
        {
            GetConnection();
            string DBCreateQuery;
            DBCreateQuery = "CREATE DATABASE IF NOT EXISTS `" + DBName + "`;";
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            MySqlCommand cmd;
            try
            {
                conn.Open();
                cmd = new MySqlCommand(DBCreateQuery, conn);
                cmd.ExecuteNonQuery();
                CreateDBTable();
                Globals.isDatabaseCreationProgressbar = true;
                //MessageBox.Show("DataBase Creation successfuly !!!");
                conn.Close();               
            }
            catch (Exception e)
            {
                MessageBox.Show("DataBase Creation Error!!!");
            }
        }
        public bool CheckDataBase()
        {
            bool isDBNotExist = false;
            MySqlConnection MyConnection2 = new MySqlConnection(ConnectionString);
            MySqlCommand MyCommand2 = new MySqlCommand("SHOW DATABASES LIKE '" + DBName + "'", MyConnection2);
            MySqlDataAdapter MyAdapter = new MySqlDataAdapter();
            MyAdapter.SelectCommand = MyCommand2;
            DataTable dTable = new DataTable();
            MyAdapter.Fill(dTable);
            MyConnection2.Close();
            if (dTable.Rows.Count == 0)
            {
                //AESMessageBox("Data Base Not Exist Please Create Database!!!", "Login Error", false);
                isDBNotExist = true;
            }
            return isDBNotExist;
        }
        #endregion
        #region //-----------------------------------------------------ColumnAdd-----------------------------------------------------//
        private void ColumnAdd(string TableName, string ColumnName, string AfterColumnName, string DataType, string Length, string Default, MySqlCommand comm)
        {
            string CheckColumnQuery = "";
            string AddColumnQuery = "";
            string AddColumnDateTimeQuery = "";
            string AddColumnDoubleQuery = "";
            CheckColumnQuery = "SELECT * FROM INFORMATION_SCHEMA.`COLUMNS` WHERE TABLE_SCHEMA='" + DBName + "' AND TABLE_NAME ='" + TableName + "' AND COLUMN_NAME = '" + ColumnName + "'";
            AddColumnQuery = "ALTER TABLE " + TableName + " ADD COLUMN `" + ColumnName + "` " + DataType + "(" + Length + ") DEFAULT '" + Default + "' AFTER `" + AfterColumnName + "`;";
            AddColumnDateTimeQuery = "ALTER TABLE " + TableName + " ADD COLUMN `" + ColumnName + "` " + DataType + " AFTER `" + AfterColumnName + "`;";
            AddColumnDoubleQuery = "ALTER TABLE " + TableName + " ADD COLUMN `" + ColumnName + "` " + DataType + " DEFAULT '" + Default + "' AFTER `" + AfterColumnName + "`;";
            string AddImageQueert = "ALTER TABLE " + TableName + " ADD COLUMN `" + ColumnName + "` " + DataType + " AFTER `" + AfterColumnName + "`;";
            comm.CommandText = CheckColumnQuery;
            MySqlDataAdapter MyAdapter = new MySqlDataAdapter();
            MyAdapter.SelectCommand = comm;
            DataTable dTable = new DataTable();
            MyAdapter.Fill(dTable);

            string value = dTable.Rows.Count.ToString();
            if (value == "0")
            {
                if(DataType == "DATETIME" || DataType == "TIME" || DataType == "DATE")
                {
                    comm.CommandText = AddColumnDateTimeQuery;
                    comm.ExecuteNonQuery();
                }
                else
                {
                    if (DataType == "DOUBLE")
                    {
                        comm.CommandText = AddColumnDoubleQuery;
                        comm.ExecuteNonQuery();
                    }
                    else if (DataType == "longblob")
                    {
                        comm.CommandText = AddImageQueert;
                        comm.ExecuteNonQuery();
                    }
                    else if (DataType == "tinytext")
                    {
                        comm.CommandText = AddColumnDateTimeQuery;
                        comm.ExecuteNonQuery();
                    }
                    else
                    {
                        comm.CommandText = AddColumnQuery;
                        comm.ExecuteNonQuery();
                    }
                }
            }
            else
            {
                //App_Settings.GenerateLogFile.WriteLog("DataBase Createtion: Table:" + TableName + ",Column:" + ColumnName + "Already Exists");
            }
        }
        #endregion
        #region //-----------------------------------------------------TableValueAdd-----------------------------------------------------//
        private void TableValueAdd(string TableName, string ColumnName,string Value, MySqlCommand comm)
        {
            comm.CommandText = "INSERT INTO " + TableName + "(" + ColumnName + ") VALUES('" + Value + "')";
            comm.ExecuteNonQuery();
        } 
        #endregion
        #region //-----------------------------------------------------DBTableCreate-----------------------------------------------------//
        private void DBTableCreate(string TableName, string ColumnName, string DataType, string Length, MySqlCommand comm)
        {
            comm.CommandText = "CREATE TABLE IF NOT EXISTS "+ TableName +"("+ ColumnName + " " + DataType +"("+ Length +") UNSIGNED NOT NULL AUTO_INCREMENT, PRIMARY KEY(`"+ ColumnName + "`))";
            comm.ExecuteNonQuery();
        }
        #endregion
        #region//----------------------------------------------------- DataBaseCreate-----------------------------------------------------//
        private void CreateDBTable()
        {
            MySqlConnection con = new MySqlConnection(G_ConnectionString);
            con.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = con;
            capacitorTable("capacitor", cmd);
            livedata_tab("livedata_tab", cmd);
            exceltoexportsettingTable("exceltoexportsetting", cmd);
            loginTable("login", cmd);
            logs_tableTable("logs_table", cmd);
            RejectionTab("rejection_tab", cmd);
            model_categoryTable("model_category", cmd);
            model_groupTable("model_group", cmd);
            model_parametersTable("model_parameters", cmd);
            outputvoltagesetupTable("outputvoltagesetup", cmd);
            pumpcodenumberTable("pumpcodenumber", cmd);
            qrcodeTable("qrcode", cmd);
            runningcapacitorsTable("runningcapacitors", cmd);
            sapdataTable("sapdata", cmd);
            startingcapacitorsTable("startingcapacitors", cmd);
            testparameterTable("testparameter", cmd);
            CompanyInfo("Company_Information", cmd);
            UserManagement("UserManagement", cmd);
            UserSession("current_session",cmd);
            con.Close();
        }
        #endregion
        #region//-----------------------------------------------------capacitorTable-----------------------------------------------------//
        private void capacitorTable(string TableName, MySqlCommand comm)
        {
            try
            {
                DBTableCreate(TableName, "ID", "INT", "10", comm);
                ColumnAdd(TableName, "CapacitorValue", "Id", "VARCHAR", "50", "0", comm);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        #endregion     
        #region//-----------------------------------------------------livedata_tab-------------------------------------------//
        private void livedata_tab(string TableName, MySqlCommand comm)
        {
            try
            {
                DBTableCreate(TableName, "ID", "INT", "50", comm);
                ColumnAdd(TableName, "rphase_outvolt", "Id", "VARCHAR", "50", "0", comm);
                ColumnAdd(TableName, "yphase_outvolt", "rphase_outvolt", "VARCHAR", "50", "0", comm);
                ColumnAdd(TableName, "bphase_outvolt", "yphase_outvolt", "VARCHAR", "50", "0", comm);
                ColumnAdd(TableName, "rphase_involt", "bphase_outvolt", "VARCHAR", "50", "0", comm);
                ColumnAdd(TableName, "yphase_involt", "rphase_involt", "VARCHAR", "50", "0", comm);
                ColumnAdd(TableName, "bphase_involt", "yphase_involt", "VARCHAR", "50", "0", comm);
                ColumnAdd(TableName, "ir_current", "bphase_involt", "VARCHAR", "50", "0", comm);
                ColumnAdd(TableName, "iy_current", "ir_current", "VARCHAR", "50", "0", comm);
                ColumnAdd(TableName, "ib_current", "iy_current", "VARCHAR", "50", "0", comm);
                ColumnAdd(TableName, "ry_outvolt", "ib_current", "VARCHAR", "50", "0", comm);
                ColumnAdd(TableName, "yb_outvolt", "ry_outvolt", "VARCHAR", "50", "0", comm);
                ColumnAdd(TableName, "br_outvolt", "yb_outvolt", "VARCHAR", "50", "0", comm);
                ColumnAdd(TableName, "ry_involt", "br_outvolt", "VARCHAR", "50", "0", comm);
                ColumnAdd(TableName, "yb_involt", "ry_involt", "VARCHAR", "50", "0", comm);
                ColumnAdd(TableName, "br_involt", "yb_involt", "VARCHAR", "50", "0", comm);
            }
            catch (Exception e)
            {
                MessageBox.Show("device3_livedatatab error");
            }
        }
        #endregion
        #region//-----------------------------------------------------exceltoexportsettingTable-----------------------------------------------------//
        private void exceltoexportsettingTable(string TableName, MySqlCommand comm)
        {
            try
            {
                DBTableCreate(TableName, "ID", "INT", "10", comm);
                ColumnAdd(TableName, "OriginalColumnName", "Id", "VARCHAR", "256", "0", comm);
                ColumnAdd(TableName, "NewColumnName", "OriginalColumnName", "VARCHAR", "256", "0", comm);
                ColumnAdd(TableName, "ColumnOrder", "NewColumnName", "VARCHAR", "4", "0", comm);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        #endregion
        #region//-----------------------------------------------------loginTable-----------------------------------------------------//
        private void loginTable(string TableName, MySqlCommand comm)
        {
            try
            {
                DBTableCreate(TableName, "ID", "INT", "10", comm);
                ColumnAdd(TableName, "UserName", "Id", "VARCHAR", "48", "0", comm);
                ColumnAdd(TableName, "UserPass", "UserName", "VARCHAR", "128", "0", comm);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        #endregion
        #region//-----------------------------------------------------logs_Table-----------------------------------------------------//
        private void logs_tableTable(string TableName, MySqlCommand comm)
        {
            try
            {
                DBTableCreate(TableName, "ID", "INT", "10", comm);
                ColumnAdd(TableName, "Model_Para_ID", "Id", "INT", "11", "0", comm);
                ColumnAdd(TableName, "Test_Sr_No", "Model_Para_ID", "VARCHAR", "256", "0", comm);
                ColumnAdd(TableName, "PumpCodeNo", "Test_Sr_No", "VARCHAR", "256", "0", comm);
                ColumnAdd(TableName, "Test_Date", "PumpCodeNo", "DATETIME", "", "", comm);
                ColumnAdd(TableName, "Test_Result", "Test_Date", "VARCHAR", "256", "0", comm);
                ColumnAdd(TableName, "Megger_Test", "Test_Result", "VARCHAR", "256", "0", comm);
                ColumnAdd(TableName, "Megger_After_Test", "Megger_Test", "VARCHAR", "256", "0", comm);
                ColumnAdd(TableName, "IR_After_Result_Value", "Megger_After_Test", "VARCHAR", "256", "0", comm);
                ColumnAdd(TableName, "HV_Test", "IR_After_Result_Value", "VARCHAR", "256", "0", comm);
                ColumnAdd(TableName, "HV_Voltage", "HV_Test", "VARCHAR", "48", "0", comm);
                ColumnAdd(TableName, "IR_Result_Value", "HV_Voltage", "VARCHAR", "256", "0", comm);
                ColumnAdd(TableName, "NL_Voltage", "IR_Result_Value", "VARCHAR", "48", "0", comm);
                ColumnAdd(TableName, "NL_Current", "NL_Voltage", "VARCHAR", "48", "0", comm);
                ColumnAdd(TableName, "NL_Power", "NL_Current", "VARCHAR", "48", "0", comm);
                ColumnAdd(TableName, "NL_Freq", "NL_Power", "VARCHAR", "48", "0", comm);
                ColumnAdd(TableName, "NL_RPM", "NL_Freq", "VARCHAR", "48", "0", comm);
                ColumnAdd(TableName, "NL_PF", "NL_RPM", "VARCHAR", "48", "0", comm);
                ColumnAdd(TableName, "Primary_NL_Voltage", "NL_PF", "VARCHAR", "48", "0", comm);
                ColumnAdd(TableName, "Primary_NL_Current", "Primary_NL_Voltage", "VARCHAR", "48", "0", comm);
                ColumnAdd(TableName, "Primary_NL_Power", "Primary_NL_Current", "VARCHAR", "48", "0", comm);
                ColumnAdd(TableName, "Primary_NL_Freq", "Primary_NL_Power", "VARCHAR", "48", "0", comm);
                ColumnAdd(TableName, "Primary_NL_RPM", "Primary_NL_Freq", "VARCHAR", "48", "0", comm);
                ColumnAdd(TableName, "Primary_NL_PF", "Primary_NL_RPM", "VARCHAR", "48", "0", comm);
                ColumnAdd(TableName, "Starting_Voltage", "Primary_NL_PF", "VARCHAR", "48", "0", comm);
                ColumnAdd(TableName, "LV_PF", "Starting_Voltage", "VARCHAR", "48", "0", comm);
                ColumnAdd(TableName, "LR_Voltage", "LV_PF", "VARCHAR", "48", "0", comm);
                ColumnAdd(TableName, "LR_Current", "LR_Voltage", "VARCHAR", "48", "0", comm);
                ColumnAdd(TableName, "LR_Power", "LR_Current", "VARCHAR", "48", "0", comm);
                ColumnAdd(TableName, "LR_Freq", "LR_Power", "VARCHAR", "48", "0", comm);
                ColumnAdd(TableName, "LR_RPM", "LR_Freq", "VARCHAR", "48", "0", comm);
                ColumnAdd(TableName, "LR_PF", "LR_RPM", "VARCHAR", "48", "0", comm);
                ColumnAdd(TableName, "Coil_ResistanceY_Value", "LR_PF", "VARCHAR", "48", "0", comm);
                ColumnAdd(TableName, "Coil_ResistanceB_Value", "Coil_ResistanceY_Value", "VARCHAR", "48", "0", comm);
                ColumnAdd(TableName, "Coil_ResistanceR_value", "Coil_ResistanceB_Value", "VARCHAR", "48", "0", comm);
                ColumnAdd(TableName, "LCT_Result_Value", "Coil_ResistanceR_value", "VARCHAR", "48", "0", comm);
                ColumnAdd(TableName, "Edit_User", "LCT_Result_Value", "VARCHAR", "256", "0", comm);
                ColumnAdd(TableName, "Test_Time", "Edit_User", "TIME", "", "", comm);
                ColumnAdd(TableName, "IsStatorTest", "Test_Time", "TINYINT", "1", "0", comm);
                ColumnAdd(TableName, "Torque", "IsStatorTest", "VARCHAR", "24", "0", comm);
                ColumnAdd(TableName, "Remarks", "Torque", "VARCHAR", "1024", "0", comm);
                ColumnAdd(TableName, "Correctd_Resistance", "Remarks", "VARCHAR", "256", "0", comm);
                ColumnAdd(TableName, "Correctd_Resistance_B", "Correctd_Resistance", "VARCHAR", "256", "0", comm);
                ColumnAdd(TableName, "Start_Current", "Correctd_Resistance_B", "VARCHAR", "48", "0", comm);
                ColumnAdd(TableName, "Run_Current", "Start_Current", "VARCHAR", "48", "0", comm);
                ColumnAdd(TableName, "Shift", "Run_Current", "VARCHAR", "48", "shiftl", comm);
                ColumnAdd(TableName, "PTStatus", "Shift", "VARCHAR", "48", "0", comm);
                ColumnAdd(TableName, "RTStatus", "PTStatus", "VARCHAR", "48", "0", comm);
                ColumnAdd(TableName, "CurrentR", "RTStatus", "VARCHAR", "48", "0", comm);
                ColumnAdd(TableName, "CurrentY", "CurrentR", "VARCHAR", "48", "0", comm);
                ColumnAdd(TableName, "CurrentB", "CurrentY", "VARCHAR", "48", "0", comm);
                ColumnAdd(TableName, "Reversal_Forward_Freq", "CurrentB", "VARCHAR", "48", "0", comm);
                ColumnAdd(TableName, "Reversal_Forward_RPM", "Reversal_Forward_Freq", "VARCHAR", "48", "0", comm);
                ColumnAdd(TableName, "Reversal_Forward_RPMDirection", "Reversal_Forward_RPM", "VARCHAR", "48", "0", comm);
                ColumnAdd(TableName, "Reversal_Reverse_Freq", "Reversal_Forward_RPMDirection", "VARCHAR", "48", "0", comm);
                ColumnAdd(TableName, "Reversal_Reverse_RPM", "Reversal_Reverse_Freq", "VARCHAR", "48", "0", comm);
                ColumnAdd(TableName, "Reversal_Reverse_RPMDirection", "Reversal_Reverse_RPM", "VARCHAR", "48", "0", comm);               
                ColumnAdd(TableName, "starting_volt_test_result", "Reversal_Reverse_RPMDirection", "VARCHAR", "48", "PASS", comm);
                ColumnAdd(TableName, "lock_rotor_test_result", "starting_volt_test_result", "VARCHAR", "48", "PASS", comm);
                ColumnAdd(TableName, "no_load_test_result", "lock_rotor_test_result", "VARCHAR", "48", "PASS", comm);
                ColumnAdd(TableName, "all_test_passfail_name", "no_load_test_result", "VARCHAR", "200", "ALL TEST PASS", comm);
                ColumnAdd(TableName, "rejection_entries", "all_test_passfail_name", "VARCHAR", "1500", "PASS", comm);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        #endregion
        #region//-----------------------------------------------------Rejection Entry tab-----------------------------------------------------//
        private void RejectionTab(string TableName, MySqlCommand comm)
        {
            try
            {
                DBTableCreate(TableName, "id", "INT", "10", comm);
                ColumnAdd(TableName, "rejection_entries", "id", "VARCHAR", "1500", "PASS", comm);
                //TableValueAdd(TableName, "CategoryName", "Value", comm);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        #endregion
        #region//-----------------------------------------------------model_categoryTable-----------------------------------------------------//
        private void model_categoryTable(string TableName, MySqlCommand comm)
        {
            try
            {
                DBTableCreate(TableName, "CategoryId", "INT", "10", comm);
                ColumnAdd(TableName, "GroupId", "CategoryId", "INT", "11", "0", comm);
                ColumnAdd(TableName, "CategoryName", "GroupId", "VARCHAR", "256", "0", comm);
                //TableValueAdd(TableName, "CategoryName", "Value", comm);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        #endregion
        #region//-----------------------------------------------------model_groupTable-----------------------------------------------------//
        private void model_groupTable(string TableName, MySqlCommand comm)
        {
            try
            {
                DBTableCreate(TableName, "GroupId", "INT", "10", comm);
                ColumnAdd(TableName, "GroupName", "GroupId", "VARCHAR", "256", "0", comm);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        #endregion
        #region//-----------------------------------------------------model_parametersTable-----------------------------------------------------//
        private void model_parametersTable(string TableName, MySqlCommand comm)
        {
            try
            {
                DBTableCreate(TableName, "Id", "INT", "10", comm);
                ColumnAdd(TableName, "CategoryId", "Id", "INT", "10", "1", comm);
                ColumnAdd(TableName, "ItemId", "CategoryId", "INT", "10", "0", comm);
                ColumnAdd(TableName, "Model_Name", "ItemId", "VARCHAR", "256", "0", comm);
                ColumnAdd(TableName, "Series", "Model_Name", "VARCHAR", "256", "0", comm);
                ColumnAdd(TableName, "Hp", "Series", "VARCHAR", "15", "0", comm);
                ColumnAdd(TableName, "MotorRating", "Hp", "VARCHAR", "15", "0", comm);
                ColumnAdd(TableName, "Phase_Select", "MotorRating", "ENUM", "'1-Phase','2-Phase','3-Phase'", "1-Phase", comm);
                ColumnAdd(TableName, "RatedVoltage", "Phase_Select", "VARCHAR", "15", "0", comm);
                ColumnAdd(TableName, "RatedSpeed", "RatedVoltage", "VARCHAR", "15", "0", comm);
                ColumnAdd(TableName, "NoLoadCurrentMin", "RatedSpeed", "VARCHAR", "15", "0", comm);
                ColumnAdd(TableName, "NoLoadCurrentMax", "NoLoadCurrentMin", "VARCHAR", "15", "0", comm);
                ColumnAdd(TableName, "NoLoadPowerMin", "NoLoadCurrentMax", "VARCHAR", "15", "0", comm);
                ColumnAdd(TableName, "NoLoadPowerMax", "NoLoadPowerMin", "VARCHAR", "15", "0", comm);
                ColumnAdd(TableName, "LockRotorVoltage", "NoLoadPowerMax", "VARCHAR", "15", "0", comm);
                ColumnAdd(TableName, "LockRotorCurrentMin", "LockRotorVoltage", "VARCHAR", "15", "0", comm);
                ColumnAdd(TableName, "LockRotorCurrentMax", "LockRotorCurrentMin", "VARCHAR", "15", "0", comm);
                ColumnAdd(TableName, "LockRotorPowerMin", "LockRotorCurrentMax", "VARCHAR", "15", "0", comm);
                ColumnAdd(TableName, "LockRotorPowerMax", "LockRotorPowerMin", "VARCHAR", "15", "0", comm);
                ColumnAdd(TableName, "RatedFrequency", "LockRotorPowerMax", "VARCHAR", "15", "0", comm);
                ColumnAdd(TableName, "ResistanceMainMin", "RatedFrequency", "VARCHAR", "15", "0", comm);
                ColumnAdd(TableName, "ResistanceMainMax", "ResistanceMainMin", "VARCHAR", "15", "0", comm);
                ColumnAdd(TableName, "ResistanceAuxMin", "ResistanceMainMax", "VARCHAR", "15", "0", comm);
                ColumnAdd(TableName, "ResistanceAuxMax", "ResistanceAuxMin", "VARCHAR", "15", "0", comm);
                ColumnAdd(TableName, "ResistancePhaseMin", "ResistanceAuxMax", "VARCHAR", "15", "0", comm);
                ColumnAdd(TableName, "ResistancePhaseMax", "ResistancePhaseMin", "VARCHAR", "15", "0", comm);
                ColumnAdd(TableName, "LCT", "ResistancePhaseMax", "VARCHAR", "15", "0", comm);
                ColumnAdd(TableName, "StartingVoltage", "LCT", "VARCHAR", "15", "0", comm);
                ColumnAdd(TableName, "HV_Voltage", "StartingVoltage", "VARCHAR", "15", "0", comm);
                ColumnAdd(TableName, "HV_Current", "HV_Voltage", "VARCHAR", "15", "0", comm);
                ColumnAdd(TableName, "Megger_Value_Mohm", "HV_Current", "VARCHAR", "15", "0", comm);
                ColumnAdd(TableName, "ConnectionType", "Megger_Value_Mohm", "ENUM", "'None','Star','Delta'", "None", comm);
                ColumnAdd(TableName, "Acceleration", "ConnectionType", "VARCHAR", "15", "0", comm);
                ColumnAdd(TableName, "Deacceleration", "Acceleration", "VARCHAR", "15", "0", comm);
                ColumnAdd(TableName, "HV_Test_Time", "Deacceleration", "INT", "11", "5", comm);
                ColumnAdd(TableName, "Megger_Test_Time", "HV_Test_Time", "INT", "11", "5", comm);
                ColumnAdd(TableName, "LR_Test_Time", "Megger_Test_Time", "INT", "11", "6", comm);
                ColumnAdd(TableName, "LV_Test_Time", "LR_Test_Time", "INT", "11", "5", comm);
                ColumnAdd(TableName, "NL_Test_Time", "LV_Test_Time", "INT", "11", "6", comm);
                ColumnAdd(TableName, "Resistance_Test_Time", "NL_Test_Time", "INT", "11", "5", comm);
                ColumnAdd(TableName, "LCT_Test_Time", "Resistance_Test_Time", "INT", "11", "5", comm);
                ColumnAdd(TableName, "Mechanical_Test_Time", "LCT_Test_Time", "INT", "11", "0", comm);
                ColumnAdd(TableName, "ResistanceCorrectedMin", "Mechanical_Test_Time", "DOUBLE", "", "0", comm);
                ColumnAdd(TableName, "ResistanceCorrectedMax", "ResistanceCorrectedMin", "DOUBLE", "", "0", comm);
                ColumnAdd(TableName, "ResistanceCorrectedBRMin", "ResistanceCorrectedMax", "DOUBLE", "", "0", comm);
                ColumnAdd(TableName, "ResistanceCorrectedBRMax", "ResistanceCorrectedBRMin", "DOUBLE", "", "0", comm);
                ColumnAdd(TableName, "Start_Capacitor", "ResistanceCorrectedBRMax", "VARCHAR", "48", "0", comm);
                ColumnAdd(TableName, "Run_Capacitor", "Start_Capacitor", "VARCHAR", "48", "0", comm);
                ColumnAdd(TableName, "AutoMotorSrNumber", "Run_Capacitor", "INT", "11", "0", comm);
                ColumnAdd(TableName, "Motor_Rotation", "AutoMotorSrNumber", "ENUM", "'Clockwise','Anti-Clockwise','None'", "Clockwise", comm);
                ColumnAdd(TableName, "PrimaryRatedVoltage", "Motor_Rotation", "VARCHAR", "15", "0", comm);
                ColumnAdd(TableName, "PrimaryNoLoadCurrentMin", "PrimaryRatedVoltage", "VARCHAR", "15", "0", comm);
                ColumnAdd(TableName, "PrimaryNoLoadCurrentMax", "PrimaryNoLoadCurrentMin", "VARCHAR", "15", "0", comm);
                ColumnAdd(TableName, "PrimaryNoLoadPowerMin", "PrimaryNoLoadCurrentMax", "VARCHAR", "15", "0", comm);
                ColumnAdd(TableName, "PrimaryNoLoadPowerMax", "PrimaryNoLoadPowerMin", "VARCHAR", "15", "0", comm);
                ColumnAdd(TableName, "Rated_Current", "PrimaryNoLoadPowerMax", "VARCHAR", "15", "0", comm);
                ColumnAdd(TableName, "LRVoltageMin", "Rated_Current", "VARCHAR", "15", "0", comm);
                ColumnAdd(TableName, "LRVoltageMax", "LRVoltageMin", "VARCHAR", "15", "0", comm);
                ColumnAdd(TableName, "Stage", "LRVoltageMax", "VARCHAR", "15", "0", comm);
                ColumnAdd(TableName, "ISI_NOISI", "Stage", "VARCHAR", "8", "0", comm);
                ColumnAdd(TableName, "StarRating", "ISI_NOISI", "VARCHAR", "8", "0", comm);                
                ColumnAdd(TableName, "CGL_Min_StartCapacitor", "StarRating", "VARCHAR", "8", "0", comm);
                ColumnAdd(TableName, "CGL_Max_StartCapacitor", "CGL_Min_StartCapacitor", "VARCHAR", "8", "0", comm);
                ColumnAdd(TableName, "CGL_RunCapacitor", "CGL_Max_StartCapacitor", "VARCHAR", "8", "0", comm);
                ColumnAdd(TableName, "ReducedVoltage", "CGL_RunCapacitor", "DOUBLE", "", "0", comm);
                ColumnAdd(TableName, "Rev_F_Test_Time", "ReducedVoltage", "INT", "11", "5", comm);
                ColumnAdd(TableName, "Rev_R_Test_Time", "Rev_F_Test_Time", "INT", "11", "5", comm);
                ColumnAdd(TableName, "Model_Code", "Rev_R_Test_Time", "VARCHAR", "1024", "0", comm);
			}
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        #endregion
        #region//-----------------------------------------------------outputvoltagesetupTable-----------------------------------------------------//
        private void outputvoltagesetupTable(string TableName, MySqlCommand comm)
        {
            try
            {
                DBTableCreate(TableName, "OVId", "INT", "10", comm);
                ColumnAdd(TableName, "OutputVoltage", "OVId", "INT", "10", "0", comm);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        #endregion
        #region//-----------------------------------------------------pumpcodenumberTable-----------------------------------------------------//
        private void pumpcodenumberTable(string TableName, MySqlCommand comm)
        {
            try
            {
                DBTableCreate(TableName, "Id", "INT", "10", comm);
                ColumnAdd(TableName, "SAPDataId", "Id", "INT", "10", "0", comm);
                ColumnAdd(TableName, "PumpCodeNum", "SAPDataId", "VARCHAR", "48", "0", comm);
                ColumnAdd(TableName, "IsProcess", "PumpCodeNum", "TINYINT", "1", "0", comm);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        #endregion
        #region//-----------------------------------------------------qrcodeTable-----------------------------------------------------//
        private void qrcodeTable(string TableName, MySqlCommand comm)
        {
            try
            {
                DBTableCreate(TableName, "QRCodeId", "INT", "10", comm);
                ColumnAdd(TableName, "ModelParaId", "QRCodeId", "INT", "10", "0", comm);
                ColumnAdd(TableName, "SrNoSeries", "ModelParaId", "VARCHAR", "256", "0", comm);
                ColumnAdd(TableName, "SrNo", "SrNoSeries", "INT", "10", "0", comm);
                ColumnAdd(TableName, "NoOfSeries", "SrNo", "INT", "10", "0", comm);
                ColumnAdd(TableName, "WinderNumber", "NoOfSeries", "VARCHAR", "256", "0", comm);
                ColumnAdd(TableName, "RCValue", "WinderNumber", "INT", "10", "0", comm);
                ColumnAdd(TableName, "RCCombination", "RCValue", "INT", "10", "0", comm);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        #endregion
        #region//-----------------------------------------------------runningcapacitorsTable-----------------------------------------------------//
        private void runningcapacitorsTable(string TableName, MySqlCommand comm)
        {
            try
            {
                DBTableCreate(TableName, "RCId", "INT", "10", comm);
                ColumnAdd(TableName, "RCValue", "RCId", "INT", "10", "0", comm);
                ColumnAdd(TableName, "RCCombination", "RCValue", "INT", "10", "0", comm);
                ColumnAdd(TableName, "ModelParaId", "RCCombination", "INT", "10", "0", comm);
                ColumnAdd(TableName, "SrNo", "ModelParaId", "INT", "10", "0", comm);
                ColumnAdd(TableName, "NoOfSeries", "SrNo", "INT", "10", "0", comm);
                ColumnAdd(TableName, "SrNoSeries", "NoOfSeries", "VARCHAR", "256", "0", comm);
                ColumnAdd(TableName, "WinderNumber", "SrNoSeries", "VARCHAR", "256", "0", comm);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        #endregion
        #region//-----------------------------------------------------sapdataTable-----------------------------------------------------//
        private void sapdataTable(string TableName, MySqlCommand comm)
        {
            try
            {
                DBTableCreate("sapdata", "ID", "INT", "10", comm);
                ColumnAdd("sapdata", "Planing_Date", "ID", "DATE", "", "", comm);
                ColumnAdd("sapdata", "Motor_Rating", "Planing_Date", "VARCHAR", "256", "0", comm);
                ColumnAdd("sapdata", "Total_Quantity", "Motor_Rating", "INT", "10", "0", comm);
                ColumnAdd("sapdata", "Remaining_Quantity", "Total_Quantity", "INT", "10", "0", comm);
                ColumnAdd("sapdata", "From_Serial_No", "Remaining_Quantity", "VARCHAR", "256", "0", comm);
                ColumnAdd("sapdata", "To_Serial_No", "From_Serial_No", "VARCHAR", "256", "0", comm);
                ColumnAdd("sapdata", "ManLoc", "To_Serial_No", "VARCHAR", "15", "0", comm);
                ColumnAdd("sapdata", "Line_No", "ManLoc", "VARCHAR", "15", "0", comm);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        #endregion
        #region//-----------------------------------------------------startingcapacitorsTable-----------------------------------------------------//
        private void startingcapacitorsTable(string TableName, MySqlCommand comm)
        {
            try
            {
                DBTableCreate(TableName, "SCId", "INT", "10", comm);
                ColumnAdd(TableName, "SCValue", "SCId", "VARCHAR", "48", "0", comm);
                ColumnAdd(TableName, "SCCombination", "SCValue", "INT", "10", "0", comm);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        #endregion
        #region//-----------------------------------------------------testparameterTable-----------------------------------------------------//
        private void testparameterTable(string TableName, MySqlCommand comm)
        {
            try
            {
                DBTableCreate(TableName, "Id", "INT", "10", comm);
                ColumnAdd(TableName, "LineId", "Id", "VARCHAR", "8", "0", comm);
                ColumnAdd(TableName, "Test1", "LineId", "TINYINT", "4", "0", comm);
                ColumnAdd(TableName, "Test2", "Test1", "TINYINT", "4", "0", comm);
                ColumnAdd(TableName, "Test3", "Test2", "TINYINT", "4", "0", comm);
                ColumnAdd(TableName, "Test4", "Test3", "TINYINT", "4", "0", comm);
                ColumnAdd(TableName, "Test5", "Test4", "TINYINT", "4", "0", comm);
                ColumnAdd(TableName, "Test6", "Test5", "TINYINT", "4", "0", comm);
                ColumnAdd(TableName, "Test7", "Test6", "TINYINT", "4", "0", comm);
                ColumnAdd(TableName, "Test8", "Test7", "TINYINT", "4", "0", comm);
                ColumnAdd(TableName, "Test9", "Test8", "TINYINT", "4", "0", comm);
                ColumnAdd(TableName, "Buzzer1", "Test9", "TINYINT", "4", "0", comm);
                ColumnAdd(TableName, "Buzzer2", "Buzzer1", "TINYINT", "4", "0", comm);
                ColumnAdd(TableName, "Buzzer3", "Buzzer2", "TINYINT", "4", "0", comm);
                ColumnAdd(TableName, "Buzzer4", "Buzzer3", "TINYINT", "4", "0", comm);
                ColumnAdd(TableName, "Buzzer5", "Buzzer4", "TINYINT", "4", "0", comm);
                ColumnAdd(TableName, "Buzzer6", "Buzzer5", "TINYINT", "4", "0", comm);
                ColumnAdd(TableName, "Buzzer7", "Buzzer6", "TINYINT", "4", "0", comm);
                ColumnAdd(TableName, "Buzzer8", "Buzzer7", "TINYINT", "4", "0", comm);
                ColumnAdd(TableName, "Buzzer9", "Buzzer8", "TINYINT", "4", "0", comm);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        #endregion       
        #region//----------------------------------------------------- Company info -----------------------------------------------------//
        private void CompanyInfo(string TableName, MySqlCommand comm)
        {
            try
            {
                Request2Response("set global max_allowed_packet = 1048576000;");
                DBTableCreate(TableName, "Id", "INT", "10", comm);
                ColumnAdd(TableName, "CompanyName", "Id", "VARCHAR", "500", "1000", comm);
                ColumnAdd(TableName, "CompanyAddress", "CompanyName", "VARCHAR", "1000", "0", comm);
                ColumnAdd(TableName, "CompanyLogo", "CompanyAddress", "longblob", "", "", comm);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        #endregion
        #region//----------------------------------------------------- UserManagmentTable -----------------------------------------------------//
        private void UserManagement(string TableName, MySqlCommand comm)
        {
            try
            {
                DBTableCreate(TableName, "Id", "INT", "11", comm);
                ColumnAdd(TableName, "UserName", "Id", "varchar", "50", "", comm);
				ColumnAdd(TableName, "Password", "UserName", "varchar", "50", "", comm);
				ColumnAdd(TableName, "a1", "Password", "tinytext", "", "", comm);
				ColumnAdd(TableName, "a2", "a1", "tinytext", "", "", comm);
                ColumnAdd(TableName, "a3", "a2", "tinytext", "", "", comm);
                ColumnAdd(TableName, "a4", "a3", "tinytext", "", "", comm);
                ColumnAdd(TableName, "a5", "a4", "tinytext", "", "", comm);
                ColumnAdd(TableName, "a6", "a5", "tinytext", "", "", comm);
                ColumnAdd(TableName, "a7", "a6", "tinytext", "", "", comm);
                ColumnAdd(TableName, "a8", "a7", "tinytext", "", "", comm);
                ColumnAdd(TableName, "a9", "a8", "tinytext", "", "", comm);
                ColumnAdd(TableName, "a10","a9", "tinytext", "", "", comm);
                ColumnAdd(TableName, "a11", "a10", "tinytext", "", "", comm);
                ColumnAdd(TableName, "a12", "a11", "tinytext", "", "", comm);
                ColumnAdd(TableName, "a13", "a12", "tinytext", "", "", comm);
                ColumnAdd(TableName, "a14", "a13", "tinytext", "", "", comm);
                ColumnAdd(TableName, "a15", "a14", "tinytext", "", "", comm);
                ColumnAdd(TableName, "a16", "a15", "tinytext", "", "", comm);
                ColumnAdd(TableName, "a17", "a16", "tinytext", "", "", comm);
                ColumnAdd(TableName, "a18", "a17", "tinytext", "", "", comm);
                ColumnAdd(TableName, "a19", "a18", "tinytext", "", "", comm);
                ColumnAdd(TableName, "a20", "a19", "tinytext", "", "", comm);
                ColumnAdd(TableName, "a21", "a20", "tinytext", "", "", comm);
                ColumnAdd(TableName, "a22", "a21", "tinytext", "", "", comm);
                ColumnAdd(TableName, "a23", "a22", "tinytext", "", "", comm);
                ColumnAdd(TableName, "a24", "a23", "tinytext", "", "", comm);
            }                               
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        #endregion
        #region//-----------------------------------------------------UserSession-----------------------------------------------------//
        private void UserSession(string TableName, MySqlCommand comm)
        {
            try
            {
                DBTableCreate(TableName, "ID", "INT", "10", comm);
                ColumnAdd(TableName, "UserName", "Id", "VARCHAR", "48", "0", comm);
                ColumnAdd(TableName, "DateTime", "UserName", "VARCHAR", "128", "0", comm);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        #endregion
    }
}   