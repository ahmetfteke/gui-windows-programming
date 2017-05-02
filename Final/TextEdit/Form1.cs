using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace TextEdit
{
    public partial class Form1 : Form
    {
        int current_line = 0;
        int files_edited = 0;
        string path = "";
        public enum ScrollBarType : uint
        {
            SbHorz = 0,
            SbVert = 1,
            SbCtl = 2,
            SbBoth = 3
        }

        public enum Message : uint
        {
            WM_VSCROLL = 0x0115
        }

        public enum ScrollBarCommands : uint
        {
            SB_THUMBPOSITION = 4
        }
        [DllImport("User32.dll")]
        public extern static int GetScrollPos(IntPtr hWnd, int nBar);

        [DllImport("User32.dll")]
        public extern static int SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
        public Form1()
        {
            InitializeComponent();
        }
        private void save()
        {
            if (path == "")
            {
                save_as();
            }
            else
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(path);
                sw.Write(richTextBox1.Text);
                sw.Close();
            }
            files_edited += 1;
            toolStripLabel8.Text = "Files Edited: " + files_edited.ToString();

        }
        private void save_as()
        {
            SaveFileDialog svf = new SaveFileDialog();
            svf.Title = "Save a file..";
            if (svf.ShowDialog() == DialogResult.OK)
            {
                path = svf.FileName;
                System.IO.StreamWriter sw = new System.IO.StreamWriter(path);
                sw.Write(richTextBox1.Text);
                sw.Close();
            }
            files_edited += 1;
            toolStripLabel8.Text = "Files Edited: " + files_edited.ToString();

        }
        private void save_as_file_name(string filename)
        {
            if (path == "")
                path = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\" + filename;
            else
            {
                string[] files = path.Split('\\');
                path = "";
                for (int i = 0; i < files.Length - 1; i++)
                {
                    path += files.GetValue(i) + "\\";
                }
                path += "\\" + filename;
            }
            MessageBox.Show("Saved: " + path);
            System.IO.StreamWriter sw = new System.IO.StreamWriter(path);
            sw.Write(richTextBox1.Text);
            sw.Close();
            files_edited += 1;
            toolStripLabel8.Text = "Files Edited: " + files_edited.ToString();
        }
        private void find()
        {
            Form2 form2 = new Form2();
            if(form2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (form2.find_text != "" && form2.replace_text == "")
                {
                    int s_start = richTextBox1.SelectionStart, startIndex = 0, index;
                    string word = form2.find_text;
                    while ((index = richTextBox1.Text.IndexOf(word, startIndex)) != -1)
                    {
                        richTextBox1.Select(index, word.Length);
                        richTextBox1.SelectionColor = Color.IndianRed;

                        startIndex = index + word.Length;
                    }

                    richTextBox1.SelectionStart = s_start;
                    richTextBox1.SelectionLength = 0;
                    richTextBox1.SelectionColor = Color.Black;
                }
                else if (form2.find_text != "" && form2.replace_text != "")
                {

                    int number_of_lines = form2.number_of_lines;
                    int starting_column = form2.starting_column;

                    int until_now_char = number_of_lines;
                    int k = 0;
                    foreach (string line in richTextBox1.Lines)
                    {
                        if (k >= number_of_lines)
                            break;
                        until_now_char += line.Length;
                        k += 1;
                    }

                    string replacement = form2.replace_text;
                    string word = form2.find_text;
                    int i = 0;
                    int n = 0;
                    int a = replacement.Length - word.Length;
                    foreach (Match m in Regex.Matches(richTextBox1.Text, word))
                    {
                        if (m.Index + i < until_now_char)
                            continue;
                        richTextBox1.Select(m.Index + i, word.Length);
                        i += a;
                        richTextBox1.SelectedText = replacement;
                        n++;
                    }
                }
            }
            
        }
        private void open()
        {
            richTextBox1.Clear();
            richTextBox2.Clear();
            List<string> lines = new List<string>();
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Open a file..";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                path = ofd.FileName;
                System.IO.StreamReader sr = new System.IO.StreamReader(ofd.FileName);
                richTextBox1.Text = sr.ReadToEnd();
                int i = richTextBox1.SelectionStart;
                toolStripLabel2.Text = "Current Position: " + i.ToString();
                i = richTextBox1.Lines.Count();
                toolStripLabel3.Text = "Total Line: " + i.ToString();
                toolStripLabel1.Text = "File Name: " + ofd.FileName;
                System.IO.FileInfo fi = new System.IO.FileInfo(ofd.FileName);
                long size = fi.Length;

                toolStripLabel6.Text = "File Size: " + size.ToString();
                sr.Close();
            }
            for (int i = 0; i <= richTextBox1.Lines.Count(); i++)
                if (!richTextBox2.Text.Contains(i.ToString()))
                    richTextBox2.Text +=  i.ToString() + "===\n";

            

        }
        private int countColumn()
        {
            // Get the line.
            int index = richTextBox1.SelectionStart;
            int line = richTextBox1.GetLineFromCharIndex(index);

            // Get the column.
            int firstChar = richTextBox1.GetFirstCharIndexFromLine(line);
            return index - firstChar;
        }

        private void changeLine(RichTextBox RTB, int line, string text)
        {
            int s1 = RTB.GetFirstCharIndexFromLine(line);
            int s2 = line < RTB.Lines.Count() - 1 ?
                      RTB.GetFirstCharIndexFromLine(line + 1) - 1 :
                      RTB.Text.Length;
            if (s1 == -1)
                s1 = 0;
            RTB.Select(s1, s2 - s1);
            RTB.SelectedText = text;
        }
        private void hideUnhideLine(RichTextBox RTB, int line, bool hide)
        {
            if (hide)
            {
                int s1 = RTB.GetFirstCharIndexFromLine(line);
                int s2 = line < RTB.Lines.Count() - 1 ?
                          RTB.GetFirstCharIndexFromLine(line + 1) - 1 :
                          RTB.Text.Length;
                RTB.Select(s1, s2 - s1);
                RTB.SelectionColor = Color.White;
            }
            else
            {
                int s1 = RTB.GetFirstCharIndexFromLine(line);
                int s2 = line < RTB.Lines.Count() - 1 ?
                          RTB.GetFirstCharIndexFromLine(line + 1) - 1 :
                          RTB.Text.Length;
                RTB.Select(s1, s2 - s1);
                RTB.SelectionColor = Color.Black;
            }
        }
        public void MarkSingleLine(int line_number)
        {
            int i = 0;
            string[] temp = richTextBox1.Lines;
            richTextBox1.Clear();
            foreach (string line in temp)
            {
                if (i == line_number)
                    richTextBox1.SelectionColor = Color.Red;
                
                richTextBox1.AppendText(line + "\r\n");
                i++;
            }

          
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            richTextBox2.Clear();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            open();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            save();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Redo();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Paste();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();
        }
        
        private void richTextBox1_keyDown(object sender, KeyEventArgs e)
        {
            for (int i = 0; i <= richTextBox1.Lines.Count(); i++)
            {
                if (!(e.KeyCode == Keys.Back))
                {
                    if (!richTextBox2.Text.Contains(i.ToString()))
                    {
                       
                        richTextBox2.Text += i.ToString() + "===\n";
                    }
                }
                else
                {
                    richTextBox2.Clear();
                }
            }

            int j = richTextBox1.SelectionStart;
            int jline = richTextBox1.GetLineFromCharIndex(j);
            toolStripLabel4.Text = "Cursor Line: " + jline.ToString();
            toolStripLabel5.Text = "Current Column: " + countColumn().ToString();

            j = richTextBox1.SelectionStart;
            toolStripLabel2.Text = "Current Position: " + jline.ToString();
            j = richTextBox1.Lines.Count();
            toolStripLabel3.Text = "Total Line: " + j.ToString();


        }
        private void toGo(int togo)
        {
            if (togo < 0 || togo > richTextBox1.Lines.Count())
                return;
            richTextBox1.SelectionStart = togo;
            richTextBox1.ScrollToCaret();
        }
        private int countChar()
        {
            int i = richTextBox1.SelectionStart;
            int untilLine = richTextBox1.GetLineFromCharIndex(i);
            int line_count = 0;
            int char_count = 0;
            foreach (string line in richTextBox1.Lines) { 
                if (line_count >= untilLine)
                    break;
                char_count += line.Length;
            }
            return char_count;
        }
        private void richTextBox1_Click(object sender, EventArgs e)
        {
            int i = richTextBox1.SelectionStart;
            int line = richTextBox1.GetLineFromCharIndex(i);
            toolStripLabel4.Text = "Cursor Line: " + line.ToString();
            toolStripLabel2.Text = "Current Position: " + line.ToString();
            toolStripLabel5.Text = "Current Column: " + countColumn().ToString();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            save();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            open();
        }

        private void toolStripTextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string input = toolStripTextBox2.Text;
                if (input.StartsWith("save as") || input.StartsWith("Save as"))
                {
                    string filename = input.Replace("Save as ", "").Replace("save as ", "");
                    save_as_file_name(filename);
                }
                else if (input.StartsWith("save") || input.StartsWith("Save"))
                {
                    save();
                }
                else if (input.StartsWith("open") || input.StartsWith("Open") || input.StartsWith("o"))
                {
                    open();
                }
                else if (input == "find" || input == "f" || input == "search" || input == "Search")
                {
                    find();
                }
                else if (input.StartsWith("find") || input.StartsWith("search") || input.StartsWith("Search"))
                {

                    string[] temp = input.Split();
                    string[] words = temp[1].Split('/');

                    string replace_text = words[1];
                    string replacement = words[2];
                    int lines = Int32.Parse(temp[2]);

                    int until_now_char = countChar();

                    int i = 0;
                    int n = 0;
                    int a = replacement.Length - replace_text.Length;
                    foreach (Match m in Regex.Matches(richTextBox1.Text, replace_text))
                    {
                        if (m.Index + i < until_now_char)
                            continue;
                        richTextBox1.Select(m.Index + i, replace_text.Length);
                        i += a;
                        richTextBox1.SelectedText = replacement;
                        n++;
                    }
                }
                else if (input.StartsWith("up") || input.StartsWith("u") || input.StartsWith("Up"))
                {
                    string[] temp = input.Split();
                    int how_many = Int32.Parse(temp[1]);
                    for(int i = 0; i < how_many; i++)
                    { 
                        IntPtr intPtr = unchecked((IntPtr)(long)(ulong)0);
                        SendMessage(richTextBox1.Handle, (uint)0x00B6, intPtr, (IntPtr)(-1));
                    }
                }
                else if (input.StartsWith("down") || input.StartsWith("d") || input.StartsWith("Down"))
                {
                    string[] temp = input.Split();
                    int how_many = Int32.Parse(temp[1]);
                    for (int i = 0; i < how_many; i++)
                    {
                        IntPtr intPtr = unchecked((IntPtr)(long)(ulong)0);
                        SendMessage(richTextBox1.Handle, (uint)0x00B6, intPtr, (IntPtr)(1));
                    }
                }
                else if (input.StartsWith("left") || input.StartsWith("l") || input.StartsWith("Left"))
                {
                    string[] temp = input.Split();
                    int how_many = Int32.Parse(temp[1]);
                    for (int i = 0; i < how_many; i++)
                    {
                        IntPtr intPtr = unchecked((IntPtr)(long)(ulong)0);
                        SendMessage(richTextBox1.Handle, (uint)0x0114, intPtr, (IntPtr)(1));
                    }
                }
                else if (input.StartsWith("right") || input.StartsWith("r") || input.StartsWith("Right"))
                {
                    string[] temp = input.Split();
                    int how_many = Int32.Parse(temp[1]);
                    for (int i = 0; i < how_many; i++)
                    {
                        IntPtr intPtr = unchecked((IntPtr)(long)(ulong)1);
                        SendMessage(richTextBox1.Handle, (uint)0x0114, intPtr, (IntPtr)(0));
                    }
                }
                else if (input.StartsWith("forward") || input.StartsWith("f") || input.StartsWith("Forward"))
                {
                    int how_many = 20;
                    for (int i = 0; i < how_many; i++)
                    {
                        IntPtr intPtr = unchecked((IntPtr)(long)(ulong)0);
                        SendMessage(richTextBox1.Handle, (uint)0x00B6, intPtr, (IntPtr)(1));
                    }
                }
                else if (input.StartsWith("back") || input.StartsWith("b") || input.StartsWith("Back"))
                {
                    int how_many = 20;
                    for (int i = 0; i < how_many; i++)
                    {
                        IntPtr intPtr = unchecked((IntPtr)(long)(ulong)0);
                        SendMessage(richTextBox1.Handle, (uint)0x00B6, intPtr, (IntPtr)(-1));
                    }
                }
                else if (input.StartsWith("setcl"))
                {
                    int i = 0;
                    string[] temp = input.Split();
                    int line_number = Int32.Parse(temp[1]);
                    MarkSingleLine(line_number);
                }
                else if (input.StartsWith("change") || input.StartsWith("c"))
                {
                    string[] temp = input.Split('/');
                    string word = temp[1];
                    string replacement = temp[2];
                    int how_many = Int32.Parse(temp[3]);
                    int i = 0;
                    int n = 0;
                    int a = replacement.Length - word.Length;
                    int cc = countChar();
                    foreach (Match m in Regex.Matches(richTextBox1.Text, word))
                    {
                        if (m.Index + i < cc)
                            continue;
                        richTextBox1.Select(m.Index + i, word.Length);
                        i += a;
                        richTextBox1.SelectedText = replacement;
                        n++;
                    }
                }
                else if (input == "help" || input == "h")
                {
                    Form3 form3 = new Form3();
                    form3.ShowDialog();
                }
                else if (Regex.IsMatch(input, @"^\d+$"))
                {
                    int togo = Int32.Parse(input);
                    toGo(togo);
                }
                toolStripTextBox2.Clear();
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            save_as();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            save_as();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            richTextBox2.Clear();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            find();
        }

        private void richTextBox1_VScroll(object sender, EventArgs e)
        {
            int nPos = GetScrollPos(richTextBox1.Handle, (int)ScrollBarType.SbVert);
            nPos <<= 16;
            uint wParam = (uint)ScrollBarCommands.SB_THUMBPOSITION | (uint)nPos;
            SendMessage(richTextBox2.Handle, (int)Message.WM_VSCROLL, new IntPtr(wParam), new IntPtr(0));
        }

        private void richTextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { 
                int i = 0;
                int temp_num;
                foreach(string line in richTextBox2.Lines)
                {
                    string input = line.Replace(i.ToString() + "===", "");
                    if (Regex.IsMatch(input, @"^\d+$"))
                    {
                        int togo = Int32.Parse(input);
                        toGo(togo);
                    }
                    else if (input.StartsWith("i"))
                    {
                        string number = input.Replace("i", "");
                        changeLine(richTextBox1, i, number + "\r\n" + richTextBox1.Lines[i]);

                    }
                    else if (input.StartsWith("b"))
                    {
                        changeLine(richTextBox1, i, "\r\n" + richTextBox1.Lines[i]);

                    }
                    else if (input.StartsWith("a"))
                    {
                        changeLine(richTextBox1, i, richTextBox1.Lines[i] + "\r\n");

                    }
                    else if (input.StartsWith("x"))
                    {
                        string number = input.Replace("x", "");
                        temp_num = Int32.Parse(number);
                        for (int j = 0; j < temp_num; j++)
                        {
                            hideUnhideLine(richTextBox1, j + i, true);
                        }

                    }
                    else if (input.StartsWith("s"))
                    {
                        string number = input.Replace("s", "");
                        temp_num = Int32.Parse(number);
                        for (int j = 0; j < temp_num; j++)
                        {
                            hideUnhideLine(richTextBox1, j + i, false);
                        }

                    }
                    else if (input.StartsWith("c"))
                    {
                        string number = input.Replace("c", "");
                        temp_num = Int32.Parse(number);
                        string temp = "";
                        for (int k = 0; k < temp_num; k++)
                        {
                            if (temp_num + k >= richTextBox1.Lines.Count())
                                continue;
                            temp += richTextBox1.Lines[temp_num + k] + "\n";

                        }
                        Clipboard.SetText(temp);

                    }
                    else if (input.StartsWith("m"))
                    {
                        string number = input.Replace("m", "");
                        temp_num = Int32.Parse(number);
                        string temp = "";
                        for (int k = 0; k < temp_num; k++)
                        {
                            if (temp_num + k >= richTextBox1.Lines.Count())
                                continue;
                            temp += richTextBox1.Lines[temp_num + k] + "\n";
                            changeLine(richTextBox1, i + k, "");

                        }
                        Clipboard.SetText(temp);

                    }
                    else if (input == ("\""))
                    {

                        changeLine(richTextBox1, i, richTextBox1.Lines[i] + "\r\n" + richTextBox1.Lines[i]);

                    }
                    changeLine(richTextBox2, i, i.ToString() + "===");
                    i += 1;
                }

            }


        }

        private void commandsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.ShowDialog();
        }
    }
}
