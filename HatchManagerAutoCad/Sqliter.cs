using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Data.Sqlite;

namespace HatchManagerAutoCad
{
    public class Sqliter
    {
        private static string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "landscape.db");

        public List<string> getChapters()
        {
            List<string> chaptersList = new List<string>();
            string getChaptersSql = "SELECT Name FROM Chapter";

            using (SqliteConnection conn = new SqliteConnection($"Data Source={dbPath}"))
            {
                conn.Open();
                using (SqliteCommand cmd = new SqliteCommand(getChaptersSql, conn))
                {
                    SqliteDataReader sqliteDataReader = cmd.ExecuteReader();
                    while (sqliteDataReader.Read())
                    {
                        chaptersList.Add(sqliteDataReader.GetString(0));
                    }
                }
                conn.Close();
            }
            return chaptersList;
        }

        public List<string> getDomains(string chapterName)
        {
            List<string> domainsList = new List<string>();
            string getDomainsSql = $"SELECT Name FROM Domain WHERE Chapter_ID IN (SELECT id FROM Chapters WHERE Name = {chapterName})";

            using (SqliteConnection conn = new SqliteConnection($"Data Source={dbPath}"))
            {
                conn.Open();
                using (SqliteCommand cmd = new SqliteCommand(getDomainsSql, conn))
                {
                    SqliteDataReader sqliteDataReader = cmd.ExecuteReader();
                    while (sqliteDataReader.Read())
                    {
                        domainsList.Add(sqliteDataReader.GetString(0));
                    }
                }
                conn.Close();
            }
            return domainsList;
        }

        public List<string> getGroups(string domainName)
        {
            List<string> groupsList = new List<string>();
            string getGroupsSql = $"SELECT Name FROM Group WHERE DOMAIN_ID IN (SELECT id FROM Domain WHERE Name = {domainName})";

            using (SqliteConnection conn = new SqliteConnection($"Data Source={dbPath}"))
            {
                conn.Open();
                using (SqliteCommand cmd = new SqliteCommand(getGroupsSql, conn))
                {
                    SqliteDataReader sqliteDataReader = cmd.ExecuteReader();
                    while (sqliteDataReader.Read())
                    {
                        groupsList.Add(sqliteDataReader.GetString(0));
                    }
                }
                conn.Close();
            }
            return groupsList;
        }

        public List<List<string>> getHatchsData(string groupeName)
        {
            List<List<string>> hatchsList = new List<List<string>>();
            string getHatchsSql = $"SELECT Name, Description, Hatch_Pattern, Layer, GUID FROM {groupeName}";

            using (SqliteConnection conn = new SqliteConnection($"Data Source={dbPath}"))
            {
                conn.Open();
                using (SqliteCommand cmd = new SqliteCommand(getHatchsSql, conn))
                {
                    SqliteDataReader sqliteDataReader = cmd.ExecuteReader();
                    while (sqliteDataReader.Read())
                    {
                        string name = (string)sqliteDataReader.GetValue(0);

                        List<string> hatchList = new List<string> { (string)sqliteDataReader.GetValue(0), 
                                                                    (string)sqliteDataReader.GetValue(1), 
                                                                    (string)sqliteDataReader.GetValue(2),
                                                                    (string)sqliteDataReader.GetValue(3),
                                                                    (string)sqliteDataReader.GetValue(5)};
                        hatchsList.Add(hatchList);
                    }
                }
                conn.Close();
            }
            return hatchsList;
        }
    }
}
