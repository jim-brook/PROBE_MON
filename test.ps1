$files =  Get-ChildItem -force -recurse | Where-Object {! $_.PSIsContainer} 
foreach($object in $files)
{
     $object.CreationTime=("10 November 2016 12:00:00")
}