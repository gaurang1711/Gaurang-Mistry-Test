using System;
using System.Configuration;

namespace PolicyProcessor
{
    public class KeySettings
    {
        #region [Input_File_Location]
        private static string Input_file_location;

        public static string Input_File_Location
        {
            get
            {
                if (String.IsNullOrEmpty(Input_file_location))
                {
                    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["InputFileLocation"]))
                    {
                        Input_file_location = ConfigurationManager.AppSettings["InputFileLocation"];
                        return Input_file_location;
                    }
                    else
                        throw new Exception("InputFileLocaion key does not exist in App.Config");
                }
                else
                    return Input_file_location;
            }
        }
        #endregion
        
        #region [Input_File_Extension]
        private static string Input_file_extension;

        public static string Input_File_Valid_Extension
        {
            get
            {
                if (String.IsNullOrEmpty(Input_file_extension))
                {
                    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["InputFileValidExtension"]))
                    {
                        Input_file_extension = ConfigurationManager.AppSettings["InputFileValidExtension"];
                        return Input_file_extension;
                    }
                    else
                        throw new Exception("InputFileValidExtension key does not exist in App.Config");
                }
                else
                    return Input_file_extension;
            }
        }
        #endregion

        #region [Approved_Policy]
        private static string Approved_policy;

        public static string Approved_Policy
        {
            get
            {
                if (String.IsNullOrEmpty(Approved_policy))
                {
                    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ApprovedPolicy"]))
                    {
                        Approved_policy = ConfigurationManager.AppSettings["ApprovedPolicy"];
                        return Approved_policy;
                    }
                    else
                        throw new Exception("ApprovedPolicy key does not exist in App.Config");
                }
                else
                    return Approved_policy;
            }
        }
        #endregion

        #region [Approved_Provinces]
        private static string Approved_provinces;

        public static string Approved_Provinces
        {
            get
            {
                if (String.IsNullOrEmpty(Approved_provinces))
                {
                    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ApprovedProvinces"]))
                    {
                        Approved_provinces = ConfigurationManager.AppSettings["ApprovedProvinces"];
                        return Approved_provinces;
                    }
                    else
                        throw new Exception("ApprovedProvinces key does not exist in App.Config");
                }
                else
                    return Approved_provinces;
            }
        }
        #endregion

        #region [Output_File_Location]
        private static string Output_file_location;

        public static string Output_File_Location
        {
            get
            {
                if (String.IsNullOrEmpty(Output_file_location))
                {
                    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["OutputFileLocation"]))
                    {
                        Output_file_location = ConfigurationManager.AppSettings["OutputFileLocation"];
                        return Output_file_location;
                    }
                    else
                        throw new Exception("OutputFileLocation key does not exist in App.Config");
                }
                else
                    return Output_file_location;
            }
        }
        #endregion   

        #region [Bot_Name]
        private static string Bot_name;

        public static string Bot_Name
        {
            get
            {
                if (String.IsNullOrEmpty(Bot_name))
                {
                    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["BotName"]))
                    {
                        Bot_name = ConfigurationManager.AppSettings["BotName"];
                        return Bot_name;
                    }
                    else
                        throw new Exception("BotName key does not exist in App.Config");
                }
                else
                    return Bot_name;
            }
        }
        #endregion   

        #region [Approved_Limit_Amount]
        private static string Approved_limit_amount;

        public static string Approved_Limit_Amount
        {
            get
            {
                if (String.IsNullOrEmpty(Approved_limit_amount))
                {
                    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ApprovedLimitAmount"]))
                    {
                        Approved_limit_amount = ConfigurationManager.AppSettings["ApprovedLimitAmount"];
                        return Approved_limit_amount;
                    }
                    else
                        throw new Exception("ApprovedLimitAmount key does not exist in App.Config");
                }
                else
                    return Approved_limit_amount;
            }
        }
        #endregion   

        #region [Log_File_Location]
        private static string Log_file_location;

        public static string Log_File_Location
        {
            get
            {
                if (String.IsNullOrEmpty(Log_file_location))
                {
                    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["LogFileLocation"]))
                    {
                        Log_file_location = ConfigurationManager.AppSettings["LogFileLocation"];
                        return Log_file_location;
                    }
                    else
                        throw new Exception("LogFileLocation key does not exist in App.Config");
                }
                else
                    return Log_file_location;
            }
        }
        #endregion   
    }
}
