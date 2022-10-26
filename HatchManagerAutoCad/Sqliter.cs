using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Microsoft.Data.Sqlite;

namespace HatchManagerAutoCad
{
    public class Sqliter
    {
        private static string dbPath = "base\\landscape.db";

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
            string getDomainsSql = $"SELECT name FROM 'DOMAIN' WHERE chaptar_id IN (SELECT id FROM 'CHAPTER' WHERE name='{chapterName}')";

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
            string getGroupsSql = $"SELECT name FROM 'GROUPS' WHERE domain_id IN (SELECT id FROM 'DOMAIN' WHERE name='{domainName}' AND chaptar_id IN (SELECT id FROM 'CHAPTER' WHERE name='{chapterName}'))";

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

            string getGroupTableName = $"SELECT od_table_name FROM 'GROUPS' WHERE name='{groupeName}'";

            using (SqliteConnection conn = new SqliteConnection($"Data Source={dbPath}"))
            {
                string groupTableName = "";
                conn.Open();
                using (SqliteCommand cmd = new SqliteCommand(getGroupTableName, conn))
                {
                    SqliteDataReader sqliteDataReader = cmd.ExecuteReader();
                    while (sqliteDataReader.Read())
                    {
                        groupTableName = (string)sqliteDataReader.GetValue(0);
                    }
                }

                string getHatchsSql = $"SELECT name, description, hatch_pattern, layer, guid FROM '{groupTableName}'";
                using (SqliteCommand cmd = new SqliteCommand(getHatchsSql, conn))
                {
                    SqliteDataReader sqliteDataReader = cmd.ExecuteReader();
                    while (sqliteDataReader.Read())
                    {
                        List<string> hatchList = new List<string>();
                        hatchList.Add((string)sqliteDataReader.GetValue(0));
                        if (sqliteDataReader.IsDBNull(1))
                            hatchList.Add("");
                        else
                            hatchList.Add((string)sqliteDataReader.GetValue(1));
                        hatchList.Add((string)sqliteDataReader.GetValue(2));
                        hatchList.Add((string)sqliteDataReader.GetValue(3));
                        hatchList.Add((string)sqliteDataReader.GetValue(4));

                        hatchsList.Add(hatchList);
                    }
                }
                conn.Close();
            }
            return hatchsList;
        }

        public ArrayList getHatchData(string hatchName, string groupeName)
        {
            ArrayList hatchList = new ArrayList();
            hatchList.Add((string)hatchName);

            string groupTableName = "";
            string getGroupTableName = $"SELECT od_table_name FROM 'GROUPS' WHERE name='{groupeName}'";
            
            using (SqliteConnection conn = new SqliteConnection($"Data Source={dbPath}"))
            {
                conn.Open();
                using (SqliteCommand cmd = new SqliteCommand(getGroupTableName, conn))
                {
                    SqliteDataReader sqliteDataReader = cmd.ExecuteReader();
                    while (sqliteDataReader.Read())
                    {
                        groupTableName = (string)sqliteDataReader.GetValue(0);
                    }
                }
                string getHatchSql = $"SELECT hatch_pattern, description, scale, angle, layer, color, color_back, trans, 'GROUPS'.od_table_name, 'GROUPS'.propertes, 'GROUPS'.propertes_data_type, 'GROUPS'.propertes_table FROM '{groupTableName}' JOIN 'GROUPS' ON '{groupTableName}'.group_id = 'GROUPS'.id WHERE '{groupTableName}'.name=@hatchName";

                using (SqliteCommand cmd = new SqliteCommand(getHatchSql, conn))
                {
                    cmd.Parameters.AddWithValue("@hatchName", hatchName);
                    SqliteDataReader sqliteDataReader = cmd.ExecuteReader();
                    while (sqliteDataReader.Read())
                    {
                        hatchList.Add(sqliteDataReader.GetValue(0)); // hatch_pattern
                        if (sqliteDataReader.IsDBNull(1)) // description
                            hatchList.Add("");
                        else
                            hatchList.Add(sqliteDataReader.GetValue(1)); 
                        hatchList.Add(sqliteDataReader.GetValue(2)); // scale
                        if (sqliteDataReader.IsDBNull(3)) // angle
                            hatchList.Add(0);
                        else
                            hatchList.Add(sqliteDataReader.GetValue(3));
                        hatchList.Add(sqliteDataReader.GetValue(4)); // layer
                        if (sqliteDataReader.IsDBNull(5)) // color
                            hatchList.Add("");
                        else    
                            hatchList.Add(sqliteDataReader.GetValue(5));
                        if (sqliteDataReader.IsDBNull(6)) // color_back
                            hatchList.Add("");
                        else
                            hatchList.Add(sqliteDataReader.GetValue(6));
                        if (sqliteDataReader.IsDBNull(7)) // trans
                            hatchList.Add(0);
                        else
                            hatchList.Add(sqliteDataReader.GetValue(7));
                        hatchList.Add(sqliteDataReader.GetValue(8)); // od_table_name
                        hatchList.Add(sqliteDataReader.GetValue(9)); // proprtes
                        hatchList.Add(sqliteDataReader.GetValue(10)); // proprtes_data_type
                        hatchList.Add(sqliteDataReader.GetValue(11)); // proprtes_table
                    }
                    conn.Close();
                }
            }
            return hatchList;
        }

        public int getIntObjData(string valueName, string groupeName, string hatchName)
        {
            int value = 0;
            string getHatchSql = $"SELECT {valueName} FROM '{groupeName}' WHERE name=@hatchName";
            using (SqliteConnection conn = new SqliteConnection($"Data Source={dbPath}"))
            {
                conn.Open();
                using (SqliteCommand cmd = new SqliteCommand(getHatchSql, conn))
                {
                    cmd.Parameters.AddWithValue("@hatchName", hatchName);
                    SqliteDataReader sqliteDataReader = cmd.ExecuteReader();
                    while (sqliteDataReader.Read())
                    {
                        value = (int)sqliteDataReader.GetValue(0);
                    }
                    conn.Close();
                }
            }
            return value;
        }
        public string getStrObjData(string valueName, string groupeName, string hatchName)
        {
            string value ="";
            string getHatchSql = $"SELECT {valueName} FROM '{groupeName}' WHERE name=@hatchName";
            using (SqliteConnection conn = new SqliteConnection($"Data Source={dbPath}"))
            {
                conn.Open();
                using (SqliteCommand cmd = new SqliteCommand(getHatchSql, conn))
                {
                    cmd.Parameters.AddWithValue("@hatchName", hatchName);
                    SqliteDataReader sqliteDataReader = cmd.ExecuteReader();
                    while (sqliteDataReader.Read())
                    {
                        value = (string)sqliteDataReader.GetValue(0);
                    }
                    conn.Close();
                }
            }
            return value;
        }
        public double getRealObjData(string valueName, string groupeName, string hatchName)
        {
            double value = 0;
            string getHatchSql = $"SELECT {valueName} FROM '{groupeName}' WHERE name=@hatchName";
            using (SqliteConnection conn = new SqliteConnection($"Data Source={dbPath}"))
            {
                conn.Open();
                using (SqliteCommand cmd = new SqliteCommand(getHatchSql, conn))
                {
                    cmd.Parameters.AddWithValue("@hatchName", hatchName);
                    SqliteDataReader sqliteDataReader = cmd.ExecuteReader();
                    while (sqliteDataReader.Read())
                    {
                        value = (double)sqliteDataReader.GetValue(0);
                    }
                    conn.Close();
                }
            }
            return value;
        }
    }
}
