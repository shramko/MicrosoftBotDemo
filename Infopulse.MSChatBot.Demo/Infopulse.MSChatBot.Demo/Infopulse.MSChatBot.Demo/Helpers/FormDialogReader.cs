using System;
using System.IO;
using System.Reflection;

namespace Infopulse.MSChatBot.Demo.Helpers
{
    [Serializable]
    public class FormDialogReader
    {
        public string Read(string dialogName)
        {
            var f = "Infopulse.MSChatBot.Demo.JSONForms." + dialogName + ".json";
            try
            {
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(f))
                using (var sr = new StreamReader(stream))
                    return sr.ReadToEnd();
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}