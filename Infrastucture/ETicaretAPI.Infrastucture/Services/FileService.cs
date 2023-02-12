
using ETicaretAPI.Infrastucture.StaticService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastucture.Services
{
    public class FileService 
    {
        public async Task<string> FileRenameAsync(string path, string fileName, bool first = true)
        {
            string newFileName = await Task.Run(async () =>
            {
                string extension = Path.GetExtension(fileName);
                string newFileName = string.Empty;

                if (first)
                {
                    string oldName = Path.GetFileNameWithoutExtension(fileName);
                    newFileName = $"{NameOperation.ChracterRegulatory(oldName)}{extension}";
                }
                else
                {
                    newFileName = fileName;
                    int indexNo = newFileName.IndexOf("-");
                    if (indexNo == -1)
                    {
                        newFileName = $"{Path.GetFileNameWithoutExtension(newFileName)}-2{extension}";
                    }
                    else
                    {
                        int lastEndex = 0;
                        while (true)
                        {
                            lastEndex = indexNo;
                            indexNo = newFileName.IndexOf("-", indexNo + 1);

                            if(indexNo == -1)
                            {
                                indexNo = lastEndex;
                                break;
                            }
                            
                        }
                        int indexNo2 = newFileName.IndexOf(".");
                        string fileNo = newFileName.Substring(indexNo + 1, indexNo2 - indexNo -1);
                        if (int.TryParse(fileNo, out int _fileNo))
                        {
                            _fileNo++;
                            newFileName = newFileName.Remove(indexNo + 1, indexNo2 - indexNo - 1)
                                                .Insert(indexNo + 1, _fileNo.ToString());
                        }
                        else
                        {
                            newFileName = $"{Path.GetFileNameWithoutExtension(newFileName)}-2{extension}";
                        }
                    }
                }

                if (File.Exists($"{path}\\{newFileName}"))
                    return await FileRenameAsync(path, newFileName, false);
                else
                    return newFileName;
            });

            return newFileName;
        }
    }
}
