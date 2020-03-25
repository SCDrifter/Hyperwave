using System;
using System.Collections.Generic;
using System.Text;

namespace CommandTree
{
    public class ConsoleTable
    {

        class Row
        {
            public string[][] Text { get; set; }
            public int MaxLines { get; set; }
        }
        int[] mColumns;
        int mColumnSpacing;
        string[] mTitle = null;

        List<Row> mRows = new List<Row>();

        public ConsoleTable(int column_count, int column_spacing)
        {
            mColumnSpacing = column_spacing;
            mColumns = new int[column_count];
            Reset();
        }

        public void Reset()
        {
            mRows.Clear();
            for (int i = 0; i < mColumns.Length; i++)
            {
                mColumns[i] = mColumnSpacing;
            }
        }

        public void SetTitle(params string[] row)
        {
            if (row.Length != mColumns.Length)
                throw new ArgumentOutOfRangeException("Row count incorrect");

            for (int i = 0; i < mColumns.Length; i++)
            {
                string title = row[i];
                if (title.Contains("\n"))
                    throw new InvalidOperationException("Title must not contain newlines");

                int column_size = title.Length + mColumnSpacing;
                mColumns[i] = Math.Max(mColumns[i], column_size);
            }

            mTitle = row;
        }

        public void AddRow(params string[] row)
        {
            if (row.Length != mColumns.Length)
                throw new ArgumentOutOfRangeException("Row count incorrect");
            string[][] row_text = new string[row.Length][];
            Row finished_row = new Row()
            {
                Text = row_text,
                MaxLines = 1
            };

            for (int i = 0; i < mColumns.Length; i++)
            {
                string text = row[i].Replace("\r\n", "\n");
                row_text[i] = text.Split('\n');
                finished_row.MaxLines = Math.Max(finished_row.MaxLines, row_text[i].Length);
                for(int j = 0;j < row_text[i].Length;j++)
                {
                    string line = row_text[i][j];
                    int column_size = line.Length + mColumnSpacing;
                    mColumns[i] = Math.Max(mColumns[i], column_size);
                }
            }

            mRows.Add(finished_row);
        }

        public void Sort(int column)
        {
            mRows.Sort((a, b) => string.Compare(string.Join("\r\n", a.Text[column]), string.Join("\r\n", b.Text[column])));
        }

        public void Print()
        {
            StringBuilder line = new StringBuilder();

            if(mTitle != null)
            {
                for(int i = 0;i < mTitle.Length;i++)
                {
                    var text = mTitle[i];
                    line.Append(text);
                    line.Append(new string(' ', mColumns[i] - text.Length));
                }
                line.AppendLine();

                for (int i = 0; i < mTitle.Length; i++)
                {
                    line.Append(new string('=', mColumns[i]));
                }
                Console.WriteLine(line.ToString());
            }

            foreach(var row in mRows)
            {                
                for (int i = 0; i < row.MaxLines; i++)
                {
                    line.Clear();
                    for (int j = 0;j < row.Text.Length;j++)
                    {
                        string text;

                        var col = row.Text[j];

                        if (col.Length <= i)
                            text = "";
                        else
                            text = col[i];
                        
                        line.Append(text);
                        line.Append(new string(' ', mColumns[j] - text.Length));
                    }
                    Console.WriteLine(line.ToString());
                }
               
            }
        }
    }
}
