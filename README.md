Welcome to All!
===================
**all.exe** is command line utility that let you execute a command on all sub folders. Try one of next:

    cd myrepos
    all git pull

↑ When you go into your git repos, update all with a single line.

    cd visualStudioProjects
    all msbuild

↑ That will build all projects in subfolders of *visualStudioProjects*

    cd javaProjects
    all ant
   
↑  Similar for java projects, I am guessing it will work for other build systems.

#How  does it work?
> Given `all command arguments` All that **all** does :) is 

>- **cd** subFolder
>- **command** arguments
>- **cd** ..

> Does that for all the direct children folders.

Yeah it is a very small, easy and useful project. It is easier type **all** than deal writing a bash script.
