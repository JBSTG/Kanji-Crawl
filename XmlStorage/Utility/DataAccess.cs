using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.IO;
using System.Diagnostics;
using System.Data.SqlClient;

namespace XmlStorage.Utility
{
    class DataAccess
    {
        public static void AddKanjiToGradeSet(int grade)
        {
            SQLiteConnection connection = new SQLiteConnection("Data Source=Kanji.db");
            SQLiteCommand command;
            connection.Open();
            command = new SQLiteCommand("SELECT rowid FROM KanjiSet WHERE kanjiSetName='grade-"+grade+"'", connection);
            int rowid = Int32.Parse(command.ExecuteScalar().ToString());

            command = new SQLiteCommand("INSERT INTO KanjiInSet(kanjiSetId,kanjiId)" +
                "VALUES ("+rowid+","+
                "(SELECT rowid " +
                "FROM Kanji " +
                "WHERE grade="+rowid+"))",connection);
            command.ExecuteNonQuery();
            connection.Close();
        }
        public static void CreateSet(string setName,string dbname)
        {
            SQLiteConnection connection = new SQLiteConnection("Data Source=" + dbname + ".db");
            SQLiteCommand command;
            connection.Open();
            command = new SQLiteCommand("INSERT INTO KanjiSet (kanjiSetName) VALUES('"+setName+"')", connection);
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
        public static void AddKanji(string character,int grade)
        {
            //Set up Sqlite
            SQLiteConnection sql = new SQLiteConnection("Data Source=Kanji.db");
            SQLiteCommand cmd;
            sql.Open();
            //Insert new kanji
            cmd = new SQLiteCommand("INSERT INTO Kanji (character,grade)" +
                "VALUES(" +
                "'"+character+"',"+
                grade+
                ")", sql);
            cmd.ExecuteNonQuery();
            sql.Close();
        }
        public static void AddMeanings(string character,List<String> meanings)
        {
            //Set up Sqlite
            SQLiteConnection sql = new SQLiteConnection("Data Source=Kanji.db");
            SQLiteCommand cmd;
            sql.Open();

            cmd = new SQLiteCommand("SELECT rowid FROM Kanji WHERE character  = '" + character + "';",sql);
            int rowid = Int32.Parse(cmd.ExecuteScalar().ToString());
            for (int i = 0;i<meanings.Count;i++)
            {
                cmd = new SQLiteCommand("INSERT INTO KanjiMeaning (meaning,kanjiId)" +
                "VALUES(" +
                "'" + meanings[i] + "'," +
                rowid +
                ")", sql);
                cmd.ExecuteNonQuery();
            }
            
            sql.Close();
        }
    }
}
