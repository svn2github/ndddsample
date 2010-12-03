NDDDSample is the project which demonstrates a practical implementation of the building block patterns described in the Eric Evans book 
based on a real but simplified cargo domain (which is also used as example in Eric Evans’ book). 

NDDDSample is the port of Domain-Driven Design Sample from Java to C# 

NDDDSample consists of the following parts:
	- Web interface allowing to book and track cargos.
	- RegisterApp allowing to register handling events for cargos.

============
= Solutions =
============
There are two solutions:
1. Web Cargo Tracking Application - NDDDSample.sln
2. Register desktop Application - RegisterApp.sln
which simulates  concept where are two different organizations, as it is in the java version of ddd sample
3. NDDDSample-full.sln doesn't have any business\domain meaning it is just full solution with all developed  
projects.
4. NDDDSample-full-with-setup.sln similar as from p.3 but with setup project.
5. NDDDSample-Cloud-full.sln is cloud(Windws Azure) version of application

============
= Building =
============
ASP.NET MVC 2.0 must be installed.
In order to build the source, run the build.bat file. ()
You'll find the built assemblies in subfolders of /build directory.

Note: The cloud version can be built and run only from Visual Studio.
	Windows Azure SDK needs to be installed. You can find it under tools\WindowsAzureSDK\
===========
= Running =
===========
In order to run application first build the source. See the building section.

- NDDDSample -
	1. To run setup from p.2 Visual Studio should be installed.
	   Please verify also if property "vs" in "NDDDSample.build" file corresponds to the path of installed Visual Studio.
	2. Setup NDDDSample by running setup_NDDDSample.bat
	   This will install Cassini Web Server and NDDDSample application.
	3. Execute run_NDDDSample.bat to run NDDDSample.
		the following services are run automatically:
			- NDDDSample.Interfaces.BookingRemoteService.Host.exe - wcf service allowing to book cargos
			- NDDDSample.Interfaces.PathfinderRemoteService.Host.exe - wcf service allowing to find itineraries
		the default browser is started with home page of NDDDSample web interface
- NDDDSample in the Cloud -
	1. Set up NDDDCloudService prjoject as start-up project
	2. Press (Ctrl + F5)/F5 to run/debug the solution
- RegisterApp -	
	1. Execute run_RegisterApp.bat to run RegisterApp.
		the following services are run automatically:
			- build\NDDDSample.Interfaces.HandlingService.Host\NDDDSample.Interfaces.HandlingService.Host.exe
		the application RegisterApp is started


===========
= More Details =
===========
The last version of the project and details can be found on project's home page: http://code.google.com/p/ndddsample/