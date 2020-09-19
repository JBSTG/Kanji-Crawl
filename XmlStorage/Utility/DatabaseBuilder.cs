using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.IO;
using System.Diagnostics;
using System.Data.SqlClient;
using XmlStorage.Classes;
using XmlStorage.Models;

namespace XmlStorage.Utility
{
    class DatabaseBuilder
    {
        public static void AddKanjiToGradeSet(KanjiListModel list,int grade)
        {
            SQLiteConnection connection = new SQLiteConnection("Data Source=Kanji.db");
            SQLiteCommand command;
            connection.Open();
            string category = "grade-" + grade;
            command = new SQLiteCommand("SELECT rowid FROM KanjiSet WHERE kanjiSetName=@Grade", connection);
            command.Parameters.AddWithValue("@Grade", category);
            int setRowid = Int32.Parse(command.ExecuteScalar().ToString());

            command = new SQLiteCommand("INSERT INTO KanjiInSet(kanjiSetId,kanjiId) " +
            "SELECT @SetRowId,rowid " +
            "FROM Kanji " +
            "WHERE grade = @SetRowId", connection);
            command.Parameters.AddWithValue("@SetRowId", setRowid);
            command.ExecuteNonQuery();

            connection.Close();
        }
        public static void CreateSet(string setName,string dbname)
        {
            SQLiteConnection connection = new SQLiteConnection("Data Source=" + dbname + ".db");
            SQLiteCommand command;
            connection.Open();
            command = new SQLiteCommand("INSERT INTO KanjiSet (kanjiSetName) VALUES(@SetName)", connection);
            command.Parameters.AddWithValue("@SetName", setName);

            var r = command.ExecuteNonQuery();
            connection.Close();
        }
        public static void ListAllKanji(string dbname)
        {
            SQLiteConnection connection = new SQLiteConnection("Data Source=" + dbname + ".db");
            SQLiteCommand command;
            connection.Open();
            command = new SQLiteCommand("SELECT *,rowid FROM Kanji;", connection);
            var r = command.ExecuteReader();
            while (r.Read())
            {
                for (int i = 0;i<r.FieldCount;i++)
                {
                    Debug.Write(r.GetValue(i).ToString()+" ");
                }
                Debug.WriteLine("");
            }
            connection.Close();
        }
        public static void ListTables(string dbname)
        {
            SQLiteConnection connection = new SQLiteConnection("Data Source=" + dbname + ".db");
            SQLiteCommand command;
            connection.Open();
            //Get all tables by name
            //command = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type = 'table' AND name NOT LIKE 'sqlite_%'; ", connection);
            //Get structure of table
            command = new SQLiteCommand("SELECT sql FROM sqlite_master;", connection);
            var r = command.ExecuteReader();
            while (r.Read())
            {
                Debug.WriteLine(r.GetString(0));
            }
            connection.Close();
        }
        public static void AddTables(string dbname)
        {
            SQLiteConnection connection = new SQLiteConnection("Data Source=" + dbname + ".db");
            SQLiteCommand command;
            int rows;
            connection.Open();
            //Kanji
            command = new SQLiteCommand("CREATE TABLE IF NOT EXISTS Kanji(" +
                "character TEXT," +
                "grade INTEGER," +
                "strokeCount INTEGER," +
                "jlpt INTEGER," +
                "unicode TEXT," +
                "heisigEnglish TEXT)",
                connection);
            rows = command.ExecuteNonQuery();
            //Debug.WriteLine(rows.ToString());
            //Kanji Meanings
            command = new SQLiteCommand("CREATE TABLE IF NOT EXISTS KanjiMeaning(" +
            "meaning TEXT," +
            "kanjiId INTEGER,"+
            "FOREIGN KEY (kanjiId) REFERENCES Kanji(rowid))",
            connection);
            rows = command.ExecuteNonQuery();

            //Kanji Kun Readings
            command = new SQLiteCommand("CREATE TABLE IF NOT EXISTS KanjiOnReading(" +
            "onReading TEXT," +
            "kanjiId INTEGER," +
            "FOREIGN KEY (kanjiId) REFERENCES Kanji(rowid))",
            connection);
            rows = command.ExecuteNonQuery();

            //Kanji On Readings
            command = new SQLiteCommand("CREATE TABLE IF NOT EXISTS KanjiKunReading(" +
            "kunReading TEXT," +
            "kanjiId INTEGER," +
            "FOREIGN KEY (kanjiId) REFERENCES Kanji(rowid))",
            connection);
            rows = command.ExecuteNonQuery();

            //Kanji Name Readings
            command = new SQLiteCommand("CREATE TABLE IF NOT EXISTS KanjiNameReading(" +
            "nameReading TEXT," +
            "kanjiId INTEGER," +
            "FOREIGN KEY (kanjiId) REFERENCES Kanji(rowid))",
            connection);
            rows = command.ExecuteNonQuery();

            //KanjiSet
            command = new SQLiteCommand("CREATE TABLE IF NOT EXISTS KanjiSet(" +
            "kanjiSetName TEXT)",
            connection);
            rows = command.ExecuteNonQuery();

            //KanjiInSet
            command = new SQLiteCommand("CREATE TABLE IF NOT EXISTS KanjiInSet(" +
            "kanjiId INTEGER,"+
            "kanjiSetId INTEGER,"+
            "FOREIGN KEY (kanjiId) REFERENCES Kanji(rowid)"+
            "FOREIGN KEY (kanjiSetId) REFERENCES KanjiSet(rowid))",
            connection);
            rows = command.ExecuteNonQuery();

            connection.Close();
        }
        public static void AddKanji(KanjiListModel list)
        {
            //Set up Sqlite
            SQLiteConnection sql = new SQLiteConnection("Data Source=Kanji.db");
            SQLiteCommand cmd;
            sql.Open();

            for (int i = 0;i<list.Kanji.Count;i++)
            {
                //Insert new kanji
                cmd = new SQLiteCommand(@"INSERT INTO Kanji (character,grade,jlpt,strokeCount,unicode,heisigEnglish)" +
                    "VALUES(" +
                    "@Character," +
                    "@Grade," +
                    "@JLPT," +
                    "@StrokeCount," +
                    "@Unicode," +
                    "@Heisig" +
                    ")", sql);
                cmd.Parameters.AddWithValue("@Character",list.Kanji[i].Character);
                cmd.Parameters.AddWithValue("@Grade",list.Kanji[i].Grade);
                cmd.Parameters.AddWithValue("@JLPT",list.Kanji[i].JLPT);
                cmd.Parameters.AddWithValue("@StrokeCount",list.Kanji[i].StrokeCount);
                cmd.Parameters.AddWithValue("@Unicode",list.Kanji[i].Unicode);
                cmd.Parameters.AddWithValue("@Heisig",list.Kanji[i].HeisigEnglish);
                cmd.ExecuteNonQuery();
            }

            sql.Close();
        }
        public static void AddMeanings(KanjiListModel kl)
        {
            //Set up Sqlite
            SQLiteConnection sql = new SQLiteConnection("Data Source=Kanji.db");
            SQLiteCommand cmd;
            sql.Open();

            for (int i = 0;i<kl.Kanji.Count;i++)
            {
                cmd = new SQLiteCommand("SELECT rowid FROM Kanji WHERE character  = '" + kl.Kanji[i].Character + "';", sql);
                int rowid = Int32.Parse(cmd.ExecuteScalar().ToString());
                for (int j = 0; j < kl.Kanji[i].Meanings.Count;j++)
                {
                    cmd = new SQLiteCommand("INSERT INTO KanjiMeaning (meaning,kanjiId)" +
                        "VALUES(" +
                        "@Meaning," +
                        "@Rowid"+
                        ")", sql);
                    cmd.Parameters.AddWithValue("@Meaning", kl.Kanji[i].Meanings[j]);
                    cmd.Parameters.AddWithValue("@Rowid", rowid);
                    cmd.ExecuteNonQuery();
                }
            }

            sql.Close();
        }

        public static void AddReadings(KanjiListModel kl)
        {
            //Set up Sqlite
            SQLiteConnection sql = new SQLiteConnection("Data Source=Kanji.db");
            SQLiteCommand cmd;
            sql.Open();

            for (int i = 0; i < kl.Kanji.Count; i++)
            {
                cmd = new SQLiteCommand("SELECT rowid FROM Kanji WHERE character  = '" + kl.Kanji[i].Character + "';", sql);
                int rowid = Int32.Parse(cmd.ExecuteScalar().ToString());
                
                //Kun Readings
                for (int j = 0; j < kl.Kanji[i].KunReadings.Count; j++)
                {
                    cmd = new SQLiteCommand("INSERT INTO KanjiKunReading (kunReading,kanjiId)" +
                        "VALUES(" +
                        "@KunReading," +
                        "@Rowid" +
                        ")", sql);
                    cmd.Parameters.AddWithValue("@KunReading", kl.Kanji[i].KunReadings[j]);
                    cmd.Parameters.AddWithValue("@Rowid", rowid);
                    cmd.ExecuteNonQuery();
                }

                //On Readings
                for (int j = 0; j < kl.Kanji[i].OnReadings.Count; j++)
                {
                    cmd = new SQLiteCommand("INSERT INTO KanjiOnReading (onReading,kanjiId)" +
                        "VALUES(" +
                        "@OnReading," +
                        "@Rowid" +
                        ")", sql);
                    cmd.Parameters.AddWithValue("@OnReading", kl.Kanji[i].OnReadings[j]);
                    cmd.Parameters.AddWithValue("@Rowid", rowid);
                    cmd.ExecuteNonQuery();
                }

                //Name Readings
                for (int j = 0; j < kl.Kanji[i].NameReadings.Count; j++)
                {
                    cmd = new SQLiteCommand("INSERT INTO KanjiNameReading (nameReading,kanjiId)" +
                        "VALUES(" +
                        "@NameReading," +
                        "@Rowid" +
                        ")", sql);
                    cmd.Parameters.AddWithValue("@NameReading", kl.Kanji[i].NameReadings[j]);
                    cmd.Parameters.AddWithValue("@Rowid", rowid);
                    cmd.ExecuteNonQuery();
                }
            }
            sql.Close();
        }

        public static void BuildDatabase()
        {
            //Create Database File
            FileUtility.CreateDataFile("Kanji");

            //Create Tables
            DatabaseBuilder.AddTables("Kanji");

            for (int i = 1; i < 7; i++)
            {
                KanjiListModel list = FileUtility.GetKanjiListModelFromXML("grade-"+i);


                //Set up grade 1 set
                DatabaseBuilder.CreateSet("grade-" + i, "Kanji");
                //Add those kanji to the database
                DatabaseBuilder.AddKanji(list);
                //Add the meanings to the database
                DatabaseBuilder.AddMeanings(list);
                //Add the readings to the database
                DatabaseBuilder.AddReadings(list);
                //Add those Kanji to the correct grade set
                DatabaseBuilder.AddKanjiToGradeSet(list, i);
            }
        }
    }
}
