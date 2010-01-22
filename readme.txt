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

NDDDSample-full.sln doesn't have any business\domain meaning it is just full solution with all developed  
projects.

============
= Building =
============

In order to build the source, run the build.bat file.

You'll find the built assemblies in subfolders of /build directory.

===========
= Running =
===========

- NDDDSample -
	1. Setup NDDDSample by running setup_NDDDSample.bat
	2. Execute run_NDDDSample.bat to run NDDDSample.
		the following services are run automatically:
			- NDDDSample.Interfaces.BookingRemoteService.Host.exe - wcf service allowing to book cargos
			- NDDDSample.Interfaces.PathfinderRemoteService.Host.exe - wcf service allowing to find itineraries
		the default browser is started with home page of NDDDSample web interface
		
- RegisterApp -	
	1. Execute run_RegisterApp.bat to run RegisterApp.
		the following services are run automatically:
			- build\NDDDSample.Interfaces.HandlingService.Host\NDDDSample.Interfaces.HandlingService.Host.exe
		the application RegisterApp is started


===========
= More Details =
===========
Can be found on project's home page: http://code.google.com/p/ndddsample/