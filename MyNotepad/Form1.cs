using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Printing;

namespace MyNotepad
{
    public partial class MyNotepad : Form
    {
        public MyNotepad()
        {
            InitializeComponent();           
        }

        private string _Filename;

        public string Filename
        {
            get
            {
                return _Filename;
            }
            set
            {
                var oldvalue = value;
                _Filename = value;
                OnFilenameChanged(oldvalue, value);
            }
        }

        private void OnFilenameChanged(string oldvalue, string value)
        {
            OnDocumentNameChanged();
        }

        private void OnDocumentNameChanged()
        {
            UpdateTitle();           
        
        }

        private void UpdateTitle()
        {
            if(this.Tag == null)
            {
                this.Tag = base.Text;
            }

            base.Text = ((string)this.Tag).FormatUsingObject(new { DocumentName } );
           
        }

        public string DocumentName
        {
            get
            {
                if (Filename == null) return "Untitled";
                return Path.GetFileName(Filename);
            }
        }



        //File.New
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            New();
        }
        private bool New()
        {
            if (!EnsureWorkNotLost()) return false;

            Filename = null;
            Content = "";
            IsDirty = false;
            _encoding = Encoding.ASCII;

            return true;
        }

        //File.Open
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EnsureWorkNotLost()) return;

            var OpenDialog = new SaveOpenDialog();
            OpenDialog.FileDlgDefaultExt = ".txt";
            OpenDialog.FileDlgFileName = Filename;
            OpenDialog.FileDlgFilter = "Text Documents (*.txt)|*.txt|All Files (*.*)|*.*";
            OpenDialog.FileDlgType = Win32Types.FileDialogType.OpenFileDlg;
            OpenDialog.FileDlgCaption = "Open";
            OpenDialog.FileDlgOkCaption = "Open";

            if (OpenDialog.ShowDialog(this) != DialogResult.OK) return;

            Open(OpenDialog.MSDialog.FileName, OpenDialog.Encoding);
        }

        private bool EnsureWorkNotLost()
        {
            if (!IsDirty) return true;

            var DialogResult = new SaveChangesPrompt(Filename).ShowDialog(this);

            switch (DialogResult)
            {
                case DialogResult.Yes:
                    return Save();
                case DialogResult.No:
                    return true;
                case DialogResult.Cancel:
                    return false;
                default:
                    throw new Exception();
            }
        }

        public void Open(string pFilename, Encoding encoding = null)
        {
            var Filename = pFilename;

            if (!File.Exists(Filename))
            {
                var FileExists = false;

                var Extension = Path.GetExtension(Filename);
                if (Extension == "")
                {
                    Filename = Filename + ".txt";
                    FileExists = File.Exists(Filename);
                }

                if (!FileExists)
                {
                    #region Message

                    var Message = @"Cannot find the {Filename} file.

Do you want to create a new file?
".FormatUsingObject(new { Filename = Filename });

                    #endregion

                    var Result = MessageBox.Show(Message, "MyNotepad", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                    switch (Result)
                    {
                        case DialogResult.Yes:
                            File.WriteAllText(Filename, "");
                            break;
                        case DialogResult.No:
                        case DialogResult.Cancel:
                            return;
                        default:
                            throw new Exception();
                    }
                }
            }

            #region Determine Encoding

            if (encoding == null)
            { // generally this means it was not opened by a user using the open file dialog
                using (var streamReader = new StreamReader(Filename, detectEncodingFromByteOrderMarks: true))
                {
                    var text = streamReader.ReadToEnd();
                    _encoding = streamReader.CurrentEncoding;
                }
            }

            #endregion

            Content = ReadAllText(Filename, encoding);
            SelectionStart = 0;
            this.Filename = Filename;
            IsDirty = false;
        }
        public int SelectionStart
        {
            get { return richTextBox1.SelectionStart; }
            set
            {
                richTextBox1.SelectionStart = value;
                richTextBox1.ScrollToCaret();
            }
        }

        private static string ReadAllText(string pFilename, Encoding encoding)
        {
            using (var FileStream = new FileStream(pFilename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                if (encoding == null)
                {
                    using (var StreamReader = new StreamReader(FileStream))
                    {
                        var text = StreamReader.ReadToEnd();
                        return text;
                    }
                }
                else
                {
                    using (var StreamReader = new StreamReader(FileStream, encoding, false))
                    {
                        var text = StreamReader.ReadToEnd();
                        return text;
                    }
                }
            }
        }
        //File.Save As
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAs();
        }

        //Format.Font
        private void fontToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();
            fd.Font = richTextBox1.SelectionFont;
            if (fd.ShowDialog() == DialogResult.OK)
                richTextBox1.SelectionFont = fd.Font;
        }

       
        //File.Exit
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        //Edit.Cut
        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Cut();
        }

        //Edit.Copy
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Copy();
        }

        //Edit.Paste
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Paste();
        }

        //Edit.Undo
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Undo();
        }

        //Edit.Redo
        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Redo();
        }

        //Help.About
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Created by Group:\n    w41k3r\nMembers:\n  Abneet Wats\n   Krishna Ahir\n  Kinjal Bhrambhatt");
            new AboutBox1().ShowDialog(this);
        }

  
        private bool Save()
        {
            if (!IsDirty) return true;

            if ((Filename == null) || new FileInfo(Filename).IsReadOnly)
            {
                return SaveAs();
            }

            File.WriteAllText(Filename, Content);
            IsDirty = false;
            return true;
        }
            
        //File.Save
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save();
        }
        private Encoding _encoding = Encoding.ASCII;

        private bool SaveAs()
        {
            var SaveDialog = new SaveOpenDialog();
            SaveDialog.FileDlgFileName = Filename;
            SaveDialog.FileDlgDefaultExt = ".txt";
            SaveDialog.FileDlgFilter = "Text Documents (*.txt)|*.txt|All Files (*.*)|*.*";
            SaveDialog.Encoding = _encoding;
            SaveDialog.FileDlgCaption = "Save";
            SaveDialog.FileDlgOkCaption = "Save";

            if (SaveDialog.ShowDialog(this) != DialogResult.OK) return false;

            var PotentialFilename = SaveDialog.MSDialog.FileName;

            _encoding = SaveDialog.Encoding;
            File.WriteAllText(PotentialFilename, Content, _encoding);

            Filename = PotentialFilename;
            IsDirty = false;

            return true;
        }

        //File.Page Setup
        private void pageSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var PageSetupDialog = new PageSetupDialog();
            PageSetupDialog.PageSettings = PageSettings;
            if (PageSetupDialog.ShowDialog(this) != DialogResult.OK) return;
            PageSettings = PageSetupDialog.PageSettings;
        }

        private PageSettings _PageSettings;
        private PageSettings PageSettings {
            get {
                if (_PageSettings == null) {
                    if (Settings.MoreSettings.PageSettings != null) {
                        _PageSettings = Settings.MoreSettings.PageSettings;
                    } else {
                        var PageSettings = new PageSettings() {
                            Margins = new Margins(75, 75, 100, 100), // 100 = 1 inch
                        };

                        _PageSettings = PageSettings;
                    }
                }

                return _PageSettings;
            }
            set {
                Settings.MoreSettings.PageSettings = value;
                Settings.Save();
            }
        
        }

        //File.Print
        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var PrintDialog = new PrintDialog();

            if (Settings.MoreSettings.PrinterSettings != null) {
                PrintDialog.PrinterSettings = Settings.MoreSettings.PrinterSettings;
            }

            if (PrintDialog.ShowDialog(this) != DialogResult.OK) return;
            Settings.MoreSettings.PrinterSettings = PrintDialog.PrinterSettings;
            Settings.Save();

            var PrintDocument = new PrintDocument();
            PrintDocument.DefaultPageSettings = PageSettings;
            PrintDocument.PrinterSettings = Settings.MoreSettings.PrinterSettings;
            PrintDocument.DocumentName = DocumentName + "MyNotepad - ";

            var RemainingContentToPrint = Content;
            var PageIndex = 0;

            PrintDocument.PrintPage += (sender1, e1) => {
               

                { 
                    var CharactersFitted = 0;
                    var LinesFilled = 0;

                    var MarginBounds = new RectangleF(e1.MarginBounds.X, e1.MarginBounds.Y + /* header */ CurrentFont.Height, e1.MarginBounds.Width, e1.MarginBounds.Height - (/* header and footer */ CurrentFont.Height * 2));

                    e1.Graphics.MeasureString(RemainingContentToPrint, CurrentFont, MarginBounds.Size, StringFormat.GenericTypographic, out CharactersFitted, out LinesFilled);
                    e1.Graphics.DrawString(RemainingContentToPrint, CurrentFont, Brushes.Black, MarginBounds, StringFormat.GenericTypographic);

                    RemainingContentToPrint = RemainingContentToPrint.Substring(CharactersFitted);

                    e1.HasMorePages = (RemainingContentToPrint.Length > 0);
                }

               
                

                PageIndex++;
            };

            PrintDocument.Print();
        
        }

        //Edit.Delete
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectionLength == 0)
            {
                SelectionLength = 1;
            }

            SelectedText = "";
        }

        public int SelectionLength
        {
            get { return richTextBox1.SelectionLength; }
            set { richTextBox1.SelectionLength = value; }
        }

        public string SelectedText
        {
            get { return richTextBox1.SelectedText; }
            set 
            { 
                richTextBox1.SelectedText = value;
                IsDirty = true;
            }
        }


        //Edit.Find

        private string _LastSearchText;
        private bool _LastMatchCase;
        private bool _LastSearchDown;

        public bool FindAndSelect(string pSearchText, bool pMatchCase, bool pSearchDown)
        {
            int Index;

            var eStringComparison = pMatchCase ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase;

            if (pSearchDown)
            {
                Index = Content.IndexOf(pSearchText, SelectionEnd, eStringComparison);
            }
            else
            {
                Index = Content.LastIndexOf(pSearchText, SelectionStart, SelectionStart, eStringComparison);
            }

            if (Index == -1) return false;

            _LastSearchText = pSearchText;
            _LastMatchCase = pMatchCase;
            _LastSearchDown = pSearchDown;

            SelectionStart = Index;
            SelectionLength = pSearchText.Length;

            return true;
        }

        public int SelectionEnd
        {
            get { return SelectionStart + SelectionLength; }
        }

        private FindDialog _FindDialog;


        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Find();
        }

        private void Find()
        {
            if (Content.Length == 0) return;

            if (_FindDialog == null)
            {
                _FindDialog = new FindDialog(this);
            }

            _FindDialog.Left = this.Left + 56 ;
            _FindDialog.Top = this.Top + 160 ;

            if (!_FindDialog.Visible)
            {
                _FindDialog.Show(this);
            }
            else
            {
                _FindDialog.Show();
            }

            _FindDialog.Triggered();
        }

        //Edit.Find Next
        private void findNextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_LastSearchText == null)
            {
                Find();
                return;
            }

            if (!FindAndSelect(_LastSearchText, _LastMatchCase, _LastSearchDown))
            {
                MessageBox.Show(this, CONST.CannotFindMessage.FormatUsingObject(new { SearchText = _LastSearchText }), "MyNotepad");
            }
        }

        //Edit.Replace

        private ReplaceDialog _ReplaceDialog;

        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Content.Length == 0) return;

            if (_ReplaceDialog == null)
            {
                _ReplaceDialog = new ReplaceDialog(this);
            }

            _ReplaceDialog.Left = this.Left + 56;
            _ReplaceDialog.Top = this.Top + 113;

            if (!_ReplaceDialog.Visible)
            {
                _ReplaceDialog.Show(this);
            }
            else
            {
                _ReplaceDialog.Show();
            }

            _ReplaceDialog.Triggered();
        }

        //Edit.Go TO
        private void goToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var GoToLinePrompt = new GoToLinePrompt(LineIndex + 1);
            GoToLinePrompt.Left = Left;
            GoToLinePrompt.Top = Top;

            if (GoToLinePrompt.ShowDialog(this) != DialogResult.OK) return;

            var TargetLineIndex = GoToLinePrompt.LineNumber - 1;

            if (TargetLineIndex > richTextBox1.Lines.Length)
            {
                MessageBox.Show(this, "The line number is beyond the total number of lines", "NotepadClone.Net - Goto Line");
                return;
            }

            LineIndex = TargetLineIndex;
        }

        private int LineIndex
        {
            get
            {
                return CaretPosition.LineIndex;
            }
            set
            {
                var TargetLineIndex = value;

                if (TargetLineIndex < 0)
                {
                    TargetLineIndex = 0;
                }

                if (TargetLineIndex >= richTextBox1.Lines.Length)
                {
                    TargetLineIndex = richTextBox1.Lines.Length - 1;
                }

                var CharIndex = 0;

                for (var CurrentLineIndex = 0; CurrentLineIndex < TargetLineIndex; CurrentLineIndex++)
                {
                    CharIndex += richTextBox1.Lines[CurrentLineIndex].Length + Environment.NewLine.Length;
                }

                SelectionStart = CharIndex;
                richTextBox1.ScrollToCaret(); //navigate to text cursor
            }
        }

        private ContentPosition CharIndexToPosition(int pCharIndex)
        {
            var CurrentCharIndex = 0;

            if (richTextBox1.Lines.Length == 0 && CurrentCharIndex == 0) return new ContentPosition { LineIndex = 0, ColumnIndex = 0 };

            for (var CurrentLineIndex = 0; CurrentLineIndex < richTextBox1.Lines.Length; CurrentLineIndex++)
            {
                var LineStartCharIndex = CurrentCharIndex;
                var Line = richTextBox1.Lines[CurrentLineIndex];
                var LineEndCharIndex = LineStartCharIndex + Line.Length + 1;

                if (pCharIndex >= LineStartCharIndex && pCharIndex <= LineEndCharIndex)
                {
                    var ColumnIndex = pCharIndex - LineStartCharIndex;
                    return new ContentPosition { LineIndex = CurrentLineIndex, ColumnIndex = ColumnIndex };
                }

                CurrentCharIndex += richTextBox1.Lines[CurrentLineIndex].Length + Environment.NewLine.Length;
            }

            return null;
        }

        private class ContentPosition
        {
            public int LineIndex;
            public int ColumnIndex;
        }

        private ContentPosition CaretPosition
        {
            get
            {
                return CharIndexToPosition(SelectionStart);
            }
        }


        //Edit.Select All
        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();
        }

        //Edit.Date/Time
        private void timeDateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedText = DateTime.Now.ToShortDateString() +" " +DateTime.Now.ToShortTimeString();
        }

        //Format.Wrap Text
        public bool WordWrap
        {
            get
            {
                return richTextBox1.WordWrap;
            }
            set
            {
                wordWrapToolStripMenuItem.Checked = richTextBox1.WordWrap = value;
            }
        }

        private void wordWrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WordWrap = !WordWrap;
        }

        private void wordWrapToolStripMenuItem_CheckedChanged_1(object sender, EventArgs e)
        {
            var Sender = (ToolStripMenuItem)sender;
            WordWrap = Sender.Checked;
        }

        

        //View.Status Bar
        private void statusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IsStatusBarVisible = !IsStatusBarVisible;
        }
        public bool IsStatusBarVisible
        {
            get
            {
                return Settings.IsStatusBarVisible;
            }
            set
            {
                statusBarToolStripMenuItem.Checked = statusStrip1.Visible = Settings.IsStatusBarVisible = value;
                Settings.Save();
            }
        }

        

        private void UpdateStatusBar()
        {
            if (controlCaretPositionLabel.Tag == null)
            {
                controlCaretPositionLabel.Tag = controlCaretPositionLabel.Text;
            }

            controlCaretPositionLabel.Text = ((string)controlCaretPositionLabel.Tag).FormatUsingObject(new
            {
                LineNumber = CaretPosition.LineIndex + 1,
                ColumnNumber = CaretPosition.ColumnIndex + 1,
            });
        }

        //Theme.Dark
        private void darkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            menuStrip1.BackColor = Color.FromArgb(50, 53, 58);
            menuStrip1.ForeColor = Color.FromArgb(255, 255, 255);
            
            richTextBox1.BackColor = Color.FromArgb(50, 53, 58);
            richTextBox1.ForeColor = Color.FromArgb(255, 255, 255);

        }

        //Theme.Light
        private void lightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            menuStrip1.BackColor = Color.FromArgb(255, 255, 255);
            menuStrip1.ForeColor = Color.FromArgb(0, 0 , 0);

            richTextBox1.BackColor = Color.FromArgb(255, 255, 255);
            richTextBox1.ForeColor = Color.FromArgb(0, 0, 0);
        }

        //Theme.Custom          


        public Color _MenuBgColor
        {
            get
            {
                return menuStrip1.BackColor;
            }
            set
            {
                menuStrip1.BackColor = value;
            }
        }

        public Color _MenuForeColor
        {
            get
            {
                return menuStrip1.ForeColor;
            }
            set
            {
                menuStrip1.ForeColor = value;
            }
        }

        public Color _TextBoxBgColor
        {
            get
            {
                return richTextBox1.BackColor;
            }
            set
            {
                richTextBox1.BackColor = value;
            }
        }

        public Color _TextBoxForeColor
        {
            get
            {
                return richTextBox1.ForeColor;
            }
            set
            {
                richTextBox1.ForeColor = value;
            }
        }


        private CustomTheme _CustomTheme;

        private void customToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _CustomTheme = new CustomTheme(this);
            _CustomTheme.ShowDialog();
            


        }

        //Theme.Default
        private void defaultToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

       

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            IsDirty = true;
        }

        private void MyNotepad_Load(object sender, EventArgs e)
        {
            UpdateTitle();
            wordWrapToolStripMenuItem.Checked = richTextBox1.WordWrap;
            CurrentFont = Settings.CurrentFont;
            UpdateStatusBar();
            statusBarToolStripMenuItem.Checked = statusStrip1.Visible = Settings.IsStatusBarVisible;
            richTextBox1.BringToFront();
        }

        public bool IsStatusVisible
        {
            get
            {
                return Settings.IsStatusBarVisible;
            }
            set
            {
                statusBarToolStripMenuItem.Checked = statusStrip1.Visible = Settings.IsStatusBarVisible = value;
                Settings.Save();
            }
        }

        private Font CurrentFont
        {
            get
            {
                return Settings.CurrentFont;
            }
            set
            {
                richTextBox1.Font = Settings.CurrentFont = value;
                Settings.Save();
            }
        }

        private static Properties.Settings Settings
        {
            get
            {
                return Properties.Settings.Default;
            }
        }

        private bool _IsDirty;
        public bool IsDirty
        {
            get
            {
                if (Filename == null && Content.IsEmpty()) return false;
                return _IsDirty;
            }
            set
            {
                _IsDirty = value;
            }
        }

        public string Content
        {
            get { return richTextBox1.Text; }
            set { richTextBox1.Text = value; }
        }

        private void richTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            UpdateStatusBar();
        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            UpdateStatusBar();
        }

        private void richTextBox1_MouseDown(object sender, MouseEventArgs e)
        {
            UpdateStatusBar();
        }


        private void MyNotepad_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !EnsureWorkNotLost();
        }

        private void MyNotepad_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Settings.WindowPosition = Bounds;
            Settings.Save();
        }

        
       

       

    }
}
