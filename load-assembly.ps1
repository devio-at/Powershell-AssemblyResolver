
# adjust paths:

$resolver = $pwd.Path + "\devio.Powershell.dll"

$filename = $pwd.Path + "\My.Assembly.dll"

#
# use the dll's config to retrieve nhibernate settings and connection string
#
# http://stackoverflow.com/questions/835862/powershell-calling-net-assembly-that-uses-app-config
#
[System.AppDomain]::CurrentDomain.SetData("APP_CONFIG_FILE", $filename + ".config")

#
# http://msdn.microsoft.com/en-us/library/system.appdomain.getdata.aspx
#
[System.AppDomain]::CurrentDomain.SetData("APPBASE", [System.IO.Path]::GetDirectoryName( $filename ) + "\")
[System.AppDomain]::CurrentDomain.SetData("PRIVATE_BINPATH", [System.IO.Path]::GetDirectoryName( $filename ) + "\")

[System.Environment]::CurrentDirectory = $pwd.Path


# http://stackoverflow.com/questions/2664028/how-can-i-get-powershell-added-types-to-use-added-types

$dummy = [System.Reflection.Assembly]::LoadFrom($resolver)
[devio.Powershell.AssemblyResolver]::AddAssemblyPath( [System.IO.Path]::GetDirectoryName( $filename ) )

$dummy = [System.Reflection.Assembly]::LoadFrom($filename)

"assembly loaded"

trap 
{ 
"<<<<<"

	$error[0].Exception.ToString() 

">>>>>"
}


$object = new-object My.Assembly.MyClass

# list all loaded assemblies:
# [System.AppDomain]::CurrentDomain.GetAssemblies() | select-object  FullName

