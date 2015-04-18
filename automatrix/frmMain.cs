using IronPython.Hosting;
using IronPython.Runtime.Exceptions;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace automatrix
{
    public partial class frmMain : Form
    {

        ScriptEngine engine;
        
        ScriptScope scope;

        Browser b;

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {

            this.engine = Python.CreateEngine();
            
            this.engine.SetTrace(OnTraceback);
            this.scope = engine.CreateScope();

            this.b = new Browser(this.IEWindow);
            
            this.scope.SetVariable("browser", b);

        }

        private TracebackDelegate OnTraceback(TraceBackFrame frame, string result, object payload)
        {

            return this.OnTraceback;

        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {

            
            string code;
            if(String.IsNullOrEmpty(this.rtePythonSource.SelectedText))
            {
                code = this.rtePythonSource.Text;
            }
            else
            {
                code = this.rtePythonSource.SelectedText;
            }

            ScriptSource source = engine.CreateScriptSourceFromString(code,
               SourceCodeKind.Statements);
            try
            {
                source.Execute(scope);
            }
            catch (Exception ex)
            {
                this.lblStatus.Text = ex.Message;
            }
        }


        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (this.dialogOpen.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.rtePythonSource.Text = System.IO.File.ReadAllText(this.dialogOpen.FileName);
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.rtePythonSource.Clear();
            this.rtePythonSource.ClearUndo();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Application.Exit();
        
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(this.dialogSave.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.File.WriteAllText(this.dialogSave.FileName, this.rtePythonSource.Text);

            }
        }

        private void toolStripTextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ToolStripTextBox tb = (ToolStripTextBox)sender;
                if (!string.IsNullOrEmpty(tb.Text))
                {
                    try
                    {
                        if (this.b.screenshot(tb.Text))
                        {
                            this.toolStripStatusLabel1.Text = string.Format("Screenshot:'{0}' taken", tb.Text);
                        }
                    }
                    catch (Exception ex)
                    {
                        this.toolStripStatusLabel1.Text = "Error:" + ex.Message;
                    }
                }
            }
        }



    }
}
