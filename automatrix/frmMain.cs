using IronPython.Hosting;
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
            this.scope = engine.CreateScope();

            this.b = new Browser(this.IEWindow);
            
            this.scope.SetVariable("browser", b);

        }


        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {

            string code = this.rtePythonSource.Text;
            ScriptSource source = engine.CreateScriptSourceFromString(code,
               SourceCodeKind.Statements);
            try
            {
                Task t = new Task(() => source.Execute(scope));
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



    }
}
