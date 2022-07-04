using System;

namespace PolicyProcessor
{    
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Logger logger = new Logger(KeySettings.Log_File_Location);
            Process process = new Process();

            logger.Info(" Bot Started");
            Console.WriteLine(" Bot Started");
          
            //Initiate Bot Process
            process.BotProcessStart();

            logger.Info(" Bot process completed successfully.");
            Console.WriteLine(" Bot process completed successfully.");
        }
    }
}
