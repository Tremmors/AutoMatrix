using System;
using System.Drawing;
using System.Windows.Forms;

namespace automatrix
{
    /// <summary>
    ///     A browser object for the python code to interact with
    /// </summary>
    public class Browser
    {

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
 
            this.IEWindow = _control;

        }

        /// <summary>
        ///     Wait for the page to load.
        /// </summary>
        public void wait()
        {

            while (this.IEWindow.ReadyState != WebBrowserReadyState.Complete)
            {
        
                Application.DoEvents();

            }

        }

        /// <summary>
        ///     Open a webpage.
        /// </summary>
        /// <param name="url">
        ///     URL to open
        /// </param>
        public void open(string url)
        {

            this.IEWindow.Navigate(url);
            
            
        }

        /// <summary>
        ///     Take a screenshot of the entire webpage
        /// </summary>
        /// <param name="filename">
        ///     The filename to save the image as
        /// </param>
        public void screenshot(string filename)
        {
           
            Bitmap bmp = LoadBitMapFromScreen();
            bmp.Save(filename);

        }

        /// <summary>
        ///     Copy the entire contents of the webpage to a BitMap image
        /// </summary>
        /// <returns>
        ///     A BitMap object
        /// </returns>
        private Bitmap LoadBitMapFromScreen()
        {
            Rectangle WindowRect = this.IEWindow.RectangleToScreen(this.IEWindow.Document.Body.ClientRectangle);
            int imgWidth = this.IEWindow.Document.Body.ScrollRectangle.Width;
            int imgHeight = this.IEWindow.Document.Body.ScrollRectangle.Height;

            Console.WriteLine("size of image: {0},{1}", imgWidth, imgHeight);

            Bitmap bitmap = new Bitmap(imgWidth, imgHeight);

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

            HtmlDocument doc = this.IEWindow.Document;
            HtmlElement elem = doc.GetElementById(ID);
            if (elem != null)
            {

                elem.SetAttribute("value", val);

            }

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

            HtmlDocument doc = this.IEWindow.Document;
            HtmlElement elem = doc.GetElementById(ID);
            if (elem != null)
            {

                return elem.GetAttribute("value");
            
            }
            else
            {

                return null;
            
            }

        }

        /// <summary>
        ///     Click a specific element on the page
        /// </summary>
        /// <param name="ID">
        ///     ID of the element to click
        /// </param>
        public void click(string ID)
        {

            HtmlDocument doc = this.IEWindow.Document;
            HtmlElement elem = doc.GetElementById(ID);
            if(elem != null)
            {

                if(elem.TagName.Equals("input", StringComparison.InvariantCultureIgnoreCase ))
                {

                    elem.InvokeMember("click");

                }

            }

        }

        /// <summary>
        ///     Submit the forms on the page
        /// </summary>
        public void submit()
        {
            HtmlDocument doc = this.IEWindow.Document;
            foreach(HtmlElement frm in doc.Forms)
            { 
                frm.InvokeMember("Submit");

            }
        }

    }

}
