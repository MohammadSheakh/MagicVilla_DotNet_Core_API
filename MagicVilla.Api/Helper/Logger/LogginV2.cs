namespace MagicVilla.Api.Helper.Logger
{
    public class LogginV2 : ILogging
    {
        public void Log(string message, string type)
        {
            // we need to implement the method 
            if (type == "error")
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR - ⚫ - " + message);
                Console.BackgroundColor = ConsoleColor.Black;
            }
            else
            {
                // type warning hoile .. color DarkYellow korte hobe 
                Console.WriteLine(message);
            }
        }
    }
}
