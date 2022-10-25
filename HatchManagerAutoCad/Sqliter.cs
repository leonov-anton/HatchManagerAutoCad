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
            string getChaptersSql = "SELECT name FROM 'CHAPTER'";

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
            string getDomainsSql = $"SELECT name FROM 'DOMAIN' WHERE chapter_id IN (SELECT id FROM FROM 'CHAPTER' WHERE name={chapterName})";

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

        public List<string> getGroups(string domainName, string chapterName)
        {
            List<string> groupsList = new List<string>();
            string getGroupsSql = $"SELECT name FROM 'GROUPS' WHERE domain_id " +
                                  $"IN $(SELECT id FROM 'DOMAIN' WHERE name = {domainName} AND chapter_id " +
                                  $"IN (SELECT id FROM 'CHAPTER' WHERE name={chapterName}))";

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
            string getHatchsSql = $"SELECT name, description, hatch_pattern, layer, guid FROM {groupeName}";

            using (SqliteConnection conn = new SqliteConnection($"Data Source={dbPath}"))
            {
                conn.Open();
                using (SqliteCommand cmd = new SqliteCommand(getHatchsSql, conn))
                {
                    SqliteDataReader sqliteDataReader = cmd.ExecuteReader();
                    while (sqliteDataReader.Read())
                    {
                        List<string> hatchList = new List<string> { (string)sqliteDataReader.GetValue(0), 
                                                                    (string)sqliteDataReader.GetValue(1), 
                                                                    (string)sqliteDataReader.GetValue(2),
                                                                    (string)sqliteDataReader.GetValue(3),
                                                                    (string)sqliteDataReader.GetValue(4)};
                        hatchsList.Add(hatchList);
                    }
                }
                conn.Close();
            }
            return hatchsList;
        }

        public List<string> getHatchData(string hatchName, string groupeName)
        {
            List<string> hatchList = new List<string>();
            string getHatchSql = $"SELECT hatch_Pattern, description, scale, angle, layer, color, color_back, trans, Group.OD_Table_Name, Group.Propertes, Group.Propertes_Data_Type, Group.Propertes_Table" +
                                $"FROM {groupeName} " +
                                $"JOIN Group " +
                                $"ON {groupeName}.Group_ID = Group.ID " +
                                $"WHERE {groupeName}.Name = {hatchName}";
            using (SqliteConnection conn = new SqliteConnection($"Data Source={dbPath}"))
            {
                conn.Open();
                using (SqliteCommand cmd = new SqliteCommand(getHatchSql, conn))
                {
                    SqliteDataReader sqliteDataReader = cmd.ExecuteReader();
                    while (sqliteDataReader.Read())
                    {
                        hatchList = new List<string> { hatchName,
                                                       (string)sqliteDataReader.GetValue(0), 
                                                       (string)sqliteDataReader.GetValue(1), 
                                                       (string)sqliteDataReader.GetValue(2),
                                                       (string)sqliteDataReader.GetValue(3),
                                                       (string)sqliteDataReader.GetValue(4),
                                                       (string)sqliteDataReader.GetValue(5),
                                                       (string)sqliteDataReader.GetValue(6),
                                                       (string)sqliteDataReader.GetValue(7),
                                                       (string)sqliteDataReader.GetValue(8),
                                                       (string)sqliteDataReader.GetValue(9),
                                                       (string)sqliteDataReader.GetValue(10),
                                                       (string)sqliteDataReader.GetValue(11)};
                    }
                }
            }
            return hatchList;
        }
    }
}
