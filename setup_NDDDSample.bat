@echo off
cls

tools\nant\NAnt.exe setup -buildfile:NDDDSample.build %*
