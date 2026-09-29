$ErrorActionPreference = "Stop"
cd E:\Semester_7\PRN232\LAB_1\prn232-lab1-grader_1\prn232-lab1-grader
. .\lib\Common.ps1
. .\lib\DynamicChecks.ps1
$rubric = Get-Content .\rubric.json -Raw | ConvertFrom-Json
$manifest = Get-Content E:\Semester_7\PRN232\LAB_1\submission.json -Raw | ConvertFrom-Json
$r = Invoke-DynamicChecks -BaseUrl 'http://localhost:8099' -Manifest $manifest -Settings $rubric.settings
foreach ($id in ($r.Keys | Sort-Object)) { 
    "{0} {1,5}%  {2}" -f $id, [math]::Round($r[$id].Ratio * 100), $r[$id].Message 
}
