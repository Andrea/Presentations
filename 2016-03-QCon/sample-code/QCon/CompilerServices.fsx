#I "packages/FSharp.Compiler.Service.2.0.0.6/lib/net45/"

#r "FSharp.Compiler.Service.dll"
#load "Utils.fs"

// A mini example of what we did
// or why would you want to compile code in your app
open Microsoft.FSharp.Compiler
open System.IO  
open Microsoft.FSharp.Compiler.SimpleSourceCodeServices

let showResults (errors:FSharpErrorInfo []) exitCode=
  let errorString = 
    errors 
    |> Seq.fold(fun acc x -> 
        if x.Severity = FSharpErrorSeverity.Error then x.Message + acc
        else acc) ""
  printfn "Exit code %i \n Errors: %s" exitCode errorString   
  if exitCode = 0 then 
    ShowCelebration()
  else 
    ShowFailure()
  


let dllCompile path = 
  let dllPath = Path.ChangeExtension(path, ".dll")
  let scs = SimpleSourceCodeServices()
  
  let errors, exitCode = scs.Compile([| "fsc.exe"; "-o"; dllPath; "-a"; path |])
  printfn "The file %s exists: %b" dllPath (File.Exists(dllPath))
  showResults errors exitCode

let dynamicCompile sourceFile=
  let dllPath = Path.ChangeExtension(sourceFile, ".dll")
  let scs = SimpleSourceCodeServices()

  let errors, exitCode, dynAssembly = scs.CompileToDynamicAssembly([| "-o"; dllPath; "-a"; sourceFile |], execute=None)
  printfn "The file %s exists: %b" dllPath (File.Exists(dllPath))
  printfn "The Assembly %s exists: %b" (dynAssembly.Value.GetName().Name ) dynAssembly.IsSome
  showResults errors exitCode


let watch =   
  let watcher = new FileSystemWatcher(Path = Path.Combine( __SOURCE_DIRECTORY__,"samples"),
                                      NotifyFilter = NotifyFilters.LastWrite,
                                      Filter = "*.fs")
  watcher.Changed.Add(fun args -> 
    printfn "The file %s is changed." args.FullPath
    dllCompile args.FullPath)
  watcher.EnableRaisingEvents <- true
  
  watcher.Dispose()