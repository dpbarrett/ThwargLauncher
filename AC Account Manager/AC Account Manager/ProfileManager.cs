﻿using System;
using System.Collections.Generic;
using System.IO;

namespace AC_Account_Manager
{
    class ProfileManager
    {
        public void Save(List<CharacterSetting> profSettings, string profile)
        {
            string filepath = GetProfileFilePath(profile);
            using (StreamWriter stream = new StreamWriter(filepath))
            {
                stream.WriteLine("Version: 1");
                stream.WriteLine("Date: {0}", DateTime.UtcNow);
                foreach (var setting in profSettings)
                {
                    stream.WriteLine("{0},{1},{2}", setting.AccountName, setting.ServerName, setting.ChosenCharacter);
                }
            }
        }
        public List<CharacterSetting> Load(string profile)
        {
            string filepath = GetProfileFilePath(profile);
            if (!File.Exists(filepath)) { return null; }
            var list = new List<CharacterSetting>();
            using (StreamReader stream = new StreamReader(filepath))
            {
                string versionStr = stream.ReadLine();
                if (versionStr != "Version: 1") { return null; }
                string dateStr = stream.ReadLine();
                if (dateStr == null || !dateStr.StartsWith("Date: ")) { return null; }
                string line;
                while ((line = stream.ReadLine()) != null)
                {
                    char[] delimiterChars = { ',' };
                    string[] pieces = line.Split(delimiterChars);
                    if (pieces.Length != 3) { return list; }
                    list.Add(new CharacterSetting()
                        {
                            AccountName = pieces[0],
                            ServerName = pieces[1],
                            ChosenCharacter = pieces[2]
                        });
                }
            }
            return list;
        }
        private string GetProfileFilePath(string profileName)
        {
            string filename = string.Format("{0}.txt", profileName);
            string filepath = System.IO.Path.Combine(Configuration.AppFolder, filename);
            return filepath;
        }

    }
}
