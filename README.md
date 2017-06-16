# PerforceWeb
Web form interface to Perforce Depot


This is the Web version of the Perforce .NET API Depot tree example provided here:https://swarm.workshop.perforce.com/files/guest/perforce_software/p4api.net/examples/sln-bld-gui

The required P4API.DLL is included but can be downloaded from Perforce's website.

Simply add your Perforce Server & port to the Web.Config then add your username to the Aspx.cs page to get this working.

I believe there may be issues connecting if you do not have the correct P4 CMD settings in place on the host machine. Further info on these settings can be found on Perforce's website.

I plan to extend this solution to demonstrate extracting file info from Perforce & storing in a GridView on the same page.

Thanks
