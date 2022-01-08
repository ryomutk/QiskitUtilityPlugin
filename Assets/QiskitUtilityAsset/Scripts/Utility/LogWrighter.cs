using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;
using Utility;

namespace Utility
{
    public static class LogWriter
    {
        /// <summary>
        /// アセット下のログフォルダの場所
        /// </summary>
        /// <returns></returns>
        const string dirLocalPath = "Logs/";

        static string dirPath { get { return Application.dataPath + "/" + dirLocalPath; } }
        static string mainLog = null;



        /// <summary>
        /// 新しいlogを開始する。
        /// </summary>
        /// <param name="fileName">作るログの名前。入力しない場合はmainLogが作成される。</param>
        /// <returns>logのidを返す。すでに同名のファイルが作成されている場合は、新しくは作らず存在するもののidを返す</returns>
        static void MakeLogFile(string fileName = null)
        {

            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }


            if (fileName == null)
            {
                if (mainLog == null)
                {
                    mainLog = DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
                }

                fileName = mainLog;
            }
            else
            {
                if (Path.GetExtension(fileName) != ".txt")
                {
                    fileName = fileName + ".txt";
                }
            }

            var fullPath = dirPath + fileName;

            if (!File.Exists(fullPath))
            {
                File.Create(fullPath).Close();
            }
        }

        /// <summary>
        /// ファイル名を指定してログをつける
        /// </summary>
        /// <param name="log"></param>
        /// <param name="logName"></param>
        /// <param name="append">falseの場合、同名でも新しいLogを作成</param>
        public static void Log(string log, string logName, bool append)
        {
            if (Path.GetExtension(logName) != ".txt")
            {
                if (!append)
                {
                    logName += "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                }

                logName += ".txt";
            }

            if (!File.Exists(dirPath + logName))
            {
                MakeLogFile(logName);
            }


            var fullPath = dirPath + logName;


            var sw = new StreamWriter(fullPath, true);
            sw.WriteLine(log);
            sw.Flush();
            sw.Close();
        }

    }
}