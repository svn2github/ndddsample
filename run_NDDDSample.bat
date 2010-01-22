@echo off
cls

start build\NDDDSample.Interfaces.BookingRemoteService.Host\NDDDSample.Interfaces.BookingRemoteService.Host.exe

start build\NDDDSample.Interfaces.PathfinderRemoteService.Host\NDDDSample.Interfaces.PathfinderRemoteService.Host.exe

start http://localhost:7756/GoToApplication.aspx?AppID=8FB05403-76D5-413F-9435-559A845DAAF8