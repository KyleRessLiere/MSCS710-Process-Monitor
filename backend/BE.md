# Flask Server, Metrics Monitoring Script, and SQLite DB

## Requirements

- Python3 (https://www.python.org/)

## How To Use (from terminal)

Change to backend directory:

	'cd backend/'

Install Local Dependencies:

	'pip install -r requirements.txt'

Run Flask server, start metrics monitor script and initialize SQLite DB:

	'python3 app.py'

## View DB

Install VScode extension: "SQLite Viewer" by Florian Klampfer

~ or ~

Download "DB Browser for SQLite" (https://sqlitebrowser.org/)

## Build
Change to backend directory:  

	‘cd backend/’
Install PyInstaller

	pip install Pyinstaller
Run a clean build

	pyinstaller --clean app.py
Enter into debugging mode

	--Open app.spec in backend and toggle debug
		
	exe = EXE(
    pyz,
    a.scripts,
    [],
    exclude_binaries=True,
    name='app',
    debug=False,
    bootloader_ignore_signals=False,
    strip=False,
    upx=True,
    console=True,
    disable_windowed_traceback=False,
    argv_emulation=False,
    target_arch=None,
    codesign_identity=None,
    entitlements_file=None,
	)
Toggle Window for backend Server

	cd backend
	pyinstaller --noconsole app.py`
	--OR--
	pyinstaller --console app.py`
