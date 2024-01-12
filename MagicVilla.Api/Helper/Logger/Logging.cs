namespace MagicVilla.Api.Helper.Logger
{
    public class Logging : ILogging
    {
        // lets add a class .. that implement that interface
        public void Log(string message, string type)
        {
            // we need to implement the method 
            if (type == "error")
            {
                Console.WriteLine("ERROR - ⚫ - " + message);
            }
            else
            {
                Console.WriteLine(message);
            }
        }
    }
}
