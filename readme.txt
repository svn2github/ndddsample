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

1. Web interface can be found under build\NDDDSample.Web directory.
   To be able to book and track cargos the follwong services should be run:
		- build\NDDDSample.Interfaces.BookingRemoteService.Host\NDDDSample.Interfaces.BookingRemoteService.Host.exe - wcf service allowing to book cargos
		- build\NDDDSample.Interfaces.PathfinderRemoteService.Host\NDDDSample.Interfaces.PathfinderRemoteService.Host.exe - wcf service allowing to find itineraries
2. Handling events registering application can be found under build\NDDDSample.RegisterApp
   To be able to register handling events the following services should be run:
		- build\NDDDSample.Interfaces.HandlingService.Host\NDDDSample.Interfaces.HandlingService.Host.exe



Project's home page is http://code.google.com/p/ndddsample/