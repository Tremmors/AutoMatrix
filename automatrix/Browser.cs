using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace automatrix
{
    /// <summary>
    ///     A browser object for the python code to interact with
    /// </summary>
    public class Browser
    {

        public int Increment;

        public bool loading = false;
        /// <summary>
        ///     The IEWindow on the form.
        /// </summary>
        public WebBrowser IEWindow = null;

        /// <summary>
        ///     Instanciate a new Browser object
        /// </summary>
        /// <param name="_control">
        ///     The WebBrowser control to interact with
        /// </param>
        public Browser(WebBrowser _control) 
        {

            Console.WriteLine("Instanciated browser");
            this.IEWindow = _control;
            this.IEWindow.DocumentCompleted += IEWindow_DocumentCompleted;
            this.IEWindow.Navigating += IEWindow_Navigating;
            this.IEWindow.ProgressChanged += IEWindow_ProgressChanged;
            this.Increment = 0;

        }

        void IEWindow_ProgressChanged(object sender, WebBrowserProgressChangedEventArgs e)
        {



        }

        void IEWindow_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {



        }

        void IEWindow_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {



        }

 

        /// <summary>
        ///     Wait for the page to load.
        /// </summary>
        public void wait()
        {
            this.IEWindow.InvokeIfRequired( 
                () => {
                    while (this.IEWindow.ReadyState != WebBrowserReadyState.Complete)
                    {

                        Application.DoEvents();

                    }
                } 
            );
        }

        /// <summary>
        ///     Open a webpage.
        /// </summary>
        /// <param name="url">
        ///     URL to open
        /// </param>
        public void open(string url)
        {

            this.IEWindow.InvokeIfRequired(() => {
                this.IEWindow.Navigate(url);
            
            });
            
        }

        public void screenshot()
        {
            this.screenshot(string.Format("{0}.jpg",this.Increment++.ToString()));
        }

        /// <summary>
        ///     Take a screenshot of the entire webpage
        /// </summary>
        /// <param name="filename">
        ///     The filename to save the image as
        /// </param>
        /// <returns>
        ///     True if the screenshot could be captured
        /// </returns>
        public bool screenshot(string filename)
        {
            Bitmap bmp = LoadBitMapFromScreen();
            if (bmp != null)
            {
                bmp.Save(filename);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        ///     Copy the entire contents of the webpage to a BitMap image
        /// </summary>
        /// <returns>
        ///     A BitMap object
        /// </returns>
        private Bitmap LoadBitMapFromScreen()
        {
            Bitmap bitmap = null;
            this.IEWindow.InvokeIfRequired(() => { 
                Rectangle WindowRect = this.IEWindow.RectangleToScreen(this.IEWindow.Document.Body.ClientRectangle);
                int imgWidth = this.IEWindow.Document.Body.ScrollRectangle.Width;
                int imgHeight = this.IEWindow.Document.Body.ScrollRectangle.Height;

                Console.WriteLine("size of image: {0},{1}", imgWidth, imgHeight);

                bitmap = new Bitmap(imgWidth, imgHeight);

                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    for (int y = 0; y <= imgHeight; y = y + WindowRect.Height )
                    {
                    
                        for(int x = 0; x <= imgWidth; x = x + WindowRect.Width)
                        {

                            this.IEWindow.Document.Body.ScrollLeft = x;
                            this.IEWindow.Document.Body.ScrollTop = y;

                            // Note:    ScrollLeft may not equal x, if x is outside the Scroll 
                            //          Area, then it reverts to the low or high limit. Same 
                            //          with ScrollTop.
                            g.CopyFromScreen(WindowRect.Left, 
                                WindowRect.Top,
                                this.IEWindow.Document.Body.ScrollLeft,
                                this.IEWindow.Document.Body.ScrollTop, 
                                WindowRect.Size);

                        }
                    }
                    
                }

                this.IEWindow.Document.Body.ScrollLeft = 0;
                this.IEWindow.Document.Body.ScrollTop = 0;
            });
            return bitmap;
        }

        /// <summary>
        ///     Set the value of an element on the page
        /// </summary>
        /// <param name="ID">
        ///     ID of the element to set
        /// </param>
        /// <param name="val">
        ///     Value to set
        /// </param>
        public void set(string ID, string val)
        {
            this.IEWindow.InvokeIfRequired(() => {
                HtmlDocument doc = this.IEWindow.Document;
                HtmlElement elem = doc.GetElementById(ID);
                if (elem != null)
                {

                    elem.SetAttribute("value", val);

                }            
            
            });

        }

        /// <summary>
        ///     Get the value of an element on the page
        /// </summary>
        /// <param name="ID">
        ///     ID of the element to look at
        /// </param>
        /// <returns>
        ///     value of the element
        /// </returns>
        public string get(string ID)
        {
            this.IEWindow.InvokeIfRequired(() => { 
            
            
                HtmlDocument doc = this.IEWindow.Document;
                HtmlElement elem = doc.GetElementById(ID);
                if (elem != null)
                {

                    return elem.GetAttribute("value");
            
                }
                return null;
            });

            return null;
        }

        /// <summary>
        ///     Click a specific element on the page
        /// </summary>
        /// <param name="ID">
        ///     ID of the element to click
        /// </param>
        public void click(string ID)
        {
            this.IEWindow.InvokeIfRequired(() => {
                HtmlDocument doc = this.IEWindow.Document;
                HtmlElement elem = doc.GetElementById(ID);
                if (elem != null)
                {

                    if (elem.TagName.Equals("input", StringComparison.InvariantCultureIgnoreCase) ||
                        elem.TagName.Equals("a", StringComparison.InvariantCultureIgnoreCase))
                    {
                        Console.WriteLine("clicking ID={0}", ID);

                        elem.InvokeMember("click");
                        this.wait();
                    }

                }
            });
            

        }

        /// <summary>
        ///     Submit the forms on the page
        /// </summary>
        public void submit()
        {
            this.IEWindow.InvokeIfRequired(()=>{
        
        
                Console.WriteLine("Submitting");
                HtmlDocument doc = this.IEWindow.Document;
                foreach(HtmlElement frm in doc.Forms)
                {

                    frm.InvokeMember("Submit");

                }
            });
        }

    }

}
