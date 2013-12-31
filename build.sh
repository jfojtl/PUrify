#!/bin/bash

#getting latest FAKE via NuGet
mono tools/NuGet/nuget.exe install FAKE -OutputDirectory tools -ExcludeVersion -ConfigFile tools/NuGet/NuGet.Config

#build
mono tools/FAKE/tools/FAKE.exe build.fsx

