# RoslynCodeWeaving
A little example of how Roslyn can be used to create controllers on the fly. 

First of all sorry for the scrappy notes here. I've just quickly thrown this together this evening. This project is really intended to serve as my own notes but I thought I'd open source it in case it's useful to anyone else. 

The library creates a controller within the web api project at build time, by ommiting the file writing part of the library code the controller will still work although the CS file will be missing. 

The cs file can be modified by a developer. However if the library write to file code is taken out then it will "code weave" the controller into the web api project.

I'm thiking this code might come in handy for a headless CMS that generates .NET WebApi projects based on some GUI interaction. Perhaps I'll work on this properly once I've completed the phone app I'm currently working on. 
