namespace HorizonControlCenterWebAPI

{
    /// <summary>
    /// </summary>
    public class Uploadlogger
    {
        /// <summary>
        /// </summary>
        public static string? ContentRootPath { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="message"></param>
        public static void WriteLogMessage(string message)
        {
            ////For Windows Server
            //using (StreamWriter writer = new StreamWriter(ContentRootPath + "log.txt", true))
            //{
            //    writer.WriteLine(DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss") + " : " + message);
            //    writer.Close();
            //}

            //  For DryRunServer
            //using (StreamWriter writer = File.AppendText(@"/var/www/log.txt"))
            //{
            //    writer.WriteLine(DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss") + " : " + message);

            //}

            string logFilePath = "/var/www/CommonService.txt";
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException("Message cannot be null or empty.", nameof(message));
            }

            try
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur while writing to the file.
                Console.WriteLine($"Error logging message: {ex.Message}");
            }
        }
    }
}
