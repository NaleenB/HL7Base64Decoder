using System;
using System.IO;
using System.Linq;
using System.Text;

namespace HL7Base64Decoder.ProcessingLogic
{
    class FileHandler
    {
        static readonly string[] allowedExtensions = { ".dat", ".hl7", "", ".txt" };
        internal static void ProcessFiles(string inputPath, string outputPath, out string outputLog)
        {
            outputLog = "";
            string[] inputFiles = Directory.GetFiles(inputPath);

            foreach (string filePath in inputFiles)
            {
                try
                {//FileInfo(path).Length
                    string inputFileName = Path.GetFileName(filePath);

                    string fileExt = Path.GetExtension(filePath);

                    FileInfo fi = new FileInfo(filePath);

                    if (fi.Length > 20 * 1024)
                    {
                        throw new Exception(inputFileName + " size larger than allowed 10kb.");
                    }

                    if (!allowedExtensions.Contains(fileExt.ToLower()))
                    {
                        throw new Exception(inputFileName + " has disallowed file extension.");
                    }

                    string fileText = File.ReadAllText(filePath);

                    HL7Handler hL7Handler = new HL7Handler(fileText);

                    byte[] pdfArr = hL7Handler.GetPDFStr();

                    if (!ASCIIEncoding.ASCII.GetString(pdfArr).ToLower().Contains("pdf"))
                    {
                        throw new Exception(Path.GetFileName(filePath) + " has unrecognised pdf string.");
                    }

                    string msh10 = hL7Handler.getMSH10();
                    string pdfName = msh10 + ".pdf";

                    SaveFile(outputPath, pdfName, pdfArr);

                    outputLog = outputLog + pdfName + " saved." + Environment.NewLine;
                }
                catch (Exception ex)
                {
                    outputLog = outputLog + ex.Message + Environment.NewLine;
                }
            }
        }

        private static void SaveFile(string savePath, string fileName, byte[] fileBody)
        {
            File.WriteAllBytes(Path.Combine(savePath, fileName), fileBody);
        }
    }
}
