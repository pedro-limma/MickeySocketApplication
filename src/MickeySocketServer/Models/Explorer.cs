namespace MickeySocketServer.Models
{
    public class Explorer
    {
        public Explorer(string dados)
        {

            var properties = dados.Split(',', StringSplitOptions.RemoveEmptyEntries);


            Filename = properties[0];
            QtnBytes = int.Parse(properties[1]);
            BaseDir = properties[2];
        }
        public static bool IsMatch { get; private set; }
        public string BaseDir { get; set; }
        public string Filename { get; set; }
        public int QtnBytes { get; set; }


        public void MatchFile()
        {
            try
            {
                Parallel.Invoke(() => ScanDirectories(BaseDir));
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ScanDirectories(string path)
        {
            if (IsMatch) return;
            try
            {
                VerifyFile(path);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine(ex.Message);
            }

            if (!IsMatch)
            {
                string[] directories = Directory.GetDirectories(path);

                try
                {
                    Parallel.ForEach(directories, d => ScanDirectories(d));
                }
                catch (UnauthorizedAccessException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void VerifyFile(string path)
        {
            string[] Files = Directory.GetFiles(path);

            foreach (string File in Files)
            {
                if (IsMatch) return;

                FileInfo info = new FileInfo(File);

                IsEquals(info);
            }
        }

        private void IsEquals(FileInfo fileInfo)
        {
            if (fileInfo.Name == this.Filename &&  QtnBytes == fileInfo.Length)
            {
                Console.WriteLine("ENCONTROU CARALHO");
            }
        }
    }
}
