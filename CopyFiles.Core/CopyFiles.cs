namespace CopyFiles.Core
{
    public class CopyFiles
    {
        public static async Task<string> RunAsync(string folderPath, string oldValue, string newValue)
        {
            string rootPath;

            try
            {
                if (File.Exists(folderPath))
                {
                    FileInfo oldFile = new FileInfo(folderPath);
                    rootPath = oldFile.DirectoryName!;
                    await FileCopyAndReplaceAsync(oldFile, oldValue, newValue);
                }
                else if (Directory.Exists(folderPath))
                {
                    DirectoryInfo oldFolder = new DirectoryInfo(folderPath);
                    rootPath = oldFolder.Parent!.FullName;
                    await FolderCopyAndReplaceAsync(oldFolder, oldValue, newValue);
                }
                else
                {
                    return "Folder Path no exists";
                }
                return $"Successful. {DateTime.Now:HH:mm:ss}";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }


            async Task FolderCopyAndReplaceAsync(DirectoryInfo oldFolder, string oldValue, string newValue)
            {
                string newFolderPath = GetNewPath(oldFolder.FullName, oldValue, newValue);
                if (!Directory.Exists(newFolderPath))
                {
                    Directory.CreateDirectory(newFolderPath);
                }

                foreach (FileInfo subOldFile in oldFolder.EnumerateFiles())
                {
                    await FileCopyAndReplaceAsync(subOldFile, oldValue, newValue);
                }

                foreach (DirectoryInfo subOldFolder in oldFolder.GetDirectories())
                {
                    await FolderCopyAndReplaceAsync(subOldFolder, oldValue, newValue);
                }
            }

            async Task FileCopyAndReplaceAsync(FileInfo oldFile, string oldValue, string newValue)
            {
                string newFilePath = GetNewPath(oldFile.FullName, oldValue, newValue);
                if (!File.Exists(newFilePath))
                {
                    string oldText = await File.ReadAllTextAsync(oldFile.FullName);
                    string newText = oldText.Replace(oldValue, newValue);

                    await File.WriteAllTextAsync(newFilePath, newText);
                }
            }

            string GetNewPath(string oldFolder, string oldValue, string newValue)
            {
                return oldFolder[..rootPath.Length] + oldFolder[rootPath.Length..].Replace(oldValue, newValue);
            }
        }
    }
}
