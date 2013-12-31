#I @"tools/FAKE/tools"
#r @"FakeLib.dll"

open Fake

let testDir = "./tests"
let buildDir = "./build"

let appReferences = !! "src/**/*.csproj"
let testReferences = !! "test/**/*.csproj"

Target "Clean" (fun _ ->
    CleanDirs [testDir; buildDir]
)

Target "RestorePackages" RestorePackages

Target "Build" (fun _ ->
    MSBuildRelease buildDir "Build" appReferences
        |> Log "AppBuild-Output: "
    
    MSBuildRelease testDir "Build" testReferences
        |> Log "AppBuild-Output: "
)

Target "PrepareForTests" (fun _ ->
    "xunit.runners"
        |> RestorePackageId (fun p ->
            { p with 
                Sources = ["http://nuget.org/api/v2/"];
                ExcludeVersion = true; 
                OutputPath = "./tools" })
)

Target "Tests" (fun _ ->
    !! (testDir + "/*.Tests.dll")
        |> xUnit (fun p -> 
            {p with 
                ToolPath = "./tools/xunit.runners/tools/xunit.console.exe";
                OutputDir = testDir })
)

Target "Default" DoNothing

"Clean" ==> 
    "RestorePackages" ==>
    "Build" ==>
    "PrepareForTests" ==>
    "Tests" ==>
    "Default"

RunTargetOrDefault "Default"