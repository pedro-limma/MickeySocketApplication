namespace MickeySocketServer.Models
{
    public class Explorer
    {
        public Explorer(dynamic properties)
        {
            BaseDir = properties.BaseDir;
            Filename = properties.Filename;
            QtnBytes = properties.QtnBytes;
        }
        public string BaseDir { get; set; }
        public string Filename { get; set; }
        public int QtnBytes { get; set; }

        public string GetContentFile()
        {
            return "RETORNANDO CONTEÚDO";
        }
    }
}
