using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using System.Data;
using PolicyProcessor.Model;
using ClosedXML.Excel;

namespace PolicyProcessor
{
    public class Process
    {
        #region [ Private Members ]
        XDocument xSummary;
        decimal totalPremium = 0;
        int totalPolicies = 0;
        List<Summary> summaryList = new List<Summary>();
        Logger logger = new Logger(KeySettings.Log_File_Location);

        List<string> approvedPolicy = KeySettings.Approved_Policy.Split(',').ToList();
        List<string> approvedProvinces = KeySettings.Approved_Provinces.Split(',').ToList();
        string dynamicPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        #endregion

        #region [ Public Methods]
        public void BotProcessStart()
        {
            try
            {
                //Get file location from app.setting
                string fileLocation = KeySettings.Input_File_Location;

                //Combine bin folder path (dynamically) with file locaion
                string path = Path.Combine(dynamicPath, fileLocation);

                logger.Info(" Check files in input folder");
                Console.WriteLine(" Check files in input folder");

                //Check input directory has file or not
                if (Directory.EnumerateFileSystemEntries(path).Any())
                {
                    DirectoryInfo di = new DirectoryInfo(path);

                    //Get all file name from directory
                    var files = di.GetFiles().Select(x => x.Name).ToList();

                    //Creating xml file for export.
                    xSummary = new XDocument(new XElement("Collection",
                                                new XElement("Carriers"),
                                                new XElement("Policies"),
                                                new XElement("TotalPolicies"),
                                                new XElement("TotalPremium")));

                    logger.Info(" Found files in input folder to process");
                    Console.WriteLine(" Found files in input folder to process");

                    //Accessing file one by one from directory
                    foreach (var file in files)
                    {
                        //Create path with input file name
                        var pathwithFile = path + @"\" + file;

                        logger.Info(" Validate input file (" + file + ") extension");
                        Console.WriteLine(" Validate input file (" + file + ") extension");

                        //Check file has valid extension or not
                        if (Path.GetExtension(pathwithFile) == KeySettings.Input_File_Valid_Extension)
                        {
                            logger.Info(" File extension is valid and start reading file");
                            Console.WriteLine(" File extension is valid and start reading file");

                            //Load XML document
                            XDocument xdoc = XDocument.Load(pathwithFile);

                            //Read and Validate XML
                            ReadAllPolicy(xdoc);
                        }
                        else
                        {
                            //invlid file extension
                            logger.Info(" Invalid file extension");
                            Console.WriteLine(" Invalid file extension");
                        }
                    }

                    logger.Info(" All input file processing completed");
                    Console.WriteLine(" All input file processing completed");

                    //Process export policy if eligible policy found
                    if (totalPolicies > 0)
                    {

                        //Assign total policies to XML
                        var policies = xSummary.Descendants("TotalPolicies").FirstOrDefault();
                        policies.Value = totalPolicies.ToString();

                        //Assign total premium to XML
                        var premium = xSummary.Descendants("TotalPremium").FirstOrDefault();
                        premium.Value = totalPremium.ToString();

                        logger.Info(" Start sorting policies based on policy number.");
                        Console.WriteLine(" Start sorting policies based on policy number.");

                        //Sort XML based on Policy Number
                        SortXML(ref xSummary);

                        logger.Info(" Sorting policies completed.");
                        Console.WriteLine(" Sorting policies completed.");

                        logger.Info(" Starting exporting summary XML file.");
                        Console.WriteLine(" Starting exporting summary XML file.");

                        //Exporting summary XML
                        ExportResultXML(xSummary);

                        logger.Info(" Summary XML exporting successfully.");
                        Console.WriteLine(" Summary XML exporting successfully.");
                    }
                    else
                    {
                        //no eligibal policy found to export
                        logger.Info(" No eligible policies found to export summary XML.");
                        Console.WriteLine(" No eligible policies found to export summary XML.");
                    }

                    if(summaryList.Count>0)
                    {

                        logger.Info(" Starting exporting report in Excel file.");
                        Console.WriteLine(" Starting exporting report in Excel file.");

                        //Export Excel report
                        ExportResultExcel(summaryList);

                        logger.Info(" Excel report exporting successfully.");
                        Console.WriteLine(" Excel report exporting successfully.");

                    }
                    else
                    {
                        //no eligibal policy found to export
                        logger.Info(" No eligible policies found to export Excel report.");
                        Console.WriteLine(" No eligible policies found to export Excel report.");
                    }
                }
                else
                {
                    //no file found
                    logger.Info(" No input file found to process.");
                    Console.WriteLine(" No input file found to process.");
                }
            }
            catch (Exception ex)
            {
                logger.Error(" Exception Occured. : " + ex.Message );
                Console.WriteLine(" Exception Occured. : " + ex.Message);
            }
        }
        #endregion

        #region [ Private Methods ]
        /// <summary>
        /// Read and validate policy
        /// </summary>
        /// <param name="xdoc">Input file in XML</param>
        private void ReadAllPolicy(XDocument xdoc)
        {
            try
            {
                logger.Info(" File processing started.");
                Console.WriteLine(" File processing started.");

                Summary summary = new Summary();

                //Assing value for report purpose
                summary.LineOfBusiness = Convert.ToString(xdoc.Root.Name);

                summary.Policy = xdoc.Descendants()
                                     .Where(x => x.Name.LocalName == "PolicyNumber")
                                     .FirstOrDefault()
                                     .Value;
                summary.Province = xdoc.Descendants()
                                     .Where(x => x.Name.LocalName == "Province")
                                     .FirstOrDefault()
                                     .Value;
                summary.Limit = xdoc.Descendants()
                                     .Where(x => x.Name.LocalName == "Limit")
                                     .FirstOrDefault()
                                     .Value
                                     .Replace("$", string.Empty);
                summary.Premium = xdoc.Descendants()
                                     .Where(x => x.Name.LocalName == "Premium")
                                     .FirstOrDefault()
                                     .Value
                                     .Replace("$", string.Empty);

                summary.Status = "Declined";

                //Validate policy type with app.setting list
                if (approvedPolicy.Contains(summary.LineOfBusiness))
                {
                    //Validate province with app.setting list
                    if (approvedProvinces.Contains(summary.Province))
                    {
                        //Validate Limit amount with approved amount
                        if (Convert.ToDecimal(summary.Limit) > Convert.ToDecimal(KeySettings.Approved_Limit_Amount))
                        {
                            summary.Status = "Referred to Underwriter";
                        }
                        else
                        {
                            summary.Status = "Approved";

                            //Count total premium
                            totalPremium += Convert.ToDecimal(summary.Premium);

                            //Count total approved policies
                            totalPolicies += 1;

                            //Get carrier node from input XML
                            XNode careerNode = xdoc.Descendants()
                                                .Where(x => x.Name.LocalName == "Carrier")
                                                .FirstOrDefault();

                            //Get carrier node from summary XML
                            var career = xSummary.Descendants("Carriers").FirstOrDefault();

                            //Can remove the duplicate career node but not written code as was not in requirement.
                            career.Add(careerNode);

                            //Get policy node from input XML
                            XNode policyNode = xdoc.Root;

                            //Get policy node from summary XML
                            var policies = xSummary.Descendants("Policies").FirstOrDefault();
                            policies.Add(policyNode);
                        }
                    }
                }

                //Add current summary detail for summary list
                summaryList.Add(summary);

                logger.Info(" File processing completed." );
                Console.WriteLine(" File processing completed.");

            }
            catch (Exception ex)
            {
                //Error in read data in given input XML
                logger.Error(" Exception Occured. : " + ex.Message);
                Console.WriteLine(" Exception Occured. : " + ex.Message);
            }
        }

        /// <summary>
        /// Expoert reult in XML
        /// </summary>
        /// <param name="xdoc">Summary document (XML)</param>
        private void ExportResultXML(XDocument xdoc)
        {
            try
            {
                //Save XML File
                string filename = "Output_XML_" + CommonFunction.GetTimestamp(DateTime.Now) + ".xml";

                string outputFilePath = KeySettings.Output_File_Location;
                string path = Path.Combine(dynamicPath, outputFilePath, filename);
                xdoc.Save(path);               
            }
            catch (Exception ex)
            {
                //Error in exporting
                logger.Error(" Exception Occured. : " + ex.Message);
                Console.WriteLine(" Exception Occured. : " + ex.Message);
            }
        }

        /// <summary>
        /// Expoert report in Excel
        /// </summary>
        /// <param name="summaryList">Summary list detail (List of Summary)</param>
        private void ExportResultExcel(List<Summary> summaryList)
        {
            try
            {
                //Save Excel File
                string outputFilePath = KeySettings.Output_File_Location;     
                string filename = "Output_Excel_" + CommonFunction.GetTimestamp(DateTime.Now) + ".xlsx";
                string path = Path.Combine(dynamicPath, outputFilePath, filename);

                var dt = CommonFunction.ConvertToDataTable(summaryList);
                XLWorkbook wb = new XLWorkbook();
                wb.Worksheets.Add(dt, "Report");
                wb.SaveAs(path);
            }
            catch (Exception ex)
            {
                //Error in exporting
                logger.Error(" Exception Occured. : " + ex.Message);
                Console.WriteLine(" Exception Occured. : " + ex.Message);
            }
        }

        /// <summary>
        /// Sort policy based on policy number
        /// </summary>
        /// <param name="xdoc">summry XML document</param>
        private void SortXML(ref XDocument xdoc)
        {
            //Order policy based on policy number
            var orderedPolicy = xdoc.Descendants("Policies").Elements().OrderBy(m => m.Element("PolicyNumber").Value).ToList();

            //Remove exisitng unordered policy from XML
            xdoc.Descendants("Policies").Elements().Remove();

            //Assing ordered policy to XML
            foreach (var policy in orderedPolicy)
            {
                xdoc.Descendants("Policies").ElementAtOrDefault(0).Add(new XElement(policy));
            }
        }
        #endregion
    }
}