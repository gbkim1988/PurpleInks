using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreyInks
{
    public class GreyUtils
    {
        /// <summary>
        /// 입력 스트림을 읽어와 출력 스트림에 전달
        /// 파일 복사 시에 사용
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        public static void CopyStream(Stream input, Stream output)
        {
            // Insert null checking here for production
            byte[] buffer = new byte[8192];

            int bytesRead;
            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, bytesRead);
            }
        }

        /// <summary>
        /// 실행 파일 내의 리소스 파일의 스트림을 반환
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public Stream GetResourceStream(string filename)
        {
            return this.GetType().Assembly.GetManifestResourceStream("GreyInks.Resources." + filename);
        }

        /// <summary>
        /// 지정된 파일과 동일한 리소스를 반환
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public bool ExtractExecutable(string filename)
        {
            try
            {
                using (Stream input = this.GetResourceStream(filename))
                using (Stream output = File.Create(Path.Combine(Directory.GetCurrentDirectory(), filename)))
                {
                    GreyUtils.CopyStream(input, output);
                }
            } catch (Exception) {
                return false;
            }

            return true;
            
        }
        /// <summary>
        /// 실행 파일 리소스 내에 저장된 파일과 출력할 파일의 이름을 전달하여
        /// 실행 파일 경로에 파일을 생성
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="newfilename"></param>
        /// <returns></returns>
        public bool ExtractExecutable(string filename, string newfilename)
        {
            try
            {
                using (Stream input = this.GetResourceStream(filename))
                using (Stream output = File.Create(Path.Combine(Directory.GetCurrentDirectory(), newfilename)))
                {
                    GreyUtils.CopyStream(input, output);
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;

        }
        // Singleton pattern 적용
        public static GreyUtils Instance { get; private set; }
        public static void Default()
        {
            GreyUtils.Instance = new GreyUtils();
        }
    }
}
