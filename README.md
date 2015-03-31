# Automatrix


A scriptable webclient for running automated tasks and capturing screenshots.

The main window has 2 panes: An IE based browser and a python source editor.
The python code can be executed to navigte and manipulate the window in the browser pane.

```python

	browser.open('http://www.google.com')
	browser.wait()
	browser.screenshot('google.jpg')
	browser.set('lst-ib','automatrix site:github.com')
	browser.submit()
 
```

supported browser actions:

# browser.open( URL )
Begin loading a page from a specific URL

#browser.wait()
Wait for the page to load.

#browser.screenshot( filename )
Take a screenshot of the current browser window

#browser.set( ID, Value )
Set a value in an element by the ID.

#browser.submit()
Submit all forms

#browser.click( ID )
Click a specific link/button
