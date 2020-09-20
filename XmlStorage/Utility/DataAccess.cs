using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;
using XmlStorage.Models;

namespace XmlStorage.Utility
{
    class DataAccess
    {
        public static List<string> GetSetNames()
        {
            SQLiteConnection connection = new SQLiteConnection("Data Source=Kanji.db",true);
            SQLiteCommand command;
            connection.Open();
            command = new SQLiteCommand("SELECT kanjiSetName FROM KanjiSet", connection);
            var r = command.ExecuteReader();
            List<string> setNames = new List<string>();

            while (r.Read())
            {
                setNames.Add(r.GetString(0));
            }
            connection.Close();
            return setNames;
        }
        public static KanjiListModel LoadKanjiFromSet(string name)
        {
            KanjiListModel kl = new KanjiListModel();
            SQLiteConnection connection = new SQLiteConnection("Data Source=Kanji.db", true);
            SQLiteCommand command;
            connection.Open();
            command = new SQLiteCommand(
                "SELECT * " +
                "FROM Kanji " +
                "WHERE rowid in " +
                "( SELECT kanjiId FROM KanjiInSet WHERE kanjiSetId in" +
                "( SELECT rowid FROM KanjiSet WHERE kanjiSetName = @KanjiSetName))", connection);
            command.Parameters.AddWithValue("@KanjiSetName",name);
            var r = command.ExecuteReader();
            while (r.Read())
            {
                string c = r.GetString(0);
                int g = r.GetInt32(1);
                int s = r.GetInt32(2);
                int j = r.GetInt32(3);
                string u = r.GetString(4);
                string h = r.GetString(5);
                kl.Kanji.Add(new KanjiModel(c,g,s,j,u,h));
            }

            for (int i = 0;i<kl.Kanji.Count;i++)
            {
                command = new SQLiteCommand(
                "SELECT meaning "+
                "FROM KanjiMeaning "+
                "WHERE kanjiId = "+
                "("+
                "SELECT rowid "+
                "FROM Kanji "+
                "WHERE character = @Character"+
                ")", connection);
                command.Parameters.AddWithValue("@Character", kl.Kanji[i].Character);
                r = command.ExecuteReader();
                while (r.Read())
                {
                    kl.Kanji[i].Meanings.Add(r.GetString(0));
                }
            }
            connection.Close();
            return kl;
        }
    }
}
