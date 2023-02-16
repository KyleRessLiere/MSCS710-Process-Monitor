import os
import platform
print(os.name)
print(platform.system())
print(platform.release())

os = ("Mac OS X" if platform.system() == "Darwin" else platform.system()) + " " + platform.release()
print(os)